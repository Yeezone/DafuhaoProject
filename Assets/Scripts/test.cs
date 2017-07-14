using UnityEngine;
using System.Collections;
using com.QH.QPGame.Lobby;

public class WebIntroduction : MonoBehaviour {

	void OnClick(){
		string tempUrl = GameApp.GameData.OfficeSiteUrl + "/Games/IntroductionList.aspx";
		Application.OpenURL(tempUrl);
	}
}
