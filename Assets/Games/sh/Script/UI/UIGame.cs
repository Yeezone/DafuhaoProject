using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;
using Shared;
using System;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.SH
{
    #region ##################结构定义#######################
    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_READY = 1,
        TIMER_CHIP = 2

    };
    public enum SoundType
    {
        READY = 0,
        START = 1,
        SENDCARD = 2,
        CHIP = 3,
        WIN = 4,
        LOSE = 5,
        CLOCK = 6
    };
    public enum GameSoundType
    {
        PASS = 0,
        FOLLOW = 1,
        ADD = 2,
        GIVEUP = 3,
        SUOHA = 4,
        KAIPAI = 5
    };
   
    public class SendCardItem
    {
        public byte Chair;
        public byte CardData;
    };

	public enum GamePlatform	//游戏平台
	{
		SH_ForPC,
		SH_ForMobile
	}


    #endregion


    public class UIGame : MonoBehaviour
    {

        #region ##################变量定义#######################

        //通用控件
        // GameObject      o_dlg_tips          = null;
        GameObject o_speak_timer = null;
        GameObject o_btn_speak = null;
        GameObject o_btn_speak_count = null;
		GameObject o_dlg_allChips = null;

		public GamePlatform curGamePlatform; 	//当前游戏平台

		GameObject[] o_clock = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_clock = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_option = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_chat = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_chip = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_c_desc = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_cards = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_flag = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_player_curr_desc = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_curr_chip = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_player_type = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_player_face = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER];
		GameObject[] o_infos = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER];





        GameObject o_result = null;
        GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_exp = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_type = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_win = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_bean = new GameObject[GameLogic.GAME_PLAYER];

        GameObject o_ready_buttons = null;
        GameObject o_btn_ready = null;
        GameObject o_btn_quit = null;





        //游戏特殊
        GameObject o_option_buttons = null;
        GameObject o_btn_pass = null;
        GameObject o_btn_followchip = null;
        GameObject o_btn_giveup = null;
        GameObject o_btn_showhand = null;
        GameObject o_btn_chip_1 = null;
        GameObject o_btn_chip_2 = null;
        GameObject o_btn_chip_3 = null;



        GameObject o_dlg_chips = null;

        GameObject o_curr_time = null;

        GameObject o_room_cell = null;

        GameObject o_rules = null;


        //通用数据
        static bool _bStart = false;
        static TimerType _bTimerType = TimerType.TIMER_NULL;


        //进入分数
        static int _lBaseScore = 0;
        //几张梭
        static int _lAllowSuoCount = 0;
        static int _lMaxScore = 0;
        static int _lCellScore = 0;
        static int _lTurnMaxScore = 0;
        static int _lTurnLessScore = 0;
        //桌面总注
        static int _lTotalScore = 0;
        //加注标志
		static bool _bAddScore = false;
        //梭哈标志
        static bool _bShowHand = false;
		//灰牌标志
		static bool grayCard = false;
        //下注数目
        static int[] _lTableScore = new int[GameLogic.GAME_PLAYER * 2];
        //游戏状态
        static byte[] _bPlayStatus = new byte[GameLogic.GAME_PLAYER];
        //
        static byte _bBackCardData = 0;
        //
        static ArrayList _SendCardList = new ArrayList();
        static byte _bSendCardCount = 0;
        static float _nSendCardDelay = 0;


        static byte[][] _bHandCardData = new byte[GameLogic.GAME_PLAYER][];
        static byte[] _bHandCardCount = new byte[GameLogic.GAME_PLAYER];
        static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER];
        static byte _bCurrentUser = GameLogic.NULL_CHAIR;
        static byte _bBankerUser = GameLogic.NULL_CHAIR;
        static int _nInfoTickCount = 0;

        static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER];
        static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER];
        static byte[] _nEndUserCardType = new byte[GameLogic.GAME_PLAYER];

        static int _nQuitDelay = 0;
        static bool _bReqQuit = false;
        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[20];
        public AudioClip[] _ManSound = new AudioClip[20];

        public static GameObject btn_chat_disabled = null;
        public static GameObject btn_chat = null;
        #endregion


		public void Update()
		{
			/*if(curGamePlatform == GamePlatform.SH_ForPC)
			{
				for(int i = 0;i<GameLogic.GAME_PLAYER;i++)
				{
					byte bViewId = ChairToView((byte)i);
					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((uint)i);
					if (userdata != null)
					{	
						ShowUserInfo(bViewId, true);
					}
				}
			}*/
		}

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
                //通用
                o_speak_timer = GameObject.Find("scene_game/dlg_speak_timer");
                o_btn_speak = GameObject.Find("scene_game/dlg_title_bar/btn_speak");
                o_btn_speak_count = GameObject.Find("scene_game/dlg_title_bar/btn_speak/lbl_count");
				o_dlg_allChips = GameObject.Find ("scene_game/dlg_allChips/lbl_curr_chips");

				btn_chat_disabled=GameObject.Find ("scene_game/dlg_title_bar/btn_voice/disabled");
				btn_chat=GameObject.Find ("scene_game/dlg_title_bar/btn_voice");

				UIMsgBox.Instance.platform = curGamePlatform;

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
					o_clock[i] = GameObject.Find("scene_game/dlg_clock_" + i.ToString());
                    o_player_clock[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_clock_bar");
                    o_player_option[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_option");
                    o_player_chat[i] = GameObject.Find("scene_game/dlg_chat_msg_" + i.ToString());
                    o_player_face[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_user_face");
                    o_player_flag[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_flag");

                    o_player_chip[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_chips");
                    o_player_c_desc[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_chips");

                    o_player_curr_chip[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_curr_chips");
                    o_player_curr_desc[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_curr_chips");

                    o_player_nick[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_nick");
                    o_player_type[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_type");
                    //o_user_speak[i] = GameObject.Find("scene_game/ctr_speak_" + i.ToString());

                    o_info[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString());
                    o_info_nick[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_nick");
                    o_info_lvl[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lvl");
                    o_info_id[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_id");
                    o_info_score[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_score");
                    o_info_win[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_win");
                    o_info_lose[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lose");
                    o_info_run[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_run");

                    o_player_cards[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_hand_cards");

                    o_result_score[i] = GameObject.Find("scene_game/dlg_result/lbl_score_" + i.ToString());
                    o_result_exp[i] = GameObject.Find("scene_game/dlg_result/lbl_bs_" + i.ToString());
                    o_result_type[i] = GameObject.Find("scene_game/dlg_result/sp_type_" + i.ToString());
                    o_result_win[i] = GameObject.Find("scene_game/dlg_result/sp_win_" + i.ToString());
                    o_result_nick[i] = GameObject.Find("scene_game/dlg_result/lbl_user_" + i.ToString());
                    o_result_bean[i] = GameObject.Find("scene_game/dlg_result/sp_bean_" + i.ToString());


                }

                //准备
                o_ready_buttons = GameObject.Find("scene_game/dlg_ready_buttons");
                o_btn_ready = GameObject.Find("scene_game/dlg_ready_buttons/btn_ready");
                o_btn_quit = GameObject.Find("scene_game/dlg_ready_buttons/btn_quit");
                o_result = GameObject.Find("scene_game/dlg_result");



                //游戏
                o_option_buttons = GameObject.Find("scene_game/dlg_option_buttons");
                o_btn_pass = GameObject.Find("scene_game/dlg_option_buttons/btn_pass");
                o_btn_followchip = GameObject.Find("scene_game/dlg_option_buttons/btn_follow");
                o_btn_giveup = GameObject.Find("scene_game/dlg_option_buttons/btn_giveup");
                o_btn_showhand = GameObject.Find("scene_game/dlg_option_buttons/btn_showhand");
                o_btn_chip_1 = GameObject.Find("scene_game/dlg_option_buttons/btn_chip_1");
                o_btn_chip_2 = GameObject.Find("scene_game/dlg_option_buttons/btn_chip_2");
                o_btn_chip_3 = GameObject.Find("scene_game/dlg_option_buttons/btn_chip_3");



                o_dlg_chips = GameObject.Find("scene_game/ctr_chips");

                //o_room_cell = GameObject.Find("scene_game/dlg_bottom_bar/lbl_cell");
                o_curr_time = GameObject.Find("scene_game/dlg_title_bar/lbl_curr_time");

                o_rules = GameObject.Find("scene_game/dlg_rules");



            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "Awake:" + ex.Message);
            }
        }
        void InitGameView()
        {
            //Data
            _bStart = false;
            _bTimerType = TimerType.TIMER_READY;

            //
            _lMaxScore = 0;
            _lCellScore = 0;
            _lTurnMaxScore = 0;
            _lTurnLessScore = 0;

            //桌面总注
            _lTotalScore = 0;
            //加注标志
            _bAddScore = false;
            //梭哈标志
			_bShowHand = false;
			//灰牌标志
			grayCard = false;

            _bCurrentUser = GameLogic.NULL_CHAIR;

            _bBankerUser = GameLogic.NULL_CHAIR;

            _nInfoTickCount = Environment.TickCount;

            Array.Clear(_bUserTrustee, 0, 3);

            _nQuitDelay = 0;
            _bReqQuit = false;
            _nInfoTickCount = Environment.TickCount;
            //UI

            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
				o_clock[i].SetActive(false);
                o_player_clock[i].SetActive(false);
                o_player_option[i].SetActive(false);
                o_player_chip[i].SetActive(false);
                o_player_c_desc[i].SetActive(false);
                o_player_curr_desc[i].SetActive(false);
                o_player_curr_chip[i].SetActive(false);

                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_type[i].SetActive(false);
                o_player_flag[i].SetActive(false);

                o_player_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
                o_player_nick[i].GetComponent<UILabel>().text = "";


                o_info[i].SetActive(false);
                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";

                _bHandCardData[i] = new byte[5];
                _bPlayStatus[i] = (byte)PlayerState.NULL;
                Array.Clear(_bHandCardData[i], 0, 5);
                Array.Clear(_bHandCardCount, 0, 5);

                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;

            }

            Array.Clear(_lTableScore, 0, GameLogic.GAME_PLAYER * 2);
            _SendCardList.Clear();
            _bSendCardCount = 0;

            o_ready_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);

            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            o_rules.SetActive(false);


        }
        void ResetGameView()
        {
            _bTimerType = TimerType.TIMER_READY;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _nInfoTickCount = Environment.TickCount;

            //
            _lMaxScore = 0;
            _lCellScore = 0;
            _lTurnMaxScore = 0;
            _lTurnLessScore = 0;

            //桌面总注
            _lTotalScore = 0;
            //加注标志
            _bAddScore = false;
            //梭哈标志
			_bShowHand = false;
			//灰牌标志
			grayCard = false;


            Array.Clear(_bUserTrustee, 0, 5);
            //UI

            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
				o_clock[i].SetActive(false);
                o_player_clock[i].SetActive(false);
                o_player_option[i].SetActive(false);
                o_player_flag[i].SetActive(false);

                o_player_curr_desc[i].SetActive(false);
                o_player_curr_chip[i].SetActive(false);

                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_type[i].SetActive(false);



                o_player_cards[i].GetComponent<UICardControl>().ClearCards();


//                o_info[i].SetActive(false);
//                o_info_nick[i].GetComponent<UILabel>().text = "";
//                o_info_lvl[i].GetComponent<UILabel>().text = "";
//                o_info_id[i].GetComponent<UILabel>().text = "";
//                o_info_score[i].GetComponent<UILabel>().text = "";
//                o_info_win[i].GetComponent<UILabel>().text = "";
//                o_info_lose[i].GetComponent<UILabel>().text = "";
//                o_info_run[i].GetComponent<UILabel>().text = "";

//				if(curGamePlatform == GamePlatform.SH_ForPC)
//				{
//					byte bViewId = ChairToView((byte)i);
//					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((uint)i);
//					if (userdata != null)
//					{	
//						Debug.LogWarning("bViewId :"+bViewId );
//						ShowUserInfo(bViewId, true);
//					}
//				}

                _bHandCardData[i] = new byte[5];
                Array.Clear(_bHandCardData[i], 0, 5);
                Array.Clear(_bHandCardCount, 0, 5);

                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;
            }

            Array.Clear(_lTableScore, 0, GameLogic.GAME_PLAYER * 2);
            _SendCardList.Clear();
            _bSendCardCount = 0;
            o_ready_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);

            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            o_rules.SetActive(false);

            Resources.UnloadUnusedAssets();

            GC.Collect();

			//声音按钮
			btn_chat.GetComponent<UIButton>().isEnabled=true;

        }

        #endregion


        #region ##################引擎调用#######################

        void Start()
        {
            try
            {
                InitGameView();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
			GameObject.Find("Panel").GetComponent<AudioSource>().volume=0.1f;
			GameObject.Find ("Panel").GetComponent<AudioSource>().Play();
			NGUITools.soundVolume=0.5f;
        }
        void FixedUpdate()
        {
            o_curr_time.GetComponent<UILabel>().text = System.DateTime.Now.ToString("hh:mm:ss");

            //o_room_cell.GetComponent<UILabel>().text = _lBaseScore.ToString();



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

            if (_bSendCardCount > 0 && _SendCardList.Count > 0)
            {
                if ((Time.realtimeSinceStartup - _nSendCardDelay) > 0.3f)
                {
                    _nSendCardDelay = Time.realtimeSinceStartup;

                    SendCardItem item = (SendCardItem)_SendCardList[0];
                    _bSendCardCount--;

                    _bHandCardData[item.Chair][_bHandCardCount[item.Chair]++] = item.CardData;

                    SendHandCard(item.Chair, new byte[1] { item.CardData }, 1);

                    PlayGameSound(SoundType.SENDCARD);
                    _SendCardList.Remove(item);
                    item = null;
                    if (_bSendCardCount <= 0)
                    {
                        Invoke("SendCardFinish", 1.0f);
                    }
                }
            }

        }
//        void Update()
//        {
//            /*if (GameEngine.Instance.MyUser.IsDisconnectFromServer && _bStart == true)
//            {
//                CancelInvoke();
//
//                _bStart = false;
//
//                PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MyUser.ServerUsed.ServerID);
//
//                UIMsgBox.Instance.Show(true, "您掉线了,网络不给力哦!");
//
//                Invoke("GoLogin", 3.0f);
//            }*/
//        }

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
                        OnGameOptionResp(packet);
                        break;
                    }
                case SubCmd.SUB_GF_SCENE:
                    {
                        OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
                case SubCmd.SUB_GF_USER_READY:
                    {
                        OnUserReadyResp(packet);
                        break;
                    }
                case SubCmd.SUB_GF_MESSAGE:
                    {
                        OnGameMessageResp(packet);
                        break;
                    }
                case SubCmd.SUB_GF_USER_CHAT:
                    {
                        OnUserChatResp(packet);
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
                        OnUserReadyResp(null);

                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_PLAY:
                    {

                        UpdateUserView();

                        ShowInfoBar();
                        break;
                    }
                case TableEvents.USER_OFFLINE:
                    {

                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_LOOKON:
                    {

                        break;
                    }
                case TableEvents.USER_SCORE:
                    {

                        UpdateUserView();

                        ShowInfoBar();

                        //Invoke("CheckScoreLimit",5.0f);

                        //CheckScoreLimit();

                        //金币限制检测
                        /*
                        if(GameEngine.Instance.MyUser.Self.lScore <_lBaseScore)
                        {
                             UIMsgBox.Instance.Show(true,"您的乐豆不足,不能继续游戏!");
                             Invoke("OnConfirmBackOKIvk",1.0f);
                             _bStart = false;
                             return;
                        }
                        */
                        break;
                    }
                case TableEvents.GAME_ENTER:
                    {
                        InitGameView();

                        _bStart = true;
                        UpdateUserView();
                        ShowInfoBar();
                        UIWaiting.Instance.Show(false);

                        break;
                    }
                case TableEvents.WAIT_DISTRIBUTE:
                    {
                        UIWaiting.Instance.CallBack = new WaitingCancelCall(BreakGame);
                        UIWaiting.Instance.Show(true);
                        break;
                    }
                case TableEvents.GAME_START:
                    {
                        //
                        for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                        {
                            if (GameEngine.Instance.GetTableUserItem(i) != null)
                            {
                                SetUserReady(i, true);
                            }
                        }

                        PlayGameSound(SoundType.READY);

                        Invoke("ClearUserReady", 0.6f);
                        break;
                    }
                case TableEvents.GAME_FINISH:
                    {
                        break;
                    }
				case TableEvents.USER_NULL:
					{
						Invoke("OnConfirmBackOKIvk",5.0f);
						break;
					}
                case TableEvents.GAME_LOST:
                    {
                        UIMsgBox.Instance.Show(true, GameMsg.MSG_CM_008);
                        Invoke("OnConfirmBackOKIvk", 5.0f);
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
                UIMsgBox.Instance.Show(true, "OnGameOptionResp:" + ex.Message);
            }
        }

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {

            switch (bGameStatus)
            {
                case (byte)GameLogic.GS_WK_FREE:
                    {
                        SwitchFreeSceneView(packet);
                        break;
                    }
                case (byte)GameLogic.GS_WK_PLAYING:
                    {
                        SwitchPlaySceneView(packet);
                        break;
                    }
            }
        }

        //用户准备消息处理函数
        void OnUserReadyResp(NPacket packet)
        {
            PlayGameSound(SoundType.READY);
            return;
        }

        //用户聊天消息处理函数
        void OnUserChatResp(NPacket packet)
        {

            try
            {
                packet.BeginRead();
                ushort wChatLen = packet.GetUShort();
                uint wEmotion = packet.GetUInt();
                uint wSendUserID = packet.GetUInt();
                uint wTargetUserID = packet.GetUInt();
                string strMsg = packet.GetString(wChatLen);

                byte bchair = GameEngine.Instance.UserIdToChairId(wSendUserID);
                if (wEmotion == 255)
                {

                    ShowUserSpeak(wSendUserID);
                }
                else
                {
                    ShowUserChat(bchair, strMsg);
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "OnUserChatResp:" + ex.Message);
            }
        }

        #endregion


        #region ##################游戏消息#######################

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            if (_bReqQuit == true) return;

            //扎金花 游戏状态
            switch (subcmd)
            {

                case SubCmd.SUB_S_GAME_START:
                    {
                        //
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GIVE_UP:
                    {
                        //消息处理
                        OnUserGiveUpResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:
                    {
                        //
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_ADD_SCORE:
                    {
                        //消息处理
                        OnUserAddScoreResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_SEND_CARD:
                    {
                        //消息处理
                        OnUserSendCardResp(packet);
                        break;
                    }

            }
        }
        //游戏消息处理函数
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
                    ShowNotice(strMsg);
                }

                if ((wType & (ushort)MsgType.MT_EJECT) != 0)
                {
                    UIMsgBox.Instance.Show(true, strMsg);
                }

                if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
                    (wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
                {
                    UIMsgBox.Instance.Show(true, strMsg);

                    Invoke("OnConfirmBackOKIvk", 2.0f);
                    _bStart = false;
                }

                if ((wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
                {
                    UIMsgBox.Instance.Show(true, strMsg);

                    Invoke("OnConfirmBackOKIvk", 2.0f);
                    _bStart = false;
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "OnGameMessageResp:" + ex.Message);
            }
        }
        //游戏开始
        void OnGameStartResp(NPacket packet)
        {
            /*
             LONG    lMaxScore;                          //最大下注
             LONG    lCellScore;                         //单元下注
             LONG    lTurnMaxScore;                      //最大下注
             LONG    lTurnLessScore;                     //最小下注

             //用户信息
             WORD    wCurrentUser;                       //当前玩家

             //扑克数据
             BYTE    cbObscureCard;                      //底牌扑克
             BYTE    cbCardData[GAME_PLAYER];            //用户扑克
            */

            try
            {

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
                packet.BeginRead();
                _lMaxScore = packet.GetInt();
                _lCellScore = packet.GetInt();
                _lTurnMaxScore = packet.GetInt();
                _lTurnLessScore = packet.GetInt();


                _bCurrentUser = (byte)packet.GetUShort();
                _bBackCardData = packet.GetByte();
                byte[] bCardData = new byte[GameLogic.GAME_PLAYER];
                packet.GetBytes(ref bCardData, GameLogic.GAME_PLAYER);

                _bAddScore = false;

                //设置变量
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //获取用户
                    PlayerInfo pUserData = GameEngine.Instance.EnumTablePlayer(i);
                    if (pUserData == null)
                    {
                        _bPlayStatus[i] = (byte)PlayerState.NULL;
                    }
                    else
                    {
                        //计算锅底和总注
                        _bPlayStatus[i] = (byte)PlayerState.PLAY;

                        _lTableScore[2 * i + 1] = _lCellScore;

                        _bHandCardData[i] = new byte[5] { 255, bCardData[i], 0, 0, 0 };


                        //下锅底
                        AppendChips(i, _lCellScore, false);

                    }


                }
                //没有抢庄所以游戏开始将准备按钮关闭
                o_ready_buttons.SetActive(false);

                ShowCurrChips();

                //播放声音
                PlayGameSound(SoundType.START);

                //发牌
                _SendCardList.Clear();
                _bSendCardCount = 0;


                for (byte bIndex = 0; bIndex < 2; bIndex++)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                    {
                        if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                        {

                            SendCardItem item = new SendCardItem();
                            item.Chair = i;
                            item.CardData = _bHandCardData[i][bIndex];

                            _SendCardList.Add(item);
                            _bSendCardCount++;
                            _nSendCardDelay = 0;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        void SendCardFinish()
        {

            if (GetSelfChair() == _bCurrentUser)
            {
                ShowOptionButtons();
            }
            SetUserClock(_bCurrentUser, 20, TimerType.TIMER_CHIP);
            _SendCardList.Clear();
            _bSendCardCount = 0;
        }
        //用户放弃
        void OnUserGiveUpResp(NPacket packet)
        {
            try
            {
                //解析封包
                packet.BeginRead();
                byte bGiveUpUser = (byte)packet.GetUShort();

                if (GetSelfChair() != bGiveUpUser)
                {
                    //
                    DisableHandCard(bGiveUpUser);
                    //播放声音
                    PlayUserSound(GameSoundType.GIVEUP, GameEngine.Instance.GetTableUserItem(bGiveUpUser).Gender);

                }

                //删除定时器
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //设置变量
                _bPlayStatus[bGiveUpUser] = (byte)PlayerState.GIVEUP;
                _nEndUserScore[bGiveUpUser] = (_lTableScore[bGiveUpUser * 2 + 1] + _lTableScore[bGiveUpUser * 2]) * (-1);

                ShowCurrChips();

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        //下注
        void OnUserAddScoreResp(NPacket packet)
        {

            /*
            WORD                                wCurrentUser;                       //当前用户
            WORD                                wAddScoreUser;                      //加注用户
            LONG                                lAddScoreCount;                     //加注数目
            LONG                                lTurnLessScore;                     //最少加注
            */

            packet.BeginRead();
            _bCurrentUser = (byte)packet.GetUShort();
            byte bAddScoreUser = (byte)packet.GetUShort();
            int lAddScore = packet.GetInt();
            int lTurnLessScore = packet.GetInt();

            Debug.Log("###################OnUserAddScoreResp########################");
            Debug.Log("add--lTurnLessScore:" + lTurnLessScore.ToString());
            Debug.Log("add--lAddScore:" + lAddScore.ToString());

            //梭哈判断
            if (_bShowHand == false)
            {
                //获取用户
                PlayerInfo userdata = GameEngine.Instance.GetTableUserItem(bAddScoreUser);
                if (userdata != null)
                {
                    /*
                    if(lAddScore>10*_lCellScore)
                    {
                        _bShowHand = true;
                    }
                    */
                    _bShowHand = (_lMaxScore == (_lTableScore[bAddScoreUser * 2 + 1] + lAddScore));
                }
            }


            //加注处理
            if (bAddScoreUser != GetSelfChair() && _bPlayStatus[bAddScoreUser] == (byte)PlayerState.PLAY)
            {

                //
                PlayerInfo userdata = GameEngine.Instance.GetTableUserItem(bAddScoreUser);

                long lTableScore = _lTableScore[bAddScoreUser * 2 + 1];
                long lTurnAddScore = _lTableScore[bAddScoreUser * 2];
                long lShowHandScore = _lMaxScore - lTableScore;


                byte bViewID = ChairToView(bAddScoreUser);

                //播放声音
                if ((lAddScore - lTurnAddScore) == 0)
                {
                    o_player_option[bViewID].SetActive(true);
                    o_player_option[bViewID].GetComponent<UISprite>().spriteName = "desc_pass";
                    PlayUserSound(GameSoundType.PASS, userdata.Gender);

                }

                //else if (lAddScore >10*_lCellScore)
                else if (lAddScore == lShowHandScore)
                {
                    o_player_option[bViewID].SetActive(true);
                    o_player_option[bViewID].GetComponent<UISprite>().spriteName = "desc_suo";
                    PlayUserSound(GameSoundType.SUOHA, userdata.Gender);


                    //总注控件
                    AppendChips(bAddScoreUser, lAddScore, true);
                }
                else if ((lAddScore + lTableScore) == _lTurnLessScore)
                {
                    o_player_option[bViewID].SetActive(true);
                    o_player_option[bViewID].GetComponent<UISprite>().spriteName = "desc_follow";
                    PlayUserSound(GameSoundType.FOLLOW, userdata.Gender);
                    //总注控件
                    AppendChips(bAddScoreUser, lAddScore, true);
                }
                else
                {
                    o_player_option[bViewID].SetActive(true);
                    o_player_option[bViewID].GetComponent<UISprite>().spriteName = "desc_add";
                    PlayUserSound(GameSoundType.ADD, userdata.Gender);
                    //总注控件
                    AppendChips(bAddScoreUser, lAddScore, true);
                }

            }


            _lTurnLessScore = lTurnLessScore;
            _lTableScore[bAddScoreUser * 2] = lAddScore;

            ShowCurrChips();


            if (_bCurrentUser == GetSelfChair())
            {
                ShowOptionButtons();
            }

            if (_bCurrentUser > GameLogic.GAME_PLAYER)
            {
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            else
            {
                SetUserClock((byte)_bCurrentUser, 20, TimerType.TIMER_CHIP);
            }
        }
        //
        void OnUserSendCardResp(NPacket packet)
        {
            try
            {
                //解析封包
                packet.BeginRead();
                _bCurrentUser = (byte)packet.GetUShort();
                byte bLastMostUser = (byte)packet.GetUShort();

				_bAddScore = false;
                _lTurnMaxScore = packet.GetInt();
                _lTurnLessScore = packet.GetInt();

                byte bSendCount = packet.GetByte();
                byte[][] bCardData = new byte[GameLogic.GAME_PLAYER][];
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    bCardData[i] = new byte[3];
                    packet.GetBytes(ref bCardData[i], 3);
                }

                //累计金币
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _lTableScore[i * 2 + 1] += _lTableScore[i * 2];
                    _lTableScore[i * 2] = 0;
                }

                ShowCurrChips();
                //发牌
                _SendCardList.Clear();
                _bSendCardCount = 0;


                //派发扑克,从上次最大玩家开始发起
                for (byte i = 0; i < bSendCount; i++)
                {
                    for (byte j = 0; j < GameLogic.GAME_PLAYER; j++)
                    {
                        byte wChairId = (byte)((bLastMostUser + j) % GameLogic.GAME_PLAYER);
                        if (_bPlayStatus[wChairId] == (byte)PlayerState.PLAY)
                        {
                            byte bCardIndex = (byte)(_bHandCardCount[wChairId] + i);
                            _bHandCardData[wChairId][bCardIndex] = bCardData[wChairId][i];

                            SendCardItem item = new SendCardItem();
                            item.Chair = wChairId;
                            item.CardData = bCardData[wChairId][i];
                            _bSendCardCount++;
                            _SendCardList.Add(item);

                        }
                    }
                }

                _nSendCardDelay = 0;

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_option[i].GetComponent<UISprite>().spriteName = "blank";
                }

            }
            catch (Exception ex)
            {

                UIMsgBox.Instance.Show(true, "OnUserSendCardResp:" + ex.Message);
            }
        }
        //游戏结束
        void OnGameEndResp(NPacket packet)
        {
            /*
             LONG                                lGameTax[GAME_PLAYER];              //游戏税收
             LONG                                lGameScore[GAME_PLAYER];            //游戏得分
             LONG                                lExp[GAME_PLAYER];                  //经验
             BYTE                                cbCardData[GAME_PLAYER][5];         //用户扑克

            */

            try
            {

                if (_bStart == false)
                {
                    return;
                }

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

                packet.BeginRead();
                int lGameTax = packet.GetInt();
                lGameTax = packet.GetInt();
                lGameTax = packet.GetInt();
                lGameTax = packet.GetInt();
                lGameTax = packet.GetInt();

                //游戏得分

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //游戏得分
                    int nScore = packet.GetInt();

                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                    {
                        _nEndUserScore[i] = nScore;
                        if (_nEndUserScore[i] > 0)
                        {
                            _bBankerUser = (byte)i;
                        }
                    }
                }
                //经验
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //经验得分
                    _nEndUserExp[i] = packet.GetInt();
                    if (_nEndUserExp[i] > 1000 || _nEndUserExp[i] < 0)
                    {
                        _nEndUserExp[i] = 0;
                    }
                }

                //用户扑克
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bHandCardData[i][0] = packet.GetByte();

                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY && _bHandCardCount[i] == 5)
                    {
                        OpenHandCardData(i, _bHandCardData[i], 5);
						SetCardType(i,true);
                    }

                }


                ushort wMeChair = GetSelfChair();

                _bAddScore = false;
				_bShowHand = false;


                WinChips(_bBankerUser);

                UpdateUserView();

                PlayGameSound(SoundType.WIN);

                o_option_buttons.SetActive(false);

                //

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    PlayerInfo ud = GameEngine.Instance.GetTableUserItem(i);
					o_result_type[i].GetComponent<UISprite>().spriteName = "blank";
                    if (ud != null)
                    {
                        byte bViewId = ChairToView(i);
                        o_result_score[bViewId].GetComponent<UILabel>().text = _nEndUserScore[i].ToString();
                        o_result_exp[bViewId].GetComponent<UILabel>().text = _nEndUserExp[i].ToString();
                        if (_nEndUserScore[i] > 0)
                        {
                            o_result_win[bViewId].GetComponent<UISprite>().spriteName = "win";
                        }
                        else
                        {
                            o_result_win[bViewId].GetComponent<UISprite>().spriteName = "lose";
                        }
						//string str = GetCardTypeTex(GameLogic.GetCardType(_bHandCardData[i], 5));
                        o_result_type[bViewId].GetComponent<UISprite>().spriteName = "blank";

                        o_result_nick[bViewId].GetComponent<UILabel>().text = ud.NickName;
//                        o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "gamemoney";
						o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "ico_gold";
                    }
                    else
                    {
                        byte bViewId = ChairToView(i);
                        o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_nick[bViewId].GetComponent<UILabel>().text = "";
                        o_result_win[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_score[bViewId].GetComponent<UILabel>().text = "";
                        o_result_exp[bViewId].GetComponent<UILabel>().text = "";
                        o_result_type[bViewId].GetComponent<UISprite>().spriteName = "";
						//o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "";
                    }
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    for (int k = 0; k < (int)(GameLogic.GAME_PLAYER - i) - 1; k++)
                    {
                        byte bViewId = ChairToView(i);
                        if (o_result_score[k].GetComponent<UILabel>().text == "")
                        {
                            string temp_score = o_result_score[k].GetComponent<UILabel>().text;
                            o_result_score[k].GetComponent<UILabel>().text = o_result_score[k + 1].GetComponent<UILabel>().text;
                            o_result_score[k + 1].GetComponent<UILabel>().text = temp_score;

                            string temp_exp = o_result_exp[k].GetComponent<UILabel>().text;
                            o_result_exp[k].GetComponent<UILabel>().text = o_result_exp[k + 1].GetComponent<UILabel>().text;
                            o_result_exp[k + 1].GetComponent<UILabel>().text = temp_exp;

                            string temp_win = o_result_win[k].GetComponent<UISprite>().spriteName;
                            o_result_win[k].GetComponent<UISprite>().spriteName = o_result_win[k + 1].GetComponent<UISprite>().spriteName;
                            o_result_win[k + 1].GetComponent<UISprite>().spriteName = temp_win;

                            string temp_type = o_result_type[k].GetComponent<UISprite>().spriteName;
                            o_result_type[k].GetComponent<UISprite>().spriteName = o_result_type[k + 1].GetComponent<UISprite>().spriteName;
                            o_result_type[k + 1].GetComponent<UISprite>().spriteName = temp_type;

                            string temp_nick = o_result_nick[k].GetComponent<UILabel>().text;
                            o_result_nick[k].GetComponent<UILabel>().text = o_result_nick[k + 1].GetComponent<UILabel>().text;
                            o_result_nick[k + 1].GetComponent<UILabel>().text = temp_nick;

                            string temp_bean = o_result_bean[k].GetComponent<UISprite>().spriteName;
                            o_result_bean[k].GetComponent<UISprite>().spriteName = o_result_bean[k + 1].GetComponent<UISprite>().spriteName;
                            o_result_bean[k + 1].GetComponent<UISprite>().spriteName = temp_bean;
                        }
                    }
                }

				Invoke("ShowResultView", 2);
			
				

                //GameEngine.Instance.MySelf.TEvent.AddTimeTok(TimeEvents.REG_GAME_END,2000,packet);


            }
            catch (Exception ex)
            {
                Debug.Log("##############ex#################");
                Debug.Log(ex.Source);
                Debug.Log(ex.StackTrace);

                UIMsgBox.Instance.Show(true, "OnGameEndResp:" + ex.Message);
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

                _lBaseScore = packet.GetInt();
                _lAllowSuoCount = packet.GetInt();

                _bStart = true;

                /*if (GameEngine.Instance.PrivateOwer)
                {
                    Invoke("OnBtnReadyIvk", 1.0f);
                    GameEngine.Instance.PrivateOwer = false;
                }
                else
                {
                    GameEngine.Instance.PrivateOwer = false;
                }*/
                SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY);

                o_ready_buttons.SetActive(true);

                UpdateUserView();

                o_option_buttons.SetActive(false);

                //PlayerPrefs.SetInt("UsedServ", 0);

                Debug.Log("_lBaseScore:" + _lBaseScore.ToString());
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SwitchFreeSceneView:" + ex.Message);
            }
        }
        //游戏场景处理函数
        void SwitchPlaySceneView(NPacket packet)
        {
            try
            {

                InitGameView();

                byte wMeChairID = GetSelfChair();

                packet.BeginRead();

                //
                _lBaseScore = packet.GetInt();
                _lAllowSuoCount = packet.GetInt();
                //
                _bShowHand = packet.GetBool();
                _bAddScore = packet.GetBool();

                //状态信息
                _lMaxScore = packet.GetInt();
                _lCellScore = packet.GetInt();
                _lTurnMaxScore = packet.GetInt();
                _lTurnLessScore = packet.GetInt();

                _bCurrentUser = (byte)packet.GetUShort();




                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    byte bStatus = packet.GetByte();

                    if (bStatus == 1)
                    {
                        _bPlayStatus[i] = (byte)PlayerState.PLAY;
                    }
                    else
                    {
                        if (GameEngine.Instance.GetTableUserItem(i) == null)
                        {
                            _bPlayStatus[i] = (byte)PlayerState.NULL;
                        }
                        else
                        {
                            _bPlayStatus[i] = (byte)PlayerState.GIVEUP;
                        }
                    }


                    if (i == wMeChairID && _bPlayStatus[i] == (byte)PlayerState.NULL)
                    {
                        _bStart = false;
                    }
                    else
                    {
                        _bStart = true;
                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
                    }
                }


                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _lTableScore[2 * i] = packet.GetInt();
                    _lTableScore[2 * i + 1] = packet.GetInt();

                    int nTotal = _lTableScore[2 * i] + _lTableScore[2 * i + 1];

                    if (nTotal > 0)
                    {

                        AppendChips(i, nTotal, false);
                    }
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bHandCardCount[i] = packet.GetByte();
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    packet.GetBytes(ref _bHandCardData[i], 5);
                }

                //保存底牌
                _bBackCardData = _bHandCardData[GetSelfChair()][0];
                _bHandCardData[GetSelfChair()][0] = 0;


                //设置界面
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                    {
                        SetHandCardData(i, _bHandCardData[i], _bHandCardCount[i]);
                    }
                    else if (_bPlayStatus[i] == (byte)PlayerState.GIVEUP)
                    {
                        DisableHandCard(i);
                        SetUserGiveUp(i, true);
                    }
                }

                //叫分按钮
                if (_bCurrentUser == GetSelfChair())
                {
                    ShowOptionButtons();
                }
                else
                {
                    o_option_buttons.SetActive(false);
                }

                //设置定时
                SetUserClock(_bCurrentUser, 20, TimerType.TIMER_CHIP);

                //玩家信息
                UpdateUserView();
                //
                ShowCurrChips();

                //
                //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MyUser.ServerUsed.ServerID);


            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                UIMsgBox.Instance.Show(true, "SwitchPlaySceneView:" + ex.Message);
            }
        }

        #endregion


        #region ##################UI 事件#######################

        /////////////////////////////游戏通用/////////////////////////////
		private void BreakGame()
		{
			
			_bStart = false;
			//GameEngine.Instance.MyUser.SendUserQuitGameReq();
			GameEngine.Instance.Quit();
			//GameEngine.Instance.Invoke("close");
		}

        void OnBtnBackIvk()
        {
            try
            {

                if (!GameEngine.Instance.IsPlaying())
                {
                    OnConfirmBackOKIvk();
                }
                else
                {
                    UIMsgBox.Instance.Show(true, "游戏还没有结束，再玩玩吧，逃跑会受到惩罚哦!");
                    /*
                    UIExitBox.Instance.ConfirmCallBack 	= new ConfirmCall(OnConfirmBackOKIvk);
                    UIExitBox.Instance.CancelCallBack	= new CancelCall(OnConfirmBackCancelIvk);
                    UIExitBox.Instance.Show(true);
                    */
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "OnBtnBackIvk:" + ex.Message);
            }
        }

        void OnConfirmBackOKIvk()
        {
            try
            {
                o_ready_buttons.SetActive(false);

                //PlayerPrefs.SetInt("UsedServ", 0);
                _bStart = false;
				GameEngine.Instance.Quit();

                _bReqQuit = true;
                _nQuitDelay = System.Environment.TickCount;

                OnBtnSpeakCancelIvk();

            }
            catch
            {
                UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);
            }
        }

        void OnConfirmBackCancelIvk()
        {

        }

        void OnBtnSettingIvk()
        {
			UISetting.Instance.Show(true);
			btn_chat.GetComponent<UIButton>().isEnabled=false;
			//GameObject.Find("Panel").GetComponent<AudioSource>().Play();
        }

		void OnBtnChatIvk()
		{
			bool show=!btn_chat_disabled.activeSelf;
			if(show)
			{
				NGUITools.soundVolume=0;
				GameObject.Find("Panel").GetComponent<AudioSource>().volume=0;
				btn_chat_disabled.SetActive(true);
				PlayerPrefs.SetString("game_music_switch", "off");
				
			}else{
				btn_chat_disabled.SetActive(false);
				PlayerPrefs.SetString("game_music_switch","on");
                if (UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>() == null)
				{
					NGUITools.soundVolume=0.5f;
				}else{
					NGUITools.soundVolume=UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>().value;
				}
				
				if(UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>()==null)
				{
					GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0.1f;
				}else
				{
                    GameObject.Find("Panel").GetComponent<AudioSource>().volume = UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>().value;
				}
				
			}
			
		}
        void OnBtnRuleIvk()
        {
            bool bshow = !o_rules.active;
            o_rules.SetActive(bshow);
            if (bshow == true)
            {
                _nInfoTickCount = Environment.TickCount;
            }
        }
        void OnBtnReadyIvk()
        {
            if (GameEngine.Instance.AutoSit)
            {
                GameEngine.Instance.SendUserSitdown();
            }
            else
            {
                GameEngine.Instance.SendUserReadyReq();
            }

            ResetGameView();
        }

        void OnBtnQuitIvk()
        {
            OnConfirmBackOKIvk();
        }




        void OnBtnSpeakStartIvk()
        {
           
        }
        void OnBtnSpeakEndIvk()
        {
            
        }

        void OnBtnSpeakCancelIvk()
        {
        }
        void OnRecordSpeakError(string strSpeak)
        {
           
        }


        void OnClearInfoIvk()
        {
            ClearAllInfo();
        }

        //
        void OnPlayerInfoIvk(GameObject obj)
        {
			if(curGamePlatform == GamePlatform.SH_ForMobile)
			{
				string[] strs = obj.name.Split("_".ToCharArray());
				int nChair = Convert.ToInt32(strs[2]);
				
				ShowUserInfo((byte)nChair, true);
			}

        }



        /////////////////////////////游戏特殊/////////////////////////////


        void OnBtnAddIvk(GameObject obj)
        {
            try
            {

                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                int nRate = 0;

                if (obj.name == "btn_chip_1")
                {
                    nRate = 1;
                }
                else if (obj.name == "btn_chip_2")
                {
                    nRate = 2;
                }
                else if (obj.name == "btn_chip_3")
                {
                    nRate = 3;
                }
                else
                {
                    return;
                }



                //获取筹码
                byte bMeChair = GetSelfChair();
                long lCurrentScore = _lTurnLessScore - _lTableScore[bMeChair * 2 + 1] + _lCellScore * nRate;

                //
                _bAddScore = true;

                //发送
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt((int)lCurrentScore);
                GameEngine.Instance.Send(packet);

                //总注控件
                AppendChips(GetSelfChair(), (int)lCurrentScore, true);

                //
                o_option_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

                //播放声音
                PlayUserSound(GameSoundType.ADD, GameEngine.Instance.MySelf.Gender);
                //
                o_player_option[ChairToView(GetSelfChair())].SetActive(true);
                o_player_option[ChairToView(GetSelfChair())].GetComponent<UISprite>().spriteName = "desc_add";

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OnBtnFollowIvk()
        {
            try
            {

                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //获取筹码
                byte wMeChairID = GetSelfChair();

                Debug.Log("##################OnBtnFollowIvk#####################");
                Debug.Log("_lTurnLessScore:" + _lTurnLessScore.ToString());
                Debug.Log("_lTableScore[wMeChairID*2+1]:" + _lTableScore[wMeChairID * 2 + 1].ToString());

                long lCurrentScore = _lTurnLessScore - _lTableScore[wMeChairID * 2 + 1];

                Debug.Log("lCurrentScore:" + lCurrentScore.ToString());

                //加注设置
                _bAddScore = true;

                //下注动画
                AppendChips(wMeChairID, (int)lCurrentScore, true);

                //关闭时钟
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //隐藏按钮
                o_option_buttons.SetActive(false);
                //播放声音
                if (lCurrentScore == 0)
                {
                    PlayUserSound(GameSoundType.PASS, GameEngine.Instance.MySelf.Gender);
                    //
                    o_player_option[ChairToView(GetSelfChair())].SetActive(true);
                    o_player_option[ChairToView(GetSelfChair())].GetComponent<UISprite>().spriteName = "desc_pass";
                }
                else
                {
                    PlayUserSound(GameSoundType.FOLLOW, GameEngine.Instance.MySelf.Gender);
                    //
                    o_player_option[ChairToView(GetSelfChair())].SetActive(true);
                    o_player_option[ChairToView(GetSelfChair())].GetComponent<UISprite>().spriteName = "desc_follow";
                }



                //发送消息
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt((int)lCurrentScore);
                GameEngine.Instance.Send(packet);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OnBtnGiveupIvk()
        {
            try
            {

                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //扑克变灰
                byte bSelfChair = GetSelfChair();
				grayCard = true;

                //放弃设置
                _bAddScore = true;

                //放弃
                DisableHandCard(bSelfChair);

                //发送消息
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GIVE_UP);
                GameEngine.Instance.Send(packet);

                //
                o_option_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

                //播放音乐
                PlayUserSound(GameSoundType.GIVEUP, GameEngine.Instance.MySelf.Gender);


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OnBtnShowHandIvk()
        {
            try
            {

                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }


                //获取筹码
                byte bMeChair = GetSelfChair();

                //检查牌数
                if (_bHandCardCount[bMeChair] < _lAllowSuoCount) return;

                //金币统计
                long lTurnAddScore = _lTableScore[bMeChair * 2];

                Debug.Log("lTurnAddScore:" + lTurnAddScore.ToString());

                Debug.Log("_lMaxScore:" + _lMaxScore.ToString());

                Debug.Log("GameEngine.Instance.MyUser.Self.lScore:" + GameEngine.Instance.MySelf.Money.ToString());

                long lPermitScore = Math.Min(GameEngine.Instance.MySelf.Money, _lMaxScore);

                Debug.Log("lPermitScore:" + lPermitScore.ToString());

                long lCurrentScore = lPermitScore - _lTableScore[bMeChair * 2 + 1];

                Debug.Log("lCurrentScore:" + lCurrentScore.ToString());

                long lDrawAddScore = _lTableScore[bMeChair * 2] + _lTableScore[bMeChair * 2 + 1];

                Debug.Log("lDrawAddScore:" + lDrawAddScore.ToString());

                //
                _bAddScore = true;

                //
                AppendChips(bMeChair, (int)lCurrentScore, true);


                //发送
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt((int)lCurrentScore);
                GameEngine.Instance.Send(packet);

                //
                o_option_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

                //播放声音
                PlayUserSound(GameSoundType.SUOHA, GameEngine.Instance.MySelf.Gender);
                //
                o_player_option[ChairToView(GetSelfChair())].SetActive(true);
                o_player_option[ChairToView(GetSelfChair())].GetComponent<UISprite>().spriteName = "desc_suo";

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }






        #endregion


        #region ##################控件事件#######################

        /////////////////////////////游戏通用/////////////////////////////

        //扑克控件点击事件
        void OnCardClick()
        {
            if (_bPlayStatus[GetSelfChair()] != (byte)PlayerState.PLAY) return;

            ShowBackCard();						
			Invoke("HideBackCard", 5.0f);			
        }


        //定时队列处理事件
        /*void OnTimerResp(TimeEvents evt, TimeTok evtContents)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            #region GAME_END
            if (evt == TimeEvents.REG_GAME_END)
            {
                ShowResultView(true);
            }
            #endregion

        }*/
        //
        void OnSpeakTimerEnd()
        {
            OnBtnSpeakEndIvk();
        }
        //定时处理事件
        void OnTimerEnd()
        {
            try
            {
                switch (_bTimerType)
                {
                    case TimerType.TIMER_READY:
                        {
                            OnConfirmBackOKIvk();
                            break;
                        }

                    case TimerType.TIMER_CHIP:
                        {
                            OnBtnGiveupIvk();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "OnTimerEnd:" + ex.Message);
            }
        }

        //扑克控件选牌事件
        void OnMoveSelect()
        {

        }

        //扑克控件向上划牌事件
        void OnMoveUp()
        {

        }

        //扑克控件向下划牌事件
        void OnMoveDown()
        {

        }


        /////////////////////////////游戏特殊/////////////////////////////

        #endregion


        #region ##################UI 控制#######################

        /////////////////////////////游戏通用/////////////////////////////

        void SetHandCardData(byte bchair, byte[] cards, byte count)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.SetCardData(cards, count);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SetHandCardData:" + ex.Message);
            }
        }
        void OpenHandCardData(byte bchair, byte[] cards, byte count)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.OpenCardData(cards, count);
                //GameLogic.SortCardList(ref cards,count);
                //ctr.ArrayHandCards(cards,count);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "OpenHandCardData:" + ex.Message);
            }
        }

        void SendHandCard(byte bchair, byte[] cards, byte cardcount)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.AppendHandCard(bViewID, cards, cardcount);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SendHandCard:" + ex.Message);
            }
        }

        void DisableHandCard(byte bchair)
        {
            try
            {
                _bHandCardData[bchair] = new byte[5] { 254, 254, 254, 254, 254 };

                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.SetCardData(_bHandCardData[bchair], _bHandCardCount[bchair]);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "DisableHandCard:" + ex.Message);
            }
        }

        void ArrayHandCardData(byte bchair, byte[] cards, byte count)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.ArrayHandCards(cards, count);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "ArrayHandCardData:" + ex.Message);
            }
        }

        void ResetHandCardData(byte bchair)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.ResetAllShoot();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "ResetHandCardData:" + ex.Message);
            }
        }

        void ShowCurrChips()
        {
			//	每次重新计算总注数之前需要清零
			_lTotalScore = 0;
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] != (byte)PlayerState.NULL)
                {
                    byte bViewID = ChairToView((byte)i);
                    o_player_curr_desc[bViewID].SetActive(true);
                    o_player_curr_chip[bViewID].SetActive(true);
                    o_player_curr_chip[bViewID].GetComponent<UILabel>().text = (_lTableScore[i * 2] + _lTableScore[i * 2 + 1]).ToString();
					//_lTableScore[i*2].ToString() + " / " +  _lTableScore[i*2+1].ToString();

					//	进行不同的倍注统计,得到当前场上所有的总倍注.
//					if(_lTableScore[i * 2 ] ==0){
//						_lTotalScore += _lTableScore[i * 2 +1];
//					}else{
//						_lTotalScore += _lTableScore[i * 2];
//						_lTotalScore += _lTableScore[i * 2 +1];
//					}
					_lTotalScore += (_lTableScore[i * 2] + _lTableScore[i * 2 + 1]);
					//	更新当前场面上的总倍注.
					o_dlg_allChips.GetComponent<UILabel>().text = _lTotalScore.ToString ();
                }
                else
                {
                    byte bViewID = ChairToView((byte)i);
                    o_player_curr_desc[bViewID].SetActive(false);
                    o_player_curr_chip[bViewID].SetActive(false);
                }

            }
        }
        void AppendChips(byte bchair, int nUserChips, bool bPlaySound)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
                ctr.AddChips(bViewID, nUserChips, _lCellScore);

                if (bPlaySound == true)
                {
                    PlayGameSound(SoundType.CHIP);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "AppendChips:" + ex.Message);
            }
        }

        void WinChips(byte bchair)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
                ctr.WinChips(bViewID);

                PlayGameSound(SoundType.CHIP);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "WinChips:" + ex.Message);
            }
        }

        void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            try
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
//                    o_player_clock[i].GetComponent<UITimerBar>().SetTimer(0);
//                    o_player_clock[i].SetActive(false);

					o_clock[i].GetComponent<UIClock>().SetTimer(0);
					o_clock[i].SetActive(false);

                }
                if (chair < GameLogic.GAME_PLAYER)
                {
                    _bTimerType = timertype;
                    byte viewId = ChairToView(chair);
                //    o_player_clock[viewId].GetComponent<UITimerBar>().SetTimer(time * 1000);
               
				
//					_bTimerType = timertype;
//					byte viewId = ChairToView(chair);
//					o_clock[viewId].SetActive(true);
					o_clock[viewId].GetComponent<UIClock>().SetTimer(time*1000);
				}


//				if (chair == GameLogic.NULL_CHAIR)
//				{
//					for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
//					{
//						o_clock[i].GetComponent<UIClock>().SetTimer(0);
//						o_clock[i].SetActive(false);
//						
//					}
//				}
//				else
//				{
//					_bTimerType = timertype;
//					byte viewId = ChairToView(chair);
//					//o_clock[viewId].SetActive(true);
//					o_clock[viewId].GetComponent<UIClock>().SetTimer(time*1000);
//				}


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SetUserClock:" + ex.Message);
            }
        }

        void SetUserGiveUp(byte chair, bool bshow)
        {
            try
            {
                byte viewId = ChairToView(chair);
                o_player_flag[viewId].SetActive(bshow);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SetUserGiveUp:" + ex.Message);
            }
        }
        void SetCardType(byte chair, bool bshow)
        {
            try
            {
                byte viewId = ChairToView(chair);
                o_player_type[viewId].SetActive(bshow);

                if (bshow)
                {
                    string str = GetCardTypeTex(GameLogic.GetCardType(_bHandCardData[chair], 5));
					//Debug.LogWarning(chair+",CardType:"+str);
                    o_player_type[viewId].GetComponent<UISprite>().spriteName = str;
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SetCardType:" + ex.Message);
            }
        }
        void SetUserReady(byte chair, bool bshow)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                    {
                        o_player_option[i].SetActive(false);
                        o_player_option[i].GetComponent<UISprite>().spriteName = "blank";
                    }
                }
                else
                {
                    byte viewId = ChairToView(chair);
                    if (bshow)
                    {
                        o_player_option[viewId].SetActive(true);
                        o_player_option[viewId].GetComponent<UISprite>().spriteName = "play_state_ready";
                    }
                    else
                    {
                        o_player_option[viewId].GetComponent<UISprite>().spriteName = "blank";
                        o_player_option[viewId].SetActive(false);
                    }

                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "SetUserReady:" + ex.Message);
            }

        }

        void UpdateUserView()
        {
            try
            {
                if (_bStart == false) return;


                for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    byte bViewId = ChairToView((byte)i);
					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {
                        //nick 
                        if (userdata.VipLevel > 0)
                        {
                            o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
                        }
                        else
                        {
                            o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(0.35f, 0.8f, 0.8f);
                        }

                        o_player_nick[bViewId].GetComponent<UILabel>().text = userdata.NickName;
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);

                        //准备
						if (userdata.UserState == (byte)UserState.US_READY)
                        {
                            SetUserReady((byte)i, true);
                        }
                        else
                        {
                            SetUserReady((byte)i, false);
                        }

                        //金币

                        o_player_c_desc[bViewId].SetActive(true);
                        o_player_chip[bViewId].SetActive(true);
                        o_player_chip[bViewId].GetComponent<UILabel>().text = userdata.Money.ToString();

						if(curGamePlatform == GamePlatform.SH_ForPC)
						{
							ShowUserInfo(bViewId, true);
							o_player_chip[bViewId].SetActive(false);
						}
                    }
                    else
                    {
                        //nick
                        o_player_nick[bViewId].GetComponent<UILabel>().text = "";
                        //face
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
                        //p
                        o_player_option[bViewId].SetActive(false);
                        //
                        o_player_c_desc[bViewId].SetActive(false);
                        //
                        o_player_chip[bViewId].SetActive(false);
						//	玩家离开,关闭对应info
						o_info[bViewId].SetActive(false);
                    }
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, "UpdateUserView" + ex.Message);
            }

        }

        void ShowInfoBar()
        {
            /*if (GameEngine.Instance.MyUser.Self != null)
            {

                GameEngine.Instance.MyUser.Exp = (uint)GameEngine.Instance.MyUser.Self.lExperience;
                GameEngine.Instance.MyUser.GameScore = (uint)GameEngine.Instance.MyUser.Self.lScore;
                o_btn_speak_count.GetComponent<UILabel>().text = "x" + GameEngine.Instance.MyUser.SmallSpeakerCount.ToString();

            }*/
        }

        void ShowUserInfo(byte bViewID, bool bshow)
        {
            if (bViewID == GameLogic.NULL_CHAIR)
            {
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
					o_info[i].SetActive(false);
					if(curGamePlatform == GamePlatform.SH_ForMobile)
					{
						o_info[i].SetActive(false);
					}
                }
            }
            else
            {
                byte bChairID = ViewToChair(bViewID);
                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(bChairID);
                if (ud != null)
                {
                    o_info[bViewID].SetActive(true);
                    o_info_nick[bViewID].GetComponent<UILabel>().text = ud.NickName;
                    o_info_lvl[bViewID].GetComponent<UILabel>().text = GameConfig.Instance.GetExpLevel((int)ud.Exp).ToString();
                    o_info_id[bViewID].GetComponent<UILabel>().text = ud.ID.ToString();
                    o_info_score[bViewID].GetComponent<UILabel>().text = ud.Money.ToString();
                    o_info_win[bViewID].GetComponent<UILabel>().text = ud.WinCount.ToString();
                    o_info_lose[bViewID].GetComponent<UILabel>().text = ud.LostCount.ToString();
                    o_info_run[bViewID].GetComponent<UILabel>().text = ud.DrawCount.ToString();

                    _nInfoTickCount = Environment.TickCount;
                }
            }
        }

        byte ChairToView(byte ChairID)
        {
			byte wViewChairID = (byte)((ChairID + GameLogic.GAME_PLAYER - GameEngine.Instance.MySelf.DeskStation));
            return (byte)(wViewChairID % GameLogic.GAME_PLAYER);
        }

        byte ViewToChair(byte ViewID)
        {
			byte wChairID = (byte)((ViewID + GameEngine.Instance.MySelf.DeskStation) % GameLogic.GAME_PLAYER);
            return wChairID;
        }

        byte GetSelfChair()
        {
			//修正MySelf 为NULL 时的获取错误
			if (GameEngine.Instance.MySelf != null) {
				return (byte)GameEngine.Instance.MySelf.DeskStation;
			} else {
			}
			return 0xFF;
        }

        void ShowBackCard()
        {
            _bHandCardData[GetSelfChair()][0] = _bBackCardData;
            SetHandCardData(GetSelfChair(), _bHandCardData[GetSelfChair()], _bHandCardCount[GetSelfChair()]);
        }

        void HideBackCard()
        {
			//	如果不是灰牌状态才能把牌翻回红色背面.
			if(grayCard){
				return;
			}else{
				_bHandCardData[GetSelfChair()][0] = 255;
				SetHandCardData(GetSelfChair(), _bHandCardData[GetSelfChair()], _bHandCardCount[GetSelfChair()]);	
			}           
        }

        void PlayGameSound(SoundType sound)
        {
            //string str = PlayerPrefs.GetString("game_effect_switch", "on");
            //if (str == "on" && o_speak_timer.active == false)
            {
                float fvol = NGUITools.soundVolume;
                NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
            }
        }

        void PlayUserSound(GameSoundType sound, byte bGender)
        {
            //string str = PlayerPrefs.GetString("game_effect_switch", "on");
           // if (str == "on" && o_speak_timer.active == false)
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
        }

        void ShowUserSpeak(uint uid)
        {

            
        }

        void OnRecordSpeakFinish(string strSpeak)
        {
           
        }
        void OnSpeakPlay(string str)
        {
           
        }
        IEnumerator UpLoadSpeak(string strSpeak)
        {
			yield return null;
        }
        int GetStringLength(string str)
        {
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            return data.Length;
        }

        void ShowUserChat(byte bChair, string strMsg)
        {
            if (bChair == GameLogic.NULL_CHAIR)
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_chat[i].SetActive(false);
                }
            }
            else
            {
                _nInfoTickCount = Environment.TickCount;
                byte bViewID = ChairToView(bChair);
                //
                /*o_player_chat[bViewID].SetActive(true);
                o_player_chat[bViewID].GetComponentInChildren<UILabel>().text = strMsg;*/
                //

            }
        }

        void ShowNotice(string strMsg)
        {
            UIMsgBox.Instance.Show(true, strMsg);

        }

        void ClearAllInfo()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
				if(curGamePlatform != GamePlatform.SH_ForPC)
				{
					o_info[i].SetActive(false);
				}

                o_player_chat[i].SetActive(false);

            }
            o_rules.SetActive(false);
        }




        /////////////////////////////游戏特殊/////////////////////////////
        void ShowResultView()
        {
            ShowResultView(true);
        }

        void ShowResultView(bool bshow)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            o_result.SetActive(bshow);


            if (bshow)
            {
                Invoke("CloseResultView", 5.0f);
            }

            /*
            if(GameEngine.Instance.MyUser.Self.lScore <_lBaseScore)
            {
                UIInfoBox.Instance.Show(true,"您的乐豆不足,不能继续游戏!");
                Invoke("OnConfirmBackOKIvk",1.0f);
                return;
            }

             if(bshow)
             {
                 Invoke("CloseResultView",5.0f);
             }
            */
        }
        string GetCardTypeTex(byte bCardType)
        {
            if (bCardType == GameLogic.CT_HU_LU) return "card_type_hl";
            if (bCardType == GameLogic.CT_ONE_DOUBLE) return "card_type_dz";
            if (bCardType == GameLogic.CT_SHUN_ZI) return "card_type_sz";
            if (bCardType == GameLogic.CT_THREE_TIAO) return "card_type_st";
            if (bCardType == GameLogic.CT_TIE_ZHI) return "card_type_tz";
            if (bCardType == GameLogic.CT_TONG_HUA) return "card_type_th";
            if (bCardType == GameLogic.CT_TONG_HUA_SHUN) return "card_type_ths";
            if (bCardType == GameLogic.CT_TWO_DOUBLE) return "card_type_ld";
            if (bCardType == GameLogic.CT_SINGLE) return "card_type_sp";

            return "blank";
        }
        void CloseResultView()
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            o_result.SetActive(false);

            if (CheckScoreLimit() == false)
            {
                return;
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _lTableScore[i] = 0;
                _lTotalScore = 0;
                o_player_cards[i].SetActive(false);
                o_player_flag[i].SetActive(false);
                o_player_type[i].SetActive(false);

                o_player_curr_chip[i].SetActive(false);
                o_player_curr_desc[i].SetActive(false);
            }

            //
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            //

            SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY);

            o_ready_buttons.SetActive(true);

            CancelInvoke();
        }

        void ShowOptionButtons()
        {
            //变量定义
            byte bMeChairID = GetSelfChair();
            if (bMeChairID != _bCurrentUser) return;


            byte cbCardCount = _bHandCardCount[bMeChairID];
            long lDrawAddScore = _lTableScore[bMeChairID * 2] + _lTableScore[bMeChairID * 2 + 1];

            //梭哈金币
            long lMaxLeaveScore = GameEngine.Instance.MySelf.Money- lDrawAddScore;
            long lUserLessScore = Math.Min(lMaxLeaveScore, Math.Max(_lTurnLessScore - lDrawAddScore, 0L));

            //下注按钮
            long lLeaveScore = Math.Max(_lTurnMaxScore - lDrawAddScore, 0L);

            o_option_buttons.SetActive(true);

            //加注
            o_btn_chip_1.SetActive(true);
            o_btn_chip_2.SetActive(true);
            o_btn_chip_3.SetActive(true);

            if ((_bShowHand == false) && (_bAddScore == false) && (lLeaveScore > 0))
            {
                o_btn_chip_1.GetComponent<UIButton>().isEnabled = true;
                o_btn_chip_2.GetComponent<UIButton>().isEnabled = true;
                o_btn_chip_3.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_btn_chip_1.GetComponent<UIButton>().isEnabled = false;
                o_btn_chip_2.GetComponent<UIButton>().isEnabled = false;
                o_btn_chip_3.GetComponent<UIButton>().isEnabled = false;
            }

            //不加和跟按钮
            if (lUserLessScore == 0L)
            {
                o_btn_pass.SetActive(true);
                o_btn_followchip.SetActive(false);
                o_btn_pass.GetComponent<UIButton>().isEnabled = true;

            }
            else
            {
                o_btn_followchip.SetActive(true);
                o_btn_pass.SetActive(false);
                o_btn_followchip.GetComponent<UIButton>().isEnabled = !_bShowHand;
            }

            //放弃
            o_btn_giveup.SetActive(true);

            //梭哈按钮
            o_btn_showhand.SetActive(true);
            o_btn_showhand.GetComponent<UIButton>().isEnabled = (cbCardCount >= _lAllowSuoCount ? true : false);

        }


        int GetPlayingUserCount()
        {
            //当前人数
            byte UserCount = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                {
                    UserCount++;
                }
            }

            return UserCount;
        }
        void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        bool CheckScoreLimit()
        {
            //金币限制检测
            //int nLimit = 0;
            ///*if (GameEngine.Instance.MySelf.ServerUsed.StationID.ToString().EndsWith("39") == true)
            //{
            //    nLimit = 10000;
            //}
            //else*/
            //{
            //    nLimit = 20 * _lCellScore;
            //}

            //if (GameEngine.Instance.MySelf.Money < nLimit)
            //{
            //    UIMsgBox.Instance.Show(true, "您的乐豆不足,不能继续游戏!");
            //    Invoke("OnConfirmBackOKIvk", 2.0f);
            //    _bStart = false;
            //    return false;
            //}
            return true;
        }
        #endregion

    }
}