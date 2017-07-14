using UnityEngine;
using System;
using System.Collections;

public class copyUserInfoBtnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		//发送复制消息
		HallTransfer.Instance.ncCopyUserIDAndName();
		
	}
}
