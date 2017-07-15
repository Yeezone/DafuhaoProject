using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.DDZ.Help
{
    public partial class PokerHelper
    {
        #region 智能选牌2.0

        public static void GetSingleCardCollection(byte[] cbHandCards, ref byte[] cbLineCardData, int lessThanCardNum = 5)
        {
            byte ST_ORDER = 0;
            byte cbLineCardCount = (byte)cbLineCardData.Length, cbHandCardCount = (byte)cbHandCards.Length;

            byte[] cbTmpCard = new byte[20];
            Buffer.BlockCopy(cbHandCards, 0, cbTmpCard, 0, cbHandCardCount);

            //short
            GameLogic.SortCardList(ref cbTmpCard, cbHandCardCount, ST_ORDER);

            cbLineCardCount = 0;

            //
            if (cbHandCardCount < lessThanCardNum) return;

            byte cbFirstCard = 0;
            //
            for (byte i = 0; i < cbHandCardCount; ++i)
            {
                if (GameLogic.GetCardLogicValue(cbTmpCard[i]) < 15)
                {
                    cbFirstCard = i;
                    break;
                }
            }

            byte[] cbSingleLineCard = new byte[12];
            byte cbSingleLineCount = 0;
            byte cbLeftCardCount = cbHandCardCount;
            bool bFindSingleLine = true;

            //连牌判断
            while (cbLeftCardCount >= lessThanCardNum && bFindSingleLine)
            {
                cbSingleLineCount = 1;
                bFindSingleLine = false;
                byte cbLastCard = cbTmpCard[cbFirstCard];
                cbSingleLineCard[cbSingleLineCount - 1] = cbTmpCard[cbFirstCard];
                for (byte i = (byte)(cbFirstCard + 1); i < cbLeftCardCount; i++)
                {
                    byte cbCardData = cbTmpCard[i];

                    //
                    if (1 != (GameLogic.GetCardLogicValue(cbLastCard) - GameLogic.GetCardLogicValue(cbCardData)) && GameLogic.GetCardValue(cbLastCard) != GameLogic.GetCardValue(cbCardData))
                    {
                        cbLastCard = cbTmpCard[i];
                        if (cbSingleLineCount < lessThanCardNum)
                        {
                            cbSingleLineCount = 1;
                            cbSingleLineCard[cbSingleLineCount - 1] = cbTmpCard[i];
                            continue;
                        }
                        else break;
                    }
                    //
                    else if (GameLogic.GetCardValue(cbLastCard) != GameLogic.GetCardValue(cbCardData))
                    {
                        cbLastCard = cbCardData;
                        cbSingleLineCard[cbSingleLineCount] = cbCardData;
                        ++cbSingleLineCount;
                    }
                }

                //保存数据
                if (cbSingleLineCount >= lessThanCardNum)
                {
                    GameLogic.RemoveCard(cbSingleLineCard, cbSingleLineCount, ref cbTmpCard, ref cbLeftCardCount);
                    Buffer.BlockCopy(cbSingleLineCard, 0, cbLineCardData, cbLineCardCount, cbSingleLineCount);
                    cbLineCardCount += cbSingleLineCount;
                    cbLeftCardCount -= cbSingleLineCount;
                    bFindSingleLine = true;
                }
            }
        }
        /// <summary>
        /// 分析选中的牌，并计算出相同逻辑值牌的个数
        /// </summary>
        /// <param name="cbHandCards"></param>
        /// <param name="cbSingleCardData"></param>
        public static void GetSingleCollection(byte[] cbHandCards, ref Dictionary<byte, OutTagCard> cbSingleCardData)
        {
            byte ST_ORDER = 0;
            byte cbHandCardCount = (byte)cbHandCards.Length;

            //short
            GameLogic.SortCardList(ref cbHandCards, cbHandCardCount, ST_ORDER);

            for (int i = 0; i < cbHandCardCount; i++)
            {
                var tmpCard = cbHandCards[i];
                var tmpCardLogicValue = GameLogic.GetCardLogicValue(tmpCard);
                byte tmpCardCount = GetCountBySpecifyCard(cbHandCards, cbHandCardCount, tmpCard);
                if (!cbSingleCardData.ContainsKey(tmpCardLogicValue))
                {
                    OutTagCard tagCard = new OutTagCard()
                    {
                        CardCount = tmpCardCount,
                        Cards = new byte[tmpCardCount],
                        LogicValue = tmpCardLogicValue
                    };
                    tagCard.Cards[0] = tmpCard;

                    cbSingleCardData.Add(tmpCardLogicValue, tagCard);
                }
                else
                {
                    byte[] resultCard = cbSingleCardData[tmpCardLogicValue].Cards;
                    for (int j = 0; j < resultCard.Length; j++)
                    {
                        if (resultCard[j] == 0)
                        {
                            resultCard[j] = tmpCard;
                            break;
                        }
                    }

                    cbSingleCardData[tmpCardLogicValue].Cards = resultCard;
                }
            }
        }

        /// <summary>
        /// 获取指定牌个数
        /// </summary>
        /// <param name="iCardList"></param>
        /// <param name="iCardCount"></param>
        /// <param name="bCard"></param>
        /// <returns></returns>
        public static byte GetCountBySpecifyCard(byte[] iCardList, int iCardCount, byte bCard)
        {
            byte count = 0;
            for (byte i = 0; i < iCardCount; i++)
            {
                if (GameLogic.GetCardLogicValue(iCardList[i]) == GameLogic.GetCardLogicValue(bCard))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 智能提牌
        /// </summary>
        /// <param name="srcCard">选中的牌型</param>
        /// <param name="outResultCard">输出的牌型</param>
		public static void SmartPickupCard(byte[] srcCard, ref byte[] outResultCard)
		{
			//少于等于2张牌 原样输出
			if (srcCard.Length <= 2)
			{
				outResultCard = srcCard;
				return;
			}
			
			Dictionary<byte, OutTagCard> analyseCard = new Dictionary<byte, OutTagCard>();
			GetSingleCollection(srcCard, ref analyseCard);
			
			if (analyseCard == null || analyseCard.Count == 0) return;
			
			//获取所有单牌
			List<OutTagCard> singleCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount == 1) singleCard.Add(card.Value);
			}
			
			//获取所有对子
			List<OutTagCard> pairCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount == 2) pairCard.Add(card.Value);
			}
			
			//获取所有三张
			List<OutTagCard> threeCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount == 3) threeCard.Add(card.Value);
			}
			
			
			//获取炸弹(不包括王炸)
			List<OutTagCard> bombCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount == 4) bombCard.Add(card.Value);
			}
			
			#region 飞机
			List<ArrayList> planeCard = new List<ArrayList>();
			
			bool hasPlane = false;
			
			List<OutTagCard> greatThanThreeCards = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount >= 3 && card.Key <= 14)
				{
					greatThanThreeCards.Add(card.Value);
				}
			}
			
			if (greatThanThreeCards.Count > 0)
			{
				OutTagCard otcFirstCard = greatThanThreeCards[greatThanThreeCards.Count - 1];
				byte bThreeFirstCard = otcFirstCard.LogicValue;
				
				ArrayList plane = new ArrayList();
				plane.AddRange(new byte[3]
				               {
					otcFirstCard.Cards[otcFirstCard.CardCount - 1],
					otcFirstCard.Cards[otcFirstCard.CardCount - 2],
					otcFirstCard.Cards[otcFirstCard.CardCount - 3]
				}
				);
				
				for (int i = greatThanThreeCards.Count - 2; i >= 0; i--)
				{
					//判断下一个元素是否存在首元素的LogicValue + 1的关系
					bool isExists = greatThanThreeCards[i].LogicValue == bThreeFirstCard + (greatThanThreeCards.Count - i - 1);
					if (greatThanThreeCards[i].LogicValue >= 14) continue;
					if (isExists)
					{
						plane.AddRange(new byte[3]
						               {
							greatThanThreeCards[i].Cards[greatThanThreeCards[i].CardCount - 1],
							greatThanThreeCards[i].Cards[greatThanThreeCards[i].CardCount - 2],
							greatThanThreeCards[i].Cards[greatThanThreeCards[i].CardCount - 3]
						});
						
						hasPlane = true;
					}
					else
					{
						if (plane.Count >= 6)
						{
							planeCard.Add(plane);
						}
						//重新初始化牌，进入下一轮查询
						plane = new ArrayList();
						bThreeFirstCard = greatThanThreeCards[i].LogicValue;
					}
				}
				if (hasPlane && plane.Count >= 6)
				{
					planeCard.Add(plane);
				}
			}
			//如果包含飞机，则检查飞机的个数
			if (hasPlane)
			{
				//飞机个数
				int planeCount = planeCard.Count;
				
				ArrayList resultCard = new ArrayList();
				//飞机长度
				int planeLength = planeCard[0].Count;
				resultCard.AddRange(planeCard[0].ToArray(typeof(byte)));
				
				//飞机个数大于1，则找到最长的飞机
				if (planeCount > 1)
				{
					foreach (ArrayList plane in planeCard)
					{
						if (plane.Count > planeLength)
						{
							resultCard = new ArrayList();
							planeLength = plane.Count;
							resultCard.AddRange(plane.ToArray(typeof(byte)));
						}
					}
				}
				
				//搭配翅膀
				int pairCount = pairCard.Count;
				//计算飞机长度
				planeLength = resultCard.Count / 3;
				
				if (pairCount >= planeLength)
				{
					//查找飞机长度相等个数的最小对子
					var myPairs = pairCard.GetRange(pairCount - planeLength, planeLength);
					foreach (var kvPair in myPairs)
					{
						resultCard.AddRange(kvPair.Cards);
					}
				}
				else
				{
					//查找单张翅膀
					int singleCount = singleCard.Count;
					if (singleCount >= planeLength)
					{
						//查找飞机长度相等个数的最小单牌
						var mySingle = singleCard.GetRange(singleCount - planeLength, planeLength);
						foreach (var kvPair in mySingle)
						{
							resultCard.AddRange(kvPair.Cards);
						}
					}
					else
					{
						//在单牌和对子不能单独满足翅膀情况下，先取出所有单牌，不够的牌由最小的对子凑齐翅膀
						List<OutTagCard> mySingle = new List<OutTagCard>();
						foreach (var card in singleCard)
						{
							if (card.LogicValue <= 14) mySingle.Add(card);
						}
						
						foreach (var kvPair in mySingle)
						{
							resultCard.AddRange(kvPair.Cards);
						}
						
						//计算需要拆的对子长度
						int mySinglePairLength = planeLength - mySingle.Count;
						if (mySinglePairLength < 0) mySinglePairLength = 0;
						
						List<byte> bytes = new List<byte>();
						//按照大小顺序获取所有对子
						foreach (var kvPair in pairCard)
						{
							bytes.AddRange(kvPair.Cards);
						}
						if (bytes.Count > 0)
						{
							bytes.Reverse(); //逆序，以便后面从最小的对子开始拆
							resultCard.AddRange(
								bytes.GetRange(0, mySinglePairLength).ToArray());
						}
					}
				}
				outResultCard = (byte[])resultCard.ToArray(typeof(byte));
				return;
			}
			
			#endregion
			
			List<byte[]> outTempResultCard = new List<byte[]>();
			#region 单顺
			//获取顺子(大于等于5张)
			List<ArrayList> singleCards = new List<ArrayList>();
			
			List<OutTagCard> greatThanOneCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount >= 1 && card.Key <= 14) greatThanOneCard.Add(card.Value);
			}
			var gtThanOneCard = greatThanOneCard;
			gtThanOneCard.Reverse();
			//最长的单顺长度
			int maxSingleStraightLength = 0;
			if (gtThanOneCard.Count > 0)
			{
				OutTagCard otcFirstCard = gtThanOneCard[0];
				byte bOneFirstCard = otcFirstCard.LogicValue;
				
				ArrayList single = new ArrayList();
				
				//连续变动值
				int iJump = 0;
				for (int i = 1; i < gtThanOneCard.Count; i++)
				{
					if (i == 1) iJump = i;
					//判断下一个元素是否存在首元素的LogicValue + 1的关系
					bool isExists = (gtThanOneCard[i].LogicValue == bOneFirstCard + iJump);
					//判断是否牌是否比2大
					if (gtThanOneCard[i].LogicValue > 14) continue;
					if (isExists)
					{
						if (single.Count == 0)
						{
							single.Add(otcFirstCard.Cards[0]);
						}
						single.Add(gtThanOneCard[i].Cards[0]);
						iJump++;
					}
					else
					{
						
						if (single.Count >= 5)
						{
							singleCards.Add(single);
						}
						else
						{
							singleCards.Remove(single);
							iJump = 1;
						}
						//重新初始化牌，进入下一轮查询
						single = new ArrayList();
						
						//gtThanOneCard[i].LogicValue - bOneFirstCard - 1;
						bOneFirstCard = gtThanOneCard[i].LogicValue;
						otcFirstCard = gtThanOneCard[i];
					}
					if (i == gtThanOneCard.Count - 1 && single.Count >= 5)
					{
						singleCards.Add(single);
					}
				}
				
			}
			//判断是否有存在顺子
			if (singleCards.Count >= 1)
			{
				ArrayList resultCard = new ArrayList();
				
				resultCard.AddRange((byte[])singleCards[0].ToArray(typeof(byte)));
				if (singleCards.Count > 1)
				{
					int singletraightLength = singleCards[0].Count;
					//查找最长的顺子
					foreach (var kvItem in singleCards)
					{
						if (kvItem.Count > singletraightLength)
						{
							resultCard = new ArrayList();
							
							singletraightLength = kvItem.Count;
							resultCard.AddRange((byte[])kvItem.ToArray(typeof(byte)));
						}
					}
					maxSingleStraightLength = singletraightLength;
				}
				outTempResultCard.Add((byte[])resultCard.ToArray(typeof(byte)));
			}
			
			#endregion
			
			#region 连对
			List<ArrayList> pairCards = new List<ArrayList>();
			
			List<OutTagCard> greatThanTwoCard = new List<OutTagCard>();
			foreach (var card in analyseCard)
			{
				if (card.Value.CardCount >= 2 && card.Value.LogicValue <= 14) greatThanTwoCard.Add(card.Value);
			}
			
			var gtThanTwoCard = greatThanTwoCard;
			gtThanTwoCard.Reverse();
			if (gtThanTwoCard.Count > 0)
			{
				OutTagCard otcFirstCard = gtThanTwoCard[0];
				byte bTwoFirstCard = otcFirstCard.LogicValue;
				
				ArrayList pairs = new ArrayList();
				//连续变动值
				int iJump = 0;
				for (int i = 1; i < gtThanTwoCard.Count; i++)
				{
					if (i == 1) iJump = i;
					//判断下一个元素是否存在首元素的LogicValue + 1的关系
					bool isExists = (gtThanTwoCard[i].LogicValue == bTwoFirstCard + iJump);
					//判断是否牌是否比2大
					if (gtThanTwoCard[i].LogicValue > 14) continue;
					if (isExists)
					{
						if (pairs.Count == 0)
						{
							pairs.AddRange(new byte[2]
							               {
								otcFirstCard.Cards[0],
								otcFirstCard.Cards[1]
							}
							);
						}
						pairs.AddRange(new byte[2]
						               {
							gtThanTwoCard[i].Cards[0],
							gtThanTwoCard[i].Cards[1]
						});
						iJump++;
					}
					else
					{
						if (pairs.Count >= 6)
						{
							pairCards.Add(pairs);
						}
						else
						{
							pairCards.Remove(pairs);
							iJump = 1;
						}
						//重新初始化牌，进入下一轮查询
						pairs = new ArrayList();
						bTwoFirstCard = gtThanTwoCard[i].LogicValue;
						otcFirstCard = gtThanTwoCard[i];
					}
					if (i == gtThanTwoCard.Count-1 && pairs.Count >= 6)
					{
						pairCards.Add(pairs);
					}
				}
				
			}
			
			int maxPairSingleCount = 0;//构成连对的最大单张牌个数
			if (pairCards.Count >= 1)
			{
				ArrayList resultCard = new ArrayList();
				resultCard.AddRange((byte[])pairCards[0].ToArray(typeof(byte)));
				
				int pairStraightLength = pairCards[0].Count;
				
				if (pairCards.Count > 1)
				{
					//查找最长的连对
					foreach (var kvItem in pairCards)
					{
						if (kvItem.Count > pairStraightLength)
						{
							resultCard = new ArrayList();
							
							pairStraightLength = kvItem.Count;
							resultCard.AddRange((byte[])kvItem.ToArray(typeof(byte)));
						}
					}
				}
				
				maxPairSingleCount = pairStraightLength / 2;
				if (maxPairSingleCount > maxSingleStraightLength) //判断单顺长度和连对的单牌个数
				{
					outTempResultCard.Add((byte[])resultCard.ToArray(typeof(byte)));
				}
			}
			
			
			#endregion
			
			
			//取得除飞机外最长的牌型
			if (outTempResultCard.Count > 0)
			{
				int maxReturnCardLength = outTempResultCard[0].Length;
				outResultCard = outTempResultCard[0];
				foreach (byte[] tempResultCard in outTempResultCard)
				{
					if (tempResultCard.Length > maxReturnCardLength)
					{
						outResultCard = tempResultCard;
					}
				}
				return;
			}
			
			#region 其他牌处理
			//三张牌
			ArrayList threeTakeOne = new ArrayList();
			if (threeCard.Count == 1)
			{
				threeTakeOne.AddRange(threeCard[0].Cards);
				if (pairCard.Count > 0)
				{
					threeTakeOne.AddRange(pairCard[pairCard.Count - 1].Cards);
					outResultCard = (byte[])threeTakeOne.ToArray(typeof(byte));
					return;
				}
				if (singleCard.Count > 0)
				{
					threeTakeOne.AddRange(singleCard[singleCard.Count - 1].Cards);
					outResultCard = (byte[])threeTakeOne.ToArray(typeof(byte));
					return;
				}
			}
			
			outResultCard = srcCard;
			return;
			#endregion
			
		}

        #endregion

        #region byte[] 数组输出
        public static string ArrayToString(byte[] oBytes)
        {
            StringBuilder sBytes = new StringBuilder();
            if (oBytes == null)
            {
                return string.Empty;
            }
            foreach (var oByte in oBytes)
            {
                sBytes.AppendFormat("{0} ", oByte.ToString());
            }
            return sBytes.ToString();
        }

        public static string PokerArrayToString(byte[] oBytes)
        {
            StringBuilder sBytes = new StringBuilder();
            if (oBytes == null)
            {
                return string.Empty;
            }
            foreach (var oByte in oBytes)
            {
                if (oByte != 0)
                {
                    sBytes.AppendFormat("{0} ", GameLogic.GetCardValue(oByte).ToString());
                }
            }
            return sBytes.ToString();
        }
        #endregion
    }

    public class OutTagCard
    {
        /// <summary>
        /// 牌的个数
        /// </summary>
        public byte CardCount { get; set; }
        /// <summary>
        /// 逻辑值
        /// </summary>
        public byte LogicValue { get; set; }
        /// <summary>
        /// 所有逻辑值相等的牌
        /// </summary>
        public byte[] Cards { get; set; }
    }
}