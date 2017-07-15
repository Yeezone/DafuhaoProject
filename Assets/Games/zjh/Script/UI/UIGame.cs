using UnityEngine;
using System.Collections;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.ZJH
{

    #region ##################结构定义#######################

    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_READY = 1,
        TIMER_QIANG = 2,
        TIMER_CHIP = 3,
        TIMER_COMPARE = 4,

    };

    public enum SoundType
    {
        READY = 0,
        START = 1,
        SENDCARD = 2,
        CHIP = 3,
        COMP = 4,
        GIVEUP = 5,
        LOOK = 6,
        WIN = 7,
        LOSE = 8,
        CLOCK = 9
    };

    public enum GameSoundType
    {

    };

	public enum GamePlatform	//游戏平台
	{
		ZJH_ForPC,
		ZJH_ForMobile

	}
    #endregion


    public class UIGame : MonoBehaviour
    {

        #region ##################变量定义#######################


		public GamePlatform curGamePlatform; 	//当前游戏平台

        //通用控件
        // GameObject      o_dlg_tips          = null;
        private GameObject o_speak_timer = null;
        private GameObject o_btn_speak = null;
        private GameObject o_btn_speak_count = null;


        private GameObject[] o_player_clock = new GameObject[GameLogic.GAME_PLAYER];
		private GameObject[] o_player_frame = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_option = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_chat = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_chip = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_c_desc = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_cards = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_cheap = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_type = new GameObject[GameLogic.GAME_PLAYER];

        private GameObject[] o_player_face = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_nick = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_player_flag = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER];

        private GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER];
		private GameObject[] o_info_bg = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER];
		private GameObject[] o_info_desc_score = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER];


        private GameObject o_result = null;
        private GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER];
		private GameObject[] o_result_card_type = new GameObject[GameLogic.GAME_PLAYER];

        private GameObject[] o_result_exp = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_result_type = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_result_win = new GameObject[GameLogic.GAME_PLAYER];

        private GameObject[] o_result_nick = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject[] o_result_bean = new GameObject[GameLogic.GAME_PLAYER];

        private GameObject o_ready_buttons = null;
        private GameObject o_btn_ready = null;
        private GameObject o_btn_quit = null;
		private GameObject o_set_setting=null;

		//结算界面信息
		private UILabel desc_win=null;





        //游戏特殊
        private GameObject o_option_buttons = null;
        private GameObject o_btn_seecard = null;
        private GameObject o_btn_compcard = null;
        private GameObject o_btn_followchip = null;
        private GameObject o_btn_giveup = null;
        private GameObject o_btn_cheapcard = null;
        private GameObject o_btn_addchip = null;
        private GameObject o_btn_opencard = null;


        private GameObject o_add_buttons = null;
        private GameObject o_btn_chip_1 = null;
        private GameObject o_btn_chip_2 = null;
        private GameObject o_btn_chip_3 = null;
        private GameObject o_btn_cancel = null;

        private GameObject o_qiang_buttons = null;
        private GameObject o_btn_qiang = null;
        private GameObject o_btn_notqiang = null;

        //信息框
        private GameObject o_room_cell_score = null;
        private GameObject o_room_base_score = null;
        private GameObject o_room_max_score = null;
        private GameObject o_room_limit_score = null;


        //比牌选择
        private GameObject o_dlg_comp_select = null;
        private GameObject[] o_btn_comp_user = new GameObject[GameLogic.GAME_PLAYER];
        private GameObject o_dlg_comp_effect = null;

        private GameObject o_dlg_rules = null;
        private GameObject o_dlg_chips = null;
        private GameObject o_current_time = null;
        private GameObject o_self_money = null;

        //通用数据
        private static bool _bStart = false;
        private static TimerType _bTimerType = TimerType.TIMER_NULL;

        //锅底分数
        private static int _lBaseScore = 0;
        //底注
        private static int _lCellScore = 0;
        //单元上限
        private static int _lMaxCellScore = 0;
        //封顶
        private static int _lMaxTopScore = 0;
        //当前倍数
        private static int _lCurrentTimes = 0;
        //当前下注
        private static int _lCurrentChipScore = 0;
        //最小加注
        private static int _lMinChipScore = 0;
        //当前局最大流水
        private static int _lLimitScore = 0;
        //当前局注次数
        private static int _number = 0;

        //看牌动作
        private static bool[] _bMingZhu = new bool[GameLogic.GAME_PLAYER];
        //诈牌动作
        private static bool[] _bCheapCard = new bool[GameLogic.GAME_PLAYER];
        //游戏状态
        private static byte[] _bPlayStatus = new byte[GameLogic.GAME_PLAYER];
        //下注数目
        private static int[] _lTableScore = new int[GameLogic.GAME_PLAYER];
        //桌面总注
        private static int _lTotalScore = 0;

        private static byte[][] _bHandCardData = new byte[GameLogic.GAME_PLAYER][];
        private static byte[] _bHandCardCount = new byte[GameLogic.GAME_PLAYER];
        private static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER];
        private static byte _bCurrentUser = GameLogic.NULL_CHAIR;
        private static byte _bBankerUser = GameLogic.NULL_CHAIR;
        private static int _nInfoTickCount = 0;

        private static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER];
        private static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER];
        private static byte[] _nEndUserCardType = new byte[GameLogic.GAME_PLAYER];

        private static int _nQuitDelay = 0;
        private static bool _bReqQuit = false;
        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[20];
        public AudioClip[] _ManSound = new AudioClip[20];


        public static GameObject btn_chat_disabled = null;
        public static GameObject btn_chat = null;


	//	public UIChipControl ctl;

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

        private void Awake()
        {
            try
            {
                //通用
                //o_dlg_tips          = GameObject.Find("scene_game/dlg_tips");
                o_speak_timer = GameObject.Find("scene_game/dlg_speak_timer");
                o_btn_speak = GameObject.Find("scene_game/btn_speak");
                o_btn_speak_count = GameObject.Find("scene_game/btn_speak/lbl_count");
				o_set_setting=GameObject.Find ("scene_setting");

				btn_chat_disabled=GameObject.Find ("scene_game/dlg_title_bar/btn_chat/disabled");
				btn_chat=GameObject.Find("scene_game/dlg_title_bar/btn_chat");

				desc_win=GameObject.Find("scene_game/dlg_result/desc_win").GetComponent<UILabel>();
				//ctl=new UIChipControl();

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
					o_player_clock[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_clock_bar");
                    o_player_option[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_option");
                    o_player_chat[i] = GameObject.Find("scene_game/dlg_chat_msg_" + i.ToString());
                    o_player_face[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_user_face");
					o_player_frame[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_frame");

                    o_player_chip[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_chips");
                    o_player_c_desc[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_chips");
                    o_player_nick[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_nick");
                    o_player_flag[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_flag");
                    o_player_cheap[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_cheap");

					o_player_type[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_type");
                    o_user_speak[i] = GameObject.Find("scene_game/ctr_speak_" + i.ToString());

                    o_info[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString());
                    o_info_nick[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_nick");
					o_info_bg[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/sp_bg");
                    o_info_lvl[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lvl");
                    o_info_id[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_id");
                    o_info_score[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_score");
					o_info_desc_score[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/desc_score");
                    o_info_win[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_win");
                    o_info_lose[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lose");
                    o_info_run[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_run");

                    o_player_cards[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_hand_cards");

                    o_result_score[i] = GameObject.Find("scene_game/dlg_result/lbl_score_" + i.ToString());
					o_result_card_type[i] = GameObject.Find("scene_game/dlg_result/sp_card_type_" + i.ToString());
                    o_result_exp[i] = GameObject.Find("scene_game/dlg_result/lbl_bs_" + i.ToString());
                    o_result_type[i] = GameObject.Find("scene_game/dlg_result/sp_type_" + i.ToString());
                    o_result_win[i] = GameObject.Find("scene_game/dlg_result/sp_win_" + i.ToString());
				
                    o_result_nick[i] = GameObject.Find("scene_game/dlg_result/lbl_user_" + i.ToString());
                    o_result_bean[i] = GameObject.Find("scene_game/dlg_result/sp_bean_" + i.ToString());

                    o_btn_comp_user[i] = GameObject.Find("scene_game/dlg_comp_select/btn_select_" + i.ToString());



                }

                //准备
                o_ready_buttons = GameObject.Find("scene_game/dlg_ready_buttons");
                o_btn_ready = GameObject.Find("scene_game/dlg_ready_buttons/btn_ready");
                o_btn_quit = GameObject.Find("scene_game/dlg_ready_buttons/btn_quit");
                o_result = GameObject.Find("scene_game/dlg_result");
				o_result.SetActive(false);


                //游戏
                o_option_buttons = GameObject.Find("scene_game/dlg_option_buttons");
                o_btn_seecard = GameObject.Find("scene_game/dlg_option_buttons/btn_see");
                o_btn_compcard = GameObject.Find("scene_game/dlg_option_buttons/btn_comp");
                o_btn_followchip = GameObject.Find("scene_game/dlg_option_buttons/btn_follow");
                o_btn_giveup = GameObject.Find("scene_game/dlg_option_buttons/btn_giveup");
                o_btn_cheapcard = GameObject.Find("scene_game/dlg_option_buttons/btn_cheap");
                o_btn_addchip = GameObject.Find("scene_game/dlg_option_buttons/btn_add");
                o_btn_opencard = GameObject.Find("scene_game/dlg_option_buttons/btn_open");


                o_add_buttons = GameObject.Find("scene_game/dlg_add_buttons");
                o_btn_chip_1 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_1");
                o_btn_chip_2 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_2");
                o_btn_chip_3 = GameObject.Find("scene_game/dlg_add_buttons/btn_chip_3");


				o_btn_cancel = GameObject.Find("scene_game/dlg_add_buttons/btn_cancel");

                o_qiang_buttons = GameObject.Find("scene_game/dlg_qiang_buttons");
                o_btn_qiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_qiang");
                o_btn_notqiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_notqiang");

                o_room_cell_score = GameObject.Find("scene_game/dlg_bottom_bar/lbl_dizhu");
                o_room_base_score = GameObject.Find("scene_game/dlg_bottom_bar/lbl_guodi");
                o_room_max_score = GameObject.Find("scene_game/dlg_bottom_bar/lbl_dingzhu");
                o_room_limit_score = GameObject.Find("scene_game/dlg_bottom_bar/lbl_fengding");


                o_dlg_comp_effect = GameObject.Find("scene_game/dlg_comp_effect");
                o_dlg_comp_select = GameObject.Find("scene_game/dlg_comp_select");
                o_dlg_rules = GameObject.Find("scene_game/dlg_rules");
                o_dlg_chips = GameObject.Find("scene_game/ctr_chips");

                o_current_time = GameObject.Find("scene_game/dlg_title_bar/lbl_curr_time");
                o_self_money = GameObject.Find("scene_game/dlg_bottom_bar/lbl_money");


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void InitGameView()
        {

            //Data
            _bStart = false;
            _bTimerType = TimerType.TIMER_READY;
            _lCellScore = 0;

            _bCurrentUser = GameLogic.NULL_CHAIR;

            _bBankerUser = GameLogic.NULL_CHAIR;

            _nInfoTickCount = Environment.TickCount;

            Array.Clear(_bUserTrustee, 0, 3);

            _nQuitDelay = 0;
            _bReqQuit = false;
            _nInfoTickCount = Environment.TickCount;
            //UI
            //o_dlg_tips.SetActive(false);

            o_btn_quit.SetActive(false);
           // o_speak_timer.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {

                o_player_clock[i].SetActive(false);
				o_player_frame[i].SetActive(false);
                o_player_option[i].SetActive(false);
                o_player_chip[i].SetActive(false);
                o_player_c_desc[i].SetActive(false);


                o_player_chat[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_cheap[i].SetActive(false);
                o_player_type[i].SetActive(false);

                o_player_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
                o_player_nick[i].GetComponent<UILabel>().text = "";
                o_player_flag[i].GetComponent<UISprite>().spriteName = "blank";
                o_result_card_type[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_type[i].GetComponent<UISprite>().spriteName = "blank";

				o_info[i].SetActive(false);
				o_info_bg[i].SetActive(false);
				o_info_desc_score[i].SetActive(false);
                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";



                _bHandCardData[i] = new byte[3];
                _bPlayStatus[i] = (byte) PlayerState.NULL;
                Array.Clear(_bHandCardData[i], 0, 3);
                Array.Clear(_bHandCardCount, 0, 3);

                _bMingZhu[i] = false;
                _bCheapCard[i] = false;
                _lTableScore[i] = 0;

                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;

            }

            o_ready_buttons.SetActive(false);
            o_qiang_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);
            o_add_buttons.SetActive(false);
            o_dlg_comp_effect.SetActive(false);
            o_dlg_comp_select.SetActive(false);
            o_dlg_rules.SetActive(false);
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();


        }

        private void ResetGameView()
        {

			//把准备隐藏
			foreach(GameObject obj in o_player_option)
			{
				obj.SetActive(false);
			}
            _bTimerType = TimerType.TIMER_READY;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _nInfoTickCount = Environment.TickCount;
            Array.Clear(_bUserTrustee, 0, 3);

            //UI
            //o_dlg_tips.SetActive(false);

         	//o_btn_quit.SetActive(false);
         	//o_speak_timer.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_clock[i].SetActive(false);
                o_player_option[i].SetActive(false);

				//o_player_frame[i].SetActive(false);
                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_cards[i].SetActive(false);
                o_player_cheap[i].SetActive(false);
                o_player_type[i].SetActive(false);

                o_player_flag[i].GetComponent<UISprite>().spriteName = "blank";
                o_result_card_type[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_type[i].GetComponent<UISprite>().spriteName = "blank";

				o_info[i].SetActive(false);
				o_info_nick[i].GetComponent<UILabel>().text = "";
				o_info_lvl[i].GetComponent<UILabel>().text = "";
				o_info_id[i].GetComponent<UILabel>().text = "";
				o_info_score[i].GetComponent<UILabel>().text = "";
				o_info_win[i].GetComponent<UILabel>().text = "";
				o_info_lose[i].GetComponent<UILabel>().text = "";
				o_info_run[i].GetComponent<UILabel>().text = "";

               
				//如果是PC端九显示玩家的信息
				if(curGamePlatform == GamePlatform.ZJH_ForPC)
				{
				
					byte bViewId = ChairToView((byte)i);
					PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((uint)i);

					if (userdata != null)
					{
						ShowUserInfo(bViewId, true);
					}
					
				}

                _bHandCardData[i] = new byte[3];
                Array.Clear(_bHandCardData[i], 0, 3);
                Array.Clear(_bHandCardCount, 0, 3);

                _bMingZhu[i] = false;
                _bCheapCard[i] = false;
                _lTableScore[i] = 0;

                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;
                _nEndUserCardType[i] = 0;
            }
		
			//o_player_clock.SetActive(true);


            o_ready_buttons.SetActive(false);
            o_qiang_buttons.SetActive(false);
            o_option_buttons.SetActive(false);
            o_result.SetActive(false);
            o_add_buttons.SetActive(false);
            o_dlg_comp_effect.SetActive(false);
            o_dlg_comp_select.SetActive(false);
            o_dlg_rules.SetActive(false);
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();

            Resources.UnloadUnusedAssets();
            GC.Collect();

			//声音按钮
			btn_chat.GetComponent<UIButton>().isEnabled=true;

        }

        #endregion


        #region ##################引擎调用#######################

        private void Start()
        {
            try
            {
                InitGameView();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
			GameObject.Find("Panel").GetComponent<AudioSource>().volume=0.3f;
			GameObject.Find("Panel").GetComponent<AudioSource>().Play();
        }

        private void FixedUpdate()
        {
            o_current_time.GetComponent<UILabel>().text = System.DateTime.Now.ToString("hh:mm:ss");
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


        private void Update()
        {

			//保证按钮不会丢失
			if(o_option_buttons.active)
			{
				if(!o_add_buttons.active)
				{
					o_btn_addchip.SetActive(true);
				}
				if( !o_btn_addchip.active)
				{
					o_add_buttons.SetActive(true);
				}
			}
			//判断按钮是否可用
			if( o_add_buttons.active){
                
				if (_bMingZhu[GetSelfChair()] == true)
				{

                    if ((_lCurrentChipScore + _lCellScore * 2) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 2) * 2 > _lMinChipScore)
					{
					    o_btn_chip_1.GetComponent<UIButton>().isEnabled=false;

                    }
                    else
                        o_btn_chip_1.GetComponent<UIButton>().isEnabled = true;

                    if ((_lCurrentChipScore + _lCellScore * 5) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 5) * 2 > _lMinChipScore)
                    {
                        o_btn_chip_2.GetComponent<UIButton>().isEnabled = false;
                    }
                    else
                    {
                        o_btn_chip_2.GetComponent<UIButton>().isEnabled = true;
                    }

                    if ((_lCurrentChipScore + _lCellScore * 10) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 10) * 2 > _lMinChipScore)
                    {
                        o_btn_chip_3.GetComponent<UIButton>().isEnabled = false;
                    }
                    else
                    {
                        o_btn_chip_3.GetComponent<UIButton>().isEnabled = true;
                    }

				}
			}


            /*if (GameEngine.Instance.MyUser.IsDisconnectFromServer && _bStart == true)
            {

                CancelInvoke();

                _bStart = false;

                PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MyUser.ServerUsed.ServerID);

                UIMsgBox.Instance.Show(true, "您的网络不给力哦,掉线了");

                Invoke("GoLogin", 3.0f);
            }*/
        }

        #endregion


        #region ##################框架消息#######################

        //框架消息入口
        private void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
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
        private void OnTableUserEvent(TableEvents tevt, uint userid, object data)
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

                        //Invoke("CheckScoreLimit",5.0f);

                        //CheckScoreLimit();


                        //金币限制检测
                        /*
                        if(GameEngine.Instance.MyUser.Self.lScore <_lCellScore*50)
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
                case TableEvents.GAME_LOST:
                    {
                        UIMsgBox.Instance.Show(true, GameMsg.MSG_CM_008);
                        Invoke("OnConfirmBackOKIvk", 5.0f);
                        break;
                    }
				case TableEvents.USER_NULL:
					{
						Invoke("OnConfirmBackOKIvk",2.0f);
						break;
					}
            }

        }

        //游戏设置消息处理函数
        private void OnGameOptionResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
               // GameEngine.Instance.MyUser.AllowLookon = packet.GetByte();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏场景消息处理函数
        private void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {

            switch (bGameStatus)
            {
                case (byte) GameLogic.GS_WK_FREE:
                    {
                        SwitchFreeSceneView(packet);
                        break;
                    }
                case (byte) GameLogic.GS_WK_PLAYING:
                    {
                        SwitchPlaySceneView(packet);
                        break;
                    }
            }
        }

        //用户准备消息处理函数
        private void OnUserReadyResp(NPacket packet)
        {
            PlayGameSound(SoundType.READY);
            return;
        }

        //用户聊天消息处理函数
        private void OnUserChatResp(NPacket packet)
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

        #endregion


        #region ##################游戏消息#######################

        //游戏消息入口
        private void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            if (_bReqQuit == true) return;
            //扎金花 游戏状态
            switch (subcmd)
            {

                case SubCmd.SUB_S_QIANG_START:
                    {
                        //抢庄开始
                        OnGameQiangStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_QIANG_BANKER:
                    {
                        //抢庄
                        OnUserQiangBankerResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_START:
                    {
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GIVE_UP:
                    {
                        //消息处理
                        OnUserGiveUpResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_PLAYER_EXIT:
                    {
                        //消息处理
                        OnUserPlayerExitResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:
                    {
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_ADD_SCORE:
                    {
                        //消息处理
                        OnUserAddScoreResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_COMPARE_CARD:
                    {

                        OnUserCompareCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_OPEN_CARD:
                    {
                        //消息处理
                        OnUserOpenCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_LOOK_CARD:
                    {
                        //消息处理
                        OnUserLookCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_CHEAP_CARD:
                    {
                        //用户诈牌
                        OnUserCheapCardResp(packet);
                        break;
                    }
            }
        }

        //游戏消息处理函数
        private void OnGameMessageResp(NPacket packet)
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

        //游戏开始
        private void OnGameStartResp(NPacket packet)
        {
            /*
            LONG                             lMaxCellScore;                      //最大下注
            LONG                             lCellScore;                         //单元下注
            LONG                             lCurrentTimes;                      //当前倍数
            LONG                             lUserMaxScore;                      //分数上限
            WORD                             wBankerUser;                        //庄家用户
            WORD                             wCurrentUser;                       //当前玩家
            */
            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_PLAYING;
                packet.BeginRead();
                _lMaxCellScore = packet.GetInt();
                _lCurrentChipScore = packet.GetInt();
                _lMinChipScore = packet.GetInt();
                _lCellScore = packet.GetInt();
                _lCurrentTimes = packet.GetInt();
                _lLimitScore = packet.GetInt();
                _bBankerUser = (byte) packet.GetUShort();
                _bCurrentUser = (byte) packet.GetUShort();


                //设置变量
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //获取用户
                    PlayerInfo pUserData = GameEngine.Instance.EnumTablePlayer(i);
                    if (pUserData == null)
                    {
                        _bPlayStatus[i] = (byte) PlayerState.NULL;
                    }
                    else
                    {
						//把准备隐藏
						foreach(GameObject obj in o_player_option)
						{
							obj.SetActive(false);
						}
                        
                        //计算锅底和总注
                        _bPlayStatus[i] = (byte) PlayerState.PLAY;

                        _lTotalScore += _lCellScore;

                        _lTableScore[i] = _lCellScore;

                        _lBaseScore = _lCellScore;

                        _bHandCardData[i] = new byte[3] {255, 255, 255};
                        _bHandCardCount[i] = 3;

                        //下锅底
						AppendChips(i, _lCellScore, false,_bMingZhu[i]);

                    }


                }

                //设置庄家
                SetBanker();

                //更新牌局信息
                UpdateGameInfoView();

				Invoke("OnDelaySendCard", 1);
                //GameEngine.Instance.MyUser.TEvent.AddTimeTok((TimeEvents) GameTimer.TIMER_SEND_CARD, 1000);

                o_ready_buttons.SetActive(false);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //抢庄
        private void OnUserQiangBankerResp(NPacket packet)
        {

            packet.BeginRead();
            byte bQiangUser = (byte) packet.GetUShort();
            bool bQiang = packet.GetBool();
            _bBankerUser = (byte) packet.GetUShort();

            SetBanker();
        }

        //诈牌
        private void OnUserCheapCardResp(NPacket packet)
        {
            try
            {

                packet.BeginRead();
                byte[] bCardData = new byte[3] {255, 255, 255};
                byte bCheapUser = (byte) packet.GetUShort();
                byte bCardCount = packet.GetByte();
                packet.GetBytes(ref bCardData, bCardCount);

                if (bCheapUser != GetSelfChair())
                {
                    SetHandCardData(bCheapUser, bCardData, 3);
                }

                SetUserCheap(bCheapUser, true);

                ResetHandCardData(GetSelfChair());

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //抢庄开始
        private void OnGameQiangStartResp(NPacket packet)
        {
			//把准备隐藏
			foreach(GameObject obj in o_player_option)
			{
				obj.SetActive(false);
			}


            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_PLAYING;
            o_btn_opencard.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_qiang_buttons.SetActive(true);

            SetUserClock(GetSelfChair(), 2, TimerType.TIMER_QIANG);

            //更新牌局信息
            UpdateGameInfoView();

        }

        //用户放弃
        private void OnUserGiveUpResp(NPacket packet)
        {
            try
            {
                //解析封包
                packet.BeginRead();
                byte bGiveUpUser = (byte) packet.GetUShort();

                //删除定时器
                SetUserClock(bGiveUpUser, 0, TimerType.TIMER_NULL);

                //设置变量
                _bPlayStatus[bGiveUpUser] = (byte) PlayerState.GIVEUP;

                //灰掉放弃用户的牌
                DisableHandCard(bGiveUpUser);

                //播放声音
                PlayGameSound(SoundType.GIVEUP);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //判断当前是否能开牌
        private void openCard(NPacket packet)
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (GameEngine.Instance.EnumTablePlayer(i) != null)
                {
                    if (GameEngine.Instance.EnumTablePlayer(i).Money < _lMaxCellScore * 2 || _lTotalScore > _lLimitScore)
                    {
                        //跟注按钮灰掉
                        o_btn_followchip.GetComponent<UIButton>().isEnabled = false;
                        //加注按钮灰掉
                        o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
                        //比牌按钮隐藏，开牌按钮设为TRUE
                        o_btn_compcard.SetActive(false);

                        o_btn_opencard.SetActive(true);
                        o_btn_opencard.GetComponent<UIButton>().isEnabled = true;
                        //开牌
                        if (_number % GetPlayingUserCount() == 0)
                        {
                            OnUserOpenCardResp(packet);
                        }

                    }
                        
                }
            }
        }

        //下注
        private void OnUserAddScoreResp(NPacket packet)
        {

            /*
            WORD                             bwCurrentUser;                      //当前用户
            WORD                             bwAddScoreUser;                     //加注用户
            WORD                             bwCompareState;                     //比牌状态
            LONG                             blAddScoreCount;                    //加注数目
            LONG                             blCurrentTimes;                     //当前倍数
            */
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
            packet.BeginRead();
            _bCurrentUser = (byte) packet.GetUShort();
            byte bAddScoreUser = (byte) packet.GetUShort();
            byte bCompareState = (byte) packet.GetUShort();
            int lAddScoreCount = packet.GetInt();
            _lCurrentTimes = packet.GetInt();
            _lCurrentChipScore = packet.GetInt();
            _lMinChipScore = packet.GetInt();


            //加注处理
            if (bAddScoreUser != GetSelfChair())
            {
                //下注金币
                _lTableScore[bAddScoreUser] += lAddScoreCount;
                _lTotalScore += lAddScoreCount;

                //总注控件
                AppendChips(bAddScoreUser, lAddScoreCount, true,_bMingZhu[GetSelfChair()]);

                //播放声音
                PlayGameSound(SoundType.CHIP);

            }

            //设置时间
            if (bCompareState == 0)
            {
                SetUserClock((byte) _bCurrentUser, 20, TimerType.TIMER_CHIP);
            }


            if (_bCurrentUser == GetSelfChair() && bCompareState == 0)
            {
                ShowOptionButtons();
            }

            UpdateGameInfoView();
            //看是否能开牌
           // openCard(packet);
            //下注数加一
            _number += 1;
            //看是否已经加注到最大单注限制
            if (_lCurrentChipScore + _lCellScore * 2 > _lMaxCellScore || _lCurrentChipScore + _lCellScore * 2 >_lMinChipScore )
            {
                //加注按钮灰掉
                o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
            }
        }

        //比牌结果
        private void OnUserCompareCardResp(NPacket packet)
        {
            byte[] bCompareUsers = new byte[2];

            packet.BeginRead();
            _bCurrentUser = (byte) packet.GetUShort();
            bCompareUsers[0] = (byte) packet.GetUShort();
            bCompareUsers[1] = (byte) packet.GetUShort();
            byte bLostUser = (byte) packet.GetUShort();

				
			if(_bCurrentUser == GetSelfChair())
			{
				ShowOptionButtons();
			}

            //设置变量
            _bPlayStatus[bLostUser] = (byte) PlayerState.GIVEUP;

            PlayGameSound(SoundType.COMP);

            SetUserClock((byte) _bCurrentUser, 20, TimerType.TIMER_CHIP);

            string strNick0 = GameEngine.Instance.GetTableUserItem(bCompareUsers[0]).NickName;
			string strNick1 = GameEngine.Instance.GetTableUserItem(bCompareUsers[1]).NickName;
			string strLostUser = GameEngine.Instance.GetTableUserItem(bLostUser).NickName;
            ShowCompEffectView(strNick0, strNick1, strLostUser);
        }

        //看牌
        private void OnUserLookCardResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                _bCurrentUser = (byte) packet.GetUShort();
                byte[] cbCardData = new byte[3]; //用户扑克
                packet.GetBytes(ref cbCardData, 3);

                _bMingZhu[_bCurrentUser] = true;

				//显示加注提示
				UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
				if(ctr.showCount!=null)
				{
					ctr.showCount[0].gameObject.GetComponent<UILabel>().text=(_lCurrentChipScore*2.0f).ToString();
                    ctr.showCount[1].gameObject.GetComponent<UILabel>().text = ((_lCurrentChipScore +_lCellScore * 2) * 2.0f).ToString();
                    ctr.showCount[2].gameObject.GetComponent<UILabel>().text = ((_lCurrentChipScore + _lCellScore * 5) * 2.0f).ToString();
                    ctr.showCount[3].gameObject.GetComponent<UILabel>().text = ((_lCurrentChipScore + _lCellScore * 10) * 2.0f).ToString();
				}



                //播放声音
                PlayGameSound(SoundType.LOOK);

				if(_bCurrentUser == GetSelfChair())
				{
					ShowOptionButtons();
				}

                //
                if (_bCurrentUser != GetSelfChair())
                {
                    //打开定时器
                    //SetUserClock((byte) _bCurrentUser, 20, TimerType.TIMER_CHIP);
                    //
                    LookHandCard(_bCurrentUser);
					//检测按钮状态
					if(!_bMingZhu[_bCurrentUser-1]){
                        if ((_lCurrentChipScore + _lCellScore * 2) >= _lMaxCellScore) { o_btn_chip_1.GetComponent<UIButton>().isEnabled = true; }
                        if ((_lCurrentChipScore + _lCellScore * 5) >= _lMaxCellScore) { o_btn_chip_2.GetComponent<UIButton>().isEnabled = true; }
                        if ((_lCurrentChipScore + _lCellScore * 10) >= _lMaxCellScore) { o_btn_chip_3.GetComponent<UIButton>().isEnabled = true; }
					}

                }
                else
                {
                    Buffer.BlockCopy(cbCardData, 0, _bHandCardData[_bCurrentUser], 0, 3);
                    _bHandCardCount[_bCurrentUser] = 3;

                    //看牌
                    OpenHandCardData(_bCurrentUser, _bHandCardData[_bCurrentUser], 3);
                }
                //看是否能开牌
              //  openCard(packet);
                //看是否已经加注到最大单注限制
                if ((_lCurrentChipScore + _lCellScore * 2) > _lMaxCellScore || (_lCurrentChipScore + _lCellScore * 2) > _lMinChipScore)
                {
                    //加注按钮灰掉
                    o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
                }
            }
            catch (Exception ex)
            {
               // UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //开牌
        private void OnUserOpenCardResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                ushort wWinner = packet.GetUShort();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户强退
        private void OnUserPlayerExitResp(NPacket packet)
        {
            packet.BeginRead();
            byte bExitUser = (byte) packet.GetUShort();
            //游戏信息
            _bPlayStatus[bExitUser] = (byte) PlayerState.NULL;
        }

        //游戏结束
        private void OnGameEndResp(NPacket packet)
        {
            /*
             LONG                                lGameTax;                           //游戏税收
             LONG                                lGameScore[GAME_PLAYER];            //游戏得分
             LONG                                lExp[3];                            //经验
             BYTE                                cbCardData[GAME_PLAYER][3];         //用户扑克
             WORD                                wCompareUser[GAME_PLAYER][4];       //比牌用户
             WORD                                wEndState;                          //结束状态
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

				//清除加注显示
				UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
				for(int i=0;i<4;i++)
				{
					ctr.showChip[i].gameObject.SetActive(false);
				}


				//游戏得分

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    //游戏得分
                    _nEndUserScore[i] = packet.GetInt();
                    if (_nEndUserScore[i] > 0)
                    {
                        _bBankerUser = (byte) i;
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
				//去除扑克上的炸牌
				if(o_player_cheap!=null)
				{
					foreach(GameObject obj in o_player_cheap){
						obj.SetActive(false);
					}

				}

                //用户手上的扑克
                byte[,] cbCardData = new byte[GameLogic.GAME_PLAYER,3];
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {

                    for (byte j = 0; j < 3; j++)
                    {
                        //用户扑克
                        cbCardData[i, j] = packet.GetByte();

                    }

                }


                //比牌用户
                ushort[,] wCompareUser = new ushort[GameLogic.GAME_PLAYER,4];
                for (ushort i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    for (ushort j = 0; j < 4; j++)
                    {
                        //看牌玩家
                        wCompareUser[i, j] = packet.GetUShort();
                        if (wCompareUser[i, j] > 256)
                        {
                            wCompareUser[i, j] = GameLogic.NULL_CHAIR;
                        }
                    }
                }
                //结束状态
                ushort wEndState = packet.GetUShort();
                ushort wMeChair = GetSelfChair();

                //比牌与被比牌用户所看到的数据

                for (ushort j = 0; j < 4; j++)
                {
                    ushort wUserID = wCompareUser[wMeChair, j];

                    if (wUserID == GameLogic.NULL_CHAIR)
                    {
                        continue;
                    }

                    byte[] tempcbCardData = new byte[GameLogic.MAX_COUNT];

                    for (byte col = 0; col < GameLogic.MAX_COUNT; col++)
                    {
                        tempcbCardData[col] = cbCardData[wUserID, col]; //用户扑克
                    }

                    OpenHandCardData((byte) wUserID, tempcbCardData, 3);

                    byte bViewID = ChairToView((byte) wUserID);
                    byte bType = GameLogic.GetCardType(tempcbCardData, 3);

                    o_player_type[bViewID].SetActive(true);
                    o_player_type[bViewID].GetComponent<UISprite>().spriteName = GetCardTypeTex(bType);
                }

                //自己扑克

               // if (wCompareUser[wMeChair, 0] != GameLogic.NULL_CHAIR || _bMingZhu[wMeChair])
               // {

                    byte[] ptemphandcbCardData = new byte[GameLogic.MAX_COUNT];
                    for (byte col = 0; col < GameLogic.MAX_COUNT; col++)
                    {
                        ptemphandcbCardData[col] = cbCardData[wMeChair, col]; //用户扑克
                    }

                    OpenHandCardData((byte) wMeChair, ptemphandcbCardData, 3);

                    byte pbViewID = ChairToView((byte) wMeChair);
                    byte pbType = GameLogic.GetCardType(ptemphandcbCardData, 3);


                    o_player_type[pbViewID].SetActive(true);
                    o_player_type[pbViewID].GetComponent<UISprite>().spriteName = GetCardTypeTex(pbType);
               // }

                //开牌结束
                if (wEndState == 1)
                {
                    byte[] temphandcbCardData = new byte[GameLogic.MAX_COUNT];
                    for (ushort i = 0; i < GameLogic.GAME_PLAYER; i++)
                    {
                        byte bViewID = ChairToView((byte) i);
						if (_bPlayStatus[i] == (byte) PlayerState.PLAY||_bPlayStatus[i] == (byte) PlayerState.PLAY)
                        {

                            for (byte col = 0; col < GameLogic.MAX_COUNT; col++)
                            {
                                temphandcbCardData[col] = cbCardData[i, col];
                            }
                            OpenHandCardData((byte) i, temphandcbCardData, 3);

                            byte bType = GameLogic.GetCardType(temphandcbCardData, 3);

                            o_player_type[bViewID].SetActive(true);
                            o_player_type[bViewID].GetComponent<UISprite>().spriteName = GetCardTypeTex(bType);

                        }
                        else
                        {
                            o_player_type[bViewID].SetActive(false);
                        }
                    }
                }

                WinChips(_bBankerUser);

                SetBanker();

                UpdateUserView();

                PlayGameSound(SoundType.WIN);

                o_option_buttons.SetActive(false);

                o_add_buttons.SetActive(false);

                //
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    PlayerInfo ud = GameEngine.Instance.EnumTablePlayer(i);
                    if (ud != null)
                    {
                        byte bViewId = ChairToView(i);

                        o_result_score[bViewId].GetComponent<UILabel>().text = _nEndUserScore[i].ToString();
                        o_result_exp[bViewId].GetComponent<UILabel>().text = _nEndUserExp[i].ToString();
						o_result_card_type[bViewId].GetComponent<UISprite>().spriteName = o_player_type[bViewId].GetComponent<UISprite>().spriteName;
						if (_nEndUserScore[i] > 0)
							{
                                if (bViewId == 0)
                                {
                                    o_result_win[bViewId].GetComponent<UISprite>().spriteName = "game_win";
                                    desc_win.text = "你赢得了金币";
                                    desc_win.color = new Color(1f, 0.929f, 0.4627f);
                                    o_result_score[bViewId].GetComponent<UILabel>().color = new Color(0.420f, 0.922f, 0.921f);
                                }
                                else
                                {
                                    o_result_win[bViewId].GetComponent<UISprite>().spriteName = "win";
                                }
						    }
                        else
							{
                                if (bViewId == 0)
                                {
                                    o_result_win[bViewId].GetComponent<UISprite>().spriteName = "game_lose";
                                    desc_win.text = "你失去了金币";
                                    desc_win.color = new Color(0.866f, 0.866f, 0.866f);
                                    o_result_score[bViewId].GetComponent<UILabel>().color = new Color(0.420f, 0.922f, 0.921f);
                                }
                                else
                                {
                                    o_result_win[bViewId].GetComponent<UISprite>().spriteName = "lose";
                                }
							}

                        if (_bBankerUser == i)
                            {
                                o_result_type[bViewId].GetComponent<UISprite>().spriteName = "banker";
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
                        byte bViewId = ChairToView(i);
                        o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_nick[bViewId].GetComponent<UILabel>().text = "";
                        o_result_win[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_score[bViewId].GetComponent<UILabel>().text = "";
                        o_result_exp[bViewId].GetComponent<UILabel>().text = "";
                        o_result_type[bViewId].GetComponent<UISprite>().spriteName = "blank";
                        o_result_card_type[bViewId].GetComponent<UISprite>().spriteName = "blank";
                    }
                }

				Invoke("ShowResultView",3);
               // GameEngine.Instance.MyUser.TEvent.AddTimeTok(TimeEvents.REG_GAME_END, 4000, packet);


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //初始场景处理函数
        private void SwitchFreeSceneView(NPacket packet)
        {

            ResetGameView();

            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_FREE;

            packet.BeginRead();
            _lCellScore = packet.GetInt();
            _lLimitScore = packet.GetInt();
            _lMaxCellScore = packet.GetInt();
            _lMaxTopScore = packet.GetInt();
            _lBaseScore = _lCellScore;


            _bStart = true;

            /*if (GameEngine.Instance.PrivateOwer)
            {
                Invoke("OnBtnReadyIvk", 1.0f);
                GameEngine.Instance.PrivateOwer = false;
            }
            else*/
			{
             SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY);
                //GameEngine.Instance.PrivateOwer = false;
			}
            o_ready_buttons.SetActive(true);

            UpdateGameInfoView();

            UpdateUserView();

            o_option_buttons.SetActive(false);

            o_add_buttons.SetActive(false);

            //PlayerPrefs.SetInt("UsedServ", 0);
        }

        //游戏场景处理函数
        private void SwitchPlaySceneView(NPacket packet)
        {
            try
            {
                InitGameView();

                byte wMeChairID = GetSelfChair();

                packet.BeginRead();
                //状态信息
                _lMaxCellScore = packet.GetInt();
                _lMaxTopScore = packet.GetInt();
                _lCellScore = packet.GetInt();
                _lCurrentTimes = packet.GetInt();
                _lLimitScore = packet.GetInt();
                _bBankerUser = (byte) packet.GetUShort();
                _bCurrentUser = (byte) packet.GetUShort();
                _lBaseScore = _lCellScore;



                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bPlayStatus[i] = packet.GetByte();

                    PlayerInfo pUserData = GameEngine.Instance.EnumTablePlayer(i);
                    if (pUserData != null)
                    {
                        if (_bPlayStatus[i] == 0)
                        {
                            _bPlayStatus[i] = (byte) PlayerState.GIVEUP;
                        }
                        else
                        {
                            _bPlayStatus[i] = (byte) PlayerState.PLAY;
                        }
                    }
                    else
                    {
                        _bPlayStatus[i] = (byte) PlayerState.NULL;
                    }


                    if (i == wMeChairID && _bPlayStatus[i] == (byte) PlayerState.NULL)
                    {
                        _bStart = false;
                    }
                    else
                    {
                        _bStart = true;
                        GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_PLAYING;
                    }
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bMingZhu[i] = packet.GetBool();
                }

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _lTableScore[i] = packet.GetInt();
                }


                packet.GetBytes(ref _bHandCardData[wMeChairID], 3);
                _bHandCardCount[wMeChairID] = 3;
                bool bCompareState = packet.GetBool();




                //设置界面
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    if (_bPlayStatus[i] != (byte) PlayerState.NULL)
                    {
                        if (i == wMeChairID)
                        {
                            if (_bMingZhu[i])
                            {
                                OpenHandCardData(wMeChairID, _bHandCardData[wMeChairID], _bHandCardCount[wMeChairID]);
                            }
                            else
                            {
                                if (_bPlayStatus[i] == (byte) PlayerState.PLAY)
                                {
                                    _bHandCardData[wMeChairID] = new byte[] {255, 255, 255};
                                    SetHandCardData(wMeChairID, _bHandCardData[wMeChairID], _bHandCardCount[wMeChairID]);
                                }
                                else if (_bPlayStatus[i] == (byte) PlayerState.GIVEUP)
                                {
                                    _bHandCardData[wMeChairID] = new byte[] {254, 254, 254};
                                    SetHandCardData(wMeChairID, _bHandCardData[wMeChairID], _bHandCardCount[wMeChairID]);

                                }
                            }
                        }
                        else
                        {
                            _bHandCardCount[i] = 3;
                            SetHandCardData(i, _bHandCardData[i], _bHandCardCount[i]);
                        }
                    }
                }


                //叫分按钮
                if (_bCurrentUser == GetSelfChair())
                {
                    //错误
                    ShowOptionButtons();
                }
                else
                {
                    o_option_buttons.SetActive(false);
                }


                //设置定时
                SetUserClock(_bCurrentUser, 20, TimerType.TIMER_CHIP);
                //加注信息
                UpdateGameInfoView();
                //庄家标志
                SetBanker();
                //玩家信息
                UpdateUserView();

                //
                //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MyUser.ServerUsed.ServerID);
            }
            catch (Exception ex)
            {
                //UIMsgBox.Instance.Show(true, ex.Message);
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

        private void OnBtnBackIvk()
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnConfirmBackOKIvk()
        {
            try
            {
                o_ready_buttons.SetActive(false);
                //PlayerPrefs.SetInt("UsedServ", 0);
                _bStart = false;
                _bReqQuit = true;
                _nQuitDelay = System.Environment.TickCount;

                OnBtnSpeakCancelIvk();
				GameEngine.Instance.Quit();
				
            }
            catch
            {
                UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);
            }
        }

        private void OnConfirmBackCancelIvk()
        {

        }

        private void OnBtnSettingIvk()
        {
			UISetting.Instance.Show(true);
			btn_chat.GetComponent<UIButton>().isEnabled=false;
           
        }

		//声音控制按钮
		void OnBtnChatIvk()
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

                if (UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>() == null)
				{
					NGUITools.soundVolume=0.5f;
				}else{
                    NGUITools.soundVolume = UISetting.Instance.o_set_effect.gameObject.GetComponentInChildren<UISlider>().value;
				}

                if (UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>() == null)
				{
					GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0.3f;
				}else
				{
                    GameObject.Find("Panel").GetComponent<AudioSource>().volume = UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>().value;
				}
				
			}
			
		}


        private void OnBtnReadyIvk()
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

        private void OnBtnQuitIvk()
        {
            OnConfirmBackOKIvk();
        }

        private void OnBtnRuleIvk()
        {
            bool bshow = !o_dlg_rules.active;
            o_dlg_rules.SetActive(bshow);
            if (bshow == true)
            {
                _nInfoTickCount = Environment.TickCount;
            }
        }


        private void OnBtnSpeakStartIvk()
        {
        }

        private void OnBtnSpeakEndIvk()
        {
            
        }

        private void OnBtnSpeakCancelIvk()
        {

        }

        private void OnRecordSpeakError(string strSpeak)
        {
          
        }


        private void OnClearInfoIvk()
        {
            ClearAllInfo();
        }

        //
        private void OnPlayerInfoIvk(GameObject obj)
        {
			if(curGamePlatform == GamePlatform.ZJH_ForMobile)
			{
				string[] strs = obj.name.Split("_".ToCharArray());
				int nChair = Convert.ToInt32(strs[2]);
				//再次点击的时候把info隐藏
				bool show=!o_info[nChair].active;
				ShowUserInfo((byte) nChair,show);
			}
            
        }

        /////////////////////////////游戏特殊/////////////////////////////

        private void OnBtnGiveupIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //扑克变灰
                byte bSelfChair = GetSelfChair();
                byte[] cbCardData = new byte[] {254, 254, 254};
                _bHandCardCount[bSelfChair] = 3;
                Buffer.BlockCopy(cbCardData, 0, _bHandCardData[bSelfChair], 0, 3);
                SetHandCardData(bSelfChair, _bHandCardData[bSelfChair], 3);

                //发送消息
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GIVE_UP);
                GameEngine.Instance.Send(packet);

                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

//        private void OnBtnAddOneIvk()
//        {
//            try
//            {
//                if (_bCurrentUser != GetSelfChair())
//                {
//                    return;
//                }
//
//                //获取筹码
//                int lCurrentScore = _lCellScore*_lCurrentTimes;
//
//                //明注加倍
//                if (_bMingZhu[GetSelfChair()] == true)
//                {
//                    lCurrentScore *= 2;
//                }
//
//                //lCurrentScore += _lCellScore * 1;
//                lCurrentScore = lCurrentScore*1;
//
//                NPacket packet = NPacketPool.GetEnablePacket();
//                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
//                packet.AddInt(lCurrentScore);
//                packet.AddShort(0);
//                GameEngine.Instance.Send(packet);
//
//                _lTableScore[GetSelfChair()] += lCurrentScore;
//                _lTotalScore += lCurrentScore;
//
//                //总注控件
//                AppendChips(GetSelfChair(), lCurrentScore, true);
//
//                //
//                UpdateGameInfoView();
//
//                o_option_buttons.SetActive(false);
//                o_add_buttons.SetActive(false);
//                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
//            }
//            catch (Exception ex)
//            {
//                UIMsgBox.Instance.Show(true, ex.Message);
//            }
//        }
		private void OnBtnAddOneIvk()
		{
			try
			{
				if (_bCurrentUser != GetSelfChair())
				{
					return;
				}
				
				//获取筹码
				int lCurrentScore = _lCurrentChipScore;

                lCurrentScore += _lCellScore * 2;
				//明注加倍
				if (_bMingZhu[GetSelfChair()] == true)
				{
                    lCurrentScore = lCurrentScore * 2;
				}
				//lCurrentScore = lCurrentScore*2;

				
				NPacket packet = NPacketPool.GetEnablePacket();
				packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
				packet.AddInt(lCurrentScore);
				packet.AddShort(0);
				GameEngine.Instance.Send(packet);
				
				_lTableScore[GetSelfChair()] += lCurrentScore;
				_lTotalScore += lCurrentScore;
				
				//总注控件
				AppendChips(GetSelfChair(), lCurrentScore, true,_bMingZhu[GetSelfChair()]);
				
				//
				UpdateGameInfoView();
				
				o_option_buttons.SetActive(false);
				o_add_buttons.SetActive(false);
				SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
			}
			catch (Exception ex)
			{
				UIMsgBox.Instance.Show(true, ex.Message);
			}
		}

        private void OnBtnAddTwoIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //获取筹码
                int lCurrentScore = _lCurrentChipScore;

                lCurrentScore += _lCellScore * 5;

                //明注加倍
                if (_bMingZhu[GetSelfChair()] == true)
                {
                    lCurrentScore *= 2;
                }

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                packet.AddShort(0);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] += lCurrentScore;
                _lTotalScore += lCurrentScore;

                //总注控件
				AppendChips(GetSelfChair(), lCurrentScore, true,_bMingZhu[GetSelfChair()]);

                //
                UpdateGameInfoView();

                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnAddThreeIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //获取筹码
                int lCurrentScore = _lCurrentChipScore;
                lCurrentScore += _lCellScore * 10;

                //明注加倍
                if (_bMingZhu[GetSelfChair()] == true)
                {
                    lCurrentScore *= 2;
                }


                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                packet.AddShort(0);
                GameEngine.Instance.Send(packet);

                _lTableScore[GetSelfChair()] += lCurrentScore;
                _lTotalScore += lCurrentScore;

                //总注控件
				AppendChips(GetSelfChair(), lCurrentScore, true,_bMingZhu[GetSelfChair()]);

                //
                UpdateGameInfoView();

                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnFollowIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair())
                {
                    return;
                }

                //获取筹码
                int lCurrentScore = _lCellScore*_lCurrentTimes;

                //明注加倍
                if (_bMingZhu[GetSelfChair()] == true)
                {
                    lCurrentScore *= 2;
                }


                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet.AddInt(lCurrentScore);
                packet.AddShort(0);
                GameEngine.Instance.Send(packet);
				//
                _lTableScore[GetSelfChair()] += lCurrentScore;
                _lTotalScore += lCurrentScore;

                //总注控件
				AppendChips(GetSelfChair(), lCurrentScore, true,_bMingZhu[GetSelfChair()]);

                //
                UpdateGameInfoView();

                o_option_buttons.SetActive(false);
                o_add_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnCompIvk()
        {

            try
            {

                //判断明注
                ushort wMeChairID = GetSelfChair();
                //比牌当前注的计算
                int lCurrentScore = (_bMingZhu[wMeChairID])
                                        ? (_lCurrentTimes*_lCellScore*4)
                                        : (_lCurrentTimes*_lCellScore*2);
                //当前人数
                int nUserCount = GetPlayingUserCount();
                //
                int lMyTableScore = _lTableScore[wMeChairID] + lCurrentScore;
                //庄家在第一轮没下注只能跟上家比牌 或 只剩下两人
                if ( /*_bBankerUser == wMeChairID && (lMyTableScore - lCurrentScore) == _lCellScore || */nUserCount == 2)
                {

                    short wCompareUser = GameLogic.NULL_CHAIR;
                    //查找上家
                    for (int i = (int) wMeChairID - 1;; i--)
                    {
                        if (i == -1)
                        {
                            i = GameLogic.GAME_PLAYER - 1;
                        }
                        if (_bPlayStatus[i] == (byte) PlayerState.PLAY)
                        {
                            wCompareUser = (short) i;
                            break;
                        }
                    }

                    //加注消息
                    _lTableScore[wMeChairID] += lCurrentScore;

                    NPacket packet = NPacketPool.GetEnablePacket();
                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                    packet.AddInt(lCurrentScore);
                    packet.AddUShort(1);
                    GameEngine.Instance.Send(packet);


                    //发送消息
                    NPacket packet1 = NPacketPool.GetEnablePacket();
                    packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_CARD);
                    packet1.AddShort(wCompareUser);
                    GameEngine.Instance.Send(packet1);

                    o_option_buttons.SetActive(false);
                    o_add_buttons.SetActive(false);
                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                }
                else
                {
                    ShowCompSelectView();
                }



            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnSeeIvk()
        {
            if (_bCurrentUser != GetSelfChair()) return;

            try
            {
                _bMingZhu[_bCurrentUser] = true;

                //发送消息
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LOOK_CARD);
                GameEngine.Instance.Send(packet);

                ShowOptionButtons();

				//看牌后让加注按钮显示
                o_add_buttons.SetActive(false);

                //SetUserClock(_bCurrentUser, 20, TimerType.TIMER_CHIP);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnOpenIvk()
        {
            //wState = 0 加注 跟注
            //wState = 1 比牌
            //wState = 2 开牌

            //查找人数
            short bUserCount = 0;
            for (short i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] == (byte) PlayerState.PLAY) bUserCount++;
            }

            //两人比牌
            /*
            if(bUserCount==2)
            {
                OnBtnCompIvk();
                return;
            }
            */
            //删除定时器
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

            //判断明注
            //ushort wMeChairID=GetSelfChair();
            //int lCurrentScore=(_bMingZhu[wMeChairID])?(_lCurrentTimes*_lCellScore*4):(_lCurrentTimes*_lCellScore*2);
            //_lTableScore[wMeChairID]+=lCurrentScore;

            //发送消息
            /*
            ushort wState = 2;
            int lScore = lCurrentScore;
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
            packet.AddInt(lScore);
            packet.AddUShort(wState);
            GameEngine.Instance.Send(packet, 100);
            */

            //发送消息
            NPacket packet1 = NPacketPool.GetEnablePacket();
            packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);
            GameEngine.Instance.Send(packet1);


            o_option_buttons.SetActive(false);
            o_add_buttons.SetActive(false);
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
        }

        //诈牌
        private void OnBtnCheapIvk()
        {
            if (_bCurrentUser != GetSelfChair()) return;

            try
            {
                byte bViewID = ChairToView(GetSelfChair());
                byte[] bCardData = new byte[3];
                byte bCardCount = 0;

                o_player_cards[bViewID].GetComponent<UICardControl>().GetShootCard(ref bCardData, ref bCardCount);
                if (bCardCount > 0 && bCardCount < 3)
                {
                    NPacket packet = NPacketPool.GetEnablePacket();
                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_CHEAP_CARD);
                    packet.Addbyte(bCardCount);
                    packet.AddBytes(bCardData, bCardCount);
                    GameEngine.Instance.Send(packet);
		
                    _bCheapCard[GetSelfChair()] = true;

                    ShowOptionButtons();

                    o_add_buttons.SetActive(false);

                    //SetUserClock(_bCurrentUser, 20, TimerType.TIMER_CHIP);

                    //看是否能开牌
                   // openCard(packet);
                }
                else
                {
                    UIMsgBox.Instance.Show(true, "只能选择1张或2张牌进行诈牌");
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //抢
        private void OnBtnNotQiangIvk()
        {
            try
            {
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                o_qiang_buttons.SetActive(false);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_QIANG_BANKER);
                packet.AddBool(false);
                GameEngine.Instance.Send(packet);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //不抢
        private void OnBtnQiangIvk()
        {
            try
            {
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                o_qiang_buttons.SetActive(false);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_QIANG_BANKER);
                packet.AddBool(true);
                GameEngine.Instance.Send(packet);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnAddIvk()
        {
            o_add_buttons.SetActive(true);
            o_btn_addchip.SetActive(false);
				
				if (_bMingZhu[GetSelfChair()] == true)
				{

                    if ((_lCurrentChipScore + _lCellScore * 2)  > _lMaxCellScore || 
                        (_lCurrentChipScore + _lCellScore * 2) * 2 > _lMinChipScore) 
                    { 
                        o_btn_chip_1.GetComponent<UIButton>().isEnabled = false;
                        
                    }
                    else o_btn_chip_1.GetComponent<UIButton>().isEnabled = true;


                    if ((_lCurrentChipScore + _lCellScore * 5) > _lMaxCellScore || 
                        (_lCurrentChipScore + _lCellScore * 5) * 2 > _lMinChipScore) 
                    { 
                        o_btn_chip_2.GetComponent<UIButton>().isEnabled = false; 
                    }
                    else o_btn_chip_2.GetComponent<UIButton>().isEnabled = true;

                    if ((_lCurrentChipScore + _lCellScore * 10) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 10) * 2 > _lMinChipScore) 
                    {
                        o_btn_chip_3.GetComponent<UIButton>().isEnabled = false;
                    }
                    else o_btn_chip_3.GetComponent<UIButton>().isEnabled = true;
					
				}else{
                    if ((_lCurrentChipScore + _lCellScore * 2) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 2) > _lMinChipScore) 
                    {
                        o_btn_chip_1.GetComponent<UIButton>().isEnabled = false; 
                    }
                    else o_btn_chip_1.GetComponent<UIButton>().isEnabled = true;

                    if ((_lCurrentChipScore + _lCellScore * 5) > _lMaxCellScore ||
                        (_lCurrentChipScore + _lCellScore * 5) > _lMinChipScore) 
                    {
                        o_btn_chip_2.GetComponent<UIButton>().isEnabled = false; 
                    }
                    else o_btn_chip_2.GetComponent<UIButton>().isEnabled = true;

                    if ((_lCurrentChipScore + _lCellScore * 10) > _lMaxCellScore||
                        (_lCurrentChipScore + _lCellScore * 10) > _lMinChipScore)
                    {
                        o_btn_chip_3.GetComponent<UIButton>().isEnabled = false; 
                    }
                    else o_btn_chip_3.GetComponent<UIButton>().isEnabled = true;
					
				}

        }

        private void OnBtnCancelAddIvk()
        {
            o_add_buttons.SetActive(false);
            o_btn_addchip.SetActive(true);


        }

        private void OnBtnCmpSelectIvk(GameObject obj)
        {
            if (_bCurrentUser != GetSelfChair()) return;

            byte bViewID = 0;
            switch (obj.name)
            {
                case "btn_select_0":
                    {
                        bViewID = 0;
                        break;
                    }
                case "btn_select_1":
                    {
                        bViewID = 1;
                        break;
                    }
                case "btn_select_2":
                    {
                        bViewID = 2;
                        break;
                    }
                case "btn_select_3":
                    {
                        bViewID = 3;
                        break;
                    }
                case "btn_select_4":
                    {
                        bViewID = 4;
                        break;
                    }
            }

            byte bChairID = ViewToChair(bViewID);

            //比牌
            int lCurrentScore = (_bMingZhu[GetSelfChair()])
                                    ? (_lCurrentTimes*_lCellScore*4)
                                    : (_lCurrentTimes*_lCellScore*2);
            _lTableScore[GetSelfChair()] += lCurrentScore;

            //加注消息
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
            packet.AddInt(lCurrentScore);
            packet.AddUShort(1);
            GameEngine.Instance.Send(packet);


            //发送消息
            NPacket packet1 = NPacketPool.GetEnablePacket();
            packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_CARD);
            packet1.AddShort(bChairID);
            GameEngine.Instance.Send(packet1);

            o_dlg_comp_select.SetActive(false);
            o_option_buttons.SetActive(false);

        }

        private void OnBtnCmpCancelIvk()
        {
            o_dlg_comp_select.SetActive(false);
        }

        #endregion


        #region ##################控件事件#######################

        /////////////////////////////游戏通用/////////////////////////////

        //扑克控件点击事件
        private void OnCardClick()
        {

        }

        /*//定时队列处理事件
        private void OnTimerResp(TimeEvents evt, TimeTok evtContents)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            #region GAME_END

            if (evt == TimeEvents.REG_GAME_END)
            {
                ShowResultView(true);
            }

            #endregion

            if (evt == (TimeEvents) GameTimer.TIMER_SEND_CARD)
            {
                
            }
        }*/

		private void OnDelaySendCard()
		{
			//发牌
			for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
			{
				if (_bPlayStatus[i] == (byte) PlayerState.PLAY)
				{
					//发牌
					SendHandCard(i, _bHandCardData[i], _bHandCardCount[i]);
					
					//发牌音效
					PlayGameSound(SoundType.SENDCARD);
				}
			}
			//显示按钮
			if (_bCurrentUser == GetSelfChair())
			{
				ShowOptionButtons();
			}
			
			//设置首家计时器
			SetUserClock((byte) _bCurrentUser, 20, TimerType.TIMER_CHIP);
		}

        //
        private void OnSpeakTimerEnd()
        {
            OnBtnSpeakEndIvk();
        }

        //定时处理事件
        private void OnTimerEnd()
        {
            try
            {
                switch (_bTimerType)
                {
					//准备好
                    case TimerType.TIMER_READY:
                        {
                            OnConfirmBackOKIvk();
                            break;
                        }
					//抢庄
                    case TimerType.TIMER_QIANG:
                        {
                            OnBtnNotQiangIvk();
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //扑克控件选牌事件
        private void OnMoveSelect()
        {

        }

        //扑克控件向上划牌事件
        private void OnMoveUp()
        {

        }

        //扑克控件向下划牌事件
        private void OnMoveDown()
        {

        }


        /////////////////////////////游戏特殊/////////////////////////////

        #endregion


        #region ##################UI 控制#######################

        /////////////////////////////游戏通用/////////////////////////////

        private void SetHandCardData(byte bchair, byte[] cards, byte count)
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

        private void OpenHandCardData(byte bchair, byte[] cards, byte count)
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

        private void SendHandCard(byte bchair, byte[] cards, byte cardcount)
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

        private void DisableHandCard(byte bchair)
        {
            try
            {
               // _bHandCardData[bchair] = new byte[3] {254, 254, 254};
               // _bHandCardCount[bchair] = 3;

                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
               // ctr.SetCardData(_bHandCardData[bchair], _bHandCardCount[bchair]);
                ctr.SetCardData(new byte[3] { 254, 254, 254 },3);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void LoseHandCard(byte bchair)
        {
            try
            {
              //  _bHandCardData[bchair] = new byte[3] {252, 252, 252};
              //  _bHandCardCount[bchair] = 3;

                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
              //  ctr.SetCardData(_bHandCardData[bchair], _bHandCardCount[bchair]);
                ctr.SetCardData(new byte[3] { 252, 252, 252 }, 3);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void LookHandCard(byte bchair)
        {
            try
            {
                _bHandCardData[bchair] = new byte[3] {253, 253, 253};
                _bHandCardCount[bchair] = 3;

                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.SetCardData(_bHandCardData[bchair], _bHandCardCount[bchair]);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void ArrayHandCardData(byte bchair, byte[] cards, byte count)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.ArrayHandCards(cards, count);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void ResetHandCardData(byte bchair)
        {
            try
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.ResetAllShoot();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void AppendChips(byte bchair, int nUserChips, bool bPlay,bool kanpai)
        {
            try
            {

                byte bViewID = ChairToView(bchair);
                UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
                ctr.AddChips(bViewID, nUserChips, _lCellScore, _lCurrentChipScore, kanpai);

                UpdateGameInfoView();

                if (bPlay == true)
                {
                    PlayGameSound(SoundType.CHIP);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void WinChips(byte bchair)
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            try
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_clock[i].GetComponent<UITimerBar>().SetTimer(0);
                    o_player_clock[i].SetActive(false);

                }
                if (chair != GameLogic.NULL_CHAIR)
                {
                    _bTimerType = timertype;
                    byte viewId = ChairToView(chair);
                    o_player_clock[viewId].GetComponent<UITimerBar>().SetTimer(time*1000);
					//o_player_clock[viewId].SetActive(true);
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetUserCheap(byte chair, bool bshow)
        {
            try
            {
                _bCheapCard[chair] = true;
                byte viewId = ChairToView(chair);
                o_player_cheap[viewId].SetActive(true);

                Invoke("ClearUserCheap", 2.0f);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void ClearUserCheap()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bCheapCard[i] == true)
                {
                    byte viewId = ChairToView(i);
                    TweenScale.Begin(o_player_cheap[viewId], 0.5f, new Vector3(0.5f, 0.51f, 1f));
                }
            }
        }

        private void SetUserReady(byte chair, bool bshow)
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void UpdateUserView()
        {
            try
            {
                if (_bStart == false) return;


                for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    byte bViewId = ChairToView((byte) i);
                    PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {

                        //nick 
                        if (userdata.VipLevel > 0)
                        {
                            //注释了玩家名的颜色更改
                            //o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
                        }
                        else
                        {
                            // o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(0.35f, 0.8f, 0.8f);
                        }
                        string strNickName = userdata.NickName;
                        if (strNickName.Length > 7)
                        {
                            strNickName = strNickName.Substring(0, 7) + "...";
                        }
                        o_player_nick[bViewId].GetComponent<UILabel>().text = strNickName;
                        //face

                        o_player_face[bViewId].GetComponent<UIFace>()
                                              .ShowFace((int) userdata.HeadID, (int) userdata.VipLevel);
                        o_player_frame[bViewId].SetActive(true);

                        //准备
                        if (userdata.UserState == (byte) UserState.US_READY)
                        {
                            SetUserReady((byte) i, true);
                        }
                        else
                        {
                            SetUserReady((byte) i, false);
                        }

                        //金币
                        if (_lTableScore[i] > 0)
                        {
                            o_player_c_desc[bViewId].SetActive(true);
                            o_player_chip[bViewId].SetActive(true);
                            o_player_chip[bViewId].GetComponent<UILabel>().text = _lTableScore[i].ToString();
                        }
                        else
                        {
                            o_player_c_desc[bViewId].SetActive(false);
                            o_player_chip[bViewId].SetActive(false);
                        }

                        //o_player_money[bViewId].GetComponent<UINumber>().SetNumber(GameEngine.Instance.GetTableUserItem((ushort)i).Money);
                        if (curGamePlatform == GamePlatform.ZJH_ForPC)
                        {
                            ShowUserInfo(bViewId, true);
                        }

                    }
                    else
                    {
                        //nick
                        o_player_nick[bViewId].GetComponent<UILabel>().text = "";
                        //face
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
                        //bg
                        o_player_frame[bViewId].SetActive(false);
                        //p
                        o_player_option[bViewId].SetActive(false);
                        o_player_c_desc[bViewId].SetActive(false);
                        o_player_chip[bViewId].SetActive(false);
                        o_player_flag[bViewId].SetActive(false);
                        //info
                        o_info[bViewId].SetActive(false);
                    }
                }

                ShowInfoBar();

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void ShowInfoBar()
        {
            if (GameEngine.Instance.MySelf != null)
            {

               // GameEngine.Instance.MyUser.Exp = (uint) GameEngine.Instance.Self.lExperience;
               // GameEngine.Instance.MyUser.GameScore = (uint) GameEngine.Instance.Self.lScore;

               // o_btn_speak_count.GetComponent<UILabel>().text = "x" +
               //                                                  GameEngine.Instance.MyUser.SmallSpeakerCount.ToString();
                o_self_money.GetComponent<UILabel>().text = GameEngine.Instance.MySelf.Money.ToString();

            }
            else
            {
                o_self_money.GetComponent<UILabel>().text = "";
            }
        }

        private void ShowUserInfo(byte bViewID, bool bshow)
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
                PlayerInfo ud = GameEngine.Instance.GetTableUserItem((ushort)bChairID);
                if (ud != null)
                {
                    o_info[bViewID].SetActive(bshow);
					o_info_bg[bViewID].SetActive(true);
					o_info_desc_score[bViewID].SetActive(true);
                    string strNickName = ud.NickName;
                    if (strNickName.Length > 7)
                    {
                        strNickName = strNickName.Substring(0, 7) + "...";
                    }
                    o_info_nick[bViewID].GetComponent<UILabel>().text = strNickName;
                    o_info_lvl[bViewID].GetComponent<UILabel>().text =
                    GameConfig.Instance.GetExpLevel((int)ud.Exp).ToString();
                    o_info_id[bViewID].GetComponent<UILabel>().text = ud.ID.ToString();
                    o_info_score[bViewID].GetComponent<UILabel>().text = ud.Money.ToString();
                    o_info_win[bViewID].GetComponent<UILabel>().text = ud.WinCount.ToString();
                    o_info_lose[bViewID].GetComponent<UILabel>().text = ud.LostCount.ToString();
                    o_info_run[bViewID].GetComponent<UILabel>().text = ud.DrawCount.ToString();

                    _nInfoTickCount = Environment.TickCount;
                }
            }
        }

        private byte ChairToView(byte ChairID)
        {
            byte wViewChairID = (byte) ((ChairID + GameLogic.GAME_PLAYER - GameEngine.Instance.MySelf.DeskStation));
            return (byte) (wViewChairID%GameLogic.GAME_PLAYER);
        }

        private byte ViewToChair(byte ViewID)
        {
			byte wChairID = (byte) ((ViewID + GameEngine.Instance.MySelf.DeskStation)%GameLogic.GAME_PLAYER);
            return wChairID;
        }

        private byte GetSelfChair()
        {
			//修正MySelf 为NULL 时的获取错误
			if (GameEngine.Instance.MySelf != null) {
				return (byte)GameEngine.Instance.MySelf.DeskStation;
			} else {
			}
			return 0xFF;
        }

        private void PlayGameSound(SoundType sound)
        {
            float fvol = NGUITools.soundVolume;
            NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
        }

        private void PlayUserSound(GameSoundType sound, byte bGender)
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

        private void ShowUserSpeak(uint uid)
        {
            /*
            byte bchairID = (byte) GameEngine.Instance.UserIdToChairId(uid);
            byte bViewID = ChairToView(bchairID);
            if (bchairID != GetSelfChair())
            {
                o_user_speak[bViewID].GetComponent<UISpeak>().Play("scene_game", "OnSpeakPlay", uid);
            }*/

        }

        private void OnRecordSpeakFinish(string strSpeak)
        {
            /*//本地预播放
            byte bViewID = ChairToView(GetSelfChair());
            o_user_speak[bViewID].GetComponent<UISpeak>()
                                 .PlayLocal("scene_game", "OnSpeakPlay", GameEngine.Instance.MySelf.ID);

            //上传网络
            string strFile = Application.persistentDataPath + "/" + strSpeak;
            StartCoroutine(UpLoadSpeak(strFile));
            */
        }

        private void OnSpeakPlay(string str)
        {
            /*string[] strs = str.Split("`".ToCharArray());

            int nTime = Convert.ToInt32(strs[0]);

            uint Uid = Convert.ToUInt32(strs[1]);

            byte bViewID = ChairToView((byte) GameEngine.Instance.UserIdToChairId(Uid));

            if (nTime < 1000)
            {
                nTime = 6000;
            }
            o_user_speak[bViewID].GetComponent<UISpeak>().SetTimer(nTime);
            */
        }

        private IEnumerator UpLoadSpeak(string strSpeak)
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
            form.AddField("userid", GameEngine.Instance.MyUser.Userid.ToString());
            form.AddField("userpass", GameEngine.Instance.MyUser.Password);
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
                GameEngine.Instance.MyUser.SmallSpeakerCount--;
                if (GameEngine.Instance.MyUser.SmallSpeakerCount < 0)
                {
                    GameEngine.Instance.MyUser.SmallSpeakerCount = 0;
                }

                o_btn_speak_count.GetComponent<UILabel>().text = "x" +
                                                                 GameEngine.Instance.MyUser.SmallSpeakerCount.ToString();

                //本地预播放
                byte bViewID = ChairToView(GetSelfChair());
                o_user_speak[bViewID].GetComponent<UISpeak>()
                                     .PlayLocal("scene_game", "OnSpeakPlay", GameEngine.Instance.MyUser.Userid);

            }*/
        }

        private int GetStringLength(string str)
        {
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            return data.Length;
        }

        private void ShowUserChat(byte bChair, string strMsg)
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

        private void ShowNotice(string strMsg)
        {
            UIMsgBox.Instance.Show(true, strMsg);

        }

        private void ClearAllInfo()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
				if(curGamePlatform != GamePlatform.ZJH_ForPC)
				{
					o_info[i].SetActive(false);
				}
                o_player_chat[i].SetActive(false);

            }

            o_dlg_rules.SetActive(false);
        }




        /////////////////////////////游戏特殊/////////////////////////////
        private void ShowResultView()
        {
            ShowResultView(true);
        }

        private void ShowResultView(bool bshow)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            o_result.SetActive(bshow);


            if (bshow)
            {
                Invoke("CloseResultView", 5.0f);
            }


            //金币限制检测
            /*
             if(GameEngine.Instance.MyUser.Self.lScore <20*_lCellScore)
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

        private string GetCardTypeTex(byte bCardType)
        {
            if (bCardType == GameLogic.CT_ERROR) return "blank";
            if (bCardType == GameLogic.CT_JIN_HUA) return "card_type_jh";
            if (bCardType == GameLogic.CT_BAO_ZI) return "card_type_bz";
            if (bCardType == GameLogic.CT_SHUN_JIN) return "card_type_sj";
            if (bCardType == GameLogic.CT_SHUN_ZI) return "card_type_sz";
            if (bCardType == GameLogic.CT_DOUBLE) return "card_type_dz";
            if (bCardType == GameLogic.CT_SINGLE) return "card_type_sp";
            if (bCardType == GameLogic.CT_SPECIAL) return "card_type_ts";

            return "blank";
        }

        private void CloseResultView()
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
                o_player_cheap[i].SetActive(false);
                o_player_type[i].SetActive(false);
            }
            //
            UpdateGameInfoView();

            //
            o_dlg_chips.GetComponent<UIChipControl>().ClearChips();
            //

            SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY);

            o_ready_buttons.SetActive(true);

            o_qiang_buttons.SetActive(false);

            o_option_buttons.SetActive(false);

            o_add_buttons.SetActive(false);

            CancelInvoke();
        }

        private void ShowTipMsg(bool bshow)
        {
            //o_tip_bar.SetActive(bshow);
        }

        private void UpdateGameInfoView()
        {
            o_room_base_score.GetComponent<UILabel>().text = _lBaseScore.ToString();
            o_room_cell_score.GetComponent<UILabel>().text = _lCellScore.ToString();
            o_room_max_score.GetComponent<UILabel>().text = _lMaxCellScore.ToString();
            o_room_limit_score.GetComponent<UILabel>().text = _lMaxTopScore.ToString();

            //
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] > 0)
                {
                    byte bViewId = ChairToView(i);
                    o_player_c_desc[bViewId].SetActive(true);
                    o_player_chip[bViewId].GetComponent<UILabel>().text = _lTableScore[i].ToString();
                }

                if (_lTableScore[i] > 0)
                {

                    byte bViewId = ChairToView(i);
                    o_player_c_desc[bViewId].SetActive(true);
                    o_player_chip[bViewId].SetActive(true);
                    o_player_chip[bViewId].GetComponent<UILabel>().text = _lTableScore[i].ToString();
                }
                else
                {
                    byte bViewId = ChairToView(i);
                    o_player_chip[bViewId].SetActive(false);
                    o_player_c_desc[bViewId].SetActive(false);
                }
            }

            //看是否已经加注到最大单注限制
            if (_lCurrentChipScore + _lCellScore * 2 > _lMaxCellScore)
            {
                //加注按钮灰掉
                o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
            }
        }

        private void SetBanker()
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

        private void ShowOptionButtons()
        {
            
            byte bMeChair = GetSelfChair();

            o_option_buttons.SetActive(true);


            if (bMeChair == _bCurrentUser)
            {


                int nTemp = _bMingZhu[bMeChair] ? 2 : 1;

               // if ((_lMaxTopScore - _lTableScore[bMeChair]) >= 0)//(_lCurrentChipScore + _lCellScore * 2) * nTemp)
                {
                    //查找上家

                    int j = bMeChair - 1;
                    int t = 0;
                    for (j = bMeChair - 1;; j--)
                    {

                        if (j == -1)
                        {
                            j = GameLogic.GAME_PLAYER - 1;
                        }

                        if (_bPlayStatus[j] == (byte) PlayerState.PLAY || t > 4)
                        {
                            break;
                        }

                        t++;
                    }



                    //加注按钮
					OnBtnCancelAddIvk();
                    if (_lMaxCellScore >= (_lCurrentChipScore + _lCellScore * 2))
                    {
                        o_btn_addchip.GetComponent<UIButton>().isEnabled = true;
                    }
                    else
                    {
                        o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
                    }

                    //跟注
                    //o_btn_followchip.GetComponent<UIButton>().isEnabled = ((_lTableScore[j] == _lCellScore)? false : true);
					o_btn_followchip.GetComponent<UIButton>().isEnabled=true;
                    //放弃
                    o_btn_giveup.GetComponent<UIButton>().isEnabled = true;

                    //诈牌
                    if (_bMingZhu[bMeChair] == true)
                    {
                        o_btn_cheapcard.SetActive(true);
                        o_btn_seecard.SetActive(false);
                        o_btn_cheapcard.GetComponent<UIButton>().isEnabled = !_bCheapCard[bMeChair];

                    }
                    else
                    {
                        o_btn_cheapcard.SetActive(false);
                        o_btn_seecard.SetActive(true);
                        o_btn_seecard.GetComponent<UIButton>().isEnabled = true;
                    }




                    //比牌
                    o_btn_compcard.SetActive(true);
                    if ((bMeChair == _bBankerUser || _lTableScore[bMeChair] >= 2*_lCurrentChipScore) &&
                        _bMingZhu[GetSelfChair()] == true)
                    {
                        bool bHas = false;
                        for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                        {
                            if (_bPlayStatus[i] == (byte) PlayerState.PLAY && i != GetSelfChair() &&
                                _bMingZhu[i] == true)
                            {
                                bHas = true;
                                break;
                            }
                        }
                        //
                        if (bHas == true)
                        {
                            o_btn_compcard.GetComponent<UIButton>().isEnabled = true;
                        }
                        else
                        {
                            o_btn_compcard.GetComponent<UIButton>().isEnabled = false;
                        }

                    }
                    else
                    {
                        o_btn_compcard.GetComponent<UIButton>().isEnabled = false;
                    }
                    //开牌
                    o_btn_opencard.SetActive(false);
                    o_btn_opencard.GetComponent<UIButton>().isEnabled = false;



                }
              /*  else
                {


                    o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
                    o_btn_followchip.GetComponent<UIButton>().isEnabled = false;
                    o_btn_giveup.GetComponent<UIButton>().isEnabled = true;

                    if (_bMingZhu[GetSelfChair()] == true)
                    {
                        o_btn_seecard.SetActive(false);
                        o_btn_cheapcard.SetActive(true);
                        o_btn_cheapcard.GetComponent<UIButton>().isEnabled = false;
                    }
                    else
                    {
                        o_btn_seecard.SetActive(true);
                        o_btn_cheapcard.SetActive(false);

                        o_btn_seecard.GetComponent<UIButton>().isEnabled = false;

                    }


                    o_btn_compcard.SetActive(false);
                    o_btn_compcard.GetComponent<UIButton>().isEnabled = false;

                    o_btn_opencard.SetActive(true);
                    o_btn_opencard.GetComponent<UIButton>().isEnabled = true;
                }*/
            }
            else
            {


                o_btn_addchip.GetComponent<UIButton>().isEnabled = false;
                o_btn_followchip.GetComponent<UIButton>().isEnabled = false;
                o_btn_seecard.GetComponent<UIButton>().isEnabled = false;
                o_btn_giveup.GetComponent<UIButton>().isEnabled = false;
                o_btn_cheapcard.GetComponent<UIButton>().isEnabled = false;
                o_btn_compcard.GetComponent<UIButton>().isEnabled = false;

                o_btn_opencard.SetActive(false);
                o_btn_opencard.GetComponent<UIButton>().isEnabled = false;
            }
        }

        private void ShowCompSelectView()
        {
            o_dlg_comp_select.SetActive(true);
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                byte bViewID = ChairToView(i);
                if (_bPlayStatus[i] == (byte) PlayerState.PLAY && i != GetSelfChair() && _bMingZhu[i] == true)
                {
                    o_btn_comp_user[bViewID].SetActive(true);
                }
                else
                {
                    o_btn_comp_user[bViewID].SetActive(false);
                }
            }
        }

        private void ShowCompEffectView(string strUserNick0, string strUserNick1, string strLoseNick)
        {
            o_dlg_comp_effect.GetComponent<UICompCard>().OnFinishCall = new FinishCall(OnCompCardFinish);
            o_dlg_comp_effect.GetComponent<UICompCard>().Show(strUserNick0, strUserNick1, strLoseNick);
        }

        private void OnCompCardFinish()
        {
            //发送比牌完成
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_FINISH_FLASH);
            packet.AddInt(_bCurrentUser);
            GameEngine.Instance.Send(packet);

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] == (byte) PlayerState.GIVEUP)
                {
					LoseHandCard(i);
                }
            }
            //播放声音
            PlayGameSound(SoundType.GIVEUP);
        }

        private int GetPlayingUserCount()
        {
            //当前人数
            byte UserCount = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] == (byte) PlayerState.PLAY)
                {
                    UserCount++;
                }
            }

            return UserCount;
        }

        private void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        private bool CheckScoreLimit()
        {
            //金币限制检测
            //int nLimit = 0;
            ///*if (GameEngine.Instance.MyUser.ServerUsed.StationID.ToString().EndsWith("39") == true)
            //{
            //    nLimit = 10000;
            //}
            //else
            //{
            //    nLimit = 50*_lCellScore;
            //}*/

            //    nLimit = 50*_lCellScore;

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