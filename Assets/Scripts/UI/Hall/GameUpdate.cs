using UnityEngine;
using System.Collections;
using com.QH.QPGame.Lobby.Surfaces;

public class GameUpdate : MonoBehaviour {

	public void CancelBtnClick()
	{
		gameObject.SetActive(false);
		
		SurfaceContainer container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
		SurfaceDownload download = container.GetSurface<SurfaceDownload>();
		download.CancelTask();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
