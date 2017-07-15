using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Services.Data;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Utility;
using System.Runtime.InteropServices;


namespace com.QH.QPGame.BRPM
{
    public class UIGame : MonoBehaviour
    {


        #region #############################################################变量定义######################################################################

        //界面
        public GameObject BetPanel;                                                         //下注界面
        public GameObject WaitPanel;                                                        //游戏进行中（等待界面）
        public GameObject RulesPanel;												        //游戏规则界面
        public GameObject o_result;													        //结算界面
        public GameObject o_notes;													        //游戏记录界面
        public GameObject o_msgbox;													        //提示框
        public GameObject conCealTween;                                                     //隐藏打开界面
        public GameObject o_setting;                                                        //设置界面
        public GameObject Trendeffect;                                                      //提示特效

        //预制
        public GameObject HorsePanbel;												        //跑马场景
        public GameObject chipDownScore;                                                    //下注金额
        

        //金币按钮
        public GameObject[] chip_btn = new GameObject[5];
        //金币按钮数值
        private Int64[] chip_value = new Int64[5];
        //下注区域按钮
        public GameObject[] area_btn = new GameObject[GameXY.AREA_ALL];
        //下注界面马对应下注闪烁
        public GameObject[] horse_bg = new GameObject[GameXY.HORSES_ALL]; 

        //玩家信息
        public GameObject o_player_money;											        //当前玩家总金币（UI显示）
        private long player_totalMoney = 0;								                    //玩家总金币(值)
        public GameObject o_player_face;											        //当前玩家头像
        public GameObject o_player_nick;												    //当前玩家用户名
        private  long[] s_lPlayerTableScore = new long[GameXY.AREA_ALL];		            //当前玩家在各区域下注筹码(值)
        private GameObject[] o_PlayerTableScore = new GameObject[GameXY.AREA_ALL];          //当前玩家在各区域下注筹码(UI显示)
        private long l_PlayerAllTableScore = 0;                                             //玩家下注总金币（值）
        private GameObject o_PlayerAllTableScore = null;                                    //玩家下注总金币（UI显示）
        private GameObject m_MySelfScorelbl;										        //玩家金币（输赢金币显示）
        private long m_selfScore = 0;											            //玩家金币（输赢金币）
        private long m_selfReturnBet = 0;                                                   //结算返回玩家低注
        private long l_AllTableScore = 0;                                                   //总区域限制
        private int areaId = 0;                                                             //

        //全部玩家信息总和
        private GameObject[] o_TableScore = new GameObject[GameXY.AREA_ALL];                //各区域下注筹码数值(UI显示)
        private long[] s_lTableScore = new long[GameXY.AREA_ALL];		                    //区域下注筹码(值)

        //小部件控制
        private GameObject o_ReadyControl = null;                                           //下注结束准备动画控制
		private GameObject o_horsePanel = null;                                             //储存的生成的horsePanel预制
		private GameObject o_horseControl = null;											//跑马控制
		private GameObject o_doorControl = null;											//门控制
        private GameObject showInterface = null;                                            //当前显示界面

        private GameObject o_clock_num = null;										        //时钟数值
        private int nJushu_num = 0;                                                         //局数        
        private GameObject jushu_num = null;                                                //局数(显示)
        private int multipleResult = 0;                                                     //中奖倍数

        //通用数据
        private byte[] horseRanking = new byte[GameXY.HORSES_ALL];                  //马匹名次
        private int[] areaMultiple = new int[GameXY.AREA_ALL];                      //区域倍数
        private byte[] userIdRanking = new byte[GameXY.GAME_PLAYER];                //结算成绩排行id
        private Int64[] userScoreRanking = new Int64[GameXY.GAME_PLAYER];           //结算成绩排行
        private static float s_nQuitDelay = 0;										//记录游戏开始到退出游戏的时间
        private static float s_nInfoTickCount = 0;									//记录系统启动后的时间
        private long chipScore = 0;                                                 //当前筹码(当前玩家选择的筹码)
        private string boxStr = "";													//当前提示信息（储存当前提示内容）
        private int m_cbTimeLeave = 0;											    //时间
        private long m_AreaLimit = 0;											    //区域限制
        private long m_PlayerAreaLimit = 0;                                         //个人区域限制
        private long m_PlayerLimitAllSocre = 0;                                     //个人下注总限制
        private float showTime = 0;                                                 //界面信息显示时间（计时）
        private long[] m_RecordAreaChipValue = new long[GameXY.AREA_ALL];           //记录上一把下注的数据 
        public  long m_UpdateScore = 0;                                             //记录得分的变化

        //续压数据储存
        public GameObject o_ContinueBetBtn;                                         //续压按钮
        public GameObject o_CleanBetBtn;                                            //清除下注按钮
        private long[] m_ContinueBet = new long[GameXY.AREA_ALL];
        private long m_ContinueBetAllScore = 0;

        #endregion




        #region #############################################################初始化######################################################################

        void Awake()
        {
            UnityEngine.Time.fixedDeltaTime = 0.005f;

            o_clock_num = GameObject.Find("BetPanel/Bet_BG/time_bg/bet_time");
            jushu_num = GameObject.Find("BetPanel/jushu_spl/value");
            o_PlayerAllTableScore = GameObject.Find("BetPanel/Bet_BG/bet_score/PlayerAllScore/score");
            o_ReadyControl = GameObject.Find("ReadyControl");

            for(int i=0; i<GameXY.AREA_ALL; i++)
            {
                o_PlayerTableScore[i] = area_btn[i].transform.Find("player_score").gameObject;
                o_TableScore[i] = area_btn[i].transform.Find("all_score").gameObject;
            }

            AddUIEvent();
        }
        void OnDisable()
        {
            UnityEngine.Time.fixedDeltaTime = 0.02f;
        }

        void AddUIEvent()
        {

            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                UIEventListener.Get(area_btn[i]).onClick = OnClick;
            }

                for (int i = 0; i < 5; i++)
                {
                    UIEventListener.Get(chip_btn[i]).onClick = OnClick;
                }
        }

        void OnClick(GameObject btnObj)
        {
            if (btnObj.name.Contains("area_"))
            {
                OnBtnAreaIvk(btnObj.name);
            }
            else if (btnObj.name.Contains("btn_chip_"))
            {
                OnBtnAddChipIvk(btnObj.name);
            }
        }

        void Start()
        {
			InitGameView();
        }

        void Update()
        {
//             if (conCealTween.active == true)
//             {
//                 showTime += Time.deltaTime;
// 
//                 if (showTime > 7.0f)
//                 {
//                     if (o_msgbox != null)
//                     {
//                         o_msgbox.SetActive(false);
//                     }
// 
//                     ConCealTween();
//                     showTime = 0;
//                 }
//             }
        }

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

        void FixedUpdate()
        {
            if ((Time.realtimeSinceStartup - s_nInfoTickCount) > 5)
            {
                s_nInfoTickCount = 0;
            }
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
            {
                OnBtnBackIvk();
            }
//             if ((Time.realtimeSinceStartup - s_nQuitDelay) > 1 && s_bReqQuit == true)
//             {
//                 s_bReqQuit = false;
//                 s_nQuitDelay = Time.realtimeSinceStartup;
// 
//                 UIManager.Instance.GoUI(enSceneType.SCENE_GAME, enSceneType.SCENE_SERVER);
//             }
        }

        void InitGameView()
        {
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                m_RecordAreaChipValue[i] = 0;
            }
            HideTableScore();
        }
        #endregion





        #region #############################################################引擎调用######################################################################

        //加载跑马场景
		void AddHorseScene()
		{
			//生成跑马场景
			o_horsePanel = Instantiate(HorsePanbel, Vector3.one, Quaternion.identity) as GameObject;
			o_horsePanel.transform.parent = this.transform;
			o_horsePanel.transform.localPosition = Vector3.one;
			o_horsePanel.transform.localScale = Vector3.one;
 
			o_horseControl = o_horsePanel.transform.Find("Horse").gameObject;
			o_doorControl = o_horsePanel.transform.Find("door").gameObject;
		}

        //发生框架事件时刷新客户端(相对于框架消息)
        void UpdateUserView()
        {
            PlayerInfo playerdata = GameEngine.Instance.MySelf;
            o_player_nick.GetComponent<UILabel>().text = playerdata.NickName;
            o_player_face.transform.GetComponent<UIFace>().ShowFace((int)playerdata.HeadID, (int)playerdata.VipLevel);
            UpdateButtonContron();
        }

        //游戏状态切换时更新(游戏消息未调用)
        void UpdateGameUserView()
         {
             PlayerInfo playerdata = GameEngine.Instance.MySelf;
             o_player_nick.GetComponent<UILabel>().text = playerdata.NickName;
             o_player_face.transform.GetComponent<UIFace>().ShowFace((int)playerdata.HeadID, (int)playerdata.VipLevel);
             player_totalMoney = playerdata.Money;
             o_player_money.GetComponent<label_number>().m_iNum = player_totalMoney;
             areaId = 0;
             UpdateButtonContron();
         }

        #endregion





        #region #############################################################框架消息######################################################################
        //框架事件入口
        private void OnTableUserEvent(TableEvents tevt, uint userid, object data)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;

            switch (tevt)
            {

                case TableEvents.GAME_START:
                    {                        
                        //游戏开始
                        Debug.Log("<color=red> GAME_START</color>");
                        UpdateUserView();
                        UpdateGameUserView();
                        break;
                    }
                case TableEvents.USER_COME:
                    {
                        //用户进入
                        Debug.Log("<color=red> USER_COME</color>");
                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_LEAVE:
                    {
                        //用户离开时调用
                        Debug.Log("<color=red> USER_LEAVE</color>");
                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_READY:
                    {
                        //用户准备
                        Debug.Log("<color=red> USER_READY</color>");
                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_PLAY:
                    {
                        Debug.Log("<color=red> USER_PLAY</color>");
                        UpdateUserView();
                        break;
                    }
                case TableEvents.USER_OFFLINE:
                    {
                        //用户掉线
                        Debug.Log("<color=red> USER_OFFLINE</color>");
                        UpdateUserView();
                        break;
                    }
                case TableEvents.GAME_ENTER:
                    {
                        //进入时
                        Debug.Log("<color=red> GAME_ENTER</color>");
                        InitGameView();
                        UpdateUserView();
                        UpdateGameUserView();
                        break;
                    }
            }
        }

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;
            switch (subcmd)
            {

                case SubCmd.SUB_GF_OPTION:
                    {
                        Debug.Log("<color=red> 框架消息+游戏配置</color>");
                        OnGameOptionResp(packet);
                        break;
                    }
                case SubCmd.SUB_GF_SCENE:
                    {
                        Debug.Log("<color=red> 框架消息+场景信息</color>");
                        //信息的初始化
                        chipScore = 0;
                        ShowInterface(o_result.transform, false);

                        //初始化筹码按钮
                        onBtnSelect();
				
						OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
                case SubCmd.SUB_GF_MESSAGE:
                    {
                        Debug.Log("<color=red> 框架消息+系统消息</color>");
                        OnGameMessageResp(packet);
                        break;
                    }

            }
        }

        //游戏设置消息处理函数
        void OnGameOptionResp(NPacket packet)
        {
            packet.BeginRead();
            GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
        }

        //系统消息
        void OnGameMessageResp(NPacket packet)
        {
            Debug.Log("<color=red>系统消息</color>");
            packet.BeginRead();
            ushort wType = packet.GetUShort();
            ushort wlen = packet.GetUShort();
            string strMsg = packet.GetString(wlen);
            if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
                (wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
            {
                Invoke("OnConfirmBackOKIvk", 2.0f);
            }

            if ((wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
            {
                Invoke("OnConfirmBackOKIvk", 2.0f);
            }


        }

        #region ################################################################场景消息处理###########################################################################

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {
            //Debuger.LogError(bGameStatus);
            switch (bGameStatus)
            {
			//空闲状态
			case (byte)GameXY.GS_FREE:
			{
                Debug.Log("<color=red>框架消息+空闲状态</color>");
                AudioSoundManger_PM._instance.PlayBgSound("betbgmusic");
                GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_FREE;
				SwitchFreeSceneView(packet);
				GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
				break;
			}
				
			//游戏状态(下注)
            case (byte)GameXY.GS_BET:
			{
                Debug.Log("<color=red>框架消息+下注状态</color>");
                AudioSoundManger_PM._instance.PlayBgSound("betbgmusic");
                GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET;
				SwitchBetSceneView(packet);
				GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
				break;
			}
				
			//下注结束状态（准备）
            case (byte)GameXY.GS_BET_END:
			{
                Debug.Log("<color=red>框架消息+下注结束状态</color>");
                AudioSoundManger_PM._instance.PlayBgSound("runbgmusic");
                GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET_END;
				SwitchBetEndSceneView(packet);
				GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
				break;
			}

            //结束状态（跑马开始）
            case (byte)GameXY.GS_HORSES:
            {
                Debug.Log("<color=red>框架消息+跑马开始状态</color>");
                AudioSoundManger_PM._instance.PlayBgSound("runbgmusic");
                GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_HORSES;
				SwitchHorsesSceneView(packet);
                GameStateChangeStart(GameEngine.Instance.MySelf.GameStatus);
                break;
            }
            }
        }

        //空闲场景信息
        void SwitchFreeSceneView(NPacket packet)
        {
			packet.BeginRead();
			//去掉包头
			byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
			int len = Marshal.SizeOf(typeof(CMD_S_SceneFree));
			packet.GetBytes(ref _buffer, len);

			CMD_S_SceneFree gamedata;
			gamedata = GameConvert.ByteToStruct<CMD_S_SceneFree>(_buffer);

            //信息赋值
            m_cbTimeLeave = 0;
            //o_clock_num.GetComponent<label_number>().m_iNum = 0;                              //时间
            o_clock_num.SetActive(false);
            nJushu_num = gamedata.nStreak;
            jushu_num.GetComponent<label_number>().m_iNum = nJushu_num;                        //场次

            //区域倍数
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = gamedata.nMultiple[i];                                      
                area_btn[i].transform.Find("multiple/multiple_value").GetComponent<label_number>().m_iNum = areaMultiple[i];
            }
            //游戏说明
            foreach (Transform child in RulesPanel.transform.Find("help_data").transform)
            {
                child.GetComponent<UILabel>().text = areaMultiple[int.Parse(child.name)].ToString();
            }

            Debug.Log("<color=red>" + gamedata.nWinCount + "全天赢的场次" + "</color>");
                                                                                                //游戏记录（赋值）
             m_AreaLimit = gamedata.lAreaLimitScore;										    //区域限制
             m_PlayerAreaLimit = gamedata.lUserLimitScore;                                      //个人区域限制
             m_PlayerLimitAllSocre = gamedata.lUserLimitAllScore;                               //个人下注总限制
             for (int i = 0; i < 5; i++)
             {
                 chip_btn[i].transform.Find("value").GetComponent<label_number>().m_iNum = gamedata.nChipScore[i];
                 chip_value[i] = gamedata.nChipScore[i];
             }

            //续压数值
             m_ContinueBetAllScore = 0;
             for (int i = 0; i < GameXY.AREA_ALL; i++)
             {
                 m_ContinueBet[i] = gamedata.nPlayerContinueScore[i];
                 m_ContinueBetAllScore += m_ContinueBet[i];
             }
             if (m_ContinueBetAllScore == 0) o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
             o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;

             //游戏记录
             byte[] horseRanking = new byte[20];
             byte[] horseMultiple = new byte[20];
             byte horsecount = 0;
             for (int i = 0; i < 20; i++)
             {
                 if (gamedata.nRiskCompensateRecord[i] > 0)
                 {
                     horseRanking[horsecount] = (byte)gamedata.nRankingRecord[i];
                     horseMultiple[horsecount] = (byte)gamedata.nRiskCompensateRecord[i];
                     horsecount++;
                 }
             }
             SetResultNote(horseRanking, horseMultiple, horsecount);

            //遮罩
             WaitPanel.SetActive(true);

             UpdateButtonContron();
        }

        //下注状态场景信息
        void SwitchBetSceneView(NPacket packet)
        {
			packet.BeginRead();
			//去掉包头
			byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
			int len = Marshal.SizeOf(typeof(CMD_S_SceneBet));
			packet.GetBytes(ref _buffer, len);
			
			CMD_S_SceneBet gamedata;
			gamedata = GameConvert.ByteToStruct<CMD_S_SceneBet>(_buffer);

            //信息赋值
            m_cbTimeLeave = gamedata.nTimeLeave;
            SetUserClock(m_cbTimeLeave-1);                                                       //计时

            nJushu_num = gamedata.nStreak;
            jushu_num.GetComponent<label_number>().m_iNum = nJushu_num;                        //场次
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = gamedata.nMultiple[i];                                      //区域倍数
                area_btn[i].transform.Find("multiple/multiple_value").GetComponent<label_number>().m_iNum = areaMultiple[i];
            }
            foreach (Transform child in RulesPanel.transform.Find("help_data").transform)
            {
                child.GetComponent<UILabel>().text = areaMultiple[int.Parse(child.name)].ToString();
            }
            Debug.Log("<color=red>" + gamedata.nWinCount + "全天赢的场次" + "</color>");
            //游戏记录（赋值）
            m_AreaLimit = gamedata.lAreaLimitScore;										        //区域限制
            m_PlayerAreaLimit = gamedata.lUserLimitScore;                                       //个人区域限制
            m_PlayerLimitAllSocre = gamedata.lUserLimitAllScore;                               //个人下注总限制

            //遮罩
            WaitPanel.SetActive(false);

            for (int i=0; i < GameXY.AREA_ALL; i++)
            {
                s_lPlayerTableScore[i] = gamedata.lPlayerBet[i];
                o_PlayerTableScore[i].GetComponent<label_number>().m_iNum = s_lPlayerTableScore[i];

                if (s_lPlayerTableScore[i] > 0)
                {
                    o_PlayerTableScore[i].SetActive(true);
                }
                else
                {
                    o_PlayerTableScore[i].SetActive(false);
                }

                l_PlayerAllTableScore += s_lPlayerTableScore[i];

                o_PlayerAllTableScore.GetComponent<label_number>().m_iNum = l_PlayerAllTableScore;
                o_PlayerAllTableScore.SetActive(true);
            }
            if (l_PlayerAllTableScore > 0)
            {
                o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            }

            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                s_lTableScore[i] = gamedata.lPlayerBetAll[i];
                o_TableScore[i].GetComponent<label_number>().m_iNum = s_lTableScore[i];

                if (s_lTableScore[i] > 0)
                {
                    o_TableScore[i].SetActive(true);
                }
                else
                {
                    o_TableScore[i].SetActive(false);
                }          
            }

            for (int i = 0; i < 5; i++)
            {
                chip_btn[i].transform.Find("value").GetComponent<label_number>().m_iNum = gamedata.nChipScore[i];
                chip_value[i] = gamedata.nChipScore[i];
            }

            //续压数值
            m_ContinueBetAllScore = 0;
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                m_ContinueBet[i] = gamedata.nPlayerContinueScore[i];
                m_ContinueBetAllScore += m_ContinueBet[i];
            }
            if (m_ContinueBetAllScore == 0) o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;

            //游戏记录
            byte[] horseRanking = new byte[20];
            byte[] horseMultiple = new byte[20];
            byte horsecount = 0;
            for (int i = 0; i < 20; i++)
            {
                if (gamedata.nRiskCompensateRecord[i] > 0)
                {
                    horseRanking[horsecount] = (byte)gamedata.nRankingRecord[i];
                    horseMultiple[horsecount] = (byte)gamedata.nRiskCompensateRecord[i];
                    horsecount++;
                }
            }
            SetResultNote(horseRanking, horseMultiple, horsecount);

/*            Debug.Log("<color=red>" + gamedata.szGameRoomName + "房间名字" + "</color>");*/

            UpdateButtonContron();
        }

		//下注结束场景信息
        void SwitchBetEndSceneView(NPacket packet)
		{
			packet.BeginRead();
			//去掉包头
			byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
			int len = Marshal.SizeOf(typeof(CMD_S_SceneBetEnd));
			packet.GetBytes(ref _buffer, len);
			
			CMD_S_SceneBetEnd gamedata;
			gamedata = GameConvert.ByteToStruct<CMD_S_SceneBetEnd>(_buffer);

            //信息赋值
            m_cbTimeLeave = 0;
            //o_clock_num.GetComponent<label_number>().m_iNum = 0;                              //时间
            o_clock_num.SetActive(false);
            nJushu_num = gamedata.nStreak;
            jushu_num.GetComponent<label_number>().m_iNum = nJushu_num;                        //场次
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = gamedata.nMultiple[i];                                      //区域倍数
                area_btn[i].transform.Find("multiple/multiple_value").GetComponent<label_number>().m_iNum = areaMultiple[i];
            }
            foreach (Transform child in RulesPanel.transform.Find("help_data").transform)
            {
                child.GetComponent<UILabel>().text = areaMultiple[int.Parse(child.name)].ToString();
            }
            Debug.Log("<color=red>" + gamedata.nWinCount + "全天赢的场次" + "</color>");

            //游戏记录（赋值）
            m_AreaLimit = gamedata.lAreaLimitScore;										    //区域限制
            m_PlayerAreaLimit = gamedata.lUserLimitScore;                                   //个人区域限制
            m_PlayerLimitAllSocre = gamedata.lUserLimitAllScore;                               //个人下注总限制

            for (int i = 0; i < 5; i++)
            {
                chip_btn[i].transform.Find("value").GetComponent<label_number>().m_iNum = gamedata.nChipScore[i];
                chip_value[i] = gamedata.nChipScore[i];
            }

            //续压数值
            m_ContinueBetAllScore = 0;
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                m_ContinueBet[i] = gamedata.nPlayerContinueScore[i];
                m_ContinueBetAllScore += m_ContinueBet[i];
            }
            if (m_ContinueBetAllScore == 0) o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;

            //游戏记录
            byte[] horseRanking = new byte[20];
            byte[] horseMultiple = new byte[20];
            byte horsecount = 0;
            for (int i = 0; i < 20; i++)
            {
                if (gamedata.nRiskCompensateRecord[i] > 0)
                {
                    horseRanking[horsecount] = (byte)gamedata.nRankingRecord[i];
                    horseMultiple[horsecount] = (byte)gamedata.nRiskCompensateRecord[i];
                    horsecount++;
                }
            }
            SetResultNote(horseRanking, horseMultiple, horsecount);

            //遮罩
            WaitPanel.SetActive(true);

            UpdateButtonContron();
		}

		//跑马场景信息
		void SwitchHorsesSceneView(NPacket packet)
		{
			packet.BeginRead();
			//去掉包头
			byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
			int len = Marshal.SizeOf(typeof(CMD_S_SceneHorses));
			packet.GetBytes(ref _buffer, len);
			
			CMD_S_SceneHorses gamedata;
			gamedata = GameConvert.ByteToStruct<CMD_S_SceneHorses>(_buffer);

            //信息赋值
            m_cbTimeLeave = gamedata.nTimeLeave;
            //o_clock_num.GetComponent<label_number>().m_iNum = 0;                              //时间
            o_clock_num.SetActive(false);
            nJushu_num = gamedata.nStreak;
            jushu_num.GetComponent<label_number>().m_iNum = nJushu_num;                        //场次
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = gamedata.nMultiple[i];                                      //区域倍数
                area_btn[i].transform.Find("multiple/multiple_value").GetComponent<label_number>().m_iNum = areaMultiple[i];
            }
            foreach (Transform child in RulesPanel.transform.Find("help_data").transform)
            {
                child.GetComponent<UILabel>().text = areaMultiple[int.Parse(child.name)].ToString();
            }
            Debug.Log("<color=red>" + gamedata.nWinCount + "全天赢的场次" + "</color>");

            //续压数值
            m_ContinueBetAllScore = 0;
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                m_ContinueBet[i] = gamedata.nPlayerContinueScore[i];
                m_ContinueBetAllScore += m_ContinueBet[i];
            }
            if (m_ContinueBetAllScore == 0) o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;

            //游戏记录（赋值）
            m_AreaLimit = gamedata.lAreaLimitScore;										    //区域限制
            m_PlayerAreaLimit = gamedata.lUserLimitScore;                                   //个人区域限制
            m_PlayerLimitAllSocre = gamedata.lUserLimitAllScore;                            //个人下注总限制

            for (int i = 0; i < 5; i++)
            {
                chip_btn[i].transform.Find("value").GetComponent<label_number>().m_iNum = gamedata.nChipScore[i];
                chip_value[i] = gamedata.nChipScore[i];
            }

            //游戏记录
            byte[] horseRanking = new byte[20];
            byte[] horseMultiple = new byte[20];
            byte horsecount = 0;
            for (int i = 0; i < 20; i++)
            {
                if (gamedata.nRiskCompensateRecord[i] > 0)
                {
                    horseRanking[horsecount] = (byte)gamedata.nRankingRecord[i];
                    horseMultiple[horsecount] = (byte)gamedata.nRiskCompensateRecord[i];
                    horsecount++;
                }
            }
            SetResultNote(horseRanking, horseMultiple, horsecount);

            //遮罩
            WaitPanel.SetActive(true);

            UpdateButtonContron();
		}

        //游戏状态切换更新
        void GameStateChangeStart(byte state)
        {
            switch (state)
            {
                case (byte)GameXY.GS_FREE:
                    {
                        break;
                    }
                case (byte)GameXY.GS_BET:
                    {
                        break;
                    }
                case (byte)GameXY.GS_BET_END:
                    {
                        break;
                    }
                case (byte)GameXY.GS_HORSES:
                    {
                        break;
                    }
            }
        }

        #endregion

        #endregion





        #region #############################################################系统消息######################################################################
        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (UIManager.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {
			case SubCmd.SUB_S_BET_START:
			{
                Debug.Log("<color=red> 开始下注</color>");
                OnGameStartResp(packet);
				break;
			}
			case SubCmd.SUB_S_BET_END:
			{
                Debug.Log("<color=red> 下注结束</color>");
                OnGameBetendResp(packet);
				break;
			}
			case SubCmd.SUB_S_HORSES_START:
			{
                Debug.Log("<color=red> 跑马开始</color>");
                OnGameHorseRaceResp(packet);
				break;
            }
			case SubCmd.SUB_S_PLAYER_BET:
			{
                Debug.Log("<color=red> 玩家下注</color>");
                OnGamePlaceJetton(packet);
				break;
			}
			case SubCmd.SUB_S_PLAYER_BET_FAIL:
			{
                Debug.Log("<color=red> 下注失败</color>");
                OnGameLoseFaire(packet);
				break;
			}
			case SubCmd.SUB_S_CONTROL_SYSTEM:
			{              
                Debug.Log("<color=red> 系统控制</color>");
				break;
			}
			case SubCmd.SUB_S_NAMED_HORSES:
			{               
                Debug.Log("<color=red> 马匹冠名</color>");
				break;
			}
			case SubCmd.SUB_S_HORSES_END:
			{
                Debug.Log("<color=red> 跑马结束</color>");
                OnGameHorseEndRep(packet);
				break;
			}
			case SubCmd.SUB_S_MANDATOY_END:
			{                
                Debug.Log("<color=red> 强制结束</color>");
				OnGameComPelEnd(packet);
				break;
			}
			case SubCmd.SUB_S_ADMIN_COMMDN:
			{
                Debug.Log("<color=red> 系统结束</color>");
				break;
			}
            case SubCmd.SUB_S_PLAYER_CLEANBET:
            {
                Debug.Log("<color=red>清除下注</color>");
                CleanBetEvent(packet);
                break;
            }
			}
        }

        //游戏开始（开始下注）
        void OnGameStartResp(NPacket packet)
        {
            GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET;

            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_BetStart));
            packet.GetBytes(ref _buffer, len);

            CMD_S_BetStart gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_BetStart>(_buffer);

            //续压数值
            m_ContinueBetAllScore = 0;
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                m_ContinueBet[i] = gamedata.nPlayerContinueScore[i];
                m_ContinueBetAllScore += m_ContinueBet[i];
            }
           
            if (m_ContinueBetAllScore > 0)
            {
                o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            }

			//删除跑马场景
			Destroy(o_horsePanel);

            //遮罩
            WaitPanel.SetActive(false);

            SetUserClock(gamedata.nTimeLeave);

            UpdateGameUserView();
		}

        //下注结束
        void OnGameBetendResp(NPacket packet)
        {
            AudioSoundManger_PM._instance.PlayBgSound("runbgmusic");
            GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET_END;

            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_BetEnd));
            packet.GetBytes(ref _buffer, len);

            CMD_S_BetEnd gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_BetEnd>(_buffer);  

			//生成跑马场景
			AddHorseScene();

            //关闭下注面板
            ShowInterface(BetPanel.transform, true);

            //准备动画播放
            o_ReadyControl.transform.GetComponent<ShowTweenControl>().IsTween = true;

            Trendeffect.SetActive(false);
		}

        //跑马开始
        void OnGameHorseRaceResp(NPacket packet)
        {
//             UnityEngine.Time.timeScale = 0.1f;
//             UnityEngine.Time.fixedDeltaTime = 0.005f;

            if (WaitPanel.activeSelf == false)
            {
                AudioSoundManger_PM._instance.PlayHorseSound();
                AudioSoundManger_PM._instance.PlaySound("horsecall");
            }
            
            GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_HORSES;

            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_HorsesStart));
            packet.GetBytes(ref _buffer, len);

            CMD_S_HorsesStart gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_HorsesStart>(_buffer);

            //马匹速度
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				for(int k=0; k<GameXY.HORSES_TIME; k++)
				{
					if(o_horseControl != null)
					{
                        o_horseControl.transform.GetComponent<HorseControl>().addSpeed[i, k] = gamedata.HorsesSpeed[i * GameXY.HORSES_TIME + k];
                        //Debug.LogWarning(gamedata.HorsesSpeed[i * GameXY.HORSES_TIME + k] + "+++++++" + i);
					}
				}
			}

            //名次
            for(int i=0; i<GameXY.HORSES_ALL; i++)
            {
                horseRanking[i] = gamedata.cbHorsesRanking[i];

				if(i == 0 && o_horseControl != null)
				{
					o_horseControl.transform.GetComponent<HorseControl>().first = horseRanking[i];
				}

				if(i == 1 && o_horseControl != null)
				{
					o_horseControl.transform.GetComponent<HorseControl>().secound = horseRanking[i];
				}

                if (i < 2) Debug.Log(i + "+" + horseRanking[i] + 1);				
            }

            if (o_horseControl != null)
            {
                o_horseControl.transform.GetComponent<HorseControl>().multipleResult = gamedata.nMultiple;
            }
            multipleResult = gamedata.nMultiple;

            //玩家结算（真实结算）
            m_selfScore = gamedata.lPlayerWinning;
            //返回的玩家低注（不返还低注）
            m_selfReturnBet = gamedata.lPlayerReturnBet;

			if(o_horseControl != null && o_doorControl != null)
			{
				o_horseControl.transform.GetComponent<HorseControl>().PlayAnimation();
				o_horseControl.transform.GetComponent<HorseControl>().IsChageSpeed = true;
				o_doorControl.transform.GetComponent<DoorControl>().PlayAnimation();
                long chipvalue = chip_value[0];
                o_horseControl.transform.GetComponent<HorseControl>().SetBetScore(areaMultiple, s_lPlayerTableScore, chipvalue);
			}            

        }

		//强制结束
		void OnGameComPelEnd(NPacket packet)
		{
            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_HorsesForceEnd));
            packet.GetBytes(ref _buffer, len);

            CMD_S_HorsesForceEnd gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_HorsesForceEnd>(_buffer);

//==========================================================================================================================
            
            //结算框排名数据
            string[] username = new string[20];
            byte playerRanking = 255;
            int userCount = 0;
            for (int i = 0; i < 20; i++)
            {
                userScoreRanking[i] = gamedata.lRankingNote[i];
                userIdRanking[i] = gamedata.lRankingIndex[i];                
                PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer(userIdRanking[i]);
                if (userdata != null)
                {
                    //Debug.LogWarning(userIdRanking[i] + "用户id" + i + "排名" + GetSelfChair() + "玩家id");
                    userCount++;
                    if (userCount <= 20)
                    {
                        username[userCount - 1] = userdata.NickName;
                    }
                }
            }
            long playerScore = m_selfScore;
            playerRanking = gamedata.lPlayerRanking;
            long betscore = o_PlayerAllTableScore.GetComponent<label_number>().m_iNum;
            //写入结算数据
            o_result.GetComponent<ResultController>().SetResultData(areaMultiple, s_lPlayerTableScore, horseRanking[0], horseRanking[1], playerScore, betscore, playerRanking);
            o_result.GetComponent<ResultController>().SetRankingList(username, userScoreRanking);
            //防止网络差声音无法停止
            AudioSoundManger_PM._instance.StopHorseSound();

//====================================================================================================================================

			if(o_horseControl != null)
			{
				o_horseControl.transform.GetComponent<HorseControl>().StopAnimation();
				o_horseControl.transform.GetComponent<HorseControl>().IsChageSpeed = false;
			}

            if (playerScore > 0 && playerScore != 0 && WaitPanel.activeSelf == false)
            {
                AudioSoundManger_PM._instance.PlayResultSound("win");
            }
            else if (playerScore != 0 && WaitPanel.activeSelf == false)
            {
                AudioSoundManger_PM._instance.PlayResultSound("lose");
            }
            //ShowInterface(o_result.transform, true);

            //让停止的吗继续跑动
            if (o_horseControl != null)
            {
                o_horseControl.transform.GetComponent<HorseControl>().IsChageSpeed = true;
                o_horseControl.transform.GetComponent<HorseControl>().horseMoveEnd = true;
                o_horseControl.transform.GetComponent<HorseControl>().PlayAnimation();
                AudioSoundManger_PM._instance.PlaySound("horsefoot");
            }
		}

        //跑马结束
        void OnGameHorseEndRep(NPacket packet)
        {
            AudioSoundManger_PM._instance.PlayBgSound("betbgmusic");

            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_HorsesEnd));
            packet.GetBytes(ref _buffer, len);

            CMD_S_HorsesEnd gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_HorsesEnd>(_buffer);

            //区域倍数
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = gamedata.nMultiple[i];
                area_btn[i].transform.Find("multiple/multiple_value").GetComponent<label_number>().m_iNum = areaMultiple[i];
            }
            //游戏说明
            foreach (Transform child in RulesPanel.transform.Find("help_data").transform)
            {
                child.GetComponent<UILabel>().text = areaMultiple[int.Parse(child.name)].ToString();
            }

            //场次
            nJushu_num++;
            jushu_num.GetComponent<label_number>().m_iNum = nJushu_num;

            Buffer.BlockCopy(s_lPlayerTableScore, 0, m_RecordAreaChipValue, 0, GameXY.AREA_ALL);
            HideTableScore(); 
           
            //打开下注界面(反播放)
            ShowInterface(BetPanel.transform, false);

            //关闭计算界面
            ShowInterface(o_result.transform, false);

            //游戏记录
            byte horseRank1 = (byte)(horseRanking[0] + 1);
            byte horseRank2 = (byte)(horseRanking[1] + 1);
            if(horseRanking[0] > horseRanking[1])
            {
                horseRank1 = (byte)(horseRanking[1] + 1);
                horseRank2 = (byte)(horseRanking[0] + 1);
            }
            o_notes.GetComponent<NoteControl>().AloneNoteData(horseRank1, horseRank2, (byte)multipleResult); 
        }

        //玩家下注
        void OnGamePlaceJetton(NPacket packet)
        {
            GameEngine.Instance.GetTableUserItem(GameEngine.Instance.MySelf.DeskStation);
            GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET;

            packet.BeginRead();
            //去掉包头
           byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
           int len = Marshal.SizeOf(typeof(CMD_S_PlayerBet));
           packet.GetBytes(ref _buffer, len);

           CMD_S_PlayerBet gamedata;
           gamedata = GameConvert.ByteToStruct<CMD_S_PlayerBet>(_buffer);
           Debug.Log(gamedata.nBetPlayerCount + "下注人数");

            byte areaIndex = 0;

            if (gamedata.wChairID == GetSelfChair()) DazzlingHorse(gamedata.areaId);
            AddChipValueShow(gamedata.areaId, gamedata.lBetScore);
            ShowTableScore((byte)gamedata.wChairID, gamedata.areaId, gamedata.lBetScore);
        }

        //下注失败
        void OnGameLoseFaire(NPacket packet)
        {
            Debug.Log("<color=red> 下注失败</color>");
            GameEngine.Instance.MySelf.GameStatus = (byte)GameXY.GS_BET;

            packet.BeginRead();
            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_PlayerBetFail));
            packet.GetBytes(ref _buffer, len);

            CMD_S_PlayerBetFail gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_PlayerBetFail>(_buffer);

            if (player_totalMoney < chip_value[0])
            {
                cnMsgBox("金币不足，无法下注！");
                return;
            }

            if (s_lPlayerTableScore[areaId] + chipScore > m_PlayerAreaLimit)
            {
                cnMsgBox("该区域超出个人下注限制！");     
            }
            else if (l_AllTableScore + chipScore > m_AreaLimit)
            {
                cnMsgBox("该区域超出下注限制！");
            }

        }
        
        #endregion




        #region #############################################################UI事件######################################################################


//===============================================================================================继压与下注===========================================================================

        //续压
        public void OnContinueBetIvk()
        {            
            PlayerInfo playerdata = GameEngine.Instance.MySelf;
            if (playerdata.Money < m_ContinueBetAllScore)
            {
                cnMsgBox("金币不足，无法续压!");
                return;
            }
            if (m_ContinueBetAllScore == 0)
            {
                cnMsgBox("未押注！");
                return;
            }

            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                if (s_lPlayerTableScore[i] + m_ContinueBet[i] > m_PlayerAreaLimit)
                {
                    cnMsgBox("区域超出个人下注限制,无法续压！");
                    return;
                }
                if (l_AllTableScore + m_ContinueBet[i] > m_AreaLimit)
                {
                    cnMsgBox("区域超出下注限制， 无法续压！");
                    return;
                }
            }

            OnCleanBetIvk();

            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                if (m_ContinueBet[i] != 0)
                OnPlaceJetton((byte)i, m_ContinueBet[i]);
            }
        }
        //清除下注
        public void OnCleanBetIvk()
        {
//             if (l_PlayerAllTableScore == 0)
//             {
//                 cnMsgBox("您还没有下注!");
//                 return;
//             } 
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_CLEANBET);
            byte selfid = GetSelfChair();
            packet.Addbyte(selfid);
            GameEngine.Instance.Send(packet);
        }
        //清除下注成功
        void CleanBetEvent(NPacket packet)
        {
            packet.BeginRead();
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_PlayerCelanBetFail));
            packet.GetBytes(ref _buffer, len);

            CMD_S_PlayerCelanBetFail gamedata;
            gamedata = GameConvert.ByteToStruct<CMD_S_PlayerCelanBetFail>(_buffer);

            if (gamedata.wChairID != GetSelfChair()) return;

            l_PlayerAllTableScore = 0;
            o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            o_PlayerAllTableScore.transform.GetComponent<label_number>().m_iNum = 0;
            PlayerInfo playerdata = GameEngine.Instance.MySelf;
            player_totalMoney = playerdata.Money;
            o_player_money.GetComponent<label_number>().m_iNum = player_totalMoney;

            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                s_lTableScore[i] -= s_lPlayerTableScore[i];
                s_lPlayerTableScore[i] = 0;
                o_PlayerTableScore[i].transform.GetComponent<label_number>().m_iNum = 0;
                o_PlayerTableScore[i].gameObject.SetActive(false);
                o_TableScore[i].transform.GetComponent<label_number>().m_iNum = s_lTableScore[i];
                if (s_lTableScore[i] == 0) o_TableScore[i].SetActive(false);
            }

            //跟新按钮状态
            UpdateButtonContron();
        }

 //==========================================================================================================================================================================




        //界面动画控制
        void ShowInterface(Transform obj, bool IsShow)
        {
            if (obj.GetComponent<TweenAlpha>() != null)
            {
                obj.GetComponent<TweenAlpha>().enabled = true;
                if (IsShow)
                {
                    obj.GetComponent<TweenAlpha>().PlayForward();
                }
                else
                {
                    obj.GetComponent<TweenAlpha>().PlayReverse();
                }
            }
            if (obj.GetComponent<TweenPosition>() != null)
            {
                obj.GetComponent<TweenPosition>().enabled = true;
                if (IsShow)
                {
                    obj.GetComponent<TweenPosition>().PlayForward();
                }
                else
                {
                    obj.GetComponent<TweenPosition>().PlayReverse();
                }
            }
            if (obj.GetComponent<TweenRotation>() != null)
            {
                obj.GetComponent<TweenRotation>().enabled = true;
                if (IsShow)
                {
                    obj.GetComponent<TweenRotation>().PlayForward();
                }
                else
                {
                    obj.GetComponent<TweenRotation>().PlayReverse();
                }
            }
            if (obj.GetComponent<TweenScale>() != null)
            {
                obj.GetComponent<TweenScale>().enabled = true;
                if (IsShow)
                {
                    obj.GetComponent<TweenScale>().PlayForward();
                }
                else
                {
                    obj.GetComponent<TweenScale>().PlayReverse();
                }
            }
            if (obj.GetComponent<TweenColor>() != null)
            {
                obj.GetComponent<TweenColor>().enabled = true;
                if (IsShow)
                {
                    obj.GetComponent<TweenColor>().PlayForward();
                }
                else
                {
                    obj.GetComponent<TweenColor>().PlayReverse();
                }
            }
        }

        //获取时间显示
        void SetUserClock(int time)
        {
            o_clock_num.SetActive(true);
            o_clock_num.GetComponent<label_number>().m_iNum = time-1;
            o_clock_num.GetComponent<label_number>().m_bIsTimer = true;
            o_clock_num.GetComponent<label_number>().m_bIsOpen = true;
            AudioSoundManger_PM._instance.PlayTimerSound();
        }

        //获取自己座位号
        byte GetSelfChair()
        {
            return (byte)GameEngine.Instance.MySelf.DeskStation;
        }

        //退出按钮事件
        public void OnBtnBackIvk()
        {
            Debug.Log("<color=red> 退出</color>");
			if (!GameEngine.Instance.IsPlaying() || l_PlayerAllTableScore == 0)
			{
				OnConfirmBackOKIvk();
			}
			else
			{
				cnMsgBox("游戏进行中，无法离开!");
			}
        }

        //确认退出
        void OnConfirmBackOKIvk()
        {
             GameEngine.Instance.Quit();
             s_nQuitDelay = Time.realtimeSinceStartup;
             CancelInvoke();
        }

        //消息框
        public void cnMsgBox(string val)
        {
            if (val == boxStr)
            {
                return;
            }
            if (o_msgbox != null)
            {
                 o_msgbox.SetActive(true);
                 o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = val;
                 Invoke("closeMsgbox", 3.0f);           
            }
            boxStr = val;
        }
        void closeMsgbox()
        {
             boxStr = ",";
             o_msgbox.transform.FindChild("label").gameObject.GetComponent<UILabel>().text = "";
             o_msgbox.SetActive(false);
        }

        //清空所有下注事件
        public void OnBtnEmptyBetChip()
        {

        }
        //续压事件
        public void OnBtnContinue()
        {

        }

        //规则按事件
        public void OnBtnRuleIvk()
        {
            RulesPanel.SetActive(true);
            ShowConCealLump(RulesPanel);         		
        }

        //记录按钮事件
        public void OnBtnTrendIvk()
        {
            o_notes.SetActive(true);
            ShowConCealLump(o_notes);
            Trendeffect.SetActive(false);
            showTime = 0;
        }

        //设置按钮事件
        public void OnBtnSettingIvk()
        {
            o_setting.SetActive(true);
            ShowConCealLump(o_setting);
            showTime = 0;
        }

        //游戏记录写入
        void SetResultNote(byte[] ranking, byte[] multiple, byte valuecount)
        {
            int i = 0;
            int indexcoun = 0;

            if (valuecount > 7)
            {
                i = valuecount - 7;
                indexcoun = 7;
            }
            else
            {
                i = 0;
                indexcoun = valuecount;
            }

            byte[] rankingChenge = new byte[indexcoun];
            byte[] multipleChenge = new byte[indexcoun];

            Buffer.BlockCopy(ranking, i, rankingChenge, 0, indexcoun);
            Buffer.BlockCopy(multiple, i, multipleChenge, 0, indexcoun);

            ResultNotes(rankingChenge, multipleChenge);
        }

        //游戏记录发送
        void ResultNotes(byte[] ranking, byte[] multiple)
        {
            o_notes.GetComponent<NoteControl>().DestroyChild();
            o_notes.GetComponent<NoteControl>().ShowNoteData(ranking, multiple);
        }

        //面板显示控制
        void ShowConCealLump(GameObject obj)
        {
            if (showInterface != null)
            {
                showInterface.GetComponent<UITweener>().PlayReverse();
                showInterface = null;
                conCealTween.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
                obj.GetComponent<UITweener>().enabled = true;
                obj.GetComponent<UITweener>().PlayForward();
                showInterface = obj;
                conCealTween.SetActive(true);
            }
        }

        //关闭弹出窗口
        public void ConCealTween()
        {
            //&& showInterface.name != "scene_setting"
            if (showInterface != null)
           {
               showInterface.GetComponent<UITweener>().PlayReverse();
               showInterface = null;
            }

            conCealTween.SetActive(false);
        }
        //关闭设置
        public void CloseSetting()
        {
            if (showInterface != null)
            {
                showInterface.GetComponent<UITweener>().PlayReverse();
                showInterface = null;
            }

            conCealTween.SetActive(false);
        }

        //显示区域数值
        void ShowTableScore(byte id, byte areaindex, Int64 areascore)
        {
            if (id == GetSelfChair())
            {
                s_lPlayerTableScore[areaindex] += areascore;
               
                l_PlayerAllTableScore += areascore;
                s_lTableScore[areaindex] += areascore;
                l_AllTableScore += areascore;
                player_totalMoney -= areascore;
                o_player_money.GetComponent<label_number>().m_iNum = player_totalMoney;
            }
            else
            {
                s_lTableScore[areaindex] += areascore;
                l_AllTableScore += areascore;
            }

            if (s_lTableScore[areaindex] > 0)
            {
                o_TableScore[areaindex].gameObject.SetActive(true);
                o_TableScore[areaindex].GetComponent<label_number>().m_iNum = s_lTableScore[areaindex];
            }
            else
            {
                o_TableScore[areaindex].gameObject.SetActive(false);
            }

            if (s_lPlayerTableScore[areaindex] > 0)
            {
                o_PlayerTableScore[areaindex].gameObject.SetActive(true);
                o_PlayerTableScore[areaindex].GetComponent<label_number>().m_iNum = s_lPlayerTableScore[areaindex];
            }
            else
            {
                o_PlayerTableScore[areaindex].gameObject.SetActive(false);
            }

            if (l_PlayerAllTableScore > 0)
            {
                o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            }
            o_PlayerAllTableScore.gameObject.SetActive(true);
            o_PlayerAllTableScore.GetComponent<label_number>().m_iNum = l_PlayerAllTableScore;
        }

        //下注界面马对应相对位置闪烁
        void DazzlingHorse(byte horseindex)
        {
            switch (horseindex)
            {
                case 0:
                    {
                        ShowDazzlingHorse(0, 5);
                        break;
                    }
                case 1:
                    {
                        ShowDazzlingHorse(0, 4);
                        break;
                    }
                case 2:
                    {
                        ShowDazzlingHorse(0, 3);
                        break;
                    }
                case 3:
                    {
                        ShowDazzlingHorse(0, 2);
                        break;
                    }
                case 4:
                    {
                        ShowDazzlingHorse(0, 1);
                        break;
                    }
                case 5:
                    {
                        ShowDazzlingHorse(1, 5);
                        break;
                    }
                case 6:
                    {
                        ShowDazzlingHorse(1, 4);
                        break;
                    }
                case 7:
                    {
                        ShowDazzlingHorse(1, 3);
                        break;
                    }
                case 8:
                    {
                        ShowDazzlingHorse(1, 2);
                        break;
                    }
                case 9:
                    {
                        ShowDazzlingHorse(2, 5);
                        break;
                    }
                case 10:
                    {
                        ShowDazzlingHorse(2, 4);
                        break;
                    }
                case 11:
                    {
                        ShowDazzlingHorse(2, 3);
                        break;
                    }
                case 12:
                    {
                        ShowDazzlingHorse(3, 5);
                        break;
                    }
                case 13:
                    {
                        ShowDazzlingHorse(3, 4);
                        break;
                    }
                case 14:
                    {
                        ShowDazzlingHorse(4, 5);
                        break;
                    }
            }
        }
        void ShowDazzlingHorse(byte index1, byte index2)
        {
            horse_bg[index1].gameObject.SetActive(true);
            horse_bg[index2].gameObject.SetActive(true);
            horse_bg[index1].GetComponent<DestroyControl>().time = 0;
            horse_bg[index2].GetComponent<DestroyControl>().time = 0;
        }

        //隐藏数值并初始化
        void HideTableScore()
        {
            l_PlayerAllTableScore = 0;
            o_PlayerAllTableScore.GetComponent<label_number>().m_iNum = 0;
            o_CleanBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            o_PlayerAllTableScore.gameObject.SetActive(true);
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {                
                s_lPlayerTableScore[i] = 0;
                s_lTableScore[i] = 0;
                o_PlayerTableScore[i].GetComponent<label_number>().m_iNum = 0;
                o_TableScore[i].GetComponent<label_number>().m_iNum = 0;
                o_PlayerTableScore[i].gameObject.SetActive(false);
                o_TableScore[i].gameObject.SetActive(false);
            }
        }


        #region ####################################################金币按钮控制######################################################

        //初始化按钮选中特效
        void onBtnSelect()
        {
            for (int i = 0; i < GameXY.CHIP_COUNT; i++)
            {
                chip_btn[i].transform.Find("selected").gameObject.SetActive(false);
            }
        }

        //跟新按钮状态
        void UpdateButtonContron()
        {
            for (int i = 0; i < GameXY.CHIP_COUNT; i++)
            {
                chip_btn[i].SetActive(true);
                chip_btn[i].GetComponent<UIButton>().isEnabled = true;
                chip_btn[i].transform.Find("value").GetComponent<label_number>().m_cColor = Color.white;

            }
            long lMoney = (long)player_totalMoney;

            DisableBtnIvk(lMoney);
        }

        void DisableBtnIvk(long lScore)
        {
            PlayerInfo playerdata = GameEngine.Instance.MySelf;
            if (m_ContinueBetAllScore == 0 || playerdata.Money < m_ContinueBetAllScore)
            {
                o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = false;
            }
            else
            {
                o_ContinueBetBtn.transform.GetComponent<UIButton>().isEnabled = true;
            }                

            do
            {
                if (lScore >= chip_value[4]) break;
                chip_btn[4].gameObject.GetComponent<UIButton>().isEnabled = false;
                chip_btn[4].transform.Find("value").GetComponent<label_number>().m_cColor = Color.gray;
                if (chip_btn[4].transform.FindChild("selected").gameObject.activeSelf)
                {
                    OnBtnAddChipIvk("btn_chip_3");
                }
                if (lScore >= chip_value[3]) break;
                chip_btn[3].gameObject.GetComponent<UIButton>().isEnabled = false;
                chip_btn[3].transform.Find("value").GetComponent<label_number>().m_cColor = Color.gray;
                if (chip_btn[3].transform.FindChild("selected").gameObject.activeSelf)
                {
                    OnBtnAddChipIvk("btn_chip_2");
                }
                if (lScore >= chip_value[2]) break;
                chip_btn[2].gameObject.GetComponent<UIButton>().isEnabled = false;
                chip_btn[2].transform.Find("value").GetComponent<label_number>().m_cColor = Color.gray;
                if (chip_btn[2].transform.FindChild("selected").gameObject.activeSelf)
                {
                    OnBtnAddChipIvk("btn_chip_1");
                }

                if (lScore >= chip_value[1]) break;
                chip_btn[1].gameObject.GetComponent<UIButton>().isEnabled = false;
                chip_btn[1].transform.Find("value").GetComponent<label_number>().m_cColor = Color.gray;
                if (chip_btn[1].transform.FindChild("selected").gameObject.activeSelf)
                {
                    OnBtnAddChipIvk("btn_chip_0");
                }
                if (lScore >= chip_value[0]) break;
                chip_btn[0].transform.Find("value").GetComponent<label_number>().m_cColor = Color.gray;
                chip_btn[0].gameObject.GetComponent<UIButton>().isEnabled = false;
            } while (false);
        }

        void OnBtnAddChipIvk(string objName)
        {
            string str = objName.Substring(9);
            int addChip = int.Parse(str);
            chipScore = chip_value[addChip];
            onBtnSelect();
            chip_btn[addChip].transform.FindChild("selected").gameObject.SetActive(true);
        }
        #endregion


        #region ############################################################下注控制##########################################################

        //加注消息
        void OnPlaceJetton(byte wParam, long lParam)
        {
            UpdateButtonContron();

            if (player_totalMoney < chip_value[0])
            {
                cnMsgBox("金币不足，无法下注！");
                return;
            }
            if (chipScore <= 0 && lParam == 0)
            {                
                cnMsgBox("请选择筹码！");
                return;
            }
            if (m_PlayerLimitAllSocre < l_PlayerAllTableScore + lParam)
            {
                cnMsgBox("超出下注限制，无法下注！");
                return;
            }

            #if UNITY_STANDALONE_WIN
            Debug.Log("<color = red> pc </color>");
            #elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS

			int i = 0;

            if(Input.touchCount > 1)
			{
				return;
			}

// 			foreach(var touch in Input.touches)
// 			{
// 				if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
// 				{
// 					i++;
// 				}
// 			}
// 			if(i > 1)
// 			{
// 				return;
// 			}	
            #endif

            NPacket packet = NPacketPool.GetEnablePacket(); ;
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PLAYER_BET);
            packet.AddLong(lParam);
            packet.Addbyte(wParam);
            GameEngine.Instance.Send(packet);
        }

        //押注区域的点击事件
        void OnBtnAreaIvk(string objName)
        {

            string str = objName.Substring(5);
            if (GameEngine.Instance.MySelf.GameStatus == (byte)GameXY.GS_BET)
            {
                byte area_value = GameXY.AREA_ALL;
                int area = int.Parse(str);

                switch (area)
                {
                    case 16:
                        {
                            area_value = GameXY.AREA_1_6;
                            break;
                        }
                    case 15:
                        {
                            area_value = GameXY.AREA_1_5;
                            break;
                        }
                    case 14:
                        {
                            area_value = GameXY.AREA_1_4;
                            break;
                        }
                    case 13:
                        {
                            area_value = GameXY.AREA_1_3;
                            break;
                        }
                    case 12:
                        {
                            area_value = GameXY.AREA_1_2;
                            break;
                        }
                    case 26:
                        {
                            area_value = GameXY.AREA_2_6;
                            break;
                        }
                    case 25:
                        {
                            area_value = GameXY.AREA_2_5;
                            break;
                        }
                    case 24:
                        {
                            area_value = GameXY.AREA_2_4;
                            break;
                        }
                    case 23:
                        {
                            area_value = GameXY.AREA_2_3;
                            break;
                        }
                    case 36:
                        {
                            area_value = GameXY.AREA_3_6;
                            break;
                        }
                    case 35:
                        {
                            area_value = GameXY.AREA_3_5;
                            break;
                        }
                    case 34:
                        {
                            area_value = GameXY.AREA_3_4;
                            break;
                        }
                    case 46:
                        {
                            area_value = GameXY.AREA_4_6;
                            break;
                        }
                    case 45:
                        {
                            area_value = GameXY.AREA_4_5;
                            break;
                        }
                    case 56:
                        {
                            area_value = GameXY.AREA_5_6;
                            break;
                        }
                }

                OnPlaceJetton(area_value, chipScore);
            }
        }

        //生成筹码
        void AddChipValueShow(byte wParam, long lParam)
        {
            Vector3 scorePosition = new Vector3(0, 0, 0);
            chipDownScore.GetComponent<UILabel>().text = lParam.ToString();
            GameObject scoreObj = Instantiate(chipDownScore, scorePosition, Quaternion.identity) as GameObject;
            scoreObj.transform.parent = area_btn[wParam].transform;        
            scoreObj.transform.localScale = Vector3.one;
            scoreObj.transform.localPosition = new Vector3(0, -15, 0);
        }

        #endregion

        #endregion


    }

}
