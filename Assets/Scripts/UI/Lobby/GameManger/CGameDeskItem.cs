using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CGameDeskItem : MonoBehaviour {

    [System.Serializable]
    public class CGameDeskInfo
    {
        public uint dwGameID;           ///游戏ID
        public uint dwRoomID;			///房间ID
		public uint dwDeskID;			///桌子ID
        public uint dwDeskCount;		///桌子数
        public uint dwDeskPeople;		///每桌人数
    }

    //游戏桌子信息
    public CGameDeskInfo m_cGameDeskInfo = new CGameDeskInfo();

	public List<HallTransfer.RoomUserInfo> m_lstUserInfos = new List<HallTransfer.RoomUserInfo>();

    public List<CGameChairItem> m_ChairItems = new List<CGameChairItem>();
	public UILabel m_lblDeskIndex;
	public UILabel m_lblDeskName;
	public UILabel m_lblManCount;
	public UISlider m_PeopleSlider;
	public UILabel m_lblDeskState;
	public GameObject m_objGaming;

    /// <summary>
    /// 更新游戏桌子信息
    /// </summary>
    /// <param name="_gameTableInfo"></param>
    public void UpdateGameDeskInfo(int deskID,int curManCount,HallTransfer.RoomDeskInfo _gameTableInfo)
    {
        m_cGameDeskInfo.dwDeskCount = _gameTableInfo.dwDeskCount;
        m_cGameDeskInfo.dwDeskPeople = _gameTableInfo.dwDeskPeople;
        m_cGameDeskInfo.dwGameID = _gameTableInfo.dwGameID;
        m_cGameDeskInfo.dwRoomID = _gameTableInfo.dwRoomId;
        m_cGameDeskInfo.dwDeskID = (uint)deskID;
		if(m_lblDeskIndex!=null) m_lblDeskIndex.text = (deskID+1).ToString();
		if(CChairManger._instance!=null)
		{
			if(CChairManger._instance.m_lRoomUChairList.Count>0)
			{
				if((uint)deskID == CChairManger._instance.m_lRoomUChairList[0].deskid)
				{
					if(m_lblDeskName!=null) m_lblDeskName.text = (deskID+1).ToString()+"号桌";
				}
			}
		}else{
			if(m_lblDeskName!=null) m_lblDeskName.text = (deskID+1).ToString()+"号桌";
		}
		if(m_lblManCount!=null) m_lblManCount.text = curManCount.ToString() + "/" + _gameTableInfo.dwDeskPeople.ToString();
		if(m_PeopleSlider!=null) m_PeopleSlider.value = ((float)curManCount/(float)_gameTableInfo.dwDeskPeople);
		if(m_lblDeskState!=null)
		{
			if(m_lblDeskState.text == "游戏中")
			{

			}else if(curManCount>=(int)_gameTableInfo.dwDeskPeople)
			{
				m_lblDeskState.text = "满员";
			}else{
				m_lblDeskState.text = "可进入";
			}
		}
    }

	public void OnClick()
	{
		if(m_lblDeskName!=null) m_lblDeskName.text = (m_cGameDeskInfo.dwDeskID+1).ToString()+"号桌";
		if(GetComponent<UIToggle>()!=null) GetComponent<UIToggle>().value = true;
		if(CGameDeskManger._instance.m_objCurDesk_phone!=null)
		{
			CGameDeskManger._instance.m_objCurDesk_phone.GetComponent<CChairManger>().UpdateChairInfo(m_cGameDeskInfo.dwDeskID,m_lstUserInfos);
		}
    }
}
