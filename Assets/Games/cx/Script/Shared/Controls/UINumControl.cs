using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.CX
{

    public class UINumControl : MonoBehaviour
    {
        public GameObject prefab;
        //public const string NumberPath = "CX_Prefabs/Number";

        private GameObject numPrefab = null;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        GameObject GetNumPrefab()
        {
            if (numPrefab == null)
            {
                numPrefab = prefab;
            }
            return GameObject.Instantiate(numPrefab);
        }

        public void SetNum(long number)
        {
            ClearNum();
            List<long> numList = new List<long>();
            while (number != 0)
            {
                numList.Add(number % 10);
                number /= 10;
            }
            for (int i = numList.Count; i > 0;--i )
            {
                GameObject pre = GetNumPrefab();
                pre.SetActive(true);
                pre.transform.SetParent(transform, false);
                UISprite sp = pre.GetComponent<UISprite>();
                if (sp != null)
                {
                    sp.spriteName = numList[i - 1].ToString();
                    sp.depth = 380 + i + 300;//这里将层级增加300，桌子上最多会有300个金币，加上3个底就303个，防止被金币遮住
                }
                pre.name = numList[i-1].ToString();
            }
            transform.GetComponent<UIGrid>().enabled = true;
        }

        public void ClearNum()
        {
            DestoryChild();
        }

        void DestoryChild()
        {
            while(transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
