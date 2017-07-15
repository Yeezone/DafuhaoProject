using System;
using Shared;
using UnityEngine;

namespace com.QH.QPGame.BRNN
{
	enum emCardType
	{
		CT_ERROR				=		0,								//错误类型
		CT_POINT				=		1,								//点数类型
		CT_SPECIAL_NIU1			=		3,								//点数类型
		CT_SPECIAL_NIU2			=		4,								//点数类型
		CT_SPECIAL_NIU3			=		5,								//点数类型
		CT_SPECIAL_NIU4			=		6,								//点数类型
		CT_SPECIAL_NIU5			=		7,								//点数类型
		CT_SPECIAL_NIU6			=		8,								//点数类型
		CT_SPECIAL_NIU7			=		9,								//点数类型
		CT_SPECIAL_NIU8			=		10,								//点数类型
		CT_SPECIAL_NIU9			=	    11,								//点数类型	
		CT_SPECIAL_NIUNIU		=		12,								//特殊类型
		CT_SPECIAL_NIUNIUXW		=		13,								//特殊类型
		CT_SPECIAL_NIUNIUDW		=		14,								//特殊类型
		CT_SPECIAL_BOMEBOME		=		15,								//特殊类型
		CT_SPECIAL_FIVECOLOR	=		16,								//特殊类型
		CT_SPECIAL_FIVESMALL	=	    17,								//特殊类型
		
	}

	public class GameLogic
	{

		//游戏信息
		public const ushort KIND_ID         = 1013;
		public const ushort GAME_PLAYER     = 100;
		public const string GAME_NAME       = "百人牛牛";

		//当前游戏状态
		public const ushort GAME_SCENE_FREE = (ushort)GameState.GS_FREE;				//空闲状态
		public const ushort GAME_SCENE_PLACE_JETTON = (ushort)GameState.GS_PLAYING;		//游戏状态
		public const ushort GAME_SCENE_GAME_END	= (ushort)GameState.GS_PLAYING+1;		//结束状态

		//赔率定义
		public const byte RATE_TWO_PAIR = 12;											//对子赔率
		public const byte SERVER_LEN = 32;												//房间长度

		//玩家索引
		public const byte AREA_COUNT = 5;												//下注区域数目

		//下注区域
		public const byte ID_TIAN_MEN = 1;												//天
		public const byte ID_DI_MEN = 2;												//地
		public const byte ID_XUAN_MEN = 3;												//玄
		public const byte ID_HUANG_MEN = 4;												//黄

		public const ushort MAX_APPLY_BANKER  = 6;										//申请上庄人数上限
		public const byte   NULL_CHAIR      = 255;										//空用户
		public const int MAX_SCORE_HISTORY = 60;  										//最大游戏记录

		public const ushort MASK_COLOR      = 0xF0; 									//花色掩码
		public const ushort MASK_VALUE      = 0x0F; 									//数值掩码	
		public const ushort MAX_COUNT       = 5;    									//最大数目

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

		//获取最大牌值
		public static byte GetMaxCardValue(byte cbCardData)
		{
			byte bCardColor = GetCardColor(cbCardData);
			byte bCardValue = GetCardValue(cbCardData);
			if(bCardColor>>4 == 4)
			{
				if(bCardValue == 1)
				{
					return 14;
				}
				else
				{
					return 15;
				}
			}
			else
			{
				return bCardValue;
			}
		}

		//获取逻辑值
		public static byte GetCardLogicValue(byte cbCardData)
		{
			//扑克属性
			byte bCardColor = GetCardColor(cbCardData);
			byte bCardValue = GetCardValue(cbCardData);

			if(bCardColor>>4 == 4)
			{
				//Debug.LogError(bCardValue);
				if(bCardValue == 1)
				{
					return 14;
				}
				else
				{
					return 15;
				}

			}
			if(bCardValue > 10)
			{
				return 10;
			}

			return bCardValue;

		}

		//排序
		public static void GetOxCard(ref byte[] cbCardData, byte cbCardCount)
		{
			//Debug.LogError(cbCardData+"进入排序");
			//设置变量
			byte[] bTemp = new byte[MAX_COUNT];
			byte[] bTempData = new byte[MAX_COUNT];
			Buffer.BlockCopy(cbCardData, 0, bTempData, 0, MAX_COUNT);
			byte bSum = 0;

			//大小王判断
			int king = 0;
			int king_0 = 255;
			int king_1 = 255;

			//两张牌和的点数(有王没有牛牛)
			byte maxValue = 0;
			byte value_0 = 0;
			byte value_1 = 0;

			//是否有王有牛牛时
			bool haveKing = true; 

			for (byte i=0; i<cbCardCount; i++)
			{
				bTemp[i] = GetCardLogicValue(cbCardData[i]);

				if(bTemp[i]==14 || bTemp[i]==15)
				{
					king++;
					if(bTemp[i] == 14)
					{
						king_0 = i;
					}
					else
					{
						king_1 = i;
					}
				}
				else
				{
					bSum += bTemp[i];
				}
			}

			//Debug.LogError(king+"+king+"+king_0+"+king_0+"+king_1+"+king_1+");
			//没有大小王时
			if(king == 0 && king_0 >5 && king_1 > 5) 
			{
				for (int i=0; i<cbCardCount-1; i++)
				{
					for (int j=i+1; j<cbCardCount; j++)
					{
						if((bSum-bTemp[i]-bTemp[j])%10 == 0)
						{
							byte bCount = 0;

							for (int k=0; k<cbCardCount; k++)
							{
								if(k!=i && k!=j)
								{
									cbCardData[bCount] = bTempData[k];
									bCount++;
								}
							}

							cbCardData[bCount] = bTempData[i];
							bCount++;
							cbCardData[bCount] = bTempData[j];
							bCount++;
							return;
						}
					}
				}
			}
			
			//当有一个小王时
			if(king == 1 && (king_0 < 5 || king_1 < 5))
			{
//				Debug.LogError(king_0+"king_0"+"+"+king_1+"king_1");
//				for(int i=0; i<5; i++)
//				{
//					Debug.LogError(bTempData[i]);
//				}
				
				//除去王有牛时
				if(haveKing)
				{
					for(int k=0; k<cbCardCount; k++)
					{
						if(k!=king_0 && k!=king_1)
						{
							if((bSum-bTemp[k])%10 == 0)
							{
								//Debug.LogError("除去王有牛时");
								byte bCount = 0;
								
								for (int f=0; f<cbCardCount; f++)
								{
									if(f!=king_0 && f!=king_1 && f!=k)
									{
										cbCardData[bCount] = bTempData[f];
										bCount++;
									}
								}
								
								cbCardData[bCount] = bTempData[k];
								bCount++;
								
								if(king_0 > 5)
								{
									cbCardData[bCount] = bTempData[king_1];
									
								}
								else
								{
									cbCardData[bCount] = bTempData[king_0];
								}
								
								return;
							}
						}
					}
					haveKing = false;
				}

				//去除王没有牛时(后两张牛牛)
				for(int i=0; i<cbCardCount-1; i++)
				{
					for (int k=i+1; k<cbCardCount; k++)
					{
						if(i!=king_0 && i!=king_1 && k!=king_0 && k!=king_1)
						{
							//当有王时，有两张和值等于10倍数的牌
							if((bTemp[k]+bTemp[i])%10 == 0)
							{
								//Debug.LogError("后两张牛牛");
								byte bCount = 1;
								for (int f=0; f<cbCardCount; f++)
								{
									if(f!=k && f!=i && f!=king_0 && f!=king_1)
									{
										cbCardData[bCount] = bTempData[f];
										bCount++;
										if(bCount == 4)
										{
											Debug.LogError("错误起源"+king_0+"+"+king_1+"+"+k+"+"+i+"+"+f);
										}
									}
								}
								
								if(king_0 > 5)
								{
									cbCardData[0] = bTempData[king_1];
								}
								else
								{
									cbCardData[0] = bTempData[king_0];
								}

								cbCardData[bCount] = bTempData[k];
								bCount++;
								if(i > 4 || bCount > 4)
								{
									Debug.LogError(bCount+"bcount"+i+"k");
								}
								cbCardData[bCount] = bTempData[i];
								bCount++;
								return;
							}
						}
					}
				}
				
				//有王时，牌型不是牛牛
				Debug.LogWarning("没有牛牛");
				for(int i=0; i<cbCardCount-1; i++)
				{
					for(int q=i+1; q<cbCardCount; q++)
					{
						if(i!=king_0 && i!=king_1 && q!=king_0 && q!=king_1)
						{
							byte cardValue = (byte)((bTemp[q]+bTemp[i]) % 10);
				
							if(maxValue < cardValue)
							{
								maxValue = cardValue;
								value_0 = (byte)q;
								value_1 = (byte)i;
							}
						}
					}
				}

				byte Count = 1;

				for (int f=0; f<cbCardCount; f++)
				{
					if(f!=king_0 && f!=king_1 && f!=value_0 && f!=value_1)
					{
						cbCardData[Count] = bTempData[f];
						Count++;
						if(Count >= 4)
						{
							Debug.LogError("错误起源"+king_0+"+"+king_1+"+"+value_0+"+"+value_0+"+"+f);
						}
					}
				}
				
				if(king_0 == 255)
				{
					cbCardData[0] = bTempData[king_1];
				}
				else
				{
					cbCardData[0] = bTempData[king_0];
				}
				
				Debug.LogWarning("value_0"+value_0+"   "+"value_1"+value_1);
				cbCardData[Count] = bTempData[value_0];
				Count++;

				if(Count > 4)
				{
					Debug.LogError(Count+"bcount"+value_1+"k");
				}

				cbCardData[Count] = bTempData[value_1];
				Count++;
				return;
			}

			//当有两个王时
			if(king == 2 && king_0 < 5 && king_1 < 5)
			{
//				UIGame uigame = new UIGame();
//				uigame.cnMsgBox("两王");
//				Debug.LogError("两王");
//				for(int i=0; i<5; i++)
//				{
//					Debug.LogError(bTempData[i]);
//				}
				byte bCount = 0;

				for(int k=0; k<cbCardCount; k++)
				{
					if(k!=king_0 && k!=king_1)
					{
						cbCardData[bCount] = bTempData[k];
						bCount++;
					}
				}

				if(bSum%10 == 0)
				{
					cbCardData[bCount] = bTempData[king_1];
					bCount++;
					cbCardData[bCount] = bTempData[king_0];
					return;
				}
				else
				{
					byte[] cardData = new byte[3];
					Buffer.BlockCopy(cbCardData, 0, cardData, 0, 3);
					cbCardData[0] = bTempData[king_1];
					cbCardData[1] = cardData[0];
					cbCardData[2] = cardData[1];
					cbCardData[3] = cardData[2];
					cbCardData[4] = bTempData[king_0];
					return;
				}
			}
		}
	}
}

