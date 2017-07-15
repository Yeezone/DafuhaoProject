using UnityEngine;
using System.Collections;

public class COnDownBeing : MonoBehaviour {


    public bool m_bIsfirstDownBeing = false;
    public bool m_bIsDownBeing = false;

    public float m_fSpeed = 0.2f;
    private float m_fWorkingTime = 0;

    public delegate void DownBeingOnChangge();
    public DownBeingOnChangge m_OnChange = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_fWorkingTime += Time.deltaTime;
        if (m_fWorkingTime >= m_fSpeed && m_bIsfirstDownBeing && m_bIsDownBeing)
        {
            if (m_OnChange != null) m_OnChange();
            m_fWorkingTime = 0;
        }
	}

    public void SetDownBeingTrue()
    {
        m_bIsfirstDownBeing = true;
        StartCoroutine(WaitaSetDownBeing(0.2f));
    }

    public void SetDownBeingFalse()
    {
        m_bIsfirstDownBeing = false;
        m_bIsDownBeing = false;
    }

    IEnumerator WaitaSetDownBeing(float _fWaitTime)
    {
        yield return new WaitForSeconds(_fWaitTime);
        if (m_bIsfirstDownBeing)
        {
            m_bIsDownBeing = true;
        }
    }
}
