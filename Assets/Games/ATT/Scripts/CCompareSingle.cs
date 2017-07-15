using UnityEngine;
using System.Collections;
namespace com.QH.QPGame.ATT
{
    public class CCompareSingle : MonoBehaviour
    {
        public CAnimatonManger m_cAnimation;
        public CAnimatonManger m_cSprite;
        public GameObject m_gLabel;
        private int _iScore;
        public GameObject m_gBg;

        public bool m_bIsCurrentReward = false;
        private float m_fWorkingTime = 0;
        public float m_fSpeed = 0.5f;
        public int m_iScore
        {
            get { return _iScore; }
            set
            {
                _iScore = value;
                m_gLabel.GetComponent<CLabelNum>().m_iNum = _iScore;
            }
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_fWorkingTime += Time.deltaTime;
            if (m_bIsCurrentReward && m_fWorkingTime >= m_fSpeed)
            {
                bool bActive = m_gLabel.active;
                m_gLabel.SetActive(!bActive);
                m_fWorkingTime = 0;
            }
            
        }
    }
}
