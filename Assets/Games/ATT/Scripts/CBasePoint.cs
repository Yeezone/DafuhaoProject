using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CBasePoint : MonoBehaviour
    {
        public static CBasePoint _instance;
        public GameObject m_gLabel5K;
        public GameObject m_gLabelRS;
        public GameObject m_gLabelSF;
        public GameObject m_gLabel4K;
        public GameObject m_gLabelComputerNum;

        public bool m_bIsOpen = false;
        public int m_iCardType = 0;
        public float m_fPerNumTime = 0.002f;
        public int m_iSpeed = 1;
        public float m_fWorkTime = 0;

        private int _str5K;
        public int m_str5K
        {
            get { return _str5K; }
            set 
            { 
                _str5K = value;
                m_gLabel5K.GetComponent<CLabelNum>().m_iNum = _str5K;
            }
        }
        private int _strRS;
        public int m_strRS
        {
            get { return _strRS; }
            set
            {
                _strRS = value;
                m_gLabelRS.GetComponent<CLabelNum>().m_iNum = _strRS;
            }
        }
        private int _strSF;
        public int m_strSF
        {
            get { return _strSF; }
            set
            {
                _strSF = value;
                m_gLabelSF.GetComponent<CLabelNum>().m_iNum = _strSF;
            }
        }
        private int _str4K;
        public int m_str4K
        {
            get { return _str4K; }
            set
            {
                _str4K = value;
                m_gLabel4K.GetComponent<CLabelNum>().m_iNum = _str4K;
            }
        }

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

        }

        // Update is called once per frame
        void Update()
        {
            m_fWorkTime += Time.deltaTime; 
            if(m_bIsOpen && m_fPerNumTime<= m_fWorkTime)
            {
                m_fWorkTime = 0;
                GetBasePointAnimation();
            }

        }

        /// <summary>
        /// 显示比倍窗口
        /// </summary>
        public void ShowWindow()
        {
            m_bIsShow = true;
            this.GetComponent<TweenPosition>().from = this.transform.localPosition;
            this.GetComponent<TweenPosition>().to = new Vector3(0, 0, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }
        /// <summary>
        /// 隐藏比倍窗口
        /// </summary>
        public void HideWindow()
        {
            m_bIsShow = false;
            this.GetComponent<TweenPosition>().from = this.transform.localPosition;
            this.GetComponent<TweenPosition>().to = new Vector3(2000, 0, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }

        public void SetBasePointAnimation(bool _bIsOpen, int _iCardType, int _iSpeed)
        {
            m_iSpeed = _iSpeed;
            m_iCardType = _iCardType;
            m_bIsOpen = _bIsOpen;
        }

        public void GetBasePointAnimation()
        {
            switch (m_iCardType)
            {
                case 7:
                    {
                        if (m_str4K <= m_iSpeed) m_iSpeed = m_str4K;
                         m_str4K -= m_iSpeed;
                         if (m_str4K <= 0)
                         {
                             m_bIsOpen = false;
                             m_str4K = 0;

                         }
                         CPlayerInfo._instance.m_iCreditNum += m_iSpeed;
                         CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_iSpeed;
                        break;
                    }
                case 8:
                    {
                        if (m_strSF <= m_iSpeed) m_iSpeed = m_strSF;
                        m_strSF -= m_iSpeed;
                        if (m_strSF <= 0)
                        {
                            m_bIsOpen = false;
                            m_strSF = 0;
                        }
                        CPlayerInfo._instance.m_iCreditNum += m_iSpeed;
                        CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_iSpeed;
                        break;
                    }
                case 9:
                    {
                        if (m_strRS <= m_iSpeed) m_iSpeed = m_strRS;
                        m_strRS -= m_iSpeed;
                        if (m_strRS <= 0)
                        {
                            m_bIsOpen = false;
                            m_strRS = 0;
                        }
                        CPlayerInfo._instance.m_iCreditNum += m_iSpeed;
                        CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_iSpeed;
                        break;
                    }
                case 10:
                    {
                        if (m_str5K <= m_iSpeed) m_iSpeed = m_str5K;
                        m_str5K -= m_iSpeed;
                        if (m_str5K <= 0)
                        {
                            m_bIsOpen = false;
                            m_str5K = 0;
                        }
                        CPlayerInfo._instance.m_iCreditNum += m_iSpeed;
                        CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_iSpeed;
                        break;
                    }
            }
       
        }

        public void SetBasePoint()
        {
            m_str5K = UIManger.Instance.m_tReadFile.n5K;
            m_strRS = UIManger.Instance.m_tReadFile.nRS;
            m_strSF = UIManger.Instance.m_tReadFile.nSF;
            m_str4K = UIManger.Instance.m_tReadFile.n4K;
        }
    }
}