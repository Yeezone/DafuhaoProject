using UnityEngine;
using System;
using System.Collections;

public class award_btnClick : MonoBehaviour {

	public  GameObject 		moneyInput;
	public  GameObject 		password;
	public  GameObject 		remark;
	public  GameObject 		submit;
	public	GameObject		money_label;
	public	GameObject		safebox_label;

	public  bool    isAllMoney;    
	HallTransfer.ExchangeRequest   awardMsg;
//	HallTransfer.RechargeRequest   awardMsg; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		if (isAllMoney) 
		{
			if(moneyInput.GetComponent<UIInput>().value != safebox_label.GetComponent<UILabel>().text)
			{
				moneyInput.GetComponent<UIInput>().value = safebox_label.GetComponent<UILabel>().text;
			}

		} 
		else 
		{	
			if (HallTransfer.Instance.ncAwardBtnClick != null)
			{
				Int64 tempMoney;
				string money = money_label.GetComponent<UILabel>().text;

				if(moneyInput.GetComponent<UIInput> ().value=="")
				{
					tempMoney = 0;
				}else
				{
					tempMoney = Int64.Parse(moneyInput.GetComponent<UIInput>().value);
				}


				if( money!="" && tempMoney<Int64.Parse(money) )
				{
					string templog = "最少充值为"+UInt64.Parse(money)+"元";
					HallTransfer.Instance.cnMsgBox(templog );
					
				}else
				{
					string tempRemark = remark.GetComponent<UIInput> ().value;
					string tempPass = password.GetComponent<UIInput> ().value;
					
					awardMsg = new HallTransfer.ExchangeRequest ();
					//			awardMsg = new HallTransfer.RechargeRequest ();
					awardMsg.dwMoney = tempMoney;
					awardMsg.dwRemark = tempRemark;
					awardMsg.dwPassword = tempPass;

					if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) {
						HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(true);
					}

					HallTransfer.Instance.ncAwardBtnClick (awardMsg);
					submit.GetComponent<UIButton>().isEnabled = false;
					Invoke("resumeButton",5.0f);		
				}

			} 
		}
	}

	public void resumeButton()
	{
		if (HallTransfer.Instance.uiConfig.page_recharge_mask != null
		    && HallTransfer.Instance.uiConfig.page_recharge_mask.activeSelf) 
		{
			HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(false);
			HallTransfer.Instance.cnMsgBox("请求超时");
		}
		submit.GetComponent<UIButton>().isEnabled = true;
	}

}
