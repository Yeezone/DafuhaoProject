using UnityEngine;

public class Regist_btnClick : MonoBehaviour {

	public	GameObject		userIDInput;     //账号
	public	GameObject		passWordInput;	 //密码
	public	GameObject		passWordAgain;	 //密码确认
	public	GameObject		gender;			 //性别
	public	GameObject		introducer;		 //推荐人
	public	GameObject		log_userid;
	public	GameObject		log_password;           

	void Awake () {
		UIEventListener.Get (userIDInput).onSelect = passwordEvent;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnClick () 
	{
		Debug.LogWarning("Regist_btnClick~~~~~");

//		this.transform.FindChild ("GameObject").GetComponent<Animation> ().Play (0);
		if (!LoginTransfer.Instance.isRegisted)// && LoginTransfer.Instance.userIdValid) 
		{	
//			string tempPass = passWordAgain.GetComponent<UIInput> ().value;
			uint temp = 0;

//			if(tempPass != passWordInput.GetComponent<UIInput> ().value)
//			{
//				log_password.gameObject.SetActive(true);
//				log_password.GetComponent<UILabel> ().text = "两次输入的密码不一致";
//				Invoke("cleanIntroducerLabel", 4.0f);
//			}
//			else
			//{
				Debug.LogWarning("Regist pass~~~");
				LoginTransfer.UserRegistMsg tempMsg  = new LoginTransfer.UserRegistMsg();
				
				tempMsg.userID = userIDInput.GetComponent<UIInput> ().value;
				tempMsg.userPassword = passWordInput.GetComponent<UIInput> ().value;

				if(tempMsg.userID.Length < 6)
				{
					log_userid.SetActive (true);
					log_userid.transform.FindChild("Logo_true").gameObject.SetActive (false);
					log_userid.GetComponent<UILabel> ().text = "账号长度不应小于6位!";
					Invoke("cleanRegistLabel",4.0f);
					return;
				}
				else if(tempMsg.userPassword.Length < 6)
				{
					log_password.SetActive(true);
					log_password.GetComponent<UILabel> ().text = "密码长度不应小于6位!";
					Invoke("cleanIntroducerLabel", 4.0f);
					return;
				}
				//注册成功
				LoginTransfer.Instance.isRegisted = true;
	            //Debug.LogWarning("性别："+gender.transform.FindChild("male").GetComponent<UIToggle>().value);
				if(!gender.transform.FindChild("male").GetComponent<UIToggle>().value && !gender.transform.FindChild("female").GetComponent<UIToggle>().value)
				{
					temp = (uint)Random.Range (0, 10);
					if(temp>5)
					{
						tempMsg.Gender = 1; //女
					}else
					{
						tempMsg.Gender = 0; //男
					}
				}
				else if(gender.transform.FindChild("male").GetComponent<UIToggle>().value)
				{ 
					tempMsg.Gender = 0; 
				}else
				{
					tempMsg.Gender = 1; 
				}
				//Debug.LogWarning("性别："+tempMsg.Gender);
				tempMsg.Introducer = introducer.GetComponent<UIInput>().value;
				
				if(LoginTransfer.Instance.ncUserRegistSubmit != null)
				{
					LoginTransfer.Instance.ncUserRegistSubmit(tempMsg);
				}
			//}

		}

	}

	private void cleanIntroducerLabel()
	{
		log_password.GetComponent<UILabel> ().text = "";
		log_password.gameObject.SetActive(false);
	}
	private void cleanRegistLabel()
	{
		log_userid.GetComponent<UILabel> ().text = "";
		log_userid.gameObject.SetActive(false);
	}

	public void passwordEvent( GameObject obj , bool state)
	{
		if (state) {
			LoginTransfer.Instance.inputSelected = true;
		}
		else if(LoginTransfer.Instance.inputSelected)
		{		
			LoginTransfer.Instance.inputSelected = false;
			if(userIDInput.GetComponent<UIInput>().value != "" && LoginTransfer.Instance.tempID != userIDInput.GetComponent<UIInput>().value)
			{
				string tempName = LoginTransfer.Instance.tempID = userIDInput.GetComponent<UIInput>().value;
				VerifyUserId(tempName);
			}
		}
	}

	public void VerifyUserId(string tempId)
	{	
		LoginTransfer.Instance.userIdValid = false;
		log_userid.GetComponent<UILabel> ().text = "";
		log_userid.transform.FindChild ("Logo_true").gameObject.SetActive (false);

		if (LoginTransfer.Instance.ncUserVerify != null) 
		{
			LoginTransfer.Instance.ncUserVerify(tempId);
		}

//		LoginTransfer.Instance.cnRegistVerityMsg (0);

	}
}
