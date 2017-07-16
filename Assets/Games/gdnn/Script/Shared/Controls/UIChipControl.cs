using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.GDNN
{
	[ExecuteInEditMode]
	[AddComponentMenu ("Custom/Controls/CardControl")]
	
	public class UIChipControl : MonoBehaviour
	{
		public GameObject ChipPrefabs;
		//金币
		public GameObject Area_Chips = null;
		//金币生成区域
		public float distance = 80;
		//其他玩家下注区域

		List<GameObject> m_chipList = new List<GameObject> ();
		//储存玩家金币

		public int m_baseDepth = 10;
		//层级

		void Start ()
		{

		}

		//清除筹码列表
		public void ClearChips ()
		{
			foreach (GameObject chip in m_chipList) {
				Destroy (chip);
			}
			m_chipList.Clear ();
		}

		//生成筹码
		public void AddChips (byte bViewID, long nUserChip, byte chipArea, Vector3 point)
		{
			float x = 0;
			float y = 0;

			//生成筹码
			GameObject obj = Instantiate (ChipPrefabs, Vector3.zero, Quaternion.Euler (new Vector3 (0f, 0f, 0f)))as GameObject;

			//设置筹码属性
			obj.transform.parent = this.transform;
			obj.transform.localScale = new Vector3 (1, 1, 1);
			obj.GetComponent<UISprite> ().depth = m_baseDepth;

			//设置筹码位置（其他玩家）
			if (bViewID != (byte)GameEngine.Instance.MySelf.DeskStation) {
				//生成包围盒变量储存物体大小
				//Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(Area_Chips.transform);

				x = UnityEngine.Random.Range (-(int)Area_Chips.transform.localScale.x * distance, (int)Area_Chips.transform.localScale.x * distance);
				y = UnityEngine.Random.Range (-(int)Area_Chips.transform.localScale.y * distance, (int)Area_Chips.transform.localScale.y * distance);

				obj.transform.localPosition = new Vector3 (x, y, 0);
			} else {
				obj.transform.position = point;
			}

			switch (nUserChip) {
			case 1:
				obj.GetComponent<UISprite> ().spriteName = "chip_1";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 5:
				obj.GetComponent<UISprite> ().spriteName = "chip_5";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 10:
				obj.GetComponent<UISprite> ().spriteName = "chip_10";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 50:
				obj.GetComponent<UISprite> ().spriteName = "chip_50";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 100:
				obj.GetComponent<UISprite> ().spriteName = "chip_100";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 500:
				obj.GetComponent<UISprite> ().spriteName = "chip_500";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 1000:
				obj.GetComponent<UISprite> ().spriteName = "chip_1000";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 5000:
				obj.GetComponent<UISprite> ().spriteName = "chip_5000";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 10000:
				obj.GetComponent<UISprite> ().spriteName = "chip_1w";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 50000:
				obj.GetComponent<UISprite> ().spriteName = "chip_5w";
				obj.transform.FindChild ("effect1").gameObject.SetActive (true);
				break;
			case 100000:
				obj.GetComponent<UISprite> ().spriteName = "chip_10w";
				obj.transform.FindChild ("effect2").gameObject.SetActive (true);
				break;
			case 500000:
				obj.GetComponent<UISprite> ().spriteName = "chip_50w";
				obj.transform.FindChild ("effect2").gameObject.SetActive (true);
				break;
			case 1000000:
				obj.GetComponent<UISprite> ().spriteName = "chip_100w";
				obj.transform.FindChild ("effect3").gameObject.SetActive (true);
				break;
			}
			m_chipList.Add (obj);
		}
	}
}