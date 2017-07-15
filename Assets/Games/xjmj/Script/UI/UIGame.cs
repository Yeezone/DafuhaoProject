using UnityEngine;
using System.Collections;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.XZMJ
{

    #region ##################结构定义#######################
    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_READY = 1,
        TIMER_PLAY = 2,
        TIMER_OPTION = 3,
        TIMER_LACK = 4

    };
    public enum SoundType
    {
        READY = 0,
        START = 1,
        SENDCARD = 2,
        CHIP = 3,
        WIN = 4,
        LOSE = 5,
        OUTCARD = 6,
        LACK = 7,
        HU = 8
    };
    public enum GameSoundType
    {
        //条
        T1 = 0,
        T2 = 1,
        T3 = 2,
        T4 = 3,
        T5 = 4,
        T6 = 5,
        T7 = 6,
        T8 = 7,
        T9 = 8,

        //饼
        B1 = 9,
        B2 = 10,
        B3 = 11,
        B4 = 12,
        B5 = 13,
        B6 = 14,
        B7 = 15,
        B8 = 16,
        B9 = 17,

        //万
        W1 = 18,
        W2 = 19,
        W3 = 20,
        W4 = 21,
        W5 = 22,
        W6 = 23,
        W7 = 24,
        W8 = 25,
        W9 = 26,

        //东西南北
        DF = 27,
        XF = 28,
        NF = 29,
        BF = 30,

        //中，发，白
        HZ = 31,
        FC = 32,
        BB = 33,

        //吃，碰，杠，听，胡

        CHI = 34,
        PENG = 35,
        GANG = 36,
        TING = 37,
        HU = 38

    };

    public enum TimerDelay
    {
        NULL = 0,
        PLAY = 20,
        READY = 20
    };

    public enum UserAction
    {
        NULL = 0,
        LACK = 1,
        PENG = 2,
        GANG = 3,
        HU = 4,
        GF = 5,
        XY = 6,
		ZM = 7
    };
    #endregion


    public class UIGame : MonoBehaviour
    {

        #region ##################变量定义#######################

        //通用控件
        GameObject o_speak_timer = null;
        GameObject o_btn_speak = null;
        GameObject o_btn_speak_count = null;




        GameObject[] o_player_chat = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_money = new GameObject[GameLogic.GAME_PLAYER];


        GameObject[] o_player_face = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_banker = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_option = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_action = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_hu = new GameObject[GameLogic.GAME_PLAYER];

        //GameObject[] o_user_speak = new GameObject[GameLogic.GAME_PLAYER];

        //信息框
        GameObject[] o_info = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_id = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lvl = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_score = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_win = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_lose = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_info_run = new GameObject[GameLogic.GAME_PLAYER];

        //结算框
        GameObject o_result = null;
        GameObject[] o_result_score = new GameObject[GameLogic.GAME_PLAYER];
		GameObject[] o_result_gangScore = new GameObject[GameLogic.GAME_PLAYER];
		GameObject[] o_result_zm=new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_ct = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_bk = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_nick = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_result_bean = new GameObject[GameLogic.GAME_PLAYER];
		GameObject sp_title;
        GameObject desc_spwinChip_lbl;
		GameObject desc_spwin;
		GameObject desc_spwin_label;

        //开始按钮
        GameObject o_ready_buttons = null;
        GameObject o_btn_ready = null;
        GameObject o_btn_quit = null;


        GameObject o_select_buttons = null;
        GameObject o_btn_select_wan = null;
        GameObject o_btn_select_bing = null;
        GameObject o_btn_select_tiao = null;
        GameObject o_lack_clock = null;

        //麻将牌
        GameObject o_Selfhand_cards = null;
        GameObject o_Selflie_cards = null;
        GameObject o_Selfcatch_cards = null;
        GameObject o_Selfdesk_cards = null;

        GameObject o_Westhand_cards = null;
        GameObject o_Westlie_cards = null;
        GameObject o_Westcatch_cards = null;
        GameObject o_Westdesk_cards = null;

        GameObject o_Easthand_cards = null;
        GameObject o_Eastlie_cards = null;
        GameObject o_Eastcatch_cards = null;
        GameObject o_Eastdesk_cards = null;

        GameObject o_Northhand_cards = null;
        GameObject o_Northlie_cards = null;
        GameObject o_Northcatch_cards = null;
        GameObject o_Northdesk_cards = null;

        //吃，碰，杠，听，胡，弃
        GameObject o_operate_buttons = null;
        GameObject o_operate_eat_buttons = null;
        GameObject o_operate_touch_buttons = null;
        GameObject o_operate_bridge_buttons = null;
        GameObject o_operate_giveup_buttons = null;
        GameObject o_operate_beard_buttons = null;

        GameObject ctr_operateEat_cards1 = null;
        GameObject ctr_operateEat_cards2 = null;
        GameObject ctr_operateEat_cards3 = null;

        //时钟指向
        GameObject o_player_clock = null;
        GameObject o_up = null;
        GameObject o_down = null;
        GameObject o_left = null;
        GameObject o_right = null;
        GameObject o_card_sum = null;

        //系统菜单
        GameObject o_sys_dlg = null;
        GameObject o_btn_sys = null;
        GameObject o_Tuoguan = null;


		// 声音按钮
        public static GameObject btn_chat_disabled = null;
        public static GameObject btn_chat = null;

		//游戏说明界面
		public GameObject o_help = null;

		//是否在手机上运行
		public bool Phone;

        //通用数据
        static bool _bStart = false;
        static TimerType _bTimerType = TimerType.TIMER_NULL;

        //底注
        static int _nBaseScore = 0;

        //游戏状态
        static byte[] _bLackCardType = new byte[GameLogic.GAME_PLAYER];

        //结算数据
        static int[] _nEndUserScore = new int[GameLogic.GAME_PLAYER];
        static int[] _nEndUserExp = new int[GameLogic.GAME_PLAYER];
        static bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER];
		static int[] _nEndGangScore = new int[GameLogic.GAME_PLAYER];

        static int _nQuitDelay = 0;
        static bool _bReqQuit = false;
        static int _nInfoTickCount = 0;

		static byte leftUserCount;
        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[40];
        public AudioClip[] _ManSound = new AudioClip[40];


        //骰子点数
        int _wSiceCount = 0;
        //庄家用户
        short _wBankerUser = GameLogic.NULL_CHAIR;
        //当前用户
        short _wCurrentUser = GameLogic.NULL_CHAIR;
        //用户动作
        byte _cbUserAction = 0;


        //堆立变量

        //堆立头部
        short _wHeapHand = 0;
        //堆立尾部
        short _wHeapTail = 0;

        //堆牌信息
        byte[,] _cbHeapCardInfo = new byte[4, 2];


        //扑克变量
        //剩余数目
        short _cbLeftCardCount = 0;
        //手中扑克
        byte[] _cbCardIndex = new byte[GameLogic.MAX_INDEX];
        //显示扑克
        byte[,] _cbCardlieIndex = new byte[GameLogic.GAME_PLAYER, GameLogic.MAX_INDEX];
        //显示数目
        byte[] _cbCardIlieCount = new byte[GameLogic.GAME_PLAYER];
        //暗杠扑克
        byte[,] _cbCardHiddenIndex = new byte[GameLogic.GAME_PLAYER, GameLogic.MAX_INDEX];

        //组合扑克
        //组合数目
        byte[] _cbWeaveCount = new byte[GameLogic.GAME_PLAYER];
        //组合扑克
        tagWeaveItem[,] _WeaveItemArray = new tagWeaveItem[GameLogic.GAME_PLAYER, 4];

        //出牌信息
        //出牌用户
        short _wOutCardUser = GameLogic.NULL_CHAIR;
        //出牌扑克
        byte _cbOutCardData = 0;
        //丢弃数目
        byte[] _cbDiscardCount = new byte[GameLogic.GAME_PLAYER];
        //丢弃记录
        byte[,] _cbDiscardCard = new byte[GameLogic.GAME_PLAYER, 60];
        //自己出牌(南)
        byte[] _cbSelfDeskCard = new byte[60];
        //西家出牌
        byte[] _cbWestDeskCard = new byte[60];
        //北家出牌
        byte[] _cbNorthDeskCard = new byte[60];
        //东家出牌
        byte[] _cbEastDeskCard = new byte[60];

        //操作代码
        byte _cbOperateCode = 0;
        //操作扑克
        byte _cbOperateCard = 0;
        //
        byte[] _cbWeaveType = new byte[3];
        //用户托管
        bool[] _bTrustee = new bool[GameLogic.GAME_PLAYER];
		//是否自摸
		bool[] _bTouch=new bool[GameLogic.GAME_PLAYER];
		//是否停牌
		bool[] _bListen=new bool[GameLogic.GAME_PLAYER];
        //
        byte _bTrustee_Card = 0;
        //
        bool _bHearStatus = false;


		//选缺显示
		bool showLack;

		//打缺门提示
		GameObject prompt_sprite;

		//流局
		Transform liuju_sp;
		TweenScale liujuTween;

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
                o_btn_speak = GameObject.Find("scene_game/dlg_bottom_bar/btn_speak");
                o_btn_speak_count = GameObject.Find("scene_game/dlg_bottom_bar/btn_speak/lbl_count");

				btn_chat_disabled = GameObject.Find ("scene_game/dlg_bottom_bar/btn_chat/disabled");
				btn_chat = GameObject.Find("scene_game/dlg_bottom_bar/btn_chat");

				prompt_sprite = GameObject.Find ("scene_game/prompt_sprite");

				liuju_sp = GameObject.Find("scene_game/liuju_sp").GetComponent<Transform>();
				liujuTween = liuju_sp.gameObject.GetComponent<TweenScale>();

				sp_title = GameObject.Find("scene_game/dlg_result/sp_title");
				desc_spwin = GameObject.Find("scene_game/dlg_result/desc_spwin");
                desc_spwinChip_lbl = GameObject.Find("scene_game/dlg_result/desc_spwin_w");
				desc_spwin_label = GameObject.Find("scene_game/dlg_result/desc_spwin_label");
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_option[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_option");
                    o_player_chat[i] = GameObject.Find("scene_game/dlg_chat_msg_" + i.ToString());
                    o_player_face[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_user_face");


                    o_player_nick[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/lbl_nick");
                    o_player_banker[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_banker");
                    o_player_action[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_action");
                    o_player_hu[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/sp_hu");
                    o_player_money[i] = GameObject.Find("scene_game/dlg_player_" + i.ToString() + "/ctr_money");
                    //o_user_speak[i] = GameObject.Find("scene_game/ctr_speak_" + i.ToString());

                    o_info[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString());
                    o_info_nick[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_nick");
                    o_info_lvl[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lvl");
                    o_info_id[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_id");
                    o_info_score[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_score");
                    o_info_win[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_win");
                    o_info_lose[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_lose");
                    o_info_run[i] = GameObject.Find("scene_game/dlg_info_" + i.ToString() + "/lbl_run");

                    o_result_score[i] = GameObject.Find("scene_game/dlg_result/lbl_score_" + i.ToString());
					o_result_gangScore[i] = GameObject.Find("scene_game/dlg_result/lbl_bs_" + i.ToString());
					o_result_zm[i]=GameObject.Find("scene_game/dlg_result/zm_"+i.ToString());
                    o_result_ct[i] = GameObject.Find("scene_game/dlg_result/sp_ct_" + i.ToString());
                    o_result_bk[i] = GameObject.Find("scene_game/dlg_result/sp_bk_" + i.ToString());
                    o_result_nick[i] = GameObject.Find("scene_game/dlg_result/lbl_user_" + i.ToString());
                    o_result_bean[i] = GameObject.Find("scene_game/dlg_result/sp_bean_" + i.ToString());


                }

                //准备
                o_ready_buttons = GameObject.Find("scene_game/dlg_ready_buttons");
                o_btn_ready = GameObject.Find("scene_game/dlg_ready_buttons/btn_ready");
                o_btn_quit = GameObject.Find("scene_game/dlg_ready_buttons/btn_quit");
                o_result = GameObject.Find("scene_game/dlg_result");




                //游戏
                o_select_buttons = GameObject.Find("scene_game/dlg_select_buttons");
                o_btn_select_wan = GameObject.Find("scene_game/dlg_select_buttons/btn_wan");
                o_btn_select_bing = GameObject.Find("scene_game/dlg_select_buttons/btn_bing");
                o_btn_select_tiao = GameObject.Find("scene_game/dlg_select_buttons/btn_tiao");
                o_lack_clock = GameObject.Find("scene_game/dlg_select_buttons/ctr_clock");

                o_Selfhand_cards = GameObject.Find("scene_game/dlg_cards/ctr_selfhand_cards");
                o_Selflie_cards = GameObject.Find("scene_game/dlg_cards/ctr_selflie_cards");
                o_Selfcatch_cards = GameObject.Find("scene_game/dlg_cards/ctr_selfcatch_cards");
                o_Selfdesk_cards = GameObject.Find("scene_game/dlg_cards/ctr_selfdesk_cards");

                o_Westhand_cards = GameObject.Find("scene_game/dlg_cards/ctr_westhand_cards");
                o_Westlie_cards = GameObject.Find("scene_game/dlg_cards/ctr_westlie_cards");
                o_Westcatch_cards = GameObject.Find("scene_game/dlg_cards/ctr_westcatch_cards");
                o_Westdesk_cards = GameObject.Find("scene_game/dlg_cards/ctr_westdesk_cards");

                o_Easthand_cards = GameObject.Find("scene_game/dlg_cards/ctr_easthand_cards");
                o_Eastlie_cards = GameObject.Find("scene_game/dlg_cards/ctr_eastlie_cards");
                o_Eastcatch_cards = GameObject.Find("scene_game/dlg_cards/ctr_eastcatch_cards");
                o_Eastdesk_cards = GameObject.Find("scene_game/dlg_cards/ctr_eastdesk_cards");

                o_Northhand_cards = GameObject.Find("scene_game/dlg_cards/ctr_northhand_cards");
                o_Northlie_cards = GameObject.Find("scene_game/dlg_cards/ctr_northlie_cards");
                o_Northcatch_cards = GameObject.Find("scene_game/dlg_cards/ctr_northcatch_cards");
                o_Northdesk_cards = GameObject.Find("scene_game/dlg_cards/ctr_northdesk_cards");

                ctr_operateEat_cards1 = GameObject.Find("scene_game/dlg_cards/ctr_operateEat1_cards");
                ctr_operateEat_cards2 = GameObject.Find("scene_game/dlg_cards/ctr_operateEat2_cards");
                ctr_operateEat_cards3 = GameObject.Find("scene_game/dlg_cards/ctr_operateEat3_cards");

                o_operate_buttons = GameObject.Find("scene_game/dlg_operate_buttons");
                o_operate_eat_buttons = GameObject.Find("scene_game/dlg_operate_buttons/btn_eat");
                o_operate_touch_buttons = GameObject.Find("scene_game/dlg_operate_buttons/btn_peng");
                o_operate_bridge_buttons = GameObject.Find("scene_game/dlg_operate_buttons/btn_gang");
                o_operate_giveup_buttons = GameObject.Find("scene_game/dlg_operate_buttons/btn_giveup");
                o_operate_beard_buttons = GameObject.Find("scene_game/dlg_operate_buttons/btn_hu");


                //时钟
                o_player_clock = GameObject.Find("scene_game/dlg_clock/ctr_clock");
                o_up = GameObject.Find("scene_game/dlg_clock/dlg_up");
                o_down = GameObject.Find("scene_game/dlg_clock/dlg_down");
                o_left = GameObject.Find("scene_game/dlg_clock/dlg_left");
                o_right = GameObject.Find("scene_game/dlg_clock/dlg_right");
                o_card_sum = GameObject.Find("scene_game/dlg_clock/lbl_card_sum");

                //系统
                o_btn_sys = GameObject.Find("scene_game/btn_sys");
                o_sys_dlg = GameObject.Find("scene_game/dlg_bottom_bar");

                o_Tuoguan = GameObject.Find("scene_game/dlg_tuoguan");

            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
		//初始化
        void InitGameView()
        {

            //------------------Data------------------------
            _bStart = false;
            _bTimerType = TimerType.TIMER_READY;
            _nInfoTickCount = Environment.TickCount;
            _nQuitDelay = 0;
            _bReqQuit = false;
            _nInfoTickCount = Environment.TickCount;

            //当前用户和庄家
            _wBankerUser = GameLogic.NULL_CHAIR;
            _wCurrentUser = GameLogic.NULL_CHAIR;

            //堆立变量
            _wHeapHand = 0;
            _wHeapTail = 0;
            Array.Clear(_cbHeapCardInfo, 0, _cbHeapCardInfo.Length);

            //出牌信息
            _cbOutCardData = 0;
			_wOutCardUser = GameLogic.NULL_CHAIR;
            Array.Clear(_cbDiscardCard, 0, _cbDiscardCard.Length);
            Array.Clear(_cbDiscardCount, 0, _cbDiscardCount.Length);

            //组合扑克
            Array.Clear(_cbWeaveCount, 0, _cbWeaveCount.Length);
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _WeaveItemArray[i, j] = new tagWeaveItem();
                }
            }

            //扑克变量
            _cbLeftCardCount = 0;
            Array.Clear(_cbCardIndex, 0, _cbCardIndex.Length);
            Array.Clear(_cbCardlieIndex, 0, _cbCardlieIndex.Length);
            Array.Clear(_cbCardIlieCount, 0, _cbCardIlieCount.Length);
            Array.Clear(_cbWeaveType, 0, _cbWeaveType.Length);


            //托管
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bTrustee[i] = false;
                _bLackCardType[i] = 255;
            }
			//声音禁用按钮
			btn_chat.GetComponent<UIButton>().isEnabled=true;

            //清桌
            ClearAllCards();

			//提示打缺门牌
			prompt_sprite.SetActive(false);


            //--------------------UI-----------------------


            o_btn_quit.SetActive(false);
            o_speak_timer.SetActive(false);
			o_help.SetActive(false);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {

                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);
                o_player_money[i].SetActive(false);


                o_player_face[i].GetComponent<UIFace>().ShowFace(-1, -1);
                o_player_nick[i].GetComponent<UILabel>().text = "";
                o_player_banker[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_action[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_option[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_hu[i].GetComponent<UISprite>().spriteName = "blank";

				o_result_zm[i].GetComponent<UISprite>().spriteName= "blank";

                o_info[i].SetActive(false);
                o_info_nick[i].GetComponent<UILabel>().text = "";
                o_info_lvl[i].GetComponent<UILabel>().text = "";
                o_info_id[i].GetComponent<UILabel>().text = "";
                o_info_score[i].GetComponent<UILabel>().text = "";
                o_info_win[i].GetComponent<UILabel>().text = "";
                o_info_lose[i].GetComponent<UILabel>().text = "";
                o_info_run[i].GetComponent<UILabel>().text = "";


                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;

            }

            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_sys_dlg.SetActive(true);
            o_select_buttons.SetActive(false);

            o_player_clock.SetActive(false);
            o_up.SetActive(false);
            o_down.SetActive(false);
            o_left.SetActive(false);
            o_right.SetActive(false);
            o_card_sum.GetComponent<UILabel>().text = "";

            o_Tuoguan.SetActive(false);


            //操作控制
            o_operate_buttons.SetActive(false);              //操作条
            o_operate_eat_buttons.SetActive(false);          //吃
            o_operate_touch_buttons.SetActive(false);        //碰
            o_operate_bridge_buttons.SetActive(false);       //杠
            o_operate_giveup_buttons.SetActive(false);       //弃
            o_operate_beard_buttons.SetActive(false);        //胡

            ctr_operateEat_cards1.SetActive(false);
            ctr_operateEat_cards2.SetActive(false);
            ctr_operateEat_cards3.SetActive(false);




        }
		//重置
        void ResetGameView()
        {

            //------------------Data------------------------
            _bTimerType = TimerType.TIMER_READY;
            _nInfoTickCount = Environment.TickCount;
            _nQuitDelay = 0;
            _bReqQuit = false;
            _nInfoTickCount = Environment.TickCount;

            //当前用户和庄家
            _wBankerUser = GameLogic.NULL_CHAIR;
            _wCurrentUser = GameLogic.NULL_CHAIR;

            //堆立变量
            _wHeapHand = 0;
            _wHeapTail = 0;
            Array.Clear(_cbHeapCardInfo, 0, _cbHeapCardInfo.Length);

            //出牌信息
            _cbOutCardData = 0;
			_wOutCardUser = GameLogic.NULL_CHAIR;
            Array.Clear(_cbDiscardCard, 0, _cbDiscardCard.Length);
            Array.Clear(_cbDiscardCount, 0, _cbDiscardCount.Length);

            //组合扑克
            Array.Clear(_cbWeaveCount, 0, _cbWeaveCount.Length);
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _WeaveItemArray[i, j] = new tagWeaveItem();
                }
            }

            //扑克变量
            _cbLeftCardCount = 0;
            Array.Clear(_cbCardIndex, 0, _cbCardIndex.Length);
            Array.Clear(_cbCardlieIndex, 0, _cbCardlieIndex.Length);
            Array.Clear(_cbCardIlieCount, 0, _cbCardIlieCount.Length);
            Array.Clear(_cbWeaveType, 0, _cbWeaveType.Length);

            //托管
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bTrustee[i] = false;
                _bLackCardType[i] = 255;
            }

            //清桌
            ClearAllCards();



            //--------------------UI-----------------------
            o_speak_timer.SetActive(false);


            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {

                o_player_chat[i].SetActive(false);
                //o_user_speak[i].SetActive(false);

                o_player_banker[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_action[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_option[i].GetComponent<UISprite>().spriteName = "blank";
                o_player_hu[i].GetComponent<UISprite>().spriteName = "blank";
				if(Phone==true)
				{
					o_info[i].SetActive(false);
				}

                _nEndUserScore[i] = 0;
                _nEndUserExp[i] = 0;

            }

            o_ready_buttons.SetActive(false);
            o_result.SetActive(false);
            o_sys_dlg.SetActive(true);
            o_select_buttons.SetActive(false);

            o_player_clock.SetActive(false);
            o_up.SetActive(false);
            o_down.SetActive(false);
            o_left.SetActive(false);
            o_right.SetActive(false);
            o_card_sum.GetComponent<UILabel>().text = "";

            o_Tuoguan.SetActive(false);


            //操作控制
            o_operate_buttons.SetActive(false);              //操作条
            o_operate_eat_buttons.SetActive(false);          //吃
            o_operate_touch_buttons.SetActive(false);        //碰
            o_operate_bridge_buttons.SetActive(false);       //杠
            o_operate_giveup_buttons.SetActive(false);       //弃
            o_operate_beard_buttons.SetActive(false);        //胡

            ctr_operateEat_cards1.SetActive(false);
            ctr_operateEat_cards2.SetActive(false);
            ctr_operateEat_cards3.SetActive(false);



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

			liuju_sp.gameObject.SetActive(false);

			GameObject.Find("Panel").GetComponent<AudioSource>().volume=0.3f;
			GameObject.Find ("Panel").GetComponent<AudioSource>().Play();
        }
        void FixedUpdate()
        {
            if (!_bStart)
            {
                return;
            }

            byte bMeChair = (byte)GameEngine.Instance.MySelf.DeskStation;
            if (bMeChair != 255)
            {
                if (_bTrustee[bMeChair] && _bTimerType == TimerType.TIMER_LACK)
                {
                    AutoLackCard();
                }
                else
                {

                    if (_bTrustee[bMeChair] && _wCurrentUser == bMeChair)
                    {
                        switch (_bTimerType)
                        {
                            case TimerType.TIMER_OPTION:
                                {
                                    if ((_cbOperateCode != GameLogic.WIK_NULL))
                                    {
                                        if (((_cbOperateCode & GameLogic.WIK_CHI_HU) != 0))
                                        {
                                            OnOperateBeardIvk();
                                        }
                                        else
                                        {
                                            OnOperateGiveupIvk();
                                        }
                                    }
                                    break;
                                }
                            case TimerType.TIMER_PLAY:
                                {
		
                                    AutoOutCard();
                                    break;
                                }
                        }

                    }
                }
            }

            //
            if ((Environment.TickCount - _nInfoTickCount) > 5000)
            {
				if(Phone==true)
				{
					ClearAllInfo();
				}
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
        void Update()
        {
            
        }
        void GoLogin()
        {
            UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_LOGIN);
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
						//Invoke("ClearUserReady", 1.0f);
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
            //游戏状态
            switch (subcmd)
            {
                case SubCmd.SUB_S_GAME_START:       //游戏开始
                    {
                        OnGameStartResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_LACK_CARD:        //选缺
                    {
                        OnUserLackResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_OUT_CARD:         //出牌消息
                    {
                        OnOutCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_SEND_CARD:        //抓牌消息
                    {
                        OnSendCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_LISTEN_CARD:      //听牌处理
                    {
                        OnListenCardResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_OPERATE_NOTIFY:   //操作提示
                    {
                        OnOperateNotifyResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_OPERATE_RESULT:   //操作结果
                    {
                        OnOperateResultResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GANG_SCORE:       //刮风下雨
                    {
                        OnUserGangScoreResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_CHI_HU:           //用户胡牌
                    {
                        OnUserHuResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_GAME_END:         //游戏结束
                    {
						
                        OnGameEndResp(packet);
                        break;
                    }
                case SubCmd.SUB_S_TRUSTEE:          //用户托管
                    {
                        OnTrusteeResp(packet);
                        break;
                    }
            }
        }

        //
        bool OnGameStartResp(NPacket packet)
        {
			showLack=true;
            o_ready_buttons.SetActive(false);

            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
            packet.BeginRead();

            _wSiceCount = packet.GetInt();
            _wBankerUser = packet.GetShort();
            _wCurrentUser = packet.GetShort();
            _cbUserAction = packet.GetByte();
            _cbLeftCardCount = GameLogic.MAX_REPERTORY - GameLogic.GAME_PLAYER * (GameLogic.MAX_COUNT - 1) - 1;

			//结算面板初始化
			for(int i=0;i<GameLogic.GAME_PLAYER;i++)
			{
				o_result_zm[i].GetComponent<UISprite>().spriteName= "blank";
			}


            //出牌信息
            _cbOutCardData = 0;
            _wOutCardUser = -1;
            Array.Clear(_cbDiscardCard, 0, _cbDiscardCard.Length);
            Array.Clear(_cbDiscardCount, 0, _cbDiscardCount.Length);

            //组合扑克
            Array.Clear(_cbWeaveCount, 0, _cbWeaveCount.Length);
            Array.Clear(_WeaveItemArray, 0, _WeaveItemArray.Length);

            //设置扑克
            byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
            packet.GetBytes(ref cbCardData, GameLogic.MAX_COUNT);


            //堆立扑克
            for (short i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _cbHeapCardInfo[i, 0] = 0;
                _cbHeapCardInfo[i, 1] = 0;
            }

            //分发扑克
            byte cbTakeCount = (GameLogic.MAX_COUNT - 1) * GameLogic.GAME_PLAYER + 1;
            byte cbSiceFirst = (byte)(_wSiceCount >> 8);
            byte cbSiceSecond = (byte)(_wSiceCount & 0xff);
            short wTakeChairID = (short)((_wBankerUser + 7 - cbSiceFirst) % GameLogic.GAME_PLAYER);
            for (short i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                //计算数目
                byte cbValidCount = (byte)(GameLogic.HEAP_FULL_COUNT - _cbHeapCardInfo[wTakeChairID, 1] - ((i == 0) ? (cbSiceSecond - 1) * 2 : 0));
                byte cbRemoveCount = Math.Min(cbValidCount, cbTakeCount);

                //提取扑克
                cbTakeCount -= cbRemoveCount;
                _cbHeapCardInfo[wTakeChairID, (i == 0) ? 1 : 0] += cbRemoveCount;

                //完成判断
                if (cbTakeCount == 0)
                {
                    _wHeapHand = wTakeChairID;
                    _wHeapTail = (short)((_wBankerUser + 7 - cbSiceFirst) % GameLogic.GAME_PLAYER);
                    break;
                }

                //切换索引
                wTakeChairID = (short)((wTakeChairID + 1) % GameLogic.GAME_PLAYER);
            }

            //扑克设置
            for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                //变量定义
                short wViewChairID = ChairToView((byte)i);

                PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                byte cbCardCount;
				//Debug.LogError(GameLogic.GAME_PLAYER);
                switch (i)
                {
                    case 0:
                        {
                            cbCardCount = (byte)(GameEngine.Instance.MySelf.DeskStation == _wBankerUser ? GameLogic.MAX_COUNT : (GameLogic.MAX_COUNT - 1.0f));
                            GameLogic.SwitchToCardIndex(cbCardData, cbCardCount, ref _cbCardIndex);
                            if (cbCardCount == 14)
                            {
                                byte[] cards = new byte[1];
                                cards[0] = cbCardData[GameLogic.MAX_COUNT - 1];
                                o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, GameLogic.MAX_COUNT - 1, Cardkind.SelfHand, cbCardData);
                                o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 1, Cardkind.SlefCatch, cbCardData);
                            }
                            else
                            {
                                o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, cbCardCount, Cardkind.SelfHand, cbCardData);
                            }
                            break;
                        }
                    case 1:
                        {
                            byte[] cards = new byte[50];
							cbCardCount = (byte)((GameEngine.Instance.MySelf.DeskStation+1)%GameLogic.GAME_PLAYER== _wBankerUser ? GameLogic.MAX_COUNT : (GameLogic.MAX_COUNT - 1.0f));

                            if (cbCardCount == 14)
                            {

                                o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, GameLogic.MAX_COUNT - 1, Cardkind.WestHand, cards);
                                o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 1, Cardkind.WestCatch, cards);
                            }
                            else
                            {
                                o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, cbCardCount, Cardkind.WestHand, cards);
                            }
                            break;
                        }
                    case 2:
                        {
                            byte[] cards = new byte[50];
                            cbCardCount = (byte)((GameEngine.Instance.MySelf.DeskStation+2)%GameLogic.GAME_PLAYER == _wBankerUser ? GameLogic.MAX_COUNT : (GameLogic.MAX_COUNT - 1.0f));
                            if (cbCardCount == 14)
                            {
                                o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, GameLogic.MAX_COUNT - 1, Cardkind.NorthHand, cards);
                                o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 1, Cardkind.NorthCatch, cards);
                            }
                            else
                            {
                                o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, cbCardCount, Cardkind.NorthHand, cards);
                            }
                            break;
                        }
                    case 3:
                        {
                            byte[] cards = new byte[50];
                            cbCardCount = (byte)((GameEngine.Instance.MySelf.DeskStation+3)%GameLogic.GAME_PLAYER == _wBankerUser ? GameLogic.MAX_COUNT : (GameLogic.MAX_COUNT - 1.0f));
                            if (cbCardCount == 14)
                            {
                                o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, GameLogic.MAX_COUNT - 1, Cardkind.EastHand, cards);
                                o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 1, Cardkind.EastCatch, cards);
                            }
                            else
                            {
                                o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, cbCardCount, Cardkind.EastHand, cards);
                            }
                            break;
                        }
                }

            }
            //
            SetBanker();
            //
            PlayGameSound(SoundType.START);


			//选缺更新
			for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
			{
				SetUserLack(i, _bLackCardType[i]);
			}
            //
            ShowLockView(true);

            //
            //PlayerPrefs.SetInt("UsedServ", GameEngine.Instance.MyUser.ServerUsed.ServerID);

            return true;
        }
        void ShowLockView(bool bshow)
        {
            if (bshow)
            {
                o_select_buttons.SetActive(true);
                o_lack_clock.GetComponent<UIClock>().SetTimer(10 * 1000);
                _bTimerType = TimerType.TIMER_LACK;
            }
            else
            {

                o_lack_clock.GetComponent<UIClock>().SetTimer(0);
                _bTimerType = TimerType.TIMER_NULL;
                o_select_buttons.SetActive(false);
            }
        }
        void AutoLackCard()
        {
            try
            {
                int nWanSum = 0;
                int nTiaoSum = 0;
                int nTongSum = 0;
                //
                for (int i = 0; i < 9; i++)
                {
                    nWanSum += _cbCardIndex[i];
                }
                for (int i = 9; i < 18; i++)
                {
                    nTiaoSum += _cbCardIndex[i];
                }
                for (int i = 18; i < 27; i++)
                {
                    nTongSum += _cbCardIndex[i];
                }

                //
                byte bLack = 0;
                if (nWanSum <= nTongSum && nWanSum <= nTiaoSum)
                {
                    bLack = 0;
                }
                if (nTiaoSum <= nWanSum && nTiaoSum <= nTongSum)
                {
                    bLack = 1;
                }
                if (nTongSum <= nWanSum && nTongSum <= nTiaoSum)
                {
                    bLack = 2;
                }

                //

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LACK_CARD);
                packet.AddUShort((ushort)GetSelfChair());
                packet.Addbyte(bLack);
                GameEngine.Instance.Send(packet);

                ShowLockView(false);
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OnOutCardResp(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_PLAYING;
            packet.BeginRead();
            ushort wOutCardUser = packet.GetUShort();
            byte cbOutCardData = packet.GetByte();
            uint CharTd = GameEngine.Instance.MySelf.DeskStation;

            if (wOutCardUser != CharTd)
            {
                //
                o_Selfdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[0] - 1, false);
                o_Westdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[1] - 1, false);
                o_Northdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[2] - 1, false);
                o_Eastdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[3] - 1, false);


                byte bViewId = ChairToView((byte)wOutCardUser);
                switch (bViewId)
                {
                    case 1:
                        {

                            _cbWestDeskCard[_cbDiscardCount[1]++] = cbOutCardData;
                            o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(_cbWestDeskCard, 0, Cardkind.WestCatch, _cbWestDeskCard);
                            o_Westdesk_cards.GetComponent<UICardControl>().SetCardData(_cbWestDeskCard, _cbDiscardCount[1], Cardkind.WestDesk, _cbWestDeskCard);
                            o_Westdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[1] - 1, true);

                            break;
                        }
                    case 2:
                        {

                            _cbNorthDeskCard[_cbDiscardCount[2]++] = cbOutCardData;
                            o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(_cbNorthDeskCard, 0, Cardkind.NorthCatch, _cbNorthDeskCard);
                            o_Northdesk_cards.GetComponent<UICardControl>().SetCardData(_cbNorthDeskCard, _cbDiscardCount[2], Cardkind.NorthDesk, _cbNorthDeskCard);
                            o_Northdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[2] - 1, true);
                            break;
                        }
                    case 3:
                        {

                            _cbEastDeskCard[_cbDiscardCount[3]++] = cbOutCardData;
                            o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(_cbEastDeskCard, 0, Cardkind.EastCatch, _cbEastDeskCard);
                            o_Eastdesk_cards.GetComponent<UICardControl>().SetCardData(_cbEastDeskCard, _cbDiscardCount[3], Cardkind.EastDesk, _cbEastDeskCard);
                            o_Eastdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[3] - 1, true);
                            break;
                        }
                }

                //音效
                PlayGameSound(SoundType.OUTCARD);

                //
                Invoke("ClearAction", 2.0f);
                //语音
                GameSoundType bVoiceType = GameSoundType.W1;
                switch (cbOutCardData)
                {
                    //万
                    case 0x01: bVoiceType = GameSoundType.W1; break;
                    case 0x02: bVoiceType = GameSoundType.W2; break;
                    case 0x03: bVoiceType = GameSoundType.W3; break;
                    case 0x04: bVoiceType = GameSoundType.W4; break;
                    case 0x05: bVoiceType = GameSoundType.W5; break;
                    case 0x06: bVoiceType = GameSoundType.W6; break;
                    case 0x07: bVoiceType = GameSoundType.W7; break;
                    case 0x08: bVoiceType = GameSoundType.W8; break;
                    case 0x09: bVoiceType = GameSoundType.W9; break;

                    //条
                    case 0x11: bVoiceType = GameSoundType.T1; break;
                    case 0x12: bVoiceType = GameSoundType.T2; break;
                    case 0x13: bVoiceType = GameSoundType.T3; break;
                    case 0x14: bVoiceType = GameSoundType.T4; break;
                    case 0x15: bVoiceType = GameSoundType.T5; break;
                    case 0x16: bVoiceType = GameSoundType.T6; break;
                    case 0x17: bVoiceType = GameSoundType.T7; break;
                    case 0x18: bVoiceType = GameSoundType.T8; break;
                    case 0x19: bVoiceType = GameSoundType.T9; break;

                    //饼
                    case 0x21: bVoiceType = GameSoundType.B1; break;
                    case 0x22: bVoiceType = GameSoundType.B2; break;
                    case 0x23: bVoiceType = GameSoundType.B3; break;
                    case 0x24: bVoiceType = GameSoundType.B4; break;
                    case 0x25: bVoiceType = GameSoundType.B5; break;
                    case 0x26: bVoiceType = GameSoundType.B6; break;
                    case 0x27: bVoiceType = GameSoundType.B7; break;
                    case 0x28: bVoiceType = GameSoundType.B8; break;
                    case 0x29: bVoiceType = GameSoundType.B9; break;
                    default: break;

                }
                //
                PlayUserSound(bVoiceType, GameEngine.Instance.GetTableUserItem(wOutCardUser).Gender);
            }

        }

        void OnSendCardResp(NPacket packet)
        {
            //设置变量
            ushort wMeChairID = GameEngine.Instance.MySelf.DeskStation;
            packet.BeginRead();
            byte cbCardData = packet.GetByte();
            byte cbActionMask = packet.GetByte();
            _wCurrentUser = packet.GetShort();
            _cbOperateCode = cbActionMask;
            _bTrustee_Card = _cbOperateCard = cbCardData;
            _nInfoTickCount = Environment.TickCount;



            //发牌处理
            if (cbCardData != 0)
            {
                SetUserClock((byte)_wCurrentUser, 20, TimerType.TIMER_PLAY, false);
                //取牌界面
                byte wViewChairID = ChairToView((byte)_wCurrentUser);
                byte[] card = new byte[1];
                switch (wViewChairID)
                {
                    case 0:
                        {

                            card[0] = cbCardData;
                            _cbCardIndex[GameLogic.SwitchToCardIndex(cbCardData)]++;
                            o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(card, 1, Cardkind.SlefCatch, card);
                            break;
                        }
                    case 1:
                        {
                            o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(card, 1, Cardkind.WestCatch, card);
                            break;
                        }
                    case 2:
                        {
                            o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(card, 1, Cardkind.NorthCatch, card);
                            break;
                        }
                    case 3:
                        {
                            o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(card, 1, Cardkind.EastCatch, card);
                            break;
                        }
                }

                _cbLeftCardCount--;

                if (_cbLeftCardCount < 0)
                    _cbLeftCardCount = 0;

                ShowScoreAndCards();
            }
        }

        void OnListenCardResp(NPacket packet)
        {
            packet.BeginRead();
            short wListenUser = packet.GetShort();
            if (wListenUser == GameEngine.Instance.MySelf.DeskStation)
            {
                _bHearStatus = true;
            }
        }
		//操作提示
        void OnOperateNotifyResp(NPacket packet)
        {
            //变量定义
            packet.BeginRead();
            short wResumeUser = packet.GetShort();
            byte cbActionMask = packet.GetByte();
            byte cbActionCard = packet.GetByte();
            _cbOperateCode = cbActionMask;

            _cbOperateCard = cbActionCard;

			Debug.Log("<color=red>"+cbActionMask+"</color>");
            //用户界面
            if ((cbActionMask != GameLogic.WIK_NULL))
            {
                if (_bTrustee[GetSelfChair()])
                {
                    if (((cbActionMask & GameLogic.WIK_CHI_HU) != 0))
                    {
                        OnOperateBeardIvk();
                    }
                    else
                    {
                        OnOperateGiveupIvk();
                    }
                    return;
                }

                o_operate_buttons.SetActive(true);
                o_operate_eat_buttons.SetActive(false);
                o_operate_touch_buttons.SetActive(false);
                o_operate_bridge_buttons.SetActive(false);
                o_operate_giveup_buttons.SetActive(true);
                o_operate_beard_buttons.SetActive(false);

                /*
                if(((cbActionMask & GameLogic.WIK_LEFT) != 0) || ((cbActionMask & GameLogic.WIK_CENTER) != 0) || ((cbActionMask & GameLogic.WIK_RIGHT) != 0))
                {
                    o_operate_eat_buttons.SetActive(true);
                }
                */
                if ((cbActionMask & GameLogic.WIK_GANG) != 0)
                {
                    o_operate_bridge_buttons.SetActive(true);
                }
				else if ((cbActionMask & GameLogic.WIK_PENG) != 0&&_cbLeftCardCount!=0)
                {
                    o_operate_touch_buttons.SetActive(true);
                }

                if ((cbActionMask & GameLogic.WIK_CHI_HU) != 0)
                {
                    o_operate_beard_buttons.SetActive(true);

					if(_cbLeftCardCount < 4)
					{
						OnOperateBeardIvk();
					}
                }
            }

            SetUserClock((byte)_wCurrentUser, 20, TimerType.TIMER_OPTION, true);
        }
		//操作结果
        void OnOperateResultResp(NPacket packet)
        {
            //消息处理
            packet.BeginRead();

            //变量定义
            byte cbPublicCard = 1;
            short wOperateUser = packet.GetShort();
            short wProvideUser = packet.GetShort();
            byte _cbOperateCode = packet.GetByte();
            byte cbOperateCard = packet.GetByte();



            byte wOperateViewID = ChairToView((byte)wOperateUser);
            byte wProvideViewID = ChairToView((byte)wProvideUser);


            //出牌变量
            if (_cbOperateCode != GameLogic.WIK_NULL)
            {
                _cbOutCardData = 0;
                _wOutCardUser = -1;
            }

            //


            //设置组合
            if ((_cbOperateCode & GameLogic.WIK_GANG) != 0)
            {
                //
                PlayUserSound(GameSoundType.GANG, GameEngine.Instance.GetTableUserItem((ushort)wOperateUser).Gender);
                if (wProvideUser == wOperateUser)
                {
                    SetUserAction((byte)wOperateUser, UserAction.GF);
                }
                else
                {
                    SetUserAction((byte)wOperateUser, UserAction.XY);
                }

                //设置变量
                _wCurrentUser = -1;
				//_wCurrentUser=wOperateUser;
                //组合扑克
                byte cbWeaveIndex = 100;
                for (byte i = 0; i < _cbWeaveCount[wOperateUser]; i++)
                {

                    byte cbWeaveKind = _WeaveItemArray[wOperateUser, i].cbWeaveKind;
                    byte cbCenterCard = _WeaveItemArray[wOperateUser, i].cbCenterCard;
                    if ((cbCenterCard == cbOperateCard) && (cbWeaveKind == GameLogic.WIK_PENG))
                    {
                        cbWeaveIndex = i;
                        _WeaveItemArray[wOperateUser, cbWeaveIndex].cbPublicCard = 1;
                        _WeaveItemArray[wOperateUser, cbWeaveIndex].cbWeaveKind = _cbOperateCode;
                        _WeaveItemArray[wOperateUser, cbWeaveIndex].wProvideUser = wProvideUser;
                        break;
                    }
                }

                //组合扑克

                if (cbWeaveIndex == 100)
                {
                    //暗杠判断
                    cbPublicCard = (byte)((wProvideUser == wOperateUser) ? 0 : 1);

                    //设置扑克
                    cbWeaveIndex = _cbWeaveCount[wOperateUser]++;
                    _WeaveItemArray[wOperateUser, cbWeaveIndex] = new tagWeaveItem();
                    _WeaveItemArray[wOperateUser, cbWeaveIndex].cbPublicCard = cbPublicCard;
                    _WeaveItemArray[wOperateUser, cbWeaveIndex].cbCenterCard = cbOperateCard;
                    _WeaveItemArray[wOperateUser, cbWeaveIndex].cbWeaveKind = _cbOperateCode;
                    _WeaveItemArray[wOperateUser, cbWeaveIndex].wProvideUser = wProvideUser;


                }



                //扑克设置
                bool bSelf = false;
                if (GameEngine.Instance.MySelf.DeskStation == wOperateUser)
                {
                    if (cbPublicCard == 0)
                    {
                        bSelf = true;
                    }
                    _cbCardIndex[GameLogic.SwitchToCardIndex(cbOperateCard)] = 0;
                }

                //
                byte[] cbWeaveCard = new byte[4];
                //
                byte cbWeavecardCount = GameLogic.GetWeaveCard(_cbOperateCode, cbOperateCard, ref cbWeaveCard);
                //处理暗杠明一张牌
                byte cbWeaveCardCount = GameLogic.GetWeaveCard(_cbOperateCode, cbOperateCard, ref _cbCardlieIndex, wOperateViewID, ref _cbCardIlieCount, ref _cbCardHiddenIndex, cbPublicCard, bSelf);


                byte[] cbCardlie = new byte[GameLogic.MAX_INDEX];
                byte[] cbCardHideen = new byte[GameLogic.MAX_INDEX];

                for (int i = 0; i < GameLogic.MAX_INDEX; i++)
                {
                    cbCardlie[i] = _cbCardlieIndex[wOperateViewID, i];
                    cbCardHideen[i] = _cbCardHiddenIndex[wOperateViewID, i];
                }

                SetUserClock((byte)wOperateUser, 20, TimerType.TIMER_PLAY, false);

                //处理杠牌用户牌
                switch (wOperateViewID)
                {
                    case 0:
                        {
                            byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
                            byte cbCardCount = GameLogic.SwitchToCardData(_cbCardIndex, ref cbCardData);
                            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, (byte)(cbCardCount), Cardkind.SelfHand, cbCardHideen);
                            o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.SelfLie, cbCardHideen);

                            break;
                        }
                    case 1:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.WestHand, cbCardHideen);
                            o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.WestLie, cbCardHideen);

                            break;
                        }
                    case 2:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.NorthHand, cbCardHideen);
                            o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.NorthLie, cbCardHideen);

                            break;
                        }
                    case 3:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.EastHand, cbCardHideen);
                            o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.EastLie, cbCardHideen);

                            break;
                        }
                }
                //处理提供者
                if (wOperateUser != wProvideUser)
                {
                    switch (wProvideViewID)
                    {
                        case 0:
                            {
                                _cbDiscardCount[0]--;
                                o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(_cbSelfDeskCard, _cbDiscardCount[0], Cardkind.SlefDesk, _cbSelfDeskCard);
                                break;
                            }
                        case 1:
                            {
                                _cbDiscardCount[1]--;
                                o_Westdesk_cards.GetComponent<UICardControl>().SetCardData(_cbWestDeskCard, _cbDiscardCount[1], Cardkind.WestDesk, _cbWestDeskCard);
                                break;
                            }
                        case 2:
                            {
                                _cbDiscardCount[2]--;
                                o_Northdesk_cards.GetComponent<UICardControl>().SetCardData(_cbNorthDeskCard, _cbDiscardCount[2], Cardkind.NorthDesk, _cbNorthDeskCard);
                                break;
                            }
                        case 3:
                            {
                                _cbDiscardCount[3]--;
                                o_Eastdesk_cards.GetComponent<UICardControl>().SetCardData(_cbEastDeskCard, _cbDiscardCount[3], Cardkind.EastDesk, _cbEastDeskCard);
                                break;
                            }
                    }
                }
            }
            else if (_cbOperateCode != GameLogic.WIK_NULL)
            {
                //
                if ((_cbOperateCode & GameLogic.WIK_PENG) != 0)
                {
                    PlayUserSound(GameSoundType.PENG, GameEngine.Instance.GetTableUserItem((ushort)wOperateUser).Gender);
                    SetUserAction((byte)wOperateUser, UserAction.PENG);
                }



                //设置变量
                _wCurrentUser = wOperateUser;
                //设置组合
                byte cbWeaveIndex = _cbWeaveCount[wOperateUser]++;
                _WeaveItemArray[wOperateUser, cbWeaveIndex] = new tagWeaveItem();
                _WeaveItemArray[wOperateUser, cbWeaveIndex].cbPublicCard = 1;
                _WeaveItemArray[wOperateUser, cbWeaveIndex].cbCenterCard = cbOperateCard;
                _WeaveItemArray[wOperateUser, cbWeaveIndex].cbWeaveKind = _cbOperateCode;
                _WeaveItemArray[wOperateUser, cbWeaveIndex].wProvideUser = wProvideUser;

                //组合界面
                byte[] cbWeaveCard = new byte[4];
                byte cbWeaveKind = _cbOperateCode;
                byte cbWeavecardCount = GameLogic.GetWeaveCard(cbWeaveKind, cbOperateCard, ref cbWeaveCard);
                byte cbWeaveCardCount = GameLogic.GetWeaveCard(_cbOperateCode, cbOperateCard, ref _cbCardlieIndex, wOperateViewID, ref _cbCardIlieCount, ref _cbCardHiddenIndex, cbPublicCard, false);

                byte[] cbCardlie = new byte[GameLogic.MAX_INDEX];
                byte[] cbCardHideen = new byte[GameLogic.MAX_INDEX];
                for (int i = 0; i < GameLogic.MAX_INDEX; i++)
                {
                    cbCardlie[i] = _cbCardlieIndex[wOperateViewID, i];
                    cbCardHideen[i] = _cbCardHiddenIndex[wOperateViewID, i];
                }

                SetUserClock((byte)wOperateUser, 20, TimerType.TIMER_PLAY, false);

                switch (wOperateViewID)
                {
                    case 0:
                        {

                            byte[] card = new byte[1];
                            card[0] = cbOperateCard;
                            GameLogic.RemoveCard1(ref cbWeaveCard, cbWeavecardCount, card, 1);
                            GameLogic.RemoveCard(ref _cbCardIndex, cbWeaveCard, (byte)(cbWeavecardCount - 1));

                            byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
                            byte cbCardCount = GameLogic.SwitchToCardData(_cbCardIndex, ref cbCardData);
                            byte[] cards = new byte[1];
                            cards[0] = cbCardData[cbCardCount - 1];
                            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, (byte)(cbCardCount - 1), Cardkind.SelfHand, cbCardHideen);
							//o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(1), Cardkind.SlefCatch, cbCardHideen);
							o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 1, Cardkind.SlefCatch, cards);
                            o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.SelfLie, cbCardHideen);


                            break;
                        }
                    case 1:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.WestHand, cbCardHideen);
                            o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.WestLie, cbCardHideen);

                            break;
                        }
                    case 2:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.NorthHand, cbCardHideen);
                            o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.NorthLie, cbCardHideen);

                            break;
                        }
                    case 3:
                        {
                            byte[] cards = new byte[GameLogic.MAX_INDEX];
                            byte cbCardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[wOperateUser] * 3);
                            o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(cbCardCount - 1), Cardkind.EastHand, cbCardHideen);
                            o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.EastLie, cbCardHideen);

                            break;
                        }
                }

                //处理提供者
                if (wOperateUser != wProvideUser)
                {
                    switch (wProvideViewID)
                    {
                        case 0:
                            {
                                _cbDiscardCount[0]--;
                                o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(_cbSelfDeskCard, _cbDiscardCount[0], Cardkind.SlefDesk, _cbSelfDeskCard);
                                break;
                            }
                        case 1:
                            {
                                _cbDiscardCount[1]--;
                                o_Westdesk_cards.GetComponent<UICardControl>().SetCardData(_cbWestDeskCard, _cbDiscardCount[1], Cardkind.WestDesk, _cbWestDeskCard);
                                break;
                            }
                        case 2:
                            {
                                _cbDiscardCount[2]--;
                                o_Northdesk_cards.GetComponent<UICardControl>().SetCardData(_cbNorthDeskCard, _cbDiscardCount[2], Cardkind.NorthDesk, _cbNorthDeskCard);
                                break;
                            }
                        case 3:
                            {
                                _cbDiscardCount[3]--;
                                o_Eastdesk_cards.GetComponent<UICardControl>().SetCardData(_cbEastDeskCard, _cbDiscardCount[3], Cardkind.EastDesk, _cbEastDeskCard);
                                break;
                            }
                    }
                }
            }



            o_operate_buttons.SetActive(false);
            o_operate_eat_buttons.SetActive(false);
            o_operate_touch_buttons.SetActive(false);
            o_operate_bridge_buttons.SetActive(false);
            o_operate_giveup_buttons.SetActive(false);
            o_operate_beard_buttons.SetActive(false);
        }

        void OnUserHuResp(NPacket packet)
        {
            /*
            BYTE                            cbCardData[MAX_INDEX];  //
            WORD                            wChiHuUser;                         //
            WORD                            wProviderUser;                      //
            BYTE                            cbChiHuCard;                        //
            BYTE                            cbCardCount;                        //
            LONG                            lGameScore;                         //
            BYTE                            cbWinOrder;                         //
            */

            byte[] cbCardData = new byte[GameLogic.MAX_INDEX];
            packet.BeginRead();
            packet.GetBytes(ref cbCardData, GameLogic.MAX_INDEX);
            byte bChiHuUser = (byte)packet.GetUShort();
            byte bProviUser = (byte)packet.GetUShort();
            byte bChiHuCard = packet.GetByte();
            byte bCardCount = packet.GetByte();
            int lGameScore = packet.GetInt();
            byte bWinOrder = packet.GetByte();
			bool bTouch = packet.GetBool();

			if(bTouch==true)
			{
				SetUserAction(bChiHuUser, UserAction.ZM);
				int i= ChairToView(bChiHuUser);
				o_result_zm[i].GetComponent<UISprite>().spriteName= "zm";
			}
			else
			{
				SetUserAction(bChiHuUser, UserAction.HU);
			}
            
            PlayUserSound(GameSoundType.HU, GameEngine.Instance.GetTableUserItem(bChiHuUser).Gender);
            PlayGameSound(SoundType.HU);

            o_operate_buttons.SetActive(false);


            //变量定义
            short wViewChairID = ChairToView(bChiHuUser);
            byte[] cbCardlie = new byte[GameLogic.MAX_INDEX];
            byte[] cbCardliel = new byte[GameLogic.MAX_INDEX];
            byte[] cbCardlieindex = new byte[GameLogic.MAX_INDEX];
            byte[] cbCardlieindexl = new byte[GameLogic.MAX_INDEX];
            byte cbCardliecout = 0;
            for (int i = 0; i < GameLogic.MAX_INDEX; i++)
            {
                cbCardlie[i] = cbCardData[i];
                cbCardliel[i] = _cbCardlieIndex[wViewChairID, i];
                cbCardlieindexl[i] = 0;
                cbCardlieindex[i] = 1;
            }
            cbCardliecout = bCardCount;

            //如果是吃胡，去除胡牌
            //if(bChiHuUser!=bProviUser)
            //{
            cbCardliecout = (byte)(cbCardliecout - 1);
            //}


            switch (wViewChairID)
            {
                case 0:
                    {

                        o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.SelfLie, cbCardlieindex);
                        o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout, Cardkind.SelfHandHu, cbCardlieindexl);
                        o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(new byte[] { bChiHuCard }, 1, Cardkind.SelfCatchGameEnd, new byte[] { 1 });
                        o_player_hu[0].GetComponent<UISprite>().spriteName = "hu_spl";
                        break;
                    }
                case 1:
                    {
                        o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.WestLie, cbCardlieindex);
                        o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout, Cardkind.WestHandHu, cbCardlieindexl);
                        o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(new byte[] { bChiHuCard }, 1, Cardkind.WestCatchGameEnd, new byte[] { 1 });
                        o_player_hu[1].GetComponent<UISprite>().spriteName = "hu_spl";
                        break;
                    }
                case 2:
                    {
                        o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.NorthLie, cbCardlieindex);
                        o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout, Cardkind.NorthHandHu, cbCardlieindexl);
                        o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(new byte[] { bChiHuCard }, 1, Cardkind.NorthCatchGameEnd, new byte[] { 1 });
                        o_player_hu[2].GetComponent<UISprite>().spriteName = "hu_spl";
                        break;
                    }
                case 3:
                    {
                        o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.EastLie, cbCardlieindex);
                        o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout, Cardkind.EastHandHu, cbCardlieindexl);
                        o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(new byte[] { bChiHuCard }, 1, Cardkind.EastCatchGameEnd, new byte[] { 1 });
                        o_player_hu[3].GetComponent<UISprite>().spriteName = "hu_spl";
                        break;
                    }
            }
        }
        void OnUserGangScoreResp(NPacket packet)
        {
            packet.BeginRead();
            byte bChair = (byte)packet.GetUShort();
            byte bXiaYu = packet.GetByte();

            Debug.Log("chair=" + bChair);
            Debug.Log("bXiaYu=" + bXiaYu);

            /*
            if(bXiaYu==1)
            {
                SetUserAction(bChair,UserAction.XY);
            }
            else
            {
                SetUserAction(bChair,UserAction.GF);
            }
            */
        }
		//游戏结束
        void OnGameEndResp(NPacket packet)
        {
			showLack=false;

            if (_bStart == false) return;

            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

            EndTuoguan();

            byte[] wProvideUser = new byte[GameLogic.GAME_PLAYER];                      //供应用户
            uint[] dwChiHuRight = new uint[GameLogic.GAME_PLAYER];                      //胡牌类型
            //扑克信息
            byte[] cbCardCount = new byte[GameLogic.GAME_PLAYER];                       //扑克数目
            byte[,] cbCardData = new byte[GameLogic.GAME_PLAYER, GameLogic.MAX_INDEX];   //扑克数据

            packet.BeginRead();
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                cbCardCount[i] = packet.GetByte();
            }
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                for (int j = 0; j < GameLogic.MAX_INDEX; j++)
                {
                    cbCardData[i, j] = packet.GetByte();
                }
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _nEndUserScore[i] = packet.GetInt();
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                wProvideUser[i] = (byte)packet.GetUShort();
            }


            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                packet.GetInt();
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                packet.GetUShort();
            }

			//详细得分
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _nEndGangScore[i] = packet.GetInt();
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                packet.GetByte();
            }

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                for (int j = 0; j < GameLogic.GAME_PLAYER; j++)
                {
                    packet.GetUShort();
                }
            }

            packet.GetUShort();

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                dwChiHuRight[i] = packet.GetUInt();
            }

			leftUserCount = packet.GetByte();//判断游戏结束，未胡人数是否大于1人

			//玩家是否自摸
			for(int i = 0; i < GameLogic.GAME_PLAYER; i++)
			{
				_bTouch[i] = packet.GetBool();
			}

			//是否听牌
			for(int i = 0; i < GameLogic.GAME_PLAYER; i++)
			{
				_bListen[i] = packet.GetBool();
			}

			if(leftUserCount > 1)
			{
				liuju_sp.gameObject.SetActive(true);
				liujuTween.PlayForward();
				StartCoroutine(DestroyLiuju());

			}
			
			SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL, false);

            UpdateUserView();


            byte[] cards = new byte[1];
            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SelfHand, cards);
            o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SlefCatch, cards);
            o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestHand, cards);
            o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestCatch, cards);
            o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastHand, cards);
            o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastCatch, cards);
            o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthHand, cards);
            o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthCatch, cards);

            for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
            {


                //变量定义
                short wViewChairID = ChairToView((byte)i);
                byte[] cbCardlie = new byte[GameLogic.MAX_INDEX];
                byte[] cbCardliel = new byte[GameLogic.MAX_INDEX];
                byte[] cbCardliecout = new byte[GameLogic.GAME_PLAYER];
                byte[] cbCardlieindex = new byte[GameLogic.MAX_INDEX];
                for (int j = 0; j < GameLogic.MAX_INDEX; j++)
                {
                    cbCardlie[j] = cbCardData[i, j];
                    cbCardliel[j] = _cbCardlieIndex[wViewChairID, j];
                    cbCardlieindex[j] = 1;
                }
                cbCardliecout[wViewChairID] = cbCardCount[i];

                switch (wViewChairID)
                {
                    case 0:
                        {
                            o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.SelfLie, cbCardlieindex);
                            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout[wViewChairID], Cardkind.SelfLieGameEnd, cbCardlieindex);
                            break;
                        }
                    case 1:
                        {
                            o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.WestLie, cbCardlieindex);
                            o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout[wViewChairID], Cardkind.WestLieGameEnd, cbCardlieindex);
                            break;
                        }
                    case 2:
                        {
                            o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.NorthLie, cbCardlieindex);
                            o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout[wViewChairID], Cardkind.NorthLieGameEnd, cbCardlieindex);
                            break;
                        }
                    case 3:
                        {
                            o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cbCardliel, (byte)(_cbCardIlieCount[wViewChairID]), Cardkind.EastLie, cbCardlieindex);
                            o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, cbCardliecout[wViewChairID], Cardkind.EastLieGameEnd, cbCardlieindex);
                            break;
                        }
                }


            }

            //结算框内容
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                PlayerInfo ud = GameEngine.Instance.GetTableUserItem(i);
			
                if (ud != null)
                {
                    byte bViewId = ChairToView(i);
                    o_result_score[bViewId].GetComponent<UILabel>().text = _nEndUserScore[i].ToString();
                    if (_nEndUserScore[i] >= 0)
                    {
                        o_result_score[bViewId].GetComponent<UILabel>().color =new Color(1.0f, 0.89f, 0.37f);
                    }
                    else
                    {
                        o_result_score[bViewId].GetComponent<UILabel>().color = Color.gray;
                    }

					o_result_gangScore[bViewId].GetComponent<UILabel>().text =_nEndGangScore[i].ToString();
                    if (_nEndGangScore[i] > 0)
                    {
                        o_result_gangScore[bViewId].GetComponent<UILabel>().color = new Color(1.0f, 0.89f, 0.37f);
                    }
                    else
                    {
                        o_result_gangScore[bViewId].GetComponent<UILabel>().color = Color.gray;
                    }


					if(bViewId==0)
					{
						desc_spwin_label.GetComponent<MJLabel>().m_iNum =(Mathf.Abs(_nEndUserScore[GameEngine.Instance.MySelf.DeskStation]));
						if(_nEndUserScore[GameEngine.Instance.MySelf.DeskStation]>0)
						{
							sp_title.GetComponent<UISprite>().spriteName = "YouWin_spl";
							desc_spwin.GetComponent<UISprite>().spriteName = "win_txt";
                            desc_spwinChip_lbl.GetComponent<UISprite>().spriteName = "ChipWin";
							//desc_spwin_label.GetComponent<MJLabel>().m_cColor = new Color(1.0f,1.0f,1.0f);
						}else{
							sp_title.GetComponent<UISprite>().spriteName = "YouLose_spl";
							desc_spwin.GetComponent<UISprite>().spriteName = "lose_txt";
                            desc_spwinChip_lbl.GetComponent<UISprite>().spriteName = "ChipLose_txt";
                            //desc_spwin_label.GetComponent<MJLabel>().m_cColor = new Color(0.274f, 0.274f, 0.274f);
						}
					}
	

                    //胡牌类型


                    Debug.Log("dwChiHuRight[" + i.ToString() + "]=" + dwChiHuRight[i].ToString());
                    //

                    if ((dwChiHuRight[i] & GameLogic.CHR_QING_LONG_QI_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qlqd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_TIAN_HU) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_tianh";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_DI_HU) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_dih";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QING_YAO_JIU) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qingyj";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_LONG_QI_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_longqd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QING_QI_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qingqd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QING_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qingd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_JIANG_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_jiangd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QI_XIAO_DUI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qd";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QING_YI_SE) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qys";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_DAI_YAO) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_dyj";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_DA_DUI_ZI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_ddz";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_QIANG_GANG) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_qianggang";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_GANG_SHANG_PAO) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_gangshangpao";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_GANG_KAI) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_gangshanghua";
                    }
                    else if ((dwChiHuRight[i] & GameLogic.CHR_SHU_FAN) != GameLogic.CHK_NULL)
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "ct_shufan";
                    }
					else if(_bListen[i] == true&&dwChiHuRight[i] == GameLogic.CHK_NULL)
					{
						o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "tingpai";
					}
					else if(_bListen[i] == false&&dwChiHuRight[i] == GameLogic.CHK_NULL)
					{
						o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "weiting";
					}
                    else
                    {
                        o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "blank";
                    }


                    //

                    if (_wBankerUser == i)
                    {
                        o_result_bk[bViewId].GetComponent<UISprite>().spriteName = "banker";
                    }
                    else
                    {
                        o_result_bk[bViewId].GetComponent<UISprite>().spriteName = "blank";
                    }



                    o_result_nick[bViewId].GetComponent<UILabel>().text = ud.NickName;
                    o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "gamemoney";
                }
                else
                {
                    byte bViewId = ChairToView(i);
                    o_result_bean[bViewId].GetComponent<UISprite>().spriteName = "blank";
                    o_result_nick[bViewId].GetComponent<UILabel>().text = "";
                    o_result_bk[bViewId].GetComponent<UISprite>().spriteName = "blank";
                    o_result_score[bViewId].GetComponent<UILabel>().text = "";
					//o_result_zm[bViewId].GetComponent<UISprite>().spriteName= "blank";
					o_result_gangScore[bViewId].GetComponent<UILabel>().text = "";
                    o_result_ct[bViewId].GetComponent<UISprite>().spriteName = "blank";

                }
            }

            Invoke("ShowResultView", 5);
            //GameEngine.Instance.MyUser.TEvent.AddTimeTok(TimeEvents.REG_GAME_END, 2000, packet);

            //
            if (_nEndUserScore[GetSelfChair()] > 0)
            {
                PlayGameSound(SoundType.WIN);
            }
            else
            {
                PlayGameSound(SoundType.LOSE);
            }

            o_card_sum.GetComponent<UILabel>().text = "";

        }

        void OnTrusteeResp(NPacket packet)
        {
            packet.BeginRead();
            bool bTrustees = packet.GetBool();
            short ChairID = packet.GetShort();
            _bTrustee[ChairToView((byte)ChairID)] = bTrustees;
        }
        //选缺
        void OnUserLackResp(NPacket packet)
        {
		
            packet.BeginRead();
            ushort wUser = packet.GetUShort();
            byte bLackCardType = packet.GetByte();

            _bLackCardType[wUser] = bLackCardType;

            SetUserAction((byte)wUser, UserAction.LACK);

            PlayGameSound(SoundType.LACK);

            if (_bLackCardType[0] != 255 && _bLackCardType[1] != 255 && _bLackCardType[2] != 255 && _bLackCardType[3] != 255)
            {
                //
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    SetUserLack(i, _bLackCardType[i]);
                }

                //
                SetUserClock((byte)_wCurrentUser, 20, TimerType.TIMER_PLAY, false);

                //
                ShowScoreAndCards();

                //
                Invoke("ClearAction", 1.0f);

            }

        }

        //初始场景处理函数
        void SwitchFreeSceneView(NPacket packet)
        {

           /* try
            {*/

                ResetGameView();

                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_WK_FREE;

                packet.BeginRead();
                _nBaseScore = packet.GetShort();

                _bStart = true;

                SetUserClock(0, 20, TimerType.TIMER_READY, false);

                o_left.SetActive(false);
                o_down.SetActive(false);
                o_up.SetActive(false);
                o_right.SetActive(false);

                o_ready_buttons.SetActive(true);

                SetBaseScore(_nBaseScore);

                UpdateUserView();


                //PlayerPrefs.SetInt("UsedServ", 0);
            /*}
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }*/
	

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

                _bTimerType = TimerType.TIMER_PLAY;

                UpdateUserView();


                //设置变量
                _nBaseScore = (short)packet.GetInt();
                _wBankerUser = packet.GetShort();


                _wCurrentUser = packet.GetShort();
			
                byte cbActionCard = packet.GetByte();
                byte cbActionMask = packet.GetByte();
                _cbLeftCardCount = packet.GetByte();
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bTrustee[i] = packet.GetBool();
                }
                short[] wWinOrder = new short[GameLogic.GAME_PLAYER];
                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    wWinOrder[i] = packet.GetShort();
                }


                _wOutCardUser = packet.GetShort();
                _cbOutCardData = packet.GetByte();
                byte[] _DiscardCount = new byte[GameLogic.GAME_PLAYER];
                packet.GetBytes(ref _DiscardCount, GameLogic.GAME_PLAYER);

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    for (int j = 0; j < 60; j++)
                    {
                        _cbDiscardCard[i, j] = packet.GetByte();
                    }
                }

                byte cbCardCount = packet.GetByte();
                byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
                packet.GetBytes(ref cbCardData, GameLogic.MAX_COUNT);
                byte cbSendCardData = packet.GetByte();

                packet.GetBytes(ref _cbWeaveCount, GameLogic.GAME_PLAYER);

                for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    for (int j = 0; j < GameLogic.MAX_WEAVE; j++)
                    {
                        _WeaveItemArray[i, j].cbWeaveKind = packet.GetByte();
                        _WeaveItemArray[i, j].cbCenterCard = packet.GetByte();
                        _WeaveItemArray[i, j].cbPublicCard = packet.GetByte();
                        _WeaveItemArray[i, j].wProvideUser = packet.GetShort();
                    }
                }

                packet.GetUShort();
                packet.GetUShort();
                packet.GetInt();
                packet.GetInt();

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    _bLackCardType[i] = packet.GetByte();
                    SetUserLack(i, _bLackCardType[i]);
                }

                SetBaseScore(_nBaseScore);

                SetBanker();

                ShowScoreAndCards();

                //扑克变量

                GameLogic.SwitchToCardIndex(cbCardData, cbCardCount, ref _cbCardIndex);

                for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    byte wOperateViewID = ChairToView((byte)i);


                    //变量定义
                    for (int j = 0; j < GameLogic.MAX_WEAVE; j++)
                    {
                        if (_WeaveItemArray[i, j].cbWeaveKind != 0)
                        {
                            bool bself = false;
                            if (i == GetSelfChair() && _WeaveItemArray[i, j].cbPublicCard == 0)
                            {
                                bself = true;
                            }
                            GameLogic.GetWeaveCard(_WeaveItemArray[i, j].cbWeaveKind, _WeaveItemArray[i, j].cbCenterCard, ref _cbCardlieIndex, wOperateViewID,
                                ref _cbCardIlieCount, ref _cbCardHiddenIndex, _WeaveItemArray[i, j].cbPublicCard, bself);
                        }
                    }


                    byte[] cbCardlie = new byte[GameLogic.MAX_INDEX];
                    byte[] cbCardHideen = new byte[GameLogic.MAX_INDEX];
                    for (int j = 0; j < GameLogic.MAX_INDEX; j++)
                    {
                        cbCardlie[j] = _cbCardlieIndex[wOperateViewID, j];
                        cbCardHideen[j] = _cbCardHiddenIndex[wOperateViewID, j];
                    }
                    _cbDiscardCount[wOperateViewID] = _DiscardCount[i];
                    switch (wOperateViewID)
                    {
                        case 0:
                            {
                                for (int j = 0; j < 60; j++)
                                {
                                    _cbSelfDeskCard[j] = _cbDiscardCard[i, j];
                                }
                                byte[] card = new byte[1];
                                card[0] = _cbOperateCard;

                                byte[] cards = new byte[1];
                                cards[0] = cbCardData[cbCardCount - 1];
                                if (_wCurrentUser == GameEngine.Instance.MySelf.DeskStation)
                                {
                                    o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, (byte)(cbCardCount - 1), Cardkind.SelfHand, cbCardHideen);
                                    o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(1), Cardkind.SlefCatch, cbCardHideen);
                                }
                                else
                                {
                                    o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, (byte)(cbCardCount), Cardkind.SelfHand, cbCardHideen);
                                }


                                o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.SelfLie, cbCardHideen);
                                o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(_cbSelfDeskCard, _cbDiscardCount[wOperateViewID], Cardkind.SlefDesk, _cbSelfDeskCard);

                                break;
                            }
                        case 1:
                            {
                                for (int j = 0; j < 60; j++)
                                {
                                    _cbWestDeskCard[j] = _cbDiscardCard[i, j];
                                }
                                byte[] cards = new byte[GameLogic.MAX_INDEX];
                                byte CardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[i] * 3);
                                o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(CardCount - 1), Cardkind.WestHand, cbCardHideen);
                                o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.WestLie, cbCardHideen);
                                o_Westdesk_cards.GetComponent<UICardControl>().SetCardData(_cbWestDeskCard, _cbDiscardCount[wOperateViewID], Cardkind.WestDesk, _cbWestDeskCard);
                                break;
                            }
                        case 2:
                            {
                                for (int j = 0; j < 60; j++)
                                {
                                    _cbNorthDeskCard[j] = _cbDiscardCard[i, j];
                                }
                                byte[] cards = new byte[GameLogic.MAX_INDEX];
                                byte CardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[i] * 3);
                                o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(CardCount - 1), Cardkind.NorthHand, cbCardHideen);
                                o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.NorthLie, cbCardHideen);
                                o_Northdesk_cards.GetComponent<UICardControl>().SetCardData(_cbNorthDeskCard, _cbDiscardCount[wOperateViewID], Cardkind.NorthDesk, _cbNorthDeskCard);
                                break;
                            }
                        case 3:
                            {
                                for (int j = 0; j < 60; j++)
                                {
                                    _cbEastDeskCard[j] = _cbDiscardCard[i, j];
                                }
                                byte[] cards = new byte[GameLogic.MAX_INDEX];
                                byte CardCount = (byte)(GameLogic.MAX_COUNT - _cbWeaveCount[i] * 3);
                                o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, (byte)(CardCount - 1), Cardkind.EastHand, cbCardHideen);
                                o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cbCardlie, (byte)(_cbCardIlieCount[wOperateViewID]), Cardkind.EastLie, cbCardHideen);
                                o_Eastdesk_cards.GetComponent<UICardControl>().SetCardData(_cbEastDeskCard, _cbDiscardCount[wOperateViewID], Cardkind.EastDesk, _cbEastDeskCard);
                                break;
                            }
                    }
                }

                if (cbActionMask != GameLogic.WIK_NULL)
                {
                    OnOperateGiveupIvk();
                }

                SetUserClock((byte)_wCurrentUser, 20, TimerType.TIMER_PLAY, false);

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
            GameEngine.Instance.Quit();
        }

        void OnBtnBackIvk()
        {
            try
            {

                if (!GameEngine.Instance.IsPlaying())
                {
                    //o_sys_dlg.SetActive(false);
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

        void OnBtnSettingIvk()
        {
            UISetting.Instance.Show(true);
			btn_chat.GetComponent<UIButton>().isEnabled=false;
			//GameObject.Find("Panel").GetComponent<AudioSource>().Play();
            //o_sys_dlg.SetActive(false);
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

                if(UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>()==null)
                {
                    GameObject.Find("Panel").GetComponent<AudioSource>().volume = 0.3f;
                }else
                {
                    GameObject.Find("Panel").GetComponent<AudioSource>().volume = UISetting.Instance.o_set_music.gameObject.GetComponentInChildren<UISlider>().value;
                }

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
            //o_sys_dlg.SetActive(false);
            OnConfirmBackOKIvk();
        }

		//托管
        void OnBtnTuoguanIvk()
        {
            if (GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_MJ_PLAY)
            {
                BeginTuoguan();
            }
            //o_sys_dlg.SetActive(false);
        }

		//规则
		public void OnBtnRuleIvk()
		{
			bool bshow = !o_help.active;
			o_help.SetActive(bshow);
			o_help.transform.FindChild("Scroll Bar").GetComponent<UIScrollBar>().value = 0;
			if (bshow == true)
			{
				_nInfoTickCount = Environment.TickCount;
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

        void OnClearInfoIvk()
        {
			if(Phone==true)
			{
				ClearAllInfo();
			}
           
        }
        void OnPlayerInfoIvk(GameObject obj)
		{
            string[] strs = obj.name.Split("_".ToCharArray());
            int nChair = Convert.ToInt32(strs[2]);
			bool showInfo=!o_info[nChair].active;
            ShowUserInfo((byte)nChair,showInfo);

        }
		void OnPlayerInfoIvkPc(byte count)
		{
			int nChair = Convert.ToInt32(count);
			ShowUserInfo((byte)nChair,true);
			
		}

		IEnumerator DestroyLiuju()
		{
			yield return new WaitForSeconds(4.0f);
			liujuTween.PlayReverse();
			yield return new WaitForSeconds(2.0f);
			liuju_sp.gameObject.SetActive(false);
		}

        /////////////////////////////游戏特殊/////////////////////////////

        void OnBtnSysIvk()
        {
            o_sys_dlg.SetActive(!o_sys_dlg.active);
        }

        //选缺
        void OnBtnSelectLackIvk(GameObject obj)
        {
            try
            {


                byte cardtype = 0;
                if (obj.name == "btn_tiao")
                {
                    cardtype = 1;
                }
                else if (obj.name == "btn_bing")
                {
                    cardtype = 2;
                }
                else if (obj.name == "btn_wan")
                {
                    cardtype = 0;
                }

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LACK_CARD);
                packet.AddUShort((ushort)GetSelfChair());
                packet.Addbyte(cardtype);
                GameEngine.Instance.Send(packet);

                ShowLockView(false);

                CancelInvoke();
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void OnOutCard(byte CardData)
        {

            if (_wCurrentUser != GameEngine.Instance.MySelf.DeskStation)
            {
                return;
            }
            if (o_operate_buttons.active)
            {
                return;
            }
            if (o_select_buttons.active)
            {
                return;
            }
            if (_bLackCardType[0] == 255 || _bLackCardType[1] == 255 || _bLackCardType[2] == 255 || _bLackCardType[3] == 255)
            {
                return;
            }

            //
            byte black = _bLackCardType[GetSelfChair()];
            for (int i = (black * 9); i < ((black + 1) * 9); i++)
            {
                if ((_cbCardIndex[i] > 0) && (GameLogic.GetCardColor(CardData) != black))
				{
					prompt_sprite.SetActive(true);
					Invoke("LabelEmpty",2.0f);
					return;
				}

            }

			//Debug.Log("out card color:"+GameLogic.GetCardColor(CardData)+" card value:"+GameLogic.GetCardValue(CardData));

            //设置变量
            _wCurrentUser = -1;
            GameLogic.RemoveCard(ref _cbCardIndex, CardData);

            //设置扑克
            byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
            byte cbCardCount = GameLogic.SwitchToCardData(_cbCardIndex, ref cbCardData);
            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cbCardData, cbCardCount, Cardkind.SelfHand, cbCardData);
            o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cbCardData, 0, Cardkind.SlefCatch, cbCardData);

            _cbSelfDeskCard[_cbDiscardCount[0]++] = CardData;
            //o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(_cbSelfDeskCard, _cbDiscardCount[GameEngine.Instance.MySelf.DeskStation], Cardkind.SlefDesk, _cbSelfDeskCard);
			o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(_cbSelfDeskCard, _cbDiscardCount[0], Cardkind.SlefDesk, _cbSelfDeskCard);

				
            o_Selfdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[0] - 1, true);
            o_Westdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[1] - 1, false);
            o_Northdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[2] - 1, false);
            o_Eastdesk_cards.GetComponent<UICardControl>().SetMaskCard(_cbDiscardCount[3] - 1, false);

            //发送数据
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OUT_CARD);
            packet.Addbyte(CardData);
            GameEngine.Instance.Send(packet);



            //音效
            PlayGameSound(SoundType.OUTCARD);

            //
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                SetUserAction(i, UserAction.NULL);
            }

            //语音
            GameSoundType bVoiceType = GameSoundType.W1;
            switch (CardData)
            {
                //万
                case 0x01: bVoiceType = GameSoundType.W1; break;
                case 0x02: bVoiceType = GameSoundType.W2; break;
                case 0x03: bVoiceType = GameSoundType.W3; break;
                case 0x04: bVoiceType = GameSoundType.W4; break;
                case 0x05: bVoiceType = GameSoundType.W5; break;
                case 0x06: bVoiceType = GameSoundType.W6; break;
                case 0x07: bVoiceType = GameSoundType.W7; break;
                case 0x08: bVoiceType = GameSoundType.W8; break;
                case 0x09: bVoiceType = GameSoundType.W9; break;

                //条
                case 0x11: bVoiceType = GameSoundType.T1; break;
                case 0x12: bVoiceType = GameSoundType.T2; break;
                case 0x13: bVoiceType = GameSoundType.T3; break;
                case 0x14: bVoiceType = GameSoundType.T4; break;
                case 0x15: bVoiceType = GameSoundType.T5; break;
                case 0x16: bVoiceType = GameSoundType.T6; break;
                case 0x17: bVoiceType = GameSoundType.T7; break;
                case 0x18: bVoiceType = GameSoundType.T8; break;
                case 0x19: bVoiceType = GameSoundType.T9; break;

                //饼
                case 0x21: bVoiceType = GameSoundType.B1; break;
                case 0x22: bVoiceType = GameSoundType.B2; break;
                case 0x23: bVoiceType = GameSoundType.B3; break;
                case 0x24: bVoiceType = GameSoundType.B4; break;
                case 0x25: bVoiceType = GameSoundType.B5; break;
                case 0x26: bVoiceType = GameSoundType.B6; break;
                case 0x27: bVoiceType = GameSoundType.B7; break;
                case 0x28: bVoiceType = GameSoundType.B8; break;
                case 0x29: bVoiceType = GameSoundType.B9; break;
                default: break;

            }
            //
            PlayUserSound(bVoiceType, GameEngine.Instance.MySelf.Gender);

        }

		void LabelEmpty()
		{
			prompt_sprite.SetActive(false);
		}



        void OnOperateTouchIvk()
        {
            //状态判断
            if ((_cbOperateCode == GameLogic.WIK_NULL))
            {
                return;
            }

            //发送命令
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(GameLogic.WIK_PENG);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);

            o_operate_buttons.SetActive(false);
        }

        void OnOperateBridgeIvk()
        {
            //状态判断
            if ((_cbOperateCode & GameLogic.WIK_GANG) == 0)
            {
                return;
            }
            //
            byte bMeChairID = GetSelfChair();
            //
            byte bActionCard = 0;

            //桌面杆牌
            if ((_wCurrentUser == -1) && (_cbOperateCard != 0))
            {
                bActionCard = _cbOperateCard;
            }

            //自己杆牌
            if (((byte)_wCurrentUser == bMeChairID) || (_cbOperateCard == 0))
            {
                //变量定义
                tagGangCardResult GangCardResult = new tagGangCardResult();
                GameLogic.AnalyseGangCard(_cbCardIndex, _WeaveItemArray, _cbWeaveCount[bMeChairID], ref GangCardResult, bMeChairID);
                //
                bActionCard = GangCardResult.cbCardData[0];
            }

            //发送命令
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(GameLogic.WIK_GANG);
            packet.Addbyte(bActionCard);
            GameEngine.Instance.Send(packet);
            o_operate_buttons.SetActive(false);
        }

        void OnOperateBeardIvk()
        {
            //状态判断
            if ((_cbOperateCode == GameLogic.WIK_NULL))
            {
                return;
            }

            //发送命令
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(GameLogic.WIK_CHI_HU);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);

            o_operate_buttons.SetActive(false);
        }



        void OnOperateGiveupIvk()
        {
            //状态判断
            if ((_cbOperateCode == GameLogic.WIK_NULL))
            {
				o_operate_buttons.SetActive(false);
                return;
            }
			if ((_cbOperateCode & GameLogic.WIK_GANG) != 0&& _wCurrentUser == GameEngine.Instance.MySelf.DeskStation)
            {	

                o_operate_buttons.SetActive(false);
				Invoke("againTimer",0.01f);
                return;
            }

            //发送命令
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(GameLogic.WIK_NULL);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);
            o_operate_buttons.SetActive(false);

        }
		void againTimer()
		{
			SetUserClock((byte)_wCurrentUser, 20, TimerType.TIMER_PLAY, false);
		}




        //保留，血战麻将没有吃牌和听牌
        void OnOperateListenIvk()
        {
            //状态判断
            if ((_cbOperateCode == GameLogic.WIK_NULL))
            {
                return;
            }

            //发送命令
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_LISTEN_CARD);
            packet.AddShort((short)GameEngine.Instance.MySelf.DeskStation);
            GameEngine.Instance.Send(packet);
            o_operate_buttons.SetActive(false);
        }
        void OnOperateEatIvk()
        {
            //变量定义
            int count = 0;

            //状态判断
            if ((_wCurrentUser == GameEngine.Instance.MySelf.DeskStation) && (_cbOperateCode == GameLogic.WIK_NULL))
            {
                return;
            }
            if (((_cbOperateCode & GameLogic.WIK_LEFT) != 0))
            {
                count++;
            }
            if ((_cbOperateCode & GameLogic.WIK_CENTER) != 0)
            {
                count++;
            }
            if (((_cbOperateCode & GameLogic.WIK_RIGHT) != 0))
            {
                count++;
            }

            o_operate_buttons.SetActive(false);

            if (count >= 2)
            {
                byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
                byte[] cbWeaveCard = new byte[GameLogic.MAX_COUNT];
                byte cbCardCount = GameLogic.SwitchToCardData(_cbCardIndex, ref cbCardData);
                byte cbWeavecardCount = GameLogic.GetWeaveCard(_cbOperateCard, ref cbWeaveCard, cbCardData, ref _cbWeaveType);
                if (cbWeavecardCount == 2)
                {

                    byte[] card = new byte[3];
                    for (int i = 0; i < 3; i++)
                    {
                        card[i] = cbWeaveCard[i + 3];
                    }
                    ctr_operateEat_cards1.SetActive(true);
                    ctr_operateEat_cards2.SetActive(true);
                    ctr_operateEat_cards1.GetComponent<UICardControl>().SetCardData(cbWeaveCard, 3, Cardkind.operateEat, cbWeaveCard);
                    ctr_operateEat_cards2.GetComponent<UICardControl>().SetCardData(card, 3, Cardkind.operateEat, card);
                }
                else if (cbWeavecardCount == 3)
                {

                    byte[] card = new byte[3];
                    byte[] cards = new byte[3];
                    for (int i = 0; i < 3; i++)
                    {
                        card[i] = cbWeaveCard[i + 3];
                        cards[i] = cbWeaveCard[i + 6];
                    }
                    ctr_operateEat_cards1.SetActive(true);
                    ctr_operateEat_cards2.SetActive(true);
                    ctr_operateEat_cards3.SetActive(true);
                    ctr_operateEat_cards1.GetComponent<UICardControl>().SetCardData(cbWeaveCard, 3, Cardkind.operateEat, cbWeaveCard);
                    ctr_operateEat_cards2.GetComponent<UICardControl>().SetCardData(card, 3, Cardkind.operateEat, card);
                    ctr_operateEat_cards3.GetComponent<UICardControl>().SetCardData(cards, 3, Cardkind.operateEat, cards);
                }


            }
            else
            {
                //发送命令
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
                packet.Addbyte(_cbOperateCode);
                packet.Addbyte(_cbOperateCard);
                GameEngine.Instance.Send(packet);
            }

        }
        void OnOperateEat1()
        {
            byte[] card = new byte[3];
            ctr_operateEat_cards1.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards2.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards3.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards1.SetActive(false);
            ctr_operateEat_cards2.SetActive(false);
            ctr_operateEat_cards3.SetActive(false);
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(_cbWeaveType[0]);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);
            Array.Clear(_cbWeaveType, 0, _cbWeaveType.Length);
        }
        void OnOperateEat2()
        {
            byte[] card = new byte[3];
            ctr_operateEat_cards1.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards2.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards3.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards1.SetActive(false);
            ctr_operateEat_cards2.SetActive(false);
            ctr_operateEat_cards3.SetActive(false);
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(_cbWeaveType[1]);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);
            Array.Clear(_cbWeaveType, 0, _cbWeaveType.Length);
        }
        void OnOperateEat3()
        {
            byte[] card = new byte[3];
            ctr_operateEat_cards1.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards2.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards3.GetComponent<UICardControl>().SetCardData(card, 0, Cardkind.operateEat, card);
            ctr_operateEat_cards1.SetActive(false);
            ctr_operateEat_cards2.SetActive(false);
            ctr_operateEat_cards3.SetActive(false);
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPERATE_CARD);
            packet.Addbyte(_cbWeaveType[2]);
            packet.Addbyte(_cbOperateCard);
            GameEngine.Instance.Send(packet);
            Array.Clear(_cbWeaveType, 0, _cbWeaveType.Length);
        }


        #endregion


        #region ##################控件事件#######################

        /////////////////////////////游戏通用/////////////////////////////


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
        }
        //
        void OnSpeakTimerEnd()
        {
            OnBtnSpeakEndIvk();
        }*/
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
                    case TimerType.TIMER_OPTION:
                        {
							//if ((_cbOperateCode != GameLogic.WIK_NULL) && _wCurrentUser == GameEngine.Instance.MySelf.DeskStation)
                            if (_cbOperateCode != GameLogic.WIK_NULL)
                            {
                                if ((_cbOperateCode & GameLogic.WIK_CHI_HU) != 0)
                                {
						
                                    OnOperateBeardIvk();
                                }
                                else
                                {
									OnOperateGiveupIvk();
									
									//BeginTuoguan();
									
                                }
                            }
                            break;
                        }
                    case TimerType.TIMER_PLAY:
                        {
                            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL, false);
							if (_wCurrentUser == GameEngine.Instance.MySelf.DeskStation)
                            {	
                                AutoOutCard();
                                BeginTuoguan();
                            }

                            break;
                        }
                    case TimerType.TIMER_LACK:
                        {
                            AutoLackCard();
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
        /*
        void OnMoveSelect()
        {

        }
        //扑克控件向上划牌事件
        void OnMoveUp()
        {

        }
        //扑克控件点击事件
        void OnCardClick()
        {

        }
        */
        //扑克控件向下划牌事件
		//出牌
        void OnMoveDown()
        {
            OnOutCard(UICardControl.CardData);
        }


        /////////////////////////////游戏特殊/////////////////////////////
        void EndTuoguan()
        {
            o_Tuoguan.SetActive(false);
            _bTrustee[GetSelfChair()] = false;
        }
        void BeginTuoguan()
        {
            o_Tuoguan.SetActive(true);
            _bTrustee[GetSelfChair()] = true;
        }
        #endregion


        #region ##################UI 控制#######################

        /////////////////////////////游戏通用/////////////////////////////





        void SetUserClock(byte chair, uint time, TimerType timertype, bool ShowOpertion)
        {
            try
            {
                o_operate_buttons.SetActive(ShowOpertion);
                o_left.SetActive(false);
                o_down.SetActive(false);
                o_up.SetActive(false);
                o_right.SetActive(false);


                if (chair != GameLogic.NULL_CHAIR)
                {
                    _bTimerType = timertype;
                    o_player_clock.GetComponent<UIClock>().SetTimer(time * 1000);

                    //
                    byte ChairID = ChairToView((byte)_wCurrentUser);
                    switch (ChairID)
                    {
                        case 0:
                            {
                                o_down.SetActive(true);
                                break;
                            }
                        case 1:
                            {
                                o_left.SetActive(true);
                                break;
                            }
                        case 2:
                            {
                                o_up.SetActive(true);
                                break;
                            }
                        case 3:
                            {
                                o_right.SetActive(true);
                                break;
                            }
                    }
                }
                else
                {
                    o_player_clock.GetComponent<UIClock>().SetTimer(0);
                    o_player_clock.SetActive(false);
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

        void UpdateUserView()
        {

			if (_bStart == false) return;
			
			for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
			{
				byte bViewId = ChairToView((byte)i);
				PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);

				if(userdata == null)
				{
					o_player_banker[bViewId].GetComponent<UISprite>().spriteName = "blank";
					o_player_banker[bViewId].SetActive(false);
				}
				
				if (userdata != null)
				{
//nick 
//                        if (userdata.VipLevel > 0)
//                        {
//                            o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
//                        }
//                        else
//                        {
//                            o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 1f, 1f);
//                        }
					o_player_nick[bViewId].GetComponent<UILabel>().text = userdata.NickName;
					
					//face
					o_player_face[bViewId].GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
					
					if (Phone == false)
					{
						OnPlayerInfoIvkPc(bViewId);
					}	
					if(GameEngine.Instance.MySelf.GameStatus == (byte)GameLogic.GS_WK_PLAYING)
					{
						showLack=true;
					}
					//准备
					if(showLack==false)
					{
						if (userdata.UserState == (byte)UserState.US_READY)
						{
							SetUserReady((byte)i, true);
						}
						else
						{
							SetUserReady((byte)i, false);
						}
						
					}
					
					o_player_money[bViewId].SetActive(true);
					o_player_money[bViewId].GetComponent<UINumber>().SetNumber(userdata.Money);
					
				}
				else
				{
					
					//nick
					o_player_nick[bViewId].GetComponent<UILabel>().text = "";
					//face
					o_player_face[bViewId].GetComponent<UIFace>().ShowFace(-1, -1);
					o_info[bViewId].SetActive(false);
					//p
					o_player_option[bViewId].GetComponent<UISprite>().spriteName="blank";
					o_player_option[i].GetComponent<UISprite>().spriteName = "blank";
					//
					o_player_money[bViewId].SetActive(false);
					
					
				}
				
				
			}


        }

        void ShowInfoBar()
        {
           
        }

        void ShowUserInfo(byte bViewID, bool showInfo)
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
					if(Phone!=true){
						o_info[bViewID].SetActive(true);
					}else{
						o_info[bViewID].SetActive(showInfo);
					}
                    o_info_nick[bViewID].GetComponent<UILabel>().text = ud.NickName;
                    o_info_lvl[bViewID].GetComponent<UILabel>().text = GameConfig.Instance.GetExpLevel((int)ud.Exp).ToString();
                    o_info_id[bViewID].GetComponent<UILabel>().text = ud.ID.ToString();
                    o_info_score[bViewID].GetComponent<UILabel>().text = ud.Money.ToString();
                    o_info_win[bViewID].GetComponent<UILabel>().text = ud.WinCount.ToString();
                    o_info_lose[bViewID].GetComponent<UILabel>().text = ud.LostCount.ToString();
                    o_info_run[bViewID].GetComponent<UILabel>().text = ud.DrawCount.ToString();

                    _nInfoTickCount = Environment.TickCount;
				}else{
					o_info[bViewID].SetActive(false);
				}

            }
        }
		//
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
            string str = PlayerPrefs.GetString("game_effect_switch", "on");

            if (str == "on" && o_speak_timer.active == false)
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
                o_player_chat[bViewID].SetActive(true);
                o_player_chat[bViewID].GetComponentInChildren<UILabel>().text = strMsg;
                //

            }
        }

        void ShowNotice(string strMsg)
        {
            //UIMsgBox.Instance.Show(true,strMsg);

        }

        void ClearAllInfo()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
				o_info[i].SetActive(false);
                o_player_chat[i].SetActive(false);

            }

        }

        void ClearAction()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                SetUserAction(i, UserAction.NULL);
            }

        }
        void ClearAllCards()
        {
            byte[] cards = new byte[1];
            o_Selfhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SelfHand, cards);
            o_Selflie_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SelfLie, cards);
            o_Selfcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SlefCatch, cards);
            o_Selfdesk_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.SlefDesk, cards);

            o_Westhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestHand, cards);
            o_Westlie_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestLie, cards);
            o_Westcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestCatch, cards);
            o_Westdesk_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.WestDesk, cards);

            o_Easthand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastHand, cards);
            o_Eastlie_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastLie, cards);
            o_Eastcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastCatch, cards);
            o_Eastdesk_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.EastDesk, cards);

            o_Northhand_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthHand, cards);
            o_Northlie_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthLie, cards);
            o_Northcatch_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthCatch, cards);
            o_Northdesk_cards.GetComponent<UICardControl>().SetCardData(cards, 0, Cardkind.NorthDesk, cards);
        }

        void SetBaseScore(int nScore)
        {

        }

        void ShowScoreAndCards()
        {
            o_card_sum.GetComponent<UILabel>().text = "底注:" + _nBaseScore.ToString() + " | 余牌:" + _cbLeftCardCount.ToString();
        }


        /////////////////////////////游戏特殊/////////////////////////////
        void AutoOutCard()
        {
            if (_wCurrentUser == GameEngine.Instance.MySelf.DeskStation)
            {
                //
                o_operate_buttons.SetActive(false);

                //必须先打缺门牌
                byte bLack = _bLackCardType[GetSelfChair()];
                for (int i = (bLack * 9); i < (bLack + 1) * 9; i++)
                {
                    if (_cbCardIndex[i] > 0)
                    {
                        OnOutCard(GameLogic.SwitchToCardData((byte)i));
                        return;
                    }
                }

                //没有缺门牌，打最右手的牌
                byte[] cbCardData = new byte[GameLogic.MAX_COUNT];
                byte cbCardCount = GameLogic.SwitchToCardData(_cbCardIndex, ref cbCardData);
                OnOutCard(cbCardData[cbCardCount - 1]);

            }
        }
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
                Invoke("CloseResultView", 7.0f);
            }

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
                o_player_money[i].SetActive(false);
                SetUserAction((byte)i, UserAction.NULL);
                o_player_hu[i].GetComponent<UISprite>().spriteName = "blank";
            }

            ClearAllCards();

            SetUserClock(GetSelfChair(), 20, TimerType.TIMER_READY, false);

            o_left.SetActive(false);
            o_down.SetActive(false);
            o_up.SetActive(false);
            o_right.SetActive(false);


            o_ready_buttons.SetActive(true);

            CancelInvoke();
        }


        void SetBanker()
        {
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_banker[i].SetActive(false);
            }


            if (_wBankerUser != GameLogic.NULL_CHAIR)
            {
                byte bViewID = ChairToView((byte)_wBankerUser);//
                o_player_banker[bViewID].SetActive(true);
                o_player_banker[bViewID].GetComponent<UISprite>().spriteName = "Zhuang_spl";
            }
        }

        void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        void SetUserAction(byte bChairID, UserAction ac)
        {
            byte bViewId = ChairToView(bChairID);

            switch (ac)
            {
                case UserAction.NULL: o_player_action[bViewId].GetComponent<UISprite>().spriteName = "blank"; break;
                case UserAction.LACK: o_player_action[bViewId].GetComponent<UISprite>().spriteName = "Lack_spl"; break;

                case UserAction.PENG:
                    {
                        ShowTween(bViewId, 0);
                        break;
                    }
                case UserAction.GANG:
                    {
                        ShowTween(bViewId, 1);
                        break;
                    }
                case UserAction.HU:
                    {
                        ShowTween(bViewId, 2);
                        break;
                    }
                case UserAction.GF:
                    {
                        ShowTween(bViewId, 3);
                        break;
                    }
                case UserAction.XY:
                    {
                        ShowTween(bViewId, 4);
                        break;
                    }

                case UserAction.ZM:
                    {
                        ShowTween(bViewId, 5);
                        break;
                    } 

            }

			if(o_player_action[bViewId].GetComponent<UISprite>().spriteName!="blank"&&o_player_action[bViewId].GetComponent<UISprite>().spriteName!="desc_xuanque")
			{
				StartCoroutine(ClearUserAction(bViewId));
			}

        }

        void ShowTween(byte id, int indecCount)
        {
            o_player_action[id].GetComponent<UISprite>().spriteName = "blank";
            o_player_action[id].GetComponent<MJAnimationController>().m_AnimationIndex = indecCount;
            o_player_action[id].GetComponent<MJAnimationController>().Play();
        }

		IEnumerator ClearUserAction(byte bViewId)
		{
			yield return new WaitForSeconds(1.0f);
			o_player_action[bViewId].GetComponent<UISprite>().spriteName="blank";
		}

        void SetUserLack(byte bChairID, byte black)
        {
            byte bViewId = ChairToView(bChairID);
			showLack=true;
            o_player_option[bViewId].SetActive(true);
            switch (black)
            {
                case 0: o_player_option[bViewId].GetComponent<UISprite>().spriteName = "desc_que_wan"; break;
                case 1: o_player_option[bViewId].GetComponent<UISprite>().spriteName = "desc_que_tiao"; break;
                case 2: o_player_option[bViewId].GetComponent<UISprite>().spriteName = "desc_que_tong"; break;
                default: o_player_option[bViewId].GetComponent<UISprite>().spriteName = "blank"; break;
            }
        }

        bool CheckScoreLimit()
        {
            ////金币限制检测
            //int nLimit = 0;
            ///*if (GameEngine.Instance.MyUser.ServerUsed.StationID.ToString().EndsWith("39") == true)
            //{
            //    nLimit = 10000;
            //}
            //else*/
            //{
            //    nLimit = 50 * _nBaseScore;
            //}

            //if (GameEngine.Instance.MySelf == null || GameEngine.Instance.MySelf.Money < nLimit)
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