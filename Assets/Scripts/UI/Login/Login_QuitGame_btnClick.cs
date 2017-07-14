using UnityEngine;
using System.Collections;

public class Login_QuitGame_btnClick : MonoBehaviour {

	void	OnClick()
	{
		LoginTransfer.Instance.ncQuitGame();
	}

}
