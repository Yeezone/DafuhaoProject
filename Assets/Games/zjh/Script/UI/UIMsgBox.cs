using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.ZJH
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
			o_MsgBox.SetActive(true);
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
            if (bshow && strMsg != "")
            {
		
                _TickCount = Environment.TickCount;
                _bShow = true;
				o_MsgBox.SetActive(true);
                o_Msg.GetComponent<UILabel>().text = strMsg;
//				if(UIManager.Instance.curGamePlatform==GamePlatform.ZJH_ForPC)
//				{
//					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height-87, -100));
//				}else{
//					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2+30, -100));
//				}
//                
            }
            else
            {
                _bShow = false;
                o_MsgBox.SetActive(false);
                /*
				o_Msg.GetComponent<UILabel>().text= strMsg;
				if(UIManager.Instance.curGamePlatform==GamePlatform.ZJH_ForPC)
				{
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height+40, -100));
				}else{
					TweenPosition.Begin(gameObject, 0.5f, new Vector3(10, Screen.height/2+120, -100));
				}
                */

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