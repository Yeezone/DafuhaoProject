using UnityEngine;
using System.Collections;

public class CHelpManger : MonoBehaviour {

    public CWindowShowHide_JSYS m_cHelpWindow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseBT_Onclick()
    {
        m_cHelpWindow.HideWindow();
    }
}
