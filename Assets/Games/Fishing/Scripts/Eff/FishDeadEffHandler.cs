using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 专门处理鱼死亡后的特效.
	/// </summary>

	public class FishDeadEffHandler 
	{

		// 特效的父节点.
		static Transform effCache;
		static Transform CreateCache()
		{
			if(effCache==null)
			{
				effCache = new GameObject().transform;
				effCache.parent = Utility.GetUICam().transform;
				effCache.localScale = Vector3.one;
				effCache.gameObject.AddComponent<UIAnchor>().side = UIAnchor.Side.BottomLeft;
				effCache.name = "effCache";
			}
			return effCache;
		}


		// 处理一条鱼死亡后要播放的特效.
		public static void CreatedFishDeadEffCtrlItem(SingleFish _sf, List<EffParam> _effList)
		{
			GameObject _effCtrlItem = new GameObject();
			_effCtrlItem.name = "EffCtrlItem";
			_effCtrlItem.transform.parent = CreateCache();
			FishDeadEffectItemCtrl _eff = _effCtrlItem.AddComponent<FishDeadEffectItemCtrl>();
			_eff.StartCreateParticle(_sf, _effList, CreateCache());
		}
	}
}