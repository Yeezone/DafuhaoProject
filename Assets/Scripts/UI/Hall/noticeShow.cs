using UnityEngine;

public class noticeShow : MonoBehaviour {

	public GameObject content;

	Vector3 	vec;
	float 		begin;
	string 		msg;

	//UIRoot root;

	void Awake () {
		UIEventListener.Get (this.gameObject).onHover = noticePause;
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		content.GetComponent<TweenPosition> ().from.y = this.transform.localPosition.y - this.gameObject.GetComponent<UISprite> ().height;
		content.GetComponent<TweenPosition> ().to.y = this.transform.localPosition.y + content.GetComponent<UILabel> ().height;
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
