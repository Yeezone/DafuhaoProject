using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleFishNum : MonoBehaviour 
	{
		private Transform curTrans;
		private enum NumState
		{
			none=0,
			punching_up,
			punching_down,
			wait,
			fadeOut
		}
		private NumState curNumState = NumState.none;

		public float punchHeight = 1f;
		public float punchSpeed = 3f;
		public float punchDoneWaitTimeLength = 0.5f;
		public float fadeOutSpeed = 1.5f;
		private Vector3 punchInitPos;
		private Vector3 punchHeightPos;

		private float targetTime;
		private UILabel label;

		void Awake()
		{
			curTrans = this.transform;
			label = GetComponent<UILabel>();
		}

		public void Init(int _num)
		{
			punchInitPos = curTrans.position;
			punchHeightPos = punchInitPos + new Vector3(0f, punchHeight, 0f);
			targetTime = 0f;
			label.text = _num.ToString ();
			curNumState = NumState.punching_up;
		}

		void Update () 
		{
			switch(curNumState)
			{
			case NumState.punching_up:
				targetTime += Time.deltaTime * punchSpeed;
				curTrans.position = Vector3.Lerp(punchInitPos, punchHeightPos, targetTime);
				if(targetTime>=1f)
				{
					targetTime = 0f;
					curNumState = NumState.punching_down;
				}
				break;
			case NumState.punching_down:
				targetTime += Time.deltaTime * punchSpeed;
				curTrans.position = Vector3.Lerp(punchHeightPos, punchInitPos, targetTime);
				if(targetTime>=1f)
				{
					targetTime = 0f;
					curNumState = NumState.wait;
				}
				break;
			case NumState.wait:
				targetTime += Time.deltaTime;
				if(targetTime>=punchDoneWaitTimeLength)
				{
					targetTime = 0f;
					curNumState = NumState.fadeOut;
				}
				break;
			case NumState.fadeOut:
				targetTime += Time.deltaTime * fadeOutSpeed;
				if(targetTime>=1f)
				{
					curNumState = NumState.none;

					Factory.Recycle(this.transform);
				}
				break;
			default:
				break;
			}
		}
	}
}