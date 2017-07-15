using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.CX
{

    [ExecuteInEditMode]
    [AddComponentMenu("Custom/Controls/CardControl")]

    public enum ccAlignType
    {
        LEFT = 0,
        RIGHT = 1,
        CENTER = 2
    }
    public class UICardControl : MonoBehaviour
    {
//        public string CardPrefabs = "CX_Prefabs/Card";
        public GameObject CardPrefabs = null;
        public GameObject target = null;
        public bool Positively = true;
        public bool DisplayItem = true;
        public int CardColSpace = 30;
        public int ShootSpace = 30;
        public int BaseDepth = 300;
        public float Duration = 0.1f;
        public int RowMaxCards = 10;
        public int CardRowSpace = 50;

		//头牌与尾牌的间距
		public int distancePos = 20;

        public bool AllowMoveSelect = false;
        public bool AllowDropCard = false;
        public bool AutoCalColSpace = false;

        public Vector2 CardSize = Vector2.zero;
        public string ClickEvent = "OnCardClick";
        public ccAlignType Align = ccAlignType.CENTER;
        public string MoveSelectEvent = "OnMoveSelect";
        public string MoveUpEvent = "OnMoveUp";
        public string MoveDownEvent = "OnMoveDown";

        private GameObject cardPrefabsObj = null;

        //----------------------------------------------------------------
        byte[] _cardata = new byte[54];
        public byte _cardcount = 0;
        List<GameObject> _cardlist = new List<GameObject>();

        //----------------------------------------------------------------
        Vector3 _oldPostion = Vector3.zero;
        Vector3 _first = Vector3.zero;
        Vector3 _second = Vector3.zero;


        void Start()
        {

        }


        void Awake()
        {
            _oldPostion = transform.localPosition;
            AllowMoveSelect = true;
            AllowDropCard = true;
        }

        public void SetCardDataHover(byte[] cards, byte count)
        {
            if (transform.childCount < 4)
            {
                return;
            }
            for (byte i = 0; i < count;++i )
            {
                UISlicedSprite ssp = transform.GetChild(3 + i).Find("Background").GetComponent<UISlicedSprite>();
                if (ssp != null)
                {
                    ssp.spriteName = GetCardTex(cards[i]);  //显示牌型
                }
            }
        }

        public void SetCardData(byte[] cards, byte count)
        {
            //清空
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

            int nColSpace = CardColSpace;
            if (AutoCalColSpace)
            {
                if (count < 2)
                {
                    nColSpace = (int)(CardSize.x / 2);
                }
                else
                {
                    nColSpace = (int)((800 - (int)CardSize.x) / (count - 1));
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

            //创建牌
            for (int i = 0; i < _cardcount; i++)
            {
                GameObject obj = GetCardPrefabs();
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                obj.transform.localScale = new Vector3(1, 1, zValue);
                int nRow = (int)(i / RowMaxCards);
                int nCol = (int)(i % RowMaxCards);
                obj.transform.localPosition = new Vector3(nColSpace * nCol, CardRowSpace * nRow * (-1), 0);
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
                sp.depth = BaseDepth + i;

                if (DisplayItem)
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = GetCardTex(_cardata[i]);  //显示牌型
                    }

                }
                else
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = "card_back";
                    }

                }

                //事件
                //UIButtonMessage msg = obj.GetComponent<UIButtonMessage>();
                //msg.functionName = ClickEvent;
                //msg.target = target;

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
                transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
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

        public void OpenCardData(byte[] cards, byte count, int cardCount, bool displayItem)
		{
			//清空
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
			
			int nColSpace = CardColSpace;
			if (AutoCalColSpace)
			{
				if (count < 2)
				{
					nColSpace = (int)(CardSize.x / 2);
				}
				else
				{
					nColSpace = (int)((800 - (int)CardSize.x) / (count - 1));
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
			
			
			//创建牌
			for (int i = 0; i < _cardcount; i++)
			{
				GameObject obj = GetCardPrefabs();
				obj.transform.parent = transform;
				float zValue = ((float)i) / 100 + 1;
				obj.transform.localScale = new Vector3(1, 1, zValue);
				int nRow = (int)(i / RowMaxCards);
				int nCol = (int)(i % RowMaxCards);
				obj.transform.localPosition = new Vector3(nColSpace * nCol, CardRowSpace * nRow * (-1), 0);
				
				//头牌和尾牌隔开点
				if (i >= 2)
				{
					obj.transform.localPosition += new Vector3(distancePos, 0, 0);
				}
				
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
                if (sp == null)
                {
                    continue;
                }
                sp.depth = BaseDepth + i;			
				
				if (displayItem)
				{
					if (_cardata[i] == 253)
					{
						sp.spriteName = "card_look";
					}
					else if (_cardata[i] == 254)
					{
						sp.spriteName = "card_giveup";
					}
					else if (_cardata[i] == 252)
					{
						sp.spriteName = "card_lose";
					}
                    else if (i >= cardCount)
                    {
                        sp.spriteName = GetCardTex(_cardata[i]);
                    }
                    else
                    {
                        sp.spriteName = "card_back";
                    }
					
				}
				else
				{
					if (_cardata[i] == 253)
					{
						sp.spriteName = "card_look";
					}
					else if (_cardata[i] == 254)
					{
						sp.spriteName = "card_giveup";
					}
					else if (_cardata[i] == 252)
					{
						sp.spriteName = "card_lose";
					}
					else
					{
						sp.spriteName = "card_back";
					}
					
				}
				
				_cardlist.Add(obj);
			}

            if (count == GameLogic.MAX_COUNT && displayItem == true)
			{
                if (cardCount == 0)
                {
                    SetHeadTrialType(cards, count);
                }
                else
                {
                    SetTrialType(cards, count);
                }
				
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
				transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);
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
            ResetAllShoot();
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
        public void ClearCards()
        {
            Array.Clear(_cardata, 0, _cardata.Length);
            _cardcount = 0;

            foreach (GameObject card in _cardlist)
            {
                Destroy(card);
            }
            _cardlist.Clear();
        }

        public void AppendCompCard(byte bViewId, byte[] cards, byte count)
        {
            if (cards == null || count == 0)
            {
                return;
            }
            //牌数据
            Buffer.BlockCopy(cards, 0, _cardata, _cardcount, count);
            _cardcount += count;

            //计算间距
            int nColSpace = CardColSpace;



            //牌对象
            for (int i = _cardcount - count; i < _cardcount; i++)
            {
                GameObject obj = GetCardPrefabs();
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                obj.transform.localScale = new Vector3(1, 1, zValue);

                int nRow = (int)(i / RowMaxCards);
                int nCol = (int)(i % RowMaxCards);

                Vector3 OldPos = Vector3.zero;

                switch (bViewId)
                {
                    case 0: { OldPos = new Vector3(200, 0, 0); break; }
                    case 1: { OldPos = new Vector3(-200, 0, 0); break; }

                }


                Vector3 NewPos = new Vector3(nColSpace * nCol, CardRowSpace * nRow * (-1), 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, 0.4f, NewPos);


                //TweenRotation.Begin(obj,0.5f,Quaternion.Euler(new Vector3(0,0,180)));
                obj.name = "card_" + i.ToString();


                //Card
                UICard card = obj.GetComponent<UICard>();
                card.shoot = new Vector3(0, ShootSpace, 0); ;
                card.recvclick = Positively;
                card.duration = Duration;
                card.CardData = _cardata[i];
                card.SetPos(NewPos);
                card.SetShoot(false);
                card.SetMask(false);
                card.recvclick = false;

                //Sprite
                UISlicedSprite sp = obj.GetComponentInChildren<UISlicedSprite>();
                sp.depth = BaseDepth + i;
                /*if (Align == ccAlignType.CENTER)
                {
                    sp.pivot = UIWidget.Pivot.Center;
                }
                else if (Align == ccAlignType.LEFT)
                {
                    sp.pivot = UIWidget.Pivot.Left;
                }
                else if (Align == ccAlignType.RIGHT)
                {
                    sp.pivot = UIWidget.Pivot.Right;
                }*/
                if (DisplayItem)
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = GetCardTex(_cardata[i]);
                    }

                }
                else
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = "card_back";
                    }

                }
                //事件
                //UIButtonMessage msg = obj.GetComponent<UIButtonMessage>();
                //msg.functionName = ClickEvent;
                //msg.target = target;
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
                transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);


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

		//发牌
        public void AppendHandCard(byte bViewId, byte[] cards, byte count)
        {
            if (cards == null || count == 0)
            {
                return;
            }
            //牌数据
            Buffer.BlockCopy(cards, 0, _cardata, _cardcount, count); //
            _cardcount += count;

            //计算间距
            int nColSpace = CardColSpace;

			if (gameObject.activeSelf == false)
			{
				gameObject.SetActive(true);
			}

            //牌对象
            for (int i = _cardcount - count; i < _cardcount; i++)
            {
                GameObject obj = GetCardPrefabs();
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                obj.transform.localScale = new Vector3(1, 1, zValue);

                int nRow = (int)(i / RowMaxCards);
                int nCol = (int)(i % RowMaxCards);

                Vector3 OldPos = Vector3.zero;

                switch (bViewId)
                {
                    case 0: { OldPos = new Vector3(120, 250, 0); break; }
                    case 1: { OldPos = new Vector3(-230, 100, 0); break; }
                    case 2: { OldPos = new Vector3(-230, -100, 0); break; }
                    case 3: { OldPos = new Vector3(300, -100, 0); break; }
                    case 4: { OldPos = new Vector3(300, 100, 0); break; }
                }


                Vector3 NewPos = new Vector3(nColSpace * nCol, CardRowSpace * nRow * (-1), 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, 0.4f, NewPos);


                //TweenRotation.Begin(obj,0.5f,Quaternion.Euler(new Vector3(0,0,180)));
                obj.name = "card_" + i.ToString();


                //Card
                UICard card = obj.GetComponent<UICard>();
                if (card == null)
                {
                    card = obj.AddComponent<UICard>();
                }
                card.shoot = new Vector3(0, ShootSpace, 0); ;
                card.recvclick = Positively;
                card.duration = Duration;
                card.CardData = _cardata[i];
                card.SetPos(NewPos);
                card.SetShoot(false);
                card.SetMask(false);
                card.recvclick = false;

                //Sprite
                UISlicedSprite sp = obj.GetComponentInChildren<UISlicedSprite>();
                sp.depth = BaseDepth + i;
                /*if (Align == ccAlignType.CENTER)
                {
                    sp.pivot = UIWidget.Pivot.Center;
                }
                else if (Align == ccAlignType.LEFT)
                {
                    sp.pivot = UIWidget.Pivot.Left;
                }
                else if (Align == ccAlignType.RIGHT)
                {
                    sp.pivot = UIWidget.Pivot.Right;
                }*/
                if (DisplayItem)
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = GetCardTex(_cardata[i]);
						//Debug.LogWarning("_cardata["+i+"] = " + _cardata[i]);
                    }

                }
                else
                {
                    if (_cardata[i] == 253)
                    {
                        sp.spriteName = "card_look";
                    }
                    else if (_cardata[i] == 254)
                    {
                        sp.spriteName = "card_giveup";
                    }
                    else if (_cardata[i] == 252)
                    {
                        sp.spriteName = "card_lose";
                    }
                    else
                    {
                        sp.spriteName = "card_back";
                    }

                }
                //事件
                //UIButtonMessage msg = obj.GetComponent<UIButtonMessage>();
                //msg.functionName = ClickEvent;
                //msg.target = target;
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
                transform.localPosition = _oldPostion + new Vector3(nX, 0, 0);


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

        public void RemoveCard(byte[] cards, byte count)
        {

        }

        public void ArrayHandCards(byte[] cards, byte count)
        {
            //计算间距
            int nColSpace = CardColSpace;

            foreach (GameObject obj in _cardlist)
            {
                UICard car = obj.GetComponent<UICard>();

                int nIdx = GetCardIndex(cards, count, car.CardData);
                if (nIdx != -1)
                {
                    float zValue = ((float)nIdx) / 100 + 1;
                    obj.transform.localScale = new Vector3(1, 1, zValue);

                    int nRow = (int)(nIdx / RowMaxCards);
                    int nCol = (int)(nIdx % RowMaxCards);

                    Vector3 NewPos = new Vector3(nColSpace * nCol, CardRowSpace * nRow * (-1), 0);
                    TweenPosition.Begin(obj, 0.6f, NewPos);
                    car.SetPos(NewPos);
                    car.recvclick = true;
                    obj.name = "card_" + nIdx.ToString();

                    UISlicedSprite sp = obj.GetComponentInChildren<UISlicedSprite>();
                    sp.depth = BaseDepth + nIdx;
                }
            }
        }

        int GetCardIndex(byte[] cards, byte count, byte card)
        {
            for (int i = 0; i < count; i++)
            {
                if (card == cards[i])
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 设置头牌尾牌类型
        /// </summary>
        /// <param name="cbcarddata"></param>
        /// <param name="cbcount"></param>
        void SetHeadTrialType(byte[] cbcarddata, byte cbcount)
        {
            if (cbcount != 4)
                return;

            UISprite headsp = transform.Find("head").GetComponent<UISprite>();
            UISprite trialsp = transform.Find("trial").GetComponent<UISprite>();
            UISprite peaceful = transform.Find("peaceful").GetComponent<UISprite>();

            if (GameLogic.GetPeaceful(cbcarddata) > 0)
            {
                
                byte cbcardtype = GameLogic.GetPeaceful(cbcarddata);
                string cardname = "a_" + cbcardtype;

                peaceful.spriteName = cardname;
                peaceful.gameObject.SetActive(true);
                headsp.gameObject.SetActive(false);
                trialsp.gameObject.SetActive(false);
                return;
            }

            byte[] headdata = new byte[2];
            byte[] trialdata = new byte[2];
            Buffer.BlockCopy(cbcarddata, 0, headdata, 0, 2);
            Buffer.BlockCopy(cbcarddata, 2, trialdata, 0, 2);
            headsp.spriteName = GameLogic.GetHeadTailTypeStr(headdata, 2);
            trialsp.spriteName = GameLogic.GetHeadTailTypeStr(trialdata,2);
            peaceful.gameObject.SetActive(false);
            headsp.gameObject.SetActive(true);
            trialsp.gameObject.SetActive(true);
        }
        void SetTrialType(byte[] cbcarddata, byte cbcount)
        {
            if (cbcount != 4)
                return;

            UISprite headsp = transform.Find("head").GetComponent<UISprite>();
            UISprite trialsp = transform.Find("trial").GetComponent<UISprite>();
            UISprite peaceful = transform.Find("peaceful").GetComponent<UISprite>();

            byte[] headdata = new byte[2];
            byte[] trialdata = new byte[2];
            Buffer.BlockCopy(cbcarddata, 0, headdata, 0, 2);
            Buffer.BlockCopy(cbcarddata, 2, trialdata, 0, 2);
            trialsp.spriteName = GameLogic.GetHeadTailTypeStr(trialdata, 2);
            headsp.gameObject.SetActive(false);
            peaceful.gameObject.SetActive(false);
            trialsp.gameObject.SetActive(true);
        }

        string GetCardTex(byte bCard)
        {
            /*
            0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//F A - K
            0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//M A - K
            0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//H A - K
            0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,	//B A - K
            0x4E,0x4F
            */

            byte MASK_COLOR = 0xF0;
            byte MASK_VALUE = 0x0F;

            byte bColor = (byte)((bCard & MASK_COLOR) >> 4);
            byte bValue = (byte)(bCard & MASK_VALUE);

            if ((bValue > 0 && bValue < 16) && (bColor >= 0 && bColor <= 4))
            {
                return ("card_" + bColor.ToString() + "_" + bValue.ToString());
            }
            else if (bValue == 254)
            {
                return "card_giveup";
            }
            else if (bValue == 252)
            {
                return "card_lose";
            }
            else if (bValue == 253)
            {
                return "card_look";
            }
            else
            {
                return "card_back";
            }
        }

        /// <summary>
        /// 获取牌预设
        /// </summary>
        /// <returns></returns>
        private GameObject GetCardPrefabs()
        {
            if(cardPrefabsObj == null)
            {
				cardPrefabsObj = (GameObject)Instantiate(CardPrefabs);
                cardPrefabsObj.SetActive(false);
            }
            GameObject obj = GameObject.Instantiate(cardPrefabsObj);
            //UIEventListener.Get(obj).onHover = OnHover;
            obj.SetActive(true);
            return obj;
        }

        void OnGUI()
        {

        }
    }

}