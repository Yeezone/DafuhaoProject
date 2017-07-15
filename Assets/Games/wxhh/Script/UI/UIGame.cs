using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.GameUtils;
using System.Text;

namespace com.QH.QPGame.WXHH
{    
	#region ##################结构定义#######################

	public enum TimerType
	{
		TIMER_NULL  = 0,	 
		TIMER_IDLE = 1,  //空闲时间
		TIMER_CHIP  = 2, //下注时间
		TIMER_END  = 3,  //开牌时间
		_GameSound
	};
	public enum SoundType
	{
		STARTGAME = 0,
		STARTCHIP = 1,
		CHIP = 2,
		AGAIN = 3,
		SENDCARD = 4,
		WIN = 5,
		LOSE = 6,
		CROWN = 7,
		COUNTDOWN
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
		public GameObject o_msgbox;
		public GameObject o_rec;		
		public GameObject o_cardView;
		public GameObject o_cardView_3d;
		public GameObject o_openBox;
		public GameObject o_openScore;
		public GameObject o_crownAni;
		public GameObject o_jackpot;
		public GameObject o_player_money;
		public GameObject o_player_chip;
		public GameObject o_rules;

		public GameObject o_btn_heitao;
		public GameObject o_btn_hongtao;
		public GameObject o_btn_meihua;
		public GameObject o_btn_fangkuai;
		public GameObject o_btn_crown;
		public GameObject o_btn_chip;
		public GameObject o_btn_again;
		public GameObject o_btn_cancel;
		public GameObject o_btn_auto;
		public GameObject o_btn_disauto;

		public GameObject[] o_players = new GameObject[8];	    //用户头像
		public GameObject[] o_records = new GameObject[10];
		public GameObject[] o_tableCount = new GameObject[5];
		public GameObject[] o_areaCount = new GameObject[5];
		public GameObject[] o_tableRatio = new GameObject[5];   //下注倍率
		public GameObject[] o_tableRecord = new GameObject[10];  
		public GameObject o_clock;
		public GameObject o_mask;
		public GameObject[] menuBtn = new GameObject[4];
		public GameObject o_music;
		public GameObject o_effect;
		public GameObject o_tipsbox;
		public GameObject o_tipsbox_end;
		public GameObject o_tipsbox_auto;

		GameObject o_player_nick = null;
		GameObject o_time_num = null;
		GameObject o_time_label = null;
		GameObject o_time_sp = null;
		GameObject o_time_bar = null;
		GameObject o_gameCount = null;			//游戏轮数
		GameObject o_BitCount = null;			//游戏局数

		GameObject o_lbl_jackpot = null;		//彩金
		GameObject o_lbl_heitao  = null;		
		GameObject o_lbl_hongtao  = null;
		GameObject o_lbl_meihua  = null;
		GameObject o_lbl_fangkuai  = null;
		GameObject o_lbl_crown  = null;

		GameObject o_result_table  = null;		//记录表格
		GameObject o_btn_arrow  = null;
		GameObject o_addbtn_bg  = null;
		GameObject o_addbtn_bg1  = null;
		GameObject o_addbtn_bg2  = null;
		GameObject o_cardImg  = null;
		GameObject o_card = null;
		GameObject o_cover1  = null;
		GameObject o_cover2 = null;

		//通用数据
		TimerType _bTimerType = TimerType.TIMER_NULL;
		static bool  _bStart = false;
		static int   _nInfoTickCount = 0;
		static int   _nQuitDelay = 0;
		static bool  _bReqQuit = false;	
		static bool  _bAgain = true;

		int[] _cbColorCount = new int[5];						//各花色统计
		static int   _cbGameCount = 0;                        	//游戏局数
		static int 	 _cbBitCount = 0;                        	//游戏轮数
		static long  m_lCurrentJetton = 0;						//当前筹码
		static int   lMonValue = 6;								//金币显示位数
		long   m_lUserWinScore;									//玩家输赢
		long   lMinTableScore;									//坐下最低金额
		long   lMinChipScore;									//最低筹码金额
		long   lMaxChipScore;									//最高筹码金额
		long   lReturnScore;									//本轮赢得金额
		long   m_lUserLimitScore;							    //个人下注总额限制

		int chipIndex = 0;										//分值索引
		long[] chipValue = new long[10];						//下注金额选择
		long curBetScore;										//当前下注
		long curPrizeGold;										//当前彩金

		//彩金
		ushort lRewardUser;										//中奖玩家
		long lGameReward;										//中奖金额

		long[] m_lAreaInAllScore = new long[GameLogic.MAX_AREA_NUM];    	//每个区域的总分	
		long[] m_lTableScore = new long[GameLogic.MAX_AREA_NUM]; 	    	//玩家各区域下注数目
		long[] m_LastScore = new long[GameLogic.MAX_AREA_NUM]; 	    		//玩家上一局下注数目
		long[] m_lBeiPoint = new long[GameLogic.MAX_AREA_NUM];		    	//各区域下注倍率
		byte[] cbHistroyRecord = new byte[GameLogic.MAX_SCORE_HISTORY];		//历史局数
		uint[] m_recordDate = new uint[GameLogic.MAX_SCORE_HISTORY];        //游戏记录

		long[] m_lAreaLimitScore = new long[GameLogic.MAX_AREA_NUM];	    //区域下注限制

		List<GameObject> recColor_List = new List<GameObject>();		    //花色记录列表		
		public AudioClip[] _AudioSound = new AudioClip[9];					//音效
		public AudioClip[] _GameSound = new AudioClip[9];					//新音效
		public AudioClip[] _ResultSound = new AudioClip[14];				//新音效
		public AudioClip[] _BgMusic = new AudioClip[5];				//新音效

		private bool m_bIsAuto = false;
		private bool m_bIsPrize = true;

		//3D翻牌效果
		public GameObject o_cardImg_2d  = null;
		public GameObject o_card_2d = null;
		public GameObject o_cardback = null;
		
		public GameObject _3d_Card;
		public GameObject cardCtr;
		public MeshMorpher m_Pai;
		public MeshMorpher m_Bei;
		public MeshMorpher m_Pai_Bot;

		public Material m_mSmallBall;
		public Texture[]  m_Texture;
		public Mesh[] LastMeshs;


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
				o_player_nick = GameObject.Find("scene_game/dlg_myself/lbl_nickname");
				o_time_num = GameObject.Find("scene_game/dlg_time/num_time");
				o_time_label = GameObject.Find("scene_game/dlg_time/lbl_time");
				o_time_sp = GameObject.Find("scene_game/dlg_time/sp_time");
				o_time_bar = GameObject.Find("scene_game/dlg_time/sp_timebar");
				o_lbl_jackpot = GameObject.Find("scene_game/dlg_jackpot/lbl_money");

				//记录
				o_gameCount = GameObject.Find("scene_game/dlg_result_lbl/lbl_GameCount");	
				o_BitCount = GameObject.Find("scene_game/dlg_result_lbl/lbl_BitCount");
				o_lbl_heitao = GameObject.Find("scene_game/dlg_result_lbl/lbl_heitao");
				o_lbl_hongtao = GameObject.Find("scene_game/dlg_result_lbl/lbl_hongtao");
				o_lbl_meihua = GameObject.Find("scene_game/dlg_result_lbl/lbl_meihua");
				o_lbl_fangkuai = GameObject.Find("scene_game/dlg_result_lbl/lbl_fangkuai");
				o_lbl_crown = GameObject.Find("scene_game/dlg_result_lbl/lbl_crown");
				o_result_table = GameObject.Find("scene_game/dlg_result_table");  	

				o_btn_arrow = GameObject.Find("scene_game/dlg_addBtn/arrow_up");
				o_addbtn_bg  = GameObject.Find("scene_game/dlg_addBtn/btn_BG");
				o_addbtn_bg1  = GameObject.Find("scene_game/dlg_addBtn/btn_BG_e1");
				o_addbtn_bg2  = GameObject.Find("scene_game/dlg_addBtn/btn_BG_e2");

				o_cardImg = o_cardView.transform.FindChild("img").gameObject;
				o_card = o_cardView.transform.FindChild("card").gameObject;
				o_cover1 = o_cardView.transform.FindChild("card_cover1").gameObject;
				o_cover2 = o_cardView.transform.FindChild("card_cover2").gameObject;

				AddEventListener();
			}
			catch(Exception ex)
			{
			}
		}

		void AddEventListener()
		{
			UIEventListener.Get(o_btn_chip).onClick = OnClick;
			UIEventListener.Get(o_btn_heitao).onClick = OnClick;
			UIEventListener.Get(o_btn_hongtao).onClick = OnClick;
			UIEventListener.Get(o_btn_meihua).onClick = OnClick;
			UIEventListener.Get(o_btn_fangkuai).onClick = OnClick;
			UIEventListener.Get(o_btn_crown).onClick = OnClick;
		}
		
		void OnClick(GameObject obj)
		{
			if (obj.name.Equals("btn_chip"))
			{
				OnChangeChips();
			}else if(obj.name.Equals("btn_heitao"))
			{
				OnPlaceJetton(GameLogic.enAreaBlack);
			}else if(obj.name.Equals("btn_hongtao"))
			{
				OnPlaceJetton(GameLogic.enAreaRed);
			}else if(obj.name.Equals("btn_meihua"))
			{
				OnPlaceJetton(GameLogic.enAreaFlower);
			}else if(obj.name.Equals("btn_fangkuai"))
			{
				OnPlaceJetton(GameLogic.enAreaSquare);
			}else if(obj.name.Equals("btn_crown"))
			{
				OnPlaceJetton(GameLogic.enAreaKing);
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

		void FixedUpdate()
		{
//			if ((Environment.TickCount - _nInfoTickCount) > 5000)
//			{
//				ClearAllInfo();
//				_nInfoTickCount = 0;
//			}

			if(m_bIsPrize){
				if ((Environment.TickCount - _nInfoTickCount) > 500)
				{
					maskPrize(curPrizeGold);
					_nInfoTickCount = Environment.TickCount;
				}
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
			_nInfoTickCount = Environment.TickCount;
			curPrizeGold = 0;

			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				_cbColorCount[i] = 0;
				m_LastScore[i] = 0;
			}

			if(o_player_nick!=null)
			{
				o_player_nick.GetComponent<UILabel>().text = "";
			}
			if(o_openScore!=null){
				o_openScore.transform.FindChild("sp_number").GetComponent<UILabel>().text = "0";
			}
			o_player_money.GetComponent<UILabel>().text = "";

			if(UIManager.Instance.curPlatform == enPlatform.PC)
			{
				o_time_num.GetComponent<UINumber>().SetNumber(0);
			}else
			{
				o_time_num.GetComponent<UILabel>().text = "";
			}

			o_lbl_jackpot.GetComponent<UILabel>().text = "0";
			o_gameCount.GetComponent<UILabel>().text = "1";
			o_BitCount.GetComponent<UILabel>().text = "1";

			m_lCurrentJetton = 1;
			m_lUserWinScore = 0;								
			lMinTableScore = 0;	
			curBetScore = 0;
		}

		#endregion

		#region 刷新

		void ResetGameView()
		{
			_nInfoTickCount = Environment.TickCount;
			o_rules.SetActive(false);
		}

		void UpdateUserView()
		{
			try
			{
				if (_bStart == false) return;
				
				PlayerInfo userdata = GameEngine.Instance.MySelf;
				if(o_player_nick!=null)
				{
					o_player_nick.GetComponent<UILabel>().text = userdata.NickName;
				}

				if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_END)
				{
					o_player_money.GetComponent<UILabel>().text = (userdata.Money - GetTableScore()).ToString();
					if(UIManager.Instance.curPlatform == enPlatform.MOBILE)
					{
						o_player_money.GetComponent<UILabel>().text = formatMoney2(userdata.Money - GetTableScore());
					}
				}
				o_gameCount.GetComponent<UILabel>().text = _cbGameCount.ToString();
				o_BitCount.GetComponent<UILabel>().text = _cbBitCount.ToString();

				for( byte chairID = 0; chairID < GameLogic.GAME_PLAYER; chairID++ )
				{
					PlayerInfo playerData = GameEngine.Instance.EnumTablePlayer((uint)chairID );
					if (playerData != null)
					{
						o_players[chairID].SetActive(true);
						o_players[chairID].transform.FindChild("chairNum").gameObject.SetActive(true);
						o_players[chairID].GetComponent<UIFace>().ShowFace((int)playerData.HeadID, 0);
						o_players[chairID].transform.FindChild("nickName").GetComponent<UILabel>().text = playerData.NickName;
					}else
					{
						o_players[chairID].GetComponent<UIFace>().ShowFace(-1,0);
						o_players[chairID].transform.FindChild("chairNum").gameObject.SetActive(false);
						o_players[chairID].SetActive(false);
					}
				}

				Transform scrollview = o_rules.transform.FindChild("scrollView");
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					o_tableRatio[i].GetComponent<UILabel>().text = (m_lBeiPoint[i]/10f).ToString() ; 
					scrollview.FindChild("lbl"+i).GetComponent<UILabel>().text = (m_lBeiPoint[i]/10f).ToString(); 
				}

				o_lbl_heitao.GetComponent<UILabel>().text = _cbColorCount[GameLogic.enAreaBlack].ToString();
				o_lbl_hongtao.GetComponent<UILabel>().text = _cbColorCount[GameLogic.enAreaRed].ToString();
				o_lbl_meihua.GetComponent<UILabel>().text = _cbColorCount[GameLogic.enAreaFlower].ToString();
				o_lbl_fangkuai.GetComponent<UILabel>().text = _cbColorCount[GameLogic.enAreaSquare].ToString();
				o_lbl_crown.GetComponent<UILabel>().text = _cbColorCount[GameLogic.enAreaKing].ToString();

				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					o_tableCount[i].transform.FindChild("num_label").GetComponent<UILabel>().text = formatMoney(m_lTableScore[i], lMonValue);
					o_areaCount[i].transform.FindChild("num_label").GetComponent<UILabel>().text = formatMoney(m_lAreaInAllScore[i], lMonValue);
				}

				o_player_chip.GetComponent<UILabel>().text =  formatMoney(m_lCurrentJetton, 4);
				updateRecTable();
			}
			catch(Exception ex)
			{
			}
		}

		//更新记录图
		void updateRecTable()
		{
			try
			{
				for(var i = 0; i < recColor_List.Count; i++){
					Destroy(recColor_List[i]);//销毁房间按钮
				}

				recColor_List.Clear();//清空房间按钮容器

				float cellHeight = 0;
				float cellWidth = 0f;

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					cellHeight = 28f;
					cellWidth = 50f;
				}else
				{
					cellHeight = 19f;
					cellWidth = 61f;
				}
				int cellColumn = 0;
				int	cellRow = 0;
				
				Vector3 point_e = o_result_table.transform.FindChild("grid").transform.localPosition;
				for( int i = 0; i < _cbGameCount % (cbHistroyRecord.Length+1); i++ )
				{
					GameObject rec = Instantiate( o_rec, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f) ) )as GameObject;
					rec.transform.parent = o_result_table.transform;
					rec.transform.localScale = new Vector3(1f,1f,1f);
					rec.transform.localPosition = point_e;
					rec.transform.localPosition -= new Vector3(-cellColumn*(cellWidth),cellRow*cellHeight,0);


					byte color = GameLogic.GetCardColor(cbHistroyRecord[i]);
					byte value = GameLogic.GetCardValue(cbHistroyRecord[i]);
					if(value == 0) break;
					string tmpValue = value.ToString();
					
					if(value == 11)
					{
						tmpValue = "J";
					}else if(value == 12)
					{
						tmpValue = "Q";
					}else if(value == 13)
					{
						tmpValue = "K";
					}else if(value == 1)
					{
						tmpValue = "A";
					}

					if(color == 4)
					{
						if(value == 15)
						{
							rec.transform.FindChild("crown").GetComponent<UISprite>().spriteName = "img_color4_2";
						}else
						{
							rec.transform.FindChild("crown").GetComponent<UISprite>().spriteName = "img_color4_1";
						}
						rec.transform.FindChild("crown").gameObject.SetActive(true);
					}else
					{
						rec.transform.FindChild("img").gameObject.SetActive(true);
						rec.transform.FindChild("number").gameObject.SetActive(true);
						rec.transform.FindChild("img").GetComponent<UISprite>().spriteName = "img_color" + color;
						rec.transform.FindChild("number").GetComponent<UILabel>().text = tmpValue;
						if(color%2 != 0)
						{
							rec.transform.FindChild("number").GetComponent<UILabel>().color = Color.red;
						}else
						{
							rec.transform.FindChild("number").GetComponent<UILabel>().color = Color.black;
						}
					}
					
					if(((m_recordDate[i]>>8) & 0xff )!= 0)
					{
						rec.transform.FindChild("light").gameObject.SetActive(true);
					}

					recColor_List.Add(rec);
					
					cellColumn++;

					if(UIManager.Instance.curPlatform == enPlatform.PC)
					{
						if(cellColumn>9){
							cellColumn = 0;
							cellRow++;
						}
					}else
					{
						if(cellColumn>5){
							cellColumn = 0;
							cellRow++;
						}
					}
				}
			}catch( Exception ex)
			{
			}
		}

//		void clearTable()
//		{
//			for(int i = 0; i<10; i++)
//			{
//				for(int j = 0; j<10; j++)
//				{
//					o_tableRecord[i].transform.FindChild("rec"+j).FindChild("color").gameObject.SetActive(false);
//					o_tableRecord[i].transform.FindChild("rec"+j).FindChild("value").gameObject.SetActive(false);
//					o_tableRecord[i].transform.FindChild("rec"+j).FindChild("crown").gameObject.SetActive(false);
//					o_tableRecord[i].transform.FindChild("rec"+j).FindChild("light").gameObject.SetActive(false);
//					o_tableRecord[i].transform.FindChild("rec"+j).gameObject.SetActive(false);
//				}
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
//					Logger.UI.LogWarning("游戏配置");
					OnGameOptionResp(packet);
					break;
				}
				case SubCmd.SUB_GF_SCENE:	//101
				{
//					Logger.UI.LogWarning("场景信息");
					canceleLoading();
					OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
					break;
				}
				case SubCmd.SUB_GF_MESSAGE:
				{
//					Logger.UI.LogWarning("系统消息");
					OnGameMessageResp(packet);
					break;
				}
				case SubCmd.SUB_GF_USER_READY:
				{
					break;
				}
				case SubCmd.SUB_GF_USER_CHAT:
				{
					break;
				}
				case SubCmd.SUB_GF_LOOKON_CONTROL:
				{
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
					if (userid != GameEngine.Instance.MySelf.ID){
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
					Logger.UI.LogWarning("空闲");
					SwitchFreeSceneView(packet);
					break;
				}
				case (byte)GameLogic.GS_WK_CHIP:
				{
					//下注阶段
					Logger.UI.LogWarning("下注");
					SwitchPlaySceneView(packet, bGameStatus);
					break;
				}
				case (byte)GameLogic.GS_WK_END:
				{
					//开牌阶段
					Logger.UI.LogWarning("开牌");
					SwitchPlaySceneView(packet, bGameStatus);
					break;
				}
			default:
				Logger.UI.LogWarning("bGameStatus:"+bGameStatus);
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
			}
		}

		//初始场景处理函数
		void SwitchFreeSceneView(NPacket packet)
		{
			try
			{
				canceleLoading();
				ResetGameView();
				_bStart = true;
				_bAgain = true;
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;
				DisableBtnInvoke();

				packet.BeginRead();
				byte lTimeLeave = packet.GetByte();

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lBeiPoint[i] = packet.GetLong();
				}
				lMinChipScore = packet.GetLong();
				lMaxChipScore = packet.GetLong();
				lMinTableScore= packet.GetLong();

				byte bColor = 0;
				for(int i=0; i<GameLogic.MAX_SCORE_HISTORY; i++)
				{
					cbHistroyRecord[i] = packet.GetByte();
					if( GameLogic.GetCardValue(cbHistroyRecord[i]) != 0)
					{
						m_recordDate[i] = cbHistroyRecord[i];
						bColor  = GameLogic.GetCardColor(cbHistroyRecord[i]);
						_cbColorCount[bColor]++;
					}
				}

				_cbGameCount = packet.GetByte();
				_cbBitCount = packet.GetByte();


				m_lUserLimitScore = packet.GetLong();
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++){
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				//可选择筹码
				for(int i = 0; i<10; i++){
					chipValue[i] = packet.GetLong();
				}

				m_lCurrentJetton  = chipValue[0];

				lMonValue = packet.GetInt();
				string strName = packet.GetString(32);				//房间名字

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					StopAllCoroutines();
					StartCoroutine(updateTime(lTimeLeave));
				}else
				{
					SetUserClock(GetSelfChair(),(uint)lTimeLeave, TimerType.TIMER_IDLE);
				}
				_bTimerType = TimerType.TIMER_IDLE;

				if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 空闲时间 -";
				if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_kongxian";

				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					m_lTableScore[i] = 0;
					m_lAreaInAllScore[i] = 0;
				}

				UpdateUserView();			
			}catch(Exception ex)
			{
			}
		}

		//游戏场景处理函数
		void SwitchPlaySceneView(NPacket packet,byte cbGameStatus)
		{
			try
			{	
				canceleLoading();
				ResetGameView();
				DisableBtnInvoke();

				_bStart = true;
				Int64[]	lAreaScore = new long[GameLogic.MAX_AREA_NUM]; 			//每个区域下注的总分	
				Int64[]	lPlayerAreaScore = new long[GameLogic.MAX_AREA_NUM]; 	//玩家每个区域的总分	
				Int64[]	lUserScore = new long[GameLogic.GAME_PLAYER];  //结束所有人得分	

				GameEngine.Instance.MySelf.GameStatus = cbGameStatus;
				if(cbGameStatus==(byte)GameLogic.GS_WK_CHIP)
				{
					if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 投注时间 -";
					if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_tz";
					
				}else
				{
					if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 开牌时间 -";
					if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_kaipai";

				}
				packet.BeginRead();
				byte lTimeLeave = packet.GetByte();

				lMinTableScore=packet.GetLong();
				
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lBeiPoint[i] = packet.GetLong();
				}
				
				lMinChipScore = packet.GetLong();
				lMaxChipScore = packet.GetLong();
//				packet.GetBytes(ref cbHistroyRecord, GameLogic.MAX_SCORE_HISTORY);

				byte bColor = 0;
				for(int i=0; i<GameLogic.MAX_SCORE_HISTORY; i++)
				{
					cbHistroyRecord[i] = packet.GetByte();
					if( GameLogic.GetCardValue(cbHistroyRecord[i]) != 0)
					{
						m_recordDate[i] = cbHistroyRecord[i];
						bColor  = GameLogic.GetCardColor(cbHistroyRecord[i]);
						_cbColorCount[bColor]++;
					}
				}

				_cbGameCount = packet.GetByte();
				_cbBitCount = packet.GetByte();

				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					lAreaScore[i] = packet.GetLong();
					m_lAreaInAllScore[i] = lAreaScore[i];
				}
				for(int i = 0; i < GameLogic.MAX_AREA_NUM; i++)
				{
					m_lTableScore[i] = packet.GetLong();
					m_LastScore[i] = m_lTableScore[i];
				}
				for(int i = 0; i < GameLogic.GAME_PLAYER; i++)
				{
					lUserScore[i] = packet.GetLong();
				}

				byte cbCardValue = packet.GetByte(); 						//结果牌值
				byte cbCardHeiTao = packet.GetByte();                       //黑桃的出现机率
				byte cbCardHongTao = packet.GetByte();                      //红桃的出现机率
				byte cbCardCaoHua = packet.GetByte();                       //梅花
				byte cbCardFanPian = packet.GetByte();                      //方块
				byte cbCardKing = packet.GetByte();  
				//历史积分
				long lEndUserScore = packet.GetLong();  					//玩家成绩
				long lEndUserReturnScore = packet.GetLong();  				//返回积分
				long lRevenue = packet.GetLong();  							//游戏税收

				m_lUserLimitScore = packet.GetLong();
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				//可选择筹码
				for(int i = 0; i<10; i++)
				{
					chipValue[i] = packet.GetLong();
				}

				m_lCurrentJetton  = chipValue[0];

				lMonValue = packet.GetInt();

				if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_CHIP)
				{
					ResumeBtnInvoke();
				}

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					StopAllCoroutines();
					StartCoroutine(updateTime(lTimeLeave));
				}else
				{
					SetUserClock(GetSelfChair(),(uint)lTimeLeave, TimerType.TIMER_CHIP);
				}

				_bTimerType = TimerType.TIMER_CHIP;
				PlayerInfo userdata = GameEngine.Instance.MySelf;
				o_player_money.GetComponent<UILabel>().text = (userdata.Money - GetTableScore()).ToString();
				if(UIManager.Instance.curPlatform == enPlatform.MOBILE)
				{
					o_player_money.GetComponent<UILabel>().text = formatMoney2(userdata.Money - GetTableScore());
				}
				UpdateUserView();

			}catch(Exception ex)
			{
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
			case SubCmd.SUB_S_GAME_END:
			{
				//游戏结束
				OnGameEndResp(packet);
				break;
			}
			case SubCmd.SUB_S_USERPOINT	:
			{
				//玩家下注
				OnSubGamePoint(packet);
				break;
			}
			case SubCmd.SUB_S_CANCEL_BET:
			{
				//清除下注
				OnCancelBet(packet);
				break;
			}
			case SubCmd.SUB_S_GAME_REWARD:
			{
				OnSubGameReward(packet);
				break;
			}
			case SubCmd.SUB_S_PRIZE_POOL:
			{
				setPrizePool(packet);
				break;
			}
			 
			default:	Debug.LogWarning("other");break;
			}
		}

		#endregion

		//========================= 消息处理 ===================================
		//空闲时间
		void OnGameFreeResp(NPacket packet)
		{
			try
			{
				o_crownAni.SetActive(false);
				_cbGameCount++;
				if( _cbGameCount > 100 )
				{
					_cbBitCount++;
					_cbGameCount = 1;
					for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
					{
						_cbColorCount[i] = 0;
					}
				}
				DisableBtnInvoke();
				clearGameScore();

				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;
				_bTimerType = TimerType.TIMER_IDLE;

				packet.BeginRead();
				byte lTimeLeave = packet.GetByte();
				byte cbCardHeiTao = packet.GetByte();                       //黑桃的出现机率
				byte cbCardHongTao = packet.GetByte();                      //红桃的出现机率
				byte cbCardCaoHua = packet.GetByte();                       //梅花
				byte cbCardFanPian = packet.GetByte();                      //方块
				byte cbCardKing = packet.GetByte();  						//皇冠

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 空闲时间 -";
				if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_kongxian";
				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					StopAllCoroutines();
					StartCoroutine(updateTime(lTimeLeave));
				}else
				{
					SetUserClock(0,(uint)lTimeLeave, TimerType.TIMER_IDLE);
				}
				UpdateUserView();

			}catch(Exception ex)
			{
			}
		}

		//下注时间
		void OnGameStartResp(NPacket packet)
		{
			try
			{
				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					PlaySound(SoundType.STARTCHIP);
				}else
				{
					PlayVoice(4,_GameSound);
//					cnMsgBox("开始押注");
					o_tipsbox.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
					o_tipsbox.SetActive(true);

					if(_3d_Card!=null) _3d_Card.SetActive(false);
					
					TweenScale.Begin(o_tipsbox,0.8f,new Vector3(1f,1f,1f));
					Invoke("OnCloseTips",3.0f);
				}
				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;	
				_bTimerType = TimerType.TIMER_CHIP;
				ResumeBtnInvoke();

				packet.BeginRead();
				byte lTimeLeave = packet.GetByte();
				_cbBitCount = packet.GetByte();
				_cbGameCount = packet.GetByte();
				byte cbCardHeiTao = packet.GetByte();                       //黑桃的出现机率
				byte cbCardHongTao = packet.GetByte();                      //红桃的出现机率
				byte cbCardCaoHua = packet.GetByte();                       //梅花
				byte cbCardFanPian = packet.GetByte();                      //方块
				byte cbCardKing = packet.GetByte();  						//皇冠

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					StopAllCoroutines();
					StartCoroutine(updateTime(lTimeLeave-1));
				}else
				{
					SetUserClock(GetSelfChair(),(uint)lTimeLeave-1, TimerType.TIMER_CHIP);
				}

				if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 投注时间 -";
				if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_tz";

				if(m_bIsAuto){
					OnContinueBtn();
//					cnMsgBox("自动下注！");
				}

			}catch(Exception ex)
			{
			}
		}

		void OnCloseTips()
		{
			if(o_tipsbox==null) return;
			if(o_tipsbox.activeSelf)
				o_tipsbox.SetActive(false);
		}

		void OnCloseTips2()
		{
			if(o_tipsbox_end==null) return;
			if(o_tipsbox_end.activeSelf)
				o_tipsbox_end.SetActive(false);
		}
		void OnEndChips()
		{
			if(o_tipsbox_end==null) return;
			if(!o_tipsbox_end.activeSelf){
				o_tipsbox_end.SetActive(true);
				Invoke("OnCloseTips2",2.0f);
			}
		}

		//游戏结束
		void OnGameEndResp(NPacket packet)
		{		
			try
			{			
				if (_bStart == false) return;

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
//					PlaySound(SoundType.STARTCHIP);
				}else
				{
					PlayVoice(5,_GameSound);
				}

				DisableBtnInvoke();

				GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_END;
				Int64[]	lUserWinScore = new long[GameLogic.GAME_PLAYER]; 	    //玩家得分
				Int64[]	lUserScore = new long[GameLogic.GAME_PLAYER]; 	    

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					_cbColorCount[i] = 0;
				}

				packet.BeginRead();
				byte cbCardValue = packet.GetByte(); 						//结果牌值
				byte lTimeLeave = packet.GetByte();

				//玩家输赢
				for(int i = 0; i < GameLogic.GAME_PLAYER; i++)
				{
					lUserWinScore[i] = packet.GetLong();
				}
				//玩家得分
				for(int i = 0; i < GameLogic.GAME_PLAYER; i++)
				{
					lUserScore[i] = packet.GetLong();
				}
				//历史牌值
				byte bColor = 0;
				for(int i=0; i<GameLogic.MAX_SCORE_HISTORY; i++)
				{
					cbHistroyRecord[i] = packet.GetByte();
					if( GameLogic.GetCardValue(cbHistroyRecord[i]) != 0)
					{
						bColor  = GameLogic.GetCardColor(cbHistroyRecord[i]);
						_cbColorCount[bColor]++;
					}
				}
				m_recordDate[_cbGameCount-1] = (uint)cbHistroyRecord[_cbGameCount-1];
				if(m_lTableScore[bColor]>0)
				{
					m_recordDate[_cbGameCount-1] = (uint)(cbHistroyRecord[_cbGameCount-1] | (0xFF<<8));
				}
				byte cbCardHeiTao = packet.GetByte();                       //黑桃的出现机率
				byte cbCardHongTao = packet.GetByte();                      //红桃的出现机率
				byte cbCardCaoHua = packet.GetByte();                       //梅花
				byte cbCardFanPian = packet.GetByte();                      //方块
				byte cbCardKing = packet.GetByte();  						//皇冠
				long lEndUserScore = packet.GetLong(); 						//玩家成绩
				long lEndUserReturnScore = packet.GetLong();  				//返回积分
				long lRevenue = packet.GetLong();  							//游戏税收

				if(o_time_label != null) o_time_label.GetComponent<UILabel>().text = "- 开牌时间 -";
				if(o_time_sp != null) o_time_sp.GetComponent<UISprite>().spriteName = "word_kaipai";
				m_lUserWinScore = lUserWinScore[GetSelfChair()];

				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					StopAllCoroutines();
					StartCoroutine(updateTime(lTimeLeave-1));
				}else
				{
					SetUserClock(GetSelfChair(),(uint)lTimeLeave-1, TimerType.TIMER_END);
				}
				_bTimerType = TimerType.TIMER_END;

//				if(UIManager.Instance.curPlatform == enPlatform.PC)
//				{
				showEffectView( cbCardValue );
				Invoke("ShowResultView", 4);
//				}else
//				{
//					showEffectView( cbCardValue );
//				}

			}catch(Exception ex)
			{
			}
		}
			
		IEnumerator updateTime( long time)
		{
			while( time >= 0 ){
				o_time_num.GetComponent<UINumber>().SetNumber(time);
				if( time < 4 && time > 0) 
					PlaySound(SoundType.COUNTDOWN);
				yield return new WaitForSeconds(1f);
				time--;
			}
		}
	
		//玩家下注
		void OnSubGamePoint(NPacket packet)
		{
			try
			{
				long[] chipScore = new long[5]; 
				packet.BeginRead();
				ushort chairId = packet.GetUShort();

				Logger.UI.LogWarning( chairId+1+"号玩家下注");

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					chipScore[i] = packet.GetLong();
					m_lAreaInAllScore[i] += chipScore[i]; 
					if(chairId == GetSelfChair())
					{
						m_lTableScore[i] += chipScore[i]; 
						if(m_lTableScore[i]>0 && o_btn_cancel!=null)
						{
							if(!o_btn_cancel.GetComponent<UIButton>().isEnabled)
							o_btn_cancel.GetComponent<UIButton>().isEnabled = true;
						}
					}
				}

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				if(chairId == GetSelfChair())
				{
					PlayerInfo userdata = GameEngine.Instance.MySelf;
					o_player_money.GetComponent<UILabel>().text = (userdata.Money - GetTableScore()).ToString();
					if(UIManager.Instance.curPlatform == enPlatform.MOBILE)
					{
						o_player_money.GetComponent<UILabel>().text = formatMoney2(userdata.Money - GetTableScore());
					}
				}

				UpdateUserView();

			}catch( Exception ex)
			{
			}
		}

		//清除下注
		void OnCancelBet(NPacket packet)
		{
			try
			{
//				Logger.UI.LogWarning( "取消下注");
				packet.BeginRead();
				ushort chairId = packet.GetUShort();
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaInAllScore[i] = packet.GetLong();
					m_lTableScore[i] = 0;
				}

				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					m_lAreaLimitScore[i] = packet.GetLong();
				}

				o_btn_cancel.GetComponent<UIButton>().isEnabled = false;

				long tempCount =0;
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++){
					tempCount += m_LastScore[i];
				}
				
				if(tempCount>0 && !m_bIsAuto){
					o_btn_again.GetComponent<UIButton>().isEnabled = true;
					if(o_btn_auto!=null)
						o_btn_auto.GetComponent<UIButton>().isEnabled = true;
				}
				UpdateUserView();

			}catch( Exception ex)
			{
			}
		}

		void OnSubGameReward(NPacket packet)
		{
			Logger.UI.LogWarning("获取彩金");
			packet.BeginRead();
			lRewardUser = packet.GetUShort();
			lGameReward = packet.GetLong();

			showReward(lRewardUser, lGameReward );
		}

		//显示中奖信息
		void showReward(ushort chairID, long reward)
		{
			m_bIsPrize = false;
			PlayerInfo playerData = GameEngine.Instance.EnumTablePlayer((uint)chairID );
			if (playerData != null)
			{
				o_jackpot.transform.FindChild("dlg_jackpot").FindChild("player").GetComponent<UIFace>().ShowFace((int)chairID,0);

				string nickname = playerData.NickName;
				string score = reward.ToString();
				string tempMsg = "恭喜"+(chairID+1)+"号玩家"+"[ff0000]" + nickname + "[-]" +"在本局获得"+"[ff0000]" + reward + "[-]"+"彩金";
//				StringBuilder sb = new StringBuilder ();
//				sb.Append (tempMsg);

				o_jackpot.transform.FindChild("dlg_jackpot").FindChild("label").GetComponent<UILabel>().text = tempMsg;
				o_jackpot.SetActive(true);
			}

			if(curPrizeGold > reward)
				o_lbl_jackpot.GetComponent<UILabel>().text = (curPrizeGold-reward).ToString();

			Invoke("hideReward",5.0f);
		}

		//关闭中奖界面
		void hideReward()
		{
			if(o_jackpot.activeSelf)
				o_jackpot.SetActive(false);
			m_bIsPrize = true;
		}

		void setPrizePool(NPacket packet)
		{
			packet.BeginRead();
			long prizepool = packet.GetLong();
			curPrizeGold = prizepool;
		}

		void maskPrize(long prizepool)
		{
			if(prizepool<0) return;
			int rand1 = UnityEngine.Random.Range(9,11);
			int rand2 = UnityEngine.Random.Range(1,20);
			o_lbl_jackpot.GetComponent<UILabel>().text = ((long)(prizepool * rand1 * rand2 / 100)).ToString() ;
		}

		//开牌效果
		void showEffectView(byte cbCardValue)
		{
			if(UIManager.Instance.curPlatform == enPlatform.PC)
			{
				o_card.SetActive(false);
				o_cover1.SetActive(false);
				o_cover2.SetActive(false);
				o_openBox.transform.FindChild("color").gameObject.SetActive(false);
				o_openBox.transform.FindChild("color_value").gameObject.SetActive(false);
				o_openBox.transform.FindChild("sp_crown").gameObject.SetActive(false);
			}else
			{
				o_card_2d.SetActive(false);
				o_cardback.SetActive(false);
			}

			o_crownAni.transform.FindChild("light").gameObject.SetActive(false);
			o_crownAni.SetActive(false);
			
			byte color = GameLogic.GetCardColor(cbCardValue);
			byte value = GameLogic.GetCardValue(cbCardValue);
			string cardName = "card_"+color+"_"+value;
			string tmpName = "img_C"+color;
			string tmpScore = "";
			string tmpValue = value.ToString();

			if( color == 4 )
			{
				if(value == 15)
				{
					cardName = "card_c1";
					tmpName = "img_C41";
				}else
				{
					cardName = "card_c2";
					tmpName = "img_C41";
				}
				if(UIManager.Instance.curPlatform == enPlatform.PC){
					o_openBox.transform.FindChild("sp_crown").gameObject.SetActive(true);
				}
			}else
			{
				if(value==11)
				{
					tmpValue  = "J";
				}else if(value==12)
				{
					tmpValue  = "Q";
				}else if(value==13)
				{
					tmpValue  = "K";
				}else if(value==1)
				{
					tmpValue  = "A";
				}
				if(UIManager.Instance.curPlatform == enPlatform.PC)
				{
					o_openBox.transform.FindChild("color").gameObject.SetActive(true);
					o_openBox.transform.FindChild("color_value").gameObject.SetActive(true);
				}
			}

			if( color == 4 ){
				tmpScore = (m_lTableScore[color]*m_lBeiPoint[color]*0.1f + GetTableScore()).ToString();
			}else
			{
				tmpScore = (m_lTableScore[color]*m_lBeiPoint[color]*0.1f).ToString();
			}

			//PC端
			if(UIManager.Instance.curPlatform == enPlatform.PC)
			{
				o_card.GetComponent<UISprite>().spriteName = cardName;
				o_cover1.GetComponent<UISprite>().spriteName = cardName;
				o_cover2.GetComponent<UISprite>().spriteName = cardName;

				o_openBox.transform.FindChild("color").GetComponent<UISprite>().spriteName = tmpName;
				o_openBox.transform.FindChild("sp_crown").GetComponent<UISprite>().spriteName = tmpName;
				o_openBox.transform.FindChild("color_value").GetComponent<UILabel>().text = tmpValue.ToString();
				o_openBox.transform.FindChild("num_bg").FindChild("sp_number").GetComponent<UILabel>().text = "+"+tmpScore;
				if(color%2 != 0)
				{
					o_openBox.transform.FindChild("color_value").GetComponent<UILabel>().color = Color.red;
				}else
				{
					o_openBox.transform.FindChild("color_value").GetComponent<UILabel>().color = Color.black;
				}

				StartCoroutine(ShowOpenEffectView(color));	
			}else
			{
				if( color == 4 )
				{
					if(value==15)
					{
						m_mSmallBall.mainTexture = m_Texture[52];
//						o_card_2d.GetComponent<UI2DSprite>().sprite2D = m_Texture[52];
					}else
					{
						m_mSmallBall.mainTexture = m_Texture[53];
//						o_card_2d.GetComponent<UI2DSprite>().material = m_Texture[53];
					}
				}else
				{
					m_mSmallBall.mainTexture = m_Texture[13*color+value-1];
//					o_card_2d.GetComponent<UI2DSprite>().mainTexture = m_Texture[13*color+value];
				}

				o_card_2d.SetActive(false);
				o_cardback.SetActive(false);
				long.TryParse(tmpScore,out lReturnScore);
				StartCoroutine(Show3DEffectView(color));	
			}
		}

		//手机开牌效果
		IEnumerator Show3DEffectView( byte color )
		{

			Vector3 _oldPos1 = o_cardback.transform.localPosition;
			Vector3 _NewPos1 = new Vector3(_oldPos1.x, _oldPos1.y+1000, 0);
			o_cardback.transform.localPosition = _NewPos1;

			o_cardback.SetActive(true);
			TweenPosition.Begin(o_cardback, 2.0f, _oldPos1);

//			yield return new WaitForSeconds(0.5f);
			PlaySound(SoundType.SENDCARD);
			
			yield return new WaitForSeconds(2.5f);

			o_cardback.SetActive(false);
			o_cardImg_2d.SetActive(false);
			_3d_Card.SetActive(true);
			cardCtr.GetComponent<PokerAnimCtr>().isShow = true;

			yield return new WaitForSeconds(2.0f);
			m_Pai_Bot.gameObject.SetActive(true);
			if(color == 4)
			{
				o_crownAni.SetActive(true);
				PlaySound(SoundType.CROWN);
				yield return new WaitForSeconds(0.5f);
				o_crownAni.transform.FindChild("light").gameObject.SetActive(true);
			}
			yield return new WaitForSeconds(0.5f);

			if(o_openScore!=null)
			{
				o_openScore.transform.FindChild("sp_number").GetComponent<UILabel>().text = lReturnScore.ToString();
			}
			yield return new WaitForSeconds(2.0f);
			playScoreEffect();
			yield return new WaitForSeconds(1.0f);	

			updateRecTable();
		}

		IEnumerator ShowOpenEffectView( byte color )
		{
			Vector3 _oldPos = o_cardImg.transform.localPosition;
			Vector3 _oldPos1 = o_cover1.transform.localPosition;
			Vector3 _NewPos1 = new Vector3(_oldPos1.x + 200, _oldPos1.y, 0);
			Vector3 _oldPos2 = o_cover2.transform.localPosition;
			Vector3 _NewPos2 = new Vector3(_oldPos2.x - 200, _oldPos2.y, 0);
			o_cover1.transform.localPosition = _NewPos1;
			o_cover2.transform.localPosition = _NewPos2;
			o_card.transform.localPosition = _oldPos;

			yield return new WaitForSeconds(1f);

			if(_cbGameCount%3 == 0)
			{
				o_card.transform.localPosition = _NewPos2;
				o_card.SetActive(true);
				TweenPosition.Begin(o_card, 1.5f, _oldPos);
			}
			else
			{
				o_cover1.SetActive(true);
				o_cover2.SetActive(true);
				TweenPosition.Begin(o_cover1, 1.5f, _oldPos);
				TweenPosition.Begin(o_cover2, 1.5f, _oldPos);
			}

			yield return new WaitForSeconds(1f);
			PlaySound(SoundType.SENDCARD);
			yield return new WaitForSeconds(0.5f);

			o_card.SetActive(true);
			if(UIManager.Instance.curPlatform == enPlatform.PC){
				o_openBox.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			}
			yield return new WaitForSeconds(0.5f);
			if(o_openScore!=null)
			{
				o_openScore.transform.FindChild("sp_number").GetComponent<UILabel>().text = lReturnScore.ToString();
			}

			if(color == 4)
			{
//				yield return new WaitForSeconds(0.5f);
				o_crownAni.SetActive(true);
				PlaySound(SoundType.CROWN);
				yield return new WaitForSeconds(0.5f);
				o_crownAni.transform.FindChild("light").gameObject.SetActive(true);
				yield return new WaitForSeconds(3f);
			}

			if(UIManager.Instance.curPlatform == enPlatform.PC)
			{
				o_openBox.SetActive(true);
				TweenScale.Begin(o_openBox,0.5f,new Vector3(1f,1f,1f));
			}else
			{
				if(color != 4){
					yield return new WaitForSeconds(3.0f);
				}
				playScoreEffect();
			}

			yield return new WaitForSeconds(0.4f);
			if(m_lUserWinScore < 0)
			{
				PlaySound(SoundType.LOSE);
			}else
			{
				PlaySound(SoundType.WIN);
			}

			yield return new WaitForSeconds(0.5f);
			updateRecTable();

		}

		void playScoreEffect()
		{		
			if(lReturnScore>0)
			{
				PlayerInfo playerInfo = GameEngine.Instance.MySelf;
				float tmpTime = 4;
				
				if(m_lUserWinScore>500000)
				{
					tmpTime = 6;
				}
				else if(m_lUserWinScore>100000)
				{
					tmpTime = 4;
				}else
				{
					tmpTime = 3;
				}
				o_openScore.GetComponent<UIScoreEffect>()._time = tmpTime;
				o_player_money.GetComponent<UIScoreEffect>()._time = tmpTime;
				o_player_money.GetComponent<UIScoreEffect>()._targetScore = playerInfo.Money /*+ lReturnScore*/;

				o_openScore.GetComponent<UIScoreEffect>().Play();
				o_player_money.GetComponent<UIScoreEffect>().Play();
			}else
			{
			}
		}

		#region ##################UI 事件#######################

		//规则
		public void OnBtnRuleIvk()
		{
			bool bshow = !o_rules.active;
			o_rules.SetActive(bshow);
			o_rules.transform.FindChild("scrollBar").GetComponent<UIScrollBar>().value = 0;
			if (bshow == true)
			{
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

		//加注消息
		void OnPlaceJetton( byte area )
		{	
			try
			{
				if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
				{
					if(UIManager.Instance.curPlatform == enPlatform.PC)
					{
						cnMsgBox("非下注时间!");
					}else
					{
						OnEndChips();
					}			

					return;
				}
				PlaySound(SoundType.CHIP);
				PlayerInfo userdata = GameEngine.Instance.MySelf;
				Int64 lMoney = userdata.Money - GetTableScore();
				if(lMoney < lMinTableScore || lMoney < 0) 
				{
					if(m_bIsAuto){
						OnAutoBtn();
					}
					cnMsgBox("金币低于本桌最低所需金币,无法下注!");
					return;
				}

				long[] chips = new long[GameLogic.MAX_AREA_NUM];
				chips[area] = m_lCurrentJetton;

				if(chips[area]+ m_lAreaInAllScore[area]> m_lAreaLimitScore[area])
				{
					if(m_bIsAuto){
						OnAutoBtn();
					}
					cnMsgBox("筹码金额大于本花色可下注金额,下注失败");
					return;
				}
				if(chips[area]+ GetTableScore()> m_lUserLimitScore)
				{
					if(m_bIsAuto){
						OnAutoBtn();
					}
					cnMsgBox("下注总金额大于个人可下注金额,下注失败");
					return;
				}

				NPacket packet = NPacketPool.GetEnablePacket ();
				packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_SET_POINT);
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
				{
					packet.AddLong(chips[i]);
				}
				
				GameEngine.Instance.Send (packet);
			}
			catch(Exception ex)
			{
			}
		}	

		//取消押分
		public void OnCancelBtn()
		{
			PlaySound((SoundType)5);
			if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP){
				return;
			}
			if(GetTableScore()==0) return;

			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_CANCEL);
			ushort chairId = (ushort)GetSelfChair();
			packet.AddUShort(chairId);
			GameEngine.Instance.Send (packet);
		}

		//续压
		public void OnContinueBtn()
		{
			if(UIManager.Instance.curPlatform==enPlatform.MOBILE){
				PlaySound((SoundType)5);
			}else
				PlaySound(SoundType.CHIP);

			if(GameEngine.Instance.MySelf.GameStatus != (byte)GameLogic.GS_WK_CHIP)
			{
				OnEndChips();
				return;
			}

			PlayerInfo userdata = GameEngine.Instance.MySelf;
			long tmpScore = 0;

			for(byte k = 0; k<GameLogic.MAX_AREA_NUM; k++)
			{
				tmpScore += m_LastScore[k];
			}

			if(tmpScore > userdata.Money ) 
			{
				if(m_bIsAuto) {OnAutoBtn();}
				if(UIManager.Instance.curPlatform == enPlatform.PC) cnMsgBox("金币不足,下注失败!");
				return;
			}

			long temp = m_lCurrentJetton ;
			for(byte i = 0; i<GameLogic.MAX_AREA_NUM; i++){
				m_lCurrentJetton = m_LastScore[i];
				if(m_lCurrentJetton>0)
				{
					OnPlaceJetton(i);
				}
			}
			m_lCurrentJetton = temp;
			_bAgain = false;
			o_btn_again.GetComponent<UIButton>().isEnabled = false;
		}

		//自动
		public void OnAutoBtn()
		{
			PlaySound((SoundType)5);
			bool bshow = o_btn_auto.activeSelf;
			if (bshow)
			{
				o_btn_auto.SetActive(false);
				o_btn_disauto.SetActive(true);
				m_bIsAuto = true;
				o_tipsbox_auto.SetActive(true);

			}else
			{
				o_btn_auto.SetActive(true);
				o_btn_disauto.SetActive(false);
				m_bIsAuto = false;
				o_tipsbox_auto.SetActive(false);
			}
			o_btn_again.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_heitao.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_hongtao.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_meihua.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_fangkuai.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_crown.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			o_btn_chip.GetComponent<UIButton>().isEnabled = !m_bIsAuto;
			if(m_bIsAuto)
			{
				if(GameEngine.Instance.MySelf.GameStatus==(byte)GameLogic.GS_WK_CHIP && (GetTableScore()==0))
				{
					OnContinueBtn();
				}
			}
		}

		public void showAddBtn()
		{
			if(UIManager.Instance.curPlatform == enPlatform.MOBILE)
			{
				Vector3 _oldPos = o_addbtn_bg2.transform.localPosition;
				Vector3 _newPos = o_addbtn_bg1.transform.localPosition;
				Vector3 _targetPos = o_addbtn_bg.transform.localPosition;
				GameObject o_arrow1	= o_btn_arrow.transform.FindChild("img1").gameObject;
				GameObject o_arrow2	= o_btn_arrow.transform.FindChild("img2").gameObject;
				bool bshow = o_arrow1.activeSelf;
				CancelInvoke();
				if (bshow)
				{
					if(_targetPos != _newPos)
					{
						TweenPosition.Begin(o_addbtn_bg, 0.4f, _newPos);
					}
					o_arrow1.SetActive(false);
					o_arrow2.SetActive(true);
				}else if(bshow == false)
				{

					if(_targetPos != _oldPos)
					{
						TweenPosition.Begin(o_addbtn_bg, 0.4f, _oldPos);
					}
					o_arrow2.SetActive(false);
					o_arrow1.SetActive(true);
				}		
		  	}
		}

		public void closeAddBtn()
		{
			if(UIManager.Instance.curPlatform == enPlatform.MOBILE)
			{
				Vector3 _oldPos = o_addbtn_bg2.transform.localPosition;
				Vector3 _newPos = o_addbtn_bg1.transform.localPosition;
				Vector3 _targetPos = o_addbtn_bg.transform.localPosition;
				GameObject o_arrow1	= o_btn_arrow.transform.FindChild("img1").gameObject;
				GameObject o_arrow2	= o_btn_arrow.transform.FindChild("img2").gameObject;
				if(_targetPos != _oldPos)
				{
					TweenPosition.Begin(o_addbtn_bg, 0.4f, _oldPos);
				}
				o_arrow2.SetActive(false);
				o_arrow1.SetActive(true);
			}
		}

		public void onMenuBtn()
		{
			bool bshow = o_mask.activeSelf;
			if (bshow)
			{
				o_mask.SetActive(false);
				menuBtn[1].SetActive(false);
				menuBtn[2].SetActive(false);
				menuBtn[3].SetActive(false);
			}else
			{
				o_mask.SetActive(true);
				menuBtn[1].SetActive(true);
				menuBtn[2].SetActive(true);
				menuBtn[3].SetActive(true);
			}
		}

		void OnChangeChips()
		{
			PlaySound(SoundType.CHIP);
//			PlaySound((SoundType)5);
			chipIndex++;
			if(chipIndex>= getChipCount())	chipIndex = 0;
			m_lCurrentJetton = chipValue[chipIndex];

			o_player_chip.GetComponent<UILabel>().text = formatMoney(m_lCurrentJetton, 4);
		}

		int getChipCount()
		{
			int tmpCount = 0;
			for(int i=0; i<10; i++)
			{
				if(chipValue[i]>0){
					tmpCount = i+1;
				}else
					break;
			}
			return tmpCount;
		}

		void DisableBtnInvoke()
		{
			if(UIManager.Instance.curPlatform == enPlatform.PC || m_bIsAuto)
			{
				o_btn_heitao.GetComponent<UIButton>().isEnabled = false;
				o_btn_hongtao.GetComponent<UIButton>().isEnabled = false;
				o_btn_meihua.GetComponent<UIButton>().isEnabled = false;
				o_btn_fangkuai.GetComponent<UIButton>().isEnabled = false;
				o_btn_crown.GetComponent<UIButton>().isEnabled = false;
			}
			o_btn_cancel.GetComponent<UIButton>().isEnabled = false;
			o_btn_chip.GetComponent<UIButton>().isEnabled = false;

			if(!m_bIsAuto)
			{
				if(o_btn_auto!=null)
				{
					long tempCount = 0;	
					for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++){
						tempCount += m_LastScore[i];
					}
					if(tempCount==0){
						o_btn_auto.GetComponent<UIButton>().isEnabled = false;
					}
				}
			}
			o_btn_again.GetComponent<UIButton>().isEnabled = false;
		}

		void ResumeBtnInvoke()
		{
			if(m_bIsAuto==false)
			{
				o_btn_heitao.GetComponent<UIButton>().isEnabled = true;
				o_btn_hongtao.GetComponent<UIButton>().isEnabled = true;
				o_btn_meihua.GetComponent<UIButton>().isEnabled = true;
				o_btn_fangkuai.GetComponent<UIButton>().isEnabled = true;
				o_btn_crown.GetComponent<UIButton>().isEnabled = true;
				o_btn_chip.GetComponent<UIButton>().isEnabled = true;
			}

			long tempCount = 0;

			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++){
				tempCount += m_LastScore[i];
			}

			if(tempCount>0 && !m_bIsAuto){
				o_btn_again.GetComponent<UIButton>().isEnabled = true;
				if(o_btn_auto!=null)
				o_btn_auto.GetComponent<UIButton>().isEnabled = true;
			}
		}

		void ShowResultView()
		{
			clearGameScore();
			ShowResultView(true);
		}
		
		void ShowResultView(bool bshow)
		{
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			if (bshow)
			{
				if(UIManager.Instance.curPlatform==enPlatform.PC)
				{	
					string strName = "girl"+(_cbGameCount%5);
					o_cardImg.GetComponent<UISprite>().spriteName = strName;
				}else
				{
					string strName = "girl"+(_cbGameCount%10);
					o_cardImg_2d.GetComponent<UISprite>().spriteName = strName;
				}
				Invoke("CloseResultView", 8.0f);
			}
		}

		void CloseResultView()
		{
			PlayerInfo userdata = GameEngine.Instance.MySelf;
			if(UIManager.Instance.curPlatform == enPlatform.PC)
			{
				o_card.SetActive(false);
				o_cover1.SetActive(false);
				o_cover2.SetActive(false);
				o_openBox.SetActive(false);
				o_player_money.GetComponent<UILabel>().text = (userdata.Money - GetTableScore()).ToString();
			}else
			{
				o_card_2d.SetActive(false);
				o_player_money.GetComponent<UILabel>().text = formatMoney2(userdata.Money - GetTableScore());
				o_cardImg_2d.SetActive(true);
				o_card_2d.SetActive(false);
				o_cardback.SetActive(false);
			}

			o_crownAni.SetActive(false);

			UpdateUserView();
		}

		void clearGameScore()
		{
			if(GetTableScore() > 0)
			{
				for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
					m_LastScore[i] = m_lTableScore[i];
			}
			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				m_lTableScore[i] = 0;
				m_lAreaInAllScore[i] = 0;
			}
		}
		
		long GetTableScore()
		{
			long tempScore = 0;
			for(int i = 0; i<GameLogic.MAX_AREA_NUM; i++)
			{
				tempScore  += m_lTableScore[i];
			}
			return tempScore;
		}
				
		//退出按钮
		public void OnBtnBackIvk()
		{
			if ( !GameEngine.Instance.IsPlaying() || GetTableScore() == 0){
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
		}

		#endregion

		void ClearAllInfo()
		{
		}

		public void PlaySound(SoundType sound)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(_AudioSound[(int)sound], fvol, 1);
		}

		public void PlayVoice(int index, AudioClip[] sound)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(sound[index], fvol, 1);
		}

		public void SetBackgroundMusic()
		{
			GameObject o_game = GameObject.Find("scene_game"); 
			if(o_music.GetComponent<UICheckbox>().isChecked)
			{
				o_game.GetComponent<AudioSource>().volume = 0.2f;
			}else
			{
				o_game.GetComponent<AudioSource>().volume = 0;
			}
		}

		public void SetAudioEffect()
		{
			if(o_effect.GetComponent<UICheckbox>().isChecked)
			{
				NGUITools.soundVolume = 1;
			}else
			{
				NGUITools.soundVolume = 0;
			}
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
			}
		}

		void SetUserClock(byte chair, uint time, TimerType timertype)
		{
			try
			{
				o_clock.GetComponent<UIClock>().SetTimer(time*1000);
			}
			catch (Exception ex){
			}
		}
		
		string formatMoney(long money, int len)
		{
			string tempMoney = money.ToString();
			int numLen = len;
			long tempValue = 1;
			
			if(numLen<4) numLen = 4;
			if(numLen > 8) numLen = 8;
			for(int i = 0; i < numLen; i++)
			{
				tempValue *=10;
			}
			
			if(money >= tempValue) 
			{			
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}
		
		string formatMoney2(long money)
		{
			string tempMoney = money.ToString();			
			if(money>9999999) {
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}

		public void canceleLoading()
		{
			if(UIManager.Instance.o_loading!=null){
				UIManager.Instance.o_loading.SetActive(false);
			}
		}

		//定时处理事件
		void OnTimerEnd()
		{
			try
			{
				switch (_bTimerType)
				{
					case TimerType.TIMER_IDLE:
					{
						Invoke("OnCloseTips",6.0f);
						break;
					}
					case TimerType.TIMER_CHIP:
					{
						break;
					}
					case TimerType.TIMER_END:
					{
						if(UIManager.Instance.curPlatform == enPlatform.PC)
						{
							o_openBox.SetActive(false);
							o_card.SetActive(false);
							o_cover1.SetActive(false);
							o_cover2.SetActive(false);
						}else
						{	
//							o_cardImg_2d.SetActive(true);
//							o_card_2d.SetActive(false);
//							o_cardback.SetActive(false);
							cardCtr.GetComponent<PokerAnimCtr>().isClose = true;
							m_Pai_Bot.gameObject.SetActive(false);
							_3d_Card.SetActive(false);

						}
						if(o_openScore!=null){
							o_openScore.transform.FindChild("sp_number").GetComponent<UILabel>().text = "0";
						}
						lReturnScore = 0;
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