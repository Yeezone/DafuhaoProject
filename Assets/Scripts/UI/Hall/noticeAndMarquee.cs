using UnityEngine;

public class noticeAndMarquee : MonoBehaviour {

	public GameObject content;
	
	Vector3 	vec;
	float 		begin;
	string 		msg;
	
	//UIRoot root;
	
	void Awake () {
		UIEventListener.Get (this.gameObject).onHover = noticePause;
		//UIEventListener.Get (this.gameObject).
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		content.GetComponent<TweenPosition> ().from.x = this.transform.localPosition.x + this.gameObject.GetComponent<UISprite> ().width;
		content.GetComponent<TweenPosition> ().to.x = this.transform.localPosition.x - content.GetComponent<UILabel> ().width;		
	}
	
	void noticePause(GameObject notice, bool isHover)
	{	
		if (isHover) 
		{
			
			content.GetComponent<TweenPosition> ().enabled = false;
			vec = content.transform.position;
		}else {
			content.GetComponent<TweenPosition> ().enabled  = true;
			//content.GetComponent<TweenPosition> ().ResetToBeginning();
			content.transform.position = vec;
		}
	}

}
