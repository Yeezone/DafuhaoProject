using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CGameDeskManger : MonoBehaviour {

    public static CGameDeskManger _instance;
	public HallTransfer.RoomDeskInfo m_RoomDeskInfo = new HallTransfer.RoomDeskInfo();
	public UILabel m_lblRoomName;//房间名称label
	public CGameItem m_ReturnBtn;//返回房间按钮
	public GameObject m_objUserTip;
	public UILabel m_lblTipUserID;
	public UILabel m_lblTipUserNickName;
	public UILabel m_lblTipUserGold;
	public UILabel m_lblTipUserGameTime;

    //桌子预设
    public GameObject[] m_objGameDeskPrefab;//每个游戏一个桌子预设()
	public GameObject m_objCurDesk;//当前桌子预设
	public GameObject m_objCurDesk_phone;//手机当前大桌子按钮
	public GameObject m_objDeskPrefab_phone;//手机桌子按钮预设
	public bool[] DeskState;	//桌子"游戏中"状态数组
	
    //桌子列表
	public List<CGameDeskItem> m_lstGameDeskList = new List<CGameDeskItem>();

    public GameObject m_objDeskGrid;

	public string DeskCreatorMethod;

	public CGameChairItem[] m_ChairItems;

    void Awake()
    {
        _instance = this;
    }
	void OnDestroy()
    {
        _instance = null;
    }
	public void SetGameDeskInfo(HallTransfer.RoomDeskInfo deskInfo)
	{
		DeskState = new bool[deskInfo.dwDeskCount];
		for(var i = 0; i < deskInfo.dwDeskCount; i++)
		{
			DeskState[i] = false;		//设置桌子初始状态
		}

		Debug.LogWarning("SetGameDeskInfo:" + deskInfo.dwGameID.ToString());
		CGameRoomManger._instance.SetTipRoomName(deskInfo.dwRoomId);
		if(CGameManager._instance.m_objFirstPage!=null) CGameManager._instance.m_objFirstPage.SetActive(false);		
		m_RoomDeskInfo = deskInfo;
		HallTransfer.Instance.roomUserList.Clear();//清空座位上玩家信息
		//if(CGameRoomManger._instance.GetComponent<TweenScale>()!=null)
		{
//			CGameRoomManger._instance.GetComponent<TweenScale>().PlayReverse();
//		}else{
			CGameRoomManger._instance.transform.localScale = Vector3.zero;
		}
		if(GetComponent<TweenScale>()!=null)
		{
			GetComponent<TweenScale>().ResetToBeginning();
			GetComponent<TweenScale>().PlayForward();
		}else{
			transform.localScale = Vector3.one;
		}
		//销毁旧桌子
		for(var i = 0; i < m_lstGameDeskList.Count; i++)
		{
			if(m_lstGameDeskList[i]!=null) DestroyImmediate(m_lstGameDeskList[i].gameObject);
		}
		m_lstGameDeskList.Clear();
		//选桌子
		for(int i = 0; i < m_objGameDeskPrefab.Length; i++)
		{
			if(m_objGameDeskPrefab[i].name == "DeskItem_" + deskInfo.dwGameID)
			{
				m_objCurDesk = m_objGameDeskPrefab[i];
				break;
			}
		}

		// 接到创建桌子消息,则把当前游戏ID传入returnBtn(防止断线重连后找不到房间)
		uint tempId;
		uint tempInt = (deskInfo.dwGameID / 1000);
		string tempStr = deskInfo.dwGameID.ToString().Substring(2);
		uint.TryParse(tempStr, out tempId);
		tempId = tempInt * 100 + tempId;
		if(tempId==101) tempId = 100;
		m_ReturnBtn.GetComponent<CGameItem>().m_cGameInfo.ID = tempId;

		this.GetType().GetMethod(DeskCreatorMethod).Invoke(this, null);

	}

	public void CreatePhoneDesk()
	{
		//创建大桌子
		if(m_objCurDesk_phone!=null) DestroyImmediate(m_objCurDesk_phone);
		m_objCurDesk_phone = (GameObject)Instantiate(m_objCurDesk,Vector3.zero,m_objCurDesk.transform.localRotation);
		m_objCurDesk_phone.transform.parent = CGameDeskManger._instance.transform;
		m_objCurDesk_phone.transform.localScale = Vector3.one;
		if(m_objCurDesk_phone.GetComponent<TweenPosition>()!=null)
		{
			m_objCurDesk_phone.GetComponent<TweenPosition>().ResetToBeginning();
			m_objCurDesk_phone.GetComponent<TweenPosition>().PlayForward();
		}else{
			m_objCurDesk_phone.transform.localPosition = Vector3.one;
		}
		CGameChairItem[] tempChairItems = m_objCurDesk_phone.GetComponentsInChildren<CGameChairItem>();
		for(int i = 0; i < tempChairItems.Length; i++)
		{
			tempChairItems[i].ClearUserInfo();
		}
		//创建桌子按钮
		for(int i = 0; i < m_RoomDeskInfo.dwDeskCount; i++)
		{
			AddGameDeskItem_Phone(m_RoomDeskInfo);
		}
		if(m_objDeskGrid.GetComponent<UIGrid>()!=null)
		{

			m_objDeskGrid.GetComponent<UIGrid>().Reposition();
			m_objDeskGrid.GetComponent<UIGrid>().repositionNow = true;
		}
		if(m_lstGameDeskList.Count>0) m_lstGameDeskList[0].OnClick();//默认选中1号桌
		Debug.LogWarning("m_lstGameDeskList[0].OnClick!!");
	}
	public void CreateGoldenDesk()
	{
		//创建大桌子
		if(m_objCurDesk_phone!=null) DestroyImmediate(m_objCurDesk_phone);
		m_objCurDesk_phone = (GameObject)Instantiate(m_objCurDesk,Vector3.zero,m_objCurDesk.transform.localRotation);
		m_objCurDesk_phone.transform.parent = CGameDeskManger._instance.transform;
		m_objCurDesk_phone.transform.localScale = new Vector3(0.85f,0.85f,0.85f);
		if(m_objCurDesk_phone.GetComponent<TweenPosition>()!=null)
		{
			m_objCurDesk_phone.GetComponent<TweenPosition>().ResetToBeginning();
			m_objCurDesk_phone.GetComponent<TweenPosition>().PlayForward();
		}else{
			m_objCurDesk_phone.transform.localPosition = new Vector2(-50f,0f);//Vector3.one;
		}
		CGameChairItem[] tempChairItems = m_objCurDesk_phone.GetComponentsInChildren<CGameChairItem>();
		for(int i = 0; i < tempChairItems.Length; i++)
		{
			tempChairItems[i].ClearUserInfo();
		}
		//创建桌子按钮
		for(int i = 0; i < m_RoomDeskInfo.dwDeskCount; i++)
		{
			AddGameDeskItem_Phone(m_RoomDeskInfo);
		}
		if(m_objDeskGrid.GetComponent<UIGrid>()!=null)
		{
			m_objDeskGrid.GetComponent<UIGrid>().Reposition();
			m_objDeskGrid.GetComponent<UIGrid>().repositionNow = true;
		}
		if(m_lstGameDeskList.Count>0) m_lstGameDeskList[0].OnClick();//默认选中1号桌
		if(GetComponentInChildren<UIScrollBar>()!=null)
		{
			GetComponentInChildren<UIScrollBar>().value = 0;
		}
		Debug.LogWarning("m_lstGameDeskList[0].OnClick!!");
	}
	public void CreatePCDesk()
	{
		for(int i = 0; i < m_RoomDeskInfo.dwDeskCount; i++)
		{
			AddGameDeskItem_PC(m_RoomDeskInfo);
		}
		ResetDeskGrid();
	}
	public void ResetDeskGrid()
	{
		UIGrid tempGrid = m_objDeskGrid.GetComponent<UIGrid>();
		UIScrollView tempScrollView = m_objDeskGrid.GetComponentInParent<UIScrollView>();
		int viewWidth, cellWidth, cellHeight, widthInterval, heightInterval, cellColumn, cellRow;
		int miniWidthInterval = 5;
		int miniHeightInterval = 40;
		cellWidth = m_objCurDesk.GetComponent<UIWidget>().width;
		cellHeight = m_objCurDesk.GetComponent<UIWidget>().height;
		viewWidth = ((int)(tempScrollView.GetComponent<UIPanel>().GetWindowSize().x-305));
		cellColumn = ((int)(tempScrollView.GetComponent<UIPanel>().GetWindowSize().x-305))/(cellWidth);
		cellRow = System.Convert.ToInt32 (m_lstGameDeskList.Count / cellColumn);
		widthInterval = (viewWidth - cellWidth * cellColumn) / (cellColumn + 1);
		int tempColumn = 0;
		int tempRow = 0;
		for (int i = 0; i < m_lstGameDeskList.Count; i++) {
			float tempX = widthInterval + (tempColumn * (widthInterval +  cellWidth));
			float tempY = (miniHeightInterval + (tempRow * (miniHeightInterval + cellHeight))) * -1;
			m_lstGameDeskList [i].transform.localPosition = new Vector3 (tempX, tempY, 0f);
			if (++tempColumn == cellColumn) {
				tempColumn = 0;
				tempRow++;
			}
		}
		Debug.LogWarning( "deskReposition!!!!!!!" + viewWidth.ToString() + "  ");
//		uiConfig.page_roomDesk.transform.FindChild ("desk_scrollBar").GetComponent<UIScrollBar> ().value = 0;
		tempScrollView.ResetPosition();
	}
	
	//创建桌子
	public void AddGameDeskItem_Phone(HallTransfer.RoomDeskInfo tableInfo)
    {
		GameObject temp_obj = null;

		//手机版
		temp_obj = (GameObject)Instantiate(m_objDeskPrefab_phone, Vector3.zero, m_objCurDesk.transform.localRotation);
		
		temp_obj.GetComponent<CGameDeskItem>().m_lblDeskName = m_objCurDesk_phone.transform.FindChild("desk_label").GetComponent<UILabel>();

		temp_obj.transform.name = "GameItem" + m_lstGameDeskList.Count.ToString();
        temp_obj.transform.parent = m_objDeskGrid.transform;
        temp_obj.transform.localScale = Vector3.one;
        CGameDeskItem gameItem = temp_obj.GetComponent<CGameDeskItem>();
         
		gameItem.UpdateGameDeskInfo(m_lstGameDeskList.Count,0,tableInfo);
		m_lstGameDeskList.Add(gameItem);
    }

	public void UpdateDeskInfo_Phone(CGameChairItem []_ChairArry)
	{

	}
	public void AddGameDeskItem_PC(HallTransfer.RoomDeskInfo tableInfo)
	{
		//PC版
		GameObject temp_obj = null;
		temp_obj = (GameObject)Instantiate(m_objCurDesk, Vector3.zero, m_objCurDesk.transform.localRotation);
		temp_obj.GetComponent<CGameDeskItem>().m_ChairItems.AddRange(temp_obj.GetComponentsInChildren<CGameChairItem>());

		for(int i = 0; i < temp_obj.GetComponent<CGameDeskItem>().m_ChairItems.Count; i++)
		{
			temp_obj.GetComponent<CGameDeskItem>().m_ChairItems[i].deskid = (uint)m_lstGameDeskList.Count;
			temp_obj.GetComponent<CGameDeskItem>().m_ChairItems[i].chairid = (uint)i;
		}
		temp_obj.transform.name = "GameItem" + m_lstGameDeskList.Count.ToString();
		temp_obj.transform.parent = m_objDeskGrid.transform;

		temp_obj.transform.localScale = Vector3.one;
		CGameDeskItem gameItem = temp_obj.GetComponent<CGameDeskItem>();
		
		gameItem.UpdateGameDeskInfo(m_lstGameDeskList.Count,0,tableInfo);
		m_lstGameDeskList.Add(gameItem);

		// 适应休闲版断线重连后的显示错乱
		if(CGameManager._instance.m_objGameScrollView != null)
		{
			CGameManager._instance.m_objGameScrollView.transform.localScale = Vector3.zero;
		}
	}

	public void QuickEnter()
	{
		uint tempMaxDeskId = m_lstGameDeskList[0].m_cGameDeskInfo.dwDeskCount;
		uint tempMaxChairId = m_lstGameDeskList[0].m_cGameDeskInfo.dwDeskPeople;
		uint tempDesk,tempChair,tempRoom;
		if(CGameRoomManger._instance.m_lstRoomItemList.Count>0)
		{
			tempRoom = CGameRoomManger._instance.m_lstRoomItemList[0].m_cGameInfo.roomId;
		}else{
			return;
		}
//		int tempListIndex;
		bool tempFinded = false;
		int tempRangeCount = 0;
		do {
			if(tempRangeCount++ > 100) return;
			tempDesk = (uint)Random.Range (0, (int)tempMaxDeskId);
			tempChair = (uint)Random.Range (0, (int)tempMaxChairId);
			for(int i = 0; i < HallTransfer.Instance.roomUserList.Count; i++)
			{
				if(HallTransfer.Instance.roomUserList[i].dwDesk==tempDesk && HallTransfer.Instance.roomUserList[i].dwChair==tempChair)
				{
					tempFinded = true;
					break;
				}
			}
			if(!tempFinded) break;
		} while(true);
		Debug.LogWarning("QuickEnter:" + tempRoom.ToString()+"  "+tempDesk.ToString()+"   " +tempChair.ToString() );
		HallTransfer.Instance.ncGameChairClick(tempRoom,tempDesk,tempChair);		
	}

	//===========================================================================================
	// 从HallTransfer移植
	public void cnSetDeskStatus( uint deskId, byte status)
	{
		DeskState[deskId] = (status == 0?false:true);
		if(!HallTransfer._instance.uiConfig.MobileEdition)
		{
			if(m_lstGameDeskList[(int)deskId] != null) m_lstGameDeskList[(int)deskId].transform.FindChild("gaming_img").gameObject.SetActive(DeskState[deskId]);
		}else{
			if(m_lstGameDeskList[(int)deskId] != null)
			{
				m_lstGameDeskList[(int)deskId].transform.FindChild("ruchang_label").GetComponent<UILabel>().text = (DeskState[deskId]?"游戏中":"可进入");
				m_lstGameDeskList[(int)deskId].transform.FindChild("ruchang_label").GetComponent<UILabel>().color = (DeskState[deskId]?Color.yellow:Color.green);
			}
		}
	}

}
