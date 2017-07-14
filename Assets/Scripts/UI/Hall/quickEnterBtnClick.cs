using UnityEngine;

public class quickEnterBtnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	public void OnClick ()
	{
		uint tempDeskCount = HallTransfer.Instance.uiConfig.deskCount;
		uint tempChairCount = HallTransfer.Instance.uiConfig.chairCount;
		uint tempDesk, tempChair;
		bool fullSeat = true;

		for(int i = 0; i < tempDeskCount; i++)
		{
			for(int j = 0; j < tempChairCount; j++)
			{
				if (!HallTransfer.Instance.uiConfig.MobileEdition) 
				{
					if(HallTransfer.Instance.roomDeskList [i].transform.FindChild ("chair" + j.ToString()).GetComponent<CGameChairItem> ().empty)
					{
						fullSeat = false;
						break;
					}
				}else{
					for(int k = 0; k < HallTransfer.Instance.roomUserList.Count; k++)
					{
						if(HallTransfer.Instance.roomUserList[k].dwDesk == (uint)i)
						{
							if(HallTransfer.Instance.roomUserList[k].dwChair == (uint)j)
							{
								fullSeat = false;
								break;
							}
						}
					}
				}
			}
			if(!fullSeat) break;
		}
		if(fullSeat) return;
		if (!HallTransfer.Instance.uiConfig.MobileEdition) 
		{
			do {
				tempDesk = (uint)Random.Range (0, tempDeskCount);
				tempChair = (uint)Random.Range (0, tempChairCount);
				Debug.LogWarning ("quickEnterBtnClick:" + tempDesk + "__" + tempChair);
				if (HallTransfer.Instance.roomDeskList [(int)tempDesk].transform.FindChild ("chair" + tempChair).GetComponent<CGameChairItem> ().empty)
					break;
			} while(true);

			Transform tranobj = HallTransfer.Instance.roomDeskList [(int)tempDesk].transform.FindChild ("chair" + tempChair);

			tranobj.GetComponent<CGameChairItem> ().OnClick ();
			tranobj.FindChild ("findUser").gameObject.SetActive(true);
			tranobj.FindChild ("findUser").GetComponent<Animator> ().Play (0);
			
			HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild ("topBar").FindChild ("findPlayer_btn").GetComponent<findUserBtnClick> ().findDesk (tempDesk, tempChair);
		
		} else {
//			bool	temp = true;
//			do {
				tempDesk = (uint)Random.Range (0, tempDeskCount);
				tempChair = (uint)Random.Range (0, tempChairCount);
				bool	tempSameSeat = false;
				Debug.LogWarning ("quickEnterBtnClick:" + tempDesk + "__" + tempChair);
				Debug.LogWarning ("empty:" + HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+tempChair).FindChild("chair_btn").GetComponent<CGameChairItem>().empty);
				for(int i = 0; i < HallTransfer.Instance.roomUserList.Count; i++)
				{
					if(HallTransfer.Instance.roomUserList[i].dwDesk == tempDesk)
					{
						if(HallTransfer.Instance.roomUserList[i].dwChair == tempChair)
						{
							tempSameSeat = true;
							break;
						}
					}
				}
				if(tempSameSeat) return;
								
//			} while(temp);
			HallTransfer.Instance.roomDeskList [(int)tempDesk].transform.GetComponent<moblieRoomDeskBtnClick>().OnClick();
			Transform tranobj = HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+tempChair);
			tranobj.FindChild ("findUser").gameObject.SetActive(true);
			if(tranobj.FindChild ("findUser").GetComponent<Animator>()!=null) tranobj.FindChild ("findUser").GetComponent<Animator>().Play (0);
//			HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+tempChair).FindChild("wait").gameObject.SetActive(true);
			tranobj.FindChild ("chair_btn").GetComponent<CGameChairItem>().OnClick();

			this.findDesk(tempDesk);
		}
	}

	public void findDesk(uint deskId)
	{
		Debug.LogWarning("findDesk:" + deskId);
		uint rowCount = HallTransfer.Instance.uiConfig.deskCount;
		
		float row =  (float)deskId / (float)rowCount ;
		GameObject scrollBar =  HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("desk_scrollBar").gameObject;
		
		scrollBar.GetComponent<UIScrollBar>().value = row;
	}
		
}
