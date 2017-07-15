using UnityEngine;
using System.Collections;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.NN
{

    #region ##################结构定义#######################
    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_READY = 1,
        TIMER_QIANG = 2,
        TIMER_CHIP = 3,
        TIMER_OPEN = 4,

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
		BULLSMLIE=7
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
        BANKER = 11,
		QING=12,
		BUQING=13,

    };

    public enum TimerDelay
    {
        NULL = 0,
        QIANG = 5,
        CHIP = 5,
        OPEN = 20,
        READY = 20
    };

	public enum GamePlatform	//游戏平台
	{
		NN_ForPC,
		NN_ForMobile
	}


    #endregion


    public class UIGame : MonoBehaviour
    {
        #region ##################变量定义#######################
		
		public GamePlatform curGamePlatform; 	//当前游戏平台

        //通用控件
        // GameObject      o_dlg_tips          = null;
        GameObject o_speak_timer = null;
        GameObject o_btn_speak = null;
        GameObject o_btn_speak_count = null;

        GameObject[] o_player_option = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_chat = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_chip = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_cards = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_money = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_type = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_player_face = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_flag = new GameObject[GameLogic.GAME_PLAYER];
		GameObject[] o_player_info = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_frame = new GameObject[GameLogic.GAME_PLAYER];

        GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER];
		GameObject[] o_player_bg = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER];

        GameObject   o_result = null;
        GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_exp = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_type = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_win = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_bean = new GameObject[GameLogic.GAME_PLAYER];

		GameObject o_result_top = null; 
        GameObject o_ready_buttons = null;
        GameObject o_btn_ready = null;
        GameObject o_btn_quit = null;

        GameObject o_current_time = null;
        GameObject o_room_cell = null;
        GameObject o_room_limit = null;


        //游戏特殊
        GameObject o_option_buttons = null;
        GameObject o_btn_opencard = null;
        GameObject o_btn_tipcard = null;
        GameObject o_btn_none = null;


        GameObject o_add_buttons = null;
        GameObject o_btn_chip_1 = null;
        GameObject o_btn_chip_2 = null;
        GameObject o_btn_chip_3 = null;
        GameObject o_btn_chip_4 = null;

        GameObject o_qiang_buttons = null;
        GameObject o_btn_qiang = null;
        GameObject o_btn_notqiang = null;

        //信息框
        GameObject o_dlg_chips = null;
        GameObject o_player_clock = null;
        GameObject o_self_money = null;

        GameObject o_rules = null;
		GameObject o_tips_bg =null;
		GameObject o_tips_text =null;

        //通用数据
        static bool _bStart = false;
        static TimerType _bTimerType = TimerType.TIMER_NULL;

        //底注
        static int _lMaxChip = 0;     //最大下注金额（8倍）
        static int _lCellScore = 0;	  //顶注，当前自己能下的最大注

        //游戏状态
        static byte[] _bPlayStatus = new byte[GameLogic.GAME_PLAYER];
        //
        static byte[] _bCallStatus = new byte[GameLogic.GAME_PLAYER];
        //下注数目
        static int[] _lTableScore = new int[GameLogic.GAME_PLAYER];
        //
        static byte[] _bOpenStatus = new byte[GameLogic.GAME_PLAYER];


        static byte[][] _bHandCardData = new byte[GameLogic.GAME_PLAYER][];
        static byte[] _bHandCardCount = new byte[GameLogic.GAME_PLAYER];
        static byte _bBankerUser = GameLogic.NULL_CHAIR;
        static int _nInfoTickCount = 0;

        static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER];
        static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER];
        static byte[] _nEndUserCardType = new byte[GameLogic.GAME_PLAYER];
        static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER];

        static int _nQuitDelay = 0;
        static bool _bReqQuit = false;
        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[20];
		public AudioClip[] _WomanSound2= new AudioClip[15];
        public AudioClip[] _ManSound = new AudioClip[20];
		public AudioClip[] _ManSound2=new AudioClip[15];
		// 声音按钮
		public static GameObject btn_chat_disabled=null;
        public static GameObject btn_chat = null;


        #endregion


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
                o_btn_speak = GameObject.Find("scene_game/btn_speak");
                o_btn_speak_count = GameObject.Find("scene_game/btn_speak/lbl_count");
                o_player_clock = GameObject.Find("scene_game/ctr_clock");
				btn_chat_disabled=GameObject.Find ("scene_game/dlg_top_bar/btn_voice/disabled");
				btn_chat=GameObject.Find ("scene_game/dlg_top_bar/btn_voice");

				UIMsgBox.Instance.platform = curGamePlatform;

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_option[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_option");
                    o_player_chat[i] = GameObject.Find("scene_game/dlg_chat_msg_" + i.ToString());
                    o_player_face[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_user_face");
                    o_player_frame[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_frame");

                    o_player_chip[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_chips");
                    o_player_nick[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_nick");
                    o_player_flag[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_flag");
                    o_player_money[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_money");
                    o_player_type[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_type");
                    //o_user_speak[i] = GameObject.Find("scene_game/ctr_speak_" + i.ToString());
					o_player_info[i] =  GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_infos");

					if(curGamePlatform == GamePlatform.NN_ForMobile)
					{
						o_player_bg[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString()+"/sp_bg");
					}

                    o_info[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString());


                    o_info_nick[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_nick");
//                    o_info_gold[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_gold");
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
					o_result_top = GameObject.Find("scene_game/dlg_result/sp_top");
                }

                //准备
                o_ready_buttons = GameObject.Find("scene_game/dlg_ready_buttons");
                o_btn_ready = GameObject.Find("scene_game/dlg_ready_buttons/btn_ready");
                o_btn_quit = GameObject.Find("scene_game/dlg_ready_buttons/btn_quit");
                o_result = GameObject.Find("scene_game/dlg_result");

                //游戏
                o_option_buttons = GameObject.Find("scene_game/dlg_option_buttons");
                o_btn_opencard = GameObject.Find("scene_game/dlg_option_buttons/btn_open");
                o_btn_none = GameObject.Find("scene_game/dlg_option_buttons/btn_none");
                o_btn_tipcard = GameObject.Find("scene_game/dlg_option_buttons/btn_tips");

                o_add_buttons = GameObject.Find("scene_game/dlg_add_buttons");
                o_btn_chip_1 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_1");
                o_btn_chip_2 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_2");
                o_btn_chip_3 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_3");
                o_btn_chip_4 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_4");

                o_qiang_buttons = GameObject.Find("scene_game/dlg_qiang_buttons");
                o_btn_qiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_qiang");
                o_btn_notqiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_notqiang");
                o_dlg_chips = GameObject.Find("scene_game/ctr_chips");
                o_self_money = GameObject.Find("scene_game/dlg_bottom_bar/ctr_money");

                o_current_time = GameObject.Find("scene_game/dlg_top_bar/lbl_curr_time");
                o_room_cell = GameObject.Find("scene_game/dlg_bottom_bar/lbl_cell");
                o_room_limit = GameObject.Find("scene_game/dlg_bottom_bar/lbl_limit");

                o_rules = GameObject.Find("scene_game/dlg_rules");

				o_tips_bg = GameObject.Find("scene_game/dlg_tips/tip_bg");
				o_tips_text = GameObject.Find("scene_game/dlg_tips/tip_text");
            }
            catch (Exception ex)
            {
               // UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

		public void Update()
		{
			if(GameEngine.Instance == null)
			{
				return;
			}
			if (curGamePlatform == GamePlatform.NN_ForPC) {
				for (int i = 0; i<GameLogic.GAME_PLAYER; i++) {
					byte bViewId = ChairToView ((byte)i);
					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer ((uint)i);
					if (userdata != null) {	
						ShowUserInfo (bViewId, true);
					}else
					{
						o_info[bViewId].SetActive(false);
					}
				}
			} else {
				for (int i = 0; i<GameLogic.GAME_PLAYER; i++) {
					byte bViewId = ChairToView ((byte)i);
					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer ((uint)i);
					if (userdata != null) {	
						o_player_money[bViewId].SetActive(true);
						o_player_bg[bViewId].SetActive(false);
					}
					else
					{
						o_player_money[bViewId].SetActive(false);
						o_player_bg[bViewId].SetActive(false);
					}
				}
			}
		}


        void InitGameView()
        {
            //Data
            _bStart = false;
            _bTimerType = TimerType.TIMER_READY;
            _lMaxChip = 0;
            _lCellScore = 0;

            _bBankerUser = GameLogic.NULL_CHAIR;
            _nInfoTickCount = Environment.TickCount;

            _nQuitDelay = 0;
            _bReqQuit = false;
//           _nInfoTickCount = Environment.TickCount;

            //UI
            o_btn_quit.SetActive(false);
//            o_speak_timer.SetActive(false);
			//声音按钮
			btn_chat.GetComponent<UIButton>().isEnabled=true;

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_option[i].SetActive(false);
                o_player_chip[i].SetActive(false);
				o_player_info[i].SetActive(false);
                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_money[i].SetActive(false);
                o_player_type[i].SetActive(false);

                o_player_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
                o_player_nick[i].GetComponent<UILabel>().text = "";
                o_player_flag[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_frame[i].SetActive(false);

				if(curGamePlatform == GamePlatform.NN_ForMobile)
				{
					o_player_bg[i].SetActive(false);
				}

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
                _bCallStatus[i] = 0xFF;
                _bOpenStatus[i] = 0xFF;
                Array.Clear(_bHandCardData[i], 0, 5);
                Array.Clear(_bHandCardCount, 0, 5);

                _lTableScore[i] = 0;
                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;

            }
            o_player_clock.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_qiang_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);
            o_add_buttons.SetActive(false);
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            o_rules.SetActive(false);
			o_tips_text.GetComponent<UILabel>().text = "";
			o_tips_bg.SetActive (false);

        }
        void ResetGameView()
        {
            _bTimerType = TimerType.TIMER_READY;
            _nInfoTickCount = Environment.TickCount;

            //UI
            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_option[i].SetActive(false);
                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_money[i].SetActive(false);
                o_player_type[i].SetActive(false);

                o_player_flag[i].GetComponent<UISprite>().spriteName = "blank";

				if(curGamePlatform == GamePlatform.NN_ForMobile)
				{
					o_player_bg[i].SetActive(false);
				}

                o_info[i].SetActive(false);
                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";

                _bHandCardData[i] = new byte[5];
                Array.Clear(_bHandCardData[i], 0, 5);
                Array.Clear(_bHandCardCount, 0, 5);

                _lTableScore[i] = 0;
                _bCallStatus[i] = 0xFF;
                _bOpenStatus[i] = 0xFF;
                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;
            }

            o_player_clock.SetActive(true);
            o_ready_buttons.SetActive(false);
            o_qiang_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);
            o_add_buttons.SetActive(false);
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            o_rules.SetActive(false);
			o_tips_text.GetComponent<UILabel>().text = "";
			o_tips_bg.SetActive (false);

            Resources.UnloadUnusedAssets();
            GC.Collect();

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

			NGUITools.soundVolume=1.0f;
        }

        void FixedUpdate()
        {

            o_current_time.GetComponent<UILabel>().text = System.DateTime.Now.ToString("hh:mm:ss");

            o_room_cell.GetComponent<UILabel>().text = _lCellScore.ToString();
            o_room_limit.GetComponent<UILabel>().text = (_lCellScore * 20).ToString();

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
                CancelInvoke();
                _bReqQuit = false;
                _nQuitDelay = System.Environment.TickCount;

                UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);
            }

        }
//        void Update()
//        {
//            /*if (GameEngine.Instance.MySelf.IsDisconnectFromServer && _bStart == true)
//            {
//                CancelInvoke();
//
//                _bStart = false;
//
//                PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);
//
//                UIMsgBox.Instance.Show(true, "您掉线了,网络不给力啊!");
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
                   
                case TableEvents.USER_READY:
                    {
                        OnUserReadyResp(null);
                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_LEAVE:
                case TableEvents.USER_COME:
                case TableEvents.USER_PLAY:
                case TableEvents.USER_OFFLINE:
                case TableEvents.USER_LOOKON:
                case TableEvents.USER_SCORE:
                    {
                        UpdateUserView();
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

                        Invoke("ClearUserReady", 1.0f);
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
                UIMsgBox.Instance.Show(true, ex.Message);
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
                case (byte)GameLogic.GS_WK_BANKER:
                    {
                        SwitchBankerSceneView(packet);
                        break;
                    }
                case (byte)GameLogic.GS_WK_CHIP:
                    {
                        SwitchChipSceneView(packet);
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
                UIMsgBox.Instance.Show(true, ex.Message);
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        #endregion


        #region ##################游戏消息#######################

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {

                case SubCmd.SUB_S_CALL_BANKER:
                    {
                        //抢庄
                        OnUserQiangBankerResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_START:
                    {
                        //抢庄结束，游戏开始
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_ADD_SCORE:
                    {
                        //闲家下注
                        OnUserAddScoreResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_SEND_CARD:
                    {
                        //下注结束，开始发牌
                        OnGameSendCard(packet);
                        break;
                    }
                case SubCmd.SUB_S_OPEN_CARD:
                    {
                        //玩家开牌
                        OnUserOpenCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_PLAYER_EXIT:
                    {
                        //玩家强退
                        OnUserPlayerExitResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:
                    {
                        //游戏结束，公布结果
                        OnGameEndResp(packet);
                        break;
                    }

            }
        }

        //抢庄开始或继续坐庄
        void OnUserQiangBankerResp(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_BANKER;
            packet.BeginRead();
            ushort wQiangUser = packet.GetUShort();
            byte bQiang = packet.GetByte();


            if (wQiangUser > GameLogic.GAME_PLAYER)
            {
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

                    }
                }

                //开始抢庄
                o_ready_buttons.SetActive(false);
                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                o_qiang_buttons.SetActive(true);

                SetUserClock(GetSelfChair(), (uint)TimerDelay.QIANG, TimerType.TIMER_QIANG);

            }
            else  
            {
                _bCallStatus[wQiangUser] = bQiang;
                SetCallBanker((byte)wQiangUser, bQiang);
            }

            //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);
        }

        //抢庄结束开始下注
         void OnGameStartResp(NPacket packet)
        {
            /*
            LONG                                lTurnMaxScore;                      //最大下注
            WORD                                wBankerUser;                        //庄家用户
            */
            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;
                packet.BeginRead();
                _lMaxChip = packet.GetInt();			   //最大下注
                _bBankerUser = (byte)packet.GetUShort();   //庄家位置

				//播放背景音乐
				//GameObject.Find("Panel").GetComponent<AudioSource>().Play();
                //
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
                        _bPlayStatus[i] = (byte)PlayerState.PLAY;

                    }

                    o_player_option[i].SetActive(false);
                }

                //设置庄家
                SetBanker();

                if (_bBankerUser == GetSelfChair())
                {
                    byte bSex = GameEngine.Instance.GetTableUserItem(_bBankerUser).Gender;
                    PlayUserSound(GameSoundType.BANKER, bSex);

					o_tips_text.GetComponent<UILabel>().text  = "等待其他玩家下注...";
					o_tips_bg.SetActive (true);
				}else{
					o_tips_text.GetComponent<UILabel>().text  = "";
					o_tips_bg.SetActive (false);
				}

                //开始抢庄
                o_ready_buttons.SetActive(false);
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);

                //显示下注按钮
                if (_bBankerUser != GetSelfChair())
                {
                    SetChipButton(true, _lMaxChip);

                    //定时
                    SetUserClock(GetSelfChair(), (uint)TimerDelay.CHIP, TimerType.TIMER_CHIP);
                }

                //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户下注
        void OnUserAddScoreResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                byte bChair = (byte)packet.GetUShort();
                int nChip = packet.GetInt();

                if (bChair != GetSelfChair())
                {
                    _lTableScore[bChair] = nChip;

                    UpdateGameInfoView();

                    AppendChips(bChair, nChip);

				}
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        //下注结束，开始发牌
        void OnGameSendCard(NPacket packet)
        {
            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
                packet.BeginRead();
                packet.GetBytes(ref _bHandCardData[GetSelfChair()], 5);

				Invoke("SendUserCards", 1.0f);
				//	发牌阶段,提示清空
				o_tips_text.GetComponent<UILabel>().text = "";			
				o_tips_bg.SetActive (false);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }
        void SendUserCards()
        {
            try
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                    {
                        SendHandCard(i, _bHandCardData[i], 5);
                        PlayGameSound(SoundType.SENDCARD);
                    }
                }

                o_ready_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);

                o_option_buttons.SetActive(true);
                o_btn_opencard.GetComponent<UIButton>().isEnabled = false;
                o_btn_tipcard.SetActive(false);

                SetUserClock(GetSelfChair(), (uint)TimerDelay.OPEN, TimerType.TIMER_OPEN);

                Invoke("ArraySelfCards", 1.0f);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        void ArraySelfCards()
        {
            SetHandCardData(GetSelfChair(), _bHandCardData[GetSelfChair()], 5);

            Invoke("ShowTipsButton", 10.0f);
        }
        void ShowTipsButton()
        {
            o_btn_tipcard.SetActive(true);
        }
        
		//用户开牌
        void OnUserOpenCardResp(NPacket packet)
        {

            try
            {
                packet.BeginRead();
                byte bChair = (byte)packet.GetUShort();
                byte bNx = packet.GetByte();
                packet.GetBytes(ref _bHandCardData[bChair], 5);

                if (bNx > 10) bNx = 10;
                _bOpenStatus[bChair] = bNx;


                OpenHandCardData(bChair, _bHandCardData[bChair], 5);
                SetCardType(bChair, bNx);


                byte bSex = GameEngine.Instance.GetTableUserItem(bChair).Gender;
                PlayUserSound((GameSoundType)bNx, bSex);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        //用户强退
        void OnUserPlayerExitResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                byte bExitUser = (byte)packet.GetUShort();
                //游戏信息
                _bPlayStatus[bExitUser] = (byte)PlayerState.NULL;
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏结束
        void OnGameEndResp(NPacket packet)
        {
            /*
             LONG                                lGameTax[GAME_PLAYER];              //游戏税收
             LONG                                lGameScore[GAME_PLAYER];            //游戏得分
             LONG                                lExp[3];                            //经验
            */
            try
            {

                if (_bStart == false)
                {
                    return;
                }

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;



                int[] lGameTax = new int[GameLogic.GAME_PLAYER];

                packet.BeginRead();
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    lGameTax[i] = packet.GetInt();
                }

                //游戏得分

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //游戏得分
                    _nEndUserScore[i] = packet.GetInt();
					int curr=(int)GameEngine.Instance.MySelf.DeskStation;
					if(curr==i)
					{
						if(_nEndUserScore[i]>=0)
						{
							PlayGameSound(SoundType.WIN);
							PlayGameSound(SoundType.BULLSMLIE);
						}else
						{
							PlayGameSound(SoundType.LOSE);
						}

					}
                }
                //经验
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //游戏得分
                    _nEndUserExp[i] = packet.GetInt();
                    if (_nEndUserExp[i] > 100 || _nEndUserExp[i] < 0)
                    {
                        _nEndUserExp[i] = 0;
                    }
                }

                SetBanker();

                UpdateUserView();

                

                o_option_buttons.SetActive(false);

                //
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    byte bViewId = ChairToView(i);
                    PlayerInfo ud = GameEngine.Instance.GetTableUserItem(i);
                    if (ud != null)
                    {
                        o_result_score[bViewId].GetComponent<UILabel>().text = _nEndUserScore[i].ToString();

						if( _nEndUserScore[GetSelfChair()]<0)
						{
							o_result_top.GetComponent<UISprite>().spriteName = "game_lose";
						}
						else if(_nEndUserScore[GetSelfChair()]>0)
						{
							o_result_top.GetComponent<UISprite>().spriteName = "game_win";
						}
						else
						{
							o_result_top.GetComponent<UISprite>().spriteName = "blank";
						}

                        o_result_exp[bViewId].GetComponent<UILabel>().text = _nEndUserExp[i].ToString();
						o_result_win[bViewId].transform.FindChild("bg").gameObject.SetActive(true);

                        byte bNx = _bOpenStatus[i];
                        if (bNx >= 0 && bNx <= 10)
                        {
                            o_result_win[bViewId].GetComponent<UISprite>().spriteName = "n_" + bNx.ToString();

                        }
                        else
                        {
                            o_result_win[bViewId].GetComponent<UISprite>().spriteName = "blank";
							//o_result_win[bViewId].transform.FindChild("bg").gameObject.SetActive(false);
                        }


                        if (_bBankerUser == i)
                        {
                            o_result_type[bViewId].GetComponent<UISprite>().spriteName = "Zhuang_2";
                        }
                        else
                        {
                            o_result_type[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        }

                        o_result_nick[bViewId].GetComponent<UILabel>().text = ud.NickName;
                        o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "gold01";                        
                    }
                    else
                    {
                        o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_nick[bViewId].GetComponent<UILabel>().text = "";
                        o_result_win[bViewId].GetComponent<UISprite>().spriteName = "blank";
						o_result_win[bViewId].transform.FindChild("bg").gameObject.SetActive(false);
                        o_result_score[bViewId].GetComponent<UILabel>().text = "";
                        o_result_exp[bViewId].GetComponent<UILabel>().text = "";
                        o_result_type[bViewId].GetComponent<UISprite>().spriteName = "blank";
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
                            o_result_exp[k].GetComponent<UILabel>().text = o_result_exp[k+1].GetComponent<UILabel>().text;
                            o_result_exp[k + 1].GetComponent<UILabel>().text = temp_exp;

                            bool temp_win_one = o_result_win[k].transform.FindChild("bg").gameObject.activeSelf;
                            bool temp_win_two = o_result_win[k+1].transform.FindChild("bg").gameObject.activeSelf;
                            o_result_win[k].transform.FindChild("bg").gameObject.SetActive(temp_win_two);
                            o_result_win[k + 1].transform.FindChild("bg").gameObject.SetActive(temp_win_one);

                            string temp_win = o_result_win[k].GetComponent<UISprite>().spriteName;
                            o_result_win[k].GetComponent<UISprite>().spriteName = o_result_win[k+1].GetComponent<UISprite>().spriteName;
                            o_result_win[k + 1].GetComponent<UISprite>().spriteName = temp_win;

                            string temp_type = o_result_type[k].GetComponent<UISprite>().spriteName;
                            o_result_type[k].GetComponent<UISprite>().spriteName = o_result_type[k+1].GetComponent<UISprite>().spriteName;
                            o_result_type[k+1].GetComponent<UISprite>().spriteName = temp_type;

                            string temp_nick = o_result_nick[k].GetComponent<UILabel>().text;
                            o_result_nick[k].GetComponent<UILabel>().text = o_result_nick[k+1].GetComponent<UILabel>().text;
                            o_result_nick[k+1].GetComponent<UILabel>().text = temp_nick;

                            string temp_bean = o_result_bean[k].GetComponent<UISprite>().spriteName;
                            o_result_bean[k].GetComponent<UISprite>().spriteName = o_result_bean[k+1].GetComponent<UISprite>().spriteName;
                            o_result_bean[k+1].GetComponent<UISprite>().spriteName = temp_bean;
                        }
                    }
                }
				o_tips_text.GetComponent<UILabel>().text = "";				
				o_tips_bg.SetActive (false);
				Invoke("ShowResultView", 2);
                //GameEngine.Instance.MySelf.TEvent.AddTimeTok(TimeEvents.REG_GAME_END,2000,packet);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //初始场景处理函数
        void SwitchFreeSceneView(NPacket packet)
        {
            try
            {
                ResetGameView();

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

                _bStart = true;

                packet.BeginRead();
                _lCellScore = packet.GetInt();

                UpdateUserView();

                SetUserClock(GetSelfChair(), (uint)TimerDelay.READY, TimerType.TIMER_READY);

                o_ready_buttons.SetActive(true);
                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
				o_tips_text.GetComponent<UILabel>().text = "";
				o_tips_bg.SetActive (false);
                //PlayerPrefs.SetInt("UsedServ", 0);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        //初始抢庄处理函数
        void SwitchBankerSceneView(NPacket packet)
        {
            try
            {
                ResetGameView();

                _bStart = true;

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_BANKER;



                packet.BeginRead();
                _lCellScore = packet.GetInt();
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bCallStatus[i] = packet.GetByte();

                }

                UpdateUserView();


                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //设置抢庄标志
                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                    {
                        SetCallBanker(i, _bCallStatus[i]);
                    }
                }

                if (_bCallStatus[GetSelfChair()] == 0xFF)
                {
                    SetUserClock(GetSelfChair(), (uint)TimerDelay.QIANG, TimerType.TIMER_QIANG);
                    o_qiang_buttons.SetActive(true);
                }
                else
                {
                    o_qiang_buttons.SetActive(false);
                }
                o_ready_buttons.SetActive(false);
                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
				o_tips_text.GetComponent<UILabel>().text = "";
				o_tips_bg.SetActive (false);

                //PlayerPrefs.SetInt("UsedServ", 0);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //初始下注处理函数
        void SwitchChipSceneView(NPacket packet)
        {
            try
            {
                ResetGameView();

                _bStart = true;

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_CHIP;

                packet.BeginRead();
                _lCellScore = packet.GetInt();
                _lMaxChip = packet.GetInt();

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _lTableScore[i] = packet.GetInt();
                }

                _bBankerUser = (byte)packet.GetUShort();

                SetBanker();

                UpdateGameInfoView();

                UpdateUserView();

                o_ready_buttons.SetActive(false);
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
                if (_lTableScore[GetSelfChair()] == 0)
                {
                    //显示下注按钮
                    SetChipButton(true, _lMaxChip);
                    SetUserClock(GetSelfChair(), (uint)TimerDelay.CHIP, TimerType.TIMER_CHIP);
                }
                else
                {
                    //显示下注按钮
                    SetChipButton(false, _lMaxChip);
                }

                //PlayerPrefs.SetInt("UsedServ", 0);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏场景处理函数
        void SwitchPlaySceneView(NPacket packet)
        {
            try
            {
                ResetGameView();

                _bStart = true;

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;

                packet.BeginRead();
                _lCellScore = packet.GetInt();
                _lMaxChip = packet.GetInt();

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _lTableScore[i] = packet.GetInt();
                }

                _bBankerUser = (byte)packet.GetUShort();

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    packet.GetBytes(ref _bHandCardData[i], 5);
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bOpenStatus[i] = packet.GetByte();

                }


                SetBanker();

                UpdateGameInfoView();

                UpdateUserView();

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                    {
                        SetHandCardData(i, _bHandCardData[i], 5);

						//玩家断线重连显示其他玩家牛判断不对问题
						byte bType = GameLogic.GetCardType(_bHandCardData[i],5);
						if(  _bOpenStatus[i] != 0xFF )
						{
							SetCardType(i,bType);
						}
//                        SetCardType(i, _bOpenStatus[i]);
                    }
                }

                o_ready_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);

                if (_bOpenStatus[GetSelfChair()] == 0xFF)
                {
                    o_option_buttons.SetActive(true);
                    o_btn_opencard.GetComponent<UIButton>().isEnabled = false;

					//定时器错误(应该放玩游戏的定时器)
//                    SetUserClock(GetSelfChair(), (uint)TimerDelay.CHIP, TimerType.TIMER_CHIP);
					SetUserClock(GetSelfChair(), (uint)TimerDelay.OPEN, TimerType.TIMER_OPEN);
                }
                else
                {
                    o_option_buttons.SetActive(false);
                }



                //PlayerPrefs.SetInt("UsedServ", 0);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
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


		//退出按钮
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
                    UIMsgBox.Instance.Show(true, "游戏还没有结束，再玩玩吧!");
                    /*
                    UIExitBox.Instance.ConfirmCallBack 	= new ConfirmCall(OnConfirmBackOKIvk);
                    UIExitBox.Instance.CancelCallBack	= new CancelCall(OnConfirmBackCancelIvk);
                    UIExitBox.Instance.Show(true);
                    */
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

		//确认退出
        void OnConfirmBackOKIvk()
        {
            try
            {
                o_ready_buttons.SetActive(false);
                _bStart = false;
				GameEngine.Instance.Quit();

                _bReqQuit = true;
                _nQuitDelay = System.Environment.TickCount;

                OnBtnSpeakCancelIvk();

                CancelInvoke();

            }
            catch
            {
                UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);
            }
        }

        void OnConfirmBackCancelIvk()
        {

        }

		//设置
        void OnBtnSettingIvk()
        {
            UISetting.Instance.Show(true);
			btn_chat.GetComponent<UIButton>().isEnabled=false;
			//GameObject.Find("Panel").GetComponent<AudioSource>().Play();

        }

		void OnBtnVoiceIvk()
		{
			bool show=!btn_chat_disabled.activeSelf;
			if(show)
			{
				NGUITools.soundVolume=0;
				GameObject.Find("Panel").GetComponent<AudioSource>().volume=0;
				btn_chat_disabled.SetActive(true);
				PlayerPrefs.SetString("game_music_switch", "off");
				//GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
				
			}else{
				btn_chat_disabled.SetActive(false);
				PlayerPrefs.SetString("game_music_switch","on");
				if(UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>()==null)
				{
					NGUITools.soundVolume=1.0f;
				}else{
                    NGUITools.soundVolume = UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>().value;
				}

                if (UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>() == null)
				{
					GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0.1f;
				}else
				{
                    GameObject.Find("Panel").GetComponent<AudioSource>().volume = UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>().value;
				}
				
			}
			
		}

		//准备
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
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
        }

		//规则
        void OnBtnRuleIvk()
        {
            bool bshow = !o_rules.active;
            o_rules.SetActive(bshow);
            if (bshow == true)
            {
                _nInfoTickCount = Environment.TickCount;
            }
        }

			
		void OnBtnQuitIvk()
		{
			OnConfirmBackOKIvk();
		}


        void OnBtnSpeakStartIvk()
        {
            /*if (GameEngine.Instance.MySelf.SmallSpeakerCount <= 0)
            {
                UIMsgBox.Instance.Show(true, "小喇叭道具已用完,请去商城购买!");
                return;
            }

            if (PlayerPrefs.GetString("game_speak_switch", "on") == "off")
            {
                UIMsgBox.Instance.Show(true, "您已设置关闭语音消息功能,不能使用语音消息!");
                return;
            }
            //调用安卓和IOS的开始录音函数
            o_speak_timer.GetComponent<UITimerBar>().SetTimer(6000);

            //暂停音乐
            GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
            SdkHelper.EndRecordSpeak("scene_game", "NONE");
            SdkHelper.StartRecordSpeak("scene_game", "OnRecordSpeakFinish");
*/
        }

        void OnBtnSpeakEndIvk()
        {
           /* if (GameEngine.Instance.MySelf.SmallSpeakerCount <= 0)
            {
                return;
            }
            if (PlayerPrefs.GetString("game_speak_switch", "on") == "off")
            {
                return;
            }

            //调用安卓和IOS的结束录音函数
            int nTimeDelay = o_speak_timer.GetComponent<UITimerBar>().GetTimedelay();
            o_speak_timer.GetComponent<UITimerBar>().SetTimer(0);

            if (nTimeDelay <= 800)
            {
                SdkHelper.EndRecordSpeak("scene_game", "OnRecordSpeakError");
            }
            else
            {
                SdkHelper.EndRecordSpeak("scene_game", "OnRecordSpeakFinish");
            }

            //恢复背景音乐
            if (PlayerPrefs.GetString("game_music_switch", "on") == "on")
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
            }
*/
        }

        void OnBtnSpeakCancelIvk()
        {
			/*
            if (PlayerPrefs.GetString("game_music_switch", "on") == "on")
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
            }

            //调用安卓和IOS的结束录音函数
            o_speak_timer.GetComponent<UITimerBar>().SetTimer(0);
            SdkHelper.EndRecordSpeak("scene_game", "OnRecordSpeakError");


        }

        void OnRecordSpeakError(string strSpeak)
        {
            if (PlayerPrefs.GetString("game_music_switch", "on") == "on")
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.Find("Panel").GetComponent<AudioSource>().Pause();
            }
            return;*/
        }

        void OnClearInfoIvk()
        {
            ClearAllInfo();
        }

        void OnPlayerInfoIvk(GameObject obj)
        {
            if (curGamePlatform != GamePlatform.NN_ForPC)
            {
                string[] strs = obj.name.Split("_".ToCharArray());
                int nChair = Convert.ToInt32(strs[2]);
                ShowUserInfo((byte)nChair, true);
            }
        }


        /////////////////////////////游戏特殊/////////////////////////////

        //不抢
        void OnBtnNotQiangIvk()
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_CALL_BANKER);
                packet.AddBool(false);
                GameEngine.Instance.Send(packet);

                SetUserClock(GameLogic.NULL_CHAIR, (uint)TimerDelay.NULL, TimerType.TIMER_NULL);

				byte bChair =(byte)GameEngine.Instance.MySelf.DeskStation;
				byte bSex = GameEngine.Instance.GetTableUserItem(bChair).Gender;
				PlayUserSound(GameSoundType.BUQING,bSex);

				o_qiang_buttons.SetActive(false);
				o_tips_text.GetComponent<UILabel>().text = "等待其他玩家抢庄...";
				o_tips_bg.SetActive (true);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //抢庄
        void OnBtnQiangIvk()
        {
            try
            {

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_CALL_BANKER);
                packet.AddBool(true);
                GameEngine.Instance.Send(packet);

				byte bChair =(byte)GameEngine.Instance.MySelf.DeskStation;
				byte bSex = GameEngine.Instance.GetTableUserItem(bChair).Gender;
				PlayUserSound(GameSoundType.QING,bSex);

				o_tips_text.GetComponent<UILabel>().text = "等待其他玩家抢庄...";
				o_tips_bg.SetActive (true);
                o_qiang_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, (uint)TimerDelay.NULL, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //1倍底注
        void OnBtnAddOneIvk()
        {

            try
            {
                if (_bBankerUser == GetSelfChair()) return;

                int lCurrentScore = (int)Math.Max(_lMaxChip / 10, 1);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] = lCurrentScore;

                //总注控件
                AppendChips(GetSelfChair(), lCurrentScore);
                //
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
                o_add_buttons.SetActive(false);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        //2倍底注
        void OnBtnAddTwoIvk()
        {
            try
            {
                if (_bBankerUser == GetSelfChair()) return;

                int lCurrentScore = (int)Math.Max(_lMaxChip / 5, 1);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] = lCurrentScore;

                //总注控件
                AppendChips(GetSelfChair(), lCurrentScore);
                //
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
                o_add_buttons.SetActive(false);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //4倍底注
        void OnBtnAddThreeIvk()
        {
            try
            {
                if (_bBankerUser == GetSelfChair()) return;

                int lCurrentScore = (int)Math.Max(_lMaxChip / 2, 1);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] = lCurrentScore;

                //总注控件
                AppendChips(GetSelfChair(), lCurrentScore);
                //
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
                o_add_buttons.SetActive(false);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //8倍底注
        void OnBtnAddFourIvk()
        {
            try
            {
                if (_bBankerUser == GetSelfChair()) return;

                int lCurrentScore = _lMaxChip;
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] = lCurrentScore;

                //总注控件
                AppendChips(GetSelfChair(), lCurrentScore);
                //
                o_option_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);
                o_add_buttons.SetActive(false);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //亮牌
        void OnBtnOpenCardIvk()
        {
            byte bOx = GameLogic.GetCardType(_bHandCardData[GetSelfChair()], 5);
            if (bOx > 0) {
				Debug.LogWarning ("亮牌" + Screen.height);
				//发送消息
				NPacket packet = NPacketPool.GetEnablePacket ();
				packet.CreateHead (MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);
				packet.Addbyte (1);

				GameEngine.Instance.Send (packet);

				o_option_buttons.SetActive (false);
				o_add_buttons.SetActive (false);
				o_qiang_buttons.SetActive (false);

				SetUserClock (GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
				CancelInvoke ();
			} 
			else {
	            //发送消息
	            NPacket packet = NPacketPool.GetEnablePacket();
	            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);
	            packet.Addbyte(0);
	
	            GameEngine.Instance.Send(packet);
	
	            o_option_buttons.SetActive(false);
	            o_add_buttons.SetActive(false);
	            o_qiang_buttons.SetActive(false);
	            o_ready_buttons.SetActive(false);
	
	            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
	            CancelInvoke();
			}
        }



        //无牛
        void OnBtnNoneCardIvk()
        {
//			Debug.LogWarning ("无牛");
//            byte bOx = GameLogic.GetCardType(_bHandCardData[GetSelfChair()], 5);
//            if (bOx > 0)
//            {
//                UIMsgBox.Instance.Show(true, "再看看吧,也许有牛哦!");
//                return;
//            }
//            //发送消息
//            NPacket packet = NPacketPool.GetEnablePacket();
//            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);
//            packet.Addbyte(0);
//
//            GameEngine.Instance.Send(packet);
//
//            o_option_buttons.SetActive(false);
//            o_add_buttons.SetActive(false);
//            o_qiang_buttons.SetActive(false);
//            o_ready_buttons.SetActive(false);
//
//            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
//            CancelInvoke();

			OnBtnTipIvk ();
        }

        //提示
        void OnBtnTipIvk()
        {

            byte bOx = GameLogic.GetCardType(_bHandCardData[GetSelfChair()], 5);

            //发送消息
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);

            if (bOx > 0)
            {
				//packet.Addbyte(1);

				o_player_cards[0].GetComponent<UICardControl>().ResetAllShoot();
				byte[] temp = new byte[5];
				Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, temp, 0, 5);

				byte[] bTemp = new byte[5];
				byte bSum=0;
				for (byte i=0;i<5;i++)
				{
					bTemp[i]=GameLogic.GetCardLogicValue(temp[i]);
					bSum+=bTemp[i];
				}

				for (int i=0;i<4;i++)
				{
					for (int j=i+1;j< 5 ;j++)
					{
						if((bSum-bTemp[i]-bTemp[j])%10==0)
						{
							for(int k = 0;k<5;k++)
							{
								if(k != i && k != j)
								{
									o_player_cards[0].transform.FindChild("card_" + k.ToString()).gameObject.GetComponent<UICard>().OnClick(); 
									OnCardClick();
								}
							}
							return;
						}
					}
				}
			}
			else
			{
				packet.Addbyte(0);
				GameEngine.Instance.Send(packet);
				
				o_option_buttons.SetActive(false);
				o_add_buttons.SetActive(false);
				o_qiang_buttons.SetActive(false);
				o_ready_buttons.SetActive(false);
				
				SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
				CancelInvoke();
			}
			
			
		}
		#endregion
		
		
		#region ##################控件事件#######################

        /////////////////////////////游戏通用/////////////////////////////

        //扑克控件点击事件
        void OnCardClick()
        {

            //计算牌值,显示亮牌按钮
            byte[] shoots = new byte[5];
            Array.Clear(shoots, 0, 5);
            byte cnt = 0;
            o_player_cards[0].GetComponent<UICardControl>().GetShootCard(ref shoots, ref cnt);
            if (cnt == 2)  //选中两张牌
            {
                byte[] temp = new byte[5];
                Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, temp, 0, 5);

                byte btempCount = 5;

				//将选中的两张的从temp[]中去掉
                GameLogic.RemoveCard(shoots, 2, ref temp, ref btempCount);

                int nValue0 = GameLogic.GetCardValue(temp[0]);
                if (nValue0 > 10) nValue0 = 10;

                int nValue1 = GameLogic.GetCardValue(temp[1]);
                if (nValue1 > 10) nValue1 = 10;

                int nValue2 = GameLogic.GetCardValue(temp[2]);
                if (nValue2 > 10) nValue2 = 10;

                if ((nValue0 + nValue1 + nValue2) % 10 == 0)
                {
                    o_btn_opencard.GetComponent<UIButton>().isEnabled = true;
                    return;
                }
            }
            else if (cnt == 3)
            {
                int nValue0 = GameLogic.GetCardValue(shoots[0]);
                if (nValue0 > 10) nValue0 = 10;

                int nValue1 = GameLogic.GetCardValue(shoots[1]);
                if (nValue1 > 10) nValue1 = 10;

                int nValue2 = GameLogic.GetCardValue(shoots[2]);
                if (nValue2 > 10) nValue2 = 10;

                if ((nValue0 + nValue1 + nValue2) % 10 == 0)
                {
                    o_btn_opencard.GetComponent<UIButton>().isEnabled = true;
                    return;
                }

            }
			else if(cnt == 5)
			{
				if( GameLogic.GetCardType(_bHandCardData[GetSelfChair()], 5) == 0 )
				{
					o_btn_opencard.GetComponent<UIButton>().isEnabled = true;
					return;
				}
			}

            o_btn_opencard.GetComponent<UIButton>().isEnabled = false;

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
			Debug.LogWarning("end~~~~:"+_bTimerType);
            try
            {
                switch (_bTimerType)
                {
                    case TimerType.TIMER_READY:
                        {
                            OnConfirmBackOKIvk();
                            break;
                        }
                    case TimerType.TIMER_QIANG:
                        {
                            OnBtnNotQiangIvk();
                            break;
                        }
                    case TimerType.TIMER_CHIP:
                        {
                            OnBtnAddOneIvk();
                            break;
                        }
                    case TimerType.TIMER_OPEN:
                        {
                        //OnBtnTipIvk();
//						byte bOx = GameLogic.GetCardType(_bHandCardData[GetSelfChair()], 5);
//						if (bOx > 0)
//						{
//							OnBtnOpenCardIvk();
//						}else{
//							OnBtnNoneCardIvk();
//						}						
               			 break;
                        }
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OpenHandCardData(byte bchair, byte[] cards, byte count)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.OpenCardData(cards, count);
                GameLogic.SortCardList(ref cards, count);
                ctr.ArrayHandCards(cards, count);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void SendHandCard(byte bchair, byte[] cards, byte cardcount)
        {
            /*try
            {*/
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.ClearCards();
                ctr.AppendHandCard(bViewID, cards, cardcount);
           /* }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }*/
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
               // UIMsgBox.Instance.Show(true, ex.Message);
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
				//UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void AppendChips(byte bchair, int nUserChips)
        {
            try
            {
                int lCurrentScore = (int)Math.Max(_lCellScore / 8, 1);
                byte bViewID = ChairToView(bchair);
                UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();

                if (lCurrentScore > nUserChips)
                {
                    lCurrentScore = nUserChips;
                }

                ctr.AddChips(bViewID, nUserChips, lCurrentScore);

                Debug.Log(nUserChips + "/" + lCurrentScore);

                UpdateGameInfoView();

                PlayGameSound(SoundType.CHIP);
            }
            catch (Exception ex)
            {
				// UIMsgBox.Instance.Show(true, ex.Message);
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
				// UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            try
            {
                if (chair != GameLogic.NULL_CHAIR)
                {
                    _bTimerType = timertype;
                    o_player_clock.GetComponent<UIClock>().SetTimer(time * 1000);
                }
                else
                {
                    o_player_clock.GetComponent<UIClock>().SetTimer(0);
                    o_player_clock.SetActive(false);
                }

            }
            catch (Exception ex)
            {
				//UIMsgBox.Instance.Show(true, ex.Message);
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
				//UIMsgBox.Instance.Show(true, ex.Message);
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
                        _bPlayStatus[i] = (byte)PlayerState.PLAY;
                       
                        //nick 
                        if (userdata.VipLevel > 0)
                        {
                          //  o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
                        }
                        else
                        {
                            //o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(0.24f, 0.17f, 0.11f);
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

                        //o_player_frame[bViewId].SetActive(true);
						if(curGamePlatform == GamePlatform.NN_ForPC)
						{
							ShowUserInfo(bViewId, true);

						}else
						{
							o_player_money[bViewId].SetActive(true);
							o_player_bg[bViewId].SetActive(false);
							
							o_player_money[bViewId].GetComponent<UINumber>().SetNumber(GameEngine.Instance.GetTableUserItem((ushort)i).Money);
						}

       					//OnPlayerInfoIvk(GameObject obj)
						
                    }
                    else
                    {
                        _bPlayStatus[i] = (byte)PlayerState.NULL;
                        //nick
                        o_player_nick[bViewId].GetComponent<UILabel>().text = "";
                        //face
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
                        //p
                        o_player_option[bViewId].SetActive(false);
                        //
                        o_player_money[bViewId].SetActive(false);

                        o_player_frame[bViewId].SetActive(false);

						if(curGamePlatform == GamePlatform.NN_ForMobile)
						{
							o_player_bg[i].SetActive(false);
						}
                    }


                }

                ShowInfoBar();

            }
            catch (Exception ex)
            {
                //UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        void ShowInfoBar()
        {
            if (GameEngine.Instance.MySelf != null)
            {
                o_self_money.GetComponent<UINumber>().SetNumber(GameEngine.Instance.MySelf.Money);

                /*GameEngine.Instance.MySelf.Exp = (uint)GameEngine.Instance.MySelf.Self.lExperience;
                GameEngine.Instance.MySelf.GameScore = (uint)GameEngine.Instance.MySelf.Self.lScore;

                o_btn_speak_count.GetComponent<UILabel>().text = "x" + GameEngine.Instance.MySelf.SmallSpeakerCount.ToString();
                o_self_money.GetComponent<UINumber>().SetNumber((int)GameEngine.Instance.MySelf.GameScore);*/

            }
            else
            {
                o_self_money.GetComponent<UINumber>().SetNumber(0);
            }
        }

        void ShowUserInfo(byte bViewID, bool bshow)
        {
            if (bViewID == GameLogic.NULL_CHAIR)
            {
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_info[i].SetActive(false);
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
            //byte wViewChairID = (byte)((ChairID + GameLogic.GAME_PLAYER - GameEngine.Instance.MySelf.Self.ChairID));
			byte wViewChairID = (byte)((ChairID + GameLogic.GAME_PLAYER - GameEngine.Instance.MySelf.DeskStation));
            return (byte)(wViewChairID % GameLogic.GAME_PLAYER);
        }

        byte ViewToChair(byte ViewID)
        {
            //byte wChairID = (byte)((ViewID + GameEngine.Instance.MySelf.Self.ChairID) % GameLogic.GAME_PLAYER );
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

        void ExitGame()
        {
            _bStart = false;
			GameEngine.Instance.Quit();
        }

        void PlayGameSound(SoundType sound)
        {
            float fvol = NGUITools.soundVolume;
            NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
        }

        void PlayUserSound(GameSoundType sound, byte bGender)
        {
			float  count= UnityEngine.Random.Range(0,2.0f);
            float fvol = NGUITools.soundVolume;
            if (bGender == (byte)UserGender.Woman)
            {
				if(count<=1.0f)
				{
					NGUITools.PlaySound(_WomanSound[(int)sound], fvol, 1);
				}else{
					NGUITools.PlaySound(_WomanSound2[(int)sound], fvol, 1);
				}
                
            }
            else
            {
				if(count<=1.0f)
				{
					NGUITools.PlaySound(_ManSound[(int)sound], fvol, 1);
				}else{
					NGUITools.PlaySound(_ManSound2[(int)sound], fvol, 1);
				}
               
            }
        }

        void ShowUserSpeak(uint uid)
        {
			/*
            byte bchairID = (byte)GameEngine.Instance.MySelf.UserIdToChairId(uid);
            byte bViewID = ChairToView(bchairID);
            if (bchairID != GetSelfChair())
            {
                o_user_speak[bViewID].GetComponent<UISpeak>().Play("scene_game", "OnSpeakPlay", uid);
            }*/

        }

        void OnRecordSpeakFinish(string strSpeak)
        {
			/*
            //上传网络
            string strFile = Application.persistentDataPath + "/" + strSpeak;
            StartCoroutine(UpLoadSpeak(strFile));
*/
        }

        void OnSpeakPlay(string str)
        {
            /*string[] strs = str.Split("`".ToCharArray());

            int nTime = Convert.ToInt32(strs[0]);
            uint Uid = Convert.ToUInt32(strs[1]);

            byte bViewID = ChairToView((byte)GameEngine.Instance.MySelf.UserIdToChairId(Uid));

            if (nTime < 1000)
            {
                nTime = 6000;
            }

            o_user_speak[bViewID].GetComponent<UISpeak>().SetTimer(nTime);*/

        }

        IEnumerator UpLoadSpeak(string strSpeak)
        {
			yield return null;
			/*
            string normalUrl = Config.Instance.WebSite + "/extrafunc.aspx";

            FileStream fileStream = new FileStream(strSpeak, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();

            WWWForm form = new WWWForm();
            form.AddField("option", "uploadspeak");
            form.AddField("userid", GameEngine.Instance.MySelf.ID.ToString());
            form.AddField("userpass", GameEngine.Instance.MySelf.Password);
            form.AddField("props", "SMALL");
            form.AddBinaryData("speak", bytes);
            WWW www = new WWW(normalUrl.ToString(), form);
            yield return www;

            if (www.text == "-1" || www.error != null)
            {
                UIMsgBox.Instance.Show(true, "您的网络环境太不给力,系统为您屏蔽语音消息");
                PlayerPrefs.SetString("game_speak_switch", "off");
            }
            else
            {
                GameEngine.Instance.SendChatMessage(GameLogic.NULL_CHAIR, "", 255);
                GameEngine.Instance.MySelf.SmallSpeakerCount--;
                if (GameEngine.Instance.MySelf.SmallSpeakerCount < 0)
                {
                    GameEngine.Instance.MySelf.SmallSpeakerCount = 0;
                }

                o_btn_speak_count.GetComponent<UILabel>().text = "x" + GameEngine.Instance.MySelf.SmallSpeakerCount.ToString();

                //本地预播放
                byte bViewID = ChairToView(GetSelfChair());
                o_user_speak[bViewID].GetComponent<UISpeak>().PlayLocal("scene_game", "OnSpeakPlay", GameEngine.Instance.MySelf.ID);


            }*/
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
                o_player_chat[bViewID].SetActive(true);
                o_player_chat[bViewID].GetComponentInChildren<UILabel>().text = strMsg;
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
				if(curGamePlatform != GamePlatform.NN_ForPC)
				{
					o_info[i].SetActive(false);
				}
//                o_player_chat[i].SetActive(false);

            }
            o_rules.SetActive(false);
        }




        /////////////////////////////游戏特殊/////////////////////////////
		/// 
		private void ShowResultView()
		{
			ShowResultView(true);
		}

        void ShowResultView(bool bshow)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            o_result.SetActive(bshow);

            //金币限制检测


            if (bshow)
            {
                Invoke("CloseResultView", 5.0f);
            }


            /*
            if(GameEngine.Instance.MySelf.Self.lScore <20*_lCellScore)
            {
               UIInfoBox.Instance.Show(true,"您的乐豆不足,不能继续游戏!");
               Invoke("OnConfirmBackOKIvk",1.0f);
            }
            else
            {
                if(bshow)
                {
                    Invoke("CloseResultView",5.0f);
                }
            }
            */


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
                o_player_cards[i].SetActive(false);
                o_player_money[i].SetActive(false);
                o_player_chip[i].SetActive(false);
				o_player_info[i].SetActive(false);
                o_player_type[i].SetActive(false);
            }
            //
            UpdateGameInfoView();

            //
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            //

            SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY);

            o_ready_buttons.SetActive(true);

            CancelInvoke();
        }

        void UpdateGameInfoView()
        {
            //
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {

                byte bViewId = ChairToView(i);

                if (_bPlayStatus[i] > 0 && _lTableScore[i] > 0)
                {
					o_player_info[bViewId].SetActive(true);
                    o_player_chip[bViewId].SetActive(true);
                    o_player_chip[bViewId].GetComponent<UINumber>().SetNumber(_lTableScore[i]);
                }
                else
                {
                    o_player_chip[bViewId].SetActive(false);
					o_player_info[bViewId].SetActive(false);
                }
            }

        }

        void SetBanker()
        {
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_flag[i].SetActive(false);
            }


            if (_bBankerUser != GameLogic.NULL_CHAIR)
            {
                byte bViewID = ChairToView(_bBankerUser);
                o_player_flag[bViewID].SetActive(true);
                o_player_flag[bViewID].GetComponent<UISprite>().spriteName = "banker";
            }
        }

        void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

		//设置抢庄标志
        void SetCallBanker(byte ChairID, byte bQiang)
        {
            byte bViewID = ChairToView(ChairID);

            if (_bCallStatus[ChairID] == 1)
            {
                o_player_option[bViewID].SetActive(true);
                o_player_option[bViewID].GetComponent<UISprite>().spriteName = "btn_desc_qiang";
				//o_player_option[bViewID].GetComponent<UISprite>().spriteName = "nn_dialog_qiang";
            }
            else if (_bCallStatus[ChairID] == 0)
            {
                o_player_option[bViewID].SetActive(true);
				o_player_option[bViewID].GetComponent<UISprite>().spriteName = "btn_not_qiang";
				//o_player_option[bViewID].GetComponent<UISprite>().spriteName = "nn_dialog_buqiang";
            }
            else
            {
                o_player_option[bViewID].SetActive(false);
            }
        }
        void SetCardType(byte ChairID, byte bNx)
        {
            byte bViewID = ChairToView(ChairID);
            if (bNx != 0xFF)
            {
                //计算NX
                //byte bType = GameLogic.GetCardType(_bHandCardData[ChairID],5);
                //
                if (bNx > 10) bNx = 10;

                o_player_type[bViewID].SetActive(true);
                o_player_type[bViewID].GetComponent<UISprite>().spriteName = "n_" + bNx.ToString();
            }
            else
            {
                o_player_type[bViewID].SetActive(false);
            }
        }
        void SetChipButton(bool bShow, int nMaxChips)
        {

            o_add_buttons.SetActive(bShow);
            if (bShow)
            {
                Debug.Log(nMaxChips.ToString());
                o_btn_chip_1.GetComponentInChildren<UINumber>().SetNumber((int)(Math.Max(nMaxChips / 10, 1)));
                o_btn_chip_2.GetComponentInChildren<UINumber>().SetNumber((int)(Math.Max(nMaxChips / 5, 1)));
                o_btn_chip_3.GetComponentInChildren<UINumber>().SetNumber((int)(Math.Max(nMaxChips / 2, 1)));
                o_btn_chip_4.GetComponentInChildren<UINumber>().SetNumber((int)(Math.Max(nMaxChips, 1)));
            }
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