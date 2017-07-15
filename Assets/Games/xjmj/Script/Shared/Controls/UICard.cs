//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;


namespace com.QH.QPGame.XZMJ
{
	
	/// <summary>
	/// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
	/// </summary>
	
	[AddComponentMenu("Custom/Controls/Card")]
	public class UICard : MonoBehaviour
	{
		public Transform tweenTarget;
		public GameObject parentCard;
		public GameObject uigameObject;
		public Vector3 shoot = new Vector3(0, 20f, 0);
		public float duration = 0.2f;
		public bool recvclick = true;
		public string MoveDownEvent = "OnMoveDown";
		
		Vector3 mPos = Vector3.zero;
		bool mInitDone = false;
		bool mSelected = false;
		bool selectedCrad=false;
		bool pressDown=false;
		
		Vector3 _first = Vector3.zero;
		Vector3 _second = Vector3.zero;

		UICardControl CardCtl;
		UIGame uigame;
		static bool _optseled = false;
		public bool OptSeled
		{
			get
			{
				return _optseled;
			}
			set
			{
				_optseled = value;
			}
		}
		
		public bool Selected
		{
			get
			{
				return mSelected;
			}
		}
		byte mCardData = 0;
		public byte CardData
		{
			get
			{
				return mCardData;
			}
			set
			{
				mCardData = value;
			}
		}
		void Awake()
		{
			parentCard=GameObject.Find("scene_game/dlg_cards/ctr_selfhand_cards");
			uigameObject=GameObject.Find ("scene_game");
		}
		void Start()
		{
			mSelected = false;
			CardCtl=parentCard.transform.GetComponent<UICardControl>();
			uigame=uigameObject.transform.GetComponent<UIGame>();

		}
		
		public void SetPos(Vector3 v)
		{
			mPos = v;
		}
		
		void Init()
		{
			mInitDone = true;
			mSelected = false;
			if (tweenTarget == null)
				tweenTarget = transform;
			mPos = tweenTarget.localPosition;
		}
		
		public void OnClick()
		{
			if(uigame.Phone==true)
			{
				if (recvclick == false) return;
				//检测物体是否移出
				if(this.transform.localPosition.y>10f)
				{
					selectedCrad=false;
				}else
				{
					selectedCrad=true;
				}
				
				//判断是否出牌
				if(CardCtl.cards!=null&&CardCtl.cards==this&&transform.localPosition.y>10f)
				{
					
					UICardControl.CardData =this.CardData;
					CardCtl.target.SendMessage(MoveDownEvent);
					CardCtl.cards=null;
				}
				
				//未选物体回到初始位置
				if(CardCtl.cards!=null&&CardCtl.cards!=this)
				{
					CardCtl.cards.transform.localPosition=CardCtl.cards.transform.localPosition-shoot;
				}
				
				//保存当前物体
				CardCtl.cards=this;
				
				//设置物体位置
				if (!mInitDone) Init();
				if (selectedCrad)
				{
					transform.localPosition = mPos + shoot;
				}
				else
				{
					transform.localPosition = mPos;
					CardCtl.cards=null;
				}
			}
			else{
				if (recvclick == false) return;
				
				//判断是否出牌
				if(CardCtl.cards!=null&&CardCtl.cards==this&&transform.localPosition.y>10f)
				{
					
					UICardControl.CardData =this.CardData;
					CardCtl.target.SendMessage(MoveDownEvent);
					CardCtl.cards=null;
				}

			}


		}

		void OnPress(bool isPressed)
		{
			if(uigame.Phone==false)
			{
				if(isPressed)
				{
					//Debug.Log("<color=red>"+this.transform.localPosition+"</color>");
					if(this.transform.localPosition.y>1f)
					{
						pressDown=true;
						//保存当前物体
						CardCtl.cards = this;
					}else
					{
						pressDown=false;
					}

				}
				else
				{
					if(pressDown && CardCtl.cards !=null)
					{
						CardCtl.cards.transform.localPosition=CardCtl.cards.transform.localPosition-shoot;
					}
				}
			}

		}

		void OnHover(bool isOver)
		{
			if(uigame.Phone==false)
			{
				if(isOver)
				{
					
					if (recvclick == false) return;
					//检测物体是否移出
					if(this.transform.localPosition.y>10f)
					{
						selectedCrad=false;
						return;
					}else
					{
						selectedCrad=true;
					}
					
					//保存当前物体
					CardCtl.cards=this;
					
					//设置物体位置
					if (!mInitDone) Init();
					
					if (selectedCrad)
					{
						
						transform.localPosition = mPos + shoot;
						
					}
					
				}else{
					//鼠标移出时将物体返回到原来的位置
					if(CardCtl.cards=this)
					{
						if(this.transform.localPosition.y>10f && CardCtl.cards !=null)
						{
							CardCtl.cards.transform.localPosition=CardCtl.cards.transform.localPosition-shoot;
						}
						CardCtl.cards=null;
					}
				}
			}
		}

		public void SetShoot(bool bselected)
		{
			if (recvclick == false) return;
			if (!mInitDone) Init();
			mSelected = bselected;
			
			if (mSelected)
			{
				return;
			}
			else
			{
				transform.localPosition = mPos;
			}
			
		}
		
		public bool GetShoot()
		{
			return mSelected;

		}
		public void SetMask(bool bmask)
		{
			if (recvclick == false) return;
			if (bmask)
			{
				//TweenColor.Begin(gameObject, 0, new Color(0.8f,0.8f,0.8f));
				gameObject.GetComponentInChildren<UISprite>().color = new Color(0.4f, 0.8f, 0.2f);//new Color(0.6f,0.9f,0.5f);
			}
			else
			{
				//TweenColor.Begin(gameObject, 0, Color.white);
				gameObject.GetComponentInChildren<UISprite>().color = Color.white;
			}
		}
	}
}