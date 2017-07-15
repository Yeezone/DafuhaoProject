using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 退出键框控制器，挂在 EscapePanel 物件上.
	/// </summary>
	public class EscapeButtonCtrl : MonoBehaviour 
	{
		public	Transform	CanonCtrl;
		public	Vector2		TargetLocalScale = Vector2.one;
		private TweenScale tweenScale;
		private GameObject	ScoreWindow;

		void Start () 
		{
//			CanonCtrl.gameObject = GameObject.Find("CanonCtrl");
			ScoreWindow = GameObject.Find("ScoreWindow");
			transform.localScale = Vector3.zero;
			tweenScale = GetComponent<TweenScale>();
			if(tweenScale!=null)
			{
				tweenScale.enabled = false;
			}


		}

		void Update ()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
//				PressEscapeKey();
				if(ScoreWindow != null && ScoreWindow.activeSelf) ScoreWindow.SetActive(false);
				CanonCtrl.GetComponent<CanonCtrl>().PressAccountButton();
			}
		}

		// 出现退出确定框.
		public void PressEscapeKey() 
		{
			//取消锁定
			if(CanonCtrl.GetComponent<CanonCtrl>().inLockMode)
//			CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].plyScore.ToString();
			{
				CanonCtrl.GetComponent<CanonCtrl>().PressLockButton();
			}
			//取消自动发炮
			if(CanonCtrl.GetComponent<CanonCtrl>().autoFire)
			{
				CanonCtrl.GetComponent<CanonCtrl>().SetAutoFire(false);
			}
			//关闭上下分窗口
			Transform CancelShow=transform.Find("ScoreWindow");
			if(CancelShow!=null)
			{
				CancelShow.gameObject.SetActive(false);
			}
			//
			if(tweenScale!=null)
			{
				tweenScale.enabled = true;
				tweenScale.PlayForward();



			}
			else
			{
				transform.localScale = (transform.localScale==Vector3.zero?TargetLocalScale:Vector2.zero);
			}

		}
		// 返回.
		public void Back()
		{
			if(tweenScale!=null)
			{
				tweenScale.PlayReverse();
			}
			else 
			{
				transform.localScale = Vector3.zero;
			}
		}
		// 退出.
		public void Exit()
		{
		}
	}
}
