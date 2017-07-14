using System;
using UnityEngine;

public class safeBox_money_btnClick : MonoBehaviour {

	public	bool		allMoney = false;
    public Int64 moneyCnt;
	public	GameObject	moneyInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick() {
		if( moneyInput != null )
		{
			if(moneyCnt > HallTransfer.Instance.uiConfig.bankMoney || allMoney)
			{
				moneyInput.GetComponent<UIInput>().value = HallTransfer.Instance.uiConfig.bankMoney.ToString();
			}else{
				moneyInput.GetComponent<UIInput>().value = moneyCnt.ToString();
			}
		}
	}
}
