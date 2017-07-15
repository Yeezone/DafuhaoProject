using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRecordManger : MonoBehaviour {

    public static CRecordManger _instance;
    /// <summary>
    /// 记录列表
    /// </summary>
    public List<CRecordItem> m_lRecordList = new List<CRecordItem>();

    public int m_iRecordNum = 40;

    public GameObject m_gRecordItemPrefab;
    public float m_fDistance = 59.0f;

    public Vector3 m_vFistPos = new Vector3(440.0f, 0, 0);

    public GameObject m_gRecordParent;

    void Awake()
    {
        _instance = this;
    }
    void OnDestroy()
    {
        _instance = null;
    }
	// Use this for initialization
	void Start () {

        for (int i = 0; i < 40; i++)
        {
            AddRecord(1);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 添加记录
    /// </summary>
    /// <param name="_AnimalIndex"></param>
   public void AddRecord(int _AnimalIndex)
    {
        GameObject temp_obj;
        if(m_lRecordList.Count>= 40 )
        {
            temp_obj = m_lRecordList[0].gameObject;
            m_lRecordList.Remove(m_lRecordList[0]);
          //  temp_obj.transform.name = "RecordItem_" + m_lRecordList.Count.ToString();
        }
        else
        {
          temp_obj = (GameObject)Instantiate(m_gRecordItemPrefab, new Vector3(0, 0, 0), m_gRecordItemPrefab.transform.localRotation);
          
          temp_obj.transform.name = "RecordItem_" + m_lRecordList.Count.ToString();
          temp_obj.transform.parent = m_gRecordParent.transform;
          temp_obj.transform.localScale = new Vector3(1,1,1);
          
        }
        
        CRecordItem temp_RecordItem = temp_obj.GetComponent<CRecordItem>();
        //走兽
        if (_AnimalIndex <= 4)
        {
            temp_RecordItem.m_BackSprite.spriteName = "record_bg_B";
            temp_RecordItem.m_AnimalSprite.spriteName = "che" + _AnimalIndex.ToString();
            temp_RecordItem.m_BackCurrentpriteMask.spriteName = "record_Line_B";
            temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
        }
            //飞禽
        else if (_AnimalIndex > 4 && _AnimalIndex <= 8)
        {
            temp_RecordItem.m_BackSprite.spriteName = "record_bg_Y";
            temp_RecordItem.m_AnimalSprite.spriteName = "che" + _AnimalIndex.ToString();
            temp_RecordItem.m_BackCurrentpriteMask.spriteName = "record_Line_Y";
            temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
        }
            //鲨鱼
        else
        {
            temp_RecordItem.m_BackSprite.spriteName = "record_bg_G";
            temp_RecordItem.m_AnimalSprite.spriteName = "che" + _AnimalIndex.ToString();
            temp_RecordItem.m_BackCurrentpriteMask.spriteName = "record_Line_G";
            temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
        }
        m_lRecordList.Add(temp_RecordItem);
        SortList();
        
    }

    /// <summary>
    /// 排序
    /// </summary>
    public void SortList()
    {
        m_gRecordParent.transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < m_lRecordList.Count; i++)
        {
            m_lRecordList[i].m_BackCurrentpriteMask.gameObject.SetActive(false);
            m_lRecordList[i].transform.localPosition = new Vector3(m_vFistPos.x - (float)(m_lRecordList.Count - i -1) * m_fDistance, 0, 0);
        }
        m_lRecordList[m_lRecordList.Count-1].m_BackCurrentpriteMask.gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 左移按钮
    /// </summary>
    public void Left_Onclick()
    {
      
        Vector3 tempos = m_gRecordParent.transform.localPosition;
        tempos.x -= m_fDistance;
        m_gRecordParent.GetComponent<TweenPosition>().from = m_gRecordParent.transform.localPosition;
        m_gRecordParent.GetComponent<TweenPosition>().to = tempos;
        m_gRecordParent.GetComponent<TweenPosition>().ResetToBeginning();
        m_gRecordParent.GetComponent<TweenPosition>().enabled = true;
        
        if (tempos.x <= 0)
        {
            SpringPosition sp = SpringPosition.Begin(m_gRecordParent.gameObject, m_gRecordParent.transform.localPosition + new Vector3(0 - m_gRecordParent.transform.localPosition.x, 0, 0), 13f);
            sp.enabled = true;
            sp.worldSpace = false;
        }
       
        
    }
    /// <summary>
    /// 右移按钮
    /// </summary>
    public void Right_Onclick()
    {
        Vector3 tempos = m_gRecordParent.transform.localPosition;
        tempos.x += m_fDistance;

        m_gRecordParent.GetComponent<TweenPosition>().from = m_gRecordParent.transform.localPosition;
        m_gRecordParent.GetComponent<TweenPosition>().to = tempos;
        m_gRecordParent.GetComponent<TweenPosition>().ResetToBeginning();
        m_gRecordParent.GetComponent<TweenPosition>().enabled = true;
        
        if (tempos.x >= 1064.0f)
        {
            SpringPosition sp = SpringPosition.Begin(m_gRecordParent.gameObject, m_gRecordParent.transform.localPosition + new Vector3(1064.0f - m_gRecordParent.transform.localPosition.x, 0, 0), 13f);
            sp.enabled = true;
            sp.worldSpace = false;
        }
      
    }
    
}
