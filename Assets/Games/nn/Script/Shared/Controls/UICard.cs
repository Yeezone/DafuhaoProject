//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

namespace com.QH.QPGame.NN
{

    /// <summary>
    /// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
    /// </summary>

    [AddComponentMenu("Custom/Controls/Card")]
    public class UICard : MonoBehaviour
    {
        public Transform tweenTarget;
        public Vector3 shoot = new Vector3(0, 20f, 0);
        public float duration = 0.2f;
        public bool recvclick = true;

        Vector3 mPos = Vector3.zero;
        bool mInitDone = false;
        bool mSelected = false;

        Vector3 _first = Vector3.zero;
        Vector3 _second = Vector3.zero;
        static bool _optseled = false;
        public bool OptSeled
        {
            get
            {
                return _optseled;
            }
            set
            {
                _optseled = value;
            }
        }

        public bool Selected
        {
            get
            {
                return mSelected;
            }
        }
        byte mCardData = 0;
        public byte CardData
        {
            get
            {
                return mCardData;
            }
            set
            {
                mCardData = value;
            }
        }
        void Start()
        {
            mSelected = false;
        }

        public void SetPos(Vector3 v)
        {
            mPos = v;
        }

        void Init()
        {
            mInitDone = true;
            mSelected = false;
            if (tweenTarget == null)
                tweenTarget = transform;
            mPos = tweenTarget.localPosition;
        }

        public void OnClick()
        {
            if (recvclick == false) return;
            if (!mInitDone) Init();

            mSelected = !mSelected;
            if (mSelected)
            {

                transform.localPosition = mPos + shoot;
            }
            else
            {
                transform.localPosition = mPos;
            }
        }
        public void SetShoot(bool bselected)
        {
            if (recvclick == false) return;
            if (!mInitDone) Init();
            mSelected = bselected;

            if (mSelected)
            {
                transform.localPosition = mPos + shoot;
            }
            else
            {
                transform.localPosition = mPos;
            }

        }

        public bool GetShoot()
        {
            return mSelected;
        }
        public void SetMask(bool bmask)
        {
            if (recvclick == false) return;
            if (bmask)
            {
                TweenColor.Begin(gameObject, 0, new Color(0.9f, 0.9f, 0.9f));
            }
            else
            {
                TweenColor.Begin(gameObject, 0, Color.white);
            }
        }
    }
}