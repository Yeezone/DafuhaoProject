using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPrizeManger : MonoBehaviour
{
    public static CPrizeManger _instance;
    public List<GameObject> m_lPrizeList = new List<GameObject>();

    public GameObject m_gPrizePool;
    public UILabel m_lMyReward;
    public UILabel m_lReward;



    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }
    // Use this for initialization
    void Start()
    {

        ShowHidePrize(m_lPrizeList.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHidePrize(int _iIndex)
    {
        for (int i = 0; i < m_lPrizeList.Count; i++)
        {
            float a = 0;
            if (_iIndex == i) a = 1;

            m_lPrizeList[i].GetComponent<TweenAlpha>().from = m_lPrizeList[i].GetComponent<UISprite>().alpha;
            m_lPrizeList[i].GetComponent<TweenAlpha>().to = a;
            m_lPrizeList[i].GetComponent<TweenAlpha>().ResetToBeginning();
            m_lPrizeList[i].GetComponent<TweenAlpha>().enabled = true;
        }
    }

    public void OpenPrizePool(int _iIndex)
    {
        ShowHidePrize(_iIndex);
        StartCoroutine(WaitTimeChange());
    }
    IEnumerator WaitTimeChange()
    {
        yield return new WaitForSeconds(5.0f);
        ShowHidePrize(0);
    }
}
