using System;
using Shared;

namespace com.QH.QPGame.CX
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
        public const ushort KIND_ID         = 205;
        public const ushort GAME_PLAYER     = 6;
        public const string GAME_NAME       = "扯旋";

        /// <summary>
        /// 最大簸簸数
        /// </summary>
        public static long GAME_MAX_INVERT = 10000;  
        /// <summary>
        /// 最小簸簸数
        /// </summary>
        public static long GAME_MIN_INVERT = 1000;
        /// <summary>
        /// 加减变化簸簸数
        /// </summary>
        public static int GAME_CHANGE_NUM = 500;
        /// <summary>
        /// 用户一局的输赢
        /// </summary>
        public static int GAME_CHANGE_RESLUT = 0;


		//当前游戏状态
        public const ushort GS_TK_FREE = (ushort)GameState.GS_FREE;
        public const ushort GS_TK_INVEST = (ushort)GameState.GS_PLAYING;
        public const ushort GS_TK_SCORE = (ushort)GameState.GS_PLAYING + 1;
        public const ushort GS_TK_OPEN_CARD = (ushort)GameState.GS_PLAYING + 2;

        public const byte NULL_CHAIR        = 255;
        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码
        public const ushort MAX_COUNT       = 4;    //最大数目

        public const ushort OX_VALUE0       = 0;    //混合牌型
        public const ushort OX_FOURKING     = 12;   //天王牌型
        public const ushort OX_FIVEKING     = 13;   //天王牌型

        //扑克类型
        public const byte D2H_VALUE0 = 0;							//无效牌型
        public const byte D2H_D2H = 199;							//丁二皇型
        public const byte D2H_DOBLUE_Q = 198;					        //天对牌型
        public const byte D2H_DOBLUE_2 = 197;							//地对牌型
        public const byte D2H_DOBLUE_8 = 196;							//仁对牌型
        public const byte D2H_DOBLUE_4 = 195;							//和对牌型
        //public const byte D2H_DOBLUE_46A = 194;							//中对牌型 拆
        //public const byte D2H_DOBLUE_67AJ = 193;							//下对牌型
        public const byte D2H_DOBLUE_M10 = 194; //梅十  中对牌型 拆
        public const byte D2H_DOBLUE_M4  = 193; //板凳
        public const byte D2H_DOBLUE_M6  = 192; //长三

        public const byte D2H_DOBLUE_HJ = 191;  //斧头
        public const byte D2H_DOBLUE_H10 = 190; //苕十
        public const byte D2H_DOBLUE_H6 = 189;  //猫猫
        public const byte D2H_DOBLUE_H7 = 188;  //膏药
        
        public const byte D2H_DOBLUE_9875 = 187;							//对子

        public const byte D2H_TH = 186;							//天皇牌型 奶狗
        public const byte D2H_TG = 185;							//天杠牌型
        public const byte D2H_DG = 184;							//地杠牌型
        //九点
        public const byte D2H_Q7 = 183;     //天关九
        public const byte D2H_27 = 182;     //地关九
        public const byte D2H_H8J = 181;    //灯笼九
        public const byte D2H_H45 = 180;    //和五九
        public const byte D2H_M45 = 179;    //板五九
        public const byte D2H_H36 = 178;    //丁长九
        public const byte D2H_M109 = 177;   //梅十九
        public const byte D2H_M36 = 176;    //丁猫九
        public const byte D2H_M8J = 175;    //乌龙九
        public const byte D2H_H109 = 174;   //苕十九

        public const byte D2H_TEN = 159;   //三花十
        public const byte D2H_SIX = 158;   //三花六
       

        /*
        0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//F A - K
        0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//M A - K
        0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//H A - K
        0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,	//B A - K
        0x4E,0x4F
         * 
        0x02,0x22,0x23,0x04,0x14,0x24,0x34,
	    0x25,0x35,0x06,0x16,0x26,0x36,
	    0x07,0x17,0x27,0x37,0x08,0x18,0x28,0x38,
	    0x29,0x39,0x0A,0x1A,0x2A,0x3A,0x1B,0x3B,0x0C,0x2C,
	    0x4E,
        */

        /// <summary>
        /// 获取牌型
        /// </summary>
        /// <param name="cbCardData"></param>
        /// <param name="cbCardCount"></param>
        /// <returns></returns>
        public static byte GetCardTypeN(byte[] cbCardData, byte cbCardCount)
        {
            if (cbCardCount != 2) return D2H_VALUE0;

            //丁二皇型
            if (cbCardData[0] == 0x23 && cbCardData[1] == 0x4E) return D2H_D2H;
            if (cbCardData[1] == 0x23 && cbCardData[0] == 0x4E) return D2H_D2H;

            	//对牌组型
	        byte []cbCardValue =
		        {0x0c,0x02,0x08,0x04,
		        0x14,0x16,0x1a,
		        0x06,0x07,0x0a,0x1b,
		        0x15,0x17,0x18,0x19};
	        byte [] cbCardValueType=
		        {D2H_DOBLUE_Q,D2H_DOBLUE_2,D2H_DOBLUE_8,D2H_DOBLUE_4,
		        D2H_DOBLUE_M4,D2H_DOBLUE_M6,D2H_DOBLUE_M10,
		        D2H_DOBLUE_H6,D2H_DOBLUE_H7,D2H_DOBLUE_H10,D2H_DOBLUE_HJ,
		        D2H_DOBLUE_9875,D2H_DOBLUE_9875,D2H_DOBLUE_9875,D2H_DOBLUE_9875};
            //查找对牌
            for (byte j = 0; j < cbCardValue.Length; j++)
            {
                byte cbValueData = GetCardValue(cbCardValue[j]);
                byte cbValueColor = (byte)(GetCardColor(cbCardValue[j]) >> 4);

                //清色对牌
                byte i = 0;
                if (cbValueColor != 2)
                {
                    for (i = 0; i < cbCardCount; i++)
                    {
                        byte cbColor = (byte)(GetCardColor(cbCardData[i]) >> 4);
                        if (GetCardValue(cbCardData[i]) != cbValueData || cbColor % 2 != cbValueColor) break;
                    }
                    if (i == cbCardCount) return cbCardValueType[j];
                }
                //混色对牌
                else
                {
                    for (i = 0; i < cbCardCount; i++)
                    {
                        if (GetCardValue(cbCardData[i]) != cbValueData) break;
                    }
                    if (i == cbCardCount)
                    {
                        byte cbColor1 = (byte)(GetCardColor(cbCardData[0]) >> 4);
                        byte cbColor2 = (byte)(GetCardColor(cbCardData[1]) >> 4);
                        if (cbColor1 % 2 != cbColor2 % 2) return cbCardValueType[j];
                    }
                }
            }

            //天王牌型
            byte cbFirstCard = GetCardValue(cbCardData[0]);
            byte cbSecondCard = GetCardValue(cbCardData[1]);
            if (cbFirstCard == 12 && cbSecondCard == 9) return D2H_TH;
            if (cbSecondCard == 12 && cbFirstCard == 9) return D2H_TH;

            //天杠牌型
            if (cbFirstCard == 12 && cbSecondCard == 8) return D2H_TG;
            if (cbSecondCard == 12 && cbFirstCard == 8) return D2H_TG;

            //地杠牌型
            if (cbFirstCard == 2 && cbSecondCard == 8) return D2H_DG;
            if (cbSecondCard == 2 && cbFirstCard == 8) return D2H_DG;

            //点数牌型
            byte cbTotalValue = 0;
            for (byte i = 0; i < cbCardCount; i++) cbTotalValue += GetCardLogicValue(cbCardData[i]);
            if (cbTotalValue >= 10) cbTotalValue %= 10;

            //天关九,地关九
            if(cbTotalValue == 9)
            {
                if (cbFirstCard == 12 || cbSecondCard == 12) return D2H_Q7;
                if (cbFirstCard == 2 || cbSecondCard == 2) return D2H_27;
                if (cbCardData[1] == 0x28 || cbCardData[1] == 0x08 || cbCardData[0] == 0x28 || cbCardData[0] == 0x08) return D2H_H8J;
                if (cbCardData[1] == 0x18 || cbCardData[1] == 0x38 || cbCardData[0] == 0x18 || cbCardData[0] == 0x38) return D2H_M8J;
                if (cbCardData[1] == 0x24 || cbCardData[1] == 0x04 || cbCardData[0] == 0x24 || cbCardData[0] == 0x04) return D2H_H45;
                if (cbCardData[1] == 0x14 || cbCardData[1] == 0x34 || cbCardData[0] == 0x14 || cbCardData[0] == 0x34) return D2H_M45;
                if (cbCardData[1] == 0x26 || cbCardData[1] == 0x06 || cbCardData[0] == 0x26 || cbCardData[0] == 0x06) return D2H_M36;
                if (cbCardData[1] == 0x16 || cbCardData[1] == 0x36 || cbCardData[0] == 0x16 || cbCardData[0] == 0x36) return D2H_H36;
                if (cbCardData[1] == 0x2A || cbCardData[1] == 0x0A || cbCardData[0] == 0x2A || cbCardData[0] == 0x0A) return D2H_H109;
                if (cbCardData[1] == 0x1A || cbCardData[1] == 0x3A || cbCardData[0] == 0x1A || cbCardData[0] == 0x3A) return D2H_M109;
            }

            return cbTotalValue;
        }

//（0x0A,	0x2A,） + （0x1A,  0x3A,） +  （0x1B,	0x3B,）						红10 + 黑10 + J 
//（0x06,	0x26,） + （0x16,  0x36,） +   0x4E									红6  + 黑6  + 大王
//判断特殊牌型
        public static byte GetPeaceful(byte[] cbCardData)
        {
            byte TenNum = 0;
            bool redTen = false;
            bool blackTen = false;
            bool jack = false;
            for (byte i = 0; i < 4; i++)
            {
                if ((cbCardData[i] == 0x0A || cbCardData[i] == 0x2A ) && (redTen == false))
                {
                    TenNum += 1;
                    redTen = true;
                    continue;
                }
                if ((cbCardData[i] == 0x1A || cbCardData[i] == 0x3A ) && (blackTen == false))
                {
                    TenNum += 1;
                    blackTen = true;
                    continue;
                }
                if ((cbCardData[i] == 0x1B || cbCardData[i] == 0x3B ) && (jack == false))
                {
                    TenNum += 1;
                    jack = true;
                    continue;
                }
            }
            if (TenNum >= 3) { return D2H_TEN;}


            byte SixNum = 0;
            bool redSix = false;
            bool blackSix = false;
            bool qeeun = false;
            for (byte i = 0; i < 4; i++)
            {
                if ((cbCardData[i] == 0x06 || cbCardData[i] == 0x26) && (redSix == false))
                {
                    SixNum += 1;
                    redSix = true;
                    continue;
                }
                if ((cbCardData[i] == 0x16 || cbCardData[i] == 0x36) && (blackSix == false))
                {
                    SixNum += 1;
                    blackSix = true;
                    continue;
                }
                if ((cbCardData[i] == 0x4E) && (qeeun == false))
                {
                    SixNum += 1;
                    qeeun = true;
                    continue;
                }
            }
            if (SixNum >= 3) { return D2H_SIX;}

            return D2H_VALUE0;
        }
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
            return (bCardColor == 0x40) ? (byte)(6) : bCardValue;
        }
        //获取牌型 这个没用
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
            //转换数值
            byte[] cbLogicValue = new byte[MAX_COUNT];
            for (byte i = 0; i < cbCardCount; i++)
                cbLogicValue[i] = GetCardValue(cbCardData[i]);

            //排序操作
            bool bSorted = true;
            byte cbTempData, bLast = (byte)(cbCardCount - 1);
            do
            {
                bSorted = true;
                for (byte i = 0; i < bLast; i++)
                {
                    if ((cbLogicValue[i] < cbLogicValue[i + 1]) ||
                        ((cbLogicValue[i] == cbLogicValue[i + 1]) && (cbCardData[i] < cbCardData[i + 1])))
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

        //混乱扑克
        public static void RandCardList(byte []cbCardBuffer, byte cbBufferCount)
        {
	        //CopyMemory(cbCardBuffer,m_cbCardListData,cbBufferCount);
            ////混乱准备
            //byte cbCardData[CountArray(m_cbCardListData)];
            //CopyMemory(cbCardData,m_cbCardListData,sizeof(m_cbCardListData));

            ////混乱扑克
            //byte bRandCount=0,bPosition=0;
            //do
            //{
            //    bPosition=rand()%(CountArray(m_cbCardListData)-bRandCount);
            //    cbCardBuffer[bRandCount++]=cbCardData[bPosition];
            //    cbCardData[bPosition]=cbCardData[CountArray(m_cbCardListData)-bRandCount];
            //} while (bRandCount<cbBufferCount);

	        return;
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


        public static void ChangeSpeCard(byte [] cbCardData, byte cbCardCount)
        {
            	byte cbSpeCount=2;
                byte []tmpcard = new byte [2];
                Buffer.BlockCopy(cbCardData,2,tmpcard,0,2);
	            byte cbCardType = GetCardTypeN(tmpcard,cbSpeCount);
	            if(cbCardType==D2H_D2H)return;

	            //查找牌型
	            byte cbThree=0xff,cbKing=0xff;
	            for(byte i=0;i<MAX_COUNT;i++)
	            {
		            if(cbThree==0xff && cbCardData[i]==0x23)cbThree=i;
		            if(cbKing==0xff && cbCardData[i]==0x4e)cbKing=i;
	            }

	            //最大牌型
	            if(cbThree!=0xff && cbKing!=0xff)
	            {
		            byte cbCount1=0,cbCount2=0;
		            byte []cbTempData1 = new byte[MAX_COUNT];
		            byte []cbTempData2 = new byte[MAX_COUNT];

		            for(byte i=0;i<MAX_COUNT;i++)
		            {
			            if(cbThree!=i && cbKing!=i)cbTempData1[cbCount1++]=cbCardData[i];
			            else cbTempData2[cbCount2++]=cbCardData[i];
		            }
                    Buffer.BlockCopy(cbTempData1,0,cbCardData,0,2);
                    Buffer.BlockCopy(cbTempData2,0,cbCardData,2,2);
		            //CopyMemory(cbCardData,cbTempData1,sizeof(BYTE)*2);
		            //CopyMemory(&cbCardData[cbSpeCount],cbTempData2,sizeof(BYTE)*2);
	            }
        }

        public static byte GetSpeCardValue(byte[] cbCardData, byte cbCardCount)
        {
            //特殊数值
	        byte []cbCardValue=
		        {0x0c,0x02,0x08,0x04,
		        0x14,0x16,0x1a,
		        0x06,0x07,0x0a,0x1b,
		        0x15,0x17,0x18,0x19,
		        0x0E,0x23};
	        byte []cbCardValueType=
		        {9,8,7,6,
		        5,5,5,
		        4,4,4,4,
		        3,3,3,3,
		        2,1};

	        //查找数值
	        byte cbAddValue = 0;
	        for(byte j=0;j<cbCardValue.Length;j++)
	        {
		        byte cbValueData = GetCardValue(cbCardValue[j]);
		        byte cbValueColor = (byte)(GetCardColor(cbCardValue[j])>>4);

		        //清色对牌
		        if(cbValueColor!=2)
		        {
			        for(byte i=0;i<cbCardCount;i++)
			        {
				        byte cbColor = (byte)(GetCardColor(cbCardData[i])>>4);
				        if(GetCardValue(cbCardData[i])==cbValueData && cbColor%2==cbValueColor)
				        {
					        cbAddValue=cbCardValueType[j];
					        break;
				        }
			        }
		        }
		        //混色对牌
		        else
		        {
			        for(byte i=0;i<cbCardCount;i++)
			        {
				        if(GetCardValue(cbCardData[i])!=cbValueData)continue;

				        cbAddValue=cbCardValueType[j];
				        break;
			        }
		        }

		        //查找结束
		        if(cbAddValue!=0)break;
	        }

	        return cbAddValue;
        }
        /// <summary>
        /// 获取牌型的图片
        /// </summary>
        /// <param name="cbCardData"></param>
        /// <param name="cbCardCount"></param>
        /// <returns></returns>
        public static string GetHeadTailTypeStr(byte[] cbCardData, byte cbCardCount)
        {
            byte cbcardtype = GetCardTypeN(cbCardData,cbCardCount);

            string cardname = "a_" + cbcardtype;
            return cardname;
        }

        /// <summary>
        /// 比牌
        /// </summary>
        /// <returns></returns>
        public static bool CompareCardN(byte[] cbFirstData, byte[] cbNextData, byte cbCardCount)
        {
            byte cbFirstType = GetCardTypeN(cbFirstData, cbCardCount);
            byte cbNextType = GetCardTypeN(cbNextData, cbCardCount);
            return (cbFirstType > cbNextType) ? true : false;
        }
        /// <summary>
        /// 交换牌
        /// </summary>
        /// <param name="cbFirstData"></param>
        /// <param name="cbNextData"></param>
        /// <param name="cbCardCount"></param>
        /// <param name="cbData1"></param>
        /// <param name="cbData2"></param>
        public static void ChangeCardData(byte[] cbFirstData, byte[] cbNextData, byte cbCardCount, out byte[] cbData1,out byte[] cbData2)
        {
            cbData1 = new byte[cbCardCount];
            cbData2 = new byte[cbCardCount];
            Buffer.BlockCopy(cbFirstData, 0, cbData2, 0, cbCardCount);
            Buffer.BlockCopy(cbNextData, 0, cbData1, 0, cbCardCount);
        }
    }
}
