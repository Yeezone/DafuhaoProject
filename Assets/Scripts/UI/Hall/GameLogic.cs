using UnityEngine;
using System;
using System.Collections;

public class GameLogic222 : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		UiManager.Instance.ncGameBtnClick += ncGameBtnClick;
//		UiManager.Instance.ncGameRoomClick += ncGameRoomClick;
//		UiManager.Instance.ncGameQuickEnterClick += ncGameQuickEnterClick;

	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void ncGameBtnClick( UInt32 a )
	{
		Debug.Log( a );
	}
	void ncGameRoomClick( UInt32 a )
	{
		Debug.Log( a );
	}
	void ncGameQuickEnterClick( UInt32 a )
	{
		Debug.Log( a );
	}
}
