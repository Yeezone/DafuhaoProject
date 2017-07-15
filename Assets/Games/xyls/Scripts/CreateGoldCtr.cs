using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class CreateGoldCtr : MonoBehaviour
    {
        public static CreateGoldCtr _instance;
        //
        public GameObject m_gGold;

        private float m_fTimer = 0;
        [HideInInspector]
        public bool m_bIsOpen = false;

        // Use this for initialization
        void Start()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            m_fTimer += Time.deltaTime;

            if (m_fTimer >= 0.1f && m_bIsOpen)
            {
                CreatGold();
                m_fTimer = 0;
            }
            //测试
//             if (Input.GetKeyDown(KeyCode.P))
//             {
//                 m_bIsOpen = true;
//                 m_fTimer = 0;
//             }
//             if (Input.GetKeyDown(KeyCode.S))
//             {
//                 m_bIsOpen = false;
//             }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        public void CreatGold()
        {
            float tempx = Random.RandomRange(-2.0f, 2.0f);
            float tempy = Random.RandomRange(6.5f, 7.0f);
            float tempz = Random.RandomRange(-2.0f, 2.0f);
            Quaternion rot = Quaternion.identity;
            rot.x = Random.RandomRange(0.0f, 180.0f);
            rot.y = Random.RandomRange(0.0f, 180.0f);
            rot.z = Random.RandomRange(0.0f, 180.0f);
            Object temp = Instantiate(m_gGold, new Vector3(tempx, tempy, tempz), rot);
            Destroy(temp, 2f);
        }
    }
}
