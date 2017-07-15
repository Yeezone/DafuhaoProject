using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 这个是用在每一个highScore里面的，用来显示highscore的数字.
	/// </summary>

	[RequireComponent(typeof(AudioSource))]
	public class HighScoreNumCtrl : MonoBehaviour 
	{
		public enum PlaceWay
		{
			left2Right,
			right2Left,
			Center
		}
		public PlaceWay placeWay = PlaceWay.Center;
		public float gap = 0.3f;
		public Vector3 scale = Vector3.one;
		private int totalSpriteNum = 0;
		private int lastSpriteNum = 0;
		private List<SpriteRenderer> spriteRendList = new List<SpriteRenderer>();
		public Sprite [] numSprite;
		public bool infrontOfNGUI = true;
		public float lifeTime;
		private float time2Recycle;
		private bool haveRecycle = false;

//		private AudioSource audioS;
		private UIPlaySound audioS;
		
//		public int NumOrder = 1;
		public Transform num;

		void Awake () 
		{
			audioS = GetComponent<UIPlaySound>();
		}
		
		void Update()
		{
			if(!haveRecycle)
			{
				if(Time.time>time2Recycle)
				{
					Factory.Recycle(this.transform);
					haveRecycle = true;
				}
			}
		}
		
		public void ApplyValue(uint _value1, int _dir)
		{
			if(audioS!=null){
				audioS.enabled=true;
			}

			haveRecycle = false;
			time2Recycle = Time.time + lifeTime; 

			int [] _value = Utility.Char2Int(_value1);
			CheckLength(_value.Length, _dir);
			SetValue(_value);
		}
		
		public void ApplyValue(int _value1, int _dir)
		{
			if(audioS!=null){
				audioS.enabled=true;
			}
			
			haveRecycle = false;
			time2Recycle = Time.time + lifeTime; 

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
			{
				int _createNum = _length - totalSpriteNum;
				for(int i=0; i<_createNum; i++)
				{
					GameObject _temp = new GameObject();
					_temp.name = totalSpriteNum.ToString();
					_temp.transform.parent = num;
					_temp.transform.localScale = scale;
					_temp.transform.localEulerAngles = new Vector3(0f,0f,_dir*90f);
					
					SpriteRenderer _sp = _temp.AddComponent<SpriteRenderer>();
					
					if(infrontOfNGUI)
					{
						_sp.sortingLayerName = "numF";
						_sp.sortingOrder = 1;
					}
					else 
					{
						_sp.sortingLayerName = "numB";
						_sp.sortingOrder = 1;
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
