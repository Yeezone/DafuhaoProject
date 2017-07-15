using UnityEngine;
using System.Collections;
using com.QH.QPGame.Fishing;

public class MaxSizeFishingGame : MonoBehaviour {

	public GameObject	Ugui_parent = null;

	private bool maxWindow = false;
	private Vector2 tempWindowSize = new Vector2(0f,0f);
//	public GameObject CanonCache;
	 
	void Start(){
//		CanonCache.GetComponent<UIAnchor>().enabled = true;
	}

	void OnClick()
	{
//		CanonCache.GetComponent<UIAnchor>().enabled = true;
		if(!maxWindow)
		{
			tempWindowSize = new Vector2((float)Screen.width,(float)Screen.height);
			Screen.fullScreen = true;
			maxWindow = true;
		}else{
			Screen.SetResolution((int)tempWindowSize.x,(int)tempWindowSize.y,false);
			maxWindow = false;
		}

//		Vector3 vec = Ugui_parent.transform.localScale;
//		vec.x = ((float)Screen.width) / (((float)Screen.width - 1280f)*0.5f + 1280f);//Utility.resol_1136x640.x;
//		vec.y = ((float)Screen.height) / (((float)Screen.height - 750f)*0.4545454545454545f + 750f);//Utility.resol_1136x640.y;
//		Ugui_parent.transform.localScale = vec;
//		//  修改屏幕分辨率的同时,修改锁定线的起始位置.
//		LockCtrl.Instance._line.position = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].transform.FindChild("canonBarrelTrans").position;

	}
}
