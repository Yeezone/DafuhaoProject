using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.XZMJ
{
    public class UIMsgBox : MonoBehaviour
    {
        //
        static UIMsgBox instance = null;
        //
        GameObject o_MsgBox = null;
        //
        GameObject o_Msg = null;
        //
        int _TickCount = 0;

        bool _bShow = false;
        void Start()
        {

        }

        void Update()
        {
        }

        void FixedUpdate()
        {
            if ((Environment.TickCount - _TickCount) > 3000 && _bShow == true)
            {
                _bShow = false;
                Show(false, "");

            }
        }
        void Awake()
        {
            o_MsgBox = GameObject.Find("scene_msgbox");
            o_Msg = GameObject.Find("scene_msgbox/lbl_msgbox_msg");
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
				o_MsgBox.SetActive(true);
                o_Msg.GetComponent<UILabel>().text = strMsg;
//                TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2, -100));
//				Debug.LogError(Screen.height/2);
            }
            else
            {
                _bShow = false;
				o_MsgBox.SetActive(false);
//                o_Msg.GetComponent<UILabel>().text = strMsg;
//                TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2+90, -100));

            }

        }
        //
        public static UIMsgBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIMsgBox").AddComponent<UIMsgBox>();
                }
                return instance;
            }
        }

    }
}