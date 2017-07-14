using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGameRoomManger : MonoBehaviour {

    public static CGameRoomManger _instance;

	public UILabel m_lblGameName;//游戏名称label
    //房间列表
    public List<CGameRoomItem> m_lstRoomItemList = new List<CGameRoomItem>();
	//房间样式预设
	public GameObject[] m_objGameRoomType;
	public GameObject m_objGameRoomType_default;
	public GameObject m_objCurGameRoomType;
    //房间预设
    public GameObject[] m_objGameRoomItemPrefab;
	public GameObject m_objGameRoomItem_default;
	public GameObject m_objCurGameRoomItem;

    public GameObject m_objCurRoomGrid;
//    public GameObject m_objRoomParent;
	public List<HallTransfer.RoomInfoS> m_lstRoomInfoList = new List<HallTransfer.RoomInfoS>();

	public string RoomCreatorMethod;

    private List<CGameRoomItem> m_lstGameRoomListBK = new List<CGameRoomItem>();

    void Awake()
    {
        _instance = this;
    }
	void OnDestroy()
    {
        _instance = null;
    }


	/// <summary>
	/// 批量创建房间
	/// </summary>
	/// <param name="roomList">房间列表</param>
	public void SetRoomList(uint NodeID,List<HallTransfer.RoomInfoS> roomList)
	{
		Debug.LogWarning("SetRoomList!!!");
		m_lstRoomInfoList = roomList;
		CGameManager._instance.SetTipGameName(NodeID);
		if(CGameManager._instance.m_objCurGameId=="")
		{
			CGameManager._instance.m_objCurGameId = NodeID.ToString();
		}
		if(CGameDeskManger._instance.m_ReturnBtn!=null) CGameDeskManger._instance.m_ReturnBtn.m_cGameInfo.ID = NodeID;
		if(CGameManager._instance.m_objFirstPage!=null) CGameManager._instance.m_objFirstPage.SetActive(false);
		m_objCurGameRoomItem = null;
		m_lstGameRoomListBK.Clear();
		if(GetComponent<TweenScale>()!=null) GetComponent<TweenScale>().ResetToBeginning();
		if(m_objCurRoomGrid != null)
		{
			foreach (Transform child in m_objCurRoomGrid.transform)
			{
				if (child != null)
				{
					Destroy(child.gameObject, 0);
				}
			}
		}
		m_lstRoomItemList.Clear();
		//选房间类型
		bool roomTypeFinded = false;
		m_objGameRoomType_default.SetActive(false);
		for(var i = 0; i < m_objGameRoomType.Length; i++)
		{
			if(m_objGameRoomType[i].name == "RoomType_" + NodeID.ToString())
			{
				roomTypeFinded = true;
				m_objGameRoomType[i].SetActive(true);
				m_objCurGameRoomType = m_objGameRoomType[i];
				m_objCurRoomGrid = m_objGameRoomType[i].transform.FindChild("Room_ScrollView").FindChild("RoomGrid").gameObject;
			}else{
				m_objGameRoomType[i].SetActive(false);
			}
		}
		if(!roomTypeFinded && m_objGameRoomType_default!=null)
		{
			m_objGameRoomType_default.SetActive(true);
			m_objCurGameRoomType = m_objGameRoomType_default;
			m_objCurRoomGrid = m_objGameRoomType_default.transform.FindChild("Room_ScrollView").FindChild("RoomGrid").gameObject;
		}else if(!roomTypeFinded)
		{
			Debug.LogError("GameRoomType is not found!");
			return;
		}
		//选房间按钮预设
		bool roomFinded = false;
		int roomItemIndex = 0;
		for(int i = 0; i < m_objGameRoomItemPrefab.Length; i++)
		{
			if(m_objGameRoomItemPrefab[i].name == "RoomItem_" + NodeID.ToString())
			{
				roomFinded = true;
				m_objCurGameRoomItem = m_objGameRoomItemPrefab[i];
			}
		}
		if(!roomFinded && m_objGameRoomItem_default!=null)
		{
			m_objCurGameRoomItem = m_objGameRoomItem_default;
		}else if(!roomFinded)
		{
			Debug.LogError("GameRoomItem is not found!");
			return;
		}
		this.GetType().GetMethod(RoomCreatorMethod).Invoke(this, null); 
	}

    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom_default()
    {
		HallTransfer.RoomInfoS roomInfo;
		for(int i = 0; i < m_lstRoomInfoList.Count; i++)
		{
			roomInfo = m_lstRoomInfoList[i];
			GameObject temp_obj = null;
			temp_obj = (GameObject)Instantiate(m_objCurGameRoomItem, Vector3.zero, m_objCurGameRoomItem.transform.localRotation);
			Debug.LogWarning("CurGameRoomItem:" + m_objCurGameRoomItem.name);
			temp_obj.transform.name = "RoomItem" + m_lstRoomItemList.Count.ToString();
			temp_obj.transform.parent = m_objCurRoomGrid.transform;
			temp_obj.transform.localScale = Vector3.one;
			temp_obj.SetActive(true);
			UIEventListener.Get(temp_obj).onClick = GameRoom_OnClick;		
			CGameRoomItem roomItem = temp_obj.GetComponent<CGameRoomItem>();
			roomItem.UpdateRoomItem(roomInfo);
			m_lstRoomItemList.Add(roomItem);
		}
		Invoke("ResetRoomPosition_default", 0.1f);
	}
	public void CreateRoom_GameblePC()
	{
		HallTransfer.RoomInfoS roomInfo;
		this.transform.localScale = Vector3.one;//new Vector3(1f,0f,0f);
		for(int i = 0; i < m_lstRoomInfoList.Count; i++)
		{
			roomInfo = m_lstRoomInfoList[i];
			GameObject temp_obj = null;
			temp_obj = (GameObject)Instantiate(m_objCurGameRoomItem, Vector3.one, m_objCurGameRoomItem.transform.localRotation);
			Debug.LogWarning("CurGameRoomItem:" + m_objCurGameRoomItem.name);
			temp_obj.transform.name = "RoomItem" + m_lstRoomItemList.Count.ToString();
			temp_obj.transform.parent = m_objCurRoomGrid.transform;
			temp_obj.transform.localScale = Vector3.one;
			temp_obj.SetActive(true);
			UIEventListener.Get(temp_obj).onClick = GameRoom_OnClick;		
			CGameRoomItem roomItem = temp_obj.GetComponent<CGameRoomItem>();
			roomItem.UpdateRoomItem(roomInfo);
			m_lstRoomItemList.Add(roomItem);
		}
//		m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().enabled = true;
		Invoke("ResetRoomPosition_GameblePC", 0.2f);
	}
	public void CreateRoom_Relax()
	{
		HallTransfer.RoomInfoS roomInfo;
		for(int i = 0; i < m_lstRoomInfoList.Count; i++)
		{
			roomInfo = m_lstRoomInfoList[i];
			GameObject temp_obj = null;
			temp_obj = (GameObject)Instantiate(m_objCurGameRoomItem, Vector3.zero, m_objCurGameRoomItem.transform.localRotation);
			Debug.LogWarning("CurGameRoomItem:" + m_objCurGameRoomItem.name);
			temp_obj.transform.name = "RoomItem" + m_lstRoomItemList.Count.ToString();
			temp_obj.transform.parent = m_objCurRoomGrid.transform;
			temp_obj.transform.localScale = Vector3.one;
			temp_obj.SetActive(true);
			UIEventListener.Get(temp_obj).onClick = GameRoom_OnClick;		
			CGameRoomItem roomItem = temp_obj.GetComponent<CGameRoomItem>();
			roomItem.UpdateRoomItem(roomInfo);
			// 休闲版独立更新游戏房间图片(暂时)
			roomItem.UpdateRoomSprite((i%4));
			m_lstRoomItemList.Add(roomItem);
			// 判断当前几个房间.不同房间不同设置
//			if(m_lstRoomInfoList.Count <= 1)
//			{
//				temp_obj.transform.localPosition = m_vRoomItemPos[1];
//			}else if(m_lstRoomInfoList.Count == 2){
//				temp_obj.transform.localPosition = m_vRoomItemPos[i+3];
//			}else if(m_lstRoomInfoList.Count >= 3){
//				temp_obj.transform.localPosition = m_vRoomItemPos[(i%3)]; 
//			}
			// 前三个房间显示出来.其他暂时隐藏.
//			if(i <= 2)
//			{
//				temp_obj.transform.localScale = Vector3.one;
//			}
		}
		m_objCurGameRoomType.transform.localScale = Vector3.one;
		Invoke("ResetRoomPosition_relax", 0.1f);
	}
	public void CreateRoom_Relax_Phone()
	{
		HallTransfer.RoomInfoS roomInfo;
		for(int i = 0; i < m_lstRoomInfoList.Count; i++)
		{
			roomInfo = m_lstRoomInfoList[i];
			GameObject temp_obj = null;
			temp_obj = (GameObject)Instantiate(m_objCurGameRoomItem, Vector3.zero, m_objCurGameRoomItem.transform.localRotation);
			Debug.LogWarning("CurGameRoomItem:" + m_objCurGameRoomItem.name);
			temp_obj.transform.name = "RoomItem" + m_lstRoomItemList.Count.ToString();
			temp_obj.transform.parent = m_objCurRoomGrid.transform;
			temp_obj.transform.localScale = Vector3.one;
			temp_obj.SetActive(true);
			UIEventListener.Get(temp_obj).onClick = GameRoom_OnClick;		
			CGameRoomItem roomItem = temp_obj.GetComponent<CGameRoomItem>();
			roomItem.UpdateRoomItem(roomInfo);
			// 休闲版独立更新游戏房间图片(暂时)
			roomItem.UpdateRoomSprite((i%4));
			m_lstRoomItemList.Add(roomItem);
		}
		Invoke("ResetRoomPosition_default", 0f);
	}
	public void CreateRoom_Golden()
	{
		HallTransfer.RoomInfoS roomInfo;
		for(int i = 0; i < 3; i++)
		{
			GameObject temp_obj = null;
			temp_obj = (GameObject)Instantiate(m_objCurGameRoomItem, Vector3.zero, m_objCurGameRoomItem.transform.localRotation);
			Debug.LogWarning("CurGameRoomItem:" + m_objCurGameRoomItem.name);
			temp_obj.transform.name = "RoomItem" + i;
			temp_obj.transform.parent = m_objCurRoomGrid.transform;
			temp_obj.transform.localPosition = new Vector3(i==1?30:0,-156*i,0);
			temp_obj.transform.localScale = Vector3.one;
			temp_obj.SetActive(true);
			CGameRoomItem roomItem = temp_obj.GetComponent<CGameRoomItem>();
			if(i<m_lstRoomInfoList.Count)
			{
				roomInfo = m_lstRoomInfoList[i];
				roomItem.UpdateRoomItem(roomInfo);
				temp_obj.GetComponent<UISprite>().spriteName = "lobby_room"+i.ToString()+"0";
				Color tempColor = Color.white;
				if(i==0)
				{
					tempColor = new Color(39f/255f,255f/255f,90f/255f);
				}else if(i==1)
				{
					tempColor = new Color(39f/255f,186f/255f,255f/255f);
				}else if(i==2)
				{
					tempColor = new Color(255f/255f,39f/255f,197f/255f);
				}
				temp_obj.transform.FindChild("room_light").GetComponent<UISprite>().color = tempColor;
				UIEventListener.Get(temp_obj).onClick = GameRoom_OnClick;
				m_lstRoomItemList.Add(roomItem);
			}else{
				temp_obj.GetComponent<UISprite>().spriteName = "lobby_room"+i.ToString()+"1";
				DestroyImmediate(temp_obj.transform.FindChild("room_light").gameObject);
			}
		}
		if(this.GetComponent<TweenPosition>()!=null)
		{
			this.GetComponent<TweenPosition>().ResetToBeginning();
			this.GetComponent<TweenPosition>().PlayForward();
		}
		this.transform.localScale = Vector3.one;
	}
	
	


	public void ResetRoomPosition_GameblePC()
	{
		if(m_objCurRoomGrid.GetComponent<UIGrid>()!=null)
		{
			if(CGameDeskManger._instance.transform.localScale == Vector3.zero || CGameManager._instance.m_bolGameBtnClicked)
			{
				transform.localScale = Vector3.one;
				CGameDeskManger._instance.transform.localScale = Vector3.zero;
			}
			for(int i = 0; i < m_lstRoomItemList.Count; i++)
			{
				if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
				{
					//水平排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3((float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellWidth),0,0);
				}else{
					//垂直排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3(0,-(float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellHeight),0);
				}
			}
			m_objCurRoomGrid.transform.localPosition = Vector3.zero;
		}else{
			//			if(GetComponent<TweenScale>()!=null) GetComponent<TweenScale>().PlayForward();
			if(CGameDeskManger._instance.transform.localScale!=Vector3.one) transform.localScale = Vector3.one;
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().SortBasedOnScrollMovement();
			if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().ResetPosition();
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().WrapContent();
			if(m_objCurGameRoomType.GetComponent<ScrollViewCtrl>()!=null) m_objCurGameRoomType.GetComponent<ScrollViewCtrl>().ScrollViewInit();
		}
		
//		for(int i = 0; i < m_lstRoomItemList.Count; i++)
//		{
//			m_lstRoomItemList[i].GetComponent<UISprite>().width = Screen.width - 307;//m_objCurGameRoomType.GetComponent<UIWidget>().width;
//		}
		m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().transform.localPosition = Vector3.zero;
		m_objCurGameRoomType.GetComponentInChildren<UIScrollBar>().value = 0;
	}
	public void ResetRoomPosition_default()
    {
		if(m_objCurRoomGrid.GetComponent<UIGrid>()!=null)
		{
			if(CGameDeskManger._instance.transform.localScale == Vector3.zero || CGameManager._instance.m_bolGameBtnClicked)
			{
				transform.localScale = Vector3.one;
				CGameDeskManger._instance.transform.localScale = Vector3.zero;
			}
//			m_objCurRoomGrid.GetComponent<UIGrid>().Reposition();
//			if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().ResetPosition();
//			m_objCurRoomGrid.GetComponent<UIGrid>().repositionNow = true;
			for(int i = 0; i < m_lstRoomItemList.Count; i++)
			{
				if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
				{
					//水平排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3((float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellWidth),0,0);
				}else{
					//垂直排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3(0,-(float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellHeight),0);
				}
			}
			m_objCurRoomGrid.transform.localPosition = Vector3.zero;
		}else{
//			if(CGameDeskManger._instance.transform.localScale!=Vector3.one) transform.localScale = Vector3.one;
//			for(int i = 0; i < m_lstRoomItemList.Count; i++)
//			{
//				if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
//				{
//					//水平排列
//					m_lstRoomItemList[i].transform.localPosition = new Vector3((float)(i*m_objCurRoomGrid.GetComponent<UIWrapContent>().itemSize),0,0);
//				}else{
//					//垂直排列
//					m_lstRoomItemList[i].transform.localPosition = new Vector3(0,-(float)(i*m_objCurRoomGrid.GetComponent<UIWrapContent>().itemSize),0);
//				}
//			}
//			m_objCurGameRoomType.GetComponentInChildren<UIPanel>().clipOffset = Vector2.zero;
//			m_objCurGameRoomType.GetComponentInChildren<UIPanel>().transform.localPosition = Vector3.zero;
			if(CGameDeskManger._instance.transform.localScale!=Vector3.one) transform.localScale = Vector3.one;
			m_objCurRoomGrid.transform.localPosition = Vector3.zero;
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().SortBasedOnScrollMovement();
			if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().ResetPosition();
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().WrapContent();
			if(m_objCurGameRoomType.GetComponent<ScrollViewCtrl>()!=null) m_objCurGameRoomType.GetComponent<ScrollViewCtrl>().ScrollViewInit();
		}
    }
	public void ResetRoomPosition_relax()
	{
		if(CGameDeskManger._instance.transform.localScale == Vector3.zero || CGameManager._instance.m_bolGameBtnClicked)
		{
			transform.localScale = Vector3.one;
			CGameDeskManger._instance.transform.localScale = Vector3.zero;
		}
		CGameManager._instance.m_objGameScrollView.transform.localScale = Vector3.zero;

//		m_lstRoomItemList
//		m_gRecordParent.transform.localPosition = new Vector3(0, 0, 0);

		creatRoom_Releax._instance.ResetRoomPosition_relax(m_lstRoomItemList);
	}
	public void ResetRoomPosition_relax_Phone()
	{
		if(m_objCurRoomGrid.GetComponent<UIGrid>()!=null)
		{
			if(CGameDeskManger._instance.transform.localScale == Vector3.zero || CGameManager._instance.m_bolGameBtnClicked)
			{
				transform.localScale = Vector3.one;
				CGameDeskManger._instance.transform.localScale = Vector3.zero;
			}
			
			for(int i = 0; i < m_lstRoomItemList.Count; i++)
			{
				if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
				{
					//水平排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3((float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellWidth),0,0);
				}else{
					//垂直排列
					m_lstRoomItemList[i].transform.localPosition = new Vector3(0,-(float)(i*m_objCurRoomGrid.GetComponent<UIGrid>().cellHeight),0);
				}
			}
			m_objCurRoomGrid.GetComponent<UIGrid>().enabled = true;
			m_objCurRoomGrid.transform.localPosition = Vector3.zero;
		}else{
			//			if(GetComponent<TweenScale>()!=null) GetComponent<TweenScale>().PlayForward();
			if(CGameDeskManger._instance.transform.localScale!=Vector3.one) transform.localScale = Vector3.one;
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().SortBasedOnScrollMovement();
			if(m_objCurGameRoomType.GetComponentInChildren<UIScrollView>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIScrollView>().ResetPosition();
			if(m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>()!=null) m_objCurGameRoomType.GetComponentInChildren<UIWrapContent>().WrapContent();
			if(m_objCurGameRoomType.GetComponent<ScrollViewCtrl>()!=null) m_objCurGameRoomType.GetComponent<ScrollViewCtrl>().ScrollViewInit();
		}
		//m_objCurRoomGrid.transform.localPosition = Vector3.zero;
		CGameManager._instance.m_objGameScrollView.transform.localScale = Vector3.zero;
	}

    /// <summary>
    /// 进房间按钮响应
    /// </summary>
	void GameRoom_OnClick(GameObject _gameRoomItem)
    {
		if(CGameDeskManger._instance.m_lblRoomName!=null) CGameDeskManger._instance.m_lblRoomName.text = _gameRoomItem.GetComponent<CGameRoomItem>().m_lblRoomNameLabel.text;
		HallTransfer.Instance.ncGameRoomClick(_gameRoomItem.GetComponent<CGameRoomItem>().m_cGameInfo.roomId);
    }
    /// <summary>
    /// 快速进入游戏
    /// </summary>
    public void QuickEnterGame_OnClick()
    {

    }
	public void SetTipRoomName(uint tempRoomId)
	{
		for(int i = 0; i < m_lstRoomInfoList.Count; i++)
		{
			if(tempRoomId == m_lstRoomInfoList[i].roomId)
			{
				if(CGameDeskManger._instance.m_lblRoomName!=null)
				{
					CGameDeskManger._instance.m_lblRoomName.text = m_lstRoomInfoList[i].roomName;
					return;
				}
			}
		}
	}

}
