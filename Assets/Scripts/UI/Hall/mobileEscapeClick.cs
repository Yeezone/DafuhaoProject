using UnityEngine;

public class mobileEscapeClick : MonoBehaviour
{

	public bool islogin;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (islogin) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Debug.LogWarning ("退出游戏");
				//显示退出游戏界面
				if (LoginTransfer.Instance.msgBox.activeSelf) {
					LoginTransfer.Instance.isQuitGame = false;
					LoginTransfer.Instance.msgBox.SetActive (false);
				} else {
					LoginTransfer.Instance.isQuitGame = true;
					LoginTransfer.Instance.msgBox.SetActive (true);
					LoginTransfer.Instance.log_msg.GetComponent<UILabel> ().text = "确定退出游戏?";
				}
			}
		} else {
			if (Input.GetKey (KeyCode.AltGr)) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					//最大化
					HallTransfer.Instance.ncMaxWindow ();
				}
			}
			
			if (Input.GetKeyDown (KeyCode.Escape)) {

				if (HallTransfer.Instance.uiConfig.msgBox.activeSelf) {
					//关闭弹出消息窗口
					HallTransfer.Instance.uiConfig.msgBox.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_CancelOrderBox.activeSelf) {
					//关闭取消订单窗口
					HallTransfer.Instance.uiConfig.window_CancelOrderBox.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.page_roomDesk.activeSelf) {
					if (HallTransfer.Instance.uiConfig.MobileEdition) {
						//手机端:关闭房间座位界面
						HallTransfer.Instance.uiConfig.curDeskNo = 0;
						HallTransfer.Instance.uiConfig.page_roomDesk.SetActive (false);
						HallTransfer.Instance.uiConfig.window_FirstPage.SetActive(false);
					} else {
						//PC端:显示退出游戏界面
						HallTransfer.Instance.uiConfig.window_Swith.SetActive (!HallTransfer.Instance.uiConfig.window_Swith.activeSelf);
					}
					if (HallTransfer.Instance.ncQuitRoomDesk != null) {
						HallTransfer.Instance.ncQuitRoomDesk ();
					}

					if (HallTransfer.Instance.uiConfig.window_MaskRoom != null) {
						HallTransfer.Instance.uiConfig.window_MaskRoom.SetActive (false);
					}
//					}
				} else if (HallTransfer.Instance.uiConfig.page_recharge.activeSelf) {
					//关闭充值界面
					HallTransfer.Instance.uiConfig.page_recharge.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_UserInfo.activeSelf) {
					GameObject tempPageFace = HallTransfer.Instance.uiConfig.window_UserInfo.transform.FindChild ("front_panel").FindChild ("pageFace").gameObject;
					if (tempPageFace.activeSelf) {
						//关闭玩家头像界面
						tempPageFace.SetActive (false);
						if(tempPageFace.transform.parent.FindChild("userInfo") != null) tempPageFace.transform.parent.FindChild("userInfo").gameObject.SetActive(true);
					} else {
						//关闭玩家资料界面
						HallTransfer.Instance.uiConfig.window_UserInfo.SetActive (false);
					}
				} else if (HallTransfer.Instance.uiConfig.window_SafeBox.activeSelf) {
					//关闭保险柜界面
					if(HallTransfer.Instance.uiConfig.window_SafeBox.GetComponent<TweenPosition>() != null)
					{
						HallTransfer.Instance.uiConfig.window_SafeBox.GetComponent<TweenPosition>().PlayReverse();
						SetActiveFalse(HallTransfer.Instance.uiConfig.window_SafeBox);
					}else{
						HallTransfer.Instance.uiConfig.window_SafeBox.SetActive (false);
					}
				} else if (HallTransfer.Instance.uiConfig.window_SafeBoxEntry.activeSelf) {
					//关闭保险柜入口界面
					HallTransfer.Instance.uiConfig.window_SafeBoxEntry.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_Security.activeSelf) {
					//关闭安全中心界面
					HallTransfer.Instance.uiConfig.window_Security.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_Feedback.activeSelf) {
					//关闭用户反馈界面
					HallTransfer.Instance.uiConfig.window_Feedback.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_LockOrUnLock != null && HallTransfer.Instance.uiConfig.window_LockOrUnLock.activeSelf) {
					//关闭 绑定 界面
					HallTransfer.Instance.uiConfig.window_LockOrUnLock.SetActive (false);
				} else if (HallTransfer.Instance.uiConfig.window_ReloginMsgBox.activeSelf) {
					//关闭 重试 返回游戏 界面
					HallTransfer.Instance.uiConfig.window_ReloginMsgBox.SetActive (false);
				} else {
					//显示退出游戏界面
					Debug.LogWarning ("KeyCode.Toggle!!!!!!!!!!!!!");
					if(HallTransfer.Instance.uiConfig.window_Swith.GetComponentInChildren<TweenScale>() != null) HallTransfer.Instance.uiConfig.window_Swith.GetComponentInChildren<TweenScale>().Toggle();
					if(HallTransfer.Instance.uiConfig.window_Swith.GetComponentInChildren<TweenAlpha>() != null) HallTransfer.Instance.uiConfig.window_Swith.GetComponentInChildren<TweenAlpha>().Toggle();

					if(!HallTransfer.Instance.uiConfig.window_Swith.activeSelf) HallTransfer.Instance.uiConfig.window_Swith.SetActive(true);
				}
			}
		}
	}
	void SetActiveFalse(GameObject tempObj)
	{		
		System.Threading.Thread.Sleep(500);
		tempObj.SetActive(false);
	}
}
