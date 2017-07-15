using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.BJL
{

    [ExecuteInEditMode]
    [AddComponentMenu("Custom/Controls/CardControl")]

    public class UIChipControl : MonoBehaviour
    {
//		public string ChipPrefabs = "LHD_Prefabs/Chip";

		public GameObject ChipPrefabs;
        public int BaseDepth = 10;
        public float Duration = 0.3f;
        public GameObject TotalLabel = null;

		public GameObject Area_Chips = null;

     //   public GameObject TotalDesc = null;
		Bounds bounds;

        List<GameObject> _chiplist = new List<GameObject>();
		Vector3 vec;
        int _totalchip = 0;

        void Start()
        {
			vec = Area_Chips.transform.position;
        }

        void Update()
        {

        }

        public void ClearChips()
        {
			foreach (GameObject chip in _chiplist)
			{
				Destroy(chip);
			}
			_chiplist.Clear();
			_totalchip = 0;
        }

		public void AddChips(byte bViewID, long nUserChip, byte chipArea , Vector3 point) 
		{
			int nChipCount = _chiplist.Count;
			float x,y,x1,x2,y1,y2,z;

			//计算筹码
			GameObject obj = Instantiate( ChipPrefabs, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
//			GameObject obj = (GameObject)Instantiate (ChipPrefabs);
			obj.transform.parent = transform;
			obj.transform.localScale = new Vector3 (1, 1, 1);
			obj.GetComponent<UISprite>().depth = BaseDepth;

			if( bViewID != (byte)GameEngine.Instance.MySelf.DeskStation)
			{
				bounds = NGUIMath.CalculateRelativeWidgetBounds(Area_Chips.transform);
				x = UnityEngine.Random.Range(0,bounds.size.x)-bounds.size.x/2;
				y = UnityEngine.Random.Range(0,bounds.size.y/2)-bounds.size.y/3;

				vec = new Vector3(x,y,0);
				obj.transform.localPosition = vec;

			}else
			{
				obj.transform.position = point;
			}

//			for(int i=1; i<4; i++)
//			{
//				obj.transform.FindChild("effect"+i).GetComponent<UISprite>().spriteName = "blank";
//			}

			switch (nUserChip) 
			{
			case 1:
				obj.GetComponent<UISprite>().spriteName = "chip_1";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 5:
				obj.GetComponent<UISprite>().spriteName = "chip_5";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 10:
				obj.GetComponent<UISprite>().spriteName = "chip_10";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 50:
				obj.GetComponent<UISprite>().spriteName = "chip_50";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 100:
				obj.GetComponent<UISprite>().spriteName = "chip_100";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 500:
				obj.GetComponent<UISprite>().spriteName = "chip_500";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 1000:
				obj.GetComponent<UISprite>().spriteName = "chip_1000";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 5000:
				obj.GetComponent<UISprite>().spriteName = "chip_5000";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 10000:
				obj.GetComponent<UISprite>().spriteName = "chip_1w";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 50000:
				obj.GetComponent<UISprite>().spriteName = "chip_5w";
				obj.transform.FindChild("effect1").gameObject.SetActive(true);
				break;
			case 100000:
				obj.GetComponent<UISprite>().spriteName = "chip_10w";
				obj.transform.FindChild("effect2").gameObject.SetActive(true);
				break;
			case 500000:
				obj.GetComponent<UISprite>().spriteName = "chip_50w";
				obj.transform.FindChild("effect2").gameObject.SetActive(true);
				break;
			case 1000000:
				obj.GetComponent<UISprite>().spriteName = "chip_100w";
				obj.transform.FindChild("effect3").gameObject.SetActive(true);
				break;
			}
			_chiplist.Add (obj);
		}

        public void WinChips(byte bViewID)
        {

        }
    }
}