using UnityEngine;

public class security_setGameNameLabel : MonoBehaviour {

	public	Transform			gameNamePopupList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public	void	setGameNameLabel()
	{
		string tempText = gameNamePopupList.GetComponent<UIPopupList>().value;
		this.GetComponent<UILabel>().text = tempText;
	}

}
