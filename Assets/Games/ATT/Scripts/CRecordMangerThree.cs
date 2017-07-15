using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CRecordMangerThree : MonoBehaviour
    {


        public static CRecordMangerThree _instance;

        //主游戏
        public GameObject m_gLabel_M_Play_S;
        public GameObject m_gLabel_M_Play_WS;
        public GameObject m_gLabel_M_Play_N;
        public GameObject m_gLabel_M_Play_WN;

        //比倍
        public GameObject m_gLabel_C_Play_S;
        public GameObject m_gLabel_C_Play_WS;
        public GameObject m_gLabel_C_Play_LN;
        public GameObject m_gLabel_C_Play_WN;

        public GameObject m_gLabel_Open_S;
        public GameObject m_gLabel_Xi_S;
        public GameObject m_gLabel_Tou_S;
        public GameObject m_gLabel_Tui_S;
        public GameObject m_gLabel_Cai_S;
        public GameObject m_gLabel_Cai_WN;


        /// <summary>
        /// 是否显示了窗口
        /// </summary>
        public bool m_bIsShow = false;

        //主游戏
        private int _strM_Play_S;
        public int m_strM_Play_S
        {
            get { return _strM_Play_S; }
            set
            {
                _strM_Play_S = value;
                m_gLabel_M_Play_S.GetComponent<CLabelNum>().m_iNum = _strM_Play_S;
            }
        }

        private int _strM_Play_WS;
        public int m_strM_Play_WS
        {
            get { return _strM_Play_WS; }
            set
            {
                _strM_Play_WS = value;
                m_gLabel_M_Play_WS.GetComponent<CLabelNum>().m_iNum = _strM_Play_WS;
            }
        }

        private int _strM_Play_N;
        public int m_strM_Play_N
        {
            get { return _strM_Play_N; }
            set
            {
                _strM_Play_N = value;
                m_gLabel_M_Play_N.GetComponent<CLabelNum>().m_iNum = _strM_Play_N;
            }
        }

        private int _strM_Play_WN;
        public int m_strM_Play_WN
        {
            get { return _strM_Play_WN; }
            set
            {
                _strM_Play_WN = value;
                m_gLabel_M_Play_WN.GetComponent<CLabelNum>().m_iNum = _strM_Play_WN;
            }
        }

        //比倍
        private int _strC_Play_S;
        public int m_strC_Play_S
        {
            get { return _strC_Play_S; }
            set
            {
                _strC_Play_S = value;
                m_gLabel_C_Play_S.GetComponent<CLabelNum>().m_iNum = _strC_Play_S;
            }
        }

        private int _strC_Play_WS;
        public int m_strC_Play_WS
        {
            get { return _strC_Play_WS; }
            set
            {
                _strC_Play_WS = value;
                m_gLabel_C_Play_WS.GetComponent<CLabelNum>().m_iNum = _strC_Play_WS;
            }
        }

        private int _strC_Play_LN;
        public int m_strC_Play_LN
        {
            get { return _strC_Play_LN; }
            set
            {
                _strC_Play_LN = value;
                m_gLabel_C_Play_LN.GetComponent<CLabelNum>().m_iNum = _strC_Play_LN;
            }
        }

        private int _strC_Play_WN;
        public int m_strC_Play_WN
        {
            get { return _strC_Play_WN; }
            set
            {
                _strC_Play_WN = value;
                m_gLabel_C_Play_WN.GetComponent<CLabelNum>().m_iNum = _strC_Play_WN;
            }
        }

        private int _strC_Open_S;
        public int M_strC_Open_S
        {
            get { return _strC_Open_S; }
            set
            {
                _strC_Open_S = value;
                m_gLabel_Open_S.GetComponent<CLabelNum>().m_iNum = _strC_Open_S;
            }
        }

        private int _strC_Xi_S;
        public int M_strC_Xi_S
        {
            get { return _strC_Xi_S; }
            set
            {
                _strC_Xi_S = value;
                m_gLabel_Xi_S.GetComponent<CLabelNum>().m_iNum = _strC_Xi_S;
            }
        }


        private int _strC_Tou_S;
        public int M_strC_Tou_S
        {
            get { return _strC_Tou_S; }
            set
            {
                _strC_Tou_S = value;
                m_gLabel_Tou_S.GetComponent<CLabelNum>().m_iNum = _strC_Tou_S;
            }
        }

        private int _strC_Tui_S;
        public int M_strC_Tui_S
        {
            get { return _strC_Tui_S; }
            set
            {
                _strC_Tui_S = value;
                m_gLabel_Tui_S.GetComponent<CLabelNum>().m_iNum = _strC_Tui_S;
            }
        }

        private int _strC_Cai_S;
        public int M_strC_Cai_S
        {
            get { return _strC_Cai_S; }
            set
            {
                _strC_Cai_S = value;
                m_gLabel_Cai_S.GetComponent<CLabelNum>().m_iNum = _strC_Cai_S;
            }
        }

        private int _strC_Cai_WN;
        public int M_strC_Cai_WN
        {
            get { return _strC_Cai_WN; }
            set
            {
                _strC_Cai_WN = value;
                m_gLabel_Cai_WN.GetComponent<CLabelNum>().m_iNum = _strC_Cai_WN;
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