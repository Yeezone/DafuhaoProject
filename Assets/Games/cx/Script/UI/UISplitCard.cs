using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.CX
{

    public class UISplitCard : MonoBehaviour
    { 
        public delegate void SplitCardsCall(byte[] bcards, ushort count);
        public SplitCardsCall splitCardsCaLL;
        byte[] _bcard;
        ushort _ucount;

		//判断是否是特殊牌型
        bool _isPeaceful = false;				

        byte[] _bleftcard = new byte[4];
        byte[] _brightcard = new byte[2];

        private byte[] m_bcard = new byte[GameLogic.MAX_COUNT];

        UISprite[] leftCardObj = new UISprite[4];
        UISprite[] rightCardObj = new UISprite[2];

        private GameObject btnSpliteCard = null;

        UISprite leftCardType = null;
        UISprite rightCardType = null;

        UIGrid leftGrid = null;
        UIGrid rightGrid = null;

        void Awake()
        {
            leftCardType = transform.Find("neidi/cardtype1").GetComponent<UISprite>();
            rightCardType = transform.Find("neidi/cardtype2").GetComponent<UISprite>();
            leftGrid = transform.Find("neidi/leftcard").GetComponent<UIGrid>();
            rightGrid = transform.Find("neidi/rightcard").GetComponent<UIGrid>();

            btnSpliteCard = transform.Find("BtnSpliteCard").gameObject;
            UIEventListener.Get(btnSpliteCard).onClick = OnClickSplite;

            for (byte i = 0; i < 4;++i )
            {
                leftCardObj[i] = transform.Find("neidi/leftcard/lcard" + i).GetComponent<UISprite>();
                UIEventListener.Get(leftCardObj[i].gameObject).onClick = OnClickCard;
            }
            for (byte i = 0; i < 2;++i )
            {
                rightCardObj[i] = transform.Find("neidi/rightcard/rcard" + i).GetComponent<UISprite>();
                UIEventListener.Get(rightCardObj[i].gameObject).onClick = OnClickCard;
            }
        }

        void OnDisable()
        {
            ResetPos();
        }
        void OnEnable()
        {
            btnSpliteCard.GetComponent<UIButton>().isEnabled = false;
            SetUserClock(15);
        }

        void OnTimerEnd()
        { 
            ClickSpliteCard();
        }

		//发送分牌
        void ClickSpliteCard()
        {
            if (_isPeaceful)
            {
                splitCardsCaLL(_bcard, _ucount);
                gameObject.SetActive(false);
                return;
            }
            SetSpliteCardData();
            //CheckHeadTrialCard();
            splitCardsCaLL(_bcard, _ucount);
            gameObject.SetActive(false);
        }

		//分牌按钮事件
        void OnClickSplite(GameObject obj)
        {
            ClickSpliteCard();
        }

		//核对头牌是否比尾牌大
        void CheckHeadTrialCard()
        {
            byte[] headcard = new byte[2];
            byte[] trialcard = new byte[2];
            Buffer.BlockCopy(_bcard, 0, headcard, 0, 2);
            Buffer.BlockCopy(_bcard, 2, trialcard, 0, 2);
            bool isFir = GameLogic.CompareCardN(headcard, trialcard, 2);
            if (isFir)
            {
                Buffer.BlockCopy(trialcard, 0, _bcard, 0, 2);
                Buffer.BlockCopy(headcard, 0, _bcard, 2, 2);
            }
        }

        /// <summary>
        /// 设置分牌数据
        /// </summary>
        void SetSpliteCardData()
        {
            byte bindex = 0;
            for (byte i = 0; i < 4;++i )
            {
                if (_bleftcard[i] != 0)
                {
                    if (bindex >= GameLogic.MAX_COUNT)
                    {
                        return;
                    }
                    _bcard[bindex] = _bleftcard[i];
                    ++bindex;
                }
            }
            for (byte i = 0; i < 2;++i )
            {
                if (_brightcard[i] != 0)
                {
                    if (bindex >= GameLogic.MAX_COUNT)
                    {
                        return;
                    }
                    _bcard[bindex] = _brightcard[i];
                    ++bindex;
                }
            }
        }

		//点击卡牌事件
        void OnClickCard(GameObject obj)
        { 
            if (_isPeaceful)
            {
                return;
            }
            if (obj.name.Contains("rcard"))
            {
                byte tindex = (byte)int.Parse(obj.name.Substring(5));
                SetLeftCardData(_brightcard[tindex]);
                _brightcard[tindex] = 0;

                if (GetLeftCount() == 2)
                {
                    btnSpliteCard.GetComponent<UIButton>().isEnabled = true;
                }
            }
            else if(obj.name.Contains("lcard"))
            {
                byte tindex = (byte)int.Parse(obj.name.Substring(5));
                if (GetLeftCount() == 2)
                {
                    btnSpliteCard.GetComponent<UIButton>().isEnabled = true;
                }
                if (GetLeftCount() <= 2)
                {
                    return;
                }
                SetRightCardData(_bleftcard[tindex]);
                _bleftcard[tindex] = 0;

                if (GetLeftCount() == 2)
                {
                    btnSpliteCard.GetComponent<UIButton>().isEnabled = true;
                }
            }
            CheckCardType();

        }

        /// <summary>
        /// 检测牌型
        /// </summary>
        void CheckCardType()
        {
            leftGrid.enabled = false;
            rightGrid.enabled = false;
            leftGrid.enabled = true;
            rightGrid.enabled = true;
            if (GetLeftCount() != 2)
            {
                leftCardType.gameObject.SetActive(false);
                rightCardType.gameObject.SetActive(false);
                return;
            }
            byte []bcards;
            GetLeftCardData(out bcards);
            leftCardType.gameObject.SetActive(true);
            rightCardType.gameObject.SetActive(true);
            leftCardType.spriteName = GameLogic.GetHeadTailTypeStr(bcards, 2);
            rightCardType.spriteName = GameLogic.GetHeadTailTypeStr(_brightcard, 2);
        }

        void GetLeftCardData(out byte[] cbcarddata)
        {
            ushort count = 0;
            cbcarddata = new byte[2];
            for (byte i = 0; i < 4; ++i)
            {
                if (_bleftcard[i] != 0)
                {
                    cbcarddata[count] = _bleftcard[i];
                    ++count;
                }
                if (count == 2)
                {
                    return;
                }
            }
        }

        void SetLeftCardData(byte carddata)
        {
            for (byte i = 0; i < 4; ++i)
            {
                if (_bleftcard[i] == 0)
                {
                    _bleftcard[i] = carddata;
                    break;
                }
            }
        }

        void SetRightCardData(byte carddata)
        {
            for (byte i = 0; i < 2;++i )
            {
                if (_brightcard[i] == 0)
                {
                    _brightcard[i] = carddata;
                    break;
                }
            }
        }

        /// <summary>
        /// 左边显示数量
        /// </summary>
        /// <returns></returns>
        ushort GetLeftCount()
        {
            ushort count = 0;
            for (byte i = 0; i < 4;++i )
            {
                if (_bleftcard[i] != 0)
                {
                    ++count;
                }
            }
            return count;
        }
        

        void SetUserClock(uint time)
        {
            gameObject.GetComponent<UIClock>().SetTimer(time * 1000);
        }

        // Update is called once per frame
        void Update()
        {
            for (byte i = 0; i < 4;++i )
            {
                if (_bleftcard[i] == 0)
                {
                    leftCardObj[i].gameObject.SetActive(false);
                }
                else
                {
                    leftCardObj[i].gameObject.SetActive(true);
                    leftCardObj[i].spriteName = GetCardTex(_bleftcard[i]);
                }
            }
            for (byte i = 0; i < 2;++i )
            {
                if (_brightcard[i] == 0)
                {
                    rightCardObj[i].gameObject.SetActive(false);
                }
                else
                {
                    rightCardObj[i].gameObject.SetActive(true);
                    rightCardObj[i].spriteName = GetCardTex(_brightcard[i]);
                }
            }
        }

		//初始卡牌信息
        public void InitData(byte[] bcards, ushort count, SplitCardsCall tcall) 
        {
			if(count < 4)
			{
				return;
			}

            leftGrid.transform.GetChild(0).localPosition = new Vector3(-45, 0, 0);
            leftGrid.transform.GetChild(1).localPosition = new Vector3(-15, 0, 0);
            leftGrid.transform.GetChild(2).localPosition = new Vector3(15, 0, 0);
            leftGrid.transform.GetChild(3).localPosition = new Vector3(45, 0, 0);

            ResetPos();
            Array.Clear(_bleftcard, 0, 4);
            Array.Clear(_brightcard, 0, 2);
            splitCardsCaLL = tcall;
            _ucount = count;
            _bcard = new byte[count];
            Buffer.BlockCopy(bcards,0,_bcard,0,count);
            Buffer.BlockCopy(bcards, 0, _bleftcard, 0, count);
            _isPeaceful = false;
            if (GameLogic.GetPeaceful(bcards) > 0)
            {
                _isPeaceful = true;
                btnSpliteCard.GetComponent<UIButton>().isEnabled = true;
                byte cbcardtype = GameLogic.GetPeaceful(bcards);
                string cardname = "a_" + cbcardtype;

                leftCardType.gameObject.SetActive(true);
                leftCardType.spriteName = cardname;
				return;
            }

			byte[] cardType = new byte[2];
			for(int i=0; i<4; i++)
			{
				for(int k=i+1; k<4; k++)
				{
					cardType[0] = bcards[i];
					cardType[1] = bcards[k];
					byte headNum = GameLogic.GetCardTypeN(cardType, 2);
					if(headNum == GameLogic.D2H_D2H)
					{
						int dataIndex = 0;
						_brightcard[0] = bcards[i];
						_brightcard[1] = bcards[k];
						byte[] headCardType = new byte[2];
						for(int j=0; j<4; j++)
						{
							if(j != i && j != k)
							{
								headCardType[dataIndex] = bcards[j];
								dataIndex++;
							}
						}
						for(int j=0; j<4; j++)
						{
							if(j < 2)
							{
								_bleftcard[j] = headCardType[j];
							}
							else
							{
								_bleftcard[j] = 0;
							}
						}

						CheckCardType();
						SetSpliteCardData();
						_isPeaceful = true;

                        if (GetLeftCount() == 2)
                        {
                            btnSpliteCard.GetComponent<UIButton>().isEnabled = true;
                        }
						return;
					}
				}
			}
        }

        /// <summary>
        /// 重置位置
        /// </summary>
        public void ResetPos()
        {
            for (byte i = 0; i < 4; ++i)
            {
                if (leftCardObj[i] != null)
                {
                    leftCardObj[i].gameObject.SetActive(true);
                }
            }
            for (byte i = 0; i < 2; ++i)
            {
                if (rightCardObj[i] != null)
                {
                    rightCardObj[i].gameObject.SetActive(false);
                }
            }
            leftGrid.enabled = true;
            rightGrid.enabled = true;
            if (leftCardType != null)
            {
                leftCardType.gameObject.SetActive(false);
            }
            if (rightCardType != null)
            {
                rightCardType.gameObject.SetActive(false);
            }
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
    }
}
