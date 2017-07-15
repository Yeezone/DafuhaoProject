using UnityEngine;

namespace com.QH.QPGame.Fishing
{

	public class SingleCylinder : MonoBehaviour 
	{
		private Transform curTrans;

		// private UILabel label;
		private NumItem label;
		private float targetTime;
		public float cylinderJumpHeight_Max = 100f;
		public float cylinderJumpHeight_Min = 20f;
		private float curCylinderJumpHeight;
		public float cylinderJumpSpeed = 3f;
		private Vector3 curStepPos;
		private Vector3 nextStepPos;
		
		private enum CoinState
		{
			none=0,
			up,
			down
		}
		private CoinState coinState = CoinState.none;
		public float coinJumpHeight = 60f;
		public float coinJumpSpeed = 2f;
		private float coinLerpPercent;
		private Vector3 coinStartLocalPos;
		private Vector3 coinTargetLocalPos;
		private Transform coin;

		private bool moveUp;

		void Awake () 
		{
			coin = transform.FindChild("coin");
			// label = transform.FindChild("label").GetComponent<UILabel>();
			label = transform.FindChild("label").GetComponent<NumItem>();
			curTrans = this.transform;
		}
		
		void Update () 
		{
			// coin.
			switch(coinState)
			{
			case CoinState.up:
				coinLerpPercent += Time.deltaTime*coinJumpSpeed;
				coin.localPosition = Vector3.Lerp(coinStartLocalPos, coinTargetLocalPos, coinLerpPercent);
				if(coinLerpPercent>1f)
				{
					coinLerpPercent = 0f;
					coinState = CoinState.down;
				}
				break;

			case CoinState.down:
				coinLerpPercent += Time.deltaTime*coinJumpSpeed;
				coin.localPosition = Vector3.Slerp(coinTargetLocalPos, coinStartLocalPos, coinLerpPercent);
				if(coinLerpPercent>1f)
				{
					coinState = CoinState.none;
				}
				break;

			default:
					break;
			}

			// cylinder.
			if(moveUp)
			{
				targetTime += Time.deltaTime * cylinderJumpSpeed;
				curTrans.localPosition = Vector3.Lerp(curStepPos, nextStepPos, targetTime);

				if(targetTime>1f)
				{
					moveUp = false;
					targetTime = 0f;
				}
			}
		}

		public void Recycle()
		{
			Factory.Recycle(curTrans);
			moveUp = false;
		}


		void CacualteJumpHeight(int _multi)
		{
			if(_multi<10)
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(0f,20f,(_multi/10f));
			}
			else if(_multi>=10 && _multi<50) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(20f,30f,(_multi-10)/40f);
			}
			else if(_multi>=50 && _multi<100) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(30f,40f,(_multi-50)/50f);
			}
			else if(_multi>=100 && _multi<200) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(40f,50f,(_multi-100)/100f);
			}
			else if(_multi>=200 && _multi<300) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(50f,60f,(_multi-200)/100f);
			}
			else if(_multi>=300 && _multi<500) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Min + Mathf.Lerp(60f,70f,(_multi-300)/200f);
			}
			else if(_multi>=500) 
			{
				curCylinderJumpHeight = cylinderJumpHeight_Max;
			}
		}

		public void Init(int _multi, int _value, int _dir)
		{
			targetTime = 0f;
			// label.text = _value.ToString();
			label.ApplyValue(_value,0);
			curStepPos = curTrans.localPosition;
			// nextStepPos = curTrans.localPosition + new Vector3(0f, cylinderJumpHeight_Max, 0f);
			CacualteJumpHeight(_multi);
			nextStepPos = curTrans.localPosition + new Vector3(0f, curCylinderJumpHeight, 0f);
			if(_dir==2)
			{
				label.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			}
			else 
			{
				label.transform.localEulerAngles = Vector3.zero;
			}

			coinState = CoinState.up;
			coinLerpPercent = 0f;
			coinStartLocalPos = coin.localPosition;
			if(_dir==2)
			{
				coinTargetLocalPos = coinStartLocalPos + new Vector3(0f, coinJumpHeight, 0f);
			}
			else 
			{
				coinTargetLocalPos = coinStartLocalPos + new Vector3(0f, coinJumpHeight, 0f);
			}

			moveUp = true;
		}
	}
}
