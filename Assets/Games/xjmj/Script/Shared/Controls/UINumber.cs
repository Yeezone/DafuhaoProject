using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace com.QH.QPGame.XZMJ
{

    [AddComponentMenu("Custom/Controls/Number")]
    public class UINumber : MonoBehaviour
    {

        public GameObject NumPrefabs;
        public int BaseDepth = 300;
        public int ColSpace = 16;
        public int Height = 26;
        public int Width = 14;

        //--------------------------------------------------------------------------
        Int64 _number = 0;
        List<GameObject> _numlist = new List<GameObject>();

        void Start()
        {

        }


        void FixedUpdate()
        {

        }

        public void SetNumber(Int64 num)
        {

            _number = System.Math.Abs(num);
            foreach (GameObject obj in _numlist)
            {
                Destroy(obj);
            }
            _numlist.Clear();



            string strNum = _number.ToString();

            int i = 0;
            foreach (char c in strNum.ToCharArray())
            {
                GameObject obj =Instantiate(NumPrefabs);
                obj.transform.parent = transform;

                UISprite sp = obj.GetComponent<UISprite>();
                sp.transform.localScale = new Vector3(Width, Height, 1);
                sp.depth = BaseDepth + i;
                sp.spriteName = sp.spriteName.Substring(0, sp.spriteName.Length - 1) + c.ToString();
                sp.transform.localPosition = new Vector3(ColSpace * i, 0, 1);
                _numlist.Add(obj);
                i++;
            }


        }
    }

}