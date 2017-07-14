using UnityEngine;

public class moblieRoomDeskBtnClick : MonoBehaviour {

	public	uint		deskId;

	public	UISlider	mobileManSlider;
	public	Transform	mobileManCountLabel;

	private	uint		manCount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	public void OnClick() {
		//移动端设置按钮Toggle状态
		for(int i = 0; i < HallTransfer.Instance.roomDeskList.Count; i++)
		{
			HallTransfer.Instance.roomDeskList[i].transform.FindChild("toggle_img").gameObject.SetActive(false);
//			HallTransfer.Instance.roomDeskList[i].transform.FindChild("wait").gameObject.SetActive(true);
		}

		this.transform.FindChild("toggle_img").gameObject.SetActive(true);
//		HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("desk_bg0_img").FindChild("deskIco_img").gameObject.SetActive(false);
		HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("gaming_img").gameObject.SetActive(false);



		//设置桌号标签
		HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("desk_label").GetComponent<UILabel>().text = (deskId+1) + "号桌";
		for(int i = 0; i < HallTransfer.Instance.uiConfig.chairCount; i++)
		{
			Transform tempChair = HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair"+i).FindChild("chair_btn");
			tempChair.parent.FindChild("face").gameObject.SetActive(false);	//隐藏头像
			tempChair.GetComponent<CGameChairItem>().deskid = deskId;		//设置桌子ID
			tempChair.GetComponent<CGameChairItem>().empty = true;			//设置椅子为空
		}
		for(int i = 0; i < HallTransfer.Instance.roomUserList.Count; i++)
		{
			if(HallTransfer.Instance.roomUserList[i].dwDesk == deskId)
			{
				Transform tempChair = HallTransfer.Instance.uiConfig.mobileFishDesk.transform.FindChild("chair" + HallTransfer.Instance.roomUserList[i].dwChair);
				//设置头像
				tempChair.FindChild("face").FindChild("face_img").GetComponent<UISprite>().spriteName = "face_" + HallTransfer.Instance.roomUserList[i].dwLogoID;
				//设置昵称
				tempChair.FindChild("face").FindChild("name_label").GetComponent<UILabel>().text = HallTransfer.Instance.roomUserList[i].dwNickName;
				//设置金钱
				tempChair.FindChild("face").FindChild("money_label").GetComponent<UILabel>().text = HallTransfer.Instance.roomUserList[i].dwMoney.ToString();
				//显示头像
				tempChair.FindChild("face").gameObject.SetActive(true);
				//设置椅子不为空
				tempChair.FindChild("chair_btn").GetComponent<CGameChairItem>().empty = false;
				//隐藏桌子游戏图片
//				tempChair.parent.FindChild("desk_bg0_img").FindChild("deskIco_img").gameObject.SetActive(true);
				tempChair.parent.FindChild("gaming_img").gameObject.SetActive(HallTransfer.Instance.DeskState[deskId]);
			}
		}
//
		if(HallTransfer.Instance.uiConfig.quickEnterDesk)
		{
//			uiConfig.quickEnterDesk = false;
//			uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("quickEnter_btn").GetComponent<quickEnterBtnClick>().OnClick();
		}
	}

	public void RefreshManCount()
	{
		manCount = 0;
		uint maxManCount = HallTransfer.Instance.uiConfig.chairCount;
		for(int i = 0; i < HallTransfer.Instance.roomUserList.Count; i++)
		{
			if(HallTransfer.Instance.roomUserList[i].dwDesk == deskId)
			{
				manCount++;//人数++
			}
		}

       UILabel uiobj = this.transform.FindChild("ruchang_label").GetComponent<UILabel>();

		if (manCount == maxManCount) {
            uiobj.text = "满员";
            uiobj.color = Color.red;
		}
//		else {
//            uiobj.text = "可进入";
//            uiobj.color = new Color(22 / 255.0f, 193 / 255.0f, 236 / 255.0f);
//		}

		mobileManCountLabel.GetComponent<UILabel>().text = manCount.ToString() + "/" + maxManCount.ToString();//设置桌子按钮上显示的人数
		mobileManSlider.value = (float)manCount / (float)maxManCount;
//		Transform tempManSlider = this.transform.FindChild("manSlider").FindChild("sliderValue_img");
//		if(manCount == 0)
//		{
//			tempManSlider.gameObject.SetActive(false);
//		}else{
//			tempManSlider.gameObject.SetActive(true);
//			int maxValue = tempManSlider.parent.GetComponent<UISprite>().width;
//			int miniValue = 32;
//			int curValue = ( miniValue + (maxValue - miniValue)/((int)maxManCount - 1)*((int)manCount - 1)  );
//			Debug.LogWarning("curValue:"+curValue);
//			tempManSlider.GetComponent<UISprite>().rightAnchor.absolute = curValue;
//		}
	}

}
