using UnityEngine;
using System.Collections;

public class UIChipEffect : MonoBehaviour {

	bool isAnimation = false;
	public string lastFrameName = "";
	public GameObject effectSprite;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if(isAnimation && effectSprite.GetComponent<UISprite>().spriteName == lastFrameName)
		{
			isAnimation = false;
			Destroy(effectSprite.GetComponent<UISpriteAnimation>());
			effectSprite.GetComponent<UISprite>().spriteName = "blank";
		}
	}

	void OnClick()
	{
		isAnimation = true;
		if(effectSprite != null)
		{
			effectSprite.AddComponent<UISpriteAnimation>();
			UISpriteAnimation effect = effectSprite.GetComponent<UISpriteAnimation>();
			effect .framesPerSecond = 20;
		}
	}
}
