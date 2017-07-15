using UnityEngine;
using System.Collections;
using com.QH.QPGame.Fishing;

public class testPowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	void OnClick()
	{
		CanonCtrl.Instance.singleCanonList[ CanonCtrl.Instance.realCanonID ].C_S_GunPowerUp();
	}

}
