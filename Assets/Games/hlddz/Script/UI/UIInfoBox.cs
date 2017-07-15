using UnityEngine;
using System;

namespace com.QH.QPGame.DDZ
{
    public class UIInfoBox : MonoBehaviour
    {
        //
        private static UIInfoBox instance = null;
        //
        private GameObject o_MsgBox = null;
        //
        private GameObject o_Msg = null;
        //
        private int _TickCount = 0;
        //
        private bool _bShow = false;
        //
        private void Start()
        {

        }

        //private void Update()
        //{
        //}

        private void FixedUpdate()
        {
            if ((Environment.TickCount - _TickCount) > 2000 && _bShow == true)
            {
                _bShow = false;
                Show(false, "");
            }
        }

        private void Awake()
        {
            o_MsgBox = GameObject.Find("scene_infobox");
            o_Msg = GameObject.Find("scene_infobox/lbl_msgbox_msg");
            if (instance == null)
            {
                instance = this;
            }

        }

        private void OnDestroy()
        {
            instance = null;
        }

        //
        public void Show(bool bshow, string strMsg)
        {
            if (bshow)
            {
                _TickCount = Environment.TickCount;
                _bShow = true;
                o_Msg.GetComponent<UILabel>().text = strMsg;
                TweenScale.Begin(gameObject, 0.2f, new Vector3(1, 1, 1));

            }
            else
            {
                _bShow = false;
                o_Msg.GetComponent<UILabel>().text = strMsg;
                TweenScale.Begin(gameObject, 0.2f, new Vector3(0, 0, 0));

            }

        }

        //
        public static UIInfoBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIInfoBox").AddComponent<UIInfoBox>();
                }
                return instance;
            }
        }

    }
}