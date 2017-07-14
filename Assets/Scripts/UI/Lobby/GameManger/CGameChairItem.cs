using UnityEngine;
using System;
using System.Collections;

public class CGameChairItem : MonoBehaviour {

	public	GameObject	toolTip;
	public	bool		empty = true;
	public	UInt32		roomid;
	public	UInt32		deskid;
	public	UInt32		chairid;
	public	uint		chairCount;

	public UILabel m_lblUserID;
	public UILabel m_lblUserName;
	public UILabel m_lblUserMoney;
	public UILabel m_lblGameCount;
	public UISprite m_sprUserFace;

	private GameObject  waiting;


	public void UserSitDown(HallTransfer.RoomUserInfo userInfo)
	{
		if(m_lblUserID!=null) m_lblUserID.text = userInfo.dwUserId.ToString();
		if(m_lblUserName!=null) m_lblUserName.text = userInfo.dwNickName.ToString();
		if(m_lblUserMoney!=null) m_lblUserMoney.text = userInfo.dwMoney.ToString();
		if(m_lblGameCount!=null) m_lblGameCount.text = userInfo.dwGameCount.ToString();
		if(m_sprUserFace!=null)
		{
			m_sprUserFace.gameObject.SetActive(true);
			m_sprUserFace.spriteName = "face_" + userInfo.dwLogoID.ToString();
		}
		empty = false;
	}
	public void ClearUserInfo()
	{
		if(m_lblUserID!=null) m_lblUserID.text = "";
		if(m_lblUserName!=null) m_lblUserName.text = "";
		if(m_lblUserMoney!=null) m_lblUserMoney.text = "";
		if(m_lblGameCount!=null) m_lblGameCount.text = "";
		if(m_sprUserFace!=null)
		{
			m_sprUserFace.gameObject.SetActive(false);
			m_sprUserFace.spriteName = "";
		}
		empty = true;
	}
	public void OnClick()
	{
		if(empty)
		{
//			if(HallTransfer.Instance.uiConfig.MobileEdition)
//			{
//				for( int i=0; i < HallTransfer.Instance.uiConfig.chairCount ; i++ )
//				{
//					Transform tranobj = HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+i);
//					using(tranobj as IDisposable)
//					{
//						tranobj.FindChild("wait").gameObject.SetActive(false);
//						tranobj.FindChild("wait").gameObject.SetActive(false);
//						if(tranobj.FindChild("chair_btn").gameObject.GetComponent<CGameChairItem>().chairid == chairid)
//						{
//							tranobj.FindChild("wait").gameObject.SetActive(true);
//							waiting = tranobj.FindChild("wait").gameObject;
//							HallTransfer.Instance.uiConfig.curTempChair = tranobj.gameObject;
//						}
//					}
//				}
//			}
//			else{
//				for( int i=0; i < HallTransfer.Instance.uiConfig.chairCount ; i++ )
//				{
//					for(int j = 0; j < HallTransfer.Instance.roomDeskList.Count; j++)
//					{
//						Transform tranobj = HallTransfer.Instance.roomDeskList[j].transform.FindChild("chair"+i);
//						using(tranobj as IDisposable)
//						{
//							if(tranobj.gameObject.GetComponent<CGameChairItem>().chairid == chairid
//							   && tranobj.gameObject.GetComponent<CGameChairItem>().deskid == deskid)	
//							{
//								tranobj.FindChild("wait").gameObject.SetActive(true);
//								waiting = tranobj.FindChild("wait").gameObject;
//								HallTransfer.Instance.uiConfig.curTempChair = tranobj.gameObject;
//							}else
//							{
//								tranobj.FindChild("wait").gameObject.SetActive(false);
//							}
//						}
//					}
//				}
//			}

			if (HallTransfer.Instance.uiConfig.window_MaskRoom != null)
			{
				HallTransfer.Instance.uiConfig.window_MaskRoom.SetActive(true);
			}

			if( HallTransfer.Instance.ncGameChairClick != null )
			{
				HallTransfer.Instance.ncGameChairClick(roomid,deskid,chairid);
			}
			Invoke("onResumeBtn",5.0f);		
		}
	}

	void onResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_MaskRoom != null
		   && HallTransfer.Instance.uiConfig.window_MaskRoom.activeSelf)
		{
			HallTransfer.Instance.uiConfig.window_MaskRoom.SetActive(false);
			HallTransfer.Instance.cnMsgBox ("响应超时");
			if (waiting != null) 
			{
				waiting.gameObject.SetActive(false);
			}
		}
		
	}


	void OnHover( bool isOver ){
		if(CGameDeskManger._instance.m_objUserTip!=null)
		{
			if(isOver && !empty)
			{
				string userId = "";
				string userNickName = "";
				string userGold = "";
				string userGameTime = "";
				for(int i = 0; i < HallTransfer.Instance.roomUserList.Count; i++)
				{
					if(HallTransfer.Instance.roomUserList[i].dwDesk == deskid && HallTransfer.Instance.roomUserList[i].dwChair == chairid)
					{
						userId = HallTransfer.Instance.roomUserList[i].dwUserId.ToString();
						userNickName = HallTransfer.Instance.roomUserList[i].dwNickName.ToString();
						userGold =HallTransfer.Instance.roomUserList[i].dwMoney.ToString();
						userGameTime = HallTransfer.Instance.roomUserList[i].dwGameCount.ToString();
						break;
					}
				}
				if(CGameDeskManger._instance.m_lblTipUserID!=null) CGameDeskManger._instance.m_lblTipUserID.text = userId;
				if(CGameDeskManger._instance.m_lblTipUserNickName!=null) CGameDeskManger._instance.m_lblTipUserNickName.text = userNickName;
				if(CGameDeskManger._instance.m_lblTipUserGold!=null) CGameDeskManger._instance.m_lblTipUserGold.text = userGold;
				if(CGameDeskManger._instance.m_lblTipUserGameTime!=null) CGameDeskManger._instance.m_lblTipUserGameTime.text = userGameTime;
				CGameDeskManger._instance.m_objUserTip.SetActive(true);
				
				Vector3 toolTipPos = new Vector3();
				toolTipPos.x = this.transform.localPosition.x + this.transform.parent.localPosition.x + this.transform.parent.parent.localPosition.x + this.transform.parent.parent.parent.localPosition.x;
				toolTipPos.y = this.transform.localPosition.y + this.transform.parent.localPosition.y + this.transform.parent.parent.localPosition.y + this.transform.parent.parent.parent.localPosition.y- this.GetComponent<UISprite>().height;
				CGameDeskManger._instance.m_objUserTip.transform.localPosition = toolTipPos;
			}else {
				CGameDeskManger._instance.m_objUserTip.SetActive(false);
			}
		}
	}
}
