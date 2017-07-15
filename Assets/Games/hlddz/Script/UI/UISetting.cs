using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    public class UISetting : MonoBehaviour
    {
        private static UISetting instance = null;
        private GameObject o_set_panel = null;
        private GameObject o_set_music = null;
        private GameObject o_set_effect = null;
        private GameObject o_set_btn_effect = null;
        private GameObject o_set_btn_music = null;
        private GameObject o_btn_speak_set = null;

        private float _musicVol = 0.5f;
        private float _effectVol = 0.5f;
		// 记录当前的游戏音乐和音效的值大小
		private float temp_music_value = 0.5f;
		private float temp_effect_value = 0.5f;
        private float count = 0;

		//设置界面是否显示
		private bool showSettingUI=false;

        public static UISetting Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
            o_set_panel = GameObject.Find("scene_setting");
            o_set_music = GameObject.Find("scene_setting/sd_music");
            o_set_effect = GameObject.Find("scene_setting/sd_effect");
            o_set_btn_effect = GameObject.Find("scene_setting/btn_effect");
            o_set_btn_music = GameObject.Find("scene_setting/btn_music");
            o_btn_speak_set = GameObject.Find("scene_setting/btn_speak");
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Start()
        {

        }

        public void OnEffectChange()
        {

            float eVol = o_set_effect.GetComponent<UISlider>().sliderValue;
            PlayerPrefs.SetFloat("game_effect", eVol);
            NGUITools.soundVolume = eVol;
			
            if (count != 0)
            {
                temp_effect_value = eVol;
            }
            

            if (eVol == 0)
            {
                PlayerPrefs.SetString("game_effect_switch", "off");
				o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_stop";
            }
            else
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
				o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_star";
            }


        }

        public void OnMusicChange()
        {
			float mVol = o_set_music.GetComponent<UISlider>().sliderValue;
            PlayerPrefs.SetFloat("game_music", mVol);
            if (count != 0)
            {
                temp_music_value = mVol;
            }

			if(showSettingUI)
			{
				GameObject.Find("Panel").GetComponent<AudioSource>().volume = mVol;
			}

			if (mVol == 0)
            {
                PlayerPrefs.SetString("game_music_switch", "off");
				o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_stop";

            }
            else
            {
                PlayerPrefs.SetString("game_music_switch", "on");
				o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_star";

            }
        }


        private void OnMusicCloseIvk()
        {
            if (PlayerPrefs.GetString("game_music_switch", "on") == "off")
            {
                PlayerPrefs.SetString("game_music_switch", "on");
                GameObject.Find("Panel").GetComponent<AudioSource>().Play();
				o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_star";

                o_set_music.GetComponent<UISlider>().sliderValue = _musicVol;
                PlayerPrefs.SetFloat("game_music", _musicVol);
                GameObject.Find("Panel").GetComponent<AudioSource>().volume = _musicVol;
            }
            else
            {
                _musicVol = o_set_music.GetComponent<UISlider>().sliderValue;

                PlayerPrefs.SetString("game_music_switch", "off");
                GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
				o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_stop";

				o_set_music.GetComponent<UISlider>().sliderValue = 0f;
				PlayerPrefs.SetFloat("game_music", 0f);
				GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0f;
            }
        }

        private void OnEffectCloseIvk()
        {
            if (PlayerPrefs.GetString("game_effect_switch", "on") == "off")
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
				o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_star";

                o_set_effect.GetComponent<UISlider>().sliderValue = _effectVol;
                PlayerPrefs.SetFloat("game_effect", _effectVol);
                NGUITools.soundVolume = _effectVol;
            }
            else
            {

                _effectVol = o_set_effect.GetComponent<UISlider>().sliderValue;

                PlayerPrefs.SetString("game_effect_switch", "off");
				o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_stop";

				o_set_effect.GetComponent<UISlider>().sliderValue = 0f;
				PlayerPrefs.SetFloat("game_effect", 0f);
				NGUITools.soundVolume = 0f;
            }
        }

        private void OnBtnSpeakSetIvk()
        {
            if (PlayerPrefs.GetString("game_speak_switch", "on") == "off")
            {
                PlayerPrefs.SetString("game_speak_switch", "on");
                o_btn_speak_set.GetComponent<UICheckbox>().isChecked = true;
            }
            else
            {
                PlayerPrefs.SetString("game_speak_switch", "off");
                o_btn_speak_set.GetComponent<UICheckbox>().isChecked = false;
            }
        }

        private void OnSettingCloseIvk()
        {
            o_set_panel.SetActive(false);
        }

        public void Show(bool bshow)
        {
            o_set_panel.SetActive(bshow);
			showSettingUI = bshow;
            if (bshow == true)
            {
                count += 1;
				// 每次打开设置面板都会记录上次退出时候的音效值.
				o_set_music.GetComponent<UISlider>().sliderValue = temp_music_value;
				o_set_effect.GetComponent<UISlider>().sliderValue = temp_effect_value;

                if (PlayerPrefs.GetString("game_music_switch", "") == "off")
                {
					o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_stop";
                }
                else
                {
                    o_set_btn_music.GetComponent<UIButton>().normalSprite = "set_btn_star";
                }
                if (PlayerPrefs.GetString("game_effect_switch", "") == "off")
                {
                    o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_stop";
                }
                else
                {
                    o_set_btn_effect.GetComponent<UIButton>().normalSprite = "set_btn_star";
                }
            }
        }
    }
}