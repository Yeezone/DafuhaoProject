using System;
using Shared;

namespace com.QH.QPGame.NN
{

    public class tagAnalyseResult
    {
         public byte     cbFourCount;
         public byte     cbThreeCount;
         public byte     cbDoubleCount;
         public byte     cbSignedCount;
         public byte[]   cbFourLogicVolue   = new byte[1];
         public byte[]   cbThreeLogicVolue  = new byte[1];
         public byte[]   cbDoubleLogicVolue = new byte[2];
         public byte[]   cbSignedLogicVolue = new byte[5];
         public byte[]   cbFourCardData     = new byte[GameLogic.MAX_COUNT];
         public byte[]   cbThreeCardData    = new byte[GameLogic.MAX_COUNT];
         public byte[]   cbDoubleCardData   = new byte[GameLogic.MAX_COUNT];
         public byte[]   cbSignedCardData   = new byte[GameLogic.MAX_COUNT];

    };
	public class GameLogic
    {
        //
        public const ushort KIND_ID         = 203;
        public const ushort GAME_PLAYER     = 5;
        public const string GAME_NAME       = "斗牛";

		//当前游戏状态
        public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;			
        public const ushort GS_WK_BANKER    = (ushort)GameState.GS_PLAYING;		 //抢庄阶段	
        public const ushort GS_WK_CHIP      = (ushort)(GameState.GS_PLAYING+1);	 //下注阶段	
        public const ushort GS_WK_PLAYING   = (ushort)(GameState.GS_PLAYING+2);	 //玩牌阶段

        public const byte NULL_CHAIR        = 255;
        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码
        public const ushort MAX_COUNT       = 5;    //最大数目

        public const ushort OX_VALUE0       = 0;    //混合牌型
        public const ushort OX_FOURKING     = 12;   //天王牌型
        public const ushort OX_FIVEKING     = 13;   //天王牌型

        //是否有效牌
        public static bool IsValidCard(byte cbCardData)
        {
            byte cbCardColor = GetCardColor(cbCardData);
            byte cbCardValue = GetCardValue(cbCardData);

            if ((cbCardData == 0x4E) || (cbCardData == 0x4F)) return true;
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
            if(bCardValue>10)
            {
                return 10;
            }
            else
            {
                return bCardValue;
            }
        }
        //获取牌型
        public static byte GetCardType(byte[] cbCardData, byte cbCardCount)
        {
            byte bKingCount=0;
            byte bTenCount=0;
            for(byte i=0;i<cbCardCount;i++)
            {
                if(GetCardValue(cbCardData[i])>10)
                {
                    bKingCount++;
                }
                else if(GetCardValue(cbCardData[i])==10)
                {
                    bTenCount++;
                }
            }
            if(bKingCount==MAX_COUNT)
            {
                return (byte)OX_FIVEKING;
            }
            else if(bKingCount==MAX_COUNT-1 && bTenCount==1)
            {
                return (byte)OX_FOURKING;
            }

            byte[] bTemp = new byte[MAX_COUNT];
            byte bSum=0;
            for (byte i=0;i<cbCardCount;i++)
            {
                bTemp[i]=GetCardLogicValue(cbCardData[i]);
                bSum+=bTemp[i];
            }

            for (int i=0;i<cbCardCount-1;i++)
            {
                for (int j=i+1;j<cbCardCount;j++)
                {
                    if((bSum-bTemp[i]-bTemp[j])%10==0)
                    {
                        return (byte)(((bTemp[i]+bTemp[j])>10)?(bTemp[i]+bTemp[j]-10):(bTemp[i]+bTemp[j]));
                    }
                }
            }

            return (byte)OX_VALUE0;
        }
        //比牌
        public static bool CompareCard(byte[] cbFirstData, byte[] cbNextData, byte cbCardCount,bool FirstOX,bool NextOX)
        {
            if(FirstOX !=NextOX) return (FirstOX==true?true:false);

             //比较牛大小
             if(FirstOX==true)
             {
                 //获取点数
                 byte cbNextType=GetCardType(cbNextData,cbCardCount);
                 byte cbFirstType=GetCardType(cbFirstData,cbCardCount);
            
                 //点数判断
                 if (cbFirstType!=cbNextType) return (cbFirstType>cbNextType);
             }
            
             //排序大小
             byte[] bFirstTemp = new byte[MAX_COUNT];
             byte[] bNextTemp = new byte[MAX_COUNT];

             Buffer.BlockCopy(cbFirstData,0,bFirstTemp,0,cbCardCount);
             Buffer.BlockCopy(cbNextData,0,bNextTemp,0,cbCardCount);

             SortCardList(ref bFirstTemp,cbCardCount);
             SortCardList(ref bNextTemp,cbCardCount);

             //比较数值
             byte cbNextMaxValue=GetCardValue(bNextTemp[0]);
             byte cbFirstMaxValue=GetCardValue(bFirstTemp[0]);
             if(cbNextMaxValue!=cbFirstMaxValue)return cbFirstMaxValue>cbNextMaxValue;
            
             //比较颜色
             return GetCardColor(bFirstTemp[0]) > GetCardColor(bNextTemp[0]);

        }
        //排序
        public static void SortCardList(ref byte[] cbCardData, byte cbCardCount)
        {
//            //转换数值
//            byte[] cbLogicValue = new byte[MAX_COUNT];
//            for (byte i = 0; i < cbCardCount; i++)
//                cbLogicValue[i] = GetCardLogicValue(cbCardData[i]);
//
//            //排序操作
//            bool bSorted = true;
//            byte cbTempData;
//            byte bLast = (Byte)(cbCardCount - 1);
//            do
//            {
//                bSorted = true;
//                for (byte i = 0; i < bLast; i++)
//                {
//                    if ((cbLogicValue[i] < cbLogicValue[i + 1]) || ((cbLogicValue[i] == cbLogicValue[i + 1]) && (cbCardData[i] < cbCardData[i + 1])))
//                    {
//                        //交换位置
//                        cbTempData = cbCardData[i];
//                        cbCardData[i] = cbCardData[i + 1];
//                        cbCardData[i + 1] = cbTempData;
//                        cbTempData = cbLogicValue[i];
//                        cbLogicValue[i] = cbLogicValue[i + 1];
//                        cbLogicValue[i + 1] = cbTempData;
//                        bSorted = false;
//                    }
//                }
//                bLast--;
//            } while (bSorted == false);

            return;
        }
        //摆牛
        public static void CombNN(ref byte[] cbCardData, byte cbCardCount)
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
        //获取倍数
        public static byte GetTimes(byte[] cbCardData, byte cbCardCount)
        {
             if(cbCardCount!=MAX_COUNT) return 0;

             byte bTimes = GetCardType(cbCardData,(byte)MAX_COUNT);
             if(bTimes<7)return 1;
             else if(bTimes==7)return 2;
             else if(bTimes==8)return 2;
             else if(bTimes==9)return 3;
             else if(bTimes==10)return 5;
             else if(bTimes==OX_FOURKING)return 5;
             else if(bTimes==OX_FIVEKING)return 5;
             return 0;
        }
        //获取牛牛
        public static bool GetOxCard(byte[] cbCardData, byte cbCardCount)
        {

            //设置变量
            byte[] bTemp = new byte[MAX_COUNT];
            byte[] bTempData = new byte[MAX_COUNT];
            Buffer.BlockCopy(cbCardData,0,bTempData,0,MAX_COUNT);

            byte bSum=0;
            for (byte i=0;i<cbCardCount;i++)
            {
                bTemp[i]=GetCardLogicValue(cbCardData[i]);
                bSum+=bTemp[i];
            }

            //查找牛牛
            for (int i=0;i<cbCardCount-1;i++)
            {
                for (int j=i+1;j<cbCardCount;j++)
                {
                    if((bSum-bTemp[i]-bTemp[j])%10==0)
                    {
                         byte bCount=0;
                         for (int k=0;k<cbCardCount;k++)
                         {
                             if(k!=i && k!=j)
                             {
                                 cbCardData[bCount++] = bTempData[k];
                             }
                         }
    
                         cbCardData[bCount++] = bTempData[i];
                         cbCardData[bCount++] = bTempData[j];
    
                         return true;
                    }
                }
            }
            
            return false;
        }
        //获取整数
        public static bool IsIntValue(byte[] cbCardData, byte cbCardCount)
        {
             byte sum=0;
             for(byte i=0;i<cbCardCount;i++)
             {
                 sum+=GetCardLogicValue(cbCardData[i]);
             }
             return (sum%10==0);
        }
        public static bool RemoveCard( byte[] cbRemoveCard, byte cbRemoveCount, ref byte[] cbCardData, ref byte cbCardCount)
        {
            byte cbDeleteCount = 0;
            byte[] cbTempCardData = new byte[MAX_COUNT];
            
            if (cbCardCount>MAX_COUNT)
             return false;
            
            Buffer.BlockCopy(cbCardData,0,cbTempCardData,0,cbCardCount);
            for (byte i=0;i<cbRemoveCount;i++)
            {
             for (byte j=0;j<cbCardCount;j++)
             {
                 if (cbRemoveCard[i]==cbTempCardData[j])
                 {
                     cbDeleteCount++;
                     cbTempCardData[j]=0;
                     break;
                 }
             }
            }
            if (cbDeleteCount!=cbRemoveCount) return false;
            
            byte cbCardPos=0;
            for (byte i=0;i<cbCardCount;i++)
            {
                if (cbTempCardData[i]!=0) cbCardData[cbCardPos++]=cbTempCardData[i];
            }
            cbCardCount = (byte)(cbCardCount - cbRemoveCount);
            return true;
        }
    }
}
