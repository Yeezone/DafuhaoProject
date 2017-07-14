using UnityEngine;
using System.Collections;

public class UIAlphaCtrl : MonoBehaviour {

//	public		float		AlphaStart;
//	public		float		AlphaDownLimit;
//	public		float		AlphaUpLimit;
//	public		bool		AlphaUpper;
//	public		bool		AlphaPingPong;
//	public		float		TweenTime = 1f;
//	public		float		WaitTime;
//	public		float		LifeTime = 5f;

//	private		UISprite	sprite;
//	private		float		curTime;
//	private		float		curWaitTime;

	public	GameObject[]	Arrows;
	public	float			WaitTime;
	public	float			DelayTime;

	private	bool			waitState = true;
	private	float			curTime;
	private	float			curWaitTime;

	// Use this for initialization
//	void Start () {
//		curTime = 0f;
//		sprite = this.GetComponentInChildren<UISprite>();
//		sprite.alpha = AlphaStart/255f;
//	}

//	void OnEnable()
//	{
//		Start ();
//	}
	
	// Update is called once per frame
	void Update () {
		if(waitState)
		{
			curWaitTime += Time.deltaTime;
			if(curWaitTime > WaitTime)
			{
				GetComponent<TweenAlpha>().enabled = true;
				waitState = false;
				return;
			}
			return;
		}

		curTime += Time.deltaTime;
		if(curTime >= DelayTime)
		{
			curTime = 0;
			foreach(GameObject tempObj in Arrows)
			{
				if(!tempObj.activeSelf)
				{
					tempObj.SetActive(true);
					return;
				}
			}
			this.enabled = false;
		}
//		curTime += Time.deltaTime;
//		if(curTime >= LifeTime)
//		{
//			sprite.alpha -= Time.deltaTime/TweenTime*(AlphaUpLimit-AlphaDownLimit)/255f;
//			if(sprite.alpha <= 0) this.enabled = false;
//			return;	
//		}
//		if(AlphaUpper)
//		{
//			if(sprite.alpha>=AlphaUpLimit/255f)
//			{
//				if(AlphaPingPong)
//				{
//					AlphaUpper = !AlphaUpper;
//				}else{
//					sprite.alpha = AlphaDownLimit/255f;
//				}
//			}
//			sprite.alpha += Time.deltaTime/TweenTime*(AlphaUpLimit-AlphaDownLimit)/255f;
//		}else{
//			if(sprite.alpha<=AlphaDownLimit/255f)
//			{
//				if(AlphaPingPong)
//				{
//					AlphaUpper = !AlphaUpper;
//				}else{
//					sprite.alpha = AlphaUpLimit/255f;
//				}
//			}
//			sprite.alpha -= Time.deltaTime/TweenTime*(AlphaUpLimit-AlphaDownLimit)/255f;
//		}
	}
}
