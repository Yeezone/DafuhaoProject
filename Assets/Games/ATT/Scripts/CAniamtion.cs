using UnityEngine;
using System.Collections;

public class CAniamtion : MonoBehaviour
{

    public GameObject[] m_gSprite = new GameObject[0];
    public float m_fSpeed = 0.05f;
    public float m_fWorkTime;

    public GameObject m_gGray;
    public Color m_cColor;

    public Color[] m_cRYGColor = new Color[3] { Color.red, Color.yellow, Color.green };

    /// <summary>
    /// 当前显示的下标
    /// </summary>
    public int m_iCurrentIndex = 0;
    /// <summary>
    /// 
    /// </summary>
    private bool _bIsOpenAnimation;
    public bool m_bIsOpenAnimation
    {
        get { return _bIsOpenAnimation; }
        set
        {
            _bIsOpenAnimation = value;
            if (_bIsOpenAnimation == false)
            {
                m_gGray.SetActive(true);
                m_gGray.GetComponent<UIWidget>().width = 10;
                m_gGray.GetComponent<UIWidget>().height = 10;
                m_gGray.GetComponent<UISprite>().color = m_cColor;
            }
            else
            {
                m_gGray.SetActive(true);
                m_fWorkTime = 0;
                SetCurrentIndex(m_iCurrentIndex);
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_bIsOpenAnimation && m_fWorkTime >= m_fSpeed)
        {

            int tempCurrentIndex = m_iCurrentIndex + 1;
            if (tempCurrentIndex >= m_gSprite.Length) tempCurrentIndex = 0;
            SetCurrentIndex(tempCurrentIndex);
            m_fWorkTime = 0;

        }
        m_fWorkTime += Time.deltaTime;
    }
    /// <summary>
    /// 设置当前节点
    /// </summary>
    /// <param name="_iCurrentIndex"></param>
    public void SetCurrentIndex(int _iCurrentIndex)
    {
        m_iCurrentIndex = _iCurrentIndex;
        m_gGray.GetComponent<UIWidget>().width = 10;
        m_gGray.GetComponent<UIWidget>().height = 10;
        m_gGray.gameObject.GetComponent<UISprite>().color = m_cRYGColor[m_iCurrentIndex];
    }

    public void SetAllFalse()
    {
        for (int i = 0; i < m_gSprite.Length; i++)
        {
            m_gSprite[i].gameObject.SetActive(false);
        }
    }
}
