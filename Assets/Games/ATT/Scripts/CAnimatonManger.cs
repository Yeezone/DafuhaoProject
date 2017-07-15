using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAnimatonManger : MonoBehaviour {
    public List<CAniamtion> m_lAnimationManger = new List<CAniamtion>();
    public Color[] m_cColor = new Color[3];
    public float m_fSpeed = 0.1f;

    public Color [] m_cRYGColor =  new Color[3]{Color.red,Color.yellow,Color.green};

    public GameObject []m_gSprite = new GameObject [2];

    public int m_iCurrentIntex = 0;

    //1-8
    Vector3 vec18 = new Vector3(-60.0f, -43, 0);
    //9-12
    Vector3 vec912 = new Vector3(60.5f, -23, 0);
    //13-20
    Vector3 vec1320 = new Vector3(60.5f, 43, 0);
    //21-24
    Vector3 vec2124 = new Vector3(-60.5f, 23, 0);

    private bool _bIsOpen;
    public bool m_bIsOpen
    {
        get { return _bIsOpen; }
        set
        {
            m_fWorkTime = 0;
            if (m_gSprite[0] != null)
            m_gSprite[0].gameObject.SetActive(false);
            if (m_gSprite[1] != null)
            m_gSprite[1].gameObject.SetActive(false);
            _bIsOpen = value;

            SetAnimation();
            SetOpenAnimation(_bIsOpen);
            
        }
    }

    public float m_fWorkTime = 0;

    
	// Use this for initialization
	void Start () {
        m_bIsOpen = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_bIsOpen && m_fWorkTime > m_fSpeed)
        {
            if (m_gSprite[0] != null)
            m_gSprite[0].gameObject.SetActive(false);
            if (m_gSprite[1] != null)
            m_gSprite[1].gameObject.SetActive(false);
            if (m_gSprite[m_iCurrentIntex] != null)
            m_gSprite[m_iCurrentIntex].gameObject.SetActive(true);
            m_iCurrentIntex += 1;
            if (m_iCurrentIntex >= 2) m_iCurrentIntex = 0;
            m_fWorkTime = 0;
        }
        m_fWorkTime += Time.deltaTime;
	}
    public void SetAnimation()
    {
        int temp = 0;
        float temp_fdistance = 17.0f + 2.0f / 7.0f;
        for (int i = 0; i < m_lAnimationManger.Count; i++)
        {
            //设置位置
            if (i < 8) m_lAnimationManger[i].transform.localPosition = new Vector3(vec18.x + (float)i * temp_fdistance,vec18.y,0);
            if (i >= 8 && i < 12) m_lAnimationManger[i].transform.localPosition = new Vector3(vec912.x, vec912.y + (float)(i - 8) * temp_fdistance, 0);
            if (i >= 12 && i < 20) m_lAnimationManger[i].transform.localPosition = new Vector3(vec1320.x - (float)(i-12) * temp_fdistance, vec1320.y, 0);
            if (i >= 20 && i < 24) m_lAnimationManger[i].transform.localPosition = new Vector3(vec2124.x, vec2124.y - (float)(i-20) * temp_fdistance, 0);
     

            m_lAnimationManger[i].m_cRYGColor = m_cRYGColor;
            m_lAnimationManger[i].SetCurrentIndex(temp);
            m_lAnimationManger[i].m_cColor = m_cColor[temp];
                temp += 1;
                if (temp >= 3) temp = 0;

                m_lAnimationManger[i].m_fSpeed = m_fSpeed;
                m_lAnimationManger[i].m_bIsOpenAnimation = false;
        }
    }

    public void SetOpenAnimation(bool _bIsOpen)
    {
        for (int i = 0; i < m_lAnimationManger.Count; i++)
        {
            
            m_lAnimationManger[i].m_bIsOpenAnimation = _bIsOpen;
        }
    }
}
