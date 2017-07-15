using UnityEngine;
using System.Collections;
using Shared;
using System;

using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.DDZ
{

    //游戏术语
    class CallType
    {
        //放弃
        public const ushort GIVE_UP = 0;
        //闷抓
        public const ushort BACK_CATCH = 1;
        //明抓
        public const ushort MING_CATCH = 2;
        //看牌
        public const ushort LOOK_CARD = 3;
        //倒牌
        public const ushort DOUBLE_SCORE = 4;
        //抓
        public const ushort CALL_BANKER = 5;
        //拉
        public const ushort CALL_SCORE = 6;
        //垒，沃
        public const ushort CALL_TWO_SCORE = 7;
        //过
        public const ushort PASS_ACTION = 8;
    };
    public enum TimerTypeEx
    {
        TIMER_NULL = 0,
        TIMER_LOOK_CATCH = 1,
        TIMER_START = 2,
        TIMER_LAND_SCORE = 3,
        TIMER_OUTCARD = 4,
        TIMER_MOSTCARD = 5,
        TIMER_CLOSE = 6
    };
    public enum SoundTypeEx
    {
        SENDCARD = 0,
        CALLSCORE = 1,
        START = 2,
        OUTCARD = 3,
        PASS = 4,
        MOSTCARD = 5,
        END = 6,
        BOMB = 7,
        WARNS = 8,
        PLANE = 9
    };
    public enum GameSoundTypeEx
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
        OPTION_QIANG = 50,
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
        OPTION_MENZHUA = 63,
        OPTION_KANPAI = 64,
        OPTION_DAO = 65,
        OPTION_LA = 66,
        OPTION_LEI = 67,
        OPTION_WO = 68

    };
    public class UIGameEx : MonoBehaviour
    {

        #region ##################变量定义#######################

        //控件
        static GameObject[] o_clock = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_pass = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_ready = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_score = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_chat = new GameObject[GameLogic.GAME_PLAYER_NUM];

        static GameObject[] o_head_face = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_head_nick = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_head_flag = new GameObject[GameLogic.GAME_PLAYER_NUM];

        static GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER_NUM];

        static GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER_NUM];

        static GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER_NUM];

        static GameObject o_buttons_menzhua = null;
        static GameObject o_buttons_dao = null;
        static GameObject o_buttons_mingzhua = null;
        static GameObject o_buttons_fan = null;
        static GameObject o_buttons_la = null;
        static GameObject o_buttons_lei = null;
        static GameObject o_buttons_wo = null;
        //


        //

        static GameObject o_play_buttons = null;
        static GameObject o_btn_outcard = null;
        static GameObject o_btn_pass = null;
        static GameObject o_btn_tip = null;
        static GameObject o_btn_reset = null;

        //

        static GameObject o_ready_buttons = null;
        static GameObject o_btn_ready = null;
        static GameObject o_btn_quit = null;

        //
        static GameObject o_result = null;
        static GameObject o_menzhua_flag = null;


        static GameObject o_hand_cards = null;
        static GameObject o_back_cards = null;
        static GameObject o_tip_bar = null;
        static GameObject[] o_out_cards = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_others = new GameObject[GameLogic.GAME_PLAYER_NUM];
        static GameObject[] o_others_count = new GameObject[GameLogic.GAME_PLAYER_NUM];


        static GameObject o_speak_timer = null;
        static GameObject o_btn_speak = null;
        static GameObject o_btn_speak_count = null;

        static GameObject o_current_time = null;


        //数据
        static bool _bStart = false;
        static TimerTypeEx _bTimerType = TimerTypeEx.TIMER_START;

        static byte _bBombTimes = 0;
        static byte _bRocketTimes = 0;
        static ushort _wTotalTimes = 1;
        static int _nBaseScore = 0;

        static byte[] _bOthersCardCount = new byte[GameLogic.GAME_PLAYER_NUM];

        static byte[][] _bOthersCardData = new byte[GameLogic.GAME_PLAYER_NUM][];

        static bool _bAutoPlay = false;

        static byte _bTurnOutType = 0;
        static byte _bTurnCardCount = 0;
        static byte[] _bTurnCardData = new byte[GameLogic.MAX_COUNT];

        static byte[] _bHandCardData = new byte[GameLogic.MAX_COUNT];
        static byte _bHandCardCount = 0;

        static byte[,] _bOutCardData = new byte[GameLogic.GAME_PLAYER_NUM, GameLogic.MAX_COUNT];
        static byte[] _bOutCardCount = new byte[GameLogic.GAME_PLAYER_NUM];

        static byte[] _bBackCardData = new byte[GameLogic.BACK_COUNT];
        static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER_NUM];

        static byte _bMostUser = GameLogic.NULL_CHAIR;
        static byte _bLander = GameLogic.NULL_CHAIR;
        static byte _bCurrentUser = GameLogic.NULL_CHAIR;
        static ushort _wCurrentScore = 0;
        static byte _bFirstOutUser = GameLogic.NULL_CHAIR;
        static ushort[] _wUserMultiple = new ushort[GameLogic.GAME_PLAYER_NUM];

        static bool[] _bShowInfo = new bool[GameLogic.GAME_PLAYER_NUM];
        static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER_NUM];
        static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER_NUM];
        static byte[] _bEndCardCount = new byte[GameLogic.GAME_PLAYER_NUM];
        static byte[] _bEndCardData = new byte[GameLogic.FULL_COUNT];


        static int _nInfoTickCount = 0;
        static int _nSendCardTick = 0;
        static int _nSendCardCount = 0;
        static bool _bSendCard = false;

        static byte _bBackCatchTag = 0;
        static byte[] _bUserCallTimes = new byte[GameLogic.GAME_PLAYER_NUM];
        static byte[] _bUserNotCall = new byte[GameLogic.GAME_PLAYER_NUM];
        static int _nAutoCount = 0;
        static int _nQuitDelay = 0;
        static bool _bReqQuit = false;
        //音效
        public AudioClip[] _GameSound = new AudioClip[10];
        public AudioClip[] _WomanSound = new AudioClip[70];
        public AudioClip[] _ManSound = new AudioClip[70];

        static byte[] _bTipsCards = new byte[20];
        static byte _bTipsCardsCount = 0;

        static string[] _TableUserName = new string[GameLogic.GAME_PLAYER_NUM];

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
                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    o_clock[i] = GameObject.Find("scene_game_ex/dlg_clock_" + i.ToString());
                    o_pass[i] = GameObject.Find("scene_game_ex/sp_pass_" + i.ToString());
                    o_ready[i] = GameObject.Find("scene_game_ex/sp_ready_" + i.ToString());
                    o_score[i] = GameObject.Find("scene_game_ex/sp_score_" + i.ToString());
                    o_chat[i] = GameObject.Find("scene_game_ex/dlg_chat_msg_" + i.ToString());

                    o_info[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString());
                    o_info_nick[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_nick");
                    o_info_lvl[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_lvl");
                    o_info_id[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_id");
                    o_info_score[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_score");
                    o_info_win[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_win");
                    o_info_lose[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_lose");
                    o_info_run[i] = GameObject.Find("scene_game_ex/dlg_info_" + i.ToString() + "/lbl_run");


                    o_head_face[i] = GameObject.Find("scene_game_ex/dlg_player_" + i.ToString() + "/ctr_user_face");
                    o_head_nick[i] = GameObject.Find("scene_game_ex/dlg_player_" + i.ToString() + "/lbl_nick");
                    o_head_flag[i] = GameObject.Find("scene_game_ex/dlg_player_" + i.ToString() + "/sp_flag");
                    o_others[i] = GameObject.Find("scene_game_ex/dlg_player_" + i.ToString() + "/dlg_card_count");
                    o_user_speak[i] = GameObject.Find("scene_game_ex/ctr_speak_" + i.ToString());
                    o_out_cards[i] = GameObject.Find("scene_game_ex/ctr_out_cards_" + i.ToString());

                    o_result_score[i] = GameObject.Find("scene_game_ex/dlg_result/lbl_score_" + i.ToString());
                    o_others_count[i] = GameObject.Find("scene_game_ex/dlg_player_" + i.ToString() + "/dlg_card_count/lbl_count");


                }


                o_play_buttons = GameObject.Find("scene_game_ex/dlg_play_buttons");
                o_ready_buttons = GameObject.Find("scene_game_ex/dlg_ready_buttons");
                o_result = GameObject.Find("scene_game_ex/dlg_result");

                o_hand_cards = GameObject.Find("scene_game_ex/ctr_hand_cards");
                o_back_cards = GameObject.Find("scene_game_ex/ctr_back_cards");

                o_btn_ready = GameObject.Find("scene_game_ex/dlg_ready_buttons/btn_ready");
                o_btn_quit = GameObject.Find("scene_game_ex/dlg_ready_buttons/btn_quit");

                o_btn_outcard = GameObject.Find("scene_game_ex/dlg_play_buttons/btn_out_card");
                o_btn_pass = GameObject.Find("scene_game_ex/dlg_play_buttons/btn_pass_card");
                o_btn_tip = GameObject.Find("scene_game_ex/dlg_play_buttons/btn_tip");
                o_btn_reset = GameObject.Find("scene_game_ex/dlg_play_buttons/btn_reset");

                o_tip_bar = GameObject.Find("scene_game_ex/dlg_tip_bar");

                o_speak_timer = GameObject.Find("scene_game_ex/dlg_speak_timer");

                o_btn_speak = GameObject.Find("scene_game_ex/btn_speak");

                o_btn_speak_count = GameObject.Find("scene_game_ex/btn_speak/lbl_count");


                o_current_time = GameObject.Find("scene_game_ex/dlg_title_bar/lbl_curr_time");

                o_menzhua_flag = GameObject.Find("scene_game_ex/dlg_bottom_bar/lbl_memzhua");


                //
                o_buttons_menzhua = GameObject.Find("scene_game_ex/dlg_buttons_menzhua");
                o_buttons_dao = GameObject.Find("scene_game_ex/dlg_buttons_dao");
                o_buttons_mingzhua = GameObject.Find("scene_game_ex/dlg_buttons_mingzhua");
                o_buttons_fan = GameObject.Find("scene_game_ex/dlg_buttons_fan");
                o_buttons_la = GameObject.Find("scene_game_ex/dlg_buttons_la");
                o_buttons_lei = GameObject.Find("scene_game_ex/dlg_buttons_lei");
                o_buttons_wo = GameObject.Find("scene_game_ex/dlg_buttons_wo");


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        void InitGameView()
        {
            //Data

            _nQuitDelay = 0;
            _bReqQuit = false;

            _bStart = false;
            _bTimerType = TimerTypeEx.TIMER_START;
            _bBombTimes = 0;
            _bRocketTimes = 0;
            _wTotalTimes = 1;
            _nBaseScore = 0;
            _bAutoPlay = false;
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
            _bBackCatchTag = 0;

            _nAutoCount = 0;

            _bTipsCardsCount = 0;

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
                o_chat[i].SetActive(false);

                o_user_speak[i].SetActive(false);


                o_head_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
                o_head_nick[i].GetComponent<UILabel>().text = "";
                o_head_flag[i].GetComponent<UISprite>().spriteName = "blank";

                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";
                o_menzhua_flag.GetComponent<UILabel>().text = "";

                o_result_score[i].GetComponent<UILabel>().text = "";
                o_others[i].SetActive(false);
                o_out_cards[i].SetActive(false);

                _bUserTrustee[i] = false;
                _wUserMultiple[i] = 0;
                _bOthersCardData[i] = new byte[20];
                _bUserCallTimes[i] = 0;

                _bUserNotCall[i] = 0;
                Array.Clear(_bOthersCardData[i], 0, 20);
            }

            o_play_buttons.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_hand_cards.SetActive(false);
            o_back_cards.SetActive(false);
            o_tip_bar.SetActive(false);

            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);


            o_buttons_menzhua.SetActive(false);
            o_buttons_dao.SetActive(false);
            o_buttons_mingzhua.SetActive(false);
            o_buttons_fan.SetActive(false);
            o_buttons_la.SetActive(false);
            o_buttons_lei.SetActive(false);
            o_buttons_wo.SetActive(false);

        }
        void ResetGameView()
        {

            _bTimerType = TimerTypeEx.TIMER_START;
            _bBombTimes = 0;
            _bRocketTimes = 0;
            _wTotalTimes = 1;
            _bAutoPlay = false;
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
            _bBackCatchTag = 0;

            _nAutoCount = 0;

            _bTipsCardsCount = 0;

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
                o_chat[i].SetActive(false);
                o_user_speak[i].SetActive(false);
                o_head_flag[i].GetComponent<UISprite>().spriteName = "blank";

                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";
                o_menzhua_flag.GetComponent<UILabel>().text = "";
                o_result_score[i].GetComponent<UILabel>().text = "";
                o_others[i].SetActive(false);
                o_out_cards[i].SetActive(false);

                _bUserTrustee[i] = false;
                _wUserMultiple[i] = 0;
                _bOthersCardData[i] = new byte[20];
                _bUserCallTimes[i] = 0;
                _bUserNotCall[i] = 0;
                Array.Clear(_bOthersCardData[i], 0, 20);
            }

            o_play_buttons.SetActive(false);
            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_hand_cards.SetActive(false);
            o_back_cards.SetActive(false);
            o_tip_bar.SetActive(false);

            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);


            o_buttons_menzhua.SetActive(false);
            o_buttons_dao.SetActive(false);
            o_buttons_mingzhua.SetActive(false);
            o_buttons_fan.SetActive(false);
            o_buttons_la.SetActive(false);
            o_buttons_lei.SetActive(false);
            o_buttons_wo.SetActive(false);

            CancelInvoke();

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

        }

        void FixedUpdate()
        {
            o_current_time.GetComponent<UILabel>().text = System.DateTime.Now.ToString("hh:mm:ss");

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
                UIManager.Instance.GoUI(enSceneType.SCENE_GAME_EX, enSceneType.SCENE_SERVER);
            }
        }
        void Update()
        {
            /*if (GameEngine.Instance.MySelf.IsDisconnectFromServer && _bStart == true)
            {
                CancelInvoke();

                _bStart = false;

                PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);

                UIMsgBox.Instance.Show(true, "您的网络不给力哦，网络中断了");

                Invoke("GoLogin", 3.0f);
            }*/
        }

        void GoLogin()
        {
            UIManager.Instance.GoUI(enSceneType.SCENE_GAME_EX, enSceneType.SCENE_LOGIN);
        }
        #endregion


        #region ##################框架消息#######################

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME_EX) return;
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
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME_EX) return;
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
                        //Invoke("CheckScoreLimit",5.0f);
                        //金币限制检测
                        /*
                        if(GameEngine.Instance.MySelf.Self.lScore <20*_nBaseScore)
                        {
                             UIMsgBox.Instance.Show(true,"您的乐豆不足,不能继续游戏!");
                             OnConfirmBackOKIvk();
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

                case TableEvents.GAME_START:
                    {
                        for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                        {
                            if (GameEngine.Instance.GetTableUserItem(i) != null)
                            {
                                SetUserReady(i, true);
                            }
                        }

                        PlaySound(SoundTypeEx.CALLSCORE);

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
                case (byte)GameLogic.GS_WK_SCORE:
                    {
                        SwitchScoreSceneView(packet);
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

            PlaySound(SoundTypeEx.CALLSCORE);
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

        #endregion


        #region ##################游戏消息#######################

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME_EX) return;
            if (_bReqQuit == true) return;

            switch (subcmd)
            {
                case SubCmdEx.SUB_S_CHOICE_LOOK:
                    {
                        Debug.Log("SUB_S_CHOICE_LOOK");
                        OnCallBankerResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_SEND_CARD:
                    {
                        Debug.Log("SUB_S_SEND_CARD");
                        OnSendCardResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_LAND_SCORE:
                    {
                        Debug.Log("SUB_S_LAND_SCORE");
                        OnLandScoreResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_GAME_START:
                    {
                        Debug.Log("SUB_S_GAME_START");
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_OUT_CARD:
                    {
                        Debug.Log("SUB_S_OUT_CARD");
                        OnOutCardResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_PASS_CARD:
                    {
                        Debug.Log("SUB_S_PASS_CARD");
                        OnPassCardResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_S_GAME_END:
                    {
                        Debug.Log("SUB_S_GAME_END");
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmdEx.SUB_C_TRUSTEE:
                    {
                        OnTuoGuanResp(packet);
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //开始打牌消息处理函数
        void OnGameStartResp(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
            try
            {

                packet.BeginRead();
                _wCurrentScore = packet.GetByte();
                _bCurrentUser = (byte)packet.GetUShort();
                packet.GetBytes(ref _bBackCardData, 3);
                _wUserMultiple[0] = packet.GetUShort();
                _wUserMultiple[1] = packet.GetUShort();
                _wUserMultiple[2] = packet.GetUShort();


                //

                _bTurnCardCount = 0;
                _bTurnOutType = GameLogic.CT_ERROR;
                _bFirstOutUser = _bCurrentUser;

                //
                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    if (i == _bLander)
                    {
                        _bOthersCardCount[i] = 20;

                    }
                    else
                    {
                        _bOthersCardCount[i] = 17;

                    }

                    SetPlayerCardCount(i, _bOthersCardCount[i]);
                }
                //
                SetLander(_bLander);
                //
                _wTotalTimes = _wUserMultiple[GetSelfChair()];
                SetTotalTimes(_wTotalTimes);
                //
                SetBackCardData(_bBackCardData, (byte)GameLogic.BACK_COUNT);
                //
                SetCallScore(GameLogic.NULL_CHAIR, 0);
                //
                if (_bLander == GetSelfChair())
                {

                    Buffer.BlockCopy(_bBackCardData, 0, _bHandCardData, _bHandCardCount, GameLogic.BACK_COUNT);
                    _bHandCardCount += (byte)GameLogic.BACK_COUNT;
                    GameLogic.SortCardList(ref _bHandCardData, _bHandCardCount, GameLogic.ST_ORDER);
                    SetHandCardData(_bHandCardData, _bHandCardCount);
                }

                //
                if (_bCurrentUser == GetSelfChair())
                {
                    HideAllButtons();
                    o_play_buttons.SetActive(true);
                }
                //
                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                {
                    SetUserClock(_bCurrentUser, 1, TimerTypeEx.TIMER_OUTCARD);
                }
                else
                {
                    SetUserClock(_bCurrentUser, GameLogic.TIME_OUT_CARD, TimerTypeEx.TIMER_OUTCARD);
                }
                //
                PlaySound(SoundTypeEx.START);


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏发牌消息处理函数
        void OnSendCardResp(NPacket packet)
        {

            try
            {
                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_SCORE;
                packet.BeginRead();
                _bCurrentUser = (byte)packet.GetUShort();
                byte[] bcarddata = new byte[60];
                packet.GetBytes(ref bcarddata, 60);
                packet.GetBytes(ref _bBackCardData, 3);


                for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    if (i == GetSelfChair())
                    {
                        Buffer.BlockCopy(bcarddata, i * 20, _bHandCardData, 0, 20);
                        _bHandCardCount = 17;
                    }
                    else
                    {
                        Array.Clear(_bOthersCardData[i], 0, 20);
                        _bOthersCardCount[i] = 17;
                    }
                }

                GameLogic.SortCardList(ref _bHandCardData, _bHandCardCount, GameLogic.ST_ORDER);
                SetHandCardData(_bHandCardData, _bHandCardCount);

                SetBackCardData(_bBackCardData, 3);

                PlaySound(SoundTypeEx.SENDCARD);

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户叫分消息处理函数
        void OnLandScoreResp(NPacket packet)
        {

            try
            {
                packet.BeginRead();
                byte bLandUser = (byte)packet.GetUShort();
                _bCurrentUser = (byte)packet.GetUShort();
                byte bScore = packet.GetByte();
                byte bCurrentState = packet.GetByte();
                byte bUserCallTimes = packet.GetByte();
                ushort bBankerUser = packet.GetUShort();

                //设置地主标志
                if (bBankerUser >= 0 && bBankerUser <= 2)
                {
                    _bLander = (byte)bBankerUser;
                    SetLander(_bLander);
                }

                //设置叫分操作
                SetCallScore(bLandUser, bScore);



                //播放声音
                byte bGender = GameEngine.Instance.GetTableUserItem(bLandUser).Gender;
                switch ((ushort)bScore)
                {
                    case CallType.BACK_CATCH:
                        {
                            _bBackCatchTag = 1;
                            o_menzhua_flag.GetComponent<UILabel>().text = "闷抓";
                            PlayGameSound(GameSoundTypeEx.OPTION_MENZHUA, bGender);

                            break;
                        }
                    case CallType.LOOK_CARD:
                        {

                            PlayGameSound(GameSoundTypeEx.OPTION_KANPAI, bGender);
                            break;
                        }
                    case CallType.MING_CATCH:
                        {
                            PlayGameSound(GameSoundTypeEx.OPTION_CALL, bGender);
                            break;
                        }
                    case CallType.CALL_SCORE:
                        {
                            if (_bLander == bLandUser)
                            {
                                PlayGameSound(GameSoundTypeEx.OPTION_LA, bGender);
                            }
                            else
                            {
                                PlayGameSound(GameSoundTypeEx.OPTION_DAO, bGender);
                            }
                            break;
                        }
                    case CallType.CALL_TWO_SCORE:
                        {
                            if (_bLander == bLandUser)
                            {
                                PlayGameSound(GameSoundTypeEx.OPTION_WO, bGender);
                            }
                            else
                            {
                                PlayGameSound(GameSoundTypeEx.OPTION_LEI, bGender);
                            }
                            break;
                        }
                    case CallType.PASS_ACTION:
                        {
                            _bUserNotCall[bLandUser] = 1;
                            PlaySound(SoundTypeEx.CALLSCORE);
                            break;
                        }
                }



                //显示操作按钮
                if (_bCurrentUser == GetSelfChair())
                {
                    switch (bCurrentState & 0x0F)
                    {
                        case CallType.CALL_BANKER:
                            {
                                HideAllButtons();
                                SetCallScore(_bCurrentUser, 255);
                                o_buttons_mingzhua.SetActive(true);
                                break;
                            }
                        case CallType.CALL_SCORE:
                            {
                                HideAllButtons();
                                SetCallScore(_bCurrentUser, 255);


                                if (_bLander == GetSelfChair())
                                {
                                    o_buttons_la.SetActive(true);
                                }
                                else
                                {
                                    o_buttons_dao.SetActive(true);
                                }


                                break;
                            }
                        case CallType.CALL_TWO_SCORE:
                            {
                                HideAllButtons();
                                SetCallScore(_bCurrentUser, 255);

                                if (_bLander == GetSelfChair())
                                {
                                    o_buttons_wo.SetActive(true);
                                }
                                else
                                {
                                    o_buttons_lei.SetActive(true);
                                }

                                break;
                            }
                    }
                }

                if (_bLander < GameLogic.GAME_PLAYER_NUM)
                {
                    _bUserCallTimes[bLandUser] = bUserCallTimes;
                    byte bUser1 = (byte)((_bLander + 1) % GameLogic.GAME_PLAYER_NUM);
                    byte bUser2 = (byte)((_bLander + 2) % GameLogic.GAME_PLAYER_NUM);

                    //倍数
                    if (_bBackCatchTag == 1)
                    {
                        if (GetSelfChair() == _bLander)
                        {
                            byte times = (byte)(_bUserCallTimes[_bLander] + Math.Max(_bUserCallTimes[bUser1], _bUserCallTimes[bUser2]));
                            _wTotalTimes = (ushort)(2 * Math.Pow(2.0f, (double)times));
                        }
                        else
                        {
                            byte times = (byte)(_bUserCallTimes[_bLander] + _bUserCallTimes[GetSelfChair()]);
                            _wTotalTimes = (ushort)(2 * Math.Pow(2.0f, (double)times));
                        }

                    }
                    else
                    {
                        if (GetSelfChair() == _bLander)
                        {
                            byte times = (byte)(_bUserCallTimes[_bLander] + Math.Max(_bUserCallTimes[bUser1], _bUserCallTimes[bUser2]));
                            _wTotalTimes = (ushort)(Math.Pow(2.0f, (double)times));
                        }
                        else
                        {
                            byte times = (byte)(_bUserCallTimes[_bLander] + _bUserCallTimes[GetSelfChair()]);
                            _wTotalTimes = (ushort)(Math.Pow(2.0f, (double)times));
                        }

                    }
                    SetTotalTimes(_wTotalTimes);

                    if (bScore == CallType.BACK_CATCH || bScore == CallType.CALL_SCORE || bScore == CallType.CALL_TWO_SCORE)
                    {
                        if (_wTotalTimes == 2 || _wTotalTimes == 4 || _wTotalTimes == 8 || _wTotalTimes == 16 || _wTotalTimes == 32 || _wTotalTimes == 64)
                        {
                            UIEffect.Instance.ShowBei(true, _wTotalTimes);
                        }
                    }
                }
                else
                {

                    if (_bBackCatchTag == 1)
                    {
                        SetTotalTimes(2);
                    }
                    else
                    {
                        SetTotalTimes(1);
                    }
                }



                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                if (_bCurrentUser < GameLogic.GAME_PLAYER_NUM)
                {
                    SetCallScore(_bCurrentUser, 255);
                    uint ntime = (uint)((_bUserTrustee[_bCurrentUser] == true) ? 1 : GameLogic.TIME_LAND_SCORE);
                    SetUserClock(_bCurrentUser, ntime, TimerTypeEx.TIMER_LAND_SCORE);
                }

            }
            catch (Exception ex)
            {
                Debug.Log("error:" + ex.Source);
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户出牌消息处理函数
        void OnOutCardResp(NPacket packet)
        {
            try
            {

                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                o_play_buttons.SetActive(false);

                //
                _bTipsCardsCount = 0;

                //Parse packet
                packet.BeginRead();
                byte bCardCount = packet.GetByte();
                _bCurrentUser = (byte)packet.GetUShort();
                byte bOutUser = (byte)packet.GetUShort();
                byte[] bCardData = new byte[20];
                packet.GetBytes(ref bCardData, 20);




                //
                if (bOutUser != GetSelfChair())
                {
                    SetOutCardData(bOutUser, bCardData, bCardCount);
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

                //sound effect
                if (bOutUser != GetSelfChair())
                {

                    byte bGender = GameEngine.Instance.GetTableUserItem(bOutUser).Gender;
                    byte bOutType = GameLogic.GetCardType(bCardData, bCardCount);
                    if (bOutType == GameLogic.CT_BOMB_CARD)
                    {

                        PlayGameSound(GameSoundTypeEx.CARD_BOMB, bGender);
                        PlaySound(SoundTypeEx.BOMB);
                        UIEffect.Instance.ShowBomb(true);
                    }
                    else if (bOutType == GameLogic.CT_MISSILE_CARD)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_ROCKET, bGender);
                        PlaySound(SoundTypeEx.BOMB);
                        UIEffect.Instance.ShowRocket(true);
                    }
                    else if ((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO) && _bTurnCardCount >= 6)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_PLANE, bGender);
                        PlaySound(SoundTypeEx.PLANE);
                        UIEffect.Instance.ShowPlane(true);
                    }
                    else
                    {
                        if (bOutType == GameLogic.CT_SINGLE)
                        {
                            byte card = GameLogic.GetCardValue(bCardData[0]);
                            if (card == 1)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_A, bGender);
                            }
                            if (card == 2)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_2, bGender);
                            }
                            if (card == 3)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_3, bGender);
                            }
                            if (card == 4)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_4, bGender);
                            }
                            if (card == 5)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_5, bGender);
                            }
                            if (card == 6)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_6, bGender);
                            }
                            if (card == 7)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_7, bGender);
                            }
                            if (card == 8)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_8, bGender);
                            }
                            if (card == 9)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_9, bGender);
                            }
                            if (card == 10)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_10, bGender);
                            }
                            if (card == 11)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_J, bGender);
                            }
                            if (card == 12)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_Q, bGender);
                            }
                            if (card == 13)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_K, bGender);
                            }
                            if (bCardData[0] == 0x4E)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_S, bGender);
                            }
                            if (bCardData[0] == 0x4F)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_B, bGender);
                            }
                        }
                        else if (bOutType == GameLogic.CT_DOUBLE)
                        {
                            byte card = GameLogic.GetCardValue(bCardData[0]);
                            if (card == 1)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_AA, bGender);
                            }
                            if (card == 2)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_22, bGender);
                            }
                            if (card == 3)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_33, bGender);
                            }
                            if (card == 4)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_44, bGender);
                            }
                            if (card == 5)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_55, bGender);
                            }
                            if (card == 6)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_66, bGender);
                            }
                            if (card == 7)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_77, bGender);
                            }
                            if (card == 8)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_88, bGender);
                            }
                            if (card == 9)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_99, bGender);
                            }
                            if (card == 10)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_11, bGender);
                            }
                            if (card == 11)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_JJ, bGender);
                            }
                            if (card == 12)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_QQ, bGender);
                            }
                            if (card == 13)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_KK, bGender);
                            }

                        }
                        else if (bOutType == GameLogic.CT_THREE)
                        {
                            byte card = GameLogic.GetCardValue(bCardData[0]);
                            if (card == 1)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_AAA, bGender);
                            }
                            if (card == 2)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_222, bGender);
                            }
                            if (card == 3)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_333, bGender);
                            }
                            if (card == 4)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_444, bGender);
                            }
                            if (card == 5)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_555, bGender);
                            }
                            if (card == 6)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_666, bGender);
                            }
                            if (card == 7)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_777, bGender);
                            }
                            if (card == 8)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_888, bGender);
                            }
                            if (card == 9)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_999, bGender);
                            }
                            if (card == 10)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_111, bGender);
                            }
                            if (card == 11)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_JJJ, bGender);
                            }
                            if (card == 12)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_QQQ, bGender);
                            }
                            if (card == 13)
                            {
                                PlayGameSound(GameSoundTypeEx.CARD_KKK, bGender);
                            }

                        }
                        else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_TWO)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_FOUR_2D, bGender);
                        }
                        else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_ONE)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_FOUR_2, bGender);
                        }
                        else if (bOutType == GameLogic.CT_SINGLE_LINE)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_LINE, bGender);
                        }
                        else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_THREE_1D, bGender);
                        }
                        else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_THREE_1, bGender);
                        }
                        else if (bOutType == GameLogic.CT_DOUBLE_LINE)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_DBL_LINE, bGender);
                        }
                        PlaySound(SoundTypeEx.OUTCARD);
                    }
                }
                //hand card count
                _bOthersCardCount[bOutUser] -= bCardCount;
                SetPlayerCardCount(bOutUser, _bOthersCardCount[bOutUser]);

                //less one card warns
                if (bOutUser != GetSelfChair())
                {
                    if (_bOthersCardCount[bOutUser] == 1)
                    {

                        PlayGameSound(GameSoundTypeEx.OPTION_WARN_1, GameEngine.Instance.GetTableUserItem(bOutUser).Gender);
                    }

                    if (_bOthersCardCount[bOutUser] == 2)
                    {

                        PlayGameSound(GameSoundTypeEx.OPTION_WARN_2, GameEngine.Instance.GetTableUserItem(bOutUser).Gender);
                    }
                }
                //Most
                if (_bCurrentUser == bOutUser)
                {
                    //
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
                    PlaySound(SoundTypeEx.MOSTCARD);

                    //Timer
                    SetUserClock(_bCurrentUser, 3, TimerTypeEx.TIMER_MOSTCARD);
                    return;
                }


                //player setting
                if (_bCurrentUser != GameLogic.NULL_CHAIR)
                {
                    SetUserPass(_bCurrentUser, false);
                    SetOutCardData(_bCurrentUser, null, 0);
                }

                //player setting
                if (_bCurrentUser == GetSelfChair())
                {
                    o_play_buttons.SetActive(true);

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
                        GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref OutCardResult);

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

                //timer setting
                if (_bCurrentUser != GameLogic.NULL_CHAIR)
                {
                    if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                    {
                        SetUserClock(_bCurrentUser, 1, TimerTypeEx.TIMER_OUTCARD);
                    }
                    else
                    {
                        SetUserClock(_bCurrentUser, GameLogic.TIME_OUT_CARD, TimerTypeEx.TIMER_OUTCARD);
                    }
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户托管消息处理函数
        void OnTuoGuanResp(NPacket packet)
        {
            try
            {
                packet.BeginRead();
                //
                byte bchair = (byte)packet.GetUShort();
                bool bAuto = packet.GetBool();

                SetUserTuoGuan(bchair, bAuto);


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //用户不出消息处理函数
        void OnPassCardResp(NPacket packet)
        {

            try
            {
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                //
                o_play_buttons.SetActive(false);
                //
                _bTipsCardsCount = 0;

                //Parse packet
                packet.BeginRead();
                bool bNewTurn = packet.GetBool();
                byte bPassUser = (byte)packet.GetUShort();
                _bCurrentUser = (byte)packet.GetUShort();

                //pass player view
                if (bPassUser != GetSelfChair())
                {
                    byte bGender = GameEngine.Instance.GetTableUserItem(bPassUser).Gender;
                    SetOutCardData(bPassUser, null, 0);
                    SetUserPass(bPassUser, true);
                    PlayGameSound(GameSoundTypeEx.OPTION_PASS, bGender);

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
                if (_bCurrentUser == GetSelfChair())
                {
                    o_play_buttons.SetActive(true);

                    o_btn_outcard.GetComponent<UIButton>().isEnabled = VerdictOutCard();
                    o_btn_pass.GetComponent<UIButton>().isEnabled = ((_bTurnCardCount > 0) ? true : false);
                    o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                    //ResetHandCardData();

                    if (_bTurnCardCount > 0)
                    {
                        tagOutCardResult OutCardResult = new tagOutCardResult();
                        GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref OutCardResult);

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

                //set timer
                if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
                {
                    SetUserClock(_bCurrentUser, 1, TimerTypeEx.TIMER_OUTCARD);
                }
                else
                {
                    SetUserClock(_bCurrentUser, GameLogic.TIME_OUT_CARD, TimerTypeEx.TIMER_OUTCARD);
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏结束消息处理函数
        void OnGameEndResp(NPacket packet)
        {
            if (_bAutoPlay == true)
            {
                _bAutoPlay = false;
                UseTuoGuan(false);
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
            //
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

            Invoke("ShowResultView", 4);
        }

        //抽牌决定庄家
        void OnCallBankerResp(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_SCORE;
            //读取封包
            packet.BeginRead();
            _bCurrentUser = (byte)packet.GetUShort();
            byte bMingCard = packet.GetByte();

            //
            Array.Clear(_bHandCardData, 0, _bHandCardData.Length);
            Array.Clear(_bBackCardData, 0, 3);

            if (_bCurrentUser == GetSelfChair())
            {
                _bHandCardData[UnityEngine.Random.Range(0, 17)] = bMingCard;
            }
            else
            {
                _bOthersCardData[_bCurrentUser][UnityEngine.Random.Range(0, 17)] = bMingCard;
            }
            _bHandCardCount = 17;

            PlaySound(SoundTypeEx.START);
            //
            SetBackCardData(null, 0);
            //
            _wTotalTimes = 1;
            SetTotalTimes(_wTotalTimes);
            //
            SetLander(GameLogic.NULL_CHAIR);
            //
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
            //
            SetCallScore(GameLogic.NULL_CHAIR, 0);
            //
            o_ready_buttons.SetActive(false);
            //
            //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);

            //开始发牌
            DispatchCardStart();

        }

        //初始场景处理函数
        void SwitchFreeSceneView(NPacket packet)
        {

            ResetGameView();

            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

            packet.BeginRead();
            _nBaseScore = packet.GetInt();

            _bStart = true;

            SetUserClock(GetSelfChair(), GameLogic.TIME_READY, TimerTypeEx.TIMER_START);

            o_ready_buttons.SetActive(true);

            SetBaseScore(_nBaseScore);

            _wTotalTimes = 1;

            SetTotalTimes(_wTotalTimes);

            UpdateUserView();

            //PlayerPrefs.SetInt("UsedServ", 0);
        }

        //叫分场景处理函数
        void SwitchScoreSceneView(NPacket packet)
        {
            /*
             *   LONG                            lBaseScore;                         //基础积分
                 WORD                            wFirstUser;                         //首叫玩家
                 WORD                            wBankerUser;                        //庄家用户
                 WORD                            wCurrentUser;                       //当前玩家
                 BYTE                            bCardData[20];                      //手上扑克
                 BYTE                            bBackCard[3];                       //底牌扑克
                 BYTE                            bMingCard;                          //首叫明牌
                 BYTE                            bCurrentState;                      //当前状态
                 BYTE                            bCallScoreTimes[3];                 //叫分次数
                 BYTE                            bBackCatchTag;                      //闷抓标志
                 bool                            bUserTrustee[GAME_PLAYER_NUM];          //玩家托管
              */

            InitGameView();

            _bStart = true;

            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_SCORE;

            packet.BeginRead();
            //状态信息
            _nBaseScore = packet.GetInt();
            byte bFirstUser = (byte)packet.GetUShort();
            _bLander = (byte)packet.GetUShort();
            _bCurrentUser = (byte)packet.GetUShort();
            //手牌
            packet.GetBytes(ref _bHandCardData, 20);
            _bHandCardCount = 17;
            //底牌
            packet.GetBytes(ref _bBackCardData, 3);


            byte bMingCard = packet.GetByte();
            byte bCurrentState = packet.GetByte();

            _bUserCallTimes[0] = packet.GetByte();
            _bUserCallTimes[1] = packet.GetByte();
            _bUserCallTimes[2] = packet.GetByte();
            _bBackCatchTag = packet.GetByte();

            //设置地主
            SetLander(_bLander);

            //计算倍数
            if (_bLander < GameLogic.GAME_PLAYER_NUM)
            {
                byte bUser1 = (byte)((_bLander + 1) % GameLogic.GAME_PLAYER_NUM);
                byte bUser2 = (byte)((_bLander + 2) % GameLogic.GAME_PLAYER_NUM);

                //倍数
                if (_bBackCatchTag == 1)
                {
                    if (GetSelfChair() == _bLander)
                    {
                        byte times = (byte)(_bUserCallTimes[_bLander] + Math.Max(_bUserCallTimes[bUser1], _bUserCallTimes[bUser2]));
                        _wTotalTimes = (ushort)(2 * Math.Pow(2.0f, (double)times));
                    }
                    else
                    {
                        byte times = (byte)(_bUserCallTimes[_bLander] + _bUserCallTimes[GetSelfChair()]);
                        _wTotalTimes = (ushort)(2 * Math.Pow(2.0f, (double)times));
                    }

                }
                else
                {
                    if (GetSelfChair() == _bLander)
                    {
                        byte times = (byte)(_bUserCallTimes[_bLander] + Math.Max(_bUserCallTimes[bUser1], _bUserCallTimes[bUser2]));
                        _wTotalTimes = (ushort)(Math.Pow(2.0f, (double)times));
                    }
                    else
                    {
                        byte times = (byte)(_bUserCallTimes[_bLander] + _bUserCallTimes[GetSelfChair()]);
                        _wTotalTimes = (ushort)(Math.Pow(2.0f, (double)times));
                    }

                }
                SetTotalTimes(_wTotalTimes);
            }
            else
            {
                SetTotalTimes(1);

            }

            //托管玩家
            _bUserTrustee[0] = packet.GetBool();
            _bUserTrustee[1] = packet.GetBool();
            _bUserTrustee[2] = packet.GetBool();


            //UI
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bOthersCardCount[i] = 17;
                SetPlayerCardCount(i, _bOthersCardCount[i]);
            }

            //底分
            SetBaseScore(_nBaseScore);

            //叫分按钮

            if (_bCurrentUser == GetSelfChair())
            {
                switch (bCurrentState & 0x0F)
                {
                    case CallType.CALL_BANKER:
                        {
                            HideAllButtons();
                            SetCallScore(_bCurrentUser, 255);
                            o_buttons_mingzhua.SetActive(true);
                            break;
                        }
                    case CallType.CALL_SCORE:
                        {
                            HideAllButtons();
                            SetCallScore(_bCurrentUser, 255);
                            if (_bLander == GetSelfChair())
                            {
                                o_buttons_la.SetActive(true);
                            }
                            else
                            {
                                o_buttons_dao.SetActive(true);
                            }
                            break;
                        }
                    case CallType.CALL_TWO_SCORE:
                        {
                            HideAllButtons();
                            SetCallScore(_bCurrentUser, 255);
                            if (_bLander == GetSelfChair())
                            {
                                o_buttons_wo.SetActive(true);
                            }
                            else
                            {
                                o_buttons_lei.SetActive(true);
                            }
                            break;
                        }
                }

            }
            else
            {
                HideAllButtons();
            }

            //底牌
            SetBackCardData(_bBackCardData, 3);
            //手牌
            SetHandCardData(_bHandCardData, _bHandCardCount);
            //时间
            SetUserClock(_bCurrentUser, GameLogic.TIME_LAND_SCORE, TimerTypeEx.TIMER_LAND_SCORE);
            //更新信息
            UpdateUserView();

            //tuoguan
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetUserTuoGuan((byte)i, _bUserTrustee[i]);
                if (i == GetSelfChair() && _bUserTrustee[i] == true)
                {
                    UseTuoGuan(true);
                }
            }

            if (_bBackCatchTag == 1)
            {
                o_menzhua_flag.GetComponent<UILabel>().text = "闷抓";
            }
            else
            {
                o_menzhua_flag.GetComponent<UILabel>().text = "";
            }

            //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);

        }

        //游戏场景处理函数
        void SwitchPlaySceneView(NPacket packet)
        {
            /*
                     LONG                            lBaseScore;                         //基础积分
                     WORD                            wFirstUser;                         //首叫玩家
                     WORD                            wBankerUser;                        //坑主玩家
                     WORD                            wCurrentUser;                       //当前玩家
                     WORD                            wLastOutUser;                       //出牌的人
                     WORD                            wOutBombCount;                      //炸弹数目
                     BYTE                            bMingCard;                          //首叫明牌
                     BYTE                            bCallScoreTimes[3];                 //叫分次数
                     BYTE                            bBackCard[3];                       //底牌扑克
                     BYTE                            bCardData[20];                      //手上扑克
                     BYTE                            bCardCount[3];                      //扑克数目
                     BYTE                            bTurnCardCount;                     //基础出牌
                     BYTE                            bTurnCardData[20];                  //出牌列表
                     BYTE                            bScoreTimes[3];                     //分数倍数
                     bool                            bUserTrustee[GAME_PLAYER_NUM];          //玩家托管
            */
            //
            InitGameView();
            //
            _bStart = true;
            //
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
            //
            packet.BeginRead();
            _nBaseScore = packet.GetInt();
            _bFirstOutUser = (byte)packet.GetUShort();
            _bLander = (byte)packet.GetUShort();
            _bCurrentUser = (byte)packet.GetUShort();
            byte bLastOutUser = (byte)packet.GetUShort();
            _bBombTimes = (byte)packet.GetUShort();
            byte bMingCard = packet.GetByte();
            byte[] bCallScoreTimes = new byte[GameLogic.GAME_PLAYER_NUM];
            bCallScoreTimes[0] = packet.GetByte();
            bCallScoreTimes[1] = packet.GetByte();
            bCallScoreTimes[2] = packet.GetByte();
            //
            packet.GetBytes(ref _bBackCardData, 3);
            packet.GetBytes(ref _bHandCardData, 20);
            //
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


            byte[] bScoreTimes = new byte[GameLogic.GAME_PLAYER_NUM];
            bScoreTimes[0] = packet.GetByte();
            bScoreTimes[1] = packet.GetByte();
            bScoreTimes[2] = packet.GetByte();


            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bUserTrustee[i] = packet.GetBool();
            }


            //UI设置
            SetLander(_bLander);
            //
            SetTotalTimes((ushort)bScoreTimes[GetSelfChair()]);
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
                    GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref OutCardResult);

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
            SetUserClock(_bCurrentUser, GameLogic.TIME_OUT_CARD, TimerTypeEx.TIMER_OUTCARD);
            //
            UpdateUserView();
            //tuoguan
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                SetUserTuoGuan((byte)i, _bUserTrustee[i]);
                if (i == GetSelfChair() && _bUserTrustee[i] == true)
                {
                    UseTuoGuan(true);
                }
            }

            //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MySelf.ServerUsed.ServerID);
        }

        #endregion


        #region ##################UI 事件#######################

        void BreakGame()
        {
            _bStart = false;
            GameEngine.Instance.Quit();
        }

        void OnBackIvk()
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
                    UIExitBox.Instance.ConfirmCallBack  = new ConfirmCall(OnConfirmBackOKIvk);
                    UIExitBox.Instance.CancelCallBack   = new CancelCall(OnConfirmBackCancelIvk);
                    UIExitBox.Instance.Show(true);
                    */
                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
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
        void OnSettingIvk()
        {
            UISetting.Instance.Show(true);
        }
        void OnTuoGuanIvk()
        {
            if (GameEngine.Instance.MySelf.GameStatus == GameLogic.GS_WK_FREE) return;

            _bAutoPlay = !_bAutoPlay;

            UseTuoGuan(_bAutoPlay);

            if (_bCurrentUser == GetSelfChair() && _bAutoPlay == true)
            {
                byte bViewID = ChairToView(_bCurrentUser);
                o_clock[bViewID].GetComponent<UIClock>().SetTimer(1000);
            }

        }

        void UseTuoGuan(bool bUsed)
        {
            try
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_TRUSTEE);
                packet.AddUShort((ushort)GetSelfChair());
                packet.AddBool(bUsed);
                GameEngine.Instance.Send(packet);

                _bUserTrustee[GetSelfChair()] = bUsed;

                _bAutoPlay = bUsed;

                _bUserTrustee[GetSelfChair()] = bUsed;

                SetUserTuoGuan(GetSelfChair(), bUsed);

                _nAutoCount = 0;


            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }
        void OnChatIvk()
        {
            //UIChat.Instance.Show(true);

        }
        void OnPlayerSelfIvk()
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

        void OnPlayerPrevIvk()
        {
            byte bchair = (byte)((GetSelfChair() + 2) % GameLogic.GAME_PLAYER_NUM);
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

        void OnPlayerNextIvk()
        {
            byte bchair = (byte)((GetSelfChair() + 1) % GameLogic.GAME_PLAYER_NUM);
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


        void OnReadyIvk()
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

        void OnQuitIvk()
        {
            OnConfirmBackOKIvk();
        }



        //看牌
        void OnBtnKanPaiIvk()
        {
            DoCall(CallType.LOOK_CARD);
        }

        //闷抓
        void OnBtnMenZhuaIvk()
        {
            DoCall(CallType.BACK_CATCH);
        }

        //明抓
        void OnBtnMingZhuaIvk()
        {
            DoCall(CallType.MING_CATCH);
        }

        //放弃
        void OnBtnGiveupIvk()
        {
            DoCall(CallType.GIVE_UP);
        }

        //倒
        void OnBtnDaoIvk()
        {
            DoCall(CallType.DOUBLE_SCORE);
        }

        //不倒
        void OnBtnNotDaoIvk()
        {
            DoCall(CallType.GIVE_UP);
        }

        //拉
        void OnBtnLaIvk()
        {
            DoCall(CallType.DOUBLE_SCORE);
        }

        //不拉
        void OnBtnNotLaIvk()
        {
            DoCall(CallType.GIVE_UP);
        }

        //垒
        void OnBtnLeiIvk()
        {
            DoCall(CallType.DOUBLE_SCORE);
        }

        //不垒
        void OnBtnNotLeiIvk()
        {
            DoCall(CallType.GIVE_UP);
        }

        //沃
        void OnBtnWoIvk()
        {
            DoCall(CallType.DOUBLE_SCORE);
        }

        //不沃
        void OnBtnNotWoIvk()
        {
            DoCall(CallType.GIVE_UP);
        }

        void HideAllButtons()
        {
            o_buttons_menzhua.SetActive(false);
            o_buttons_dao.SetActive(false);
            o_buttons_mingzhua.SetActive(false);
            o_buttons_fan.SetActive(false);
            o_buttons_la.SetActive(false);
            o_buttons_lei.SetActive(false);
            o_buttons_wo.SetActive(false);
        }
        void DoCall(ushort calltype)
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_LAND_SCORE);
                packet.Addbyte((byte)calltype);
                GameEngine.Instance.Send(packet);
                //
                HideAllButtons();
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        void OnOutCardIvk()
        {
            try
            {

                if (_bCurrentUser != GetSelfChair()) return;
                if (GameEngine.Instance.MySelf.GameStatus != GameLogic.GS_WK_PLAYING) return;
                if (VerdictOutCard() == false)
                {
                    return;
                }



                byte[] bcards = new byte[GameLogic.MAX_COUNT];
                byte count = 0;
                UICardControl ctr = o_hand_cards.GetComponent<UICardControl>();
                ctr.GetShootCard(ref bcards, ref count);
                if (count == 0) return;

                //send data
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                packet.Addbyte(count);
                packet.AddBytes(bcards, count);
                GameEngine.Instance.Send(packet);
                //
                _bTipsCardsCount = 0;
                //set out cards
                SetOutCardData(GetSelfChair(), bcards, count);
                //
                SetUserPass(GetSelfChair(), false);
                //delete first
                GameLogic.RemoveCard(bcards, count, ref _bHandCardData, ref _bHandCardCount);
                //
                byte bOutType = GameLogic.GetCardType(bcards, count);
                byte bGender = GameEngine.Instance.MySelf.Gender;
                if (bOutType == GameLogic.CT_BOMB_CARD)
                {

                    PlayGameSound(GameSoundTypeEx.CARD_BOMB, bGender);
                    PlaySound(SoundTypeEx.BOMB);
                    UIEffect.Instance.ShowBomb(true);
                }
                else if (bOutType == GameLogic.CT_MISSILE_CARD)
                {
                    PlayGameSound(GameSoundTypeEx.CARD_ROCKET, bGender);
                    PlaySound(SoundTypeEx.BOMB);
                    UIEffect.Instance.ShowRocket(true);
                }
                else if ((bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE || bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO) && count >= 6)
                {
                    PlayGameSound(GameSoundTypeEx.CARD_PLANE, bGender);
                    PlaySound(SoundTypeEx.PLANE);
                    UIEffect.Instance.ShowPlane(true);
                }
                else
                {
                    if (bOutType == GameLogic.CT_SINGLE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_A, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_2, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_3, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_4, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_5, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_6, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_7, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_8, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_9, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_10, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_J, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_Q, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_K, bGender);
                        }
                        if (bcards[0] == 0x4E)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_S, bGender);
                        }
                        if (bcards[0] == 0x4F)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_B, bGender);
                        }
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_AA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_22, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_33, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_44, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_55, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_66, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_77, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_88, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_99, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_11, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_JJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_QQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_KK, bGender);
                        }

                    }
                    else if (bOutType == GameLogic.CT_THREE)
                    {
                        byte card = GameLogic.GetCardValue(bcards[0]);
                        if (card == 1)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_AAA, bGender);
                        }
                        if (card == 2)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_222, bGender);
                        }
                        if (card == 3)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_333, bGender);
                        }
                        if (card == 4)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_444, bGender);
                        }
                        if (card == 5)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_555, bGender);
                        }
                        if (card == 6)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_666, bGender);
                        }
                        if (card == 7)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_777, bGender);
                        }
                        if (card == 8)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_888, bGender);
                        }
                        if (card == 9)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_999, bGender);
                        }
                        if (card == 10)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_111, bGender);
                        }
                        if (card == 11)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_JJJ, bGender);
                        }
                        if (card == 12)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_QQQ, bGender);
                        }
                        if (card == 13)
                        {
                            PlayGameSound(GameSoundTypeEx.CARD_KKK, bGender);
                        }

                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_TWO)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_FOUR_2D, bGender);
                    }
                    else if (bOutType == GameLogic.CT_FOUR_LINE_TAKE_ONE)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_FOUR_2, bGender);
                    }
                    else if (bOutType == GameLogic.CT_SINGLE_LINE)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_LINE, bGender);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_TWO)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_THREE_1D, bGender);
                    }
                    else if (bOutType == GameLogic.CT_THREE_LINE_TAKE_ONE)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_THREE_1, bGender);
                    }
                    else if (bOutType == GameLogic.CT_DOUBLE_LINE)
                    {
                        PlayGameSound(GameSoundTypeEx.CARD_DBL_LINE, bGender);
                    }
                    PlaySound(SoundTypeEx.OUTCARD);
                }
                //set hand cards
                SetHandCardData(_bHandCardData, _bHandCardCount);
                //
                o_play_buttons.SetActive(false);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                //
                ShowTipMsg(false);

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

        void OnPassCardIvk()
        {
            try
            {
                if (_bCurrentUser != GetSelfChair()) return;
                if (_bTurnCardCount == 0)
                {
                    return;
                }



                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_PASS_CARD);
                GameEngine.Instance.Send(packet);
                //
                SetUserPass(GetSelfChair(), true);
                //
                PlayGameSound(GameSoundTypeEx.OPTION_PASS, GameEngine.Instance.MySelf.Gender);

                //
                o_play_buttons.SetActive(false);
                //
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                //
                ShowTipMsg(false);
                //
                ResetHandCardData();
                //
                _bTipsCardsCount = 0;
                //
                o_btn_reset.GetComponent<UIButton>().isEnabled = false;

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

        void OnTipIvk()
        {
            try
            {
                ResetHandCardData();

                tagOutCardResult OutCardResult = new tagOutCardResult();
                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref OutCardResult);

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
                    GameLogic.SearchOutCard(bTempHandCards, bTempHandCardsCount, _bTurnCardData, _bTurnCardCount, ref TipsResult);

                    if (TipsResult.cbCardCount <= 0)
                    {
                        _bTipsCardsCount = 0;
                        Buffer.BlockCopy(_bHandCardData, 0, bTempHandCards, 0, _bHandCardCount);
                        bTempHandCardsCount = _bHandCardCount;
                        TipsResult.cbCardCount = 0;
                        GameLogic.SearchOutCard(bTempHandCards, bTempHandCardsCount, _bTurnCardData, _bTurnCardCount, ref TipsResult);

                    }


                    //保存提示列表
                    Buffer.BlockCopy(TipsResult.cbResultCard, 0, _bTipsCards, _bTipsCardsCount, TipsResult.cbCardCount);
                    _bTipsCardsCount += TipsResult.cbCardCount;

                    //
                    o_hand_cards.GetComponent<UICardControl>().SetShootCard(TipsResult.cbResultCard, TipsResult.cbCardCount);
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

        void OnResetIvk()
        {
            try
            {
                ResetHandCardData();

                _bTipsCardsCount = 0;

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

        #endregion


        #region ##################控件事件#######################

        //扑克控件点击事件
        void SmartSelect()
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

                if (count1 == 2 && _bHandCardCount > 4 && (bLogicValue0 - bLogicValue1) == 1 && bLogicValue0 < 12 && bLogicValue1 < 12)
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
                Debug.Log("smart select error");
            }

        }
        void OnCardClick()
        {

            if (_bCurrentUser == GetSelfChair())
            {
                SmartSelect();
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

        //
        void OnSpeakTimerEnd()
        {
            OnBtnSpeakEndIvk();
        }

        //定时队列处理事件
        /*void OnTimerResp(TimeEvents evt, TimeTok evtContents)
        {
            #region GAME_END
            if (evt == TimeEvents.REG_GAME_END)
            {
                try
                {

                    NPacket packet = evtContents._packet;
                    //
                    GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;
                    //
                    PlayerPrefs.SetInt("UsedServ", 0);
                    //
                    if (_bStart == false) return;

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

                    //
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
                    //timer setting
                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                    //
                    if (_bUserTrustee[GetSelfChair()] == true)
                    {

                        SetUserClock(GetSelfChair(), 5, TimerTypeEx.TIMER_START);
                    }
                    else
                    {
                        SetUserClock(GetSelfChair(), GameLogic.TIME_READY, TimerTypeEx.TIMER_START);
                    }
                    //
                    ShowResultView(true);
                    //
                    ShowInfoBar();
                    //
                    SetLander(GameLogic.NULL_CHAIR);
                    //
                    o_play_buttons.SetActive(false);
                    //
                    o_ready_buttons.SetActive(true);

                }
                catch (Exception ex)
                {
                    UIMsgBox.Instance.Show(true, ex.Message);
                }
            }
            #endregion
        }*/
        //定时处理事件
        void OnTimerEnd()
        {
            try
            {
                switch (_bTimerType)
                {
                    case TimerTypeEx.TIMER_START:
                        {
                            OnConfirmBackOKIvk();
                            break;
                        }
                    case TimerTypeEx.TIMER_LOOK_CATCH:
                        {
                            if (_bCurrentUser != GetSelfChair()) return;

                            OnBtnKanPaiIvk();
                            break;
                        }
                    case TimerTypeEx.TIMER_LAND_SCORE:
                        {
                            if (_bCurrentUser != GetSelfChair()) return;

                            OnBtnGiveupIvk();

                            CheckAutoTimes();
                            break;
                        }
                    case TimerTypeEx.TIMER_OUTCARD:
                        {
                            if (_bCurrentUser != GetSelfChair()) return;

                            if (_bTurnCardCount == 0)
                            {

                                tagOutCardResult res = new tagOutCardResult();
                                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref res);
                                if (res.cbCardCount > 0)
                                {

                                    SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                    //first
                                    SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                    //delete first
                                    GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData, ref _bHandCardCount);

                                    SetHandCardData(_bHandCardData, _bHandCardCount);

                                    //Send
                                    NPacket packet = NPacketPool.GetEnablePacket();
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                    packet.Addbyte(res.cbCardCount);
                                    packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                    GameEngine.Instance.Send(packet);
                                    //
                                    PlaySound(SoundTypeEx.OUTCARD);
                                    //
                                    o_play_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
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
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                    packet.Addbyte(1);
                                    packet.Addbyte(bData[0]);
                                    GameEngine.Instance.Send(packet);
                                    //
                                    PlaySound(SoundTypeEx.OUTCARD);
                                    //
                                    o_play_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                                }
                            }
                            else
                            {
                                //next
                                tagOutCardResult res = new tagOutCardResult();
                                GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref res);
                                if (res.cbCardCount > 0)
                                {
                                    //
                                    bool bres = GameLogic.CompareCard(_bTurnCardData, res.cbResultCard, _bTurnCardCount, res.cbCardCount);
                                    if (bres == false)
                                    {

                                        if (_bCurrentUser != GetSelfChair()) return;
                                        if (_bTurnCardCount == 0)
                                        {
                                            return;
                                        }

                                        NPacket packet1 = NPacketPool.GetEnablePacket();
                                        packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PASS_CARD);
                                        GameEngine.Instance.Send(packet1);
                                        //
                                        SetUserPass(GetSelfChair(), true);
                                        //
                                        PlayGameSound(GameSoundTypeEx.OPTION_PASS, GameEngine.Instance.MySelf.Gender);
                                        //
                                        o_play_buttons.SetActive(false);
                                        //
                                        SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                                        //
                                        ShowTipMsg(false);
                                        //
                                        ResetHandCardData();
                                        //
                                        o_btn_reset.GetComponent<UIButton>().isEnabled = false;

                                        return;

                                    }
                                    //

                                    //
                                    SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                    //first
                                    SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                    //delete first
                                    GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData, ref _bHandCardCount);

                                    SetHandCardData(_bHandCardData, _bHandCardCount);

                                    //Send
                                    NPacket packet = NPacketPool.GetEnablePacket();
                                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                    packet.Addbyte(res.cbCardCount);
                                    packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                    GameEngine.Instance.Send(packet);
                                    //
                                    PlaySound(SoundTypeEx.OUTCARD);
                                    //
                                    o_play_buttons.SetActive(false);
                                    //
                                    SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
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
                                        GameEngine.Instance.Send(packet);
                                        //
                                        SetUserPass(GetSelfChair(), true);
                                        //
                                        PlayGameSound(GameSoundTypeEx.OPTION_PASS, GameEngine.Instance.MySelf.Gender);
                                        //
                                        o_play_buttons.SetActive(false);
                                        //
                                        SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
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
                    case TimerTypeEx.TIMER_MOSTCARD:
                        {
                            if (_bMostUser == GameLogic.NULL_CHAIR) return;

                            byte bCurrUser = _bMostUser;
                            _bMostUser = GameLogic.NULL_CHAIR;
                            //
                            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
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
                                    GameLogic.SearchOutCard(_bHandCardData, _bHandCardCount, _bTurnCardData, _bTurnCardCount, ref res);
                                    if (res.cbCardCount > 0)
                                    {

                                        SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
                                        //first
                                        SetOutCardData(GetSelfChair(), res.cbResultCard, res.cbCardCount);

                                        //delete first
                                        GameLogic.RemoveCard(res.cbResultCard, res.cbCardCount, ref _bHandCardData, ref _bHandCardCount);

                                        SetHandCardData(_bHandCardData, _bHandCardCount);

                                        //Send
                                        NPacket packet = NPacketPool.GetEnablePacket();
                                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                        packet.Addbyte(res.cbCardCount);
                                        packet.AddBytes(res.cbResultCard, res.cbCardCount);
                                        GameEngine.Instance.Send(packet);
                                        //
                                        PlaySound(SoundTypeEx.OUTCARD);
                                        //
                                        o_play_buttons.SetActive(false);

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
                                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmdEx.SUB_C_OUT_CARD);
                                        packet.Addbyte(1);
                                        packet.Addbyte(bData[0]);
                                        GameEngine.Instance.Send(packet);
                                        //
                                        PlaySound(SoundTypeEx.OUTCARD);
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
                    default: break;
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void SetTimerAfterMost()
        {
            SetUserClock(_bCurrentUser, GameLogic.TIME_OUT_CARD, TimerTypeEx.TIMER_OUTCARD);
        }

        //扑克控件选牌事件
        void OnMoveSelect()
        {
            if (_bCurrentUser == GetSelfChair())
            {
                SmartSelect();
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
        void OnMoveUp()
        {
            OnOutCardIvk();
        }

        //扑克控件向下划牌事件
        void OnMoveDown()
        {

        }

        #endregion


        #region ##################UI 控制#######################


        void SetHandCardData(byte[] cards, byte count)
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
        void ArrayHandCardData(byte[] cards, byte count)
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
        void ResetHandCardData()
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

        void SetOutCardData(byte chair, byte[] cards, byte count)
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
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }
        void SetBackCardData(byte[] cards, byte count)
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
        void CheckAutoTimes()
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
        void SetUserClock(byte chair, uint time, TimerTypeEx timertype)
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
                    o_clock[viewId].GetComponent<UIClock>().SetTimer(time * 1000);
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        void SetUserPass(byte chair, bool bshow)
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
        void SetCallScore(byte chair, ushort bScore)
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
                    o_score[viewId].SetActive(true);

                    if (bScore == CallType.BACK_CATCH)
                    {
                        o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_meizhua";
                    }
                    else if (bScore == CallType.LOOK_CARD)
                    {
                        o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_kp";
                    }
                    else if (bScore == CallType.MING_CATCH)
                    {
                        o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_mz";
                    }
                    else if (bScore == CallType.CALL_SCORE)
                    {
                        if (_bLander == chair)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_l";
                        }
                        else
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_dao";
                        }
                    }
                    else if (bScore == CallType.CALL_TWO_SCORE)
                    {
                        if (_bLander == chair)
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_wo";
                        }
                        else
                        {
                            o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_lei";
                        }
                    }
                    else if (bScore == CallType.PASS_ACTION)
                    {
                        o_score[viewId].GetComponent<UISprite>().spriteName = "btn_desc_giveup";
                    }
                    else
                    {
                        o_score[viewId].GetComponent<UISprite>().spriteName = "blank";
                    }

                }

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void SetUserReady(byte chair, bool bshow)
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
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }
        void SetLander(byte bChair)
        {
            try
            {

                _bLander = bChair;
                if (_bLander == GameLogic.NULL_CHAIR || _bLander >= GameLogic.GAME_PLAYER_NUM)
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

        void UpdateUserView()
        {
            try
            {
                if (_bStart == false) return;


                for (uint i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    byte bViewId = ChairToView((byte)i);
                    PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {
                        string strfaceName = "";
                        if (_bUserTrustee[i] == true)
                        {
                            strfaceName = "face_robot";
                        }
                        else
                        {
                            if (userdata.UserState == (byte)UserState.US_OFFLINE)
                            {
                                strfaceName = "face_offline";
                            }
                        }
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
                        //face

                        if (strfaceName == "face_robot")
                        {

                            o_head_face[bViewId].GetComponent<UIFace>().ShowFace(-2, (int)userdata.VipLevel);
                        }
                        else if (strfaceName == "face_offline")
                        {

                            o_head_face[bViewId].GetComponent<UIFace>().ShowFace(-3, (int)userdata.VipLevel);
                        }
                        else
                        {

                            o_head_face[bViewId].GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
                        }
                        //
                        if (userdata.UserState == (byte)UserState.US_READY)
                        {
                            SetUserReady((byte)i, true);
                        }
                        else
                        {
                            SetUserReady((byte)i, false);
                        }
                    }
                    else
                    {
                        //nick
                        o_head_nick[bViewId].GetComponent<UILabel>().text = "";
                        //face
                        o_head_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
                        //p
                        SetUserReady((byte)i, false);
                    }


                }

                ShowInfoBar();

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }

        }

        void ShowInfoBar()
        {
            if (GameEngine.Instance.MySelf != null)
            {
                GameObject.Find("scene_game_ex/dlg_bottom_bar/lbl_money").GetComponent<UILabel>().text = GameEngine.Instance.MySelf.Money.ToString();
                /*GameEngine.Instance.MySelf.Exp = (uint)GameEngine.Instance.MySelf.Self.lExperience;
                GameEngine.Instance.MySelf.GameScore = (uint)GameEngine.Instance.MySelf.Self.lScore;
                o_btn_speak_count.GetComponent<UILabel>().text = "x" + GameEngine.Instance.MySelf.SmallSpeakerCount.ToString();*/

            }
        }

        void SetBaseScore(int nscore)
        {
            GameObject.Find("scene_game_ex/dlg_bottom_bar/lbl_base_score").GetComponent<UILabel>().text = nscore.ToString();


        }

        void DispatchCard(int nSendCount)
        {
            //
            AppendHandCard(new byte[1] { _bHandCardData[nSendCount] }, 1);

            //
            for (int i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                if (i != GetSelfChair())
                {
                    AppendOtherHandCard((byte)i, new byte[1] { _bOthersCardData[i][nSendCount] }, 1);
                }

                byte bViewID = ChairToView((byte)i);
                o_others_count[bViewID].GetComponent<UILabel>().text = nSendCount.ToString();
            }

            PlaySound(SoundTypeEx.SENDCARD);

        }
        void AppendHandCard(byte[] cards, byte cardcount)
        {

            o_hand_cards.GetComponent<UICardControl>().AppendHandCard(cards, cardcount);
        }
        void AppendOtherHandCard(byte bchair, byte[] cards, byte cardcount)
        {

            byte bviewid = ChairToView(bchair);
            if (o_others[bviewid].active == false)
            {
                o_others[bviewid].SetActive(true);
            }
            o_others[bviewid].GetComponentInChildren<UICardControl>().AppendCard(cards, cardcount);
            o_others[bviewid].GetComponentInChildren<UILabel>().text = _bOthersCardCount[bchair].ToString();
        }
        void DispatchCardFinish()
        {
            _bSendCard = false;

            _nSendCardCount = 0;

            //底牌
            o_current_time.SetActive(false);
            o_back_cards.GetComponent<UICardControl>().ClearCards();
            o_back_cards.GetComponent<UICardControl>().AppendCard(new byte[] { 0, 0, 0 }, 3);

            //
            Array.Clear(_nEndUserScore, 0, _nEndUserScore.Length);
            Array.Clear(_bEndCardCount, 0, 3);
            Array.Clear(_bEndCardData, 0, 54);

            //
            SetHandCardData(_bHandCardData, 17);
            //
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                _bOthersCardCount[i] = 17;
                SetPlayerCardCount(i, _bOthersCardCount[i]);
            }

            //给庄家设置时钟
            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
            if (_bCurrentUser == GetSelfChair() && _bUserTrustee[GetSelfChair()] == true)
            {
                SetUserClock(_bCurrentUser, 1, TimerTypeEx.TIMER_LOOK_CATCH);
            }
            else
            {
                SetUserClock(_bCurrentUser, GameLogic.TIME_LAND_SCORE, TimerTypeEx.TIMER_LOOK_CATCH);
            }

            //闷抓按钮显示
            if (_bCurrentUser == GetSelfChair())
            {
                HideAllButtons();
                o_buttons_menzhua.SetActive(true);
            }

            //闷抓
            PlaySound(SoundTypeEx.START);

        }
        void DispatchCardStart()
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

        void ShowUserInfo(byte bchair, bool bshow)
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
            byte wViewChairID = (byte)((ChairID + GameLogic.GAME_PLAYER_NUM - /*GameEngine.Instance.MySelf.Self.ChairID*/GameEngine.Instance.MySelf.DeskStation));
            return (byte)(wViewChairID % GameLogic.GAME_PLAYER_NUM);
        }
        byte GetSelfChair()
        {
            return (byte)GameEngine.Instance.MySelf.DeskStation;//GameEngine.Instance.MySelf.Self.DeskStation;
        }

        bool VerdictOutCard()
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
        void SetPlayerCardCount(byte bchair, byte bcardcount)
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

        void SetTotalTimes(ushort btime)
        {
            GameObject.Find("scene_game_ex/dlg_bottom_bar/lbl_rate").GetComponent<UILabel>().text = btime.ToString();
        }

		private void ShowResultView()
		{
			ShowResultView(true);
		}

        void ShowResultView(bool bshow)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME_EX) return;
            o_result.SetActive(bshow);

            //金币限制检测
            /*
           if(GameEngine.Instance.MySelf.Self.lScore <20*_nBaseScore)
           {
                UIMsgBox.Instance.Show(true,"您的乐豆不足,不能继续游戏!");
                OnConfirmBackOKIvk();
                _bStart = false;
                return;
           }
            */
            if (bshow)
            {
                //
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
                //timer setting
                SetUserClock(GameLogic.NULL_CHAIR, 0, TimerTypeEx.TIMER_NULL);
                //
                if (_bUserTrustee[GetSelfChair()] == true)
                {

                    SetUserClock(GetSelfChair(), 5, TimerTypeEx.TIMER_START);
                }
                else
                {
                    SetUserClock(GetSelfChair(), GameLogic.TIME_READY, TimerTypeEx.TIMER_START);
                }
                //
                ShowInfoBar();
                //
                SetLander(GameLogic.NULL_CHAIR);
                //
                o_play_buttons.SetActive(false);
                //
                o_ready_buttons.SetActive(true);

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

                for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
                {
                    //tagUserData ud = GameEngine.Instance.GetTableUserItem(i);
                    //if(ud!=null)
                    string strUName = _TableUserName[i];
                    if (strUName != "")
                    {
                        byte bViewId = ChairToView(i);
                        o_result_score[bViewId].GetComponent<UILabelEx>().SetChangeNumber(_nEndUserScore[i], _nEndUserScore[i]);
                        GameObject.Find("scene_game_ex/dlg_result/lbl_bs_" + bViewId.ToString()).GetComponent<UILabel>().text = _nEndUserExp[i].ToString();
                        if (_nEndUserScore[i] > 0)
                        {
                            GameObject.Find("scene_game_ex/dlg_result/sp_win_" + bViewId.ToString()).GetComponent<UISprite>().spriteName = "win";
                        }
                        else
                        {
                            GameObject.Find("scene_game_ex/dlg_result/sp_win_" + bViewId.ToString()).GetComponent<UISprite>().spriteName = "lose";
                        }

                        GameObject.Find("scene_game_ex/dlg_result/lbl_user_" + bViewId.ToString()).GetComponent<UILabel>().text = strUName;
                    }
                }
                GameObject.Find("scene_game_ex/dlg_result/lbl_rocket").GetComponent<UILabel>().text = _bRocketTimes.ToString();
                GameObject.Find("scene_game_ex/dlg_result/lbl_bomb").GetComponent<UILabel>().text = _bBombTimes.ToString();

                //
                byte bUser1 = (byte)((_bLander + 1) % GameLogic.GAME_PLAYER_NUM);
                byte bUser2 = (byte)((_bLander + 2) % GameLogic.GAME_PLAYER_NUM);

                if (_bOthersCardCount[bUser1] == 17 && _bOthersCardCount[bUser2] == 17)
                {
                    _wTotalTimes = (ushort)(_wTotalTimes * 2);
                    SetTotalTimes(_wTotalTimes);
                    GameObject.Find("scene_game_ex/dlg_result/lbl_spring").GetComponent<UILabel>().text = "1";
                }
                else
                {
                    GameObject.Find("scene_game_ex/dlg_result/lbl_spring").GetComponent<UILabel>().text = "0";
                }

                GameObject.Find("scene_game_ex/dlg_result/lbl_total").GetComponent<UILabel>().text = _wTotalTimes.ToString();
                PlaySound(SoundTypeEx.END);
                Invoke("CloseResultView", 30);
                SetUserClock(GameLogic.NULL_CHAIR, 30, TimerTypeEx.TIMER_NULL);

            }

        }
        void CloseResultView()
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME_EX) return;
            ////金币限制检测
            //if (GameEngine.Instance.MySelf.Money < 20 * _nBaseScore)
            //{
            //    UIInfoBox.Instance.Show(true, "您的乐豆不足,不能继续游戏!");
            //    Invoke("OnConfirmBackOKIvk", 2.0f);
            //    return;
            //}
            //
            SetPlayerCardCount(GameLogic.NULL_CHAIR, 0);
            //
            SetOutCardData(GameLogic.NULL_CHAIR, null, 0);
            //
            SetBackCardData(null, 0);
            //
            //SetHandCardData(null,0);
            //
            o_result.SetActive(false);

        }
        //
        void ShowTipMsg(bool bshow)
        {
            o_tip_bar.SetActive(bshow);
        }
        void PlaySound(SoundTypeEx sound)
        {
            float fvol = NGUITools.soundVolume;
            NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
        }
        void PlayGameSound(GameSoundTypeEx sound, byte bGender)
        {
            float fvol = NGUITools.soundVolume;
            if (bGender == 0)
            {

                NGUITools.PlaySound(_WomanSound[(int)sound], fvol, 1);
            }
            else
            {

                NGUITools.PlaySound(_ManSound[(int)sound], fvol, 1);
            }
        }
        void ShowUserSpeak(uint uid)
        {
            /*
            byte bchairID = (byte)GameEngine.Instance.MySelf.UserIdToChairId(uid);
            byte bViewID = ChairToView(bchairID);
            if (bchairID != GetSelfChair())
            {
                o_user_speak[bViewID].GetComponent<UISpeak>().Play("scene_game_ex", "OnSpeakPlay", uid);
            }*/

        }
        void OnRecordSpeakFinish(string strSpeak)
        {
           /* //本地预播放
            byte bViewID = ChairToView(GetSelfChair());
            o_user_speak[bViewID].GetComponent<UISpeak>().PlayLocal("scene_game_ex", "OnSpeakPlay", GameEngine.Instance.MySelf.ID);

            //上传网络
            string strFile = Application.persistentDataPath + "/" + strSpeak;
            StartCoroutine(UpLoadSpeak(strFile));*/

        }
        void OnSpeakPlay(string str)
        {
            /*string[] strs = str.Split("`".ToCharArray());

            int nTime = Convert.ToInt32(strs[0]);
            uint Uid = Convert.ToUInt32(strs[1]);

            byte bViewID = ChairToView((byte)GameEngine.Instance.MySelf.UserIdToChairId(Uid));

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
        void ShowNotice(string strMsg)
        {
            //UIMsgBox.Instance.Show(true, strMsg);

        }
        void SetUserTuoGuan(byte bChair, bool bAuto)
        {
            byte bViewID = ChairToView(bChair);


            if (bAuto)
            {

                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(bChair);
                o_head_face[bViewID].GetComponent<UIFace>().ShowFace(-2, (int)ud.VipLevel);
            }
            else
            {

                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(bChair);
                o_head_face[bViewID].GetComponent<UIFace>().ShowFace((int)ud.HeadID, (int)ud.VipLevel);
            }
        }

        void ClearAllInfo()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER_NUM; i++)
            {
                o_info[i].SetActive(false);
                o_chat[i].SetActive(false);

            }
        }

        void OnClearInfoIvk()
        {
            ClearAllInfo();
            OnResetIvk();
        }

        void OnResultCloseIvk()
        {
            CloseResultView();
        }

        void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        bool CheckScoreLimit()
        {
            //金币限制检测
            /*
            int nLimit = 0;
            if(GameEngine.Instance.MySelf.ServerUsed.StationID.ToString().EndsWith("39")==true)
            {
                nLimit = 10000;
            }
            else
            {
                nLimit = 20*_lCellScore;
            }
            */
            if (GameEngine.Instance.MySelf.Money < 20 * _nBaseScore)
            {
                UIMsgBox.Instance.Show(true, "您的乐豆不足,不能继续游戏!");
                Invoke("OnConfirmBackOKIvk", 1.0f);
                _bStart = false;
                return false;
            }
            return true;
        }
        #endregion

    }
}