using System;
using Shared;

namespace com.QH.QPGame.SH
{
    public class tagAnalyseResult
    {
        public byte     cbFourCount;
        public byte     cbThreeCount;
        public byte     cbDoubleCount;
        public byte     cbSignedCount;
        public byte[]   cbFourLogicVolue    = new byte[1];
        public byte[]   cbThreeLogicVolue   = new byte[1];
        public byte[]   cbDoubleLogicVolue  = new byte[2];
        public byte[]   cbSignedLogicVolue  = new byte[5];
        public byte[]   cbFourCardData      = new byte[GameLogic.MAX_COUNT];
        public byte[]   cbThreeCardData     = new byte[GameLogic.MAX_COUNT];
        public byte[]   cbDoubleCardData    = new byte[GameLogic.MAX_COUNT];
        public byte[]   cbSignedCardData    = new byte[GameLogic.MAX_COUNT];

    };

	public class GameLogic
    {
        //
        public const ushort KIND_ID         = 204;
        public const ushort GAME_PLAYER     = 5;
        public const string GAME_NAME       = "梭哈";

        public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;
        public const ushort GS_WK_PLAYING   = (ushort)GameState.GS_PLAYING;

        public const byte NULL_CHAIR        = 255;

        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码


        //宏定义
        public const ushort MAX_COUNT       = 5;    //最大数目


        //扑克类型
        public const byte CT_SINGLE         = 1;    //单牌类型
        public const byte CT_ONE_DOUBLE     = 2;    //对子类型
        public const byte CT_TWO_DOUBLE     = 3;    //两对类型
        public const byte CT_THREE_TIAO     = 4;    //三条类型
        public const byte CT_SHUN_ZI        = 5;    //顺子类型
        public const byte CT_TONG_HUA       = 6;    //同花类型
        public const byte CT_HU_LU          = 7;    //葫芦类型
        public const byte CT_TIE_ZHI        = 8;    //铁支类型
        public const byte CT_TONG_HUA_SHUN  = 9;    //同花顺型


        //扑克数据
        byte[] m_cbCardListData = new byte[32]
        {
             0x01,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,                            //方块 A - K
             0x11,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,                            //梅花 A - K
             0x21,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,                            //红桃 A - K
             0x31,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,                            //黑桃 A - K
        };
        //检查是否有效
        public static bool IsValidCard(byte cbCardData)
        {
            byte cbCardColor = GetCardColor(cbCardData);
            byte cbCardValue = GetCardValue(cbCardData);

            if ((cbCardColor <= 0x30) && (cbCardValue >= 0x01) && (cbCardValue <= 0x0D)) return true;

            return false;
        }
        //获取牌值
        public static byte GetCardValue(byte cbCardData)
        {
            return (byte)(cbCardData & MASK_VALUE);
        }
        //获取花色
        public static byte GetCardColor(byte cbCardData)
        {
            return (byte)(cbCardData & MASK_COLOR);
        }
        //获取逻辑值
        public static byte GetCardLogicValue(byte cbCardData)
        {
            //扑克属性
            byte bCardColor = GetCardColor(cbCardData);
            byte bCardValue = GetCardValue(cbCardData);

            //转换数值
            return (byte)((bCardValue<=2)?(bCardValue+13):bCardValue);
        }

        //获取牌型
        public static byte GetCardType(byte[] cardData, byte cardCount)
        {
            byte[] cbCardData  = new byte[5];
            byte   cbCardCount = cardCount;
            Buffer.BlockCopy(cardData,0,cbCardData,0,cardCount);

            SortCardList(ref cbCardData,cbCardCount);
             //简单牌形
             switch (cbCardCount)
             {
             case 1: //单牌
                 {
                     return CT_SINGLE;
                 }
             case 2: //对牌
                 {
                     return (GetCardLogicValue(cbCardData[0])==GetCardLogicValue(cbCardData[1]))?CT_ONE_DOUBLE:CT_SINGLE;
                 }
             }

             //五条类型
             if (cbCardCount==5)
             {
                 //变量定义
                 bool cbSameColor=true,bLineCard=true;
                 byte cbFirstColor=GetCardColor(cbCardData[0]);
                 byte cbFirstValue=GetCardLogicValue(cbCardData[0]);
            
                 //牌形分析
                 for (byte i=1;i<cbCardCount;i++)
                 {
                     //数据分析
                     if (GetCardColor(cbCardData[i])!=cbFirstColor) cbSameColor=false;
                     if (cbFirstValue!=(GetCardLogicValue(cbCardData[i])+i)) bLineCard=false;
            
                     //结束判断
                     if ((cbSameColor==false)&&(bLineCard==false)) break;
                 }
            
                 //顺子类型
                 if ((cbSameColor==false)&&(bLineCard==true)) return CT_SHUN_ZI;
            
                 //同花类型
                 if ((cbSameColor==true)&&(bLineCard==false)) return CT_TONG_HUA;

                 //同花顺类型
                 if ((cbSameColor==true)&&(bLineCard==true)) return CT_TONG_HUA_SHUN;
             }

             //扑克分析
             tagAnalyseResult AnalyseResult = new tagAnalyseResult();
             AnalysebCardData(cbCardData,cbCardCount,ref AnalyseResult);
            
             //类型判断
             if (AnalyseResult.cbFourCount==1) return CT_TIE_ZHI;
             if (AnalyseResult.cbDoubleCount==2) return CT_TWO_DOUBLE;
             if ((AnalyseResult.cbDoubleCount==1)&&(AnalyseResult.cbThreeCount==1)) return CT_HU_LU;
             if ((AnalyseResult.cbThreeCount==1)&&(AnalyseResult.cbDoubleCount==0)) return CT_THREE_TIAO;
             if ((AnalyseResult.cbDoubleCount==1)&&(AnalyseResult.cbSignedCount==3)) return CT_ONE_DOUBLE;

             return CT_SINGLE;
        }

        //分析扑克
        public static void AnalysebCardData(byte[] cbCardData, byte cbCardCount, ref tagAnalyseResult AnalyseResult)
        {
             //设置结果
             //ZeroMemory(&AnalyseResult,sizeof(AnalyseResult));

             //扑克分析
             for (byte i=0;i<cbCardCount;i++)
             {
                 //变量定义
                 byte cbSameCount=1;
                 byte[] cbSameCardData = new byte[4]{cbCardData[i],0,0,0};
                 byte cbLogicValue=GetCardLogicValue(cbCardData[i]);

                 //获取同牌
                 for (int j=i+1;j<cbCardCount;j++)
                 {
                     //逻辑对比
                     if (GetCardLogicValue(cbCardData[j])!=cbLogicValue) break;
    
                     //设置扑克
                     cbSameCardData[cbSameCount++]=cbCardData[j];
                 }
    
                 //保存结果
                 switch (cbSameCount)
                 {
                 case 1:     //单张
                     {
                         AnalyseResult.cbSignedLogicVolue[AnalyseResult.cbSignedCount]=cbLogicValue;
                         //CopyMemory(&AnalyseResult.cbSignedCardData[(AnalyseResult.cbSignedCount++)*cbSameCount],cbSameCardData,cbSameCount);
                         Buffer.BlockCopy(cbSameCardData,0,AnalyseResult.cbSignedCardData,(AnalyseResult.cbSignedCount++)*cbSameCount,cbSameCount);
                         break;
                     }
                 case 2:     //两张
                     {
                         AnalyseResult.cbDoubleLogicVolue[AnalyseResult.cbDoubleCount]=cbLogicValue;
                         //CopyMemory(&AnalyseResult.cbDoubleCardData[(AnalyseResult.cbDoubleCount++)*cbSameCount],cbSameCardData,cbSameCount);
                         Buffer.BlockCopy(cbSameCardData,0,AnalyseResult.cbDoubleCardData,(AnalyseResult.cbDoubleCount++)*cbSameCount,cbSameCount);
                         break;
                     }
                 case 3:     //三张
                     {
                         AnalyseResult.cbThreeLogicVolue[AnalyseResult.cbThreeCount]=cbLogicValue;
                         //CopyMemory(&AnalyseResult.cbThreeCardData[(AnalyseResult.cbThreeCount++)*cbSameCount],cbSameCardData,cbSameCount);
                         Buffer.BlockCopy(cbSameCardData,0,AnalyseResult.cbThreeCardData,(AnalyseResult.cbThreeCount++)*cbSameCount,cbSameCount);
                         break;
                     }
                 case 4:     //四张
                     {
                         AnalyseResult.cbFourLogicVolue[AnalyseResult.cbFourCount]=cbLogicValue;
                         //CopyMemory(&AnalyseResult.cbFourCardData[(AnalyseResult.cbFourCount++)*cbSameCount],cbSameCardData,cbSameCount);
                         Buffer.BlockCopy(cbSameCardData,0,AnalyseResult.cbFourCardData,(AnalyseResult.cbFourCount++)*cbSameCount,cbSameCount);
                         break;
                     }
                 }
    
                 //设置递增
                 i+=(byte)(cbSameCount-1);
            }

            return;
       }

        //比牌
        public static bool CompareCard(byte[] cbFirstData, byte[] cbNextData, byte cbCardCount)
        {
            //获取类型
             byte cbNextType=GetCardType(cbNextData,cbCardCount);
             byte cbFirstType=GetCardType(cbFirstData,cbCardCount);
            
             //类型判断
             if (cbFirstType!=cbNextType) return (cbFirstType>cbNextType?true:false);

             //简单类型
             switch(cbFirstType)
             {
             case CT_SINGLE:         //单牌
                 {
                     //对比数值
                     for (byte i=0;i<cbCardCount;i++)
                     {
                         byte cbNextValue=GetCardLogicValue(cbNextData[i]);
                         byte cbFirstValue=GetCardLogicValue(cbFirstData[i]);
                         if (cbFirstValue!=cbNextValue) return cbFirstValue>cbNextValue;
                     }
            
                     //对比花色
                     return GetCardColor(cbFirstData[0])>GetCardColor(cbNextData[0]);
                 }
             case CT_HU_LU:          //葫芦
             case CT_TIE_ZHI:        //铁支
             case CT_ONE_DOUBLE:     //对子
             case CT_TWO_DOUBLE:     //两对
             case CT_THREE_TIAO:     //三条
                 {
                     //分析扑克
                     tagAnalyseResult AnalyseResultNext = new tagAnalyseResult();
                     tagAnalyseResult AnalyseResultFirst = new tagAnalyseResult();
                     AnalysebCardData(cbNextData,cbCardCount,ref AnalyseResultNext);
                     AnalysebCardData(cbFirstData,cbCardCount,ref AnalyseResultFirst);

                     //四条数值
                     if (AnalyseResultFirst.cbFourCount>0)
                     {
                         byte cbNextValue=AnalyseResultNext.cbFourLogicVolue[0];
                         byte cbFirstValue=AnalyseResultFirst.cbFourLogicVolue[0];
                         return cbFirstValue>cbNextValue;
                     }
            
                     //三条数值
                     if (AnalyseResultFirst.cbThreeCount>0)
                     {
                         byte cbNextValue=AnalyseResultNext.cbThreeLogicVolue[0];
                         byte cbFirstValue=AnalyseResultFirst.cbThreeLogicVolue[0];
                         return cbFirstValue>cbNextValue;
                     }
            
                     //对子数值
                     for (byte i=0;i<AnalyseResultFirst.cbDoubleCount;i++)
                     {
                         byte cbNextValue=AnalyseResultNext.cbDoubleLogicVolue[i];
                         byte cbFirstValue=AnalyseResultFirst.cbDoubleLogicVolue[i];
                         if (cbFirstValue!=cbNextValue) return cbFirstValue>cbNextValue;
                     }
            
                     //散牌数值
                     for (byte i=0;i<AnalyseResultFirst.cbSignedCount;i++)
                     {
                         byte cbNextValue=AnalyseResultNext.cbSignedLogicVolue[i];
                         byte cbFirstValue=AnalyseResultFirst.cbSignedLogicVolue[i];
                         if (cbFirstValue!=cbNextValue) return cbFirstValue>cbNextValue;
                     }
            
                     //对子花色
                     if (AnalyseResultFirst.cbDoubleCount>0)
                     {
                         byte cbNextColor=GetCardColor(AnalyseResultNext.cbDoubleCardData[0]);
                         byte cbFirstColor=GetCardColor(AnalyseResultFirst.cbDoubleCardData[0]);
                         return cbFirstColor>cbNextColor;
                     }
            
                     //散牌花色
                     if (AnalyseResultFirst.cbSignedCount>0)
                     {
                         byte cbNextColor=GetCardColor(AnalyseResultNext.cbSignedCardData[0]);
                         byte cbFirstColor=GetCardColor(AnalyseResultFirst.cbSignedCardData[0]);
                         return cbFirstColor>cbNextColor;
                     }
            
                     break;
                 }
             case CT_SHUN_ZI:        //顺子
             case CT_TONG_HUA:       //同花
             case CT_TONG_HUA_SHUN:  //同花顺
                 {
                     //数值判断
                     byte cbNextValue=GetCardLogicValue(cbNextData[0]);
                     byte cbFirstValue=GetCardLogicValue(cbFirstData[0]);
                     if (cbFirstValue!=cbNextValue) return (cbFirstValue>cbNextValue);
            
                     //花色判断
                     byte cbNextColor=GetCardColor(cbNextData[0]);
                     byte cbFirstColor=GetCardColor(cbFirstData[0]);
            
                     return (cbFirstColor>cbNextColor);
                 }
             }
            
             return false;
        }

        //排序
        public static void SortCardList(ref byte[] cbCardData, byte cbCardCount)
        {
            try
            {
                //转换数值
                byte[] cbLogicValue = new byte[MAX_COUNT];
                for (byte i = 0; i < cbCardCount; i++)
                {
                    cbLogicValue[i] = GetCardLogicValue(cbCardData[i]);
                }
                //排序操作
                bool bSorted = true;
                byte cbTempData;
                byte bLast = (byte)(cbCardCount - 1);
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
            }
            catch(Exception ex)
            {
                //NGUIDebug.Log(ex.Source);
                //NGUIDebug.Log(ex.StackTrace);
                //NGUIDebug.Log(ex.Message);
            }
            return;

        }


    }
}
