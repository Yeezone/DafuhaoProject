using UnityEngine;
using System.Collections;

public class CancelOrderBtnClick : MonoBehaviour {

	public  bool  isRecharge;
	public  bool  isAward;

	public  string  recordIndex;  
	public  GameObject RechargeRecord;
	public  GameObject AwardRecord;
	public  GameObject CancelOrder;

	HallTransfer.OrderMsg orderMsg;

	public string[] result;
	// Use this for initialization
	void Start () {
		result = new string[5]; 
		result [0] = "充值金额输入错误";
		result [1] = "不想充值了";
		result [2] = "客服提供的转帐方式我没有";
		result [3] = "不想提现了";
		result [4] = "提现数量输入错误";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		GameObject tempList = HallTransfer.Instance.uiConfig.window_CancelOrderBox.transform.FindChild ("front_panel").FindChild ("CancelPopup_list").gameObject;

		if (isRecharge || isAward) {
			HallTransfer.Instance.uiConfig.window_CancelOrderBox.SetActive (true);
			recordIndex = this.gameObject.GetComponent<cancelBtnTag>().tags;
			HallTransfer.Instance.uiConfig.window_CancelOrderBox.gameObject.GetComponent<cancelBtnTag>().tags = recordIndex;
			HallTransfer.Instance.uiConfig.OrderId = recordIndex;

			//初始化下拉菜单
			tempList.GetComponent<UIPopupList> ().Clear ();
			tempList.GetComponent<UIPopupList> ().value = result [2];
			
			for (int i = 0; i < 5; i++) {
				//				dt = DateTime.Now.AddDays(-i);
				tempList.GetComponent<UIPopupList> ().AddItem (result [i]);
			}
			
			if (isRecharge) {
				HallTransfer.Instance.isRecharge = true;
				
			} else if (isAward) {
				HallTransfer.Instance.isRecharge = false;
			}


		} else {		

			recordIndex = HallTransfer.Instance.uiConfig.OrderId;

			orderMsg = new HallTransfer.OrderMsg();
			orderMsg.dwCancelReason = tempList.GetComponent<UIPopupList> ().value;
			orderMsg.dwApplyNumber = recordIndex;


			if(HallTransfer.Instance.isRecharge )
			{	
				Debug.LogWarning ("确认取消充值订单");
				if(HallTransfer.Instance.ncCancelRechargeClick != null)
				{

					HallTransfer.Instance.ncCancelRechargeClick(orderMsg);
				}
			}else
			{
				Debug.LogWarning ("确认取消提现订单");
				if(HallTransfer.Instance.ncCancelAwardClick != null)
				{
					HallTransfer.Instance.ncCancelAwardClick(orderMsg);
				}
			}

			this.gameObject.GetComponent<UIButton>().isEnabled = false;
			Invoke("onResumeBtn", 3.0f);

		}

	}

	void onResumeBtn()
	{
		this.gameObject.GetComponent<UIButton>().isEnabled = true;
	}

}
