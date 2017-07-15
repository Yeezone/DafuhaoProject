using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.JSYS
{
    public class CHeadManger : MonoBehaviour
    {
        public static CHeadManger _instance;
        public List<CHeadItem> m_lHeadList = new List<CHeadItem>();

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
        public void ShowPlayerHead(int _iChairID, int _iFaceID,string _strNickName)
        {
            if (_iChairID < m_lHeadList.Count)
            {
                m_lHeadList[_iChairID].m_HeadPhotoSprite.gameObject.SetActive(true);
                m_lHeadList[_iChairID].m_HeadPhotoSprite.spriteName = "face_" + _iFaceID.ToString();
                m_lHeadList[_iChairID].m_LabelNickName.gameObject.SetActive(true);
                //截取名字过长只显示5个字
                if (_strNickName.Length > 5) _strNickName = _strNickName.Substring(0, 5) + "...";
                m_lHeadList[_iChairID].m_LabelNickName.text = _strNickName;
            }
        }
        /// <summary>
        /// 玩家退出隐藏头像
        /// </summary>
        /// <param name="_iChairID">退出玩家ID</param>
        public void PlayerExit(int _iChairID)
        {
            if (_iChairID < m_lHeadList.Count)
            {
                m_lHeadList[_iChairID].m_HeadPhotoSprite.gameObject.SetActive(false);
                m_lHeadList[_iChairID].m_LabelNickName.gameObject.SetActive(false);
            }
        }

        public void SetAllFalse()
        {
            for (int i = 0; i < m_lHeadList.Count; i++)
            {
                m_lHeadList[i].m_HeadPhotoSprite.gameObject.SetActive(false);
                m_lHeadList[i].m_LabelNickName.gameObject.SetActive(false);

               // if (i >= UIManger.Instance.m_iGamePlayer) m_lHeadList[i].GetComponent<CHeadItem>().gameObject.SetActive(false);
            }
        }
        public void SetHeadHide()
        {
            for (int i = 0; i < m_lHeadList.Count; i++)
            {
                m_lHeadList[i].gameObject.SetActive(false);
            }
        }
    }
}
