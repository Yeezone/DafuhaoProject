using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.ATT
{
    public class CPoker : MonoBehaviour
    {

        /// <summary>
        /// 牌
        /// </summary>
        public GameObject m_gCardButton;
        /// <summary>
        /// 蓝色标识
        /// </summary>
        public GameObject m_gHeldBlue;
        /// <summary>
        /// 红色标识
        /// </summary>
        public GameObject m_gHeldRed;
        /// <summary>
        /// 牌背
        /// </summary>
        public GameObject m_gCardBg;
        /// <summary>
        /// Held背景
        /// </summary>
        public GameObject m_gHeldBg;

        /// <summary>
        /// 开完牌
        /// </summary>
        public bool _bIsOpenCardOver = false;
        public bool m_bIsOpenCardOver
        {
            get { return _bIsOpenCardOver; }
            set
            { 
                _bIsOpenCardOver = value;
                m_gHeldRed.SetActive(_bIsOpenCardOver);
                m_gHeldBlue.SetActive(_bIsOpenCardOver);
                m_gHeldBg.SetActive(_bIsOpenCardOver);
                if (_bIsOpenCardOver)
                {
                    m_gHeldBlue.SetActive(!_bIsChecked);
                    m_gHeldRed.SetActive(_bIsChecked);
                }
                
            }
        }


        /// <summary>
        /// 是否开牌
        /// </summary>
        private bool _bIsOpenCard = false;

        public int m_iIndex;
        /// <summary>
        /// 牌值
        /// </summary>
        public int m_iCardValue;
        public bool m_bIsOpenCard
        {
            get { return _bIsOpenCard; }
            set
            {
                _bIsOpenCard = value;

                m_gCardBg.SetActive(!_bIsOpenCard);
                m_gCardButton.SetActive(_bIsOpenCard);
                
            }
        }
        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool _bIsChecked = false;
        public bool m_bIsChecked
        {
            get { return _bIsChecked; }
            set
            {
                _bIsChecked = value;
                if (_bIsOpenCardOver)
                {
                    m_gHeldBlue.SetActive(!_bIsChecked);
                    m_gHeldRed.SetActive(_bIsChecked);
                }
                
            }
        }

        // Use this for initialization
        void Start()
        {
            m_bIsOpenCardOver = false;
            Reset();


        }

        // Update is called once per frame
        void Update()
        {
           
        }
        /// <summary>
        /// 点击选中扑克
        /// </summary>
        public void CardBT_OnClick()
        {
            m_bIsChecked = !m_bIsChecked;

            if (m_bIsChecked && CMusicManger._instance.m_bIsOpen) m_gCardButton.GetComponent<UIPlaySound>().Play();

            //设置清楚按钮
            int temp = 0;
            for (int i = 0; i < CPokerManger._instance.m_lCPokerList.Count; i++)
            {
                if (CPokerManger._instance.m_lCPokerList[i]._bIsChecked)
                {
                    temp += 1;
                }
            }
            if (temp == 0)
            {
                CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gClearBT, true);
            }
            else
            {
                CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gClearBT, false);
            }
            CPokerManger._instance.SetCheckCard(m_iIndex, !m_bIsChecked);
        }
        /// <summary>
        /// 重置poker
        /// </summary>
        public void Reset()
      {
            
            m_bIsChecked = false;
            m_bIsOpenCard = false;
        }
        /// <summary>
        /// 设置牌是否响应
        /// </summary>
        /// <param name="_bIsDisble"></param>
        public void SetCardBTDisable(bool _bIsDisble)
        {
            m_gCardButton.GetComponent<BoxCollider>().enabled = _bIsDisble;
            m_gHeldBg.GetComponent<BoxCollider>().enabled = _bIsDisble;
        }


    }
}