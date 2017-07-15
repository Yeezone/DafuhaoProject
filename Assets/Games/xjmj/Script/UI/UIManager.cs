using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.XZMJ
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

    public class UIManager : MonoBehaviour
    {
        static UIManager instance = null;
        GameObject o_quick = null;
        GameObject o_login = null;
        GameObject o_reg = null;
        GameObject o_server = null;
        GameObject o_game = null;
        GameObject o_single = null;
        GameObject o_task = null;
        GameObject o_shop = null;

        GameObject o_help = null;

        GameObject o_person = null;
        GameObject o_modify = null;

		GameObject o_msgbox=null;

        enSceneType _SceneType = enSceneType.SCENE_NONE;

        public static UIManager Instance
        {
            get
            {
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
            o_quick = GameObject.Find("scene_quick");
            o_reg = GameObject.Find("scene_reg");
            o_server = GameObject.Find("scene_server");

            o_game = GameObject.Find("scene_game");
            o_login = GameObject.Find("scene_login");

            o_task = GameObject.Find("scene_task");
            o_shop = GameObject.Find("scene_shop");

            o_help = GameObject.Find("scene_help");
            o_person = GameObject.Find("scene_person");
            o_modify = GameObject.Find("scene_modify");

			o_msgbox=GameObject.Find ("scene_msgbox");
            GoUI(enSceneType.SCENE_NONE, enSceneType.SCENE_LOGIN);

        }

        void Update()
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
            _SceneType = enSceneType.SCENE_GAME;
            o_game.SetActive(true);
            o_game.GetComponent<UIGame>().Init();
        }

        void HideAllUI()
        {
            //o_quick.SetActive(false);
           // o_task.SetActive(false);
           // o_shop.SetActive(false);
           // o_modify.SetActive(false);
           // o_reg.SetActive(false);
           // o_server.SetActive(false);

            o_game.SetActive(false);
           // o_login.SetActive(false);
            //o_help.SetActive(false);
           // o_person.SetActive(false);


            ClearAllMsgBox();
        }
        public void ClearAllMsgBox()
        {
            UIConfirmBox.Instance.Show(false, "");
            UIMsgBox.Instance.Show(false, "");
            UILoading.Instance.Show(false);
            UIEffect.Instance.ClearAllEffect();
            UIWaiting.Instance.Show(false);
            UIExitBox.Instance.Show(false);
            UISetting.Instance.Show(false);
            UIInfoBox.Instance.Show(false, "");
			UIMsgBox.Instance.Show(false,"");
        }


    }
}