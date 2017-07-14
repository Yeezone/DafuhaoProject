using UnityEngine;
using System;
using System.Collections;

public class findUserBtnClick : MonoBehaviour {

	public	GameObject		userId_input;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick () {

		UInt32 userId = UInt32.Parse( userId_input.GetComponent<UIInput>().value );
		int userCount =  HallTransfer.Instance.roomUserList.Count;
		UInt32 deskId,chairId;
		for(int i = 0; i < userCount; i++)
		{
			if(HallTransfer.Instance.roomUserList[i].dwUserId == userId)
			{
//				int columnCount = HallTransfer.Instance.uiConfig.deskColumnCount;
//				int rowCount = HallTransfer.Instance.uiConfig.deskRowCount;
//
				deskId = HallTransfer.Instance.roomUserList[i].dwDesk;
				chairId = HallTransfer.Instance.roomUserList[i].dwChair;
//
//				
//				int row =  (int)deskId / columnCount ;
				findDesk(deskId,chairId);

//				scrollBar.GetComponent<UIScrollBar>().value = ((float)rowCount/((float)rowCount-1)/(float)rowCount*(float)row);


				return;
			}
		}
		GameObject userIdLogLabel = HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("userId_log_label").gameObject;
		userIdLogLabel.GetComponent<UILabel>().text = "查无此人";
		Invoke("CancelLogLabel",2.0f);
	}
	public void findDesk(uint deskId, uint chairId)
	{
		Debug.LogWarning("findDesk:" + deskId);
		int columnCount = HallTransfer.Instance.uiConfig.deskColumnCount;
		int rowCount = HallTransfer.Instance.uiConfig.deskRowCount;
		if(columnCount == 0) return;
		int row =  (int)deskId / columnCount ;
		GameObject scrollBar =  HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("desk_scrollBar").gameObject;
		
		scrollBar.GetComponent<UIScrollBar>().value = (float)rowCount/((float)rowCount-1)/(float)rowCount*(float)row;
		//999999不播放动画
		if(chairId == 999999) return;	
		HallTransfer.Instance.roomDeskList[(int)deskId].transform.FindChild("chair"+chairId).FindChild("findUser").gameObject.SetActive(true);
		HallTransfer.Instance.roomDeskList[(int)deskId].transform.FindChild("chair"+chairId).FindChild("findUser").GetComponent<Animator>().Play(0);

	}
	private void CancelLogLabel()
	{
		GameObject userIdLogLabel = HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("userId_log_label").gameObject;
		userIdLogLabel.GetComponent<UILabel>().text = "";
	}
}
