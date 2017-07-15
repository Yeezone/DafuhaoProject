using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.WXHH
{

    [AddComponentMenu("Custom/Controls/Number")]
    public class UINumber : MonoBehaviour
    {
		public GameObject Target;
        public GameObject NumPrefabs;
        public int BaseDepth = 10;
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

			string strNum = "";
			int strLen = 0;
			int midNum = 0;

//			if(num<1000000)
//			{
	            strNum = _number.ToString();
			    strLen = strNum.Length;

				if( strLen%2 ==0)
				{
					midNum = strLen/2;
				}else
				{
					midNum = strLen/2 + 1;
				}

	            int i = 0;
				foreach (char c in strNum.ToCharArray())
				{
					if(midNum == 0) return;
					GameObject obj = Instantiate(NumPrefabs);
					obj.transform.parent = transform;
					
					UISprite sp = obj.GetComponent<UISprite>();
					sp.transform.localScale = new Vector3(Width, Height, 1);
					sp.depth = BaseDepth/* + i*/;
					sp.spriteName = sp.spriteName.Substring(0, sp.spriteName.Length - 1) + c.ToString();
					
					if( strLen%2 ==0)
					{
						sp.transform.localPosition = new Vector3(ColSpace * (i - midNum + 1 )- ColSpace*0.5f, 0, 1);
					}else
					{
						sp.transform.localPosition = new Vector3(ColSpace * (i - midNum + 1 ), 0, 1);
					}
					
					_numlist.Add(obj);
					i++;
				}

//			}else
//			{
//
//			}

        }
    }

}