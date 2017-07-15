using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.Fishing
{
	// 炮台的位置大小角度.
	[Serializable]
	public class PlayerParameter
	{
		public Vector3 pos;
		public Vector3 rot;
		public Vector3 scale;
		public int barrelID = 0;
	}

	// 炮台的预设，每个炮值下的炮管预设.
	[Serializable]
	public class Canon_Barrel_Params
	{
		public Transform canonPrefab;
		public List<int> canonBarrelLevel = new List<int>();
		public List<Transform> canonBarrelPrefab = new List<Transform>();
	}

	// 每个炮值下的子弹预设，网格预设.
	[Serializable]
	public class BulletType_NetType
	{
		public List<float> level = new List<float>();
		public List<Transform> bulletTransform = new List<Transform>();
		public List<Transform> netTransform = new List<Transform>();
	}

    // phone版按钮预设
    [Serializable]
    public class Phone_Left_Button
    {
        public Transform lockButton;
        public Transform speedUpButton;
        public Transform autoFireButton;
    }

	// 用来创建炮台.
	public class CanonCtrl : MonoBehaviour 
	{
		public static CanonCtrl Instance;
		private int [] deskTypeArray = new int[8]{1,2,3,4,4,6,8,10};

		// 真实玩家的id.
		[HideInInspector] public int realCanonID = 1;
		// 真实玩家的unique id.
		[HideInInspector] public int realPlyUniqueID;

		// 机台类型.
		[HideInInspector] public int deskType = 6;
		// 房间比例.
		[HideInInspector] public int roomMulti = 1;
		// 一币几分.
		[HideInInspector] public int oneGoldScore = 100;
		// 暴机峰值.
		public uint peak = 99999999;

		// 最小最大子弹分数.
		[HideInInspector] public int minBulletValue = 100;
		[HideInInspector] public int maxBulletValue = 1000;
		// 加炮幅度.
		[HideInInspector] public int addBulletValueStep = 100;
		// 子弹倍数.
		[HideInInspector] public int maxBulletMulti = 10;

		// 金币数量.
		[HideInInspector] public int goldValue;
		// Voucher.
		[HideInInspector] public int plyVoucher;

		// 发泡间隔.
		public float fireGap = 0.25f;
        public float fireGapSpeedUp = 0.125f;
		// 一个玩家在场景中最多的子弹数.
		public int maxBulletNum4OnePly = 10;

		// 玩家名字 label.
		private UILabel plyNameLabel;
		
		// 显示金币的数字.
		private GameObject goldValueLabel;

		// 所有玩家是否允许发泡.
		private bool allowAllPlyFire = true;

		// 背包，button 等 ui.
		public Transform backage;
		
		// 买分分值.
		public int [] buyBulletPowerScore;
		
		// 炮台的预设，每个炮值下的炮管预设.
		public List <Canon_Barrel_Params> canon_Barrel = new List<Canon_Barrel_Params>();

		// 炮台的父节点.
		private Transform canonCache;

		// 挂在每一个炮台下的独立脚本.
		[HideInInspector] public SingleCanon [] singleCanonList;

		// 每个炮值下的子弹和网格类型.
		public List<BulletType_NetType> bullet_Net;

		// 不同玩家下，炮台的位置大小角度.
		public List<PlayerParameter> plyParameters_10;
		public List<PlayerParameter> plyParameters_8;
		public List<PlayerParameter> plyParameters_6;
		public List<PlayerParameter> plyParameters_4;
		public List<PlayerParameter> plyParameters_2;
		public List<PlayerParameter> plyParameters_1;
		
		// 记录炮台的方向，0为底，1为右，2,为顶，3为左.
		[HideInInspector] public int [] dirArray;
		// 炮台的适配比例.
		private Vector3 [] canonScaleFitResolRatio;

		
		// 网格的父节点.
		public Transform netCache;

		// 自动发泡按钮.
		private Transform autoFireButton;
		// 自动发泡状态.
		[HideInInspector] public bool autoFire;
		// 锁定按钮.
        private Transform lockButton;
        // 加速按钮.
        private Transform speedUpButton;

        //  手机版锁定按钮的附加图片
        public GameObject lockButton_img;
        public GameObject autoButton_img;
        public GameObject speedUpButton_img;
		
		// 是否在锁定状态.
		[HideInInspector] public bool inLockMode = false;
		// 真实玩家锁定的鱼.
		[HideInInspector] public Transform lockedFish = null;
		//锁定鱼的最小倍率
		public int minLockMulti = 20;

		// 结算 panel.
		private Transform accountPanel;
		// 杀死的每条鱼的数量.
		[HideInInspector] public int [] fishAccount;
		// 是否打开结算 account.
		[HideInInspector] public bool openingAccount = false;
		// 是否已经接受到 server 发来的 account 的消息.
		[HideInInspector] public bool S_C_AccountMsgHaveReceive = false;

		// 上一次购买的分数
		[HideInInspector] public int BuyLastScore = 0;

		// account面板的鱼数量和捕鱼总分
		[HideInInspector] public int account_shuliang= 0;
		[HideInInspector] public int account_zongfen = 0;
		// 上下分面板
		public GameObject scoreWindow;
        // 上下分面板显示玩家分数(phone版本)
        public UILabel ScoreWindow_plyScoreLabel;
		// 获取当前子弹炮值,方便用于一网打尽和全屏杀鱼的统计分数.(和其他无关)
		[HideInInspector] public int singlecanon_power = 1000;
		// 是否需要旋转Camera(使得任意玩家的炮台都处于屏幕下方)
		[HideInInspector] public bool turn_screen = false;
		// 是否旋转屏幕的开关.
		public bool turn_screen_on_of;
		// 是否自动购买子弹的开关.
		public bool autoBuyScore;
        // 锁定按钮开关
         public bool Locking_off = false;
		// 显示当前玩家位置(用于15s没操作时候显示的标识)
		[HideInInspector] public GameObject playerLogo;
		// 炮火的prefabs
		public Transform[] firePrefabs;
        // 手机版左侧按钮
        public Phone_Left_Button m_pPhone_Left_Button;
        // 左右两边按钮父节点
        public GameObject m_gLeftButton;
        public GameObject m_gRightButton;
        // 按钮面板控制箭头
        public GameObject m_gAllButtonBG;
		// 炮管跟随鼠标状态
		public bool m_bFollowMouse = true;

		void Awake()
		{
			Instance = this;
			GameObject _canonCache = GameObject.Find("CanonCache");
			if(_canonCache==null)
			{
				_canonCache = new GameObject();
				_canonCache.name = "CanonCache";
				_canonCache.transform.parent = GameObject.Find("Camera").transform;
				_canonCache.transform.localScale = Vector3.one;
				_canonCache.AddComponent<UIAnchor>().side = UIAnchor.Side.BottomLeft;
			}
			canonCache = _canonCache.transform;

			// 创建 10 个足够多.
			dirArray = new int[10];
			singleCanonList = new SingleCanon[10];
			canonScaleFitResolRatio = new Vector3[10];

			// 以下获取背包系统.
			if(backage==null)
			{
				backage = GameObject.Find("backage").transform;
			}
#if !UNITY_STANDALONE_WIN
			if(lockButton==null)
			{
				//lockButton = backage.FindChild("lockButton");
                lockButton = m_pPhone_Left_Button.lockButton;
			}
			if(speedUpButton==null)
			{
				//speedUpButton = backage.FindChild("speedUpButton");
                speedUpButton = m_pPhone_Left_Button.speedUpButton;
			}
			if(autoFireButton==null)
			{
				//autoFireButton = backage.FindChild("autoFireButton");
                autoFireButton = m_pPhone_Left_Button.autoFireButton;
			}
#endif
			if(accountPanel==null)
			{
				accountPanel = backage.FindChild("accountPanel");
			}
			if(plyNameLabel==null)
			{
				plyNameLabel = backage.FindChild("plyNameLabel").GetComponent<UILabel>();
			}
			if(goldValueLabel==null)
			{
				//goldValueLabel = backage.FindChild("gold").GetChild(0).GetComponent<UILabel>();
				goldValueLabel = backage.FindChild("gold").gameObject;
			}

			// 结算 account 的大小为 0（当前不用这个结算界面）..
//			accountPanel.localScale = Vector3.zero;

			// 重置 account 里杀死鱼的数量
			ClearAccountKillFishNum();

			// 重置 account 里的 金币数 和 豆豆数量
			SetTempGoldAndBeansValue(0,0);
		}

		void Start()
		{
			autoBuyScore = Fishing.Instance.AutoBuyScore;//读取是否打完子弹时自动上分

			defaulFireSpeed = fireGap;//发炮默认速度

			fishAccount = new int[FishCtrl.Instance.fish.Count];

            // 适配各种房间的玩家座位(现用NGUI控制,可忽略)
// 			for(int i=0; i<plyParameters_10.Count; i++)
// 			{
// 				plyParameters_10[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_10[i].pos.x), 
// 				                                      Utility.TransformPixelY(plyParameters_10[i].pos.y), 
// 				                                      plyParameters_10[i].pos.z);
// 			}
// 			for(int i=0; i<plyParameters_8.Count; i++)
// 			{
// 				Vector3 vec = plyParameters_8[i].pos;
// 				plyParameters_8[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_8[i].pos.x), 
// 				                                     Utility.TransformPixelY(plyParameters_8[i].pos.y),
// 				                                     plyParameters_8[i].pos.z);
// 
// 				Debug.Log("transform:"+vec.ToString()+"  "+plyParameters_8[i].pos.ToString());
// 			}
// 			for(int i=0; i<plyParameters_6.Count; i++)
// 			{
// 				plyParameters_6[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_6[i].pos.x), 
// 				                                     Utility.TransformPixelY(plyParameters_6[i].pos.y), 
// 				                                     plyParameters_6[i].pos.z);
// 				plyParameters_6[i].pos = plyParameters_6[i].pos;
// 			}
// 			for(int i=0; i<plyParameters_4.Count; i++)
// 			{
// 				plyParameters_4[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_4[i].pos.x), 
// 				                                     Utility.TransformPixelY(plyParameters_4[i].pos.y), 
// 				                                     plyParameters_4[i].pos.z);
// 			}
// 			for(int i=0; i<plyParameters_2.Count; i++)
// 			{
// 				plyParameters_2[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_2[i].pos.x), 
// 				                                     Utility.TransformPixelY(plyParameters_2[i].pos.y),
// 				                                     plyParameters_2[i].pos.z);
// 			}
// 			for(int i=0; i<plyParameters_1.Count; i++)
// 			{
// 				plyParameters_1[i].pos = new Vector3(Utility.TransformPixelX(plyParameters_1[i].pos.x), 
// 				                                     Utility.TransformPixelY(plyParameters_1[i].pos.y), 
// 				                                     plyParameters_1[i].pos.z);
// 			}

			InvokeRepeating ("Collect", 5f, 5f);

            // 不使用适配,通过NGUI自动适配
			//InitBackage();
			//InitBulletCache();
			//InitNetCache();
			//InitCanonScale ();
		}


		void OnDestroy()
		{
			Instance = null;
			System.GC.Collect ();
			Resources.UnloadUnusedAssets ();
		}

		void Collect()
		{
			/*System.GC.Collect ();
			Resources.UnloadUnusedAssets ();*/
		}

		public void Init(
						 int  jitaileixing,		
					     int  toubibili,		
					     int  gameBeilv,		
					     int  powerMin,			
					     int  powerMax,			
					     int  powerStep,		
					     int  powerMultiMax,	
					     uint baoji,			
					     uint serverTime,		
					     int  mapIndex,			
					     float mapTime,			
						 int  selfChair,
						 int  selfUserId,	
						 int  selfGold,	
						 int  selfVoucher
						)
		{
			deskType   			= deskTypeArray[jitaileixing];
			oneGoldScore 		= toubibili;
			roomMulti 			= gameBeilv;
			minBulletValue 	 	= powerMin;
			maxBulletValue  	= powerMax;
			addBulletValueStep  = powerStep;
			maxBulletMulti 		= powerMultiMax;
			peak				= baoji;
			realCanonID			= selfChair;
			realPlyUniqueID		= selfUserId;
			goldValue			= selfGold;
			plyVoucher			= selfVoucher;

			// 初始化本地时间.
			TimeCtrl.Instance.StartCountDown(serverTime);
			Debug.Log("serverTime = "+serverTime +" / mapTime = "+mapTime);
			// 初始化背景.
			WaveCtrl.Instance.S_C_Init(serverTime, mapIndex, mapTime);
			
			// 初始化背景音乐.
			AudioCtrl.Instance.InitBG(mapIndex);

			Debug.LogWarning("4 CanonCtrl Init: deskType = "+deskType +" / goldValue = "+goldValue+" / realCanonID = "+realCanonID+" / realPlyUniqueID = "
			                 +realPlyUniqueID+" / mapIndex = "+mapIndex);
		}

		// 适配背包(UI Button).
		void InitBackage()
		{
//			backage.transform.localScale = new Vector3(backage.transform.localScale.x*Utility.device_to_1600x900_ratio.x, backage.transform.localScale.y*Utility.device_to_1600x900_ratio.y, 1f);
		}
		// 适配子弹.
		void InitBulletCache()
		{
//			transform.localScale = new Vector3(transform.localScale.x*Utility.device_to_1600x900_ratio.x, transform.localScale.y*Utility.device_to_1600x900_ratio.y, 1f);
		}
		// 适配网格.
		void InitNetCache()
		{
			// 网格的大小
//			netCache = new GameObject().transform;
//			netCache.name = "netCache";
//			netCache.localScale = new Vector3(1,1,1);

			/*
			netCache.transform.localScale = new Vector3(netCache.transform.localScale.x*Utility.device2ServerRatio.x, netCache.transform.localScale.x*Utility.device2ServerRatio.y, 1f);
			*/
		}
		// 适配炮台.
		void InitCanonScale()
		{
			for(int i=0; i<canonScaleFitResolRatio.Length; i++)
			{
				canonScaleFitResolRatio[i] = new Vector3(Utility.device_to_1600x900_ratio.x, Utility.device_to_1600x900_ratio.y, 1f);
			}
		}

		// 玩家加入.
		public void S_C_UserJoin(int _canonID, int _plyUniqueID, uint _plyScore, int _level, string _name, int _shipColorID, int _bulletColorID, int _bulletPower, int _bulletMulti)
		{
			Vector3 _pos = Vector3.zero;
			Vector3 _rot = Vector3.zero;
			Vector3 _scale = Vector3.zero;
			int dir = -1;
			int _barrelID = 0;
			switch(deskType)
			{
			case 10:
				_pos = plyParameters_10[_canonID].pos;
				_rot = plyParameters_10[_canonID].rot;
				// 适配炮台大小.
				_scale = new Vector3(plyParameters_10[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_10[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_10[_canonID].barrelID;
				break;
			case 8:
				_pos = plyParameters_8[_canonID].pos;
				_rot = plyParameters_8[_canonID].rot;
				_scale = new Vector3(plyParameters_8[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_8[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_8[_canonID].barrelID;
				switch(_canonID)
				{
				case 0:
				case 1:
				case 2:
					dir = 0;
					dirArray[_canonID] = 0; 
					break;
				case 3:
					dir = 1;
					dirArray[_canonID] = 1; 
					break;
				case 4:
				case 5:
				case 6:
					dir = 2;
					dirArray[_canonID] = 2; 
					break;
				case 7:
					dir = 3;
					dirArray[_canonID] = 3; 
					break;
				}
				break;
			case 6:
				_pos = plyParameters_6[_canonID].pos;
				_rot = plyParameters_6[_canonID].rot;
				//_scale = new Vector3(plyParameters_6[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_6[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_6[_canonID].barrelID;
				switch(_canonID)
				{
				case 0:
				case 1:
				case 2:	
					dir = 0;
					break;
				case 3:
				case 4:
				case 5:
					dir = 2;
					break;
				}
				break;
			case 4:
				_pos = plyParameters_4[_canonID].pos;
				_rot = plyParameters_4[_canonID].rot;
				_scale = new Vector3(plyParameters_4[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_4[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_4[_canonID].barrelID;
				switch(_canonID)
				{
				case 0:
				case 1:
					dir = 0;
					break;
				case 2:
				case 3:
					dir = 2;
					break;
				}
				break;
			case 2:
				_pos = plyParameters_2[_canonID].pos;
				_rot = plyParameters_2[_canonID].rot;
				_scale = new Vector3(plyParameters_2[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_2[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_2[_canonID].barrelID;
				switch(_canonID)
				{
				case 0:
				case 1:
					dir = 0;
					break;
				}
				break;
			case 1:
				_pos = plyParameters_1[_canonID].pos;
				_rot = plyParameters_1[_canonID].rot;
				_scale = new Vector3(plyParameters_1[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_1[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				_barrelID = plyParameters_1[_canonID].barrelID;
				switch(_canonID)
				{
				case 0:
					dir = 0;
					break;
				}
				break;
			default:
				Debug.Log(" deskType = "+deskType +" / deskType = "+deskType+" / _pos = "+_pos +" / _rot = "+_rot +" / _scale "+_scale);
				return;
            }

			// 创建炮台.
//			Transform _temp = Factory.Create(canon_Barrel[_canonID].canonPrefab, Vector3.zero, Quaternion.identity);
			// 实现任意玩家的炮台都处于屏幕下方			
			if( plyParameters_6[realCanonID].barrelID == 2 ){
				turn_screen = true;
			}else{
				turn_screen = false;
			}
			if(turn_screen == true && turn_screen_on_of){
				if(plyParameters_6[_canonID].barrelID == 2){
					_pos = plyParameters_6[_canonID-3].pos;
					_rot = plyParameters_6[_canonID-3].rot;
					_scale = new Vector3(plyParameters_6[_canonID-3].scale.x * canonScaleFitResolRatio[_canonID-3].x, plyParameters_6[_canonID-3].scale.y * canonScaleFitResolRatio[_canonID-3].y, 1f);
					_barrelID = 0;
				}else{
					_pos = plyParameters_6[_canonID+3].pos;
					_rot = plyParameters_6[_canonID+3].rot;
					_scale = new Vector3(plyParameters_6[_canonID+3].scale.x * canonScaleFitResolRatio[_canonID+3].x, plyParameters_6[_canonID+3].scale.y * canonScaleFitResolRatio[_canonID+3].y, 1f);
					_barrelID = 2;
				}
				GameObject camera = GameObject.Find("Camera");
				camera.GetComponent<Transform>().rotation = new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
				GameObject lockCamera = GameObject.Find ("LockCtrl");
				lockCamera.GetComponent<Transform>().rotation = new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
				
                // 跑马灯的旋转	开启
                Fishing.Instance.MarqueeReverse = true;
			}else {
				if(plyParameters_6[_canonID].barrelID == 2){
					_barrelID = 2;
				}else{
					_barrelID = 0;
				}
				_pos = plyParameters_6[_canonID].pos;
				_rot = plyParameters_6[_canonID].rot;
				_scale = new Vector3(plyParameters_6[_canonID].scale.x * canonScaleFitResolRatio[_canonID].x, plyParameters_6[_canonID].scale.y * canonScaleFitResolRatio[_canonID].y, 1f);
				
                // 跑马灯的旋转	关闭
                Fishing.Instance.MarqueeReverse = false;
			}

			// 玩家进入,回收该位置炮台,并且singleCanonList列表设置为空
			SingleCanon[] sc_temp = canonCache.GetComponentsInChildren<SingleCanon>();
			// 遍历canonCache下的所有炮台,如果有炮台的位置和即将生成的炮台的位置重叠,则删除原有的炮台
			foreach(SingleCanon var in sc_temp)
			{
				if(_pos == var.transform.localPosition)
				{
					Destroy(var.gameObject);
				}
			}
			singleCanonList[_canonID] = null;

			Transform _temp = Factory.Create(canon_Barrel[_barrelID].canonPrefab, Vector3.zero, Quaternion.identity);

			_temp.parent = canonCache;
			_temp.localPosition = _pos;
			_temp.localEulerAngles = _rot;
//			_temp.localScale = _scale;
			_temp.localScale = Vector3.one;

			Debug.Log("Init canon:"+_pos+" "+_scale+" parent:"+canonCache.localScale+" "+canonCache.localPosition);
			
			// 初始化炮台参数.
	 		SingleCanon _sc = _temp.gameObject.GetComponent<SingleCanon>();
			singleCanonList[_canonID] = _sc;
			_sc.canonID = _canonID;
			_sc.m_sCanonNumSprite.spriteName = "CanonNum_" + (_canonID + 1).ToString();
			_sc.InitCanonParam(_canonID, _plyUniqueID, transform, fireGap, _plyScore, _bulletMulti, _bulletPower, _level, _name, dir, _shipColorID, _bulletColorID, allowAllPlyFire);
						
			// 真实的玩家才显示 player name.
			if(_canonID==realCanonID)
			{
				// 显示名字.
				SetPlyName(_name);
				// 显示金币数.
				SetGoldValue(goldValue);
				// 创建锁定线.
				LockCtrl.Instance.CreateLockLine(_canonID);

				// 对炮台预设的标识赋值,便于计时调用
				playerLogo = _temp.FindChild ("Switch2").gameObject;

				//	把真实玩家的图层调高,盖过蒙版.2s后改回正常值.
				//changeDepth_hight();	

				// 开场黑屏效果
				if(_temp.FindChild ("Switch").gameObject != null){
					_temp.FindChild ("Switch").gameObject.SetActive (true);
				}else{
					return;
				}
			}
			
			// 创建金币柱控制器.
			Vector3 showUpPos = _sc.cylinderShowUpPos.position;
			Vector3 _targetPos = _sc.cylinderTargetPos.position;
			TotalCylinderCtrl.Instance.CreateCylinderCtrl4OnePly(_canonID, showUpPos, _targetPos);

			// 播放玩家加入的声音.
			AudioCtrl.Instance.UserIn();
		}

		// 玩家离开.
		public void UserLeave(int _canonID)
		{
			// 回收玩家.
			Factory.Recycle(singleCanonList[_canonID].transform);
			singleCanonList[_canonID] = null;

			// 回收玩家的金币柱控制器.
			TotalCylinderCtrl.Instance.ClearSingleCylinder(_canonID);
			// 回收锁定线.
			LockCtrl.Instance.ClearSingleLock(_canonID);

			// 播放玩家离开声音.
			AudioCtrl.Instance.UserLeave();
		}

		// 显示玩家名字.
		void SetPlyName(string _name)
		{
			if(plyNameLabel!=null)
			{
				plyNameLabel.text = _name;
			}
		}

		// 全屏炸弹死亡后全部玩家不可以发泡.
		public void AllowFire(bool _allow)
		{
			for(int i=0; i<singleCanonList.Length; i++)
			{
				if(singleCanonList[i]!=null)
				{
					singleCanonList[i].AllowFire(_allow);
				}
			}
			allowAllPlyFire = _allow;
		}


		// 1, 点击锁定按钮.
		public void PressLockButton()
		{
			// 如果有面板在打开(购买分数的面板，结算的面板).
			if(OpeningAnyPanel())
			{
				return;
			}

			// 分数不够.
//			if(singleCanonList[realCanonID].plyScore<=0)
//			{
//				return;
//			}

			
#if UNITY_STANDALONE_WIN

            // 更改锁定状态.
             ChangeLockState();

             Locking_off = true;
#endif

#if !UNITY_STANDALONE_WIN
            if (Locking_off)
            {
                Locking_off = false;
                if (inLockMode)
                {
                    ChangeLockState();
                }
                // 开关显示 关
                if (CanonCtrl.Instance.lockButton_img != null) CanonCtrl.Instance.lockButton_img.SetActive(false);
            }
            else
            {
                Locking_off = true;
                // 开关显示 开
                if (CanonCtrl.Instance.lockButton_img != null) CanonCtrl.Instance.lockButton_img.SetActive(true);
            }
#endif
        }

        public void PressKeyQ()
        {
            if (Locking_off)
            {
                Locking_off = false;
                if (inLockMode)
                {
                    ChangeLockState();
                }
            }
        }

		// 更改锁定状态.
		void ChangeLockState()
		{
			//切换场景时不允许更改锁定状态.
			if(WaveCtrl.Instance.changingWave)
			{
				return;
			}
			// 如果不在锁定状态.
			if(!inLockMode)
			{
				// 锁定一条鱼.
				lockedFish = FishCtrl.Instance.GetALockFish();
				if(lockedFish!=null)
				{
					inLockMode = true;
					// 通知锁定线去锁定这条鱼.
					LockCtrl.Instance.lockCtrls[realCanonID].ShowLockRend(lockedFish);
					// 告诉这条鱼，它被锁定了.
					lockedFish.GetComponent<SingleFish>().locking = true;
					// 播放打开锁定声音.
					AudioCtrl.Instance.PressButton(true);
				}
			}
			else 
			{
				// 停止锁定.
				StopLock();
				// 播放取消锁定声音.
				AudioCtrl.Instance.PressButton(false);
			}
		}

		// 开始加速发炮.
		private	bool	fireSpeedUped = false;
		private	float	defaulFireSpeed;
		public void FireSpeedUp()
		{
			fireSpeedUped = !fireSpeedUped;
			if(fireSpeedUped)
			{
                fireGap = fireGapSpeedUp;
			}else
			{
				fireGap = defaulFireSpeed;
			}
			
			singleCanonList[realCanonID].initFireSpeed( fireGap );
			// 开关显示 关
            if (speedUpButton!=null)
            {
                if (speedUpButton_img != null) speedUpButton_img.gameObject.SetActive(fireSpeedUped);
            }			
		}

		// 开始切换场景的时候，记录当前锁定的状态.
		private bool lastStateIsLock = false;
//		private bool lastStateIsAuto = false;
		public void StopFireAndRecordLastLockState()
		{
			lastStateIsLock = inLockMode;
			if(inLockMode)
			{
				// inLockMode = true 的话，取消锁定状态.
				ChangeLockState();
			}
			//将锁定按钮禁用
            if (lockButton!=null)
            {
                if (lockButton.GetComponent<UIButton>() != null)
                {
                    lockButton.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    return;
                }
            }
			
			//将自动按钮禁用
            if(autoFireButton!=null){
                if (autoFireButton.GetComponent<UIButton>() != null)
                {
                    autoFireButton.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    return;
                }
            }
			
			//将按钮下的显示图标也关闭
            if (autoFireButton != null)
            {
                if (autoButton_img != null)
                {
                    autoButton_img.gameObject.SetActive(false);
                }
                else
                {
                    return;
                }
            }
			
		}

		// 结束切换场景的时候，恢复锁定状态.
		public void ResumeFireAndResumeLockState()
		{
			if(lastStateIsLock)
			{
				// lastStateIsLock = true 的话， 把当前锁定状态设置为inLockMode = false， 然后执行恢复锁定状态, 就可以打开锁定了.
				inLockMode = false;
                ChangeLockState();
                //恢复锁定的图标显示状态
                if (lockButton!=null)
                {
                    if (lockButton.FindChild("toggled_img") != null) lockButton.FindChild("toggled_img").gameObject.SetActive(true);
                }                
			}

			//恢复按钮的图标显示状态
            if (autoFireButton!=null)
            {
                if (autoButton_img != null) autoButton_img.gameObject.SetActive(autoFire);
            }			

			//将锁定按钮激活
            if (lockButton!=null)
            {
                if (lockButton.GetComponent<UIButton>() != null)
                {
                    lockButton.GetComponent<UIButton>().isEnabled = true;
                }
                else
                {
                    return;
                }
            }
			
			//将自动按钮激活
            if (autoFireButton!=null)
            {
                if (autoFireButton.GetComponent<UIButton>() != null)
                {
                    autoFireButton.GetComponent<UIButton>().isEnabled = true;
                }
                else
                {
                    return;
                }
            }			
		}

		// 停止锁定.
		public void StopLock()
		{
			inLockMode = false;

			// 告诉锁定的那条鱼，它不被锁定了.
			if(lockedFish!=null)
			{
				lockedFish.GetComponent<SingleFish>().locking = true;
				lockedFish = null;
			}

			// 锁定控制器取消锁定.
			if(LockCtrl.Instance.lockCtrls[realCanonID]!=null)
			{
				LockCtrl.Instance.lockCtrls[realCanonID].StopLock();
			}
			// 开关显示 关
            if (lockButton!=null)
            {
                if (lockButton.FindChild("toggled_img") != null) lockButton.FindChild("toggled_img").gameObject.SetActive(false);
            }			
		}

		// 点击了自动发泡按钮.
		public void PressAutoFireButton()
		{
			// 没有打开面板.
			if(OpeningAnyPanel())
			{
				return;
			}

			/*
			// 打开买分面板.
			if(singleCanonList[realCanonID].plyScore<=0)
			{
				BuyScorePanelCtrl.Instance.ShowBuyScorePanel();
			}
			*/

			// 取反自动发状态.
			if(Utility.IsMouseOverUI())
			{
				SetAutoFire(!autoFire);
			}
		}

		// 设置自动发炮标志.
		public void SetAutoFire(bool _set)
		{
			autoFire = _set;
            if (autoFireButton!=null)
            {
                if (autoButton_img != null) autoButton_img.gameObject.SetActive(autoFire);
            }			
			// 播放点击自动发泡按钮的声音.
			if(autoFireButton!=null)
			{
				AudioCtrl.Instance.PressButton(autoFire);
			}
		}

		// 设置金币数.
//		private Transform tempScoreWindow = backage.FindChild("ScoreWindow");
//		private Transform tempScore =  backage.FindChild("ScoreWindow").FindChild("front_panel").FindChild("field1").FindChild("score_label");
		public void SetGoldValue(int _goldValue)
		{
			goldValue = _goldValue;
#if  !UNITY_EDITOR &&!UNITY_STANDALONE_WIN
			Transform tempScoreWindow = backage.FindChild("ScoreWindow");
			Transform tempScore =  backage.FindChild("ScoreWindow").FindChild("front_panel").FindChild("field1").FindChild("score_label");
			if(tempScoreWindow.gameObject.activeSelf && tempScore != null) tempScore.GetComponent<UILabel>().text = goldValue.ToString();
#endif
			// goldValueLabel.text = goldValue.ToString();
//			goldValueLabel.ApplyValue(_goldValue, 0);
			goldValueLabel.GetComponent<UILabel>().text = _goldValue.ToString ();
		}

		// 结算 account 里记录杀死了一条鱼.
		public void KillOneFish(int _canonID, int _fishPool)
		{
//			return;
			if(_fishPool>=0 && _fishPool<fishAccount.Length)
			{
				// 只用于结算面板的总鱼数显示.(和其他无关)
				account_shuliang ++;

                // 屏蔽每种鱼死亡数量的统计
// 				fishAccount[_fishPool]++;
// 				Transform _fishItem = accountPanel.FindChild("fishContainer").FindChild("fish_"+_fishPool);
// 				if(_fishItem!=null)
// 				{
// 					_fishItem.FindChild("value").GetComponent<UILabel>().text = fishAccount[_fishPool].ToString();
// 				}
			}
		}

		// 显示结算 account 界面里杀死鱼的数量.
		void ShowKillFishValue()
		{
//			return;
			for(int i=0; i<fishAccount.Length; i++)
			{
				Transform _fishItem = accountPanel.FindChild("fishContainer").FindChild("fish_"+i);
				if(_fishItem!=null)
				{
					_fishItem.FindChild("value").GetComponent<UILabel>().text = fishAccount[i].ToString();
				}
			}
		}

		// 清空结算 account 界面里杀死鱼的数量
		public void CleanKillFishValue()
		{
			for(int i=0 ;i<fishAccount.Length;i++)
			{
				Transform _fishItem = accountPanel.FindChild("fishContainer").FindChild("fish_"+i);
				if(_fishItem!=null)
				{
					fishAccount[i] = 0;
					_fishItem.FindChild("value").GetComponent<UILabel>().text = fishAccount[i].ToString();
				}
			}
		}

		// 设置结算 account 界面里的金币数和豆豆数量.
		private int tempGoldValue4RealPly;
		private int tempBeansValue;
		public void SetTempGoldAndBeansValue(int _tempGold, int _beans)
		{
//			return;
			tempGoldValue4RealPly = _tempGold;
			tempBeansValue = _beans;
		
			// update value label once we get these values, incase we have showed the tempGold and beans labels but not set their value yet.
			ShowRealPlyTempGoldAndBeansValue();
		}
		// 显示 结算account界面里的金币数和豆豆数
		public void ShowRealPlyTempGoldAndBeansValue()
		{
//			return;
			Transform _golds = accountPanel.FindChild("fishContainer").FindChild("golds");
			if(_golds!=null)
			{
				_golds.FindChild("value").GetComponent<UILabel>().text = tempGoldValue4RealPly.ToString()+"金币";
			}
			Transform _beans = accountPanel.FindChild("fishContainer").FindChild("beans");
			if(_beans!=null)
			{
				_beans.FindChild("value").GetComponent<UILabel>().text = tempBeansValue.ToString();
			}
		}

		// 按下了结算 account 的按钮（当前不用这个结算界面）.
		public void PressAccountButton()
		{
//			return;
			// 如果当前正在打开任何界面.
			if(OpeningAnyPanel())
			{
				return;
			}

			// 把玩家杀死的鱼，还没返还给玩家的分数马上给玩家.
			singleCanonList[realCanonID].GiveValue2PlyImmediate(); 

			// account 面板变大.
			ScaleUpAccountPanel(true);
			// 显示杀死鱼的数量
			//ShowKillFishValue();
			// 显示 account 结算界面里的金币数和豆豆数.
			ShowRealPlyTempGoldAndBeansValue();

			// 设置还没接收到服务器发回来消息.
			S_C_AccountMsgHaveReceive = false;
			// 给服务器发送消息.
//			MessageHandler.Instance.C_S_Balance();

			// 播放声音.
			AudioCtrl.Instance.ShowAccount();

			//取消锁定
			if(inLockMode)
			{
				PressLockButton();
			}
			//取消自动发炮
			if(autoFire)
			{
				SetAutoFire(false);
			}
			//关闭上下分窗口
			hiddenScoreWindow();
		}

		// 打开和关闭 结算 account 面板（当前不用这个结算界面）.
		void ScaleUpAccountPanel(bool _open)
		{
//			return;
			openingAccount = _open;
			if(openingAccount)
			{
				iTween.Stop(accountPanel.gameObject);
				accountPanel.localScale = Vector3.one;
				iTween.ScaleTo(accountPanel.gameObject, Vector3.one, 1f);
			}
			else 
			{
				iTween.Stop(accountPanel.gameObject);
				iTween.ScaleTo(accountPanel.gameObject, Vector3.zero, 1f);
			}
		}
		private Transform temp;
		// 点击“继续” 按钮.（当前不用这个结算界面）.
		public void Continue()
		{
//			return;
			ScaleUpAccountPanel(false);

			// 还没接收到服务器发回来的消息.
			if(S_C_AccountMsgHaveReceive==false)
			{
				return;
			}
			// clear fish account.
//			ClearAccountKillFishNum();

			// 把结算 account 面板里的金币数加给玩家.
//			SetGoldValue(tempGoldValue4RealPly);
			
			// 重置结算 account 面板里的 金币数和豆豆数.
//			SetTempGoldAndBeansValue(0, 0);

			//清除玩家分数.
//			singleCanonList[realCanonID].SetPlyValue(0);

			AudioCtrl.Instance.PressButton(true);
		}

		// 按了“退出”按钮（当前不用这个结算界面）.
		public void Exit()
		{
//			return;
			ScaleUpAccountPanel(true);

			// 重置结算 account 面板里的 金币数和豆豆数.
			SetTempGoldAndBeansValue(0, 0);
			
			// 清除 account 面板里的总分
			account_zongfen = 0;
			
			// 清除 account 面板里杀死每种鱼的数量
			CleanKillFishValue();
			
			AudioCtrl.Instance.PressButton(true);
			// 玩家退出房间时候需要把跑马灯旋转关掉.
            Fishing.Instance.MarqueeReverse = false;
			Fishing.Instance.Quit();
		}
		
		// 清楚结算 account界面里杀死鱼的数量（当前不用这个结算界面）.
		void ClearAccountKillFishNum()
		{
//			return;
			for(int i=0; i<fishAccount.Length; i++)
			{
				fishAccount[i] = 0;
			}
		}

		// 按下了返回大厅的按钮.
		public void PressBackButton()
		{
			ScaleUpAccountPanel(true);
			if(OpeningAnyPanel())
			{
				return;
			}

			AudioCtrl.Instance.PressButton(false);

            Fishing.Instance.Quit();
		}

		// 是否在打开 购买分数面板，或者在打开 结算面板.（当前不用这个结算界面）.
		public bool OpeningAnyPanel()
		{
			// 1. opening buy score panel.
			// 2. opening account panel.
			/*
			if(BuyScorePanelCtrl.Instance.showPanel || openingAccount)
			{
				return true;
			}
			*/
			return false;
		}

		// 显示上下分面板
		public void showScoreWindow(){
			if(scoreWindow!=null){
//				scoreWindow.transform.localScale = Vector3.one;
				scoreWindow.SetActive (true);
			}else{
				return;
			}

		}
		// 隐藏上下分面板
		public void hiddenScoreWindow(){
			if(scoreWindow!=null){
//				scoreWindow.transform.localScale = Vector3.zero;
				scoreWindow.SetActive (false);
			}else{
				return;
			}
		}

		void Update(){	
			#if UNITY_STANDALONE_WIN
			if(Input.GetMouseButtonDown (1)){
				SingleCanon _sc = singleCanonList[realCanonID];
				_sc.C_S_GunPowerUp ();
			}
			#endif

            // 如果打开按钮面板,并且超时.就自动关闭按钮面板
            if (m_bIsOpenButton && m_bTimeOut)
            {
                TweenPosition tp_LeftButton = m_gLeftButton.GetComponent<TweenPosition>();
                tp_LeftButton.from = new Vector3(-35, 0, 0);
                tp_LeftButton.to = new Vector3(-160, 0, 0);
                tp_LeftButton.enabled = true;
                tp_LeftButton.ResetToBeginning();

                TweenPosition tp_RightButton = m_gRightButton.GetComponent<TweenPosition>();
                tp_RightButton.from = new Vector3(35, 0, 0);
                tp_RightButton.to = new Vector3(160, 0, 0);
                tp_RightButton.enabled = true;
                tp_RightButton.ResetToBeginning();

                m_bIsOpenButton = false;
                m_gAllButtonBG.transform.localScale = new Vector3(1, 1, 1);
            }
		}

		//	关闭开场黑屏.
		void blackScreen(){
			Transform _temp = singleCanonList[realCanonID].transform;
			_temp.FindChild ("Switch").gameObject.SetActive (false);
		}


        private bool m_bIsOpenButton = false;
        public bool m_bTimeOut = false;

        public void OpenAllButton()
        {
            if (m_bIsOpenButton)
            {
                CloseAllButton();
            }
            else
            {
                TweenPosition tp_LeftButton = m_gLeftButton.GetComponent<TweenPosition>();
                tp_LeftButton.from = new Vector3(-160, 0, 0);
                tp_LeftButton.to = new Vector3(-35, 0, 0);
                tp_LeftButton.enabled = true;
                tp_LeftButton.ResetToBeginning();

                TweenPosition tp_RightButton = m_gRightButton.GetComponent<TweenPosition>();
                tp_RightButton.from = new Vector3(160, 0, 0);
                tp_RightButton.to = new Vector3(35, 0, 0);
                tp_RightButton.enabled = true;
                tp_RightButton.ResetToBeginning();

                m_gAllButtonBG.transform.localScale = new Vector3(-1,1,1);

                m_bIsOpenButton = true;
            }            
        }
        
        void CloseAllButton()
        {
            if (m_bIsOpenButton )
            {
                TweenPosition tp_LeftButton = m_gLeftButton.GetComponent<TweenPosition>();
                tp_LeftButton.from = new Vector3(-35, 0, 0);
                tp_LeftButton.to = new Vector3(-160, 0, 0);
                tp_LeftButton.enabled = true;
                tp_LeftButton.ResetToBeginning();

                TweenPosition tp_RightButton = m_gRightButton.GetComponent<TweenPosition>();
                tp_RightButton.from = new Vector3(35, 0, 0);
                tp_RightButton.to = new Vector3(160, 0, 0);
                tp_RightButton.enabled = true;
                tp_RightButton.ResetToBeginning();

                m_bIsOpenButton = false;
                m_gAllButtonBG.transform.localScale = new Vector3(1, 1, 1);
            }
        }        
	}
}
