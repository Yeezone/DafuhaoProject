using UnityEngine;
using System.Collections;


namespace com.QH.QPGame.XZMJ
{


    public delegate void ConfirmCall();
    public delegate void CancelCall();

    public class UIConfirmBox : MonoBehaviour
    {
        //
        static UIConfirmBox instance = null;
        //
        ConfirmCall _ConfirmCall = null;
        //
        CancelCall _CancelCall = null;
        //
        GameObject o_MsgBox = null;
        //
        GameObject o_Msg = null;


        void Start()
        {

        }

        void Update()
        {

        }
        void Awake()
        {
            o_MsgBox = GameObject.Find("scene_confirmbox");
            o_Msg = GameObject.Find("scene_confirmbox/lbl_confirm_msg");
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
                o_MsgBox.SetActive(bshow);
                o_Msg.GetComponent<UILabel>().text = strMsg;
            }
            else
            {
                o_Msg.GetComponent<UILabel>().text = strMsg;
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
        public static UIConfirmBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIConfirmBox").AddComponent<UIConfirmBox>();
                }
                return instance;
            }
        }
        //
        void OnOkIvk()
        {

            if (_ConfirmCall != null)
                _ConfirmCall();

            Show(false, "");

            _ConfirmCall = null;
            _CancelCall = null;
        }
        //
        void OnCancelIvk()
        {

            if (_CancelCall != null)
                _CancelCall();

            Show(false, "");

            _ConfirmCall = null;
            _CancelCall = null;
        }

    }
}