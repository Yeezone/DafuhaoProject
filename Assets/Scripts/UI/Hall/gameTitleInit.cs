using UnityEngine;
using System.Collections;
using com.QH.QPGame.Lobby.Surfaces;

public class gameTitleInit : MonoBehaviour {

	public	UILabel		GameTitleLabel;
	public	UILabel		GameVersionLabel;

	// Use this for initialization
	void Start () {
		Invoke("GetTitleName",1.0f);
	}

	void GetTitleName()
	{
		GameTitleLabel.text = this.GetComponent<SurfaceGame>().GetRoomTitle();
		GameVersionLabel.text = "v" + this.GetComponent<SurfaceGame>().GetGameVersion();
	}
}
