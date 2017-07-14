using UnityEngine;
using System;
using System.Collections;

public class gameChairClick : MonoBehaviour {

	public	GameObject	toolTip;
	public	bool		empty = true;
	public	UInt32		roomid;
	public	UInt32		deskid;
	public	UInt32		chairid;
	public	uint		chairCount;

	private GameObject  waiting;

	// Use this for initialization
	void Start () {
		if(HallTransfer.Instance.uiConfig.MobileEdition != true) toolTip = HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("userInfo_toolTip").gameObject;
	}


	public void OnClick() {
		if(empty)
		{
			if(HallTransfer.Instance.uiConfig.MobileEdition)
			{
				for( int i=0; i < HallTransfer.Instance.uiConfig.chairCount ; i++ )
				{
					Transform tranobj = HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+i);
					using(tranobj as IDisposable)
					{
						tranobj.FindChild("wait").gameObject.SetActive(false);
						tranobj.FindChild("wait").gameObject.SetActive(false);
						if(tranobj.FindChild("chair_btn").gameObject.GetComponent<gameChairClick>().chairid == chairid)
						{
							tranobj.FindChild("wait").gameObject.SetActive(true);
							waiting = tranobj.FindChild("wait").gameObject;
							HallTransfer.Instance.uiConfig.curTempChair = tranobj.gameObject;
						}
					}
				}
			}
			else{
				for( int i=0; i < HallTransfer.Instance.uiConfig.chairCount ; i++ )
				{
					for(int j = 0; j < HallTransfer.Instance.roomDeskList.Count; j++)
					{
						Transform tranobj = HallTransfer.Instance.roomDeskList[j].transform.FindChild("chair"+i);
						using(tranobj as IDisposable)
						{
							if(tranobj.gameObject.GetComponent<gameChairClick>().chairid == chairid
							   && tranobj.gameObject.GetComponent<gameChairClick>().deskid == deskid)	
							{
								tranobj.FindChild("wait").gameObject.SetActive(true);
								waiting = tranobj.FindChild("wait").gameObject;
								HallTransfer.Instance.uiConfig.curTempChair = tranobj.gameObject;
							}else
							{
								tranobj.FindChild("wait").gameObject.SetActive(false);
							}
						}
					}
				}
			}

			if (HallTransfer.Instance.uiConfig.window_MaskRoom != null) {
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
		if(!HallTransfer.Instance.uiConfig.MobileEdition)
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
				toolTip.transform.FindChild("userId_label").GetComponent<UILabel>().text = userId;
				toolTip.transform.FindChild("userNickname_label").GetComponent<UILabel>().text = userNickName;
				toolTip.transform.FindChild("userGold_label").GetComponent<UILabel>().text = userGold;
				toolTip.transform.FindChild("userGameTime_label").GetComponent<UILabel>().text = userGameTime;
				toolTip.SetActive(true);
				
				Vector3 toolTipPos = new Vector3();
				toolTipPos.x = this.transform.localPosition.x + this.transform.parent.localPosition.x + this.transform.parent.parent.localPosition.x + this.transform.parent.parent.parent.localPosition.x;
				toolTipPos.y = this.transform.localPosition.y + this.transform.parent.localPosition.y + this.transform.parent.parent.localPosition.y + this.transform.parent.parent.parent.localPosition.y- this.GetComponent<UISprite>().height;
				toolTip.transform.localPosition = toolTipPos;
			}else {
				if(toolTip != null) toolTip.SetActive(false);
			}
		}
	}
}
