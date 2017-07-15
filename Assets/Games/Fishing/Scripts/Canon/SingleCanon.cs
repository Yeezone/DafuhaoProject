using UnityEngine;
using System;
using System.Collections.Generic;


namespace com.QH.QPGame.Fishing
{
	// 子弹的参数.
	[Serializable]
	public class BulletParameter
	{
		public int bulletPower;
		public int bulletCost;
		// 倍率.
		public int bulletMulti;
	}

	// 鱼死亡后给分的参数.
	[Serializable]
	public class FishDeadGiveValue
	{
		// 鱼死后要给分数的时间.
		public float targetTime2GiveValue;
		// 鱼死后要给的分数.
		public int addValue;
	}

	[Serializable]
	public class PG_anm_ctr
	{
		//	炮管组件
		public UISprite paoguan;
		//	缓存时间
		public float m_waitTime_pg ;
		//	每帧时间
		public float m_ffps_pg ;
		//	动画开关
		public bool m_bIsOpen_pg ;
		//	缓存图片帧数
		public int num_pg;
		//	整个动画帧数
		public int ani_num_pg;
	}

	[Serializable]
	public class PH_anm_ctr
	{		
		//	炮火组件
		public UISprite paohuo;
		//	缓存时间
		public float m_waitTime_ph ;
		//	每帧时间
		public float m_ffps_ph ;
		//	动画开关
		public bool m_bIsOpen_ph ;
		//	缓存图片帧数
		public int num_ph;
		//	整个动画帧数
		public int ani_num_ph;
	}


	public class SingleCanon : MonoBehaviour 
	{
		// 当前玩家的id.
		[HideInInspector] public int canonID;
		//　ngui 的 camera.
		private Camera UICam;

		//　炮管.
		public Transform canonBarrelTrans;
		//　这个 transform 和 canonBarrelTrans 的大小位置都一样， 是用来计算服务器叫机器人收集往一条鱼发射的炮台角度.
		public Transform canonBarrelTrans_robot;
		// 这个是服务器叫机器人的发泡位置.
		private Transform bulletShowUpPos_robot;

		// 子弹出现的位置.
		public Transform bulletShowUpPos;
		// 炮台的动画控制器.
//		public Animator stationAnimator;
		// 炮管的动画控制器.
		public Transform barrelAnchor;
		private Animator barrelAnimator;
		// 炮火的动画控制器.
		//private Animator fireAnimator;

		// 当前炮值所对应的子弹的数据.
		private BulletParameter curBulletParam;
		// 当前炮值所对应的子弹是哪个.
		private Transform curBulletPrefab;
		// 当前炮值所对应的网格.
		private Transform curNetPrefab;
		
		// 子弹的数目.
		private int curBulletCount;

		// 子弹的父节点.
		private Transform bulletCache;
		// 金币回收到炮台的位置.
		public Transform coinHomePos;

		// 是否允许发泡.
		private bool allowFire;

		// 发泡间隔.
		private float fireGap;
		// 允许发射下一个子弹的时间.
		private float nextFireTargetTime;

		// 暴机提示的预设.
		public Transform peakHint;

		// 玩家分数.
		[HideInInspector] public uint plyScore;

		// 显示玩家分数的脚本.
		//		public UILabel UILabel;
		public UILabel plyScoreLabel;
		// 显示子弹 power 的脚本.
//		public NumItem bulletPowerLabel;
		public UILabel bulletPowerLabel;
		// 显示子弹倍率的脚本.
		public NumItem bulletMultiLabel;

		// 打死鱼后出现 bubble 的位置.
		public Transform bubbleShowUpPos;

		// 金币柱出现的位置.
		public Transform cylinderShowUpPos;
		// 金币柱回收的位置.
		public Transform cylinderTargetPos;

		// 加炮的按钮.
		public Transform addBarrelValue;

		// 炮台在上下左右哪个位置
		[HideInInspector] public int dir;
		// 炮台是否在上面.
		[HideInInspector] public bool upsideDown = false;

		// 锁定线开始的位置.
		public Transform lockStartPos;
		private float targetTime2CheckFishMoveOutViewport = 0f;

		// high score 出现的位置.
		public Transform highScorePos;

		// 玩家上一次在屏幕上不是点击NGUI控件的位置.
		private Vector3 lastMouseDownPosition;

		// 以下几个玩家数据暂时还没用到.
		[HideInInspector] public int plyUniqueID;
		[HideInInspector] public int level;
		[HideInInspector] public string plyName;
		[HideInInspector] public int boatColorID;
		[HideInInspector] public int bulletColorID;

		//长按时间
		private float tempTouchTime = 0f;

		// 炮值取到百分比,用于更新炮台和子弹.
		private float proportion = 0.1f;

		// 炮火和炮管
		public PH_anm_ctr ph_anm_ctr = new PH_anm_ctr();
		public PG_anm_ctr pg_anm_ctr = new PG_anm_ctr();

		// 炮台编号
		public UISprite m_sCanonNumSprite;

		void Awake () 
		{
			UICam = Utility.GetUICam();

			// 如果在 Inspector 面板上没有赋值，那么就会通过查找方式获取下面的物件.
			if(canonBarrelTrans==null)
			{
				canonBarrelTrans = transform.FindChild("canonBarrelTrans");
			}
			if(canonBarrelTrans_robot==null)
			{
				canonBarrelTrans_robot = transform.FindChild("canonBarrelTrans_robot");
			}
			if(bulletShowUpPos_robot==null)
			{
				bulletShowUpPos_robot = canonBarrelTrans_robot.FindChild("bulletShowUpPos");
			}
			if(barrelAnchor==null)
			{
				barrelAnchor = canonBarrelTrans.FindChild("barrelAnchor");
			}
//			if(stationAnimator)
//			{
//				stationAnimator = transform.FindChild("station").GetComponent<Animator>();
//			}
			if(bulletShowUpPos==null)
			{
				bulletShowUpPos = canonBarrelTrans.FindChild("bulletShowUpPos");
			}
			if(plyScoreLabel)
			{
				plyScoreLabel = transform.FindChild("plyScoreLabel").GetComponent<UILabel>();
			}

			if(bulletPowerLabel)
			{
				bulletPowerLabel = transform.FindChild("bulletPowerLabel").GetComponent<UILabel>();
			}
			if(bulletMultiLabel)
			{
				bulletMultiLabel = transform.FindChild("bulletMulti").FindChild("bulletMultiLabel").GetComponent<NumItem>();
			}
			if(bubbleShowUpPos==null)
			{
				bubbleShowUpPos = transform.FindChild("bubbleShowUpPos");
			}
			if(cylinderShowUpPos==null)
			{
				cylinderShowUpPos = transform.FindChild("cylinderShowUpPos");
			}
			if(cylinderTargetPos==null)
			{
				cylinderTargetPos = transform.FindChild("cylinderTargetPos");
			}
			if(lockStartPos==null)
			{
				lockStartPos = canonBarrelTrans.FindChild("lockStartPos");
			}
			if(highScorePos==null)
			{
				highScorePos = transform.FindChild("highScorePos");
			}
			if(coinHomePos==null)
			{
				coinHomePos = transform.FindChild("coinHomePos");
			}
			if(peakHint==null)
			{
				peakHint = transform.FindChild("peakHint");
			}
			if(addBarrelValue==null)
			{
				addBarrelValue = transform.FindChild("addBarrelValue");
			}
		}


		// 初始化.
		public void InitCanonParam(int _canonID, 
		                           int _plyUniqueID,
		                           Transform _bulletCache, 
		                           float _fireGap,
		                           uint _plyScore, 
		                           int _bulletMulti,
		                           int _bulletPower,
		                           int _level,
		                           string _name,
		                           int _dir,
		                           int _shipColorID,
		                           int _bulletColorID,
		                           bool _plyFirePermit
		                           )
		{
			if(transform.localEulerAngles.z>170f && transform.localEulerAngles.z<190f)
			{
				upsideDown = true;
			}
			// 如果炮台在上面就更改玩家分数和子弹炮值得角度.
			if(upsideDown)
			{
				plyScoreLabel.transform.localEulerAngles = new Vector3(plyScoreLabel.transform.localEulerAngles.x, plyScoreLabel.transform.localEulerAngles.y, 180f);
				bulletPowerLabel.transform.localEulerAngles = new Vector3(bulletPowerLabel.transform.localEulerAngles.x, bulletPowerLabel.transform.localEulerAngles.y, 180f);
			}
			
			canonID = _canonID;
			dir = _dir;
			bulletCache = _bulletCache;
			allowFire = _plyFirePermit;
			fireGap = _fireGap;
			curBulletCount = 0;

			plyScore = _plyScore;
			plyUniqueID = _plyUniqueID;
			level = _level;
			plyName = _name;
			boatColorID = _shipColorID;
			bulletColorID = _bulletColorID;

			// 初始化子弹参数.
			curBulletParam = new BulletParameter();
			curBulletParam.bulletPower = _bulletPower;
			curBulletParam.bulletMulti = _bulletMulti;

			// 如果是真实玩家, 比如是机器人或者游客，那就把加炮这个按钮给去掉.
			UnShowFakeCanonObj();

			// 不显示暴机提示.
			ShowExploHint(false);

			// 显示玩家分数.
			SetPlyValue(plyScore);
			// 显示子弹 multi .
//			SetBulletMulti(curBulletParam.bulletMulti);
			// 显示子弹 power .
			proportion =(float) curBulletParam.bulletPower / CanonCtrl.Instance.maxBulletValue;
			SetBulletPower(proportion);

			// 初始化鱼死后给分的list.
			giveValueList.Clear();
		}

		// 如果不是真实玩家, 比如是机器人或者游客，那就把加炮这个按钮给去掉.
		void UnShowFakeCanonObj()
		{
			if(canonID!=CanonCtrl.Instance.realCanonID)
			{
//				addBarrelValue.gameObject.SetActive(false);
			}
		}

		public void initFireSpeed(float tempSpeed)
		{
			fireGap = tempSpeed;
		}


		// 全屏死后不许发泡，直到鱼回收了.
		public void AllowFire(bool _allow)
		{
			allowFire = _allow;
		}


		void Update ()
        {
#if UNITY_STANDALONE_WIN
			if(canonID == CanonCtrl.Instance.realCanonID)
			{				
				if(CanonCtrl.Instance.m_bFollowMouse)
				{
					C_S_SetDir_Normal(Input.mousePosition, false);
				}
			}
#endif

			//	炮火动画控制
			if (ph_anm_ctr.m_waitTime_ph >= ph_anm_ctr.m_ffps_ph && ph_anm_ctr.m_bIsOpen_ph)
			{
				if(ph_anm_ctr.paohuo != null){
					if(ph_anm_ctr.num_ph>=ph_anm_ctr.ani_num_ph){
						ph_anm_ctr.num_ph = 0;
						ph_anm_ctr.m_bIsOpen_ph = false;;
					}
					ph_anm_ctr.paohuo.spriteName = "huohua0_" + (ph_anm_ctr.num_ph);
					ph_anm_ctr.num_ph++;
					ph_anm_ctr.m_waitTime_ph = 0;
				}
			}
			ph_anm_ctr.m_waitTime_ph += Time.deltaTime;

			// 炮管动画控制
			if (pg_anm_ctr.m_waitTime_pg >= pg_anm_ctr.m_ffps_pg && pg_anm_ctr.m_bIsOpen_pg)
			{
				if(pg_anm_ctr.paoguan != null){
					if(pg_anm_ctr.num_pg >= pg_anm_ctr.ani_num_pg){
						pg_anm_ctr.num_pg = 0;
						pg_anm_ctr.m_bIsOpen_pg=false;
     					}
					pg_anm_ctr.paoguan.spriteName = "paoguan"+_curBulletLevel+"-"+(pg_anm_ctr.num_pg); 
					pg_anm_ctr.num_pg++;
					pg_anm_ctr.m_waitTime_pg = 0;
				}
			}
			pg_anm_ctr.m_waitTime_pg += Time.deltaTime;

			Check2GiveValue();

			// real player.
			if(canonID==CanonCtrl.Instance.realCanonID)
			{
				// 刚开始进来时候如果分数不够，就打开买分面板（当前不用这个买分界面）.
				// CheckHaveNoValueAndHaveGold();

				// 是否打开面板（当前不用这个买分界面）.
				if(CanonCtrl.Instance.OpeningAnyPanel())
				{
					return;
				}

				// 是否不许发泡.
				if(!allowFire)
				{
					return;
				}

				// 是否在切换场景.
				if(WaveCtrl.Instance.changingWave)
				{
					return;
				}

				// 在锁定
				if(CanonCtrl.Instance.inLockMode)
				{
					// 炮管方向跟着鱼.
					SetDir_Lock();
				}
				else 
				{
					// 炮管方向指向按下的屏幕方向.
					if(Input.GetMouseButton(0) && !Utility.IsMouseOverUI())
					{
						lastMouseDownPosition = Input.mousePosition; 
						C_S_SetDir_Normal(lastMouseDownPosition, false);
					}
				}

				// 是否有足够的分数.
				if(PlyHaveEnoughValue())
				{
					//原来的封装为函数
					GunFire( curBulletParam.bulletCost );
//					// 场景的子弹是否已经达到上限.
//					if(curBulletCount<CanonCtrl.Instance.maxBulletNum4OnePly)
//					{
//						// 如果在自动发泡.
//						if(CanonCtrl.Instance.autoFire)
//						{
//							// 间隔时间.
//							if(Time.time>=nextFireTargetTime)
//							{
//								// 获取鱼的 server id.
//								int _serverID = -1;
//								if(CanonCtrl.Instance.lockedFish!=null && CanonCtrl.Instance.inLockMode)
//								{
//									_serverID = CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().fishServerID;
//								}
//
//								// 发一炮, S-C-GunFire msg won't be send to the real player, so we fire here.
//								RealGunFire(curBulletParam.bulletCost, curBulletParam.bulletPower);
//
//								// 给 server 发送消息.
//								C_S_RealPly_GunFire(canonBarrelTrans.localEulerAngles.z, bulletShowUpPos.position, _serverID, curBulletParam.bulletCost, TimeCtrl.Instance.serverTime);
//								nextFireTargetTime = Time.time + fireGap;
//							}
//						}
//						else 
//						{
//							// 点击屏幕，并且不是点在按钮上.
//							if((Input.GetMouseButtonDown(0)) && !Utility.IsMouseOverUI())
//							{
//								// 获取鱼的 server id.
//								int _serverID = -1;
//								if(CanonCtrl.Instance.lockedFish!=null && CanonCtrl.Instance.inLockMode)
//								{
//									_serverID = CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().fishServerID;
//								}
//
//								// 发一炮, S-C-GunFire msg won't be send to the real player, so we fire here.
//								RealGunFire(curBulletParam.bulletCost, curBulletParam.bulletPower);
//								
//								// 给 server 发送消息.
//								C_S_RealPly_GunFire(canonBarrelTrans.localEulerAngles.z, bulletShowUpPos.position, _serverID, curBulletParam.bulletCost, TimeCtrl.Instance.serverTime);
//							}
//						}
//					}
				}
				// 不够分了.
				else 
				{
					if( plyScore > 0 )
					{
						//分不够将剩余分作为一炮发出
						GunFire( (int)plyScore );
					}
					else
					{
						if( GunFireBuyScore() )
						{
							//购买子弹
							if( CanonCtrl.Instance.BuyLastScore != 0 && CanonCtrl.Instance.autoBuyScore && BuyScoreAndAccount.Instance.AutoBuyScore_on)
							{
								BuyScoreAndAccount.Instance.AutoBuyScore();
							}
						}
					}

//					// 取消自动发泡状态.
//					if(CanonCtrl.Instance.autoFire)
//					{
//						CanonCtrl.Instance.SetAutoFire(false);
//					}
//					// 取消锁定状态.
//					if(CanonCtrl.Instance.inLockMode)
//					{
//						LockFish(false);
//					}
				}

				// 点击鼠标右键进行加分
// 				if(Input.GetMouseButtonDown (1)){
// 					C_S_GunPowerUp ();
// 				}
			}
		}

		//判断是否做发炮操作
		private bool GunFireBuyScore()
		{
			// 场景的子弹是否已经达到上限.
			if(curBulletCount<CanonCtrl.Instance.maxBulletNum4OnePly)
			{
				// 如果在自动发泡.
				if(CanonCtrl.Instance.autoFire)
				{
					// 间隔时间.
					if(Time.time>=nextFireTargetTime)
					{
						nextFireTargetTime = Time.time + fireGap;
						return true;
					}
				}
				else 
				{
					// 点击屏幕，并且不是点在按钮上.
					if((Input.GetMouseButton(0)) && !Utility.IsMouseOverUI())
					{
						return true;
					}
					// 按住空格,也算是发炮状态.不然按住空格不会自动购买子弹
					if(Input.GetKey(KeyCode.Space)){
						return true;
					}
				}
			}
			return false;
		}

		// 发送发炮函数
		private void SendGunFire( int bulletCost )
		{
			// 获取鱼的 server id.
			int _serverID = -1;
			if(CanonCtrl.Instance.lockedFish!=null && CanonCtrl.Instance.inLockMode)
			{
				_serverID = CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().fishServerID;
			}
			
			// 发一炮, S-C-GunFire msg won't be send to the real player, so we fire here.
			RealGunFire( bulletCost, curBulletParam.bulletPower );
			
			// 给 server 发送消息.
			C_S_RealPly_GunFire(canonBarrelTrans.localEulerAngles.z, bulletShowUpPos.position, _serverID, bulletCost, TimeCtrl.Instance.serverTime);
		}


		//炮台发炮
		private void GunFire( int bulletCost )
		{
			// 场景的子弹是否已经达到上限.
			if(curBulletCount<CanonCtrl.Instance.maxBulletNum4OnePly)
			{
				// 如果在自动发泡.
				if(CanonCtrl.Instance.autoFire)
				{
					// 间隔时间.
					if(Time.time>=nextFireTargetTime)
					{
						SendGunFire(bulletCost);
						nextFireTargetTime = Time.time + fireGap;
					}
				}
				else 
				{
					//长按鼠标  点击屏幕，并且不是点在按钮上.
					if(Input.GetMouseButton(0) && !Utility.IsMouseOverUI())
					{
						if(Time.time >= tempTouchTime)
						{
							SendGunFire(bulletCost);
							tempTouchTime = Time.time + fireGap;
						}
					}
					//按空格键发炮
					if(Input.GetKey(KeyCode.Space))
					{
						if(Time.time >= tempTouchTime)
						{
							SendGunFire(bulletCost);
							tempTouchTime = Time.time + fireGap;
						}
					}
				}
			}
		}
		
		// 显示暴机提示.
		public void ShowExploHint(bool _show)
		{
			if(_show)
			{
				if(!peakHint.gameObject.activeSelf)
				{
					peakHint.gameObject.SetActive(true);
				}
			}
			else 
			{
				if(peakHint.gameObject.activeSelf)
				{
					peakHint.gameObject.SetActive(false);
				}
			}
		}

		// 鱼死亡后加分给玩家的list. _timeLength 是延时多久给分.
		private List<FishDeadGiveValue> giveValueList = new List<FishDeadGiveValue>();
		public void AddValueList(int _addValue, float _timeLength)
		{
			FishDeadGiveValue _fdgv = new FishDeadGiveValue();
			_fdgv.addValue = _addValue;
			_fdgv.targetTime2GiveValue = Time.time + _timeLength;
			giveValueList.Add(_fdgv);
		}

		// 这个在update里面更新，时间到了就给分.
		void Check2GiveValue()
		{
			for(int i=0; i<giveValueList.Count; i++)
			{
				if(Time.time > giveValueList[i].targetTime2GiveValue)
				{
					AddPlyValue(giveValueList[i].addValue);
					giveValueList.Remove(giveValueList[i]);
				}
			}
		}


		// when pressing the account panel, we should give all the value to the player immediately incase when the account panel is showing, but the player's value haven't update yet.
		// 按下了结算 account 的按钮 ,把上面还在list里要给玩家的分数马上给玩家.
		public void GiveValue2PlyImmediate()
		{
			for(int i=0; i<giveValueList.Count; i++)
			{
				giveValueList[i].targetTime2GiveValue = 0f;
				if(Time.time > giveValueList[i].targetTime2GiveValue)
				{
					AddPlyValue(giveValueList[i].addValue);
					giveValueList.Remove(giveValueList[i]);
				}
			}
		}
		
		// 是否还够发一炮.
		bool PlyHaveEnoughValue()
		{
			if(plyScore>=(uint)curBulletParam.bulletCost)
			{
				return true;
			}
			return false;
		}

		// 发一炮减分.
		void PlyMinisOneBulletValue(int _cost)
		{
//			plyScore -= (uint)curBulletParam.bulletCost;
			plyScore -= (uint)_cost;
			if(canonID==CanonCtrl.Instance.realCanonID)
			{
				// 玩家真实发炮,刷新倒计时
				OverTime.Instance.RefreshWaitTime();
				// 判断是否爆机
				if(plyScore<CanonCtrl.Instance.peak)
				{
					ShowExploHint(false);
				}
			}
			SetPlyValueLabel();
		}

		// 给玩家加减分数.
		void AddPlyValue(int _add)
		{
			AddPlyValue((uint)_add);
		}
		void AddPlyValue(uint _add)
		{
			plyScore += _add;
			if(canonID==CanonCtrl.Instance.realCanonID && plyScore>=CanonCtrl.Instance.peak)
			{
				ShowExploHint(true);
			}
			SetPlyValueLabel();
		}

		// 更新界面上显示的分数.
		public void SetPlyValue(uint _plyValue)
		{
			plyScore = _plyValue;
			if(plyScore>=CanonCtrl.Instance.peak)
			{
				ShowExploHint(true);
			}
			SetPlyValueLabel();
		}
		void SetPlyValueLabel()
		{
//			plyScoreLabel.ApplyValue(plyScore, 0);
			plyScoreLabel.text = plyScore.ToString ();			
			// 检测是否显示暴机提示
			if (plyScore >= CanonCtrl.Instance.peak)
			{
				ShowExploHint(true);
			}else
			{
				ShowExploHint(false);
			}

			// 上下分面板的上分分数显示
			if(canonID==CanonCtrl.Instance.realCanonID)
			{
//				GameObject temp = CanonCtrl.Instance.scoreWindow.transform.FindChild ("front_panel").FindChild ("field0").FindChild ("score_label").gameObject;
                if (CanonCtrl.Instance.ScoreWindow_plyScoreLabel != null)
                {
                    CanonCtrl.Instance.ScoreWindow_plyScoreLabel.text = plyScore.ToString();
				}
			}

#if !UNITY_STANDALONE_WIN
//			if(canonID == CanonCtrl.Instance.realCanonID)
//			{
//				Transform tempScoreWindow = GameObject.Find("ScoreWindow").transform;
//				Transform tempScore =  GameObject.Find("ScoreWindow").transform.FindChild("front_panel").FindChild("field0").FindChild("score_label");
//				if(tempScoreWindow.gameObject.activeSelf && tempScore != null) tempScore.GetComponent<UILabel>().text = plyScore.ToString();
//			}
#endif
		}
		
		// 刚开始进来时候如果分数不够，就打开买分面板（当前不用这个买分界面）.
		void CheckHaveNoValueAndHaveGold()
		{
			if(Input.GetMouseButtonDown(0) && !Utility.IsMouseOverUI())
			{
				if(plyScore<=0)
				{
					BuyScorePanelCtrl.Instance.ShowBuyScorePanel();
				}
			}
		}

		// 本地按下屏幕，更改炮管的rotation, 
		void C_S_SetDir_Normal(Vector3 _pos, bool _s_c)
		{
			Vector3 _upward = UICam.ScreenToWorldPoint(_pos) - canonBarrelTrans.position;
			_upward = new Vector3(_upward.x, _upward.y, 0f);
			canonBarrelTrans.rotation = Quaternion.LookRotation(Vector3.forward, _upward);
		}
		// 设置这个炮台的角度, 这个值已经在本地转换过了.
		void S_C_SetDir_Normal(float _eulerZ)
		{
			canonBarrelTrans.localEulerAngles = new Vector3(0f, 0f, _eulerZ);
		}
		
		// 锁定时候炮管方向跟着鱼.
		void SetDir_Lock()
		{
			// 如果仍在锁定，并且鱼还在锁定状态.
			if(CanonCtrl.Instance.lockedFish!=null && CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().locking)
			{
				// 实时设置炮管rotation.
				Vector3 _upward = CanonCtrl.Instance.lockedFish.position - canonBarrelTrans.position;
				//_upward = new Vector3(_upward.x, _upward.y, 0f);
                _upward.x = _upward.x / gameObject.transform.parent.localScale.x / gameObject.transform.parent.parent.localScale.x / gameObject.transform.parent.parent.parent.localScale.x;
                _upward.y = _upward.y / gameObject.transform.parent.localScale.y / gameObject.transform.parent.parent.localScale.y / gameObject.transform.parent.parent.parent.localScale.y;
                canonBarrelTrans.rotation = Quaternion.LookRotation(Vector3.forward, _upward);
				// 实时设置锁定线的rotation.
				Quaternion temp = Quaternion.LookRotation(Vector3.forward, _upward);
				//LockCtrl.Instance._line.rotation = new Quaternion(temp.x,temp.y,temp.w,temp.z);
                LockCtrl.Instance._line.rotation = new Quaternion(temp.x, temp.y, temp.w, -temp.z);

				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of)
                {
                    for (int i = 0; i < LockCtrl.Instance._line.childCount; i++)
                    {
                        LockCtrl.Instance._line.GetChild(i).GetComponent<UISprite>().transform.rotation = new Quaternion(0, 0, 1.0f, 0);
                        
                    }
				}else
                {
                    for (int i = 0; i < LockCtrl.Instance._line.childCount; i++)
                    {
                        LockCtrl.Instance._line.GetChild(i).GetComponent<UISprite>().transform.rotation = new Quaternion(0, 0, 0, 1.0f);
                    }
				}
			}
			else 
			{
				// 获取下一跳鱼.
				if(Time.time>targetTime2CheckFishMoveOutViewport)
				{
					LockFish(true);
					targetTime2CheckFishMoveOutViewport = Time.time + 0.1f;
				}
			}
		}

		// 锁定或取消锁定一条鱼.
		public void LockFish(bool _lock)
		{
			if(_lock)
			{
				// 获取一条合适的可以被锁定的鱼.
				CanonCtrl.Instance.lockedFish = FishCtrl.Instance.GetALockFish();
				if(CanonCtrl.Instance.lockedFish!=null)
				{
					// 通知这条鱼，它被锁定了.
					CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().locking = true;
					// 当前在锁定状态.
					CanonCtrl.Instance.inLockMode = true;
					// 通知这个玩家的锁定线去锁定这条鱼.
					LockCtrl.Instance.lockCtrls[canonID].ShowLockRend(CanonCtrl.Instance.lockedFish);
				}
			}
			else 
			{
				// 通知这条鱼，它不被锁定了.
				CanonCtrl.Instance.lockedFish.GetComponent<SingleFish>().locking = false;
				CanonCtrl.Instance.lockedFish = null;
				// 当前不在锁定状态.
				CanonCtrl.Instance.inLockMode = false;
				// 锁定线停止锁定.
				LockCtrl.Instance.lockCtrls[canonID].StopLock();
			}
		}



		// 真实玩家发一个子弹. only run by real gun.
		public void RealGunAddBulletNumInScene(int _num)
		{
			curBulletCount += _num;
		}
		
		// 买分/买子弹.
		public void BuyBullet(int _canonID, uint _buyScore)
		{
			AddPlyValue(_buyScore);
		}

		// 真实玩家发一炮..
		public void RealGunFire (int _cost, int _power)
		{
			// 创建一个子弹.
			Transform _bullet = Factory.Create(curBulletPrefab, bulletShowUpPos.position, bulletShowUpPos.rotation);
			_bullet.parent = bulletCache;
			_bullet.localScale = Vector3.one;
			_bullet.GetComponent<SingleBullet>().Init(canonID, _cost, _power, CanonCtrl.Instance.lockedFish, -1);

			// 减分.
			PlyMinisOneBulletValue(_cost);

			// 该玩家场景中子弹数目加1.
			if(canonID==CanonCtrl.Instance.realCanonID)
			{
				RealGunAddBulletNumInScene(1);
			}

			//播放发炮动画.
			PlayFireAni();
		}

		// 本地到服务器发送一条发泡消息.(自己发炮信息)
		private void C_S_RealPly_GunFire(float _eulerZ, Vector3 _firePos, int _serverId, int _bulletCost, uint _serverTime)
		{
			// 把发泡位置和角度转换为服务器认识的（1600*900）的位置和角度.
			_firePos = Utility.C_S_TransformFirePos(_firePos); 
			_eulerZ  = Utility.C_S_TranformFireEulerZ(_eulerZ); 

			MessageHandler.Instance.C_S_GunFire(_eulerZ, _firePos, _serverId, _bulletCost, _serverTime);
		}

		// 收到服务器到本地发送一条发炮消息.(其他玩家发炮信息)
		// _posX and _posY those get from server are 1600*900 mode, we must transform them here, and S-C-GunFire msg won't be send to the real player anymore.
		public void S_C_FakePly_GunFire(int _canonID, float _eulerZ, float _posX, float _posY, int _serverId, int _bulletCost, uint _serverTime)
		{
			// 转换为本地设备的角度.
			_eulerZ = Utility.S_C_TranformFireEulerZ(_eulerZ);
			// unity 和 服务器所记录的角度是反过来的.
			S_C_SetDir_Normal(-_eulerZ);

			//其他玩家只产生子弹
			Transform KillFish = null;

			// 创建子弹.
			Transform _bullet = Factory.Create(curBulletPrefab, bulletShowUpPos.position, bulletShowUpPos.rotation);
			_bullet.parent = bulletCache;
			_bullet.localScale = Vector3.one;
			_bullet.GetComponent<SingleBullet>().Init(canonID, 0, 0,KillFish/*CanonCtrl.Instance.lockedFish*/, -1);

			// 减分.
			if(plyScore>=_bulletCost)
			{
				PlyMinisOneBulletValue(_bulletCost);
			}
			else 
			{
				Debug.Log(" ========================================= FakeGunFire .. plyScore = "+plyScore +" / bulletCost = "+_bulletCost);
			}
			
			//播放发炮动画.
			PlayFireAni();
		}

		// 服务器通知机器人需要发泡.
		public void S_C_RobotGunFireNotify(int _serverID)
		{
			// 是否不许发泡.
			if(!allowFire)
			{
				return;
			}
			// 是否在切换场景.
			if(WaveCtrl.Instance.changingWave)
			{
				return;
			}

			SingleFish _sf 	 = FishCtrl.Instance.GetRobotFireFish(_serverID);
			Vector3 _fishPos = Vector3.zero; 
			
			if(_sf==null)
			{
				// 鱼不见了，那就只能打中间了.
				_fishPos = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
			}
			else 
			{
				// 鱼还在就获取位置.
				_fishPos = _sf.transform.localPosition;
			}

			// 一下是模拟并获取炮管要打这条鱼的角度.
			Vector3 _robot2FireDir 	= UICam.ScreenToWorldPoint(_fishPos) - canonBarrelTrans_robot.position;
			_robot2FireDir 			= new Vector3(_robot2FireDir.x, _robot2FireDir.y, 0f);
			canonBarrelTrans_robot.rotation = Quaternion.LookRotation(Vector3.forward, _robot2FireDir);

			// 转换服务器认识的发泡位置和角度.
			Vector2 _firePos_robot 	= Utility.C_S_TransformFirePos(bulletShowUpPos_robot.position); 
			float 	euelrZ_robot 	= Utility.C_S_TranformFireEulerZ(canonBarrelTrans_robot.localEulerAngles.z); 
			
			MessageHandler.Instance.C_S_RobotGunFire(canonID, euelrZ_robot, _firePos_robot, _serverID, curBulletParam.bulletCost, TimeCtrl.Instance.serverTime);
		}

		// 机器人发一炮.
		public void S_C_RobotGunFire(float _eulerZ, float _posX, float _posY, int _serverId, int _bulletCost, uint _serverTime, uint _monitorCanonID)
		{
			// 是否不许发泡.
			if(!allowFire)
			{
				return;
			}
			// 是否在切换场景.
			if(WaveCtrl.Instance.changingWave)
			{
				return;
			}

			//机器人锁定打鱼
			Transform lockedFish = null;
//			SingleFish _sf 	 = FishCtrl.Instance.GetRobotFireFish(_serverId);
//			if(_sf != null)
//			{
//				lockedFish = _sf.transform;
//			}

			// 转换为本地设备的角度.
			_eulerZ = Utility.S_C_TranformFireEulerZ(_eulerZ);
			S_C_SetDir_Normal(-_eulerZ);

			// 创建子弹.
			Transform _bullet = Factory.Create(curBulletPrefab, bulletShowUpPos.position, bulletShowUpPos.rotation);
			_bullet.parent = bulletCache;
			_bullet.localScale = Vector3.one;


			
			// 服务器通知 realCanonID 来检查鱼的碰撞.
			if(_monitorCanonID==CanonCtrl.Instance.realCanonID)
			{
				// this canonID is robot id, monitor canon is the real Canon who's id is equal to the monitor CanonID to detect the robot's bullet collision.
				// 那就由这个玩家来检查这个子弹碰到哪条鱼.
				_bullet.GetComponent<SingleBullet>().Init(canonID, _bulletCost, curBulletParam.bulletPower, lockedFish/* CanonCtrl.Instance.lockedFish*/, (int)_monitorCanonID);
			}
			else 
			{
				_bullet.GetComponent<SingleBullet>().Init(canonID, _bulletCost, curBulletParam.bulletPower, lockedFish/*CanonCtrl.Instance.lockedFish*/, -1);
			}

			// 减分.
			if(plyScore>=_bulletCost)
			{
				PlyMinisOneBulletValue(_bulletCost);
			}
			else 
			{
				Debug.Log(" ========================================= S_C_RobotGunFire .. plyScore = "+plyScore +" / bulletCost"+_bulletCost);
			}
			
			//播放发炮动画.
			PlayFireAni();
		}

		
		// 播放炮台开炮动作.
		void PlayFireAni()
		{
			ph_anm_ctr.m_bIsOpen_ph =true;
			pg_anm_ctr.m_bIsOpen_pg =true;

//			if(barrelAnimator!=null)
//			{
//				barrelAnimator.SetTrigger("fire");
//			}
	
//			if(stationAnimator!=null)
//			{
//				stationAnimator.SetTrigger("fire");
//			}
//			if(fireAnimator != null){
//				fireAnimator.SetTrigger ("fire");
//			}
		}

		// 点击了加炮的按钮.
		public void C_S_GunPowerUp()
		{
			if(CanonCtrl.Instance.OpeningAnyPanel())
			{
				return;
			}
			int temp = curBulletParam.bulletPower + CanonCtrl.Instance.addBulletValueStep;
			if(temp>CanonCtrl.Instance.maxBulletValue)
			{
				if(curBulletParam.bulletPower==CanonCtrl.Instance.maxBulletValue)
				{
					temp = CanonCtrl.Instance.minBulletValue;
				}
				else 
				{
					temp = CanonCtrl.Instance.maxBulletValue;
				}
			}

			// 更新加炮.
                                                                                                                            			// we add gun power up value to this player, because s-c wont send back to this player again, just send to other players.
			GunPowerUp(canonID, temp);
			MessageHandler.Instance.C_S_GunPowerUp((uint)temp);
		}

		//减炮按钮
		public void C_S_GunPowerDown()
		{
			if(CanonCtrl.Instance.OpeningAnyPanel())
			{
				return;
			}
			int temp = curBulletParam.bulletPower - CanonCtrl.Instance.addBulletValueStep;
			if(temp < CanonCtrl.Instance.minBulletValue)
			{
				if(curBulletParam.bulletPower==CanonCtrl.Instance.minBulletValue)
				{
					temp = CanonCtrl.Instance.maxBulletValue;
				}
				else 
				{
					temp = CanonCtrl.Instance.minBulletValue;
				}
			}

			GunPowerUp(canonID, temp);
			MessageHandler.Instance.C_S_GunPowerUp((uint)temp);
		}

		// 更新加炮.
		public void GunPowerUp(int _canonID, int _bulletValue)
		{
//			CanonCtrl.Instance.paozhi = _bulletValue;
			// 当前炮值/最大炮值,取到百分比,用于更新炮台和子弹.
			proportion =(float) _bulletValue / CanonCtrl.Instance.maxBulletValue;
			SetBulletPower(proportion);
			AudioCtrl.Instance.ChangeGun();
		}

		// 更新子弹的 power， cost 等.
		public void SetBulletPower(float _power)
		{
			// power.
			float _temp;
			_temp = _power * CanonCtrl.Instance.maxBulletValue;
			curBulletParam.bulletPower =(int) _temp;
			// cost.
			curBulletParam.bulletCost = curBulletParam.bulletPower * curBulletParam.bulletMulti;

			// 更新显示的power值.
//			bulletPowerLabel.ApplyValue(curBulletParam.bulletPower, 0);
			bulletPowerLabel.text = curBulletParam.bulletPower.ToString ();

			// 通过炮值来更换子弹类型, 网格类型, 炮管类型.
			SetBullet_Net_Barrel_ByBulletValue(_power);
		}

		// 通过炮值来更换子弹类型, 网格类型, 炮管类型和炮火类型.
		private int lastBulletLevel = -1;
		private Transform lastBarrel;
		private Transform lastFire;
		private int _curBulletLevel;
		public void SetBullet_Net_Barrel_ByBulletValue(float _value)
		{
			_curBulletLevel =0;
			for(int i=CanonCtrl.Instance.bullet_Net[canonID].level.Count-1; i>=0; i--)
			{
				if(_value >= CanonCtrl.Instance.bullet_Net[canonID].level[i])
				{
					_curBulletLevel = i;
					break;
				}
			}

			// 更换子弹和网格.
			curBulletPrefab = CanonCtrl.Instance.bullet_Net[canonID].bulletTransform[_curBulletLevel];
			curNetPrefab	= CanonCtrl.Instance.bullet_Net[canonID].netTransform[_curBulletLevel];

			// 如果子弹等级更改了，那么就要更换炮管.
			if(_curBulletLevel!=lastBulletLevel)
			{
				if(lastBarrel!=null)
				{
//					Factory.Recycle(lastBarrel);
					Destroy (lastBarrel.gameObject);
				}
				if(lastFire != null){
//					Factory.Recycle(lastFire);
					Destroy (lastFire.gameObject);
				}
//				if(barrelAnimator==null)
//				{
//					lastBarrel = Factory.Create(CanonCtrl.Instance.canon_Barrel[canonID].canonBarrelPrefab[_curBulletLevel], barrelAnchor.transform.position, barrelAnchor.transform.rotation);
//				}
//				else 
//				{
//					lastBarrel = Factory.Create(CanonCtrl.Instance.canon_Barrel[canonID].canonBarrelPrefab[_curBulletLevel], barrelAnimator.transform.position, barrelAnimator.transform.rotation);
//				}
				lastBarrel = Factory.Create(CanonCtrl.Instance.canon_Barrel[canonID].canonBarrelPrefab[_curBulletLevel], barrelAnchor.transform.position, barrelAnchor.transform.rotation);
//				Vector3 _localScale = lastBarrel.localScale;
				lastBarrel.parent = barrelAnchor.parent;
//				lastBarrel.localScale = _localScale;
				lastBarrel.localScale =  new Vector3(0.01f,0.01f,0.01f);
				pg_anm_ctr.paoguan = lastBarrel.GetComponent<UISprite>();
				pg_anm_ctr.paoguan.spriteName = "paoguan"+_curBulletLevel+"-0";
//				pg_anm_ctr.paoguan.depth = 3;
//				barrelAnimator = lastBarrel.GetComponent<Animator>();

				// 获取炮火生成位置
				if(CanonCtrl.Instance.firePrefabs.Length != 0){
					lastFire = Factory.Create (CanonCtrl.Instance.firePrefabs[_curBulletLevel],Vector3.zero,Quaternion.identity);
					Vector3 _localposition = CanonCtrl.Instance.firePrefabs[_curBulletLevel].localPosition;
					lastFire.parent = barrelAnchor.parent.FindChild ("bulletShowUpPos").transform;

					lastFire.localPosition = _localposition;
					lastFire.localRotation = new Quaternion();
					lastFire.localScale = new Vector3(0.01f,0.01f,0.01f);
					ph_anm_ctr.paohuo  = lastFire.GetComponent<UISprite>();
					//fireAnimator = lastFire.GetComponent<Animator>();
				}else{
					return;
				}
			}
			// 记录上次的子弹等级.
			lastBulletLevel = _curBulletLevel;
		}

		
		// 按下炮管 multi 按钮(右边那个加号按钮)（暂时还没用到）.
		public void C_S_GunMulti()
		{
			if(CanonCtrl.Instance.OpeningAnyPanel())
			{
				return;
			}
			int temp = curBulletParam.bulletMulti;
			temp += 1;
			if(temp>=CanonCtrl.Instance.maxBulletMulti)
			{
				temp = 1;
			}
			MessageHandler.Instance.C_S_GunPowerMulti(temp);
		}
		// 更新炮管 multi(右边那个加号按钮)（暂时还没用到）.
		public void S_C_GunPowerMulti(int _canonID, int _multi)
		{
			SetBulletMulti(_multi);
		}

		// 设置炮管右边的那个multi.（暂时还没用到）.
		void SetBulletMulti(int _bulletMulti)
		{
			curBulletParam.bulletMulti = _bulletMulti;
			curBulletParam.bulletCost = curBulletParam.bulletPower * curBulletParam.bulletMulti;
			bulletMultiLabel.ApplyValue(_bulletMulti, 0);
		}

		// 创建一个网格.
		public void CreateOneNet(Vector3 _pos, Quaternion _rot)
		{
			Transform _net = Factory.Create(curNetPrefab, _pos, _rot);
			_net.parent = CanonCtrl.Instance.netCache;
		}

		/*void OnGUI()
		{
			if(!MessageHandler.Instance.showAllGui)
			{
				return;
			}
			GUILayout.Space(400f + canonID*30f);
			GUILayout.Label("cost = "+curBulletParam.bulletCost +" / power = "+ curBulletParam.bulletPower);
			GUILayout.Label("curBulletCount = "+curBulletCount);
		}*/
	}
}