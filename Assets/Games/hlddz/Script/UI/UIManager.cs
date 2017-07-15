using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.DDZ
{
    public enum enSceneType
    {
        SCENE_NONE = 0,
        SCENE_LOGIN,
        SCENE_REG,
        SCENE_QUICK,
        SCENE_SERVER,
        SCENE_TASK,
        SCENE_RANK,
        SCENE_SHOP,
        SCENE_HELP,
        SCENE_PERSON,
        SCENE_MODIFY,
        SCENE_GAME,
        SCENE_GAME_EX,
        SCENE_GAME_OFF,
        SCENE_VIP
    };

    public class UIManager : MonoBehaviour
    {
        private static UIManager instance = null;

//        private GameObject o_login = null;
//        private GameObject o_reg = null;
//        private GameObject o_quick = null;
//        private GameObject o_server = null;
//        private GameObject o_task = null;
//        private GameObject o_shop = null;
//        private GameObject o_help = null;
//        private GameObject o_person = null;
//        private GameObject o_modify = null;
//        private GameObject o_rank = null;
        private GameObject o_game = null;
        private GameObject o_game_ex = null;
//        private GameObject o_game_off = null;

        private enSceneType _SceneType = enSceneType.SCENE_NONE;

        public static UIManager Instance
        {
            get
            {
                return instance;
            }
        }

        public enSceneType SceneType
        {
            get { return _SceneType; }
        }

        private void Start()
        {
            //创建所有UI对象
//            o_quick = GameObject.Find("scene_quick");
//            o_reg = GameObject.Find("scene_reg");
//            o_login = GameObject.Find("scene_login");
//            o_server = GameObject.Find("scene_server");
//            o_task = GameObject.Find("scene_task");
//            o_shop = GameObject.Find("scene_shop");

//            o_help = GameObject.Find("scene_help");
//            o_person = GameObject.Find("scene_person");
//            o_modify = GameObject.Find("scene_modify");

            o_game = GameObject.Find("scene_game");
            o_game_ex = GameObject.Find("scene_game_ex");
//            o_game_off = GameObject.Find("scene_game_off");


            if (GameEngine.Instance.GameID == 1003)
            {
                GoUI(enSceneType.SCENE_NONE, enSceneType.SCENE_GAME_EX);
            }
            else
            {
                GoUI(enSceneType.SCENE_NONE, enSceneType.SCENE_GAME);
            }

        }

        private void Update()
        {

        }

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void GoUI(enSceneType stFrom, enSceneType stTo)
        {
            HideAllUI();

            _SceneType = stTo;

            switch (stTo)
            {
                case enSceneType.SCENE_GAME:
                    {
                        o_game.SetActive(true);
                        o_game.GetComponent<UIGame>().Init();
                        break;
                    }
                case enSceneType.SCENE_GAME_EX:
                    {
                        o_game_ex.SetActive(true);
                        o_game_ex.GetComponent<UIGameEx>().Init();
                        break;
                    }
            }
        }

        private void HideAllUI()
        {
//            o_quick.SetActive(false);
//            o_login.SetActive(false);
//            o_reg.SetActive(false);
//            o_task.SetActive(false);
//            o_shop.SetActive(false);
//            o_modify.SetActive(false);
//            o_help.SetActive(false);
//            o_person.SetActive(false);
//            o_server.SetActive(false);
            o_game.SetActive(false);
            o_game_ex.SetActive(false);
//            o_game_off.SetActive(false);

            ClearAllMsgBox();
        }

        public void ClearAllMsgBox()
        {
//            UIConfirmBox.Instance.Show(false, "");
            UIMsgBox.Instance.Show(false, "");
            UILoading.Instance.Show(false);
            UIEffect.Instance.ClearAllEffect();
            UIWaiting.Instance.Show(false);
            UIExitBox.Instance.Show(false);
            UISetting.Instance.Show(false);
            UIInfoBox.Instance.Show(false, "");
        }


    }
}