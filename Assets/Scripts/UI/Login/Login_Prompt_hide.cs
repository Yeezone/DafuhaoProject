using UnityEngine;

public class Login_Prompt_hide : MonoBehaviour
{

	public float closeTime;//窗口关闭的时间
	private float curTime;
	public Transform obj;
	private bool showInterface;//界面显示与关闭
	public UILabel showTimeLabel;//时间Label

	void Awake ()
	{
		obj = this.transform;
		curTime = 0;
		showInterface = false;
	}

	void OnEnable ()
	{
		Invoke ("Hide", closeTime);
		showInterface = true;
	}

	void Update ()
	{
		if (showInterface) {	
			curTime += Time.deltaTime;
			showTimeLabel.text = ((int)(closeTime - curTime)).ToString () + "秒后关闭弹窗";
		}
	}

	void Hide ()
	{
		obj.gameObject.SetActive (false);
		this.enabled = false;
		curTime = 0;
	}

	void OnDisable ()
	{
		showInterface = false;
	}

}
