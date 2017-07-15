using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.DDZ
{
    public class UIMsgBox : MonoBehaviour
    {
        //
        private static UIMsgBox instance = null;
        //
        private GameObject o_MsgBox = null;
        //
        private GameObject o_Msg = null;
        //
        private int _TickCount = 0;

        private bool _bShow = false;

        private void Start()
        {

        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            if ((Environment.TickCount - _TickCount) > 5000 && _bShow == true)
            {
                _bShow = false;
                Show(false, "");

            }
        }

        private void Awake()
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
			Debug.Log(strMsg);

            if (bshow && strMsg != "")
            {
                _TickCount = Environment.TickCount;
                _bShow = true;
                o_Msg.GetComponent<UILabel>().text = strMsg;
//                TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, 218, -100));
				o_MsgBox.transform.localScale = Vector2.one;
            }
            else
            {
                _bShow = false;
                o_Msg.GetComponent<UILabel>().text = strMsg;
//                TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, 280, -100));
				o_MsgBox.transform.localScale = Vector2.zero;
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