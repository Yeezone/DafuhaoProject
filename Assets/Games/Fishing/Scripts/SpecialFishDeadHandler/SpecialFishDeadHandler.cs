using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// 一网打尽，全屏炸弹显示线的控制器.
	/// </summary>


	public class SpecialFishDeadHandler : MonoBehaviour 
	{

		// 创建一网打尽的线.
		public static void Show_YWDJ_Lines(SingleFish _sf, Transform _ywdjLine, int _canonID, int _killFishPool, int _bulletCost, Vector3 _ywdjFishPos, float _timeLength)
		{
			if(_ywdjLine==null)
			{
				return;
			}

			int maxCoinNum = 0;
			SingleFish maxCoinNumFish = null;

			SingleFish [] sfArray = FishCtrl.Instance.fishHaveCreated.ToArray();
			for(int i=0; i<sfArray.Length; i++)
			{
				if(sfArray[i]!=null && sfArray[i]!=_sf && sfArray[i].fishPool==_killFishPool && !sfArray[i].dead && Utility.CheckIfPosInViewport(sfArray[i].transform.position))
				{
					// 创建线.
					Transform _item = Factory.Create(_ywdjLine, Vector3.zero, Quaternion.identity);

					//线会自动回收.
					AutoRecycle _ar = _item.gameObject.GetComponent<AutoRecycle>();
					if(_ar==null)
					{
						_ar = _item.gameObject.AddComponent<AutoRecycle>();
					}
					_ar.SetLifeTime(_timeLength);


					_item.parent = LockCtrl.Instance.transform;
					LineRenderer _lineRend = _item.GetComponent<LineRenderer>();
//					Vector3 _pos0_InLockCam = LockCtrl.Instance.uiCamPos_to_LockCamPos(_ywdjFishPos);
//					Vector3 _pos1_InLockCam = LockCtrl.Instance.uiCamPos_to_LockCamPos(sfArray[i].transform.position);
					Vector3 temp = _ywdjFishPos;
                    temp.z = 0.1f;
                    Vector3 _pos0_InLockCam = temp;
					Vector3 _pos1_InLockCam = sfArray[i].transform.position;
						
					_lineRend.SetPosition(0, _pos1_InLockCam);
					_lineRend.SetPosition(1, _pos0_InLockCam);

					// play fish dead eff.
					int _npcType = -1;
					sfArray[i].KillByBullet(_canonID, -1, -1, _npcType);

					if(sfArray[i].coinNum>maxCoinNum)
					{
						maxCoinNum 		= sfArray[i].coinNum;
						maxCoinNumFish 	= sfArray[i]; 
					}

					// show value eff.
					NumCtrl.Instance.CreateFishDeadNum(_bulletCost*sfArray[i].multi, sfArray[i].transform.position, Quaternion.identity);
				}
			}

			if(maxCoinNumFish!=null)
			{
				maxCoinNumFish.PlayCoinAudio(true);
			}
		}


		// 记录全屏炸弹要杀死的鱼.
		private static List<SingleFish> quanPing_KillFish_List = new List<SingleFish>();
		public static void Record_quanPing_KillFish(SingleFish _mySelf)
		{
			SingleFish [] sfArray = FishCtrl.Instance.fishHaveCreated.ToArray();
			for(int i=0; i<sfArray.Length; i++)
			{
				if(sfArray[i].fishType==BaseFish.FishType.normal)
				{
					if(!_mySelf.killNorml)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.ywdj)
				{
					if(!_mySelf.killYWDJ)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.quanping)
				{
					if(!_mySelf.killQuanPing)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.blackHall)
				{
					if(!_mySelf.killBlackHall)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.likui)
				{
					if(!_mySelf.killLikui)
					{
						continue;
					}
				}
				
				if(sfArray[i]!=null && sfArray[i]!=_mySelf && !sfArray[i].dead)
				{
					sfArray[i].KillByQuanping();
					quanPing_KillFish_List.Add(sfArray[i]);
				}
			}
		}


		// 告诉这些鱼，它们被全屏炸弹杀死了.
		public static void QuanPing_Kill_Fish(SingleFish _mySelf, int _canonID, int _bulletCost)
		{
			int maxCoinNum = 0;
			SingleFish maxCoinNumFish = null;

			SingleFish [] sfArray = quanPing_KillFish_List.ToArray();
			for(int i=0; i<sfArray.Length; i++)
			{
				if(sfArray[i].fishType==BaseFish.FishType.normal)
				{
					if(!_mySelf.killNorml)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.ywdj)
				{
					if(!_mySelf.killYWDJ)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.quanping)
				{
					if(!_mySelf.killQuanPing)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.blackHall)
				{
					if(!_mySelf.killBlackHall)
					{
						continue;
					}
				}
				else if(sfArray[i].fishType==BaseFish.FishType.likui)
				{
					if(!_mySelf.killLikui)
					{
						continue;
					}
				}

				// no recycle by the wave.
				// if(sfArray[i]!=null && !sfArray[i].dead && sfArray[i].gameObject.activeSelf)
				if(sfArray[i]!=null && !sfArray[i].dead && sfArray[i].gameObject.activeSelf)
				{
					// play fish dead eff.
					int _npcType = -1;
					sfArray[i].KillByBullet(_canonID, -1, -1, _npcType);
					sfArray[i].PlayCoinAudio(false);
					
					if(sfArray[i].coinNum>maxCoinNum)
					{
						maxCoinNum 		= sfArray[i].coinNum;
						maxCoinNumFish 	= sfArray[i]; 
					}

					// show value eff.
					NumCtrl.Instance.CreateFishDeadNum(_bulletCost*sfArray[i].multi, sfArray[i].transform.position, Quaternion.identity);
				}
			}

			if(maxCoinNumFish!=null)
			{
				maxCoinNumFish.PlayCoinAudio(true);
			}

			quanPing_KillFish_List.Clear();
		}
	}
}