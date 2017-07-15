using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.SHZ
{

    [AddComponentMenu("Custom/Controls/Number")]
    public class UINumber : MonoBehaviour
    {

        public string NumPrefabs = null;
        public int BaseDepth = 300;
        public int ColSpace = 16;
        public int Height = 39;
        public int Width = 21;

        //--------------------------------------------------------------------------
        long _number = 0;
        List<GameObject> _numlist = new List<GameObject>();

        void Start()
        {

        }


        void FixedUpdate()
        {

        }

        public void SetNumber(long num)
        {

            _number = System.Math.Abs(num);
            foreach (GameObject obj in _numlist)
            {
                Destroy(obj);
            }
            _numlist.Clear();



            string strNum = _number.ToString();
			int strLen = strNum.Length;
            int i = 0;
            foreach (char c in strNum.ToCharArray())
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load(NumPrefabs));
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