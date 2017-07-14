using UnityEngine;
using System;
using System.Collections;
using com.QH.QPGame.Lobby.Surfaces;

public class changeUserInfoBtnClick : MonoBehaviour {
	
	public	GameObject		UserinfoWindow;
	public	UIInput			NameInput;
	public	UIInput			NicknameInput;
	public	UIInput			PhoneInput;
	public	UIInput			QqInput;
	public	UIInput			SignInput;
	public	UInt32			faceId;
	public	bool			ChangeInfo;
	public	GameObject		NicknameLog;

	private MessageBoxPopup	MsgBox;
	private	HallTransfer.UserInfomation userInfo = new HallTransfer.UserInfomation();

	// Use this for initialization
	void Start () {
		ChangeInfo = false;
	    MsgBox = FindObjectOfType<SurfaceContainer>().GetSurface<MessageBoxPopup>();
	}
	
	// Update is called once per frame
	void Update () {
		if(UserinfoWindow.activeSelf)
		{
			if(NicknameInput.value.Length == 0)
			{
				NicknameLog.SetActive(true);
			}else{
				NicknameLog.SetActive(false);
			}
		}
	}

	public void OnClick () {

        if (NameInput!=null) userInfo.dwName = NameInput.value;
        if (NicknameInput != null) userInfo.dwNickname = NicknameInput.value;
        if (PhoneInput != null) userInfo.dwCellPhone = PhoneInput.value;
        if (QqInput != null) userInfo.dwIM = QqInput.value;
		if(SignInput!=null) userInfo.dwSign = SignInput.value;

		if(userInfo.dwNickname.Length < 2)
		{
			//MsgBox.Show("昵称的长度不能少于2个字符!");
			return;
		}
		byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(userInfo.dwNickname);
		char[] charArray = userInfo.dwNickname.ToCharArray();
		for(int i = 0; i< byteArray.Length; i++)
		{
			if((byteArray[i]<97 || byteArray[i]>122) && (byteArray[i]<65 || byteArray[i]>90) && (byteArray[i]<48 || byteArray[i]>57))
			{
				if(charArray[i]<0x4e00 || charArray[i]>0x9fbb)
				{
					//MsgBox.Show("昵称只能包括汉字、字母和数字!不能使用非法字符!");

					MsgBox.Confirm("","昵称只能包括汉字、字母和数字!不能使用非法字符!");
					return;
				}
			}
		}

		userInfo.dwLogoID = faceId;
		HallTransfer.Instance.uiConfig.isChangeFace = false;

		//this.GetComponent<UIButton> ().isEnabled = false;
//		if( HallTransfer.Instance.uiConfig.MobileEdition)
//		{
//			this.transform.parent.parent.FindChild ("close_btn").GetComponent<UIButton> ().isEnabled = false;
//		}else{
//			this.transform.parent.FindChild ("close_btn").GetComponent<UIButton> ().isEnabled = false;
//		}
//
//		if (!HallTransfer.Instance.uiConfig.MobileEdition) {
//			this.transform.parent.FindChild ("changeFace_btn").GetComponent<UIButton> ().isEnabled = false;
//		}


//		if (userInfo.dwName != HallTransfer.Instance.userInfos.dwName 
//			|| userInfo.dwIdentification != HallTransfer.Instance.userInfos.dwIdentification
//		    || userInfo.dwCellPhone != HallTransfer.Instance.userInfos.dwCellPhone) 
//		{
//		
//			if(userInfo.dwIM != HallTransfer.Instance.userInfos.dwIM) 
//			{
//				ChangeInfo = true;
//			}
//		} else {
//			ChangeInfo = false;
//		}
//
//		if(HallTransfer.Instance.uiConfig.isChangeFace && ChangeInfo)
//		{
//			Debug.LogWarning("changeAll" );
//			HallTransfer.Instance.uiConfig.isChangeFace = false;
//
//			if(HallTransfer.Instance.ncChangeUserFace != null)
//				HallTransfer.Instance.ncChangeUserFace (userInfo);
//
//			if(HallTransfer.Instance.ncChangeUserInformation != null)
//				HallTransfer.Instance.ncChangeUserInformation (userInfo);
//		}
//		else if (HallTransfer.Instance.uiConfig.isChangeFace) 
//		{
//			Debug.LogWarning("changeUserFace" );
//			HallTransfer.Instance.uiConfig.isChangeFace = false;
//			if(HallTransfer.Instance.ncChangeUserFace != null)
//			{
//				HallTransfer.Instance.ncChangeUserFace (userInfo);
//			}
//		} else {
//			Debug.LogWarning("changeInformation" );
//			//发送消息
//			if(HallTransfer.Instance.ncChangeUserInformation != null)
//			HallTransfer.Instance.ncChangeUserInformation (userInfo);
//		}

		if (HallTransfer.Instance.ncChangeUserInformation != null) {
			HallTransfer.Instance.ncChangeUserInformation (userInfo);
		}

		
	}

	MessageBoxCallback2 ggyyg(ref MessageBoxResult _sssss)
	{
		
		Debug.Log(_sssss);
		return null;
		
	}

}
