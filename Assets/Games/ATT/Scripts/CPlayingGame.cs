using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CPlayingGame : MonoBehaviour
    {
        public static CPlayingGame _instance;
        public bool m_bIsShow = false;
        void Awake()
        {
            _instance = this;
        }
        void Destroy()
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
