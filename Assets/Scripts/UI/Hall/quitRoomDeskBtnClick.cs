using UnityEngine;

public class quitRoomDeskBtnClick : MonoBehaviour {

	void OnClick ()
	{
//		HallTransfer.Instance.cnComfirmQuitRoom ();
		HallTransfer.Instance.ncQuitRoomDesk();
		quickRoom();
	}

	public void quickRoom()
	{
		//Debug.LogWarning ("quitRoom");
		if(CGameDeskManger._instance.transform.localScale == Vector3.one)
		{
			CGameDeskManger._instance.transform.localScale = Vector3.zero;
			CGameRoomManger._instance.transform.localScale = Vector3.zero;
		}
		if(CGameManager._instance.m_objFirstPage!=null)
		{
			CGameManager._instance.m_objFirstPage.SetActive(true);
		}
	}

}
