using UnityEngine;
using System.Collections;
using System;



namespace com.QH.QPGame.XZMJ
{

    public class UIDownBox : MonoBehaviour
    {
        //
        static UIDownBox instance = null;
        //
        GameObject o_MsgBox = null;
        //
        GameObject o_Msg = null;
        //
        int _TickCount = 0;
        //
        bool _bShow = false;
        //
        int _TotalPrg = 0;
        //
        void Start()
        {

        }

        void Update()
        {
        }

        void FixedUpdate()
        {
            if ((Environment.TickCount - _TickCount) > 1000 && _bShow == true)
            {

                _TotalPrg++;

                if (_TotalPrg >= 99)
                {
                    _TotalPrg = 99;
                }

                string str = "正在下载更新包,请耐心等待\n\n" + _TotalPrg.ToString() + "%";

                o_Msg.GetComponent<UILabel>().text = str;

                _TickCount = Environment.TickCount;
            }
        }
        void Awake()
        {
            o_MsgBox = GameObject.Find("scene_downbox");
            o_Msg = GameObject.Find("scene_downbox/lbl_msgbox_msg");
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
        public void Show(bool bshow, string str)
        {
            if (bshow)
            {
                o_MsgBox.SetActive(true);
                _TickCount = Environment.TickCount;
                _bShow = true;
                _TotalPrg = 0;
                o_Msg.GetComponent<UILabel>().text = str;


            }
            else
            {
                _bShow = false;
                o_Msg.GetComponent<UILabel>().text = "";
                o_MsgBox.SetActive(false);
                _TotalPrg = 0;

            }

        }


        //
        public static UIDownBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIDownBox").AddComponent<UIDownBox>();
                }
                return instance;
            }
        }

    }
}