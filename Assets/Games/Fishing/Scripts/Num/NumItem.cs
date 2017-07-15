using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	public class NumItem : MonoBehaviour {

		public enum PlaceWay
		{
			left2Right,
			right2Left,
			Center
		}
		public PlaceWay placeWay = PlaceWay.Center;
		public float gap = 20f;
		public Vector3 scale = Vector3.one;
		private int totalSpriteNum = 0;
		private int lastSpriteNum = 0;
		private List<SpriteRenderer> spriteRendList = new List<SpriteRenderer>();
		public Sprite [] numSprite;
		public bool infrontOfNGUI = true;

		public int NumOrder = 0;

		void Awake () 
		{
			int childCount = this.transform.childCount;
			for(int i=0; i<childCount; i++)
			{
				SpriteRenderer _sp = transform.GetChild(i).GetComponent<SpriteRenderer>();
				if(_sp!=null)
				{
					spriteRendList.Add(_sp);
					totalSpriteNum++;
					lastSpriteNum++;
					scale = _sp.transform.localScale;
					_sp.sprite = null;
				
					if(infrontOfNGUI)
					{
						_sp.sortingLayerName = "numF";
					}
					else 
					{
						_sp.sortingLayerName = "numB";
					}
				}
			}
		}

		void Updatess()
		{
			if(Input.GetKeyDown(KeyCode.C))
			{
				ApplyValue(Random.Range(0,100000), 0);
			}
		}

		public void ApplyValue(uint _value1, int _dir)
		{
			int [] _value = Utility.Char2Int(_value1);
			CheckLength(_value.Length, _dir);
			SetValue(_value);
		}

		public void ApplyValue(int _value1, int _dir)
		{
			int [] _value = Utility.Char2Int(_value1);
			CheckLength(_value.Length, _dir);
			SetValue(_value);
		}

		void CheckLength(int _length, int _dir)
		{
			if(_length==lastSpriteNum)
			{
				return;
			}

			if(_length>totalSpriteNum)
			// if(_length>spriteRendList.Count)
			{
				int _createNum = _length - totalSpriteNum;
				// int _createNum = _length - spriteRendList.Count;
				for(int i=0; i<_createNum; i++)
				{
					GameObject _temp = new GameObject();
					_temp.name = totalSpriteNum.ToString();
					// _temp.name = spriteRendList.Count.ToString();
					_temp.transform.parent = transform;
					_temp.transform.localScale = scale;
					_temp.transform.localEulerAngles = new Vector3(0f,0f,_dir*90f);

					SpriteRenderer _sp = _temp.AddComponent<SpriteRenderer>();

					if(infrontOfNGUI)
					{
						_sp.sortingLayerName = "numF";
						_sp.sortingOrder = NumOrder;
					}
					else 
					{
						_sp.sortingLayerName = "numB";
						_sp.sortingOrder = NumOrder;
					}
					spriteRendList.Add(_sp);
					totalSpriteNum++;				     
				}
			}

			Vector3 rightestPos = Vector3.zero; 
			if(placeWay==PlaceWay.right2Left)
			{
				for(int i=0; i<_length; i++)
				{
					spriteRendList[i].transform.localPosition = new Vector3(-gap*i, 0f, 0f);
				}
			}
			else if(placeWay==PlaceWay.Center)
			{
				bool singular = _length%2==0?false:true;
				if(singular)
				{
					int rightestIndex = _length/2;
					rightestPos = new Vector3(rightestIndex*gap, 0f, 0f);
				}
				else 
				{
					int rightestIndex = _length/2;
					rightestPos = new Vector3(rightestIndex*gap - gap*0.5f, 0f, 0f);
				}
				for(int i=0; i<_length; i++)
				{
					spriteRendList[i].transform.localPosition = rightestPos - new Vector3(gap*i, 0f, 0f);
				}
			}
			else if(placeWay==PlaceWay.left2Right)
			{
				for(int i=0; i<_length; i++)
				{
					spriteRendList[i].transform.localPosition = new Vector3(gap*(_length-1-i), 0f, 0f);
				}
			}

			lastSpriteNum = _length;
		}

		void SetValue(int [] value)
		{
			int _valueLength = value.Length;
			int _applyLength = 0;
			// int count = spriteRendList.Count;
			for(int i=0; i<totalSpriteNum; i++)
			{
				_applyLength++;
				if(_applyLength>_valueLength)
				{
					spriteRendList[i].sprite = null;
				}
				else 
				{
					// spriteRendList[i].sprite = NumCtrl.Instance.Sprite_1[value[i]];
					spriteRendList[i].sprite = numSprite[value[i]];
				}	
			}
		}
	}
}
