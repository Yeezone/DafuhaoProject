using UnityEngine;
using System.Collections;
using com.QH.QPGame.Fishing;

public class UguiResetWindowSize : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vec = this.transform.localScale;
		vec.x = ((float)Screen.width) / (((float)Screen.width - 1280f)*0.5f + 1280f);//Utility.resol_1136x640.x;
		vec.y = ((float)Screen.height) / (((float)Screen.height - 750f)*0.4545454545454545f + 750f);//Utility.resol_1136x640.y;
		this.transform.localScale = vec;
		//  修改屏幕分辨率的同时,修改锁定线的起始位置.
        if (LockCtrl.Instance._line!=null)
        {
            LockCtrl.Instance._line.position = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].transform.FindChild("canonBarrelTrans").position;
        }
	}
}