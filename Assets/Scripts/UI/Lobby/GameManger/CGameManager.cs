using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGameManager : MonoBehaviour {

    public static CGameManager _instance;
	// 预设场景中游戏按钮的位置
	public Vector3[] m_gameItemPoss;

    /// <summary>
    /// 游戏列表
    /// </summary>
    public List<CGameItem> m_lstGameItemList = new List<CGameItem>();
    /// <summary>
    /// 游戏预设
    /// </summary>
    public GameObject m_objGameItemPrefab;
    /// <summary>
    /// 管理父节点
    /// </summary>
//    public GameObject m_objGameItemParent;

	// 是否加载休闲版场景
	public bool RelaxScene_switch;

	// 游戏列表父节点
    public GameObject m_objGameGrid;
	// 整个游戏列表的父节点(用于点击进入游戏时候隐藏列表)
	public GameObject m_objGameScrollView;
	// 游戏列表当前页数
	public int m_gameListCurPage = 0;
	// 游戏列表页数的最大值
	public int m_gameListMaxPage = 0;
	// 休闲版的游戏列表(比金色版本多出空白列表)
	public List<GameObject> m_lstGameItemList_Relax = new List<GameObject>();

	public GameObject m_objFirstPage;

	public string m_objCurGameId;

	public bool m_bolGameBtnClicked=false;

	public List<HallTransfer.GameInfoS> m_lstGameInfoList = new List<HallTransfer.GameInfoS>();
	/// 是否创建6个游戏(false创建9个)
	public bool m_bIsCreatSixGames = true;
	/// 一页创建几个游戏(6个/9个)
	public int m_iCreatGamesNum = 0;

    void Awake()
    {
        _instance = this;
		MousePointerCtr tempMouse = HallTransfer.Instance.GetComponent<MousePointerCtr>();
		if(tempMouse != null) tempMouse.Lobby_SetMouse_exit();//还原鼠标
    }

	void Start()
	{
		if(m_bIsCreatSixGames){
			m_iCreatGamesNum = 6;
			m_objGameGrid.transform.localScale = Vector3.one;
		}else{
			m_iCreatGamesNum = 9;
			m_objGameGrid.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
	}

	void OnDestroy()
    {
        _instance = null;
    }

    /// <summary>
    /// 添加游戏
    /// </summary>
    /// <param name="_gameInfo">游戏信息结构体</param>
    public void AddGameInfo(HallTransfer.GameInfoS _gameInfo)
    {
        GameObject temp_obj = (GameObject)Instantiate(m_objGameItemPrefab, Vector3.zero, m_objGameItemPrefab.transform.localRotation);
        temp_obj.transform.name = "GameItem" + m_lstGameItemList.Count.ToString();
        
		if(m_objGameGrid.GetComponent<UIGrid>()!=null) m_objGameGrid.GetComponent<UIGrid>().repositionNow = true;
		temp_obj.transform.parent = m_objGameGrid.transform;
		temp_obj.transform.localScale = Vector3.zero;
        CGameItem gameItem = temp_obj.GetComponent<CGameItem>();

		
        UIEventListener.Get(gameItem.gameObject).onClick = GameBtn_OnClick;
        gameItem.UpdateGameInfo(_gameInfo);
        m_lstGameItemList.Add(gameItem);
    }

	/// <summary>
	/// 添加游戏_休闲版
	/// </summary>
	/// <param name="_gameInfo">游戏信息结构体</param>
	public void AddGameInfo_Relax(HallTransfer.GameInfoS _gameInfo, int i)
	{
		GameObject temp_obj = (GameObject)Instantiate(m_objGameItemPrefab, Vector3.zero, m_objGameItemPrefab.transform.localRotation);
		temp_obj.transform.name = "GameItem" + i.ToString();
		temp_obj.transform.parent = m_objGameGrid.transform;
		i = (i % m_iCreatGamesNum);
		if(m_bIsCreatSixGames)
		{			
			temp_obj.transform.localPosition = m_gameItemPoss[i];
		}else{
			temp_obj.transform.localPosition = m_gameItemPoss[i+6];
		}
		temp_obj.transform.localScale = Vector3.zero;
		//temp_obj.SetActive(false);
		CGameItem gameItem = temp_obj.GetComponent<CGameItem>();
		
		
		UIEventListener.Get(gameItem.gameObject).onClick = GameBtn_OnClick;
		gameItem.UpdateGameInfo(_gameInfo);
		m_lstGameItemList.Add(gameItem);

		m_lstGameItemList_Relax.Add(temp_obj);
	}
	public void AddGameInfo_Relax(int i)
	{
		GameObject temp_obj = (GameObject)Instantiate(m_objGameItemPrefab, Vector3.zero, m_objGameItemPrefab.transform.localRotation);
		temp_obj.transform.name = "GameItem" + i.ToString();
		temp_obj.transform.parent = m_objGameGrid.transform;
		i = (i % m_iCreatGamesNum);
		if(m_bIsCreatSixGames)
		{
			temp_obj.transform.localPosition = m_gameItemPoss[i];
		}else{
			temp_obj.transform.localPosition = m_gameItemPoss[i+6];
		}
		temp_obj.transform.localScale = Vector3.zero;
		//temp_obj.SetActive(false);
		
		if(temp_obj.GetComponent<CGameItem>().m_sprGameicoSprite != null)
		{
			temp_obj.GetComponent<CGameItem>().m_sprGameicoSprite.spriteName = "gameico_null";
		}
		if(temp_obj.GetComponent<CGameItem>().m_sprHoticoSprite != null)
		{
			temp_obj.GetComponent<CGameItem>().m_sprHoticoSprite.gameObject.SetActive(false);
		}

		m_lstGameItemList_Relax.Add(temp_obj);
	}

	// 点击左侧按钮
	public void LeftBtnOnClick()
	{
		// 每次点击按钮,页数-1,并且判断是否小于等于0
		m_gameListCurPage -= 1;
		if(m_gameListCurPage >= 0)
		{
			if(m_bIsCreatSixGames)
			{				
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏上一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListCurPage + 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i].x-500,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i].x,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}else{				
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏上一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListCurPage + 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i+6].x-500, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}

			creatGamePageLogo._instance.UpdatePageLogo(m_gameListCurPage+1, m_gameListCurPage);
		}else{
			m_gameListCurPage = m_gameListMaxPage - 1;
			if(m_bIsCreatSixGames)
			{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏上一页的GameLogo
					m_lstGameItemList_Relax[i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i].x-500,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i].x,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}else{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏上一页的GameLogo
					m_lstGameItemList_Relax[i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i+6].x-500, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}

			creatGamePageLogo._instance.UpdatePageLogo(0, m_gameListCurPage);
		}
	}

	//点击右侧按钮
	public void RightBtnOnClick()
	{
		
		// 每次点击按钮,页数+1,并且判断是否超过最大页数
		m_gameListCurPage += 1;
		if(m_gameListCurPage <= m_gameListMaxPage - 1)
		{
			if(m_bIsCreatSixGames)
			{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏下一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListCurPage - 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i].x+500,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i].x,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}else{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏下一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListCurPage - 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].SetActive(true);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i+6].x+500, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[(m_gameListCurPage * m_iCreatGamesNum) + i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}

			creatGamePageLogo._instance.UpdatePageLogo(m_gameListCurPage - 1 , m_gameListCurPage);
		}else{
			m_gameListCurPage = 0;
			if(m_bIsCreatSixGames)
			{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏下一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListMaxPage - 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[i].SetActive(true);
					m_lstGameItemList_Relax[i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i].x+500,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i].x,m_gameItemPoss[i].y,m_gameItemPoss[i].z);
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}else{
				for(int i = 0; i < m_iCreatGamesNum; i++)
				{
					// 隐藏下一页的GameLogo
					m_lstGameItemList_Relax[((m_gameListMaxPage - 1) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
					// 显示当前页的GameLogo
					m_lstGameItemList_Relax[i].SetActive(true);
					m_lstGameItemList_Relax[i].transform.localScale = Vector3.one;
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().from = new Vector3(m_gameItemPoss[i+6].x+500, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().enabled = true;
					m_lstGameItemList_Relax[i].GetComponent<TweenPosition>().ResetToBeginning();
				}
			}

			creatGamePageLogo._instance.UpdatePageLogo(m_gameListMaxPage - 1 , 0);
		}
	}

    public void FindLastDesk()
    {
		if(m_objGameGrid.GetComponent<UIGrid>()!=null)
		{
			for(var i = 0; i < m_lstGameItemList.Count; i++)
			{		
				if(m_lstGameItemList[i].GetComponent<TweenScale>()!=null)
				{
					m_lstGameItemList[i].GetComponent<TweenScale>().ResetToBeginning();
					m_lstGameItemList[i].GetComponent<TweenScale>().Play();
				}else{
					m_lstGameItemList[i].transform.localScale = Vector3.one;
				}

				if(m_objGameGrid.GetComponentInParent<UIScrollView>() != null)
				{
					if(m_objGameGrid.GetComponentInParent<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
					{
						//水平排列
						m_lstGameItemList[i].transform.localPosition = new Vector3((float)(i*m_objGameGrid.GetComponent<UIGrid>().cellWidth),0,0);
					}else{
						//垂直排列
						m_lstGameItemList[i].transform.localPosition = new Vector3(0,-(float)(i*m_objGameGrid.GetComponent<UIGrid>().cellHeight),0);
					}
				}
				// 如果是休闲版
				if(RelaxScene_switch)
				{
					if(i >= m_iCreatGamesNum) 
					{
						m_lstGameItemList[i].transform.localScale = Vector3.zero; 
					}
				}
			}
			m_objGameGrid.transform.localPosition = Vector3.zero;
			if(GetComponentInChildren<ScrollViewCtrl>()!=null)
			{
				GetComponentInChildren<UIScrollView>().ResetPosition();
			}
			m_objGameGrid.GetComponent<UIGrid>().Reposition();
			if(m_objCurGameId!="")
			{
				for(int i = 0; i < m_lstGameItemList.Count; i++)
				{
					if(m_objCurGameId == m_lstGameInfoList[i].ID.ToString())
					{
						if(GetComponentInChildren<UIScrollBar>()!=null)
						{
							if(GetComponentInChildren<UIScrollBar>()!=null)
								GetComponentInChildren<UIScrollBar>().value = (float)i/(float)m_lstGameInfoList.Count;
							m_lstGameItemList[i].GetComponent<UIToggle>().value = true;
						}
						break;
					}
				}
			}
		}else{
			if(m_objGameGrid.GetComponent<UIWrapContent>() !=null)
			{
				m_objGameGrid.GetComponent<UIWrapContent>().SortBasedOnScrollMovement();
			}
//			CGameItem[] tempGameBtn = m_GridSprite.transform.GetComponentsInChildren<CGameItem>();

			for(var i = 0; i < m_lstGameItemList.Count; i++)
			{
				if(m_objGameGrid.GetComponentInParent<UIScrollView>() != null)
				{
					if(m_objGameGrid.GetComponentInParent<UIScrollView>().movement == UIScrollView.Movement.Horizontal)
					{
						//水平排列
						m_lstGameItemList[i].transform.localPosition = new Vector3((float)(i*m_objGameGrid.GetComponent<UIWrapContent>().itemSize),0,0);
					}else{
						//垂直排列
						m_lstGameItemList[i].transform.localPosition = new Vector3(0,(float)(i*m_objGameGrid.GetComponent<UIWrapContent>().itemSize),0);
					}
				}
				if(m_lstGameItemList[i].GetComponent<TweenScale>()!=null)
				{
					m_lstGameItemList[i].GetComponent<TweenScale>().ResetToBeginning();
					m_lstGameItemList[i].GetComponent<TweenScale>().Play();
				}else{
					m_lstGameItemList[i].transform.localScale = Vector3.one;
				}
				// 如果是休闲版
				if(RelaxScene_switch)
				{
					if(i >= m_iCreatGamesNum)
					{
						m_lstGameItemList[i].transform.localScale = Vector3.zero;
					}
				}
			}
		}
    }
    /// <summary>
    /// 创建游戏列表
    /// </summary>
    public void SetGameList(List<HallTransfer.GameInfoS> gamelist)
    {
		Debug.LogWarning("SetGameList:  " + gamelist.Count.ToString());
		m_lstGameInfoList = gamelist;
		// 休闲版才计算页数并且生成页面Logo
		if(RelaxScene_switch)
		{			
			if((gamelist.Count % m_iCreatGamesNum) == 0)
			{				
				m_gameListMaxPage = (gamelist.Count /m_iCreatGamesNum);
			}else{				
				m_gameListMaxPage = (gamelist.Count /m_iCreatGamesNum) + 1;
			}
			creatGamePageLogo._instance.CreatPageLogo(m_gameListMaxPage);
		}
        //清空列表
		foreach (Transform child in m_objGameGrid.transform)
        {
            if (child != null)
            {
                Destroy(child.gameObject,0);
            }
        }
        m_lstGameItemList.Clear();

		m_lstGameItemList_Relax.Clear();
		// 判断是否休闲版场景
		if(RelaxScene_switch)
		{
			for (int i = 0; i < (m_gameListMaxPage * m_iCreatGamesNum); i++)
			{
				// 如果超出游戏列表范围,则加载空白游戏列表
				if(i < gamelist.Count)
				{
					AddGameInfo_Relax(gamelist[i], i);
				}
				else
				{
					AddGameInfo_Relax(i);
				}
			}
		}
		else
		{
			for (int i = 0; i < gamelist.Count; i++)
			{
				AddGameInfo(gamelist[i]);
			}
		}
        
		Invoke("FindLastDesk", 0.1f);
		if(CGameDeskManger._instance.m_ReturnBtn!=null)
		{
			UIEventListener.Get(CGameDeskManger._instance.m_ReturnBtn.gameObject).onClick = GameBtn_OnClick;
           
		}
    }
    /// <summary>
    /// 游戏按钮响应
    /// </summary>
    /// <param name="_gameItem">游戏信息结构体</param>
	void GameBtn_OnClick(GameObject _gameItem)
    {
        for (int i = 0; i < m_lstGameItemList.Count; i++)
        {
            if (_gameItem.GetComponent<CGameItem>() == m_lstGameItemList[i])
            {
                m_lstGameItemList[i].SetCheckedItemUI();
            }
            else
            {
                m_lstGameItemList[i].SetNormalGameItemUI();
            }
        }
        //发消息请求房间列表
		if (CGameRoomManger._instance.m_objCurRoomGrid != null)
        {
            //房间列表
			foreach (Transform child in CGameRoomManger._instance.m_objCurRoomGrid.transform)
            {
                if (child != null)
                {
                    Destroy(child.gameObject, 0);
                }
            }
        }
		if(HallTransfer.Instance.ncGameBtnClick != null)
		{
			m_bolGameBtnClicked = true;
			//发送gameid
			m_objCurGameId = _gameItem.GetComponent<CGameItem>().m_cGameInfo.ID.ToString();
			if(CGameRoomManger._instance.m_lblGameName!=null) CGameRoomManger._instance.m_lblGameName.text = _gameItem.GetComponent<CGameItem>().m_cGameInfo.Name;
			HallTransfer.Instance.ncGameBtnClick(_gameItem.GetComponent<CGameItem>().m_cGameInfo.ID);

			UIEventListener.Get(CGameDeskManger._instance.m_ReturnBtn.gameObject).onClick = GameBtn_OnClick;
			HallTransfer.GameInfoS _gameInfo = new HallTransfer.GameInfoS();
			_gameInfo.ID = _gameItem.GetComponent<CGameItem>().m_cGameInfo.ID;
			_gameInfo.Name = _gameItem.GetComponent<CGameItem>().m_cGameInfo.Name;
			CGameDeskManger._instance.m_ReturnBtn.UpdateGameInfo(_gameInfo);
		}
    }
	public void SetTipGameName(uint tempGameId)
	{
		for(int i = 0; i < m_lstGameInfoList.Count; i++)
		{
			if(tempGameId == m_lstGameInfoList[i].ID)
			{
				if(CGameRoomManger._instance.m_lblGameName!=null)
				{
					CGameRoomManger._instance.m_lblGameName.text = m_lstGameInfoList[i].Name;
					return;
				}
			}
		}
	}
}
