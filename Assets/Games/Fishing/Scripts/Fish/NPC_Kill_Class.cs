using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	// 缓存击杀保留的数据.
	[Serializable]
	public class CacheFishParam
	{
		public int value;
		public SingleFish sf;
		public Vector3 pos;
		public int _serverID;
	}

	// NPC_KILL 检测.
	public class NPC_Kill_Class : MonoBehaviour 
	{
		// 缓存击杀保留的数据.
		public static List<CacheFishParam> cacheFish = new List<CacheFishParam>();

		// 1 普通 npc kill.
		public static void NPC_Kill (int _canonID, int _multi, int _power, int _serverID, int _npcType) 
		{
			int _value = _multi*_power;
			SingleCanon _singleCanon = CanonCtrl.Instance.singleCanonList[_canonID];
			SingleFish _sf = FishCtrl.Instance.HaveSingleFishInList(_serverID);
			
			// 如果这条鱼不在了，直接给分.
			if(_sf==null)
			{
				_singleCanon.AddValueList(_value, 0);
				return;
			}

			// 如果还在，那就跳到子弹击中的逻辑.
			_sf.KillByBullet(_canonID, _multi, _power, _npcType);

			// 如果是全屏炸弹，就不要显示数字，bubble, 金币柱.
			if(_sf.fishType==BaseFish.FishType.quanping)
			{
				return;
			}
			
			// 鱼死亡后在身上出现的数字.
			NumCtrl.Instance.CreateFishDeadNum(_value, _sf.transform.position, Quaternion.identity);
			
			//　bubble.
			NumCtrl.Instance.AdddBubble2List(_multi, _value, _singleCanon.bubbleShowUpPos.position, Quaternion.identity, _singleCanon.upsideDown);
			
			// 金币柱子.
			if(TotalCylinderCtrl.Instance.showCylider)
			{
				TotalCylinderCtrl.Instance.singleCylinderList[_canonID].Add2Need2CreateList(_multi, _value, _singleCanon.cylinderShowUpPos.position);
			}

			// 给分.
			_singleCanon.AddValueList(_value, 0f);
		}

		// 2-1 不是普通鱼，是缓存击杀，那么把这条缓存击杀的鱼添加到 list 中.
		public static void NPC_Kill_AddCacheInList (int _canonID, int _multi, int _power, int _serverID, int _npcType) 
		{
			SingleFish _sf = FishCtrl.Instance.HaveSingleFishInList(_serverID);
			Vector3 _fishPos = Vector3.zero;

			// 如果这条鱼还在.
			if(_sf!=null)
			{
				_fishPos = _sf.transform.position;
				_sf.KillByBullet(_canonID, _multi, _power, _npcType);
			}

			CacheFishParam _cache = new CacheFishParam();

			// 记录这条缓存的鱼.
			_cache.sf 			= _sf;
			_cache.pos 			= _fishPos;
			_cache.value 		= _multi * _power;
			_cache._serverID 	= _serverID;
			cacheFish.Add(_cache);
		} 
		
		// 2-2. 接收到缓存的分. 
		public static void NPC_Kill_GiveCacheFromList (int _canonID, int _multi, int _value) 
		{
			SingleCanon _singleCanon = CanonCtrl.Instance.singleCanonList[_canonID];
			
			int _cacheIndexInList = -1;
			for(int i=0; i<cacheFish.Count; i++)
			{
				if(cacheFish[i].value == _value)
				{
					_cacheIndexInList = i;

					// 如果是黑洞，那么退出，因为在上面 NPC_Kill_AddCacheInList 中就已经跳进 KillByBullet 中慢慢吸鱼给分了.
					if(cacheFish[i].sf.fishType == BaseFish.FishType.blackHall)
					{
						if(cacheFish.Contains(cacheFish[_cacheIndexInList]))
						{
							cacheFish.Remove(cacheFish[_cacheIndexInList]);
						}
						return;
					}
					break;
				}
			}

			// 如果在上面的缓存击杀的 list 中没有找到记录的这个分数.
			if(_cacheIndexInList==-1)
			{
				// 直接加分.
				_singleCanon.AddValueList(_value, 0f);
				cacheFish.Remove(cacheFish[_cacheIndexInList]);
				return;
			}
			
			// 如果这条鱼不在了.
			if(cacheFish[_cacheIndexInList].sf==null)
			{
				// 直接加分.
				_singleCanon.AddValueList(_value, 0f);
				cacheFish.Remove(cacheFish[_cacheIndexInList]);
				return;
			}

			// 加分.
			_singleCanon.AddValueList(_value, 0f);

			// 鱼死亡后在身上出现的数字.
			NumCtrl.Instance.CreateFishDeadNum(_value, cacheFish[_cacheIndexInList].pos, Quaternion.identity);
			
			// bubble.
			NumCtrl.Instance.AdddBubble2List(_multi, _value, _singleCanon.bubbleShowUpPos.position, Quaternion.identity, _singleCanon.upsideDown);
			
			// 金币柱子.
			if(TotalCylinderCtrl.Instance.showCylider)
			{
				TotalCylinderCtrl.Instance.singleCylinderList[_canonID].Add2Need2CreateList(_multi, _value, _singleCanon.cylinderShowUpPos.position);
			}

			// 从 list 中移除这个缓存击杀.
			cacheFish.Remove(cacheFish[_cacheIndexInList]);
		}
	}
}

