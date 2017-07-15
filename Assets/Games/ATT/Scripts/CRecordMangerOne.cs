using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CRecordMangerOne : MonoBehaviour
    {

        public static CRecordMangerOne _instance;
        //盈亏
        public GameObject m_gLabel_A_In;
        public GameObject m_gLabel_B_In;
        public GameObject m_gLabel_A_Out;
        public GameObject m_gLabel_B_Out;
        public GameObject m_gLabel_A_Balance;
        public GameObject m_gLabel_B_Balance;

        //高倍
        public GameObject m_gLabel_5K_H;
        public GameObject m_gLabel_RS_H;
        public GameObject m_gLabel_SF_H;
        public GameObject m_gLabel_4K_H;
        //中倍
        public GameObject m_gLabel_5K_M;
        public GameObject m_gLabel_RS_M;
        public GameObject m_gLabel_SF_M;
        public GameObject m_gLabel_4K_M;
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
        /// <summary>
        /// 是否显示了窗口
        /// </summary>
        public bool m_bIsShow = false;

        //盈亏
        private int _strA_In;
        public int m_strA_In
        {
            get { return _strA_In; }
            set
            {
                _strA_In = value;
                m_gLabel_A_In.GetComponent<CLabelNum>().m_iNum = _strA_In;
            }
        }

        private int _strB_In;
        public int m_strB_In
        {
            get { return _strB_In; }
            set
            {
                _strB_In = value;
                m_gLabel_B_In.GetComponent<CLabelNum>().m_iNum = _strB_In;
            }
        }

        private int _strA_Out;
        public int m_strA_Out
        {
            get { return _strA_Out; }
            set
            {
                _strA_Out = value;
                m_gLabel_A_Out.GetComponent<CLabelNum>().m_iNum = _strA_Out;
            }
        }

        private int _strB_Out;
        public int m_strB_Out
        {
            get { return _strB_Out; }
            set
            {
                _strB_Out = value;
                m_gLabel_B_Out.GetComponent<CLabelNum>().m_iNum = _strB_Out;
            }
        }

        private int _strA_Balance;
        public int m_strA_Balance
        {
            get { return _strA_Balance; }
            set
            {
                _strA_Balance = value;
                m_gLabel_A_Balance.GetComponent<CLabelNum>().m_iNum = _strA_Balance;
            }
        }

        private int _strB_Balance;
        public int m_strB_Balance
        {
            get { return _strB_Balance; }
            set
            {
                _strB_Balance = value;
                m_gLabel_B_Balance.GetComponent<CLabelNum>().m_iNum = _strB_Balance;
            }
        }


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

        //中倍
        private int _str5K_M;
        public int m_str5K_M
        {
            get { return _str5K_M; }
            set
            {
                _str5K_M = value;
                m_gLabel_5K_M.GetComponent<CLabelNum>().m_iNum = _str5K_M;
            }
        }

        private int _strRS_M;
        public int m_strRS_M
        {
            get { return _strRS_M; }
            set
            {
                _strRS_M = value;
                m_gLabel_RS_M.GetComponent<CLabelNum>().m_iNum = _strRS_M;
            }
        }

        private int _strSF_M;
        public int m_strSF_M
        {
            get { return _strSF_M; }
            set
            {
                _strSF_M = value;
                m_gLabel_SF_M.GetComponent<CLabelNum>().m_iNum = _strSF_M;
            }
        }

        private int _str4K_M;
        public int m_str4K_M
        {
            get { return _str4K_M; }
            set
            {
                _str4K_M = value;
                m_gLabel_4K_M.GetComponent<CLabelNum>().m_iNum = _str4K_M;
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
            this.GetComponent<TweenPosition>().to = new Vector3(-2000, -546, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }
    }
}