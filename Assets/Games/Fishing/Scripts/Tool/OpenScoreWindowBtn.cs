using UnityEngine;
using com.QH.QPGame.Fishing;

public class OpenScoreWindowBtn : MonoBehaviour {

	public Transform ScoreLabel;
	public Transform GoldLabel;
//	private Transform CanonCtrl;
	void Awake(){

	}
	// Use this for initialization
	void Start () {
//		CanonCtrl = GameObject.Find("CanonCtrl");
//		GoldLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.goldValue.ToString();
//		ScoreLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].plyScore.ToString();
		Invoke("updateScore",0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClick()
	{
		if(CanonCtrl.Instance.autoFire) CanonCtrl.Instance.PressAutoFireButton();
		if(CanonCtrl.Instance.inLockMode) CanonCtrl.Instance.PressLockButton();
		//ScoreLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].plyScore.ToString();
		//GoldLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.goldValue.ToString();
        setScoreLabel();
		Invoke("updateScore",0.5f);
	}

	void updateScore()
	{
		if(CanonCtrl.Instance.autoFire) CanonCtrl.Instance.PressAutoFireButton();
		if(CanonCtrl.Instance.inLockMode) CanonCtrl.Instance.PressLockButton();
        setScoreLabel();
		//ScoreLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID].plyScore.ToString();
		//GoldLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.goldValue.ToString();
	}

    void setScoreLabel()
    {
        //修正SingleCanon 对象为空时的Crash
        SingleCanon scTmp = CanonCtrl.Instance.singleCanonList[CanonCtrl.Instance.realCanonID];
        if (scTmp != null)
        {
            ScoreLabel.GetComponent<UILabel>().text = scTmp.plyScore.ToString();
        }
        else
        {
            ScoreLabel.GetComponent<UILabel>().text = "0";
            //Debug.LogError("用户炮台脚本NULL,用户ID: " + CanonCtrl.Instance.realCanonID.ToString());
        }
        GoldLabel.GetComponent<UILabel>().text = CanonCtrl.Instance.goldValue.ToString();
    }
}
