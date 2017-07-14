using UnityEngine;
using System.Collections;

public class noticeMarquee : MonoBehaviour {

	public GameObject content;
	
	Vector3 	vec;
	float 		begin;
	string 		msg;

	bool canMove = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
//		content.GetComponent<TweenPosition> ().from.y = this.transform.localPosition.y + this.gameObject.GetComponent<UISprite> ().height;
//		content.GetComponent<TweenPosition> ().to.y = this.transform.localPosition.y ;
		content.GetComponent<TweenPosition> ().from.x = this.transform.localPosition.x + this.gameObject.GetComponent<UISprite> ().width;
		content.GetComponent<TweenPosition> ().to.x = this.transform.localPosition.x - content.GetComponent<UILabel> ().width;
	
		Debug.LogWarning ("anMove:"+canMove);
		if (canMove) 
		{
			if (Mathf.Abs(content.transform.localPosition.x -this.transform.localPosition.x)<10) 
			{
				canMove = false;
				content.GetComponent<TweenPosition> ().enabled = false;
				vec = content.transform.position;
				StartCoroutine(onRePlay(2));
			}
		}

//		if ( this.transform.localPosition.x - content.transform.localPosition.x > 20 ) {
//			canMove = true;
//		}

	}

	IEnumerator onRePlay(int interval)
	{
		yield return new WaitForSeconds (interval);
		content.GetComponent<TweenPosition> ().enabled = true;
		//vec.x -= 8;
		content.transform.position = vec;

		yield return new WaitForSeconds (1);
		canMove = true;
	}


}
