using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class AudioBgCtr : MonoBehaviour
    {
        public static AudioBgCtr _instance;
        // 背景音乐列表
        public AudioClip[] m_aBgAudios;

        // AudioSource 组件_背景(loop播放)
        public AudioSource m_aAudio_bg;

        void Start()
        {
            _instance = this;
        }


        void Update()
        {

        }

        void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBGM(int index)
        {
            m_aAudio_bg.clip = m_aBgAudios[index];
            m_aAudio_bg.Play();
        }

        /// <summary>
        /// 停止播放BGM
        /// </summary>
        public void StopBGM()
        {
            m_aAudio_bg.clip = null;
        }
    }
}
