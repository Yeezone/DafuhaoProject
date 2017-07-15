using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.SHZ
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
        public static UIManager Instance = null;

		public GameObject o_loading = null;
		public GameObject o_loading_Prefabs = null;
		public GameObject o_game = null;
		public GameObject o_setting = null;
		public GameObject o_help = null;
        GameObject o_server = null;

        enSceneType _SceneType = enSceneType.SCENE_NONE;

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

            GoUI(enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
            ClearAllMsgBox();
        }

        void Update()
        {

        }

		void OnDestroy(){
			Instance=null;
		}

        void Awake()
        {
			Instance = this;
        }
        public void GoUI(enSceneType stFrom, enSceneType stTo)
        {
			HideAllUI();
			_SceneType = enSceneType.SCENE_GAME;
			o_game.SetActive(true);
			if(o_loading != null){
				o_loading.SetActive(true);
			}else
			{
				GameObject o_load = Instantiate( o_loading_Prefabs, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f) ) )as GameObject;
				o_load.transform.parent = UIManager.Instance.o_game.transform;
				o_load.transform.localScale = new Vector3(1f,1f,1f);
				o_loading = o_load;
				o_loading.SetActive(true);
			}
			o_game.GetComponent<UIGame>().Init();
        }

        void HideAllUI()
        {
            o_game.SetActive(false);
			if(o_loading != null){
				o_loading.SetActive(false);
			}
			if(o_help != null){
				o_help.SetActive(false);
			}
            ClearAllMsgBox();
        }
        public void ClearAllMsgBox()
        {

        }

    }
}