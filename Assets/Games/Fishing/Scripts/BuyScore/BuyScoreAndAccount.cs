using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 没有面板的 买子弹 和 结算.（现在有使用）.
	/// </summary>
	public class BuyScoreAndAccount : MonoBehaviour 
	{
		public static BuyScoreAndAccount Instance;
		public bool AutoBuyScore_on = true;

		void Awake()
		{
			Instance = this;
		}

		void OnDestroy()
		{
			Instance = null;
		}

		// 点击了买分按钮.
		public void C_S_BuyScore_NoPanel()
		{
			CheckAndBuy();
            // 延时设置自动发泡状态,避免重复上分
            StartCoroutine("autoBuyScore");
		}

		// 检查是否够金币买分.
		void CheckAndBuy()
		{
			int _buyScore = CanonCtrl.Instance.buyBulletPowerScore[0];          
//			int _goldCost = (_buyScore / CanonCtrl.Instance.oneGoldScore) * CanonCtrl.Instance.roomMulti; 

			// 够分才发送到服务器.2015-10-10：ADa去掉判断条件
			if(true)//(CanonCtrl.Instance.goldValue>=_goldCost)
			{
				MessageHandler.Instance.C_S_BuyBullet((uint)_buyScore);
				CanonCtrl.Instance.BuyLastScore = _buyScore;// 记录最后一次购买的分数
				AudioCtrl.Instance.ShowBuyBulletPanel();
			}
		}

		// 按了结算按钮.
		public void C_S_Account_NoPanel()
		{
			AutoBuyScore_on = false;

			// 玩家自动购买炮弹数清零.
			CanonCtrl.Instance.BuyLastScore = 0;

			// 把玩家杀死的鱼，还没返还给玩家的分数马上给玩家.
			CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].GiveValue2PlyImmediate();

			MessageHandler.Instance.C_S_Balance();
			AudioCtrl.Instance.ShowAccount();

			// 显示结算
//			CanonCtrl.Instance.PressAccountButton ();

			// 停止锁定.
			CanonCtrl.Instance.StopLock ();
			// 播放取消锁定声音.
			AudioCtrl.Instance.PressButton(false);

			// 玩家下分就不显示爆机提示.
			SingleCanon _singleCanon = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID];
			_singleCanon.ShowExploHint (false);
		}

		// 从服务器接收到的金币数量.(设置对应玩家结算后的参数)
		public void S_C_Account_NoPanel(int _goldValue ,int chair)
		{
			// 设置金币数量.(如果是真实玩家才改变)
			if( chair == CanonCtrl.Instance.realCanonID)
			{
				CanonCtrl.Instance.S_C_AccountMsgHaveReceive = true;
				CanonCtrl.Instance.SetGoldValue(_goldValue);
				// 玩家自动购买炮弹数记零
				CanonCtrl.Instance.BuyLastScore = 0;
			}
			// 把对应玩家的分数清零.
//			CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].SetPlyValue(0);
			CanonCtrl.Instance.singleCanonList[chair].SetPlyValue(0);
		}

		// 玩家自动购买炮弹
		public void AutoBuyScore()
		{
			int _buyScore = CanonCtrl.Instance.BuyLastScore;
			//int _goldCost = (_buyScore / CanonCtrl.Instance.oneGoldScore) * CanonCtrl.Instance.roomMulti; 
			
			// 够分才发送到服务器.
			if(true)//(CanonCtrl.Instance.goldValue>=_goldCost)
			{
				AutoBuyScore_on = false;
				StartCoroutine ("autoBuyScore");
				MessageHandler.Instance.C_S_BuyBullet((uint)_buyScore);
//				CanonCtrl.Instance.BuyLastScore = _buyScore;// 记录最后一次购买的分数
			}
		}

		IEnumerator autoBuyScore(){
			yield return new WaitForSeconds(0.5f);
			AutoBuyScore_on = true;
		}

	}
}
