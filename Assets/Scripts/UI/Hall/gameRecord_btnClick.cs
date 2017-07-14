using UnityEngine;
using System;
using System.Collections;

public class gameRecord_btnClick : MonoBehaviour {

	public	GameObject			logonLogs;
	public	GameObject			pageInput;
	public	GameObject			recordGameType;
	public 	UIPopupList 		detePopup_list;

	public	uint				gameId;
	public	uint				gamePage;
	public	uint				gameTime;

    // 开关_表示当前点击的是登录记录按钮还是游戏记录按钮
    public bool IsLoginRecord;

	// Use this for initialization
	void Start () {
		gamePage = 1;
		gameTime = uint.Parse( DateTime.Now.ToString("yyyyMMdd") );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick ()
	{
        if (!IsLoginRecord) 
		{
			HallTransfer.GameRecordRequest gameRecord = new HallTransfer.GameRecordRequest();
			gamePage = HallTransfer.Instance.uiConfig.curRecordPageCount = 1;
			HallTransfer.Instance.resetGameRecord(0);
			if(!HallTransfer.Instance.uiConfig.MobileEdition)
			{
				pageInput.GetComponent<UIInput> ().value = gamePage.ToString ();	
				detePopup_list.value = DateTime.Now.ToString ("yyyy-MM-dd");
				
				uint tempGameType = 0;
				if(recordGameType.GetComponent<UIPopupList>().value == "全部")
				{
					gameRecord.dwGameKind = 0;
				}else
				{
					for(int i = 0; i < HallTransfer.Instance.gameIdsRecordList.Count; i++)
					{
						if( HallTransfer.Instance.uiConfig.hallGameNames[i] == recordGameType.GetComponent<UIPopupList>().value )
						{
							tempGameType = HallTransfer.Instance.uiConfig.hallGameIds[i];
							break;
						}
					}
					gameRecord.dwGameKind = tempGameType;
				}
			}else
			{
				gameRecord.dwGameKind = gameId;

			}

			gameRecord.dwPage = gamePage;
			gameRecord.dwTime = gameTime;
			gameRecord.dwPageSize = HallTransfer.Instance.uiConfig.gameRecordPageNum;

//			Debug.LogWarning ("gameRecord.dwPage:" + gameRecord.dwPage);
//			Debug.LogWarning ("gameRecord.dwTime:" + gameRecord.dwTime);
			if(HallTransfer.Instance.uiConfig.page_gameRecord_mask != null)
			{
				HallTransfer.Instance.uiConfig.page_gameRecord_mask.SetActive(true);
			}

			if(HallTransfer.Instance.ncGameRecord != null)
			{
				HallTransfer.Instance.ncGameRecord (gameRecord);
				Invoke("OnResumePage",3.0f);
			}

		} 
		else if(IsLoginRecord)
		{
			gamePage = 1;
			if(!HallTransfer.Instance.uiConfig.MobileEdition)
			{
				pageInput.GetComponent<UIInput>().value = gamePage.ToString();
			}
			HallTransfer.Instance.resetLogonRecord(0);//跳
		}

//		this.gameObject.GetComponent<UIButton> ().isEnabled = false;
//		Invoke("resumeBtn",3.0f);
	}

	public	void OnResumePage()
	{
		if (HallTransfer.Instance.uiConfig.page_gameRecord_mask != null) {
			HallTransfer.Instance.uiConfig.page_gameRecord_mask.SetActive (false);
		}
	}
}
