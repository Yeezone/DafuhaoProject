using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.WXHH
{
	public class UISetting : MonoBehaviour
	{
		static UISetting instance = null;
		static GameObject o_set_panel = null;
		public static Transform o_set_music = null;
		public static Transform o_set_effect = null;
		static GameObject o_set_btn_effect = null;
		static GameObject o_set_btn_music = null;
//		static GameObject o_btn_speak_set = null;
		private float mVol;
		private float eVol;
		public static UISetting Instance
		{
			get
			{
                return instance;
			}
		}
		void Start(){
			mVol=GameObject.Find("scene_game").GetComponent<AudioSource>().volume;
			eVol=o_set_effect.gameObject.GetComponent<UISlider>().sliderValue;
			
			o_set_music.gameObject.GetComponent<UISlider>().sliderValue = mVol;
			o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = eVol;
			
//			ShowDisabled();
		}
		
		void Update()
		{
			if( o_set_panel.activeSelf)
			{
				OnEffectChange();
				OnMusicChange();
			}
		}
		
		void Awake()
		{
            instance = this;

			o_set_panel = GameObject.Find("scene_setting");
			o_set_music =GameObject.Find("scene_setting/sd_music").GetComponent<Transform>();
			o_set_effect =GameObject.Find("scene_setting/sd_effect").GetComponent<Transform>();
			o_set_btn_effect = GameObject.Find("scene_setting/btn_effect");
			o_set_btn_music = GameObject.Find("scene_setting/btn_music");
//			o_btn_speak_set = GameObject.Find("scene_setting/btn_speak");
		}
		
		void OnEffectChange()
		{
			//音量大小的设置
			eVol = o_set_effect.gameObject.GetComponent<UISlider>().sliderValue;
			NGUITools.soundVolume = eVol;
			
			//喇叭按钮的显示
			PlayerPrefs.SetFloat("game_effect", eVol);
			if (eVol == 0)
			{
				PlayerPrefs.SetString("game_effect_switch", "off");
				o_set_btn_effect.GetComponent<UISprite>().spriteName = "btn_audio_off";
			}
			else
			{
				PlayerPrefs.SetString("game_effect_switch", "on");
				o_set_btn_effect.GetComponent<UISprite>().spriteName = "btn_audio_on";
			}
			
			//声音禁用按钮显示
			ShowDisabled();
			
		}
		
		
		void OnMusicChange()
		{
			//音量大小的设置
			mVol = o_set_music.gameObject.GetComponent<UISlider>().sliderValue;
			GameObject.Find("scene_game").GetComponent<AudioSource>().volume = mVol;
			//喇叭按钮的显示
			PlayerPrefs.SetFloat("game_music", mVol);
			if (mVol == 0)
			{
				PlayerPrefs.SetString("game_music_switch", "off");
				o_set_btn_music.GetComponent<UISprite>().spriteName = "btn_audio_off";
			}
			else
			{
				PlayerPrefs.SetString("game_music_switch", "on");
				o_set_btn_music.GetComponent<UISprite>().spriteName = "btn_audio_on";
				
			}
			
			//声音禁用按钮的显示
			ShowDisabled();
			
		}
		
		void OnMusicCloseIvk()
		{
			if (PlayerPrefs.GetString("game_music_switch") == "off")
			{
				PlayerPrefs.SetString("game_music_switch", "on");
				GameObject.Find("scene_game").GetComponent<AudioSource>().Play();
				o_set_btn_music.GetComponent<UISprite>().spriteName = "btn_audio_on";
				o_set_music.gameObject.GetComponent<UISlider>().sliderValue = 0.5f;
				PlayerPrefs.SetFloat("game_music", 0.5f);
				GameObject.Find("scene_game").GetComponent<AudioSource>().volume = 0.5f;
			}
			else
			{
				PlayerPrefs.SetString("game_music_switch", "off");
				GameObject.Find("scene_game").GetComponent<AudioSource>().Pause();
				o_set_btn_music.GetComponent<UISprite>().spriteName = "btn_audio_off";
				
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
				o_set_btn_effect.GetComponent<UISprite>().spriteName="btn_audio_on";
				
				o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = 0.5f;
				PlayerPrefs.SetFloat("game_effect", 0.5f);
				NGUITools.soundVolume = 0.5f;
			}
			else
			{
				PlayerPrefs.SetString("game_effect_switch", "off");
				o_set_btn_effect.GetComponent<UISprite>().spriteName = "btn_audio_off";
				
				o_set_effect.gameObject.GetComponent<UISlider>().sliderValue = 0;
				PlayerPrefs.SetFloat("game_effect", 0);
				NGUITools.soundVolume = 0;
			}
		}
		//声音关闭显示
		void ShowDisabled()
		{
			if(eVol==0&&mVol==0)
			{
//				UIGame.btn_chat_disabled.SetActive(true);
			}else{
//				UIGame.btn_chat_disabled.SetActive(false);
			}
		}
		
		void OnSettingCloseIvk()
		{
//			o_set_panel.SetActive(false);
//			UIGame.btn_chat.GetComponent<UIButton>().isEnabled=true;
		}
		public void Show(bool bshow)
		{
//			o_set_panel.SetActive(bshow);
			if (bshow == true)
			{

				
			}
		}
	}
}