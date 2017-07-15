using UnityEngine;
using System;

namespace com.QH.QPGame.Fishing
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioCtrl : MonoBehaviour
	{
		public static AudioCtrl Instance;
		private AudioSource audioSource;

		// 背景音乐.
		public AudioClip [] bgAudios;
		// 声音大小.
		public float bgVol = 1f;
		// 当前播放到哪一首背景音乐.
		private int bgIndex;

		// 加炮音效.
		public AudioClip changeGun;
		// 声音大小.
		public float changeVol = 1f;

		//玩家加入音效.
		public AudioClip userIn;
		// 声音大小.
		public float userInVol = 1f;

		// 玩家离开音效.
		public AudioClip userLeave;
		// 声音大小.
		public float userLeaveVol = 1f;

		// 买子弹成功音效.
		public AudioClip showBuyBullet;
		// 声音大小.
		public float showBuyBulletVol;

		// 结算成功音效.
		public AudioClip showAccount;
		// 声音大小.
		public float showAccountVol;

		// 点击button音效(开启，关闭).
		public AudioClip pressButtonOn;
		public AudioClip pressButtonOff;
		// 声音大小.
		public float pressButtonVol;
        // 声音播放开关
		[HideInInspector]
        public bool m_bIsOpen = true;
		// 高分板父节点(音效)
		public Transform m_tHightScoreAudio;
        // 喇叭图标
        public UISprite m_sSoundButton;

		void Awake()
		{
			Instance = this;
			audioSource = GetComponent<AudioSource>();
			NGUITools.soundVolume = 1;
		}

		// 初始化背景音乐.
		public void InitBG(int _bgIndex)
		{
			bgIndex = _bgIndex % bgAudios.Length;

			NextBG();
		}

		// 切换下一首背景音乐.
		public void NextBG()
		{
			if(bgAudios[bgIndex]!=null)
			{
				audioSource.clip = bgAudios[bgIndex];
				audioSource.volume = bgVol;
				audioSource.loop = true;
				audioSource.Play();
			}
			bgIndex++;
			if(bgIndex>=bgAudios.Length)
			{
				bgIndex = 0;
			}
		}
		void Play(AudioClip _clip, Vector3 _pos, float _vol=1f)
		{
            if (!m_bIsOpen) return;
			if(_clip!=null)
				AudioSource.PlayClipAtPoint(_clip, _pos, _vol);
		}
		public void UserIn()
		{
            if (!m_bIsOpen) return;
            Play(userIn, Vector3.zero, userInVol);
		}
		public void UserLeave()
		{
            if (!m_bIsOpen) return;
            Play(userLeave, Vector3.zero, userLeaveVol);
		}

		public void ChangeGun()
		{
            if (!m_bIsOpen) return;
            Play(changeGun, Vector3.zero, changeVol);
		}
		public void ShowBuyBulletPanel()
		{
            if (!m_bIsOpen) return;
            Play(showBuyBullet, Vector3.zero, showBuyBulletVol);
		}
		public void ShowAccount()
		{
            if (!m_bIsOpen) return;
            Play(showAccount, Vector3.zero, showAccountVol);
		}
		public void PressButton(bool _buttonOn)
		{
			if(_buttonOn)
				Play(pressButtonOn,  Vector3.zero, pressButtonVol);
			else 
				Play(pressButtonOff, Vector3.zero, pressButtonVol);
		}

        void OnDestroy()
        {
            Instance = null;
        }

        // 点击喇叭按钮,关闭或者开启AudioListener.
        public void audio_on_off()
        {
            if (m_bIsOpen)
            {
                NGUITools.soundVolume = 0;
				audioSource.enabled = false;
                m_bIsOpen = false;

				AudioSource[] audios = m_tHightScoreAudio.GetComponentsInChildren<AudioSource>();
				foreach(AudioSource var in audios){
					if(var!=null){
						var.enabled = false;
					}
                }
                if (m_sSoundButton != null)
                {
                    m_sSoundButton.spriteName = "btn_sound_off";
                }  
            }
            else
            {
                NGUITools.soundVolume = 1;
				audioSource.enabled = true;
                m_bIsOpen = true;

				AudioSource[] audios = m_tHightScoreAudio.GetComponentsInChildren<AudioSource>();
				foreach(AudioSource var in audios){
					if(var!=null){
						var.enabled = true;
					}
                }
                if (m_sSoundButton != null)
                {
                    m_sSoundButton.spriteName = "btn_sound";
                }
            }
		}
	}
}