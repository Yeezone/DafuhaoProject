using UnityEngine;
using System.Collections;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Data;
using System.Collections.Generic;

namespace com.QH.QPGame.CX
{

    #region ##################结构定义#######################
    public enum TimerType
    {
        TIMER_NULL = 0,
        TIMER_READY = 1,
        TIMER_INVERT = 2,
        TIMER_ADDSCORE = 3,
        TIMER_OPEN = 4
    };

    public enum SoundType
    {
        FLLOW = 0,
        MORE = 1,
        ALL = 2,
        SLEEP = 3,
        LOST = 4
    };
    public enum GameSoundType
    {
        READY = 0,      //准备
        BGM = 1,        //背景音乐
        START = 2,      //游戏开始
        SENDCARD = 3,   //发牌
        CLICK = 4,      //点击
        SETOK = 5,      //设置啵啵数
        EARN = 6,       //结束收钱
        BANKER = 7,     //设置庄家
        SETSCORE = 8,  //投币
        OPENCARD = 9   //开牌
    };

    public enum TimerDelay
    {
        NULL = 0,
        QIANG = 5,
        CHIP = 5,
        OPEN = 20,
        READY = 20,
        Invert = 15
    };

    public enum GamePlatform	//游戏平台
    {
        NN_ForPC,
        NN_ForMobile
    }
    public enum BetType
    {
        Follow = 1,
        Big = 2,
        Knock = 3,
        Stop = 4,
        Lose = 5
    }

    /// <summary>
    /// 游戏结束原因
    /// </summary>
    public enum GameExitType : byte
    {
        /// <summary>
        /// //强退结束
        /// </summary>
        END_REASON_EXIT = 1,
        /// <summary>
        /// //让牌结束
        /// </summary>
        END_REASON_PASS = 2,
        /// <summary>
        /// //正常结束
        /// </summary>
        END_REASON_NORMAL = 3,
        /// <summary>
        /// //放弃结束
        /// </summary>
        END_REASON_GIVEUP = 4,
    }

    #endregion


    public class UIGame : MonoBehaviour
    {
        #region ##################变量定义######################
        //游戏是否开牌中
        private bool IsOpenCard = false;

        public  long currentBoboshu = 0;
        private int setBoboshuCount = 0;

        //帮助面板
        private GameObject o_HelpPanel = null;

        //是否播放开始动画
        private bool startTween = true;
        //开始动画
        public GameObject startTweenObj;

        //筹码框显示筹码
        public GameObject[] o_ChipFrame = new GameObject[3];
        //筹码个数
        private byte chipCount = 0;
        //当前筹码数值
        private int[] chipValue = new int[3];

        //通用数据
        bool _bStart = false;
        public GamePlatform curGamePlatform; 	//当前游戏平台
        /// <summary>
        /// 开始
        /// </summary>
        bool _bStartGame = false;
        /// <summary>
        /// 结算
        /// </summary>
        public static bool _bEndGame = false;

        //明牌开关
        bool _bMing = true;

        public static bool _IsSplit = false;

        int _nQuitDelay = 0;
        bool _bReqQuit = false;
        /// <summary>
        /// 定时类型
        /// </summary>
        static TimerType _bTimerType = TimerType.TIMER_NULL;
        /// <summary>
        /// 定时显示
        /// </summary>
        GameObject[] o_player_clock = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// //最大下注
        /// </summary>
        long _lTurnMaxScore = 0;
        /// <summary>
        /// //最小下注
        /// </summary>
        long _lTurnLessScore = 0;

        /// <summary>
        /// 游戏结束原因
        /// </summary>
        GameExitType _gameExitReasion = GameExitType.END_REASON_NORMAL;

        /// <summary>
        /// 下本
        /// </summary>
        long[] _linvertscore = new long[GameLogic.GAME_PLAYER];

        //地皮
        long[] _lAllCellScore = new long[GameLogic.GAME_PLAYER];

        /// <summary>
        /// 最小加注
        /// </summary>
        public static int MIN_ADDSCORE = 0;

        /// <summary>
        /// 下底
        /// </summary>
        //public static int BASE_SCORE = 1;

        /// <summary>
        /// 当前USER
        /// </summary>
        byte _bCurrentUser = GameLogic.NULL_CHAIR;
        byte _bBankerUser = GameLogic.NULL_CHAIR;
        byte _firstUser = GameLogic.NULL_CHAIR;
		byte _addScoreUser = GameLogic.NULL_CHAIR;
        long _lOneScore = 0;

        /// <summary>
        /// 发牌总数
        /// </summary>
        byte _bCardNum = 2;
        /// <summary>
        /// 上次牌总数
        /// </summary>
        byte _bBackCardNum = 0;
        //下注数目
        int[] _lTableScore = new int[GameLogic.GAME_PLAYER];
        //游戏状态
        public static byte[] _bPlayStatus = new byte[GameLogic.GAME_PLAYER];


        public int _lCellScore = 0;	  //底注 

        byte[][] _bHandCardData = new byte[GameLogic.GAME_PLAYER][];

		//游戏结束，卡牌存放
		byte[][] EndHandCardData = new byte[GameLogic.GAME_PLAYER][];

        //储存游戏中的玩家（手牌有四张的玩家）
		int[] cardDataCount = new int[GameLogic.GAME_PLAYER];    //id
        int nCardCount = 0;                                      //人数

        /// <summary>
        /// 开始两张牌,点击显示
        /// </summary>
        public byte[] _bMyCardData = new byte[2];

        bool[] _bUserTrustee = new bool[GameLogic.GAME_PLAYER];

        bool[] _bUserInvent = new bool[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 头像
        /// </summary>
        GameObject[] o_player_face = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 名字
        /// </summary>
        GameObject[] o_player_nick = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 准备
        /// </summary>
        GameObject[] o_player_option = new GameObject[GameLogic.GAME_PLAYER];
        GameObject[] o_player_cards = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 玩家下注
        /// </summary>
        UILabel[] l_player_addscore = new UILabel[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 设置簸簸数
        /// </summary>
        UILabel[] l_player_bobonum = new UILabel[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 玩家下注
        /// </summary>
        public static GameObject[] o_player_chip = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 金币回收位置
        /// </summary>
        public  GameObject[] player_head_pos = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 玩家信息
        /// </summary>
        GameObject[] o_player_obj = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 簸簸好了
        /// </summary>
        GameObject[] o_player_bobook = new GameObject[GameLogic.GAME_PLAYER];

        /// <summary>
        /// 庄-标志
        /// </summary>
        GameObject[] o_player_flag = new GameObject[GameLogic.GAME_PLAYER];
        /// <summary>
        /// 下注时,操作
        /// </summary>
        GameObject[] o_player_action = new GameObject[GameLogic.GAME_PLAYER];

        /// <summary>
        /// 自己的金币
        /// </summary>
        UILabel l_player_money = null;

        GameObject o_player_info = null;

        /// <summary>
        /// 玩家名字
        /// </summary>
        UILabel l_player_name = null;
        /// <summary>
        /// 玩家总下注
        /// </summary>
        public static GameObject o_player_allinvert = null;

        Dictionary<byte, long> _playerInvest = new Dictionary<byte, long>();

        /// <summary>
        /// 设置簸簸数界面
        /// </summary>
        private GameObject o_setwinnows = null;
        /// <summary>
        /// 分牌界面
        /// </summary>
        private GameObject o_splitcards = null;
        /// <summary>
        /// 结算界面
        /// </summary>
        private GameObject o_settlement = null;
        /// <summary>
        /// 设置界面
        /// </summary>
        private GameObject o_gamesetting = null;

        //消息面板
        private GameObject o_MsgBox = null;
        private GameObject o_Msg = null;

        //声音面板
        private GameObject o_Music = null;
        public static GameObject o_backGroundMusic = null;
        public static GameObject o_effectMusic = null;
        private GameObject o_btn_bgm = null;
        private GameObject o_btn_effect = null;
        public static float _musicVol = 0.5f;
        public static float _effectVol = 0.5f;

        /// <summary>
        /// 加注金币显示
        /// </summary>
        GameObject o_dlg_chips = null;

        GameObject o_lbl_cellScore = null;

        GameObject o_lbl_maxScore = null;

        private GameObject btn_ready = null;

        #region Top_Bar
        /// <summary>
        /// 结束
        /// </summary>
        private GameObject btn_exit = null;

        private GameObject btn_help = null;
		public GameObject btn_horn = null;
		private GameObject btn_menu = null;
        /// <summary>
        /// 设置-音效
        /// </summary>
        private GameObject btn_setting = null;
        #endregion

        #region 选择跟大敲休丢
        private GameObject obj_option = null;

        private GameObject btn_follow = null;   //跟
        private GameObject btn_big = null;      //大
        private GameObject btn_knock = null;    //敲
        private GameObject btn_stop = null;     //休
        private GameObject btn_lose = null;     //丢
        private UIButton uibtn_big = null;
        private UIButton uibtn_follow = null;
        private UIButton uibtn_stop = null;
        #endregion
        private GameObject obj_addscore = null;
        private cx_number _addscoreObj = null;
        /// <summary>
        /// 加注-20-500界面
        /// </summary>
        private GameObject o_bet_bg = null;

        private GameObject o_bet_shade = null;
        #region 大-加注
        private GameObject btn_bet20 = null;
        private GameObject btn_bet50 = null;
        private GameObject btn_bet100 = null;
        private GameObject btn_bet200 = null;
        private GameObject btn_bet500 = null;
        private GameObject btn_betmax = null;
        private GameObject btn_betmin = null;

        private UIButton uibtn_bet20 = null;
        private UIButton uibtn_bet50 = null;
        private UIButton uibtn_bet100 = null;
        private UIButton uibtn_bet200 = null;
        private UIButton uibtn_bet500 = null;
        #endregion

        //音效
        public AudioClip[] _GameSound = new AudioClip[20];
        public AudioClip[] _WomanSound = new AudioClip[30];
        public AudioClip[] _ManSound = new AudioClip[30];

        /// <summary>
        /// 选择大-加注 时数据
        /// </summary>
        long _laddscore = 0;
        //bool _bisaddscore = false;


        PlayerInfo[] _userdata = new PlayerInfo[GameLogic.GAME_PLAYER];

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

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_face[i] = transform.Find("dlg_player_" + i.ToString() + "/ctr_user_face").gameObject;
                o_player_nick[i] = transform.Find("dlg_player_" + i.ToString() + "/lbl_nick").gameObject;
                o_player_option[i] = transform.Find("dlg_player_" + i.ToString() + "/sp_option").gameObject;
                o_player_cards[i] = transform.Find("dlg_player_" + i.ToString() + "/ctr_hand_cards").gameObject;

                o_player_bobook[i] = transform.Find("dlg_player_" + i.ToString() + "/sp_bobook").gameObject;

                o_player_obj[i] = transform.Find("dlg_player_" + i.ToString()).gameObject;

                l_player_addscore[i] = transform.Find("dlg_player_" + i.ToString() + "/sp_infos/addscoreLabel").GetComponent<UILabel>();
                l_player_bobonum[i] = transform.Find("dlg_player_" + i.ToString() + "/sp_infos/bobonumLabel").GetComponent<UILabel>();

                _bHandCardData[i] = new byte[GameLogic.MAX_COUNT];

                o_player_chip[i] = transform.Find("dlg_player_" + i.ToString() + "/ctr_chips").gameObject;

                o_player_flag[i] = transform.Find("dlg_player_" + i.ToString() + "/sp_flag").gameObject;

                o_player_action[i] = transform.Find("dlg_player_" + i.ToString() + "/player_action").gameObject;

                o_player_clock[i] = transform.Find("ctr_clock_" + i.ToString()).gameObject;
                o_player_clock[i].SetActive(false);

            }
            //只能看自己的
            UIEventListener.Get(o_player_cards[0]).onHover = OnHover;
            UIEventListener.Get(o_player_cards[0]).onPress = OnHover;

            //o_player_clock = transform.Find("ctr_clock").gameObject;
            o_dlg_chips = transform.Find("ctr_chips").gameObject;

            o_lbl_cellScore = transform.Find("Top_bar/dlg_title_bar/lbl_base_score").gameObject;

            o_lbl_maxScore = transform.Find("Top_bar/dlg_title_bar/lbl_max_score").gameObject;

            o_player_info = transform.Find("ButtonInfo_bar").gameObject;

            l_player_money = transform.Find("ButtonInfo_bar/Info/mymoney").GetComponent<UILabel>();

            l_player_name = transform.Find("ButtonInfo_bar/Info/Label").GetComponent<UILabel>();

            o_player_allinvert = transform.Find("PlayerALLInvert").gameObject;

            o_bet_bg = transform.Find("dlg_option_buttons/bg2").gameObject;

            o_bet_shade = transform.Find("dlg_option_buttons/shade").gameObject;

            o_setwinnows = transform.parent.Find("SetWinnows").gameObject;
            o_splitcards = transform.parent.Find("SplitCard").gameObject;
            o_settlement = transform.parent.Find("Settlement").gameObject;
            o_gamesetting = transform.parent.Find("Setting").gameObject;

            obj_option = transform.Find("dlg_option_buttons").gameObject;

            btn_follow = transform.Find("dlg_option_buttons/bg1/btn_step01").gameObject;
            btn_big = transform.Find("dlg_option_buttons/bg1/btn_step02").gameObject;
            btn_knock = transform.Find("dlg_option_buttons/bg1/btn_step03").gameObject;
            btn_stop = transform.Find("dlg_option_buttons/bg1/btn_step04").gameObject;
            btn_lose = transform.Find("dlg_option_buttons/bg1/btn_step05").gameObject;

            uibtn_big = btn_big.GetComponent<UIButton>();
            uibtn_big.isEnabled = false;
            uibtn_follow = btn_follow.GetComponent<UIButton>();
            uibtn_stop = btn_stop.GetComponent<UIButton>();

            btn_ready = transform.Find("dlg_ready_buttons/btn_ready").gameObject;

            btn_exit = transform.Find("Top_bar/gameclose").gameObject;
            btn_help = transform.Find("Top_bar/gamehelp").gameObject;
			btn_horn = transform.Find("Top_bar/gamehorn").gameObject;
			btn_menu = transform.Find("Top_bar/gamemenu").gameObject;
            //btn_help.GetComponent<UIButton>().isEnabled = false;
			//btn_horn.GetComponent<UIButton>().isEnabled = false;
            btn_setting = transform.Find("Top_bar/gameset").gameObject;

            _addscoreObj = transform.Find("dlg_option_buttons/bg1/addscore").GetComponent<cx_number>();
            obj_addscore = transform.Find("dlg_option_buttons/bg2").gameObject;

            o_MsgBox = transform.parent.Find("scene_msgbox").gameObject;
            o_Msg = transform.parent.Find("scene_msgbox/lbl_msgbox_msg").gameObject;

            o_Music = transform.parent.Find("Setting").gameObject;
            o_backGroundMusic = transform.parent.Find("Setting/BGMusic/bgpro").gameObject;
            o_effectMusic = transform.parent.Find("Setting/GameMusic/bgpro").gameObject;
            o_btn_bgm = transform.parent.Find("Setting/BGMusic/bgsilence").gameObject;
            o_btn_effect = transform.parent.Find("Setting/GameMusic/gamesilence").gameObject;


            btn_bet20 = transform.Find("dlg_option_buttons/bg2/btn_bet20").gameObject;
            btn_bet50 = transform.Find("dlg_option_buttons/bg2/btn_bet50").gameObject;
            btn_bet100 = transform.Find("dlg_option_buttons/bg2/btn_bet1").gameObject;
            btn_bet200 = transform.Find("dlg_option_buttons/bg2/btn_bet2").gameObject;
            btn_bet500 = transform.Find("dlg_option_buttons/bg2/btn_bet3").gameObject;
            btn_betmax = transform.Find("dlg_option_buttons/bg2/btn_betmax").gameObject;
            btn_betmin = transform.Find("dlg_option_buttons/bg2/btn_betmin").gameObject;

            uibtn_bet20 = btn_bet20.GetComponent<UIButton>();
            uibtn_bet50 = btn_bet50.GetComponent<UIButton>();
            uibtn_bet100 = btn_bet100.GetComponent<UIButton>();
            uibtn_bet200 = btn_bet200.GetComponent<UIButton>();
            uibtn_bet500 = btn_bet500.GetComponent<UIButton>();

            o_HelpPanel = GameObject.Find("helpPanel");

            AddUIEvent();
        }
        /// <summary>
        /// 查看自己的牌
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isHover"></param>

        void OnHover(GameObject obj, bool isHover)
        {
            if (GetSelfChair() < GameLogic.GAME_PLAYER)
            {
                if (_bMing == false || _bPlayStatus[GetSelfChair()] == (byte)PlayerState.GIVEUP)
                {
                    return;
                }
            }

            byte[] tmpcards = new byte[2];
            if (isHover)
            {
                Buffer.BlockCopy(_bMyCardData, 0, tmpcards, 0, 2);
            }
            o_player_cards[0].GetComponent<UICardControl>().SetCardDataHover(tmpcards, 2);
        }

        void AddUIEvent()
        {
            UIEventListener.Get(btn_ready).onClick = OnClick;
            UIEventListener.Get(btn_exit).onClick = OnClick;

            UIEventListener.Get(btn_setting).onClick = OnClick;
            UIEventListener.Get(btn_help).onClick = OnClick;
            UIEventListener.Get(btn_horn).onClick = OnClick;
			UIEventListener.Get(btn_menu).onClick = OnClick;

            UIEventListener.Get(btn_follow).onClick = OnClick;
            UIEventListener.Get(btn_big).onClick = OnClick;
            UIEventListener.Get(btn_knock).onClick = OnClick;
            UIEventListener.Get(btn_stop).onClick = OnClick;
            UIEventListener.Get(btn_lose).onClick = OnClick;

            UIEventListener.Get(btn_bet20).onClick = OnClick;
            UIEventListener.Get(btn_bet50).onClick = OnClick;
            UIEventListener.Get(btn_bet100).onClick = OnClick;
            UIEventListener.Get(btn_bet200).onClick = OnClick;
            UIEventListener.Get(btn_bet500).onClick = OnClick;
            UIEventListener.Get(btn_betmax).onClick = OnClick;
            UIEventListener.Get(btn_betmin).onClick = OnClick;
        }

        void OnClick(GameObject obj)
        {
            PlayGameSound(GameSoundType.CLICK);

            if (obj.name.Equals("btn_ready"))
            {
                OnBtnReadyIvk();

                GameObject tpobj = GameObject.Find("UI_Marquee_Default(Clone)");
                if (tpobj != null)
                {
                    tpobj.SetActive(false);
                }
                o_player_clock[0].GetComponent<UIClock>().SetTimer(0);
                o_player_clock[0].SetActive(false);
            }
            else if (0 == obj.name.CompareTo("gameset"))
            {
                o_gamesetting.SetActive(true);
                UISetting uisetting = o_gamesetting.GetComponent<UISetting>();
                if (uisetting == null)
                {
                    uisetting = o_gamesetting.AddComponent<UISetting>();
                }
            }
            else if (obj.name.Equals("gameclose"))
            {
                if (!GameEngine.Instance.IsPlaying() && IsOpenCard == false)
                {
                    OnConfirmBackOKIvk();
                }
                else
                {
                    MsgShow(true, "游戏还没有结束，再玩玩吧!");
                }
            }
            else if (obj.name.Contains("btn_step"))
            {
                OnClickOptionStep(obj.name);
            }
            else if (obj.name.Contains("btn_bet"))
            {
                OnClickBet(obj.name);
            }
            else if (obj.name.Equals("gamehelp"))
            {
                OnClickHelp();
            }
            else if (obj.name.Equals("gamehorn"))
            {
                OnBtnVoiceIvk();
            }
			else if(obj.name.Equals("gamemenu"))
			{
				OnBtnMenuIvk();
			}
        }

		//菜单按钮
		void OnBtnMenuIvk()
		{
			Vector3 originPos = new Vector3(-490, 47, 0);
			if(btn_setting.transform.localPosition == originPos)
			{
				btn_setting.transform.GetComponent<TweenPosition>().enabled = true;
				btn_help.transform.GetComponent<TweenPosition>().enabled = true;
				btn_exit.transform.GetComponent<TweenPosition>().enabled = true;
				btn_exit.transform.GetComponent<TweenPosition>().PlayForward();
				btn_setting.transform.GetComponent<TweenPosition>().PlayForward();
				btn_help.transform.GetComponent<TweenPosition>().PlayForward();
			}
			else
			{
				btn_setting.transform.GetComponent<TweenPosition>().enabled = true;
				btn_help.transform.GetComponent<TweenPosition>().enabled = true;
				btn_exit.transform.GetComponent<TweenPosition>().enabled = true;
				btn_exit.transform.GetComponent<TweenPosition>().PlayReverse();
				btn_setting.transform.GetComponent<TweenPosition>().PlayReverse();
				btn_help.transform.GetComponent<TweenPosition>().PlayReverse();
			}

		}

        /// <summary>
        /// 大-加注
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBet(string name)
        {
            //_laddscore = _lTurnLessScore;
            string str = name.Substring(7);
            if (str.Equals("max"))
            {
                _laddscore = _lTurnMaxScore;
                if (_laddscore == 0)
                {
                    _laddscore = _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore;
                }
            }
            else if (str.Equals("min"))
            {
                _laddscore = _lTurnLessScore;
                if (_laddscore == 0)
                {
                    _laddscore = MIN_ADDSCORE;
                }
            }

            if (!UISetWinnows.Isint(str) || _bCurrentUser != GetSelfChair())
            {
                SetCurrAddScore();
                return;
            }
            
            int addscoreindex = int.Parse(str);
            int addscore = 0;
            switch (addscoreindex)
            {
                case 1: addscore = _lCellScore; break;
                case 2: addscore = _lCellScore * 10; break;
                case 3: addscore = _lCellScore * 100; break;
                default: addscore = _lCellScore; break;
            }

            //点加注
            if (_laddscore == 0)
            {
                _laddscore += (addscore + MIN_ADDSCORE);
            }
            else
            {
                _laddscore += addscore;
            }

            if(chipCount < 3)
            {
                int chipcount = 0;

                if (chipCount < 1)
                {
                    chipcount = 0;
                }
                else
                {
                    chipcount = chipCount - 1;
                }
                if (chipValue[chipCount] != addscore && chipValue[chipCount] == 0 && chipValue[chipcount] != addscore && chipValue[0] != addscore)
                {
                    chipValue[chipCount] = addscore;
					ShowFrameChip(chipCount, addscore);
					chipCount++;
                }
            }

            SetCurrAddScore();
        }

        void ShowFrameChip(byte chipIndex, int chipScore)
        {
			int nameindex = 0;

            if (_lCellScore == chipScore) nameindex = 0;
            if (_lCellScore * 10 == chipScore) nameindex = 1;
            if (_lCellScore * 100 == chipScore) nameindex = 2;

			o_ChipFrame[chipIndex].GetComponent<UISprite>().spriteName = "frameChip_" + nameindex.ToString();
			o_ChipFrame[chipIndex].transform.Find("value").GetComponent<cx_number>().m_iNum = chipScore;
			o_ChipFrame[chipIndex].SetActive(true);
        }
        void StartFrameChip()
        {
            for (int i = 0; i < 3; i++)
            {
                chipCount = 0;
                chipValue[i] = 0;
                o_ChipFrame[i].transform.Find("value").GetComponent<cx_number>().m_iNum = 0;
                o_ChipFrame[i].SetActive(false);
            }
        }

        /// <summary>
        /// 显示当前加注
        /// </summary>
        void SetCurrAddScore()
        {
            if (_laddscore > _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore)
            {
                _laddscore = _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore;
            }
            if (_laddscore == 0)
            {
                _addscoreObj.m_iNum = _lCellScore;
            }
            else
            {
                _addscoreObj.m_iNum = _laddscore;
            }

            //跟
            if (_addscoreObj.m_iNum == _lTurnLessScore && _lTurnLessScore != 0)
            {
                uibtn_follow.isEnabled = true;
            }

            //大
            if (_addscoreObj.m_iNum <= _lTurnLessScore)
            {
                uibtn_big.isEnabled = false;
            }
            
            if (_addscoreObj.m_iNum > _lTurnLessScore && _lTurnLessScore != 0 )
            {
                uibtn_big.isEnabled = true;
            }

            if (_addscoreObj.m_iNum - _lCellScore > _lTurnLessScore && _lTurnLessScore == 0)
            {
                uibtn_big.isEnabled = true;
            }
            

            //敲
            if (_addscoreObj.m_iNum >= _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore)
            {
                uibtn_follow.isEnabled = false;
                uibtn_big.isEnabled = false;
            }
        }
        /// <summary>
        /// 选择跟大敲休丢
        /// </summary>
        /// <param name="name"></param>
        void OnClickOptionStep(string name)
        {
            StartFrameChip();
            string str = name.Substring(8);
            if (!UISetWinnows.Isint(str) || _bCurrentUser != GetSelfChair())
            {
                return;
            }
            long tmpaddscore = _laddscore;
            switch ((BetType)int.Parse(str))
            {
                case BetType.Follow:
                    {
                        tmpaddscore = _lTurnLessScore;
                        if (tmpaddscore == 0)
                        {
                            tmpaddscore = MIN_ADDSCORE;
                        }

                        int myAddType = UnityEngine.Random.Range(0 * 5, 0 * 5 + 5 - 1);
                        PlayUserSound(myAddType, GameEngine.Instance.MySelf.Gender);

                        break;
                    }
                case BetType.Big:
                    {
                        tmpaddscore = _addscoreObj.m_iNum;
                        if (tmpaddscore > _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore)
                        {
                            tmpaddscore = _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore;
                        }
                        int myAddType = UnityEngine.Random.Range(1 * 5, 1 * 5 + 5 - 1);
                        PlayUserSound(myAddType, GameEngine.Instance.MySelf.Gender);

                        break;
                    }
                case BetType.Knock:
                    {
                        tmpaddscore = _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore;

                        int myAddType = UnityEngine.Random.Range(2 * 5, 2 * 5 + 5 - 1);
                        PlayUserSound(myAddType, GameEngine.Instance.MySelf.Gender);

                        break;
                    }
                case BetType.Stop:
                    {
                        tmpaddscore = 0;
                        int myAddType = UnityEngine.Random.Range(3 * 5, 3 * 5 + 5 - 1);
                        PlayUserSound(myAddType, GameEngine.Instance.MySelf.Gender);
                        break;
                    }
                case BetType.Lose:
                    {
                        int myAddType = UnityEngine.Random.Range(4 * 5, 4 * 5 + 5 - 1);
                        PlayUserSound(myAddType, GameEngine.Instance.MySelf.Gender);

                        NPacket packet = NPacketPool.GetEnablePacket();
                        packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GIVE_UP);
                        SendMsgToServer(packet);
                        _bPlayStatus[GetSelfChair()] = (byte)PlayerState.GIVEUP;
                        return;
                    }
            }
            //此处添加加注状态判断
            if (_bPlayStatus[GetSelfChair()] == (byte)PlayerState.PLAY)
            {
                NPacket packet1 = NPacketPool.GetEnablePacket();
                packet1.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_ADD_SCORE);
                packet1.AddLong(tmpaddscore);
                SendMsgToServer(packet1);
            }

            BetType betType = (BetType)int.Parse(str);
            SetInvertAction(_bCurrentUser, betType, tmpaddscore);
        }

        //帮助界面显示与关闭
        void OnClickHelp()
        {
            o_HelpPanel.SetActive(true);
        }
        public void HideHelpPanel()
        {
            o_HelpPanel.SetActive(false);
        }

        public void Update()
        {
            if (GameEngine.Instance == null)
            {
                return;
            }
            if (curGamePlatform == GamePlatform.NN_ForPC)
            {
            }
            else
            {
            }
        }


        void InitGameView()
        {
            PlayerInfo myinfo = GameEngine.Instance.GetTableUserItem(GetSelfChair());
            l_player_money.text = myinfo.Money.ToString();
            l_player_name.text = myinfo.NickName;
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_clock[i].SetActive(false);

                byte bViewId = ChairToView((byte)i);
                l_player_addscore[bViewId].text = "";
                l_player_bobonum[bViewId].text = "";
                o_player_action[i].SetActive(false);

                _linvertscore[i] = 0;
                _lTableScore[i] = 0;
                _lAllCellScore[i] = 0;
                _bUserInvent[i] = false;
                o_player_flag[i].SetActive(false);
                _userdata[i] = null;
            }
            o_player_allinvert.SetActive(true);
            _bMing = true;
            _IsSplit = false;
            _firstUser = GameLogic.NULL_CHAIR;
			_addScoreUser = GameLogic.NULL_CHAIR;
            _lOneScore = 0;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _bBankerUser = GameLogic.NULL_CHAIR;

            _lTurnMaxScore = 0;
            _lTurnLessScore = 0;
            _laddscore = 0;
            //缓存玩家信息，结算用
            for (byte j = 0; j < GameLogic.GAME_PLAYER; ++j)
                {
                _userdata[j] = GameEngine.Instance.EnumTablePlayer(j);
                }

			for (byte j = 0; j < GameLogic.GAME_PLAYER; ++j)
			{
				EndHandCardData[j] = new byte[4];
			}

            for (int i = 0; i < 3; i++)
            {
                chipValue[i] = 0;
                o_ChipFrame[i].transform.Find("value").GetComponent<cx_number>().m_iNum = 0;
                o_ChipFrame[i].SetActive(false);
            }

            o_HelpPanel.SetActive(false);
            btn_horn.GetComponent<UISprite>().spriteName = "voice_play";
            obj_option.SetActive(true);

            btn_bet100.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = _lCellScore;
            btn_bet200.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = _lCellScore * 10;
            btn_bet500.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = _lCellScore * 100;

            ForbiddenChipBtn(false);
        }
        void ResetGameView()
        {
            PlayerInfo myinfo = GameEngine.Instance.GetTableUserItem(GetSelfChair());
            l_player_money.text = myinfo.Money.ToString();
            l_player_name.text = myinfo.NickName;
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_clock[i].SetActive(false);
                byte bViewId = ChairToView((byte)i);
                l_player_addscore[bViewId].text = "";
                l_player_bobonum[bViewId].text = "";
                o_player_action[i].SetActive(false);

                _linvertscore[i] = 0;
                _lTableScore[i] = 0;
                _lAllCellScore[i] = 0;
                _bUserInvent[i] = false;
                o_player_flag[i].SetActive(false);
                _userdata[i] = null;
            }
            o_player_allinvert.SetActive(true);
            _bMing = true;
            _IsSplit = false;
            _firstUser = GameLogic.NULL_CHAIR;
			_addScoreUser = GameLogic.NULL_CHAIR;
            _lOneScore = 0;
            _bCurrentUser = GameLogic.NULL_CHAIR;
            _bBankerUser = GameLogic.NULL_CHAIR;

            _lTurnMaxScore = 0;
            _lTurnLessScore = 0;
            _laddscore = 0;
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
                // UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        void FixedUpdate()
        {
            OnEffectChange();
            OnMusicChange();
        }
        #endregion


        #region ##################框架消息#######################

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (subcmd)
            {
                case SubCmd.SUB_GF_OPTION:
                    {
                        OnGameOptionResp(packet);
                        break;
                    }
                case SubCmd.SUB_GF_SCENE:
                    {
                        currentBoboshu = 0;
                        setBoboshuCount = 0;
                        transform.Find("repeat_boboshu").gameObject.SetActive(false);

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
                        //缓存玩家信息，结算用
                        for (byte j = 0; j < GameLogic.GAME_PLAYER; ++j)
                        {
                            _userdata[j] = GameEngine.Instance.EnumTablePlayer(j);
                        }

                        break;
                    }
                case TableEvents.WAIT_DISTRIBUTE:
                    {
                        UIWaiting.Instance.CallBack = new WaitingCancelCall(OnConfirmBackOKIvk);
                        UIWaiting.Instance.Show(true);
                        o_player_info.SetActive(false);
                        btn_ready.SetActive(false);
                        o_HelpPanel.SetActive(false);
                        obj_option.SetActive(false);                        
                        o_bet_shade.SetActive(true);
                        ForbiddenChipBtn(false);
                        transform.Find("Top_bar").gameObject.SetActive(false);
                        transform.Find("dlg_player_0").gameObject.SetActive(false);
                        startTween = false;

                        for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
                        {
                            o_player_obj[i].SetActive(false);
                        }

                        break;
                    }
                case TableEvents.GAME_START:
                    {
                        Invoke("ClearUserReady", 1.0f);

                        //缓存玩家信息，结算用
                        for (byte j = 0; j < GameLogic.GAME_PLAYER; ++j)
                            {
                            _userdata[j] = GameEngine.Instance.EnumTablePlayer(j);
                            }
                        break;
                    }
                case TableEvents.GAME_LOST:
                    {
                        UIMsgBox.Instance.Show(true, GameMsg.MSG_CM_008);
                        Invoke("OnConfirmBackOKIvk", 3.0f);
                        break;
                    }
                case TableEvents.GAME_FINISH:
                    {
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
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {

            switch (bGameStatus)
            {
                case (byte)GameLogic.GS_TK_FREE: //空闲
                    {
                        SwitchFreeSceneView(packet);
                        break;
                    }

                case (byte)GameLogic.GS_TK_INVEST://下本
                    {
                        setBoboshuCount++; //设置簸簸数次数

                        SwitchInvestSceneView(packet);
                        break;
                    }

                case (byte)GameLogic.GS_TK_SCORE://下注 
                    {
                        SwitchScoreSceneView(packet);
                        break;
                    }

                case (byte)GameLogic.GS_TK_OPEN_CARD://摊牌
                    {
                        SwitchOpenCardSceneView(packet);
                        break;
                    }
            }
        }

        //用户准备消息处理函数
        void OnUserReadyResp(NPacket packet)
        {
            UpdateUserView();
            PlayGameSound(GameSoundType.READY);
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
                case SubCmd.SUB_S_GAME_START:
                    {
                        //抢庄结束，游戏开始
                        StartCoroutine(ShowStartTween(packet));
                        break;
                    }
                case SubCmd.SUB_S_USER_INVEST:
                    {
                        //玩家下本
                        OnGameStartInvest(packet);
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
                case SubCmd.SUB_S_GIVE_UP:
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
                case SubCmd.SUB_S_OPEN_START:
                    {
                        OnStartSlipCardResp(packet);
                        break;
                    }

            }
            UpdateUserView();
        }

        //开始动画
        IEnumerator ShowStartTween(NPacket packet)
        {
            if (startTween == true)
            {
                GameObject obj = Instantiate(startTweenObj.gameObject, o_dlg_chips.transform.position, Quaternion.identity) as GameObject;
                obj.transform.parent = this.transform;
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                yield return new WaitForSeconds(2.0f);
                Destroy(obj.gameObject);
                OnGameStartResp(packet);
            }
            else
            {
                OnGameStartResp(packet);
            }           
            
        }

        //玩家下本
        void OnGameStartInvest(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_INVEST;
            packet.BeginRead();
            byte bcharuser = (byte)packet.GetUShort();
            long lscore = packet.GetLong();

            //服务器自动下注
            if (bcharuser == GetSelfChair())
            {
                UISetWinnows uisetwinnows = o_setwinnows.GetComponent<UISetWinnows>();
                if (uisetwinnows == null)
                {
                    uisetwinnows = o_setwinnows.AddComponent<UISetWinnows>();
                }
                uisetwinnows.CloseWindows();
            }
            
            _linvertscore[bcharuser] = lscore;
            _lAllCellScore[bcharuser] = _lCellScore;
            o_player_bobook[ChairToView(bcharuser)].SetActive(true);
            _bUserInvent[bcharuser] = true;
            AppendChips(bcharuser, (int)_lCellScore);
            SetAllInvertData();
            PlayGameSound(GameSoundType.SETOK);
        }

        //开始下注
        void OnGameStartResp(NPacket packet)
        {
//             try
//             {
                GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_INVEST;

                _bStartGame = true;

                //播放声音
                PlayGameSound(GameSoundType.START);

                packet.BeginRead();

                _bBankerUser = (byte)packet.GetUShort();   //庄家位置
                _firstUser = _bBankerUser;
                long[] luserscore = new long[GameLogic.GAME_PLAYER];
                long lmyscore = 0;
                for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
                {
                    luserscore[i] = packet.GetLong();
                    if (GetSelfChair() == i)
                    {
                        lmyscore = luserscore[i];
                    }
                }
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


                if (_bBankerUser == GetSelfChair())
                {
                    byte bSex = GameEngine.Instance.GetTableUserItem(_bBankerUser).Gender;
                    PlayGameSound(GameSoundType.BANKER);
                }
                btn_ready.SetActive(false);
                o_setwinnows.SetActive(true);

                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_clock[i].SetActive(false);
                }

                UISetWinnows uisetwinnows = o_setwinnows.GetComponent<UISetWinnows>();
                if (uisetwinnows == null)
                {
                    uisetwinnows = o_setwinnows.AddComponent<UISetWinnows>();
                }
                PlayerInfo pmyUserData = GameEngine.Instance.GetTableUserItem(GetSelfChair());
                lmyscore = pmyUserData.Money;

                long winScore = 0;
                bool repeat = false;   //是否重复上一局下注
                if (setBoboshuCount == 0)
                {
                    repeat = false;
                    GameLogic.GAME_CHANGE_RESLUT = (int)pmyUserData.Money;
                }
                else
                {
                    winScore = lmyscore - GameLogic.GAME_CHANGE_RESLUT;
//                     if (winScore < 0)
//                     {
//                         currentBoboshu = _lCellScore * 10;
//                         GameLogic.GAME_MIN_INVERT = currentBoboshu;
//                         winScore = 0;
//                         repeat = false;
//                     } 
                    GameLogic.GAME_CHANGE_RESLUT = (int)pmyUserData.Money;
                }
//                 if (currentBoboshu > lmyscore)
//                 {
//                     currentBoboshu = _lCellScore * 10;
//                     GameLogic.GAME_MIN_INVERT = currentBoboshu;
//                     winScore = 0;
//                     repeat = false;
//                 }
                GameLogic.GAME_MIN_INVERT = currentBoboshu + winScore;
                if (GameLogic.GAME_MIN_INVERT < _lCellScore)
                {
                    GameLogic.GAME_MIN_INVERT = _lCellScore;
                }
                o_player_clock[GetSelfChair()].SetActive(false);
                uisetwinnows.InitData(lmyscore, repeat, winScore);
                o_player_allinvert.SetActive(false);

                obj_option.SetActive(true);

//             }
//             catch (Exception ex)
//             {
//                 UIMsgBox.Instance.Show(true, ex.Message);
//             }
        }

        //用户下注
        void OnUserAddScoreResp(NPacket packet)
        {
            packet.BeginRead();
            _firstUser = (byte)packet.GetShort();
            byte bChair = (byte)packet.GetUShort();
			_addScoreUser = bChair;
            _bCurrentUser = (byte)packet.GetUShort();
            long addsorce = packet.GetLong();

            o_player_clock[bChair].SetActive(false);

            //判断---
            if (bChair != GetSelfChair())
            {
                BetType betType = BetType.Follow;
                int addType = 0;
                if (addsorce == _lTurnLessScore)
                {
                    betType = BetType.Follow;
                }
                if (addsorce > _lTurnLessScore && _lTurnLessScore != 0)
                {
                    betType = BetType.Big;
                    addType = 1;
                }
                //if (bChair == _firstUser && _lOneScore == 0)
                if (_lOneScore == 0)
                {
                    betType = BetType.Big;
                    addType = 1;
                }

                if (addsorce >= _linvertscore[bChair] - _lTableScore[bChair] - _lCellScore)
                {
                    betType = BetType.Knock;
                    addType = 2;
                }
                if (addsorce == 0)
                {
                    betType = BetType.Stop;
                    addType = 3;
                }

                int myAddType = UnityEngine.Random.Range(addType * 5, addType * 5 + 5 - 1);
                PlayUserSound(myAddType, GameEngine.Instance.GetTableUserItem(bChair).Gender);
                SetInvertAction(bChair, betType, addsorce);
            }

            _lTableScore[bChair] += (int)addsorce;
            _lTurnMaxScore = packet.GetLong();
            _lTurnLessScore = packet.GetLong();
            _lOneScore += addsorce;
            
            //Debug.LogError("_lTurnLessScore = " + _lTurnLessScore);

            AppendChips(bChair, (int)addsorce);
            CheckIsSelfAddScroce();
            CheckAddScroceStatue();
            SetAllInvertData();
            o_player_allinvert.SetActive(true);

            if (_bCurrentUser != 255)
            {
                if (_linvertscore[_bCurrentUser] - _lTableScore[_bCurrentUser] - _lCellScore > 0)
                {
                    SetUserClock(_bCurrentUser, 15, TimerType.TIMER_ADDSCORE);
                }
            }
            //SetUserClock(_bCurrentUser, 15, TimerType.TIMER_ADDSCORE);
        }

        //下注结束，开始发牌
        void OnGameSendCard(NPacket packet)
        {
            setBoboshuCount++;
            transform.Find("repeat_boboshu").gameObject.SetActive(false);

            //设置庄家
            SetBanker();

            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                o_player_bobook[i].SetActive(false);
            }

            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_OPEN_CARD;
            packet.BeginRead();
            _bCardNum = packet.GetByte();
            _bCurrentUser = (byte)packet.GetUShort();
            packet.GetBytes(ref _bMyCardData, 2);
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                byte[] tmpCardData = new byte[2];
                packet.GetBytes(ref tmpCardData, 2);
                _bHandCardData[i][2] = tmpCardData[0];
                _bHandCardData[i][3] = tmpCardData[1];
            }
            SendUserCards();
            //Invoke("SendUserCards", 1.0f);
        }

        void SendUserCards()
        {
            _lOneScore = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                {
                    byte addcard = (byte)(_bCardNum - _bBackCardNum);
                    SendHandCard(i, GetSendCards(_bBackCardNum, addcard, i), addcard);
                    PlayGameSound(GameSoundType.SENDCARD);
                }
            }

            _bBackCardNum = _bCardNum;
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_action[i].SetActive(false);
            }
            SetUserClock(_bCurrentUser, 15, TimerType.TIMER_ADDSCORE);
            CheckIsSelfAddScroce();
            CheckAddScroceStatue();
            Invoke("ArraySelfCards", 1.0f);
        }

        void SendUserCardsForScene(long[] cardsNum, byte CurrentNum)
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                if (userdata != null)
                {
                    byte num = (byte)cardsNum[i];
                    if (num == 0)
                    {
                        num = CurrentNum;
                    }
                    byte[] bCards = new byte[num];
                    for (byte j = 0; j < num; ++j)
                    {
                        bCards[j] = _bHandCardData[i][j];
                        if (_bPlayStatus[i] == (byte)PlayerState.GIVEUP)
                        {
                            bCards[j] = 252;
                        }
                    }
                    SendHandCard(i, bCards, num);
                    PlayGameSound(GameSoundType.SENDCARD);
                }
            }
            _bBackCardNum = CurrentNum;
            SetUserClock(_bCurrentUser, 15, TimerType.TIMER_ADDSCORE);

            CheckIsSelfAddScroce();
            CheckAddScroceStatue();
            Invoke("ArraySelfCards", 1.0f);
        }

        byte[] GetSendCards(byte pos, byte count, byte bchar)
        {
            byte[] bCards = new byte[count];
            for (byte i = pos; i < pos + count; ++i)
            {
                bCards[i - pos] = _bHandCardData[bchar][i];
            }
            return bCards;
        }

        void ShowTipsButton()
        {

        }

        //用户开牌
        void OnUserOpenCardResp(NPacket packet)
        {
            packet.BeginRead();
            byte buser = (byte)packet.GetUShort();
            if (buser != GetSelfChair())
            {
                PlayGameSound(GameSoundType.OPENCARD);
            }
            else
            {
                //服务器给玩家开牌
                o_splitcards.SetActive(false);
            }
        }

        /// <summary>
        /// 开始分牌  等牌发完再分
        /// </summary>
        /// <param name="packet"></param>
        void OnStartSlipCardResp(NPacket packet)
        {
            _bMing = false;
            _IsSplit = true;
            //自己在遊戲,才显示
            if (_bPlayStatus[GetSelfChair()] == (byte)PlayerState.PLAY)
            {
                StartCoroutine(IEStartSlipCards(packet));
            }
        }
        private int _bwaitSec = 0;
        IEnumerator IEStartSlipCards(NPacket packet)
        {
            if (_bBackCardNum < 4 && _bwaitSec < 3)
            {
                ++_bwaitSec;
                yield return new WaitForSeconds(1.0f);
                yield return StartCoroutine(IEStartSlipCards(packet));
            }
            else
            {
                yield return new WaitForSeconds(2.0f);
                StartSlipCards();
                _bwaitSec = 0;
                yield return 0;
            }
        }

        void StartSlipCards()
        {
            //obj_option.SetActive(false);
            o_bet_shade.SetActive(true);
            ForbiddenChipBtn(false);
            ShowOperateButton(false);
            o_player_allinvert.SetActive(false);
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_action[i].SetActive(false);
            }
            if (_bPlayStatus[GetSelfChair()] != (byte)PlayerState.PLAY)
            {
                return;
            }
            o_splitcards.SetActive(true);
            UISplitCard spcard = o_splitcards.GetComponent<UISplitCard>();
            if (spcard == null)
            {
                spcard = o_splitcards.AddComponent<UISplitCard>();
            }
            byte[] bcardsdata = new byte[GameLogic.MAX_COUNT];
            Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, bcardsdata, 0, 4);
            Buffer.BlockCopy(_bMyCardData, 0, bcardsdata, 0, 2);
            spcard.InitData(bcardsdata, GameLogic.MAX_COUNT, SlipCardAfter);
        }


        public void SlipCardAfter(byte[] bcards, ushort count)
        {

            o_player_allinvert.SetActive(true);
            if (_bPlayStatus[GetSelfChair()] != (byte)PlayerState.PLAY)
            {
                return;
            }
            NPacket packet = NPacketPool.GetEnablePacket();
            DataManager(bcards);
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_OPEN_CARD);
            packet.AddBytes(bcards, count);
            SendMsgToServer(packet);
        }

        private void DataManager(byte[] bcards)
        {
            if (bcards.Length != 4)
            {
                return;
            }
            if (GameLogic.GetPeaceful(bcards) > 0)
            {
                return;
            }
            byte[] headArr = new byte[2];
            headArr[0] = bcards[0];
            headArr[1] = bcards[1];

            byte[] tailArr = new byte[2];
            tailArr[0] = bcards[2];
            tailArr[1] = bcards[3];

            byte headNum = GameLogic.GetCardTypeN(headArr, 2);
            byte tailNum = GameLogic.GetCardTypeN(tailArr, 2);
            if (headNum < tailNum)
            {
                Array.Clear(bcards, 0, bcards.Length);
                bcards[0] = tailArr[0];
                bcards[1] = tailArr[1];
                bcards[2] = headArr[0];
                bcards[3] = headArr[1];
            }
        }

        //用户丢牌，点退出，或者加注时间到直接调这个
        void OnUserPlayerExitResp(NPacket packet)
        {
            packet.BeginRead();
            //游戏状态
            ushort gamestatus = packet.GetUShort();
            //放弃用户
            byte giveupUser = (byte)packet.GetUShort();

            int myAddType = UnityEngine.Random.Range(4 * 5, 4 * 5 + 5 - 1);
            PlayUserSound(myAddType, GameEngine.Instance.GetTableUserItem(giveupUser).Gender);

            //输掉金币	
            long lostScore = packet.GetLong();

            SetInvertAction(giveupUser, BetType.Lose, 0);

            //设置状态
            _bPlayStatus[giveupUser] = (byte)PlayerState.GIVEUP;

            //这里添加弃牌效果图，参考炸金花
            byte bViewID = ChairToView(giveupUser);
            UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
            int count = ctr.transform.GetChildCount() - 3;
            byte[] cardArr = new byte[count];
            for (int i = 0; i < count; i++)
            {
                cardArr[i] = 252;
            }
            ctr.SetCardData(cardArr, (byte)count);

            StartFrameChip();
        }

        //游戏结束
        void OnGameEndResp(NPacket packet)
        {
            IsOpenCard = true;

            SetUserClock(GameLogic.NULL_CHAIR, 0, TimerType.TIMER_NULL);
            _bStartGame = false;
            packet.BeginRead();
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                long gametax = packet.GetLong();
            }
            long[] lscore = new long[GameLogic.GAME_PLAYER];

            int areaCount = 0;

            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                lscore[i] = packet.GetLong();

                if (lscore[i] > 0)
                {
                    //赢的玩家人数
                    areaCount++;
                }
            }

            byte[][] bcards = new byte[GameLogic.GAME_PLAYER][];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                byte[] bcarddata = new byte[GameLogic.MAX_COUNT];
                packet.GetBytes(ref bcarddata, GameLogic.MAX_COUNT);
                bcards[i] = new byte[GameLogic.MAX_COUNT];
                Buffer.BlockCopy(bcarddata, 0, bcards[i], 0, GameLogic.MAX_COUNT);

            }
            //结束原因
            _gameExitReasion = (GameExitType)packet.GetByte();

			for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
			{
				cardDataCount[i] = 0;
			}

			int indexCount = 0;

            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                if (_gameExitReasion == GameExitType.END_REASON_NORMAL)
                {
                    byte[] bcarddata = new byte[GameLogic.MAX_COUNT];
                    Buffer.BlockCopy(bcards[i], 0, bcarddata, 0, GameLogic.MAX_COUNT);
                    byte[] tmpdata1 = new byte[2];
                    byte[] tmpdata2 = new byte[2];
                    Buffer.BlockCopy(bcarddata, 0, tmpdata1, 0, 2);
                    Buffer.BlockCopy(bcarddata, 2, tmpdata2, 0, 2);
                    //if (!GameLogic.CompareCardN(tmpdata1, tmpdata2, 2))
                    //{
                    //    Buffer.BlockCopy(tmpdata2, 0, bcarddata, 0, 2);
                    //    Buffer.BlockCopy(tmpdata1, 0, bcarddata, 2, 2);
                    //}
                    if (_bPlayStatus[i] == (byte)PlayerState.PLAY && _bCardNum == 4)
                    {
						
						if (GameLogic.GetPeaceful(bcarddata) > 0)
						{
							OpenHandCardData(i, bcarddata, 4, 0, true);
						}
						else
						{
							OpenHandCardData(i, bcarddata, 4, 0, false);
						}
						
						Buffer.BlockCopy(bcarddata, 0, EndHandCardData[i], 0, 4);

                        cardDataCount[indexCount] = i;
                        indexCount++;
                       
                	}
				}
                else if (_bPlayStatus[i] == (byte)PlayerState.PLAY)
                {
                    OpenHandCardData(i, _bHandCardData[i], _bCardNum, 0, true);
                }
            }

			//判断开牌顺序
            bool sortCard = false;
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                int id = (_bBankerUser + i) % GameLogic.GAME_PLAYER;

                sortCard = SortUserCard(id, indexCount);

                if (sortCard == true)
                {
                    break;
                }
            }

            nCardCount = indexCount;

			//卡牌显示
			StartCoroutine(ShowCardTweenTwo(0.5f));
            StartCoroutine(ShowCardTween(0.5f, areaCount, lscore, _userdata));

            byte[] cbPlayStatus = new byte[GameLogic.GAME_PLAYER];
            packet.GetBytes(ref cbPlayStatus, GameLogic.GAME_PLAYER);

            //obj_option.SetActive(false);
            o_bet_shade.SetActive(true);
            ForbiddenChipBtn(false);
            ShowOperateButton(false);
            StartCoroutine(OpenSettlement(cbPlayStatus,bcards, lscore, _gameExitReasion, SettlementContinue));

		}

        //用户开牌顺序（正常结束时）
        bool SortUserCard(int id, int indexcount)
        {
            int[] endCardDataCount = new int[GameLogic.GAME_PLAYER];

            for (int i = 0; i < indexcount; i++)
            {
                if (cardDataCount[i] == id)
                {
					int current = 0;
                    for (int k = 0; k < indexcount; k++)
                    {
                        current = (i + k) % indexcount;
						endCardDataCount[k] = cardDataCount[current];  
                    }                

					for(int k=0; k<indexcount; k++)
					{
						cardDataCount[k] = endCardDataCount[k];
					}

                    return true;
                }
            }

            return false;
        }

        //金币回收
		void ChipRecover(int count, long[] lscore, PlayerInfo[] userdata)
        {
            float  areaCount = 0;
            float chipCount = 0;
            float chipCountMean = 0;

            //名次
            int nFirst = 0;
            int nSecond = 0;
            int nThird = 0;
            int nFourth = 0;
            int nFifth = 0;

            for (int i = 1; i <= count; i++)
            {
                areaCount += i;
            }
            foreach (Transform chip in o_dlg_chips.transform)
            {
                if (chip.name != "lbl_total_count" && chip.name != "player_head_0" && chip.name != "player_head_1"
                    && chip.name != "player_head_2" && chip.name != "player_head_3" && chip.name != "player_head_4" && chip.name != "player_head_5")
                {
                    chipCount++;
                }
            }
            chipCountMean = chipCount / areaCount;


            if (count != 0)
            {
                long nTemp = 0;
                if (count > 0)
                {
                    //第一名
                    for (int playerId = 0; playerId < GameLogic.GAME_PLAYER; ++playerId)
                    {
                        if (nTemp == 0 || nTemp < lscore[playerId])
                        {
                            nTemp = lscore[playerId];
                            nFirst = playerId;
                        }
                    }
                }
                if (count > 1)
                {
                    //第二名
                    nTemp = 0;
                    for (int playerId = 0; playerId < GameLogic.GAME_PLAYER; ++playerId)
                    {
                        if (playerId != nFirst && (nTemp == 0 || nTemp < lscore[playerId]))
                        {
                            nTemp = lscore[playerId];
                            nSecond = playerId;
                        }
                    }
                }
                if (count > 2)
                {
                    //第三名
                    nTemp = 0;
                    for (int playerId = 0; playerId < GameLogic.GAME_PLAYER; ++playerId)
                    {
                        if (playerId != nFirst && playerId != nSecond && (nTemp == 0 || nTemp < lscore[playerId]))
                        {
                            nTemp = lscore[playerId];
                            nThird = playerId;
                        }
                    }
                }
                if (count > 3)
                {
                    //第四名
                    nTemp = 0;
                    for (int playerId = 0; playerId < GameLogic.GAME_PLAYER; ++playerId)
                    {
                        if (playerId != nFirst && playerId != nSecond && playerId != nThird && (nTemp == 0 || nTemp < lscore[playerId]))
                        {
                            nTemp = lscore[playerId];
                            nFourth = playerId;
                        }
                    }
                }
                if (count > 4)
                {
                    //第五名
                    nTemp = 0;
                    for (int playerId = 0; playerId < GameLogic.GAME_PLAYER; ++playerId)
                    {
                        if (playerId != nFirst && playerId != nSecond && playerId != nThird && playerId != nFifth && (nTemp == 0 || nTemp < lscore[playerId]))
                        {
                            nTemp = lscore[playerId];
                            nFifth = playerId;
                        }
                    }
                }

                int chipIndex = 0;
                foreach (Transform chip in o_dlg_chips.transform)
                {
                    if (chip.name != "lbl_total_count" && chip.name != "player_head_0" && chip.name != "player_head_1"
                        && chip.name != "player_head_2" && chip.name != "player_head_3" && chip.name != "player_head_4" && chip.name != "player_head_5")
                    {
                        chipIndex++;

                        switch (count)
                        {
                            case 1:
                                {
                                    byte bViewID = ChairToView((byte)nFirst);
                                    TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                    break;
                                }
                            case 2:
                                {
                                    //Debug.LogError("chipIndex" + chipIndex + "++++++++++++++(int)(count*chipCountMean)" + (int)(count * chipCountMean));
                                    if ((int)(count*chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nFirst);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        //Debug.LogError(player_head_pos[bViewID].transform.position);
										break;
                                    }
									if (chipIndex > (int)(count*chipCountMean))
                                    {
                                        byte bViewID = ChairToView((byte)nSecond);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        break;
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    //Debug.LogError("chipIndex" + chipIndex + "++++++++++++++(int)(count*chipCountMean)" + (int)(count * chipCountMean));
                                    if ((int)(count * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nFirst);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    if ((int)((count - 1) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nSecond);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    if (chipIndex > (int)((count - 1) * chipCountMean))
                                    {
                                        byte bViewID = ChairToView((byte)nThird);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    if ((int)(count * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nFirst);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        break;
                                    }
                                    if ((int)((count - 1) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nSecond);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        break;
                                    }
                                    if ((int)((count - 2) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nThird);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        break;
                                    }
                                    if (chipIndex > (int)((count - 2) * chipCountMean))
                                    {
                                        byte bViewID = ChairToView((byte)nFourth);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                    }
                                    break;
                                }  
                            case 5:
                                {
                                    if ((int)(count * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nFirst);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    if ((int)((count-1) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nSecond);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    if ((int)((count-2) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nThird);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
										break;
                                    }
                                    if ((int)((count-3) * chipCountMean) >= chipIndex)
                                    {
                                        byte bViewID = ChairToView((byte)nThird);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                        break;
                                    }
                                    if (chipIndex > (int)((count - 3) * chipCountMean))
                                    {
                                        byte bViewID = ChairToView((byte)nFourth);
                                        TweenPosition.Begin(chip.gameObject, 1.0f, player_head_pos[bViewID].transform.localPosition);
                                    }
                                    break;
                                }                      
                        }
                    }
                }
            }
            else
            {
                foreach (Transform chip in o_dlg_chips.transform)
                {
                    if (chip.name != "lbl_total_count" && chip.name != "player_head_0" && chip.name != "player_head_1"
                    && chip.name != "player_head_2" && chip.name != "player_head_3" && chip.name != "player_head_4" && chip.name != "player_head_5")
                    {
                        TweenPosition.Begin(chip.gameObject, 0.5f, new Vector3(0, 0, 0));   
                    }
                }
            }
            
        }

        //销毁金币
        void DestroyChip()
        {
            foreach (Transform chip in o_dlg_chips.transform)
            {
                if (chip.name != "lbl_total_count" && chip.name != "player_head_0" && chip.name != "player_head_1"
                    && chip.name != "player_head_2" && chip.name != "player_head_3" && chip.name != "player_head_4" && chip.name != "player_head_5")
                {
                    Destroy(chip.gameObject);
                }
            }
        }

        //正常结束显示后面两张
		IEnumerator ShowCardTweenTwo(float time)
		{
			yield return new WaitForSeconds(2.0f);

            if (nCardCount != 1)
            {
                for (int i = 0; i < nCardCount; i++)
                {
                    if (GameLogic.GetPeaceful(EndHandCardData[cardDataCount[i]]) <= 0)
                    {
                        OpenHandCardData((byte)cardDataCount[i], EndHandCardData[cardDataCount[i]], 4, 2, true);
                    }

                    yield return new WaitForSeconds(time);
                }

            }
		}
        //正常结束显示全部
        IEnumerator ShowCardTween(float time, int count, long[] lscore, PlayerInfo[] userdata)
		{
			yield return new WaitForSeconds(nCardCount*time+2.0f);


            if (nCardCount == 1)
            {
                //回收金币
                ChipRecover(count, lscore, _userdata);
                yield return new WaitForSeconds(1.0f);
                DestroyChip();

            }
            else
            {
                for (int i = 0; i < nCardCount; i++)
                {
                    OpenHandCardData((byte)cardDataCount[i], EndHandCardData[cardDataCount[i]], 4, 0, true);
                    yield return new WaitForSeconds(time);
                }
                ChipRecover(count, lscore, _userdata);
                yield return new WaitForSeconds(1.0f);
                DestroyChip();

            }
		}

        /// <summary>
        /// 弹出结算
        /// </summary>
        IEnumerator OpenSettlement(byte[] cbPlayStatus, byte[][] bcards, long[] lscore, GameExitType type = GameExitType.END_REASON_NORMAL, Action action = null)
        {
			yield return new WaitForSeconds(nCardCount*2.0f + 2.0f);

            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_action[i].SetActive(false);
            }
            o_player_allinvert.SetActive(false);
            o_settlement.SetActive(true);
            UISettlement settlement = o_settlement.GetComponent<UISettlement>();
            if (settlement == null)
            {
                settlement = o_settlement.AddComponent<UISettlement>();
            }
            settlement.InitData(cbPlayStatus, bcards, lscore, _bCardNum, _userdata, type, action);

            IsOpenCard = false;
            //UpdateUserView();
            yield return 0;
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        void SettlementContinue()
        {
            _addscoreObj.m_iNum = _lCellScore;
            UISettlement settlement = o_settlement.GetComponent<UISettlement>();
            o_settlement.SetActive(false);
            IsOpenCard = false;
            UpdateUserView();
            ClearGameContinue();
            OnBtnReadyIvk();
        }

        //初始场景处理函数
        void SwitchFreeSceneView(NPacket packet)
        {
            //Debug.LogError("初始场景处理函数");
            InitGameView();
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_FREE;
            _bStart = true;
            UIWaiting.Instance.Show(false);
            btn_ready.SetActive(false);
            packet.BeginRead();

            _lCellScore = (int)packet.GetLong();
            MIN_ADDSCORE = _lCellScore;
            GameLogic.GAME_MAX_INVERT = 10000000000;
            GameLogic.GAME_MIN_INVERT = 50 * _lCellScore;
            GameLogic.GAME_CHANGE_NUM = 5 * _lCellScore;
            currentBoboshu = GameLogic.GAME_MIN_INVERT;
            o_lbl_cellScore.GetComponent<UILabel>().text = (_lCellScore).ToString();
            o_lbl_maxScore.GetComponent<UILabel>().text = (_lCellScore * 1000).ToString();
            UpdateUserView();
            SetUserClock(GetSelfChair(), (uint)TimerDelay.READY, TimerType.TIMER_READY);
            btn_ready.SetActive(true);
        }

        //下本场景处理函数
        void SwitchInvestSceneView(NPacket packet)
        {
            //Debug.LogError("下本场景处理函数");
            /*
                WORD								wBankerUser;						//庄家用户
                BYTE								cbPlayStatus[GAME_PLAYER];			//游戏状态
                bool								bInvestFinish[GAME_PLAYER];			//完成标志
                LONGLONG							lCellScore;							//基础积分
                LONGLONG							lUserScore[GAME_PLAYER];			//用户积分
            */
            _addscoreObj.m_iNum = _lCellScore;
            InitGameView();
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_INVEST;
            _bStart = true;
            UIWaiting.Instance.Show(false);
            btn_ready.SetActive(false);
            packet.BeginRead();

            _bBankerUser = (byte)packet.GetUShort();                //庄

            byte[] _bPlay = new byte[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bPlay[i] = packet.GetByte();                       //游戏状态
            }

            bool[] _bBoBoStatus = new bool[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bBoBoStatus[i] = packet.GetBool();                 //完成标志
            }
            _lCellScore = (int)packet.GetLong();                    //基础积分
            //设置底注和最大啵啵数
            GameLogic.GAME_MAX_INVERT = 1000 * _lCellScore;
            GameLogic.GAME_MIN_INVERT = 50 * _lCellScore;
            GameLogic.GAME_CHANGE_NUM = 5 * _lCellScore;
            currentBoboshu = GameLogic.GAME_MIN_INVERT;
            MIN_ADDSCORE = _lCellScore;

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _linvertscore[i] = packet.GetLong();                //用户积分
                if (_linvertscore[i] > 0)
                {
                    _bUserInvent[i] = true;
                    _lAllCellScore[i] = _lCellScore;
                }
            }
            UpdateUserView();   //更新玩家头像信息
            SetBanker();        //设置庄家
            SetAllInvertData(); //设置总加注

            o_lbl_cellScore.GetComponent<UILabel>().text = (_lCellScore).ToString();
            o_lbl_maxScore.GetComponent<UILabel>().text = (_lCellScore * 1000).ToString();

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {                
                if (_bPlay[i] == 1)
                {
                    _bPlayStatus[i] = (byte)PlayerState.PLAY;
                    if (_bBoBoStatus[i])
                    {
                        o_player_bobook[i].GetComponent<UISprite>().spriteName = "bobook";
                        o_player_bobook[i].SetActive(true);
                        AppendChips(i, _lCellScore);
                    }
                    if (!_bBoBoStatus[i] && i == GetSelfChair())
                    {
                        o_setwinnows.SetActive(true);
                        UISetWinnows uisetwinnows = o_setwinnows.GetComponent<UISetWinnows>();
                        if (uisetwinnows == null)
                        {
                            uisetwinnows = o_setwinnows.AddComponent<UISetWinnows>();
                        }
                        PlayerInfo pmyUserData = GameEngine.Instance.GetTableUserItem(GetSelfChair());
                        GameLogic.GAME_CHANGE_RESLUT = (int)pmyUserData.Money;
                        o_player_clock[GetSelfChair()].SetActive(false);
                        uisetwinnows.InitData(pmyUserData.Money, false, 0);                        
                    }
                }

                //关闭闹钟
                o_player_clock[i].SetActive(false);
            }
        }

        //下注场景处理函数
        void SwitchScoreSceneView(NPacket packet)
        {
            //Debug.LogError("下注场景处理函数");
            /*
                WORD								wCurrentUser;						//当前用户
                WORD								wBankerUser;						//庄家用户
                BYTE								cbHandCard[2];						//用户扑克
                BYTE								cbMingCard[GAME_PLAYER][2];			//用户扑克
                BYTE								cbPlayStatus[GAME_PLAYER];			//游戏状态
                LONGLONG							lCellScore;							//基础积分
                LONGLONG							lUserScore[GAME_PLAYER];			//用户积分
                LONGLONG							lTotalScore[GAME_PLAYER];			//总注数目
                LONGLONG							lTurnMaxScore;						//最大下注
                LONGLONG							lTurnMinScore;						//最小下注
            */
            InitGameView();
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_SCORE;
            _bStart = true;
            UIWaiting.Instance.Show(false);
            btn_ready.SetActive(false);
            packet.BeginRead();

            _bCurrentUser = (byte)packet.GetUShort();               //当前用户
            _bBankerUser = (byte)packet.GetUShort();                //庄家用户
            packet.GetBytes(ref _bMyCardData, 2);                   //用户扑克
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                byte[] tmpCardData = new byte[2];
                packet.GetBytes(ref tmpCardData, 2);
                _bHandCardData[i][2] = tmpCardData[0];              //用户扑克
                _bHandCardData[i][3] = tmpCardData[1];
            }

            byte[] _bPlay = new byte[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bPlay[i] = packet.GetByte();                       //游戏状态
            }
            _lCellScore = (int)packet.GetLong();                    //基础积分
            //设置底注和最大啵啵数
            GameLogic.GAME_MAX_INVERT = 10000000000;
            GameLogic.GAME_MIN_INVERT = 50 * _lCellScore;
            GameLogic.GAME_CHANGE_NUM = 5 * _lCellScore;
            currentBoboshu = GameLogic.GAME_MIN_INVERT;
            MIN_ADDSCORE = _lCellScore;

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _linvertscore[i] = packet.GetLong();                //用户积分
                if (_linvertscore[i] > 0)
                {
                    _bUserInvent[i] = true;
                    _lAllCellScore[i] = _lCellScore;
                }
            }

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _lTableScore[i] = (int)packet.GetLong() - _lCellScore;            //每个人的总注数目
            }

            _lTurnMaxScore = packet.GetLong();                      //最大下注

            _lTurnLessScore = packet.GetLong();                     //最小下注时



            UpdateUserView();   //更新玩家头像信息
            SetBanker();        //设置庄家
            SetAllInvertData(); //设置总加注



            o_lbl_cellScore.GetComponent<UILabel>().text = (_lCellScore).ToString();
            o_lbl_maxScore.GetComponent<UILabel>().text = (_lCellScore * 1000).ToString();
            //发牌
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlay[i] == 1)
                {
                    _bPlayStatus[i] = (byte)PlayerState.PLAY;
                    AppendChips(i, _lTableScore[i]);
                }
                else
                {
                    PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {
                        _bPlayStatus[i] = (byte)PlayerState.GIVEUP;
                    }
                }
            }

            byte[] bcardsdata = new byte[GameLogic.MAX_COUNT];
            Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, bcardsdata, 0, 4);
            Buffer.BlockCopy(_bMyCardData, 0, bcardsdata, 0, 2);


            long[] cardsNum = new long[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                cardsNum[i] = packet.GetLong();                            //每家牌个数

                }
            byte CurrentNum = 2;//默认2
            CurrentNum = (byte)packet.GetUShort();               //当前牌个数
            //SendUserCards();
            SendUserCardsForScene(cardsNum, CurrentNum);
        }

        //摊牌场景处理函数
        void SwitchOpenCardSceneView(NPacket packet)
        {
            //Debug.LogError("摊牌场景处理函数");
            /*
                WORD								wBankerUser;						//庄家用户
                BYTE								cbOpenFinish[GAME_PLAYER];			//完成标志
                BYTE								cbPlayStatus[GAME_PLAYER];			//游戏状态
                BYTE								cbHandCard[2];						//用户扑克
                BYTE								cbMingCard[GAME_PLAYER][2];			//用户扑克
                LONGLONG							lCellScore;							//基础积分
                LONGLONG							lUserScore[GAME_PLAYER];			//用户积分
                LONGLONG							lTotalScore[GAME_PLAYER];			//总注数目
            */
            /*
                public const ushort GS_TK_FREE = (ushort)GameState.GS_FREE;
                public const ushort GS_TK_INVEST = (ushort)GameState.GS_PLAYING;
                public const ushort GS_TK_SCORE = (ushort)GameState.GS_PLAYING + 1;
                public const ushort GS_TK_OPEN_CARD = (ushort)GameState.GS_PLAYING + 2;
             */


            InitGameView();
            GameEngine.Instance.MySelf.GameStatus = (byte)GameLogic.GS_TK_OPEN_CARD;
            _bStart = true;
            _IsSplit = true;
            UIWaiting.Instance.Show(false);
            btn_ready.SetActive(false);
            packet.BeginRead();

            _bBankerUser = (byte)packet.GetUShort();                //庄家用户

            //游戏状态
            byte[] _bState = new byte[GameLogic.GAME_PLAYER];       //完成标识
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bState[i] = packet.GetByte();
            }

            byte[] _bPlay = new byte[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _bPlay[i] = packet.GetByte();                       //游戏状态
            }

            packet.GetBytes(ref _bMyCardData, 2);                   //用户扑克

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                byte[] tmpCardData = new byte[2];
                packet.GetBytes(ref tmpCardData, 2);

                _bHandCardData[i][2] = tmpCardData[0];              //用户扑克
                _bHandCardData[i][3] = tmpCardData[1];
            }

            _lCellScore = (int)packet.GetLong();                    //基础积分

            //设置底注和最大啵啵数
            GameLogic.GAME_MAX_INVERT = 10000000000;
            GameLogic.GAME_MIN_INVERT = 50 * _lCellScore;
            GameLogic.GAME_CHANGE_NUM = 5 * _lCellScore;
            currentBoboshu = GameLogic.GAME_MIN_INVERT;
            MIN_ADDSCORE = _lCellScore;

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _linvertscore[i] = packet.GetLong();                //用户积分
                if (_linvertscore[i] > 0)
                {
                    _bUserInvent[i] = true;
                    _lAllCellScore[i] = _lCellScore;
                }
            }

            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                _lTableScore[i] = (int)packet.GetLong() - _lCellScore;            //每个人的总注数目
            }

            UpdateUserView();   //更新玩家头像信息
            SetBanker();        //设置庄家
            SetAllInvertData(); //设置总加注

            o_lbl_cellScore.GetComponent<UILabel>().text = (_lCellScore).ToString();
            o_lbl_maxScore.GetComponent<UILabel>().text = (_lCellScore * 1000).ToString();

            //发牌
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                if (_bPlay[i] == 1)
                {
                    _bPlayStatus[i] = (byte)PlayerState.PLAY;
                }
                else
                {
                    PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                    if (userdata != null)
                    {
                        _bPlayStatus[i] = (byte)PlayerState.GIVEUP;
                    }
                }
            }

            byte[] bcardsdata = new byte[GameLogic.MAX_COUNT];
            Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, bcardsdata, 0, 4);
            Buffer.BlockCopy(_bMyCardData, 0, bcardsdata, 0, 2);


            long[] cardsNum = new long[GameLogic.GAME_PLAYER];
            for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                cardsNum[i] = packet.GetLong();                            //每家牌个数

                }

            byte CurrentNum = 2;//默认2
            CurrentNum = (byte)packet.GetUShort();               //当前牌个数

            //SendUserCards();
            SendUserCardsForScene(cardsNum, CurrentNum);

            if (_bState[GetSelfChair()] == 0)
            {
                StartSlipCards();
            }
            else
            {
                SlipCardAfter(bcardsdata, 4);
            }

        }

        #endregion


        #region ##################UI 事件#######################

        /////////////////////////////游戏通用/////////////////////////////


        //退出按钮
        void OnConfirmBackOKIvk()
        {
            if (_bStartGame)
            {
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GIVE_UP);
                SendMsgToServer(packet);
                _bPlayStatus[GetSelfChair()] = (byte)PlayerState.GIVEUP;
            }
            _bStartGame = false;

            _bStart = false;
            GameEngine.Instance.Quit();
            _bReqQuit = true;
            _nQuitDelay = System.Environment.TickCount;
            OnBtnSpeakCancelIvk();
            CancelInvoke();
        }

        //设置
        public void OnBtnSettingIvk()
        {
            UISetting.Instance.Show(true);
        }
        void OnBtnChatIvk()
        {
            UIChat.Instance.Show(true);
        }

        //准备
        void OnBtnReadyIvk()
        {
            btn_ready.SetActive(false);
            SetUserClock(GetSelfChair(), (uint)TimerDelay.NULL, TimerType.TIMER_NULL);

            if (GameEngine.Instance.AutoSit)
            {
                GameEngine.Instance.SendUserSitdown();
            }
            else
            {
                GameEngine.Instance.SendUserReadyReq();
            }
            _bPlayStatus[GetSelfChair()] = (byte)PlayerState.READY;
            InitGameView();

            PlayGameSound(GameSoundType.READY);
        }

        //规则
        void OnBtnRuleIvk()
        {
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

        void OnPlayerInfoIvk(GameObject obj)
        {
        }

        //喇叭按钮
        public void OnBtnVoiceIvk()
        {
            if (btn_horn.transform.GetComponent<UISprite>().spriteName == "voice_play")
            {

                o_effectMusic.GetComponent<UISlider>().sliderValue = 0;
                o_backGroundMusic.GetComponent<UISlider>().sliderValue = 0;
                btn_horn.transform.GetComponent<UISprite>().spriteName = "voice_close";
            }
            else
            {
                o_effectMusic.GetComponent<UISlider>().sliderValue = _effectVol;
                o_backGroundMusic.GetComponent<UISlider>().sliderValue = _musicVol;
                btn_horn.transform.GetComponent<UISprite>().spriteName = "voice_play";
            }             
        }

        //操作按钮显示
        void ShowOperateButton(bool show)
        {
            btn_follow.gameObject.SetActive(show);
            btn_big.gameObject.SetActive(show);
            btn_knock.gameObject.SetActive(show);
            btn_stop.gameObject.SetActive(show);
            btn_lose.gameObject.SetActive(show);
			_addscoreObj.gameObject.SetActive(show);
        }
        #endregion

        #region ##################控件事件#######################

        /////////////////////////////游戏通用/////////////////////////////
        bool isclick = false;
        //扑克控件点击事件
        void OnCardClick()
        {
            if (!isclick)
            {
                _bHandCardData[GetSelfChair()][0] = _bMyCardData[0];
                _bHandCardData[GetSelfChair()][1] = _bMyCardData[1];
            }
            else
            {
                _bHandCardData[GetSelfChair()][0] = 0;
                _bHandCardData[GetSelfChair()][1] = 0;
            }

        }
        void OnSpeakTimerEnd()
        {
            OnBtnSpeakEndIvk();
        }
        //定时器事件
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

                    case TimerType.TIMER_ADDSCORE:
                        {
                            if (!_bStartGame)
                                return;
                            if (_bCurrentUser == GetSelfChair())
                            {
                                NPacket packet = NPacketPool.GetEnablePacket();
                                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GIVE_UP);
                                SendMsgToServer(packet);
                                _bPlayStatus[GetSelfChair()] = (byte)PlayerState.GIVEUP;
                            }
                            break;
                        }
                    case TimerType.TIMER_OPEN:
                        {
                            byte[] bcardsdata = new byte[GameLogic.MAX_COUNT];
                            Buffer.BlockCopy(_bHandCardData[GetSelfChair()], 0, bcardsdata, 0, 4);
                            Buffer.BlockCopy(_bMyCardData, 0, bcardsdata, 0, 2);
                            SlipCardAfter(bcardsdata, 4);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                UIMsgBox.Instance.Show(true, ex.Message);
            }
        }
        /////////////////////////////游戏特殊/////////////////////////////

        #endregion


        #region ##################UI 控制#######################

        /////////////////////////////游戏通用/////////////////////////////
		void OpenHandCardData(byte bchair, byte[] cards, byte count, int cardCount, bool displayItem)
        {
            byte bViewID = ChairToView(bchair);
            UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
			ctr.OpenCardData(cards, count, cardCount, displayItem);
        }

        void SendHandCard(byte bchair, byte[] cards, byte cardcount)
        {
            {
                byte bViewID = ChairToView(bchair);
                UICardControl ctr = o_player_cards[bViewID].GetComponent<UICardControl>();
                ctr.DisplayItem = true;
                if (_bBackCardNum == 0)
                {
                    ctr._cardcount = 0;
                }
                ctr.AppendHandCard(bViewID, cards, cardcount);
            }
        }

        void AppendChips(byte bchair, int nUserChips)
        {
            byte bViewID = ChairToView(bchair);
            UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
            ctr.AddChips(bViewID, nUserChips, _lCellScore);

            PlayGameSound(GameSoundType.SETSCORE);
        }

        void SetUserClock(byte chair, uint time, TimerType timertype)
        {
            for (int i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                o_player_clock[i].SetActive(false);
            }
            if (chair != GameLogic.NULL_CHAIR && o_setwinnows.activeSelf == false)
            {
                _bTimerType = timertype;
                o_player_clock[ChairToView(chair)].GetComponent<UIClock>().SetTimer(time * 1000);
                o_player_clock[ChairToView(chair)].SetActive(true);           
            }
            else
            {
                o_player_clock[ChairToView(chair)].GetComponent<UIClock>().SetTimer(0);
                o_player_clock[ChairToView(chair)].SetActive(false);
            }
        }

        void SetUserReady(byte chair, bool bshow)
        {
            if (chair == GameLogic.NULL_CHAIR)
            {
                for (byte i = 0; i < GameLogic.GAME_PLAYER; i++)
                {
                    o_player_option[i].SetActive(false);
                }
            }
            else
            {
                byte viewId = ChairToView(chair);
                o_player_option[viewId].SetActive(bshow);
            }
        }

        void UpdateUserView()
        {
            if (_bStart == false) return;
            for (uint i = 0; i < GameLogic.GAME_PLAYER; i++)
            {
                byte bViewId = ChairToView((byte)i);
                PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                if (userdata != null)
                {

                    o_player_obj[bViewId].SetActive(true);
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
                        o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(1f, 0, 0);
                    }
                    else
                    {
                        o_player_nick[bViewId].GetComponent<UILabel>().color = new Color(0.35f, 0.8f, 0.8f);
                    }
                    o_player_nick[bViewId].GetComponent<UILabel>().text = userdata.NickName;
                    //face
                    if (_bUserInvent[i] == true)
                    {
                        l_player_addscore[bViewId].text = (_lTableScore[i] + _lCellScore).ToString();
                        if ((_linvertscore[i] - (_lTableScore[i] + _lCellScore)) >= 0)
                        {
                            l_player_bobonum[bViewId].text = (_linvertscore[i] - (_lTableScore[i] + _lCellScore)).ToString();
                        }
                    }

                    if (strfaceName == "face_robot")
                    {

                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace(-2, (int)userdata.VipLevel);
                    }
                    else if (strfaceName == "face_offline")
                    {
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
                    }
                    else
                    {
                        o_player_face[bViewId].GetComponent<UIFace>().ShowFace((int)userdata.HeadID, (int)userdata.VipLevel);
                    }
                    //准备
                    if (userdata.UserState == (byte)UserState.US_READY)
                    {
                        SetUserReady((byte)i, true);
                    }
                    else
                    {
                        SetUserReady((byte)i, false);
                    }
                }
                else if (IsOpenCard == false && o_settlement.activeSelf == false)
                {
                    //nick
                    o_player_nick[bViewId].GetComponent<UILabel>().text = "";
                    o_player_option[bViewId].SetActive(false);
                    o_player_obj[bViewId].SetActive(false);
                }
            }
        }

        void ShowInfoBar()
        {
			transform.Find("Top_bar").gameObject.SetActive(true);
            o_player_info.SetActive(true);
            obj_option.SetActive(false);
        }

        void ShowUserInfo(byte bViewID, bool bshow)
        {
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
            return (byte)GameEngine.Instance.MySelf.DeskStation;
        }

        void PlayGameSound(GameSoundType sound)
        {
            float fvol = NGUITools.soundVolume;
            NGUITools.PlaySound(_GameSound[(int)sound], fvol, 1);
        }

        void PlayUserSound(int sound, byte bGender)
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
        }

        void ShowNotice(string strMsg)
        {
            UIMsgBox.Instance.Show(true, strMsg);
        }

        void ClearAllInfo()
        {
        }

        //禁用游戏下注操作按钮
        void ForbiddenChipBtn(bool IsShow)
        {
            btn_bet20.GetComponent<UIButton>().isEnabled = IsShow;
            btn_bet50.GetComponent<UIButton>().isEnabled = IsShow;
            btn_bet100.GetComponent<UIButton>().isEnabled = IsShow;
            btn_bet200.GetComponent<UIButton>().isEnabled = IsShow;
            btn_bet500.GetComponent<UIButton>().isEnabled = IsShow;
            btn_betmax.GetComponent<UIButton>().isEnabled = IsShow;
            btn_betmin.GetComponent<UIButton>().isEnabled = IsShow;

			if(IsShow == true)
			{
				btn_bet100.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
				btn_bet200.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
				btn_bet500.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
			}
			else
			{
				btn_bet100.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
				btn_bet200.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
				btn_bet500.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
			}
        }
        /////////////////////////////游戏特殊/////////////////////////////

        void CheckIsSelfAddScroce()
        {
            if (_bCurrentUser == GetSelfChair())
            {
                _laddscore = _lTurnLessScore;
                _lTurnMaxScore = _linvertscore[GetSelfChair()] - _lTableScore[GetSelfChair()] - _lCellScore;
                SetCurrAddScore();
                if(_linvertscore[_bCurrentUser] - _lTableScore[_bCurrentUser] - _lCellScore > 0)
                {
                    o_bet_shade.SetActive(false);
                    obj_option.SetActive(true);
                    ForbiddenChipBtn(true);
                    ShowOperateButton(true);
                }
            }
            else
            {
                o_bet_shade.SetActive(true);
                ForbiddenChipBtn(false);
                uibtn_big.isEnabled = false;
                //obj_option.SetActive(false);
                ShowOperateButton(false);
            }
        }
        /// <summary>
        /// 设置  跟  加注BUTTON状态
        /// </summary>
        void CheckAddScroceStatue()
        {
            if (_bCurrentUser == GetSelfChair())
            {
                //跟
                if (_linvertscore[_bCurrentUser] - _lTableScore[_bCurrentUser] - _lCellScore <= _lTurnLessScore)
                {
                    uibtn_follow.isEnabled = false;
                    uibtn_big.isEnabled = false;
                }
                else if (_lTurnLessScore == 0)
                {
                    uibtn_follow.isEnabled = false;
                    uibtn_big.isEnabled = false;
                }
                else
                {
                    uibtn_follow.isEnabled = true;
                }

                //休
                /*
                if (_bCurrentUser == _firstUser)
                {
                    uibtn_stop.isEnabled = true;
                }
                else
                {
                    uibtn_stop.isEnabled = false;
                }
                 * */
                if (_lOneScore == 0)
                {
                    uibtn_stop.isEnabled = true;
                }
                else
                {
                    uibtn_stop.isEnabled = false;
                }
                //需要敲
                if (_lTurnLessScore >= _linvertscore[_bCurrentUser] - _lTableScore[_bCurrentUser] - _lCellScore)
                {
                    uibtn_follow.isEnabled = false;
                    uibtn_big.isEnabled = false;
                    uibtn_stop.isEnabled = false;
                    btn_bet20.GetComponent<UIButton>().isEnabled = false;
                    btn_bet50.GetComponent<UIButton>().isEnabled = false;
                    btn_bet100.GetComponent<UIButton>().isEnabled = false;
                    btn_bet200.GetComponent<UIButton>().isEnabled = false;
                    btn_bet500.GetComponent<UIButton>().isEnabled = false;
                    btn_betmax.GetComponent<UIButton>().isEnabled = false;
                    btn_betmin.GetComponent<UIButton>().isEnabled = false;
                    btn_bet100.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
                    btn_bet200.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
                    btn_bet500.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.gray;
                }
                else
                {
                    btn_bet20.GetComponent<UIButton>().isEnabled = true;
                    btn_bet50.GetComponent<UIButton>().isEnabled = true;
                    btn_bet100.GetComponent<UIButton>().isEnabled = true;
                    btn_bet200.GetComponent<UIButton>().isEnabled = true;
                    btn_bet500.GetComponent<UIButton>().isEnabled = true;
                    btn_betmax.GetComponent<UIButton>().isEnabled = true;
                    btn_betmin.GetComponent<UIButton>().isEnabled = true;
                    btn_bet100.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
                    btn_bet200.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
                    btn_bet500.transform.Find("sp_txt").GetComponent<cx_number>().m_cColor = Color.white;
                }
                SetCurrAddScore();
            }
        }

        void ClearUserReady()
        {
            SetUserReady(GameLogic.NULL_CHAIR, false);
        }

        #endregion


        #region 发送消息给服务器
        public static void SendMsgToServer(NPacket packet)
        {
            if (_bEndGame)
            {
                return;
            }
            GameEngine.Instance.Send(packet);
        }
        #endregion
        /// <summary>
        /// 加注-显示
        /// </summary>
        /// <param name="userChar"></param>
        /// <param name="actionType"></param>
        /// <param name="num"></param>
        void SetInvertAction(byte userChar, BetType actionType, long num)
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                o_player_action[i].SetActive(false);
                o_player_clock[i].SetActive(false);
            }
            o_player_action[ChairToView(userChar)].SetActive(true);
            string spname = "";
            switch (actionType)
            {
                case BetType.Big: spname = "big"; break;
                case BetType.Follow: spname = "follow"; break;
                case BetType.Knock: spname = "knock"; break;
                case BetType.Lose: spname = "giveup"; break;
                case BetType.Stop: spname = "sleep"; break;
                default:
                    spname = "sleep";
                    break;
            }

            Transform tr = o_player_action[ChairToView(userChar)].transform;
            tr.Find("action").GetComponent<UISprite>().spriteName = spname;
            tr.Find("num").GetComponent<UILabel>().text = num.ToString();

            if (_bPlayStatus[userChar] == (byte)PlayerState.GIVEUP)
            {
                tr.Find("action").GetComponent<UISprite>().spriteName = "giveup";
                tr.Find("num").GetComponent<UILabel>().text = "0";
            }
        }

        /// <summary>
        /// 设置-庄
        /// </summary>
        void SetBanker()
        {
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                if (ChairToView(_bBankerUser) == i)
                {
                    o_player_flag[i].SetActive(true);
                }
                else
                {
                    o_player_flag[i].SetActive(false);
                }
            }
            PlayGameSound(GameSoundType.BANKER);
        }

        /// <summary>
        /// 设置总加注
        /// </summary>
        void SetAllInvertData()
        {
            long num = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                if (_lTableScore[i] > 0)
                {
                    num += _lTableScore[i];
                }
                PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(i);
                if (userdata != null)
                {
                    if (_lAllCellScore[i] > 0)
                    {
                        num += _lAllCellScore[i];
                    }
                }
            }
            UINumControl numcrl = o_player_allinvert.GetComponent<UINumControl>();
            if (numcrl == null)
            {
                numcrl = o_player_allinvert.AddComponent<UINumControl>();
            }
            numcrl.SetNum(num);
        }

        /// <summary>
        /// 继续游戏,清理数据
        /// </summary>
        void ClearGameContinue()
        {
            _lTurnMaxScore = 0;
            _lTurnLessScore = 0;
            _bCardNum = 2;
            _bBackCardNum = 0;
            Array.Clear(_lTableScore, 0, GameLogic.GAME_PLAYER);
            Array.Clear(_bPlayStatus, 0, GameLogic.GAME_PLAYER);
            Array.Clear(_linvertscore, 0, GameLogic.GAME_PLAYER);
            UIChipControl ctr = o_dlg_chips.GetComponent<UIChipControl>();
            if (ctr != null)
            {
                ctr.ClearChips();
            }
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                if (o_player_cards[i] == null || o_player_cards[i].transform.childCount < 3) continue;
                o_player_cards[i].transform.GetChild(0).gameObject.SetActive(false);
                o_player_cards[i].transform.GetChild(1).gameObject.SetActive(false);
                o_player_cards[i].transform.GetChild(2).gameObject.SetActive(false);
                while (o_player_cards[i].transform.childCount > 3)
                {
                    DestroyImmediate(o_player_cards[i].transform.GetChild(3).gameObject);
                }
                _bPlayStatus[i] = (byte)PlayerState.NULL;
            }
            ResetGameView();
            SetAllInvertData();
        }

        void MsgShow(bool bshow, string strMsg)
        {
            o_MsgBox.transform.gameObject.SetActive(bshow);
            o_Msg.GetComponent<UILabel>().text = strMsg;
            //o_MsgBox.transform.localPosition = new Vector3(0, Screen.height / 8 * 3, -100);
            Invoke("hideMsg", 3.0f);
        }

        void hideMsg()
        {
            o_MsgBox.transform.gameObject.SetActive(false);
        }


        void OnMusicChange()
        {
            float mVol = o_backGroundMusic.GetComponent<UISlider>().sliderValue;
            transform.parent.Find("scene_game").GetComponent<AudioSource>().volume = mVol;
        }

        void OnEffectChange()
        {
            float eVol = o_effectMusic.GetComponent<UISlider>().sliderValue;
            NGUITools.soundVolume = eVol;
        }

        public static void StopBGM()
        {
            if (o_backGroundMusic.GetComponent<UISlider>().sliderValue == 0)
            {
                o_backGroundMusic.GetComponent<UISlider>().sliderValue = _musicVol;
            } 
            else
            {
                _musicVol = o_backGroundMusic.GetComponent<UISlider>().sliderValue;
                o_backGroundMusic.GetComponent<UISlider>().sliderValue = 0;
            }
        }

        public static void StopEffect()
        {
            if (o_effectMusic.GetComponent<UISlider>().sliderValue == 0)
            {
                o_effectMusic.GetComponent<UISlider>().sliderValue = _effectVol;
            }
            else
            {
                _effectVol = o_effectMusic.GetComponent<UISlider>().sliderValue;
                o_effectMusic.GetComponent<UISlider>().sliderValue = 0;
            }
        }

    }
}