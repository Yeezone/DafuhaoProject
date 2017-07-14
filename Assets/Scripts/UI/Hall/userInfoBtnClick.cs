using UnityEngine;

public class userInfoBtnClick : MonoBehaviour {

	public GameObject waiting;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () 
	{
//		if (waiting != null) {
//			waiting.SetActive(true);
//		}
		//发送资料请求
		if (HallTransfer.Instance.ncUserInformationRequest != null) {
			HallTransfer.Instance.msgTooLate_UIF = true;
			HallTransfer.Instance.canExecuteUIF = true;
			HallTransfer.Instance.ncUserInformationRequest();		
		}

		//this.gameObject.GetComponent<UIButton> ().isEnabled = false;
//		Invoke("onResumeBtn",5.0f);
	}

	void onResumeBtn()
	{
		//waiting.SetActive(false);
		//this.gameObject.GetComponent<UIButton> ().isEnabled = true;
		if (waiting != null) {
			waiting.SetActive(false);
		}
		if (HallTransfer.Instance.msgTooLate_UIF ) 
		{
			HallTransfer.Instance.canExecuteUIF = false;
			HallTransfer.Instance.msgTooLate_UIF = false;
			HallTransfer.Instance.cnMsgBox ("请求超时,请稍候再试");		
		}

	}
}
