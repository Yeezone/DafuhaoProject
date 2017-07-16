using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.GDNN
{
	[ExecuteInEditMode]
	[AddComponentMenu ("Custom/Controls/CardControl")]

	public class UICardControl : MonoBehaviour
	{
		public GameObject CardPrefabs;
		//牌预制
		public GameObject target = null;
		//scene_game
		public bool DisplayItem = true;
		//是否显示牌（修改牌name）
		public int CardColSpace = 30;
		//牌与牌之间的间距
		public int CardTypeSpace = 10;
		//牛值与组牛的间距
		public int BaseDepth = 300;
		//牌的层级
		
		public Vector2 CardSize = Vector2.zero;
		//牌宽高
		public Vector2 CardScale = Vector2.one;
		//牌型大小
	
		private byte[] _cardata = new byte[54];
		//牌数组
		private byte _cardcount = 0;
		//要生成牌的张数
		private List<GameObject> _cardlist = new List<GameObject> ();
		private List<GameObject> list = new List<GameObject> ();


		//存放生成牌
		private Transform[] fiveCrad = new Transform[6];
		//存放单个区域牌(长度为6是让0下标留给createPos_？物体)
		private Vector3[] fiveCardPos = new Vector3[6];
		//每个区域单张牌位置

		public static byte[] maxCardArea = new byte[5];
		//每组牌中最大的一张牌
		public static byte[] maxCardColor = new byte[5];
		//最大牌的花色

		//存放牌的父级物体
		public GameObject cardParent_0;
		public GameObject cardParent_1;
		public GameObject cardParent_2;
		public GameObject cardParent_3;
		public GameObject cardParent_4;

		//牌生成的位置
		public GameObject createPos_0;
		public GameObject createPos_1;
		public GameObject createPos_2;
		public GameObject createPos_3;
		public GameObject createPos_4;

		//牌类型显示
		private GameObject[] cardType = new GameObject[5];

		//单组牌
		private byte[] m_card_0 = new byte[5];
		private byte[] m_card_1 = new byte[5];
		private byte[] m_card_2 = new byte[5];
		private byte[] m_card_3 = new byte[5];
		private byte[] m_card_4 = new byte[5];

		//时间控制
		private float oneCardTime = 0.1f;
		//发牌时两张牌时间的间隔
		//private float cardMoveTime = 0.3f;									//牌移动到终点的时间

		void Awake ()
		{
			for (int i = 0; i < 5; i++) {
				cardType [i] = GameObject.Find ("CardType/cardType_" + i.ToString ());
			}
		}

		void Start ()
		{
			if (target == null) {
				target = GameObject.Find ("scene_game");
			}

			this.oneCardTime = target.GetComponent<UIGame> ().oneCardTime;
			//this.cardMoveTime = target.GetComponent<UIGame>().oneCardTime;
		}

		//生成牌(框架消息调用)
		public void FrameSetCardData (byte[] cards, byte count)
		{
			//清空
			if (count == 0) {
				Array.Clear (_cardata, 0, _cardata.Length);
				_cardcount = 0;
				
				foreach (GameObject card in _cardlist) {
					Destroy (card);
				}
				_cardlist.Clear ();
				
				return;
			}
			
			//牌数据
			Buffer.BlockCopy (cards, 0, _cardata, 0, count);
			_cardcount = count;
			
			int nColSpace = CardColSpace;
			
			//初始化
			foreach (GameObject card in _cardlist) {
				Destroy (card);
			}
			_cardlist.Clear ();
			
			if (gameObject.activeSelf == false) {
				gameObject.SetActive (true);
			}
			
			//创建牌
			for (int i = 0; i < _cardcount; i++) {
				GameObject obj = Instantiate (CardPrefabs);
				if (i < 5) {
					m_card_0 [i] = _cardata [i];
					obj.transform.parent = cardParent_0.transform;
					obj.transform.localPosition = transform.localPosition + new Vector3 (i * CardColSpace, 0, 0);
				} else if (i < 10) {
					m_card_1 [i - 5] = _cardata [i];
					obj.transform.parent = cardParent_1.transform;
					obj.transform.localPosition = transform.localPosition + new Vector3 ((i - 5) * CardColSpace, 0, 0);
				} else if (i < 15) {
					m_card_2 [i - 10] = _cardata [i];
					obj.transform.parent = cardParent_2.transform;
					obj.transform.localPosition = transform.localPosition + new Vector3 ((i - 10) * CardColSpace, 0, 0);
				} else if (i < 20) {
					m_card_3 [i - 15] = _cardata [i];
					obj.transform.parent = cardParent_3.transform;
					obj.transform.localPosition = transform.localPosition + new Vector3 ((i - 15) * CardColSpace, 0, 0);
				} else if (i < 25) {
					m_card_4 [i - 20] = _cardata [i];
					obj.transform.parent = cardParent_4.transform;
					obj.transform.localPosition = transform.localPosition + new Vector3 ((i - 20) * CardColSpace, 0, 0);
				}	
				
				obj.transform.localScale = CardScale;
				obj.name = "card_" + i.ToString ();
				obj.GetComponent<UICard> ().GetCardTex = cards [i];
				//Debug.LogWarning(obj.GetComponent<UICard>().GetCardTex+"初始");
				obj.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				obj.transform.GetComponent<UISprite> ().depth = BaseDepth + 5 * i;
				//obj.transform.GetComponent<UICard>().FrameShowCard();

				_cardlist.Add (obj);					
			}

			bool frame = true;
			SortCard ("ctr_banker", frame);
			SortCard ("ctr_tian", frame);
			SortCard ("ctr_di", frame);
			SortCard ("ctr_xuan", frame);
			SortCard ("ctr_huang", frame);

			//显示牌值
			for (int i = 0; i < _cardcount; i++) {
				_cardlist [i].transform.GetComponent<UICard> ().FrameShowCard ();
			}
			//显示牌型
			for (int j = 0; j < 5; j++) {
				ShowCardType (j, true);
				StartCoroutine (CardPosition (j, 0.01f, true));
			}
	

			//比较大小
			MaxCardArea ();

		}


		public void ClearCard ()
		{
			foreach (GameObject card in _cardlist) {
				Destroy (card);
			}
			_cardcount = 0;


			foreach (GameObject card in list) {
				Destroy (card);
			}
			list.Clear ();
			

		}

		public void SetTwoCardData (byte[] cards, byte count)
		{
			//清空
			if (count == 0) {
				Array.Clear (_cardata, 0, _cardata.Length);
				_cardcount = 0;

				foreach (GameObject card in _cardlist) {
					Destroy (card);
				}
				_cardlist.Clear ();

				return;
			}

			//牌数据
			Buffer.BlockCopy (cards, 0, _cardata, 0, count);
			_cardcount = count;

			int nColSpace = CardColSpace;





			//初始化
			foreach (GameObject card in _cardlist) {
				Destroy (card);
			}
			_cardlist.Clear ();




			if (gameObject.activeSelf == false) {
				gameObject.SetActive (true);
			}

//			for (int i = 0; i < 10; i++) {
//
//				Debug.LogWarning (i + "初始值 =" + _cardata [i] + "颜色=" + GameLogic.GetCardColor (_cardata [i]) + "    牌值=" + GameLogic.GetMaxCardValue (_cardata [i]));
//
//			}

			//创建牌
			for (int i = 0; i < 25; i++) {
				GameObject obj = Instantiate (CardPrefabs);
				if (i < 5) {
					if (i < 2) {
						m_card_0 [i] = _cardata [i];
					} else {
						m_card_0 [i] = 23;
					}
											
//					m_card_0 [i] = _cardata [i];
					obj.transform.parent = cardParent_0.transform;
					obj.transform.localPosition = createPos_0.transform.localPosition + new Vector3 (0, i * 2.0f, 0);


				} else if (i < 10) {

					if (i < 7) {
						m_card_1 [i - 5] = _cardata [i - 3];
					} else {
						m_card_1 [i - 5] = 23;
					}

//					m_card_1 [i - 2] = _cardata [i];
					obj.transform.parent = cardParent_1.transform;
					obj.transform.localPosition = createPos_1.transform.localPosition + new Vector3 (0, (i - 5) * 2.0f, 0);
					;

				} else if (i < 15) {
					if (i < 12) {
						m_card_2 [i - 10] = _cardata [i - 6];
					} else {
						m_card_2 [i - 10] = 23;
					}

//					m_card_2 [i - 4] = _cardata [i];

					obj.transform.parent = cardParent_2.transform;
					obj.transform.localPosition = createPos_2.transform.localPosition + new Vector3 (0, (i - 10) * 2.0f, 0);

				} else if (i < 20) {
					if (i < 17) {
						m_card_3 [i - 15] = _cardata [i - 9];
					} else {
						m_card_3 [i - 15] = 23;
					}
//					m_card_3 [i - 6] = _cardata [i];

					obj.transform.parent = cardParent_3.transform;
					obj.transform.localPosition = createPos_3.transform.localPosition + new Vector3 (0, (i - 15) * 2.0f, 0);

				} else if (i < 25) {
					if (i < 22) {
						m_card_4 [i - 20] = _cardata [i - 12];
					} else {
						m_card_4 [i - 20] = 23;
					}
//					m_card_4 [i - 8] = _cardata [i];
					obj.transform.parent = cardParent_4.transform;
					obj.transform.localPosition = createPos_4.transform.localPosition + new Vector3 (0, (i - 20) * 2.0f, 0);

				}	
			

//
				if ((2 < i && i < 5) || (7 < i && i < 10) || (12 < i && i < 15) || (17 < i && i < 20) || (22 < i && i < 25)) {
					obj.GetComponent<UISprite> ().alpha = 1.0f;

				} else {
					obj.GetComponent<UISprite> ().alpha = 1.0f;
				
				}

				obj.transform.localScale = CardScale;
				obj.name = "card_" + i.ToString ();
				obj.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180.0f, 0));
				obj.transform.GetComponent<UISprite> ().depth = BaseDepth + 5 * i;
				UISprite sp = obj.transform.FindChild ("back").GetComponent<UISprite> ();
				sp.depth = BaseDepth + 5 * i + 1;
				sp.spriteName = "card_back";

				_cardlist.Add (obj);
//				//显示牌值
//				for (int k = 0; k < _cardcount; k++) {
//					_cardlist [k].transform.GetComponent<UICard> ().FrameShowCard ();
//				}
//				//显示牌型
//				for (int j = 0; j < 5; j++) {
//					ShowCardType (j, true);
//					StartCoroutine (CardPosition (j, 0.01f, true));
//				}
			}
		
		
		}
			
		//生成牌
		public void SetCardData (byte[] cards, byte count)
		{
			//清空
			if (count == 0) {
				Array.Clear (_cardata, 0, _cardata.Length);
				_cardcount = 0;
				
				foreach (GameObject card in _cardlist) {
					Destroy (card);
				}
				_cardlist.Clear ();
				
				return;
			}
			
			//牌数据
			Buffer.BlockCopy (cards, 0, _cardata, 0, count);
			_cardcount = count;
			
			int nColSpace = CardColSpace;
		

//			//初始化
//			foreach (GameObject card in _cardlist) {
//				list.Add (card);
//
////				Destroy (card);
//			}

//			Debug.LogWarning ("复制之后的数据个数" + list.Count);
//
//
//			_cardlist.Clear ();


			if (gameObject.activeSelf == false) {
				gameObject.SetActive (true);
			}
//			for (int i = 0; i < 25; i++) {
//				Debug.LogWarning ("初始值=" + _cardata [i] + "颜色=" + GameLogic.GetCardColor (_cardata [i]) + "    牌值=" + GameLogic.GetMaxCardValue (_cardata [i]));
//			}


			Debug.LogWarning ("测试上还在111===" + m_card_0 [0] + "===" + m_card_0 [1] + "===" + m_card_0 [2] + "===" + m_card_0 [3] + "===" + m_card_0 [4]);



			//创建牌
			for (int i = 0; i < _cardcount; i++) {
//				GameObject obj = Instantiate (CardPrefabs);
				if (i < 5) {
//					if (i < 2) {
//						Debug.LogWarning ("测试上个创建的OBJ 是否还在   1");
//
//						_cardlist.Add (list [Math.Abs (i - 4)]);
//						list [Math.Abs (i - 4)].name = "card_" + i.ToString ();
//
//					} else {
//						Destroy (list [Math.Abs (i - 5)].gameObject);
//						m_card_0 [i] = _cardata [i];
//					_cardlist.Add (obj);	
//						obj.name = "card_" + i.ToString ();
//						obj.transform.parent = cardParent_0.transform;

//					}
					m_card_0 [i] = _cardata [i];
//					obj.transform.parent = cardParent_0.transform;
//					obj.transform.localPosition = createPos_0.transform.localPosition + new Vector3 (0, i * 2.0f, 0);
				} else if (i < 10) {
//					if (i < 7) {
//						_cardlist.Add (list [Math.Abs (i - 14)]);
//						list [Math.Abs (i - 14)].name = "card_" + i.ToString ();

//						m_card_1 [i - 5] = _cardata [i - 3];
//					} else {
//						obj.name = "card_" + i.ToString ();

//						Destroy (list [Math.Abs (i - 5)].gameObject);
//						m_card_1 [i - 5] = _cardata [i];
//						_cardlist.Add (obj);	
//						obj.transform.parent = cardParent_1.transform;

//					}
										
					m_card_1 [i - 5] = _cardata [i];
//					obj.transform.parent = cardParent_1.transform;
//					obj.transform.localPosition = createPos_1.transform.localPosition + new Vector3 (0, (i - 5) * 2.0f, 0);
					;
				} else if (i < 15) {
			
					if (i < 12) {
//						_cardlist.Add (list [Math.Abs (i - 24)]);
//						list [Math.Abs (i - 24)].name = "card_" + i.ToString ();
//
//
//						m_card_2 [i - 10] = _cardata [i - 6];
					} else {
//						obj.name = "card_" + i.ToString ();

//						Destroy (list [Math.Abs (i - 5)].gameObject);
//						m_card_2 [i - 10] = _cardata [i];
//						_cardlist.Add (obj);	
//						obj.transform.parent = cardParent_2.transform;

					}
					m_card_2 [i - 10] = _cardata [i];
//					obj.transform.parent = cardParent_2.transform;
//					obj.transform.localPosition = createPos_2.transform.localPosition + new Vector3 (0, (i - 10) * 2.0f, 0);
				} else if (i < 20) {

//					if (i < 17) {
//						list [Math.Abs (i - 34)].name = "card_" + i.ToString ();
//						_cardlist.Add (list [Math.Abs (i - 34)]);
//					} else {
//						obj.name = "card_" + i.ToString ();
//
////						Destroy (list [Math.Abs (i - 5)].gameObject);
////						m_card_3 [i - 15] = _cardata [i];
//						_cardlist.Add (obj);	
//						obj.transform.parent = cardParent_3.transform;
//
//					}

					m_card_3 [i - 15] = _cardata [i];
//					obj.transform.parent = cardParent_3.transform;
//					obj.transform.localPosition = createPos_3.transform.localPosition + new Vector3 (0, (i - 15) * 2.0f, 0);
				} else if (i < 25) {

//					if (i < 22) {
//						
//						_cardlist.Add (list [Math.Abs (i - 44)]);
//						list [Math.Abs (i - 44)].name = "card_" + i.ToString ();
//
////						m_card_4 [i - 20] = _cardata [i - 12];
//					} else {
//						obj.name = "card_" + i.ToString ();
//
////						Destroy (list [Math.Abs (i - 5)].gameObject);
////						m_card_4 [i - 20] = _cardata [i];
//						_cardlist.Add (obj);	
//						obj.transform.parent = cardParent_4.transform;
//
//					}
					m_card_4 [i - 20] = _cardata [i];
//					obj.transform.parent = cardParent_4.transform;
//					obj.transform.localPosition = createPos_4.transform.localPosition + new Vector3 (0, (i - 20) * 2.0f, 0);
				}	



//				if ((1 < i && i < 5) || (6 < i && i < 10) || (11 < i && i < 15) || (16 < i && i < 20) || (21 < i && i < 25)) {
//					obj.GetComponent<UISprite> ().alpha = 1f;
//
//				} else {
//					obj.GetComponent<UISprite> ().alpha = 0f;
//				}
								
//				obj.transform.localScale = CardScale;
//				obj.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180.0f, 0));
//				obj.transform.GetComponent<UISprite> ().depth = BaseDepth + 5 * i;
//				UISprite sp = obj.transform.FindChild ("back").GetComponent<UISprite> ();
//				sp.depth = BaseDepth + 5 * i + 1;
//				sp.spriteName = "card_back";

//				_cardlist.Add (obj);	


			}
//			Debug.LogWarning ("最后的数据个数" + _cardlist.Count);
//			Debug.LogWarning ("arr1的数据个数" + m_card_0.Length);
//
			Debug.LogWarning ("测试上还在2222===" + m_card_0 [0] + "===" + m_card_0 [1] + "===" + m_card_0 [2] + "===" + m_card_0 [3] + "===" + m_card_0 [4]);

		
		}




		public void FirstSendOutTween (float time, bool show, bool isFirst)
		{
			StartCoroutine (FirstSendCardTween (time, show, isFirst));
		}


		//发牌特效
		public void SendOutTween (float time, bool show, bool isFirst)
		{
			StartCoroutine (SendCardTween (time, show, isFirst));
		}


		IEnumerator FirstSendCardTween (float time, bool show, bool isFirst)
		{
			
			yield return new WaitForSeconds (time);
			FirstSendOutCard (cardParent_0.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			FirstSendOutCard (cardParent_1.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			FirstSendOutCard (cardParent_2.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			FirstSendOutCard (cardParent_3.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			FirstSendOutCard (cardParent_4.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

		}


		IEnumerator SendCardTween (float time, bool show, bool isFirst)
		{

		
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_0.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_1.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);
			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_2.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);
			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_3.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);

			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_4.transform, show, isFirst);
			target.GetComponent<UIGame> ().PlayGameSound (SoundType.SENDCARD);
		}

		//显示开牌时候是否有牛的特效
		public void CardRotationTween (float time, bool show, bool isFirst)
		{
			StartCoroutine (ShowCardTween (time, show, isFirst));
		}

		IEnumerator ShowCardTween (float time, bool show, bool isFirst)
		{			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_1.transform, show, isFirst);
			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_2.transform, show, isFirst);
			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_3.transform, show, isFirst);
			
			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_4.transform, show, isFirst);

			yield return new WaitForSeconds (time);
			SendOutCard (cardParent_0.transform, show, isFirst);
		}

		void FirstSendOutCard (Transform CardParent, bool show, bool isFirst)
		{


			int i = 0;
			Array.Clear (fiveCrad, 0, 6);
			Array.Clear (fiveCardPos, 0, 6);


			foreach (Transform child in CardParent) {
				Vector3 newPos = new Vector3 ((i - 1) * CardColSpace, 0, 0);
				if (child.name != "createPos") {
					fiveCrad [i] = child;
					fiveCardPos [i] = newPos;
				}
				i++;
			}					
			//false

			StartCoroutine (FirstSendOutChild (oneCardTime, isFirst));
			StartCoroutine (FirstShowCardTween (CardParent, isFirst));

		}
		//发牌动画
		IEnumerator FirstSendOutChild (float time, bool isFirst)
		{

			yield return new WaitForSeconds (time);
			TweenPosition.Begin (fiveCrad [5].gameObject, 0.2f, fiveCardPos [1]);
			fiveCrad [5].GetComponent<UISprite> ().depth = BaseDepth + 0;
			fiveCrad [5].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 1;

			yield return new WaitForSeconds (time);
			TweenPosition.Begin (fiveCrad [4].gameObject, 0.2f, fiveCardPos [2]);
			fiveCrad [4].GetComponent<UISprite> ().depth = BaseDepth + 5;
			fiveCrad [4].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 6;


			yield return new WaitForSeconds (time);
			TweenPosition.Begin (fiveCrad [2].gameObject, 0.3f, fiveCardPos [4]);
			fiveCrad [2].GetComponent<UISprite> ().depth = BaseDepth + 15;
			fiveCrad [2].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 16;

			yield return new WaitForSeconds (time);
			TweenPosition.Begin (fiveCrad [1].gameObject, 0.3f, fiveCardPos [5]);
			fiveCrad [1].GetComponent<UISprite> ().depth = BaseDepth + 20;
			fiveCrad [1].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 21;

//			yield return new WaitForSeconds (time);
//			TweenPosition.Begin (fiveCrad [5].gameObject, 0.3f, fiveCardPos [1]);
//			fiveCrad [5].GetComponent<UISprite> ().depth = BaseDepth + 0;
//			fiveCrad [5].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 1;
//
//			yield return new WaitForSeconds (time);
//			TweenPosition.Begin (fiveCrad [4].gameObject, 0.3f, fiveCardPos [2]);
//			fiveCrad [4].GetComponent<UISprite> ().depth = BaseDepth + 5;
//			fiveCrad [4].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 6;

			yield return new WaitForSeconds (time);
			TweenPosition.Begin (fiveCrad [3].gameObject, 0.3f, fiveCardPos [3]);
			fiveCrad [3].GetComponent<UISprite> ().depth = BaseDepth + 10;
			fiveCrad [3].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 11;



		}

		//旋转动画、牌型显示以及牌型排序
		IEnumerator FirstShowCardTween (Transform cardParent, bool isFirst)
		{
			yield return new WaitForSeconds (0.3f);

			int id = 0;
			bool frame = false;
			string objName = cardParent.gameObject.name;

			//排序以及获取id
			id = SortCard (objName, frame);

			//牌型显示
			//			ShowCardType (id, false);
//			fiveCrad [1].GetComponent<UICard> ().ShowCard ();
//			fiveCrad [2].GetComponent<UICard> ().ShowCard ();
//			fiveCrad [3].GetComponent<UICard> ().ShowCard ();
			fiveCrad [4].GetComponent<UICard> ().ShowCard ();
			fiveCrad [5].GetComponent<UICard> ().ShowCard ();



//
//			Destroy (fiveCrad [1].gameObject);
//			Destroy (fiveCrad [2].gameObject);
//			Destroy (fiveCrad [3].gameObject);
//
			//牌值显示
			//			//有牛位置重设
			//			if (cardType [id].GetComponent<UISprite> ().spriteName != "cardType_0") {
			//				StartCoroutine (CardPosition (id, 0.01f, false));
			//			}

			//			MaxCardArea ();
		}





		void SendOutCard (Transform CardParent, bool show, bool isFirst)
		{
			int i = 0;
			Array.Clear (fiveCrad, 0, 6);
			Array.Clear (fiveCardPos, 0, 6);

			int K = 0;


			Debug.LogWarning ("子物体的个数" + CardParent.childCount);

			foreach (Transform child in CardParent) {
				Vector3 newPos = new Vector3 ((i - 1) * CardColSpace, 0, 0);
				if (child.name != "createPos") {
					fiveCrad [i] = child;
					fiveCardPos [i] = newPos;
				}
				i++;
				K++;
			}		
					


			if (show == false) {
				StartCoroutine (SendOutChild (oneCardTime, isFirst));
			} else {
				StartCoroutine (ShowCardTween (CardParent, isFirst));
			}

		}

		//发牌动画
		IEnumerator SendOutChild (float time, bool isFirst)
		{

			if (isFirst) {
				yield return new WaitForSeconds (time);
				TweenPosition.Begin (fiveCrad [2].gameObject, 0.3f, fiveCardPos [4]);
				fiveCrad [2].GetComponent<UISprite> ().depth = BaseDepth + 15;
				fiveCrad [2].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 16;
//				fiveCrad [2].GetComponent<UICard> ().ShowCard ();



				yield return new WaitForSeconds (time);
				TweenPosition.Begin (fiveCrad [1].gameObject, 0.3f, fiveCardPos [5]);
				fiveCrad [1].GetComponent<UISprite> ().depth = BaseDepth + 20;
				fiveCrad [1].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 21;
//				fiveCrad [1].GetComponent<UICard> ().ShowCard ();

			} else {
				yield return new WaitForSeconds (time);
				TweenPosition.Begin (fiveCrad [5].gameObject, 0.3f, fiveCardPos [1]);
				fiveCrad [5].GetComponent<UISprite> ().depth = BaseDepth;
				fiveCrad [5].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 1;

				yield return new WaitForSeconds (time);
				TweenPosition.Begin (fiveCrad [4].gameObject, 0.3f, fiveCardPos [2]);
				fiveCrad [4].GetComponent<UISprite> ().depth = BaseDepth + 5;
				fiveCrad [4].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 6;

				yield return new WaitForSeconds (time);
				TweenPosition.Begin (fiveCrad [3].gameObject, 0.3f, fiveCardPos [3]);
				fiveCrad [3].GetComponent<UISprite> ().depth = BaseDepth + 10;
				fiveCrad [3].FindChild ("back").GetComponent<UISprite> ().depth = BaseDepth + 11;


			}

		}

		//旋转动画、牌型显示以及牌型排序
		IEnumerator ShowCardTween (Transform cardParent, bool isFirst)
		{
			yield return new WaitForSeconds (0.1f);

			int id = 0;
			bool frame = false;
			string objName = cardParent.gameObject.name;

			//排序以及获取id

			//牌型显示

//			if (isFirst) {
//				for (int i = 1; i < 3; i++) {
//					fiveCrad [i].GetComponent<UICard> ().ShowCard ();
//				}
//			} else {
//			for (int i = 3; i < 6; i++) {
//				fiveCrad [i].GetComponent<UICard> ().ShowCard ();
//			}

			fiveCrad [1].GetComponent<UICard> ().ShowCard ();
			fiveCrad [2].GetComponent<UICard> ().ShowCard ();
			fiveCrad [3].GetComponent<UICard> ().ShowCard ();
//			}
			//牌值显示
//			//有牛位置重设
//			if (cardType [id].GetComponent<UISprite> ().spriteName != "cardType_0") {
//				StartCoroutine (CardPosition (id, 0.01f, false));
//			}


			id = SortCard (objName, frame);


//			MaxCardArea ();
			ShowCardType (id, false);

		}

		//清除牌型显示
		public void ClearCardType ()
		{
			for (int i = 0; i < 5; i++) {
				cardType [i].SetActive (false);
				cardType [i].GetComponent<UISprite> ().spriteName = "";
			}
		}

		//牌型判断
		void ShowCardType (int id, bool Frame)
		{
			byte bSex = 255;

			if (id == 0) {
				if (UIGame.s_bBankerUser == 255) {
					bSex = 0;
				} else {
					bSex = GameEngine.Instance.GetTableUserItem ((ushort)UIGame.s_bBankerUser).Gender;
				}
			} else {
				byte bChair = (byte)GameEngine.Instance.MySelf.DeskStation;
				bSex = GameEngine.Instance.GetTableUserItem (bChair).Gender;
			}

			switch (target.GetComponent<UIGame> ().m_cbCardType [id]) {
			case (byte)emCardType.CT_ERROR:
				{
					Debug.LogError ("牌型错误");
					break;
				}
			case (byte)emCardType.CT_POINT:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_0";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N0, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU1:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_1";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N1, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU2:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_2";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N2, bSex);
					}
					break; 
				}
			case (byte)emCardType.CT_SPECIAL_NIU3:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_3";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N3, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU4:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_4";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N4, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU5:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_5";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N5, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU6:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_6";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N6, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU7:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_7";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N7, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU8:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_8";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N8, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIU9:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_9";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N9, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIUNIU:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_10";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N10, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIUNIUXW:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_10";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N10, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_NIUNIUDW:
				{
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_10";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N10, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_BOMEBOME:
				{
					Debug.LogWarning ("炸弹");
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_bomb";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.N10, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_FIVECOLOR:
				{
					Debug.LogWarning ("五花");
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_FiveColor";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.WUXIAONIU, bSex);
					}
					break;
				}
			case (byte)emCardType.CT_SPECIAL_FIVESMALL:
				{
					Debug.LogWarning ("五小");
					cardType [id].SetActive (true);
					cardType [id].GetComponent<UISprite> ().spriteName = "cardType_FiveSmall";
					if (Frame == false) {
						target.GetComponent<UIGame> ().PlayUserSound (GameSoundType.BOMEBOME, bSex);
					}
					break;
				}
			}
		}

		//牌型排序
		int SortCard (string name, bool frame)
		{
			int id = 0;
			switch (name) {
			case "ctr_banker":
				{
//					GameLogic.GetOxCard (ref m_card_0, (byte)m_card_0.Length);

					for (int i = 0; i < m_card_0.Length; i++) {
						if (frame == true) {
							_cardlist [i].GetComponent<UICard> ().GetCardTex = m_card_0 [i];
						} else {
							_cardlist [i].GetComponent<UICard> ().GetCardTex = m_card_0 [Mathf.Abs (i - 4)];
						}
					}

					id = 0;
					break;
				}
			case "ctr_tian":
				{
//					GameLogic.GetOxCard (ref m_card_1, (byte)m_card_1.Length);

					for (int i = 0; i < m_card_1.Length; i++) {
						if (frame == true) {
							_cardlist [i + 5].GetComponent<UICard> ().GetCardTex = m_card_1 [i];
						} else {
							_cardlist [i + 5].GetComponent<UICard> ().GetCardTex = m_card_1 [Mathf.Abs (i - 4)];
						}
					}

					id = 1;
					break;
				}
			case "ctr_di":
				{
//					GameLogic.GetOxCard (ref m_card_2, (byte)m_card_2.Length);

					for (int i = 0; i < m_card_2.Length; i++) {
						if (frame == true) {
							_cardlist [i + 10].GetComponent<UICard> ().GetCardTex = m_card_2 [i];
						} else {
							_cardlist [i + 10].GetComponent<UICard> ().GetCardTex = m_card_2 [Mathf.Abs (i - 4)];
						}
					}

					id = 2;
					break;
				}
			case "ctr_xuan":
				{
//					GameLogic.GetOxCard (ref m_card_3, (byte)m_card_3.Length);

					for (int i = 0; i < m_card_3.Length; i++) {
						if (frame == true) {
							_cardlist [i + 15].GetComponent<UICard> ().GetCardTex = m_card_3 [i];
						} else {
							_cardlist [i + 15].GetComponent<UICard> ().GetCardTex = m_card_3 [Mathf.Abs (i - 4)];
						}
					}

					id = 3;
					break;
				}
			case "ctr_huang":
				{
//					GameLogic.GetOxCard (ref m_card_4, (byte)m_card_4.Length);

					for (int i = 0; i < m_card_4.Length; i++) {
						if (frame == true) {
							_cardlist [i + 20].GetComponent<UICard> ().GetCardTex = m_card_4 [i];
						} else {
							_cardlist [i + 20].GetComponent<UICard> ().GetCardTex = m_card_4 [Mathf.Abs (i - 4)];
						}
					}

					id = 4;
					break;
				}
			}
			return id;
		}

		//获取每组牌中最大的牌
		void MaxCardArea ()
		{
			for (int i = 0; i < 5; i++) {
				maxCardArea [i] = 0;
				maxCardColor [i] = 0;
				for (int k = 0; k < 5; k++) {
					switch (i) {
					case 0:
						{
							byte cardValue = GameLogic.GetMaxCardValue (m_card_0 [k]);
							byte cardColor = GameLogic.GetCardColor (m_card_0 [k]);
							GameLogic.GetCardColor (m_card_0 [k]);
							if ((maxCardArea [0] < cardValue) || ((cardValue == maxCardArea [0]) && (GameLogic.GetCardColor (m_card_0 [k]) > GameLogic.GetCardColor (m_card_0 [PositiveNumber (k - 1)])))) {
								maxCardArea [0] = cardValue;
								maxCardColor [0] = cardColor;
							}
							break;
						}
					case 1:
						{
							byte cardValue = GameLogic.GetMaxCardValue (m_card_1 [k]);
							byte cardColor = GameLogic.GetCardColor (m_card_1 [k]);
							if ((maxCardArea [1] < cardValue) || ((cardValue == maxCardArea [1]) && (GameLogic.GetCardColor (m_card_1 [k]) > GameLogic.GetCardColor (m_card_1 [PositiveNumber (k - 1)])))) {
								maxCardArea [1] = cardValue;
								maxCardColor [1] = cardColor;

							}
							break;
						}
					case 2:
						{
							byte cardValue = GameLogic.GetMaxCardValue (m_card_2 [k]);
							byte cardColor = GameLogic.GetCardColor (m_card_2 [k]);
							if ((maxCardArea [2] < cardValue) || ((cardValue == maxCardArea [2]) && (GameLogic.GetCardColor (m_card_2 [k]) > GameLogic.GetCardColor (m_card_2 [PositiveNumber (k - 1)])))) {
								maxCardArea [2] = cardValue;
								maxCardColor [2] = cardColor;
							}
							break;
						}
					case 3:
						{
							byte cardValue = GameLogic.GetMaxCardValue (m_card_3 [k]);
							byte cardColor = GameLogic.GetCardColor (m_card_3 [k]);
							if ((maxCardArea [3] < cardValue) || ((cardValue == maxCardArea [3]) && (GameLogic.GetCardColor (m_card_3 [k]) > GameLogic.GetCardColor (m_card_3 [PositiveNumber (k - 1)])))) {
								maxCardArea [3] = cardValue;
								maxCardColor [3] = cardColor;
							}
							break;
						}
					case 4:
						{
							byte cardValue = GameLogic.GetMaxCardValue (m_card_4 [k]);
							byte cardColor = GameLogic.GetCardColor (m_card_4 [k]);
							if ((maxCardArea [4] < cardValue) || ((cardValue == maxCardArea [4]) && (GameLogic.GetCardColor (m_card_4 [k]) > GameLogic.GetCardColor (m_card_4 [PositiveNumber (k - 1)])))) {
								maxCardArea [4] = cardValue;
								maxCardColor [4] = cardColor;
							}
							break;
						}
					}
				}
			}
		}

		//有牛时卡牌位置重排
		IEnumerator CardPosition (int id, float time, bool frame)
		{
			yield return new WaitForSeconds (time);
			int index = 0;
			int i = 0;

			switch (id) {
			case 0:
				{
					index = 0;
					break;
				}
			case 1:
				{
					index = 5;
					break;
				}
			case 2:
				{
					index = 10;
					break;
				}
			case 3:
				{
					index = 15;
					break;
				}
			case 4:
				{
					index = 20;
					break;
				}
			}

			if (frame == false) {
				for (int k = index + 4; k >= index; k--) {
					if (i < 3) {
						_cardlist [k].transform.localPosition = new Vector3 ((CardColSpace - CardTypeSpace / 5) * i, 0, 0);
					} else {
						_cardlist [k].transform.localPosition = new Vector3 ((CardColSpace - CardTypeSpace / 5) * i + CardTypeSpace, 0, 0);
					}
					i++;
				}
			} else {
				for (int k = index; k < index + 5; k++) {
					if (i < 3) {
						_cardlist [k].transform.localPosition = new Vector3 ((CardColSpace - 1) * i, 0, 0);
					} else {
						_cardlist [k].transform.localPosition = new Vector3 ((CardColSpace - 1) * i + 5, 0, 0);
					}
					i++;
				}
			}
		}

		int PositiveNumber (int number)
		{
			if (number < 0) {
				return 0;
			}
			return number;
		}
	}
}