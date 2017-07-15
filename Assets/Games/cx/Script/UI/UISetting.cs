using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.CX
{

    public class UISetting : MonoBehaviour
    {
        static UISetting instance = null;
        public GameObject scene_game; 
        GameObject o_set_panel = null;
        GameObject o_set_music = null;
        GameObject o_set_effect = null;
        GameObject o_set_btn_effect = null;
        GameObject o_set_btn_music = null;
        GameObject o_btn_speak_set = null;

        private GameObject o_close = null;
        /// <summary>
        /// ±≥æ∞“Ù¿÷
        /// </summary>
        private bool bgMusicOpen = true;
        private GameObject o_bgMusic = null;
        private GameObject o_bgPro = null;

        /// <summary>
        /// ”Œœ∑“Ù–ß
        /// </summary>
        private bool gameMusicOpen = true;
        private  GameObject o_gameMusic = null;
        private GameObject o_gamePro = null;

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

        void Awake()
        {
            //o_set_panel = GameObject.Find("scene_setting");
            //o_set_music = GameObject.Find("scene_setting/sd_music");
            //o_set_effect = GameObject.Find("scene_setting/sd_effect");
            //o_set_btn_effect = GameObject.Find("scene_setting/btn_effect");
            //o_set_btn_music = GameObject.Find("scene_setting/btn_music");
            //o_btn_speak_set = GameObject.Find("scene_setting/btn_speak");
            o_close = transform.Find("SetClose").gameObject;
            UIEventListener.Get(o_close).onClick = OnClick;

            o_bgMusic = transform.Find("BGMusic/bgsilence").gameObject;
            o_gameMusic = transform.Find("GameMusic/gamesilence").gameObject;
            UIEventListener.Get(o_bgMusic).onClick = OnClick;
            UIEventListener.Get(o_gameMusic).onClick = OnClick;

            o_bgPro = transform.Find("BGMusic/bgpro").gameObject;
            o_gamePro = transform.Find("GameMusic/bgpro").gameObject;
            UIEventListener.Get(o_bgPro).onDragEnd = OnDragEnd;
            UIEventListener.Get(o_gamePro).onDragEnd = OnDragEndGame;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        void OnDragEnd(GameObject obj)
        {
        }
        void OnDragEndGame(GameObject obj)
        {
        }

        public void OnClick(GameObject obj)
        {
            if (0 == obj.name.CompareTo("SetClose"))
            {
                gameObject.SetActive(false);
            }
            else if (0 == obj.name.CompareTo("bgsilence"))
            {
                UIGame.StopBGM();
                bgMusicOpen = !bgMusicOpen;
            }
            else if (0 == obj.name.CompareTo("gamesilence"))
            {
                UIGame.StopEffect();
                gameMusicOpen = !gameMusicOpen;
            }
        }

        void Start()
        {

        }

        void Update()
        {
            if (this.gameObject.activeSelf == true)
            {
                if (o_gamePro.GetComponent<UISlider>().value == 0)
                {
                    o_gameMusic.transform.Find("silence").gameObject.SetActive(true);
                    o_gameMusic.transform.Find("open").gameObject.SetActive(false);
                }
                else
                {
                    o_gameMusic.transform.Find("silence").gameObject.SetActive(false);
                    o_gameMusic.transform.Find("open").gameObject.SetActive(true);
                }

                if (o_bgPro.GetComponent<UISlider>().value == 0)
                {
                    o_bgMusic.transform.Find("silence").gameObject.SetActive(true);
                    o_bgMusic.transform.Find("open").gameObject.SetActive(false);
                }
                else
                {
                    o_bgMusic.transform.Find("silence").gameObject.SetActive(false);
                    o_bgMusic.transform.Find("open").gameObject.SetActive(true);
                }

                if (o_gamePro.GetComponent<UISlider>().value == 0 && o_bgPro.GetComponent<UISlider>().value == 0)
                {
                    scene_game.transform.GetComponent<UIGame>().btn_horn.transform.GetComponent<UISprite>().spriteName = "voice_close";
                }
                else
                {
                    scene_game.transform.GetComponent<UIGame>().btn_horn.transform.GetComponent<UISprite>().spriteName = "voice_play";
                }
            }
        }

        void OnEffectChange()
        {
            float eVol = o_set_effect.GetComponent<UISlider>().sliderValue;
            PlayerPrefs.SetFloat("game_effect", eVol);
            NGUITools.soundVolume = eVol;

            if (eVol == 0)
            {
                PlayerPrefs.SetString("game_effect_switch", "off");
                o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";
            }
            else
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
                o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";
            }


        }
        void OnMusicChange()
        {

            float mVol = o_set_music.GetComponent<UISlider>().sliderValue;
            PlayerPrefs.SetFloat("game_music", mVol);
            GameObject.Find("Panel").GetComponent<AudioSource>().volume = mVol;

            if (mVol == 0)
            {
                PlayerPrefs.SetString("game_music_switch", "off");
                o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";
            }
            else
            {
                PlayerPrefs.SetString("game_music_switch", "on");
                o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";

            }
        }


        void OnMusicCloseIvk()
        {
            if (PlayerPrefs.GetString("game_music_switch", "on") == "off")
            {
                PlayerPrefs.SetString("game_music_switch", "on");
                GameObject.Find("Panel").GetComponent<AudioSource>().Play();
                o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";

                o_set_music.GetComponent<UISlider>().sliderValue = 0.5f;
                PlayerPrefs.SetFloat("game_music", 0.5f);
                GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0.5f;
            }
            else
            {
                PlayerPrefs.SetString("game_music_switch", "off");
                GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
                o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";

                o_set_music.GetComponent<UISlider>().sliderValue = 0;
                PlayerPrefs.SetFloat("game_music", 0);
                GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0;
            }
        }
        void OnEffectCloseIvk()
        {
            if (PlayerPrefs.GetString("game_effect_switch", "on") == "off")
            {
                PlayerPrefs.SetString("game_effect_switch", "on");
                o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";

                o_set_effect.GetComponent<UISlider>().sliderValue = 0.5f;
                PlayerPrefs.SetFloat("game_effect", 0.5f);
                NGUITools.soundVolume = 0.5f;
            }
            else
            {
                PlayerPrefs.SetString("game_effect_switch", "off");
                o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";

                o_set_effect.GetComponent<UISlider>().sliderValue = 0;
                PlayerPrefs.SetFloat("game_effect", 0);
                NGUITools.soundVolume = 0;
            }
        }
        void OnBtnSpeakSetIvk()
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
        void OnSettingCloseIvk()
        {
            o_set_panel.SetActive(false);
        }
        public void Show(bool bshow)
        {
            o_set_panel.SetActive(bshow);
            if (bshow == true)
            {
                o_set_music.GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("game_music", 0.5f);
                o_set_effect.GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("game_effect", 0.5f);
                if (PlayerPrefs.GetString("game_music_switch", "") == "off")
                {
                    o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";
                }
                else
                {
                    o_set_btn_music.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";
                }
                if (PlayerPrefs.GetString("game_effect_switch", "") == "off")
                {
                    o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_stop";
                }
                else
                {
                    o_set_btn_effect.GetComponentInChildren<UISlicedSprite>().spriteName = "set_btn_star";
                }

                if (PlayerPrefs.GetString("game_speak_switch", "") == "off")
                {
                    o_btn_speak_set.GetComponent<UICheckbox>().isChecked = false;
                }
                else
                {
                    o_btn_speak_set.GetComponent<UICheckbox>().isChecked = true;
                }

            }
        }
    }
}