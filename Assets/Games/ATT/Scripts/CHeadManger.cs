using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.ATT
{
    public class CHeadManger : MonoBehaviour
    {

        public static CHeadManger _instance;
        public List<GameObject> m_lHeadList = new List<GameObject>();
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
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 设置新进来的用户头像
        /// </summary>
        /// <param name="_iChairID">玩家椅子ID</param>
        /// <param name="_iFaceID">玩家头像ID</param>
        public void ShowPlayerHead(int _iChairID, int _iFaceID)
        {
            m_lHeadList[_iChairID].GetComponent<CHeadItem>().m_gFace.SetActive(true);
            m_lHeadList[_iChairID].GetComponent<CHeadItem>().m_gFace.GetComponent<UISprite>().spriteName = "face_" + _iFaceID.ToString();
        }
        /// <summary>
        /// 玩家退出隐藏头像
        /// </summary>
        /// <param name="_iChairID">退出玩家ID</param>
        public void PlayerExit(int _iChairID)
        {
            if (_iChairID < m_lHeadList.Count)
            m_lHeadList[_iChairID].GetComponent<CHeadItem>().m_gFace.SetActive(false);
        }

        public void SetAllFalse()
    {
        for (int i = 0; i < m_lHeadList.Count; i++)
        {
            m_lHeadList[i].GetComponent<CHeadItem>().m_gFace.SetActive(false);
            
            if (i >= UIManger.Instance.m_iGamePlayer) m_lHeadList[i].GetComponent<CHeadItem>().gameObject.SetActive(false);
        }
    }
        public void SetHeadHide()
        {
            for (int i = 0; i < m_lHeadList.Count; i++)
            {
                m_lHeadList[i].GetComponent<CHeadItem>().gameObject.SetActive(false);
            }
        }
    }
}