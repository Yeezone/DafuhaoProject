using UnityEngine;
using System.Collections;

public class SafeBoxCtrl : MonoBehaviour {

	public	UILabel			SaveMoneyLable;
	public	UILabel			SaveSafeMoneyLable;
	public	UIInput			SaveMoneyInput;
	public	UIInput			SaveSafeMoneyInput;


	public	void	SetMoneyInput()
	{
		SaveMoneyInput.value = SaveMoneyLable.text;
	}
	
	public	void	SetSafeMoneyInput()
	{
		SaveMoneyInput.value = SaveSafeMoneyLable.text;
	}
}
