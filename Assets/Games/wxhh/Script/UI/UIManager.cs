using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.WXHH
{

    public enum enSceneType
    {
        SCENE_NONE = 0,
        SCENE_LODING,
        SCENE_SERVER,
        SCENE_GAME,
    };

	public enum enPlatform
	{
		MOBILE = 0,
		PC
	};


    public class UIManager : MonoBehaviour
    {
		public static UIManager Instance = null;

        public GameObject o_loading;
		public GameObject o_setting;
        GameObject o_server = null;
        GameObject o_game = null;

        enSceneType _SceneType = enSceneType.SCENE_NONE;

		public enPlatform  curPlatform; 	//当前平台

//		public bool isAudioEffect = true;			//游戏音效
//		public bool isAudioMusic = true;			//背景音乐

//		public static UIManager Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new GameObject("UIManager").AddComponent<UIManager>();
//                }
//                return instance;
//            }
//        }
        public enSceneType SceneType
        {
            get
            {
                return _SceneType;
            }
        }
        void Start()
        {
            //创建所有UI对象
            o_game = GameObject.Find("scene_game");
			GoUI(enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
            ClearAllMsgBox();
        }

		void Update()
        {

        }

        void Awake()
        {
			if (Instance == null){
				Instance = this;
            }
        }

		void OnDestroy()
{
			Instance = null;
		}

        public void GoUI(enSceneType stFrom, enSceneType stTo)
        {
			HideAllUI();
			_SceneType = enSceneType.SCENE_GAME;
			o_game.SetActive(true);
			if(o_loading != null){
				o_loading.SetActive(true);
			}
			o_game.GetComponent<UIGame>().Init();
        }

        void HideAllUI()
        {
            o_game.SetActive(false);
			if(o_loading != null) o_loading.SetActive(false);
			if(o_setting != null)
			{
				o_setting.transform.localPosition = new Vector3(0,0,0);
				o_setting.SetActive(false);
			}
            ClearAllMsgBox();
        }
        public void ClearAllMsgBox()
        {

        }

    }
}