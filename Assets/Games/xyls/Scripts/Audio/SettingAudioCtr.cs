using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class SettingAudioCtr : MonoBehaviour
    {

        //  背景音乐滑条
        public UISlider m_sBGM;
        //  游戏音乐滑条
        public UISlider m_sGameMusic;

        // 背景音乐喇叭
        public AudioSource m_aBGMAudioSource;

        void Start()
        {
            m_sBGM.value = 1;
            m_sGameMusic.value = 1;
        }

        void Update()
        {
            // 更新游戏声音音量
            m_aBGMAudioSource.volume = m_sBGM.value;
            NGUITools.soundVolume = m_sGameMusic.value;
        }
    }
}
