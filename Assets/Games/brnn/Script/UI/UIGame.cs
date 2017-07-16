using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Services.Data;
using System.IO;
using Shared;
using System;


namespace com.QH.QPGame.BRNN
{
    #region ################结构定义##############

	//游戏状态
	public enum TimerType
	{
		TIMER_NULL  = 0,	 
		TIMER_READY = 1,  //空闲时间
		TIMER_CHIP  = 2,  //下注时间
		TIMER_OPEN  = 3,  //开牌时间
		
	};

    public enum SoundType
    {
        READY = 0,
        START = 1,
        SENDCARD = 2,
        CHIP = 3,
        WIN = 4,
        LOSE = 5,
        CLOCK = 6,
		TURN = 7,
    };

    public enum GameSoundType
    {
        N0 = 0,
        N1 = 1,
        N2 = 2,
        N3 = 3,
        N4 = 4,
        N5 = 5,
        N6 = 6,
        N7 = 7,
        N8 = 8,
        N9 = 9,
        N10 = 10,
        WUXIAONIU = 11,
        BOMEBOME = 12,
    };

    #endregion
    public class UIGame : MonoBehaviour
    {
        #region ###############变量定义#################

		//配置数据
		private string systemBankerName;													//系统坐庄名字

		public float   distance = 90;														//金币下注范围

		//时间控制
		public float sendCardTypeTime = 0.9f;												//发每组牌的时间间隔
		public float showCardTypeTime = 2.0f;												//开每组牌的时间间隔
		public float oneCardTime = 0.1f;													//发牌时两张牌时间的间隔
		public float multipleTime = 6.5f;													//时间倍数(计算五组总共的时间)

		public GameObject o_player_money;													//当前玩家总金币
		public GameObject o_player_face;													//当前玩家头像
		public GameObject o_player_nick;													//当前玩家用户名
		public GameObject o_rules;															//游戏规则
		public GameObject controlCard;														//牌控制
		public GameObject NotesPrefabs;														//胜负记录
        public GameObject conCealTween;                                                     //隐藏打开界面

		//金币按钮数据
        public GameObject[] o_chip_Btn = new GameObject[6];
		public GameObject[] o_chip_Pos = new GameObject[6];

		int   m_lChipNum = 1;
        int[] m_lAllChips = new int[13];	                                                //筹码种类
        int[] m_lGameChips = new int[7];					                                //显示筹码
        long[] m_lChipScore = new long[7];					                                //筹码金额
        long m_lLastJetton = 0;                                                             //上一局所选筹码值
	
		public GameObject o_apply_btn;														//申请上庄按钮
		public GameObject o_cancel_btn;														//下庄按钮
		public GameObject o_player_info;													//当前玩家的信息界面
		public GameObject o_banker_info;													//庄家的信息界面
		
		public GameObject o_apply_list;														//申请上庄界面
		public GameObject o_msgbox;															//提示框
		public GameObject o_result;															//结算界面
		public GameObject o_close_box;														//退出按钮

		private GameObject o_chip_buttons = null;											//金币按钮父级
		private GameObject o_bank_player = null;											//庄家信息
		private GameObject o_add_buttons = null;											//加钱按钮
		private GameObject o_gameTime_label = null;											//游戏状态提示
		private GameObject o_clock = null;													//时钟
		private GameObject o_clock_num = null;												//时钟数值

		//显示筹码
		private GameObject ctr_chip_tian = null;											//天
		private GameObject ctr_chip_di = null;												//地
		private GameObject ctr_chip_xuan = null;											//玄
		private GameObject ctr_chip_huang = null;											//黄
		
		private byte[][] m_cbTableCardArray = new byte[GameLogic.AREA_COUNT][];				//桌面扑克
		
		private byte[] m_cbTableCardArrayAll = new byte[25];								//桌面扑克

		public  byte[]				m_cbCardType = new byte[GameLogic.AREA_COUNT];			//牌型信息

		public static long[] 	   	s_lTableScore = new long[GameLogic.AREA_COUNT];			//当前玩家在各区域下注筹码(取值)
		public static GameObject[] 	o_player_chips = new GameObject[GameLogic.AREA_COUNT];	//当前玩家在各区域下注筹码(UI显示)
		private long[]		 	  	m_lAreaInAllScore = new long[GameLogic.AREA_COUNT];		//每个区域下注总分(取值)
		private GameObject[] 	   	o_chips_count = new GameObject[GameLogic.AREA_COUNT];   //每个区域下注总分（UI显示）

		public List<Vector3> m_lTouchPoint = new List<Vector3>();							//记录下注位置

		//通用数据
		private static bool 		s_bReqQuit = false;										//玩家是否离开
		private static bool 		s_bStart = false;										//游戏是否开始
		private static TimerType 	s_bTimerType = TimerType.TIMER_NULL;					//游戏状态（空闲、下注、开牌）
		private static float		s_nQuitDelay = 0;										//记录游戏开始到退出游戏的时间（毫秒）
		private static float		s_nInfoTickCount = 0;									//记录系统启动后的时间（接受Environment.TickCount）
		public  static byte  		s_bBankerUser = GameLogic.NULL_CHAIR;					//作庄玩家
		private static int  		s_lGameCount = 0;                  						//玩家游戏局数

		private long 	  			player_totalMoney = 0;								    //玩家总金币
		private long	  			banker_totalMoney = 0;									//庄家总金币
		private byte[] 				m_buffer = new byte[100];								//记录走势 
		private GameObject			m_MySelfScorelbl;										//玩家金币（输赢金币显示）
		private GameObject			m_BanckScorelbl;										//庄家金币（输赢金币显示）
		private long				m_selfScore;											//玩家金币（输赢金币）
		private long				m_bankerScore;											//庄家金币（输赢金币）			

		//游戏变量
		private long 				m_lMaxChipBanker;										//最大下注 (庄家)
		private byte				m_cbTimeLeave;											//剩余时间
		private int 				m_nChipTime;											//下注次数 (本局)
		private int 				m_nChipTimeCount;										//已下次数 (本局)
		private long 				m_nListUserCount;										//列表人数 (申请上庄)
		private long 				m_AreaLimit;											//区域限制
		private bool 				m_canChip;												//是否可下注

		private Vector3 			m_vecClick = new Vector3(0, 0, 0);  					//筹码位置
		private List<PlayerInfo> 	bankerList = new List<PlayerInfo>();					//申请庄家列表										
		private byte[][]			result_notes = new byte[GameLogic.AREA_COUNT][];		//记录走势
		private byte				notesData_leng = 0;										//记录信息长度

		private long m_lCurrentJetton;														//当前筹码(当前玩家选择的筹码)
		private bool m_bShowChangeBanker;													//轮换庄家
		private bool m_bNeedSetGameRecord;													//完成设置
		private long m_ApplyCondition;														//上庄条件

		//庄家信息		
		private ushort m_wCurrentBanker;													//当前庄家
		private ushort m_wBankerUser;														//实际庄家
		private ushort m_wBankerTime;														//做庄次数
		private long   m_lBankerScore;														//庄家积分（空值）
		private long   m_lBankerWinScore;													//庄家总金币
		private bool   m_bEnableSysBanker;													//是否系统做庄
		private bool   applyBanker = false;													//玩家是否申请上庄
   		//private long   m_lTmpBankerWinScore;												//庄家本轮成绩

		private int	   startBankerTime;														//普通坐庄次数
		private int	   allBankerTime;														//可做庄次数

		//玩家成绩
		private long m_lMeCurGameScore;														//我的成绩
		private long m_lMeCurWinScore;														//本轮成绩
		private long m_lMeCurGameReturnScore;												//本次返回金币
		private long m_lMeGameScoreCount;													//玩家总成绩
		private long m_lGameRevenue;														//游戏税收
		private long m_lMeStatisticScore;													//游戏成绩

		//游戏记录
		private GameObject    o_notes;														//游戏记录界面
		private GameObject[]  notes_data = new GameObject[5];								//显示输赢  

        private GameObject      ShowInterface = null;

        private float showTime = 0;                                                         //界面信息显示时间
		private string boxStr;																//提示信息（当前提示内容）

        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[20];
        public AudioClip[] _ManSound = new AudioClip[20];
                                               
        #endregion

        #region ###############初始化###################

		void Awake()
		{
			o_add_buttons = GameObject.Find("scene_game/dlg_player/ctr_money/add_btn");
			o_clock = GameObject.Find ("scene_game/dlg_clock");
			o_clock_num = GameObject.Find("scene_game/dlg_clock/num");
			o_gameTime_label = GameObject.Find("scene_game/dlg_clock/dlg_timelabel");
			o_bank_player = GameObject.Find("scene_game/dlg_player_bank");
			o_chip_buttons = GameObject.Find("scene_game/dlg_add_btns");

			//下注区域（显示筹码）
			ctr_chip_tian = GameObject.Find("scene_game/ctr_chips_tian");
			ctr_chip_di = GameObject.Find("scene_game/ctr_chips_di");
			ctr_chip_xuan = GameObject.Find("scene_game/ctr_chips_xuan");
			ctr_chip_huang = GameObject.Find("scene_game/ctr_chips_huang");

			//当前玩家在各区域下注总分
			o_player_chips[1] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_tian");
			o_player_chips[2] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_di");
			o_player_chips[3] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_xuan");
			o_player_chips[4] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_huang");

			//每个区域下注总分
			o_chips_count[1] = GameObject.Find("scene_game/dlg_player/lbl_chips_tian");
			o_chips_count[2] = GameObject.Find("scene_game/dlg_player/lbl_chips_di");
			o_chips_count[3] = GameObject.Find("scene_game/dlg_player/lbl_chips_xuan");
			o_chips_count[4] = GameObject.Find("scene_game/dlg_player/lbl_chips_huang");

			//结算数据
			m_BanckScorelbl = GameObject.Find("scene_game/result/BanckScoreLable");
			m_MySelfScorelbl = GameObject.Find("scene_game/result/MySelfScoreLable");

			//游戏记录界面
			o_notes = GameObject.Find("scene_game/Notes");

			for(int i=0; i<5; i++)
			{
				notes_data[i] = GameObject.Find("scene_game/Notes/notes_data_"+i.ToString());
				result_notes[i] =new byte[16];
			}

		}

		void Start()
		{
			InitGameView();
		}

		void Update()
		{
			if (conCealTween.active == true || o_msgbox.active == true)
            {
                showTime += Time.deltaTime;

                if (showTime > 7.0f)
                {
					if(o_msgbox !=null)
					{
						o_msgbox.SetActive(false);
					}

                    ConCealTween();
                    showTime = 0;
                }
            }

		}

        public void Init()
        {
            try
            {
                GameEngine.Instance.SetTableEventHandle(new TableEventHandle(OnTableUserEvent));
                GameEngine.Instance.AddPacketHandle(MainCmd.MDM_GF_FRAME, new PacketHandle(OnFrameResp));
                GameEngine.Instance.AddPacketHandle(MainCmd.MDM_GF_GAME, new PacketHandle(OnGameResp));
                if (!GameEngine.Instance.AutoSit || GameEngine.Instance.IsPlaying())
                {
                    GameEngine.Instance.SendUserSetting();
                }
                else
                {
                    GameEngine.Instance.SendUserSitdown();
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
          
              
        }

		void FixedUpdate()
		{
			if ((Time.realtimeSinceStartup - s_nInfoTickCount) > 5)
			{
				ClearAllInfo();
				s_nInfoTickCount = 0;
			}
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
			{
				OnBtnBackIvk();			
			}
			
			if ((Time.realtimeSinceStartup - s_nQuitDelay) > 1 && s_bReqQuit == true)
			{
				s_bReqQuit = false;
                s_nQuitDelay = Time.realtimeSinceStartup;

				UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);

			}
		}

		void InitGameView()
		{
			s_bReqQuit = false;
			s_bStart = false;

           UISetting.o_set_panel.SetActive(false);
            //o_apply_list.SetActive(false);
            //o_notes.SetActive(false);
            conCealTween.SetActive(false);
			o_player_info.SetActive(false);
			o_banker_info.SetActive(false);
			//o_rules.SetActive(false);

			m_lCurrentJetton = 0;
			systemBankerName = "";
			s_nQuitDelay = 0;
			s_nInfoTickCount = 0;
			s_lGameCount = 0;
			m_ApplyCondition = 5000000;
			m_AreaLimit = 10000000000;
			m_bEnableSysBanker = true;
			
			s_bTimerType = TimerType.TIMER_READY;
            s_nInfoTickCount = Time.realtimeSinceStartup;

			o_msgbox.SetActive(false);
			o_result.SetActive(false);
			o_clock_num.SetActive (false);
			o_gameTime_label.GetComponent<UISprite>().spriteName = "blank";
			o_bank_player.transform.FindChild ("ctr_user_face").FindChild ("sp_face").gameObject.GetComponent<UISprite> ().spriteName = "blank";
			o_chip_buttons.SetActive (false);

			//庄家信息
			m_lBankerScore = 0; 	 	//庄家积分
			m_wCurrentBanker = 255;	    //庄家位置
			m_wBankerUser = 255;
			
			m_lMeCurGameScore = 0; 		
			m_lMeGameScoreCount = 0;
			m_lMeCurWinScore = 0;
			bankerList.Clear();

			for(int i=0; i<GameLogic.AREA_COUNT; i++)
			{
				m_cbTableCardArray[i] = new byte[5];
				Array.Clear(m_cbTableCardArray[i], 0, 5);

				if(o_player_chips[i] !=null)
				{
					o_player_chips[i].transform.GetComponent<UISprite>().spriteName = "blank";
					o_player_chips[i].transform.FindChild("reslut_count").gameObject.SetActive(false);
					o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(false);
				}

				if(o_chips_count[i] !=null)
				{
					o_chips_count[i].SetActive(false);
				}
			}

			Array.Clear(m_cbTableCardArrayAll, 0, m_cbTableCardArrayAll.Length);

			controlCard.GetComponent<UICardControl>().ClearCardType();
		
		}

		void ResetGameView()
		{
            s_nInfoTickCount = Time.realtimeSinceStartup;
			//o_rules.SetActive(false);

			ctr_chip_tian.GetComponent<UIChipControl>().ClearChips();
			ctr_chip_di.GetComponent<UIChipControl>().ClearChips();
			ctr_chip_xuan.GetComponent<UIChipControl>().ClearChips();
			ctr_chip_huang.GetComponent<UIChipControl>().ClearChips();
		}
        #endregion

		#region ###############引擎调用#################

        //刷新界面
		void UpdateUserView()
		{
			if (s_bStart == false) return;
			
			m_wBankerUser = m_wCurrentBanker;
			s_bBankerUser = (byte)m_wCurrentBanker;
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			o_player_nick.GetComponent<UILabel>().text = userdata.NickName; 
			player_totalMoney = userdata.Money;
			o_player_money.GetComponent<UILabel>().text = player_totalMoney.ToString("0,0");

			o_player_face.transform.GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);

			if(GetSelfChair() == m_wCurrentBanker)
			{
				o_apply_btn.SetActive(false);
				o_cancel_btn.SetActive(true);
				PlayerInfo player = GameEngine.Instance.EnumTablePlayer((uint)m_wCurrentBanker);
				o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);

				banker_totalMoney = userdata.Money;
				o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = banker_totalMoney.ToString("0,0");
					

				o_bank_player.transform.FindChild("ctr_money").transform.FindChild("lbl_nick").GetComponent<UILabel>().text = player.NickName;

				//自动下庄
				int bankerTime = 0;

				if(allBankerTime == 0)
				{
					bankerTime = startBankerTime;
				}
				else
				{
					bankerTime = allBankerTime;
				}

				if(userdata.Money < m_ApplyCondition || m_wBankerTime >= bankerTime)
				{
					if( GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GAME_SCENE_FREE)
					{
						OnCancelForBanker(); 
						//cnMsgBox("玩家下庄！");
					}
				}
				
			}else
			{
				o_apply_btn.SetActive(true);
				o_cancel_btn.SetActive(false);
				
				PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer((uint)m_wCurrentBanker);
				if (bankerdata != null)
				{
					m_bEnableSysBanker = false;
					o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace((int)bankerdata.HeadID, (int)bankerdata.VipLevel);
					banker_totalMoney = bankerdata.Money;
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = (bankerdata.Money).ToString("0,0");
					o_bank_player.transform.FindChild("ctr_money").transform.FindChild("lbl_nick").GetComponent<UILabel>().text = bankerdata.NickName;
				}else
				{
					o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace(0, 0);
					//o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = m_lBankerWinScore.ToString();
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = "";
					o_bank_player.transform.FindChild("ctr_money").transform.FindChild("lbl_nick").GetComponent<UILabel>().text = systemBankerName;
				}

				//跟新申请上庄列表
				updateApplyList();
			}

		}

		//更新申请列表
		public void updateApplyList()
		{
			for(int i = 0; i<5; i++)
			{
				o_apply_list.transform.FindChild("sp"+i).FindChild("nickname").gameObject.GetComponent<UILabel>().text = "";
				o_apply_list.transform.FindChild("sp"+i).FindChild("nickmoney").gameObject.GetComponent<UILabel>().text = "";
				o_apply_list.transform.FindChild("sp"+i).FindChild("face").gameObject.GetComponent<UISprite>().spriteName = "blank";
			}
			
			//申请上庄列表
			for(int i = 0; i < bankerList.Count;i++)
			{
				if(bankerList[i]!= null)
				{	
					PlayerInfo player = GameEngine.Instance.EnumTablePlayer(bankerList[i].DeskStation);
					if(player != null && bankerList[i].ID != (uint)m_wCurrentBanker && player.ID == bankerList[i].ID)
					{
						if(i > 4) break;
						o_apply_list.transform.FindChild("sp"+i).FindChild("nickname").GetComponent<UILabel>().text = "昵称:" + bankerList[i].NickName;
						o_apply_list.transform.FindChild("sp"+i).FindChild("nickmoney").GetComponent<UILabel>().text = "金币:" + (bankerList[i].Money).ToString("0,0");
						o_apply_list.transform.FindChild("sp"+i).gameObject.GetComponent<UIFace>().ShowFace((int)bankerList[i].HeadID , (int)bankerList[i].VipLevel);
					}
					else
					{
						bankerList.RemoveAt(i);
					}
				}
			}
		}

        //获取金币按钮金额
        void SetChipScore()
        {
            int tempValue = 0;
            for (int i = 0; i < 6; i++)
            {
                tempValue = m_lGameChips[i];
				if( tempValue==0) break;
                switch (tempValue)
                {
				case 1:   m_lChipScore[i] = 1;	 	    break;
				case 2:   m_lChipScore[i] = 5; 	 	    break;
				case 3:   m_lChipScore[i] = 10; 	 	break;
				case 4:   m_lChipScore[i] = 50;	 	    break;
				case 5:   m_lChipScore[i] = 100; 	 	break;
				case 6:   m_lChipScore[i] = 500; 	 	break;
				case 7:   m_lChipScore[i] = 1000;	 	break;
				case 8:   m_lChipScore[i] = 5000; 	 	break;
				case 9:   m_lChipScore[i] = 10000; 	 	break;
				case 10:  m_lChipScore[i] = 50000;	 	break;
				case 11:  m_lChipScore[i] = 100000; 	break;
				case 12:  m_lChipScore[i] = 500000; 	break;
				case 13:  m_lChipScore[i] = 1000000; 	break;
                }
            }
            SetAddBtn();
        }

        //添加显示金币
        void SetAddBtn()
        {
			if(m_lChipNum==0) return;

			Vector3 point1 = o_chip_Pos[0].transform.localPosition;
			Vector3 point2 = o_chip_Pos[5].transform.localPosition;
			float distance = (point2.x - point1.x)/m_lChipNum;

			for (int i = 0; i < m_lChipNum; i++)
            {
                o_chip_Btn[i].transform.FindChild("btn_show").GetComponent<UISprite>().spriteName = "chip_" + formatMoneyW(m_lChipScore[i]);
				o_chip_Btn[i].transform.localPosition = point1 + new Vector3( (i+0.5f)*distance,0,0);
				o_chip_Btn[i].SetActive(true);
            }
        }

		#endregion

        #region ###############框架消息#################
        //框架事件入口
        private void OnTableUserEvent(TableEvents tevt, uint userid, object data)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

			switch (tevt)
			{
				case TableEvents.GAME_START:
				{	
					//Debug.LogWarning("GAME_START");
					UpdateUserView();
					break;
				}
				case TableEvents.USER_COME:
				{	
					//Debug.LogWarning("USER_COME");
					UpdateUserView();
					break;
				}
				case TableEvents.USER_LEAVE:
				{
					//任意玩家离开时调用
					//Debug.LogWarning("USER_LEAVE");
					UpdateUserView();
					break;
				}
				case TableEvents.USER_READY:
				{
					//任意玩家进入时调用
					//Debug.LogWarning("USER_READY");
					UpdateUserView();
					break;
				}
				case TableEvents.USER_PLAY:
				{		
					//Debug.LogWarning("USER_PLAY");
					UpdateUserView();	
					break;
				}
				case TableEvents.USER_OFFLINE:
				{	
					//Debug.LogWarning("USER_OFFLINE");
					UpdateUserView();
					break;
				}
                case TableEvents.GAME_ENTER:
                {
					//进入时(本人)
					//Debug.LogWarning("GAME_ENTER");
				    InitGameView();
				    s_bStart = true;
					UpdateUserView();
                    break;
	            }
			}
        }

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;
            switch (subcmd)
            {

				case SubCmd.SUB_GF_OPTION:
					{
						//Debug.LogWarning("框架消息+游戏配置");
						OnGameOptionResp(packet);
						break;
					}
                case SubCmd.SUB_GF_SCENE:
                    {
						//Debug.LogWarning("框架消息+场景信息");

						//接受场景时初始化
						m_lCurrentJetton = 0;
						m_canChip = false;

                        OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
				case SubCmd.SUB_GF_MESSAGE:
					{
						//Debug.LogWarning("框架消息+系统消息");
						OnGameMessageResp(packet);
						break;
					}
            }
        }

		//游戏设置消息处理函数
		void OnGameOptionResp(NPacket packet)
		{
			packet.BeginRead();				
			GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
		}

		//系统消息
		void OnGameMessageResp(NPacket packet)
		{
			Debug.LogWarning("系统消息");
			packet.BeginRead();
			ushort wType = packet.GetUShort();
			ushort wlen = packet.GetUShort();
			string strMsg = packet.GetString(wlen);					
				if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
				    (wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
				{
					
					Invoke("OnConfirmBackOKIvk", 2.0f);
					//_bStart = false;
				}
				
				if ((wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
				{
					Invoke("OnConfirmBackOKIvk", 2.0f);
					//_bStart = false;
				}
			

		}

		#region ##########################场景消息处理############################
		//游戏场景消息处理函数
		void OnGameSceneResp(byte bGameStatus, NPacket packet)
		{
			//Debuger.LogError(bGameStatus);
			switch (bGameStatus)
			{
				//空闲状态
				case (byte)GameLogic.GAME_SCENE_FREE:
				{
					//Debug.LogWarning("框架消息+空闲状态");
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_FREE;
					SwitchFreeSceneView(packet);
					GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
					break; 
				}
					
				//游戏状态(下注)
				case (byte)GameLogic.GAME_SCENE_PLACE_JETTON:
				{
					//Debug.LogWarning("框架消息+下注状态");
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_PLACE_JETTON;
					SwitchPlaySceneView(packet);
					PlayGameSound(SoundType.START);
					GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
					break;
				}
					
				//结束状态（开牌）
				case (byte)GameLogic.GAME_SCENE_GAME_END:
				{
					//Debug.LogWarning("框架消息+开牌状态");
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_GAME_END;
					SwitchPlaySceneView(packet);
					GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
					break;
				}
			}
		}

		//游戏状态切换更新(接受场景)
		void GameStateChangeStart(byte state)
		{
			o_clock_num.SetActive(true);
			o_chip_buttons.SetActive(true);

			switch(state)
			{
				case (byte)GameLogic.GAME_SCENE_FREE:
				{
					o_gameTime_label.GetComponent<UISprite>().spriteName = "word_idle";
					break;
				}
				case (byte)GameLogic.GAME_SCENE_PLACE_JETTON:
				{
					o_gameTime_label.GetComponent<UISprite>().spriteName = "word_xiazhu";
					break;
				}
				case (byte)GameLogic.GAME_SCENE_GAME_END:
				{
					o_gameTime_label.GetComponent<UISprite>().spriteName = "word_kaipai";
					break;
				}
			}
		}
		
		//初始场景处理函数
		void SwitchFreeSceneView(NPacket packet)
		{
			ResetGameView();				
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_FREE;
			
			packet.BeginRead();
			
			//全局信息
			byte cbTimeLeave = packet.GetByte();			//剩余时间

			//玩家信息
			long lUserMaxScore = packet.GetLong();			//玩家金币(总金币)
			//庄家信息
			ushort lBankerUser = packet.GetUShort();		//当前庄家
			ushort lBankerTimes = packet.GetUShort();		//庄家局数（坐庄次数）
			long lBankerWinScore = packet.GetLong();		//庄家总金币
			long lBankerScore = packet.GetLong();			//庄家积分（空值）
			bool lEnableSysBanker = packet.GetBool();		//系统做庄
			
			//控制信息
			long lApplybankerCondition = packet.GetLong();	//申请条件
			long lAreaLimitScore = packet.GetLong();		//区域限制

            //筹码类型
            int tempCount = 0;
            for (int i = 0; i < 13; i++)
            {
                m_lAllChips[i] = packet.GetInt();
                if (m_lAllChips[i] == 1 && tempCount < 6)
                {
                    m_lGameChips[tempCount] = i + 1;
                    tempCount++;
                }
            }
			m_lChipNum = tempCount;
//             int tempCount2 = 0;
//             while (tempCount < 6)
//             {
//                 m_lAllChips[tempCount2++] = 1;
//                 tempCount = 0;
//                 for (int i = 0; i < 13; i++)
//                 {
//                     if (tempCount >= 5) break;
//                     if (m_lAllChips[i] == 1 && tempCount < 6)
//                     {
//                         m_lGameChips[tempCount] = i + 1;
//                         tempCount++;
//                     }
//                 }
//             }

            SetChipScore();


			//系统坐庄名称
			systemBankerName = packet.GetString(32);

			//普通做庄次数
			startBankerTime = packet.GetInt();
			//Debug.LogWarning(startBankerTime+"初始");
			//房间信息
			//TCHAR							szGameRoomName[SERVER_LEN];			//房间名称
			string szGameRoomName = packet.GetString(32);

			m_wCurrentBanker = lBankerUser;
			m_lBankerWinScore = lBankerWinScore;
			m_bEnableSysBanker = lEnableSysBanker;
			m_wBankerTime = lBankerTimes;

			s_bBankerUser = (byte)m_wCurrentBanker;
			m_ApplyCondition = lApplybankerCondition;
			m_AreaLimit = lAreaLimitScore;
			
			for(int i=0; i<5; i++)
			{
				m_lAreaInAllScore[i] = 0;
				s_lTableScore[i] = 0;

				if(o_chips_count[i] != null)
				{
					o_chips_count[i].SetActive(false);
				}

				if(o_player_chips[i] != null)
				{
					o_player_chips[i].transform.GetComponent<UISprite>().spriteName = "blank";
					o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(false);
					o_player_chips[i].transform.FindChild("reslut_count").gameObject.SetActive(false);
				}
			}

			SetUserClock(GetSelfChair(),(uint)cbTimeLeave, TimerType.TIMER_READY);
			SetBankerInfo(lBankerUser, lBankerScore);

			if(UIManager.Instance.o_loading!=null)
			{
				UIManager.Instance.o_loading.SetActive(false);
			}

			for(int i=0; i<25; i++)
			{
				int randomCount = UnityEngine.Random.Range(1, 66);
				m_cbTableCardArrayAll[i] = (byte)randomCount;
			}
			controlCard.GetComponent<UICardControl>().SetCardData(m_cbTableCardArrayAll, (byte)m_cbTableCardArrayAll.Length);

			closeLoading();
			UpdateUserView();
			UpdateButtonContron();
		}
		
		//游戏场景处理函数
		void SwitchPlaySceneView(NPacket packet)
		{
			s_bStart = true;
			
			ResetGameView();
			
			packet.BeginRead();
			
			long[]	lAreaScore = new long[GameLogic.AREA_COUNT]; 			//每个区域的总分	

			
			//全局下注
			for(int i = 0; i < GameLogic.AREA_COUNT; i++)
			{
				lAreaScore[i] = packet.GetLong();
			}
			
			//玩家下注
			for(int i = 0; i < GameLogic.AREA_COUNT; i++)
			{
				s_lTableScore[i] = packet.GetLong();
			}
			
			long lUserScore = packet.GetLong();								//最大下注
			long lApplyCondition = packet.GetLong();						//上庄申请条件
			m_AreaLimit = packet.GetLong();								//区域限制

			//扑克信息
			int j = 0;
			for(int x=0; x<5; x++)
			{
				for(int y=0; y<5; y++ )
				{
					m_cbTableCardArray[x][y] = packet.GetByte();
					m_cbTableCardArrayAll[j] = m_cbTableCardArray[x][y];
					j++;
				}
			}

			//扑克类型
			for(int idCount=0; idCount<m_cbCardType.Length; idCount++)
			{
				m_cbCardType[idCount] = packet.GetByte();
			}
			
			//庄家信息
			ushort lBankerUser = packet.GetUShort();				//当前庄家
			ushort lBankTime = packet.GetUShort();					//庄家局数（坐庄局数）
			long lBankerWinScore = packet.GetLong();   				//庄家总金币
			long lBankerScore = packet.GetLong(); 					//庄家分数（空值）

			bool bEnableSysBanker = packet.GetBool();   			//系统做庄
			//结束信息
			long lEndBankerScore = packet.GetLong();				//庄家成绩
			long lEndUserScore = packet.GetLong();					//玩家成绩
			long lEndUserReturnScore = packet.GetLong();			//返回积分
			long lEndRevenue = packet.GetLong();					//游戏税收
			
			//全局信息
			long  TimeLeave = packet.GetByte();						//剩余时间
			byte cbGameStatus = packet.GetByte();					//游戏状态

			//筹码类型
            int tempCount = 0;
            for (int i = 0; i < 13; i++)
            {
                m_lAllChips[i] = packet.GetInt();
                if (m_lAllChips[i] == 1 && tempCount < 6)
                {
                    m_lGameChips[tempCount] = i + 1;
                    tempCount++;
                }
            }
			m_lChipNum = tempCount;
//             int tempCount2 = 0;
//             while (tempCount < 6)
//             {
//                 m_lAllChips[tempCount2++] = 1;
//                 tempCount = 0;
//                 for (int i = 0; i < 13; i++)
//                 {
//                     if (tempCount >= 5) break;
//                     if (m_lAllChips[i] == 1 && tempCount < 6)
//                     {
//                         m_lGameChips[tempCount] = i + 1;
//                         tempCount++;
//                     }
//                 }
//             }

            SetChipScore();

			
			//系统坐庄名称
			systemBankerName = packet.GetString(32);

			//普通做庄次数
			startBankerTime = packet.GetInt();
			//Debug.LogWarning(startBankerTime+"初始");
			
			//#define SERVER_LEN 32	//房间长度
			//房间信息
			//TCHAR	szGameRoomName[SERVER_LEN]; //房间名称
			string szGameRoowName = packet.GetString(32);

			m_ApplyCondition = lApplyCondition;
			m_wBankerTime = lBankTime;

			m_wCurrentBanker = lBankerUser;
			m_lBankerWinScore = lBankerWinScore;
			m_bEnableSysBanker = bEnableSysBanker;

			s_bBankerUser = (byte)m_wCurrentBanker;
			UpdateUserView();
			UpdateButtonContron();
			SetUserClock(GetSelfChair(),(uint)TimeLeave, TimerType.TIMER_OPEN);

			//场景更改
			if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GAME_SCENE_GAME_END)
			{
				controlCard.GetComponent<UICardControl>().FrameSetCardData(m_cbTableCardArrayAll, (byte)m_cbTableCardArrayAll.Length);

				if(lEndUserScore >= 0)
				{
					m_MySelfScorelbl.GetComponent<CLabelNum>().m_strTextureName = "win";
				}
				else
				{
					m_MySelfScorelbl.GetComponent<CLabelNum>().m_strTextureName = "lose";
				}
				
				if(lEndBankerScore >= 0)
				{
					m_BanckScorelbl.GetComponent<CLabelNum>().m_strTextureName = "win";
				}
				else
				{
					m_BanckScorelbl.GetComponent<CLabelNum>().m_strTextureName = "lose";
				}
				
				m_selfScore = lEndUserScore;
				m_MySelfScorelbl.GetComponent<CLabelNum>().m_iNum = lEndUserScore;
				m_bankerScore = lEndBankerScore;
				m_BanckScorelbl.GetComponent<CLabelNum>().m_iNum = lEndBankerScore;
				ShowResult();

				for (byte i=0; i<5; i++)
				{
					if (o_chips_count[i] != null && lAreaScore[i] >= 0)
					{
						o_chips_count[i].GetComponent<CLabelNum>().m_iNum = lAreaScore[i];
						o_chips_count[i].SetActive(true);
					}

					AreaShowCount(i);
				}
			}
			else if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
			{
				controlCard.GetComponent<UICardControl>().SetCardData(m_cbTableCardArrayAll, (byte)m_cbTableCardArrayAll.Length);

				for (byte i=0; i<5; i++)
				{
					if (o_chips_count[i] != null && lAreaScore[i] >= 0)
					{
						o_chips_count[i].GetComponent<CLabelNum>().m_iNum = lAreaScore[i];
						o_chips_count[i].SetActive(true);

						m_lAreaInAllScore[i] = (long)lAreaScore[i];

						o_player_chips[i].SetActive(true);

						if(s_lTableScore[i] > 0)
						{
							o_player_chips[i].transform.FindChild("chip_label").gameObject.GetComponent<CLabelNum>().m_iNum = s_lTableScore[i];
							o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(true);

						}
					}
				}
			}

			closeLoading();
		}
		#endregion

        #endregion

        #region ################游戏消息################
        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        { 
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {
                case SubCmd.SUB_S_GAME_FREE:
                    {
                        //游戏空闲
                        OnGameFreeResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_START:
                    {
                        //游戏开始(下注)
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_PLACE_JETTON:
                    {
                        //用户下注
                        OnGamePlaceJetton(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:
                    {
                        //游戏结束（开牌）
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_APPLY_BANKER:
                    {
                        //申请庄家
                        OnSubUserApplyBanker(packet);
                        break;
                    }
                case SubCmd.SUB_S_CHANGE_BANKER:
                    {
                        //切换庄家
                        OnSubUserChangeBanker(packet);
                        break;
                    }
                case SubCmd.SUB_S_CHANGE_USER_SCORE:
                    {
                        //更新积分
                        //Debug.LogWarning("更新积分");

                        break;
                    }
                case SubCmd.SUB_S_SEND_RECORD:
                    {
                        //游戏记录
                        OnSubGameRecord(packet);
                        break;
                    }
                case SubCmd.SUB_S_PLACE_JETTON_FAIL:
                    {
                        //下注失败
                        //Debug.LogWarning("下注失败");							
						OnGameLoseFaire(packet);
                        break;
                    }
                case SubCmd.SUB_S_CANCEL_BANKER:
                    {
                        //取消申请
                        //Debug.LogWarning("取消申请");
                        OnSubUserCancelBanker(packet);
                        break;
                    }
            }
        }

		//游戏状态切换更新
		IEnumerator GameStateCange(byte time, byte	state)
		{
			PlayGameSound(SoundType.TURN);
			TweenRotation.Begin(o_clock.gameObject, 0.35f, Quaternion.Euler(new Vector3(0, 90.0f, 0)));
			yield return new WaitForSeconds(0.35f);
			TweenRotation.Begin(o_clock.gameObject, 0.01f, Quaternion.Euler(new Vector3(0, 270.0f, 0)));
			TweenRotation.Begin(o_clock.gameObject, 0.35f, Quaternion.Euler(new Vector3(0, 360.0f, 0)));
			switch(state)
			{
			case (byte)GameLogic.GAME_SCENE_FREE:
			{
				o_chip_buttons.SetActive(true);
				o_clock_num.SetActive(true);
				SetUserClock(GetSelfChair(),(uint)time, TimerType.TIMER_READY);
				o_gameTime_label.GetComponent<UISprite>().spriteName = "word_idle";
				break;
			}
			case (byte)GameLogic.GAME_SCENE_PLACE_JETTON:
			{
				o_chip_buttons.SetActive(true);
				o_clock_num.SetActive(true);
				SetUserClock(GetSelfChair(),(uint)time, TimerType.TIMER_CHIP);
				PlayGameSound(SoundType.START);
				o_gameTime_label.GetComponent<UISprite>().spriteName = "word_xiazhu";
				break;
			}
			case (byte)GameLogic.GAME_SCENE_GAME_END:
			{
				SetUserClock(GetSelfChair(),(uint)time, TimerType.TIMER_OPEN);
				o_gameTime_label.GetComponent<UISprite>().spriteName = "word_kaipai";
				break;
			}
			}

		}

        //空闲时间
        void OnGameFreeResp(NPacket packet)
        {
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_FREE;

			packet.BeginRead();
			byte cbTimeLeave = packet.GetByte();
			long lListUserCount = packet.GetLong();

			//可坐庄次数
			allBankerTime = packet.GetInt();							
			//Debug.LogWarning(allBankerTime+"总做庄次数");

			StartCoroutine(GameStateCange(cbTimeLeave, GameEngine.Instance.MySelf.GameStatus));
			o_result.SetActive(false);
            //o_result.GetComponent<UITweener>().PlayReverse();
			UpdateButtonContron();

			//清除金币
			foreach(Transform child in ctr_chip_tian.transform)
			{
				Destroy(child.gameObject);
			}
			foreach(Transform child in ctr_chip_di.transform)
			{
				Destroy(child.gameObject);
			}
			foreach(Transform child in ctr_chip_xuan.transform)
			{
				Destroy(child.gameObject);
			}
			foreach(Transform child in ctr_chip_huang.transform)
			{
				Destroy(child.gameObject);
			}

			//下注数据清理
			for(int i=0; i<GameLogic.AREA_COUNT; i++)
			{
				m_cbTableCardArray[i] = new byte[5];
				Array.Clear(m_cbTableCardArray[i], 0, 5);

				if(o_player_chips[i] !=null)
				{
					o_player_chips[i].transform.GetComponent<UISprite>().spriteName = "blank";
					o_player_chips[i].transform.FindChild("reslut_count").gameObject.SetActive(false);
					o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(false);
				}
				
				if(o_chips_count[i] !=null)
				{
					o_chips_count[i].SetActive(false);
				}

				s_lTableScore[i] = 0;
				m_lAreaInAllScore[i] = 0;
			}

			Array.Clear(m_cbTableCardArrayAll, 0, m_cbTableCardArrayAll.Length);
			controlCard.GetComponent<UICardControl>().SetCardData(m_cbTableCardArrayAll, (byte)m_cbTableCardArrayAll.Length);
			controlCard.GetComponent<UICardControl>().ClearCardType();

			//将金币小于上庄条件的玩家从上庄列表中清除
			if(m_ApplyCondition > player_totalMoney && applyBanker == true)
			{
				OnCancelBanker();
			}

			UpdateUserView();
		
        }

        //游戏开始（下注）
        void OnGameStartResp(NPacket packet)
        {
            Debug.LogWarning("游戏开始，玩家进入下注阶段");
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_PLACE_JETTON;

			packet.BeginRead();

			long lBankerScore = packet.GetLong();						//庄家金币
			long lUserMaxScore = packet.GetLong();						//我的金币
			byte cbTimeLeave = packet.GetByte();						//剩余时间

			bool bContiueCard = packet.GetBool();						//继续发牌
			int nChipRobotCount = packet.GetInt();						//人数上限（下注机器人）
			ushort lBankerUser = packet.GetUShort();					//庄家位置

			StartCoroutine(GameStateCange(cbTimeLeave, GameEngine.Instance.MySelf.GameStatus));
			UpdateButtonContron();

			for(int i=0; i<5; i++)
			{
				if(o_chips_count[i] != null)
				{
					o_chips_count[i].GetComponent<CLabelNum>().m_iNum = m_lAreaInAllScore[i];
					o_chips_count[i].SetActive(true);
										
					o_player_chips[i].SetActive(true);

					if(s_lTableScore[i] > 0)
					{
						o_player_chips[i].transform.FindChild("chip_label").gameObject.GetComponent<CLabelNum>().m_iNum = s_lTableScore[i];
						o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(true);
					}
				}
			}

			o_result.SetActive(false);
        }

		//玩家下注
		void OnGamePlaceJetton(NPacket packet)
		{
			GameEngine.Instance.GetTableUserItem(GameEngine.Instance.MySelf.DeskStation);
			//Debug.LogWarning("玩家下注");
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_PLACE_JETTON;


			packet.BeginRead();
			ushort lPlaceUser = packet.GetUShort();						//下注玩家
			byte cbJettonArea = packet.GetByte();						//下注区域
			long lPlaceScore = packet.GetLong();						//当前下注

			//Debug.LogWarning(lPlaceScore+"下注值");

			Vector3 point = new Vector3(0, 0, 0);

			if(lPlaceUser == GetSelfChair())
			{
				point =	UICamera.currentCamera.ScreenToWorldPoint(m_vecClick);
			}

			m_lAreaInAllScore[cbJettonArea] += lPlaceScore;
			
			if ((byte)lPlaceUser == GetSelfChair())
			{
				PlayerInfo playerdata = GameEngine.Instance.GetTableUserItem(lPlaceUser);
				playerdata.Money -= lPlaceScore;
				s_lTableScore[cbJettonArea] += lPlaceScore;
				player_totalMoney = playerdata.Money;
				o_player_money.GetComponent<UILabel>().text = player_totalMoney.ToString("0,0");

				UpdateButtonContron();
			}

			ShowCurChips();
			AppendChips((byte)lPlaceUser,lPlaceScore, cbJettonArea, point);

		}

		//下注失败
		void OnGameLoseFaire(NPacket packet)
		{
			//Debug.LogWarning("下注失败");
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_PLACE_JETTON;
			
			packet.BeginRead();
			ushort lPlaceUser = packet.GetUShort();						//下注玩家
			byte cbJettonArea = packet.GetByte();						//下注区域(客户端需要减去一个单位-1)
			long lPlaceScore = packet.GetLong();						//当前下注

			if(lPlaceUser==GetSelfChair())
			{
				//m_lTouchPoint.RemoveAt(0);
			}

			//Debug.LogWarning("下注失败"+"lPlaceUser："+lPlaceUser+"cbJettonArea:"+cbJettonArea+"lPlaceScore:"+lPlaceScore);

			for(int i=0; i<4; i++)
			{
				if(m_AreaLimit < (m_lAreaInAllScore[i+1]+lPlaceScore))
				{
					cnMsgBox("该区域筹码已达到封顶值，无法下注！");
				}
			}


			if((s_lTableScore[1] + s_lTableScore[2] + s_lTableScore[3] + s_lTableScore[4] + lPlaceScore)*10 > player_totalMoney)
			{
				cnMsgBox("金币不足，无法下注！");
			}

			if(s_bBankerUser != GameLogic.NULL_CHAIR && (m_lAreaInAllScore[4] + m_lAreaInAllScore[1] + m_lAreaInAllScore[2] + m_lAreaInAllScore[3] + + lPlaceScore) * 10
			   > (banker_totalMoney - m_lAreaInAllScore[4] - m_lAreaInAllScore[1] - m_lAreaInAllScore[2] - m_lAreaInAllScore[3]))
			{
				cnMsgBox("庄家金币不足，无法下注！");
			}
		}

        //游戏结束（进入开牌）
        void OnGameEndResp(NPacket packet)
        {
            //Debug.LogWarning("开牌时间");
			GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GAME_SCENE_GAME_END;

			packet.BeginRead();

			//下局信息
			byte cbTimeLeave = packet.GetByte();						//剩余时间

			//扑克信息
			int i = 0;
			for(int x=0; x<5; x++)										
			{
				for(int y=0; y<5; y++)
				{
					m_cbTableCardArray[x][y] = packet.GetByte();
					m_cbTableCardArrayAll[i] = m_cbTableCardArray[x][y];
					i++;
				}
			}

			byte cbLeftCardCount = packet.GetByte();					//扑克数目

			//扑克类型
			for(int idCount=0; idCount<m_cbCardType.Length; idCount++)
			{
				m_cbCardType[idCount] = packet.GetByte();
			}

			byte cbFirstCard = packet.GetByte();						//发牌中第一张牌(无用)

			//庄家信息
			long lBankerScore = packet.GetLong();						//庄家金币
			long lBankerTotallScore = packet.GetLong();					//庄家全部金币
			int nBankerTimeCount = packet.GetInt();						//做庄次数
			//Debug.LogWarning("做庄次数"+nBankerTimeCount);
			//玩家信息
			long lUserScore = packet.GetLong();							//玩家金币(结算金币)
			long lUserReturnScore = packet.GetLong();					//返回积分

			//全局信息
			long lRevenue = packet.GetLong();							//游戏税收

			o_clock_num.SetActive(true);

			m_lBankerWinScore = lBankerTotallScore;

			if(lUserScore >= 0)
			{
				m_MySelfScorelbl.GetComponent<CLabelNum>().m_strTextureName = "win";
			}
			else
			{
				m_MySelfScorelbl.GetComponent<CLabelNum>().m_strTextureName = "lose";
			}

			if(lBankerScore >= 0)
			{
				m_BanckScorelbl.GetComponent<CLabelNum>().m_strTextureName = "win";
			}
			else
			{
				m_BanckScorelbl.GetComponent<CLabelNum>().m_strTextureName = "lose";
			}

			m_selfScore = lUserScore;
			m_MySelfScorelbl.GetComponent<CLabelNum>().m_iNum = lUserScore;
			m_bankerScore = lBankerScore;
			m_BanckScorelbl.GetComponent<CLabelNum>().m_iNum = lBankerScore;
			m_wBankerTime = (ushort)nBankerTimeCount;

			StartCoroutine(GameStateCange(cbTimeLeave, GameEngine.Instance.MySelf.GameStatus));

			byte cardCount = 25;
			controlCard.GetComponent<UICardControl>().SetCardData(m_cbTableCardArrayAll, cardCount);

			//显示先后调整
			Invoke("SendEffectView", 0.1f);
			Invoke("ShowEffectView", sendCardTypeTime*multipleTime);
			Invoke("ShowResult", (sendCardTypeTime + showCardTypeTime) * multipleTime + 0.5f);
			
			//区域结算
			StartCoroutine(ShowAreaCount((sendCardTypeTime + showCardTypeTime) * multipleTime - 1.0f));
        }
		//显示结算界面
		void ShowResult()
		{
//			player_totalMoney += m_selfScore;
//			UpdateButtonContron();
			if(m_selfScore >= 0)
			{
				o_result.transform.FindChild("nn_sprite").gameObject.GetComponent<UISprite>().spriteName = "result_win";
				PlayGameSound(SoundType.WIN);
			}
			else{
				o_result.transform.FindChild("nn_sprite").gameObject.GetComponent<UISprite>().spriteName = "result_lose";
				PlayGameSound(SoundType.LOSE);
			}
			o_result.SetActive(true);
            //o_result.GetComponent<UITweener>().PlayForward();

		}

		//发牌效果
		void SendEffectView()
		{
			controlCard.GetComponent<UICardControl>().SendOutTween(sendCardTypeTime, false);
		}

        //开牌效果
        void ShowEffectView()
        {
			controlCard.GetComponent<UICardControl>().CardRotationTween(showCardTypeTime, true);
        }

		//区域结算显示(延时操作调用)
		IEnumerator ShowAreaCount(float time)
		{
			yield return new WaitForSeconds(time);
			for(byte i=0; i<5; i++)
			{
				AreaShowCount(i);
			}
		}

        //申请庄家
        void OnSubUserApplyBanker(NPacket packet)
        {
            //Debug.LogWarning("申请上庄成功");

			packet.BeginRead();
			ushort lApplyUser = packet.GetUShort();							//申请玩家
	
			PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer(lApplyUser);
			if (bankerdata != null)
			{
				bankerList.Add(bankerdata);
			}
			if( lApplyUser == GetSelfChair() )
			{
				applyBanker = true;
				o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(false);
				o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				o_apply_list.transform.FindChild("cancelBanker").gameObject.SetActive(true);
			}
			updateApplyList();
        }

        //切换庄家
        void OnSubUserChangeBanker(NPacket packet)
        {
            //Debug.LogWarning("切换庄家");

			packet.BeginRead();
			ushort lBankerUser = packet.GetUShort();						//当庄玩家	
			//Debug.LogWarning("当庄玩家"+lBankerUser);
			long  lBankerScore = packet.GetLong();							//庄家金币
			PlayerInfo bankerdata = GameEngine.Instance.GetTableUserItem(lBankerUser);
			SetBankerInfo(lBankerUser,lBankerScore);
			PlayerInfo banker = GameEngine.Instance.EnumTablePlayer(lBankerUser);

			s_bBankerUser = (byte)lBankerUser;
			byte currUser = (byte)GetSelfChair();
			if(banker == null)
			{
				m_bEnableSysBanker = true;
				o_bank_player.transform.FindChild("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace(0, 0);
				o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = "";
				o_bank_player.transform.FindChild("ctr_money").transform.FindChild("lbl_nick").GetComponent<UILabel>().text = systemBankerName;
			}else
			{
				bankerdata.Money = banker.Money;
				m_bEnableSysBanker = false;
				o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace((int)banker.HeadID , (int)banker.VipLevel);
				o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = (banker.Money).ToString("0,0");
				o_bank_player.transform.FindChild("ctr_money").transform.FindChild("lbl_nick").GetComponent<UILabel>().text = banker.NickName;
			}

			if((byte)GetSelfChair() == lBankerUser)
			{
				applyBanker = false;
				o_apply_btn.SetActive(false);
				o_cancel_btn.SetActive(true);
				o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(false);
				//o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(true);
				o_apply_list.transform.FindChild("cancelBanker").gameObject.SetActive(false);
			}else
			{
				o_apply_btn.SetActive(true);
				o_cancel_btn.SetActive(false);

				o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(true);
				//o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				o_apply_list.transform.FindChild("cancelBanker").gameObject.SetActive(false);

				PlayerInfo playerData = GameEngine.Instance.EnumTablePlayer((uint)currUser);

				for(int i = 0; i < bankerList.Count; i++)
				{
					if(playerData == bankerList[i])
					{	
						o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(false);
						o_apply_list.transform.FindChild("cancelBanker").gameObject.SetActive(true);
					}
				}

			}

			bankerList.Remove(bankerdata);
			cnMsgBox("庄家轮换！");

			updateApplyList();
			UpdateUserView();
        }

		
		//取消申请
		public void OnCancelBanker()
		{
			applyBanker = false;
			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_CANCEL_BANKER);
			GameEngine.Instance.Send (packet);
		}

        //取消申请(服务发送)
        void OnSubUserCancelBanker(NPacket packet)
        {
            //Debug.LogWarning("取消申请！");
			packet.BeginRead();
			string szCancelUser = packet.GetString(32);

			PlayerInfo userdata = GameEngine.Instance.MySelf;
			if(userdata != null)
			{
				for(int i = 0; i < bankerList.Count; i++)
				{
					if(szCancelUser == bankerList[i].NickName)
					{	
						bankerList.Remove(bankerList[i]);
					}
				}
			}
			if(userdata.NickName == szCancelUser)
			{
				o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(true);
				//o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				o_apply_list.transform.FindChild("cancelBanker").gameObject.SetActive(false);
			}
			updateApplyList();
        }

        //游戏记录
        void OnSubGameRecord(NPacket packet)
        {
           //Debug.LogWarning("游戏记录");
			packet.BeginRead();
			ushort lDataSize = packet.DataSize;
			notesData_leng = (byte)lDataSize;
			int cbNum = 1;
			int j = 0;

			if(lDataSize-8 <= 100)
			{
				for(int i=0; i<lDataSize-8; i++)
				{
					m_buffer[i] = packet.GetByte();
					int k = i/4;
					int id = i%4;
					switch(id)
					{
					case 0:
					{
						result_notes[0][k] = m_buffer[i];
						break;
					}
					case 1:
					{
						result_notes[1][k] = m_buffer[i];
						break;
					}
					case 2:
					{
						result_notes[2][k] = m_buffer[i];
						break;
					}
					case 3:
					{
						result_notes[3][k] = m_buffer[i];
						break;
					}
					}
				}
			}

			//初始化记录
			ResultNotes();
        }
        #endregion   

		#region ###############UI事件###################
        //获取时间显示
        void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            if (time >= 2)
            {
                o_clock_num.GetComponent<UIClock>().SetTimer((time - 1));
            }
        }
	
		//上庄界面申请上庄按钮
		public void OnApplyForBanker()
		{
			//庄家判断
			if ( GetSelfChair() == (byte)m_wCurrentBanker ) return;
			
			if(bankerList.Count > 4)
			{
				cnMsgBox("申请人数已满！");
				return;
			}
			if(GameEngine.Instance.EnumTablePlayer(GetSelfChair()).Money < m_ApplyCondition)
			{
				if(m_ApplyCondition >= 10000)
				{
					cnMsgBox("金币少于"+ m_ApplyCondition + "，无法申请上庄！");
				}
				else
				{
					cnMsgBox("金币不足，无法申请上庄！");
				}
				return;
			}

			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_APPLY_BANKER);
			GameEngine.Instance.Send (packet);
		}

		//获取自己座位号
		byte GetSelfChair()
		{
			return (byte)GameEngine.Instance.MySelf.DeskStation;
		}

		//金币数显示格式
		string formatMoney(long money)
		{
			string tempMoney = money.ToString();			
			if(money>9999) 
			{
				tempMoney = ((money / 10000)+((money % 10000)/1000)*0.1).ToString()+"万";
			}
			return tempMoney;
		}

        //金币按钮显示格式
        string formatMoneyW(long money)
        {
            string tempMoney = money.ToString();
            if (money > 9999)
            {
                tempMoney = (money / 10000).ToString() + "w";
            }
            return tempMoney;
        }

		//退出按钮事件
		public void OnBtnBackIvk()
		{
			//Debug.LogWarning("退出");

			if((byte)GetSelfChair() == s_bBankerUser)
			{
				if (!GameEngine.Instance.IsPlaying())
				{
					OnConfirmBackOKIvk();
				}
				else
				{
					cnMsgBox("游戏进行中，无法离开!");
				}
			}
			else
			{
				if (!GameEngine.Instance.IsPlaying() || (s_lTableScore[1] + s_lTableScore[2] + s_lTableScore[3] + s_lTableScore[4] == 0))
				{
					OnConfirmBackOKIvk();
				}
				else
				{
					cnMsgBox("游戏进行中，无法离开!");
				}
			}
		}

		//确认退出
		void OnConfirmBackOKIvk()
		{
			s_bStart = false;
			s_bReqQuit = true;
			GameEngine.Instance.Quit();
            s_nQuitDelay = Time.realtimeSinceStartup;
			CancelInvoke();				
		}

		//消息框
		public void cnMsgBox(string val)
		{
			if(val == boxStr)
			{
				return;
			}

			if(o_msgbox != null)
			{
				o_msgbox.SetActive(true);
				o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = val;
				Invoke("closeMsgbox",3);
			}

			boxStr = val;
		}
		void closeMsgbox()
		{
			boxStr = ",";
			o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = "";
			o_msgbox.SetActive(false);
		}

		//关闭信息显示
	    public void ClearAllInfo()
		{
			o_player_info.SetActive(false);
			o_banker_info.SetActive(false);
		}

		//申请上庄按钮事件
		public void OnBtnApplyIvk()
		{
            o_apply_list.SetActive(true);
            StartCoroutine(ShowConCealLump(o_apply_list));
            updateApplyList();

		} 

		//规则按事件
		public void OnBtnRuleIvk()
		{
            o_rules.SetActive(true);
            StartCoroutine(ShowConCealLump(o_rules));
//          s_nInfoTickCount = Environment.TickCount;            		
        }

		//记录按钮事件
		public void OnBtnTrendIvk()
		{
            StartCoroutine(ShowConCealLump(o_notes));
		}

		#region ##########金币按钮控制###########

        //初始化筹码
		void onBtnSelect()
		{
            for (int i = 0; i < 6; i++)
            {
                o_chip_Btn[i].transform.FindChild("selected").gameObject.SetActive(false);
            }
		}

        //跟新筹码显示
		void UpdateButtonContron()
		{
            for (int i = 0; i < 6; i++)
            {
                o_chip_Btn[i].GetComponent<UIButton>().isEnabled = true;
                o_chip_Btn[i].transform.FindChild("btn_show").GetComponent<UISprite>().color = Color.white;
            }

			if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
			{
				//onBtnSelect();
			}

			long lMoney = (long)(player_totalMoney/10);

			DisableBtnIvk(lMoney);
		}
		
        //判断需要关闭与隐藏的金币按钮
		void DisableBtnIvk( long lScore)
		{
            do
            {
				if(m_lChipScore[6]!=0 && o_chip_Btn[6].activeSelf){
	                if (lScore >= m_lChipScore[6]) break;
	                o_chip_Btn[6].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[6].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[6].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_6();
	                }
				}
				if(m_lChipScore[5]!=0 && o_chip_Btn[5].activeSelf){
	                if (lScore >= m_lChipScore[5]) break;
	                o_chip_Btn[5].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[5].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[5].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_5();
	                }
				}
				if(m_lChipScore[4]!=0 && o_chip_Btn[4].activeSelf){
	                if (lScore >= m_lChipScore[4]) break;
	                o_chip_Btn[4].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[4].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[4].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_4();
	                }
				}
				if(m_lChipScore[3]!=0 && o_chip_Btn[3].activeSelf){
	                if (lScore >= m_lChipScore[3]) break;
	                o_chip_Btn[3].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[3].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[3].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_3();
	                }
				}
				if(m_lChipScore[2]!=0 && o_chip_Btn[2].activeSelf){
	                if (lScore >= m_lChipScore[2]) break;
	                o_chip_Btn[2].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[2].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[2].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_2();
	                }
				}
				if(m_lChipScore[1]!=0 && o_chip_Btn[1].activeSelf){
	                if (lScore >= m_lChipScore[1]) break;
	                o_chip_Btn[1].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[1].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[1].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    OnBtnAddIvk_1();
	                }
				}
				if(m_lChipScore[0]!=0 && o_chip_Btn[0].activeSelf){
	                if (lScore >= m_lChipScore[0]) break;
	                o_chip_Btn[0].GetComponent<UIButton>().isEnabled = false;
	                o_chip_Btn[0].transform.FindChild("btn_show").GetComponent<UISprite>().color = new Color(67 / 255f, 67 / 255f, 67 / 255f, 1f);
	                if (o_chip_Btn[0].transform.FindChild("selected").gameObject.activeSelf)
	                {
	                    o_chip_Btn[0].transform.FindChild("selected").gameObject.SetActive(false);
	                }
				}
            } while (false);
		}

		//金币按钮事件
        public void OnBtnAddIvk_1()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[0];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[0].transform.FindChild("selected").gameObject.SetActive(true);
        }
        public void OnBtnAddIvk_2()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[1];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[1].transform.FindChild("selected").gameObject.SetActive(true);
        }
        public void OnBtnAddIvk_3()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[2];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[2].transform.FindChild("selected").gameObject.SetActive(true);
        }
        public void OnBtnAddIvk_4()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[3];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[3].transform.FindChild("selected").gameObject.SetActive(true);
        }
        public void OnBtnAddIvk_5()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[4];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[4].transform.FindChild("selected").gameObject.SetActive(true);
        }
        public void OnBtnAddIvk_6()
        {
            onBtnSelect();
            m_canChip = true;
            m_lCurrentJetton = m_lChipScore[5];
            m_lLastJetton = m_lCurrentJetton;
            o_chip_Btn[5].transform.FindChild("selected").gameObject.SetActive(true);
        }

		#endregion

		#region #########下注控制##########

		//加注消息
		void OnPlaceJetton(byte wParam, long lParam)
		{
            //Debug.LogWarning("加注消息");
			//庄家判断
			if(GetSelfChair() == (byte)m_wBankerUser)
			{
				cnMsgBox("你已经是庄家，无法下注!");
				return;
			}
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			long lMoney = userdata.Money - (s_lTableScore[1] + s_lTableScore[2] + s_lTableScore[3] + s_lTableScore[4]);
            if (lMoney < m_lChipScore[0]*10) 
			{
				cnMsgBox("金币低于本桌最低所需金币，无法下注!");
				return;
			}

#if UNITY_STANDALONE_WIN
			m_vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
			//m_lTouchPoint.Add(m_vecClick);

#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
			m_vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
			int i = 0;
			foreach(var touch in Input.touches)
			{
				if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					i++;
				}
			}
			if(i > 1)
			{
				return;
			}
				
			if(Input.touchCount > 1)
			{
				return;
			}

//			if(Input.GetTouch(1).phase == TouchPhase.Stationary)
//			{
//				return;
//			}
#endif
			NPacket packet = NPacketPool.GetEnablePacket();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_PLACE_JETTON);
			packet.AddLong(lParam);
			packet.Addbyte(wParam);
			GameEngine.Instance.Send(packet);
		}

		//押注区域的点击事件
		public void OnBtnPlace_Tian()
		{
            if (GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
            {
                cnMsgBox("非下注时间!");
                return;
            }
           
            if (m_canChip == false)
            {
                cnMsgBox("请选择筹码!");
                return;
            }

             OnPlaceJetton(GameLogic.ID_TIAN_MEN, m_lCurrentJetton);
		}
		public void OnBtnPlace_Di()
		{
            if (GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
            {
                cnMsgBox("非下注时间!");
                return;
            }

            if (m_canChip == false)
            {
                cnMsgBox("请选择筹码!");
                return;
            }

            OnPlaceJetton(GameLogic.ID_DI_MEN, m_lCurrentJetton);
		}
		public void OnBtnPlace_Xuan()
		{
            if (GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
            {
                cnMsgBox("非下注时间!");
                return;
            }

            if (m_canChip == false)
            {
                cnMsgBox("请选择筹码!");
                return;
            }

            OnPlaceJetton(GameLogic.ID_XUAN_MEN, m_lCurrentJetton);
		}
		public void OnBtnPlace_Huang()
		{
            if (GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_PLACE_JETTON)
            {
                cnMsgBox("非下注时间!");
                return;
            }

            if (m_canChip == false)
            {
                cnMsgBox("请选择筹码!");
                return;
            }

            OnPlaceJetton(GameLogic.ID_HUANG_MEN, m_lCurrentJetton);
		}

		//下注筹码数值显示
		void ShowCurChips()
		{
			for (byte i=0; i<5; i++)
			{
				if (o_chips_count[i] != null && m_lAreaInAllScore[i] > 0)
				{
					o_chips_count[i].GetComponent<CLabelNum>().m_iNum = m_lAreaInAllScore[i];
					o_chips_count[i].SetActive(true);
				}
				
				if(o_player_chips[i] != null && s_lTableScore[i] >= 0)
				{
					o_player_chips[i].transform.FindChild("reslut_count").gameObject.SetActive(false);

					if(s_lTableScore[i] > 0)
					{
						o_player_chips[i].transform.FindChild("chip_label").gameObject.GetComponent<CLabelNum>().m_iNum = s_lTableScore[i];
						o_player_chips[i].transform.FindChild("chip_label").gameObject.SetActive(true);
					}
				}
			}
		}

		//生成筹码
		void AppendChips(byte bchair, long nUserChips, byte chipsArea , Vector3 point)
		{
			switch(chipsArea)
			{
			case GameLogic.ID_TIAN_MEN:
				
				UIChipControl ctr1 = ctr_chip_tian.GetComponent<UIChipControl>();
				ctr1.AddChips(bchair, nUserChips, chipsArea, point);			
				break;	
				
			case GameLogic.ID_DI_MEN:
				UIChipControl ctr2 = ctr_chip_di.GetComponent<UIChipControl>();
				ctr2.AddChips(bchair, nUserChips, chipsArea, point);
				break;
				
			case GameLogic.ID_XUAN_MEN:
				UIChipControl ctr3 = ctr_chip_xuan.GetComponent<UIChipControl>();
				ctr3.AddChips(bchair, nUserChips, chipsArea, point);
				break;
				
			case GameLogic.ID_HUANG_MEN:
				UIChipControl ctr4 = ctr_chip_huang.GetComponent<UIChipControl>();
				ctr4.AddChips(bchair, nUserChips, chipsArea, point);
				break;
			}
		}

		#endregion

		//庄家信息
		void SetBankerInfo(ushort wBankerUser,long lBankerScore) 
		{
			//切换判断
			if (m_wCurrentBanker!=wBankerUser)
			{
				m_wCurrentBanker = wBankerUser;
				m_wBankerTime = 0;
				m_lBankerWinScore = 0;	
//				m_lTmpBankerWinScore = 0;
			}
			m_lBankerScore=lBankerScore;
		}

		//申请下庄
		public void OnCancelForBanker()
		{
			if (GetSelfChair() != (byte)m_wCurrentBanker) return;
			if (GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GAME_SCENE_FREE)
			{
				cnMsgBox("游戏进行中，无法下庄！");
				return ;
			}
			NPacket packet = NPacketPool.GetEnablePacket();
			packet.CreateHead(MainCmd.MDM_GF_GAME,SubCmd.SUB_C_CANCEL_BANKER);
			GameEngine.Instance.Send(packet);

			updateApplyList();
		}

		//玩家信息
		public void OnPlayerInfoIvk()
		{
			bool show = !o_player_info.activeSelf;
			o_player_info.SetActive(show);
			o_banker_info.SetActive(false);

			clearPlayerInfo(o_player_info);
			updatePlayerInfo(o_player_info);

            if (show == true)
            {
                conCealTween.SetActive(true);
            }

            s_nInfoTickCount = Time.realtimeSinceStartup;
		}
		
        //更新玩家信息
		void updatePlayerInfo(GameObject info)
		{
			if(info.activeSelf)
			{
				PlayerInfo player = GameEngine.Instance.EnumTablePlayer(GetSelfChair());
//				info.GetComponent<UIFace>().ShowFace((int)player.HeadID,(int)player.VipLevel);
				info.transform.FindChild("lbl_id").GetComponent<UILabel>().text = player.ID.ToString();
				info.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = player.NickName;
				info.transform.FindChild("lbl_money").GetComponent<UILabel>().text = player.Money.ToString("0,0");
				info.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "0";
				info.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lMeGameScoreCount.ToString("0,0");
				
				if(player.Gender == 0)
				{
					info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				}
				else
				{
					info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "女";
				}
				
				if(GetSelfChair() == (byte)m_wBankerUser)
				{
					info.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
				}
				else
				{
					info.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = "0";
				}
			}
		}
		
		//庄家信息框信息
		public void OnBankerInfoIvk()
		{
			bool show = !o_banker_info.activeSelf;
			o_player_info.SetActive(false);

			// 
			if(s_bBankerUser == GameLogic.NULL_CHAIR || m_wCurrentBanker == GameLogic.NULL_CHAIR)
			{
				return;
			}

			o_banker_info.SetActive(show);

			clearPlayerInfo(o_banker_info);
			updateBankerInfo();

			if(show == true)
			{
				conCealTween.SetActive(true);
            }

            s_nInfoTickCount = Time.realtimeSinceStartup;
		}
		
        //更新庄家信息
		void updateBankerInfo()
		{
			PlayerInfo player = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);
			if(player != null)
			{
//				o_banker_info.GetComponent<UIFace>().ShowFace((int)player.HeadID,(int)player.VipLevel);
				o_banker_info.transform.FindChild("lbl_id").GetComponent<UILabel>().text = player.ID.ToString();
				o_banker_info.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = player.NickName;
				o_banker_info.transform.FindChild("lbl_money").GetComponent<UILabel>().text = player.Money.ToString("0,0");
				o_banker_info.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "0";	
				o_banker_info.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lBankerWinScore.ToString("0,0");
				o_banker_info.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
				
				if(player.Gender == 0)
				{
					o_banker_info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				}else
				{
					o_banker_info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "女";
				}
			}
			else
			{
//				o_banker_info.GetComponent<UIFace>().ShowFace(0,0);
				o_banker_info.transform.FindChild("lbl_id").GetComponent<UILabel>().text = "0";
				o_banker_info.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = systemBankerName;
				o_banker_info.transform.FindChild("lbl_money").GetComponent<UILabel>().text = "0";
				o_banker_info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				o_banker_info.transform.FindChild("lbl_score").GetComponent<UILabel>().text = "0";
				o_banker_info.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
			}
		}
		
        //清除玩家信息
		void clearPlayerInfo(GameObject info)
		{
			info.transform.FindChild("lbl_id").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_money").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_score").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "";
			info.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = "";
		}

		//游戏记录(初始化)
		void ResultNotes()
		{
			int data_leng = (notesData_leng-8)/4; 

			for(int i=0; i<4; i++)
			{
				foreach(Transform child in notes_data[i].transform)
				{
					Destroy(child.gameObject);
				}

				if(data_leng <= 10)
				{
					for(int k=0; k<data_leng; k++)
					{
						GameObject obj = Instantiate(NotesPrefabs);
						obj.transform.parent = notes_data[i].transform;
						obj.transform.localScale = new Vector3(1, 1, 1);
						obj.transform.localPosition = new Vector3(40.0f*(k), 0, 0);
						if(result_notes[i][k] == 1)
						{
							obj.transform.GetComponent<UISprite>().spriteName = "data_win";
						}
						else
						{
							obj.transform.GetComponent<UISprite>().spriteName = "data_lose";
						}
					}
				}
				else
				{
					for(int k=data_leng%10; k<data_leng; k++)
					{
						GameObject obj = Instantiate(NotesPrefabs);
						obj.transform.parent = notes_data[i].transform;
						obj.transform.localScale = new Vector3(1, 1, 1);
						obj.transform.localPosition = new Vector3(40.0f*(k-data_leng%10), 0, 0);
						if(result_notes[i][k] == 1)
						{
							obj.transform.GetComponent<UISprite>().spriteName = "data_win";
						}
						else
						{
							obj.transform.GetComponent<UISprite>().spriteName = "data_lose";
						}
					}
				}
			}
		}

		//各区域结算显示
		void AreaShowCount(byte id)
		{
			if(s_lTableScore[id]==0 && id!=0)
			{
				o_player_chips[id].transform.GetComponent<UISprite>().spriteName = "not_result";
				o_player_chips[id].transform.FindChild("chip_label").gameObject.SetActive(false);
			}
			else if(id != 0)
			{
				bool win = false;
				int i = 1;
				byte cardTypeCount = 0;
				o_player_chips[id].transform.GetComponent<UISprite>().spriteName = "blank";
				o_player_chips[id].transform.FindChild("reslut_count").gameObject.SetActive(true);
				o_player_chips[id].transform.FindChild("chip_label").gameObject.SetActive(false);

				if(m_cbCardType[id] == m_cbCardType[0])
				{
					if((UICardControl.maxCardArea[id] > UICardControl.maxCardArea[0])
					   || ((UICardControl.maxCardArea[id] == UICardControl.maxCardArea[0]) 
					     && UICardControl.maxCardColor[id] > UICardControl.maxCardColor[0]))
					{
						win = true;
					}
					else
					{
						win = false;
					}

				}

				if(m_cbCardType[id]>m_cbCardType[0] || win==true)
				{
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("point")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "win";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("count")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "win";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("niu")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "win";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("count_x")
						.transform.GetComponent<UISprite>().spriteName = "win_x";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("area_win")
						.gameObject.SetActive(true);
					
					cardTypeCount = m_cbCardType[id];
				}
				else
				{
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("point")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "lose";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("count")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "lose";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("niu")
						.transform.GetComponent<CLabelNum>().m_strTextureName = "lose";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("count_x")
						.transform.GetComponent<UISprite>().spriteName = "lose_x";
					o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("area_win")
						.gameObject.SetActive(false);
					
					cardTypeCount = m_cbCardType[0];
					i = -1;
				}
				
				if(cardTypeCount >= 12)
				{
					cardTypeCount = 10;
				}
				else if(cardTypeCount == 1)
				{
					cardTypeCount = 1;
				}
				else if(cardTypeCount > 1)
				{
					cardTypeCount = (byte)(cardTypeCount-2);
				}
				
				o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("point")
					.transform.GetComponent<CLabelNum>().m_iNum = s_lTableScore[id] * cardTypeCount * i;
				o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("count")
					.transform.GetComponent<CLabelNum>().m_iNum = s_lTableScore[id];
				o_player_chips[id].transform.FindChild("reslut_count").transform.FindChild("niu")
					.transform.GetComponent<CLabelNum>().m_iNum = cardTypeCount;
			}

		}

        //面板显示控制
        IEnumerator ShowConCealLump(GameObject obj)
        {
            ClearAllInfo();
            if (ShowInterface != null)
            {
                ShowInterface.GetComponent<UITweener>().PlayReverse();
                ShowInterface = null;
                conCealTween.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
                obj.GetComponent<UITweener>().enabled = true;
                obj.GetComponent<UITweener>().PlayForward();
                ShowInterface = obj;
                conCealTween.SetActive(true);
            }

             yield return new WaitForSeconds(0.01f);
        }

        //关闭弹出窗口
        public void ConCealTween()
        {
            ClearAllInfo();

            if (ShowInterface != null)
            {
                ShowInterface.GetComponent<UITweener>().PlayReverse();
                ShowInterface = null;
            }
           
            conCealTween.SetActive(false);
        }

		//关闭loading界面
		public void closeLoading()
		{
			if(UIManager.Instance.o_loading != null)
			{
				UIManager.Instance.o_loading.SetActive(false);
			}
		}

        #region ###################声音设置###################

        //设置按钮事件
        public void SettingShow()
        {
            bool bshow = !UISetting.o_set_panel.active;
            UISetting.o_set_panel.SetActive(bshow);
            UISetting.Instance.Show(bshow);
        }

        //关闭设置界面按钮
        public void OnSettingCloseIvk()
        {
            UISetting.o_set_panel.SetActive(false);
        }

        //游戏声音播放
        public void PlayGameSound(SoundType sound)
        {
            float count = UnityEngine.Random.Range(0, 2.0f);
            float fvol = NGUITools.soundVolume;
            NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
        }

        //玩家声音播放
        public void PlayUserSound(GameSoundType sound, byte bGender)
        {
            float fvol = NGUITools.soundVolume;

            if (bGender == (byte)UserGender.Woman)
            {
                NGUITools.PlaySound(_WomanSound[(int)sound], fvol, 1);
            }
            else
            {
                NGUITools.PlaySound(_ManSound[(int)sound], fvol, 1);
            }
        }
        #endregion

        #endregion
    }

}
