using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 自动回收脚本.
	/// </summary>
	public class AutoRecycle : MonoBehaviour 
	{
		public float lifeTime = 1f;
		private float targetTime = 0f;

		void OnEnable() 
		{
			targetTime = Time.time + lifeTime;
		}

		public void SetLifeTime(float _lifeTime)
		{
			lifeTime = _lifeTime;
			targetTime = Time.time + lifeTime;
		}

		void Update () 
		{
			if(Time.time >= targetTime)
			{
				// recycle.
				Factory.Recycle(transform);
			}
		}
	}
}