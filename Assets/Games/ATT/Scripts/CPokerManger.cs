using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.ATT
{
    public class CPokerManger : MonoBehaviour
    {
        /// <summary>
        /// 扑克列表
        /// </summary>
        public List<CPoker> m_lCPokerList = new List<CPoker>();
        /// <summary>
        /// 开牌时间
        /// </summary>
        public float m_fSingleOpenTime = 0.2f;
        /// <summary>
        /// 当前开牌下表
        /// </summary>
        public int m_iCurrentOpenIndex = 0;
        /// <summary>
        /// 定时器
        /// </summary>
        private float m_fWorkTime = 0;
        /// <summary>
        /// 是否是开拍时间
        /// </summary>
        private bool m_bIsOpenCardTime = false;

        private bool m_bPlayOpenMusic = true;

        int sss = 0;

        public static CPokerManger _instance;

        public bool m_bIsShow = false;
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
#if !UNITY_STANDALONE_WIN
            HideWindow();
#endif
        }

        public void SecondSendCard()
        {

            for (int i = 0; i < 5; i++)
            {
                if (!m_lCPokerList[i].m_bIsChecked)
                {

                   // SetCard(i, 2);
                }
            }
            OpenCard();
        }
        // Update is called once per frame
        void Update()
        {
            OpenCardControl();
        }


        /// <summary>
        /// 设置牌
        /// </summary>
        /// <param name="_iIndex"></param>
        public void SetCard(int _iIndex,int _color ,int _value)
        {

            if (_iIndex < m_lCPokerList.Count)
            {
                m_lCPokerList[_iIndex].m_gCardButton.gameObject.SetActive(true);
                m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UISprite>().spriteName = "Card_" + _color.ToString() + "_" + _value.ToString();
                m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UIButton>().normalSprite = m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UISprite>().spriteName;
                m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UIButton>().hoverSprite = m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UISprite>().spriteName;
                m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UIButton>().pressedSprite = m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UISprite>().spriteName;
                m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UIButton>().disabledSprite = m_lCPokerList[_iIndex].m_gCardButton.GetComponent<UISprite>().spriteName;
                m_lCPokerList[_iIndex].m_iIndex = _iIndex;
                m_lCPokerList[_iIndex].Reset();

            }
           
        }

        public void  SetCheckCard(int _iIndex, bool _bIschecked)
        {
            UIManger.Instance.m_cUpdateCard.cCard.bBarter[_iIndex] = _bIschecked;
        }
        /// <summary>
        /// 启动开牌
        /// </summary>
        public void OpenCard()
        {
            m_fWorkTime = 0;
            m_bIsOpenCardTime = true;
            m_iCurrentOpenIndex = 0;
            CMusicManger._instance.PlaySound("Sendcard");
            SetCurrentIndex(m_iCurrentOpenIndex);
            m_bPlayOpenMusic = true; 

        }
        /// <summary>
        /// 控制开牌动画时间
        /// </summary>
        private void OpenCardControl()
        {
            m_fWorkTime += Time.deltaTime;
            if (m_fWorkTime >= (m_fSingleOpenTime - 0.1f) && m_bIsOpenCardTime && m_bPlayOpenMusic)
            {
                 CMusicManger._instance.PlaySound("Music" + (m_iCurrentOpenIndex + 1).ToString());
                 m_bPlayOpenMusic = false;
            }
            if (m_fWorkTime >= m_fSingleOpenTime && m_bIsOpenCardTime)
            {
                m_fWorkTime = 0;
                m_bPlayOpenMusic = true;
                m_lCPokerList[m_iCurrentOpenIndex].m_bIsOpenCard = true;
        
                if (m_iCurrentOpenIndex + 1 >= m_lCPokerList.Count) m_bIsOpenCardTime = false;
                SetCurrentIndex(m_iCurrentOpenIndex + 1);

            }
        }
        /// <summary>
        /// 设置当前开牌下标
        /// </summary>
        /// <param name="_iIndex">下标</param>
        private void SetCurrentIndex(int _iIndex)
        { 
            
            if (_iIndex >= m_lCPokerList.Count)
            {
                for (int i = 0; i < m_lCPokerList.Count; i++)
                {
                    m_lCPokerList[i].m_bIsChecked = !UIManger.Instance.m_cUpdateCard.cCard.bBarter[i];
                }
                    return;
            }
            m_iCurrentOpenIndex = _iIndex;
            if (m_lCPokerList[m_iCurrentOpenIndex].m_bIsChecked)
            {
                SetCurrentIndex(m_iCurrentOpenIndex + 1);
            }

            if(m_iCurrentOpenIndex>=4)
            {
                for (int i = 0; i < m_lCPokerList.Count; i++)
                {
                    m_lCPokerList[i].m_bIsOpenCardOver = true;
                }
            }
           
            
        }

        public void SetPockerBTDisable(bool _bIsDisable)
        {
            for (int i = 0; i < m_lCPokerList.Count; i++)
            {
                m_lCPokerList[i].SetCardBTDisable(_bIsDisable);
            }
        }
        /// <summary>
        /// 重置扑克牌
        /// </summary>
        public void RectCard()
        {
            for (int i = 0; i < m_lCPokerList.Count; i++)
            {
                m_lCPokerList[i].Reset();
                m_lCPokerList[i].m_bIsOpenCardOver = false;
                
            }
        }

        /// <summary>
        /// 显示比倍窗口
        /// </summary>
        public void ShowWindow()
        {
            m_bIsShow = true;
//             this.GetComponent<TweenPosition>().from = this.transform.localPosition;
//             this.GetComponent<TweenPosition>().to = new Vector3(0, 0, 0);
//             this.GetComponent<TweenPosition>().enabled = true;

            this.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏比倍窗口
        /// </summary>
        public void HideWindow()
        {
            m_bIsShow = false;
//             this.GetComponent<TweenPosition>().from = this.transform.localPosition;
//             this.GetComponent<TweenPosition>().to = new Vector3(2000, 0, 0);
//             this.GetComponent<TweenPosition>().enabled = true;

            this.gameObject.SetActive(false);
        }
    }
}