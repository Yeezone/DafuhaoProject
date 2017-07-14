using UnityEngine;

public class returnGameBtnClick : MonoBehaviour {


	public	bool			reLogin = false;
	public	uint			roomid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	void OnClick ()
	{
		if(reLogin)
		{
			//重新连接
			if(roomid != 0) HallTransfer.Instance.ncGameRoomClick(roomid);
			HallTransfer.Instance.uiConfig.window_ReloginMsgBox.SetActive(false);
		}else{
			//取消
			HallTransfer.Instance.uiConfig.window_ReloginMsgBox.SetActive(false);
		}
	}

}
