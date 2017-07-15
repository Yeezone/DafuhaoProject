using UnityEngine;
using System.Collections;
using Shared;

namespace com.QH.QPGame.SH
{
    public class UIHelp : MonoBehaviour
    {

        void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }


        void OnBackIvk()
        {
            //返回登陆界面
            UIManager.Instance.GoUI(enSceneType.SCENE_HELP, enSceneType.SCENE_SERVER);
        }

    }
}