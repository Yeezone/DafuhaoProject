using UnityEngine;
using com.QH.QPGame.Fishing;

public class testPowerDown: MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
	
	void OnClick()
	{
		CanonCtrl.Instance.singleCanonList[ CanonCtrl.Instance.realCanonID ].C_S_GunPowerDown();
	}
	
}

