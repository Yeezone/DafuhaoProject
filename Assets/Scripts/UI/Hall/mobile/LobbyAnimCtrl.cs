using UnityEngine;
using System.Collections;

public class LobbyAnimCtrl : MonoBehaviour {

//	private	UIPlaySound			AnimAudioSource;
	private	GameObject			Page_FishGameRoom;		//捕鱼类选游戏房间页面
	private	GameObject			Page_FishGameDesk;		//残类选座位页面
	private GameObject			Page_Games;				//选游戏页面
	private GameObject			Page_Desks;				//选桌子模块
	private GameObject			BottomBar;				//下栏
	private GameObject			SwitchBtn;				//退出按钮
	private GameObject			SwitchBtnCopy;			//退出按钮_副本
	private	GameObject			FirstGirl;				//大厅首页妹子
	private GameObject			RoomReturnBtn;			//房间页面返回按钮
	private GameObject			RoomReturnBtnCopy;		//房间返回按钮_副本
	private GameObject			DeskReturnBtn;			//座位页面返回按钮
	private GameObject			DeskReturnBtnCopy;		//座位返回按钮_副本
	private GameObject			GameLogo;				//游戏LOGO
	private GameObject			RoomLogo;				//房间LOGO



	// Use this for initialization
	void Start () {
//		AnimAudioSource = GetComponent<UIPlaySound>();
		Page_FishGameRoom = HallTransfer.Instance.uiConfig.page_gameRoom;
		Page_FishGameDesk = HallTransfer.Instance.uiConfig.page_roomDesk;
		BottomBar = GameObject.Find("BottomBar");
		Page_Games = GameObject.Find("Games");
//		Page_Desks = Page_FishGameRoom.transform.FindChild("Desks").gameObject;
//		FirstGirl = GameObject.Find("FirstGirl");
//		SwitchBtn = GameObject.Find("Title").transform.FindChild("switch_btn").gameObject;
//		SwitchBtnCopy = SwitchBtn.transform.parent.FindChild("switch_copy_img").gameObject;
//		RoomReturnBtn = GameObject.Find("Title").transform.FindChild("return0_btn").gameObject;
//		RoomReturnBtnCopy = RoomReturnBtn.transform.parent.FindChild("return0_copy_img").gameObject;
//		DeskReturnBtn = GameObject.Find("Title").transform.FindChild("return1_btn").gameObject;
//		DeskReturnBtnCopy = DeskReturnBtn.transform.parent.FindChild("return1_copy_img").gameObject;
		GameLogo = GameObject.Find("GameLogo");
		RoomLogo = GameObject.Find("RoomLogo");
	}

	public void LobbyStart()
	{

	}
	public void EnterGameRoom()
	{
		//房间显示
		Page_FishGameRoom.GetComponent<TweenAlpha>().PlayForward();
		//游戏界面隐藏
		Page_Games.GetComponent<TweenAlpha>().PlayReverse();
		FirstGirl.GetComponent<TweenPosition>().PlayReverse();
		BottomBar.GetComponent<TweenPosition>().PlayReverse();
		//退出按钮隐藏
		SwitchBtn.SetActive(false);
		RoomReturnBtn.SetActive(true);
		//游戏LOGO隐藏
//		GameLogo.GetComponent<TweenRotation>().ResetToBeginning();
		GameLogo.GetComponent<TweenRotation>().PlayForward();
		RoomLogo.GetComponent<TweenRotation>().ResetToBeginning();
	}
	public void QuitGameRoom()
	{
		if(Page_Desks.transform.localPosition.x == 568f)
		{
			//退出选择桌子界面
			Page_FishGameRoom.transform.FindChild(HallTransfer.Instance.uiConfig.curGameType.ToString()).GetComponent<TweenAlpha>().PlayForward();//.gameObject.SetActive(true);
			Page_Desks.GetComponent<TweenPosition>().PlayReverse();//.SetActive(false);
			foreach(var i in HallTransfer.Instance.uiConfig.gambleRoomDesk) i.GetComponent<TweenPosition>().PlayReverse();//.gameObject.SetActive(false);
			HallTransfer.Instance.uiConfig.page_gameRoom.transform.FindChild("UserMoney").GetComponent<TweenPosition>().PlayReverse();
		}else{
			//退出选择房间界面
			//房间隐藏
			Page_FishGameRoom.GetComponent<TweenAlpha>().PlayReverse();
			//游戏显示
			Page_Games.GetComponent<TweenAlpha>().ResetToBeginning();
			Page_Games.GetComponent<TweenAlpha>().PlayForward();
			BottomBar.GetComponent<TweenPosition>().ResetToBeginning();
			BottomBar.GetComponent<TweenPosition>().PlayForward();
			FirstGirl.GetComponent<TweenPosition>().PlayForward();
			//退出按钮显示
			SwitchBtn.SetActive(true);
			RoomReturnBtn.SetActive(false);
			//游戏LOGO显示
			GameLogo.GetComponent<TweenRotation>().PlayReverse();
			RoomLogo.GetComponent<TweenRotation>().PlayReverse();
		}
	}
}
