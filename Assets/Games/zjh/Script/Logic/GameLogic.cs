using System;
using Shared;

namespace com.QH.QPGame.ZJH
{
	public class GameLogic
    {
        //
        public const ushort KIND_ID         = 202;
        public const ushort GAME_PLAYER     = 5;
        public const string GAME_NAME       = "扎金花";

        public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;
        public const ushort GS_WK_PLAYING   = (ushort)GameState.GS_PLAYING;

        public const byte ST_ORDER          = 0;
        public const byte ST_COUNT          = 1;

        public const ushort FULL_COUNT      = 54;
        public const ushort NORMAL_COUNT    = 3;

        public const byte NULL_CHAIR        = 255;

        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码





        //宏定义
        public const ushort MAX_COUNT       = 3;    //最大数目
        public const byte DRAW              = 2;    //和局类型

        //扑克类型
        public const byte CT_ERROR          = 0;
        public const byte CT_SINGLE         = 1;    //单牌类型
        public const byte CT_DOUBLE         = 2;    //对子类型
        public const byte CT_SHUN_ZI        = 3;    //顺子类型
        public const byte CT_JIN_HUA        = 4;    //金花类型
        public const byte CT_SHUN_JIN       = 5;    //顺金类型
        public const byte CT_BAO_ZI         = 6;    //豹子类型
        public const byte CT_SPECIAL        = 7;    //特殊类型
        public const byte TRUE              = 1;
        public const byte FALSE             = 0;

        //筹码种类
        public const ushort CHIP_MAX_COUNT  = 11;

        //
        public static byte[] HandCardData   = new byte[MAX_COUNT];
        public static byte HandCardCount    = 0;
        //
        public static byte[,] OutCardData   = new byte[GAME_PLAYER, MAX_COUNT];
        public static byte[] OutCardCount   = new byte[GAME_PLAYER];
        //


        //扑克数据
        byte[] m_cbCardListData = new byte[52]
        {
             0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,   //方块 A - K
             0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,   //梅花 A - K
             0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,   //红桃 A - K
             0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D    //黑桃 A - K
        };
        //
        public static bool IsValidCard(byte cbCardData)
        {
            byte cbCardColor = GetCardColor(cbCardData);
            byte cbCardValue = GetCardValue(cbCardData);

            if ((cbCardData == 0x4E) || (cbCardData == 0x4F)) return true;
            if ((cbCardColor <= 0x30) && (cbCardValue >= 0x01) && (cbCardValue <= 0x0D)) return true;

            return false;
        }
        //Get Value
        public static byte GetCardValue(byte cbCardData)
        {
            return (byte)(cbCardData & MASK_VALUE);
        }
        //Get Flr
        public static byte GetCardColor(byte cbCardData)
        {
            return (byte)(cbCardData & MASK_COLOR);
        }
        //
        public static byte GetCardLogicValue(byte cbCardData)
        {
            //扑克属性
            byte bCardColor = GetCardColor(cbCardData);
            byte bCardValue = GetCardValue(cbCardData);

            //转换数值
            return (byte)((bCardValue == 1) ? (bCardValue + 13) : bCardValue);
        }
  
        public static byte GetCardType(byte[] cbCardData, byte cbCardCount)
        {

            if (cbCardCount == MAX_COUNT)
            {
                //变量定义
                bool cbSameColor = true;
                bool bLineCard = true;
                byte cbFirstColor = GetCardColor(cbCardData[0]);
                byte cbFirstValue = GetCardLogicValue(cbCardData[0]);

                //牌形分析
                for (byte i = 1; i < cbCardCount; i++)
                {
                    //数据分析
                    if (GetCardColor(cbCardData[i]) != cbFirstColor)
                        cbSameColor = false;
                    if (cbFirstValue != (GetCardLogicValue(cbCardData[i]) + i))
                        bLineCard = false;

                    //结束判断
                    if ((cbSameColor == false) && (bLineCard == false))
                        break;
                }

                //特殊A32
                if (!bLineCard)
                {
                    bool bOne1 = false;
                    bool bTwo1 = false;
                    bool bThree1 = false;
                    for (byte i = 0; i < MAX_COUNT; i++)
                    {
                        if (GetCardValue(cbCardData[i]) == 1)
                            bOne1 = true;
                        else if (GetCardValue(cbCardData[i]) == 2)
                            bTwo1 = true;
                        else if (GetCardValue(cbCardData[i]) == 3)
                            bThree1 = true;
                    }
                    if (bOne1 && bTwo1 && bThree1)
                        bLineCard = true;
                }

                //顺金类型
                if ((cbSameColor) && (bLineCard))
                    return CT_SHUN_JIN;

                //顺子类型
                if ((!cbSameColor) && (bLineCard))
                    return CT_SHUN_ZI;

                //金花类型
                if ((cbSameColor) && (!bLineCard))
                    return CT_JIN_HUA;

                //牌形分析
                bool bDouble = false;
                bool bPanther = true;

                //对牌分析
                for (byte i = 0; i < cbCardCount - 1; i++)
                {
                    for (byte j = (byte)(i + 1); j < cbCardCount; j++)
                    {
                        if (GetCardLogicValue(cbCardData[i]) == GetCardLogicValue(cbCardData[j]))
                        {
                            bDouble = true;
                            break;
                        }
                    }
                    if (bDouble)
                        break;
                }

                //三条(豹子)分析
                for (byte i = 1; i < cbCardCount; i++)
                {
                    if (bPanther && cbFirstValue != GetCardLogicValue(cbCardData[i]))
                        bPanther = false;
                }

                //对子和豹子判断
                if (bDouble == true)
                    return (bPanther) ? CT_BAO_ZI : CT_DOUBLE;

                //特殊235
                bool bTwo = false;
                bool bThree = false;
                bool bFive = false;
                for (byte i = 0; i < cbCardCount; i++)
                {
                    if (GetCardValue(cbCardData[i]) == 2)
                        bTwo = true;
                    else if (GetCardValue(cbCardData[i]) == 3)
                        bThree = true;
                    else if (GetCardValue(cbCardData[i]) == 5)
                        bFive = true;
                }
                if (bTwo && bThree && bFive)
                    return CT_SPECIAL;
            }

            return CT_SINGLE;
        }



        public static byte CompareCard(byte[] cbFirstData, byte[] cbNextData, byte cbCardCount)
        {
            //设置变量
            byte[] FirstData = new byte[MAX_COUNT];
            byte[] NextData = new byte[MAX_COUNT];
            Buffer.BlockCopy(cbFirstData, 0, FirstData, 0, MAX_COUNT);
            Buffer.BlockCopy(cbNextData, 0, NextData, 0, MAX_COUNT);


            //大小排序
            SortCardList(ref FirstData, cbCardCount);
            SortCardList(ref NextData, cbCardCount);

            //获取类型
            byte cbNextType = GetCardType(NextData, cbCardCount);
            byte cbFirstType = GetCardType(FirstData, cbCardCount);

            //特殊情况分析
            if ((cbNextType + cbFirstType) == (CT_SPECIAL + CT_BAO_ZI))
                return (cbFirstType > cbNextType) ? (byte)1 : (byte)0;

            //还原单牌类型
            if (cbNextType == CT_SPECIAL)
                cbNextType = CT_SINGLE;
            if (cbFirstType == CT_SPECIAL)
                cbFirstType = CT_SINGLE;

            //类型判断
            if (cbFirstType != cbNextType)
                return (cbFirstType > cbNextType) ? TRUE : FALSE;

            //简单类型
            switch (cbFirstType)
            {
                case CT_BAO_ZI: //豹子
                case CT_SINGLE: //单牌
                case CT_JIN_HUA: //金花
                    {
                        //对比数值
                        for (byte i = 0; i < cbCardCount; i++)
                        {
                            byte cbNextValue = GetCardLogicValue(NextData[i]);
                            byte cbFirstValue = GetCardLogicValue(FirstData[i]);
                            if (cbFirstValue != cbNextValue)
                                return (cbFirstValue > cbNextValue) ? TRUE : FALSE;
                        }
                        return DRAW;
                    }
                case CT_SHUN_ZI: //顺子
                case CT_SHUN_JIN: //顺金 432>A32
                    {
                        byte cbNextValue = GetCardLogicValue(NextData[0]);
                        byte cbFirstValue = GetCardLogicValue(FirstData[0]);

                        //特殊A32
                        if (cbNextValue == 14 && GetCardLogicValue(NextData[cbCardCount - 1]) == 2)
                        {
                            cbNextValue = 3;
                        }
                        if (cbFirstValue == 14 && GetCardLogicValue(FirstData[cbCardCount - 1]) == 2)
                        {
                            cbFirstValue = 3;
                        }

                        //对比数值
                        if (cbFirstValue != cbNextValue)
                            return (cbFirstValue > cbNextValue) ? TRUE : FALSE;
                        return DRAW;
                    }
                case CT_DOUBLE: //对子
                    {
                        byte cbNextValue = GetCardLogicValue(NextData[0]);
                        byte cbFirstValue = GetCardLogicValue(FirstData[0]);

                        //查找对子/单牌
                        byte bNextDouble = 0;
                        byte bNextSingle = 0;
                        byte bFirstDouble = 0;
                        byte bFirstSingle = 0;
                        if (cbNextValue == GetCardLogicValue(NextData[1]))
                        {
                            bNextDouble = cbNextValue;
                            bNextSingle = GetCardLogicValue(NextData[cbCardCount - 1]);
                        }
                        else
                        {
                            bNextDouble = GetCardLogicValue(NextData[cbCardCount - 1]);
                            bNextSingle = cbNextValue;
                        }
                        if (cbFirstValue == GetCardLogicValue(FirstData[1]))
                        {
                            bFirstDouble = cbFirstValue;
                            bFirstSingle = GetCardLogicValue(FirstData[cbCardCount - 1]);
                        }
                        else
                        {
                            bFirstDouble = GetCardLogicValue(FirstData[cbCardCount - 1]);
                            bFirstSingle = cbFirstValue;
                        }

                        if (bNextDouble != bFirstDouble)
                            return (bFirstDouble > bNextDouble) ? TRUE : FALSE;
                        if (bNextSingle != bFirstSingle)
                            return (bFirstSingle > bNextSingle) ? TRUE : FALSE;
                        return DRAW;
                    }
            }

            return DRAW;
        }

        //

        public static void SortCardList(ref byte[] cbCardData, byte cbCardCount)
        {
            //转换数值
            byte[] cbLogicValue = new byte[MAX_COUNT];
            for (byte i = 0; i < cbCardCount; i++)
                cbLogicValue[i] = GetCardLogicValue(cbCardData[i]);

            //排序操作
            bool bSorted = true;
            byte cbTempData;
            byte bLast = (Byte)(cbCardCount - 1);
            do
            {
                bSorted = true;
                for (byte i = 0; i < bLast; i++)
                {
                    if ((cbLogicValue[i] < cbLogicValue[i + 1]) || ((cbLogicValue[i] == cbLogicValue[i + 1]) && (cbCardData[i] < cbCardData[i + 1])))
                    {
                        //交换位置
                        cbTempData = cbCardData[i];
                        cbCardData[i] = cbCardData[i + 1];
                        cbCardData[i + 1] = cbTempData;
                        cbTempData = cbLogicValue[i];
                        cbLogicValue[i] = cbLogicValue[i + 1];
                        cbLogicValue[i + 1] = cbTempData;
                        bSorted = false;
                    }
                }
                bLast--;
            } while (bSorted == false);

            return;
        }
        private static System.Random r;

        internal static int NextNumber()
        {
            if (r == null)
                Seed();

            return r.Next();
        }

        internal static int NextNumber(int ceiling)
        {
            if (r == null)
                Seed();

            return r.Next(ceiling);
        }

        internal static void Seed()
        {
            r = new System.Random();
        }

        internal static void Seed(int seed)
        {
            r = new System.Random(seed);
        }

        public void RandCardList(byte[] cbCardBuffer, byte cbBufferCount)
        {
            //混乱准备
            byte[] cbCardData = new byte[m_cbCardListData.Length];
            Buffer.BlockCopy(m_cbCardListData, 0, cbCardData, 0, m_cbCardListData.Length);
            //混乱扑克
            byte bRandCount = 0;
            byte bPosition = 0;
            do
            {
                bPosition = (byte)(NextNumber() % (m_cbCardListData.Length - bRandCount));
                cbCardBuffer[bRandCount++] = cbCardData[bPosition];
                cbCardData[bPosition] = cbCardData[m_cbCardListData.Length - bRandCount];
            } while (bRandCount < cbBufferCount);

            return;
        }

        //
        public static bool RemoveCard(byte[] cbRemoveCard, byte cbRemoveCount, ref byte[] cbCardData, ref byte cbCardCount)
        {
            byte cbDeleteCount = 0;
            byte[] cbTempCardData = new byte[MAX_COUNT];

            if (cbCardCount > MAX_COUNT)
                return false;

            Buffer.BlockCopy(cbCardData, 0, cbTempCardData, 0, cbCardCount);
            for (byte i = 0; i < cbRemoveCount; i++)
            {
                for (byte j = 0; j < cbCardCount; j++)
                {
                    if (cbRemoveCard[i] == cbTempCardData[j])
                    {
                        cbDeleteCount++;
                        cbTempCardData[j] = 0;
                        break;
                    }
                }
            }
            if (cbDeleteCount != cbRemoveCount) return false;

            byte cbCardPos = 0;
            for (byte i = 0; i < cbCardCount; i++)
            {
                if (cbTempCardData[i] != 0) cbCardData[cbCardPos++] = cbTempCardData[i];
            }
            cbCardCount = (byte)(cbCardCount - cbRemoveCount);
            return true;
        }
        //

    }
}
