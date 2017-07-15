using UnityEngine;
using System.Collections;

public class CHelp : MonoBehaviour {

    public static CHelp _instance;
    public bool m_bIsShow;
    public UILabel m_text0;
    public UILabel m_text1;
    public UILabel m_text2;
    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    /// <summary>
    /// 显示比倍窗口
    /// </summary>
    public void ShowWindow()
    {
        m_bIsShow = true;
        this.GetComponent<TweenPosition>().from = this.transform.localPosition;
        this.GetComponent<TweenPosition>().to = new Vector3(0, 0, 0);
        this.GetComponent<TweenPosition>().enabled = true;
    }
    /// <summary>
    /// 隐藏比倍窗口
    /// </summary>
    public void HideWindow()
    {
        m_bIsShow = false;
        this.GetComponent<TweenPosition>().from = this.transform.localPosition;
        this.GetComponent<TweenPosition>().to = new Vector3(-1000, 1000, 0);
        this.GetComponent<TweenPosition>().enabled = true;
    }

}
