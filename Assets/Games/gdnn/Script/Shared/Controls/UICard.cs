using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.GDNN
{
	[AddComponentMenu ("Custom/Controls/UICard")]

	public class UICard : MonoBehaviour
	{
		public 	Transform tweenTarget = null;
		//当前物体

		public GameObject cardValue = null;
		//当前卡牌值
		public GameObject cardCorol = null;
		//卡牌花色
		public GameObject cardHead = null;
		//卡牌头像花色
		public GameObject kingColor = null;
		//大小王花色
		public GameObject back = null;
		//牌背面
		public GameObject cardHead_person = null;
		//卡牌头像人物

		private string carddName = null;
		//卡牌名字
		public 	byte GetCardTex;
		//卡牌数据

		void Awake ()
		{
			if (tweenTarget == null) {
				tweenTarget = this.transform;
			}
		}

		//翻转卡牌
		public void ShowCard ()
		{
			cardValue.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			cardCorol.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			kingColor.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			cardHead.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 1;
			cardHead_person.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 1;
			back.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 3;
			StartCoroutine (ShowCardRotation ());
		}

		//卡牌显示（框架消息调用）
		public void FrameShowCard ()
		{
			cardValue.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			cardCorol.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			kingColor.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 2;
			cardHead.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 1;
			cardHead_person.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 1;
			back.GetComponent<UISprite> ().depth = this.GetComponent<UISprite> ().depth + 0;
			ShowCardType (GetCardTex);
		}

		IEnumerator ShowCardRotation ()
		{
			TweenRotation.Begin (this.gameObject, 0.3f, Quaternion.Euler (new Vector3 (0, 90.0f, 0)));

			yield return new WaitForSeconds (0.25f);
			ShowCardType (GetCardTex);

			TweenRotation.Begin (this.gameObject, 0.2f, Quaternion.Euler (new Vector3 (0, 1.0f, 0)));

			yield return new WaitForSeconds (0.1f);
			this.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		}

		private void ShowCardType (byte bCard)
		{
			
			byte MASK_COLOR = 0xF0;
			byte MASK_VALUE = 0x0F;
			
			byte bColor = (byte)((bCard & MASK_COLOR) >> 4);
			byte bValue = (byte)(bCard & MASK_VALUE);

			back.SetActive (false);

			if ((bValue > 0 && bValue < 16) && (bColor >= 0 && bColor <= 4)) {
				if (bColor < 4) {
					if (bValue > 10) {
						cardHead_person.GetComponent<UISprite> ().spriteName = "cardHead_person_" + bColor + "_" + bValue;
						cardValue.GetComponent<UISprite> ().spriteName = "cardValue_" + (bColor % 2).ToString () + "_" + bValue.ToString ();
						cardCorol.GetComponent<UISprite> ().spriteName = "cardCorol_" + bColor.ToString ();

						cardHead.SetActive (false);
						cardHead_person.SetActive (true);
					} else {
						cardHead.GetComponent<UISprite> ().spriteName = "head_" + bColor.ToString ();
						cardValue.GetComponent<UISprite> ().spriteName = "cardValue_" + (bColor % 2).ToString () + "_" + bValue.ToString ();
						cardCorol.GetComponent<UISprite> ().spriteName = "cardCorol_" + bColor.ToString ();

						cardHead.SetActive (true);
						cardHead_person.SetActive (false);
					}
						
					kingColor.SetActive (false);
					cardValue.SetActive (true);
					cardCorol.SetActive (true);


				} else {
					kingColor.GetComponent<UISprite> ().spriteName = "king_" + bValue.ToString ();
					cardHead_person.GetComponent<UISprite> ().spriteName = "kingHead_" + bValue.ToString ();
					kingColor.SetActive (true);
					cardValue.SetActive (false);
					cardCorol.SetActive (false);
					cardHead.SetActive (false);
					cardHead_person.SetActive (true);
				}
			}	
		}
	}

}


