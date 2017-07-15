using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 本地时间控制器.
	/// </summary>
	public class TimeCtrl : MonoBehaviour 
	{
		public static TimeCtrl Instance;
		[HideInInspector] public uint serverTime;
		private bool countDown = false;

		// 本地开始计时.
		public void StartCountDown(uint _serverTime)
		{
			countDown = true;
			serverTime = _serverTime;
		}

		void Awake () 
		{
			Instance = this;
		}

		// Unity 固定帧率，每一帧20毫秒.
		void FixedUpdate()
		{
			if(countDown)
			{
				serverTime += 20;
			}
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}