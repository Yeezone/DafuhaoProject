using UnityEngine;
using System.Collections;

public class UIMaryRewadTimeItem : MonoBehaviour {

    public UISprite m_SpriteTimes0;
    public UISprite m_SpriteTimes1;

    public bool m_bIsReward = false;

    private float m_fWorkTime = 0;

    private bool m_bIsSprite = false;
	// Use this for initialization
	void Start () {

        RectReward();
	}
	
	// Update is called once per frame
	void Update () {

        m_fWorkTime += Time.deltaTime;
        if (m_fWorkTime >= 0.5f && m_bIsReward)
        {
            m_SpriteTimes0.gameObject.SetActive(!m_bIsSprite);
            m_SpriteTimes1.gameObject.SetActive(m_bIsSprite);
            m_bIsSprite = !m_bIsSprite;
            m_fWorkTime = 0;
        }
	
	}

    public void SetReward(bool _bIsReward)
    {
        m_bIsReward = _bIsReward;
        m_SpriteTimes0.gameObject.SetActive(!_bIsReward);
        m_SpriteTimes1.gameObject.SetActive(_bIsReward);
    }

    public void RectReward()
    {
        SetReward(false);
    }
}
