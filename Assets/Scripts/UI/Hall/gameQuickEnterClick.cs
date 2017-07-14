using UnityEngine;
using System;
using System.Collections;


public class gameQuickEnterClick : MonoBehaviour {

	public	UInt32		roomid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick() {

		if( true )
		{
			HallTransfer.Instance.uiConfig.quickEnterDesk = true;
			HallTransfer.Instance.ncGameRoomClick(roomid);
		}
	}
}
