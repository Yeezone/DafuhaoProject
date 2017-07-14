using System;
using UnityEngine;

public class safeBox_give_btnClick : MonoBehaviour {

	public	GameObject		money_input;
	public	GameObject		receiver_input;
	public	Transform		money_log;
	public	Transform		receiver_log;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick() {
		money_log.GetComponent<UILabel>().text = "";
		receiver_log.GetComponent<UILabel>().text = "";

        Int64 money = Int64.Parse(money_input.GetComponent<UIInput>().value);
		string	receiver = receiver_input.GetComponent<UIInput>().value;

		if(money > HallTransfer.Instance.uiConfig.bankMoney || money <= 0)
		{
			money_log.GetComponent<UILabel>().color = Color.red;
			money_log.GetComponent<UILabel>().text = "金币不足!";
		}else if(receiver == "")
		{
			receiver_log.GetComponent<UILabel>().text = "接收者不能为空!";
		}

		Invoke("cleanLogs",3.0f);
	}
	void cleanLogs()
	{
		money_log.GetComponent<UILabel>().text = "";
		receiver_log.GetComponent<UILabel>().text = "";
	}
}
