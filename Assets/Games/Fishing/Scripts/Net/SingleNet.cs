using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleNet : MonoBehaviour 
	{
		public enum ScaleState
		{
			scaling=0,
			stopScale
		}
		private ScaleState curScaleState = ScaleState.scaling;
		private float targetScaleFactor;
		public Vector3 targetScale = Vector3.one;
		public float scaleSpeed = 4f;
		public float timeLengthAfterScaleUp = 0.5f;

		void OnEnable () 
		{
			targetScaleFactor = 0f;
			transform.localScale = Vector3.zero;
			curScaleState = ScaleState.scaling;
		}
		
		void Update () 
		{
			if(curScaleState==ScaleState.scaling)
			{
				targetScaleFactor += Time.deltaTime * scaleSpeed;
				transform.localScale = Vector3.Slerp(Vector3.zero, targetScale, targetScaleFactor); 
				if(targetScaleFactor > 1f)
				{
					curScaleState = ScaleState.stopScale;
					targetScaleFactor = 0f;
				}
			}
			else if(curScaleState==ScaleState.stopScale)
			{
				targetScaleFactor += Time.deltaTime;
				if(targetScaleFactor > timeLengthAfterScaleUp)
				{
					Factory.Recycle(transform);
				}
			}
		}
	}
}
