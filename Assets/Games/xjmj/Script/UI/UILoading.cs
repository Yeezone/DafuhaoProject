using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.XZMJ
{
    public class UILoading : MonoBehaviour
    {
        static UILoading instance = null;
        //
        GameObject o_loading_panel = null;
        GameObject o_loading_wait = null;

        static bool _bShow = false;
        static int _nTick = 0;
        static int _nTotalTime = 0;
        static int _nLimit = 0;
        void Start()
        {

        }

        public bool IsShow
        {
            get { return _bShow; }
        }
        void FixedUpdate()
        {
            if (_bShow)
            {
                if (System.Environment.TickCount - _nTick > 100)
                {
                    _nTick = System.Environment.TickCount;
                    //o_loading_wait.transform.RotateAround(Vector3.zero,Vector3.back,5);
                    o_loading_wait.transform.Rotate(0, 0, -5);

                }

                if ((System.Environment.TickCount - _nTotalTime) > _nLimit * 1000 && _nLimit > 0)
                {
                    Show(false);
                    UIInfoBox.Instance.Show(true, GameMsg.MSG_CM_009);
                }
            }

        }

        void Awake()
        {
            o_loading_panel = GameObject.Find("scene_loading");
            o_loading_wait = GameObject.Find("scene_loading/sp_wait");
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
        public void Show(bool bshow, int ntime)
        {
            if (bshow)
            {
                o_loading_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = ntime;
            }
            else
            {
                o_loading_panel.SetActive(false);
                _nTick = 0;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = 0;
            }
            _bShow = bshow;

        }
        public void Show(bool bshow)
        {
            if (bshow)
            {
                o_loading_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = 15;
            }
            else
            {
                o_loading_panel.SetActive(false);
                _nTick = 0;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = 0;
            }
            _bShow = bshow;

        }

        //
        public static UILoading Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UILoading").AddComponent<UILoading>();
                }
                return instance;
            }
        }

    }
}