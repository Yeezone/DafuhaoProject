using UnityEngine;
using System.Collections;

public class CWindowShowHide_JSYS : MonoBehaviour {

    public bool m_bIsShow = false;

    public Vector3 m_vShowPos = new Vector3(0,0,0);
    public Vector3 m_vHidePos = new Vector3(-2000, 0, 0);

    public Vector3 m_vShowScale = new Vector3(1,1,1);
    public Vector3 m_vHideScale = new Vector3(0, 0, 0);

    public float m_fShowAlpha = 1.0f;
    public float m_fHideAlpha = 1.0f;

    [System.Serializable]
    public class CShowWindow
    {
        public bool m_bIsEnablePos = true;
        public bool m_bIsEnableScale = true;
        public bool m_bIsEnableAlpha = true;
    }
    public CShowWindow m_cShow = new CShowWindow();
    [System.Serializable]
    public class CHideWindow
    {
        public bool m_bIsEnablePos = true;
        public bool m_bIsEnableScale = true;
        public bool m_bIsEnableAlpha = true;
    }
    public CHideWindow m_cHide = new CHideWindow();
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
        if(m_cShow.m_bIsEnablePos) ShowPos();
        if (m_cShow.m_bIsEnableScale) ShowScale();
        if (m_cShow.m_bIsEnableAlpha)  ShowAlpha();
        
    }

    public void ShowPos()
    {
        TweenPosition tweenpos = this.GetComponent<TweenPosition>();
        if (tweenpos != null && m_bIsShow)
        {
            tweenpos.from = this.transform.localPosition;
            tweenpos.to = m_vShowPos;
            tweenpos.ResetToBeginning();
            tweenpos.enabled = true;
        }
    }

    public void ShowScale()
    {
        TweenScale tweenscal = this.GetComponent<TweenScale>();
        if (tweenscal != null && m_bIsShow)
        {
            tweenscal.from = this.transform.localScale;
            tweenscal.to = m_vShowScale;
            tweenscal.ResetToBeginning();
            tweenscal.enabled = true;
        }
    }
    public void ShowAlpha()
    {
        TweenAlpha temp_Alpha = this.GetComponent<TweenAlpha>();
        if (temp_Alpha != null && m_bIsShow)
        {
            temp_Alpha.from = this.transform.GetComponent<UISprite>().alpha;
            temp_Alpha.to = m_fShowAlpha;
            temp_Alpha.ResetToBeginning();
            temp_Alpha.enabled = true;
        }
    }
    /// <summary>
    /// 隐藏比倍窗口
    /// </summary>
    public void HideWindow()
    {
        m_bIsShow = false;
        if (m_cHide.m_bIsEnablePos) HidePos();
        if (m_cHide.m_bIsEnableScale) HideScale();
        if (m_cHide.m_bIsEnableAlpha) HideAlpha();

    }

    public void HidePos()
    {
        TweenPosition tweenpos = this.GetComponent<TweenPosition>();
        if (tweenpos != null && !m_bIsShow)
        {
            tweenpos.from = this.transform.localPosition;
            tweenpos.to = m_vHidePos;
            tweenpos.ResetToBeginning();
            tweenpos.enabled = true;
        }
    }

    public void HideScale()
    {
        TweenScale tweenscal = this.GetComponent<TweenScale>();
        if (tweenscal != null && !m_bIsShow)
        {
            tweenscal.from = this.transform.localScale;
            tweenscal.to = m_vHideScale;
            tweenscal.ResetToBeginning();
            tweenscal.enabled = true;
        }
    }

    public void HideAlpha()
    {
        TweenAlpha temp_Alpha = this.GetComponent<TweenAlpha>();
        if (temp_Alpha != null && !m_bIsShow)
        {
            temp_Alpha.from = this.transform.GetComponent<UISprite>().alpha;
            temp_Alpha.to = m_fHideAlpha;
            temp_Alpha.ResetToBeginning();
            temp_Alpha.enabled = true;
        }
    }

}
