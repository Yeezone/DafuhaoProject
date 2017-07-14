using UnityEngine;
using System;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.NetFox;
using System.Collections;

namespace com.QH.QPGame.Lobby.Surfaces
{
    [System.Serializable]
    public class Hall_userInfo
    {
        public UILabel userNickName;
        public UILabel userGameId;
        public UILabel[] userMoney;
        public UILabel userBank;
        public UISprite userFace;
        public UILabel userVip;
    }

    [System.Serializable]
    public class Window_userInfo
    {
        public GameObject userInfo_window;
        public Transform front_panel;
        public UIInput nameInput;
        public UIInput nickNameInput;
        public UIInput phoneInput;
        public UIInput qqInput;
        public UIInput signInput;
        public UISprite face_img;
        //public GameObject submit_btn;
    }

    public class SurfaceUserInfo : Surface 
    {
        public Hall_userInfo hall_userInfo;
        public Window_userInfo window_userInfo;
        private MessageBoxPopup MsgBox;
        // 缓存玩家头像ID.每次点击头像都会改变这个值
        public uint m_iFaceId;
        // 缓存玩家个人信息.(每次修改资料/头像,都需匹配)
        public HallTransfer.UserInfomation m_uTempUserInfo = new HallTransfer.UserInfomation();

		// Use this for initialization
		public changeUserFaceBtnClick[] UserFaceBtns;

		public static SurfaceUserInfo _instance = null;

		public static SurfaceUserInfo Instance
		{
			get{
				if (UIRoot.list.Count > 0 && UIRoot.list[0].isActiveAndEnabled)
				{
					_instance=UIRoot.list[0].gameObject.GetComponentInChildren<SurfaceUserInfo>();
				}
				return _instance;
			}
		}

		void Start ()
        {
            MsgBox = FindObjectOfType<SurfaceContainer>().GetSurface<MessageBoxPopup>();
            RegisterEvent();
        }

		void OnDestroy()
		{
			UnRegisterEvent();
			_instance = null;
		}

        //注册事件
		private void RegisterEvent()
		{
            GameApp.Account.UserUpdatedEvent += OnSendUserInfo;//玩家数据事件
            GameApp.Account.ChangeInformationEvent += OnSendChangeUserInforResult;//发送修改资料返回事件
            GameApp.Account.UserInformationEvent += OnSendUserInformation;//发送资料事件

            HallTransfer.Instance.ncCopyUserIDAndName += NcCopyUserIDAndName;//注册复制事件
            HallTransfer.Instance.ncUserInformationRequest += NcUserInformationRequest;//注册资料事件
            HallTransfer.Instance.ncChangeUserInformation += NcChangeUserInformation;//注册修改资料事件
            HallTransfer.Instance.ncChangeUserFace += NcChangeUserFace;//注册修改头像事件
		}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         

		//销毁注册事件
		private void UnRegisterEvent()
		{
                GameApp.Account.UserUpdatedEvent -= OnSendUserInfo;//玩家数据事件
                GameApp.Account.ChangeInformationEvent -= OnSendChangeUserInforResult;//发送修改资料返回事件
                GameApp.Account.UserInformationEvent -= OnSendUserInformation;//发送资料事件

            if (HallTransfer.Instance != null)
            {
                HallTransfer.Instance.ncCopyUserIDAndName -= NcCopyUserIDAndName;//注册复制事件
                HallTransfer.Instance.ncUserInformationRequest -= NcUserInformationRequest;//注册资料事件
                HallTransfer.Instance.ncChangeUserInformation -= NcChangeUserInformation;//注册修改资料事件
                HallTransfer.Instance.ncChangeUserFace -= NcChangeUserFace;//注册修改头像事件
            }
		}

        public void OnSendUserInfo()//玩家数据改变发送
        {
            HallTransfer.MySelfInfo tempInfo = new HallTransfer.MySelfInfo();
            tempInfo.dwUserId = GameApp.GameData.UserInfo.UserID;
            tempInfo.dwNickName = GameApp.GameData.UserInfo.NickName;
            tempInfo.dwMoney = GameApp.GameData.UserInfo.CurMoney;
            tempInfo.dwLockMathine = GameApp.GameData.UserInfo.MoorMachine;
            tempInfo.dwVip = GameApp.GameData.UserInfo.Vip;
            tempInfo.dwRoomID = GameApp.GameData.EnterRoomID;
            tempInfo.dwDeskNo = GameApp.GameData.UserInfo.LastDeskNO;
            tempInfo.dwDeskStation = GameApp.GameData.UserInfo.LastDeskStation;
			tempInfo.dwInsureMoney = GameApp.GameData.UserInfo.CurBank;
            if (GameApp.GameData.UserInfo.HeadId == 0)
            {
                if (!GameApp.GameData.UserInfo.IsBoy)
                {
                    tempInfo.dwHeadID = 1;
                }
                else
                {
                    tempInfo.dwHeadID = 0;
                }
            }
            else
            {
                tempInfo.dwHeadID = GameApp.GameData.UserInfo.HeadId;
            }
            HallTransfer.Instance.cnSetUserInfo(tempInfo);//发送玩家数据
        }

        public void OnSendChangeUserInforResult(int wHandleCode, string msg)
        {
            //发送资料修改结果
            //HallTransfer.Instance.cnChangeUserInformation(wHandleCode, msg);
            cnChangeUserInformation(wHandleCode, msg);
            OnSendUserInfo();
        }

        public void OnSendUserInformation(string dwName,
            string dwIdentification,
            string dwCellPhone,
            string dwIM,
            UInt32 dwLogoID)
        {
            var Userinfo = new HallTransfer.UserInfomation();
            Userinfo.dwName = dwName;
            Userinfo.dwIdentification = dwIdentification;
            Userinfo.dwCellPhone = dwCellPhone;
            Userinfo.dwIM = dwIM;
            Userinfo.dwLogoID = dwLogoID;
            Userinfo.dwSign = GameApp.GameData.UserInfo.UnderWrite;
            Userinfo.dwNickname = GameApp.GameData.UserInfo.NickName;

            //发送资料
            //HallTransfer.Instance.cnUserInformation(Userinfo);
            cnUserInformation(Userinfo);
        }

        public void NcUserInformationRequest()
        {
            //请求用户资料
            GameApp.Account.SendGetUserInfoRequest();
        }

        public void NcChangeUserInformation(HallTransfer.UserInfomation Info)
        {
            //修改用户资料
            GameApp.GameData.UserInfo.HeadId = Info.dwLogoID;
            GameApp.GameData.UserInfo.UnderWrite = Info.dwSign;
            GameApp.Account.SendChangeUserInformation(Info.dwName, Info.dwNickname, Info.dwCellPhone, Info.dwIM,
                                                            Info.dwLogoID, Info.dwSign);
        }

        public void NcChangeUserFace(HallTransfer.UserInfomation Info)
        {
            //修改用户头像
            GameApp.GameData.UserInfo.HeadId = Info.dwLogoID;
            GameApp.Account.SendChangeUserFace(Info.dwName, Info.dwIdentification, Info.dwCellPhone, Info.dwIM,
                                                            Info.dwLogoID);
        }

        public void NcCopyUserIDAndName()
        {
            string UserId = GameApp.GameData.UserInfo.UserID.ToString();
            string NickName = GameApp.GameData.UserInfo.NickName;
            string UserIDAndName = "游戏ID: " + UserId + "  玩家昵称:" + NickName;

            CopyTextToClipboard(UserIDAndName);
        }

        public void CopyTextToClipboard(string text)
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().CopyTextToClipboard(text);
#endif
        }

		//==================================================================================================


		public void SetUserInfo(HallTransfer.MySelfInfo mySelfInfo)
		{
			Debug.LogWarning("!!!!!!!!!!!!!SetUserInfo============================== " + mySelfInfo.dwMoney );

            if (hall_userInfo.userVip != null) hall_userInfo.userVip.text = (mySelfInfo.dwVip == 0 ? "非会员" : "会员");
            if (hall_userInfo.userNickName != null) hall_userInfo.userNickName.text = mySelfInfo.dwNickName;
            if (hall_userInfo.userGameId != null) hall_userInfo.userGameId.text = mySelfInfo.dwUserId.ToString();
			foreach(UILabel tempMoneyLabel in hall_userInfo.userMoney)
			{
				tempMoneyLabel.text = mySelfInfo.dwMoney>=0 ? mySelfInfo.dwMoney.ToString("N0") : "0";
			}
            if (hall_userInfo.userFace != null) hall_userInfo.userFace.spriteName = "face_" + mySelfInfo.dwHeadID;
			int tempSex = (int)mySelfInfo.dwHeadID%2;
			//for(int i = 0; i < UserFaceBtns.Length;i++)
			//{
			//	UserFaceBtns[i].faceId = (uint)((i+1)*2+tempSex);
			//	if(UserFaceBtns[i].GetComponent<UISprite>()!=null)
			//	{
			//		UserFaceBtns[i].GetComponent<UISprite>().spriteName = "face_" + ((i+1)*2+tempSex).ToString();
			//	}
			//}

			// 休闲PC版银行金币可开关.其他版本照常.
			if(showUserBankMoney._instance == null)
			{
				if(hall_userInfo.userBank != null) hall_userInfo.userBank.text = mySelfInfo.dwInsureMoney.ToString();
			}else
			{
				showUserBankMoney._instance.m_lBankMoney = mySelfInfo.dwInsureMoney;
				showUserBankMoney._instance.ShowUserBank();
			}

			bool tempLock = false;
			if(mySelfInfo.dwLockMathine == 1) tempLock = true;
			HallTransfer.Instance.uiConfig.LockOrUnLockAccount = tempLock;
			for(var j = 0; j < HallTransfer.Instance.uiConfig.hallGameRoomIds.Length; j++)
			{
				if(mySelfInfo.dwRoomID == HallTransfer.Instance.uiConfig.hallGameRoomIds[j])
				{
					HallTransfer.Instance.uiConfig.curRoomName = HallTransfer.Instance.uiConfig.hallGameRoomNames[j];
					break;
				}
			}
			HallTransfer.Instance.uiConfig.curRoomId = mySelfInfo.dwRoomID;
			HallTransfer.Instance.uiConfig.curDeskNo = mySelfInfo.dwDeskNo;
			HallTransfer.Instance.uiConfig.curStation = mySelfInfo.dwDeskStation;

			//返回玩家上次游戏进入的桌子列表
			if(mySelfInfo.dwDeskNo!=0)
			{
				if(CGameDeskManger._instance.GetComponentInChildren<UIScrollBar>()!=null)
				{
					float tempVal = (float)mySelfInfo.dwDeskNo/(float)CGameDeskManger._instance.m_lstGameDeskList.Count;
					CGameDeskManger._instance.GetComponentInChildren<UIScrollBar>().value = tempVal;
				}
				if(CChairManger._instance!=null)
				{
					if(CGameDeskManger._instance.m_lstGameDeskList.Count>=mySelfInfo.dwDeskNo+1)
					{
						if(CGameDeskManger._instance.m_lstGameDeskList[(int)mySelfInfo.dwDeskNo].GetComponent<CGameDeskItem>()!=null)
						{
							CGameDeskManger._instance.m_lstGameDeskList[(int)mySelfInfo.dwDeskNo].GetComponent<CGameDeskItem>().OnClick();
						}
					}
				}
			}
		}
		public void ShowUserInfo(HallTransfer.UserInfomation userInfo)
		{
            // 缓存玩家个人信息
            m_uTempUserInfo = userInfo;

            window_userInfo.userInfo_window.transform.localScale = Vector3.zero;
            window_userInfo.userInfo_window.SetActive(true);
            TweenScale tempTweenScale = window_userInfo.userInfo_window.GetComponent<TweenScale>();
            if (tempTweenScale != null)
			{
                tempTweenScale.ResetToBeginning();
                tempTweenScale.Play();
			}else{
				window_userInfo.userInfo_window.transform.localScale = Vector3.one;
			}
            if (window_userInfo.nameInput != null) window_userInfo.nameInput.value = userInfo.dwName;
            if (window_userInfo.nickNameInput != null) window_userInfo.nickNameInput.value = userInfo.dwNickname;
            if (window_userInfo.phoneInput != null) window_userInfo.phoneInput.value = userInfo.dwCellPhone;
            if (window_userInfo.qqInput != null) window_userInfo.qqInput.value = userInfo.dwIM;
            if (window_userInfo.signInput != null) window_userInfo.signInput.value = userInfo.dwSign;
            if (window_userInfo.face_img != null) window_userInfo.face_img.spriteName = "face_" + userInfo.dwLogoID;
		}
		public void CommitUserInfo()
		{
            if (window_userInfo.nameInput != null) m_uTempUserInfo.dwName = window_userInfo.nameInput.value;
            if (window_userInfo.nickNameInput != null) m_uTempUserInfo.dwNickname = window_userInfo.nickNameInput.value;
            if (window_userInfo.phoneInput != null) m_uTempUserInfo.dwCellPhone = window_userInfo.phoneInput.value;
            if (window_userInfo.qqInput != null) m_uTempUserInfo.dwIM = window_userInfo.qqInput.value;
            if (window_userInfo.signInput != null) m_uTempUserInfo.dwSign = window_userInfo.signInput.value;
            NcChangeUserFace(m_uTempUserInfo);
            if (m_uTempUserInfo.dwNickname.Length < 2)
            {
                //MsgBox.Show("昵称的长度不能少于2个字符!");
                return;
            }
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(m_uTempUserInfo.dwNickname);
            char[] charArray = m_uTempUserInfo.dwNickname.ToCharArray();
            for (int i = 0; i < byteArray.Length; i++)
            {
                if ((byteArray[i] < 97 || byteArray[i] > 122) && (byteArray[i] < 65 || byteArray[i] > 90) && (byteArray[i] < 48 || byteArray[i] > 57))
                {
                    if (charArray[i] < 0x4e00 || charArray[i] > 0x9fbb)
                    {
                        //MsgBox.Show("昵称只能包括汉字、字母和数字!不能使用非法字符!");

                        MsgBox.Confirm("", "昵称只能包括汉字、字母和数字!不能使用非法字符!");
                        return;
                    }
                }
            }

            if (HallTransfer.Instance.ncChangeUserInformation != null) HallTransfer.Instance.ncChangeUserInformation(m_uTempUserInfo);
		}
		public void CommitUserFace()
		{
			//HallTransfer.UserInfomation userInfo = new HallTransfer.UserInfomation();
			//if(Window_userface_sprite!=null) userInfo.dwLogoID = uint.Parse(Window_userface_sprite.spriteName.Substring(5,Window_userface_sprite.spriteName.Length-5));
			if(m_iFaceId==0 || m_iFaceId==1) return;//如果0或1就跳过
            if (window_userInfo.face_img != null) m_uTempUserInfo.dwLogoID = m_iFaceId;
			if (HallTransfer.Instance.ncChangeUserInformation != null) HallTransfer.Instance.ncChangeUserFace(m_uTempUserInfo);
		}

		//=================================================================================================================================
		/// <summary>
		/// 接收 点击"资料"后 玩家信息
		/// </summary>
		public void cnUserInformation(HallTransfer.UserInfomation userInfo)
		{
			ShowUserInfo(userInfo);
			if (HallTransfer._instance.canExecuteUIF)
			{
				if (HallTransfer._instance.uiConfig.window_MaskLayer != null)
				{
					HallTransfer._instance.uiConfig.window_MaskLayer.SetActive(false);
				}
				HallTransfer._instance.msgTooLate_UIF = false;
				HallTransfer._instance.userInfos = userInfo;
				GameObject tempInfo = HallTransfer._instance.uiConfig.window_UserInfo;
				tempInfo.SetActive(true);
				
				if (window_userInfo.front_panel != null)    window_userInfo.front_panel.localPosition = Vector3.zero;
				if (window_userInfo.nameInput != null)      window_userInfo.nameInput.value = userInfo.dwName;
				if (window_userInfo.nickNameInput != null)  window_userInfo.nickNameInput.value = userInfo.dwNickname;
				if (window_userInfo.phoneInput != null)     window_userInfo.phoneInput.value = userInfo.dwCellPhone;
				if (window_userInfo.qqInput != null)        window_userInfo.qqInput.value = userInfo.dwIM;
				if (window_userInfo.signInput != null)      window_userInfo.signInput.value = userInfo.dwSign;
				if (window_userInfo.face_img != null)       window_userInfo.face_img.spriteName = "face_" + userInfo.dwLogoID;
				//if (window_userInfo.submit_btn != null)     window_userInfo.submit_btn.GetComponent<changeUserInfoBtnClick>().faceId = userInfo.dwLogoID;
			}
		}	
		
		/// <summary>
		/// 接收 修改资料 返回消息
		/// </summary>
		public void cnChangeUserInformation(int wHandleCode, string msg)
		{
			Debug.LogWarning("cnChangeUserInformation-----------------" + wHandleCode);
			//wHandleCode		0:成功	1:失败
			
			if (wHandleCode == 0)
			{
				hall_userInfo.userFace.spriteName = window_userInfo.face_img.spriteName;
				HallTransfer._instance.cnTipsBox("资 料 修 改 成 功 !");
			}
			else
			{
				HallTransfer._instance.cnTipsBox(msg);
			}
			
 			//每次资料修改成功后,自动隐藏玩家信息窗口.(暂时不隐藏.如果解决Toggle功能,则隐藏的同时调用一次Toggle重新排列.不然会出现层级问题.)
             if (!HallTransfer._instance.uiConfig.isChangeFace)
             {
                 HallTransfer._instance.uiConfig.isChangeFace = false;
                 HallTransfer._instance.uiConfig.window_UserInfo.SetActive(false);
             }
		}

	}

}

