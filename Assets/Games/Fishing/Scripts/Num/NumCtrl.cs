using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class BubbleParam
	{
		// bubble 的倍率.
		public int mul;
		// bubble 的分值.
		public int value;
		// 位置，rotation.
		public Vector3 pos;
		public Quaternion rot;
		// bubble 是否需要翻转180°.
		public bool upsideDown;
	}


	/// <summary>
	/// 用来创建数字（鱼死了以后在鱼身上创建的数字，bubble 数字）.
	/// </summary>
	public class NumCtrl : MonoBehaviour 
	{
		public static NumCtrl Instance;

		// 创建出来的 num 的父节点.
		public Transform numCache;
		// 鱼死了以后在鱼身上创建的数字.
		public Transform fishDeadNum;
		// bubble 数字.
		public Transform fishBubbleNum;

		// 要创建的 bubble 队列. 
		private List<BubbleParam> bubbleList = new List<BubbleParam>();
		// bubble 时间间隔.
		public float bubbleTimeGap = 1f;
		private float targetBubbleShowUpTime;


		void Awake () 
		{
			Instance = this;
			if(numCache==null)
			{
				numCache = Utility.uiRoot.gameObject.transform.FindChild("Camera").FindChild("NumCache");
			}
		}

		// 数值根据 1600*900 进行适配. 
		void Start()
		{
			numCache.localScale = new Vector3(numCache.localScale.x * Utility.device_to_1600x900_ratio.x, numCache.localScale.y * Utility.device_to_1600x900_ratio.y, 1f);
		}

		
		// 添加一个 bubble 到队列中.
		public void AdddBubble2List(int _mul, int _value, Vector3 _pos, Quaternion _rot, bool _upsideDown)
		{
			if(Time.time>targetBubbleShowUpTime)
			{
				targetBubbleShowUpTime = Time.time;
			}
			
			BubbleParam _b = new BubbleParam();
			_b.mul = _mul;
			_b.value = _value;
			_b.pos = _pos;
			// 如果玩家炮台需要旋转,则调整泡泡上数字显示的方向
			if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
				_b.rot = new Quaternion(0.0f,0.0f,1.0f,0.0f);
			}else{
				_b.rot = _rot;
			}
			_b.upsideDown = _upsideDown;
			
			bubbleList.Add(_b);
		}


		void Update () 
		{
			if(bubbleList.Count>0)
			{
				if(Time.time>=targetBubbleShowUpTime)
				{
					CreateFishBubble(bubbleList[0]);
					bubbleList.Remove(bubbleList[0]);
					targetBubbleShowUpTime = Time.time + bubbleTimeGap;
				}
			}
		}
		
		// 创建 bubble.
		void CreateFishBubble(BubbleParam _b)
		{			
			Transform _bubble = Factory.Create(fishBubbleNum, _b.pos, _b.rot);
			// 暴力初始化SingleBubbleNum脚本数据,消除泡泡个位和十位数字颠倒bug.
			_bubble.GetComponent<SingleBubbleNum>().Init(0,0,false);
			_bubble.GetComponent<SingleBubbleNum>().Init(_b.mul, _b.value, _b.upsideDown);
			_bubble.parent = numCache;
			_bubble.localScale = Vector3.one;
		}


		// 创建 鱼死亡后身上的数字.
		public void CreateFishDeadNum(int _value, Vector3 _pos, Quaternion _rot)
		{
			// 如果玩家炮台需要旋转,则调整鱼身上数字显示的方向
			if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
				_rot = new Quaternion(0.0f,0.0f,1.0f,0.0f);
			}
			Transform _fishDeadNum = Factory.Create(fishDeadNum, _pos, _rot);
			_fishDeadNum.GetComponent<SingleFishNum>().Init(_value);
			_fishDeadNum.parent = numCache;
			_fishDeadNum.localScale = Vector3.one;
			if(_value<0)
			{
				Debug.LogError("_value = " +_value);
				return;
			}
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}