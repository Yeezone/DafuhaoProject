using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleShadow : MonoBehaviour
	{
		private Transform curTransform;
		// 鱼的位置的偏移量.
		public Vector2 offset = new Vector2(50f, 50f);
		public Vector3 localScale = new Vector3(0.75f, 0.75f, 0f);
		private Navigation navigation;
		private Animator [] m_animator;
		private int deadHash; 
		public bool lastMap2ClearFish;
		private float lerpPercent;
		private bool killByBlackHall;
		private Transform blackHall;
		
		void Awake()
		{
			curTransform = this.transform;
			m_animator = GetComponentsInChildren<Animator>();
			deadHash = Animator.StringToHash("dead");
			navigation = new Navigation();
			localScale = new Vector3(localScale.x*Utility.device_to_1136x640_ratio.x, localScale.y*Utility.device_to_1136x640_ratio.x, localScale.z);

            gameObject.GetComponent<UISprite>().depth = -2;
		}
		
		void OnEnable()
		{
			lastMap2ClearFish = false;
			lerpPercent = 0f;
			killByBlackHall = false;
			blackHall = null;
		
			qp_lerp = false;
			lerpPercent = 0f;
		}

		public void InitShadowParam(NavPath _nav)
		{
			iTween.ScaleTo(gameObject, localScale, 1f);
			NavPath _path = PathCtrl.Instance.GetShadowPath(_nav, offset);
			navigation.init(transform, _path);
		}

		void Update()
		{
			navigation.update();
			AbosorbByBlackHallEff();
			LerpShadow2Center();
		}
		
		public void KillByBullet()
		{
			for(int i=0; i<m_animator.Length; i++)
			{
				if(gameObject.activeSelf)
				{
					m_animator[i].SetTrigger(deadHash);
				}
			}
			navigation.forcePathEnd();
		}

		public void AbosorbByBlackHall(Transform _blackHall)
		{
			KillByBullet();

			blackHall = _blackHall;
			killByBlackHall = true;
		}

		void AbosorbByBlackHallEff()
		{
			if(killByBlackHall)
			{
				curTransform.localEulerAngles += new Vector3(0f, 0f, 1f);
				lerpPercent += Time.deltaTime * 0.5f;
				curTransform.localScale = Vector3.Lerp(curTransform.localScale, Vector3.zero, lerpPercent);
				if(blackHall!=null)
				{
					curTransform.position = Vector3.Lerp(curTransform.position, blackHall.position, lerpPercent);
				}
			}
		}

		private float lerpSpeed;
		private bool qp_lerp;
		private Vector3 qp_lerp_startPos;
		public void StartLerpShadow2Center(float _timeLength)
		{
			qp_lerp_startPos = curTransform.position;
			lerpSpeed = 1f/_timeLength;
			qp_lerp = true;
		}

		void LerpShadow2Center()
		{
			if(qp_lerp)
			{
				lerpPercent += Time.deltaTime * lerpSpeed;
				curTransform.position = Vector3.Lerp(qp_lerp_startPos, Vector3.zero, lerpPercent);
				if(lerpPercent>1f)
				{
					qp_lerp = false;
				}
			}
		}
	}
}