using UnityEngine;

public class Login_Esc_Click : MonoBehaviour {
	
	public bool islogin;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (islogin) 
		{
			//登录界面======================================
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.LogWarning("退出游戏");
				//显示退出游戏界面
				if(LoginTransfer.Instance.msgBox.activeSelf)
				{
					LoginTransfer.Instance.isQuitGame =false;
					LoginTransfer.Instance.msgBox.SetActive(false);
				}
				else
				{
					LoginTransfer.Instance.isQuitGame = true;
					LoginTransfer.Instance.msgBox.SetActive(true);
					LoginTransfer.Instance.MsgBoxShow(2,"确认要退出游戏?");
				}
			}
		} 
		else 
		{
			//大厅界面========================================
			if (Input.GetKey(KeyCode.AltGr))
			{
				if(Input.GetKeyDown(KeyCode.Return))
				{
					//最大化
					HallTransfer.Instance.ncMaxWindow();
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.LogWarning("KeyCode.Escape");
				
				//if(HallTransfer.Instance.uiConfig.window_Swith.activeSelf)
				//{
					//关闭切换框
					//HallTransfer.Instance.uiConfig.window_Swith.SetActive(false);
				//}else 
				if(HallTransfer.Instance.uiConfig.msgBox.activeSelf)
				{
					//关闭弹出消息窗口
					HallTransfer.Instance.uiConfig.msgBox.SetActive(false);
				}else if(HallTransfer.Instance.uiConfig.window_CancelOrderBox.activeSelf)
				{
					//关闭取消订单窗口
					HallTransfer.Instance.uiConfig.window_CancelOrderBox.SetActive(false);
				}
				else if(HallTransfer.Instance.uiConfig.page_roomDesk.activeSelf)
				{
					if(HallTransfer.Instance.uiConfig.MobileEdition)
					{
						//关闭房间座位界面				
						HallTransfer.Instance.uiConfig.page_roomDesk.SetActive(false);
					}	
					if(HallTransfer.Instance.ncQuitRoomDesk != null)
					{
						HallTransfer.Instance.ncQuitRoomDesk();
					}
					
					if (HallTransfer.Instance.uiConfig.window_MaskRoom != null) {
						HallTransfer.Instance.uiConfig.window_MaskRoom.SetActive(false);
					}
					//					}
				}else if(HallTransfer.Instance.uiConfig.page_recharge.activeSelf)
				{
					//关闭充值界面
					HallTransfer.Instance.uiConfig.page_recharge.SetActive(false);
				}else if(HallTransfer.Instance.uiConfig.window_UserInfo.activeSelf)
				{
					GameObject gameobj = HallTransfer.Instance.uiConfig.window_UserInfo.transform.FindChild("front_panel").FindChild("pageFace").gameObject;
					if(gameobj.activeSelf)
					{
						//关闭玩家头像界面
						gameobj.SetActive(false);
					}else{
						//关闭玩家资料界面
						HallTransfer.Instance.uiConfig.window_UserInfo.SetActive(false);
					}
				}else if(HallTransfer.Instance.uiConfig.window_SafeBox.activeSelf)
				{
					//关闭保险柜界面
					HallTransfer.Instance.uiConfig.window_SafeBox.SetActive(false);
				}else if(HallTransfer.Instance.uiConfig.window_SafeBoxEntry.activeSelf)
				{
					//关闭保险柜入口界面
					HallTransfer.Instance.uiConfig.window_SafeBoxEntry.SetActive(false);
				}else if(HallTransfer.Instance.uiConfig.window_Security.activeSelf)
				{
					//关闭安全中心界面
					HallTransfer.Instance.uiConfig.window_Security.SetActive(false);
				}else if(HallTransfer.Instance.uiConfig.window_Feedback.activeSelf)
				{
					//关闭用户反馈界面
					HallTransfer.Instance.uiConfig.window_Feedback.SetActive(false);
				}else if (HallTransfer.Instance.uiConfig.window_LockOrUnLock != null && HallTransfer.Instance.uiConfig.window_LockOrUnLock.activeSelf)
				{
					//关闭 绑定 界面
					HallTransfer.Instance.uiConfig.window_LockOrUnLock.SetActive(false);
				}
				else if (HallTransfer.Instance.uiConfig.window_ReloginMsgBox.activeSelf)
				{
					//关闭 重试 返回游戏 界面
					HallTransfer.Instance.uiConfig.window_ReloginMsgBox.SetActive(false);
				}
				else{
					//显示退出游戏界面
					HallTransfer.Instance.uiConfig.window_Swith.SetActive(!HallTransfer.Instance.uiConfig.window_Swith.activeSelf);
				}
			}
			
		}
		
		
	}
}
