using UnityEngine;
using System.Collections;

public class CEffectItem1 : MonoBehaviour {

    public float m_fShowTime;
    public float m_fWorkTime;
    public bool m_bIsPlay = false;
    public GameObject m_gScale;
    public GameObject m_gName;
    public string m_strName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_bIsPlay && m_fWorkTime >= m_fShowTime)
        {
            m_bIsPlay = false;
            this.gameObject.SetActive(false);
        }
        m_fWorkTime += Time.deltaTime;
	}

    public void SetEffect(int _iCardType)
    {
        m_bIsPlay = true;
        m_fWorkTime = 0;
        m_gName.GetComponent<UISprite>().spriteName = m_strName + _iCardType.ToString();
        m_gScale.GetComponent<TweenAlpha>().enabled = true;
        m_gScale.GetComponent<TweenScale>().enabled = true;
    }
}
