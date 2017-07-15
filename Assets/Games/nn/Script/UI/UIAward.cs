using Shared;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.NN
{

    public class UIAward : MonoBehaviour
    {
        //

        static UIAward instance = null;
        GameObject o_Dlg = null;
        GameObject o_loading = null;
        static bool _bShow = false;


        static int _nTick = 0;

        public static UIAward Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIAward").AddComponent<UIAward>();
                }
                return instance;
            }
        }

        void Awake()
        {
            o_Dlg = GameObject.Find("scene_server/dlg_award");
            o_loading = GameObject.Find("scene_server/dlg_award/sp_msg");
            if (instance == null)
            {
                instance = this;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        void Start()
        {

        }

        void FixedUpdate()
        {

            if (_bShow)
            {
                if (System.Environment.TickCount - _nTick > 80)
                {

                    _nTick = System.Environment.TickCount;
                }
            }


        }


        public void Show(bool bshow)
        {
            o_Dlg.SetActive(bshow);
            ShowWaitting(false);
        }
        public void ShowWaitting(bool bshow)
        {
            if (bshow)
            {
                o_loading.SetActive(true);
                _nTick = System.Environment.TickCount;
            }
            else
            {
                o_loading.SetActive(false);
                _nTick = 0;
            }
            _bShow = bshow;
        }

        void OnCloseIvk()
        {
            Show(false);
        }


    }
}