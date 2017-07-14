using UnityEngine;
using System;

public class rechargeRecord_btnClick : MonoBehaviour {

	HallTransfer.RecordRequest record;
	DateTime dt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		//Debug.LogWarning("rechargeRecord_btnClick~~~~~~");

		HallTransfer.Instance.uiConfig.curRechargeRecordPage = 1;
		HallTransfer.Instance.uiConfig.curRechargeRPageCount = 1;
		HallTransfer.Instance.msgTooLate_RcLogs = true;
		HallTransfer.Instance.canReceiveRecord_Rc = true;

		//初始化日期下拉菜单
		GameObject tempDateList =HallTransfer.Instance.uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild("rechargeRecords").FindChild("datePopup_list").gameObject;
		tempDateList.GetComponent<UIPopupList>().Clear();
		tempDateList.GetComponent<UIPopupList>().value = DateTime.Now.ToString("yyyy-MM-dd");
		
		for(int i = 4; i >= 0; i--)
		{
			dt = DateTime.Now.AddDays(-i);
			tempDateList.GetComponent<UIPopupList>().AddItem( dt.ToString("yyyy-MM-dd"));
		}

		if (HallTransfer.Instance.ncRechargeRecord != null) 
		{
			record = new HallTransfer.RecordRequest();
			record.dwPage = 1;
			record.dwPageSize = 10;
			record.dwTime = DateTime.Now;
			//record.dwTime = UInt64.Parse(DateTime.Now.ToFileTime().ToString());

			if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) {
				HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(true);
			}
			HallTransfer.Instance.ncRechargeRecord(record);

			Invoke("onResumeBtn",5.0f);
		}

	}

	void onResumeBtn()
	{
		if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) 
		{
			HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(false);
		}
		if(HallTransfer.Instance.msgTooLate_RcLogs) {	
			HallTransfer.Instance.canReceiveRecord_Rc = false;
			HallTransfer.Instance.cnTipsBox ("请求超时,稍候再试!");
		}
	}


}
