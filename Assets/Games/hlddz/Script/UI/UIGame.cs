using UnityEngine;
using System.Collections;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.DDZ
{

    #region ##################结构定义#######################

    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_START = 1,
        TIMER_SCORE = 2,
        TIMER_OUTCARD = 3,
        TIMER_MOSTCARD = 4,
        TIMER_DOUBLE = 5,
        TIMER_CLOSE = 6
    };

    public enum SoundType
    {
        SENDCARD = 0,
        CALLSCORE = 1,
        START = 2,
        OUTCARD = 3,
        PASS = 4,
        MOSTCARD = 5,
        END_WIN = 6,
        BOMB_0 = 7,
        WARNS = 8,
        PLANE = 9,
        END_LOSE=10,
        BOMB_1=11,
        OUTCARD_1=12,
		MISSILE=13,
		SUNZI=14,
		TUOGUAN=15,
		CHUNTIAN=16,
		FANCHUN=17,

    };

    public enum GameSoundType
    {
        CARD_A = 0,
        CARD_2 = 1,
        CARD_3 = 2,
        CARD_4 = 3,
        CARD_5 = 4,
        CARD_6 = 5,
        CARD_7 = 6,
        CARD_8 = 7,
        CARD_9 = 8,
        CARD_10 = 9,
        CARD_J = 10,
        CARD_Q = 11,
        CARD_K = 12,
        CARD_S = 13,
        CARD_B = 14,
        CARD_AA = 15,
        CARD_22 = 16,
        CARD_33 = 17,
        CARD_44 = 18,
        CARD_55 = 19,
        CARD_66 = 20,
        CARD_77 = 21,
        CARD_88 = 22,
        CARD_99 = 23,
        CARD_11 = 24,
        CARD_JJ = 25,
        CARD_QQ = 26,
        CARD_KK = 27,
        CARD_AAA = 28,
        CARD_222 = 29,
        CARD_333 = 30,
        CARD_444 = 31,
        CARD_555 = 32,
        CARD_666 = 33,
        CARD_777 = 34,
        CARD_888 = 35,
        CARD_999 = 36,
        CARD_111 = 37,
        CARD_JJJ = 38,
        CARD_QQQ = 39,
        CARD_KKK = 40,
        CARD_ROCKET = 41,
        CARD_BOMB = 42,
        CARD_FOUR_2D = 43,
        CARD_FOUR_2 = 44,
        CARD_LINE = 45,
        CARD_THREE_1D = 46,
        CARD_THREE_1 = 47,
        CARD_PLANE = 48,
        CARD_DBL_LINE = 49,
        OPTION_QIANG_0 = 50,
        OPTION_CALL = 51,
        OPTION_NOTQIANG = 52,
        OPTION_NOTCALL = 53,
        OPTION_MING = 54,
        OPTION_JIABEI = 55,
        OPTION_NOTJIABEI = 56,
        OPTION_PASS = 57,
        OPTION_WARN_1 = 58,
        OPTION_WARN_2 = 59,
        OPTION_CALL_1 = 60,
        OPTION_CALL_2 = 61,
        OPTION_CALL_3 = 62,
        OPTION_QIANG_1=63,
        OPTION_WARN_3 = 64,
		CARD_PLANE_1=65,
		CARD_DANI=66,
		CARD_YASI=67

    };

    #endregion


    public class UIGame : MonoBehaviour
    {

        #region ##################变量定义#######################

        //控件
        private static GameObject[] o_clock = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_pass = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_ready = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_score = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_chat = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_firstLand = new GameObject[GameLogic.GAME_PLAYER_NUM];

		private static GameObject[] o_head_player = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_head_face = new GameObject[GameLogic.GAME_PLAYER_NUM];
		private static GameObject[] o_head_nick = new GameObject[GameLogic.GAME_PLAYER_NUM];
		private static GameObject[] o_head_gold = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_head_flag = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_head_bright = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_head_double = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_head_robot = new GameObject[GameLogic.GAME_PLAYER_NUM];

        private static GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER_NUM];


        private static GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER_NUM];

//        private static GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER_NUM];

        private static GameObject o_score_buttons = null;
        private static GameObject o_btn_call_1 = null;
        private static GameObject o_btn_call_2 = null;
        private static GameObject o_btn_call_3 = null;
        private static GameObject o_btn_giveup = null;

        private static GameObject o_qiang_buttons = null;
        private static GameObject o_btn_qiang = null;
        private static GameObject o_btn_notqiang = null;

        private static GameObject o_play_buttons = null;
        private static GameObject o_btn_outcard = null;
        private static GameObject o_btn_pass = null;
        private static GameObject o_btn_tip = null;
        private static GameObject o_btn_reset = null;
        private static GameObject o_btn_BigTuo = null;
        private static GameObject o_btn_rememberCard = null;

        private static GameObject o_ready_buttons = null;
        private static GameObject o_btn_ready = null;
       // private static GameObject o_btn_quit = null;
      //  private static GameObject o_btn_bright_start = null;

        //
        private static GameObject o_result = null;


        private static GameObject o_hand_cards = null;
        private static GameObject o_back_cards = null;
        private static GameObject o_back_cards_two = null;
        private static GameObject o_back_cards_three = null;
        private static GameObject o_back_cards_four = null;
        private static GameObject landType = null;
        private static GameObject landWait = null;
        
        private static GameObject o_tip_bar = null;
        private static GameObject[] o_out_cards = new GameObject[GameLogic.GAME_PLAYER_NUM];
        private static GameObject[] o_others = new GameObject[GameLogic.GAME_PLAYER_NUM];


        private static GameObject o_bright_buttons = null;
        private static GameObject o_double_buttons = null;
        private static GameObject o_land_buttons = null;
//        private static GameObject o_speak_timer = null;
//        private static GameObject o_btn_speak = null;
//        private static GameObject o_btn_speak_count = null;

        private static GameObject o_bright_rate = null;
        private static GameObject o_current_time = null;
        //数据
        private static bool _bStart = false;
        private static TimerType _bTimerType = TimerType.TIMER_START;

        private static byte _bBombTimes = 0;
        private static byte _bRocketTimes = 0;
        private static ushort _wTotalTimes = 1; //游戏结束时该盘的倍数
        private static byte _doubleNum = 0;
        private static int _nBaseScore = 0;

        private static byte[] _bOthersCardCount = new byte[GameLogic.GAME_PLAYER_NUM]; //三个玩家手上牌个数

        private static byte[][] _bOthersCardData = new byte[GameLogic.GAME_PLAYER_NUM][];//三个玩家手上牌数据

        private static bool _bAutoPlay = false;
        private static bool _bNoneOutCard = true;

        private static byte _bTurnOutType = 0;  //上家牌类型
        private static byte _bTurnCardCount = 0;//上家牌个数
        private static byte[] _bTurnCardData = new byte[GameLogic.MAX_COUNT];//上家牌数据

        private static byte[] _bHandCardData = new byte[GameLogic.MAX_COUNT];
        private static byte _bHandCardCount = 0;

        private static byte[,] _bOutCardData = new byte[GameLogic.GAME_PLAYER_NUM,GameLogic.MAX_COUNT];
        private static byte[] _bOutCardCount = new byte[GameLogic.GAME_PLAYER_NUM];

        private static byte[] _bBackCardData = new byte[GameLogic.BACK_COUNT];
        private static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER_NUM];

        private static byte _bMostUser = GameLogic.NULL_CHAIR;
        private static byte _bLander = GameLogic.NULL_CHAIR;
        private static byte _bCurrentUser = GameLogic.NULL_CHAIR;
        private static ushort _wCurrentScore = 0;
        private static byte _bFirstOutUser = GameLogic.NULL_CHAIR;

        private static bool[] _bShowInfo = new bool[GameLogic.GAME_PLAYER_NUM];
        private static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER_NUM];
        private static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER_NUM];
        private static byte[] _bEndCardCount = new byte[GameLogic.GAME_PLAYER_NUM];
        private static byte[] _bEndCardData = new byte[GameLogic.FULL_COUNT];

        private static byte _bGameType = 1;
        private static int _nInfoTickCount = 0;
        private static int _nSendCardTick = 0;
        private static int _nSendCardCount = 0;
        private static bool _bSendCard = false;

        private static int _nBrightCardTime = 1;
        private static int _nTempBrightCardTime = 1;
        private static int _nDoubleLimit = 0;

        private static bool[] _bBrightStart = new bool[GameLogic.GAME_PLAYER_NUM];
        private static bool[] _bUserBright = new bool[GameLogic.GAME_PLAYER_NUM];
        private static bool[] _bDoubleScore = new bool[GameLogic.GAME_PLAYER_NUM];
        private static bool[] _bUserDoubleOpt = new bool[GameLogic.GAME_PLAYER_NUM];

        private static int _nAutoCount = 0;
        private static int _nQuitDelay = 0;
        private static bool _bReqQuit = false;
        private static byte _firstLander = 0;
        //音效
        public AudioClip[] _GameSound = new AudioClip[10];
        public AudioClip[] _WomanSound = new AudioClip[60];
        public AudioClip[] _WomanSound1 = new AudioClip[60];
        public AudioClip[] _ManSound = new AudioClip[60];
        public AudioClip[] _ManSound1 = new AudioClip[60];

        //
        private static string[] _TableUserName = new string[GameLogic.GAME_PLAYER_NUM];

        //
        private static byte[] _bTipsCards = new byte[20];
        private static byte _bTipsCardsCount = 0;
        private static byte LandOutCardCount = 0;

        private bool _unBright = false; //是否执行过明牌


		private bool overtime = false;//是否超时
        private bool outCardLock = false; //在等待玩家加倍中地主可以强行通过右键出牌，加个额外条件，让等待过后才能用右键出牌
        public static bool isCanSmart = false;

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
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_clock[i] = GameObject.Find("scene_game/dlg_clock_" + i.ToString());
                    o_pass[i] = GameObject.Find("scene_game/sp_pass_" + i.ToString());
                    o_ready[i] = GameObject.Find("scene_game/sp_ready_" + i.ToString());
                    o_score[i] = GameObject.Find("scene_game/sp_score_" + i.ToString());
                    o_chat[i] = GameObject.Find("scene_game/dlg_chat_msg_" + i.ToString());
                    o_firstLand[i] = GameObject.Find("scene_game/firstLand_" + i.ToString());

                    o_info[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString());
                    o_info_nick[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_nick");
                    o_info_lvl[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lvl");
                    o_info_id[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_id");
                    o_info_score[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_score");
                    o_info_win[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_win");
                    o_info_lose[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lose");
                    o_info_run[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_run");


                    o_head_face[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_user_face");
					o_head_nick[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_nick");
					o_head_gold[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_gold");
                    o_head_flag[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_flag");
                    o_head_bright[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_Bright"); //明牌标识
                    o_head_double[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_Double"); //加倍标识
                    o_head_double[i].transform.localScale = Vector2.one;
					o_head_robot[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/dlg_card_count/sp_count_bg");//托管标识
                    o_head_robot[i].transform.localScale = Vector2.zero;
                    o_others[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/dlg_card_count");
                    o_out_cards[i] = GameObject.Find("scene_game/ctr_out_cards_" + i.ToString());

                    o_result_score[i] = GameObject.Find("scene_game/dlg_result/lbl_score_" + i.ToString());

					o_head_player[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString());
//                    o_user_speak[i] = GameObject.Find("scene_game/ctr_speak_" + i.ToString());

                }

                o_score_buttons = GameObject.Find("scene_game/dlg_score_buttons");
                o_qiang_buttons = GameObject.Find("scene_game/dlg_qiang_buttons");
                o_play_buttons = GameObject.Find("scene_game/dlg_play_buttons");
                o_ready_buttons = GameObject.Find("scene_game/dlg_ready_buttons");
                o_result = GameObject.Find("scene_game/dlg_result");
                o_btn_BigTuo = GameObject.Find("scene_game/btn_tuoguan");
                o_btn_rememberCard = GameObject.Find("scene_game/dlg_title_bar/sp_title_bg/btn_chat");
                o_btn_rememberCard.GetComponent<UIButton>().isEnabled = false;


                o_hand_cards = GameObject.Find("scene_game/ctr_hand_cards");
                o_back_cards = GameObject.Find("scene_game/ctr_back_cards");
                o_back_cards_two = GameObject.Find("scene_game/ctr_back_cards/backcard_double");
                o_back_cards_three = GameObject.Find("scene_game/ctr_back_cards/backcard_three");
                o_back_cards_four = GameObject.Find("scene_game/ctr_back_cards/backcard_four");
                landType = GameObject.Find("scene_game/landType");
                landWait = GameObject.Find("scene_game/wait_farm_double");
                

                o_btn_ready = GameObject.Find("scene_game/dlg_ready_buttons/btn_ready");
                //o_btn_quit = GameObject.Find("scene_game/dlg_ready_buttons/btn_quit");
               // o_btn_bright_start = GameObject.Find("scene_game/dlg_ready_buttons/btn_bright_start");


                o_btn_call_1 = GameObject.Find("scene_game/dlg_score_buttons/btn_call_1");
                o_btn_call_2 = GameObject.Find("scene_game/dlg_score_buttons/btn_call_2");
                o_btn_call_3 = GameObject.Find("scene_game/dlg_score_buttons/btn_call_3");
                o_btn_giveup = GameObject.Find("scene_game/dlg_score_buttons/btn_giveup");

                o_btn_outcard = GameObject.Find("scene_game/dlg_play_buttons/btn_out_card");
                o_btn_pass = GameObject.Find("scene_game/dlg_play_buttons/btn_pass_card");
                o_btn_tip = GameObject.Find("scene_game/dlg_play_buttons/btn_tip");
                o_btn_reset = GameObject.Find("scene_game/dlg_play_buttons/btn_reset");

                o_btn_qiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_qiang");
                o_btn_notqiang = GameObject.Find("scene_game/dlg_qiang_buttons/btn_notqiang");
                o_tip_bar = GameObject.Find("scene_game/dlg_tip_bar");


                //明牌按钮
                o_bright_buttons = GameObject.Find("scene_game/dlg_bright_buttons");
                //加倍，不加倍按钮
                o_double_buttons = GameObject.Find("scene_game/dlg_double_buttons");
                //叫地主，不叫按钮
                o_land_buttons = GameObject.Find("scene_game/dlg_land_buttons");

//                o_speak_timer = GameObject.Find("scene_game/dlg_speak_timer");

//                o_btn_speak = GameObject.Find("scene_game/btn_speak");

//                o_btn_speak_count = GameObject.Find("scene_game/btn_speak/lbl_count");

                o_bright_rate = GameObject.Find("scene_game/dlg_bright_buttons/btn_bright/sp_rate");

                o_current_time = GameObject.Find("scene_game/dlg_title_bar/lbl_curr_time");

                //GameEngine.Instance.SetTableEventHandle(new TableEventHandle(OnTableUserEvent));


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void InitGameView()
        {
            //Data

            _bReqQuit = false;
            _bStart = false;
            _bTimerType = TimerType.TIMER_START;
            _bBombTimes = 0;
            _bRocketTimes = 0;
            _wTotalTimes = 1;
            _doubleNum = 0;
            _nBaseScore = 0;
            _bAutoPlay = false;
            _bNoneOutCard = true;
            _bTurnOutType = 0;
            _bTurnCardCount = 0;
            _bHandCardCount = 0;
            _bMostUser = GameLogic.NULL_CHAIR;
            _bLander = GameLogic.NULL_CHAIR;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _wCurrentScore = 0;
            _bFirstOutUser = GameLogic.NULL_CHAIR;
            //_bEndTax			= 0;
            _nInfoTickCount = Environment.TickCount;
            _bGameType = 1;
            _nSendCardTick = 0;
            _nSendCardCount = 0;
            _bSendCard = false;
            _nBrightCardTime = 1;
            _nTempBrightCardTime = 1;
            _nAutoCount = 0;
            _bTipsCardsCount = 0;
            _unBright = false;
            LandOutCardCount = 0;
            isCanSmart = false;

            Array.Clear(_bTurnCardData, 0, _bTurnCardData.Length);
            Array.Clear(_bOthersCardCount, 0, _bOthersCardCount.Length);
            Array.Clear(_bHandCardData, 0, _bHandCardData.Length);
            Array.Clear(_bOutCardData, 0, _bOutCardData.Length);
            Array.Clear(_bOutCardCount, 0, _bOutCardCount.Length);
            Array.Clear(_bBackCardData, 0, _bBackCardData.Length);
            Array.Clear(_nEndUserScore, 0, _nEndUserScore.Length);
            Array.Clear(_nEndUserExp, 0, _nEndUserExp.Length);
            Array.Clear(_bEndCardCount, 0, _bEndCardCount.Length);
            Array.Clear(_bShowInfo, 0, _bShowInfo.Length);
            //UI
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                o_clock[i].SetActive(false);
                o_pass[i].SetActive(false);
                o_ready[i].SetActive(false);
                o_score[i].SetActive(false);
                o_info[i].SetActive(false);
                o_head_bright[i].SetActive(false);
                o_head_double[i].SetActive(false);
                o_firstLand[i].SetActive(false);
//                o_chat[i].SetActive(false);

//                o_user_speak[i].SetActive(false);
				o_head_player[i].SetActive(false);

                o_head_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
				o_head_nick[i].GetComponent<UILabel>().text = "";
				o_head_gold[i].GetComponent<UILabel>().text = "";
                o_head_flag[i].GetComponent<UISprite>().spriteName = "blank";

                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";

                o_result_score[i].GetComponent<UILabel>().text = "";
                o_others[i].SetActive(false);
                o_out_cards[i].SetActive(false);

                _bUserTrustee[i] = false;

                _bOthersCardData[i] = new byte[20];

                _bBrightStart[i] = false;
                _bDoubleScore[i] = false;
                _bUserBright[i] = false;
                _bUserDoubleOpt[i] = false;

                Array.Clear(_bOthersCardData[i], 0, 20);
            }

            o_score_buttons.SetActive(false);
            o_btn_call_1.GetComponent<UIButton>().isEnabled = true;
            o_btn_call_2.GetComponent<UIButton>().isEnabled = true;
            o_btn_call_3.GetComponent<UIButton>().isEnabled = true;
            o_qiang_buttons.SetActive(false);
            o_play_buttons.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_hand_cards.SetActive(false);
            o_back_cards.SetActive(false);
            o_back_cards_two.SetActive(false);
            o_back_cards_three.SetActive(false);
            o_back_cards_four.SetActive(false);
            landType.SetActive(false);
            landWait.SetActive(false);
            o_tip_bar.SetActive(false);
            o_btn_BigTuo.SetActive(false);


            o_bright_buttons.SetActive(false);
            o_double_buttons.SetActive(false);
            //o_btn_quit.SetActive(false);
            //o_btn_bright_start.SetActive(false);
            o_land_buttons.SetActive(false);
//            o_speak_timer.SetActive(false);

            CancelInvoke();
        }

        private void ResetGameView()
        {
            _bTimerType = TimerType.TIMER_START;
            _bBombTimes = 0;
            _bRocketTimes = 0;
            _wTotalTimes = 1;
            _doubleNum = 0;
            _bAutoPlay = false;
            _bNoneOutCard = true;
            _bTurnOutType = 0;
            _bTurnCardCount = 0;
            _bHandCardCount = 0;
            _bMostUser = GameLogic.NULL_CHAIR;
            _bLander = GameLogic.NULL_CHAIR;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _wCurrentScore = 0;
            _bFirstOutUser = GameLogic.NULL_CHAIR;
            _nInfoTickCount = Environment.TickCount;
            _nSendCardTick = 0;
            _nSendCardCount = 0;
            _bSendCard = false;
            _nBrightCardTime = 1;
            _nTempBrightCardTime = 1;
            _nAutoCount = 0;
            _bTipsCardsCount = 0;
            _unBright = false;
            LandOutCardCount = 0;
            isCanSmart = false;

            Array.Clear(_bTurnCardData, 0, _bTurnCardData.Length);
            Array.Clear(_bOthersCardCount, 0, _bOthersCardCount.Length);
            Array.Clear(_bHandCardData, 0, _bHandCardData.Length);
            Array.Clear(_bOutCardData, 0, _bOutCardData.Length);
            Array.Clear(_bOutCardCount, 0, _bOutCardCount.Length);
            Array.Clear(_bBackCardData, 0, _bBackCardData.Length);
            Array.Clear(_nEndUserScore, 0, _nEndUserScore.Length);
            Array.Clear(_nEndUserExp, 0, _nEndUserExp.Length);
            Array.Clear(_bEndCardCount, 0, _bEndCardCount.Length);
            Array.Clear(_bShowInfo, 0, _bShowInfo.Length);

            //UI
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                o_clock[i].SetActive(false);
                o_pass[i].SetActive(false);
                o_ready[i].SetActive(false);
                o_score[i].SetActive(false);
                o_firstLand[i].SetActive(false);
                o_info[i].SetActive(false);
                o_head_bright[i].SetActive(false);
                o_head_double[i].SetActive(false);
//                o_chat[i].SetActive(false);
//                o_user_speak[i].SetActive(false);
                o_head_flag[i].GetComponent<UISprite>().spriteName = "blank";

                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";

                o_result_score[i].GetComponent<UILabel>().text = "";
                o_others[i].SetActive(false);
                o_out_cards[i].SetActive(false);

                _bUserTrustee[i] = false;

                _bOthersCardData[i] = new byte[20];

                //_bBrightStart[i] = false;
                _bDoubleScore[i] = false;
                //_bUserBright[i]  = false;
                _bUserDoubleOpt[i] = false;

                Array.Clear(_bOthersCardData[i], 0, 20);
            }

            o_score_buttons.SetActive(false);
            o_btn_call_1.GetComponent<UIButton>().isEnabled = true;
            o_btn_call_2.GetComponent<UIButton>().isEnabled = true;
            o_btn_call_3.GetComponent<UIButton>().isEnabled = true;

            o_qiang_buttons.SetActive(false);
            o_play_buttons.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_hand_cards.SetActive(false);
            o_back_cards.SetActive(false);
            o_back_cards_two.SetActive(false);
            o_back_cards_three.SetActive(false);
            o_back_cards_four.SetActive(false);
            landType.SetActive(false);
            landWait.SetActive(false);
            o_tip_bar.SetActive(false);
            o_bright_buttons.SetActive(false);
            o_double_buttons.SetActive(false);
            //o_btn_quit.SetActive(false);
            //o_btn_bright_start.SetActive(false);
            o_land_buttons.SetActive(false);
            o_btn_BigTuo.SetActive(false);
//            o_speak_timer.SetActive(false);

            Resources.UnloadUnusedAssets();
            GC.Collect();

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
                        UpdateUserView();
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

                        break;
                    }
                case TableEvents.GAME_ENTER:
                    {
                        GameEngine.Instance.SendUserSetting();

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
						// 初始化玩家的托管头像
						o_head_robot[0].transform.localScale = Vector2.zero;
                        //
                        for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                        {
                            if (GameEngine.Instance.GetTableUserItem(i) != null)
                            {
                                SetUserReady(i, true);
                            }
                        }

                        PlaySound(SoundType.CALLSCORE);
						
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
						Invoke("OnConfirmBackOKIvk",5.0f);
						break;
					}
            }

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

			//声音初始化
			GameObject.Find("Panel").GetComponent<AudioSource>().volume=0.2f;
			GameObject.Find("Panel").GetComponent<AudioSource>().Play();
			NGUITools.soundVolume=1.0f;

        }

        private void FixedUpdate()
        {
            if ((Environment.TickCount - _nInfoTickCount) > 5000)
            {
                ClearAllInfo();
                _nInfoTickCount = 0;
            }

            if (_bSendCard == true)
            {

                if ((Environment.TickCount - _nSendCardTick) > 250)
                {
                    _nSendCardTick = Environment.TickCount;
                    if (_nSendCardCount >= _bHandCardCount)
                    {
                        DispatchCardFinish();
                    }
                    else
                    {
                        DispatchCard(_nSendCardCount);
                        _nSendCardCount++;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
            {
                OnBackIvk();
            }

            if ((System.Environment.TickCount - _nQuitDelay) > 1000 && _bReqQuit == true)
            {
                _bReqQuit = false;
                _nQuitDelay = System.Environment.TickCount;
                GameEngine.Instance.Quit();
            }

            if (Input.GetMouseButtonUp(1) && outCardLock == true)
            {
                //判断当前牌是不是能大过上家的牌
                if (VerdictOutCard() == true && _bCurrentUser == GetSelfChair())
                {
                    OnOutCardIvk();
                }               
            }

			// 更新用户拉动音效滑条的数值
			UISetting.Instance.OnEffectChange ();
			UISetting.Instance.OnMusicChange ();
        }

        private void Update()
        {
        }

        private void Quit()
        {
            GameEngine.Instance.Quit();
        }

        private void Send(NPacket packet, int delay)
        {
            GameEngine.Instance.Send(packet);
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


        //游戏设置消息处理函数
        private void OnGameOptionResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
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
                case (byte) GameLogic.GS_WK_SCORE:
                    {
                        SwitchScoreSceneView(packet);
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
            PlaySound(SoundType.CALLSCORE);
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

            switch (subcmd)
            {
                case SubCmd.SUB_S_SEND_CARD:
                    {
                        OnSendCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_LAND_SCORE:
                    {
                        OnLandScoreResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_START:
                    {
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_OUT_CARD:
                    {
                        OnOutCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_PASS_CARD:
                    {
                        OnPassCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:
                    {
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmd.SUB_C_TRUSTEE:
                    {
                        OnTuoGuanResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_USER_BRIGHT:
                    {
                        OnUserBrightResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_USER_DOUBLE:
                    {
                        OnUserDoubleResp(packet);
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

        //用户明牌消息处理函数
        private void OnUserBrightResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                ushort wChairID = packet.GetUShort();
                ushort wLandScore = packet.GetUShort();
                byte bType = packet.GetByte();
                _bCurrentUser = (byte) packet.GetUShort();
                _nBrightCardTime = wLandScore;

                _unBright = true;
                o_bright_buttons.SetActive(false);
                if (wChairID == GetSelfChair())
                {
                    o_head_bright[0].SetActive(true);
                }
                else
                {
                    byte tempId = ChairToView((byte)wChairID);
                    o_head_bright[tempId].SetActive(true);
                }


                if (bType > 0)
                {
                    _bBrightStart[wChairID] = false;
                    _bUserBright[wChairID] = true;
                    if (wChairID != GetSelfChair())
                    {
                        packet.GetBytes(ref _bOthersCardData[wChairID], 20);
                        SetPlayerCardCount((byte) wChairID, (byte) _nSendCardCount);
                    }

                    PlayGameSound(GameSoundType.OPTION_MING, GameEngine.Instance.GetTableUserItem(wChairID).Gender);
                }
                else
                {
                    _bBrightStart[wChairID] = true;
                    _bUserBright[wChairID] = true;
                }

                SetTotalTimes(wLandScore);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户加倍消息处理函数
        private void OnUserDoubleResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                ushort wChairID = packet.GetUShort();
                bool bDouble = packet.GetBool();
                ushort wLandScore = packet.GetUShort();

                _bDoubleScore[wChairID] = bDouble;
                _bUserDoubleOpt[wChairID] = true;
                _wTotalTimes = wLandScore;

                SetUserDouble((byte)wChairID, bDouble);
                if (bDouble == true)
                {
                    if (wChairID == GetSelfChair())
                    {
                        o_head_double[0].SetActive(true);
                        o_double_buttons.SetActive(false);
                    }
                    else 
                    {
                        byte tempId = ChairToView((byte)wChairID);
                        o_head_double[tempId].SetActive(true);
                    }   
                }
                else  //配合服务器定时器
                {
                    if (wChairID == GetSelfChair())
                    {
                        o_double_buttons.SetActive(false);
                        SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                    }
                }

                SetTotalTimes(wLandScore);

                if (bDouble)
                {
                    PlayGameSound(GameSoundType.OPTION_JIABEI, GameEngine.Instance.GetTableUserItem(wChairID).Gender);
                }
                else
                {
                    //PlayGameSound(GameSoundType.OPTION_NOTJIABEI,GameEngine.Instance.GetTableUserItem(wChairID).Gender);
                }
				//如果是地主的加倍信息返回,则显示出牌界面
				//if( wChairID == _bLander)
				//{
				//	DoubleFinish();
				//}
                _doubleNum += 1;
                if (_doubleNum == 3)
                {
                    DoubleFinish();
                }
                
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //开始打牌消息处理函数
        private void OnGameStartResp(NPacket packet)
        {
            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_PLAYING;
                packet.BeginRead();
                _bLander = (byte) packet.GetUShort();
                ushort wCurrentScore = packet.GetUShort();
                _bCurrentUser = (byte) packet.GetUShort();
                packet.GetBytes(ref _bBackCardData, 3);
                _nDoubleLimit = (int) packet.GetUInt();
                ushort wLastUser = packet.GetUShort();
                ushort wLastScore = packet.GetUShort();

                //

                SetCallScore((byte) wLastUser, wLastScore);
               
                //上轮叫分
                byte bGender = GameEngine.Instance.GetTableUserItem(wLastUser).Gender;

                if (_bGameType == 0)
                {
                    if (wLastScore == 1)
                    {

                        PlayGameSound(GameSoundType.OPTION_CALL_1, bGender);
                    }
                    else if (wLastScore == 2)
                    {

                        PlayGameSound(GameSoundType.OPTION_CALL_2, bGender);
                    }
                    else if (wLastScore == 3)
                    {

                        PlayGameSound(GameSoundType.OPTION_CALL_3, bGender);
                    }
                    else if (wLastScore == 255)
                    {
                        PlayGameSound(GameSoundType.OPTION_NOTCALL, bGender);
                    }
                }
                else
                {
                    if (wLastScore <= 3)
                    {
                        PlayGameSound(GameSoundType.OPTION_CALL, bGender);
                    }
                    else if (wLastScore < 255)
                    {
                        float randomCount = UnityEngine.Random.Range(0, 2.0f);
                        if (randomCount < 1)
                        {
                            PlayGameSound(GameSoundType.OPTION_QIANG_0, bGender);
                        }
                        else 
                        {
                            PlayGameSound(GameSoundType.OPTION_QIANG_1, bGender);
                        }
                        
                    }
                    else
                    {
                        if (wCurrentScore >= 3)
                        {
                            PlayGameSound(GameSoundType.OPTION_NOTQIANG, bGender);
                        }
                        else
                        {
                            PlayGameSound(GameSoundType.OPTION_NOTCALL, bGender);
                        }
                    }
                }
                /*
                if(wLastScore <=3)
                {
                    PlayGameSound(GameSoundType.OPTION_CALL,bGender);
                }
                else if(wLastScore<255)
                {
                    PlayGameSound(GameSoundType.OPTION_QIANG,bGender);
                }
                else
                {
                    if(_wCurrentScore>=3)
                    {
                        PlayGameSound(GameSoundType.OPTION_NOTQIANG,bGender);
                    }
                    else
                    {
                        PlayGameSound(GameSoundType.OPTION_NOTCALL,bGender);
                    }
                }
                */
                //
                _wCurrentScore = wCurrentScore;


                _wTotalTimes = _wCurrentScore;
                _bTurnCardCount = 0;
                _bTurnOutType = GameLogic.CT_ERROR;
                _bFirstOutUser = _bCurrentUser;
                //
                o_qiang_buttons.SetActive(false);
                o_score_buttons.SetActive(false);
                o_ready_buttons.SetActive(false);
                o_play_buttons.SetActive(false);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

                //
                SetTotalTimes(_wTotalTimes);
                //
                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    if (i == _bLander)
                    {
                        if (_bUserBright[i] == true)
                        {
                            _bOthersCardCount[i] = 20;
                            _bOthersCardData[i][17] = _bBackCardData[0];
                            _bOthersCardData[i][18] = _bBackCardData[1];
                            _bOthersCardData[i][19] = _bBackCardData[2];

                            GameLogic.SortCardList(ref _bOthersCardData[i], _bOthersCardCount[i], GameLogic.ST_ORDER);
                        }
                        else
                        {
                            _bOthersCardCount[i] = 20;
                        }

                    }
                    else
                    {
                        if (_bUserBright[i] == true)
                        {
                            _bOthersCardCount[i] = 17;
                            GameLogic.SortCardList(ref _bOthersCardData[i], _bOthersCardCount[i], GameLogic.ST_ORDER);
                        }
                        else
                        {
                            _bOthersCardCount[i] = 17;
                        }

                    }

                    //
                    /*
                    if(i!=GetSelfChair())
                    {
                        if(_bUserBright[i]==false && _bBrightStart[i]==false)
                        {
                            Array.Clear(_bOthersCardData[i],0,20);
                        }
                    }
                    */
                    SetPlayerCardCount(i, _bOthersCardCount[i]);
                }
                //
                SetLander(_bLander);
                //
                SetBackCardData(_bBackCardData, (byte) GameLogic.BACK_COUNT);
                //

                if ((GameLogic.GetCardLogicValue(_bBackCardData[0]) == GameLogic.GetCardLogicValue(_bBackCardData[1]))
                    || ((GameLogic.GetCardLogicValue(_bBackCardData[0]) == GameLogic.GetCardLogicValue(_bBackCardData[2]))
                            && (GameLogic.GetCardLogicValue(_bBackCardData[0]) != GameLogic.GetCardLogicValue(_bBackCardData[2])))
                    || ((GameLogic.GetCardLogicValue(_bBackCardData[1]) == GameLogic.GetCardLogicValue(_bBackCardData[2]))
                            && (GameLogic.GetCardLogicValue(_bBackCardData[0]) != GameLogic.GetCardLogicValue(_bBackCardData[2])))
                    || _bBackCardData[0] == 0x4F 
                    || _bBackCardData[0] == 0x4E
                    )
                {
                    //两倍
                    o_back_cards_two.SetActive(true);
                    o_back_cards_three.SetActive(false);
                    o_back_cards_four.SetActive(false);
                }
                else if (
                        ((GameLogic.GetCardLogicValue(_bBackCardData[0]) - GameLogic.GetCardLogicValue(_bBackCardData[1])) == (GameLogic.GetCardLogicValue(_bBackCardData[1]) - GameLogic.GetCardLogicValue(_bBackCardData[2]))
                         &&(GameLogic.GetCardLogicValue(_bBackCardData[0]) - GameLogic.GetCardLogicValue(_bBackCardData[1])) == 1)
                        || (GameLogic.GetCardColor(_bBackCardData[0]) == GameLogic.GetCardColor(_bBackCardData[1]) && GameLogic.GetCardColor(_bBackCardData[0]) == GameLogic.GetCardColor(_bBackCardData[2])) 
                        || (GameLogic.GetCardLogicValue(_bBackCardData[0]) == GameLogic.GetCardLogicValue(_bBackCardData[1])) && (GameLogic.GetCardLogicValue(_bBackCardData[0]) == GameLogic.GetCardLogicValue(_bBackCardData[2]))
                        )                   
                {
                    //三倍
                    o_back_cards_two.SetActive(false);
                    o_back_cards_three.SetActive(true);
                    o_back_cards_four.SetActive(false);
                }
                else if (
                        (_bBackCardData[0] == 0x4F || _bBackCardData[0] == 0x4E) && (_bBackCardData[1] == 0x4E || _bBackCardData[1] == 0x4F)
                        || (_bBackCardData[0] == 0x4F || _bBackCardData[0] == 0x4E) && (_bBackCardData[2] == 0x4E || _bBackCardData[2] == 0x4F)
                        || (_bBackCardData[1] == 0x4F || _bBackCardData[1] == 0x4E) && (_bBackCardData[2] == 0x4E || _bBackCardData[2] == 0x4F)
                        )
                {
                    //四倍
                    o_back_cards_two.SetActive(false);
                    o_back_cards_three.SetActive(false);
                    o_back_cards_four.SetActive(true);
                }


                if (_bLander == GetSelfChair())
                {
                    Buffer.BlockCopy(_bBackCardData, 0, _bHandCardData, _bHandCardCount, GameLogic.BACK_COUNT);
                    _bHandCardCount += (byte) GameLogic.BACK_COUNT;
                    GameLogic.SortCardList(ref _bHandCardData, _bHandCardCount, GameLogic.ST_ORDER);
                    SetHandCardData(_bHandCardData, _bHandCardCount);
                    o_hand_cards.GetComponent<UICardControl>().SetShootCard(_bBackCardData, (byte)GameLogic.BACK_COUNT);
                    
                }
                //如果是欢乐斗地主，显示加倍按钮
                if (_bGameType == 0)
                {
                    //
                    if (_bCurrentUser == GetSelfChair())
                    {
                        o_play_buttons.SetActive(true);

                        o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                        o_btn_pass.GetComponent<UIButton>().isEnabled = false;

                        //

                    }
                    //
                    if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                    {
                        SetUserClock(_bCurrentUser, 1, TimerType.TIMER_OUTCARD);
                    }
                    else
                    {
                        SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bHandCardCount), TimerType.TIMER_OUTCARD);
                    }
                    PlaySound(SoundType.START);
                    SetCallScore(GameLogic.NULL_CHAIR, 0);
                }
                else
                {
                    DoubleStart();
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏发牌消息处理函数
        private void OnSendCardResp(NPacket packet)
        {

            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_SCORE;
                packet.BeginRead();
                _bCurrentUser = (byte) packet.GetUShort();
                byte[] bcarddata = new byte[60];
                packet.GetBytes(ref bcarddata, 60);
                byte[] bbackdata = new byte[3];
                packet.GetBytes(ref bbackdata, 3);
                _wTotalTimes = packet.GetUShort();

                //
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    _bBrightStart[i] = packet.GetBool();
                    if (_bBrightStart[i] == true)
                    {
                        _bUserBright[i] = true;
                        o_bright_buttons.SetActive(false);
                    }
                    else
                    {
                        _bUserBright[i] = false;
                    }

                }
                //
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    if (i == GetSelfChair())
                    {
                        Buffer.BlockCopy(bcarddata, i*20, _bHandCardData, 0, 20);
                        _bHandCardCount = 17;
                    }
                    else
                    {
                        Buffer.BlockCopy(bcarddata, i*20, _bOthersCardData[i], 0, 20);
                        _bOthersCardCount[i] = 17;
                    }

                }
                //如果是普通斗地主，清除其他手中牌
                if (_bGameType == 0)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            _bOthersCardData[j][i] = 0;
                        }
                    }
                }
                PlaySound(SoundType.CALLSCORE);
                //
                SetBackCardData(null, 0);

                SetTotalTimes(_wTotalTimes);
                //
                SetLander(GameLogic.NULL_CHAIR);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                SetCallScore(GameLogic.NULL_CHAIR, 0);
                //
                o_ready_buttons.SetActive(false);
                //
                DispatchCardStart();


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户叫分消息处理函数
        private void OnLandScoreResp(NPacket packet)
        {

          /*  try
            {*/
                packet.BeginRead();
                _bLander = (byte) packet.GetUShort();
                _bCurrentUser = (byte) packet.GetUShort();
                ushort wScore = packet.GetUShort();
                ushort wCurrentScore = packet.GetUShort();

                o_firstLand[_firstLander].SetActive(false);
                    
                if ((_bBrightStart[0] == true) || (_bBrightStart[1] == true) || (_bBrightStart[2] == true))
                {
                    _nBrightCardTime = 8;
                }

                //配合服务器定时器
                if (_bLander == GetSelfChair())
                {
                    o_score_buttons.SetActive(false);
                    o_land_buttons.SetActive(false);
                    o_qiang_buttons.SetActive(false);

                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                }

                SetCallScore(_bLander, wScore);

                if (wScore != 255 && wScore < 25)
                {
                    _wTotalTimes = (ushort) (wScore*_nBrightCardTime);
                    SetTotalTimes(_wTotalTimes);
                    _wCurrentScore = wScore;
                }

                if (_bCurrentUser < GameLogic.GAME_PLAYER_NUM)
                {
                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                    uint ntime = (uint) ((_bUserTrustee[_bCurrentUser] == true) ? 1 : GameLogic.TIME_LAND_SCORE);
                    SetUserClock(_bCurrentUser, ntime, TimerType.TIMER_SCORE);
                }
                byte bGender = GameEngine.Instance.GetTableUserItem(_bLander).Gender;

                if (_bGameType == 0)
                {
                    if (wScore == 1)
                    {
                        PlayGameSound(GameSoundType.OPTION_CALL_1, bGender);
                    }
                    else if (wScore == 2)
                    {
                        PlayGameSound(GameSoundType.OPTION_CALL_2, bGender);
                    }
                    else if (wScore == 3)
                    {
                        PlayGameSound(GameSoundType.OPTION_CALL_3, bGender);
                    }
                    else if (wScore == 255)
                    {
                        PlayGameSound(GameSoundType.OPTION_NOTCALL, bGender);
                    }
                }
                else
                {
                    if (wScore <= 3)
                    {
                        PlayGameSound(GameSoundType.OPTION_CALL, bGender);
                    }
                    else if (wScore < 255)
                    {
                        float randomCount = UnityEngine.Random.Range(0, 2.0f);
                        if (randomCount < 1)
                        {
                            PlayGameSound(GameSoundType.OPTION_QIANG_0, bGender);
                        }
                        else
                        {
                            PlayGameSound(GameSoundType.OPTION_QIANG_1, bGender);
                        }
                    }
                    else
                    {
                        if (wCurrentScore >= 3)
                        {
                            PlayGameSound(GameSoundType.OPTION_NOTQIANG, bGender);
                        }
                        else
                        {
                            PlayGameSound(GameSoundType.OPTION_NOTCALL, bGender);
                        }
                    }
                }
                if (_bCurrentUser == GetSelfChair())
                {
                    if (_bGameType == 0)
                    {
                        if (wCurrentScore > 2)
                        {
                            o_qiang_buttons.SetActive(true);
                            o_score_buttons.SetActive(false);
                        }
                        else
                        {
                            o_qiang_buttons.SetActive(false);
                            o_score_buttons.SetActive(true);

                            if (wCurrentScore != 255)
                            {
                                o_btn_call_1.GetComponent<UIButton>().isEnabled = (wCurrentScore == 0 ? true : false);
                                o_btn_call_2.GetComponent<UIButton>().isEnabled = (wCurrentScore <= 1 ? true : false);
                                o_btn_call_3.GetComponent<UIButton>().isEnabled = (wCurrentScore <= 2 ? true : false);
                            }
                        }
                    }
                    else
                    {
                        if (wCurrentScore >= 3)
                        {
                            o_qiang_buttons.SetActive(true);
                            o_land_buttons.SetActive(false);
                        }
                        else
                        {
                            o_qiang_buttons.SetActive(false);
                            o_land_buttons.SetActive(true);
                        }
                    }
                }

            /*}
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }*/
        }

        //用户出牌消息处理函数
        private void OnOutCardResp(NPacket packet)
        {
            try
            {
                SetUserDouble(GameLogic.NULL_CHAIR, false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                o_play_buttons.SetActive(false);
                o_double_buttons.SetActive(false);
                o_land_buttons.SetActive(false);

                _bTipsCardsCount = 0;

                //Parse packet
                packet.BeginRead();
                byte bCardCount = packet.GetByte();
                _bCurrentUser = (byte) packet.GetUShort();
                byte bOutUser = (byte) packet.GetUShort();
                byte[] bCardData = new byte[20];
                packet.GetBytes(ref bCardData, 20);



                //把地主出牌次数记住，检测反春
                if (bOutUser == _bLander)
                {
                    LandOutCardCount += 1;
                }
                    
                if (bOutUser != GetSelfChair())
                {
                    SetOutCardData(bOutUser, bCardData, bCardCount);
                }
                
                //配合服务器定时器
                if (bOutUser == GetSelfChair())
                {
                    //SetOutCardData(GetSelfChair(), bCardData, bCardCount);
                    GameLogic.RemoveCard(bCardData, bCardCount, ref _bHandCardData, ref _bHandCardCount);
                    SetHandCardData(_bHandCardData, _bHandCardCount);
                    o_play_buttons.SetActive(false);
                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                }
                
                if (bCardCount > 0)
                {
                    _bFirstOutUser = bOutUser;
                }
                //clear turn view
                if (_bTurnCardCount == 0)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        if (i != bOutUser)
                        {
                            SetOutCardData(i, null, 0);
                        }
                    }
                    SetUserPass(GameLogic.NULL_CHAIR, false);
                }

					
				if (bOutUser != GetSelfChair() ||overtime == true)
                {
                    #region GameSound
				overtime = false;
                float randomCount = UnityEngine.Random.Range(0, 3.0f);
                byte bGender = GameEngine.Instance.GetTableUserItem(bOutUser).Gender;
                byte bOutType = GameLogic.GetCardType(bCardData, bCardCount);					
                if (bOutType == GameLogic.CT_BOMB_CARD)
                {

                    PlayGameSound(GameSoundType.CARD_BOMB, bGender);
                    if (randomCount < 1.5f)
                    {
                        PlaySound(SoundType.BOMB_0);
                    }
                    else
                    {
                        PlaySound(SoundType.BOMB_1);
                    }
                        
                    UIEffect.Instance.ShowBomb(true);
                }
                else if (bOutType == GameLogic.CT_MISSILE_CARD)
                {
                    PlayGameSound(GameSoundType.CARD_ROCKET, bGender);
					PlaySound(SoundType.MISSILE);
                    UIEffect.Instance.ShowRocket(true);
                }
				else if ((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO||bOutType == GameLogic.CT_THREE_LINE)  
						        && bCardCount >= 6&&(bCardCount<=9||bCardCount==12||bCardCount==16||bCardCount==15))
	            {
					if (bOutType == _bTurnOutType)
					{
						if(randomCount<1.5f)
						{
							PlayGameSound(GameSoundType.CARD_DANI,bGender);
						}
						else
						{
							PlayGameSound(GameSoundType.CARD_YASI,bGender);
						}
								
					}
					else
					{						
						PlayGameSound(GameSoundType.CARD_PLANE, bGender);
					}
	                   
	                PlaySound(SoundType.PLANE);
	                UIEffect.Instance.ShowPlane(true);
				}else if((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
						        &&bCardCount>=10&&(bCardCount!=12||bCardCount!=16||bCardCount!=15))
				{
					if (bOutType == _bTurnOutType)
					{
						if(randomCount<1.5f)
						{
							PlayGameSound(GameSoundType.CARD_DANI,bGender);
						}
						else
						{
							PlayGameSound(GameSoundType.CARD_YASI,bGender);
						}
								
					}
					else
					{						
						PlayGameSound(GameSoundType.CARD_PLANE_1, bGender);
					}
						
					PlaySound(SoundType.PLANE);
					UIEffect.Instance.ShowPlane(true);
				}
                else
                {
                    if (bOutType == GameLogic.CT_SINGLE)
                    {
                        byte card = GameLogic.GetCardValue(bCardData[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_A, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_2, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_3, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_4, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_5, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_6, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_7, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_8, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_9, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_10, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_J, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_Q, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_K, bGender);
                        }
                        if (bCardData[0] == 0x4E)
                        {
                            PlayGameSound(GameSoundType.CARD_S, bGender);
                        }
                        if (bCardData[0] == 0x4F)
                        {
                            PlayGameSound(GameSoundType.CARD_B, bGender);
                        }
                        //
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE)
                    {
                        byte card = GameLogic.GetCardValue(bCardData[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_AA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_22, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_33, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_44, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_55, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_66, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_77, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_88, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_99, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_11, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_JJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_QQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_KK, bGender);
                        }
                        //
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_THREE)
                    {
                        byte card = GameLogic.GetCardValue(bCardData[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_AAA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_222, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_333, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_444, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_555, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_666, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_777, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_888, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_999, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_111, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_JJJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_QQQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_KKK, bGender);
                        }
                        //
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_TWO)
                    {

						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}

						}
						else
						{
							PlayGameSound(GameSoundType.CARD_FOUR_2D, bGender);
						}
                            
                        PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_ONE)
                    {

						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
								
						}
						else
						{
                            PlayGameSound(GameSoundType.CARD_FOUR_2, bGender);
						}
						PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_SINGLE_LINE)
                    {
						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
								
						}
						else
						{						
                           	PlayGameSound(GameSoundType.CARD_LINE, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
						PlaySound(SoundType.SUNZI);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
                    {
						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
								
						}
						else
						{
                            PlayGameSound(GameSoundType.CARD_THREE_1D, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE)
                    {
						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
								
						}
						else
						{
                            PlayGameSound(GameSoundType.CARD_THREE_1, bGender);
						}
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE_LINE)
                    {
						if (bOutType == _bTurnOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
								
						}
						else
						{
                           	PlayGameSound(GameSoundType.CARD_DBL_LINE, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
						PlaySound(SoundType.SUNZI);
                    }
                        
                }

                    #endregion
                }
                
				//record out card
				_bTurnCardCount = bCardCount;
				_bTurnOutType = GameLogic.GetCardType(bCardData, bCardCount);
				Buffer.BlockCopy(bCardData, 0, _bTurnCardData, 0, bCardCount);
				//bomb handle
				if (_bTurnOutType == GameLogic.CT_BOMB_CARD)
				{
					_bBombTimes++;
					_wTotalTimes *= 2;
					SetTotalTimes(_wTotalTimes);
					
				}
				
				if (_bTurnOutType == GameLogic.CT_MISSILE_CARD)
				{
					_bRocketTimes++;
					_wTotalTimes *= 2;
					SetTotalTimes(_wTotalTimes);
					
				}

                //hand card count
                if (_bUserBright[bOutUser] == true && bOutUser != GetSelfChair())
                {

                    GameLogic.RemoveCard(bCardData, bCardCount, ref _bOthersCardData[bOutUser],
                                         ref _bOthersCardCount[bOutUser]);
                    SetPlayerCardCount(bOutUser, _bOthersCardCount[bOutUser]);
                }
                else
                {
                    _bOthersCardCount[bOutUser] -= bCardCount;
                    SetPlayerCardCount(bOutUser, _bOthersCardCount[bOutUser]);
                }
                //less one card warns
                if (bOutUser != GetSelfChair())
                {
                    if (_bOthersCardCount[bOutUser] == 1)
                    {

                        PlayGameSound(GameSoundType.OPTION_WARN_1, GameEngine.Instance.GetTableUserItem(bOutUser).Gender);
                    }

                    if (_bOthersCardCount[bOutUser] == 2)
                    {

                        PlayGameSound(GameSoundType.OPTION_WARN_2, GameEngine.Instance.GetTableUserItem(bOutUser).Gender);
                    }
                    if (_bOthersCardCount[bOutUser] == 3)
                    {
                        PlayGameSound(GameSoundType.OPTION_WARN_3, GameEngine.Instance.GetTableUserItem(bOutUser).Gender);
                    }


                }
                //Most
                if (_bCurrentUser == bOutUser)
                {
                    _bTurnCardCount = 0;
                    _bTurnOutType = GameLogic.CT_ERROR;
                    _bMostUser = _bCurrentUser;
                    Array.Clear(_bTurnCardData, 0, _bTurnCardData.Length);
                    //
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        if (i != bOutUser)
                        {
                            SetUserPass(i, true);
                            SetOutCardData(i, null, 0);
                        }
                    }

                    //
                    PlaySound(SoundType.MOSTCARD);

                    //Timer
                    SetUserClock(_bCurrentUser, 3, TimerType.TIMER_MOSTCARD);
                    return;
                }


                //player setting
                if (_bCurrentUser != GameLogic.NULL_CHAIR)
                {
                    SetUserPass(_bCurrentUser, false);
                    SetOutCardData(_bCurrentUser, null, 0);
                }

                bool tempLock = false;
                //player setting
                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == false)
                {
                    o_play_buttons.SetActive(true);
                    o_btn_outcard.GetComponent<UIButton>().isEnabled = true;

                    o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                    o_btn_pass.GetComponent<UIButton>().isEnabled = (_bTurnCardCount > 0 ? true : false);

                    byte[] cards = new byte[20];
                    byte count = 0;
                    o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref cards, ref count);
                    if (count > 0)
                    {
                        o_btn_reset.GetComponent<UIButton>().isEnabled = true;
                    }
                    else
                    {
                        o_btn_reset.GetComponent<UIButton>().isEnabled = false;
                    }

                    if (_bTurnCardCount > 0)
                    {
                        tagOutCardResult OutCardResult = new tagOutCardResult();
                        GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,ref OutCardResult);
                        if (OutCardResult.cbCardCount > 0)
                        {
                            ShowTipMsg(false);
                        }
                        else
                        {
                            tempLock = true;
                            ShowTipMsg(true);
                            if (_bHandCardCount == 1 && _bTurnCardCount > 1)
                            {
                                OnPassCardIvk();
                            }
                        }
                    }
                }
                if (_bHandCardCount == 1)
                {
                    _nAutoCount = 0;
                }

                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                {
                    SetUserClock(_bCurrentUser, 1, TimerType.TIMER_OUTCARD);
                }
                else
                {
                    if (tempLock == true)
                    {
                        SetUserClock(GetSelfChair(), 5, TimerType.TIMER_OUTCARD);
                    }
                    else
                    {
                        SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bOthersCardCount[_bCurrentUser]), TimerType.TIMER_OUTCARD);
                    }
                }
                _bNoneOutCard = false;

            }
            catch (Exception ex)
            {
               // UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户托管消息处理函数
        private void OnTuoGuanResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                //
                byte bchair = (byte) packet.GetUShort();
                bool bAuto = packet.GetBool();

                SetUserTuoGuan(bchair, bAuto);

                if (bchair == GetSelfChair())
                {
                    o_head_robot[0].SetActive(bAuto);
                }
                else
                {
                    byte tempId = ChairToView(bchair);
                    o_head_robot[tempId].SetActive(bAuto);
                    o_head_robot[tempId].transform.localScale = Vector2.one;
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户不出消息处理函数
        private void OnPassCardResp(NPacket packet)
        {
            try
            {
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                o_play_buttons.SetActive(false);

                _bTipsCardsCount = 0;

                //Parse packet
                packet.BeginRead();
                bool bNewTurn = packet.GetBool();
                byte bPassUser = (byte) packet.GetUShort();
                _bCurrentUser = (byte) packet.GetUShort();

                //配合服务器定时器
                if (bPassUser == GetSelfChair())
                {
                    SetUserPass(GetSelfChair(), true);
                    o_play_buttons.SetActive(false);
                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                }

                //pass player view
                if (bPassUser != GetSelfChair())
                {
                    byte bGender = GameEngine.Instance.GetTableUserItem(bPassUser).Gender;
                    SetOutCardData(bPassUser, null, 0);
                    SetUserPass(bPassUser, true);
                    PlayGameSound(GameSoundType.OPTION_PASS, bGender);
                }
                //
                if (bNewTurn == true)
                {
                    _bTurnCardCount = 0;
                    _bTurnOutType = GameLogic.CT_ERROR;
                }

                //current player view
                SetUserPass(_bCurrentUser, false);
                SetOutCardData(_bCurrentUser, null, 0);

                //myself view
                bool tempLock = false;//判定能否大过上家的牌
                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == false )
                {
                    o_play_buttons.SetActive(true);

                    o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                    o_btn_pass.GetComponent<UIButton>().isEnabled = ((_bTurnCardCount > 0) ? true : false);
                    o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                    //ResetHandCardData();

                    if (_bTurnCardCount > 0)
                    {
                        tagOutCardResult OutCardResult = new tagOutCardResult();
                        GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,ref OutCardResult);
                                              
                        if (OutCardResult.cbCardCount > 0)
                        {
                            ShowTipMsg(false);
                        }
                        else
                        {
                            tempLock = true;
                            ShowTipMsg(true);
                        }
                    }
                }

                //set timer
                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                {
                    SetUserClock(_bCurrentUser, 1, TimerType.TIMER_OUTCARD);
                }
                else
                {
                    if (tempLock == true)
                    {
                        SetUserClock(GetSelfChair(), 5, TimerType.TIMER_OUTCARD);
                    }
                    else
                    {
                        SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bOthersCardCount[_bCurrentUser]), TimerType.TIMER_OUTCARD);
                    }
                }				

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏结束消息处理函数
        private void OnGameEndResp(NPacket packet)
        {
            packet.BeginRead();

            int lGameTax = packet.GetInt();

            _nEndUserScore[0] = packet.GetInt();
            _nEndUserScore[1] = packet.GetInt();
            _nEndUserScore[2] = packet.GetInt();

            _nEndUserExp[0] = packet.GetInt();
            _nEndUserExp[1] = packet.GetInt();
            _nEndUserExp[2] = packet.GetInt();

            _bEndCardCount[0] = packet.GetByte();
            _bEndCardCount[1] = packet.GetByte();
            _bEndCardCount[2] = packet.GetByte();

            packet.GetBytes(ref _bEndCardData, 54);

            //
            if (_bAutoPlay == true)
            {
                _bAutoPlay = false;
                UseTuoGuan(false);
            }
            o_back_cards_two.SetActive(false);
            o_back_cards_three.SetActive(false);
            o_back_cards_four.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bBrightStart[i] = false;
                _bUserBright[i] = false;
            }
            //
            for (ushort i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(i);
                if (ud == null)
                {
                    _TableUserName[i] = "";
                }
                else
                {
                    _TableUserName[i] = ud.NickName;
                }
            }

            Invoke("OnGameEnd", 2f);
            //OnGameEnd();
			overtime = false;
        }

        private void OnGameEnd()
        {
            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_FREE;
            if (_bStart == false) return;

            byte bCardPos = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                byte[] bLastData = new byte[20];
                Buffer.BlockCopy(_bEndCardData, bCardPos, bLastData, 0, _bEndCardCount[i]);
                bCardPos += _bEndCardCount[i];
                SetUserPass(i, false);
                if (i != GetSelfChair())
                {
                    SetOutCardData(i, bLastData, _bEndCardCount[i]);
                }
            }

            ShowResultView(true);

            //timer setting
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

            //
			if(GetSelfChair() != 255)
			{
				if (_bUserTrustee[GetSelfChair()] == true)
				{
					SetUserClock(GetSelfChair(), 5, TimerType.TIMER_START);
				}
				else
				{
					SetUserClock(GetSelfChair(), GameLogic.TIME_READY, TimerType.TIMER_START);
				}
			}
               
            ShowInfoBar();
            //
            SetLander(GameLogic.NULL_CHAIR);
            //
            o_play_buttons.SetActive(false);
            //
            o_ready_buttons.SetActive(true);
        }

        //初始场景处理函数
        private void SwitchFreeSceneView(NPacket packet)
        {

            ResetGameView();

            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_FREE;

            packet.BeginRead();
            _nBaseScore = packet.GetInt();
            _bGameType = packet.GetByte();
            _nBrightCardTime = packet.GetByte();
            _bStart = true;

            SetUserClock(GetSelfChair(), GameLogic.TIME_READY, TimerType.TIMER_START);


            o_ready_buttons.SetActive(true);
            SetBaseScore(_nBaseScore);

            _wTotalTimes = (ushort) _nBrightCardTime;

            SetTotalTimes(_wTotalTimes);

            UpdateUserView();

        }

        //叫分场景处理函数
        private void SwitchScoreSceneView(NPacket packet)
        {
            InitGameView();

            _bStart = true;

			UpdateUserView();
			
            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_SCORE;

            packet.BeginRead();
            //状态信息
            _wCurrentScore = packet.GetUShort();
            _nBaseScore = packet.GetInt();
            _bCurrentUser = (byte) packet.GetUShort();

            //叫分信息
            ushort[] wScoreInfo = new ushort[GameLogic.GAME_PLAYER_NUM];
            wScoreInfo[0] = packet.GetUShort();
            wScoreInfo[1] = packet.GetUShort();
            wScoreInfo[2] = packet.GetUShort();

            //手牌
            _bHandCardCount = 17;
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bOthersCardCount[i] = 17;
                if (i == GetSelfChair())
                {
                    packet.GetBytes(ref _bHandCardData, 20);
                }
                else
                {
                    packet.GetBytes(ref _bOthersCardData[i], 20);

                }
            }

            //托管玩家
            _bUserTrustee[0] = packet.GetBool();
            _bUserTrustee[1] = packet.GetBool();
            _bUserTrustee[2] = packet.GetBool();

            _bGameType = packet.GetByte();

            //UI
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetPlayerCardCount(i, _bOthersCardCount[i]);
                SetCallScore(i, wScoreInfo[i]);
            }

            //底分
            SetBaseScore(_nBaseScore);

            //叫分按钮
            if (_bCurrentUser == GetSelfChair())
            {
                if (_bGameType == 0)
                {
                    if (_wCurrentScore < 3)
                    {
                        o_qiang_buttons.SetActive(false);
                        o_score_buttons.SetActive(true);

                        o_btn_call_1.GetComponent<UIButton>().isEnabled = (_wCurrentScore == 0 ? true : false);
                        o_btn_call_2.GetComponent<UIButton>().isEnabled = (_wCurrentScore <= 1 ? true : false);
                        o_btn_call_3.GetComponent<UIButton>().isEnabled = (_wCurrentScore <= 2 ? true : false);
                    }
                    else
                    {
                        o_qiang_buttons.SetActive(true);
                        o_score_buttons.SetActive(false);

                    }
                }
                else
                {
                    if (_wCurrentScore < 3)
                    {
                        o_land_buttons.SetActive(true);
                        o_qiang_buttons.SetActive(false);
                    }
                    else
                    {
                        o_qiang_buttons.SetActive(true);
                        o_qiang_buttons.SetActive(false);
                    }
                }

            }
            else
            {
                o_qiang_buttons.SetActive(false);
                o_score_buttons.SetActive(false);
                o_land_buttons.SetActive(false);
            }

            //底牌
            SetBackCardData(new byte[] {0, 0, 0}, 3);
            //手牌
            SetHandCardData(_bHandCardData, _bHandCardCount);
            //时间
            SetUserClock(_bCurrentUser, GameLogic.TIME_LAND_SCORE, TimerType.TIMER_SCORE);
            //更新信息
            UpdateUserView();

            //tuoguan
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetUserTuoGuan((byte) i, _bUserTrustee[i]);
                if (i == GetSelfChair() && _bUserTrustee[i] == true)
                {
                    UseTuoGuan(true);
                }
            }

        }

        //游戏场景处理函数
        private void SwitchPlaySceneView(NPacket packet)
        {
            //
            InitGameView();
            //
            _bStart = true;
            isCanSmart = true;

			UpdateUserView();
            //
            GameEngine.Instance.MySelf.GameStatus = (byte) GameLogic.GS_WK_PLAYING;
            //
            packet.BeginRead();
            _bLander = (byte) packet.GetUShort();
            _bBombTimes = (byte) packet.GetUShort();
            _nBaseScore = packet.GetInt();
            _wCurrentScore = packet.GetUShort();
            byte bLastOutUser = (byte) packet.GetUShort();
            _bCurrentUser = (byte) packet.GetUShort();
            packet.GetBytes(ref _bBackCardData, 3);



            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (i == GetSelfChair())
                {
                    packet.GetBytes(ref _bHandCardData, 20);
                }
                else
                {
                    packet.GetBytes(ref _bOthersCardData[i], 20);
                }
            }

            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bOthersCardCount[i] = packet.GetByte();
                if (i == GetSelfChair())
                {
                    _bHandCardCount = _bOthersCardCount[i];
                }
            }

            _bTurnCardCount = packet.GetByte();
            packet.GetBytes(ref _bTurnCardData, 20);

            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bUserTrustee[i] = packet.GetBool();
            }

            _bNoneOutCard = packet.GetBool();

            _bGameType = packet.GetByte();

            //UI设置
            SetLander(_bLander);

            _wTotalTimes = (ushort) (_bBombTimes*_wCurrentScore);
            //
            SetTotalTimes(_wTotalTimes);
            //
            SetBaseScore(_nBaseScore);
            //
            if (_bTurnCardCount != 0)
            {
                SetOutCardData(bLastOutUser, _bTurnCardData, _bTurnCardCount);
            }
            //底牌不显示
            SetBackCardData(_bBackCardData, 3);
            //手牌
            SetHandCardData(_bHandCardData, _bHandCardCount);
            //其他人手牌
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetPlayerCardCount(i, _bOthersCardCount[i]);
            }
            //出牌按钮
            if (_bCurrentUser == GetSelfChair())
            {
                o_play_buttons.SetActive(true);
                o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                o_btn_pass.GetComponent<UIButton>().isEnabled = (_bTurnCardCount > 0 ? true : false);
                o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                if (_bTurnCardCount > 0)
                {
                    tagOutCardResult OutCardResult = new tagOutCardResult();
                    GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,
                                            ref OutCardResult);

                    if (OutCardResult.cbCardCount > 0)
                    {

                        ShowTipMsg(false);
                    }
                    else
                    {
                        ShowTipMsg(true);
                    }
                }
            }
            //
            SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bHandCardCount), TimerType.TIMER_OUTCARD);
            //
            UpdateUserView();
            //tuoguan
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetUserTuoGuan((byte) i, _bUserTrustee[i]);
                if (i == GetSelfChair() && _bUserTrustee[i] == true)
                {
                    UseTuoGuan(true);
                }
            }

        }

        #endregion


        #region ##################UI 事件#######################

        private void BreakGame()
        {

            _bStart = false;
            GameEngine.Instance.Quit();
        }

        private void OnBackIvk()
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
                _bStart = false;

                _bReqQuit = true;
                _nQuitDelay = System.Environment.TickCount;

                OnBtnSpeakCancelIvk();
				CancelInvoke();
				GameEngine.Instance.Quit();
            }
            catch
            {
                GameEngine.Instance.Quit();
            }
        }

        private void OnConfirmQuitOKIvk()
        {
            try
            {

                o_ready_buttons.SetActive(false);
                _bStart = false;
                UIMsgBox.Instance.Show(true, "正在退出游戏,请稍后...");


                Invoke("Quit", 3.0f);
                OnBtnSpeakCancelIvk();
            }
            catch
            {

            }
        }

        private void OnConfirmBackCancelIvk()
        {

        }

        private void OnSettingIvk()
        {
            UISetting.Instance.Show(true);
        }

        private void OnTuoGuanIvk()
        {

            if (GameEngine.Instance.MySelf.GameStatus == GameLogic.GS_WK_FREE) return;

            _bAutoPlay = !_bAutoPlay;

            UseTuoGuan(_bAutoPlay);

            if (_bCurrentUser == GetSelfChair())
			{
				byte bViewID = ChairToView(_bCurrentUser);
				UIClock clock = o_clock[bViewID].GetComponent<UIClock>();
				if(clock.Remain > 1000)
				{
					clock.SetTimer(1000);
				}

			}
            o_btn_BigTuo.SetActive(_bAutoPlay);
			if(_bAutoPlay)
			{
				PlaySound(SoundType.TUOGUAN);
			}
        }

        private void UseTuoGuan(bool bUsed)
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_TRUSTEE);
                packet.AddUShort((ushort) GetSelfChair());
                packet.AddBool(bUsed);
                Send(packet, 100);

                _bUserTrustee[GetSelfChair()] = bUsed;

                _bAutoPlay = bUsed;
                o_btn_BigTuo.SetActive(_bAutoPlay);
                _bUserTrustee[GetSelfChair()] = bUsed;

                SetUserTuoGuan(GetSelfChair(), bUsed);

                _nAutoCount = 0;
				
				o_head_robot[0].transform.localScale = Vector2.one;
	
				if(_bAutoPlay)
				{
					PlaySound(SoundType.TUOGUAN);
				}
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnChatIvk()
        {
			//UIChat.Instance.Show(true);
        }

        private void OnPlayerSelfIvk()
        {
            if (_bShowInfo[GetSelfChair()] == true)
            {
                _bShowInfo[GetSelfChair()] = false;
                ShowUserInfo(GameLogic.NULL_CHAIR, false);
            }
            else
            {
                _bShowInfo[GetSelfChair()] = true;
                ShowUserInfo(GetSelfChair(), true);
            }
        }

        private void OnPlayerPrevIvk()
        {
            byte bchair = (byte) ((GetSelfChair() + 2)%GameLogic.GAME_PLAYER_NUM);
            if (_bShowInfo[bchair] == true)
            {
                _bShowInfo[bchair] = false;
                ShowUserInfo(GameLogic.NULL_CHAIR, false);
            }
            else
            {
                _bShowInfo[bchair] = true;
                ShowUserInfo(bchair, true);
            }

        }

        private void OnPlayerNextIvk()
        {
            byte bchair = (byte) ((GetSelfChair() + 1)%GameLogic.GAME_PLAYER_NUM);
            if (_bShowInfo[bchair] == true)
            {
                _bShowInfo[bchair] = false;
                ShowUserInfo(GameLogic.NULL_CHAIR, false);
            }
            else
            {
                _bShowInfo[bchair] = true;
                ShowUserInfo(bchair, true);
            }

        }


        private void OnReadyIvk()
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

        private void OnQuitIvk()
        {
            OnConfirmBackOKIvk();
        }

        private void OnCallOneIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;
                if (_wCurrentScore > 0)
                {
                    return;
                }
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort(1);
                Send(packet, 100);

                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                o_score_buttons.SetActive(false);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnCallTwoIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;
                if (_wCurrentScore > 1)
                {
                    return;
                }
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort(2);
                Send(packet, 100);

                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                o_score_buttons.SetActive(false);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnCallThreeIvk()
        {
            try
            {

                if (_bCurrentUser != GetSelfChair()) return;
                if (_wCurrentScore > 2)
                {
                    return;
                }
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort(3);
                Send(packet, 100);

                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                if (_bGameType == 0)
                {
                    o_score_buttons.SetActive(false);
                }
                else
                {
                    o_land_buttons.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnGiveupIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort(255);
                Send(packet, 100);

                o_score_buttons.SetActive(false);
                o_land_buttons.SetActive(false);
                o_qiang_buttons.SetActive(false);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnOutCardIvk()
        {
            try
            {
                if (_bAutoPlay == true)
                {
                    _bAutoPlay = false;
                    UseTuoGuan(false);
                }
                if (_bCurrentUser != GetSelfChair()) return;
                if (GameEngine.Instance.MySelf.GameStatus != GameLogic.GS_WK_PLAYING) return;
                //判断当前牌是不是能大过上家的牌
                if (VerdictOutCard() == false)
                {
                    return;
                }
                //
                float randomCount = UnityEngine.Random.Range(0, 3.0f);

                //
                byte[] bcards = new byte[GameLogic.MAX_COUNT];
                byte count = 0;
                UICardControl ctr = o_hand_cards.GetComponent<UICardControl>();
                ctr.GetShootCard(ref bcards, ref count);
                if (count == 0) return;

                //send data
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OUT_CARD);
                packet.Addbyte(count);
                packet.AddBytes(bcards, count);
                Send(packet, 100);

                //set out cards
                SetOutCardData(GetSelfChair(), bcards, count);
                //
                SetUserPass(GetSelfChair(), false);
                //delete first
                GameLogic.RemoveCard(bcards, count, ref _bHandCardData, ref _bHandCardCount);

#region 


                //
                byte bOutType = GameLogic.GetCardType(bcards, count);
                byte bGender = GameEngine.Instance.MySelf.Gender;
                if (bOutType == GameLogic.CT_BOMB_CARD)
                {

                    PlayGameSound(GameSoundType.CARD_BOMB, bGender);
                    if (randomCount < 1.5f)
                    {
                        PlaySound(SoundType.BOMB_0);
                    }
                    else 
                    {
                        PlaySound(SoundType.BOMB_1);
                    }
                    
                    UIEffect.Instance.ShowBomb(true);
                }
                else if (bOutType == GameLogic.CT_MISSILE_CARD)
                {
                    PlayGameSound(GameSoundType.CARD_ROCKET, bGender);

					PlaySound(SoundType.MISSILE);

                    UIEffect.Instance.ShowRocket(true);
                }
				else if ((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO||bOutType == GameLogic.CT_THREE_LINE)  
				         && count >= 6&&(count<=9||count==12||count==16||count==15))
                {
                    PlayGameSound(GameSoundType.CARD_PLANE, bGender);
                    PlaySound(SoundType.PLANE);
                    UIEffect.Instance.ShowPlane(true);
				}else if((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
				         &&count>=10&&(count!=12||count!=16||count!=15))
				{
					PlayGameSound(GameSoundType.CARD_PLANE_1, bGender);
					PlaySound(SoundType.PLANE);
					UIEffect.Instance.ShowPlane(true);
				}


                else
                {
                    if (bOutType == GameLogic.CT_SINGLE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_A, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_2, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_3, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_4, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_5, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_6, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_7, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_8, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_9, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_10, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_J, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_Q, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_K, bGender);
                        }
                        if (bcards[0] == 0x4E)
                        {
                            PlayGameSound(GameSoundType.CARD_S, bGender);
                        }
                        if (bcards[0] == 0x4F)
                        {
                            PlayGameSound(GameSoundType.CARD_B, bGender);
                        }
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_AA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_22, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_33, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_44, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_55, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_66, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_77, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_88, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_99, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_11, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_JJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_QQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_KK, bGender);
                        }
                        PlaySound(SoundType.OUTCARD);

                    }
                    else if (bOutType == GameLogic.CT_THREE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundType.CARD_AAA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundType.CARD_222, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundType.CARD_333, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundType.CARD_444, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundType.CARD_555, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundType.CARD_666, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundType.CARD_777, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundType.CARD_888, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundType.CARD_999, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundType.CARD_111, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundType.CARD_JJJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundType.CARD_QQQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundType.CARD_KKK, bGender);
                        }
                        PlaySound(SoundType.OUTCARD);

                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_TWO)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_FOUR_2D, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_ONE)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_FOUR_2, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_SINGLE_LINE)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_LINE, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
						PlaySound(SoundType.SUNZI);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_THREE_1D, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_THREE_1, bGender);
						}
                        PlaySound(SoundType.OUTCARD);
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE_LINE)
                    {
						if(_bTurnOutType==bOutType)
						{
							if(randomCount<1.5f)
							{
								PlayGameSound(GameSoundType.CARD_DANI,bGender);
							}
							else
							{
								PlayGameSound(GameSoundType.CARD_YASI,bGender);
							}
							
						}
						else
						{
                        	PlayGameSound(GameSoundType.CARD_DBL_LINE, bGender);
						}
                        PlaySound(SoundType.OUTCARD_1);
						PlaySound(SoundType.SUNZI);
                    }

                }
#endregion
                //set hand cards
                SetHandCardData(_bHandCardData, _bHandCardCount);
                //
                o_play_buttons.SetActive(false);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                ShowTipMsg(false);
                //
                _bTipsCardsCount = 0;
                //

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnPassCardIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;
                if (_bTurnCardCount == 0)
                {
                    return;
                }

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PASS_CARD);
                Send(packet, 100);
                //
                SetUserPass(GetSelfChair(), true);
                //
                PlayGameSound(GameSoundType.OPTION_PASS, GameEngine.Instance.MySelf.Gender);

                //
                o_play_buttons.SetActive(false);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                //
                ShowTipMsg(false);
                //
                ResetHandCardData();
                //
                _bTipsCardsCount = 0;
                //
                o_btn_reset.GetComponent<UIButton>().isEnabled = false;
                //
                if (_bAutoPlay == true)
                {
                    _bAutoPlay = false;
                    UseTuoGuan(false);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnTipIvk()
        {
            try
            {
                ResetHandCardData();

                tagOutCardResult OutCardResult = new tagOutCardResult();
                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,
                                        ref OutCardResult);

                if (OutCardResult.cbCardCount > 0)
                {
                    //如果有能管上的牌
                    byte[] bTempHandCards = new byte[20];
                    Buffer.BlockCopy(_bHandCardData, 0, bTempHandCards, 0, _bHandCardCount);
                    byte bTempHandCardsCount = _bHandCardCount;

                    //删除提示列表中的牌
                    if (_bTipsCardsCount >= _bHandCardCount)
                    {
                        _bTipsCardsCount = 0;
                    }
                    GameLogic.RemoveCard(_bTipsCards, _bTipsCardsCount, ref bTempHandCards, ref bTempHandCardsCount);

                    tagOutCardResult TipsResult = new tagOutCardResult();
                    GameLogic.SearchOutCard(bTempHandCards, bTempHandCardsCount, _bTurnCardData, _bTurnCardCount,
                                            ref TipsResult);

                    if (TipsResult.cbCardCount <= 0)
                    {
                        _bTipsCardsCount = 0;
                        Buffer.BlockCopy(_bHandCardData, 0, bTempHandCards, 0, _bHandCardCount);
                        bTempHandCardsCount = _bHandCardCount;
                        TipsResult.cbCardCount = 0;
                        GameLogic.SearchOutCard(bTempHandCards, bTempHandCardsCount, _bTurnCardData, _bTurnCardCount,
                                                ref TipsResult);

                    }


                    //保存提示列表
                    Buffer.BlockCopy(TipsResult.cbResultCard, 0, _bTipsCards, _bTipsCardsCount, TipsResult.cbCardCount);
                    _bTipsCardsCount += TipsResult.cbCardCount;

                    //
                    o_hand_cards.GetComponent<UICardControl>()
                                .SetShootCard(TipsResult.cbResultCard, TipsResult.cbCardCount);
                    o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                    o_btn_reset.GetComponent<UIButton>().isEnabled = true;

                }
                else
                {
                    OnPassCardIvk();
                    o_btn_reset.GetComponent<UIButton>().isEnabled = false;
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnResetIvk()
        {
            try
            {
                _bTipsCardsCount = 0;

                ResetHandCardData();

                o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();


                byte[] cards = new byte[20];
                byte count = 0;
                o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref cards, ref count);

                if (count > 0)
                {
                    o_btn_reset.GetComponent<UIButton>().isEnabled = true;
                }
                else
                {
                    o_btn_reset.GetComponent<UIButton>().isEnabled = false;
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnQiangIvk()
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort((ushort) (_wCurrentScore*2));
                Send(packet, 100);

                o_qiang_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void OnNotQiangIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LAND_SCORE);
                packet.AddUShort(255);
                Send(packet, 100);
                o_qiang_buttons.SetActive(false);
                o_score_buttons.SetActive(false);
                o_land_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        #endregion


        #region ##################控件事件#######################

        /*
        //扑克控件点击事件
        private void SmartSelect()
        {
            //获取选牌
            try
            {
                byte[] cards1 = new byte[20];
                byte count1 = 0;
                o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref cards1, ref count1);

                GameLogic.SortCardList(ref cards1, count1, GameLogic.ST_ORDER);

                byte bLogicValue0 = GameLogic.GetCardLogicValue(cards1[0]);
                byte bLogicValue1 = GameLogic.GetCardLogicValue(cards1[1]);

                if (count1 == 2 && _bHandCardCount > 4 && (bLogicValue0 - bLogicValue1) == 1 && bLogicValue0 < 12 &&
                    bLogicValue1 < 12)
                {
                    byte[] linecards = new byte[10];
                    byte linecount = 0;



                    //检索当前索引
                    int npos = -1;
                    for (int i = 0; i < _bHandCardCount; i++)
                    {
                        if (_bHandCardData[i] == cards1[0])
                        {
                            npos = i;
                            break;
                        }
                    }

                    //
                    if (npos < 3) return;

                    //
                    linecards[0] = cards1[1];
                    linecards[1] = cards1[0];

                    byte bCurrValue = bLogicValue0;
                    linecount = 2;
                    for (int i = npos - 1; i >= 0; i--)
                    {
                        byte btemp = GameLogic.GetCardLogicValue(_bHandCardData[i]);
                        if (btemp < 15 && (btemp - bCurrValue) == 1)
                        {
                            linecards[linecount] = _bHandCardData[i];
                            bCurrValue = btemp;
                            linecount++;
                            if (linecount >= 5)
                            {
                                break;
                            }
                        }
                    }


                    if (linecount >= 5)
                    {
                        o_hand_cards.GetComponent<UICardControl>().SetShootCard(linecards, linecount);
                    }
                }

            }
            catch
            {
            }

        }
        */
        private void OnCardClick()
        {

            if (_bCurrentUser == GetSelfChair())
            {
               // SmartSelect();
            }
            //状态判断
            o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();


            byte[] cards = new byte[20];
            byte count = 0;
            o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref cards, ref count);

            if (count > 0)
            {
                o_btn_reset.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_btn_reset.GetComponent<UIButton>().isEnabled = false;
            }
        }

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
                    case TimerType.TIMER_START:
                        {
                            OnConfirmBackOKIvk();
                            break;
                        }
                    case TimerType.TIMER_SCORE:
                        {
                            if (_bCurrentUser != GetSelfChair()) return;
                            OnNotQiangIvk();

                            CheckAutoTimes();
                            break;
                        }
                    case TimerType.TIMER_OUTCARD:
                        {
                            if (_bCurrentUser != GetSelfChair()) return;
							//超时
							overtime = true;

                            if (_bTurnCardCount == 0)
                            {
                                tagOutCardResult res = new tagOutCardResult();
                                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,
                                                        ref res);
                                if (res.cbCardCount > 0)
                                {
                                    //
                                    SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                    //first
                                    SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                    //delete first
                                    GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData,
                                                         ref _bHandCardCount);

                                    SetHandCardData(_bHandCardData, _bHandCardCount);

                                    //Send
                                    NPacket packet = NPacketPool.GetEnablePacket();
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OUT_CARD);
                                    packet.Addbyte(res.cbCardCount);
                                    packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                    Send(packet, 100);
                                    //
									if(res.cbCardCount>=5)
									{
										PlaySound(SoundType.OUTCARD_1);
									}
									else
									{
										PlaySound(SoundType.OUTCARD);
									}
                                    
                                    //
                                    o_play_buttons.SetActive(false);
                                    o_double_buttons.SetActive(false);
                                    o_land_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                                }
                                else
                                {
                                    SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                    //first
                                    byte[] bData = new byte[1];
                                    bData[0] = _bHandCardData[_bHandCardCount - 1];
                                    SetOutCardData(GetSelfChair(), bData, 1);

                                    //delete first
                                    _bHandCardCount -= 1;
                                    SetHandCardData(_bHandCardData, _bHandCardCount);

                                    //Send
                                    NPacket packet = NPacketPool.GetEnablePacket();
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OUT_CARD);
                                    packet.Addbyte(1);
                                    packet.Addbyte(bData[0]);
                                    Send(packet, 100);
                                    //
                                    PlaySound(SoundType.OUTCARD);
                                    //
                                    o_play_buttons.SetActive(false);
                                    o_double_buttons.SetActive(false);
                                    o_land_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                                }
                            }
							
                            else
                            {
                                //next
                                tagOutCardResult res = new tagOutCardResult();
                                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount,
                                                        ref res);
                                if (res.cbCardCount > 0)
                                {
                                    bool bres = GameLogic.CompareCard(_bTurnCardData, res.cbResultCard, _bTurnCardCount,
                                                                      res.cbCardCount);
                                    if (bres == false)
                                    {
                                        if (_bCurrentUser != GetSelfChair()) return;
                                        if (_bTurnCardCount == 0)
                                        {
                                            return;
                                        }
                                        NPacket packet1 = NPacketPool.GetEnablePacket();
                                        packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PASS_CARD);
                                        Send(packet1, 100);
                                        //
                                        SetUserPass(GetSelfChair(), true);
                                        //
                                        PlayGameSound(GameSoundType.OPTION_PASS, GameEngine.Instance.MySelf.Gender);
                                        //
                                        o_play_buttons.SetActive(false);
                                        o_double_buttons.SetActive(false);
                                        o_land_buttons.SetActive(false);
                                        //
                                        SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                                        //
                                        ShowTipMsg(false);
                                        //
                                        ResetHandCardData();
                                        //
                                        o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                                        return;

                                    }
                                    //
                                    SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                    //first
                                    SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                    //delete first
                                    GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData,
                                                         ref _bHandCardCount);

                                    SetHandCardData(_bHandCardData, _bHandCardCount);

                                    //Send
                                    NPacket packet = NPacketPool.GetEnablePacket();
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OUT_CARD);
                                    packet.Addbyte(res.cbCardCount);
                                    packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                    Send(packet, 100);
                                    //
									if(res.cbCardCount>=5)
									{
										PlaySound(SoundType.OUTCARD_1);
									}
									else
									{
										PlaySound(SoundType.OUTCARD);
									}
                                    //
                                    o_play_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                                }
                                else
                                {
                                    try
                                    {
                                        if (_bCurrentUser != GetSelfChair()) return;
                                        if (_bTurnCardCount == 0)
                                        {
                                            return;
                                        }

                                        NPacket packet = NPacketPool.GetEnablePacket();
                                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PASS_CARD);
                                        Send(packet, 100);
                                        //
                                        SetUserPass(GetSelfChair(), true);
                                        //
                                        PlayGameSound(GameSoundType.OPTION_PASS, GameEngine.Instance.MySelf.Gender);
                                        //
                                        o_play_buttons.SetActive(false);
                                        o_double_buttons.SetActive(false);
                                        o_land_buttons.SetActive(false);
                                        //
                                        SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                                        //
                                        ShowTipMsg(false);
                                        //
                                        ResetHandCardData();
                                        //
                                        o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                                    }
                                    catch (Exception ex)
                                    {
                                        UIMsgBox.Instance.Show(true, ex.Message);
                                    }

                                }
                            }
                            //
                            CheckAutoTimes();
                            break;
                        }
                    case TimerType.TIMER_MOSTCARD:
                        {
                            if (_bMostUser == GameLogic.NULL_CHAIR) return;

                            byte bCurrUser = _bMostUser;
                            _bMostUser = GameLogic.NULL_CHAIR;
                            //
                            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
                            //
                            SetUserPass(GameLogic.NULL_CHAIR, false);
                            //
                            o_play_buttons.SetActive(false);
                            //
                            SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                            //
                            if (bCurrUser == GetSelfChair())
                            {
                                if (_bUserTrustee[GetSelfChair()] == true)
                                {
                                    _bTurnCardCount = 0;
                                    tagOutCardResult res = new tagOutCardResult();
                                    GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData,
                                                            _bTurnCardCount, ref res);
                                    if (res.cbCardCount > 0)
                                    {
                                        //
                                        SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                        //first
                                        SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                        //delete first
                                        GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData,
                                                             ref _bHandCardCount);
                                        //
                                        SetHandCardData(_bHandCardData, _bHandCardCount);

                                        //Send
                                        NPacket packet = NPacketPool.GetEnablePacket();
                                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                        packet.Addbyte(res.cbCardCount);
                                        packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                        Send(packet, 100);
                                        //
										if(res.cbCardCount<5)
										{
											PlaySound(SoundType.OUTCARD);
										}
										else
										{
											PlaySound(SoundType.OUTCARD_1);
										}
                                        
                                        //
                                        o_play_buttons.SetActive(false);

                                    }
                                    else
                                    {
                                        //
                                        SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                        //first
                                        byte[] bData = new byte[1];
                                        bData[0] = _bHandCardData[_bHandCardCount - 1];
                                        SetOutCardData(GetSelfChair(), bData, 1);

                                        //delete first
                                        _bHandCardCount -= 1;
                                        SetHandCardData(_bHandCardData, _bHandCardCount);

                                        //Send
                                        NPacket packet = NPacketPool.GetEnablePacket();
                                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                        packet.Addbyte(1);
                                        packet.Addbyte(bData[0]);
                                        Send(packet, 100);
                                        //
                                        PlaySound(SoundType.OUTCARD);
                                        //
                                        o_play_buttons.SetActive(false);

                                    }
                                    return;
                                }
                                else
                                {
                                    o_play_buttons.SetActive(true);
                                    o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                                    o_btn_pass.GetComponent<UIButton>().isEnabled = false;
                                }
                            }

                            Invoke("SetTimerAfterMost", 1.0f);

                            break;
                        }
                    case TimerType.TIMER_DOUBLE:
                        {
                            OnBtnNotDoubleIvk();
                            break;
                        }
                    case TimerType.TIMER_CLOSE:
                        {
                            GameEngine.Instance.Quit();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetTimerAfterMost()
        {
            SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bHandCardCount), TimerType.TIMER_OUTCARD);
        }

        //扑克控件选牌事件
        private void OnMoveSelect()
        {
            if (_bCurrentUser == GetSelfChair())
            {
               // SmartSelect();
            }

            o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();

            byte[] cards = new byte[20];
            byte count = 0;
            o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref cards, ref count);

            if (count > 0)
            {
                o_btn_reset.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_btn_reset.GetComponent<UIButton>().isEnabled = false;
            }
        }

        //扑克控件向上划牌事件
        private void OnMoveUp()
        {
            OnOutCardIvk();
        }

        //扑克控件向下划牌事件
        private void OnMoveDown()
        {

        }

        #endregion


        #region ##################UI 控制#######################

        private void SetHandCardData(byte[] cards, byte count)
        {
            try
            {
                UICardControl ctr = o_hand_cards.GetComponent<UICardControl>();
                ctr.SetCardData(cards, count);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void ArrayHandCardData(byte[] cards, byte count)
        {
            try
            {
                UICardControl ctr = o_hand_cards.GetComponent<UICardControl>();
                ctr.ArrayHandCards(cards, count);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void ResetHandCardData()
        {
            try
            {
                UICardControl ctr = o_hand_cards.GetComponent<UICardControl>();
                ctr.ResetAllShoot();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetOutCardData(byte chair, byte[] cards, byte count)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        UICardControl ctr = o_out_cards[i].GetComponent<UICardControl>();
                        ctr.SetCardData(null, 0);
                    }
                }
                else
                {
                    GameLogic.SortCardList(ref cards, count, GameLogic.ST_ORDER);

                    byte bViewId = ChairToView(chair);
                    UICardControl ctr = o_out_cards[bViewId].GetComponent<UICardControl>();
                    ctr.ClearCards();
                    ctr.AppendCard(cards, count);

                    //ctr.SetCardData(cards,count);

                }
                //

                if (ChairToView(chair) == ChairToView(_bLander))
                {
                    GameObject ctr = o_out_cards[ChairToView(chair)].gameObject;
                    int num = count - 1; 
                    GameObject card = ctr.transform.FindChild("card_" + num).gameObject;
                    GameObject myLandType = Instantiate(landType);
                    myLandType.transform.parent = card.transform;
                    myLandType.SetActive(true);
                    myLandType.transform.localPosition = new Vector3(27, 50, 0);
                    myLandType.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                }
            }
            catch (Exception ex)
            {
                //UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void SetBackCardData(byte[] cards, byte count)
        {
            try
            {
                o_back_cards.GetComponent<UICardControl>().SetCardData(cards, count);

                if (count > 0)
                {
                    o_current_time.SetActive(false);
                }
                else
                {
                    o_current_time.SetActive(true);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void CheckAutoTimes()
        {
            if (_bCurrentUser == GetSelfChair())
            {
                _nAutoCount++;
                if (_nAutoCount >= 2)
                {
                    _bAutoPlay = true;
                    UseTuoGuan(_bAutoPlay);
                }
            }
        }

        private void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        o_clock[i].GetComponent<UIClock>().SetTimer(0);
                        o_clock[i].SetActive(false);

                    }
                }
                else
                {
                    _bTimerType = timertype;
                    byte viewId = ChairToView(chair);
                    o_clock[viewId].GetComponent<UIClock>().SetTimer(time*1000);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetUserPass(byte chair, bool bshow)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        o_pass[i].SetActive(false);
                    }
                }
                else
                {
                    byte viewId = ChairToView(chair);
                    //byte n = (byte)(viewId % 2);
                    o_pass[viewId].SetActive(bshow);
                    o_pass[viewId].GetComponent<UISprite>().spriteName = "state_pass";

                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetCallScore(byte chair, ushort bScore)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        o_score[i].SetActive(false);
                    }
                }
                else
                {
                    byte viewId = ChairToView(chair);
                    byte n = (byte) (viewId%2);
                    o_score[viewId].SetActive(true);

                    if (_bGameType == 0)
                    {
                        if (bScore == 0)
                        {
                            o_score[viewId].SetActive(false);
                        }
                        else if (bScore == 1)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_1fen";
                        }
                        else if (bScore == 2)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_2fen";
                        }
                        else if (bScore == 3)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_3fen";
                        }
                        else if (bScore == 255)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_not_land";
                        }
                        else
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_qiang";
                        }
                    }
                    else
                    {
                        if (bScore == 0)
                        {
                            o_score[viewId].SetActive(false);
                        }
                        else if (bScore == 3)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_land";
                        }
                        else if (bScore == 255)
                        {
                            if (_wCurrentScore >= 3)
                            {
                                o_score[viewId].GetComponent<UISprite>().spriteName = "state_not_qiang";
                            }
                            else
                            {
                                o_score[viewId].GetComponent<UISprite>().spriteName = "state_not_land";
                            }
                        }
                        else
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "state_qiang";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void SetUserDouble(byte chair, bool bdouble)
        {
            if (_bLander == chair)
            {
                byte viewId = ChairToView(chair);
                o_score[viewId].SetActive(false);
                return;
            }
            
            if (chair == GameLogic.NULL_CHAIR)
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_score[i].SetActive(false);
                }
            }
            else
            {
                byte viewId = ChairToView(chair);
                o_score[viewId].SetActive(true);
                if (bdouble == true)
                {
                    o_score[viewId].GetComponent<UISprite>().spriteName = "state_double";
                }
                else
                {
                    o_score[viewId].GetComponent<UISprite>().spriteName = "state_not_double";
                }
                
            }

        }

        private void SetUserReady(byte chair, bool bshow)
        {
            try
            {
                if (chair == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        o_ready[i].SetActive(false);
                    }
                }
                else
                {
                    byte viewId = ChairToView(chair);
                    o_ready[viewId].SetActive(bshow);
					//o_head_face[viewId].SetActive(bshow);
					
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void SetLander(byte bChair)
        {
            try
            {

                _bLander = bChair;
                if (_bLander == GameLogic.NULL_CHAIR)
                {
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        o_head_flag[i].GetComponent<UISprite>().spriteName = "blank";
                    }
                }
                else
                {
                    byte viewId = ChairToView(_bLander);
                    for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                    {
                        if (i == viewId)
                        {
                            o_head_flag[i].GetComponent<UISprite>().spriteName = "play_land_flag";
                        }
                        else
                        {
                            o_head_flag[i].GetComponent<UISprite>().spriteName = "play_farmer_flag";
                        }
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

                for (uint i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    byte bViewId = ChairToView((byte) i);
                    PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {
                        o_head_player[bViewId].SetActive(true);
                        //nick 
                        if (userdata.VipLevel > 0)
                        {
                            o_head_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
                        }
                        else
                        {
                            o_head_nick[bViewId].GetComponent<UILabel>().color = new Color(0.35f, 0.8f, 0.8f);
                        }
                        o_head_nick[bViewId].GetComponent<UILabel>().text = userdata.NickName;
                        //gold
                        o_head_gold[bViewId].GetComponent<UILabel>().text = userdata.Money.ToString();
                        //face

						o_head_face[bViewId].GetComponent<UIFace>().ShowFace((int) userdata.HeadID, (int) userdata.VipLevel);

                        if (userdata.UserState == (byte) UserState.US_READY)
                        {
                            SetUserReady((byte) i, true);
                        }
                        else
                        {
                            SetUserReady((byte) i, false);
                        }
                    }
                    else
                    {
                        //nick
                        o_head_nick[bViewId].GetComponent<UILabel>().text = "";
                        //nick
                        o_head_gold[bViewId].GetComponent<UILabel>().text = "";
                        //face
                        o_head_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
                        //p

                        SetUserReady((byte) i, false);
                        o_head_player[bViewId].SetActive(false);
                    }


                }

                ShowInfoBar();

            }
            catch (Exception ex)
            {
               // UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        private void ShowInfoBar()
        {
            if (GameEngine.Instance.MySelf != null)
            {

                GameObject.Find("scene_game/dlg_bottom_bar/lbl_money").GetComponent<UILabel>().text =
                GameEngine.Instance.MySelf.Money.ToString();
            }
            else
            {
                GameObject.Find("scene_game/dlg_bottom_bar/lbl_money").GetComponent<UILabel>().text = "";

            }
        }

        private void SetBaseScore(int nscore)
        {
            GameObject.Find("scene_game/dlg_bottom_bar/lbl_base_score").GetComponent<UILabel>().text = nscore.ToString();


        }

        private void DispatchCard(int nSendCount)
        {
            if (_bGameType == 1 )
            {
                if (_unBright == false)
                {
                    if (nSendCount >= 1 && nSendCount <= 7)
                    {
                        _nTempBrightCardTime = 6;
                        if (_bBrightStart[GetSelfChair()] == false && _bUserBright[GetSelfChair()] == false)
                        {
                            o_bright_buttons.SetActive(true);
                            o_bright_rate.GetComponent<UISprite>().spriteName = "6bei";
                        }
                        else
                        {
                            o_bright_buttons.SetActive(false);
                        }
                    }
                    else if (nSendCount >= 8 && nSendCount <= 13)
                    {
                        _nTempBrightCardTime = 4;
                        if (_bBrightStart[GetSelfChair()] == false && _bUserBright[GetSelfChair()] == false)
                        {
                            o_bright_buttons.SetActive(true);
                            o_bright_rate.GetComponent<UISprite>().spriteName = "4bei";
                        }
                        else
                        {
                            o_bright_buttons.SetActive(false);
                        }
                    }
                    else if (nSendCount >= 14 && nSendCount <= 17)
                    {
                        _nTempBrightCardTime = 2;
                        if (_bBrightStart[GetSelfChair()] == false && _bUserBright[GetSelfChair()] == false)
                        {
                            o_bright_buttons.SetActive(true);
                            o_bright_rate.GetComponent<UISprite>().spriteName = "2bei";
                        }
                        else
                        {
                            o_bright_buttons.SetActive(false);
                        }
                    }
                }
                else
                {
                    o_bright_buttons.SetActive(false);
                }
               
            }
            else
            {
                o_bright_buttons.SetActive(false);
            }


            AppendHandCard(new byte[1] {_bHandCardData[nSendCount]}, 1);

            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (i != GetSelfChair())
                {
                    AppendOtherHandCard((byte) i, new byte[1] {_bOthersCardData[i][nSendCount]}, 1);
                }
            }

            PlaySound(SoundType.SENDCARD);

        }

        private void AppendHandCard(byte[] cards, byte cardcount)
        {

            o_hand_cards.GetComponent<UICardControl>().AppendHandCard(cards, cardcount);

        }

        private void AppendOtherHandCard(byte bchair, byte[] cards, byte cardcount)
        {

            byte bviewid = ChairToView(bchair);
            if (o_others[bviewid].active == false)
            {
                o_others[bviewid].SetActive(true);
            }
            o_others[bviewid].GetComponentInChildren<UICardControl>().AppendCard(cards, cardcount);
            o_others[bviewid].GetComponentInChildren<UILabel>().text = _bOthersCardCount[bchair].ToString();
        }

        private void DispatchCardFinish()
        {
            if (_unBright == true)
            {
                //OnCallOneIvk();
                OnCallThreeIvk();
            }
          
            _bSendCard = false;

            _nSendCardCount = 0;

            o_bright_buttons.SetActive(false);
            //sorted
            GameLogic.SortCardList(ref _bHandCardData, _bHandCardCount, GameLogic.ST_ORDER);
            //排序对家牌
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (i != GetSelfChair())
                {
                    GameLogic.SortCardList(ref _bOthersCardData[i], _bOthersCardCount[i], GameLogic.ST_ORDER);
                }
            }
            //back cards
            o_current_time.SetActive(false);
            o_back_cards.GetComponent<UICardControl>().ClearCards();
            o_back_cards.GetComponent<UICardControl>().AppendCard(new byte[] {0, 0, 0}, 3);
            //hand cards
            ArrayHandCardData(_bHandCardData, _bHandCardCount);

            //
            Array.Clear(_nEndUserScore, 0, _nEndUserScore.Length);
            Array.Clear(_bEndCardCount, 0, 3);
            Array.Clear(_bEndCardData, 0, 54);
            //_bEndTax = 0;

            //

            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bOthersCardCount[i] = 17;
                SetPlayerCardCount(i, _bOthersCardCount[i]);
            }
            //
            if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
            {
                SetUserClock(_bCurrentUser, 1, TimerType.TIMER_SCORE);
            }
            else
            {
                SetUserClock(_bCurrentUser, GameLogic.TIME_LAND_SCORE, TimerType.TIMER_SCORE);
            }

            //
            if (_bCurrentUser == GetSelfChair())
            {
                if (_bGameType == 0)
                {
                    o_score_buttons.SetActive(true);
                    o_land_buttons.SetActive(false);
                }
                else
                {
                    if (_unBright == true)
                    {
                        o_land_buttons.SetActive(false);
                    }
                    else
                    {
                        o_land_buttons.SetActive(true);   
                    }
                    o_score_buttons.SetActive(false);

                }
            }

            int tempVal = UnityEngine.Random.Range(0,16);
            byte landChair = ChairToView(_bCurrentUser);
            _firstLander = landChair;
            byte[] cardData = new byte[1];
            cardData[0] = _bOthersCardData[landChair][tempVal];
            UICardControl ctr = o_firstLand[landChair].transform.FindChild("card").GetComponent<UICardControl>();
            ctr.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
            o_firstLand[landChair].SetActive(true);

            PlaySound(SoundType.START);

        }

        private void DispatchCardStart()
        {
            _bSendCard = true;
            _nSendCardCount = 0;
            _nSendCardTick = Environment.TickCount;

            SetHandCardData(null, 0);

            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (i != GetSelfChair())
                {
                    SetPlayerCardCount(i, 0);
                }
            }

            o_bright_buttons.SetActive(false);


            //播放明牌开始语音
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (_bBrightStart[i] == true)
                {
                    PlayGameSound(GameSoundType.OPTION_MING, GameEngine.Instance.GetTableUserItem((ushort) i).Gender);
                }
            }
        }

        private void DoubleStart()
        {
            //加倍判断
            byte bSelf = GetSelfChair();
            byte bNext = (byte) ((GetSelfChair() + 1)%GameLogic.GAME_PLAYER_NUM);
            byte bPrev = (byte) ((GetSelfChair() + 2)%GameLogic.GAME_PLAYER_NUM);

            //如果自己是地主
            PlayerInfo udSelf = GameEngine.Instance.GetTableUserItem(bSelf);
            PlayerInfo udNext = GameEngine.Instance.GetTableUserItem(bNext);
            PlayerInfo udPrev = GameEngine.Instance.GetTableUserItem(bPrev);

            if (udSelf == null || udNext == null || udPrev == null)
            {
                OnBtnNotDoubleIvk();
                return;
            }
           
           // if (_bCurrentUser == GetSelfChair())
           // {
                if (_bLander == GetSelfChair())
                {
                    o_double_buttons.SetActive(false);
                    SetUserClock(GetSelfChair(), GameLogic.TIME_DOUBLE, TimerType.TIMER_DOUBLE);
                    landWait.SetActive(true);
                }
                else
                {
                    if (udSelf.Money >= (Int64)_nDoubleLimit 
                        && udNext.Money >= (Int64)_nDoubleLimit
                        && udPrev.Money >= (Int64)_nDoubleLimit)
                        
                    {
                        o_double_buttons.SetActive(true);
                        SetUserClock(GetSelfChair(), GameLogic.TIME_DOUBLE, TimerType.TIMER_DOUBLE);
                    }
                    else
                    {
                        OnBtnNotDoubleIvk();
                    }
                }
           // }
           // else
           // {
           //     PlayerInfo udLand = GameEngine.Instance.GetTableUserItem(_bCurrentUser);
           //     if (udSelf.Money >= (Int64) _nDoubleLimit && udLand.Money >= (Int64) _nDoubleLimit)
           //     {
           //         o_double_buttons.SetActive(true);
           //         SetUserClock(GetSelfChair(), GameLogic.TIME_DOUBLE, TimerType.TIMER_DOUBLE);
           //     }
           //     else
           //     {
           //         OnBtnNotDoubleIvk();
           //     }
           // }

            SetCallScore(GameLogic.NULL_CHAIR, 0);
        }

        private void DoubleFinish()
        {
            SetHandCardData(_bHandCardData, _bHandCardCount);
            o_double_buttons.SetActive(false);
            SetUserDouble(GameLogic.NULL_CHAIR, false);

            //
            if (_bCurrentUser == GetSelfChair())
            {
                o_play_buttons.SetActive(true);
                landWait.SetActive(false);
                o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                o_btn_pass.GetComponent<UIButton>().isEnabled = false;

                o_btn_reset.GetComponent<UIButton>().isEnabled = false;
            }
            //
            if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
            {
                SetUserClock(_bCurrentUser, 1, TimerType.TIMER_OUTCARD);
            }
            else
            {
                SetUserClock(_bCurrentUser, GameLogic.GetOutCardTime(_bHandCardCount), TimerType.TIMER_OUTCARD);
            }
            PlaySound(SoundType.START);
            outCardLock = true;
            isCanSmart = true;
        }

        private void OnBtnNotDoubleIvk()
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_DOUBLE);
                packet.AddUShort(GetSelfChair());
                packet.Addbyte(0);
                Send(packet, 100);
                o_double_buttons.SetActive(false);

                PlayGameSound(GameSoundType.OPTION_NOTJIABEI, GameEngine.Instance.MySelf.Gender);

                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnDoubleIvk()
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_DOUBLE);
                packet.AddUShort(GetSelfChair());
                packet.Addbyte(1);
                Send(packet, 100);

                o_head_double[0].SetActive(true);

                o_double_buttons.SetActive(false);
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnBrightStartIvk()
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_BRIGHT);
                packet.AddUShort(GetSelfChair());
                packet.Addbyte(0);
                Send(packet, 100);

                _bUserBright[GetSelfChair()] = true;

                _bBrightStart[GetSelfChair()] = true;

                o_bright_buttons.SetActive(false);

                o_ready_buttons.SetActive(false);


                Invoke("OnReadyIvk", 1.0f);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        private void OnBtnBrightCardIvk()
        {
            try
            {
                o_bright_buttons.SetActive(false);
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_BRIGHT);
                packet.AddUShort(GetSelfChair());
                if (_nTempBrightCardTime == 6)
                {
                    packet.Addbyte(1);
                }
                else if (_nTempBrightCardTime == 4)
                {
                    packet.Addbyte(2);
                }
                else if (_nTempBrightCardTime == 2)
                {
                    packet.Addbyte(3);
                }

                Send(packet, 100);

                _bUserBright[GetSelfChair()] = true;


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
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
            
            return;
        }

        private void ShowUserInfo(byte bchair, bool bshow)
        {
            if (bchair == GameLogic.NULL_CHAIR)
            {
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_info[i].SetActive(false);
                }
            }
            else
            {
                byte bViewID = ChairToView(bchair);
                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(bchair);
                if (ud != null)
                {
                    o_info[bViewID].SetActive(true);
                    o_info_nick[bViewID].GetComponent<UILabel>().text = ud.NickName;
                    o_info_lvl[bViewID].GetComponent<UILabel>().text =
                        GameConfig.Instance.GetExpLevel((int) ud.Exp).ToString();
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
            byte wViewChairID = (byte) ((ChairID + GameLogic.GAME_PLAYER_NUM - GameEngine.Instance.MySelf.DeskStation));
            return (byte) (wViewChairID%GameLogic.GAME_PLAYER_NUM);
        }

        private byte GetSelfChair()
        {
			//修正MySelf 为NULL 时的获取错误
			if (GameEngine.Instance.MySelf != null) {
				return (byte)GameEngine.Instance.MySelf.DeskStation;
			} 
            else 
            {
			}
			return 0xFF; //GameEngine.Instance.MyUser.Self.ChairID;
        }

        private bool VerdictOutCard()
        {
            byte[] bCardData = new byte[GameLogic.MAX_COUNT];
            byte bShootCount = 0;

            o_hand_cards.GetComponent<UICardControl>().GetShootCard(ref bCardData, ref bShootCount);

            if (bShootCount > 0)
            {
                GameLogic.SortCardList(ref bCardData, bShootCount, GameLogic.ST_ORDER);

                byte bCardType = GameLogic.GetCardType(bCardData, bShootCount);
                if (bCardType == GameLogic.CT_ERROR) return false;
                if (_bTurnCardCount == 0) return true;

                return GameLogic.CompareCard(_bTurnCardData, bCardData, _bTurnCardCount, bShootCount);
            }
            return false;
        }

        private void SetPlayerCardCount(byte bchair, byte bcardcount)
        {
            if (bchair == GameLogic.NULL_CHAIR)
            {
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_others[i].SetActive(false);
                }
            }
            else
            {
                byte bViewID = ChairToView(bchair);
                byte[] bCards = _bOthersCardData[bchair];
                o_others[bViewID].SetActive(true);
                o_others[bViewID].GetComponentInChildren<UICardControl>().SetCardData(bCards, bcardcount);
                o_others[bViewID].GetComponentInChildren<UILabel>().text = bcardcount.ToString();
            }
        }

        private void SetTotalTimes(ushort btime)
        {
            GameObject.Find("scene_game/dlg_bottom_bar/lbl_rate").GetComponent<UILabel>().text = btime.ToString();
        }

        private void ShowResultView(bool bshow)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            o_result.SetActive(bshow);

            if (bshow)
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    string strUName = _TableUserName[i];
                    if (strUName != "")
                    {
                        byte bViewId = ChairToView(i);
                        
                        GameObject.Find("scene_game/dlg_result/lbl_score_" + bViewId.ToString())
                                  .GetComponent<UILabel>()
                                  .text = _nEndUserScore[i].ToString();
                        if (_nEndUserScore[GameEngine.Instance.MySelf.DeskStation]>0)
                        {
                            PlaySound(SoundType.END_WIN);
                        }
                        else
                        {
                            PlaySound(SoundType.END_LOSE);
                        }
                        if (_nEndUserScore[i] > 0)
                        {
                            GameObject.Find("scene_game/dlg_result/sp_win_" + bViewId.ToString())
                                      .GetComponent<UISprite>()
                                      .spriteName = "win";
                        }
                        else
                        {
                            GameObject.Find("scene_game/dlg_result/sp_win_" + bViewId.ToString())
                                      .GetComponent<UISprite>()
                                      .spriteName = "lose";
                        }

                        GameObject.Find("scene_game/dlg_result/lbl_user_" + bViewId.ToString())
                                  .GetComponent<UILabel>()
                                  .text = strUName;
                    }
                }
                GameObject.Find("scene_game/dlg_result/lbl_rocket").GetComponent<UILabel>().text =
                    _bRocketTimes.ToString();
                GameObject.Find("scene_game/dlg_result/lbl_bomb").GetComponent<UILabel>().text = _bBombTimes.ToString();

                //
                byte bUser1 = (byte) ((_bLander + 1)%GameLogic.GAME_PLAYER_NUM);
                byte bUser2 = (byte) ((_bLander + 2)%GameLogic.GAME_PLAYER_NUM);

                if (_bOthersCardCount[bUser1] == 17 && _bOthersCardCount[bUser2] == 17)
                {
					PlaySound(SoundType.CHUNTIAN);
                    _wTotalTimes = (ushort) (_wTotalTimes*2);
                    SetTotalTimes(_wTotalTimes);
                    GameObject.Find("scene_game/dlg_result/lbl_spring").GetComponent<UILabel>().text = "1";
                }
                else
                {
                    GameObject.Find("scene_game/dlg_result/lbl_spring").GetComponent<UILabel>().text = "0";
                }
				//	反春数值显示和翻倍操作
				if (LandOutCardCount <= 1)
				{
					PlaySound(SoundType.FANCHUN);
					_wTotalTimes = (ushort) (_wTotalTimes*2);
					SetTotalTimes(_wTotalTimes);
					GameObject.Find("scene_game/dlg_result/lbl_spring_fan").GetComponent<UILabel>().text = "1";
				}
				else
				{
					GameObject.Find("scene_game/dlg_result/lbl_spring_fan").GetComponent<UILabel>().text = "0";
				}

                GameObject.Find("scene_game/dlg_result/lbl_total").GetComponent<UILabel>().text =
                    _wTotalTimes.ToString();
                

                //根据需求将时间延长至30秒，同时开启准备的计时器
                Invoke("CloseResultView",3.0f);
            }

        }

        private void CloseResultView()
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            //
            SetPlayerCardCount(GameLogic.NULL_CHAIR, 0);
            //
            SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
            //
            SetBackCardData(null, 0);
            //
            o_result.SetActive(false);
            o_hand_cards.SetActive(false);
        }

        //
        private void ShowTipMsg(bool bshow)
        {
            o_tip_bar.SetActive(bshow);
        }

        private void PlaySound(SoundType sound)
        {
            {
                float fvol = NGUITools.soundVolume;

                NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
            }
        }

        private void PlayGameSound(GameSoundType sound, byte bGender)
        {
            {
                float fvol = NGUITools.soundVolume;
                float i = UnityEngine.Random.Range(0, 2.0f);
                if (bGender == (byte)UserGender.Woman)
                {

                    if (i < 1)
                    {
                        NGUITools.PlaySound(_WomanSound[(int)sound], fvol, 1);
                    }
                    else
                    {
                        NGUITools.PlaySound(_WomanSound1[(int)sound], fvol, 1);
                    }

                }
                else
                {
                    if (i < 1)
                    {
                        NGUITools.PlaySound(_ManSound[(int)sound], fvol, 1);
                    }
                    else
                    {
                        NGUITools.PlaySound(_ManSound1[(int)sound], fvol, 1);
                    }

                }
            }
        }

        private void ShowUserSpeak(uint uid)
        {

            /*byte bchairID = (byte) GameEngine.Instance.UserIdToChairId(uid);
            byte bViewID = ChairToView(bchairID);
            if (bchairID != GetSelfChair())
            {
                o_user_speak[bViewID].GetComponent<UISpeak>().Play("scene_game", "OnSpeakPlay", uid);
            }*/

        }

        private void OnRecordSpeakFinish(string strSpeak)
        {
            //本地预播放
            /*byte bViewID = ChairToView(GetSelfChair());
            o_user_speak[bViewID].GetComponent<UISpeak>()
                                 .PlayLocal("scene_game", "OnSpeakPlay", GameEngine.Instance.MySelf.ID);

            //上传网络
            string strFile = Application.persistentDataPath + "/" + strSpeak;
            StartCoroutine(UpLoadSpeak(strFile));*/

        }

        private void OnSpeakPlay(string str)
        {
            /*string[] strs = str.Split("`".ToCharArray());

            int nTime = Convert.ToInt32(strs[0]);
            uint Uid = Convert.ToUInt32(strs[1]);

            byte bViewID = ChairToView((byte) GameEngine.Instance.UserIdToChairId(Uid));

            o_user_speak[bViewID].GetComponent<UISpeak>().SetTimer(nTime);
*/
        }

        private IEnumerator UpLoadSpeak(string strSpeak)
        {
            yield break;
            /*
             string normalUrl  =   "/extrafunc.aspx";

             FileStream fileStream = new FileStream(strSpeak, FileMode.Open, FileAccess.Read, FileShare.Read);
             byte[] bytes = new byte[fileStream.Length];
             fileStream.Read(bytes, 0, bytes.Length);
             fileStream.Close();
        
             WWWForm form = new WWWForm();
             form.AddField("option", "uploadspeak");
             form.AddField("userid", GameEngine.Instance.MyUser.UserID.ToString());
             form.AddField("userpass", GameEngine.Instance.MyUser.Password);
             form.AddField("props", "SMALL");
             form.AddBinaryData("speak",bytes);
             WWW www = new WWW(normalUrl.ToString(), form);
             yield return www;

             if (www.text=="-1" || www.error != null)
             {
                UIMsgBox.Instance.Show(true,"您的网络环境太不给力,系统为您屏蔽语音消息");
                PlayerPrefs.SetString("game_speak_switch","off");
             }
             else
             {
                GameEngine.Instance.SendChatMessage(GameLogic.NULL_CHAIR,"",255);
                GameEngine.Instance.MyUser.SmallSpeakerCount--;
                if(GameEngine.Instance.MyUser.SmallSpeakerCount<0)
                {
                    GameEngine.Instance.MyUser.SmallSpeakerCount=0;
                }

                o_btn_speak_count.GetComponent<UILabel>().text = "x"+GameEngine.Instance.MyUser.SmallSpeakerCount.ToString();
			
                //本地预播放
                byte bViewID = ChairToView(GetSelfChair());
                o_user_speak[bViewID].GetComponent<UISpeak>().PlayLocal("scene_game","OnSpeakPlay",GameEngine.Instance.MyUser.Userid);

             }*/
        }

        private void ShowUserChat(byte bChair, string strMsg)
        {
            if (bChair == GameLogic.NULL_CHAIR)
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_chat[i].SetActive(false);
                }
            }
            else
            {
                _nInfoTickCount = Environment.TickCount;
                byte bViewID = ChairToView(bChair);
                //
                o_chat[bViewID].SetActive(true);
                o_chat[bViewID].GetComponentInChildren<UILabel>().text = strMsg;
                //

            }
        }

        private void ShowNotice(string strMsg)
        {
            UIMsgBox.Instance.Show(true, strMsg);

        }

        private void SetUserTuoGuan(byte bChair, bool bAuto)
        {
            byte bViewID = ChairToView(bChair);


            if (bAuto)
            {

                //tagUserData ud = GameEngine.Instance.GetTableUserItem(bChair);
                //o_head_face[bViewID].GetComponent<UIFace>().ShowFace(-2,ud.Member);
            }
            else
            {

                //tagUserData ud = GameEngine.Instance.GetTableUserItem(bChair);
                //o_head_face[bViewID].GetComponent<UIFace>().ShowFace(ud.FaceId,ud.Member);
            }
        }

        private void ClearAllInfo()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                o_info[i].SetActive(false);
//                o_chat[i].SetActive(false);

            }
        }

        private void OnClearInfoIvk()
        {
            ClearAllInfo();
            OnResetIvk();
        }


        private bool CheckScoreLimit()
        {
            //金币限制检测
            /*
            int nLimit = 0;
            if(GameEngine.Instance.MyUser.ServerUsed.StationID.ToString().EndsWith("39")==true)
            {
                nLimit = 10000;
            }
            else
            {
                nLimit = 20*_lCellScore;
            }
            */

            //if (GameEngine.Instance.MySelf.Money < 20*(Int64) _nBaseScore)
            //{
            //    UIMsgBox.Instance.Show(true, "您的乐豆不足,不能继续游戏!");
            //    Invoke("OnConfirmBackOKIvk", 1.0f);
            //    _bStart = false;
            //    return false;
            //}
            return true;
        }


        private void OnResultCloseIvk()
        {
            CloseResultView();
        }

        private void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        #endregion

    }
}