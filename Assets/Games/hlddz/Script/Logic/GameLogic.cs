using System;
using Shared;

namespace com.QH.QPGame.DDZ
{
	public class tagAnalyseResult
	{
		public byte 	cbFourCount;						
		public byte 	cbThreeCount;						
		public byte 	cbDoubleCount;						
		public byte		cbSignedCount;						
		public byte[]	cbFourCardData 	 = new byte[GameLogic.MAX_COUNT];			
		public byte[]	cbThreeCardData  = new byte[GameLogic.MAX_COUNT];		
		public byte[]	cbDoubleCardData = new byte[GameLogic.MAX_COUNT];			
		public byte[]	cbSignedCardData = new byte[GameLogic.MAX_COUNT];	

	};
	
	public class tagOutCardResult
	{
		public byte		cbCardCount;
		public byte[]	cbResultCard = new byte[GameLogic.MAX_COUNT];
	};

	
	public class GameLogic
	{
		//
		public const ushort GAME_PLAYER_NUM 			=	3;
		
		public const ushort GS_WK_FREE					=	(ushort)GameState.GS_FREE;
		public const ushort GS_WK_SCORE					=	(ushort)GameState.GS_PLAYING;
		public const ushort GS_WK_PLAYING				=	(ushort)GameState.GS_PLAYING+1;
		
		public const byte 	ST_ORDER					=	0;
		public const byte 	ST_COUNT					=	1;
		
		public const ushort MAX_COUNT					=	20;
		public const ushort FULL_COUNT					=	54;
		public const ushort BACK_COUNT					=	3;
		public const ushort NORMAL_COUNT				=	17;
		
		public const byte	NULL_CHAIR					=	255;
		
		public const ushort MASK_COLOR					=	0xF0;
		public const ushort MASK_VALUE					=	0x0F;
		
		public const byte 	CT_ERROR					=	0;              //错误
		public const byte 	CT_SINGLE					=	1;              //一张
		public const byte 	CT_DOUBLE					=	2;              //两张
		public const byte 	CT_THREE					=	3;              //三张
		public const byte 	CT_SINGLE_LINE				=	4;              //顺子
		public const byte 	CT_DOUBLE_LINE				=	5;              //连对
		public const byte 	CT_THREE_LINE				=	6;              //飞机
		public const byte 	CT_THREE_LINE_TAKE_ONE		=	7;              //三带1
		public const byte 	CT_THREE_LINE_TAKE_TWO		=	8;              //三带2
		public const byte 	CT_FOUR_LINE_TAKE_ONE		=	9;              //四带一
		public const byte 	CT_FOUR_LINE_TAKE_TWO		=	10;             //四带二
		public const byte 	CT_BOMB_CARD				=	11;             //炸弹
		public const byte 	CT_MISSILE_CARD				=	12;             //火箭
		
		//
		public static byte[] 	HandCardData  	 = new byte[MAX_COUNT];
		public static byte	 	HandCardCount 	 = 0;
		//
		public static byte[,] 	OutCardData 	 = new byte[GAME_PLAYER_NUM,MAX_COUNT];
		public static byte[]   	OutCardCount 	 = new byte[GAME_PLAYER_NUM];
		//
		
		public const  int   TIME_OUT_CARD               =   0;
        public const  int   TIME_LAND_SCORE             =   5;
        public const  int   TIME_READY                  =   15;
        public const  int   TIME_DOUBLE                 =   5;

        //
        public const  int   AUTO_TIME_MIN               =   3;
        public const  int   AUTO_TIME_MAX               =   5;


        //获取出牌时间
        public static byte GetOutCardTime(byte _bHandCardCount)
        {
            byte time = 0;
            if (_bHandCardCount > 17) { time = 30; }
            else if (_bHandCardCount >= 12 && _bHandCardCount <= 17) { time = 20; }
            else if (_bHandCardCount >= 7 && _bHandCardCount <= 11){ time = 15; }
            else if (_bHandCardCount >= 2 && _bHandCardCount <= 6 ){ time = 10; }
            else if (_bHandCardCount == 1){ time = 1; }

            return time;
        }

		//混乱扑克
        public static void RandCardList(byte[] cbOldCards,ref byte[] cbNewCards, byte cbCount)
        {
             //混乱准备
             byte[] cbCardData = new byte[54];
             Buffer.BlockCopy(cbOldCards,0,cbCardData,0,54);

             //混乱扑克
             byte cbRandCount=0;
             byte cbPosition=0;
             do
             {
                 cbPosition=(byte)(UnityEngine.Random.Range(0,53) % (cbCount-cbRandCount));
                 cbNewCards[cbRandCount++]=cbCardData[cbPosition];
                 cbCardData[cbPosition]=cbCardData[cbCount-cbRandCount];
             } while (cbRandCount<cbCount);
        }
		//
		public static bool IsValidCard(byte cbCardData)
		{
			byte cbCardColor=GetCardColor(cbCardData);
			byte cbCardValue=GetCardValue(cbCardData);
		
			if ((cbCardData==0x4E)||(cbCardData==0x4F)) return true;
			if ((cbCardColor<=0x30)&&(cbCardValue>=0x01)&&(cbCardValue<=0x0D)) return true;
		
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

			byte cbCardColor=GetCardColor(cbCardData);
			byte cbCardValue=GetCardValue(cbCardData);
		

			if (cbCardColor==0x40) return (byte)(cbCardValue+2);
			return (byte) ((cbCardValue<=2)?(cbCardValue+13):cbCardValue);
		}
        //
        public static bool HasContain(byte[] cbCardData,byte cbCardCount,byte[] cbChildCards,byte cbChildCardCount)
        {
            byte[] flags = new byte[cbChildCardCount];
            Array.Clear(flags,0,cbChildCardCount);

            for(int c=0;c < cbChildCardCount;c++)
            {
                byte cCard = cbChildCards[c];
                for(int p=0;p < cbCardCount;p++)
                {
                    byte pcard = cbCardData[p];
                    if( GetCardLogicValue(cCard)==GetCardLogicValue(pcard))
                    {
                        flags[c]=1;
                        break;
                    }
                }
            }

            int nTotals = 0;
            for(int i=0;i<cbChildCardCount;i++)
            {
                nTotals += flags[i];
            }

            if(nTotals==cbChildCardCount)
            {
                UnityEngine.Debug.Log("has contain");
                return true;
            }
            else
            {
                UnityEngine.Debug.Log("not contain");
                return false;
            }
        }
		//
		public static bool AnalysebCardData( ref byte[] cbCardData, byte cbCardCount,ref tagAnalyseResult  AnalyseResult)
		{
			
			for(byte i=0;i<cbCardCount;i++)
			{
				byte cbSameCount=1;
				byte cbLogicValue=GetCardLogicValue(cbCardData[i]);
				if(cbLogicValue<=0) 
				{
					return false;
				}
		
				for (byte j= (byte)(i+1);j<cbCardCount;j++)
				{
			
					if (GetCardLogicValue(cbCardData[j])!=cbLogicValue) break;

					cbSameCount++;
				}
		

				switch (cbSameCount)
				{
				case 1:		
					{
						byte cbIndex=AnalyseResult.cbSignedCount++;
						AnalyseResult.cbSignedCardData[cbIndex*cbSameCount]=cbCardData[i];
						break;
					}
				case 2:		
					{
						byte cbIndex=AnalyseResult.cbDoubleCount++;
						AnalyseResult.cbDoubleCardData[cbIndex*cbSameCount]=cbCardData[i];
						AnalyseResult.cbDoubleCardData[cbIndex*cbSameCount+1]=cbCardData[i+1];
						break;
					}
				case 3:		
					{
						byte cbIndex=AnalyseResult.cbThreeCount++;
						AnalyseResult.cbThreeCardData[cbIndex*cbSameCount]=cbCardData[i];
						AnalyseResult.cbThreeCardData[cbIndex*cbSameCount+1]=cbCardData[i+1];
						AnalyseResult.cbThreeCardData[cbIndex*cbSameCount+2]=cbCardData[i+2];
						break;
					}
				case 4:		
					{
						byte cbIndex=AnalyseResult.cbFourCount++;
						AnalyseResult.cbFourCardData[cbIndex*cbSameCount]=cbCardData[i];
						AnalyseResult.cbFourCardData[cbIndex*cbSameCount+1]=cbCardData[i+1];
						AnalyseResult.cbFourCardData[cbIndex*cbSameCount+2]=cbCardData[i+2];
						AnalyseResult.cbFourCardData[cbIndex*cbSameCount+3]=cbCardData[i+3];
						break;
					}
				}
		

				i = (byte)(i + cbSameCount-1);
			}
		
			return true;
		}
		//
		public static byte GetCardType(byte[] cbCardData, byte cbCardCount)
		{

			switch(cbCardCount)
			{
			case 0:	
				{
					return CT_ERROR;
				}
			case 1: 
				{
					return CT_SINGLE;
				}
			case 2:	
				{
					if ((cbCardData[0]==0x4F)&&(cbCardData[1]==0x4E)) return CT_MISSILE_CARD;
					if (GetCardLogicValue(cbCardData[0])==GetCardLogicValue(cbCardData[1])) return CT_DOUBLE;
		
					return CT_ERROR;
				}
			}
			
			tagAnalyseResult AnalyseResult = new tagAnalyseResult();
			AnalysebCardData(ref cbCardData,cbCardCount, ref AnalyseResult);
		

			if (AnalyseResult.cbFourCount>0)
			{

				if ((AnalyseResult.cbFourCount==1)&&(cbCardCount==4)) return CT_BOMB_CARD;
				if ((AnalyseResult.cbFourCount==1)&&(AnalyseResult.cbSignedCount==2)&&(cbCardCount==6)) return CT_FOUR_LINE_TAKE_ONE;
				if ((AnalyseResult.cbFourCount==1)&&(AnalyseResult.cbDoubleCount==2)&&(cbCardCount==8)) return CT_FOUR_LINE_TAKE_TWO;
		
				return CT_ERROR;
			}
		

			if (AnalyseResult.cbThreeCount>0)
			{

				if(AnalyseResult.cbThreeCount==1 && cbCardCount==3) return CT_THREE ;
		

				if (AnalyseResult.cbThreeCount>1)
				{
	
					byte bCardData=AnalyseResult.cbThreeCardData[0];
					byte cbFirstLogicValue=GetCardLogicValue(bCardData);
		
	
					if (cbFirstLogicValue>=15) return CT_ERROR;
		
				
					for (byte i=1;i<AnalyseResult.cbThreeCount;i++)
					{
						byte bCard=AnalyseResult.cbThreeCardData[i*3];
						if (cbFirstLogicValue!=(GetCardLogicValue(bCard)+i)) return CT_ERROR;
					}
				}
		
	
				if (AnalyseResult.cbThreeCount*3==cbCardCount) return CT_THREE_LINE;
				if (AnalyseResult.cbThreeCount*4==cbCardCount) return CT_THREE_LINE_TAKE_ONE;
				if ((AnalyseResult.cbThreeCount*5==cbCardCount)&&(AnalyseResult.cbDoubleCount==AnalyseResult.cbThreeCount)) return CT_THREE_LINE_TAKE_TWO;
		
				return CT_ERROR;
			}
		

			if (AnalyseResult.cbDoubleCount>=3)
			{
			
				byte bCardData=AnalyseResult.cbDoubleCardData[0];
				byte cbFirstLogicValue=GetCardLogicValue(bCardData);
		
		
				if (cbFirstLogicValue>=15) return CT_ERROR;
		
			
				for (byte i=1;i<AnalyseResult.cbDoubleCount;i++)
				{
					byte bCard=AnalyseResult.cbDoubleCardData[i*2];
					if (cbFirstLogicValue!=(GetCardLogicValue(bCard)+i)) return CT_ERROR;
				}
		
			
				if ((AnalyseResult.cbDoubleCount*2)==cbCardCount) return CT_DOUBLE_LINE;
		
				return CT_ERROR;
			}
		
	
			if ((AnalyseResult.cbSignedCount>=5)&&(AnalyseResult.cbSignedCount==cbCardCount))
			{
			
				byte bCardData=AnalyseResult.cbSignedCardData[0];
				byte cbFirstLogicValue=GetCardLogicValue(bCardData);
		
			
				if (cbFirstLogicValue>=15) return CT_ERROR;
		
		
				for (byte i=1;i<AnalyseResult.cbSignedCount;i++)
				{
					byte bCard=AnalyseResult.cbSignedCardData[i];
					if (cbFirstLogicValue!=(GetCardLogicValue(bCard)+i)) return CT_ERROR;
				}
		
				return CT_SINGLE_LINE;
			}
			
			return CT_ERROR;
		}
		//
		public static bool CompareCard(byte[] cbFirstCard, byte[] cbNextCard, byte cbFirstCount, byte cbNextCount)
		{

			byte cbNextType=GetCardType(cbNextCard,cbNextCount);
			byte cbFirstType=GetCardType(cbFirstCard,cbFirstCount);
		
		
			if (cbNextType==CT_ERROR) return false;
			if (cbNextType==CT_MISSILE_CARD) return true;
		
	
			if ((cbFirstType!=CT_BOMB_CARD)&&(cbNextType==CT_BOMB_CARD)) return true;
			if ((cbFirstType==CT_BOMB_CARD)&&(cbNextType!=CT_BOMB_CARD)) return false;
		

			if ((cbFirstType!=cbNextType)||(cbFirstCount!=cbNextCount)) return false;
		

			switch (cbNextType)
			{
			case CT_SINGLE:
			case CT_DOUBLE:
			case CT_THREE:
			case CT_SINGLE_LINE:
			case CT_DOUBLE_LINE:
			case CT_THREE_LINE:
			case CT_BOMB_CARD:
				{
					
					byte cbNextLogicValue=GetCardLogicValue(cbNextCard[0]);
					byte cbFirstLogicValue=GetCardLogicValue(cbFirstCard[0]);
		
					
					return cbNextLogicValue>cbFirstLogicValue;
				}
			case CT_THREE_LINE_TAKE_ONE:
			case CT_THREE_LINE_TAKE_TWO:
				{
					
					tagAnalyseResult NextResult = new tagAnalyseResult();
					tagAnalyseResult FirstResult = new tagAnalyseResult();
					AnalysebCardData(ref cbNextCard,cbNextCount,ref NextResult);
					AnalysebCardData(ref cbFirstCard,cbFirstCount,ref FirstResult);
		
					
					byte cbNextLogicValue=GetCardLogicValue(NextResult.cbThreeCardData[0]);
					byte cbFirstLogicValue=GetCardLogicValue(FirstResult.cbThreeCardData[0]);
		
	
					return cbNextLogicValue>cbFirstLogicValue;
				}
			case CT_FOUR_LINE_TAKE_ONE:
			case CT_FOUR_LINE_TAKE_TWO:
				{
		
					tagAnalyseResult NextResult = new tagAnalyseResult();
					tagAnalyseResult FirstResult = new tagAnalyseResult();
					AnalysebCardData(ref cbNextCard,cbNextCount, ref NextResult);
					AnalysebCardData(ref cbFirstCard,cbFirstCount,ref FirstResult);
		
				
					byte cbNextLogicValue=GetCardLogicValue(NextResult.cbFourCardData[0]);
					byte cbFirstLogicValue=GetCardLogicValue(FirstResult.cbFourCardData[0]);
		
			
					return cbNextLogicValue>cbFirstLogicValue;
				}
			}
			
			return false;
		}
		//
		public static void SortCardList(ref byte[] cbCardData, byte cbCardCount, byte cbSortType)
		{

			if (cbCardCount==0) return;
		
		
			byte[] cbSortValue = new byte[MAX_COUNT];
			for (byte i=0;i<cbCardCount;i++) 
			{
				cbSortValue[i]=GetCardLogicValue(cbCardData[i]);	
		
			}
			bool bSorted=true;
			byte cbThreeCount,cbLast=(byte)(cbCardCount-1);
			do
			{
				bSorted=true;
				for (byte i=0;i<cbLast;i++)
				{
					if ((cbSortValue[i]<cbSortValue[i+1])||
						((cbSortValue[i]==cbSortValue[i+1])&&(cbCardData[i]<cbCardData[i+1])))
					{
				
						cbThreeCount=cbCardData[i];
						cbCardData[i]=cbCardData[i+1];
						cbCardData[i+1]=cbThreeCount;
						cbThreeCount=cbSortValue[i];
						cbSortValue[i]=cbSortValue[i+1];
						cbSortValue[i+1]=cbThreeCount;
						bSorted=false;
					}	
				}
				cbLast--;
			} 
			while(bSorted==false);
		
	
			if (cbSortType==ST_COUNT)
			{
		
				byte cbIndex=0;
				tagAnalyseResult AnalyseResult = new tagAnalyseResult();
				AnalysebCardData(ref cbCardData,cbCardCount,ref AnalyseResult);
		
			
				Buffer.BlockCopy(AnalyseResult.cbFourCardData,0,cbCardData,cbIndex,sizeof(byte)*AnalyseResult.cbFourCount*4);
				cbIndex+=(byte)(AnalyseResult.cbFourCount*4);
		
			
				Buffer.BlockCopy(AnalyseResult.cbThreeCardData,0,cbCardData,cbIndex,sizeof(byte)*AnalyseResult.cbThreeCount*3);
				cbIndex+=(byte)(AnalyseResult.cbThreeCount*3);
		
			
				Buffer.BlockCopy(AnalyseResult.cbDoubleCardData,0,cbCardData,cbIndex,sizeof(byte)*AnalyseResult.cbDoubleCount*2);
				cbIndex+=(byte)(AnalyseResult.cbDoubleCount*2);
		
				
				Buffer.BlockCopy(AnalyseResult.cbSignedCardData,0,cbCardData,cbIndex,sizeof(byte)*AnalyseResult.cbSignedCount);
				cbIndex+=(byte)(AnalyseResult.cbSignedCount);
			}
		
			return;
		}
        //
        public static void GetSingeCard(byte[] cbHandCards,byte cbHandCardCount,ref byte[] cbLineCardData,ref byte cbLineCardCount)
        {

             byte[] cbTmpCard = new byte[20];
             Buffer.BlockCopy(cbHandCards,0,cbTmpCard,0,cbHandCardCount);

             //short
             SortCardList(ref cbTmpCard, cbHandCardCount, ST_ORDER);
            
             cbLineCardCount = 0 ;

             //
             if(cbHandCardCount<5) return ;

             byte cbFirstCard = 0 ;
             //
             for(byte i=0 ; i<cbHandCardCount ; ++i)
             {
                if(GetCardLogicValue(cbTmpCard[i])<15)
                {
                    cbFirstCard = i ;
                    break ;
                }
             }

             byte[] cbSingleLineCard = new byte[12];
             byte cbSingleLineCount  = 0;
             byte cbLeftCardCount    = cbHandCardCount ;
             bool bFindSingleLine    = true ;
            
             //连牌判断
             while (cbLeftCardCount>=5 && bFindSingleLine)
             {
                 cbSingleLineCount = 1;
                 bFindSingleLine   = false;
                 byte cbLastCard   = cbTmpCard[cbFirstCard];
                 cbSingleLineCard[cbSingleLineCount-1] = cbTmpCard[cbFirstCard];
                 for(byte i=(byte)(cbFirstCard+1); i<cbLeftCardCount; i++)
                 {
                     byte cbCardData=cbTmpCard[i];

                     //
                     if (1!=(GetCardLogicValue(cbLastCard)-GetCardLogicValue(cbCardData)) && GetCardValue(cbLastCard)!=GetCardValue(cbCardData))
                     {
                         cbLastCard = cbTmpCard[i] ;
                         if(cbSingleLineCount<5)
                         {
                             cbSingleLineCount = 1 ;
                             cbSingleLineCard[cbSingleLineCount-1] = cbTmpCard[i] ;
                             continue ;
                         }
                         else break ;
                     }
                     //
                     else if(GetCardValue(cbLastCard)!=GetCardValue(cbCardData))
                     {
                         cbLastCard = cbCardData ;
                         cbSingleLineCard[cbSingleLineCount] = cbCardData ;
                         ++cbSingleLineCount ;
                     }
                 }

                 //保存数据
                 if(cbSingleLineCount>=5)
                 {
                     RemoveCard(cbSingleLineCard, cbSingleLineCount, ref cbTmpCard, ref cbLeftCardCount) ;
                     Buffer.BlockCopy(cbSingleLineCard,0,cbLineCardData,cbLineCardCount,cbSingleLineCount);
                     cbLineCardCount += cbSingleLineCount ;
                     cbLeftCardCount -= cbSingleLineCount ;
                     bFindSingleLine = true ;
                 }
             }
        }
		//
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
		//
		public static bool SearchOutCard(byte[] cbHandCardData, byte cbHandCardCount, byte[] cbTurnCardData, byte cbTurnCardCount, ref tagOutCardResult OutCardResult)
		{
		
			
			byte[] cbCardData = new byte[MAX_COUNT];
			byte cbCardCount=cbHandCardCount;
			Buffer.BlockCopy(cbHandCardData,0,cbCardData,0,cbHandCardCount);
		
			
			SortCardList(ref cbCardData,cbCardCount,ST_ORDER);
		
			
			byte cbTurnOutType=GetCardType(cbTurnCardData,cbTurnCardCount);
		
			
			switch (cbTurnOutType)
			{
			case CT_ERROR:					
				{
					
					byte cbLogicValue=GetCardLogicValue(cbCardData[cbCardCount-1]);
		
					
					byte cbSameCount=1;
					for (byte i=1;i<cbCardCount;i++)
					{
						if (GetCardLogicValue(cbCardData[cbCardCount-i-1])==cbLogicValue) cbSameCount++;
						else break;
					}
		
					
					if (cbSameCount>1)
					{
						OutCardResult.cbCardCount=cbSameCount;
						for (byte j=0;j<cbSameCount;j++) OutCardResult.cbResultCard[j]=cbCardData[cbCardCount-1-j];
						return true;
					}
		
					
					OutCardResult.cbCardCount=1;
					OutCardResult.cbResultCard[0]=cbCardData[cbCardCount-1];
		
					return true;
				}
			case CT_SINGLE:					
			case CT_DOUBLE:					
			case CT_THREE:					
				{
					
					byte cbLogicValue=GetCardLogicValue(cbTurnCardData[0]);
		
					
					tagAnalyseResult AnalyseResult = new tagAnalyseResult();
					AnalysebCardData(ref cbCardData,cbCardCount,ref AnalyseResult);
		
					
					if (cbTurnCardCount<=1)
					{
						for (byte i=0;i<AnalyseResult.cbSignedCount;i++)
						{
							byte cbIndex=(byte)(AnalyseResult.cbSignedCount-i-1);
							if (GetCardLogicValue(AnalyseResult.cbSignedCardData[cbIndex])>cbLogicValue)
							{
								
								OutCardResult.cbCardCount=cbTurnCardCount;
								Buffer.BlockCopy(AnalyseResult.cbSignedCardData,cbIndex,OutCardResult.cbResultCard,0,cbTurnCardCount);
								return true;
							}
						}
					}
		
					
					if (cbTurnCardCount<=2)
					{
						for (byte i=0;i<AnalyseResult.cbDoubleCount;i++)
						{
							byte cbIndex=(byte)((AnalyseResult.cbDoubleCount-i-1)*2);
							if (GetCardLogicValue(AnalyseResult.cbDoubleCardData[cbIndex])>cbLogicValue)
							{
								
								OutCardResult.cbCardCount=cbTurnCardCount;
								Buffer.BlockCopy(AnalyseResult.cbDoubleCardData,cbIndex,OutCardResult.cbResultCard,0,cbTurnCardCount);
		
								return true;
							}
						}
					}
		
					
					if (cbTurnCardCount<=3)
					{
						for (byte i=0;i<AnalyseResult.cbThreeCount;i++)
						{
							byte cbIndex=(byte)((AnalyseResult.cbThreeCount-i-1)*3);
							if (GetCardLogicValue(AnalyseResult.cbThreeCardData[cbIndex])>cbLogicValue)
							{
								
								OutCardResult.cbCardCount=cbTurnCardCount;
								Buffer.BlockCopy(AnalyseResult.cbThreeCardData,cbIndex,OutCardResult.cbResultCard,0,cbTurnCardCount);
								return true;
							}
						}
					}
		
					break;
				}
			case CT_SINGLE_LINE:		
				{
			
					if (cbCardCount<cbTurnCardCount) break;
		
					
					byte cbLogicValue=GetCardLogicValue(cbTurnCardData[0]);
		
					
					for (byte i=(byte)(cbTurnCardCount-1);i<cbCardCount;i++)
					{
						
						byte cbHandLogicValue=GetCardLogicValue(cbCardData[cbCardCount-i-1]);
		
						
						if (cbHandLogicValue>=15) break;
						if (cbHandLogicValue<=cbLogicValue) continue;
		
					
						byte cbLineCount=0;
						for (byte j=(byte)(cbCardCount-i-1);j<cbCardCount;j++)
						{
							if ((GetCardLogicValue(cbCardData[j])+cbLineCount)==cbHandLogicValue) 
							{
								
								OutCardResult.cbResultCard[cbLineCount++]=cbCardData[j];
		
								
								if (cbLineCount==cbTurnCardCount)
								{
									OutCardResult.cbCardCount=cbTurnCardCount;
									return true;
								}
							}
						}
					}
		
					break;
				}
			case CT_DOUBLE_LINE:		
				{
				
					if (cbCardCount<cbTurnCardCount) break;
		
					
					byte cbLogicValue=GetCardLogicValue(cbTurnCardData[0]);
		
					
					for (byte i=(byte)(cbTurnCardCount-1);i<cbCardCount;i++)
					{
						
						byte cbHandLogicValue=GetCardLogicValue(cbCardData[cbCardCount-i-1]);
		
						
						if (cbHandLogicValue<=cbLogicValue) continue;
						if ((cbTurnCardCount>1)&&(cbHandLogicValue>=15)) break;
		
						
						byte cbLineCount=0;
						for (byte j=(byte)(cbCardCount-i-1);j<(cbCardCount-1);j++)
						{
							if (((GetCardLogicValue(cbCardData[j])+cbLineCount)==cbHandLogicValue)
								&&((GetCardLogicValue(cbCardData[j+1])+cbLineCount)==cbHandLogicValue))
							{
						
								OutCardResult.cbResultCard[cbLineCount*2]=cbCardData[j];
								OutCardResult.cbResultCard[(cbLineCount++)*2+1]=cbCardData[j+1];
		
								
								if (cbLineCount*2==cbTurnCardCount)
								{
									OutCardResult.cbCardCount=cbTurnCardCount;
									return true;
								}
							}
						}
					}
		
					break;
				}
			case CT_THREE_LINE:				
			case CT_THREE_LINE_TAKE_ONE:	
			case CT_THREE_LINE_TAKE_TWO:	
				{
				
					if (cbCardCount<cbTurnCardCount) break;
		
					
					byte cbLogicValue=0;
					for (byte i=0;i<cbTurnCardCount-2;i++)
					{
						cbLogicValue=GetCardLogicValue(cbTurnCardData[i]);
						if (GetCardLogicValue(cbTurnCardData[i+1])!=cbLogicValue) continue;
						if (GetCardLogicValue(cbTurnCardData[i+2])!=cbLogicValue) continue;
						break;
					}
		
				
					byte cbTurnLineCount=0;
					if (cbTurnOutType==CT_THREE_LINE_TAKE_ONE) cbTurnLineCount=(byte)(cbTurnCardCount/4);
					else if (cbTurnOutType==CT_THREE_LINE_TAKE_TWO) cbTurnLineCount=(byte)(cbTurnCardCount/5);
					else cbTurnLineCount=(byte)(cbTurnCardCount/3);
		
				
					for (byte i=(byte)(cbTurnLineCount*3-1);i<cbCardCount;i++)
					{
						
						byte cbHandLogicValue=GetCardLogicValue(cbCardData[cbCardCount-i-1]);
		
						
						if (cbHandLogicValue<=cbLogicValue) continue;
						if ((cbTurnLineCount>1)&&(cbHandLogicValue>=15)) break;
		
					
						byte cbLineCount=0;
						for (byte j=(byte)(cbCardCount-i-1);j<(byte)(cbCardCount-2);j++)
						{
						
							OutCardResult.cbCardCount=0;
		
						
							if ((GetCardLogicValue(cbCardData[j])+cbLineCount)!=cbHandLogicValue) continue;
							if ((GetCardLogicValue(cbCardData[j+1])+cbLineCount)!=cbHandLogicValue) continue;
							if ((GetCardLogicValue(cbCardData[j+2])+cbLineCount)!=cbHandLogicValue) continue;
		
							
							OutCardResult.cbResultCard[cbLineCount*3]=cbCardData[j];
							OutCardResult.cbResultCard[cbLineCount*3+1]=cbCardData[j+1];
							OutCardResult.cbResultCard[(cbLineCount++)*3+2]=cbCardData[j+2];
		
					
							if (cbLineCount==cbTurnLineCount)
							{
							
								OutCardResult.cbCardCount=(byte)(cbLineCount*3);
		
						
								byte[] cbLeftCardData = new byte[MAX_COUNT];
								byte cbLeftCount=(byte)(cbCardCount-OutCardResult.cbCardCount);
								//CopyMemory(cbLeftCardData,cbCardData,sizeof(byte)*cbCardCount);
								Buffer.BlockCopy(cbCardData,0,cbLeftCardData,0,cbCardCount);
								RemoveCard(OutCardResult.cbResultCard,OutCardResult.cbCardCount,ref cbLeftCardData,ref cbCardCount);
		
								
								tagAnalyseResult AnalyseResultLeft = new tagAnalyseResult();
								AnalysebCardData(ref cbLeftCardData,cbLeftCount,ref AnalyseResultLeft);
		
							
								if (cbTurnOutType==CT_THREE_LINE_TAKE_ONE)
								{
									
									for (byte k=0;k<AnalyseResultLeft.cbSignedCount;k++)
									{
										
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)(AnalyseResultLeft.cbSignedCount-k-1);
										byte cbSignedCard=AnalyseResultLeft.cbSignedCardData[cbIndex];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbSignedCard;
									}
		
									
									for (byte k=0;k<AnalyseResultLeft.cbDoubleCount*2;k++)
									{
										
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)(AnalyseResultLeft.cbDoubleCount*2-k-1);
										byte cbSignedCard=AnalyseResultLeft.cbDoubleCardData[cbIndex];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbSignedCard;
									}
		
									
									for (byte k=0;k<AnalyseResultLeft.cbThreeCount*3;k++)
									{
										
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)(AnalyseResultLeft.cbThreeCount*3-k-1);
										byte cbSignedCard=AnalyseResultLeft.cbThreeCardData[cbIndex];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbSignedCard;
									}
		
									
									for (byte k=0;k<AnalyseResultLeft.cbFourCount*4;k++)
									{
										
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)(AnalyseResultLeft.cbFourCount*4-k-1);
										byte cbSignedCard=AnalyseResultLeft.cbFourCardData[cbIndex];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbSignedCard;
									}
								}
		
								
								if (cbTurnOutType==CT_THREE_LINE_TAKE_TWO)
								{
									
									for (byte k=0;k<AnalyseResultLeft.cbDoubleCount;k++)
									{
										
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)((AnalyseResultLeft.cbDoubleCount-k-1)*2);
										byte cbCardData1=AnalyseResultLeft.cbDoubleCardData[cbIndex];
										byte cbCardData2=AnalyseResultLeft.cbDoubleCardData[cbIndex+1];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData1;
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData2;
									}
		
									
									for (byte k=0;k<AnalyseResultLeft.cbThreeCount;k++)
									{
									
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
									
										byte cbIndex=(byte)((AnalyseResultLeft.cbThreeCount-k-1)*3);
										byte cbCardData1=AnalyseResultLeft.cbThreeCardData[cbIndex];
										byte cbCardData2=AnalyseResultLeft.cbThreeCardData[cbIndex+1];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData1;
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData2;
									}
		
							
									for (byte k=0;k<AnalyseResultLeft.cbFourCount;k++)
									{
									
										if (OutCardResult.cbCardCount==cbTurnCardCount) break;
		
										
										byte cbIndex=(byte)((AnalyseResultLeft.cbFourCount-k-1)*4);
										byte cbCardData1=AnalyseResultLeft.cbFourCardData[cbIndex];
										byte cbCardData2=AnalyseResultLeft.cbFourCardData[cbIndex+1];
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData1;
										OutCardResult.cbResultCard[OutCardResult.cbCardCount++]=cbCardData2;
									}
								}
		
								
								if (OutCardResult.cbCardCount==cbTurnCardCount) return true;
							}
						}
					}
		
					break;
				}
			}
		
		
			if ((cbCardCount>=4)&&(cbTurnOutType!=CT_MISSILE_CARD))
			{
			
				byte cbLogicValue=0;
				if (cbTurnOutType==CT_BOMB_CARD) cbLogicValue=GetCardLogicValue(cbTurnCardData[0]);
		
	
				for (byte i=3;i<cbCardCount;i++)
				{
			
					byte cbHandLogicValue=GetCardLogicValue(cbCardData[cbCardCount-i-1]);
		
				
					if (cbHandLogicValue<=cbLogicValue) continue;
		
				
					byte cbTempLogicValue=GetCardLogicValue(cbCardData[cbCardCount-i-1]);
					byte j = 0;
					for ( j=1;j<4;j++)
					{
						if (GetCardLogicValue(cbCardData[cbCardCount+j-i-1])!=cbTempLogicValue) break;
					}
					if (j!=4) continue;
		
	
					OutCardResult.cbCardCount=4;
					OutCardResult.cbResultCard[0]=cbCardData[cbCardCount-i-1];
					OutCardResult.cbResultCard[1]=cbCardData[cbCardCount-i];
					OutCardResult.cbResultCard[2]=cbCardData[cbCardCount-i+1];
					OutCardResult.cbResultCard[3]=cbCardData[cbCardCount-i+2];
		
					return true;
				}
			}
		

			if ((cbCardCount>=2)&&(cbCardData[0]==0x4F)&&(cbCardData[1]==0x4E))
			{
	
				OutCardResult.cbCardCount=2;
				OutCardResult.cbResultCard[0]=cbCardData[0];
				OutCardResult.cbResultCard[1]=cbCardData[1];
		
				return true;
			}
		
			return false;
		}
	}
}
