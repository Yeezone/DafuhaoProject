using UnityEngine;
using System.Collections;

public class UIUpdateChild : MonoBehaviour {

	public	bool			UpdateChildren;
	public	bool			RoomInfoOutSide;
	public	gameRoomClick	RoomInfo;
	public	float			RoomMaxSizeOffsetPositionX;
	public	float			OutRoomWorldPositionX;

	private	float			outPositionApproach = 0.2f;
	private	CGameRoomItem	tempRoom = null;
	private	int				tempIndex = -1;

	public void Init()
	{
		tempIndex = -1;
	}
	// Update is called once per frame
	void Update () {
		if(GetComponentsInChildren<Transform>().Length <= 1) return;
		if(RoomInfoOutSide)
		{
			for(int i = 0; i < CGameRoomManger._instance.m_lstRoomItemList.Count; i++)
			{
				float tempX = CGameRoomManger._instance.m_lstRoomItemList[i].transform.position.x;
				if(tempX+outPositionApproach >= OutRoomWorldPositionX && tempX-outPositionApproach <= OutRoomWorldPositionX)
				{
					if(tempIndex == i) return;
					tempRoom = CGameRoomManger._instance.m_lstRoomItemList[i].GetComponent<CGameRoomItem>();
					RoomInfo.SetRoomInfo(tempRoom.m_cGameInfo.roomId,tempRoom.m_cGameInfo.roomName,
					                     tempRoom.m_cGameInfo.roomPeopleCnt.ToString(),tempRoom.m_cGameInfo.roomDifen.ToString(),
					                     tempRoom.m_cGameInfo.roomRuchang.ToString());
					tempIndex = i;
				}
			}
		}
		if(UpdateChildren){
			for(int i = 0; i < CGameRoomManger._instance.m_lstRoomItemList.Count; i++)
			{
				float tempPosition = (Mathf.Abs(CGameRoomManger._instance.m_lstRoomItemList[i].transform.position.x/2)>1f?
				                      1f:Mathf.Abs(CGameRoomManger._instance.m_lstRoomItemList[i].transform.position.x/2));
				float tempScale0 = (1f - tempPosition<0.6f?0.6f:1f - tempPosition);
				tempScale0 = (tempScale0>1f?1f:tempScale0);
				CGameRoomManger._instance.m_lstRoomItemList[i].transform.localScale = new Vector2( tempScale0,tempScale0);
			}
		}
	}
}
