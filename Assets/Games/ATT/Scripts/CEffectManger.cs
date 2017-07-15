using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEffectManger : MonoBehaviour {
    public List<GameObject> m_gEffectList = new List<GameObject>();
    public GameObject m_gMyEffect;

    public static CEffectManger _instance;

    void Awake()
    {
        _instance = this;
        SetAllFalse();
    }
    void OnDestroy()
    {
        _instance = null;
    }
    public void SetAllFalse()
    {
        for (int i = 0; i < m_gEffectList.Count; i++)
        {
            m_gEffectList[i].SetActive(false);
        }
        m_gMyEffect.SetActive(false);

    }
}
