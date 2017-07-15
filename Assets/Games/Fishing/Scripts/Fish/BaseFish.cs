using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	public class BaseFish : MonoBehaviour 
	{
		protected Transform cur_Transform;
		[HideInInspector] public Collider2D cur_Collider;
		//protected Animator [] cur_Animator;
        protected UISpriteAnimation[] cur_Animator;
		//protected SpriteRenderer cur_SpriteRenderer;
        protected UISprite cur_SpriteRenderer;

		// 导航路径.
		protected Navigation navigation;
		// 影子.
		[HideInInspector] public SingleShadow singleShadow;

		// 被这个玩家打中. 
		[HideInInspector] public int canonID = 0;
		// 被打中的 bullet cost. 
		[HideInInspector] public int bulletCost;
		
		// 死亡动作的 hash 值.
		protected int deadHash;
		// 是否已经死亡.
		[HideInInspector] public bool dead = false;
		// 鱼的倍率.
		public int multi = 0;
		// 鱼死亡后从服务器接收到的倍率（黑洞死亡后从服务器接受到的倍率和本地配到的倍率是不一样的）. 
		[HideInInspector] public int serverMulti;
		// power.
		[HideInInspector] public int power;
		// fish id.
		[HideInInspector] public int fishServerID = 0;
		// 死亡后的分数.
		protected int fishDeadValue;
		// 是否需要震屏.
		public bool shakeCam;

		// 属于哪个鱼池(FishCtrl.cs 里的).
		[HideInInspector] public int fishPool;

		// 鱼的大小.
		public Vector3 localScale = Vector3.one;
		// 鱼的动画播放速度
		public int FishAnimSpeed = 10;

		// 用来记录屏幕外面的位置(viewport).
		protected Vector4 viewportRect;
		// 每隔一秒钟检查鱼是否已经游完，或者游出屏幕.
		protected float checkReycleTimeLength = 1f;
		protected float targetTime2CheckRecycle = 0f;

		// 鱼的类型.
		public enum FishType
		{
			normal=0,
			ywdj,
			quanping,
			blackHall,
			likui
		}
		public FishType fishType = FishType.normal;

		// 如果是全屏炸弹，那全屏炸弹可以炸死鱼的类型.
		public bool killNorml;
		public bool killYWDJ;
		public bool killQuanPing;
		public bool killBlackHall;
		public bool killLikui;

		// 这条鱼死亡后玩家是否可以玩，直到回收了这条鱼才解锁.(比如特殊鱼).
		public bool canNotPlayUntilRecycle;

		// 一网打尽杀死哪个鱼池的鱼.
		public int ywdjKillFishPool = 0;
		// 一网打尽的线.
		public Transform ywdjLine;

		// high score 预设.
		public Transform highScorePrefab;
		// 鱼死亡后是否需要显示 high score.
		public bool showHighScoreWhenFishDead = true;

		// 是否可以被黑洞吸收.
		public bool beAbsorbed;

		// 是否被锁定.
		[HideInInspector] public bool locking = false;
		// 锁定的次序，值越大，那么在开启锁定的时候就容易被锁定.
		public int lockFishOrder = 0;

		// 是否已经从缓存list中移除这条鱼.
		protected bool removeFromFishList = false;

		// 死亡音效.
		public AudioClip deadAudio;
		// 音效播放的概率, 百分之....
		public int deadProbability;

		// 死亡后金币类型.
		public int coinType;
		// 个数.
		public int coinNum = 5;

		// 死后鱼层现在最上层, 这个是第几层.
		public int deadOrderInLayer;
		protected int orgOrderInlayer;

		// 死亡时间长度.
		public float deadTimeLength = 2f;
		protected float recycleTargetTime = 0f;


		// 黑洞和全屏移动到屏幕中心的状态.
		public enum SpecialFishDeadState
		{
			none,
			drag2Center,
			wait,
			recycle
		}
		[HideInInspector] public SpecialFishDeadState specialFishDeadState = SpecialFishDeadState.none;
		// 黑洞和全屏移动到中心点的时间.
		public float move2CenterTimeLength = 3f;
		// 由时间计算出来的速度.
		protected float move2CenterSpeed;
		// 黑洞和全屏移动到中心点的percent.
		protected float specialFishMovePercent = 0f;
		// 黑洞和全屏移动到中心点的初始位置.
		protected Vector3 move2CenterStartPos;
		// 黑洞和全屏移动到中心点的初始角度.
		protected Vector3 move2CenterStartEuler;
		// 黑洞死亡后吸收一条鱼的倍率.
		public int blackHallAbsorbMulti = 3;
		// 黑洞死后的数字的预设.
		public Transform blackHallNumPrefab;
		protected Transform blackHallNumCreated;
		protected NumItem blackHallNum;
		// 数字的高度.
		public float blackHallHeight;
		// 数字的大小.
		public Vector3 blackHallScale;
		// 黑洞死亡后的线的预设.
		public Transform blackHallLine;
		protected Transform blackHallLineCreated;
		protected LineRenderer blackHallLineRend;
		// 黑洞需要吸收的倍率.
		protected int bh_need2AbsorbMulti = 0;
		// 黑洞x需要吸收的分数.
		protected int bh_need2AbsorbValue = 0;
		// 黑洞已经吸收的分数.
		protected int bh_haveAbsorbValue = 0;
		// 是否是被黑洞杀死.
		protected bool killByBlackHall;
		// 黑洞.
		protected Transform blackHall;
		// 被黑洞杀死后移动到屏幕中间的 percent.
		protected float lerp2BlackHallPercent;

		//李逵开始倍率
		public int LikuiBeginBeilv = 40;
		//李逵增加倍率的间隔时间
		public float LiKuiBeilvTime;
		//李逵每次增加的倍率
		public int LiKuiAddBeilv;
		//李逵最大倍率
		public int LiKuibeilvHigh;
		//李逵头上的倍率
		public Transform LikuiNumPrefab;
		protected Transform LikuiNumCreated;
		protected UILabel LiKuiCurrentBeilv;
		//李逵数字高度
		public float LikuiHeight;
		//李逵 数字的大小.
		public Vector3 LikuiNumScale;
		//李逵当前时间
		public float likuiCurrent = 0f;
		

		// 用来记录这条鱼是否会被波浪清除.
		[HideInInspector] public bool lastMap2ClearFish;

		// 特殊鱼（全屏和黑洞）是否已经给分. 在切换场景的时候可以用到, 没给分，那么在回收这条特殊鱼的时候给分.
		protected bool specialFishHaveAddValue = false;

		// 是否播放金币声音.
		protected bool playCoinAudio;

		// 被子弹击中的颜色.
		public Color hurtColor = Color.white;
		// 被子弹打中后要更改的renderer.
//		protected SpriteRenderer [] hurtRends;
		protected UISprite [] hurtRends;
		// 鱼原始的颜色.
		protected Color orgColor;
		// 颜色变化时长.
		public float hurtTimeLength = 0.5f;
		protected float hurtColorCancelTime = 0f;
		protected bool startHurt = false;

		// 死亡效果.
		public List<EffParam> deadEffList = new List<EffParam>();

        // 鱼动画播放帧率
        public int fishAnimFPS = 10; 

		protected virtual void Awake () 
		{
			cur_Transform = transform;
			deadHash = Animator.StringToHash("dead");
			//cur_Animator = GetComponentsInChildren<Animator>();
            cur_Animator = GetComponentsInChildren<UISpriteAnimation>();
			cur_Collider = GetComponent<Collider2D>();
			//cur_SpriteRenderer = GetComponent<SpriteRenderer>();
            cur_SpriteRenderer = GetComponent<UISprite>();
			navigation = new Navigation();

//			hurtRends = GetComponentsInChildren<SpriteRenderer>();
			hurtRends = GetComponentsInChildren<UISprite>();
			orgColor = cur_SpriteRenderer.color;


			//orgOrderInlayer = cur_SpriteRenderer.sortingOrder;
            orgOrderInlayer = cur_SpriteRenderer.depth;



			// 获取屏幕四侧的位置.
			viewportRect = Utility.GetViewportFramePos();

			// 根据屏幕适配鱼的大小. 按照 1136*640 作为标准适配.
			localScale = new Vector3(localScale.x*Utility.device_to_1136x640_ratio.x, localScale.y*Utility.device_to_1136x640_ratio.x, localScale.z);

			//是李逵保存倍率
			if( fishType == FishType.likui)
			{
				LikuiBeginBeilv = multi;
			}
		}

		protected virtual void Update()
		{
		}

		/// <summary>
		/// 鱼死亡.
		/// </summary>
		/// <param name="_canonID"> 被哪个玩家 id 杀死.	</param>
		/// <param name="_multi">   从服务器接收到的倍率. 	</param>
		/// <param name="_power">	从服务器接受到的power.</param>
		/// <param name="_npcType"> npc type.			</param>
		public virtual void KillByBullet(int _canonID, int _multi, int _power, int _npcType)
		{
		}
	}
}
