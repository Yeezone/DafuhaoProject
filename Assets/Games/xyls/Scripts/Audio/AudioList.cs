using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class AudioList : MonoBehaviour
    {
        public static AudioList _instance;
        // 开奖音效列表
        public AudioClip[] m_aPrizeAudio;
        //
        private UIPlaySound m_uPlaySound;
        // 计时
        private float m_fTimer = 0;
        // 开关
        private bool m_bIsopen = false;

        void Start()
        {
            _instance = this;
            m_uPlaySound = gameObject.GetComponent<UIPlaySound>();
        }


        void Update()
        {
            m_fTimer += Time.deltaTime;

            if (m_fTimer >= 2f && m_bIsopen)
            {
                StopPrizeAudio();
                m_bIsopen = false;
                m_fTimer = 0;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// 播放开奖音效_动物颜色
        /// </summary>
        public void PlayPrizeAnimalColorAudio(int animal, int color, int prizeMode)
        {
            if (prizeMode == 0)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[(animal * 3) + color];
            }
            else if (prizeMode == 1)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[12 + color];
            }
            else if (prizeMode == 2)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[15 + animal];
            }
            else if (prizeMode == 3)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[19];
            }
            else if (prizeMode == 4)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[23];
            }
            else if (prizeMode == 5)
            {
                m_uPlaySound.audioClip = m_aPrizeAudio[19];
            }
            else
            {
                Debug.LogError("游戏音效:未知开奖模式");
            }
			
			m_fTimer = 0;
            m_uPlaySound.enabled = true;
            m_bIsopen = true;
        }

        /// <summary>
        /// 播放开奖音效_庄闲和
        /// </summary>
        public IEnumerator PlayPrizeEnjoyGameTypeAudio(int enjoyGameType,float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (enjoyGameType >= 3)
            {
                Debug.LogError("音效:未知开奖庄闲和类型");
            }
            m_uPlaySound.audioClip = m_aPrizeAudio[20 + enjoyGameType];

            m_uPlaySound.enabled = true;
            m_bIsopen = true;
            m_fTimer = 0;
        }

        /// <summary>
        /// 关闭开奖音效(关闭组件)
        /// </summary>
        public void StopPrizeAudio()
        {
            m_uPlaySound.enabled = false;
        }

    }
}
