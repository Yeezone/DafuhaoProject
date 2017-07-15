using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.JSYS
{
    public class CPlayGame : MonoBehaviour
    {

        public List<GameObject> m_SpriteList = new List<GameObject>();
        public float m_fX = -310.2f;
        public int num = 0;
        // Use this for initialization
        void Start()
        {
            num = 0;
        }

        // Update is called once per frame
        void Update()
        {
            
            if (num < 60)
            {
                num++;
                for (int i = 0; i < m_SpriteList.Count; i++)
                {
                    Vector3 pos = m_SpriteList[i].transform.localPosition;
                   // pos.x = -310.2f + i * 70.0f;
                  //  m_SpriteList[i].transform.localPosition = pos;
                }
            }

        }
    }
}
