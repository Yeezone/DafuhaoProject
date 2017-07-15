using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    [System.Serializable]
    public class TimeCtr_Banker
    {
        // 加速阶段持续时间
        public float m_fSpeedUpTime = 3.0f;
        // 匀速阶段持续时间
        public float m_fSpeedKeepTime = 6.0f;
        // 减速阶段持续时间
        public float m_fSpeedDownTime = 10.0f;
    }

    public class ModelRot_Banker : MonoBehaviour
    {
        // 庄闲和面板
        public UITexture m_gBankerPanelTex;
        // 计时器
        //public float m_fTimer;
        // 缓存Rect
        private Rect temp_rect = new Rect(0,0,1,1);
        // 检测开关
        //public bool m_bIsOpen = false;
        //
        //private bool temp = false;
        // 目标信息
        //public int m_iTarget;
        // 时间控制
        //public TimeCtr_Banker m_tTimeCtr = new TimeCtr_Banker();

        // 庄闲和的滚轮差值
        private GameObject m_gBanker;
        public TweenPosition m_tpBanker;

        void Start()
        {
            m_gBanker = m_gBankerPanelTex.gameObject;
        }

        void Update()
        {
            temp_rect.y = m_gBanker.transform.localPosition.z * 0.1f;
            m_gBankerPanelTex.uvRect = temp_rect;

//             m_fTimer += Time.deltaTime;
// 
//             if (m_fTimer > 0 && m_fTimer < m_tTimeCtr.m_fSpeedUpTime && m_bIsOpen)
//             {
//                 // 加速过程
//                 temp_rect.y = -m_fTimer;
//             }
//             else if (m_fTimer >= m_tTimeCtr.m_fSpeedUpTime && m_fTimer < m_tTimeCtr.m_fSpeedKeepTime && m_bIsOpen)
//             {
//                 // 匀快速
//                 temp_rect.y = -m_fTimer * 2;
// 
//             }
//             else if (m_fTimer >= m_tTimeCtr.m_fSpeedKeepTime && m_fTimer < m_tTimeCtr.m_fSpeedDownTime && m_bIsOpen)
//             {
//                 // 减速
//                 temp_rect.y = -m_fTimer;
// 
//             }
//             else if (m_fTimer >= m_tTimeCtr.m_fSpeedDownTime && m_bIsOpen)
//             {
//                 if (m_iTarget == 0)
//                 {
//                     temp_rect.y = -m_fTimer;
//                     if (temp_rect.y <= -9.0f)
//                     {
//                         temp_rect.y = 0.0f;
//                         m_bIsOpen = false;
//                         // 播放开奖音效
//                         AudioList._instance.PlayPrizeEnjoyGameTypeAudio(m_iTarget);
//                     }
//                 }
//                 else if (m_iTarget == 1)
//                 {
//                     temp_rect.y = -m_fTimer;
//                     if (temp_rect.y <= -9.33f)
//                     {
//                         temp_rect.y = -0.33f;
//                         m_bIsOpen = false;
//                         // 播放开奖音效
//                         AudioList._instance.PlayPrizeEnjoyGameTypeAudio(m_iTarget);
//                     }
//                 }
//                 else if (m_iTarget == 2)
//                 {
//                     temp_rect.y = -m_fTimer;
//                     if (temp_rect.y <= -9.66f)
//                     {
//                         temp_rect.y = -0.66f;
//                         m_bIsOpen = false;
//                         // 播放开奖音效
//                         AudioList._instance.PlayPrizeEnjoyGameTypeAudio(m_iTarget);
//                     }
//                 }
//             }
        }
    }
}
