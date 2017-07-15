using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.SHZ
{

    public class UIBigSmallRecord : MonoBehaviour
    {
        public GameObject m_gRecordPrfab;

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<GameObject> m_gRecordList = new List<GameObject>();

        public Vector3 m_vFirstPos = new Vector3(597, 251, 0);
        public Vector3 m_vEndPos = new Vector3(-305, 251, 0);

        public GameObject m_gRecordParent;


        public float m_fDistance = 53;

        public int m_iLimitCount = 13;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void sdsdad()
        {
            AddRecord(0);
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="_iIndex">大“2”小“0”和“1”下标</param>
        public void AddRecord(int _iIndex)
        {
            GameObject temp_obj;
            if (m_gRecordList.Count >= (m_iLimitCount))
            {
                temp_obj = m_gRecordList[0].gameObject;
                temp_obj.transform.name = m_gRecordList[0].gameObject.transform.name;
            }
            else
            {
                temp_obj = (GameObject)Instantiate(m_gRecordPrfab, new Vector3(0, 0, 0), m_gRecordPrfab.transform.localRotation);
                temp_obj.transform.parent = m_gRecordParent.transform;
                temp_obj.transform.localScale = new Vector3(1, 1, 1);
                temp_obj.transform.name = "RecordItem_" + m_gRecordList.Count.ToString();
            }
            temp_obj.transform.parent = m_gRecordParent.transform;
            temp_obj.transform.localScale = new Vector3(1, 1, 1);
            temp_obj.transform.localPosition = m_vFirstPos;
            temp_obj.GetComponent<UISprite>().spriteName = "BS_Rcord" + _iIndex.ToString() + "0";
            if (m_gRecordList.Count == 13) m_gRecordList.Remove(m_gRecordList[0]);
            if ((m_iLimitCount - 1) > m_gRecordList.Count)
            {
                temp_obj.GetComponent<TweenPosition>().from = m_vFirstPos;
                temp_obj.GetComponent<TweenPosition>().to = new Vector3(m_vEndPos.x + m_fDistance * m_gRecordList.Count, m_vEndPos.y, m_vEndPos.z);
                temp_obj.GetComponent<TweenPosition>().ResetToBeginning();
                temp_obj.GetComponent<TweenPosition>().enabled = true;
            }
            else
            {
                temp_obj.GetComponent<TweenPosition>().from = m_vFirstPos;
                temp_obj.GetComponent<TweenPosition>().to = m_gRecordList[m_gRecordList.Count - 1].transform.localPosition;
                temp_obj.GetComponent<TweenPosition>().ResetToBeginning();
                temp_obj.GetComponent<TweenPosition>().enabled = true;
                for (int i = m_gRecordList.Count - 1; i > 0; i--)
                {
                    m_gRecordList[i].GetComponent<TweenPosition>().from = m_gRecordList[i].transform.localPosition;
                    m_gRecordList[i].GetComponent<TweenPosition>().to = m_gRecordList[i - 1].transform.localPosition;
                    m_gRecordList[i].GetComponent<TweenPosition>().ResetToBeginning();
                    m_gRecordList[i].GetComponent<TweenPosition>().enabled = true;
                }
                m_gRecordList[0].GetComponent<TweenPosition>().from = m_gRecordList[0].transform.localPosition;
                m_gRecordList[0].GetComponent<TweenPosition>().to = new Vector3(-597, m_vFirstPos.y, m_vFirstPos.z);
                m_gRecordList[0].GetComponent<TweenPosition>().ResetToBeginning();
                m_gRecordList[0].GetComponent<TweenPosition>().enabled = true;


            }
            m_gRecordList.Add(temp_obj);

        }
    }
}
