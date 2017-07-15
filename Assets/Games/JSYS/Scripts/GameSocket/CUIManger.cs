using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.JSYS
{
    public enum enSceneType
    {
        SCENE_NONE = 0,
        SCENE_LOGIN,
        SCENE_REG,
        SCENE_QUICK,
        SCENE_SERVER,
        SCENE_GAME,
        SCENE_TASK,
        SCENE_SHOP,
        SCENE_HELP,
        SCENE_PERSON,
        SCENE_MODIFY,
        SCENE_SINGLE,
        SCENE_VIP
    };
    public class CUIManger : MonoBehaviour
    {


        static CUIManger _instance = null;
        GameObject o_game = null;

        enSceneType _SceneType = enSceneType.SCENE_NONE;

        public static CUIManger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("UIManger").AddComponent<CUIManger>();
                }
                return _instance;
            }
        }

        public enSceneType SceneType
        {
            get
            {
                return _SceneType;
            }
        }
        /// <summary>
        /// 游戏状态
        /// </summary>
        public C_CMD_S_StatusPlay m_cStatusPlay = new C_CMD_S_StatusPlay();
        /// <summary>
        /// 游戏返回结果
        /// </summary>
        public C_CMD_S_GameEnd m_cGameResualt = new C_CMD_S_GameEnd();
        public C_CMD_S_PlayBet m_cPlayBet = new C_CMD_S_PlayBet();
        /// <summary>
        /// 游戏进行中
        /// </summary>
        public CWindowShowHide_JSYS m_cGamePlayState;
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        void OnDestroy()
        {
            _instance = null;
        }
        void Start()
        {
             o_game = GameObject.Find("GameParent");
             GoUI(enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
        }

        public void GoUI(enSceneType stFrom, enSceneType stTo)
        {
            // HideAllUI();
            _SceneType = enSceneType.SCENE_GAME;
            if (o_game != null)
            {
                o_game.SetActive(true);
               o_game.GetComponent<CUIGame>().Init();
            }
        }
    }
}
