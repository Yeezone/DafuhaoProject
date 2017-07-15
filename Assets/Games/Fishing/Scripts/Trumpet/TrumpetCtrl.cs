using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class TrumpetParam
	{
		// 玩家id.
		public int plyID;
		// 倍率.
		public int multi;
		// 分数.
		public int value;
		// 下一次创建喇叭的时间.
		public float time2Create;
	}


	/// <summary>
	/// 喇叭总控制器.
	/// </summary>
	public class TrumpetCtrl : MonoBehaviour 
	{
		public static TrumpetCtrl Instance;
		// 喇叭预设.
		public Transform trumpetItem;
		// 打死的鱼的倍率大于这个 targetMulti 才创建喇叭.
		public int targetMulti;
		// 喇叭时间间隔.
		public float trumpetTimeGap = 3f;
		// 需要创建的喇叭的缓存列表.
		private List<TrumpetParam> trumpet2CreateList = new List<TrumpetParam>();
		// 是否还有喇叭要创建.
		private bool haveTrumpet2Create = false;
		private float nextTrumpet2CteateTime = 0f;

		void Awake ()
		{
			Instance = this;
		}

		//适配屏幕.
		void Start()
		{
			transform.localScale = new Vector3(transform.localScale.x * Utility.device_to_1136x640_ratio.x, transform.localScale.y * Utility.device_to_1136x640_ratio.y, 1f);
		}

		// 创建喇叭.
		public void CreateOneTrumpet (int _plyID, int _multi, int _value)
		{
			// 达到足够倍率才创建.
			if(_multi<targetMulti)
			{
				return;
			}

			// 赋值.
			TrumpetParam temp = new TrumpetParam();
			temp.plyID = _plyID;
			temp.multi = _multi;
			temp.value = _value;
			if(trumpet2CreateList.Count>0)
			{
				temp.time2Create = trumpet2CreateList[trumpet2CreateList.Count-1].time2Create + trumpetTimeGap;
			}
			else 
			{
				if(Time.time>nextTrumpet2CteateTime)
				{
					temp.time2Create = Time.time;
				}
				else 
				{
					temp.time2Create = nextTrumpet2CteateTime;
				}
			}
			// 添加到队列中.
			trumpet2CreateList.Add(temp);

			haveTrumpet2Create = true;
			nextTrumpet2CteateTime = trumpet2CreateList[trumpet2CreateList.Count-1].time2Create + trumpetTimeGap;
		}
		
		void Update ()
		{
			//　没有喇叭要创建了.
			if(!haveTrumpet2Create)
			{
				return;
			}
			
			if(Time.time>=trumpet2CreateList[0].time2Create)
			{
				if(trumpet2CreateList.Count>0)
				{
					// 创建一个喇叭.
					if(trumpetItem!=null)
					{
						if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
							Transform _temp_over = Factory.Create(trumpetItem, Vector3.zero,new Quaternion(0.0f, 0.0f, 1.0f, 0.0f));
							_temp_over.parent = transform;
							_temp_over.GetComponent<TrumpetItem>().ApplyParameters(trumpet2CreateList[0].plyID, trumpet2CreateList[0].multi, trumpet2CreateList[0].value);
						}else{
							Transform _temp = Factory.Create(trumpetItem, Vector3.zero, Quaternion.identity);
							_temp.parent = transform;
							_temp.GetComponent<TrumpetItem>().ApplyParameters(trumpet2CreateList[0].plyID, trumpet2CreateList[0].multi, trumpet2CreateList[0].value);
						}							
					}
			
					// 判断是否还有喇叭需要创建.
					trumpet2CreateList.Remove(trumpet2CreateList[0]);
					if(trumpet2CreateList.Count<=0)
					{
						haveTrumpet2Create = false;
					}
				}
				else 
				{
					haveTrumpet2Create = false;
				}
			}
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}
