using UnityEngine;

namespace com.QH.QPGame.Fishing
{

	/// <summary>
	/// Single coin.
	/// </summary>

	public class SingleCoin : MonoBehaviour 
	{
		private Transform curTrans;
		private enum CoinState
		{
			none=0,
			wait,
			punching_up,
			punching_down,
			wait2,
			lerping
		}
		private CoinState curCoinState = CoinState.none;
		
		private float targetTime;
		//private int ownerIndex;
		private Vector3 initPos;
		private Vector3 targetPos;
		private Vector3 ownerPos;
		public float punchHeight = 1.2f;
		public float punchSpeed = 4f;
		public float wait4Lerp = 0.4f;
		public float lerp2CanonSpeed = 0.85f;

		private AudioSource audioS;
		private bool playAudio;

		void Awake()
		{
 			curTrans = this.transform;
// 			audioS = GetComponent<AudioSource>();
// 			audioS.playOnAwake = false;
// 			audioS.Stop();
		}

		// 初始化每一个金币.
		public void Init(int _canonID, Vector3 _ownerPos, float _targetTime2Punch, bool _play)
		{
			curCoinState = CoinState.wait;
			targetTime = Time.time + _targetTime2Punch;
			//ownerIndex = _ownerIndex;
			initPos = curTrans.position;
			switch(CanonCtrl.Instance.dirArray[_canonID])
			{
			case 0:
				targetPos = curTrans.position + new Vector3(0f, punchHeight, 0f);
				break;
			case 1:
				targetPos = curTrans.position - new Vector3(punchHeight, 0f, 0f);
				break;
			case 2:
				targetPos = curTrans.position - new Vector3(0f, punchHeight, 0f);
				break;
			case 3:
				targetPos = curTrans.position + new Vector3(punchHeight, 0f, 0f);
				break;
			}
			ownerPos = _ownerPos;
			playAudio = _play;
		}

		void Update () 
		{
			if(curCoinState==CoinState.wait)
			{
				if(Time.time>targetTime)
				{
					curTrans.localScale = Vector3.one;
					targetTime = 0f;

// 					if(playAudio)
// 					{
// 						audioS.Play();
// 					}
					curCoinState = CoinState.punching_up;
				}
			}
			else if(curCoinState==CoinState.punching_up)
			{
				targetTime+=Time.deltaTime*punchSpeed;
				curTrans.position = Vector3.Lerp(initPos, targetPos, targetTime);
				if(targetTime>1f)
				{
					targetTime = 0f;
					curCoinState = CoinState.punching_down; 
				}
			}
			else if(curCoinState==CoinState.punching_down)
			{
				targetTime+=Time.deltaTime*punchSpeed;
				curTrans.position = Vector3.Lerp(targetPos, initPos, targetTime);
				if(targetTime>1f)
				{
					targetTime = Time.time + wait4Lerp;
					curCoinState = CoinState.wait2;                           
				}
			}
			else if(curCoinState==CoinState.wait2)
			{
				if(Time.time>targetTime)
				{
					curCoinState = CoinState.lerping;
					targetTime = 0f;
				}
			}
			else if(curCoinState==CoinState.lerping)
			{
				targetTime += Time.deltaTime * lerp2CanonSpeed;
				curTrans.position = Vector3.Lerp(initPos, ownerPos, targetTime);
				if(targetTime>1f)
				{
//					audioS.Stop();
					Factory.Recycle(curTrans);
				}
			}
		}
	}
}
