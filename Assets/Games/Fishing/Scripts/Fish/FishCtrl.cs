using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class FishAndShadow
	{
		// 要创建的fish.
		public Transform fish;
		// 要创建的shadow。
		public Transform shadow;
	}

	[Serializable]
	public class FishParam
	{
		// 鱼的名字.
		public string name;
		// 线的位置偏移 x,y.
		public Vector3 pos;
		// 顺时针旋转0°
		public float rotX;

		// 鱼的id.
		public int serverID;
		public uint serverTime;
		
		// 鱼的路径.
		public NavPath navPath;
		// 鱼的路径的时间.
		public float pathTimeLength;

		// 记录这条鱼所在的鱼池, 创建这条鱼的时候, 依照这个鱼池创建.
		public int fishPool;
	}



	/// <summary>
	/// 鱼和影子的控制器.
	/// </summary>
	public class FishCtrl : MonoBehaviour 
	{
		public static FishCtrl Instance;
		// 鱼的预设.
		public List<FishAndShadow> fish;
		// 鱼和影子创建后的 parent.
		private Transform FishCache;
		// 需要创建的鱼.
		private List<FishParam> fish2Create = new List<FishParam>();
		// 已经创建的鱼.
		[HideInInspector] public List<SingleFish> fishHaveCreated = new List<SingleFish>();
		// 已经创建的影子.
		[HideInInspector] public List<SingleShadow> shadowHaveCreated = new List<SingleShadow>();

        private bool m_bMouseDownDouble = false;

        private RaycastHit2D hit;

        // 生成鱼的父节点
        public Transform FishCacheNoTurn;
        // 生成鱼的节点(旋转屏幕)
        public Transform FishCacheTurn;

		void Awake()
		{
			Instance = this;
		}
		
		void Update()
		{
            int createMaxOneFrame = 3;
			for(int i=0; i<fish2Create.Count; i++)
			{
				if(TimeCtrl.Instance.serverTime>=fish2Create[i].serverTime)
				{
                    --createMaxOneFrame;

                    if ( createMaxOneFrame == 0 )
                    {
                        break;
                    }

					CreateFishAndShadow(fish2Create[i]);
					fish2Create.Remove(fish2Create[i]);
				}
			}

            //  如果锁定开关打开,才开启双击锁定功能.
            if (CanonCtrl.Instance.Locking_off)
            {
                OnClickFishLock();
            }
		}

		// 接收到创建一条鱼的消息.
		public void AddFishInCreatList(FishParam _fp)
		{
			if(CheckIfFish2CreateIsEmpty(_fp))
			{
				return;
			}
			fish2Create.Add(_fp);
		}

		// 通过名字判断这条鱼是否在鱼池中, 有这条鱼存在, 才可以创建.
		private bool CheckIfFish2CreateIsEmpty(FishParam _fp)
		{
			int _fishCount = fish.Count;
			for(int i=0; i<_fishCount; i++)
			{
				if(fish[i].fish.name ==_fp.name)
				{
					_fp.fishPool = i;
					return false;
				}
			}
			return true;
		}

		private void CreateFishAndShadow(FishParam _fp)
		{            
			// 检测是否需要旋转屏幕,如果需要,则鱼的生成位置也需要更换并且旋转
			if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
				FishCache = FishCacheTurn;
			}else{
                FishCache = FishCacheNoTurn;	
			}

			int _fishPool = _fp.fishPool;
		
			// fish.
			Transform _fish = Factory.Create(fish[_fishPool].fish, Vector3.zero, Quaternion.identity);
			_fish.parent = FishCache;
			_fish.localPosition = Vector3.zero;
			_fish.rotation = Quaternion.identity;
			_fish.localScale = Vector3.zero;
			SingleFish _sf = _fish.GetComponent<SingleFish>();
			
			// shadow.
			Transform _shadow = Factory.Create(fish[_fishPool].shadow, Vector3.zero, Quaternion.identity);
			_shadow.parent = FishCache;
			_shadow.localPosition = Vector3.zero;
			_shadow.localScale = Vector3.zero;
			_shadow.rotation = Quaternion.identity;
			SingleShadow _ss = _shadow.GetComponent<SingleShadow>();

			// 给fish和shadow的路径赋时间.
			_fp.navPath._time = _fp.pathTimeLength;
			_sf.InitFishParam(_fp.navPath, _fp.serverID, _ss, _fishPool);
			_ss.InitShadowParam(_fp.navPath);

			// 缓存该鱼.
			fishHaveCreated.Add(_sf);
			shadowHaveCreated.Add(_ss);
		}

		// 通过 serverID 获取 fish.
		public SingleFish HaveSingleFishInList(int _serverID)
		{
		    for (int i = 0; i < fishHaveCreated.Count; i++)
		    {
		        SingleFish sf = fishHaveCreated[i];
                if (_serverID == sf.fishServerID)
                {
                    return sf;
                }
		    }
			return null;
		}

		// 回收fish的时候（打死或者游完路径），就从缓存 list 中 remove 该条鱼和它的影子.
		public void RemoveFishAndShadowInList(SingleFish _sf)
		{
			fishHaveCreated.Remove(_sf);
			shadowHaveCreated.Remove(_sf.singleShadow);
		}


		// 按下锁定键时候，获取一条鱼.
		private int lastLockIndex = 0;
		private SingleFish lastLockFish;
		public Transform GetALockFish()
		{
			int _fishNum = fishHaveCreated.Count;
			if(fishHaveCreated.Count==0)
			{
				return null;
			}
			else if(fishHaveCreated.Count==1)
			{
				//判断鱼的倍率是否满足锁定条件
				if( fishHaveCreated[0].multi >= CanonCtrl.Instance.minLockMulti)
				{
					return fishHaveCreated[0].transform;;
				}
				else 
				{
					return null;
				}
			}

			// 获取可以锁定的鱼.
			List<SingleFish> usefulFishList = new List<SingleFish>();
			for(int i=0; i<_fishNum; i++)
			{
				// 鱼存是否为空					鱼碰撞体没有关闭（1，鱼没死， 2，没有正在被全屏炸弹炸死中）					鱼是否在屏幕内.
				if(fishHaveCreated[i]!=null && fishHaveCreated[i].cur_Collider.enabled && Utility.CheckIfPosInViewport(fishHaveCreated[i].transform.position))
				{
					//判断鱼的倍率是否满足锁定条件
					if(fishHaveCreated[i].multi >= CanonCtrl.Instance.minLockMulti)
					{
						usefulFishList.Add(fishHaveCreated[i]);
					}
//					continue;
				}
			}

			_fishNum = usefulFishList.Count;

			//如果获取到的满足条件的鱼为空,返回
			if( _fishNum == 0 )
			{
				return null;
			}

			//将可以锁定的鱼排序
			for(int i=0; i<_fishNum-1; i++)
			{
				for(int j=0; j<_fishNum-1-i; j++)
				{
					if(usefulFishList[j].multi<usefulFishList[j+1].multi)
					{
						SingleFish temp = usefulFishList[j];
						usefulFishList[j] = usefulFishList[j+1];
						usefulFishList[j+1] = temp;
					}
				}
			}

			if(lastLockIndex>_fishNum - 1)
			{
				lastLockIndex = 0;
			}

			if(usefulFishList[lastLockIndex]!=lastLockFish)
			{

				lastLockFish = usefulFishList[lastLockIndex];

				lastLockIndex++;
				//如果连续切换4个后,返回第一个
				if(lastLockIndex>=4)
				{
					lastLockIndex = 0;
				}
				return lastLockFish.transform;
			}
			else 
			{
				lastLockIndex++;
				if(_fishNum>=lastLockIndex)
				{
					lastLockIndex = 0;
				}
				return usefulFishList[lastLockIndex].transform;
			}
		}


		// 获取 robot 要打击的鱼.
		public SingleFish GetRobotFireFish(int _serverID)
		{
			for(int i=0 ;i<fishHaveCreated.Count; i++)
			{
				if(fishHaveCreated[i].fishServerID==_serverID)
				{
					return fishHaveCreated[i];
				}
			}
			return null;
		}

		// 场景切换到左边边框的时候，记录场景里的鱼和影子，以便波浪移动完成后清理这些鱼.
		public void RecordLastMapFish()
		{
			SingleFish [] _sfa = fishHaveCreated.ToArray();
			for(int i=0; i<_sfa.Length; i++)
			{
				// include all fish.
				//if(_sfa[i].m_collider.enabled)
				{
					// 把这条鱼的 lastMap2ClearFish 标志位 true.
					_sfa[i].lastMap2ClearFish = true;
				}
			}
			SingleShadow [] _ssa = shadowHaveCreated.ToArray();
			for(int i=0; i<_ssa.Length; i++)
			{
				_ssa[i].lastMap2ClearFish = true;
			}
		}
		
		// 切换完场景的时候，清除在切换场景的时候记录的鱼和影子.
		public void ClearLastMapFish()
		{
			SingleFish [] _sfa = fishHaveCreated.ToArray();
			for(int i=0; i<_sfa.Length; i++)
			{
				if(_sfa[i].lastMap2ClearFish)
				{
					// clear all fish we have included.
					// if(_sfa[i].m_collider.enabled)
					{
						fishHaveCreated.Remove(_sfa[i]);
						_sfa[i].WaveForceRecycle();
						Factory.Recycle(_sfa[i].transform);
					}
				}
			}
			SingleShadow [] _ssa = shadowHaveCreated.ToArray();
			for(int i=0; i<_ssa.Length; i++)
			{
				if(_ssa[i].lastMap2ClearFish)
				{
					shadowHaveCreated.Remove(_ssa[i]);
					Factory.Recycle(_ssa[i].transform);
				}
			}
		}



		/*void OnGUI()
		{
			if(!MessageHandler.Instance.showAllGui)
			{
				return;
			}
			GUILayout.Space(200f);
			GUILayout.Label("即将要创建的鱼 = "+fish2Create.Count);
			GUILayout.Label("场景中鱼的数量 = "+fishHaveCreated.Count+"/"+shadowHaveCreated.Count);
		}*/

        void OnDestroy()
        {
            Instance = null;
        }

        /// <summary>
        /// 点击锁定鱼
        /// </summary>
        public void OnClickFishLock()
        {
            if (Input.GetMouseButtonDown(0) && m_bMouseDownDouble==false )
            {
                m_bMouseDownDouble = true;
                StartCoroutine(WaitTimeClearMouseDouble(0.5f));
                                

            }else  if (Input.GetMouseButtonDown(0) && m_bMouseDownDouble)
            {
                m_bMouseDownDouble = false;
                if (UICamera.currentCamera == null) return;
                hit = Physics2D.Raycast(UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit != null)
                {
                    if (hit.collider != null)
                        for (int i = 0; i < fishHaveCreated.Count; i++)
                        {
                            if (hit.collider.gameObject.transform == fishHaveCreated[i].transform)
                            {
                                if (fishHaveCreated[i].multi >= CanonCtrl.Instance.minLockMulti)
                                {
                                    FishLockState(fishHaveCreated[i].transform);
                                }
                            }
                        }
                }
            }
            else
            {
                if (UICamera.currentCamera == null) return;
                hit = Physics2D.Raycast(UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit != null)
                {
                    if (hit.collider != null)
                        for (int i = 0; i < fishHaveCreated.Count; i++)
                        {
                            if (hit.collider.gameObject.transform == fishHaveCreated[i].transform)
                            {
                                if (fishHaveCreated[i].multi > CanonCtrl.Instance.minLockMulti)
                                {
                                    fishHaveCreated[i].ChangeColor();
                                }
                            }
                        }
                }
            }
        }

        // 更改锁定状态.
        void FishLockState(Transform _transform)
        {
            //切换场景时不允许更改锁定状态.
            if (WaveCtrl.Instance.changingWave)
            {
                return;
            }
            if (CanonCtrl.Instance.inLockMode)
            {
                // 停止锁定.
                CanonCtrl.Instance.StopLock();
                // 播放取消锁定声音.
                AudioCtrl.Instance.PressButton(false);
                // 开关显示 关
                if (CanonCtrl.Instance.lockButton_img != null) CanonCtrl.Instance.lockButton_img.SetActive(false);
            }
            // 如果不在锁定状态.
            if (!CanonCtrl.Instance.inLockMode)
            {
                // 锁定一条鱼.
                CanonCtrl.Instance.lockedFish = _transform;
                if (CanonCtrl.Instance.lockedFish != null)
                {
                    CanonCtrl.Instance.inLockMode = true;
                    // 通知锁定线去锁定这条鱼.
                    LockCtrl.Instance.lockCtrls[CanonCtrl.Instance.realCanonID].ShowLockRend(CanonCtrl.Instance.lockedFish);
                    // 告诉这条鱼，它被锁定了.
                    CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().locking = true;
                    // 播放打开锁定声音.
                    AudioCtrl.Instance.PressButton(true);

                    // 开关显示 开
                    if (CanonCtrl.Instance.lockButton_img != null) CanonCtrl.Instance.lockButton_img.SetActive(true);
                }
            }
        }

        System.Collections.IEnumerator WaitTimeClearMouseDouble(float _fWaitTime)
        {
            yield return new WaitForSeconds(_fWaitTime);
            m_bMouseDownDouble = false;
        }
	}
}

