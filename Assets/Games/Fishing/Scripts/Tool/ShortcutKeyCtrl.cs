using UnityEngine;

namespace com.QH.QPGame.Fishing
{

	/// <summary>
	/// 手势上分，结算，锁定，自动发泡.
	/// </summary>

	public class ShortcutKeyCtrl : MonoBehaviour 
	{

		// 快捷键.
		public enum FingerDir
		{
			none,
			left,
			right,
			up,
			down,
			S,
			Q
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
		// 左做什么功能.
		public Func left = Func.none;
		// 右做什么功能.
		public Func right = Func.none;
		// 上做什么功能.
		public Func up = Func.none;
		// 下做什么功能.
		public Func down = Func.none;
		// S做什么功能.
		public Func S = Func.none;
		// Q做什么功能.
		public Func Q = Func.none;


		/*void Update () 
		{
			if(Input.GetKey(KeyCode.UpArrow))
			{
				//上键
			}else if(Input.GetKey(KeyCode.DownArrow))
			{
				//下键
			}else if(Input.GetKey(KeyCode.LeftArrow))
			{
				//左键
			}else if(Input.GetKey(KeyCode.RightArrow))
			{
				//右键
			}else if(Input.GetKey(KeyCode.S))
			{
				//S键
			}else if(Input.GetKey(KeyCode.Q))
			{
				//Q键
			}
			#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
			#endif
		}
		*/

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
