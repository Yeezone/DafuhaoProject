using UnityEngine;
using System.Collections;

public class CButtonManger : MonoBehaviour {


    public CWindowShowHide_JSYS m_cMenuBt0;
    public CWindowShowHide_JSYS m_cMenuBt1;
    public CWindowShowHide_JSYS m_cMenuBt2;
    public CWindowShowHide_JSYS m_cMenuBt3;
    public bool m_bIsopen = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenCloseMenu()
    {

        if (m_bIsopen)
        {
            m_cMenuBt0.HideWindow();
            m_cMenuBt1.HideWindow();
            m_cMenuBt2.HideWindow();
            m_cMenuBt3.HideWindow();
        }
        else
        {
            m_cMenuBt0.ShowWindow();
            m_cMenuBt1.ShowWindow();
            m_cMenuBt2.ShowWindow();
            m_cMenuBt3.ShowWindow();

        }
        m_bIsopen = !m_bIsopen;
    }
}
