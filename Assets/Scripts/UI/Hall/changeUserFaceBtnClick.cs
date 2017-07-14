using UnityEngine;
using System;
using System.Collections;
using com.QH.QPGame.Lobby.Surfaces;

public class changeUserFaceBtnClick : MonoBehaviour {

	public	GameObject		faceLight;
	public	GameObject		submitBtn;
    public	UISprite		faceImg;

	public	UInt32			faceId;

	private	HallTransfer.UserInfomation userInfo = new HallTransfer.UserInfomation();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
	
	
	void OnClick () 
	{
// 		changeUserInfoBtnClick tmpBtnObj = submitBtn.GetComponent<changeUserInfoBtnClick> ();
// 
// 		userInfo.dwName = tmpBtnObj.NameInput.gameObject.GetComponent<UIInput>().value;
// 		userInfo.dwIdentification =  tmpBtnObj.NicknameInput.gameObject.GetComponent<UIInput>().value;
// 		userInfo.dwCellPhone = tmpBtnObj.PhoneInput.gameObject.GetComponent<UIInput>().value;
// 		userInfo.dwIM = tmpBtnObj.QqInput.gameObject.GetComponent<UIInput>().value;
// 		userInfo.dwLogoID = faceId;
// 
// 		HallTransfer.Instance.uiConfig.isChangeFace = true;
//      submitBtn.GetComponent<changeUserInfoBtnClick>().faceId = faceId;

        SurfaceUserInfo._instance.m_iFaceId = faceId;


		faceImg.spriteName = this.GetComponent<UISprite>().spriteName;


// 		if(HallTransfer.Instance.ncChangeUserFace != null)
// 		{
// 			HallTransfer.Instance.ncChangeUserFace (userInfo);
// 		}
// 
// 		this.transform.parent.gameObject.SetActive(false);
	}

	void OnHover ( bool isOver ) {
		faceLight.SetActive(isOver);
	}
}
