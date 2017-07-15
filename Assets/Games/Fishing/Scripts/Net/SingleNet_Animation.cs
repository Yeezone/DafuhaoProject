using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleNet_Animation : MonoBehaviour
	{
		private float targetTime2Recycle;
		public float timeLength;


		void OnEnable()
		{
			targetTime2Recycle = Time.time + timeLength;
		}
		
		void Update () 
		{
			if(Time.time >= targetTime2Recycle)
			{
				Factory.Recycle(this.transform);
			}
		}
	}
}