using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.JSYS
{
    public class CMoveBet : MonoBehaviour
    {

        /// <summary>
        /// 展开坐标
        /// </summary>
        public Vector3 m_vPackUpPos;
        /// <summary>
        /// 隐藏坐标
        /// </summary>
        public Vector3 m_vHidePos;

        public bool m_bIsPackUp = false;

        public GameObject m_gMoveObj;

        public float m_fMoveTime = 0.5f;
        // Use this for initialization
        void Start()
        {
            m_bIsPackUp = false;
            m_gMoveObj.transform.localPosition = m_vHidePos;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void PackDown_Onclick()
        {
 #if !UNITY_STANDALONE_WIN
           CBETManger._instance.m_gUpBT.SetActive(true);
           CBETManger._instance.m_gDownBT.SetActive(false);
#endif

            m_bIsPackUp = false;
            m_gMoveObj.GetComponent<TweenPosition>().style = UITweener.Style.Once;
            m_gMoveObj.GetComponent<TweenPosition>().from = m_gMoveObj.transform.localPosition;
            m_gMoveObj.GetComponent<TweenPosition>().to = m_vHidePos;
            m_gMoveObj.GetComponent<TweenPosition>().duration = m_fMoveTime;
            m_gMoveObj.GetComponent<TweenPosition>().ResetToBeginning();
            m_gMoveObj.GetComponent<TweenPosition>().enabled = true;
        }
        public void PackUp_Onclick()
        {
#if !UNITY_STANDALONE_WIN
            CBETManger._instance.m_gUpBT.SetActive(false);
            CBETManger._instance.m_gDownBT.SetActive(true);
#endif
            m_bIsPackUp = true;
            m_gMoveObj.GetComponent<TweenPosition>().style = UITweener.Style.Once;
            m_gMoveObj.GetComponent<TweenPosition>().from = m_gMoveObj.transform.localPosition;
            m_gMoveObj.GetComponent<TweenPosition>().to = m_vPackUpPos;
            m_gMoveObj.GetComponent<TweenPosition>().duration = m_fMoveTime;
            m_gMoveObj.GetComponent<TweenPosition>().ResetToBeginning();
            m_gMoveObj.GetComponent<TweenPosition>().enabled = true;
        }
    }
}