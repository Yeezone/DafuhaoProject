using UnityEngine;

public class Login_Guest_btnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		Debug.LogWarning("Login_Guest_btnClick~~~");
	//	LoginTransfer.Instance.cnLoginMsg(1);
		if(LoginTransfer.Instance.ncGuestBtnClick != null)
		{
			LoginTransfer.Instance.ncGuestBtnClick();
		}

	}
}
