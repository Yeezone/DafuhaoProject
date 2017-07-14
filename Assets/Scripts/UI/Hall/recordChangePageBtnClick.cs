using UnityEngine;
using System.Collections;

public class recordChangePageBtnClick : MonoBehaviour {

	public	GameObject		logonLogs;
	public	bool			prePage;
	public	bool			nextPage;
	public	GameObject		pageInput;

	public	GameObject		refreshBtn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

	public void OnClick ()
	{
		if(logonLogs.activeSelf)
		{
			//登录记录
			if(prePage)
			{
				//上一页
                int firstIndex = 0;
                int.TryParse(logonLogs.transform.FindChild("log0").FindChild("index_label").GetComponent<UILabel>().text, out firstIndex);
				if( firstIndex > 8 )
				{
					firstIndex -= 9;
					pageInput.GetComponent<UIInput>().value = (firstIndex / 8 + 1).ToString();
					HallTransfer.Instance.resetLogonRecord(firstIndex);//跳
				}
			}else if(nextPage)
			{
				//下一页
				int lastIndex = 0;
                int.TryParse(logonLogs.transform.FindChild("log7").FindChild("index_label").GetComponent<UILabel>().text, out lastIndex);
				if( lastIndex < HallTransfer.Instance.logonRecordList.Count )
				{
					pageInput.GetComponent<UIInput>().value = (lastIndex / 8 + 1).ToString();
					HallTransfer.Instance.resetLogonRecord(lastIndex);//跳
				}
			}else if(pageInput.GetComponent<UIInput>().value != "")
			{
				//跳转到N页
				int pageIndex = 0;
                int.TryParse(pageInput.GetComponent<UIInput>().value, out pageIndex);
				int pageCount = HallTransfer.Instance.logonRecordList.Count / 8;
				if( (HallTransfer.Instance.logonRecordList.Count % 8) != 0 ) pageCount += 1;
				if(pageIndex != 0 && pageIndex <= pageCount)
				{
					HallTransfer.Instance.resetLogonRecord((pageIndex - 1) * 8);
				}
			}
		}else{
			//游戏记录
			//uint curPage = refreshBtn.GetComponent<securityCenter_refresh_btnClick>().gameRecord.dwPage;
			uint curPage = HallTransfer.Instance.uiConfig.curRecordPageCount;
			if(prePage)
			{
				//上一页
				if(curPage > 1)
				{
					curPage -= 1;
					HallTransfer.Instance.uiConfig.curRecordPageCount -= 1;
					this.transform.parent.FindChild("page_input").GetComponent<UIInput>().value = curPage.ToString();
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().gameRecord.dwPage = curPage;
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().OnClick();
				}
			}else if(nextPage)
			{
				//下一页
				Debug.LogWarning("总页:" + HallTransfer.Instance.uiConfig.gameRecordPageCount);
				Debug.LogWarning("当前:" + curPage);
				if(curPage+1 <= HallTransfer.Instance.uiConfig.gameRecordPageCount)
				{
					curPage += 1;
					HallTransfer.Instance.uiConfig.curRecordPageCount += 1;
					this.transform.parent.FindChild("page_input").GetComponent<UIInput>().value = curPage.ToString();
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().gameRecord.dwPage = curPage;
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().OnClick();
				}
			}else if(pageInput.GetComponent<UIInput>().value != "")
			{
				//跳转到N页
				uint tempPage;
				int pageIndex = int.Parse(pageInput.GetComponent<UIInput>().value);
				if(pageIndex < 1)
					pageIndex = 1;
				if(curPage > 0)
				{
					tempPage = curPage;
					curPage = (uint)pageIndex;
					HallTransfer.Instance.uiConfig.curRecordPageCount = curPage;
					this.transform.parent.FindChild("page_input").GetComponent<UIInput>().value = curPage.ToString();
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().gameRecord.dwPage = curPage;
					refreshBtn.GetComponent<securityCenter_refresh_btnClick>().OnClick();
					curPage = tempPage;
				}
			}


		}

	}

}
