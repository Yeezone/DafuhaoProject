using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CNomaney : MonoBehaviour
    {

        public GameObject m_gConfirBT;
        public bool m_bIsShow = false;
        public static CNomaney _instance;
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

        }
        public void Confir_Onclick()
        {

            HideWindow();
            GameEngine.Instance.Quit();
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