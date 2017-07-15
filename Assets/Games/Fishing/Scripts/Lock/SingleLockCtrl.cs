using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	[RequireComponent(typeof(AudioSource))]
	public class SingleLockCtrl : MonoBehaviour
	{
		private AudioSource audios;
		private UISprite lineRend;
		[HideInInspector] public Transform startAnchor;
		[HideInInspector] public Transform lockFish;
		private bool startLock = false;
		private SingleFish lockFishScript;
		private Transform number;

		void Awake () 
		{
			lineRend = GetComponent<UISprite>();
			audios = GetComponent<AudioSource>();
			audios.playOnAwake = false;
			audios.loop = true;
		}

		// 初始化锁定线.
		public void Init(Transform _startAnchor, Transform _number)
		{
			// 锁定线初始位置的锚点.
			startAnchor = _startAnchor;
			number = _number;
			number.GetComponent<UISprite>().enabled = false;
			startLock = true;
		}
		
		void Update () 
		{
			// 没有锁定.
			if(!startLock)
			{
				return;
			}
			
			// 没有鱼锁定.
			if(lockFish==null || startAnchor==null )
			{
				StopLock();
				return;
			}
			
			// have fish in lock, but fish is dead, or fish move out of viewport.
			//　有鱼被锁定，但是这条鱼没被锁定（死亡，游出了屏幕外面）.
			if(lockFishScript!=null && !lockFishScript.locking)
			{
				StopLock();
				return;
			}

			// 锁定线的起始位置.
//			Vector3 _startPos 	= startAnchor.position;
//			lineRend.SetPosition(0, _startPos);
			
			// 锁定线的末尾位置.
			Vector3 _endPos = lockFish.position;
//			lineRend.SetPosition(1, _endPos);
                   
            Vector3 temp = lockFish.position - startAnchor.position;
            temp.x = temp.x / gameObject.transform.parent.localScale.x / gameObject.transform.parent.parent.localScale.x / gameObject.transform.parent.parent.parent.localScale.x / gameObject.transform.parent.parent.parent.parent.localScale.x;
            temp.y = temp.y / gameObject.transform.parent.localScale.y / gameObject.transform.parent.parent.localScale.y / gameObject.transform.parent.parent.parent.localScale.y / gameObject.transform.parent.parent.parent.parent.localScale.y;
            
			float longer =Mathf.Sqrt((temp.x * temp.x )+(temp.y * temp.y )) ;
            gameObject.GetComponent<UISprite>().height = (int)(longer);


			// 锁定线的数字位置.
			number.position = _endPos;
		}

		// 显示锁定线.
		public void ShowLockRend(Transform _lockFish)
		{
			// 标志锁定状态.
			startLock = true;

			// 获取锁定的鱼.
			lockFish = _lockFish;
			lockFishScript = lockFish.GetComponent<SingleFish>();

			// 显示锁定线.
			lineRend.enabled = true;

			// 显示数字.
			number.GetComponent<UISprite>().enabled = true;

			// 播放音效.
			if(audios.clip!=null)
			{
				audios.Play();
			}
		}

		//　停止锁定.
		public void StopLock()
		{
			startLock = false;
			
			lockFish = null;
			lockFishScript = null;
			
			lineRend.enabled = false;
			number.GetComponent<UISprite>().enabled = false;

			if(audios.clip!=null)
			{
				audios.Stop();
			}
		}
	}
}