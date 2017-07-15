using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class CButtonLongPress : MonoBehaviour
    {

        public static CButtonLongPress _instance;
        // 第一次点击状态
        public bool m_bIsfirstDownBeing = false;
        // 是否按住状态
        public bool m_bIsDownBeing = false;
        // 每次响应的间隔时间
        public float m_fSpeed = 0.2f;
        // 计时
        private float m_fWorkingTime = 0;
        // 事件回调
        public delegate void DownBeingOnChangge();
        public DownBeingOnChangge m_OnChange = null;
        [SerializeField]
        UnityEvent m_OnLongpress = new UnityEvent();

        void Start()
        {
            _instance = this;
        }

        void Update()
        {
            m_fWorkingTime += Time.deltaTime;
            if (m_fWorkingTime >= m_fSpeed && m_bIsfirstDownBeing && m_bIsDownBeing)
            {
                //if (m_OnChange != null) m_OnChange();
                // 触发点击
                m_OnLongpress.Invoke();
                m_fWorkingTime = 0;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        // 按下按钮
        public void SetDownBeingTrue()
        {
            m_bIsfirstDownBeing = true;
            StartCoroutine(WaitaSetDownBeing(0.2f));
        }

        // 弹起按钮
        public void SetDownBeingFalse()
        {
            m_bIsfirstDownBeing = false;
            m_bIsDownBeing = false;
        }

        // 单击,不立即响应按住
        IEnumerator WaitaSetDownBeing(float _fWaitTime)
        {
            yield return new WaitForSeconds(_fWaitTime);
            if (m_bIsfirstDownBeing)
            {
                m_bIsDownBeing = true;
            }
        }
    }
}
