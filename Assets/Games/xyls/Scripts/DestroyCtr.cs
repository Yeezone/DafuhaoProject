using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class DestroyCtr : MonoBehaviour
    {

        public float m_fDestroyTime = 2.0f;

        private float m_fTimer = 0;

        void Start()
        {
            //Destroy(this.gameObject, m_fDestroyTime);
        }


        void Update()
        {
            m_fTimer += Time.deltaTime;
            // 如果超时,把自己隐藏.
            if (m_fTimer >= m_fDestroyTime)
            {
                this.gameObject.SetActive(false);
                m_fTimer = 0;
            }
        }
    }
}
