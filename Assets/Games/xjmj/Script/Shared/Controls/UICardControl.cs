using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.XZMJ
{
	
	
	//[ExecuteInEditMode]//实时调用该脚本
	[AddComponentMenu("Custom/Controls/CardControl")]
	
	public enum ccAlignType
	{
		LEFT = 0,
		RIGHT = 1,
		CENTER = 2
	}
	
	public enum Cardkind
	{
		SelfHand = 0,
		SelfLie = 1,
		SlefCatch = 2,
		SlefDesk = 3,
		WestHand = 4,
		WestLie = 5,
		WestCatch = 6,
		WestDesk = 7,
		EastHand = 8,
		EastLie = 9,
		EastCatch = 10,
		EastDesk = 11,
		NorthHand = 12,
		NorthLie = 13,
		NorthCatch = 14,
		NorthDesk = 15,
		operateEat = 16,
		SelfLieGameEnd = 17,
		WestLieGameEnd = 18,
		EastLieGameEnd = 19,
		NorthLieGameEnd = 20,
		
		SelfCatchGameEnd = 21,
		WestCatchGameEnd = 22,
		EastCatchGameEnd = 23,
		NorthCatchGameEnd = 24,
		
		SelfHandHu = 25,
		WestHandHu = 26,
		EastHandHu = 27,
		NorthHandHu = 28,
		
	}
	
	public class UICardControl : MonoBehaviour
	{
		public GameObject target = null;
		public bool Positively = true;
		public bool DisplayItem = true;
		public int CardColSpace;
		public int ShootSpace;
		public int BaseDepth;
		public float Duration;
		public int RowMaxCards;
		public int CardRowSpace;
		
		public bool AllowMoveSelect = false;
		public bool AllowDropCard = false;
		public bool AutoCalColSpace = false;
		
		public Vector2 CardSize = Vector2.zero;
		public string ClickEvent = "OnCardClick";
		public ccAlignType Align = ccAlignType.CENTER;
		public string MoveSelectEvent = "OnMoveSelect";
		public string MoveUpEvent = "OnMoveUp";
		public string MoveDownEvent = "OnMoveDown";
		
		public static byte CardData;
	
		public GameObject Card;
		public GameObject NorthCard;
		public GameObject SelfCard;
		public GameObject SelfdeskCard;
		public GameObject SelflieCard;
		public GameObject WestdeskCard;
		public GameObject WesthandCard;
		public GameObject NorthdeskCard;
		public Transform LieCard;//玩家手牌父级


		public UICard cards;
		public bool Phone;//运行平台

		UIGame uiGame=new UIGame();
		//----------------------------------------------------------------
		byte[] _cardata = new byte[54];
		byte _cardcount = 0;
		List<GameObject> _cardlist = new List<GameObject>();
		
		//----------------------------------------------------------------
		Vector3 _oldPostion = Vector3.zero;
		Vector3 _first = Vector3.zero;
		Vector3 _second = Vector3.zero;
		
		//----------------------------------------------------------------
		int _rows = 0;
		int handCount;//Lie牌数

		void Awake()
		{
			cards=null;
			_oldPostion = transform.localPosition;
			AllowMoveSelect = true;
			AllowDropCard = true;
		}
		//计算玩家的手牌
		void HanCount(string objectName)
		{
			handCount=13;
			LieCard=GameObject.Find(objectName).GetComponent<Transform>();
			foreach(Transform child in LieCard)
			{
                handCount--;
			}
			if(handCount<0)
			{
                
				handCount=0;
			}
		}
		//牌的创建
		public void SetCardData(byte[] cards, byte count, Cardkind cardkind, byte[] cbCardHideen)
		{
			GameObject obj = null;
			if (count == 0)
			{
				Array.Clear(_cardata, 0, _cardata.Length);
				_cardcount = 0;
				
				foreach (GameObject card in _cardlist)
				{
					Destroy(card);
				}
				_cardlist.Clear();
				
				return;
			}
			
			//牌数据
			Buffer.BlockCopy(cards, 0, _cardata, 0, count);
			_cardcount = count;
			//int operate_count =0;
			int nColSpace = CardColSpace;
			if (AutoCalColSpace)
			{
				if (count < 2)
				{
					nColSpace = (int)(CardSize.x / 2);
				}
				else
				{
					nColSpace = (int)((800 - (int)CardSize.x) / (count - 1));//??
					if (nColSpace > (int)(CardSize.x / 2))
					{
						nColSpace = (int)(CardSize.x / 2);
					}
				}
			}
			//初始化
			foreach (GameObject card in _cardlist)
			{
				Destroy(card);
			}
			_cardlist.Clear();
			
			
			if (gameObject.activeSelf == false)
			{
				gameObject.SetActive(true);
			}
			
			int save_x = 0;
			switch (cardkind)
			{
			case Cardkind.SelfHand:
			case Cardkind.WestHand:
			case Cardkind.EastHand:
			case Cardkind.NorthHand:
			{
				save_x = 13 - _cardcount;
				break;
			}

			case Cardkind.SelfLieGameEnd:
			{
                save_x = 14 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_selfhand_cards");
				break;
			}
			case Cardkind.WestLieGameEnd:
			{
                save_x = 14 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_westhand_cards");
				break;
			}
			case Cardkind.EastLieGameEnd:
			{
                save_x = 14 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_easthand_cards");
				break;
			}
			
			case Cardkind.NorthLieGameEnd:
			{
                save_x = 14 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_northhand_cards");
				break;
			}


			
			case Cardkind.WestHandHu:
			{
                save_x = 13 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_westhand_cards");
				break;
			}
			case Cardkind.EastHandHu:
			{
                save_x = 13 - _cardcount;
				HanCount("scene_game/dlg_cards/ctr_easthand_cards");
				break;
			}
			case Cardkind.SelfHandHu:
			case Cardkind.NorthHandHu:
			{
				save_x = 13 - _cardcount;
				break;
			}
			
			case Cardkind.EastCatchGameEnd:
			case Cardkind.EastCatch:
			{
                save_x = 0;
				HanCount("scene_game/dlg_cards/ctr_easthand_cards");
				break;
			}
			case Cardkind.NorthCatchGameEnd:
			case Cardkind.NorthCatch:
			{
                save_x = 0;
				HanCount("scene_game/dlg_cards/ctr_northhand_cards");
				break;
			}
			case Cardkind.SelfCatchGameEnd:
			case Cardkind.SlefCatch:
			{
                save_x = 0;
				HanCount ("scene_game/dlg_cards/ctr_selfhand_cards");
				break;
			}
			case Cardkind.WestCatchGameEnd:
			case Cardkind.WestCatch:
			{
                save_x = 0;
				HanCount("scene_game/dlg_cards/ctr_westhand_cards");
				break;
			}

			}
            
			if(save_x<0)
			{
				if(save_x == -1)
				{
					save_x = 0;
					return;
				}
				Debug.LogError(save_x);
				save_x=3;
			}
			
			//创建牌
			int cardcount = 0, ncol = 0, liecount = 2,ncount=0;
			for (int i = 0; i < _cardcount; i++)
			{
				//
				//选择预设
				//

				switch (cardkind)
				{
				case Cardkind.SelfHand:
				case Cardkind.SlefCatch:
				{
					obj= Instantiate(SelfCard);
					break;
				}
				case Cardkind.SelfLie:
				case Cardkind.operateEat:
				case Cardkind.SelfLieGameEnd:
				case Cardkind.SelfCatchGameEnd:
				case Cardkind.SelfHandHu:
				{
					obj=Instantiate(SelflieCard);
					
					break;
				}
				case Cardkind.SlefDesk:
				{
					obj=Instantiate(SelfdeskCard);
					break;
				}
				case Cardkind.WestHand:
				case Cardkind.WestCatch:
				{
					obj= Instantiate(WesthandCard);
					break;
				}
				case Cardkind.WestLie:
				case Cardkind.WestDesk:
				case Cardkind.WestLieGameEnd:
				case Cardkind.WestCatchGameEnd:
				case Cardkind.WestHandHu:
				{
					obj= Instantiate(WestdeskCard);
					break;
				}
				case Cardkind.EastHand:
				case Cardkind.EastCatch:
				{
					obj=Instantiate(WesthandCard);
					break;
				}
				case Cardkind.EastLie:
				case Cardkind.EastDesk:
				case Cardkind.EastLieGameEnd:
				case Cardkind.EastCatchGameEnd:
				case Cardkind.EastHandHu:
				{
					obj =Instantiate(WestdeskCard);
					break;
				}
				case Cardkind.NorthHand:
				case Cardkind.NorthCatch:
				case Cardkind.NorthLie:
				case Cardkind.NorthLieGameEnd:
				case Cardkind.NorthCatchGameEnd:
				case Cardkind.NorthHandHu:
				{
					obj =Instantiate(NorthCard);
					break;
				}
				case Cardkind.NorthDesk:
				{
					obj=Instantiate(NorthdeskCard);
					break;
				}
				}
				
				
				
				
				//
				//计算位置
				//
				obj.transform.parent = transform;
				float zValue = ((float)i) / 100+1 ;
				obj.transform.localScale = new Vector3(1, 1, zValue);
				
				int nRow = (int)(i / RowMaxCards);//判断是否是桌面的牌
				int nCol = (int)(i % RowMaxCards);
				if(Phone)
				{
					//手机端位置

					switch (cardkind)
					{
					case Cardkind.SelfHand:
					{
						int a = 0;
						if (_cardcount % 2 == 1)
						{
							a = nColSpace;
						}
						else
						{
							a = 0;
						}
						obj.transform.localPosition = new Vector3(nColSpace * (nCol + save_x)+(save_x/3f)*10f, 0, 0);
						break;
					}
					case Cardkind.SelfLie:
					case Cardkind.SelfHandHu:
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(nColSpace * (nCol - liecount + save_x)+(ncount-1)*10f, CardRowSpace-10f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							if(cardkind==Cardkind.SelfLie)
							{
								obj.transform.localPosition = new Vector3(nColSpace * (nCol - ncol + save_x)+ncount*10f, 0, 0);
							}else{
								obj.transform.localPosition = new Vector3(nColSpace * (nCol  -ncol+ save_x)+(save_x/3f)*10f, 0, 0);
							}
							
						}
						if(cardcount==2)
						{
							ncount++;
							
						}
						break;
					}
						
					case Cardkind.SelfLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(nColSpace * (nCol+save_x)+(save_x/3f)*10f, 0, 0);
						break;
					}
						
						
					case Cardkind.SlefCatch:
					case Cardkind.SelfCatchGameEnd:
					{
						obj.transform.localPosition = new Vector3(nColSpace * nCol+(handCount/3f)*10f, 0, 0);

						break;
					}
					case Cardkind.SlefDesk:
					{
						obj.transform.localPosition = new Vector3(nColSpace * nCol, CardRowSpace * nRow, 0);
						break;
					}
						
					case Cardkind.WestHand:
					{
						//obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (-1), 0);
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (-1), 0);
						break;
					}
					case Cardkind.WestLie:
						
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-7.5f*(nCol-liecount+ncount/2f)+1.5f, 16f * (nCol - liecount+ncount/2f) * (-1f) + 17f, 0);
							ncol++;
							liecount++;
						}
						else
						{
							
							obj.transform.localPosition = new Vector3(-(nCol-ncol+ncount/2f)*7.5f, 16f * (nCol-ncol+ncount/2f) * (-1f), 0);
							
						}
						if(cardcount==2)
						{
							ncount++;
						}
						break;
					}
						
					case Cardkind.WestHandHu:
					case Cardkind.WestLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/2f)*7.5f, 16f * (nCol +save_x+save_x/3f/2f) * (-1), 0);;
						break;
					}	
					case Cardkind.WestCatch:
					case Cardkind.WestCatchGameEnd:
					{

						if(cardkind==Cardkind.WestCatchGameEnd)
						{
//							obj.transform.localPosition = new Vector3(-(15f+handCount/3f/2f)*7.5f, 16f * (15f+handCount/3f/2f)*(-1), 0);
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f)*(-1), 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f)*(-1), 0);
						}
						break;
					}
						
					case Cardkind.WestDesk:
					{
						obj.transform.localPosition = new Vector3(nColSpace * nRow-nCol*7.5f, 16f * nCol * (-1), 0);
						break;
					}
						
					case Cardkind.EastHand:
					{
						//obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f+15f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (1), 0);
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f+15f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (1), 0);
						BaseDepth = 50 - nCol;
						break;
					}
					case Cardkind.EastLie:
						
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-7.5f*(nCol-liecount+ncount/2f)+4.5f, 16f * (nCol - liecount + ncount/2f) * (1) + 5f, 0);
							//obj.transform.localPosition = new Vector3(-6f*(1+ncol*3+(save_x-1)/3f), 14f * (nCol - liecount + save_x) * (1) + 13f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							obj.transform.localPosition = new Vector3(-(nCol-ncol+ncount/2f)*7.5f, 16f * (nCol-ncol+ncount/2f) * (1), 0);
							BaseDepth = 100 - nCol;
						}
						if(cardcount==2)
						{
							ncount++;
						}
						
						break;
					}
					case Cardkind.EastHandHu:
					case Cardkind.EastLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/2f)*7.5f, 16f * (nCol + save_x+save_x/3f/2f) * (1), 0);
						BaseDepth = 80 - nCol;
						break;
						
					}
						
					case Cardkind.EastCatch:
					case Cardkind.EastCatchGameEnd:
					{
						if(cardkind==Cardkind.EastCatchGameEnd)
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f-10.5f, 16f * (nCol+handCount/3f/2f), 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f), 0);
						}

						
						BaseDepth = 10;
						break;
					}
						
					case Cardkind.EastDesk:
					{
						obj.transform.localPosition = new Vector3(-nColSpace * nRow-nCol*7.5f, 16f * nCol * (1), 0);
						BaseDepth = 50 - nCol;
						break;
					}
						
					case Cardkind.NorthHand:
					{
						
						obj.transform.localPosition = new Vector3(-nColSpace * (nCol + save_x)-(save_x/3f)*5f, 0, 0);
						break;
					}
					case Cardkind.NorthLie:
					case Cardkind.NorthHandHu:
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-nColSpace * (nCol - liecount + save_x)-(ncount-1)*5f, CardRowSpace-14f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							if(cardkind==Cardkind.NorthLie)
							{
								obj.transform.localPosition = new Vector3(-nColSpace * (nCol-ncol)-ncount*5f, -8f, 0);
							}else{
								obj.transform.localPosition = new Vector3(-nColSpace * (nCol-ncol+save_x)-(save_x/3f)*5f, -8f, 0);
							}
							
						}
						if(cardcount==2)
						{
							ncount++;
						}
						break;
					}
					case Cardkind.NorthLieGameEnd:
					{
						
						obj.transform.localPosition = new Vector3(-nColSpace * (nCol + save_x)-(save_x/3f)*5f, -8, 0);	
						
						break;
					}
						
					case Cardkind.NorthCatch:
					case Cardkind.NorthCatchGameEnd:
					{
						if(cardkind==Cardkind.NorthCatchGameEnd)
						{
							obj.transform.localPosition = new Vector3(-nColSpace * nCol-(handCount/3f)*5f, -8f, 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-nColSpace * nCol-(handCount/3f)*5f, 0, 0);
						}
						break;
					}
					case Cardkind.NorthDesk:
					{
						obj.transform.localPosition = new Vector3(-nColSpace * nCol, -CardRowSpace * nRow, 0);
						break;
					}
					case Cardkind.operateEat:
					{
						obj.transform.localPosition = new Vector3(nColSpace * (nCol - 1), 0, 0);
						break;
					}
					}
				}
				else
				{
					//PC端位置
					switch (cardkind)
					{
					case Cardkind.SelfHand:
					{
						int a = 0;
						if (_cardcount % 2 == 1)
						{
							a = nColSpace;
						}
						else
						{
							a = 0;
						}
						obj.transform.localPosition = new Vector3(nColSpace * (nCol + save_x)+(save_x/3f)*10f, 0, 0);
						break;
					}
					case Cardkind.SelfLie:
					case Cardkind.SelfHandHu:
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(nColSpace * (nCol - liecount + save_x)+(ncount-1)*10f, CardRowSpace-10f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							if(cardkind==Cardkind.SelfLie)
							{
								obj.transform.localPosition = new Vector3(nColSpace * (nCol - ncol + save_x)+ncount*10f, 0, 0);
							}else{
								
								obj.transform.localPosition = new Vector3(nColSpace * (nCol  -ncol+ save_x)+(save_x/3f)*10f, 0, 0);
							}
							
						}
						if(cardcount==2)
						{
							ncount++;
							
						}
						break;
					}
						
					case Cardkind.SelfLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(nColSpace * (nCol+save_x)+(save_x/3f)*10f, 0, 0);
						break;
					}
						
						
					case Cardkind.SlefCatch:
					case Cardkind.SelfCatchGameEnd:
					{
						//Debug.LogError("进"+nColSpace+"+++"+nCol+"++"+handCount);
						obj.transform.localPosition = new Vector3(nColSpace * nCol+(handCount/3f)*10f, 0, 0);
						break;
					}
					case Cardkind.SlefDesk:
					{
						obj.transform.localPosition = new Vector3(nColSpace * nCol, CardRowSpace * nRow, 0);
						break;
					}
						
					case Cardkind.WestHand:
					{
						//obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (-1), 0);
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (-1), 0);
						break;
					}
					case Cardkind.WestLie:
						
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-7.5f*(nCol-liecount+ncount/2f)+1.5f, 16f * (nCol - liecount+ncount/2f) * (-1f) + 17f, 0);
							ncol++;
							liecount++;
						}
						else
						{
							
							obj.transform.localPosition = new Vector3(-(nCol-ncol+ncount/2f)*7.5f, 16f * (nCol-ncol+ncount/2f) * (-1f), 0);
							
						}
						if(cardcount==2)
						{
							ncount++;
						}
						break;
					}
						
					case Cardkind.WestHandHu:
					case Cardkind.WestLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/2f)*7.5f, 16f * (nCol +save_x+save_x/3f/2f) * (-1), 0);
						break;
					}
						
					case Cardkind.WestCatch:
					case Cardkind.WestCatchGameEnd:
					{
						if(cardkind==Cardkind.WestCatchGameEnd)
						{
//							obj.transform.localPosition = new Vector3(-(14+handCount/3f/2f)*7.5f, 16f * (14+handCount/3f/2f)*(-1), 0);
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f)*(-1), 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f)*(-1), 0);
						}
						break;
					}
						
					case Cardkind.WestDesk:
					{
						obj.transform.localPosition = new Vector3(nColSpace * nRow-nCol*7.5f, 16f * nCol * (-1), 0);
						break;
					}
						
					case Cardkind.EastHand:
					{
						//obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f+15f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (1), 0);
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/3f)*8.5f+15f, CardRowSpace * (nCol + save_x+save_x/3f/3f) * (1), 0);
						BaseDepth = 50 - nCol;
						break;
					}
					case Cardkind.EastLie:
						
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-7.5f*(nCol-liecount+ncount/2f)+4.5f, 16f * (nCol - liecount + ncount/2f) * (1) + 5f, 0);
							//obj.transform.localPosition = new Vector3(-6f*(1+ncol*3+(save_x-1)/3f), 14f * (nCol - liecount + save_x) * (1) + 13f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							obj.transform.localPosition = new Vector3(-(nCol-ncol+ncount/2f)*7.5f, 16f * (nCol-ncol+ncount/2f) * (1), 0);
							BaseDepth = 100 - nCol;
						}
						if(cardcount==2)
						{
							ncount++;
						}
						
						break;
					}
					case Cardkind.EastHandHu:
					case Cardkind.EastLieGameEnd:
					{
						obj.transform.localPosition = new Vector3(-(nCol+save_x+save_x/3f/2f)*7.5f, 16f * (nCol + save_x+save_x/3f/2f) * (1), 0);
						BaseDepth = 80 - nCol;
						break;
						
					}
						
					case Cardkind.EastCatch:
					case Cardkind.EastCatchGameEnd:
					{
						if(cardkind==Cardkind.EastCatchGameEnd)
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f-10.5f, 16f * (nCol+handCount/3f/2f), 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-(nCol+handCount/3f/2f)*7.5f, 16f * (nCol+handCount/3f/2f), 0);
						}
						
						BaseDepth = 10;
						break;
					}
						
					case Cardkind.EastDesk:
					{
						obj.transform.localPosition = new Vector3(-nColSpace * nRow-nCol*7.5f, 16f * nCol * (1), 0);
						BaseDepth = 50 - nCol;
						break;
					}
						
					case Cardkind.NorthHand:
					{
						
						obj.transform.localPosition = new Vector3(-nColSpace * (nCol + save_x)-(save_x/3f)*5f, 0, 0);
						break;
					}
					case Cardkind.NorthLie:
					case Cardkind.NorthHandHu:
					{
						if (i - 1 >= 0)
						{
							if (_cardata[i] == _cardata[i - 1])
							{
								cardcount++;
							}
							else
							{
								cardcount = 0;
							}
						}
						
						if (cardcount == 3)
						{
							obj.transform.localPosition = new Vector3(-nColSpace * (nCol - liecount + save_x)-(ncount-1)*5f, CardRowSpace-13f, 0);
							ncol++;
							liecount++;
							BaseDepth = 110;
						}
						else
						{
							if(cardkind==Cardkind.NorthLie)
							{
								obj.transform.localPosition = new Vector3(-nColSpace * (nCol-ncol)-ncount*5f, -7f, 0);
							}else{
								obj.transform.localPosition = new Vector3(-nColSpace * (nCol-ncol+save_x)-(save_x/3f)*5f, -7f, 0);
							}
							
						}
						if(cardcount==2)
						{
							ncount++;
						}
						break;
					}
					case Cardkind.NorthLieGameEnd:
					{
						
						obj.transform.localPosition = new Vector3(-nColSpace * (nCol + save_x)-(save_x/3f)*5f, -7f, 0);	
						break;
					}
						
					case Cardkind.NorthCatch:
					case Cardkind.NorthCatchGameEnd:
					{
						if(cardkind==Cardkind.NorthCatchGameEnd)
						{
							obj.transform.localPosition = new Vector3(-nColSpace * nCol-(handCount/3f)*5f, -7f, 0);
						}else
						{
							obj.transform.localPosition = new Vector3(-nColSpace * nCol-(handCount/3f)*5f, 0, 0);
						}

						break;
					}
					case Cardkind.NorthDesk:
					{
						obj.transform.localPosition = new Vector3(-nColSpace * nCol, -CardRowSpace * nRow, 0);
						break;
					}
					case Cardkind.operateEat:
					{
						obj.transform.localPosition = new Vector3(nColSpace * (nCol - 1), 0, 0);
						break;
					}
					}
				}

				
				//
				//选择贴图
				//
				obj.name = "card_" + i.ToString();
				//Card
				UICard card = obj.GetComponent<UICard>();
				card.shoot = new Vector3(0, ShootSpace, 0); ;
				card.recvclick = Positively;
				card.duration = Duration;
				card.CardData = _cardata[i];
				card.SetShoot(false);
				card.SetMask(false);
				//Sprite
				UISlicedSprite sp = obj.GetComponentInChildren<UISlicedSprite>();
				sp.depth = BaseDepth;
				sp.pivot = UIWidget.Pivot.Center;
				
				if (DisplayItem)
				{
					sp.spriteName = GetCardTex("", _cardata[i]);
				}
				else
				{
					
					sp.spriteName = "card_back";
				}
				
				//
				switch (cardkind)
				{
				case Cardkind.SelfHand:
				case Cardkind.SlefCatch:
				{
					sp.spriteName = GetCardTex("self_", _cardata[i]);
					//事件
					UIButtonMessage msg = obj.GetComponent<UIButtonMessage>();
					msg.functionName = ClickEvent;
					msg.target = target;
					break;
				}
					
				case Cardkind.SelfLie:
				case Cardkind.SelfLieGameEnd:
				case Cardkind.SelfCatchGameEnd:
				case Cardkind.SelfHandHu:
				{
					if (cbCardHideen[i] == 1)
					{
						sp.spriteName = GetCardTex("selflie_", _cardata[i]);
					}
					else
					{
						sp.spriteName = "self_0_0";
					}
					
					break;
				}
				case Cardkind.operateEat:
				{
					sp.spriteName = GetCardTex("selflie_", _cardata[i]);
					break;
				}
				case Cardkind.SlefDesk:
				{
					sp.spriteName = GetCardTex("selfdesk_", _cardata[i]);
					break;
				}
					
				case Cardkind.WestHand:
				case Cardkind.WestCatch:
				{
					sp.spriteName = "westhand_s";
					break;
				}
				case Cardkind.WestLie:
				case Cardkind.WestLieGameEnd:
				case Cardkind.WestCatchGameEnd:
				case Cardkind.WestHandHu:
				{
					if (cbCardHideen[i] == 1)
					{
						sp.spriteName = GetCardTex("westlie_", _cardata[i]);
					}
					else
					{
						sp.spriteName = "westlie_0_0";
					}
					
					break;
				}
				case Cardkind.WestDesk:
				{
					sp.spriteName = GetCardTex("westlie_", _cardata[i]);
					break;
				}
					
				case Cardkind.EastHand:
				case Cardkind.EastCatch:
				{
					sp.spriteName = "easthand_s";
					break;
				}
				case Cardkind.EastLie:
				case Cardkind.EastLieGameEnd:
				case Cardkind.EastCatchGameEnd:
				case Cardkind.EastHandHu:
				{
					if (cbCardHideen[i] == 1)
					{
						sp.spriteName = GetCardTex("eastlie_", _cardata[i]);
					}
					else
					{
						sp.spriteName = "eastlie_0_0";
					}
					
					break;
				}
				case Cardkind.EastDesk:
				{
					sp.spriteName = GetCardTex("eastlie_", _cardata[i]);
					break;
				}
					
				case Cardkind.NorthHand:
				case Cardkind.NorthCatch:
				{
					sp.spriteName = "northhand_s";
					break;
				}
				case Cardkind.NorthLie:
				case Cardkind.NorthLieGameEnd:
				case Cardkind.NorthCatchGameEnd:
				case Cardkind.NorthHandHu:
				{
					if (cbCardHideen[i] == 1)
					{
						sp.spriteName = GetCardTex("northlie_", _cardata[i]);
					}
					else
					{
						sp.spriteName = "northlie_0_0";
					}
					
					break;
				}
				case Cardkind.NorthDesk:
				{
					sp.spriteName = GetCardTex("northlie_", _cardata[i]);
					break;
				}
				}
				
				_cardlist.Add(obj);
			}
			
			if (Align == ccAlignType.CENTER)
			{
				float nXRate = transform.localScale.x;
				if (_cardcount > RowMaxCards)
				{
					int nX = (-1) * (((int)((RowMaxCards - 1) * nColSpace * nXRate) + (int)CardSize.x) / 2 - ((int)CardSize.x / 2));
					transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
				}
				else
				{
					int nX = (-1) * (((int)((_cardcount - 1) * nColSpace * nXRate) + (int)CardSize.x) / 2 - ((int)CardSize.x / 2));
					transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
				}
			}
			else if (Align == ccAlignType.LEFT)
			{
				int nX = (-1) * (int)(CardSize.x / 2);
				//transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
			}
			else if (Align == ccAlignType.RIGHT)
			{
				float nXRate = transform.localScale.x;
				if (_cardcount > RowMaxCards)
				{
					int nX = (-1) * (int)(((int)((RowMaxCards - 1) * nColSpace * nXRate) + CardSize.x) / 2 - (CardSize.x / 2));
					transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
				}
				else
				{
					int nX = (-1) * (int)(((int)((_cardcount - 1) * nColSpace * nXRate) + CardSize.x) / 2 - (CardSize.x / 2));
					transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
				}
			}
		}
		public void SetShootCard(byte[] cards, byte count)
		{
			for (byte i = 0; i < count; i++)
			{
				foreach (GameObject obj in _cardlist)
				{
					UICard card = obj.GetComponent<UICard>();
					if (cards[i] == card.CardData)
					{
						card.SetShoot(true);
					}
					
				}
			}
		}
		public void SetMaskCard(int nIndex, bool bmask)
		{
			for (byte i = 0; i < _cardcount; i++)
			{
				GameObject obj = _cardlist[i];
				UICard card = obj.GetComponent<UICard>();
				if (nIndex == i)
				{
					card.SetMask(bmask);
				}
				
			}
		}
		public void GetShootCard(ref byte[] cards, ref byte count)
		{
			foreach (GameObject obj in _cardlist)
			{
				UICard card = obj.GetComponent<UICard>();
				if (card.Selected)
				{
					cards[count++] = card.CardData;
				}
			}
		}
		
		public void ResetAllShoot()
		{
			foreach (GameObject obj in _cardlist)
			{
				UICard card = obj.GetComponent<UICard>();
				card.SetShoot(false);
			}
			
		}
		
		string GetCardTex(string name, byte bCard)
		{
			byte MASK_COLOR = 0xF0;
			byte MASK_VALUE = 0x0F;
			
			byte bColor = (byte)((bCard & MASK_COLOR) >> 4);
			byte bValue = (byte)(bCard & MASK_VALUE);
			
			if ((bValue > 0 && bValue < 16) && (bColor >= 0 && bColor <= 4))
				return (name + bColor.ToString() + "_" + bValue.ToString());
			else
				return "card_back";
			
		}

//
//		void OnGUI()
//		{
//			
//			if (Input.GetMouseButtonDown(0))
//			//if (Event.current.type == EventType.MouseDown)
//			{
//				//记录鼠标按下的位置
//				_first = Input.mousePosition;
//				_second = Input.mousePosition;
//				
//				Ray ray = UICamera.currentCamera.ScreenPointToRay(_second);
//				RaycastHit hitinfo;
//				if (Physics.Raycast(ray, out hitinfo))
//				{
//					UICard card = hitinfo.collider.gameObject.GetComponent<UICard>();
//					if (card != null)
//					{
//						//						card.SetMask(true);
//						card.SetShoot(!card.GetShoot());
//						
//						if (cards != null)
//						{
//							cards.SetShoot(false);
//						}
//						if (card.GetShoot())
//						{
//							cards = card;
//						}
//						else
//						{
//							if (MoveDownEvent != "" && target != null)
//							{
//								CardData = card.CardData;
//								target.SendMessage(MoveDownEvent);
//							}
//						}
//						card=null;
//					}
//				}
//			}
//			
//			
//			if (Event.current.type == EventType.mouseDrag)
//			{
//				//记录鼠标拖动的位置
//				_second = Input.mousePosition;
//				
//				if (_second.x != _first.x && System.Math.Abs(_second.x - _first.x) > 3 && AllowMoveSelect)
//				{
//					Ray ray = UICamera.currentCamera.ScreenPointToRay(_second);
//					RaycastHit hitinfo;
//					if (Physics.Raycast(ray, out hitinfo))
//					{
//						UICard card = hitinfo.collider.gameObject.GetComponent<UICard>();
//						
//						if (card != null)
//						{
//							//						card.SetMask(true);
//							card.SetShoot(!card.GetShoot());
//							if (cards != null)
//							{
//								cards.SetShoot(false);
//							}
//							if (card.GetShoot())
//							{
//								cards = card;
//							}
//							else
//							{
//								if (MoveDownEvent != "" && target != null)
//								{
//									CardData = card.CardData;
//									target.SendMessage(MoveDownEvent);
//								}
//							}
//						}
//					}
//				}
//				
//			}
//			
//			
//		}
		
		
	}
	
}