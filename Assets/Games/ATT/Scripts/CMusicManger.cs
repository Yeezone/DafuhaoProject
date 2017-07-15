using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMusicManger : MonoBehaviour {

    public static CMusicManger _instance;
    public List<AudioClip> m_lMusicList = new List<AudioClip>();

    /// <summary>
    /// 背景音乐
    /// </summary>
    public List<AudioClip> m_lBgMusicList = new List<AudioClip>();
   // public GameObject m_gBgMusic;



    private bool _bIsOpen;
    public bool m_bIsOpen
    {
        get { return _bIsOpen; }
        set
        {
            _bIsOpen = value;

            if (_bIsOpen)
            {
                // m_gBgMusic.GetComponent<AudioSource>().Stop();
               // PlayBgSound();
            }
           // else m_gBgMusic.GetComponent<AudioSource>().Stop();
        }
    }

    void Awake()
    {
        _instance = this;
        PlayBgSound();
       // m_gBgMusic.GetComponent<AudioSource>().Stop();
        m_bIsOpen = true;
        
    }

    void OnDestroy()
    {
        _instance = null;
    }
    public void PlaySound(string _strMusicName)
    {

       if (m_bIsOpen == false) return;
        for (int i = 0; i < m_lMusicList.Count; i++)
        {
            if (m_lMusicList[i].name == _strMusicName)
            {
                this.GetComponent<AudioSource>().clip = m_lMusicList[i];
                this.GetComponent<AudioSource>().volume = 0.6f;
                this.GetComponent<AudioSource>().PlayOneShot(m_lMusicList[i]);
            }
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBgSound()
    {
        if (m_lBgMusicList.Count > 0)
        {
            int _temp = Random.Range(0, m_lBgMusicList.Count);

            if (_temp >= m_lBgMusicList.Count) _temp = 0;
           // m_gBgMusic.GetComponent<AudioSource>().clip = m_lBgMusicList[_temp];
           // m_gBgMusic.GetComponent<AudioSource>().Play();
        }
       // m_gBgMusic.GetComponent<AudioSource>().Stop();
    }
    public void CloseBgSound()
    {
       // m_gBgMusic.GetComponent<AudioSource>().Stop();
    }

}
