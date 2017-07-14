using System;
using UnityEngine;


public class safeBox_saveTake_btnClick : MonoBehaviour {

	public	bool		saved;

	public	GameObject	money_label;
	public	GameObject	safeboxMoney_label;

	public	GameObject	money_input;
	public	GameObject	pass_input;

	public	GameObject	log0_label;
	public	GameObject	log1_label;
	public	GameObject	lable_log;

	public	GameObject	save_button;
	public	GameObject	take_button;
	public	GameObject	close_button;
	public	GameObject	toggleBtn;
	public	GameObject	toggleBtn1;
	public	GameObject	toggleBtn2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string formatMoney(string money)
	{
		if (money.Contains (",")) {
			string tempMoney = money.Replace(",","");
			return tempMoney;
		}
		return money;
	}

	void OnClick() 
	{
		log0_label.GetComponent<UILabel>().text = "";
		log1_label.GetComponent<UILabel>().text = "";

		string userMoney = money_label.GetComponent<UILabel> ().text;
		string SafeMoney = safeboxMoney_label.GetComponent<UILabel> ().text;

		userMoney = formatMoney(userMoney);
		SafeMoney = formatMoney(SafeMoney);

		Int64 maxMoney, maxSafeboxMoney;

//		int maxMoney = 0;
//		int maxSafeboxMoney=0;
//
//		try
//		{
//		maxMoney = int.Parse(userMoney);
//		maxSafeboxMoney = int.Parse(SafeMoney);
//		}catch
//		{
//			maxMoney = maxSafeboxMoney = 0;
//		}

		if (!Int64.TryParse(userMoney, out maxMoney))
		{
			Debug.LogWarning("用户金额转换失败");
			lable_log.GetComponent<UILabel>().text = "金币操作出错";
			Invoke("cleanLogLabel",2.0f);
		}
		if (!Int64.TryParse(SafeMoney, out maxSafeboxMoney))
		{
			Debug.LogWarning("保险柜金额转换失败");
			lable_log.GetComponent<UILabel>().text = "金币操作出错";
			Invoke("cleanLogLabel",2.0f);
		}


		Int64 money = 0;
        if (!Int64.TryParse(money_input.GetComponent<UIInput>().value, out money))
        {
            Debug.LogWarning("保险柜 要存储的金额转换失败");
            lable_log.GetComponent<UILabel>().text = "金币只能为数字";
            Invoke("cleanLogLabel", 2.0f);
        }
		string pass = pass_input.GetComponent<UIInput>().value;

		if(saved)
		{
			//存钱
			if(money > maxMoney)
			{
//				log0_label.GetComponent<UILabel>().color = Color.red;
//				log0_label.GetComponent<UILabel>().text = "没有足够的钱存入";
				money_input.GetComponent<UIInput>().value = maxMoney.ToString();
				money = maxMoney;
				if(money != 0){
//					save_button.GetComponent<UIButton>().isEnabled = false;
//					take_button.GetComponent<UIButton>().isEnabled = false;
//					close_button.GetComponent<UIButton>().isEnabled = false;
//					if(toggleBtn != null) toggleBtn.GetComponent<UIButton>().isEnabled = false;
//					if(toggleBtn != null) toggleBtn.GetComponent<showWindow>().enabled = false;
//					if(toggleBtn1 != null) toggleBtn1.GetComponent<UIButton>().isEnabled = false;
//					//if(toggleBtn1 != null) toggleBtn1.GetComponent<showWindow>().enabled = false;
//					if(toggleBtn2 != null) toggleBtn2.GetComponent<UIButton>().isEnabled = false;
//					//if(toggleBtn2 != null) toggleBtn2.GetComponent<showWindow>().enabled = false;
					if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
					{
						HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(true);
					}
					if(HallTransfer.Instance.ncCheckInMoney != null)
					{
						HallTransfer.Instance.ncCheckInMoney(money);
					}
				}
			}else if(money != 0)
			{
//				save_button.GetComponent<UIButton>().isEnabled = false;
//				take_button.GetComponent<UIButton>().isEnabled = false;
//				close_button.GetComponent<UIButton>().isEnabled = false;
//				if(toggleBtn != null) toggleBtn.GetComponent<UIButton>().isEnabled = false;
//				if(toggleBtn != null) toggleBtn.GetComponent<showWindow>().enabled = false;
//				if(toggleBtn1 != null) toggleBtn1.GetComponent<UIButton>().isEnabled = false;
//				//if(toggleBtn1 != null) toggleBtn1.GetComponent<showWindow>().enabled = false;
//				if(toggleBtn2 != null) toggleBtn2.GetComponent<UIButton>().isEnabled = false;
//				//if(toggleBtn2 != null) toggleBtn2.GetComponent<showWindow>().enabled = false;
				//发存钱消息
				if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
				{
					HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(true);
				}
				if(HallTransfer.Instance.ncCheckInMoney != null)
				{
					HallTransfer.Instance.ncCheckInMoney(money);
				}
			}
		}else{
			//取钱
			if(pass.Length < 6)
			{
				log1_label.GetComponent<UILabel>().color = Color.red;
				log1_label.GetComponent<UILabel>().text = "密码长度小于6位";
				Invoke("cleanLabel",2.0f);

			}else if(money > maxSafeboxMoney)
			{
				money_input.GetComponent<UIInput>().value = maxSafeboxMoney.ToString();
				money = maxSafeboxMoney;
				if(money != 0)
				{
//					save_button.GetComponent<UIButton>().isEnabled = false;
//					take_button.GetComponent<UIButton>().isEnabled = false;
//					close_button.GetComponent<UIButton>().isEnabled = false;

//					if(toggleBtn != null) toggleBtn.GetComponent<UIButton>().isEnabled = false;
//					if(toggleBtn != null) toggleBtn.GetComponent<showWindow>().enabled = false;
//					if(toggleBtn1 != null) toggleBtn1.GetComponent<UIButton>().isEnabled = false;
//					//if(toggleBtn1 != null) toggleBtn1.GetComponent<showWindow>().enabled = false;
//					if(toggleBtn2 != null) toggleBtn2.GetComponent<UIButton>().isEnabled = false;
//					//if(toggleBtn2 != null) toggleBtn2.GetComponent<showWindow>().enabled = false;

					if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
					{
						HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(true);
					}
					if(HallTransfer.Instance.ncCheckOutMoney != null)
					{
						HallTransfer.Instance.ncCheckOutMoney(money,pass);
					}
				}

			}else if(money != 0)
			{
//				save_button.GetComponent<UIButton>().isEnabled = false;
//				take_button.GetComponent<UIButton>().isEnabled = false;
//				close_button.GetComponent<UIButton>().isEnabled = false;

//				if(toggleBtn != null)  toggleBtn.GetComponent<UIButton>().isEnabled = false;
//				if(toggleBtn != null)  toggleBtn.GetComponent<showWindow>().enabled = false;
//				if(toggleBtn1 != null) toggleBtn1.GetComponent<UIButton>().isEnabled = false;
//				//if(toggleBtn1 != null) toggleBtn1.GetComponent<showWindow>().enabled = false;
//				if(toggleBtn2 != null) toggleBtn2.GetComponent<UIButton>().isEnabled = false;
//				//if(toggleBtn2 != null) toggleBtn2.GetComponent<showWindow>().enabled = false;


				if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
				{
					HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(true);
				}

				if(HallTransfer.Instance.ncCheckOutMoney != null)
				{
					HallTransfer.Instance.ncCheckOutMoney(money,pass);
				}
			}


		}

		this.gameObject.GetComponent<UIButton> ().isEnabled = false;
		Invoke("onResumeBtn",3.0f);
	}

	void cleanLabel()
	{
		log1_label.GetComponent<UILabel>().text = "";
	}

	void cleanLogLabel()
	{
		lable_log.GetComponent<UILabel>().text = "";
	}

	void onResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
		{
			HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(false);
		}
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;		
	}
}
