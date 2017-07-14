using UnityEngine;

public class Login_InputSelected : MonoBehaviour {

	public GameObject accountInput;
	public GameObject passwardInput;

	public bool isAccount;
	public bool isPassword;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnClick()
	{
		if (isAccount) {
			accountInput.gameObject.GetComponent<UIInput>().isSelected = true;
		//	passwardInput.gameObject.GetComponent<UIInput>().isSelected = false;
		}
		else if (isPassword) {
		//	accountInput.gameObject.GetComponent<UIInput>().isSelected = false;
			passwardInput.gameObject.GetComponent<UIInput>().isSelected = true;
		}
	}
}
