using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CChairManger : MonoBehaviour {

	public static CChairManger _instance;

	public List<CGameChairItem> m_lRoomUChairList = new List<CGameChairItem>();

	void Awake()
	{
		_instance = this;
	}
	void OnDestroy()
	{
		_instance = null;
	}

	public void UpdateChairInfo(uint deskid, List<HallTransfer.RoomUserInfo> _lRoomUserInfoList)
	{
		RectAllChair();
		for(int i = 0; i < m_lRoomUChairList.Count; i++)
		{
			m_lRoomUChairList[i].deskid = deskid;
			m_lRoomUChairList[i].chairid = (uint)i;
		}
		for(int i = 0;i<_lRoomUserInfoList.Count;i++)
		{
			int tempChairID = (int)_lRoomUserInfoList[i].dwChair;
			m_lRoomUChairList[tempChairID].UserSitDown(_lRoomUserInfoList[i]);
		}
	}
	public void RectAllChair()
	{
		for(int i= 0;i<m_lRoomUChairList.Count;i++)
		{
			m_lRoomUChairList[i].ClearUserInfo();
		}
	}

}
