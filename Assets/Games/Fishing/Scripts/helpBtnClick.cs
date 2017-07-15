using UnityEngine;
using System.Collections;

public class helpBtnClick : MonoBehaviour {

	public Transform		windowHelp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick (){
		windowHelp.gameObject.SetActive(true);
		Transform label0 = this.transform.FindChild("front_panel").FindChild("label0");
		Transform hand = this.transform.FindChild("front_panel").FindChild("hand");
		hand.GetComponent<Animator>().Play(0);
		label0.GetComponent<Animator>().Play(0);
	}
}
