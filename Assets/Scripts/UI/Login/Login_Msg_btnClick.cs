using UnityEngine;

public class Login_Msg_btnClick : MonoBehaviour {

	public GameObject log_lable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnClick()	
	{
		if (LoginTransfer.Instance.isQuitGame) 
		{
			Debug.LogWarning("quit game!");
			LoginTransfer.Instance.ncQuitGame();
		} else 
		{	
			LoginTransfer.Instance.msgBox.gameObject.SetActive (false);
			if (LoginTransfer.Instance.ncConnectQuest != null) 
			{
				log_lable.GetComponent<UILabel> ().text = "正在连接服务器...";
				LoginTransfer.Instance.ncConnectQuest();
			}
		}
	}
	public void	ReLogin()
	{
		LoginTransfer.Instance.ncConnectQuest();
	}
}
