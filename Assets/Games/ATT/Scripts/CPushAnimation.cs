using UnityEngine;
using System.Collections;

public class CPushAnimation : MonoBehaviour {

    public float m_fWorkTime = 0;
    public float m_fSpeed = 0.2f;
    public string m_strName;

    private bool m_bIsOpen = false;
    public int m_iIndex = 0;

    public static CPushAnimation _instance;
    void Awake()
    {
        _instance = this;
    }
    void OnDestroy()
    {
        _instance = null;
    }
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        m_fWorkTime += Time.deltaTime;
        if (m_bIsOpen && m_fWorkTime >= m_fSpeed)
        {
            if (m_iIndex == 0)
            {
                m_iIndex = 1;
                Color _color = this.transform.GetComponent<UISprite>().color;
                _color.a = 0;
                this.transform.GetComponent<UISprite>().color = _color;
            }
            else 
            {
                m_iIndex = 0;
                Color _color = this.transform.GetComponent<UISprite>().color;
                _color.a = 1.0f;
                this.transform.GetComponent<UISprite>().color = _color;
            }
            m_fWorkTime = 0;
        }
	}

    public void SetPushAnimation(string _strName)
    {
        m_strName = _strName;
        this.transform.GetComponent<UISprite>().spriteName = m_strName;
        Color _color = this.transform.GetComponent<UISprite>().color;
        _color.a = 1.0f;
        this.transform.GetComponent<UISprite>().color = _color;
        m_iIndex = 0;
        m_fWorkTime = 0;
        PlayPushAnimation();
    }

    public void StopPushAnimation()
    {
        m_bIsOpen = false;
    }
    public void PlayPushAnimation()
    {
        m_bIsOpen = true;
    }
}
