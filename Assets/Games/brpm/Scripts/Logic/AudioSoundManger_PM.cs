using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioSoundManger_PM : MonoBehaviour
{
    public static AudioSoundManger_PM _instance;
    public List<AudioClip> m_lMusicList = new List<AudioClip>();
    public GameObject resultObj;
    public List<AudioClip> m_lMusicListReslut = new List<AudioClip>();
    /// <summary>
    /// 背景音乐
    /// </summary>
    public List<AudioClip> m_lBgMusicList = new List<AudioClip>();
    public GameObject m_gBgMusic;
    /// <summary>
    /// 定时器声音
    /// </summary>
    public GameObject m_gTimerMusic;

    /// <summary>
    /// 马蹄声
    /// </summary>
    public GameObject m_HorseMusic;

    /// <summary>
    /// 音量控制
    /// </summary>
    public float m_fMusicValue0 = 1.0f;
    public float m_fMusicValue1 = 1.0f;

    public UISlider m_MusicSlider0;
    public UISlider m_MusicSlider1;

    private bool _bIsOpen;
    public bool m_bIsOpen
    {
        get { return _bIsOpen; }
        set
        {
            _bIsOpen = value;

            if (_bIsOpen)
            {
                m_gBgMusic.GetComponent<AudioSource>().Play();

            }
            else m_gBgMusic.GetComponent<AudioSource>().Stop();
        }
    }
    void Awake()
    {
        _instance = this;

        m_bIsOpen = true;
    }

    void OnDestroy()
    {
        _instance = null;
    }
    // Use this for initialization
    void Start()
    {
        m_MusicSlider0.value = 1.0f;
        m_MusicSlider1.value = 1.0f;
    }

    public void PlaySound(string _strMusicName)
    {
        if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lMusicList.Count; i++)
        {
            if (m_lMusicList[i].name == _strMusicName)
            {
                this.GetComponent<AudioSource>().clip = m_lMusicList[i];
                this.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
                this.GetComponent<AudioSource>().PlayOneShot(m_lMusicList[i]);
            }
        }
    }
    public void StopSound(string _strMusicName)
    {
        if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lMusicList.Count; i++)
        {
            if (m_lMusicList[i].name == _strMusicName)
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void PlayResultSound(string _strMusicName)
    {
        if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lMusicListReslut.Count; i++)
        {
            if (m_lMusicListReslut[i].name == _strMusicName)
            {
                resultObj.transform.GetComponent<AudioSource>().clip = m_lMusicListReslut[i];
                resultObj.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
                resultObj.transform.GetComponent<AudioSource>().PlayOneShot(m_lMusicListReslut[i]);
            }
        }
    }
    public void PlayResultStopSound(string _strMusicName)
    {
        if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lMusicListReslut.Count; i++)
        {
            if (m_lMusicListReslut[i].name == _strMusicName)
            {
                resultObj.transform.GetComponent<AudioSource>().Stop();
            }
        }
    }

    /// <summary>
    /// 定时器
    /// </summary>
    public void PlayTimerSound()
    {
        if (m_bIsOpen == false) return;
        m_gTimerMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_gTimerMusic.GetComponent<AudioSource>().Play();
    }
    public void StopTimerSound()
    {
        m_gTimerMusic.GetComponent<AudioSource>().Stop();
    }

    /// <summary>
    /// 定时器
    /// </summary>
    public void PlayHorseSound()
    {
        if (m_bIsOpen == false) return;
        m_HorseMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_HorseMusic.GetComponent<AudioSource>().Play();
    }
    public void StopHorseSound()
    {
        m_HorseMusic.GetComponent<AudioSource>().Stop();
    }

    //播放背景音乐
    public void PlayBgSound(string _strMusicName)
    {
        if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lBgMusicList.Count; i++)
        {
            if (m_lBgMusicList[i].name == _strMusicName)
            {
                m_gBgMusic.GetComponent<AudioSource>().clip = m_lBgMusicList[i];
                m_gBgMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue0;
                m_gBgMusic.GetComponent<AudioSource>().Play();
            }
        }
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void StopBgMusic()
    {
        m_gBgMusic.GetComponent<AudioSource>().Stop();
    }
    /// <summary>
    /// 音效设置
    /// </summary>
    /// <param name="_Slider"></param>
    public void MusicSlider0_OnClick(UISlider _Slider)
    {
        Debug.Log("   public void MusicSlider0_OnClick(UISlider _Slider)");
        m_fMusicValue0 = _Slider.value;
        m_gBgMusic.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue0;
    }

    public void MusicSlider1_OnClick(UISlider _Slider)
    {
        Debug.Log("   dddddddddddddddd");
        m_fMusicValue1 = _Slider.value;
        this.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_gTimerMusic.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        resultObj.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_HorseMusic.transform.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
    }
}
