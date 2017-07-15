using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.BJL
{

    public enum enSceneType
    {
        SCENE_NONE = 0,
        SCENE_LODING,
        SCENE_SERVER,
        SCENE_GAME,
    };

    public class UIManager : MonoBehaviour
    {
        static UIManager instance = null;

        public GameObject o_loading;
        GameObject o_server = null;
		public GameObject o_game;
		public GameObject o_setting;
        enSceneType _SceneType = enSceneType.SCENE_NONE;

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIManager").AddComponent<UIManager>();
                }
                return instance;
            }
        }
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
//            o_game = GameObject.Find("scene_game");

            GoUI(enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
            ClearAllMsgBox();
        }

        void Update()
        {

        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
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
//			if(o_loading != null){
//				o_loading.SetActive(false);
//			}
			o_setting.SetActive(false);
            ClearAllMsgBox();
        }
        public void ClearAllMsgBox()
        {

        }

    }
}