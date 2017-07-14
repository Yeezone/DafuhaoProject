using UnityEngine;
using System;
using System.Collections;
//using com.QH.QPGame.Lobby.Surfaces;

public class gameRoomClick : MonoBehaviour {
	
	public	UInt32			RoomId;
	public	UILabel		RoomNameLabel;				//房间名称标签
	public	UILabel		RoomPeopleLabel;			//在线人数标签
	public	UILabel		RoomRateLabel;				//房间倍率标签
	public	UILabel		RoomRateScoreLabel;			//房间倍率分数标签
	public	UILabel		RoomLimitLabel;				//限制标签

	public	GameObject  waiting;

	public void SetRoomInfo(UInt32 roomId, string roomName, string roomPeople, string roomRateScore, string roomLimit)
	{
		RoomId = roomId;
		if(RoomNameLabel != null) RoomNameLabel.text = roomName;
		if(RoomPeopleLabel != null) RoomPeopleLabel.text = roomPeople;
//		if(RoomRateLabel != null) RoomRateLabel.text = roomRate;
		if(RoomRateScoreLabel != null) RoomRateScoreLabel.text = roomRateScore;
		if(RoomLimitLabel != null) RoomLimitLabel.text = roomLimit;
	}

	void OnClick() 
	{
		if (HallTransfer.Instance.uiConfig.MobileEdition) 
		{
			for(int i = 0 ; i<HallTransfer.Instance.gameRoomList.Count; i++)
			{
				if(HallTransfer.Instance.gameRoomList[i].GetComponent<gameRoomClick>().waiting != null)
					HallTransfer.Instance.gameRoomList[i].GetComponent<gameRoomClick>().waiting.SetActive(false);
			}
		}
		if( RoomId != 0 )
		{
			//发送房间ID
			Debug.LogWarning("^^^^^^^^^^^^^ncGameRoomClick^^^^^^^^^^^^^^" + RoomId);

			if(HallTransfer.Instance.ncGameRoomClick!=null)
			{
				if (waiting != null) 
				{
					waiting.gameObject.SetActive(true);
				}

				if (HallTransfer.Instance.uiConfig.window_MaskHall != null)
				{
					//Debug.LogWarning("ShowMaskHall");
					HallTransfer.Instance.uiConfig.window_MaskHall.SetActive(true);
				}

				if(HallTransfer.Instance.ncGameRoomClick!=null)
				{
					HallTransfer.Instance.ncGameRoomClick(RoomId);
				}
			}

			HallTransfer.Instance.uiConfig.curRoomName = this.transform.FindChild("name_label").GetComponent<UILabel>().text;
			Invoke("onResumeBtn",30.0f);
		}
	}

	void onResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_MaskHall!= null
		   && HallTransfer.Instance.uiConfig.window_MaskHall.activeSelf)
		{
			HallTransfer.Instance.uiConfig.window_MaskHall.SetActive(false);
			HallTransfer.Instance.cnTipsBox ("请求超时,稍候再试!");
			if (waiting != null) 
			{
				waiting.gameObject.SetActive(false);
			}
		}

	}

}
