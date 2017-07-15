using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 有面板的 买子弹 和 结算. 挂在名为 “buyScorePanel” Gameobject上（现在没使用）.
	/// </summary>
	public class BuyScorePanelCtrl : MonoBehaviour 
	{
		public static BuyScorePanelCtrl Instance;

		// default value should be -1.
		private int centerIndex = -1;
		private UICenterOnChild centerOnChild;
		[HideInInspector] public bool showPanel = false;

		void Awake()
		{
			Instance = this;
			centerOnChild = GetComponentInChildren<UICenterOnChild>();
			centerOnChild.onDragFinishedWithIndex += SetScrollViewCenterIndex;
		}

		void OnDestroy()
		{
			centerOnChild.onDragFinishedWithIndex -= SetScrollViewCenterIndex;
			Instance = null;
		}

		void Start()
		{
			for(int i=0; i<CanonCtrl.Instance.buyBulletPowerScore.Length; i++)
			{
				centerOnChild.transform.GetChild(i).FindChild("value").GetComponent<UILabel>().text = CanonCtrl.Instance.buyBulletPowerScore[i].ToString();
			}
			// Debug.Log("centerOnChild.transform.childCount = "+centerOnChild.transform.childCount.ToString());
		}

		private int lastCenterIndex = -2;
		public void SetScrollViewCenterIndex(GameObject _centerOBJ)
		{
			for(int i=0; i<centerOnChild.transform.childCount; i++)
			{
				if(_centerOBJ == centerOnChild.transform.FindChild("Item_"+i).gameObject)
				{
					centerIndex = i;
					break;
				}
			}

			if(centerIndex==lastCenterIndex)
			{
				return;
			}
			lastCenterIndex = centerIndex;

			// HightLight.
			for(int i=0; i<centerOnChild.transform.childCount; i++)
			{
				if(i==centerIndex)
				{
					centerOnChild.transform.FindChild("Item_"+i).FindChild("HightLight").gameObject.SetActive(true);
				}
				else 
				{
					centerOnChild.transform.FindChild("Item_"+i).FindChild("HightLight").gameObject.SetActive(false);
				}
			}

			CheckIfEnoughGold();
		}


		void CheckIfEnoughGold()
		{
			for(int i=0; i<centerOnChild.transform.childCount; i++)
			{
				if(i<CanonCtrl.Instance.buyBulletPowerScore.Length)
				{
					int _buyScore = CanonCtrl.Instance.buyBulletPowerScore[i];
					int _goldCost = (_buyScore / CanonCtrl.Instance.oneGoldScore) * CanonCtrl.Instance.roomMulti; 

					if(CanonCtrl.Instance.goldValue>=_goldCost)
					{
						centerOnChild.transform.FindChild("Item_"+i).FindChild("notEnough").gameObject.SetActive(false);
					}
					else 
					{
						centerOnChild.transform.FindChild("Item_"+i).FindChild("notEnough").gameObject.SetActive(true);
					}
				}
			}
		}

		// Press buy score panel.
		public void ShowBuyScorePanel()
		{
			// opening any panel.
			if(CanonCtrl.Instance.OpeningAnyPanel())
			{
				return;
			}

			showPanel = !showPanel;

			if(showPanel)
			{
				// stop auto fire.
				CanonCtrl.Instance.StopLock();
				CanonCtrl.Instance.SetAutoFire(false);

				GetComponent<TweenPosition>().PlayForward();
				GetComponent<TweenRotation>().PlayForward();
			}
			else
			{
				GetComponent<TweenPosition>().PlayReverse();
				GetComponent<TweenRotation>().PlayReverse();
			}

			CheckIfEnoughGold();

			AudioCtrl.Instance.ShowBuyBulletPanel();
		}

		// press buy button.
		public void Buy()
		{
			if(centerIndex>CanonCtrl.Instance.buyBulletPowerScore.Length-1)
			{
				return;
			}
			int _buyScore = CanonCtrl.Instance.buyBulletPowerScore[centerIndex];
			int _goldCost = (_buyScore / CanonCtrl.Instance.oneGoldScore) * CanonCtrl.Instance.roomMulti; 

			if(CanonCtrl.Instance.goldValue>=_goldCost)
			{
				// send to server.
				MessageHandler.Instance.C_S_BuyBullet((uint)_buyScore);
				Back();
				AudioCtrl.Instance.PressButton(true);
			}
		}

		// press back button.
		public void Back()
		{
			showPanel = false;
			GetComponent<TweenPosition>().PlayReverse();
			GetComponent<TweenRotation>().PlayReverse();
		}
	}
}
