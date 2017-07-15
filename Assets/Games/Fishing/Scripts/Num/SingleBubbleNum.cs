using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleBubbleNum : MonoBehaviour 
	{

		private Transform curTrans;
		private enum NumState
		{
			none=0,
			showUp,
			moveSlow,
			fadeOut
		}
		private NumState curNumState = NumState.none;
		private UILabel mulLabel;
//		private NumItem mulLabel;
		private UILabel valueLabel;
//		private NumItem valueLabel;
		public float showUpSpeed = 2f;
		public float showUpTimeLength = 0.3f;

		public float moveUpTimeSpeed = 1f;
		public float moveUpTimeLength = 0.5f;

		private float targetTime = 0f;
		public float fadeOutSpeed = 0.75f;

		//private UISprite mulSprite;
		//private UISprite valueSprite;

		private Color mulSpriteColor;
		private Color valueSpriteColor;
		private Color mulLabelColor;
		private Color valueLabelColor;

		private int dir = 1;


		void Awake()
		{
			curTrans = this.transform;

			Transform mulTrans   = curTrans.FindChild("mul");
			Transform valueTrans = curTrans.FindChild("value");

			mulLabel 	= mulTrans.FindChild("Label").GetComponent<UILabel>();
			valueLabel 	= valueTrans.FindChild("Label").GetComponent<UILabel>();

			//mulSprite 	= mulTrans.GetComponent<UISprite>();
			//valueSprite = valueTrans.GetComponent<UISprite>();

			//mulLabelColor = mulLabel.color;
			//valueLabelColor = valueLabel.color;

			//mulSpriteColor = mulSprite.color;
			//valueSpriteColor = valueSprite.color;
		}

		public void Init(int _mul, int _value, bool _upsideDown) 
		{
			mulLabel.text = _mul.ToString();
			valueLabel.text = _value.ToString();
			
			if(_upsideDown)
			{
				// 如果玩家炮台需要旋转,则调整泡泡运动方向
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
					dir = 1;
				}else{
					dir = -1;
				}			
			}
			else 
			{
				// 如果玩家炮台需要旋转,则调整泡泡运动方向
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
					dir = -1;
				}else{
					dir = 1;
				}
			}

			targetTime = 0f;
			curNumState = NumState.showUp;

			// init color.
			//mulLabel.color = mulLabelColor;
			//valueLabel.color = valueLabelColor;
			
			//mulSprite.color = mulSpriteColor;
		//	valueSprite.color = valueSpriteColor;
		}
		
		void Update ()
		{
			switch(curNumState)
			{
			case NumState.showUp:
				float delta = Time.deltaTime * showUpSpeed * dir;
				curTrans.position += new Vector3(0f,delta,0f);

				targetTime += Time.deltaTime;
				if(targetTime>=showUpTimeLength)
				{
					moveUpTimeLength = 0f;
					curNumState = NumState.moveSlow;
				}
				break;

			case NumState.moveSlow:
				delta = Time.deltaTime * moveUpTimeSpeed * dir;
				curTrans.position += new Vector3(0f,delta,0f);
				
				targetTime += Time.deltaTime;
				// if(moveUpTimeLength>=moveUpTimeLength)
				if(targetTime>=moveUpTimeLength)
				{
					targetTime = 1f;
					curNumState = NumState.fadeOut;
				}
				break;

			case NumState.fadeOut:

				targetTime -= Time.deltaTime * fadeOutSpeed;

				delta = Time.deltaTime * moveUpTimeSpeed * dir;
				curTrans.position += new Vector3(0f,delta,0f);

				//mulLabel.color = new Color(mulLabelColor.r, mulLabelColor.g, mulLabelColor.b, targetTime);
				//valueLabel.color = new Color(valueLabelColor.r, valueLabelColor.g, valueLabelColor.b, targetTime);

				// mulSprite.color = new Color(mulSpriteColor.r, mulSpriteColor.g, mulSpriteColor.b, targetTime);
				// valueSprite.color = new Color(valueSpriteColor.r, valueSpriteColor.g, valueSpriteColor.b, targetTime);
				if(targetTime<=0f)
				{
					Factory.Recycle(this.transform);
					curNumState = NumState.none;
				}
				break;
			default:
				break;
			}
		}
	}
}
