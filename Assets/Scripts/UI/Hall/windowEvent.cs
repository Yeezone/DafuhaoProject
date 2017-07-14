using UnityEngine;

public class windowEvent : MonoBehaviour {

	public	bool		officialSite = false;
	public	bool		miniSize = false;
	public	bool		maxSize = false;
	public	bool		changeAccount = false;
	public	bool		closeWindow = false;

	public	int			scWidth = 0;
	public	int			scHeight = 0;
	private	bool		resized = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!resized)
		{
            if (scWidth != Screen.width || scHeight != Screen.height)
			{
				Invoke("resetContent",0.01f);
				resized = true;
//				resetContent();
			}
		}
	}

	void OnClick () {
		if(officialSite)
		{
			//官方网站
			HallTransfer.Instance.ncOfficialSite();

		}else if(miniSize)
		{
			//最小化
			if(HallTransfer.Instance.ncMiniWindow != null)
			{
				HallTransfer.Instance.ncMiniWindow();
			}

		}else if(maxSize)
		{
			//最大化
			scWidth = Screen.width;
			scHeight = Screen.height;
			HallTransfer.Instance.ncMaxWindow();
			resized = false;
//			Invoke("resetContent",0.5f);
		}else if(changeAccount){
			//切换账号
			HallTransfer.Instance.ncChangeAccount();
		}else if(closeWindow)
		{
			//退出游戏
			if(!HallTransfer.Instance.uiConfig.BackToGame){
				HallTransfer.Instance.ncCloseHall();
			}
			HallTransfer.Instance.uiConfig.BackToGame = false;
		}
	}

	void resetContent()
	{
		Debug.LogWarning ("resetContent");
		HallTransfer.Instance.resetGamesScrollView();//重置游戏列表scrollview
		if(HallTransfer.Instance.uiConfig.page_roomDesk.activeSelf) HallTransfer.Instance.deskReposition();//重置桌子列表scrollview
		if(HallTransfer.Instance.uiConfig.page_gameRoom.activeSelf) HallTransfer.Instance.resetRoomScrollView();//重置房间列表scrollview
	}
}
