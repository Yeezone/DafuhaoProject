using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Lobby;



public class WebInline : MonoBehaviour 
{
	public enum WebType
	{
		Notice,				//公告
		Ranking,			//富豪榜
		Recharge,			//充值
		Share,				//分享
		LuckDraw,			//抽奖
		max
	}
	
	public WebType CurWebType;

	public int WebTop;
	public int WebBottom;
	public int WebLeft;
	public int WebRight;
	
//#if !UNITY_EDITOR
	
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
	//1. First of all, we need a reference to hold an instance of UniWebView
	private UniWebView _webView;
	private string _errorMessage;

#endif

    void OnEnable()
	{
		string url = "";
		string title = "";
		switch (CurWebType)
		{
		case WebType.Notice:
			title = "公告";
			url = GameApp.GameData.BackStorgeUrl+"/ReferencePage/Notice.aspx?Platform="+Application.platform;
			break;
		case WebType.Ranking:
			title = "排行榜";
			url = GameApp.GameData.BackStorgeUrl+
				"/ReferencePage/ScoreRanking.aspx?Platform="+Application.platform+
					"&UserID="+GameApp.GameData.UserInfo.UserID+
					"&Token="+GameApp.GameData.PrivateKey;
			break;
		case WebType.Share:
			title = "分享";
			url = GameApp.GameData.BackStorgeUrl+
				"/ReferencePage/Share.aspx?Platform="+Application.platform+
					"&UserID="+GameApp.GameData.UserInfo.UserID+
					"&Token="+GameApp.GameData.PrivateKey;
			break;
		case WebType.Recharge:
			title = "充值";
			url = GameApp.GameData.BackStorgeUrl+
				"/MobileRechargeCenter/Default.aspx?Platform="+Application.platform+
					"&UserID="+GameApp.GameData.UserInfo.UserID+
					"&Token="+GameApp.GameData.PrivateKey;
			break;
		case WebType.LuckDraw:
			title = "抽奖";
			url = GameApp.GameData.BackStorgeUrl+
				"/ReferencePage/LuckDraw.aspx?Platform="+Application.platform+
					"&UserID="+GameApp.GameData.UserInfo.UserID+
					"&Token="+GameApp.GameData.PrivateKey;
			break;
		default:
			url = "";
			break;
		}

#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
        _webView = gameObject.AddComponent<UniWebView>();
		_webView.backButtonEnable = false;
        _webView.zoomEnable = true;
        //_webView.CleanCache();
        _webView.SetSpinnerLabelText("努力加载页面中。。。");
        _webView.SetShowSpinnerWhenLoading(true);
        //_webView.SetBackgroundColor(Color.red);
        _webView.OnLoadComplete += OnLoadComplete;
		_webView.OnWebViewShouldClose += OnWebViewShouldClose;
		//_webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
		_webView.insets = new UniWebViewEdgeInsets(

			(int)((float)UniWebViewHelper.screenHeight/640f*WebTop),
			(int)((float)UniWebViewHelper.screenWidth/1136f*WebLeft),
			(int)((float)UniWebViewHelper.screenHeight/640f*WebBottom),
			(int)((float)UniWebViewHelper.screenWidth/1136f*WebRight)
		);
		//_webView.InsetsForScreenOreitation += InsetsForScreenOreitation;
		
        _webView.Load(url);
        _errorMessage = null;
#elif UNITY_STANDALONE_WIN
		Win32Api.Instance.OpenUrlInBrowser(title, url, Screen.width - WebRight - WebLeft, Screen.height - WebBottom - WebTop);
#endif

    }

	void OnDisable()
	{

#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
        if (_webView != null)
		{
			_webView.OnLoadComplete -= OnLoadComplete;
			_webView.OnWebViewShouldClose -= OnWebViewShouldClose;
            //_webView.CleanCache();
			Destroy(_webView);
			_webView = null;
		}
#elif UNITY_STANDALONE_WIN
        Win32Api.Instance.CloseBrowser();
#endif

    }

#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
	void OnLoadComplete(UniWebView webView, bool success, string errorMessage) 
	{
        Logger.UI.Log("WebNotice Load Result:"+success+" Err:"+errorMessage);

		if (success) 
		{
			webView.Show();
		} else {
			Debug.Log("Something wrong in webview loading: " + errorMessage);
			_errorMessage = errorMessage;
		}
	}


	bool OnWebViewShouldClose(UniWebView webView) 
	{
        Logger.UI.Log("OnWebViewShouldClose");
		if (webView == _webView) {
			_webView = null;
			return true;
		}
		return false;
	}

#endif
//#endif

}
