using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    public delegate void WaitingCancelCall();

    public class UIWaiting : MonoBehaviour
    {
        private static UIWaiting instance = null;
        //
        private GameObject o_loading_panel = null;
        private GameObject o_loading_wait = null;
        private GameObject o_btnQuit = null;

        private WaitingCancelCall _Call = null;

        private static bool _bShow = false;
        private static int _nTick = 0;
        private static int _nTotalTime = 0;
        private static int _nLimit = 0;
        //static int   _nQuitDelay    = 0;
        // static bool  _bReqQuit      = false;
        public WaitingCancelCall CallBack
        {
            set { _Call = value; }
        }

        private void Start()
        {

        }

        public bool IsShow
        {
            get { return _bShow; }
        }

        private void FixedUpdate()
        {
            if (_bShow)
            {
                if (System.Environment.TickCount - _nTick > 100)
                {
                    _nTick = System.Environment.TickCount;
                    o_loading_wait.transform.Rotate(0, 0, -5);

                }

                if ((System.Environment.TickCount - _nTotalTime) > _nLimit*1000 && _nLimit > 0)
                {
                    Show(false);
                    UIMsgBox.Instance.Show(true, GameMsg.MSG_CM_009);
                }
            }

        }

        private void Awake()
        {
            o_loading_panel = GameObject.Find("scene_waiting");
            o_loading_wait = GameObject.Find("scene_waiting/sp_wait");
            o_btnQuit = GameObject.Find("scene_waiting/btn_cancel");
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
                o_btnQuit.SetActive(false);
                Invoke("ShowCancelButton", 8.0f);
            }
            else
            {
                o_loading_panel.SetActive(false);
                _nTick = 0;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = 0;
                CancelInvoke();
                o_btnQuit.SetActive(false);

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
                _nLimit = 0;
                o_btnQuit.SetActive(false);
                Invoke("ShowCancelButton", 8.0f);

            }
            else
            {
                o_loading_panel.SetActive(false);
                _nTick = 0;
                _nTotalTime = System.Environment.TickCount;
                _nLimit = 0;
                CancelInvoke();
                o_btnQuit.SetActive(false);
            }
            _bShow = bshow;

        }

        private void ShowCancelButton()
        {
            o_btnQuit.SetActive(true);
        }

        private void OnBtnCancelIvk()
        {
            o_btnQuit.SetActive(false);
            //GameEngine.Instance.AddTimeEventHandle(TimeEvents.REG_GAME_ENTER,new TimeEventHandle(OnRoomEnterGame));
            //    GameEngine.Instance.MyUser.SendUserQuitGameReq();

            Invoke("OnDelayQuit", 1.0f);

        }

        private void OnDelayQuit()
        {
            //PlayerPrefs.SetInt("UsedServ", 0);

            if (_Call != null)
            {
                _Call();
            }
        }

        //
        public static UIWaiting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIWaiting").AddComponent<UIWaiting>();
                }
                return instance;
            }
        }

    }
}