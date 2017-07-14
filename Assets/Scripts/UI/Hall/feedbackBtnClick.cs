using UnityEngine;
using System;
using System.Collections;

public class feedbackBtnClick : MonoBehaviour {

	public	GameObject				typeList;
	public	GameObject				phoneInput;
	public	GameObject				contentInput;

	private	HallTransfer.UserSuggestion	suggestion = new HallTransfer.UserSuggestion();

	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void Submit () 
	{
		suggestion.dwType = typeList.GetComponent<UIPopupList>().value;
		suggestion.dwCellPhone = phoneInput.GetComponent<UIInput>().value;
		suggestion.dwUserSuggestion = contentInput.GetComponent<UIInput>().value;

		if (suggestion.dwCellPhone != "" && suggestion.dwUserSuggestion != "" ) 
		{
			if(suggestion.dwCellPhone.Length < 11)
			{
				HallTransfer.Instance.cnTipsBox("请输入正确的手机号码！");
			}
			else
			{
				if (HallTransfer.Instance.uiConfig.window_Feedback_mask != null) {
					HallTransfer.Instance.uiConfig.window_Feedback_mask.SetActive (true);
				}
				
				if (HallTransfer.Instance.ncUserSuggestion != null) {
					HallTransfer.Instance.ncUserSuggestion (suggestion);
				}
			}

			Invoke ("OnResumeBtn", 5.0f);
		} 
		else {
			return;
		}
	}

	public void CancelFeedbackInvoke()
	{
		CancelInvoke("OnResumeBtn");
		HallTransfer.Instance.uiConfig.window_Feedback_mask.SetActive (false);
	}

	void OnResumeBtn()
	{
		if(HallTransfer.Instance.uiConfig.window_Feedback_mask != null
		   && HallTransfer.Instance.uiConfig.window_Feedback_mask.activeSelf)
		{
			HallTransfer.Instance.uiConfig.window_Feedback_mask.SetActive(false);
			HallTransfer.Instance.cnTipsBox ("请求超时,稍候再试!");
		}
	}

}
