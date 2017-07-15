using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMusicManger_JSYS : MonoBehaviour
{
    public static CMusicManger_JSYS _instance;
    public List<AudioClip> m_lMusicList = new List<AudioClip>();

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
    /// 音量控制
    /// </summary>
    public float m_fMusicValue0 = 0.3f;
    public float m_fMusicValue1 = 0.5f;

    public UISlider m_MusicSlider0;
    public UISlider m_MusicSlider1;
    public GameObject m_gSet;

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
	void Start () {

        m_MusicSlider0.value = 0.3f;
        m_MusicSlider1.value = 0.5f;

	   
	}
	
	// Update is called once per frame
	void Update () {
	
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
    /// <summary>
    /// 定时器
    /// </summary>
    public void PlayTimerSound()
    {
        if (m_bIsOpen == false) return;
        m_gTimerMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_gTimerMusic.GetComponent<AudioSource>().Play();
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
        m_fMusicValue0 = _Slider.value;
        m_gBgMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue0;
    }

    public void MusicSlider1_OnClick(UISlider _Slider)
    {
        m_fMusicValue1 = _Slider.value;
        this.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
        m_gTimerMusic.GetComponent<AudioSource>().volume = 1.0f * m_fMusicValue1;
    }
}
