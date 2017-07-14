using UnityEngine;

public class safeBox_Entry_btnClick : MonoBehaviour {

	public	GameObject		passInput;
	public	GameObject		log_lable;
	public	GameObject		tempLabel;
	public	GameObject		submit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		string pass = passInput.GetComponent<UIInput>().value;
		if(pass.Length < 6)
		{
			log_lable.GetComponent<UILabel>().text = "密码不应少于6位";
			Invoke("cleanLogLabel",2.0f);
		}else{
			this.GetComponent<UIButton>().isEnabled = false;
			this.transform.parent.FindChild("close_btn").GetComponent<UIButton>().isEnabled = false;
			//HallTransfer.Instance.uiConfig.window_SafeBoxEntry.SetActive(false);
			log_lable.GetComponent<UILabel>().text = "";
			Debug.LogWarning("发送密码:" + pass);
			HallTransfer.Instance.ncSafetyBoxClick(pass);
		}
		this.gameObject.GetComponent<UIButton>().isEnabled = false;
		Invoke("resumeBtn",3.0f);

	}

	private void cleanLogLabel()
	{
		log_lable.GetComponent<UILabel>().text = "";
	}

	public	void	clearTempLabel()
	{
		tempLabel.GetComponent<UILabel>().text = "";
	}
	public	void	restoreTempLabel()
	{
		tempLabel.GetComponent<UILabel>().text = "初始密码为登录密码";
	}

	public	void resumeBtn()
	{
		this.gameObject.GetComponent<UIButton> ().isEnabled = true;
		this.transform.parent.FindChild("close_btn").GetComponent<UIButton>().isEnabled = true;
	}
}
