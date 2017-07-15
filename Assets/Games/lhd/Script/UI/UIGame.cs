using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.LHD
{    
	#region ##################结构定义#######################

	public enum TimerType
	{
		TIMER_NULL  = 0,	 
		TIMER_READY = 1,  //空闲时间
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
		COUNTDOWN = 7
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
		public GameObject o_info_recZP;
		public GameObject o_info_recSY;

		public GameObject o_apply_list;
		public GameObject o_msgbox;

		public GameObject o_rec_Ball;
		public GameObject o_rec_table;

		public GameObject o_pup_btn;
		public GameObject o_close_box;

		GameObject o_chip_buttons = null;
		GameObject o_bank_player = null;
		GameObject o_add_buttons = null;
		GameObject o_time_label = null;
		GameObject o_clock = null;
	
		//显示筹码
		GameObject o_chip_long = null;
		GameObject o_chip_hu = null;
		GameObject o_chip_he = null;
		//开牌
		public GameObject o_openCard;
		public GameObject o_openCard_Bg;
		GameObject o_card_long = null;
		GameObject o_card_hu = null;
		GameObject o_cardback_long= null;
		GameObject o_cardback_hu = null;
		GameObject o_card_cover1 = null;
		GameObject o_card_cover2 = null;

		//玩家各区域下注筹码数量
		GameObject[] o_playr_chips = new GameObject[GameLogic.MAX_AREA_NUM];	
		//每个区域的总分	
		long[]	m_lAreaInAllScore = new long[GameLogic.MAX_AREA_NUM];
		GameObject[] o_chips_count = new GameObject[GameLogic.MAX_AREA_NUM];
		//各区域可下注分数
		long[] m_lAreaLeftScore = new long[GameLogic.MAX_AREA_NUM];	

		//各区域下注数目
		static long[] _lTableScore = new long[GameLogic.MAX_AREA_NUM];
		//结果统计
		static int[] _resultCount = new int[GameLogic.MAX_AREA_NUM];
		//通用数据
		static bool _bStart = false;
		static TimerType _bTimerType = TimerType.TIMER_NULL;
		static int _nInfoTickCount = 0;
		static int _lGameCount = 0;                        	//玩家游戏局数

		int[]	m_lAllChips = new int[GameLogic.CHIP_ALL];	//筹码种类
		int[]	m_lGameChips = new int[7];					//显示筹码
		long[]	m_lChipScore = new long[7];					//筹码金额
		byte[]	m_cbCardCount = new byte[2];				//扑克数目
		byte[,]	m_cbTableCardArray = new byte[2,3];			//桌面扑克
		byte[]	m_cbSendCount = new byte[2];				//扑克数目
		byte[,]	m_recSYDate = new byte[15,3];				//输赢表数据
		byte[]	m_recordDate = new byte[GameLogic.MAX_SCORE_HISTORY];  //游戏记录
		byte[] 	m_buffer 	= new byte[100];
		long[] 	m_lAreaLimitScore = new long[GameLogic.MAX_AREA_NUM];  //区域下注限制

		public List<Vector3> m_lTouchPoint = new List<Vector3>();

		PlayerInfo userdata;

		//音效
		public AudioClip[] _GameSound = new AudioClip[8];
		//游戏变量
		long            lMinTableScore;						//坐下最低金额
		long			m_lMaxChipBanker;					//最大下注 (庄家)
		byte		    m_cbTimeLeave;						//剩余时间
		int				m_nChipTime;						//下注次数 (本局)
		int				m_nChipTimeCount;					//已下次数 (本局)
		long            m_nListUserCount;					//列表人数
		bool            _canChip;							//是否可下注
		static 	 byte   _bBankerUser = GameLogic.NULL_CHAIR;
		static   int    _nQuitDelay = 0;
		static   bool   _bReqQuit = false;	
//		static   bool   _bReqChip = true;					//等待下注

		Vector3 vecClick = new Vector3 ( 0, 0, 0 );					//筹码位置
		long  	nListUserCount;										//列表人数
		List<PlayerInfo> bankerList = new List<PlayerInfo>();		//申请庄家列表										
		List<GameObject> recDL_List = new List<GameObject>();		//大路图记录列表
		List<GameObject> recZP_List = new List<GameObject>();		//珠盘图记录列表
		List<GameObject> recSY_List = new List<GameObject>();		//输赢图记录列表

		int 						m_lChipNum;
		Int64						m_lCurrentJetton;					//当前筹码
		Int64						m_lLastJetton;						//上一局筹码
		bool						m_bShowChangeBanker;				//轮换庄家
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
		long						m_lMeCurGameReturnScore;			//本次返回金币
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
			
//			GameEngine.Instance.SendUserSitdown();
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

				o_chip_long = GameObject.Find("scene_game/ctr_chips_long");
				o_chip_hu = GameObject.Find("scene_game/ctr_chips_hu");
				o_chip_he = GameObject.Find("scene_game/ctr_chips_he");

				o_playr_chips[1] = GameObject.Find("scene_game/dlg_player/lbl_chips_1");
				o_playr_chips[2] = GameObject.Find("scene_game/dlg_player/lbl_chips_2");
				o_playr_chips[3] = GameObject.Find("scene_game/dlg_player/lbl_chips_3");

				o_chips_count[1] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_long");
				o_chips_count[2] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_he");
				o_chips_count[3] = GameObject.Find("scene_game/dlg_chip_area/ChipCount_hu");
			
				o_card_long = GameObject.Find("scene_game/dlg_openCard/Card_Long");
				o_card_hu = GameObject.Find("scene_game/dlg_openCard/Card_Hu");
				o_cardback_long =  GameObject.Find("scene_game/dlg_openCard/Card_Long_back");
				o_cardback_hu =  GameObject.Find("scene_game/dlg_openCard/Card_Hu_back");
				o_card_cover1 = GameObject.Find("scene_game/dlg_openCard/card_cover1");
				o_card_cover2 = GameObject.Find("scene_game/dlg_openCard/card_cover2");
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
			_bStart = false;
			_nQuitDelay = 0;
			_bReqQuit = false;
			_canChip = false;
			m_ApplyCondition = 1000000;
//			m_AreaLimit = 10000000000;
			m_bEnableSysBanker = true;
//			_bReqChip = true;
			_bTimerType = TimerType.TIMER_READY;
			_nInfoTickCount = Environment.TickCount;

			o_clock.SetActive (false);
			o_openCard.SetActive (false);
			userdata = GameEngine.Instance.MySelf;
			o_time_label.GetComponent<UISprite>().spriteName = "blank";
			o_bank_player.transform.FindChild ("ctr_user_face").FindChild ("sp_face").GetComponent<UISprite> ().spriteName = "blank";
			o_chip_buttons.SetActive (false);

			m_lChipNum = 1;

			//庄家信息
			m_lBankerScore = 0; 	 	//庄家积分
			m_wCurrentBanker = 255;	    //庄家位置
			m_wBankerUser = 255;

			m_lMeCurGameScore = 0; 		
			m_lMeGameScoreCount = 0;
			m_lMeCurWinScore = 0;
			bankerList.Clear();
			m_lLastJetton = 0;
			o_chip_long.GetComponent<UIChipControl>().ClearChips();
			o_chip_he.GetComponent<UIChipControl>().ClearChips();
			o_chip_hu.GetComponent<UIChipControl>().ClearChips();
			o_card_cover1.SetActive(false);
			o_card_cover2.SetActive(false);

			//游戏各区域可下注信息
			for(byte i = 1; i < 4; i++)
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
			o_rules.SetActive(false);

			o_chip_long.GetComponent<UIChipControl>().ClearChips();
			o_chip_he.GetComponent<UIChipControl>().ClearChips();
			o_chip_hu.GetComponent<UIChipControl>().ClearChips();
		}

		void UpdateUserView()
		{
			try
			{
				if (_bStart == false) return;

				m_wBankerUser = m_wCurrentBanker;	

				o_player_nick.GetComponent<UILabel>().text = userdata.NickName;
				o_player_money.GetComponent<UILabel>().text = (userdata.Money-_lTableScore[0]-_lTableScore[1]-_lTableScore[2]).ToString("N0");

				o_player_face.GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
				
				if(GetSelfChair() == m_wCurrentBanker)
				{
					o_apply_btn.SetActive(false);
					o_cancel_btn.SetActive(true);
					o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = formatMoney(userdata.Money - _lTableScore[0]-_lTableScore[1]-_lTableScore[2] );
					//自动下庄
					if(m_wBankerTime > 4)
					{
						if( GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_FREE){
							OnCancelForBanker(); 
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
					o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace((int)bankerdata.HeadID, (int)bankerdata.VipLevel);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = formatMoney(bankerdata.Money);
					}else
					{
					o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace(0, 0);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = systemName;					}
				}
				o_info_chip.transform.FindChild("label_score").GetComponent<UILabel>().text = m_lMeGameScoreCount.ToString("N0");
				//游戏各区域可下注信息
				for(byte i = 1; i < 4; i++){
					countLeftScore(i);
				}
				updateApplyList();
				updateRecord();
				SetAddBtn();
			}
			catch(Exception ex)
			{
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

		string formatMoneyW(long money)
		{
			string tempMoney = money.ToString();			
			if(money>9999) 
			{
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}

		//更新走势图
		public void updateRecord()
		{
			if(o_info_recDL.activeSelf)
			{
				Transform dlTmp = o_info_recDL.transform.FindChild("content");
				dlTmp.FindChild("Long").FindChild("label").GetComponent<UILabel>().text = _resultCount[1].ToString();
				dlTmp.FindChild("He").FindChild("label").GetComponent<UILabel>().text = _resultCount[2].ToString();
				dlTmp.FindChild("Hu").FindChild("label").GetComponent<UILabel>().text = _resultCount[3].ToString();

				updateRecDL();

			}else if(o_info_recZP.activeSelf)
			{
				updateRecZP();

			}else if(o_info_recSY.activeSelf)
			{
				Transform syTmp = o_info_recSY.transform.FindChild("content").FindChild("label");
				syTmp.FindChild("label_long").GetComponent<UILabel>().text = _resultCount[1].ToString();
				syTmp.FindChild("label_he").GetComponent<UILabel>().text = _resultCount[2].ToString();
				syTmp.FindChild("label_hu").GetComponent<UILabel>().text = _resultCount[3].ToString();

				GameObject scrollBar = o_info_recSY.transform.FindChild("content").FindChild("scrollBar").gameObject;

				if(_lGameCount%65 < 5)
				{
					scrollBar.GetComponent<UIScrollBar> ().value = 0;
				}else if(_lGameCount%65 < 15)
				{
					scrollBar.GetComponent<UIScrollBar> ().value = (_lGameCount%65 - 1)/15f;
				}else
				{
					scrollBar.GetComponent<UIScrollBar> ().value = 0.99f;
				}
				updateRecSY();
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
			for(int i = 0; i < _lGameCount % GameLogic.MAX_SCORE_HISTORY + 1; i++)
			{
				if(m_recordDate[i] != 0)
				{
					if(tempdata + m_recordDate[i]%10 == 4 || count == cRowCount)
					{
						cellColumn++;
						cellRow = 0;
						count = 0;
					}
					GameObject ball = Instantiate(o_rec_Ball ,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
					ball.GetComponent<UISprite>().spriteName = "tag_ball"+ (int)m_recordDate[i]%10 ;
					ball.transform.parent = scrollView.transform.FindChild("grid");
					ball.transform.localScale = new Vector3(1f,1f,1f);
					ball.GetComponent<UISprite>().depth = 20;
					ball.transform.localPosition = point_e;
					ball.transform.localPosition -= new Vector3(-cellColumn*(cellWidth),cellRow*cellHeight+heightInterval,0);
					recDL_List.Add(ball);
					cellRow++;
					count++;
					if(m_recordDate[i]%10 != 2) tempdata = (byte)(m_recordDate[i]%10);
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
				Destroy(recZP_List[i]);
			}
			recZP_List.Clear();

			GameObject scrollView = o_info_recZP.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollBar = o_info_recZP.transform.FindChild("content").FindChild("scrollBar").gameObject;
			GameObject scrollBg = scrollView.transform.parent.FindChild("scrollBg").gameObject;
			int viewWidth, viewHeight, widthInterval, heightInterval, cellColumn, cellRow;
			int cRowCount = (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.y /20;
			int cColumnCount =  (int)NGUIMath.CalculateRelativeWidgetBounds(scrollBg.transform).size.x / (21+1);
			float cellHeight = 20f;
			float cellWidth = 21f;
			widthInterval= heightInterval = 1;
			cellColumn = cellRow = 0;

			Vector3 point_e = scrollView.transform.FindChild("grid").FindChild("ball_e").transform.localPosition;
			Vector3 point_ee = scrollView.transform.FindChild("grid").FindChild("ball_ee").transform.localPosition;
			point_e.y = point_ee.y;
//			if(_lGameCount < 17)
//			{
				point_e.x = point_ee.x;
//			}
			for(int i = 0; i < _lGameCount % GameLogic.MAX_SCORE_HISTORY + 1; i++)
			{
				if(m_recordDate[i] != 0)
				{
					GameObject ball = Instantiate(o_rec_Ball ,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject;
					ball.GetComponent<UISprite>().spriteName = "tag_ball"+ (int)m_recordDate[i]%10 ;
					ball.transform.parent = scrollView.transform.FindChild("grid");
					ball.transform.localScale = new Vector3(1f,1f,1f);
					ball.GetComponent<UISprite>().depth = 20;
					ball.transform.localPosition = point_e;
					ball.transform.localPosition -= new Vector3(-cellColumn*(cellWidth+widthInterval),cellRow*cellHeight,0);
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

		void updateRecSY()
		{
			for(var i = 0; i < recSY_List.Count; i++)
			{
				Destroy(recSY_List[i]);
			}
			recSY_List.Clear();
			GameObject scrollView = o_info_recSY.transform.FindChild("content").FindChild("scrollView").gameObject;
			GameObject scrollBar = o_info_recSY.transform.FindChild("content").FindChild("scrollBar").gameObject;
			GameObject scrollBg = scrollView.transform.parent.FindChild("scrollBg").gameObject;

			for(int i=0; i < 15; i++)
			{
				for(int j=1; j < 4; j++)
				{
				scrollView.transform.FindChild("table").FindChild("round"+i).FindChild("sp"+j).gameObject.SetActive(false);
				if(m_recSYDate[i,j-1] != 0)
				{
				scrollView.transform.FindChild("table").FindChild("round"+i).FindChild("sp"+j).gameObject.SetActive(true);		
				scrollView.transform.FindChild("table").FindChild("round"+i).FindChild("sp"+j).GetComponent<UISprite>().spriteName = "gou02";
				if(m_recSYDate[i,j-1]>20)
				{
					scrollView.transform.FindChild("table").FindChild("round"+i).FindChild("sp"+j).GetComponent<UISprite>().spriteName = "gou01";
				}
				else if(m_recSYDate[i,j-1]>10)
				{
					scrollView.transform.FindChild("table").FindChild("round"+i).FindChild("sp"+j).GetComponent<UISprite>().spriteName = "cha";
				}
				}
				}
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

		public void closeLoading()
		{
			if(UIManager.Instance.o_loading!=null)
			{
				UIManager.Instance.o_loading.SetActive(false);
			}
		}

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
//					Debug.LogWarning("游戏配置");
					OnGameOptionResp(packet);
					break;
				}
				case SubCmd.SUB_GF_SCENE:	//101
				{
//					Debug.LogWarning("场景信息");
					OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
					break;
				}
				case SubCmd.SUB_GF_MESSAGE:
				{
//					Debug.LogWarning("系统消息");
					OnGameMessageResp(packet);
					break;
				}
				case SubCmd.SUB_GF_USER_READY:
				{
//					Debug.LogWarning("用户同意");
					break;
				}
				case SubCmd.SUB_GF_USER_CHAT:
				{
//					Debug.LogWarning("用户聊天");
					break;
				}
				case SubCmd.SUB_GF_LOOKON_CONTROL:
				{
//					Debug.LogWarning("旁观控制");
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
				//GameEngine.Instance.MySelf.AllowLookon = packet.GetByte();
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
//					Debug.LogWarning("空闲");
					SwitchFreeSceneView(packet);
					break;
				}
				case (byte)GameLogic.GS_WK_CHIP:
				{
					//下注阶段
//					Debug.LogWarning("下注");
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;
					o_clock.SetActive(true);
					o_time_label.GetComponent<UISprite>().spriteName = "word_xiazhu";

					o_chip_buttons.SetActive(true);
					o_chip_long.GetComponent<UIChipControl>().ClearChips();
					o_chip_he.GetComponent<UIChipControl>().ClearChips();
					o_chip_hu.GetComponent<UIChipControl>().ClearChips();
					PlaySound(SoundType.STARTCHIP);
					SwitchPlaySceneView(packet);
					break;
				}
				case (byte)GameLogic.GS_WK_OPEN:
				{
					//开牌阶段
//					Debug.LogWarning("开牌");
					o_clock.SetActive(true);
					o_time_label.GetComponent<UISprite>().spriteName = "word_kaipai";
					GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_OPEN;
					SwitchPlaySceneView(packet);
					break;
				}
			default:
				Debug.LogWarning("Other!");
				break;
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
			try
			{
				ResetGameView();				
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;
				
				packet.BeginRead();

				byte TimeLeave = packet.GetByte();
				Int64 lUserGold = packet.GetLong();
				ushort lBanker = packet.GetUShort();				//当前庄家
				ushort lBankTimes = packet.GetUShort();				//庄家局数
				Int64 lBankerWinScore = packet.GetLong();   		//庄家成绩
				Int64 lBankerScore = packet.GetLong(); 				//庄家积分
				bool bEnableSysBanker = packet.GetBool();   		//系统做庄
				Int64 lApplyCondition = packet.GetLong();			//上庄申请条件

//				Int64 lAreaLimit  = packet.GetLong();				//区域限制
				for(int i=1; i<4; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				long lMinScore = packet.GetLong();
				int tempCount = 0;
				for(int i = 0; i<GameLogic.CHIP_ALL; i++)
				{
					m_lAllChips[i] = packet.GetInt();
					if(m_lAllChips[i]==1 && tempCount<7)
					{
						m_lGameChips[tempCount] = i+1;
						tempCount++;
					}
				}
				m_lChipNum = tempCount;

				systemName = packet.GetString(32);					//系统昵称
				string roomName = packet.GetString(32);						//房间名字

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

				SetChipScore();
				lMinTableScore = lMinScore;

				m_ApplyCondition = lApplyCondition;
//				m_AreaLimit = lAreaLimit;
				o_time_label.GetComponent<UISprite>().spriteName = "word_idle";
				SetUserClock(GetSelfChair(),(uint)TimeLeave-1, TimerType.TIMER_READY);
				SetBankerInfo(lBanker, lBankerScore);
				m_bEnableSysBanker = bEnableSysBanker;
				m_wBankerTime = lBankTimes;

				for(int i = 1; i <= 3; i++)
				{
					m_lAreaInAllScore[i] = 0;
					_lTableScore[i] = 0;
					o_playr_chips[i].GetComponent<UILabel>().text = "";
					o_chips_count[i].transform.FindChild("chip_label").gameObject.GetComponent<UILabel>().text = "";
				}
				UpdateUserView();
				
				if(UIManager.Instance.o_loading!=null)
				{
					UIManager.Instance.o_loading.SetActive(false);
				}

			}catch(Exception ex)
			{
			}
		}
		

		//游戏场景处理函数
		void SwitchPlaySceneView(NPacket packet)
		{
			try
			{	
				if(UIManager.Instance.o_loading!=null)
				{
					UIManager.Instance.o_loading.SetActive(false);
				}

				ResetGameView();
				_bStart = true;

				Int64[]	lAreaScore = new long[GameLogic.MAX_AREA_NUM]; 			//每个区域的总分	
				Int64[]	lPlayerAreaScore = new long[GameLogic.MAX_AREA_NUM]; 	//每个玩家每个区域的总分	

				packet.BeginRead();

				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lAreaScore[i] = packet.GetLong();
				}
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lPlayerAreaScore[i] = packet.GetLong();
				}
				Int64 lUserScore = packet.GetLong();					//最大下注
				Int64 lApplyCondition = packet.GetLong();				//上庄申请条件
//				Int64 lAreaLimit = packet.GetLong();					//区域限制
				for(int i=1; i<4; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				m_ApplyCondition = lApplyCondition;
//				m_AreaLimit = lAreaLimit;

				//扑克设置
				m_cbCardCount[0] = packet.GetByte();
				m_cbCardCount[1] = packet.GetByte();
				m_cbTableCardArray[0,0] =  packet.GetByte();
				m_cbTableCardArray[0,1] =  packet.GetByte();
				m_cbTableCardArray[0,2] =  packet.GetByte();
				m_cbTableCardArray[1,0] =  packet.GetByte();
				m_cbTableCardArray[1,1] =  packet.GetByte();
				m_cbTableCardArray[1,2] =  packet.GetByte();

				ushort lBanker = packet.GetUShort();				//当前庄家
				ushort lBankTimes = packet.GetUShort();				//庄家局数
				Int64 lBankerWinScore = packet.GetLong();   		//庄家成绩
				Int64 lBankerScore = packet.GetLong(); 				//庄家积分
				bool bEnableSysBanker = packet.GetBool();   		//系统做庄

				Int64 lEndBankerScore = packet.GetLong();			//庄家成绩
				Int64 lEndUserScore = packet.GetLong();				//玩家成绩
				Int64 lEndUserReturnScore = packet.GetLong();		//返回积分
				Int64 lEndRevenue = packet.GetLong();				//游戏税收
				byte  TimeLeave = packet.GetByte();					//剩余时间
				byte cbGameStatus = packet.GetByte();				//游戏状态
				long lMinScore = packet.GetLong();					//最低坐下成绩

				int tempCount = 0;
				for(int i = 0; i<GameLogic.CHIP_ALL; i++)
				{
					m_lAllChips[i] = packet.GetInt();
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

				SetChipScore();
				lMinTableScore = lMinScore;

				m_bEnableSysBanker = bEnableSysBanker;
				m_wBankerTime = lBankTimes;
				m_lAreaInAllScore[1] = lAreaScore[1];
				m_lAreaInAllScore[2] = lAreaScore[2];
				m_lAreaInAllScore[3] = lAreaScore[3];
				_lTableScore[1] = lPlayerAreaScore[1];
				_lTableScore[2] = lPlayerAreaScore[2];
				_lTableScore[3] = lPlayerAreaScore[3];

				SetUserClock(GetSelfChair(),(uint)TimeLeave, TimerType.TIMER_OPEN);
				SetBankerInfo(lBanker, lBankerScore);
				ShowCurChips();
				UpdateButtonContron();
				//===============================================================================
				if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_OPEN && TimeLeave < 3)
				{
					o_chip_buttons.SetActive(false);
				}

				SetCurGameScore( lEndUserScore, lEndUserReturnScore,lBankerWinScore, lBankerScore, lBankTimes, lEndRevenue);
				UpdateUserView();

			}catch(Exception ex)
			{
//				cnMsgBox("OpenSceneView:"+ex.Message);
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
					if((byte)bChair==GetSelfChair())
					{
						m_lTouchPoint.RemoveAt(0);
					}
					
					break;
				}
				case SubCmd.SUB_S_CANCEL_BANKER:
				{
					//取消申请
					Debug.LogWarning("取消申请");
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
			try
			{
				if(UIManager.Instance.o_loading!=null && UIManager.Instance.o_loading.activeSelf)
				{
					UIManager.Instance.o_loading.SetActive(false);
				}

				bankerList.Clear();
				o_time_label.GetComponent<UISprite>().spriteName = "word_idle";
				o_clock.SetActive(true);
				o_chip_buttons.SetActive(true);
				o_openCard.SetActive (false);

				_canChip = false;
				UpdateUserView();

				for(int i = 1; i <= 3; i++)
				{
					m_lAreaInAllScore[i] = 0;
					_lTableScore[i] = 0;
					o_playr_chips[i].GetComponent<UILabel>().text = "";
					o_chips_count[i].transform.FindChild("chip_label").gameObject.GetComponent<UILabel>().text = "";
				}

				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

				packet.BeginRead();
				byte timeLeave = packet.GetByte();
				long userCount = packet.GetLong();

				ushort[] apUserId = new ushort[5];
				for(int i = 0; i<5;i++)
				{
					apUserId[i] = packet.GetUShort();
					if(GameEngine.Instance.EnumTablePlayer((uint)apUserId[i])!= null && apUserId[i] != m_wCurrentBanker)
					{
						bankerList.Add(GameEngine.Instance.EnumTablePlayer((uint)apUserId[i]));
					}
				}
				if(userCount<0)
				{
					bankerList.Clear();
				}
				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_READY);

				nListUserCount = userCount;
				UpdateUserView();

				updateApplyList();
				UpdateButtonContron();

			}catch(Exception ex)
			{
//				cnMsgBox("OnGameFreeResp"+ex.Message);
			}
		}

		//玩家下注
		void OnGamePlaceJetton(NPacket packet)
		{
//			Debug.LogWarning("玩家下注");
			try
			{
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;
				Bounds bounds;
				float x,y,x1,x2,y1,y2,z;

				packet.BeginRead();
				ushort bChair = packet.GetUShort();
				byte chipArea = packet.GetByte();
				long nChip = packet.GetLong();
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
					long tempMoney = bankerdata.Money - ( _lTableScore[1]+_lTableScore[2]+_lTableScore[3] );
					o_player_money.GetComponent<UILabel>().text = tempMoney.ToString("N0");
				}
				ShowCurChips();
				AppendChips((byte)bChair, nChip, chipArea, point);

			}catch(Exception ex)
			{

			}
		}

		//计算可下注
		void countLeftScore( byte chipArea)
		{
			PlayerInfo bankerdata = GameEngine.Instance.EnumTablePlayer((uint)m_wBankerUser);

			switch(chipArea)
			{
			case 1:
				if(bankerdata!=null){
					m_lAreaLeftScore[chipArea] = bankerdata.Money + m_lAreaInAllScore[2] + m_lAreaInAllScore[3]- m_lAreaInAllScore[1];
				}else{
					m_lAreaLeftScore[chipArea] = m_lAreaLimitScore[1]- m_lAreaInAllScore[1];
				}
				o_info_chip.transform.FindChild("label_long").gameObject.GetComponent<UILabel>().text = m_lAreaLeftScore[chipArea].ToString("###,###");
				break;
			case 2:
				if(bankerdata!=null){
					m_lAreaLeftScore[chipArea] = (bankerdata.Money + m_lAreaInAllScore[1]/2 + m_lAreaInAllScore[3]/2)*1/7- m_lAreaInAllScore[2];
				}else{
					m_lAreaLeftScore[chipArea] = m_lAreaLimitScore[2]- m_lAreaInAllScore[2];
				}
				o_info_chip.transform.FindChild("label_he").gameObject.GetComponent<UILabel>().text = m_lAreaLeftScore[chipArea].ToString("###,###");
				break;
			case 3:
				if(bankerdata!=null){
					m_lAreaLeftScore[chipArea] = bankerdata.Money + m_lAreaInAllScore[1] + m_lAreaInAllScore[3]- m_lAreaInAllScore[3];
				}else{
					m_lAreaLeftScore[chipArea] =  m_lAreaLimitScore[3] - m_lAreaInAllScore[3];
				}
				o_info_chip.transform.FindChild("label_hu").gameObject.GetComponent<UILabel>().text = m_lAreaLeftScore[chipArea].ToString("###,###");
				break;
			}
		}

		//下注时间
		void OnGameStartResp(NPacket packet)
		{
			Debug.LogWarning ("下分时间");
			try
			{
				o_time_label.GetComponent<UISprite>().spriteName = "word_xiazhu";
				o_clock.SetActive(true);
				o_chip_buttons.SetActive(true);
				o_chip_long.GetComponent<UIChipControl>().ClearChips();
				o_chip_he.GetComponent<UIChipControl>().ClearChips();
				o_chip_hu.GetComponent<UIChipControl>().ClearChips();
				PlaySound(SoundType.STARTCHIP);

				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;
				UpdateButtonContron();

				if (!_canChip && GameEngine.Instance.EnumTablePlayer(GetSelfChair()).Money >= lMinTableScore) {
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

				packet.BeginRead();				
				SetBankerInfo(packet.GetUShort(),packet.GetLong());

				long mineMoney = packet.GetLong();
				byte timeLeave = packet.GetByte();
				int  playerNum = packet.GetInt();
 				
				SetUserClock(GetSelfChair(),(uint)timeLeave-1, TimerType.TIMER_CHIP);

			}catch(Exception ex)
			{
//				cnMsgBox("OnGameStartResp"+ ex.Message);
			}
		}

		//游戏结束
		void OnGameEndResp(NPacket packet)
		{		
			Debug.LogWarning ("开牌时间");
			try
			{			
				if (_bStart == false) return;

				o_openCard.SetActive(true);
				o_clock.SetActive(true);
				_canChip = false;
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_OPEN;
				o_time_label.GetComponent<UISprite>().spriteName = "word_kaipai";

				packet.BeginRead();
				byte l_time = packet.GetByte(); //剩余时间
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
				long lUserScore = packet.GetLong();			//玩家本轮成绩
				long lUserReturnScore = packet.GetLong();   //返回积分
				long lRevenue = packet.GetLong(); 
				
				byte card_long = GetCardValue(m_cbTableCardArray[0,0]);
				byte card_hu = GetCardValue(m_cbTableCardArray[1,0]);

				//===============================================================================
				//游戏记录
				_lGameCount++;
				if( _lGameCount%GameLogic.MAX_SCORE_HISTORY == 0 )
				{
					_resultCount[1] = 0;
					_resultCount[2] = 0;
					_resultCount[3] = 0;
					for(int i=0; i < 15; i++)
					{
						for(int j=0; j < 3; j++)
						{
							m_recSYDate[i,j] = 0;
						}
					}
				}
	
				int _result = 0;
				if(card_long>card_hu)
				{
					_resultCount[1]++;
					_result = 1;
				}else if(card_long<card_hu)
				{
					_resultCount[3]++;
					_result = 3;
				}else{
					_resultCount[2]++;
					_result = 2;
				}

				GetRecord(_result);	
				//===============================================================================

				SetUserClock(GetSelfChair(),(uint)l_time-1, TimerType.TIMER_OPEN);
				SetCardInfo(m_cbCardCount);
				SetCurGameScore( lUserScore, lUserReturnScore, lBankerScore, lBankerTotallScore, nBankerTime,lRevenue);
				showEffectView(lUserScore, card_long, card_hu);

				Invoke("ShowResultView", 3);

			}catch(Exception ex)
			{
//				cnMsgBox("OnGameEndResp:"+ex.Message);
			}
		}

		void GetRecord( int _result)
		{
			if(_lGameCount % GameLogic.MAX_SCORE_HISTORY != 0)
			{
				m_recordDate[_lGameCount % GameLogic.MAX_SCORE_HISTORY] = (byte)_result;
				
				if(_lTableScore[(int)_result]> 0)
				{
					m_recordDate[_lGameCount % GameLogic.MAX_SCORE_HISTORY] += 20;
				}else if(_lTableScore[1]+_lTableScore[2]+_lTableScore[3] > 0)
				{
					m_recordDate[_lGameCount % GameLogic.MAX_SCORE_HISTORY] += 10;
				}
				
				if((_lGameCount% GameLogic.MAX_SCORE_HISTORY) < 15)
				{
					for(int i=0; i < 3; i++)
					{
						m_recSYDate[(_lGameCount% GameLogic.MAX_SCORE_HISTORY)-1,i] = 0;
						if(_result == i+1)
						{
							m_recSYDate[(_lGameCount% GameLogic.MAX_SCORE_HISTORY)-1,i] = m_recordDate[_lGameCount% GameLogic.MAX_SCORE_HISTORY];
						}
					}
				}else
				{
					for(int i=0; i < 15; i++)
					{
						for(int j=0; j < 3; j++)
						{
							m_recSYDate[i,j] = 0;
							if(m_recordDate[(_lGameCount% GameLogic.MAX_SCORE_HISTORY)-14+i]%10 == j+1)
							{
								m_recSYDate[i,j] = m_recordDate[(_lGameCount% GameLogic.MAX_SCORE_HISTORY)-14+i];
							}
						}
					}
				}
				
			}else
			{
				for(int i = 0; i < GameLogic.MAX_SCORE_HISTORY; i++)
				{
					m_recordDate[i] = 0;
				}

				o_info_recDL.transform.FindChild("content").FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();
				o_info_recZP.transform.FindChild("content").FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();

				for(var j = 0; j < recDL_List.Count; j++){
					Destroy(recDL_List[j]);//销毁房间按钮
				}
				for(var j = 0; j < recZP_List.Count; j++){
					Destroy(recZP_List[j]);//销毁房间按钮
				}
				for(var j = 0; j < recSY_List.Count; j++){
					Destroy(recSY_List[j]);
				}
				recDL_List.Clear();
				recZP_List.Clear();
				recSY_List.Clear();
				m_recordDate[0] = (byte)_result;
			}
		}
		
		//开牌效果
		void showEffectView(long lUserScore, byte card_long, byte card_hu)
		{
			if(o_openCard.activeSelf)
			{
				o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(false);
				if(card_long>card_hu)
				{
					if(lUserScore < 0)
					{
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_win2";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1_1";
					}else
					{
						o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(true);
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_win1";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1";
					}
				}else if(card_long<card_hu)
				{
					if(lUserScore < 0)
					{
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_win4";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1_1";
					}else
					{
						o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(true);
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_win3";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1";
					}
				}else
				{
					if(lUserScore < 0)
					{
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_he2";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1_1";
					}else
					{
						o_openCard_Bg.transform.FindChild("light").gameObject.SetActive(true);
						o_openCard_Bg.transform.FindChild ("img1").GetComponent<UISprite> ().spriteName ="jiesuan_he1";
						o_openCard_Bg.transform.FindChild ("img2").GetComponent<UISprite> ().spriteName ="jiesuan1";
					}
				}

				o_openCard_Bg.transform.FindChild ("ScoreLable").GetComponent<UILabel> ().text = lUserScore.ToString ("N0");
//				o_openCard_Bg.transform.FindChild ("ScoreLable").gameObject.GetComponent<UINumber>().SetNumber(lUserScore); 
				if(lUserScore < 0)
				{
					o_openCard_Bg.transform.FindChild ("ScoreLable").GetComponent<UILabel> ().color = Color.black;
				}else
				{
					o_openCard_Bg.transform.FindChild ("ScoreLable").GetComponent<UILabel> ().color = new Color(1.0f,240.0f/250,130.0f/250);
				}

				o_card_cover1.SetActive(true);
				o_card_cover2.SetActive(true);
				StartCoroutine(ShowOpenEffectView(card_long, card_hu));
			}
		}

		void OnSubUserApplyBanker(NPacket packet)
		{
//			Debug.LogWarning ("申请上庄");
			try
			{
				packet.BeginRead();
				ushort ApplyId = packet.GetUShort();
				Debug.LogWarning("Apply Id:"+ApplyId);
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
					o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace(0 ,0);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = "系统坐庄";
				}else
				{
					m_bEnableSysBanker = false;
					o_bank_player.transform.FindChild ("ctr_user_face").GetComponent<UIFace>().ShowFace((int)banker.HeadID , (int)banker.VipLevel);
					o_bank_player.transform.FindChild("ctr_money").FindChild("label_money").GetComponent<UILabel>().text = formatMoney(banker.Money);
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
//				cnMsgBox("ChangeBanker:"+ex.Message);
			}
		}

		void OnSubUserCancelBanker(NPacket packet)
		{
//			Debug.LogWarning ("取消申请");
			try
			{
				packet.BeginRead();

				ushort wUser = packet.GetUShort();
				string szCancelUser = packet.GetString(32);

				int tempPos = 1;
				PlayerInfo player = GameEngine.Instance.EnumTablePlayer(wUser);
				if(player != null && player.NickName == szCancelUser)
				{
					for(int i = 0; i < bankerList.Count;i++)
					{
						if(player.ID == bankerList[i].ID)
						{
							bankerList.Remove(player);
						}
					}
				}
				if( wUser == GetSelfChair() )
				{
					o_apply_list.transform.FindChild("applyBtn").gameObject.SetActive(true);
					o_apply_list.transform.FindChild("cancelBtn").gameObject.SetActive(false);
				}
				updateApplyList();
			}catch(Exception ex){
//				cnMsgBox("CancelBanker"+ex.Message);
			}
		}

		void OnSubGameRecord(NPacket packet)
		{
//			Debug.LogWarning ("游戏记录");

			_resultCount[1] = 0;
			_resultCount[2] = 0;
			_resultCount[3] = 0;
			try
			{
				packet.BeginRead();
				ushort dataSize = packet.DataSize;
				byte[] pack = packet.Buff;
				int cbNum = 1;
				for(int i = 0;i < dataSize - 8; i++)
				{
					m_buffer[i] = packet.GetByte();
					if(i%4==0)
					{
						m_recordDate[cbNum] = m_buffer[i];
						cbNum++;
						if(m_buffer[i]<12)
						_resultCount[m_buffer[i]]++;
					}
				}

				for(int j = 1 ;j <= (dataSize - 8)/4; j++)
				{
					_lGameCount = j;
					GetRecord(m_recordDate[j]);
				}

				updateRecord();
			}catch(Exception ex){

			}	
		} 
			
	
		#region ##################UI 事件#######################

		public void OnBtnAddIvk_1()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[0];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[0].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_2()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[1];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[1].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_3()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[2];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[2].transform.FindChild ("selected").gameObject.SetActive (true);
		}
		public void OnBtnAddIvk_4()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[3];	
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[3].transform.FindChild ("selected").gameObject.SetActive (true);
		}
		public void OnBtnAddIvk_5()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[4];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[4].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_6()
		{
			onBtnSelect ();
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[5];
			m_lLastJetton = m_lCurrentJetton;
			o_chip_Btn[5].transform.FindChild ("selected").gameObject.SetActive(true);
		}
		public void OnBtnAddIvk_7()
		{
			onBtnSelect ();
//			Debug.LogWarning ("chip:"+ m_lChipScore[6]);
			_canChip = true;
			m_lCurrentJetton =  m_lChipScore[6];
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

			if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
			{
				onBtnSelect ();
			}
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			Int64 lMoney = userdata.Money -(_lTableScore[1]+_lTableScore[2]+_lTableScore[3]);


			DisableBtnIvk(lMoney);
		}

		void DisableBtnIvk( Int64 lScore)
		{
			do{
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

		public void OnBtnPlace_Long()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
//				vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
				OnPlaceJetton (GameLogic.AREA_LONG, m_lCurrentJetton);
			}else
			{
				if(GetSelfChair() != (byte)m_wCurrentBanker )
					cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_HE()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
//				vecClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
				OnPlaceJetton (GameLogic.AREA_PING, m_lCurrentJetton);
			}else
			{
				if(GetSelfChair() != (byte)m_wCurrentBanker )
				cnMsgBox("请选择下注筹码");
			}
		}

		public void OnBtnPlace_Hu()
		{
			if ( GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
			{
				cnMsgBox("非下注时间!");
				return ;
			}
			if (_canChip) {
				OnPlaceJetton (GameLogic.AREA_HU, m_lCurrentJetton);
			}else
			{
				if(GetSelfChair() != (byte)m_wCurrentBanker )
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
					cnMsgBox("金币足,无法申请上庄！");
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
		void SetCardInfo(byte[] cbCardCount)
		{
			if (cbCardCount!=null)
			{			
				UICardControl ctr1 = o_card_long.GetComponent<UICardControl>();
				UICardControl ctr2 = o_card_hu.GetComponent<UICardControl>();
				UICardControl ctr3 = o_cardback_long.GetComponent<UICardControl>();
				UICardControl ctr4 = o_cardback_hu.GetComponent<UICardControl>();

				ctr1.SetCardData(m_cbTableCardArray[0,0],1);
				ctr2.SetCardData(m_cbTableCardArray[1,0],1);

				ctr4.SetCardData(254, 1);
				ctr3.SetCardData(254, 1);
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
		
		byte GetCardColor(byte bCard)
		{
			byte MASK_COLOR = 0xF0;			
			byte bColor = (byte)((bCard & MASK_COLOR) >> 4);	
			return bColor;
		}

		IEnumerator ShowOpenEffectView(byte card_long, byte card_hu)
		{
			GameObject card_l = o_cardback_long.transform.FindChild("card_0").gameObject;
			GameObject card_2 = o_cardback_hu.transform.FindChild("card_0").gameObject;
			GameObject card_L = o_card_long.transform.FindChild("card_0").gameObject;
			GameObject card_H = o_card_hu.transform.FindChild("card_0").gameObject;
			Vector3 _oldPos1 = card_l.transform.localPosition;
			Vector3 _NewPos1 = new Vector3(_oldPos1.x +50, _oldPos1.y, 0);
			Vector3 _oldPos2 = card_2.transform.localPosition;
			Vector3 _NewPos2 = new Vector3(_oldPos2.x +50, _oldPos2.y, 0);
			TweenPosition.Begin(card_l, 1.0f, _NewPos1);
			PlaySound(SoundType.SENDCARD);
			yield return new WaitForSeconds (1f);

			_oldPos1 = card_l.transform.localPosition;
			_NewPos1 = new Vector3(_oldPos1.x +1200, _oldPos1.y, 0);
			TweenPosition.Begin(card_l, 0.8f, _NewPos1);
			o_card_cover1.SetActive(false);
			yield return new WaitForSeconds (1.0f);
			Destroy(card_l);

			TweenPosition.Begin(card_2, 0.8f, _NewPos2);
			PlaySound(SoundType.SENDCARD);
			yield return new WaitForSeconds (1f);

			_oldPos2 = card_2.transform.localPosition;
			_NewPos2 = new Vector3(_oldPos2.x +1200, _oldPos2.y, 0);
			TweenPosition.Begin(card_2, 1.0f, _NewPos2);
			o_card_cover2.SetActive(false);
			yield return new WaitForSeconds (1.0f);
			Destroy(card_2);

			if(card_long > card_hu)
			{
				card_L.transform.FindChild("Outside").gameObject.SetActive(true);
				card_L.transform.localScale = new Vector3(1.2f, 1.2f, 1);
			}else if(card_long < card_hu)
			{
				card_H.transform.FindChild("Outside").gameObject.SetActive(true);
				card_H.transform.localScale = new Vector3(1.2f, 1.2f, 1);
			}else
			{
				card_L.transform.FindChild("Outside").gameObject.SetActive(true);
				card_H.transform.FindChild("Outside").gameObject.SetActive(true);
			}
			o_openCard_Bg.SetActive(true);
			o_openCard_Bg.transform.localScale = new Vector3(0,0,0);
			TweenScale.Begin(o_openCard_Bg,0.5f,new Vector3(1f,1f,1f));
			yield return new WaitForSeconds (0.5f);
			if(m_lMeCurWinScore>=0)
			{
				PlaySound(SoundType.WIN);
			}else
			{
				PlaySound(SoundType.LOSE);
			}
			yield return new WaitForSeconds (0.5f);
			updateRecord();
		}

		//当局成绩
		void SetCurGameScore(long lMeCurGameScore, long lMeCurGameReturnScore, long lBankerCurGameScore, 
		                     long lBankerTotallScore, int nBankerTime, long lGameRevenue)
		{
			m_lMeCurGameScore = lMeCurGameScore;			
			m_lMeCurGameReturnScore = lMeCurGameReturnScore;			
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
			Int64 lMoney = userdata.Money -(_lTableScore[1]+_lTableScore[2]+_lTableScore[3]);
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
			for (byte i = 0; i < 12; i++)
			{
				if (o_playr_chips[i]!= null && _lTableScore[i] > 0)
				{
					o_playr_chips[i].GetComponent<UILabel>().text = _lTableScore[i].ToString("###,###");
				}

				if(o_chips_count[i] != null && m_lAreaInAllScore[i] >= 0)
				{
					o_chips_count[i].transform.FindChild("chip_label").gameObject.GetComponent<UILabel>().text = m_lAreaInAllScore[i].ToString("###,###");
				}
			}
		}
		void AppendChips(byte bchair, long nUserChips, byte chipsArea , Vector3 point)
		{
			try
			{
				switch(chipsArea)
				{
					case GameLogic.AREA_LONG:

						UIChipControl ctr1 = o_chip_long.GetComponent<UIChipControl>();
					ctr1.AddChips(bchair, nUserChips, chipsArea, point);			
					break;	
					case GameLogic.AREA_PING:
						UIChipControl ctr2 = o_chip_he.GetComponent<UIChipControl>();
						ctr2.AddChips(bchair, nUserChips, chipsArea, point);

					break;	
					case GameLogic.AREA_HU:
					    UIChipControl ctr3 = o_chip_hu.GetComponent<UIChipControl>();
						ctr3.AddChips(bchair, nUserChips, chipsArea, point);

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
				Invoke("CloseResultView", 8.0f);
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
			o_card_cover1.SetActive(false);
			o_card_cover2.SetActive(false);
			Destroy(o_card_long.transform.FindChild("card_0").gameObject);
			Destroy(o_card_hu.transform.FindChild("card_0").gameObject);
			o_openCard_Bg.SetActive(false);
			o_openCard.SetActive(false);

			o_chip_long.GetComponent<UIChipControl>().ClearChips();
			o_chip_he.GetComponent<UIChipControl>().ClearChips();
			o_chip_hu.GetComponent<UIChipControl>().ClearChips();
		}

		//规则
		public void OnBtnRuleIvk()
		{
			bool bshow = !o_rules.active;
			o_rules.SetActive(bshow);
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
				
		long GetTableScore()
		{
			long tempScore = 0;
			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				tempScore  += _lTableScore[i];
			}
			return tempScore;
		}

		//退出按钮
		public void OnBtnBackIvk()
		{
//			Debug.LogWarning("退出");
				
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
			o_rules.SetActive(false);
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
//				cnMsgBox("OnConfirmBackOKIvk"+ex.Message);
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