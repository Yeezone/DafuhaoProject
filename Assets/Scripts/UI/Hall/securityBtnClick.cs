using UnityEngine;
using System;

public class securityBtnClick : MonoBehaviour {
	// 等待界面
	public GameObject waiting;
	// 日期下拉菜单
	public UIPopupList m_gDateList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	DateTime dt;


	void OnClick () 
	{
//		if (waiting != null) {
//			waiting.SetActive(true);
//		}
		//清空登录/游戏信息List
		HallTransfer.Instance.logonRecordList.Clear();
		HallTransfer.Instance.gameRecordList.Clear();
		//发送消息
		if(HallTransfer.Instance.uiConfig.page_gameRecord_mask != null)
		{
			HallTransfer.Instance.uiConfig.page_gameRecord_mask.SetActive(true);
		}

		if (HallTransfer.Instance.ncSafetyCenter != null) {
			HallTransfer.Instance.ncSafetyCenter();
		}

		//初始化日期下拉菜单
		//GameObject tempDateList =HallTransfer.Instance.uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("datePopup_list").gameObject;
		if(m_gDateList!=null)
		{
			m_gDateList.Clear();
			m_gDateList.value = DateTime.Now.ToString("yyyy-MM-dd");
			
			for(int i = 4; i >= 0; i--)
			{
				dt = DateTime.Now.AddDays(-i);
				m_gDateList.AddItem( dt.ToString("yyyy-MM-dd"));
			}
		}


		//初始化游戏类型下拉菜单
		if(!HallTransfer.Instance.uiConfig.MobileEdition)
		{
			GameObject tempGameList = HallTransfer.Instance.uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("gamePopup_list").gameObject;
			//		tempGameList.GetComponent<UIPopupList>().Clear();
			//		tempGameList.GetComponent<UIPopupList>().value = "ALL";
			//		tempGameList.GetComponent<UIPopupList>().AddItem( "ALL" );
			//		for(int i = 0; i <HallTransfer.Instance.uiConfig.hallGameIds.Length; i++)
			//		{
			//			tempGameList.GetComponent<UIPopupList>().AddItem(HallTransfer.Instance.uiConfig.hallGameIds[i].ToString() );
			//		}
			tempGameList.SetActive(false);
		}

//		this.gameObject.GetComponent<UIButton> ().isEnabled = false;
//		Invoke("onResumeBtn",3.0f);

	}
		
	void onResumeBtn()
	{
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;
		if (waiting != null) {
			waiting.SetActive(false);
		}

		if(HallTransfer.Instance.uiConfig.page_gameRecord_mask != null)
		{
			HallTransfer.Instance.uiConfig.page_gameRecord_mask.SetActive(false);
		}

//		if (HallTransfer.Instance.uiConfig.msgTooLate) 
//		{
//			HallTransfer.Instance.canExecuteUIF = false;
//			HallTransfer.Instance.uiConfig.msgTooLate = false;
//			HallTransfer.Instance.cnMsgBox ("请求超时,请稍候再试");		
//		}
		
	}
}
