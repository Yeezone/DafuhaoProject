using UnityEngine;
using System.Collections;


namespace com.QH.QPGame.XZMJ
{
    public class MJAnimationController : MonoBehaviour
    {

        public GameObject[] m_AnimationType;
        public int m_AnimationIndex = 0;
        private GameObject TweenObj;

        public float m_destroyTime = 0;
        private float TimeCount = 0;
        private bool StartTime = false;

        void Update()
        {
            if (StartTime)
            {
                TimeCount += Time.deltaTime;
                if (TimeCount >= m_destroyTime)
                {
                    Destroy(TweenObj);
                    StartTime = false;
                }
            }
        }

        public void Play()
        {
            TweenObj = Instantiate(m_AnimationType[m_AnimationIndex], Vector3.zero, Quaternion.identity) as GameObject;
            TweenObj.transform.parent = this.transform;
            TweenObj.transform.localPosition = Vector3.zero;
            TweenObj.transform.localScale = Vector3.one;
            TweenObj.transform.GetComponent<MJAnimation>().m_bIsPlay = true;
            TimeCount = 0;
            StartTime = true;
        }
    }
}
