using UnityEngine;

namespace com.QH.QPGame.Fishing
{

	/// <summary>
	/// 手势上分，结算，锁定，自动发泡.
	/// </summary>

	public class GestureDetector : MonoBehaviour 
	{
		// 是否使用手势功能.
		public bool useGesture = false;

		// 手势移动超过200个像素才算是一个手势.
		public float slideTargetPixel = 200f;

		// 手势方向.
		public enum FingerDir
		{
			none,
			left,
			right,
			up,
			down
		}
		private FingerDir fingerDir = FingerDir.none;

		public enum Func
		{
			none,
			lockFire,
			autoFire,
			upscore,
			account
		}
		// 左手势做什么功能.
		public Func left = Func.none;
		// 右手势做什么功能.
		public Func right = Func.none;
		// 上手势做什么功能.
		public Func up = Func.none;
		// 下手势做什么功能.
		public Func down = Func.none;

		//　手势起始和末尾位置.
		private Vector3 touchPos_0;
		private Vector3 touchPos_1;


		void Update () 
		{
#if !UNITY_EDITOR && UNITY_ANDROID

			if(!useGesture)
			{
				return;
			}

			if(Input.touchCount!=1)
			{
				return;
			}

			if(Input.GetTouch(0).phase== TouchPhase.Began || Input.GetTouch(0).phase== TouchPhase.Stationary || Input.GetTouch(0).phase== TouchPhase.Canceled)
			{
				fingerDir = FingerDir.none;
				touchPos_0 = Input.GetTouch(0).position;
				touchPos_1 = Input.GetTouch(0).position;
			}

			// 手势停止了.
			if(Input.GetTouch(0).phase== TouchPhase.Ended)
			{
				touchPos_1 = Input.GetTouch(0).position;
				float xMoveDistance = touchPos_1.x - touchPos_0.x;
				float yMoveDistance = touchPos_1.y - touchPos_0.y;

				// 手势是否满足移动距离.
				if(Mathf.Abs(xMoveDistance)<slideTargetPixel && Mathf.Abs(yMoveDistance)<slideTargetPixel)
				{
					return;
				}
				
				// 判断方向.
				if(Mathf.Abs(xMoveDistance)>=Mathf.Abs(yMoveDistance))
				{
					if(xMoveDistance>0f)
					{
						fingerDir = FingerDir.right;
					}
					else 
					{
						fingerDir = FingerDir.left;
					}
				}
				else 
				{
					if(yMoveDistance>0f)
					{
						fingerDir = FingerDir.up;
					}
					else 
					{
						fingerDir = FingerDir.down;
					}
				}

				DoGes();

				fingerDir = FingerDir.none;
			}
#endif
		}


		// 手势.
		public void DoGes()
		{
			switch(fingerDir)
			{
			case FingerDir.left:
				DoFunc(left);
				break;
			
			case FingerDir.right:
				DoFunc(right);
				break;
				
			case FingerDir.up:
				DoFunc(up);
				break;

			case FingerDir.down:
				DoFunc(down);
				break;
			}
		}
		
		// 手势对应的功能.
		void DoFunc(Func _func)
		{
			switch(_func)
			{
				// 锁定.
			case Func.lockFire:
				CanonCtrl.Instance.PressLockButton();
				break;
			
				// 自动发泡.
			case Func.autoFire:
				CanonCtrl.Instance.PressAutoFireButton();
				break;
			
				// 上分.
			case Func.upscore:
				BuyScoreAndAccount.Instance.C_S_BuyScore_NoPanel();
				break;

				// 结算.
			case Func.account:
				BuyScoreAndAccount.Instance.C_S_Account_NoPanel();
				break;
			}
		}
			
	}
}
