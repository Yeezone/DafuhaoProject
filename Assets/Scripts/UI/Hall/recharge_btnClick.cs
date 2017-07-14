using UnityEngine;
using System;
using System.Collections;

public class recharge_btnClick : MonoBehaviour {
	
	public  GameObject 		moneyInput;
	public  GameObject 		remark;
	public  GameObject 		submit;
	public	GameObject		log_lable;
	public	GameObject		money_label;

	HallTransfer.RechargeRequest   rechargeMsg;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
	
	public void OnClick()
	{
		Debug.LogWarning ("recharg_btnClick~~~~~~");
		
		string capValue = moneyInput.GetComponent<UIInput>().value;
//		if (capValue.Length>1 && capValue.Substring (capValue.Length - 1, 1) != "0") 
//		{
//			moneyInput.GetComponent<UIInput>().value = capValue = capValue.Substring(0,capValue.Length - 1)+"0";
//		}

		if (HallTransfer.Instance.ncRechargeBtnClick != null) 
		{
			Int64  tempMoney;
			string money = money_label.GetComponent<UILabel>().text;
			if(moneyInput.GetComponent<UIInput> ().value=="")
			{
				tempMoney = 0;
			}
			else
			{
				tempMoney = Int64.Parse( moneyInput.GetComponent<UIInput> ().value );
				
				if( money!="" && tempMoney<Int64.Parse(money) )
				{
					string templog = "最少充值为"+Int64.Parse(money)+"元";
					HallTransfer.Instance.cnMsgBox(templog );
					
				}else
				{
					string  tempRemark = remark.GetComponent<UIInput>().value;
					
					rechargeMsg = new HallTransfer.RechargeRequest();
					rechargeMsg.dwMoney = tempMoney;
					rechargeMsg.dwRemark = tempRemark;

					if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) {
						HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(true);
					}
					HallTransfer.Instance.ncRechargeBtnClick(rechargeMsg);
					
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
	
	public void cleanLable()
	{
		log_lable.gameObject.GetComponent<UILabel>().text = "";
	}
	
	public void OnMoneyChange () 
	{
//		string capValue = moneyInput.GetComponent<UIInput>().value;
//		if(capValue.Length == 0) return;
//		
//		if(capValue.Substring(0,1) == "-" || capValue.Substring(0,1)=="0")
//		{
//			moneyInput.GetComponent<UIInput>().value = capValue = capValue.Substring(1);
//		}

		
	}
	
	public void OnSubmit() 
	{
//		string capValue = moneyInput.GetComponent<UIInput>().value;
//		if (capValue.Length>1 && capValue.Substring (capValue.Length - 1, 1) != "0") 
//		{
//			moneyInput.GetComponent<UIInput>().value = capValue = capValue.Substring(0,capValue.Length - 1)+"0";
//		}
		
	}
}
