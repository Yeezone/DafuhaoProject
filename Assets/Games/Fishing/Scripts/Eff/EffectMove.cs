using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class EffectMove : MonoBehaviour 
	{
		private Transform curTransform;
		private float movePercent;
		private float moveSpeed;
		private Vector3 startPos;
		private Vector3 targetPos;
		private bool startMove = false;

		void Start()
		{
			curTransform = transform;
		}

		public void Move(float _timeLength, Vector3 _targetPos)
		{
			startPos = transform.localPosition;
			targetPos = _targetPos;
			moveSpeed = 1f/_timeLength;
			movePercent = 0f;
			startMove = true;
		}

		void Update () 
		{
			if(!startMove)
			{
				return;
			}
			movePercent += Time.deltaTime * moveSpeed;
			curTransform.localPosition = Vector3.Lerp(startPos, targetPos, movePercent);
			if(movePercent>=1f)
			{
				startMove = false;
			}
		}
	}
}
