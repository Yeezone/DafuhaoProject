using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.xyls
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

        /// <summary>
        /// 设置新进来的用户头像
        /// </summary>
        /// <param name="_iChairID">玩家椅子ID</param>
        /// <param name="_iFaceID">玩家头像ID</param>
        public void ShowPlayerHead(int _iChairID, int _iFaceID,string _strName)
        {
            var headItem = m_lHeadList[_iChairID].GetComponent<CHeadItem>();
            headItem.m_gFace.SetActive(true);
            headItem.m_gFace.GetComponent<UISprite>().spriteName = "face_" + _iFaceID.ToString();
            headItem.m_uPlayerName.text = _strName;
        }
        /// <summary>
        /// 玩家退出隐藏头像
        /// </summary>
        /// <param name="_iChairID">退出玩家ID</param>
        public void PlayerExit(int _iChairID)
        {
            var headItem = m_lHeadList[_iChairID].GetComponent<CHeadItem>();
            headItem.m_gFace.SetActive(false);
            headItem.m_uPlayerName.text = "";
        }

        public void SetAllFalse()
    {
        for (int i = 0; i < m_lHeadList.Count; i++)
        {
            m_lHeadList[i].GetComponent<CHeadItem>().m_gFace.SetActive(false);
            
           // if (i >= 6) m_lHeadList[i].GetComponent<CHeadItem>().gameObject.SetActive(false);
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