using UnityEngine;
using System;


public class securityCenter_refresh_btnClick : MonoBehaviour {


	public	GameObject					logonRecord;
	public	GameObject					recordDateTime;
	public	GameObject					recordGameType;
	public	GameObject					pageInput;

	public	HallTransfer.GameRecordRequest	gameRecord = new HallTransfer.GameRecordRequest();

	private	uint						dateTime;
	private	uint						gameKind = 0;
	private uint						page;

	// Use this for initialization
	void Start () {
		gameRecord.dwPage = 1;
		gameRecord.dwPageSize = HallTransfer.Instance.uiConfig.gameRecordPageNum;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		Debug.LogWarning("securityCenter_refresh_btnClick");
		dateTime = uint.Parse( DateTime.Parse(recordDateTime.GetComponent<UIPopupList>().value).ToString("yyyyMMdd"));
		
		if (logonRecord.activeSelf) {
			//登录记录
			Debug.LogWarning ("dateTime:" + dateTime);
			if (!HallTransfer.Instance.uiConfig.MobileEdition) {
				pageInput.GetComponent<UIInput> ().value = "1";
			}
			for (int i = 0; i <HallTransfer.Instance.logonRecordList.Count; i++) {
				DateTime dt = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds (HallTransfer.Instance.logonRecordList [i].dwTmlogonTime + 28800);
				string stdt = dt.ToString ("yyyy-MM-dd");
				if (recordDateTime.GetComponent<UIPopupList> ().value == stdt) {
					int pageCount = i / 8;
					if ((i % 8) != 0 || i == 0)
						pageCount += 1;
					HallTransfer.Instance.resetLogonRecord ((pageCount - 1) * 8);
					break;
				}
			}
		} else {
			//游戏记录
			uint tempGameType = 0;

			if (!HallTransfer.Instance.uiConfig.MobileEdition) 
			{
				if (recordGameType.GetComponent<UIPopupList> ().value == "全部") {
					gameRecord.dwGameKind = 0;
				} else {
					for (int i = 0; i < HallTransfer.Instance.gameIdsRecordList.Count; i++) {
						if (HallTransfer.Instance.uiConfig.hallGameNames [i] == recordGameType.GetComponent<UIPopupList> ().value) {
							tempGameType = HallTransfer.Instance.uiConfig.hallGameIds [i];
							break;
						}
					}
					gameRecord.dwGameKind = tempGameType;						
				}

				if (recordGameType.GetComponent<UIPopupList> ().value != HallTransfer.Instance.uiConfig.curRecordGameType) {
					uint pageIndex = 1;
					HallTransfer.Instance.uiConfig.curRecordGameType = recordGameType.GetComponent<UIPopupList> ().value;
					HallTransfer.Instance.uiConfig.curRecordPageCount = pageIndex;
					this.transform.parent.FindChild ("page_input").GetComponent<UIInput> ().value = pageIndex.ToString ();
					gameRecord.dwPage = pageIndex;
				}
				
			}else{
				gameRecord.dwGameKind = 0;
				gameRecord.dwPage = HallTransfer.Instance.uiConfig.curRecordPageCount;
			}
			gameRecord.dwTime = dateTime;
			Debug.LogWarning ("dwGameKind:" + gameRecord.dwGameKind);
			Debug.LogWarning ("dwPage:" + gameRecord.dwPage);
			Debug.LogWarning ("dwTime:" + gameRecord.dwTime);
			HallTransfer.Instance.ncGameRecord (gameRecord);

		}
	}

}

