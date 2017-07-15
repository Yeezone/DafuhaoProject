using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.xyls
{
    public class RecordManager : MonoBehaviour
    {

        public static RecordManager _instance;
        /// <summary>
        /// 记录列表
        /// </summary>
        public List<RecordItem> m_lRecordList = new List<RecordItem>();

        public int m_iRecordNum = 40;

        public GameObject m_gRecordItemPrefab;
        public float m_fDistance = 59.0f;

        public Vector3 m_vFistPos;

        public GameObject m_gRecordParent;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
#if UNITY_STANDALONE_WIN
            m_vFistPos = new Vector3(440.0f, 0, 0);
#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
            m_vFistPos = new Vector3(390.0f, 0, 0);
#endif
        }

        void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="_AnimalIndex"></param>
        public void AddRecord(GamePrizeRecord _gameRecord)
        {
            GameObject temp_obj;
            if (m_lRecordList.Count >= 20)
            {
                temp_obj = m_lRecordList[0].gameObject;
                m_lRecordList.Remove(m_lRecordList[0]);
                //  temp_obj.transform.name = "RecordItem_" + m_lRecordList.Count.ToString();
            }
            else
            {
                temp_obj = (GameObject)Instantiate(m_gRecordItemPrefab, new Vector3(0, 0, 0), m_gRecordItemPrefab.transform.localRotation);
                temp_obj.transform.parent = m_gRecordParent.transform;
                temp_obj.transform.localScale = new Vector3(1, 1, 1);
                temp_obj.transform.name = "RecordItem_" + m_lRecordList.Count.ToString();
            }

            // 配置记录信息
            RecordItem temp_RecordItem = temp_obj.GetComponent<RecordItem>();

            if (_gameRecord.gameType == 0)
            {
                //temp_RecordItem.m_BackSprite.spriteName =;
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(true);
                temp_RecordItem.m_AnimalSprite.spriteName = "animal_" + _gameRecord.animalIndex[0] + "_" + _gameRecord.colorIndex[0].ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(false);
                temp_RecordItem.m_Animal2.gameObject.SetActive(false);
                //temp_RecordItem.m_BackCurrentpriteMask.spriteName = "record_Line_B";
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(false);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else if (_gameRecord.gameType == 1)
            {
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(true);
                temp_RecordItem.m_AnimalSprite.spriteName = "singleColor_" + _gameRecord.colorIndex[0].ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(false);
                temp_RecordItem.m_Animal2.gameObject.SetActive(false);
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(false);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else if (_gameRecord.gameType == 2)
            {
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(true);
                temp_RecordItem.m_AnimalSprite.spriteName = "singleAnimal_" + _gameRecord.animalIndex[0].ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(false);
                temp_RecordItem.m_Animal2.gameObject.SetActive(false);
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(false);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else if (_gameRecord.gameType == 3)
            {
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(true);
                temp_RecordItem.m_AnimalSprite.spriteName = "animal_" + _gameRecord.animalIndex[0] + "_" + _gameRecord.colorIndex[0].ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(false);
                temp_RecordItem.m_Animal2.gameObject.SetActive(false);
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(true);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else if (_gameRecord.gameType == 4)
            {
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(false);
                //temp_RecordItem.m_AnimalSprite.spriteName = "animal_" + _gameRecord.animalIndex + "_" + _gameRecord.colorIndex.ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(true);
                temp_RecordItem.m_Animal2.gameObject.SetActive(true);
                temp_RecordItem.m_Animal1.spriteName = "animal_" + _gameRecord.animalIndex[0] + "_" + _gameRecord.colorIndex[0].ToString();
                temp_RecordItem.m_Animal2.spriteName = "animal_" + _gameRecord.animalIndex[1] + "_" + _gameRecord.colorIndex[1].ToString();
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(false);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else if (_gameRecord.gameType == 5)
            {
                temp_RecordItem.m_AnimalSprite.gameObject.SetActive(true);
                temp_RecordItem.m_AnimalSprite.spriteName = "animal_" + _gameRecord.animalIndex + "_" + _gameRecord.colorIndex.ToString();
                temp_RecordItem.m_EnjoyGameTypeSprite.spriteName = "recordBanker_" + _gameRecord.enjoyGameIndex.ToString();
                temp_RecordItem.m_Animal1.gameObject.SetActive(false);
                temp_RecordItem.m_Animal2.gameObject.SetActive(false);
                //temp_RecordItem.m_BounsLogo.gameObject.SetActive(false);
                temp_RecordItem.m_BackCurrentpriteMask.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("游戏记录:未知开奖模式");
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
                m_lRecordList[i].transform.localPosition = new Vector3(m_vFistPos.x - (float)(m_lRecordList.Count - i - 1) * m_fDistance, 0, 0);
            }
            m_lRecordList[m_lRecordList.Count - 1].m_BackCurrentpriteMask.gameObject.SetActive(true);

        }
        /// <summary>
        /// 左移按钮
        /// </summary>
        public void Left_Onclick()
        {
            Vector3 tempos = m_gRecordParent.transform.localPosition;
            tempos.x += m_fDistance;

            m_gRecordParent.GetComponent<TweenPosition>().from = m_gRecordParent.transform.localPosition;
            m_gRecordParent.GetComponent<TweenPosition>().to = tempos;
            m_gRecordParent.GetComponent<TweenPosition>().ResetToBeginning();
            m_gRecordParent.GetComponent<TweenPosition>().enabled = true;
#if UNITY_STANDALONE_WIN
            if (tempos.x >= 295.0f)
            {
                SpringPosition sp = SpringPosition.Begin(m_gRecordParent.gameObject, m_gRecordParent.transform.localPosition + new Vector3(295.0f - m_gRecordParent.transform.localPosition.x, 0, 0), 13f);
                sp.enabled = true;
                sp.worldSpace = false;
            }
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
            if (tempos.x >= 395.0f)
            {
                SpringPosition sp = SpringPosition.Begin(m_gRecordParent.gameObject, m_gRecordParent.transform.localPosition + new Vector3(395.0f - m_gRecordParent.transform.localPosition.x, 0, 0), 13f);
                sp.enabled = true;
                sp.worldSpace = false;
            }
#endif
        }

        /// <summary>
        /// 右移按钮
        /// </summary>
        public void Right_Onclick()
        {
            Vector3 tempos = m_gRecordParent.transform.localPosition;
            tempos.x -= m_fDistance;
            m_gRecordParent.GetComponent<TweenPosition>().from = m_gRecordParent.transform.localPosition;
            m_gRecordParent.GetComponent<TweenPosition>().to = tempos;
            m_gRecordParent.GetComponent<TweenPosition>().ResetToBeginning();
            m_gRecordParent.GetComponent<TweenPosition>().enabled = true;

            if (tempos.x <= -60.0f)
            {
                SpringPosition sp = SpringPosition.Begin(m_gRecordParent.gameObject, m_gRecordParent.transform.localPosition + new Vector3(-60.0f - m_gRecordParent.transform.localPosition.x, 0, 0), 13f);
                sp.enabled = true;
                sp.worldSpace = false;
            }
        }

    }
}
