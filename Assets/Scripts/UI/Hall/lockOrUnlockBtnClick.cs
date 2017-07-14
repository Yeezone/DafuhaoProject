using UnityEngine;
using System;

public class lockOrUnlockBtnClick : MonoBehaviour {

	public	GameObject		passInput;
	public	GameObject		log_label;
	public	GameObject		tempLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	void OnClick() 
	{
		log_label.GetComponent<UILabel>().text = "";
		UInt32 tempType = 1;
		if(HallTransfer.Instance.uiConfig.LockOrUnLockAccount) tempType = 0;
		string tempPass = passInput.GetComponent<UIInput>().value;
		if(tempPass.Length < 6)
		{
			log_label.GetComponent<UILabel>().color = Color.red;
			log_label.GetComponent<UILabel>().text = "密码小于6位";
			Invoke ("clearLogLabel",3.0f);
		}
		else 
		{
//			this.GetComponent<UIButton>().isEnabled = false;
//			this.transform.parent.FindChild("cancel_btn").GetComponent<UIButton>().isEnabled = false;
//			this.transform.parent.FindChild("close_btn").GetComponent<UIButton>().isEnabled = false;

			if(HallTransfer.Instance.uiConfig.window_Lock_mask != null)
			{
				HallTransfer.Instance.uiConfig.window_Lock_mask.SetActive(true);
			}

			if(HallTransfer.Instance.ncLockOrUnLockAccount != null)
			{
				HallTransfer.Instance.ncLockOrUnLockAccount(tempPass,tempType);
				this.gameObject.GetComponent<UIButton> ().isEnabled = false;
				Invoke ("resumeBtn",5.0f);
			}
		}

	}

	public	void	clearLogLabel()
	{
		log_label.GetComponent<UILabel>().text = "";
	}

	public	void	clearTempLabel()
	{
		tempLabel.GetComponent<UILabel>().text = "";
	}

	public	void	restoreTempLabel()
	{
		tempLabel.GetComponent<UILabel>().text = "初始密码为登录密码";
	}

	public	void	resumeBtn()
	{
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;
		if(HallTransfer.Instance.uiConfig.window_Lock_mask != null
		   && HallTransfer.Instance.uiConfig.window_Lock_mask.activeSelf)
		{
			HallTransfer.Instance.uiConfig.window_Lock_mask.SetActive(false);
			HallTransfer.Instance.cnMsgBox ("响应超时");
		}

//		this.transform.parent.FindChild("cancel_btn").GetComponent<UIButton>().isEnabled = true;
//		this.transform.parent.FindChild("close_btn").GetComponent<UIButton>().isEnabled = true;
	}
}
