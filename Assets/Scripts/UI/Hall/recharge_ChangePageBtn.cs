using UnityEngine;
using System;

public class recharge_ChangePageBtn : MonoBehaviour {

	public	GameObject		recordLogs;
	public	GameObject		button;
	public	bool			prePage;
	public	bool			nextPage;

	public  bool   isRecharge;
	public  bool   isAward;
	HallTransfer.RecordRequest record;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	//void Update () {
	
	//}

	public void OnClick () {

		uint curPage = HallTransfer.Instance.uiConfig.curRechargeRecordPage;

		if (prePage) {
			//上一页
//			Debug.LogWarning ("prePage~");

			Int32 firstIndex = 0;

			try
			{
				firstIndex = Int32.Parse( recordLogs.transform.FindChild("log0").FindChild("index_label").GetComponent<UILabel>().text );
			}
			catch
			{
				firstIndex = 0;
			}
			
			if( firstIndex > 10 )
			{
				firstIndex -= 10;

				if(HallTransfer.Instance.uiConfig.curRechargeRecordPage > 0)
				{
					HallTransfer.Instance.uiConfig.curRechargeRecordPage -= 1;
				}else{
					HallTransfer.Instance.uiConfig.curRechargeRecordPage = 1;
				}


				record = new HallTransfer.RecordRequest();

				record.dwPage = HallTransfer.Instance.uiConfig.curRechargeRecordPage;
				record.dwPageSize = 10;
				record.dwTime = DateTime.Now;

				if(isRecharge)
				{
					HallTransfer.Instance.ncRechargeRecord(record);
				}
				else if(isAward)
				{
					HallTransfer.Instance.ncAwardRecord(record);
				}

			}
			this.gameObject.GetComponent<UIButton> ().isEnabled = false;
			if(HallTransfer.Instance.uiConfig.MobileEdition)
			{
				Invoke("OnResumeBtn", 0.5f);
			}else
			{
				Invoke("OnResumeBtn",0.2f);
			}

		}
		else if (nextPage) {
			//下一页
//			Debug.LogWarning ("nextPage~");

			if(curPage+1 <= HallTransfer.Instance.uiConfig.curRechargeRPageCount)
			{
				curPage += 1;
				HallTransfer.Instance.uiConfig.curRechargeRecordPage += 1;

				record = new HallTransfer.RecordRequest();
				
				record.dwPage = curPage;
				record.dwPageSize = 10;
				record.dwTime = DateTime.Now;
				
				if(isRecharge)
				{
					HallTransfer.Instance.ncRechargeRecord(record);
				}
				else if(isAward)
				{
					HallTransfer.Instance.ncAwardRecord(record);
				}
			}
			this.gameObject.GetComponent<UIButton> ().isEnabled = false;
			if(HallTransfer.Instance.uiConfig.MobileEdition)
			{
				Invoke("OnResumeBtn", 0.5f);
			}else
			{
				Invoke("OnResumeBtn",0.2f);
			}

		}
		else 
		{
			//HallTransfer.Instance.uiConfig.curRechargeRecordPage = 1;
			//HallTransfer.Instance.uiConfig.curRechargeRPageCount = 1;
		}

	}

	void OnResumeBtn()
	{
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;
	}
}
