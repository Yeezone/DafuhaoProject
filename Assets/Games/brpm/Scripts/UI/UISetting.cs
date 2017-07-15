using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.BRPM
{
    public class UISetting : MonoBehaviour
    {
        static UISetting instance = null;
        private GameObject o_set_panel = null;
        private  Transform o_set_music = null;
        private  Transform o_set_effect = null;
        private GameObject o_set_btn_effect = null;
        private GameObject o_set_btn_music = null;
        private GameObject o_btn_speak_set = null;
        private float mVol;
        private float eVol;

        public static UISetting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UISetting").AddComponent<UISetting>();
                }
                return instance;
            }
        }

        void Start()
        {
            mVol = GameObject.Find("scene_game").GetComponent<AudioSource>().volume;
            eVol = o_set_effect.gameObject.GetComponent<UISlider>().sliderValue;

            o_set_music.gameObject.GetComponent<UISlider>().sliderValue = 1.0f;
            o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = 1.0f;
        }

        void Update()
        {
            if (o_set_panel.activeSelf && (o_set_effect.gameObject.GetComponent<UISlider>().sliderValue != eVol
                || mVol != o_set_music.gameObject.GetComponent<UISlider>().sliderValue))
            {
                OnEffectChange();
                OnMusicChange();
            }
        }

        void Awake()
        {
            instance = this;

            o_set_panel = GameObject.Find("scene_setting");
            o_set_music = GameObject.Find("scene_setting/sd_music").GetComponent<Transform>();
            o_set_effect = GameObject.Find("scene_setting/sd_effect").GetComponent<Transform>();
            o_set_btn_effect = GameObject.Find("scene_setting/btn_effect");
            o_set_btn_music = GameObject.Find("scene_setting/btn_music");
            o_btn_speak_set = GameObject.Find("scene_setting/btn_speak");
        }

        void OnDestroy()
        {
            instance = null;
        }

        void OnEffectChange()
        {
            AudioSoundManger_PM._instance.MusicSlider1_OnClick(o_set_effect.gameObject.GetComponent<UISlider>());

            //音量大小的设置
            eVol = o_set_effect.gameObject.GetComponent<UISlider>().sliderValue;
            NGUITools.soundVolume = eVol;

            //喇叭按钮的显示
            PlayerPrefs.SetFloat("game_effect", eVol);
            if (eVol == 0)
            {
                PlayerPrefs.SetString("game_effect_switch", "off");
                o_set_btn_effect.GetComponent<UISprite>().spriteName = "set_btn_stop";
            }
            else
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
                o_set_btn_effect.GetComponent<UISprite>().spriteName = "set_btn_star";
            }
        }


        void OnMusicChange()
        {
            AudioSoundManger_PM._instance.MusicSlider0_OnClick(o_set_music.gameObject.GetComponent<UISlider>());

            //音量大小的设置
            mVol = o_set_music.gameObject.GetComponent<UISlider>().sliderValue;
            GameObject.Find("scene_game").GetComponent<AudioSource>().volume = mVol;
            //喇叭按钮的显示
            PlayerPrefs.SetFloat("game_music", mVol);
            if (mVol == 0)
            {
                PlayerPrefs.SetString("game_music_switch", "off");
                o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_stop";
            }
            else
            {
                PlayerPrefs.SetString("game_music_switch", "on");
                o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_star";

            }
        }

        void OnMusicCloseIvk()
        {
            if (PlayerPrefs.GetString("game_music_switch") == "off")
            {
                PlayerPrefs.SetString("game_music_switch", "on");
                GameObject.Find("scene_game").GetComponent<AudioSource>().Play();
                o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_star";
                o_set_music.gameObject.GetComponent<UISlider>().sliderValue = 0.5f;
                PlayerPrefs.SetFloat("game_music", 0.5f);
                GameObject.Find("scene_game").GetComponent<AudioSource>().volume = 0.5f;
            }
            else
            {
                PlayerPrefs.SetString("game_music_switch", "off");
                GameObject.Find("scene_game").GetComponent<AudioSource>().Pause();
                o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_stop";

                o_set_music.gameObject.GetComponent<UISlider>().sliderValue = 0;
                PlayerPrefs.SetFloat("game_music", 0);
                GameObject.Find("scene_game").GetComponent<AudioSource>().volume = 0;
            }
        }
        void OnEffectCloseIvk()
        {
            if (PlayerPrefs.GetString("game_effect_switch") == "off")
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
                o_set_btn_effect.GetComponent<UISprite>().spriteName = "set_btn_star";

                o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = 0.5f;
                PlayerPrefs.SetFloat("game_effect", 0.5f);
                NGUITools.soundVolume = 0.5f;
            }
            else
            {
                PlayerPrefs.SetString("game_effect_switch", "off");
                o_set_btn_effect.GetComponent<UISprite>().spriteName = "set_btn_stop";

                o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = 0;
                PlayerPrefs.SetFloat("game_effect", 0);
                NGUITools.soundVolume = 0;
            }
        }

        public void Show(bool bshow)
        {
            o_set_panel.SetActive(bshow);
            if (bshow == true)
            {
                NGUITools.soundVolume = o_set_effect.gameObject.GetComponentInChildren<UISlider>().value;
                GameObject.Find("scene_game").GetComponent<AudioSource>().volume = o_set_music.gameObject.GetComponent<UISlider>().value;

                //喇叭按钮的显示
                if (PlayerPrefs.GetString("game_music_switch") == "off")
                {
                    o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_stop";
                }
                else
                {
                    o_set_btn_music.GetComponent<UISprite>().spriteName = "set_btn_star";
                }
                if (PlayerPrefs.GetString("game_effect_switch") == "off")
                {
                    o_set_btn_effect.GetComponent<UISprite>().spriteName = "set_btn_stop";
                }
                else
                {
                    o_set_btn_effect.GetComponentInChildren<UISprite>().spriteName = "set_btn_star";
                }
            }
        }

    }
}