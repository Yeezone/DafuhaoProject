using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.SHZ
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

	public enum AniType
	{
		Null = 0,
		Result = 0,
		Light = 2
	};

	public enum ResultType
	{
		Axe = 0,     //斧头
		Spear = 1,  //矛
		Sword = 2,	//刀
		LZS = 3,
		LC = 4,
		SONG = 5,
		TTXD = 6,
		ZYT = 7,
		SHZ = 8
	};

	#endregion

	public class UIGame : MonoBehaviour
	{
		static UIGame _instance = null;
		//比倍游戏
		GameObject o_RiskGame =null; 
		public GameObject o_RG_Prefabs;
		//小玛丽游戏
		public GameObject o_MaryGame=null;
		public GameObject o_MG_Prefabs;
		//游戏对象
		public GameObject o_btn_start;
		public GameObject o_btn_stop;
		public GameObject o_dlg_result;
		public GameObject o_dlg_selfMoney;
		public GameObject o_number;
		public GameObject o_player;
		public GameObject o_msgbox;
		public GameObject o_gold_light;
		public GameObject o_btn_auto;
		public GameObject o_btn_disauto;
		public GameObject o_tips_auto;
		public GameObject o_tips_connect;
		//帮助界面
		GameObject o_help;
		public GameObject o_help_Prefabs;
		//滚动条
		public GameObject o_mask;
		//选择框
		public GameObject o_showPlayer = null;
		public GameObject o_showLines = null;
		//用户头像
		public GameObject[] o_players = new GameObject[6];
		//菜单
		public GameObject[] menuBtn = new GameObject[4];
		public GameObject[] o_table = new GameObject[15];
		public GameObject[] o_lines = new GameObject[10];
		public UIAtlas[] o_atlas = new UIAtlas[9];		

		//滚动条
		public GameObject o_jackpot;

		//音效	
		public AudioClip[] _GameSound = new AudioClip[12];
		public AudioClip[] _ResultSound = new AudioClip[9];

		//按钮
		GameObject o_add_score  = null;
		GameObject o_sub_score  = null;
		GameObject o_add_line  = null;
		GameObject o_sub_line  = null;
		GameObject o_btn_soufen = null;
		GameObject o_btn_bibei = null;

		GameObject dlg_lines = null;
		GameObject lbl_lines = null;
		GameObject lbl_betScore = null;
		GameObject lbl_allScore = null;
		GameObject o_player_money = null;

		//彩金
		GameObject o_lbl_jackpot = null;

		//配置参数
		static bool  _bStart = false;
		int   _nInfoTickCount = 0;
		static bool  _isScorlling = true;
		static bool  _isPlayingAni = false;
		bool  _bReqQuit = false;	

		//分值种类
		byte m_lChipCount;
		//分值索引
		static int chipIndex = 0;	
		//当前筹码
		public static long  m_lCurrentJetton = 0;		
		//当前压线
		public static byte  curChipLines = 9;
		//显示彩线
		byte[] m_lTmpLine = new byte[10];
		//结果音效索引
		byte[] m_lAudioIndex = new byte[15];
		//玩家下注限制
		static long lUserLimitScore = 0; 
		//可选择分值
		long[] betScore = new long[10];
		//场景1游戏结果缓存
		int[] m_lResult = new int[15];
		//玩家本轮赢钱
		public static long m_lUserWinScore = 0;
		//最低坐下金额
		static long lMinTableScore = 0;
		//剩余BounsGame次数
		public static int lBounsGames = 0;
		//BounsGame赢分
		public static long lBGScore = 0;

		bool m_bIsConnect = false;
		bool m_reConnect = false;
		private bool m_bIsOpen = false;

		//是否自动
		private bool m_bIsAuto = false;
//		private bool m_bWaitingAuto = false;

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
			_instance = this;
			try
			{
				//游戏
				dlg_lines = GameObject.Find("scene_game/dlg_lines");
				//dlg_bar
				lbl_lines = GameObject.Find("scene_game/dlg_bar/ctr_line/lbl_lines");
				lbl_betScore = GameObject.Find("scene_game/dlg_bar/ctr_chip/lbl_chip");
				lbl_allScore = GameObject.Find("scene_game/dlg_bar/ctr_allScore/lbl_allScore");
				o_player_money = GameObject.Find("scene_game/dlg_bar/ctr_selfScore/lbl_mScore");
				o_add_score = GameObject.Find("scene_game/dlg_bar/ctr_chip/btn_addScore");
				o_sub_score = GameObject.Find("scene_game/dlg_bar/ctr_chip/btn_subScore");
				o_add_line  = GameObject.Find("scene_game/dlg_bar/ctr_line/btn_addlines");
				o_sub_line  = GameObject.Find("scene_game/dlg_bar/ctr_line/btn_sublines");

				o_lbl_jackpot = GameObject.Find("scene_game/dlg_jackpot/money_lbl");
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
				AddEventListener();
				AddDelegate();
            }
            catch (Exception ex)
            {
            }
		}

		void OnDestroy(){
			UIEventListener.Destroy(o_btn_soufen);
			UIEventListener.Destroy(o_btn_bibei);
			_instance=null;
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
		}

		void InitGameView()
		{
			_bStart = false;
			_bReqQuit = false;
			_isScorlling = true;
			_isPlayingAni = false;
			_bStart = false;
			_nInfoTickCount = Environment.TickCount;
			lMinTableScore = 0;
			m_reConnect = false;
			m_bIsConnect = false;

			lbl_lines.GetComponent<UILabel>().text = "9";
			lbl_betScore.GetComponent<UILabel>().text = "0";
			lbl_allScore.GetComponent<UILabel>().text = "0";
			o_player_money.GetComponent<UILabel>().text = "0";

			o_btn_soufen = o_dlg_result.transform.FindChild("btn_soufen").gameObject;
			o_btn_bibei = o_dlg_result.transform.FindChild("btn_bibei").gameObject;
			o_btn_stop.SetActive(false);

			o_dlg_result.transform.FindChild("lbl_score").GetComponent<UILabel>().text = "0";
			o_lbl_jackpot.GetComponent<UILabel>().text = "0";

			curChipLines = 9;
			m_lCurrentJetton = 0;
			
		}

		void AddEventListener()
		{
			UIEventListener.Get(o_btn_soufen).onClick = EventClick;
			UIEventListener.Get(o_btn_bibei).onClick = EventClick;	
		}
		
		void EventClick(GameObject obj)
		{
			if (obj.name.Equals("btn_soufen"))
			{
				OnGetResult();
			}else if(obj.name.Equals("btn_bibei"))
			{
				StartRiskGame();
			}
			else if(obj.name.Equals("GetScoreBT"))
			{
				OnGetResult();
			}
			else if(obj.name.Equals("close_btn")||obj.name.Equals("scrollArea"))
			{
				OnBtnRuleIvk();
			}
		}

		void AddDelegate()
		{
			UIAutoCtr._instance.OnStartScene += OnStartScene1;
			UIAutoCtr._instance.OnGetScore += OnGetResult;
		}

		#endregion

		#region 刷新

		void ResetGameView()
		{
		}

		void UpdateUserView()
		{
			try
			{		
				PlayerInfo playerInfo = GameEngine.Instance.MySelf;
				if(!_isPlayingAni){
					o_player_money.GetComponent<UILabel>().text = formatMoney(playerInfo.Money,9);
				}
				lbl_betScore.GetComponent<UILabel>().text = formatMoney(m_lCurrentJetton,4);
				SetAllScore();

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
			}
			catch(Exception ex)
			{
			}
		}

		public static UIGame Instance
		{
			get
			{
				if (_instance == null)
				{
//					_instance = new GameObject("scene_game").AddComponent<UIGame>();
//					instance = new GameObject("scene_game").GetComponent<UIGame>();
//					_instance = this;
				}
				return _instance;
			}
		}

		#endregion

		#region ##################框架消息#######################

		//框架消息入口
		void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
		{   
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

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
			}
		}

		//框架事件入口
		void OnTableUserEvent(TableEvents tevt, uint userid, object data)
		{	
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

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
//				Logger.UI.LogWarning("设置游戏状态："+GameEngine.Instance.MySelf.GameStatus);
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
				case (byte)GameLogic.GAME_SCENE_PART1: 
				{
					closeLoading();
					Logger.UI.LogWarning("PART1");
					SwitchPlayScene1View(packet);
					break;
				}
				case (byte)GameLogic.GAME_SCENE_PART2:
				{
					closeLoading();
					Logger.UI.LogWarning("PART2");
					break;
				}
				case (byte)GameLogic.GAME_SCENE_PART3:
				{
					Logger.UI.LogWarning("PART3");
					break;
				}
				default:
					Logger.UI.LogWarning("Other!");
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

			}catch(Exception ex)
			{
			}
		}

		//游戏场景处理函数
		void SwitchPlayScene1View(NPacket packet)
		{
			try
			{	
				_isPlayingAni = false;
				m_reConnect = false;
				m_bIsConnect = false;
				curChipLines = 9;
				chipIndex = 0;
				lBGScore = 0;
				packet.BeginRead();
				int exc_ratio_uscore = packet.GetInt();
				int exc_ratio_credit = packet.GetInt();
				int exc_cell_score = packet.GetInt();
				int exc_max_score = packet.GetInt();

				m_lChipCount= packet.GetByte();
				for(int i = 0; i<10; i++){
					betScore[i] = packet.GetLong();
				}
				lUserLimitScore = packet.GetLong();
				lMinTableScore = packet.GetLong();

//				if(m_lCurrentJetton == 0)
				m_lCurrentJetton = betScore[0];

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
				case SubCmd.SUB_S_SCENE1_START:
				{
					Logger.UI.LogWarning("场景1开始");
					OnGameScene1Resp(packet);
					break;
				}
				case SubCmd.SUB_S_SCENE2_RESULT:
				{
					Logger.UI.LogWarning("买大小");
					OnRiskGameResp(packet);
					break;
				}
				case SubCmd.SUB_S_SCENE3_RESULT:
				{
					Logger.UI.LogWarning("小玛丽");
					OnMaryGameResp(packet);
					break;
				}
				case SubCmd.SUB_S_STOCK_RESULT:
				{
					Logger.UI.LogWarning("103");
//					OnGameEndResp(packet);
					break;
				}
				case SubCmd.SUB_S_ADMIN_LIST_WHITE:
				{
//					Logger.UI.LogWarning("104");
					break;
				}
				case SubCmd.SUB_S_ADMIN_LIST_BLACK:
				{
					Logger.UI.LogWarning("105");
					break;
				}
				case SubCmd.SUB_S_PLACE_JETTON_FAIL:
				{
					cnMsgBox("金额不足，下注失败");
					if(m_bIsAuto) OnAutoBtn();
					resumeBtnIvk();
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
			}
		}

		#endregion

		//========================= 消息处理 ===================================
		//场景1
		void OnGameScene1Resp(NPacket packet)
		{
			try
			{
				m_bIsConnect = true;
				byte[] tempRes = new byte[15];

				packet.BeginRead();

				for(int i=0; i<15; i++)
				{
					m_lResult[i] = packet.GetInt();
					tempRes[i] = (byte)m_lResult[i];
					m_lAudioIndex[i] = 0;
				}

				m_lUserWinScore = packet.GetLong();
				lBounsGames =  packet.GetInt();
				onSetResult(tempRes);

				UIResult.CalcResultTimes(tempRes,curChipLines);
				StartCoroutine(closeEffect());

				PlayerInfo playerInfo = GameEngine.Instance.MySelf;

				o_dlg_result.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lUserWinScore.ToString();

				if(m_lUserWinScore>0)
				{
					o_player_money.GetComponent<UILabel>().text = formatMoney(playerInfo.Money - GetAllScore(), 8);
				}else{
					o_player_money.GetComponent<UILabel>().text = formatMoney(playerInfo.Money,8);
				}
				Invoke("showStopBtn",0.5f);
				UpdateUserView();

			}catch(Exception ex)
			{
			}
		}

		/// <summary>
		/// 买大小 
		/// </summary>
		/// <param name="packet">Packet.</param>
		void OnRiskGameResp(NPacket packet)
		{
			long wScore = 0;
			bool isWin = true;

			packet.BeginRead();
			ushort lpoint1 = packet.GetUShort();
			ushort lpoint2 = packet.GetUShort();

			long lscore = packet.GetLong();

			if(lscore > 0){
				wScore = lscore;
				isWin = true;
			}else
			{
				wScore = -m_lUserWinScore;
				isWin = false;
			}
			m_lUserWinScore = lscore;
			UIBigSmallManger._inatance.OpenReward(isWin,lpoint1,lpoint2,wScore);
		}

		/// <summary>
		/// 小玛丽
		/// </summary>
		/// <param name="packet">Packet.</param>
		void OnMaryGameResp(NPacket packet)
		{
			m_reConnect = true;
			int[] wResult = new int[4];
			int wDes = 0;
			
			packet.BeginRead();
			for(int i=0; i<4; i++)
			{
				wResult[i] = packet.GetInt();
			}
			wDes = packet.GetInt();
			lBounsGames = packet.GetInt();

			long winScore = packet.GetLong();  //赢分
			long lscore = packet.GetLong();    //得分

			lBGScore += winScore;
			m_lUserWinScore = lscore;

			UIMaryManger._instance.SetMaryGame(wDes, wResult, StartMaryGame);
		}

		/// <summary>
		///设置每个格子的动画 
		/// </summary>
		void onSetResult( byte[] res ){
			for(int i=0; i<15; i++){
			  OnSetResultAni(i, (ResultType)res[i]);
			  m_lAudioIndex[i] = res[i];
			}
		}

		IEnumerator StartEffect()
		{	
			dlg_lines.SetActive(true);
			int tempIndex = 0;
			do
			{
				OnEffectStart(tempIndex,true);
				tempIndex++;
			}while(tempIndex<15);
			yield return new WaitForSeconds(0.1f);
		}

		IEnumerator closeEffect()
		{
			int tempIndex = 0;
			yield return new WaitForSeconds(3f);
			
			while(tempIndex<5)
			{
				OnEffectStart(tempIndex,false);
				OnEffectStart(tempIndex+5,false);
				OnEffectStart(tempIndex+10,false);
				yield return new WaitForSeconds(0.1f);
				tempIndex++;
			}
			if(_isScorlling){
				_isScorlling = false;
				o_btn_stop.GetComponent<UIButton>().isEnabled = false;
				yield return new WaitForSeconds(1f);
				OnPlayResAnimation();
				stopScroll();
			}
		}

		
		/// <summary>
		///关闭结果动画 
		/// </summary>
		void onCloseResult()
		{
			for(int i=0; i<15; i++){
				OnEffectStart(i,false);
			}
			Invoke ("stopScroll",0.5f);
			_isScorlling = false;
			hideBoder();
			Invoke("OnPlayResAnimation",1.0f);
		}

		/// <summary>
		/// 启动小游戏
		/// </summary>
		void showRiskGame()
		{
			getLine();
			showLine();

			if(m_lUserWinScore == 0){
				_isPlayingAni = false;
				hideLine();
				UpdateUserView();
			}
			StopAllCoroutines();
			
			if(UIResult.m_Result.Count>0 && m_lUserWinScore>0 )
			{
				o_btn_soufen.GetComponent<UIButton>().isEnabled = true;
				o_btn_bibei.GetComponent<UIButton>().isEnabled = true;
				if(!o_dlg_result.activeSelf) o_dlg_result.SetActive(true);
			}

			if(lBounsGames > 0){
				if(o_MaryGame==null)
				{
					GameObject marygame = Instantiate( o_MG_Prefabs, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f) ) )as GameObject;
					marygame.transform.parent = UIManager.Instance.o_game.transform;
					marygame.transform.localScale = new Vector3(1f,1f,1f);
					o_MaryGame = marygame;
				}

				if(o_MaryGame!=null & !o_MaryGame.activeSelf){
					o_MaryGame.SetActive(true);
					o_MaryGame.GetComponent<UIMaryManger>().o_times.GetComponent<UILabel>().text = lBounsGames.ToString("N0");
					o_MaryGame.GetComponent<UIMaryManger>().o_allScore.GetComponent<UILabel>().text = m_lUserWinScore.ToString("N0");
					o_MaryGame.GetComponent<UIMaryManger>().o_winScore.GetComponent<UILabel>().text = "0";
					o_MaryGame.GetComponent<UIMaryManger>().o_betScore.GetComponent<UILabel>().text = GetAllScore().ToString("N0");
				}

				o_btn_soufen.GetComponent<UIButton>().isEnabled = false;
				o_btn_bibei.GetComponent<UIButton>().isEnabled = false;

				Invoke("OnStartScene3",1.0f);
				return;
			}else
			{
				if(m_bIsAuto){
					o_btn_soufen.GetComponent<UIButton>().isEnabled = false;
					o_btn_bibei.GetComponent<UIButton>().isEnabled = false;
					SetAutoState(AutoState.GET);
				}
			}

			resumeBtnIvk();
		}

		void showStopBtn()
		{
			if(_isScorlling && !m_bIsAuto)
				o_btn_stop.GetComponent<UIButton>().isEnabled = true;
		}

		void OnPlayResAnimation()
		{
			if(  m_lUserWinScore>0 && UIResult.m_Result.Count>0)
			{
				StartCoroutine(PlayAni());
			}else
			{
				if(m_lUserWinScore == 0){
					_isPlayingAni = false;
					UpdateUserView();
				}
				Invoke ("resumeBtnIvk",0.5f);
				if(m_bIsAuto){
					o_btn_soufen.GetComponent<UIButton>().isEnabled = false;
					o_btn_bibei.GetComponent<UIButton>().isEnabled = false;
					SetAutoState(AutoState.START);
				}
//				resumeBtnIvk();
			}
		}

		IEnumerator PlayAni()
		{
			byte[] tmpRes = new byte[15]; 
			byte tmpIndex = 0;
			for(int k=0; k<UIResult.m_Result.Count; k++)
			{
				yield return new WaitForSeconds(0.5f);
				var tempList = UIResult.m_Result[k];
				if(tempList == null) break; 

				byte arr = 0;
				for(int i=0; i<tempList.Count; i++)
				{
					arr = tempList[i];
					OnPlayEffect(arr,  AniType.Light);
					PlaySound(11,_GameSound);
					tmpRes[arr]++;
				}

				if(tempList.Count!=15)
				{
					if(tmpIndex < UIResult.tLines.Count )
					{
						int curline = UIResult.tLines[tmpIndex];
						m_lTmpLine[curline]++;
						if(curline!=0){
							o_lines[curline].SetActive(true);
							yield return new WaitForSeconds(1f);
							o_lines[curline].SetActive(false);
						}
					}
				}

				tmpIndex++;
			}

			yield return new WaitForSeconds(0.5f);

			for(int i=0; i<15; i++)
			{
				if(tmpRes[i]>0){
					OnPlayEffect( i,  AniType.Result);
					OnResultAudio(i);
				}
			}

			yield return new WaitForSeconds(4f);
			showRiskGame();
		}
	
		void getLine()
		{
			int curline = 0;
			for(int i=0; i<UIResult.tLines.Count; i++)
			{
				curline  =  UIResult.tLines[i];
				m_lTmpLine[curline]++;
			}
	    }

		void showLine()
		{
			for(int j=1; j<10; j++)
			{
				if(m_lTmpLine[j]>0){
					o_lines[j].SetActive(true);
				}
			}
			dlg_lines.GetComponent<TweenAlpha>().to = 0;
			dlg_lines.GetComponent<TweenAlpha>().ResetToBeginning();
			dlg_lines.GetComponent<TweenAlpha>().enabled = true;
		}

		void hideLine()
		{
			dlg_lines.GetComponent<TweenAlpha>().ResetToBeginning();
			dlg_lines.GetComponent<TweenAlpha>().to = 1;
			for(int j=1; j<10; j++)
			{
				m_lTmpLine[j] = 0;
				if(o_lines[j].activeSelf){
					o_lines[j].SetActive(false);
				}
			}
		}

		void hideBoder()
		{
			GameObject boder = null;
			for(int i=0; i<15; i++){
				boder  = o_table[i].transform.FindChild("frame").FindChild("border").gameObject;
				boder.GetComponent<UIAnimation>().Stop();
				boder.SetActive(false);
			}
		}

		/// <summary>
		/// 获取彩金
		/// </summary>
		void OnSubGameReward(NPacket packet)
		{
//			Logger.UI.LogWarning("获取彩金");
			packet.BeginRead();
			ushort lRewardUser = packet.GetUShort();
			long lGameReward = packet.GetLong();
			
			showReward(lRewardUser, lGameReward );
		}

		/// <summary>
		/// 彩池更新
		/// </summary>
		void setPrizePool(NPacket packet)
		{
//			Logger.UI.LogWarning("彩池更新");
			packet.BeginRead();
			long prizepool = packet.GetLong();

			long lastValue = 0;
			string sValue = o_lbl_jackpot.GetComponent<UILabel>().text;
			long.TryParse(sValue, out lastValue);


			if(prizepool == lastValue)
			{
				o_lbl_jackpot.GetComponent<UILabel>().text = prizepool.ToString();
			}else
			{
				o_lbl_jackpot.GetComponent<UIScoreEffect>()._targetScore = prizepool;
				Invoke("playPrize",3.0f);
			}
		}

		void playPrize(){
			o_lbl_jackpot.GetComponent<UIScoreEffect>().Play();
		}
	
		//显示中奖信息
		void showReward(ushort chairID, long reward)
		{
			PlayerInfo playerData = GameEngine.Instance.EnumTablePlayer((uint)chairID );
			string nickname = "";
			string tempMsg = "";
			if (playerData != null)
			{		
				string score = reward.ToString();
				if(GetSelfChair() == (byte)chairID)
				{
					tempMsg = "恭喜您在本局获得"+"[ff0000]" + reward + "[-]"+"彩金";
				}else
				{
					nickname = playerData.NickName;
					tempMsg = "恭喜玩家"+"[ff0000]" + nickname + "[-]" +"在本局获得"+"[ff0000]" + reward + "[-]"+"彩金";
				}

				o_jackpot.transform.FindChild("dlg_jackpot").FindChild("label").GetComponent<UILabel>().text = tempMsg;
				o_jackpot.SetActive(true);
			}
			Invoke("hideReward",5.0f);
		}

		//关闭中奖界面
		void hideReward()
		{
			if(o_jackpot.activeSelf) o_jackpot.SetActive(false);
			o_jackpot.transform.FindChild("dlg_jackpot").FindChild("label").GetComponent<UILabel>().text = "0";
		}

		void OnResultAudio(int inx)
		{
			byte tmpIndex = m_lAudioIndex[inx];
			PlaySound(tmpIndex,_ResultSound);
		}

//--------------------下注控制-----------------------------------------------------

		/// <summary>
		/// 主游戏开始
		/// </summary>
		public void OnStartScene1()
		{
			PlayerInfo playerInfo = GameEngine.Instance.MySelf;
			if( GetAllScore() > playerInfo.Money )
			{
				cnMsgBox("金币不足，下注失败！");
				if(m_bIsAuto) OnAutoBtn();
				o_btn_start.GetComponent<UIButton>().isEnabled = true;
				return;
			}

			if( playerInfo.Money < lMinTableScore)
			{
				cnMsgBox("金币少于游戏最低所需金币，下注失败！");
				if(m_bIsAuto) OnAutoBtn();
				o_btn_start.GetComponent<UIButton>().isEnabled = true;
				return;
			}

			long mJetton = GetAllScore();

			if( mJetton > lUserLimitScore )
			{
				cnMsgBox("押注金额大于最大可押注金额，下注失败！");
				if(m_bIsAuto) OnAutoBtn();
				o_btn_start.GetComponent<UIButton>().isEnabled = true;
				return;
			}

			StopAllCoroutines();
			hideLine();
			hideBoder();

			PlaySound(1, _GameSound);
			_isScorlling = true;
			_isPlayingAni = true;
			m_bIsConnect = false;

			long lParam = mJetton;
			int lines = (int)curChipLines;
			NPacket packet = NPacketPool.GetEnablePacket();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_SCENE1_START );
			packet.AddLong(lParam);
			packet.AddInt(lines);
			GameEngine.Instance.Send (packet);

			StartCoroutine(StartEffect());
			DisableBtnIvk( );	

			Invoke("showAgain",2.0f);
		}

		void showAgain(){
			if(!m_bIsConnect){
				Invoke("showConnect",5.0f);
			}
		}
		void showConnect(){
			if(m_bIsConnect) return;
			o_tips_connect.SetActive(true);
		}
		/// <summary>
		/// 游戏2开始
		/// </summary>
		public void OnStartScene2(int btype, int dtype = 0)
		{
			if(m_lUserWinScore > 0)
			{
				int dParam = dtype;
				int bParam = btype;
				long lParam = (long)m_lUserWinScore;
				NPacket packet = NPacketPool.GetEnablePacket ();
				packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_SCENE2_BUY_TYPE);
				packet.AddInt(dtype);
				packet.AddInt(btype);
				packet.AddLong(lParam);
				GameEngine.Instance.Send (packet);
			}
		}

		/// <summary>
		/// 游戏3开始
		/// </summary>
		public void OnStartScene3()
		{
			m_reConnect = false;
			int  IParam = lBounsGames;
			long lParam = GetAllScore();
			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_SCENE3_START );
			packet.AddInt(IParam);
			packet.AddLong(lParam);
			GameEngine.Instance.Send (packet);

			Invoke("reConnect",4.0f);
		}

		public void reConnect()
		{
			if(!m_reConnect){
				NPacket packet = NPacketPool.GetEnablePacket ();
				packet.CreateHead( MainCmd.MDM_GF_GAME,SubCmd.SUB_C_RECONNECT );
				GameEngine.Instance.Send (packet);
			}
		}

		/// <summary>
		/// 主游戏结束
		/// </summary>
		public void onStopScene1()
		{
			o_btn_stop.GetComponent<UIButton>().isEnabled = false;
			o_btn_start.SetActive(true);
			o_btn_stop.SetActive(false);
			onCloseResult();
		}

		//加线
		public void OnAddLine()
		{
			PlaySound(6, _GameSound);
			curChipLines++;
			if(curChipLines>9)	curChipLines = 1;
			lbl_lines.GetComponent<UILabel>().text = curChipLines.ToString();
			OnUpdateLine();
			SetAllScore();
		}

		//减线
		public void OnSubLine()
		{
			PlaySound(6, _GameSound);
			curChipLines--;
			if(curChipLines<1)  curChipLines = 9;
			lbl_lines.GetComponent<UILabel>().text = curChipLines.ToString();
			OnUpdateLine();
			SetAllScore();
		}

		//加分
		public void OnAddScore()
		{
			PlaySound(7, _GameSound);
			chipIndex++;
			if(chipIndex>= m_lChipCount)	chipIndex = 0;
			m_lCurrentJetton = betScore[chipIndex];
			lbl_betScore.GetComponent<UILabel>().text = formatMoney(m_lCurrentJetton,4);
			SetAllScore();
		}

		//减分
		public void OnSubScore()
		{
			PlaySound(7, _GameSound);
			chipIndex--;
			if(chipIndex < 0)  chipIndex = m_lChipCount-1;
			m_lCurrentJetton = betScore[chipIndex];
			lbl_betScore.GetComponent<UILabel>().text = formatMoney(m_lCurrentJetton,4);
			SetAllScore();
		}

		void OnUpdateLine()
		{
			dlg_lines.SetActive(true);
			for(int i=1; i<10; i++)
			{
				if(o_lines[i]!=null ){
					o_lines[i].SetActive(false);
					if(i <= curChipLines) ShowLine((byte)i);
				}
			}
		}

		void ShowLine( byte i)
		{
			if(o_lines[i]!=null && !o_lines[i].activeSelf)
				o_lines[i].SetActive(true);
		}

		/// <summary>
		/// 收分事件
		/// </summary>
		public void OnGetResult( )
		{
//			Logger.UI.LogWarning("收分");

			hideLine();	

			o_btn_soufen.GetComponent<UIButton>().isEnabled = false;
			o_btn_bibei.GetComponent<UIButton>().isEnabled = false;
			if(!o_dlg_result.activeSelf) o_dlg_result.SetActive(true);
			o_dlg_result.transform.FindChild("lbl_score").GetComponent<UILabel>().text = m_lUserWinScore.ToString();

			if(o_MaryGame!=null) 
			{
				if(o_MaryGame.activeSelf)
				{
					o_MaryGame.SetActive(false);
					Destroy(o_MaryGame);
					o_MaryGame = null;
				}
			}
			if(o_RiskGame!=null) 
			{
				o_RiskGame.SetActive(false);
				Destroy(o_RiskGame);
				o_RiskGame =null;
			}

			CloseBiBei();

			lBounsGames = 0;
			lBGScore = 0;
			resumeBtnIvk();

			o_gold_light.SetActive(true);

			NPacket packet = NPacketPool.GetEnablePacket ();
			packet.CreateHead (MainCmd.MDM_GF_GAME,SubCmd.SUB_C_SCORE);
			GameEngine.Instance.Send (packet);

			Invoke("closeLightEffect",2f);
		}

		void closeLightEffect(){
			if(o_gold_light.activeSelf)
				o_gold_light.SetActive(false);
		}

		//自动模式控制
		void SetAutoState(AutoState state)
		{
			if(state == AutoState.START)
			{
				Invoke("OnChangeStart",2.0f);
			}else if(state == AutoState.GET)
			{
				Invoke("OnChangeGet",2.0f);
			}
		}

		void OnChangeStart(){
			UIAutoCtr._instance.curState = AutoState.START;
		}
		void OnChangeGet(){
			UIAutoCtr._instance.curState = AutoState.GET;
		}

		void CloseBiBei()
		{
			if(o_dlg_result.activeSelf) o_dlg_result.SetActive(false);

			_isPlayingAni = false;

			UpdateUserView();
			if(m_bIsAuto){
				SetAutoState(AutoState.START);
			}

			PlayerInfo playerInfo = GameEngine.Instance.MySelf;
			o_dlg_selfMoney.GetComponent<UIScoreEffect>()._time = 2f;
			long tmpScore = m_lUserWinScore - GetAllScore();

			if(tmpScore>0)
			{
				o_dlg_selfMoney.GetComponent<UIScoreEffect>()._targetScore = playerInfo.Money + tmpScore;
			}else{
				o_dlg_selfMoney.GetComponent<UIScoreEffect>()._score = playerInfo.Money - GetAllScore();
				o_dlg_selfMoney.GetComponent<UIScoreEffect>()._targetScore = playerInfo.Money + tmpScore;
			}

			o_dlg_selfMoney.GetComponent<UIScoreEffect>().Play();
		}

		void SetAllScore(){
			lbl_allScore.GetComponent<UILabel>().text = formatMoney( GetAllScore(), 6 );
		}

		long GetAllScore(){
			return curChipLines*m_lCurrentJetton;
		}

		/// <summary>
		/// 比倍游戏
		/// </summary>
		public void StartRiskGame( )
		{
			if(o_RiskGame==null)
			{
				GameObject riskgame = Instantiate( o_RG_Prefabs, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f) ) )as GameObject;

				riskgame.transform.parent = UIManager.Instance.o_game.transform;
				riskgame.transform.localScale = new Vector3(1f,1f,1f);
				o_RiskGame = riskgame;
			}
			if( o_RiskGame!=null )
			{	
				o_RiskGame.SetActive(true);
				Invoke("DiceAni",0.5f);
			}
		}
		void DiceSound(){
			PlaySound(10,_GameSound);
		}
		void DiceAni(){
			UIBigSmallManger._inatance.PlayFreeAnimation1();
			UIBigSmallManger._inatance.m_lAllBet  =  m_lUserWinScore;
			UIBigSmallManger._inatance.m_lWinScore = 0;
			Invoke("DiceSound",0.5f);
		}

		/// <summary>
		/// 小玛丽游戏
		/// </summary>
		void StartMaryGame()
		{
			if(lBounsGames>0)
			{
				if(!o_MaryGame.activeSelf) o_MaryGame.SetActive(true);
				OnStartScene3();
			}else
			{
				OnGetResult( );
			}
		}


		//自动
		public void OnAutoBtn()
		{
			bool bshow = o_btn_auto.activeSelf;
			if (bshow)
			{
				o_btn_auto.GetComponent<UIButton>().isEnabled = false;
				o_btn_auto.SetActive(false);
				o_btn_disauto.SetActive(true);
				m_bIsAuto = true;
				if( !_isPlayingAni){
					SetAutoState(AutoState.START);
				}
				Invoke("DisAutoBtn",2f);
				o_btn_start.GetComponent<UIButton>().isEnabled = false;
				o_btn_stop.GetComponent<UIButton>().isEnabled = false;
			}else
			{
				o_btn_disauto.GetComponent<UIButton>().isEnabled = false;
				o_btn_auto.SetActive(true);
				o_btn_disauto.SetActive(false);
				m_bIsAuto = false;
				Invoke("SetAutoBtn",2f);
			}
			o_tips_auto.SetActive(m_bIsAuto);
		}
		void SetAutoBtn(){
			o_btn_auto.GetComponent<UIButton>().isEnabled = true;
		}
		void DisAutoBtn(){
			o_btn_disauto.GetComponent<UIButton>().isEnabled = true;
		}

//--------------------动画控制-----------------------------------------------------

		/// <summary>
		/// 播放结果动画
		/// </summary>
		void OnPlayEffect( int index ,  AniType atype)
		{
			GameObject border = o_table[index].transform.FindChild("frame").FindChild("border").gameObject;
			GameObject result = o_table[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table[index].transform.FindChild("result").FindChild("light").gameObject;
			GameObject img = o_table[index].transform.FindChild("img").gameObject;
			border.SetActive(true);

			if(img.activeSelf) img.SetActive(false);
			if(atype == AniType.Result)
			{
				border.GetComponent<UIAnimation>().m_bRepeatTimes = 6;
				border.GetComponent<UIAnimation>().m_fPerFrameTime = 0.22f;
				light.SetActive(false);
				result.SetActive(true);
				result.GetComponent<UIAnimation>().Play();
				
			}else if(atype == AniType.Light)
			{
				border.GetComponent<UIAnimation>().m_bRepeatTimes = 1;
				border.GetComponent<UIAnimation>().m_fPerFrameTime = 0.3f;
				result.SetActive(false);
				light.SetActive(true);
				light.GetComponent<UIAnimation>().Play();
			}
			border.GetComponent<UIAnimation>().Play();
		}
		
		/// <summary>
		/// 设置动画图标
		/// </summary>
		void OnSetResultAni( int index, ResultType rtype)
		{
			GameObject result = o_table[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table[index].transform.FindChild("result").FindChild("light").gameObject;
			GameObject img = o_table[index].transform.FindChild("img").gameObject;
			result.GetComponent<UISprite>().atlas = o_atlas[(int)rtype];
			result.GetComponent<UISprite>().spriteName = "result_"+(int)rtype+"_3_0";
			result.GetComponent<UIAnimation>().m_strTextureName = "result_"+(int)rtype+"_3_";
			light.GetComponent<UISprite>().spriteName = "result_"+(int)rtype+"_2_0";
			light.GetComponent<UIAnimation>().m_strTextureName = "result_"+(int)rtype+"_2_";
			img.GetComponent<UISprite>().spriteName = "result_"+(int)rtype;
		}

		/// <summary>
		/// 暂停滚动动画
		/// </summary>
		void OffResultEffect( int index )
		{
			GameObject img = o_table[index].transform.FindChild("img").gameObject;
			GameObject img1 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg").gameObject;
			GameObject img2 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg2").gameObject;
			GameObject result = o_table[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table[index].transform.FindChild("result").FindChild("light").gameObject;
			Vector3 _oldPos =  new Vector3(0, 25,0);
			Vector3 _newPos = new Vector3(_oldPos.x, _oldPos.y -50,_oldPos.z);

			img.transform.localPosition = _newPos;
			img.SetActive(true);

			float sec = 0;
			if(index%5 == 0){
				sec = 0.3f;
			}else if(index%5 == 1)
			{
				sec = 0.34f;
			}else if(index%5 == 2)
			{
				sec = 0.38f;
			}else if(index%5 == 3)
			{
				sec = 0.42f;
			}else if(index%5 == 4)
			{
				sec = 0.46f;
			}
			if(_isScorlling)
			{
				TweenPosition.Begin(img, sec, _oldPos);
			}else
			{
				img.transform.localPosition = _oldPos;
			}
		}

		void stopScroll()
		{
			for(int index=0; index<15; index++)
			{
			GameObject img = o_table[index].transform.FindChild("img").gameObject;
			GameObject img1 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg").gameObject;
			GameObject img2 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg2").gameObject;
			if(img1.GetComponent<TweenPosition>().enabled || img2.GetComponent<TweenPosition>().enabled)
			{
				img1.GetComponent<UITweenEnd>().Stop();
				img2.GetComponent<UITweenEnd>().Stop();
				img1.GetComponent<TweenPosition>().enabled = false;
				img2.GetComponent<TweenPosition>().enabled = false;	
				img1.SetActive(false);
				img2.SetActive(false);		
			}		
			}
		}

		/// <summary>
		/// 播放滚动动画
		/// </summary>
		void OnResultEffect( int index )
		{
			GameObject img = o_table[index].transform.FindChild("img").gameObject;
			GameObject img1 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg").gameObject;
			GameObject img2 = o_table[index].transform.FindChild("scroll").FindChild("scrollImg2").gameObject;
			img1.GetComponent<TweenPosition>().enabled = true;
			img2.GetComponent<TweenPosition>().enabled = true;
			img1.GetComponent<UITweenEnd>().Play();
			img2.GetComponent<UITweenEnd>().Play();	
			img.SetActive(false);
			img1.SetActive(true);
			img2.SetActive(true);
		}

		void OnEffectStart(int i ,bool start)
		{
			if(start){
				OnResultEffect(i);
			}else
			{
				OffResultEffect(i);
			}
		}

//---------------------------------------------------------------------------------------------------


		#region ##################UI 事件#######################

		void DisableBtnIvk( )
		{	
			o_btn_start.SetActive(false);
			o_btn_stop.SetActive(true);
			o_btn_start.GetComponent<UIButton>().isEnabled = false;
			o_btn_stop.GetComponent<UIButton>().isEnabled = false;

			o_add_score.GetComponent<UIButton>().isEnabled = false;
			o_sub_score.GetComponent<UIButton>().isEnabled = false;
			o_add_line.GetComponent<UIButton>().isEnabled = false;
			o_sub_line.GetComponent<UIButton>().isEnabled = false;
		}

		void resumeBtnIvk()
		{
			if(!m_bIsAuto){
				o_btn_start.GetComponent<UIButton>().isEnabled = true;
				o_btn_stop.GetComponent<UIButton>().isEnabled = true;
			}

			o_add_score.GetComponent<UIButton>().isEnabled = true;
			o_sub_score.GetComponent<UIButton>().isEnabled = true;
			o_add_line.GetComponent<UIButton>().isEnabled = true;
			o_sub_line.GetComponent<UIButton>().isEnabled = true;
			o_btn_start.SetActive(true);
			o_btn_stop.SetActive(false);
		}

		void DisableLines( )
		{	
			for(int i=1; i<10; i++){
				o_lines[i].SetActive(false);
			}
		}
		
		public void SetShowPlayer()
		{
			if(o_showPlayer.GetComponent<UIToggle>().value)
			{
				o_number.SetActive(false);
				o_player.SetActive(true);
				for(int i=1; i<10; i++){
					o_lines[i].transform.FindChild("num1").gameObject.SetActive(false);
					o_lines[i].transform.FindChild("num2").gameObject.SetActive(false);
				}
			}else
			{
				o_player.SetActive(false);
				o_number.SetActive(true);
				for(int i=1; i<10; i++){
					o_lines[i].transform.FindChild("num1").gameObject.SetActive(false);
					o_lines[i].transform.FindChild("num2").gameObject.SetActive(false);
				}
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

		private void ShowResultView()
		{
			ShowResultView(true);
		}
		
		void ShowResultView(bool bshow)
		{
			if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
			if (bshow){
				Invoke("CloseResultView", 8.0f);
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

		//规则
		public void OnBtnRuleIvk()
		{
			if(o_help==null)
			{
				GameObject dlg_help = Instantiate( o_help_Prefabs, Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f) ) )as GameObject;
				dlg_help.transform.parent = UIManager.Instance.o_game.transform;
				dlg_help.transform.localScale = new Vector3(1f,1f,1f);
				o_help = dlg_help;
				UIEventListener.Get(o_help.transform.FindChild("close_btn").gameObject).onClick = EventClick;	
				UIEventListener.Get(o_help.transform.FindChild("scrollArea").gameObject).onClick = EventClick;
			}

			bool bshow = !o_help.active;
			o_help.SetActive(bshow);
			o_help.transform.FindChild("scrollView").GetComponent<UIScrollView>().ResetPosition();
			if (bshow == true)
			{
				_nInfoTickCount = Environment.TickCount;
			}else
			{
				Destroy(o_help);
			}
		} 

		//退出按钮
		public void OnBtnBackIvk()
		{
			OnConfirmBackOKIvk();
		}

		//清除UI
		void OnClearInfoIvk()
		{
			ClearAllInfo();
		}

		#endregion

		void ClearAllInfo()
		{
		}

		public void PlaySound(int sound , AudioClip[] _Audio)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(_Audio[sound], fvol, 1);
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
//				_nQuitDelay = System.Environment.TickCount;
				CancelInvoke();			
			}
			catch (Exception ex)
			{
			}
		}

		
		string formatMoney(long money, int index)
		{
			string tempMoney = money.ToString();
			if(money<10000)return tempMoney;

			long tempScore = 1;
			for(int i=0; i<index; i++)
				tempScore *= 10;
			if(money>=tempScore) 
			{
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}
				
		public void closeLoading()
		{
			if(UIManager.Instance.o_loading!=null)
			{
				UIManager.Instance.o_loading.SetActive(false);
				Destroy(UIManager.Instance.o_loading);
				UIManager.Instance.o_loading =null;
			}
		}

		//定时处理事件
		void OnTimerEnd()
		{
			try
			{
			}
			catch (Exception ex)
			{
			}
		}

	}

}