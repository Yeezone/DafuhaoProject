using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 休闲版大厅排列游戏房间
/// </summary>
public class creatRoom_Releax : MonoBehaviour {

	public static creatRoom_Releax _instance;

	// 父节点
	public GameObject m_objCurRoomGrid;
	// 左右按钮
	public Transform m_objLeftBtn;
	public Transform m_objRightBtn;
	// SpringPosition组件
	public SpringPosition m_springPos;
	// 每个房间的间距
	public float m_fSpacing;
	// 每个房间的间距(两个房间的单独配置)
	public float m_fSpacing_twoRoom;
	// 父节点的初始位置.
	public Vector3 m_v3RoomGridPos;

	void Awake()
	{
		_instance = this;
	}
	void OnDestroy()
	{
		_instance = null;
	}

	void Start ()
	{

	}


	void Update () {
	
	}

	public void ResetRoomPosition_relax(List<CGameRoomItem>  m_lstRoomItemList)
	{		
		if(m_lstRoomItemList.Count >0 && m_lstRoomItemList.Count <=1)
		{
			m_lstRoomItemList[0].transform.localPosition = new Vector3(230, 0, 0);
			//m_lstRoomItemList[0].GetComponentInChildren<UIDragScrollView>().enabled = false;

			m_objCurRoomGrid.transform.localPosition = m_v3RoomGridPos;
			m_objCurRoomGrid.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_lstRoomItemList.Count == 2)
		{
			for (int i = 0; i < m_lstRoomItemList.Count; i++)
			{
				m_lstRoomItemList[i].transform.localPosition = new Vector3((float)(m_lstRoomItemList.Count - i -1) * m_fSpacing_twoRoom + 100, 0, 0);
				//m_lstRoomItemList[i].GetComponentInChildren<UIDragScrollView>().enabled = false;
			}

			m_objCurRoomGrid.transform.localPosition = m_v3RoomGridPos;
			m_objCurRoomGrid.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_lstRoomItemList.Count >= 3)
		{
			for (int i = 0; i < m_lstRoomItemList.Count; i++)
			{
				m_lstRoomItemList[i].transform.localPosition = new Vector3((float)i * m_fSpacing, 0, 0);
			}

			m_objCurRoomGrid.transform.localPosition = m_v3RoomGridPos;
			m_objCurRoomGrid.GetComponent<BoxCollider>().enabled = true;
		}	

		// 检测当前游戏列表是否大于3个.
		if(m_lstRoomItemList.Count > 3)
		{			
			m_objLeftBtn.localScale = Vector3.one;
			m_objRightBtn.localScale = new Vector3(-1,1,1);
		}else{
			
			m_objLeftBtn.localScale = Vector3.zero;
			m_objRightBtn.localScale = Vector3.zero;
		}
	}

	// 左侧按钮
	public void GameRoom_LeftBtn()
	{		
		Vector3 tempos = m_objCurRoomGrid.transform.localPosition;
		tempos.x += m_fSpacing;

		if (tempos.x >= 25)
		{
			m_springPos = SpringPosition.Begin(m_objCurRoomGrid.gameObject, new Vector3(25 , 0, 0), 13f);
			m_springPos.enabled = true;
			m_springPos.worldSpace = false;
		}else{
			m_objCurRoomGrid.GetComponent<TweenPosition>().from = m_objCurRoomGrid.transform.localPosition;
			m_objCurRoomGrid.GetComponent<TweenPosition>().to = tempos;
			m_objCurRoomGrid.GetComponent<TweenPosition>().ResetToBeginning();
			m_objCurRoomGrid.GetComponent<TweenPosition>().enabled = true;
		}
	}
	
	// 右侧按钮
	public void GameRoom_RightBtn()
	{
		Vector3 tempos = m_objCurRoomGrid.transform.localPosition;
		tempos.x -= m_fSpacing;

		if (tempos.x <= -185)
		{
			m_springPos = SpringPosition.Begin(m_objCurRoomGrid.gameObject, new Vector3(-185 , 0, 0), 13f);
			m_springPos.enabled = true;
			m_springPos.worldSpace = false;
		}else{
			m_objCurRoomGrid.GetComponent<TweenPosition>().from = m_objCurRoomGrid.transform.localPosition;
			m_objCurRoomGrid.GetComponent<TweenPosition>().to = tempos;
			m_objCurRoomGrid.GetComponent<TweenPosition>().ResetToBeginning();
			m_objCurRoomGrid.GetComponent<TweenPosition>().enabled = true;
		}
	}

}
