using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    public class UIExitBox : MonoBehaviour
    {
        //
        private static UIExitBox instance = null;
        //
        private ConfirmCall _ConfirmCall = null;
        //
        private CancelCall _CancelCall = null;
        //
        private GameObject o_MsgBox = null;


        private void Start()
        {

        }

        //private void Update()
        //{

        //}

        private void Awake()
        {
            o_MsgBox = GameObject.Find("scene_exit");
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
        public void Show(bool bshow)
        {
            if (bshow)
            {
                o_MsgBox.SetActive(bshow);
            }
            else
            {
                o_MsgBox.SetActive(bshow);
            }
        }

        //
        public ConfirmCall ConfirmCallBack
        {
            set { _ConfirmCall = value; }
        }

        //
        public CancelCall CancelCallBack
        {
            set { _CancelCall = value; }
        }

        //
        public static UIExitBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIExitBox").AddComponent<UIExitBox>();
                }
                return instance;
            }
        }

        //
        private void OnOkIvk()
        {

            if (_ConfirmCall != null)
                _ConfirmCall();

            Show(false);

            _ConfirmCall = null;
            _CancelCall = null;
        }

        //
        private void OnCancelIvk()
        {

            if (_CancelCall != null)
                _CancelCall();

            Show(false);

            _ConfirmCall = null;
            _CancelCall = null;
        }

    }
}