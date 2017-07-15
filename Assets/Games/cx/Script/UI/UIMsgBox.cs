using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.CX
{

    public class UIMsgBox : MonoBehaviour
    {
        //
        private static UIMsgBox instance = null;
        //
        private GameObject o_MsgBox = null;

        private GameObject o_Msg = null;
        //
        int _TickCount = 0;

		public GamePlatform platform;

		Vector3 vec = new Vector3 (0,0,0);

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
            instance = this;
            o_MsgBox = GameObject.Find("scene_msgbox");
            o_Msg = GameObject.Find("scene_msgbox/lbl_msgbox_msg");
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
				vec = o_Msg.transform.position;

				if(platform == GamePlatform.NN_ForPC)
				{
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2-50, -100));
				}
                else
                {
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2-100, -100));

				}
            }
            else
            {
                _bShow = false;
                o_Msg.GetComponent<UILabel>().text = strMsg;
				if(platform == GamePlatform.NN_ForPC)
				{
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2+100, -100));
				}
                else
                {
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2+120, -100));

				}
            }
        }
        //
        public static UIMsgBox Instance
        {
            get
            {
                return instance;
            }
        }

    }
}