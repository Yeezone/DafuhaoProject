using UnityEngine;

public class lockBtnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	void OnClick() {
		GameObject tempLockOrUnLock =  HallTransfer.Instance.uiConfig.window_LockOrUnLock;
		tempLockOrUnLock.SetActive(true);
		Transform tempFrontPanel = tempLockOrUnLock.transform.FindChild("front_panel");
		if(HallTransfer.Instance.uiConfig.LockOrUnLockAccount)
		{
			//锁定状态 打开解锁界面
			tempFrontPanel.FindChild("label0").gameObject.SetActive(false);
			tempFrontPanel.FindChild("label1").gameObject.SetActive(true);
			tempFrontPanel.FindChild("lock_btn").gameObject.SetActive(false);
			tempFrontPanel.FindChild("unlock_btn").gameObject.SetActive(true);
		}else{
			//解锁状态 打开锁定界面
			tempFrontPanel.FindChild("label0").gameObject.SetActive(true);
			tempFrontPanel.FindChild("label1").gameObject.SetActive(false);
			tempFrontPanel.FindChild("lock_btn").gameObject.SetActive(true);
			tempFrontPanel.FindChild("unlock_btn").gameObject.SetActive(false);
		}

		tempFrontPanel.FindChild("lock_btn").GetComponent<lockOrUnlockBtnClick>().restoreTempLabel();
	}
}
