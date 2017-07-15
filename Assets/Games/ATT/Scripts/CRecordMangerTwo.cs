using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CRecordMangerTwo : MonoBehaviour
    {

        public static CRecordMangerTwo _instance;
        //高倍
        public GameObject m_gLabel_5K_H;
        public GameObject m_gLabel_RS_H;
        public GameObject m_gLabel_SF_H;
        public GameObject m_gLabel_4K_H;

        //低倍
        public GameObject m_gLabel_5K_L;
        public GameObject m_gLabel_RS_L;
        public GameObject m_gLabel_SF_L;
        public GameObject m_gLabel_4K_L;

        //总数
        public GameObject m_gLabel_5K_T;
        public GameObject m_gLabel_RS_T;
        public GameObject m_gLabel_SF_T;
        public GameObject m_gLabel_4K_T;
        public GameObject m_gLabel_FH_T;
        public GameObject m_gLabel_FL_T;
        public GameObject m_gLabel_ST_T;
        public GameObject m_gLabel_3K_T;
        public GameObject m_gLabel_2P_T;
        public GameObject m_gLabel_1P_T;
        public GameObject m_gLabel_NP_T;
        public GameObject m_gLabel_All_T;
        /// <summary>
        /// 是否显示了窗口
        /// </summary>
        public bool m_bIsShow = false;

        //高倍
        private int _str5K_H;
        public int m_str5K_H
        {
            get { return _str5K_H; }
            set
            {
                _str5K_H = value;
                m_gLabel_5K_H.GetComponent<CLabelNum>().m_iNum = _str5K_H;
            }
        }

        private int _strRS_H;
        public int m_strRS_H
        {
            get { return _strRS_H; }
            set
            {
                _strRS_H = value;
                m_gLabel_RS_H.GetComponent<CLabelNum>().m_iNum = _strRS_H;
            }
        }

        private int _strSF_H;
        public int m_strSF_H
        {
            get { return _strSF_H; }
            set
            {
                _strSF_H = value;
                m_gLabel_SF_H.GetComponent<CLabelNum>().m_iNum = _strSF_H;
            }
        }

        private int _str4K_H;
        public int m_str4K_H
        {
            get { return _str4K_H; }
            set
            {
                _str4K_H = value;
                m_gLabel_4K_H.GetComponent<CLabelNum>().m_iNum = _str4K_H;
            }
        }

        //低倍
        private int _str5K_L;
        public int m_str5K_L
        {
            get { return _str5K_L; }
            set
            {
                _str5K_L = value;
                m_gLabel_5K_L.GetComponent<CLabelNum>().m_iNum = _str5K_L;
            }
        }

        private int _strRS_L;
        public int m_strRS_L
        {
            get { return _strRS_L; }
            set
            {
                _strRS_L = value;
                m_gLabel_RS_L.GetComponent<CLabelNum>().m_iNum = _strRS_L;
            }
        }

        private int _strSF_L;
        public int m_strSF_L
        {
            get { return _strSF_L; }
            set
            {
                _strSF_L = value;
                m_gLabel_SF_L.GetComponent<CLabelNum>().m_iNum = _strSF_L;
            }
        }

        private int _str4K_L;
        public int m_str4K_L
        {
            get { return _str4K_L; }
            set
            {
                _str4K_L = value;
                m_gLabel_4K_L.GetComponent<CLabelNum>().m_iNum = _str4K_L;
            }
        }

        //总数
        private int _str5K_T;
        public int m_str5K_T
        {
            get { return _str5K_T; }
            set
            {
                _str5K_T = value;
                m_gLabel_5K_T.GetComponent<CLabelNum>().m_iNum = _str5K_T;
            }
        }

        private int _strRS_T;
        public int m_strRS_T
        {
            get { return _strRS_T; }
            set
            {
                _strRS_T = value;
                m_gLabel_RS_T.GetComponent<CLabelNum>().m_iNum = _strRS_T;
            }
        }

        private int _strSF_T;
        public int m_strSF_T
        {
            get { return _strSF_T; }
            set
            {
                _strSF_T = value;
                m_gLabel_SF_T.GetComponent<CLabelNum>().m_iNum = _strSF_T;
            }
        }

        private int _str4K_T;
        public int m_str4K_T
        {
            get { return _str4K_T; }
            set
            {
                _str4K_T = value;
                m_gLabel_4K_T.GetComponent<CLabelNum>().m_iNum = _str4K_T;
            }
        }

        private int _strFH_T;
        public int m_strFH_T
        {
            get { return _strFH_T; }
            set
            {
                _strFH_T = value;
                m_gLabel_FH_T.GetComponent<CLabelNum>().m_iNum = _strFH_T;
            }
        }

        private int _strFL_T;
        public int m_strFL_T
        {
            get { return _strFL_T; }
            set
            {
                _strFL_T = value;
                m_gLabel_FL_T.GetComponent<CLabelNum>().m_iNum = _strFL_T;
            }
        }

        private int _strST_T;
        public int m_strST_T
        {
            get { return _strST_T; }
            set
            {
                _strST_T = value;
                m_gLabel_ST_T.GetComponent<CLabelNum>().m_iNum = _strST_T;
            }
        }

        private int _str3K_T;
        public int m_str3K_T
        {
            get { return _str3K_T; }
            set
            {
                _str3K_T = value;
                m_gLabel_3K_T.GetComponent<CLabelNum>().m_iNum = _str3K_T;
            }
        }

        private int _str2P_T;
        public int m_str2P_T
        {
            get { return _str2P_T; }
            set
            {
                _str2P_T = value;
                m_gLabel_2P_T.GetComponent<CLabelNum>().m_iNum = _str2P_T;
            }
        }

        private int _str1P_T;
        public int m_str1P_T
        {
            get { return _str1P_T; }
            set
            {
                _str1P_T = value;
                m_gLabel_1P_T.GetComponent<CLabelNum>().m_iNum = _str1P_T;
            }
        }

        private int _strNP_T;
        public int m_strNP_T
        {
            get { return _strNP_T; }
            set
            {
                _strNP_T = value;
                m_gLabel_NP_T.GetComponent<CLabelNum>().m_iNum = _strNP_T;
            }
        }

        private int _strAll_T;
        public int m_strAll_T
        {
            get { return _strAll_T; }
            set
            {
                _strAll_T = value;
                m_gLabel_All_T.GetComponent<CLabelNum>().m_iNum = _strAll_T;
            }
        }

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
    }
}