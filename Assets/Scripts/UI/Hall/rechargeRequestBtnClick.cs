using UnityEngine;

public class rechargeRequestBtnClick : MonoBehaviour {

	public bool isRecharge;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	public void OnClick()
	{
		if (isRecharge) 
		{
			Debug.LogWarning("Recharge");
			if(HallTransfer.Instance.ncRechargeEventClick != null)
			{
				HallTransfer.Instance.ncRechargeEventClick();
			}
		} 
		else {
			Debug.LogWarning("Award");
			if(HallTransfer.Instance.ncAwardEventClick != null)
			{
				HallTransfer.Instance.ncAwardEventClick();
			}
		}
	}

}
