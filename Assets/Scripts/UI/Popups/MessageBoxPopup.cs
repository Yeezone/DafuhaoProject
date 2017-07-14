

namespace com.QH.QPGame.Lobby.Surfaces
{
	using UnityEngine;
	using System.Collections;
	
	public class MessageBoxPopup : Surface
	{
		public MessageBoxCallback2 callback = null;
		
		public UILabel Title_lbl;
		public UILabel Text_lbl;
		public GameObject Close_btn;
		public GameObject Confirm_btn;
		public GameObject Ok_btn;
		public GameObject Cancel_btn;
		
		public Transform Front_panel;

		/// <summary>
		/// 回调类型
		/// </summary>
		public MessageBoxResult m_mResultStyle;

	    public bool IsShown()
	    {
            return gameObject.gameObject.activeSelf;
	    }

	    public void Show(string title, string message,ButtonStyle _ButtonStyle, MessageBoxCallback2 _callback = null, float _fWaitTime = 5.0f)
		{
			gameObject.SetActive(true);
			//设置UI
			Close_btn.SetActive(false);
			Ok_btn.SetActive(false);
			Confirm_btn.SetActive(false);
			Cancel_btn.SetActive(false);

			if ((_ButtonStyle & ButtonStyle.Confirm) != 0) Confirm_btn.SetActive(true);
			if ((_ButtonStyle & ButtonStyle.Yes) != 0) Ok_btn.SetActive(true);
			if ((_ButtonStyle & ButtonStyle.No) != 0) Cancel_btn.SetActive(true);
			if ((_ButtonStyle & ButtonStyle.OK) != 0) Ok_btn.SetActive(true);
			if ((_ButtonStyle & ButtonStyle.Cancel) != 0) Cancel_btn.SetActive(true);
			
			Title_lbl.text = title;
			Text_lbl.text = message;

			
			callback = _callback;

	        if (_fWaitTime > 0.0f)
	        {
                StartCoroutine(WaitTime(_fWaitTime));
	        }

	        //if (!HallTransfer.Instance.uiConfig.MobileEdition) Front_panel.transform.localPosition = Vector3.zero;
		}

	    public void Confirm(string title, string message, MessageBoxCallback2 _callback = null, float _fWaitTime = 5.0f)
	    {
	        Show(title, message, ButtonStyle.Confirm, _callback, _fWaitTime);
	    }

	    /// <summary>
		/// confirm按钮点击
		/// </summary>
		public void Onclick_Confirm()
		{
            if (callback != null) callback(MessageBoxResult.Confirm);
		}
		
		/// <summary>
		/// yes按钮点击
		/// </summary>
		public void Onclick_Yes()
		{
			//Debug.Log("<color=red>"+MessageBoxResult.Yes+"</color>");
			if (callback != null) callback(MessageBoxResult.Yes);
            gameObject.SetActive(false);
        }
		/// <summary>
		/// 否决按钮点击
		/// </summary>
		public void Onclick_No()
		{
			//Debug.Log("<color=red>"+_ResualtStyle+"</color>");
            if (callback != null) callback(MessageBoxResult.No);
            gameObject.SetActive(false);
        }
		/// <summary>
		/// OK按钮点击
		/// </summary>
		public void Onclick_Ok()
		{
			//Debug.Log("<color=red>"+_ResualtStyle+"</color>");
            if (callback != null) callback(MessageBoxResult.OK);
            gameObject.SetActive(false);
        }
		/// <summary>
		/// 取消按钮点击
		/// </summary>
		public void Onclick_Cancel()
		{
			//Debug.Log("<color=red>"+_ResualtStyle+"</color>");
            if (callback != null) callback(MessageBoxResult.Cancel);
            gameObject.SetActive(false);
        }
		/// <summary>
		/// 倒计时
		/// </summary>
		public void Onclick_WaitTime()
		{
			//Debug.Log("<color=red>"+_ResualtStyle+"</color>");
            if (callback != null) callback(MessageBoxResult.Timeout);
            gameObject.SetActive(false);
        }

		IEnumerator WaitTime(float _time)
		{
            string text = Title_lbl.text;
            while (_time > 0f)
            {
                yield return new WaitForFixedUpdate();
                _time -= Time.deltaTime;

                if (_time < 30.0f)
                {
                    Title_lbl.text = text + string.Format(" ({0})", (int)_time);
                }
            }
		   
			Onclick_WaitTime();
		}
	}
	
}

