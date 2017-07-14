using UnityEngine;
using System;
using com.QH.QPGame.Lobby.Surfaces;

public class LoginTransfer : MonoBehaviour
{


	public struct UserRegistMsg
	{
		public	string	userID;			//用户帐户
		public	string	userPassword; 	//登录密码
		public	byte	Gender;			//性别 男0,女1
		public	string	Introducer;		//推荐人
	}
	
	public	GameObject		userNameInput;
	public	GameObject		passWordInput;
	public	GameObject		savePass;
	public	GameObject		msgBox;
	public	GameObject		log_msg;
	public	GameObject		log_lable;
	public	GameObject		log_userid;
	public	GameObject		log_Introducer;
	public	GameObject		version_label;
	public	GameObject		window_register;
	public	bool            islogged;
	public	bool            isRegisted;
	public  bool            userIdValid; 	//注册账号有效
	public  bool            inputSelected;
	public  bool            isQuitGame;
	public	bool			isMobileEdition;
	public  string          tempID;

	private GameObject		MsgBox_value_label;
	private	Transform		LoginPanel;
	private MessageBoxPopup	MsgBox;
	// Use this for initialization
	void Start ()
	{
        LoginPanel = GameObject.Find("LoginPanel").transform;
		LoginPanel.FindChild ("LoginBtn").GetComponent<UIButton> ().isEnabled = false;
		LoginPanel.FindChild ("GuestBtn").GetComponent<UIButton> ().isEnabled = false;
		LoginPanel.FindChild ("InputAccount").FindChild ("InputField").GetComponent<UIInput> ().enabled = false;
		LoginPanel.FindChild ("InputPassword").FindChild ("PasswordField").GetComponent<UIInput> ().enabled = false;
//		tranObj.FindChild ("LoginBtn").FindChild ("loading_Anim").gameObject.SetActive (true);

		//this.transform.FindChild ("LoginPanel").FindChild ("GuestBtn").FindChild ("regist_Anim").gameObject.SetActive (true);

		//log_lable.GetComponent<UILabel> ().text = "正在连接服务器...";

		MsgBox_value_label = msgBox.transform.FindChild("front_panel").FindChild("value_label").gameObject;
    }


	public static LoginTransfer _instance = null;
	
	public static LoginTransfer Instance {
		get {
			if (_instance == null) {
                _instance = UIRoot.list[0].gameObject.GetComponentInChildren<LoginTransfer>();
			}
			return _instance;
		}
	}

	void OnDestroy()
	{
		_instance = null;
	}


	#region LoginReturnMsg

	public void cnLoginMsg (Int32 wHandleCode, string msg)
	{
		Debug.LogWarning ("cnLoginMsg-----------------:" + wHandleCode + "___" + msg);
		islogged = false;
		LoginTransfer.Instance.isRegisted = false;
		Transform reglog_lable = window_register.transform.FindChild ("font_panel").FindChild ("Content").FindChild ("Log_userID");
		if (!window_register.activeSelf) {
			if (!string.IsNullOrEmpty (msg)) {
				MsgBoxShow(0,msg);
			} else {
				MsgBoxShow(3,"用户名密码错误");
			}
		} else {
			if (msg != "") {
				MsgBoxShow(0,msg);
			} else {
				switch (wHandleCode) {
				case	1:
//					reglog_lable.GetComponent<UILabel> ().text = "注册暂停";
//					Invoke ("cleanLogLabel", 3.0f);
					MsgBoxShow(0,"注册暂停");
					break;
				case	4:
//					reglog_lable.GetComponent<UILabel> ().text = "账号格式不正确";
					//					Invoke ("cleanLogLabel", 3.0f);
					MsgBoxShow(0,"账号格式不正确");
					break;
				case	5:
//					reglog_lable.GetComponent<UILabel> ().text = "机器被禁止注册";
					//					Invoke ("cleanLogLabel", 3.0f);
					MsgBoxShow(0,"机器被禁止注册");
					break;
				case	7:
//					reglog_lable.GetComponent<UILabel> ().text = "账号已存在";
					//					Invoke ("cleanLogLabel", 3.0f);
					MsgBoxShow(0,"账号已存在");
					break;
				case	8:
//					reglog_lable.GetComponent<UILabel> ().text = "推荐人不存在";
//					Invoke ("cleanLogLabel", 3.0f);
					MsgBoxShow(0,"推荐人不存在");
					break;
				}
			}
		}
	}
	
	public	void	cnConnectingEvent ()
	{
		LoginPanel.FindChild ("LoginBtn").FindChild ("loading_Anim").gameObject.SetActive (true);
		LoginPanel.FindChild ("LoginBtn").GetComponent<UIButton> ().isEnabled = false;
		LoginPanel.FindChild ("GuestBtn").GetComponent<UIButton> ().isEnabled = false;
		LoginPanel.FindChild ("InputAccount").FindChild ("InputField").GetComponent<UIInput> ().enabled = false;
		LoginPanel.FindChild ("InputPassword").FindChild ("PasswordField").GetComponent<UIInput> ().enabled = false;
	}

	public	void	clearnLoginMask()
	{
		LoginPanel.FindChild ("LoginBtn").FindChild ("loading_Anim").gameObject.SetActive (false);
		LoginPanel.FindChild ("LoginBtn").GetComponent<UIButton> ().isEnabled = true;
		LoginPanel.FindChild ("GuestBtn").GetComponent<UIButton> ().isEnabled = true;
		LoginPanel.FindChild ("InputAccount").FindChild ("InputField").GetComponent<UIInput> ().enabled = true;
		LoginPanel.FindChild ("InputPassword").FindChild ("PasswordField").GetComponent<UIInput> ().enabled = true;
	}

	public void cnNetworkError (Int32 wError)
	{
		isQuitGame = false;
		Debug.LogWarning ("cnLoginMsg-----------------:" + wError.ToString ());
		islogged = false;
		clearnLoginMask();

		switch (wError) {
		case	0:
				
			log_lable.GetComponent<UILabel> ().text = "连接成功";
			Invoke ("cleanLogLabel", 3.0f);
			break;
		case	1:
			log_lable.GetComponent<UILabel> ().text = "连接超时";
			Invoke ("cleanLogLabel", 3.0f);
			MsgBoxShow(1,"连接超时,请重新连接!");
			break;
		case	2:
			log_lable.GetComponent<UILabel> ().text = "连接错误";
			Invoke ("cleanLogLabel", 3.0f);
			MsgBoxShow(1,"连接错误,请重新连接!");
			break;
        case 3:
            log_lable.GetComponent<UILabel>().text = "连接失败";
            Invoke("cleanLogLabel", 999.0f);
            break;
        case 4:
            log_lable.GetComponent<UILabel>().text = "";
            Invoke("cleanLogLabel", 999.0f);
            break;
		}
	}

	public void MsgBoxShow(int msgType,string msg)
	{
		//=====================================================
		clearnLoginMask();
		//=====================================================
		MsgBox_value_label.GetComponent<UILabel>().text = msg;
		msgBox.GetComponent<Login_Prompt_hide>().enabled = false;

		if(msgType == -1)
		{
			msgBox.SetActive(false);
		}else
		{
			msgBox.SetActive(true);

            Transform msgBoxParent = MsgBox_value_label.transform.parent;
            msgBoxParent.FindChild("Close_btn").gameObject.SetActive(false);
			msgBoxParent.FindChild("Cancel_btn").gameObject.SetActive(false);
			msgBoxParent.FindChild("Retry_btn").gameObject.SetActive(false);
			msgBoxParent.FindChild("Confirm_btn").gameObject.SetActive(false);
			msgBoxParent.FindChild("ConfirmQuit_btn").gameObject.SetActive(false);
			switch (msgType)
			{
			case	0:
				//正常消息框
				msgBoxParent.FindChild("Close_btn").gameObject.SetActive(true);
				msgBoxParent.FindChild("Confirm_btn").gameObject.SetActive(true);
				break;
			case	1:
				//连接超时,重新连接框
				msgBoxParent.FindChild("Cancel_btn").gameObject.SetActive(true);
				msgBoxParent.FindChild("Retry_btn").gameObject.SetActive(true);
				break;
			case	2:
				//确认退出框
				msgBoxParent.FindChild("Close_btn").gameObject.SetActive(true);
				msgBoxParent.FindChild("ConfirmQuit_btn").gameObject.SetActive(true);
				break;
			case	3:
				//倒计时框
				msgBox.GetComponent<Login_Prompt_hide>().enabled = true;
				break;
			}
		}
	}
	private void cleanLogLabel ()
	{
		log_lable.GetComponent<UILabel> ().text = "";
	}

	#endregion	
	
	#region LoginReturnData
	public void cnUserLoginMsg (string userName, string passWord, string version, bool save, bool showServerConfig)
	{
		userNameInput.GetComponent<UIInput> ().value = userName;
		passWordInput.GetComponent<UIInput> ().value = passWord;
		if(isMobileEdition && userName == "")
		{
			savePass.GetComponent<UIToggle> ().value = true;
		}else{
			savePass.GetComponent<UIToggle> ().value = save;
		}

		version_label.GetComponent<UILabel>().text = "版本号:" + version;
	}
	#endregion	


	#region 注册登录成功事件

	public void cnLoginSuccessEvent ()
	{
		if (window_register.activeSelf)
			window_register.SetActive (false);
	}
	#endregion	


	#region RegisterMsg

	public void cnRegistVerityMsg (Int32 wHandleCode)
	{
		Debug.LogWarning ("cnRegistVerityMsg-----------------:" + wHandleCode.ToString ());

		log_userid.gameObject.SetActive (true);

		if (wHandleCode == 0) {
			Debug.LogWarning ("验证通过");
			userIdValid = true;
			log_userid.GetComponent<UILabel> ().text = "";
			//暂时注释下面划勾代码
			//log_userid.transform.FindChild ("Logo_true").gameObject.SetActive (true);
		} else {
			Debug.LogWarning ("用户名重复");
			log_userid.GetComponent<UILabel> ().text = "用户名重复";
			Invoke ("cleanRegistLabel", 3.0f);
		}
	}

	public void cnRegistSubmitMsg (Int32 wHandleCode)
	{
		Debug.LogWarning ("cnRegistSubmit-----------------:" + wHandleCode.ToString ());
		LoginTransfer.Instance.isRegisted = false;

		//20150909 优化逻辑
		if (wHandleCode == 0) {
			Debug.LogWarning ("注册成功");
		} else {
			log_userid.gameObject.SetActive (true);
			switch (wHandleCode) {
			case	1:
//				Debug.LogWarning ("用户名重复");
//				log_userid.GetComponent<UILabel> ().text = "用户名重复";
//				Invoke ("cleanRegistLabel", 3.0f);	
				MsgBoxShow(0,"用户名重复");
				break;
			case	2:
//				Debug.LogWarning ("用户名为空");
//				log_userid.GetComponent<UILabel> ().text = "用户名为空";
//				Invoke ("cleanRegistLabel", 3.0f);
				MsgBoxShow(0,"用户名为空");
				break;
			case	3:
//				Debug.LogWarning ("密码为空");
				MsgBoxShow(0,"密码为空");
				break;
			case	7:
//				Debug.LogWarning ("推荐人不存在");
//				log_Introducer.GetComponent<UILabel> ().text = "推荐人不存在";
//				Invoke ("cleanIntroducerLabel", 3.0f);
				MsgBoxShow(0,"推荐人不存在");
				break;
			case	9:
//				Debug.LogWarning ("玩家账号已存在");
//				log_userid.GetComponent<UILabel> ().text = "玩家账号已存在";
//				Invoke ("cleanRegistLabel", 3.0f);
				MsgBoxShow(0,"玩家账号已存在");
				break;
			case	13:
//				Debug.LogWarning ("账号不合法");
//				log_userid.GetComponent<UILabel> ().text = "账号不合法";
//				Invoke ("cleanRegistLabel", 3.0f);
				MsgBoxShow(0,"账号不合法");
				break;
            case 14:
                MsgBoxShow(0, "网络已经断开，正在重连！");
                break;
			}
		}
	}

	private void cleanRegistLabel ()
	{
		log_userid.GetComponent<UILabel> ().text = "";
		log_userid.gameObject.SetActive (false);
	}

	private void cleanIntroducerLabel ()
	{
		log_Introducer.GetComponent<UILabel> ().text = "";
		//	log_Introducer.gameObject.SetActive (false);
	}

	#endregion


	//点击用户登录模式事件
	public delegate void _ncLoginBtnClick (string userName, string passWord, bool savePass);

	public _ncLoginBtnClick ncLoginBtnClick;


	//点击游客登录模式事件
	public delegate void _ncGuestBtnClick ();

	public _ncGuestBtnClick ncGuestBtnClick;

	//账号验证
	public delegate void _ncUserVerify (string userId);

	public _ncUserVerify  ncUserVerify;

	//注册信息提交
	public delegate void _ncUserRegistSubmit (UserRegistMsg userMsg);

	public _ncUserRegistSubmit ncUserRegistSubmit;

	//重新连接请求
	public delegate void _ncConnectQuest ();

	public _ncConnectQuest  ncConnectQuest;

	//退出游戏请求
	public delegate void _ncQuitGame ();

	public _ncQuitGame  ncQuitGame;

}
