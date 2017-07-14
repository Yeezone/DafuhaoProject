using UnityEngine;

public class safeBox_change_btnClick : MonoBehaviour {

	public	GameObject		oldPassInput;
	public	GameObject		newPass0Input;
	public	GameObject		newPass1Input;
	public	GameObject		log0_label;
	public	GameObject		log1_label;
	public	GameObject		log2_label;

	public	GameObject		change_btn;
	public	GameObject		close_btn;
	public	GameObject		ridio0_btn;
	public	GameObject		ridio1_btn;
	public	GameObject		toggleBtn;

	// 默认为修改安全密码(true则代表修改登录密码)
	public bool ChangeLoginPass = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		log0_label.GetComponent<UILabel>().text = "";
		log1_label.GetComponent<UILabel>().text = "";
		log2_label.GetComponent<UILabel>().text = "";
		string tempOldPass = oldPassInput.GetComponent<UIInput>().value;
		string tempNewPass0 = newPass0Input.GetComponent<UIInput>().value;
		string tempNewPass1 = newPass1Input.GetComponent<UIInput>().value;
		int passType;
		if(ChangeLoginPass)
		{
			passType = 0;//登陆密码
		}else
		{
			passType = 1;//保险柜密码
		}
		Debug.LogWarning("passType:" + passType);
		if(tempOldPass.Length < 6)
		{
			log0_label.GetComponent<UILabel>().color = Color.red;
			log0_label.GetComponent<UILabel>().text = "旧密码小于6位";
			Invoke("cleanLog", 3.0f);
		}else if(tempNewPass0.Length < 6)
		{
			log1_label.GetComponent<UILabel>().color = Color.red;
			log1_label.GetComponent<UILabel>().text = "新密码小于6位";
			Invoke("cleanLog", 3.0f);
		}else if(tempNewPass0 != tempNewPass1)
		{
			log2_label.GetComponent<UILabel>().color = Color.red;
			log2_label.GetComponent<UILabel>().text = "两次密码输入不一致";
			Invoke("cleanLog", 3.0f);
		}else{
			this.GetComponent<UIButton>().isEnabled = false;
			//this.transform.parent.parent.FindChild("btns").FindChild("1save&take_btn").gameObject.GetComponent<UIButton>().isEnabled = false;
			//this.transform.parent.parent.FindChild("btns").FindChild("3log_btn").gameObject.GetComponent<UIButton>().isEnabled = false;
			//this.transform.parent.parent.parent.FindChild("close_btn").gameObject.GetComponent<UIButton>().isEnabled = false;

//			change_btn.GetComponent<UIButton>().isEnabled = false;
//		//	close_btn.GetComponent<UIButton>().isEnabled = false;
//			ridio0_btn.GetComponent<UIButton>().isEnabled = false;
//			ridio1_btn.GetComponent<UIButton>().isEnabled = false;
//			ridio0_btn.GetComponent<showWindow>().enabled = false;
//			ridio1_btn.GetComponent<showWindow>().enabled = false;
//			toggleBtn.GetComponent<UIButton>().isEnabled = false;
//			toggleBtn.GetComponent<showWindow>().enabled = false;
			if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
			{
				HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(true);
			}

			if(HallTransfer.Instance.ncChangePassWD != null)
			{
				HallTransfer.Instance.ncChangePassWD( passType, tempOldPass, tempNewPass0);
			}

		}
		this.gameObject.GetComponent<UIButton> ().isEnabled = false;
		Invoke("onResumeBtn",3.0f);
	}
	void cleanLog()
	{
		log0_label.GetComponent<UILabel>().text = "";
		log1_label.GetComponent<UILabel>().text = "";
		log2_label.GetComponent<UILabel>().text = "";
	}
	void onResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
		{
			HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(false);
		}
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;		
	}

	// 选择当前为修改 登录密码 模式
	public void ChooseLoginPass()
	{
		ChangeLoginPass = true;
	}
	// 选择当前为修改 安全密码 模式
	public void ChooseBankPass()
	{
		ChangeLoginPass = false;
	}

}
