using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.CX
{
    public class CxAnimationCotroller : MonoBehaviour
    {
        public GameObject TimeNumber;
        public GameObject TimeBg;

        void Update()
        {
            if (TimeNumber.transform.GetComponent<cx_number>().m_iNum == 5)
            {
                TimeNumber.SetActive(false);
                AnimationPlay();
            }
        }

        void AnimationPlay()
        {
            this.transform.GetComponent<CXAnimation>().enabled = true;
            this.transform.GetComponent<CXAnimation>().m_bIsPlay = true;
        }

        void OnDisable()
        {
            this.transform.GetComponent<CXAnimation>().RecetAnimation();
            TimeBg.transform.GetComponent<UISprite>().spriteName = "clock";
            this.transform.GetComponent<CXAnimation>().m_bIsPlay = false;
            this.transform.GetComponent<CXAnimation>().enabled = false;
            TimeNumber.SetActive(true);
        }
    }
}


