using UnityEngine;

public class safeBox_request_btnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
	//	Debug.LogWarning ("cnSafetyBoxRequest-----------------:"); 
	//	HallTransfer.Instance.cnSafetyBoxAnswer( 0 );
//		if( HallTransfer.Instance.ncSafetyBoxRequest != null )
//		{
//			HallTransfer.Instance.ncSafetyBoxRequest();
//		}
		if( HallTransfer.Instance.ncSafetyBoxClick != null )
		{
			HallTransfer.Instance.ncSafetyBoxClick("1");
		}
	}
}
