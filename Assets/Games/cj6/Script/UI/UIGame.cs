using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.BJL
{    
	#region ##################结构定义#######################

	public enum TimerType
	{
		TIMER_NULL  = 0,	 
		TIMER_IDLE  = 1,  //空闲时间
		TIMER_CHIP  = 2,  //下注时间
		TIMER_OPEN  = 3,  //开牌时间
		
	};
	public enum SoundType
	{
		STARTGAME = 0,
		STARTCHIP = 1,
		CHIP1 = 2,
		CHIP2 = 3,
		SENDCARD = 4,
		WIN = 5,
		LOSE = 6,
		COUNTDOWN = 7,
		SUPER6,
		PAIRCARD
	};

	public enum TimerDelay
	{
		NULL = 0,
		QIANG = 5,
		CHIP = 5,
		OPEN = 20,
		READY = 20
	};

	#endregion

	public class UIGame : MonoBehaviour
	{

		public GameObject o_player_money;
		public GameObject o_player_face;
		public GameObject o_player_nick;
		public GameObject o_rules;

		public GameObject[] o_chip_Btn = new GameObject[7];
		public GameObject[] o_chip_Pos = new GameObject[7];

		public GameObject o_apply_btn;
		public GameObject o_cancel_btn;
		public GameObject o_player_inf;
		//游戏记录
		public GameObject o_info_chip;
		public GameObject o_info_recDL;
		public GameObject o_info_recXL;
		public GameObject o_info_recZP;

		public GameObject o_apply_list;
		public GameObject o_msgbox;
		public GameObject o_rec_Ball;
		public GameObject o_pup_btn;
		public GameObject o_close_box;

		GameObject o_chip_buttons = null;
		GameObject o_bank_player = null;
		GameObject o_add_buttons = null;
		GameObject o_time_label = null;
		GameObject o_clock = null;
	
		//显示筹码
		public GameObject[] o_chip_container;
		public GameObject o_win_area;
		//开牌
		public GameObject o_openCard;
		GameObject o_openCard_Bg = null;
		GameObject o_card_X = null;
		GameObject o_card_Z = null;
		GameObject o_openDes_X = null;
		GameObject o_openDes_Z = null;
		GameObject o_openDes_lbl= null;
		GameObject o_ScoreX_lbl= null;
		GameObject o_ScoreZ_lbl= null;
		GameObject o_outlight_x= null;
		GameObject o_outlight_z= null;

		//房间名
		GameObject o_RoomName = null;

		//玩家各区域下注筹码数量
		GameObject[] o_playr_chips = new GameObject[GameLogic.MAX_AREA_NUM];	
		//每个区域的总分	
		long[]	m_lAreaInAllScore = new long[GameLogic.MAX_AREA_NUM];
		GameObject[] o_chips_count = new GameObject[GameLogic.MAX_AREA_NUM];
		//各区域可下注分数
		long[] m_lAreaLeftScore = new long[GameLogic.MAX_AREA_NUM];	
		public GameObject[] o_winArea = new GameObject[GameLogic.MAX_AREA_NUM];

		//各区域下注数目
		static long[] _lTableScore = new long[GameLogic.MAX_AREA_NUM];
		//结果统计
		static int[] _resultCount = new int[GameLogic.MAX_AREA_NUM];
		//通用数据
		static bool _bStart = false;
		static TimerType _bTimerType = TimerType.TIMER_NULL;
		static int _nInfoTickCount = 0;
		static int _lGameCount = 0;                        	//玩家游戏局数
		static int _lPairCount = 0;                        	//对子局数
//		static bool _bReqChip = true;						//下注要求
		int[]	m_lAllChips = new int[GameLogic.CHIP_ALL];	//筹码种类
		int[]	m_lGameChips = new int[7];					//显示筹码
		long[]	m_lChipScore = new long[7];					//筹码金额
		byte[]	m_cbCardCount = new byte[2];				//扑克数目
		byte[,]	m_cbTableCardArray = new byte[2,3];			//桌面扑克
		byte[]	m_cbSendCount = new byte[2];				//扑克数目
		byte[]	m_recordDate = new byte[GameLogic.MAX_SCORE_HISTORY];  //游戏记录
		byte[]	m_pairDate = new byte[GameLogic.MAX_SCORE_HISTORY];  	//对子记录

		public AudioClip[] _GameSound = new AudioClip[10];  //音效文件
		//游戏变量
		long            lMinTableScore;						//坐下最低金额
		long			m_lMaxChipBanker;					//最大下注 (庄家)
		byte		    m_cbTimeLeave;						//剩余时间
		int				m_nChipTime;						//下注次数 (本局)
		int				m_nChipTimeCount;					//已下次数 (本局)
		long            m_nListUserCount;					//列表人数
//		long            m_AreaLimit;						//区域限制
		long[] m_AreaLimit = new long[GameLogic.MAX_AREA_NUM];

		bool            _canChip;							//是否可下注
		int             _updateTag;						

		static 	byte  _bBankerUser = GameLogic.NULL_CHAIR;
		static  int   _nQuitDelay = 0;
		static  bool  _bReqQuit = false;	

		public List<Vector3> m_lTouchPoint = new List<Vector3>();
		Vector3 vecClick = new Vector3 ( 0, 0, 0 );					//筹码位置
		long  	nListUserCount;										//列表人数
		List<PlayerInfo> bankerList = new List<PlayerInfo>();		//申请庄家列表										
		List<GameObject> recDL_List = new List<GameObject>();		//大路图记录列表
		List<GameObject> recZP_List = new List<GameObject>();		//珠盘图记录列表
		List<GameObject> recXL_List = new List<GameObject>();		//输赢图记录列表

		int 						m_lChipNum;
		Int64						m_lCurrentJetton;					//当前筹码
		Int64						m_lLastJetton;							//上一局筹码
		bool						m_bShowChangeBanker;				//轮换庄家
		bool						m_bNeedSetGameRecord;				//完成设置
		long                        m_ApplyCondition;					//上庄条件
		//庄家信息
		ushort						m_wCurrentBanker;					//当前庄家
		ushort                      m_wBankerUser;						//实际庄家
		ushort						m_wBankerTime;						//做庄次数
		long						m_lBankerScore;						//庄家积分（金币数）
		long						m_lBankerWinScore;					//庄家总成绩	
		long						m_lTmpBankerWinScore;				//庄家本轮成绩	
		bool						m_bEnableSysBanker;					//是否系统做庄
		//玩家成绩
		long						m_lMeCurGameScore;					//我的成绩
		long						m_lMeCurWinScore;					//本轮成绩
		long						m_lMeCurGameAllScore;				//我的积分
		long						m_lMeGameScoreCount;				//玩家总成绩
		long						m_lGameRevenue;						//游戏税收
		long						m_lMeStatisticScore;				//游戏成绩

		string systemName  = "";		//系统昵称

		#region ##################初 始 化#######################
		public void Init()
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

		void Awake()
		{
			try
			{
				//游戏
				o_add_buttons = GameObject.Find("scene_game/dlg_player/ctr_money/add_btn"); //加钱按钮
				o_clock = GameObject.Find("scene_game/dlg_clock");
				o_time_label = GameObject.Find("scene_game/dlg_timelabel");
				o_bank_player = GameObject.Find("scene_game/dlg_player_bank");
				o_chip_buttons = GameObject.Find("scene_game/dlg_add_btns");

				o_playr_chips[0] = GameObject.Find("scene_game/dlg_player/lbl_chips_dx");
				o_playr_chips[1] = GameObject.Find("scene_game/dlg_player/lbl_chips_he");
				o_playr_chips[2] = GameObject.Find("scene_game/dlg_player/lbl_chips_tz");
				o_playr_chips[6] = GameObject.Find("scene_game/dlg_player/lbl_chips_dd");
				o_playr_chips[7] = GameObject.Find("scene_game/dlg_player/lbl_chips_td");
				o_playr_chips[5] = GameObject.Find("scene_game/dlg_player/lbl_chips_cj6");

				o_chips_count[0] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_dx");
				o_chips_count[1] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_he");
				o_chips_count[2] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_tz");
				o_chips_count[6] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_dd");
				o_chips_count[7] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_td");
				o_chips_count[5] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_cj6");

				o_RoomName = GameObject.Find("scene_game/Top_bar/gameroom/gamelabel");
			}
			catch(Exception ex)
			{
			}
		}

		void Start()
		{
            try
            {
                InitGameView();
            }
            catch (Exception ex)
            {
            }
		}

		void Update()
		{

		}

		void FixedUpdate()
		{
			if ((Environment.TickCount - _nInfoTickCount) > 5000)
			{
				ClearAllInfo();
				_nInfoTickCount = 0;
			}
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
			{
				OnBtnBackIvk();			
			}

			if ((System.Environment.TickCount - _nQuitDelay) > 1000 && _bReqQuit == true)
			{
				_bReqQuit = false;
				_nQuitDelay = System.Environment.TickCount;
				UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);			
			}
		}

		void InitGameView()
		{
			o_openCard_Bg = o_openCard.transform.FindChild("BG2").gameObject;
			o_card_X = o_openCard.transform.FindChild("Card_x").gameObject;
			o_card_Z = o_openCard.transform.FindChild("Card_z").gameObject;
			o_openDes_X = o_openCard.transform.FindChild("desc_xian").gameObject;
			o_openDes_Z = o_openCard.transform.FindChild("desc_zhuang").gameObject;
			o_openDes_lbl = o_openCard.transform.FindChild("desc_compcard").gameObject;
			o_ScoreX_lbl = o_openCard.transform.FindChild("lbl_score_x").gameObject;
			o_ScoreZ_lbl = o_openCard.transform.FindChild("lbl_score_z").gameObject;
			o_outlight_x = o_openCard.transform.FindChild("BG2").FindChild("outlight_x").gameObject;
			o_outlight_z = o_openCard.transform.FindChild("BG2").FindChild("outlight_z").gameObject;

			_bStart = false;
			_canChip = false;
			m_ApplyCondition = 1000000;
			m_lChipNum = 1;
			for( int i = 0; i< GameLogic.MAX_AREA_NUM; i++)
			{
				m_AreaLimit[i] = 1000000000;
			};

			m_bEnableSysBanker = true;

			_nInfoTickCount = Environment.TickCount;
			
			o_clock.SetActive (false);
			o_openCard_Bg.SetActive(false);
			o_openDes_X.SetActive(false);
			o_openDes_Z.SetActive(false);
			o_openDes_lbl.GetComponent<UILabel>().text = "";
			o_ScoreX_lbl.GetComponent<UILabel>().text = "";
			o_ScoreZ_lbl.GetComponent<UILabel>().text = "";
			o_openCard.SetActive (false);
			o_time_label.GetComponent<UISprite>().spriteName = "blank";
			o_chip_buttons.SetActive (false);
			
			//庄家信息
			m_lBankerScore = 0; 	 	//庄家积分
			m_wCurrentBanker = 255;	    //庄家位置
			m_wBankerUser = 255;
			m_lLastJetton = 0;
			m_lMeCurGameScore = 0; 		
			m_lMeGameScoreCount = 0;
			m_lMeCurWinScore = 0;
			bankerList.Clear();

			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++ )
			{
				if(o_winArea[i] != null) o_winArea[i].SetActive(false);
			}
			o_win_area .SetActive(false);
			for(byte i = 0; i < 8; i++)
			{
				countLeftScore(i);
			}

			GameObject scrollView1 = o_info_recDL.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollView2 = o_info_recZP.transform.FindChild("content").FindChild("scrollView").gameObject;
			Vector3 point_ee1 = scrollView1.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			Vector3 point_ee2 = scrollView2.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			scrollView1.transform.FindChild("grid").FindChild("ball_e").transform.localPosition = point_ee1;
			scrollView2.transform.FindChild("grid").FindChild("ball_e").transform.localPosition = point_ee2;
		}

		#endregion

		#region 刷新

		void ResetGameView()
		{
			_nInfoTickCount = Environment.TickCount;

		}

		void UpdateUserView()
		{
			try
			{
				if (_bStart == false) return;
				
				m_wBankerUser = m_wCurrentBanker;				
				PlayerInfo userdata = GameEngine.Instance.MySelf;
				o_player_nick.GetComponent<UILabel>().text = userdata.NickName;
				o_player_money.GetComponent<UILabel>().text = (userdata.Money - GetTableScore()).ToString("N0");
				
//				if(userdata.Money > 9999)
//				{
//					o_player_money.GetComponent<UILabel>().text  = formatMoney(userdata.Money - GetTableScore());
//				}
				o_player_face.GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
				
				if(GetSelfChair() == m_wCurrentBanker)
				{
					o_apply_btn.SetActive(false);
					o_cancel_btn.SetActive(true);
					o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace((int)userdata.HeadID , (int)userdata.VipLevel);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = formatMoney(userdata.Money);
					//自动下庄
					if(/* userdata.Money < 1000000 || */m_wBankerTime > 4)
					{
						if( GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_FREE)
						{
							OnCancelForBanker(); 
//							cnMsgBox("玩家下庄！");
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
						o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = formatMoney(bankerdata.Money);
					}else
					{
						o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace(0, 0);
						o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = systemName;					
					}
				}
				o_info_chip.transform.FindChild("label_score").gameObject.GetComponent<UILabel>().text = m_lMeGameScoreCount.ToString("N0");

			
				//游戏各区域可下注信息
				for(byte i = 0; i < 7; i++){
					countLeftScore(i);
				}

				updateApplyList();
				updateRecord();

			}
			catch(Exception ex)
			{
//				cnMsgBox("UpdateUserView:"+ex.Message);
			}
		}
		
		string formatMoney(long money)
		{
			string tempMoney = money.ToString("N0");
			if(money>9999999) 
			{
				tempMoney = ((money / 10000)+((money % 10000)/1000)*0.1).ToString()+"万";
			}
			return tempMoney;
		}

		public string transMoney( Int64 money )
		{
			string tempMoney, str;

			long absMoney = (long)Mathf.Abs((float)money);
			tempMoney = str = absMoney.ToString ();

			for (int i = 0; i < (str.Length-1)/3; i++) {
				tempMoney = tempMoney.Insert((tempMoney.Length-3*(i+1)-i),",");
			}
			if(money >= 0)
			{
				tempMoney = " + " + tempMoney;
			}else
			{
				tempMoney = " - " + tempMoney;
			}
			return tempMoney;
		}

		string formatMoneyW(long money)
		{
			string tempMoney = money.ToString();			
			if(money>9999) 
			{
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}

		public void endLoading()
		{
			if(UIManager.Instance.o_loading!=null)
			{
				UIManager.Instance.o_loading.SetActive(false);
			}
		}

		//更新走势图
		public void updateRecord()
		{
			if(o_info_recDL.activeSelf)
			{
				updateRecDL();

			}else if(o_info_recZP.activeSelf)
			{
				updateRecZP();

			}else if(o_info_recXL.activeSelf)
			{
				updateRecXL();
			}
		}

		void updateRecDL()
		{
			for(var i = 0; i < recDL_List.Count; i++)
			{
				Destroy(recDL_List[i]);//销毁房间按钮
			}
			recDL_List.Clear();//清空房间按钮容器
			GameObject scrollView = o_info_recDL.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollBar = o_info_recDL.transform.FindChild("content").FindChild("scrollBar").gameObject;
			GameObject scrollBg = scrollView.transform.parent.FindChild("scrollBg").gameObject;
			int widthInterval, heightInterval, cellColumn, cellRow;
			int cRowCount = (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.y / 20;
			float cellHeight = 20f;
			float cellWidth = 21f;
			widthInterval= heightInterval = 1;
			cellColumn = cellRow = 0;
			byte tempdata,count;
			tempdata = count = 0;
				
			Vector3 point_e = scrollView.transform.FindChild("grid").FindChild("ball_e").transform.localPosition;
			Vector3 point_ee = scrollView.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			point_e.y = point_ee.y;
//			if(_lGameCount<17)
//			{
				point_e.x = point_ee.x;
//			}
			for(int i = 0; i < _lGameCount % GameLogic.MAX_SCORE_HISTORY+1; i++)
			{
				if(m_recordDate[i] != 0)
				{
					if((tempdata!=0 && tempdata + m_recordDate[i] != 2 * m_recordDate[i]) || count == cRowCount)
					{
						cellColumn++;
						cellRow = 0;
						count = 0;
					}
					GameObject ball = Instantiate(o_rec_Ball ,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
					ball.GetComponent<UISprite>().spriteName = "word_"+ (int)m_recordDate[i]%10 ;
					ball.transform.parent = scrollView.transform.FindChild("grid");
					ball.transform.localScale = new Vector3(1f,1f,1f);
					ball.GetComponent<UISprite>().depth = 20;
					ball.transform.localPosition = point_e;
					ball.transform.localPosition -= new Vector3(-cellColumn*(cellWidth),cellRow*cellHeight+heightInterval,0);
					recDL_List.Add(ball);
					cellRow++;
					count++;
					tempdata = (byte)(m_recordDate[i]%10);
				}
			}
			if(cellColumn < 6)
			{
				scrollView.GetComponent<UIScrollView>().enabled = false;
			}else
			{
				scrollView.GetComponent<UIScrollView>().enabled = true;
			}
		}

		void updateRecZP()
		{
			for(var i = 0; i < recZP_List.Count; i++)
			{
				Destroy(recZP_List[i]);//销毁房间按钮
			}
			recZP_List.Clear();//清空房间按钮容器

			GameObject scrollView = o_info_recZP.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollBar = o_info_recZP.transform.FindChild("content").FindChild("scrollBar").gameObject;
			GameObject scrollBg = scrollView.transform.parent.FindChild("scrollBg").gameObject;
			int viewWidth, viewHeight, widthInterval, heightInterval, cellColumn, cellRow;
			int cRowCount = (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.y /21;
			int cColumnCount =  (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.x / (21+1);
			float cellHeight = 20f;
			float cellWidth = 21f;
			widthInterval= heightInterval = 1;
			cellColumn = cellRow = 0;

			Vector3 point_e = scrollView.transform.FindChild("grid").FindChild("ball_e").transform.localPosition;
			Vector3 point_ee = scrollView.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			point_e.y = point_ee.y;
//			if(_lGameCount<17)
//			{
			point_e.x = point_ee.x;
//			}

			for(int i = 0; i < _lGameCount % GameLogic.MAX_SCORE_HISTORY + 1; i++)
			{
				if(m_recordDate[i] != 0)
				{
					GameObject ball = Instantiate(o_rec_Ball ,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
					ball.GetComponent<UISprite>().spriteName = "word_"+ (int)m_recordDate[i]%10 ;
					ball.transform.parent = scrollView.transform.FindChild("grid");
					ball.transform.localScale = new Vector3(1f,1f,1f);
					ball.GetComponent<UISprite>().depth = 20;
					ball.transform.localPosition = point_e;
					ball.transform.localPosition -= new Vector3(-cellColumn*(cellWidth+widthInterval),cellRow*cellHeight+heightInterval,0);
					recZP_List.Add(ball);
					cellRow++;
					if( i != 0 && i%cRowCount == 0)
					{
						cellColumn++;
						cellRow = 0;
					}
				}
			}
			if(cellColumn < 6)
			{
				scrollView.GetComponent<UIScrollView>().enabled = false;
			}else
			{
				scrollView.GetComponent<UIScrollView>().enabled = true;
			}
		}

		void updateRecXL()
		{
			for(var i = 0; i < recXL_List.Count; i++)
			{
				Destroy(recXL_List[i]);
			}
			recXL_List.Clear();
			GameObject scrollView = o_info_recXL.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollBar = o_info_recXL.transform.FindChild("content").FindChild("scrollBar").gameObject;
			GameObject scrollBg = scrollView.transform.parent.FindChild("scrollBg").gameObject;
			int widthInterval, heightInterval, cellColumn, cellRow;
			int cRowCount = (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.y / 20;
			float cellHeight = 20f;
			float cellWidth = 21f;
			widthInterval= heightInterval = 1;
			cellColumn = cellRow = 0;
			byte tempdata,count;
			tempdata = count = 0;
			
			Vector3 point_e = scrollView.transform.FindChild("grid").FindChild("ball_e").transform.localPosition;
			Vector3 point_ee = scrollView.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			point_e.y = point_ee.y;
//			if(_lPairCount < 17)
//			{
			point_e.x = point_ee.x;
//			}
			for(int i = 0; i < _lPairCount ; i++)
			{
				if( i == 0) tempdata = (byte)(m_pairDate[0]);
				if(m_pairDate[i] != 0)
				{
					if( ( tempdata != 0 && tempdata + m_pairDate[i] != 2 * m_pairDate[i]) || count == cRowCount )
					{
						cellColumn++;
						cellRow = 0;
						count = 0;
					}
					GameObject ball = Instantiate(o_rec_Ball ,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
					ball.GetComponent<UISprite>().spriteName = "tag_ball"+ (int)m_pairDate[i] ;
					ball.transform.parent = scrollView.transform.FindChild("grid");
					ball.transform.localScale = new Vector3(1f,1f,1f);
					ball.GetComponent<UISprite>().depth = 20;
					ball.transform.localPosition = point_e;
					ball.transform.localPosition -= new Vector3(-cellColumn*(cellWidth+widthInterval),cellRow*cellHeight+heightInterval,0);
					recXL_List.Add(ball);
					cellRow++;
					count++;
					tempdata = (byte)(m_pairDate[i]);
				}
			}
			if(cellColumn < 6)
			{
				scrollView.GetComponent<UIScrollView>().enabled = false;
			}else
			{
				scrollView.GetComponent<UIScrollView>().enabled = true;
			}

		}

		//更新申请列表
		public void updateApplyList()
		{
			for(int i = 1; i<6; i++)
			{
				o_apply_list.transform.FindChild("sp"+i).FindChild("nickname").gameObject.GetComponent<UILabel>().text = "";
				o_apply_list.transform.FindChild("sp"+i).FindChild("nickmoney").gameObject.GetComponent<UILabel>().text = "";
				o_apply_list.transform.FindChild("sp"+i).FindChild("face").gameObject.GetComponent<UISprite>().spriteName = "blank";
			}

			//申请上庄列表
			int tempPos = 1;
			for(int i = 0; i < bankerList.Count;i++)
			{
				if(bankerList[i]!= null)
				{	
					PlayerInfo player = GameEngine.Instance.EnumTablePlayer(bankerList[i].DeskStation);
					if(player != null && bankerList[i].ID != (uint)m_wCurrentBanker && player.ID == bankerList[i].ID)
					{
						if(tempPos>5) break;
						o_apply_list.transform.FindChild("sp"+tempPos).FindChild("nickname").GetComponent<UILabel>().text = "昵称:" + bankerList[i].NickName;
						o_apply_list.transform.FindChild("sp"+tempPos).FindChild("nickmoney").GetComponent<UILabel>().text = "金币:" + formatMoney( bankerList[i].Money );
						o_apply_list.transform.FindChild("sp"+tempPos).gameObject.GetComponent<UIFace>().ShowFace( (int)bankerList[i].HeadID , (int)bankerList[i].VipLevel);
						tempPos++;
					}else
					{
						bankerList.RemoveAt(i);
					}
				}
			}
		}

		void SetChipScore()
		{
			int tempValue = 0; 
			for(int i = 0; i < 7; i++)
			{
				tempValue = m_lGameChips[i];
				if(tempValue==0) break;
				switch(tempValue)
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

		void SetAddBtn()
		{
			if(m_lChipNum==0) return;
			
			Vector3 point1 = o_chip_Pos[0].transform.localPosition;
			Vector3 point2 = o_chip_Pos[6].transform.localPosition;
			float distance = (point2.x - point1.x)/m_lChipNum;
			
			for(int i = 0; i < m_lChipNum; i++)
			{
				o_chip_Btn[i].transform.FindChild("img").GetComponent<UISprite>().spriteName = "chip_"+formatMoneyW( m_lChipScore[i] );
				o_chip_Btn[i].transform.localPosition = point1 + new Vector3( (i+0.5f)*distance,0,0);
				o_chip_Btn[i].SetActive(true);
			}
			if(m_lChipNum==2)
			{
				o_chip_Btn[0].transform.localPosition = o_chip_Pos[2].transform.localPosition;
				o_chip_Btn[1].transform.localPosition = o_chip_Pos[4].transform.localPosition;
			}
			if(m_lChipNum==3)
			{
				o_chip_Btn[0].transform.localPosition = o_chip_Pos[2].transform.localPosition;
				o_chip_Btn[1].transform.localPosition = o_chip_Pos[3].transform.localPosition;
				o_chip_Btn[2].transform.localPosition = o_chip_Pos[4].transform.localPosition;
			}
		}

//		void SetAddBtn()
//		{
//			for(int i = 0; i < 7; i++)
//			{
//				o_chip_Btn[i].transform.FindChild("img").GetComponent<UISprite>().spriteName = "chip_"+formatMoneyW( m_lChipScore[i] );
//			}
//		}
		#endregion

		#region ##################框架消息#######################

		//框架消息入口
		void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
		{   
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			if (_bReqQuit == true) return;
			switch (subcmd) 
			{
				case SubCmd.SUB_GF_OPTION:
				{
					Logger.UI.LogWarning("游戏配置");
					OnGameOptionResp(packet);
					break;
				}
				case SubCmd.SUB_GF_SCENE:	//101
				{
					Logger.UI.LogWarning("场景信息");
					OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
					break;
				}
				case SubCmd.SUB_GF_MESSAGE:
				{
					Logger.UI.LogWarning("系统消息");
					OnGameMessageResp(packet);
					break;
				}
				case SubCmd.SUB_GF_USER_READY:
				{
					Logger.UI.LogWarning("用户同意");
					break;
				}
				case SubCmd.SUB_GF_USER_CHAT:
				{
					Logger.UI.LogWarning("用户聊天");
					break;
				}
				case SubCmd.SUB_GF_LOOKON_CONTROL:
				{
					Logger.UI.LogWarning("旁观控制");
					break;
				}
			}
		}

		//框架事件入口
		void OnTableUserEvent(TableEvents tevt, uint userid, object data)
		{	
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			if (_bReqQuit == true) return;
			switch (tevt)
			{
				case TableEvents.GAME_START:
				{					
					UpdateUserView();
					break;
				}
				case TableEvents.USER_COME:
				{					
					UpdateUserView();
					break;
				}
				case TableEvents.USER_LEAVE:
				{
					if (userid != GameEngine.Instance.MySelf.ID)
					{
						UpdateUserView();
					}
					break;
				}
				case TableEvents.USER_READY:
				{
					UpdateUserView();
					break;
				}
				case TableEvents.USER_PLAY:
				{					
					UpdateUserView();	
					break;
				}
				case TableEvents.USER_OFFLINE:
				{				
					UpdateUserView();
					break;
				}
                case TableEvents.GAME_ENTER:
                {
                    InitGameView();
					_bStart = true;
                    UpdateUserView();
                    break;
                }
			}
		}


		//游戏设置消息处理函数
		void OnGameOptionResp(NPacket packet)
		{
			try
			{
				packet.BeginRead();				
				GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
			}
			catch (Exception ex)
			{
//				Debug.LogError("GameOptionResp");
			}
		}

		//游戏场景消息处理函数
		void OnGameSceneResp(byte bGameStatus, NPacket packet)
		{		
			switch (bGameStatus)
			{

				case (byte)GameLogic.GS_WK_FREE: 
				{
					//空闲阶段
					SwitchFreeSceneView(packet);
					break;
				}
				case (byte)GameLogic.GS_WK_PLAY:
				{
					//下注阶段
//					Debug.LogWarning("下注");
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAY;
					o_clock.SetActive(true);
					o_time_label.GetComponent<UISprite>().spriteName = "word_xiazhu";
					_bTimerType = TimerType.TIMER_CHIP;

					for(int i = 0;i<GameLogic.MAX_AREA_NUM; i++)
					{
						if(o_chip_container[i] != null)
						{
							o_chip_container[i].GetComponent<UIChipControl>().ClearChips();
						}
					}
					o_chip_buttons.SetActive(true);

					PlaySound(SoundType.STARTCHIP);

					SwitchPlaySceneView(packet);
					break;
				}
				case (byte)GameLogic.GS_WK_END:
				{
					//开牌阶段
//					Debug.LogWarning("结束");
					o_clock.SetActive(true);
					o_time_label.GetComponent<UISprite>().spriteName = "word_kaipai";
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_END;
					_bTimerType = TimerType.TIMER_OPEN;
					SwitchPlaySceneView(packet);
					break;
				}
//				default:  Debug.LogWarning("Other!");
//					break;
			}
		}

		void OnGameMessageResp(NPacket packet)
		{
			packet.BeginRead();
			ushort wType = packet.GetUShort();
			ushort wlen = packet.GetUShort();
			string strMsg = packet.GetString(wlen);	
			
			try
			{
                if (((wType & (ushort)MsgType.MT_INFO) != 0) || ((wType & (ushort)MsgType.MT_GLOBAL) != 0))
                {
                }

                if ((wType & (ushort)MsgType.MT_EJECT) != 0)
                {
                }

                if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
                    (wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
                {

                    Invoke("OnConfirmBackOKIvk", 2.0f);
                    _bStart = false;
                }

                if ((wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
                {
                    Invoke("OnConfirmBackOKIvk", 2.0f);
                    _bStart = false;
                }
			}
			catch (Exception ex)
			{
//				cnMsgBox("OnGameMessageResp" + ex.Message);
			}
		}

		//初始场景处理函数
		void SwitchFreeSceneView(NPacket packet)
		{
//			Debug.LogWarning("空闲");
			try
			{
				endLoading();
				ResetGameView();				
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;
				o_clock.SetActive(true);
				o_time_label.GetComponent<UISprite>().spriteName = "word_idle";
				_bTimerType = TimerType.TIMER_IDLE;
				packet.BeginRead();
				byte timeLeave = packet.GetByte();					//剩余时间
				Int64 lUserGold = packet.GetLong();					//玩家自由金币
				ushort lBanker = packet.GetUShort();				//当前庄家
				Int64 lBankerWinScore = packet.GetLong();   		//庄家成绩
				Int64 lBankerScore = packet.GetLong(); 				//庄家积分
				ushort lBankTimes = packet.GetUShort();				//庄家局数

				bool bEnableSysBanker = packet.GetBool();   		//系统做庄
				Int64 lApplyCondition = packet.GetLong();			//上庄申请条件
//				Int64 lAreaLimit  = packet.GetLong();				//区域限制
				for( int i = 0; i< GameLogic.MAX_AREA_NUM; i++)
				{
					m_AreaLimit[i] = packet.GetLong();
				}

				long lMinScore = packet.GetLong();					//最低游戏金币

				int tempCount = 0;
				for(int i = 0; i<GameLogic.CHIP_ALL; i++)
				{
					m_lAllChips[i] = packet.GetInt();				//下注筹码种类
					if(m_lAllChips[i]==1 && tempCount<7)
					{
						m_lGameChips[tempCount] = i+1;
						tempCount++;
					}
				}
				m_lChipNum = tempCount;

				systemName = packet.GetString(32);					//系统昵称
				string roomName = packet.GetString(32);				//房间名字

//				int tempCount2 = 0;
//				while(tempCount<7)
//				{
//					m_lAllChips[tempCount2++] = 1;
//					tempCount = 0;
//					for(int i = 0; i<GameLogic.CHIP_ALL; i++)
//					{
//						if(tempCount >= 7) break;
//						if(m_lAllChips[i]==1 && tempCount<7)
//						{
//							m_lGameChips[tempCount] = i+1;
//							tempCount++;
//						}
//					}
//				}

				string strName = packet.GetString(32);				//房间名字

				if(o_RoomName != null)
				{
					o_RoomName.GetComponent<UILabel>().text = strName;
				}

				SetChipScore();
				lMinTableScore = lMinScore;
				m_ApplyCondition = lApplyCondition;

				for(int i = 1; i <= 3; i++)
				{
					m_lAreaInAllScore[i] = 0;
					_lTableScore[i] = 0;
				}
				m_bEnableSysBanker = bEnableSysBanker;
				m_wBankerTime = lBankTimes;

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++ )
				{
					if(o_winArea[i] != null) o_winArea[i].SetActive(false);
				}
				o_win_area.SetActive(false);

				SetBankerInfo(lBanker, lBankerScore);
				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_OPEN);
				UpdateUserView();

			}catch(Exception ex)
			{
//				cnMsgBox("FreeSceneView:"+ex.Message);
			}
		}
		

		//游戏场景处理函数
		void SwitchPlaySceneView(NPacket packet)
		{
			try
			{	
				endLoading();
				ResetGameView();
				_bStart = true;
				
				packet.BeginRead();

				byte  timeLeave = packet.GetByte();								//剩余时间
				byte cbGameStatus = packet.GetByte();							//游戏状态

				Int64[]	lAreaScore = new long[GameLogic.MAX_AREA_NUM]; 			//每个区域的总分	
				Int64[]	lPlayerAreaScore = new long[GameLogic.MAX_AREA_NUM]; 	//每个玩家每个区域的总分	
				
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lAreaScore[i] = packet.GetLong();
					m_lAreaInAllScore[i] = lAreaScore[i];
				}
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lPlayerAreaScore[i] = packet.GetLong();
					_lTableScore[i] = lPlayerAreaScore[i];
				}
				Int64 lUserBetScore = packet.GetLong();					    //玩家最大下注
				Int64 lUserScore = packet.GetLong();					    //玩家自由金币
				Int64[]	lPlayScore = new long[GameLogic.MAX_AREA_NUM]; 		//玩家输赢
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lPlayScore[i] = packet.GetLong();
				}

				Int64 lUserALLScore = packet.GetLong();					    //玩家成绩
				ushort lBanker = packet.GetUShort();						//当前庄家
				Int64 lBankerScore = packet.GetLong(); 						//庄家积分(总分)
				Int64 lBankerWinScore = packet.GetLong();   				//庄家成绩
				ushort lBankTimes = packet.GetUShort();						//庄家局数
				bool bEnableSysBanker = packet.GetBool();   				//系统做庄
				Int64 lApplyCondition = packet.GetLong();					//上庄申请条件
				//Int64 lAreaLimit  = packet.GetLong();						//区域限制
				for( int i = 0; i< GameLogic.MAX_AREA_NUM; i++)
				{
					m_AreaLimit[i] = packet.GetLong();
				}

				m_ApplyCondition = lApplyCondition;
//				m_AreaLimit = lAreaLimit;

				m_cbCardCount[0] = packet.GetByte();						//扑克设置
				m_cbCardCount[1] = packet.GetByte();
				m_cbTableCardArray[0,0] =  packet.GetByte();
				m_cbTableCardArray[0,1] =  packet.GetByte();
				m_cbTableCardArray[0,2] =  packet.GetByte();
				m_cbTableCardArray[1,0] =  packet.GetByte();
				m_cbTableCardArray[1,1] =  packet.GetByte();
				m_cbTableCardArray[1,2] =  packet.GetByte();

				long lMinScore = packet.GetLong();							//最低游戏成绩

				int tempCount = 0;
				for(int i = 0; i<GameLogic.CHIP_ALL; i++)
				{
					m_lAllChips[i] = packet.GetInt();						//下注筹码种类
					if(m_lAllChips[i]==1 && tempCount<7)
					{
						m_lGameChips[tempCount] = i+1;
						tempCount++;
					}
				}

				m_lChipNum = tempCount;

				systemName = packet.GetString(32);					//系统昵称
				string roomName = packet.GetString(32);				//房间名字

//				int tempCount2 = 0;
//				while(tempCount<7)
//				{
//					m_lAllChips[tempCount2++] = 1;
//					tempCount = 0;
//					for(int i = 0; i<GameLogic.CHIP_ALL; i++)
//					{
//						if(tempCount >= 7) break;
//						if(m_lAllChips[i]==1 && tempCount<7)
//						{
//							m_lGameChips[tempCount] = i+1;
//							tempCount++;
//						}
//					}
//				}

				string strName = packet.GetString(32);						//房间名字

				if(o_RoomName != null)
				{
					o_RoomName.GetComponent<UILabel>().text = strName;
				}


				SetChipScore();
				lMinTableScore = lMinScore;
//				if(lMinTableScore > 5000000){
//					lMinTableScore = 10000;
//				}

				m_bEnableSysBanker = bEnableSysBanker;
				m_wBankerTime = lBankTimes;

				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_OPEN);

				SetBankerInfo(lBanker, lBankerScore);
				ShowCurChips();
				UpdateButtonContron();
				//===============================================================================
				if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_END && timeLeave < 4)
				{
					o_chip_buttons.SetActive(false);
				}
				
				SetCurGameScore( lUserALLScore, 0,lBankerWinScore, lBankerScore, lBankTimes, 0);
				UpdateUserView();

			}catch(Exception ex)
			{
//				cnMsgBox("OpenSceneView:"+ex.Message);
			}
		}

		#endregion

		#region ##################游戏消息#######################
		
		//游戏消息入口
		void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
		{
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			if (_bReqQuit == true) return;
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
					//游戏开始
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
					//游戏结束
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
					Debug.LogWarning("更新积分");
					
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
					Logger.UI.LogWarning("下注失败");
					packet.BeginRead();
					ushort bChair = packet.GetUShort();
					byte chipArea = packet.GetByte();
					long nChip = packet.GetLong();
					if(bChair==GetSelfChair())
					{
						m_lTouchPoint.RemoveAt(0);
					}

					break;
				}
				case SubCmd.SUB_S_CANCEL_BANKER:
				{
					//取消申请
					OnSubUserCancelBanker(packet);
					break;
				}
				case SubCmd.SUB_S_AMDIN_COMMAND :
				{
					//系统控制
					Debug.LogWarning("系统控制");
					break;
				}

			}
		}

		#endregion

		//========================= 消息处理 ===================================
		//空闲时间
		void OnGameFreeResp(NPacket packet)
		{
			Debug.LogWarning("空闲时间");
			try
			{
				endLoading();
				if(UIManager.Instance.o_loading!=null && UIManager.Instance.o_loading.activeSelf)
				{
					UIManager.Instance.o_loading.SetActive(false);
				}
				UpdateUserView();

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++ )
				{
					if(o_winArea[i] != null) o_winArea[i].SetActive(false);
				}
				o_win_area.SetActive(false);

				o_time_label.GetComponent<UISprite>().spriteName = "word_idle";
				o_clock.SetActive(true);
				_bTimerType = TimerType.TIMER_IDLE;
				o_chip_buttons.SetActive(true);
				_canChip = false;
				o_openCard.SetActive (false);

				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaInAllScore[i] = 0;
					_lTableScore[i] = 0;
				}

				packet.BeginRead();
				byte timeLeave = packet.GetByte();
				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_IDLE);

				UpdateUserView();
				UpdateButtonContron();
			}catch(Exception ex)
			{
//				cnMsgBox("OnGameFreeResp"+ex.Message);
			}
		}

		//玩家下注
		void OnGamePlaceJetton(NPacket packet)
		{
			Debug.LogWarning("玩家下注");
			try
			{
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAY;
				Bounds bounds;
				float x,y,x1,x2,y1,y2,z;
						
				packet.BeginRead();
				ushort bChair = packet.GetUShort();
				byte chipArea = packet.GetByte();
				long nChip = packet.GetLong();		//加注数目
				m_lAreaInAllScore[chipArea] += nChip;

				Vector3 point = new Vector3(0, 0, 0);
				countLeftScore(chipArea);

				if ((byte)bChair == GetSelfChair())
				{
					Vector3 point2 = m_lTouchPoint[0];
					point =	UICamera.currentCamera.ScreenToWorldPoint(point2);
					m_lTouchPoint.RemoveAt(0);

					_lTableScore[chipArea] += nChip;
					UpdateButtonContron();
					PlayerInfo bankerdata = GameEngine.Instance.GetTableUserItem( bChair );
					long tempMoney = bankerdata.Money - GetTableScore();
					o_player_money.GetComponent<UILabel>().text = tempMoney.ToString("N0");
				}
				
				ShowCurChips();
				AppendChips((byte)bChair, nChip, chipArea, point);
				
			}catch(Exception ex)
			{
//				cnMsgBox("OnGamePlaceJetton:"+ex.Message);
			}
		}

		//计算可下注
		void countLeftScore( byte area)
		{
			PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);

			if(bankerdata!=null)
			{
				m_lAreaLeftScore[area] = calculateScore(area);
				switch(area)
				{
				case 0:
					o_info_chip.transform.FindChild("label_xian").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 1:
					o_info_chip.transform.FindChild("label_he").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 2:
					o_info_chip.transform.FindChild("label_zhuang").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 5:
					break;
				}

			}else
			{

				switch(area)
				{
				case 0:
					m_lAreaLeftScore[area] = m_AreaLimit[0] - m_lAreaInAllScore[0];
					o_info_chip.transform.FindChild("label_xian").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 1:
					m_lAreaLeftScore[area] = m_AreaLimit[1] - m_lAreaInAllScore[1];
					o_info_chip.transform.FindChild("label_he").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 2:
					m_lAreaLeftScore[area] = m_AreaLimit[2] - m_lAreaInAllScore[2];
					o_info_chip.transform.FindChild("label_zhuang").GetComponent<UILabel>().text = m_lAreaLeftScore[area].ToString("N0");
					break;
				case 5:
					m_lAreaLeftScore[area] = m_AreaLimit[5] - m_lAreaInAllScore[5];
					break;
				case 6:
					m_lAreaLeftScore[area] = m_AreaLimit[6] - m_lAreaInAllScore[6];
					break;
				case 7:
					m_lAreaLeftScore[area] = m_AreaLimit[7] - m_lAreaInAllScore[7];
					break;
				}

			}
		}

		long calculateScore(byte chipArea)
		{
			long tempScore = 0;
			long tempMoney = 0;
			long tempMoney2 = 0;
			PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);
			if(bankerdata == null) return tempScore;

			tempMoney = bankerdata.Money;

			switch(chipArea)
			{
			case GameLogic.AREA_X:
				tempScore = (long)( tempMoney + m_lAreaInAllScore[5] + m_lAreaInAllScore[1] + m_lAreaInAllScore[2] - (m_lAreaInAllScore[6]*11f + m_lAreaInAllScore[7]*11f )- m_lAreaInAllScore[0]);
				break;
			case GameLogic.AREA_P:
				tempScore = (long)((tempMoney + m_lAreaInAllScore[5] - (m_lAreaInAllScore[6]*11f + m_lAreaInAllScore[7]*11f))/8f - m_lAreaInAllScore[1]);
				break;
			case GameLogic.AREA_Z:
				tempScore = (long)( tempMoney + m_lAreaInAllScore[5]+ m_lAreaInAllScore[1]+ m_lAreaInAllScore[0] - (m_lAreaInAllScore[6]*11f + m_lAreaInAllScore[7]*11f )- m_lAreaInAllScore[2]);
				break;
			case GameLogic.AREA_CJ:
				tempScore = (long)((tempMoney + m_lAreaInAllScore[1]+ m_lAreaInAllScore[0] - m_lAreaInAllScore[2]/2f - (m_lAreaInAllScore[6]*11f + m_lAreaInAllScore[7]*11f)) / 12f - m_lAreaInAllScore[5]);
				break;
			case GameLogic.AREA_DD:
				if(m_lAreaInAllScore[0]> m_lAreaInAllScore[2])
				{
					tempMoney2 = m_lAreaInAllScore[0]- m_lAreaInAllScore[2];
				}else
				{
					tempMoney2 = m_lAreaInAllScore[2]- m_lAreaInAllScore[0];
				}
				tempScore = (long)( (tempMoney + tempMoney2 - 8*m_lAreaInAllScore[1] -  m_lAreaInAllScore[7]*11 - m_lAreaInAllScore[5]*12) / 11f - m_lAreaInAllScore[6]);
				break;
			case GameLogic.AREA_TD:
				if(m_lAreaInAllScore[0]> m_lAreaInAllScore[2])
				{
					tempMoney2 = m_lAreaInAllScore[0]- m_lAreaInAllScore[2];
				}else
				{
					tempMoney2 = m_lAreaInAllScore[2]- m_lAreaInAllScore[0];
				}
				tempScore = (long)( (tempMoney + tempMoney2 - 8*m_lAreaInAllScore[1] -  m_lAreaInAllScore[6]*11 - m_lAreaInAllScore[5]*12) / 11f - m_lAreaInAllScore[7]);

				break;
			}
			return tempScore;
		}


		//下注时间
		void OnGameStartResp(NPacket packet)
		{
			try
			{
				PlaySound(SoundType.STARTCHIP);
				o_time_label.GetComponent<UISprite>().spriteName = "word_xiazhu";
				o_clock.SetActive(true);
				_bTimerType = TimerType.TIMER_CHIP;
				o_chip_buttons.SetActive(true);
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAY;

				UpdateButtonContron();
				if (!_canChip && GameEngine.Instance.EnumTablePlayer(GetSelfChair()).Money >= lMinTableScore)
				{
					if(m_lLastJetton==m_lChipScore[0])
					{
						OnBtnAddIvk_1();
					}else if(m_lLastJetton==m_lChipScore[1])
					{
						OnBtnAddIvk_2();
					}else if(m_lLastJetton==m_lChipScore[2])
					{
						OnBtnAddIvk_3();
					}else if(m_lLastJetton==m_lChipScore[3])
					{
						OnBtnAddIvk_4();
					}else if(m_lLastJetton==m_lChipScore[4])
					{
						OnBtnAddIvk_5();
					}else if(m_lLastJetton==m_lChipScore[5])
					{
						OnBtnAddIvk_6();
					}else if(m_lLastJetton==m_lChipScore[6])
					{
						OnBtnAddIvk_7();
					}else
					{
						OnBtnAddIvk_1();
					}
				}

				for(int i = 0;i<GameLogic.MAX_AREA_NUM; i++)
				{
					if(o_chip_container[i] != null){
						o_chip_container[i].GetComponent<UIChipControl>().ClearChips();
					}
				}

				packet.BeginRead();
				byte timeLeave = packet.GetByte();
				SetBankerInfo(packet.GetUShort(),packet.GetLong());

				Int64 lPlayBetScore = packet.GetLong();						//玩家最大下注	
				long  mineMoney = packet.GetLong();							//玩家自由金币
				int   playerNum = packet.GetInt();							//人数上限（下注机器人）
				Int64 nListUserCount = packet.GetLong();					//列表人数
				int	  nAndriodCount = packet.GetInt();						//机器人列表人数

				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_CHIP);


			}catch(Exception ex)
			{
//				cnMsgBox("OnGameStartResp"+ ex.Message);
			}
		}

		//游戏结束
		void OnGameEndResp(NPacket packet)
		{		
//			Debug.LogWarning ("开牌时间");
			try
			{	
				if (_bStart == false) return;
				o_clock.SetActive(true);
				_canChip = false;
				o_time_label.GetComponent<UISprite>().spriteName = "word_kaipai";
				_bTimerType = TimerType.TIMER_OPEN;
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_END;

				packet.BeginRead();
				byte timeLeave = packet.GetByte();
				m_cbCardCount[0] = packet.GetByte();
				m_cbCardCount[1] = packet.GetByte();
				m_cbTableCardArray[0,0] =  packet.GetByte();
				m_cbTableCardArray[0,1] =  packet.GetByte();
				m_cbTableCardArray[0,2] =  packet.GetByte();
				m_cbTableCardArray[1,0] =  packet.GetByte();
				m_cbTableCardArray[1,1] =  packet.GetByte();
				m_cbTableCardArray[1,2] =  packet.GetByte();

				long lBankerScore = packet.GetLong();		//庄家本轮成绩
				long lBankerTotallScore = packet.GetLong();
				int nBankerTime = packet.GetInt();			//坐庄次数

				Int64[]	lPlayScore = new long[GameLogic.MAX_AREA_NUM]; 		//玩家输赢
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lPlayScore[i] = packet.GetLong();
				}

				long lUserAllScore = packet.GetLong();   //玩家成绩
				long lRevenue = packet.GetLong(); 

				//===============================================================================
				//游戏记录
				_lGameCount++;
				if( _lGameCount%GameLogic.MAX_SCORE_HISTORY == 0 )
				{
					for(int i = 0; i< GameLogic.MAX_AREA_NUM; i++)
					{
						_resultCount[i] = 0;
					}
				}

				//===============================================================================

				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_OPEN);

				SetCurGameScore( lUserAllScore, lUserAllScore, lBankerScore, lBankerTotallScore, nBankerTime,lRevenue);
				StartCoroutine(showEffectView(lUserAllScore));
//				showEffectView(lUserAllScore, m_cbCardCount, m_cbTableCardArray);
				Invoke("ShowResultView", 5);

			}catch(Exception ex)
			{
//				cnMsgBox("OnGameEndResp:"+ex.Message);
			}
		}
		
		//开牌效果
		IEnumerator showEffectView(long lUserScore)
		{
			yield return new WaitForSeconds(2.0f);
			o_openCard.SetActive(true);
			SetCardInfo(m_cbCardCount,m_cbTableCardArray);

			if(o_openCard.activeSelf)
			{
				o_openCard_Bg.transform.FindChild("img_word").gameObject.SetActive(false);
				o_openCard_Bg.transform.FindChild("img_cj6").gameObject.SetActive(false);
				o_openCard_Bg.transform.FindChild("img_he").gameObject.SetActive(false);
				o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(false);
				o_ScoreX_lbl.SetActive(false);
				o_ScoreZ_lbl.SetActive(false);
				o_outlight_x.SetActive(false);
				o_outlight_z.SetActive(false);

				byte[] card_x = new byte[3];
				byte[] card_z = new byte[3];
				for(int i = 0; i < m_cbCardCount[0]; i++)
				{
					card_x[i] = m_cbTableCardArray[0,i];
					o_card_X.transform.FindChild("card_"+i).gameObject.SetActive(false);
				}
				for(int i = 0; i < m_cbCardCount[1]; i++)
				{
					card_z[i] = m_cbTableCardArray[1,i];
					o_card_Z.transform.FindChild("card_"+i).gameObject.SetActive(false);
				}
				GetPairRecord( card_x, card_z );

				byte point1 = GetCardPoint(card_x, m_cbCardCount[0]);
				byte point2 = GetCardPoint(card_z, m_cbCardCount[1]);

				int _result = 0;

				if( point1 > point2 )
				{
					_resultCount[1]++;
					_result = 1;

					o_openCard_Bg.transform.FindChild("img_word").gameObject.SetActive(true);
					if(lUserScore < 0)
					{
						o_openCard_Bg.transform.FindChild("img_word").GetComponent<UISprite>().spriteName = "js_xian_grey";
						o_openCard_Bg.transform.FindChild("img_word").FindChild("img_bg").GetComponent<UISprite>().spriteName = "js_word_grey";
					}else
					{
						o_openCard_Bg.transform.FindChild("img_word").GetComponent<UISprite>().spriteName = "js_xian";
						o_openCard_Bg.transform.FindChild("img_word").FindChild("img_bg").GetComponent<UISprite>().spriteName = "js_word_bg";
					}
					o_ScoreX_lbl.GetComponent<UILabel>().text = transMoney(lUserScore);
					o_winArea[GameLogic.AREA_X].SetActive(true);
					if(lUserScore < 0)
					{
						o_ScoreX_lbl.GetComponent<UILabel>().color = Color.red;
					}else
					{
						o_ScoreX_lbl.GetComponent<UILabel>().color = new Color(1.0f,240.0f/250,130.0f/250);
					}
				}else if( point1 < point2 )
				{
					_resultCount[3]++;
					_result = 3;
					o_winArea[GameLogic.AREA_Z].SetActive(true);
					if(point2 == 6)
					{
						_resultCount[GameLogic.AREA_Z]++;
						o_openCard_Bg.transform.FindChild("img_cj6").gameObject.SetActive(true);
						if(lUserScore < 0)
						{
							o_openCard_Bg.transform.FindChild("img_cj6").GetComponent<UISprite>().spriteName = "cj6_grey";
						}else
						{
							o_openCard_Bg.transform.FindChild("img_cj6").GetComponent<UISprite>().spriteName = "cj6_gold";
						}
						o_winArea[GameLogic.AREA_CJ].SetActive(true);
					}
					else
					{
						o_openCard_Bg.transform.FindChild("img_word").gameObject.SetActive(true);
						if(lUserScore < 0)
						{
							o_openCard_Bg.transform.FindChild("img_word").GetComponent<UISprite>().spriteName = "js_zhuang_grey";
							o_openCard_Bg.transform.FindChild("img_word").FindChild("img_bg").GetComponent<UISprite>().spriteName = "js_word_grey";
						}else
						{
							o_openCard_Bg.transform.FindChild("img_word").GetComponent<UISprite>().spriteName = "js_zhuang";
							o_openCard_Bg.transform.FindChild("img_word").FindChild("img_bg").GetComponent<UISprite>().spriteName = "js_word_bg";
						}
					}

					o_ScoreZ_lbl.GetComponent<UILabel>().text = transMoney(lUserScore);	
					if(lUserScore < 0)
					{
						o_ScoreZ_lbl.GetComponent<UILabel>().color = Color.red;
					}else
					{
						o_ScoreZ_lbl.GetComponent<UILabel>().color = new Color(1.0f,240.0f/250,130.0f/250);
					}
				}else
				{
					_resultCount[2]++;
					_result = 2;
					o_openCard_Bg.transform.FindChild("img_he").gameObject.SetActive(true);
					o_winArea[GameLogic.AREA_P].SetActive(true);
					o_ScoreX_lbl.GetComponent<UILabel>().text = transMoney(lUserScore);
					if(lUserScore < 0)
					{
						o_ScoreX_lbl.GetComponent<UILabel>().color = Color.red;
						o_openCard_Bg.transform.FindChild("img_he").GetComponent<UISprite>().spriteName = "js_he_grey";
					}else
					{
						o_ScoreX_lbl.GetComponent<UILabel>().color = new Color(1.0f,240.0f/250,130.0f/250);
						o_openCard_Bg.transform.FindChild("img_he").GetComponent<UISprite>().spriteName = "js_he";
					}
				}

				o_openDes_X.transform.FindChild("sp_number").GetComponent<UISprite>().spriteName = "n" + point1;
				o_openDes_Z.transform.FindChild("sp_number").GetComponent<UISprite>().spriteName = "n" + point2;

				GetRecord(_result);
				StartCoroutine(ShowOpenEffectView(lUserScore, card_x, card_z));
			}
		}

		IEnumerator ShowOpenEffectView(long lUserScore, byte[] cardX, byte[] cardZ)
		{
			GameObject[] card_x = new GameObject[3];
			GameObject[] card_z = new GameObject[3];
			Vector3 _oldPos, _newPos1, _newPos2;
			for(int i = 0; i < m_cbCardCount[0]; i++)
			{
				card_x[i] = o_card_X.transform.FindChild("card_"+i).gameObject;
			}
			for(int i = 0; i < m_cbCardCount[1]; i++)
			{
				card_z[i] = o_card_Z.transform.FindChild("card_"+i).gameObject;
			}

			yield return new WaitForSeconds (0.4f);
			int tmpCnt = 0;
			do
			{
				yield return new WaitForSeconds (0.1f);
				_oldPos = card_x[tmpCnt].transform.localPosition;
				_newPos1 = new Vector3(_oldPos.x +60, _oldPos.y + 1200, 0);
				_newPos2 = new Vector3(_oldPos.x +60, _oldPos.y, 0);
				card_x[tmpCnt].transform.localPosition = _newPos1;
				card_x[tmpCnt].SetActive(true);
				TweenPosition.Begin(card_x[tmpCnt], 0.5f, _newPos2);
				PlaySound(SoundType.SENDCARD);
				yield return new WaitForSeconds (0.5f);
				TweenPosition.Begin(card_x[tmpCnt], 0.1f, _oldPos);

				yield return new WaitForSeconds (0.1f);
				_oldPos = card_z[tmpCnt].transform.localPosition;
				_newPos1 = new Vector3(_oldPos.x +60, _oldPos.y + 1200, 0);
				_newPos2 = new Vector3(_oldPos.x +60, _oldPos.y, 0);
				card_z[tmpCnt].transform.localPosition = _newPos1;
				card_z[tmpCnt].SetActive(true);
				TweenPosition.Begin(card_z[tmpCnt], 0.5f, _newPos2);
				PlaySound(SoundType.SENDCARD);
				yield return new WaitForSeconds (0.5f);
				TweenPosition.Begin(card_z[tmpCnt], 0.1f, _oldPos);

				tmpCnt++;

			}while( tmpCnt<2 );

			byte point1 = GetCardPoint(cardX, 2 );
			byte point2 = GetCardPoint(cardZ, 2 );
			byte pointX = GetCardPoint(cardX, m_cbCardCount[0]);
			byte pointZ = GetCardPoint(cardZ, m_cbCardCount[1]);
			//card_x_3
			if(m_cbCardCount[0] > 2)
			{
				yield return new WaitForSeconds (0.3f);
				o_openDes_lbl.GetComponent<UILabel>().text = "闲"+point1+"点,庄"+point2+"点,闲继续拿牌";
				yield return new WaitForSeconds (0.7f);
				_oldPos = card_x[2].transform.localPosition;
				_newPos1 = new Vector3(_oldPos.x +50, _oldPos.y + 1200, 0);
				_newPos2 = new Vector3(_oldPos.x +50, _oldPos.y, 0);
				card_x[2].transform.localPosition = _newPos1;
				card_x[2].SetActive(true);
				TweenPosition.Begin(card_x[2], 0.8f, _newPos2);
				PlaySound(SoundType.SENDCARD);
				yield return new WaitForSeconds (0.8f);
				TweenPosition.Begin(card_x[2], 0.1f, _oldPos);
			}

			if(m_cbCardCount[1] > 2)
			{	
				o_openDes_lbl.GetComponent<UILabel>().text = "";
				yield return new WaitForSeconds (0.3f);
				o_openDes_lbl.GetComponent<UILabel>().text = "闲"+point1+"点,庄"+point2+"点,庄继续拿牌";
				yield return new WaitForSeconds (0.7f);
				_oldPos = card_z[2].transform.localPosition;
				_newPos1 = new Vector3(_oldPos.x +50, _oldPos.y + 1200, 0);
				_newPos2 = new Vector3(_oldPos.x +50, _oldPos.y, 0);
				card_z[2].transform.localPosition = _newPos1;
				card_z[2].SetActive(true);
				TweenPosition.Begin(card_z[2], 0.8f, _newPos2);
				PlaySound(SoundType.SENDCARD);
				yield return new WaitForSeconds (0.8f);
				TweenPosition.Begin(card_z[2], 0.1f, _oldPos);
			}

			yield return new WaitForSeconds (1.0f);
			o_openDes_X.SetActive(true);
			o_openDes_Z.SetActive(true);
			o_openCard_Bg.transform.localScale = new Vector3(0,0,0);
			TweenScale.Begin(o_openCard_Bg,0.5f,new Vector3(1f,1f,1f));

			yield return new WaitForSeconds (1.0f);
			o_openCard_Bg.SetActive(true);
			o_openCard_Bg.transform.localScale = new Vector3(0,0,0);
			TweenScale.Begin(o_openCard_Bg,0.5f,new Vector3(1f,1f,1f));
			yield return new WaitForSeconds (0.5f);

			if(lUserScore<0)
			{
				o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(false);
				PlaySound(SoundType.LOSE);
			}else
			{
				o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(true);
				PlaySound(SoundType.WIN);
			}
			o_win_area.SetActive(true);
			o_ScoreX_lbl.SetActive(true);
			o_ScoreZ_lbl.SetActive(true);

			if(pointX > pointZ)
			{
				o_outlight_x.SetActive(true);
				o_openDes_lbl.GetComponent<UILabel>().text = "闲"+pointX+"点,庄"+pointZ+"点,闲赢！";
			}else if(pointX < pointZ)
			{
				o_outlight_z.SetActive(true);
				o_openDes_lbl.GetComponent<UILabel>().text = "闲"+pointX+"点,庄"+pointZ+"点,庄赢！";
			}else
			{
				o_outlight_x.SetActive(true);
				o_outlight_z.SetActive(true);
				o_openDes_lbl.GetComponent<UILabel>().text = "闲"+pointX+"点,庄"+pointZ+"点,和赢！";
			}
			yield return new WaitForSeconds (0.5f);
			updateRecord();
			o_win_area.SetActive(true);
		}

		long GetTableScore()
		{
			long tempScore = 0;
			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				tempScore  += _lTableScore[i];
			}
			return tempScore;
		}

		long GetAllScore()
		{
			long tempScore = 0;
			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				tempScore  += m_lAreaInAllScore[i];
			}
			return tempScore;
		}
		
		void GetRecord( int _result)
		{
			if(_lGameCount % GameLogic.MAX_SCORE_HISTORY != 0)
			{
				m_recordDate[_lGameCount % GameLogic.MAX_SCORE_HISTORY] = (byte)_result;
				
			}else
			{
				for(int i = 0; i < GameLogic.MAX_SCORE_HISTORY; i++)
				{
					m_recordDate[i] = 0;
				}
				o_info_recDL.transform.FindChild("content").FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();
				o_info_recZP.transform.FindChild("content").FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();
				o_info_recXL.transform.FindChild("content").FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();
				
				for(var j = 0; j < recDL_List.Count; j++){
					Destroy(recDL_List[j]);//销毁房间按钮
				}
				for(var j = 0; j < recZP_List.Count; j++){
					Destroy(recZP_List[j]);//销毁房间按钮
				}
				recDL_List.Clear();
				recZP_List.Clear();
				m_recordDate[0] = (byte)_result;
			}
		}
		void GetPairRecord( byte[] card_x, byte[] card_z )
		{
			byte tempValueX = 0;
			byte tempValueZ = 0;
			byte pairX = 0;
			byte pairZ = 0;

			if(_lPairCount == GameLogic.MAX_SCORE_HISTORY)
			{
				_lPairCount = 0;
	
				for(var j = 0; j < recXL_List.Count; j++){
					Destroy(recXL_List[j]);
				}
				recXL_List.Clear();
				for(int i = 0; i<GameLogic.MAX_SCORE_HISTORY; i++)
				{
					m_pairDate[i] = 0;
				}
			}

			if( GetCardValue(card_x[0])== GetCardValue(card_x[1]))
			{
				pairX++;
				o_winArea[GameLogic.AREA_DD].SetActive(true);
			}
			if( GetCardValue(card_z[0])== GetCardValue(card_z[1]))
			{
				pairZ++;
				o_winArea[GameLogic.AREA_TD].SetActive(true);
			}

			if( pairX>0 && pairZ>0)
			{
				_resultCount[6]++;
				_resultCount[7]++;
				m_pairDate[_lPairCount] = 2;
				_lPairCount++;
			}else if( pairX>0 )
			{
				_resultCount[6]++;
				m_pairDate[_lPairCount] = 1;
				_lPairCount++;
			}else if( pairZ>0 )
			{
				_resultCount[7]++;
				m_pairDate[_lPairCount] = 3;
				_lPairCount++;
			}
		}

		void OnSubUserApplyBanker(NPacket packet)
		{
//			Debug.LogWarning ("申请上庄");
			try
			{
				packet.BeginRead();
				ushort ApplyId = packet.GetUShort();
				PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer(ApplyId);
				if (bankerdata != null)
				{
					bankerList.Add(bankerdata);
				}
				if( ApplyId == GetSelfChair() )
				{
					o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(false);
					o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(true);
				}

				updateApplyList();
			}catch(Exception ex){
//				cnMsgBox("ApplyBanker"+ex.Message);
			}
		}

		void OnSubUserChangeBanker(NPacket packet)
		{
//			Debug.LogWarning ("切换庄家");
			try
			{
				packet.BeginRead();
				ushort wbanker = packet.GetUShort();
				Int64  wbankerScore = packet.GetLong();
				PlayerInfo bankerdata = GameEngine.Instance.GetTableUserItem(wbanker);
				SetBankerInfo(wbanker,wbankerScore);
				PlayerInfo banker = GameEngine.Instance.EnumTablePlayer(wbanker);
				if(banker == null)
				{
					m_bEnableSysBanker = true;
					o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace(0 ,0);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = "系统坐庄";
				}else
				{
					m_bEnableSysBanker = false;
					o_bank_player.transform.FindChild ("ctr_user_face").gameObject.GetComponent<UIFace>().ShowFace((int)banker.HeadID , (int)banker.VipLevel);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").gameObject.GetComponent<UILabel>().text = formatMoney(banker.Money);
				}
				if((byte)GetSelfChair() == wbanker)
				{
					o_apply_btn.SetActive(false);
					o_cancel_btn.SetActive(true);
					o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(true);
					o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				}else
				{
					o_apply_btn.SetActive(true);
					o_cancel_btn.SetActive(false);
				}
				bankerList.Remove(banker);
				cnMsgBox("庄家轮换");
				UpdateUserView();

			}catch(Exception ex){
//				cnMsgBox("ChangeBanker"+ex.Message);
			}
		}

		void OnSubUserCancelBanker(NPacket packet)
		{
//			Debug.LogWarning ("取消申请");
			try
			{
				packet.BeginRead();

				ushort wUser = packet.GetUShort();

				int tempPos = 1;
				PlayerInfo player = GameEngine.Instance.EnumTablePlayer(wUser);
				if(player != null )
				{
							bankerList.Remove(player);
				}
				if( wUser == GetSelfChair() )
				{
					o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(true);
					o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				}

				updateApplyList();
			}catch(Exception ex){
//				cnMsgBox("CancelBanker:"+ex.Message);
			}
		}

		void OnSubGameRecord(NPacket packet)
		{
//			Debug.LogWarning ("游戏记录");

			try
			{
				byte[] m_buffer = new byte[100];
				for(int i = 0; i< GameLogic.MAX_AREA_NUM; i++)
				{
					_resultCount[i] = 0;
				}
				_lPairCount = 0;
				packet.BeginRead();
				ushort dataSize = packet.DataSize;
				byte data = packet.Buff[1];
				int cbNum = 1;
				byte tw  = 0;
				bool xd  = false;
				bool zd  = false;
				byte dx  = 0;
				byte tz  = 0;
				for(int i = 0;i < dataSize - 8; i++)
				{	
					if(i%5==0)
					{
						tw = packet.GetByte();
						xd = packet.GetBool();
						zd = packet.GetBool();
						dx = packet.GetByte();
						tz = packet.GetByte();

						if(dx > tz)
						{
							m_recordDate[cbNum] = 1;
						}else if(dx < tz)
						{
							m_recordDate[cbNum] = 3;
						}else
						{
							m_recordDate[cbNum] = 2;
						}
						cbNum++;
						if(xd && zd)
						{
							m_pairDate[_lPairCount] = 2;
							_lPairCount++;
						}else if(xd)
						{
							m_pairDate[_lPairCount] = 1;
							_lPairCount++;
						}else if(zd)
						{
							m_pairDate[_lPairCount] = 3;
							_lPairCount++;
						}
					}
				}

				_lGameCount = (dataSize - 8)/4;
//				for(int j = 1 ;j <= (dataSize - 8)/4; j++)
//				{
//					_lGameCount = j;
//					GetRecord(m_recordDate[j]);
//				}
				
				updateRecord();
			}catch(Exception ex){
//				cnMsgBox("CancelBanker"+ex.Message);
			}	
		} 			
	
		#region ##################UI 事件#######################

		public void OnBtnAddIvk_1()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[0];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[0].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_2()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[1];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[1].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_3()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[2];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[2].transform.FindChild ("selected").gameObject.SetActive (true);
		}
		public void OnBtnAddIvk_4()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[3];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[3].transform.FindChild ("selected").gameObject.SetActive (true);
		}
		public void OnBtnAddIvk_5()
		{
			onBtnSelect ();
//			Debug.LogWarning ("50w");
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[4];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[4].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_6()
		{
			onBtnSelect ();
//			Debug.LogWarning ("100w");
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[5];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[5].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_7()
		{
			onBtnSelect ();
//			Debug.LogWarning ("chip:"+ m_lChipScore[6]);
			_canChip = true;
			m_lCurrentJetton = m_lChipScore[6];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[6].transform.FindChild ("selected").gameObject.SetActive(true);
		}

		void onBtnSelect()
		{
			for(int i = 0; i<7; i++)
			{
				o_chip_Btn[i].transform.FindChild ("selected").gameObject.SetActive(false);
			}
		}

		void UpdateButtonContron()
		{
			for(int i = 0; i<7; i++)
			{
				o_chip_Btn[i].GetComponent<UIButton>().isEnabled = true;
				o_chip_Btn[i].transform.FindChild("img").GetComponent<UISprite>().color = Color.white;
			}

			if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				onBtnSelect ();
			}
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			Int64 lMoney = userdata.Money - GetTableScore();

			DisableBtnIvk(lMoney);
		}

		void DisableBtnIvk( Int64 lScore)
		{		
			do
			{
				if(m_lChipScore[6]!=0 && o_chip_Btn[6].activeSelf){
					if(lScore >= m_lChipScore[6]) break;
					o_chip_Btn[6].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[6].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[6].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_6();
					}
				}
				if(m_lChipScore[5]!=0 && o_chip_Btn[5].activeSelf){
					if(lScore >= m_lChipScore[5]) break;
					o_chip_Btn[5].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[5].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[5].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_5();
					}
				}
				if(m_lChipScore[4]!=0 && o_chip_Btn[4].activeSelf){
					if(lScore >= m_lChipScore[4]) break;
					o_chip_Btn[4].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[4].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[4].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_4();
					}
				}
				if(m_lChipScore[3]!=0 && o_chip_Btn[3].activeSelf){
					if(lScore >= m_lChipScore[3]) break;
					o_chip_Btn[3].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[3].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[3].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_3();
					}
				}
				if(m_lChipScore[2]!=0 && o_chip_Btn[2].activeSelf){
					if(lScore >= m_lChipScore[2]) break;
					o_chip_Btn[2].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[2].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[2].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_2();
					}
				}
				if(m_lChipScore[1]!=0 && o_chip_Btn[1].activeSelf){
					if(lScore >= m_lChipScore[1]) break;
					o_chip_Btn[1].GetComponent<UIButton> ().isEnabled = false;
					o_chip_Btn[1].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
					if(o_chip_Btn[1].transform.FindChild ("selected").gameObject.activeSelf)
					{
						OnBtnAddIvk_1();
					}
				}
				if(lScore >= m_lChipScore[0]) break;
				o_chip_Btn[0].GetComponent<UIButton> ().isEnabled = false;
				o_chip_Btn[0].transform.FindChild("img").GetComponent<UISprite>().color = new Color(67/255f,67/255f,67/255f,1f);
			}while(false);

		}

		public void OnBtnPlace_DD()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip ) {
				OnPlaceJetton (GameLogic.AREA_DD, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_DX()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip ) {
				OnPlaceJetton (GameLogic.AREA_X, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_HE()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
				OnPlaceJetton (GameLogic.AREA_P, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_TZ()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
				OnPlaceJetton (GameLogic.AREA_Z, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_TD()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
				OnPlaceJetton (GameLogic.AREA_TD, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_CJ6()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_PLAY)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
				OnPlaceJetton (GameLogic.AREA_CJ, m_lCurrentJetton);
			}else
			{
				cnMsgBox("请选择下注筹码");
			}
		}

		// 消息框
		public void cnMsgBox(string val)
		{
			if(o_msgbox != null)
			{
				o_msgbox.SetActive(true);
				o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = val;
				Invoke("closeMsgbox",5);
			}
		}

		void closeMsgbox()
		{
			o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = "";
			o_msgbox.SetActive(false);
		}

		//申请上庄
		public void OnApplyForBanker()
		{
			//庄家判断
			if ( GetSelfChair() == (byte)m_wCurrentBanker ) return;

			if(nListUserCount > 4)
			{
				cnMsgBox("申请人数已满！");
			}
			if(GameEngine.Instance.EnumTablePlayer(GetSelfChair()).Money < m_ApplyCondition)
			{
				if(m_ApplyCondition >= 10000)
				{
					cnMsgBox("金币少于"+ m_ApplyCondition/10000 + "万,无法申请上庄！");
				}
				else
				{
					cnMsgBox("金币不足,无法申请上庄！");
				}
				return;
			}

			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_APPLY_BANKER);
			GameEngine.Instance.Send (packet);
		}

		//取消申请
		public void OnCancelBanker()
		{
			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_CANCEL_BANKER);
			GameEngine.Instance.Send (packet);
		}
		
		//申请下庄
		public void OnCancelForBanker()
		{
			if ( GetSelfChair() != (byte)m_wCurrentBanker ) return;
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_FREE)
			{
				cnMsgBox("处于非空闲时间，无法下庄！");
				return ;
			}
			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_CANCEL_BANKER);
			GameEngine.Instance.Send (packet);
		}

		//玩家信息
		public void OnPlayerInfoIvk()
		{
			clearPlayerInfo();
			o_player_inf.SetActive(true);
			updatePlayerInfo();
			_nInfoTickCount = Environment.TickCount;
		}

		void updatePlayerInfo()
		{
			if(o_player_inf.activeSelf)
			{
				PlayerInfo player = GameEngine.Instance.EnumTablePlayer(GetSelfChair());
				o_player_inf.GetComponent<UIFace>().ShowFace((int)player.HeadID,(int)player.VipLevel);
				o_player_inf.transform.FindChild("lbl_id").GetComponent<UILabel>().text = player.ID.ToString();
				o_player_inf.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = player.NickName;
				o_player_inf.transform.FindChild("lbl_money").GetComponent<UILabel>().text = player.Money.ToString("N0");
				o_player_inf.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "0";
				o_player_inf.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lMeGameScoreCount.ToString("N0");

				if(player.Gender==0)
				{
					o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				}else
				{
					o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "女";
				}

				if(GetSelfChair() == (byte)m_wBankerUser)
				{
					o_player_inf.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
				}else
				{
					o_player_inf.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = "0";
				}
			}
		}

		//庄家信息
		public void OnBankerInfoIvk()
		{
			PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);
			if(bankerdata!=null)
			{
				clearPlayerInfo();
				o_player_inf.SetActive(true);
				updateBankerInfo();
				_nInfoTickCount = Environment.TickCount;
			}
		}

		void updateBankerInfo()
		{
			PlayerInfo player = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);
			if(player != null)
			{
				o_player_inf.GetComponent<UIFace>().ShowFace((int)player.HeadID,(int)player.VipLevel);
				o_player_inf.transform.FindChild("lbl_id").GetComponent<UILabel>().text = player.ID.ToString();
				o_player_inf.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = player.NickName;
				o_player_inf.transform.FindChild("lbl_money").GetComponent<UILabel>().text = player.Money.ToString();
				o_player_inf.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "0";	
				o_player_inf.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lBankerWinScore.ToString();
				o_player_inf.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
				
				if(player.Gender==0)
				{
					o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				}else
				{
					o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "女";
				}
			}
			else
			{
				o_player_inf.GetComponent<UIFace>().ShowFace(0,0);
				o_player_inf.transform.FindChild("lbl_id").GetComponent<UILabel>().text = "0";
				o_player_inf.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = systemName;
				o_player_inf.transform.FindChild("lbl_money").GetComponent<UILabel>().text = "0";
				o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "男";
				o_player_inf.transform.FindChild("lbl_score").GetComponent<UILabel>().text = "0";
				o_player_inf.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = m_wBankerTime.ToString();
			}
		}

		void clearPlayerInfo()
		{
			o_player_inf.transform.FindChild("lbl_id").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_nick").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_money").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_gender").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_score").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_ticket").GetComponent<UILabel>().text = "";
			o_player_inf.transform.FindChild("lbl_bank").GetComponent<UILabel>().text = "";
		}

		//设置扑克
		void SetCardInfo(byte[] cbCardCount, byte[,] cbTableCardArray)
		{
			if (cbCardCount!=null)
			{			
				UICardControl ctr1 = o_card_X.GetComponent<UICardControl>();
				UICardControl ctr2 = o_card_Z.GetComponent<UICardControl>();
				byte[] card_x = new byte[3];
				byte[] card_z = new byte[3];
				for(int i = 0; i<3; i++)
				{
					card_x[i] = cbTableCardArray[0,i];
					card_z[i] = cbTableCardArray[1,i];
				}
				ctr1.SetCardData( card_x, cbCardCount[0]);
				ctr2.SetCardData( card_z, cbCardCount[1]);
			}
			else
			{
			}
		}

		byte GetCardValue(byte bCard)
		{
			byte MASK_VALUE = 0x0F;
			byte bValue = (byte)(bCard & MASK_VALUE);
			return bValue;
		}

		byte GetCardPoint(byte[] bCards, int counts)
		{
			byte bValue = 0;
			byte bPoint = 0;
			for(int i = 0; i < counts; i++)
			{
				bValue = GetCardValue(bCards[i]);
				if(bValue > 9)
				{
					bValue = 0;
				}
				bPoint += bValue;
			}
			return (byte)(bPoint%10);
		}

		byte GetCardColor(byte bCard)
		{
			byte MASK_COLOR = 0xF0;			
			byte bColor = (byte)((bCard & MASK_COLOR) >> 4);	
			return bColor;
		}

		//当局成绩
		void SetCurGameScore(long lMeCurGameScore, long lMeCurGameAllScore, long lBankerCurGameScore, 
		                     long lBankerTotallScore, int nBankerTime, long lGameRevenue)
		{
			m_lMeCurGameScore = lMeCurGameScore;			
			m_lMeCurGameAllScore = lMeCurGameAllScore;			
			m_lTmpBankerWinScore = lBankerCurGameScore;
			m_lBankerWinScore = lBankerTotallScore;
			m_wBankerTime = (ushort)nBankerTime;
			m_lGameRevenue = lGameRevenue;
			m_lMeCurWinScore = lMeCurGameScore;
			m_lMeGameScoreCount += lMeCurGameScore;
		}

		//加注消息
		void OnPlaceJetton(byte wParam, Int64 lParam)
		{		
			//庄家判断
			if ( GetSelfChair() == (byte)m_wBankerUser ) return;
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			Int64 lMoney = userdata.Money - GetTableScore();
			if(lMoney < lMinTableScore) 
			{
				cnMsgBox("金币低于本桌最低所需金币,无法下注!");
				return;
			}
			if(m_lCurrentJetton>m_lAreaLeftScore[wParam])
			{
				cnMsgBox("下注金额大于此区域可下注金额!");
				return;
			}

			#if UNITY_STANDALONE_WIN
			vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
			
			#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
			vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
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
			#endif

			m_lTouchPoint.Add(vecClick);
			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_PLACE_JETTON);
			packet.Addbyte(wParam);
			packet.AddLong(lParam);
			GameEngine.Instance.Send (packet);
		}	

		//下注筹码
		void ShowCurChips()
		{
			for (byte i = 0; i < GameLogic.MAX_AREA_NUM; i++)
			{
				if (o_playr_chips[i]!= null && _lTableScore[i] > 0)
				{
					o_playr_chips[i].GetComponent<UILabel>().text = _lTableScore[i].ToString("N0");
				}

				if(o_chips_count[i] != null && m_lAreaInAllScore[i] >= 0)
				{
					o_chips_count[i].transform.FindChild("chip_label").gameObject.GetComponent<UILabel>().text = m_lAreaInAllScore[i].ToString("N0");
				}
			}
		}
		void AppendChips(byte bchair, long nUserChips, byte chipsArea , Vector3 point)
		{
			try
			{
				switch(chipsArea)
				{
					case GameLogic.AREA_X:
						UIChipControl ctr1 = o_chip_container[GameLogic.AREA_X].GetComponent<UIChipControl>();
						ctr1.AddChips(bchair, nUserChips, chipsArea, point);			
					break;	

					case GameLogic.AREA_P:
						UIChipControl ctr2 = o_chip_container[GameLogic.AREA_P].GetComponent<UIChipControl>();
						ctr2.AddChips(bchair, nUserChips, chipsArea, point);
					break;

					case GameLogic.AREA_Z:
						UIChipControl ctr3 = o_chip_container[GameLogic.AREA_Z].GetComponent<UIChipControl>();
						ctr3.AddChips(bchair, nUserChips, chipsArea, point);
					break;

					case GameLogic.AREA_DD:
						UIChipControl ctr4 = o_chip_container[GameLogic.AREA_DD].GetComponent<UIChipControl>();
						ctr4.AddChips(bchair, nUserChips, chipsArea, point);
					break;

					case GameLogic.AREA_TD:
						UIChipControl ctr5 = o_chip_container[GameLogic.AREA_TD].GetComponent<UIChipControl>();
						ctr5.AddChips(bchair, nUserChips, chipsArea, point);
						break;
					case GameLogic.AREA_CJ:
						UIChipControl ctr6 = o_chip_container[GameLogic.AREA_CJ].GetComponent<UIChipControl>();
						ctr6.AddChips(bchair, nUserChips, chipsArea, point);
						break;
				}
				if(nUserChips>=1000000)
				{
					PlaySound(SoundType.CHIP2);
				}else
				{
					PlaySound(SoundType.CHIP1);
				}

			}
			catch (Exception ex)
			{

			}
		}
		
		private void ShowResultView()
		{
			ShowResultView(true);
		}
		
		void ShowResultView(bool bshow)
		{
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			//金币限制检测
			if (bshow)
			{
				Invoke("CloseResultView", 10.0f);
			}
		}

		void CloseResultView()
		{
			for (byte i = 0; i < GameLogic.MAX_AREA_NUM; i++)
			{
				if (o_playr_chips[i]!= null)
				{
					o_playr_chips[i].GetComponent<UILabel>().text = "";
					_lTableScore[i] = 0;
				}

				if(o_chips_count[i] != null)
				{
					o_chips_count[i].transform.FindChild("chip_label").gameObject.GetComponent<UILabel>().text = "";
					m_lAreaInAllScore[i] = 0;
				}
			}

			for(int i = 0;i<GameLogic.MAX_AREA_NUM; i++)
			{
				if(o_chip_container[i] != null){
					o_chip_container[i].GetComponent<UIChipControl>().ClearChips();
				}
			}

			for(int i = 0; i < m_cbCardCount[0]; i++)
			{
				Destroy(o_card_X.transform.FindChild("card_"+i).gameObject);
			}
			for(int i = 0; i < m_cbCardCount[1]; i++)
			{
				Destroy(o_card_Z.transform.FindChild("card_"+i).gameObject);
			}

			o_openCard_Bg.SetActive(false);
			o_openDes_X.SetActive(false);
			o_openDes_Z.SetActive(false);
			o_openDes_lbl.GetComponent<UILabel>().text = "";
			o_ScoreX_lbl.GetComponent<UILabel>().text = "";
			o_ScoreZ_lbl.GetComponent<UILabel>().text = "";
			o_openCard.SetActive(false);
		}

		//规则
		public void OnBtnRuleIvk()
		{
			bool bshow = !o_rules.active;
			o_rules.SetActive(bshow);
			o_rules.transform.FindChild("scrollBar").GetComponent<UIScrollBar>().value = 0;
			if (bshow == true)
			{
				_nInfoTickCount = Environment.TickCount;
			}
		} 

		//退出按钮显示
		public void OnBtnLeaveIvk()
		{
			bool bshow = !o_close_box.active;
			o_close_box.SetActive(bshow);

			if (bshow == true)
			{
				o_pup_btn.SetActive(false);
				_nInfoTickCount = Environment.TickCount;
			}
		} 
		//上庄
		public void OnBtnApplyIvk()
		{
			bool bshow = !o_apply_list.activeSelf;
			o_apply_list.SetActive(bshow);
			updateApplyList();
			if (bshow == true)
			{
				_nInfoTickCount = Environment.TickCount;
			}
		} 	
				
		//退出按钮
		public void OnBtnBackIvk()
		{
			Debug.LogWarning("退出");
			if (!GameEngine.Instance.IsPlaying()||(GetTableScore()==0 && GetSelfChair() != (byte)m_wBankerUser))
			{
				OnConfirmBackOKIvk();
			}
			else
			{
				cnMsgBox("游戏进行中，无法离开！");
			}
		}

		//清除UI
		void OnClearInfoIvk()
		{
			ClearAllInfo();
		}


		#endregion

		void ClearAllInfo()
		{
//			o_rules.SetActive(false);
			o_apply_list.SetActive(false);
			o_player_inf.SetActive(false);

			if(o_close_box != null){
				o_close_box.SetActive(false);
			}
			if(o_pup_btn != null){
				o_pup_btn.SetActive(true);
			}
		}

		public void PlaySound(SoundType sound)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
		}

		//获取自己座位号
		byte GetSelfChair()
		{
			return (byte)GameEngine.Instance.MySelf.DeskStation;
		}

		//确认退出
		void OnConfirmBackOKIvk()
		{
			try
			{
				_bStart = false;
				GameEngine.Instance.Quit();

				_bReqQuit = true;
				_nQuitDelay = System.Environment.TickCount;
				CancelInvoke();			
			}
			catch (Exception ex)
			{
//				cnMsgBox("OnConfirmBackOKIvk"+ex.Message);
			}
		}

		void SetUserClock(byte chair, uint time, TimerType timertype)
		{
			try
			{
				o_clock.GetComponent<UIClock>().SetTimer(time*1000);			
			}
			catch (Exception ex)
			{
			}
		}

		//庄家信息
		void SetBankerInfo(ushort wBankerUser,long lBankerScore) 
		{
			//切换判断
			if (m_wCurrentBanker!=wBankerUser)
			{
				m_wCurrentBanker = wBankerUser;
				m_wBankerTime = 0;
				m_lBankerWinScore = 0;	
				m_lTmpBankerWinScore = 0;
			}
			m_lBankerScore=lBankerScore;
		}

		//允许系统做庄
		void EnableSysBanker( bool bEnableSysBanker ) {m_bEnableSysBanker=bEnableSysBanker;}

		//定时处理事件
		void OnTimerEnd()
		{
			try
			{
				switch (_bTimerType)
				{
					case TimerType.TIMER_NULL:
					{
						Debug.LogWarning ("time null");
						//OnConfirmBackOKIvk();
						break;
					}
						
					case TimerType.TIMER_CHIP:
					{
						Debug.LogWarning ("time chip");
						//OnBtnGiveupIvk();
						break;
					}
					case TimerType.TIMER_OPEN:
					{
						Debug.LogWarning ("time open");
						break;
					}
				}
			}
			catch (Exception ex)
			{

			}
		}

	}

}