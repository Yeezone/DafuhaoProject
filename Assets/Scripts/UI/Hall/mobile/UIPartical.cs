using UnityEngine;
using System.Collections;

public class UIPartical : MonoBehaviour {

	public	float		LifeTime;

	private	float		curTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		curTime += Time.deltaTime;
		if(curTime >= LifeTime)
		{
			GetComponent<TweenAlpha>().PlayForward();
		}
	}

	public void DestroyObj()
	{
		Destroy(this.gameObject);
	}

}
