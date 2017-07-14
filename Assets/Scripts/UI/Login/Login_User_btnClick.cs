using UnityEngine;

public class Login_User_btnClick : MonoBehaviour {

	public	GameObject		userNameInput;
	public	GameObject		passWordInput;
	public	GameObject		savePass;
	public	GameObject		log_lable;
	
	// Use this for initialization
	void Start () 
	{
//		userNameInput.GetComponent<UIInput>().value = "boss01";
//		passWordInput.GetComponent<UIInput>().value = "123456";
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
	
	public void OnClick()
	{
		Debug.LogWarning("Login_User_btnClick~~~");
		string user = userNameInput.GetComponent<UIInput>().value;
		string pass = passWordInput.GetComponent<UIInput>().value;
		bool save = savePass.GetComponent<UIToggle>().value;

		if(user=="") 
		{
			LoginTransfer.Instance.clearnLoginMask();
			log_lable.GetComponent<UILabel>().text = "请输入账号";
			passWordInput.GetComponent<UIInput> ().value = "";
			Invoke("cleanLogLabel",3.0f);
		}
		else if(pass.Length < 6)
		{
			LoginTransfer.Instance.clearnLoginMask();
			Debug.LogWarning("pass.Length < 6");
			log_lable.GetComponent<UILabel>().text = "密码不应少于6位！";
			Invoke("cleanLogLabel",3.0f);

		}else{
			//this.GetComponent<UIButton>().isEnabled = false;

			if(!LoginTransfer.Instance.islogged)
			{
				LoginTransfer.Instance.islogged = true;
				if(LoginTransfer.Instance.ncLoginBtnClick != null)
				{
					LoginTransfer.Instance.cnConnectingEvent();
					LoginTransfer.Instance.ncLoginBtnClick(user, pass, save);
				}else{
					LoginTransfer.Instance.clearnLoginMask();
				}
			}

		}
	}

	private void cleanLogLabel()
	{
		log_lable.GetComponent<UILabel>().text = "";
	}

//	private void cleanPassWord()
//	{
//		
//	}

}
