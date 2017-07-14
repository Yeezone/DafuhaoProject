using UnityEngine;
using System;
using System.Collections;
using com.QH.QPGame.Lobby.Surfaces;

public class gameBtnClick : MonoBehaviour {

	public	UInt32		gameid;
	public	UInt32		gameKind;
	public	bool		installed;
	public	bool		needUpdate;
	public	GameObject  wating_animation;

	public GameObject	ObjUpdate;
	public GameObject	ObjUpdateSlider;
	public GameObject	ObjUpdateCancelBtn;

	private GameObject  tempGame;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	void OnClick() 
	{
		if(!HallTransfer.Instance.uiConfig.MobileEdition)
		{
			HallTransfer.Instance.uiConfig.page_roomDesk.SetActive(false);
			HallTransfer.Instance.uiConfig.page_gameRoom.SetActive(true);
		}
		HallTransfer.Instance.uiConfig.curGameId = gameid;
		HallTransfer.Instance.uiConfig.curGameKind = gameKind;

		if (HallTransfer.Instance.uiConfig.difen_lable != null) 
		{
			if (gameKind != 3) {
				HallTransfer.Instance.uiConfig.difen_lable.GetComponent<UILabel>().text = "底分";
			} 
			else {
				HallTransfer.Instance.uiConfig.difen_lable.GetComponent<UILabel>().text = "倍率";	
			}
		}
		
		if (HallTransfer.Instance.ncGameBtnClick != null) 
		{
			//Debug.LogWarning("gameid:"+gameid);
			//发送gameid
			HallTransfer.Instance.ncGameBtnClick(gameid);
			Invoke("onResumeBtn",5.0f);
		}
	
	}


	void onResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_MaskHall!= null
		   && HallTransfer.Instance.uiConfig.window_MaskHall.activeSelf
		   && HallTransfer.Instance.gameRoomList.Count == 0)
		{
			HallTransfer.Instance.uiConfig.window_MaskHall.SetActive(false);
			HallTransfer.Instance.cnTipsBox ("响应超时,稍候再试！");
			if( tempGame != null)
			{
				tempGame.SetActive(false);
			}
		}
		
	}

}
