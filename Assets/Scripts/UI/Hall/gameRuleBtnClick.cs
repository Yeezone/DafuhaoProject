using UnityEngine;
using System.Collections;



public class gameRuleBtnClick : MonoBehaviour {

	public	UITextList		chatArea;	//文本框
	public	uint			gameid;		//游戏ID
	public	uint			ruleCount;	//游戏规则按钮数量
	private	string			tempText = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick ()
	{
		chatArea.GetComponent<UITextList>().Clear();
		tempText = this.transform.parent.FindChild("rule_" + gameid).GetComponent<UILabel>().text;

		char[] delimiterChars = {'\r','\n'};
		string[] words = tempText.Split(delimiterChars);
		foreach(string s in words)
		{
			chatArea.Add(s);
		}
		chatArea.transform.parent.FindChild("Scroll Bar").GetComponent<UIScrollBar>().value = 0;
	}
}
