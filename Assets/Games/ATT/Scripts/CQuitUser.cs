using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CQuitUser : MonoBehaviour
    {

        public GameObject m_gConfirBT;
        public bool m_bIsShow = false;
        public static CQuitUser _instance;
        public UILabel m_lTiShiLabel;
        private float m_fTime = 0;
        void Awake()
        {
            _instance = this;
            HideWindow();
           
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
            m_fTime -= Time.deltaTime;
            if (m_fTime <= 0)
                m_fTime = 0;
            SetGameTime(m_fTime);
        }
        public void Confir_Onclick()
        {
            UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
            HideWindow();
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

        public void SetGameTime(float gametime)
        {
            m_fTime = gametime;
            m_lTiShiLabel.text = gametime.ToString("F0");
        }
    }
}
