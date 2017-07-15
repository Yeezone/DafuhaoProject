using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Services.Data;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Utility;
using System.Runtime.InteropServices;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services;
using com.QH.QPGame.Lobby.Surfaces;

namespace com.QH.QPGame.xyls
{
    // 押分面板上的动物属性(倍率,押分)
    [System.Serializable]
    public class BetPanelAnimals
    {
        // 动物按钮预设
        public GameObject _red;
        public GameObject _green;
        public GameObject _yellow;
        // 
        public SingleAnimal sa_red;
        public SingleAnimal sa_green;
        public SingleAnimal sa_yellow;
    }

    // 押分面板上的动物属性(倍率,押分)
    [System.Serializable]
    public class BetPanelEnjoyGame
    {
        // 动物按钮预设
        public GameObject _enjoyGame;
        // 
        public SingleEnjoyGame seg_enjoyGame;
    }

    // 管理员设置开奖结果
    [System.Serializable]
    public class administratorSetPrize
    {
        public int prizeGameMode;
        public int prizeAnimal;
        public int prizeColor;
        public int prizeEnjoyGame;
    }

    // 按钮上的筹码值
    [System.Serializable]
    public class ChipNum_Label
    {
        public UILabel chip01;
        public UILabel chip02;
        public UILabel chip03;
    }

    public class CUIGame : MonoBehaviour
    {
        public static CUIGame _instance;
        // 当前金币(客户端存储的临时金币数量)
        [HideInInspector]
        public long m_iCurGoldNum;
        [HideInInspector]
        public long m_iCurGoldNum_temp;
        // 场景倒计时面板
        public CLabelNum m_cTimeNum;
        // 彩金面板
        public CLabelNum m_cBounsNum;

        // 押分面板上的动物按钮(属性,倍率,押分)
        public BetPanelAnimals[] _betPanelAnimals = new BetPanelAnimals[4];
        // 缓存动物倍率
        [HideInInspector]
        public int[,] m_iAnimalRatio;

        // 押分面板上的庄和闲(属性,倍率,押分)
        public BetPanelEnjoyGame[] enjoyGames = new BetPanelEnjoyGame[3];
        // 缓存庄闲和倍率
        [HideInInspector]
		public int[] m_iEnjoyGameRatio;

        // 缓存各个下注按钮上的分值(用于更新金币)
		private Int64[] m_iAllButtonBet = new Int64[15];

        // 押分面板上的筹码按钮
        public GameObject[] ChipsButtons;
        // 清除下注按钮
        public GameObject CleanButton;
        // 续押按钮
        public GameObject ContinueButton;
        // 自动按钮
        public GameObject AutoButton;

        // 色板
        public GameObject[] ColorPlate = new GameObject[24];
        // 动物
        public GameObject[] AllAnimal = new GameObject[24];
        // 黑屏文字(等待游戏界面)
        public GameObject WaitLabel;
        // 开奖结果的下标
        private int[] m_iPrizeIndex = new int[3];
        // 下注失败提示
        public GameObject m_gJettonFailTip;
        // 获得彩金提示
        public GameObject m_gPrizePoolTip;
        // 是否处于下注时间
        [HideInInspector]
        public bool m_bBetTime = false;
        // 筹码配置
        public ChipNum_Label m_cChipNum;
        // 是否播放游戏结束动画(在进入场景_下注时候,不允许播动画)
        private bool m_bPlayGameEndAnim = false;
        // 游戏提示父节点
        public Transform m_tTipsParent;

        // 管理员设置开奖结果
        public administratorSetPrize _administratorSetPrize;

        void Start()
        {
            _instance = this; 
            m_iAnimalRatio = new int[4, 3];
			m_iEnjoyGameRatio = new int[3];
        }

        void OnDestroy()
        {
            _instance = null;
        }

        public void Init()
        {
            CGameEngine.Instance.SetTableEventHandle(new TableEventHandle(OnTableUserEvent));
            CGameEngine.Instance.AddPacketHandle2(MainCmd.MDM_GF_FRAME, new PacketHandle2(OnFrameResp));
            CGameEngine.Instance.AddPacketHandle2(MainCmd.MDM_GF_GAME, new PacketHandle2(OnGameResp));

            if (!CGameEngine.Instance.AutoSit || CGameEngine.Instance.IsPlaying())
            {
                CGameEngine.Instance.SendUserSetting();
            }
            else
            {
                CGameEngine.Instance.SendUserSitdown();
            }
        }

        void Update()
        {
            //测试
            //             if (Input.GetKeyDown(KeyCode.J))
            //             {
            //                 AdministratorSetPrize();
            //             }
        }

        //框架事件入口
        private void OnTableUserEvent(TableEvents tevt, uint userid, object data)
        {

            if (CUIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (tevt)
            {
                //用户进来
                case TableEvents.USER_COME:
                    {
                        PlayerInfo userdata = CGameEngine.Instance.GetTableUserItem((byte)CGameEngine.Instance.MySelf.DeskStation);
                        if (userid != userdata.ID)
                        {
                            int temp_iCahirID = CGameEngine.Instance.UserIdToChairId(userid);
                            PlayerInfo userInfo = CGameEngine.Instance.GetTableUserItem((ushort)temp_iCahirID);
                            if (userInfo != null)
                            {
                                int temp_iFaceID = (int)userInfo.HeadID;
								CHeadManger._instance.ShowPlayerHead(temp_iCahirID, temp_iFaceID, userInfo.NickName);
							}
						}
                        break;
                    }
                case TableEvents.USER_LEAVE:
                    {
                        for (int i = 0; i < CUIManger.Instance.m_iGamePlayer; i++)
                        {
                            if (CGameEngine.Instance.GetTableUserItem((ushort)i) == null)
                            {
                                CHeadManger._instance.PlayerExit(i);
                            }
                        }
                        break;
                    }
                case TableEvents.GAME_ENTER:
                    {
                        CHeadManger._instance.SetAllFalse();
                        for (int i = 0; i < CUIManger.Instance.m_iGamePlayer; i++)
                        {
                            PlayerInfo playerdata = CGameEngine.Instance.GetTableUserItem((ushort)i);
                            if (playerdata == null) continue;
                            CHeadManger._instance.ShowPlayerHead(i, (int)playerdata.HeadID, playerdata.NickName);
                        }
                        break;
                    }
                case TableEvents.GAME_START:
                    {
                        CHeadManger._instance.SetAllFalse();
                        for (int i = 0; i < CUIManger.Instance.m_iGamePlayer; i++)
                        {
                            PlayerInfo playerdata = CGameEngine.Instance.GetTableUserItem((ushort)i);
                            if (playerdata == null) continue;
                            CHeadManger._instance.ShowPlayerHead(i, (int)playerdata.HeadID, playerdata.NickName);
                        }
                        break;
                    }
            }
        }

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, Packet packet)
        {
            if (CUIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (subcmd)
            {
                // 设置游戏状态
                case SubCmd.SUB_GF_OPTION:
                    {
                        /*packet.BeginRead();
                        CGameEngine.Instance.MySelf.GameStatus = packet.GetByte();*/
                        CGameEngine.Instance.MySelf.GameStatus = packet.Data[0];
                        break;
                    }
                // 游戏场景消息
                case SubCmd.SUB_GF_SCENE:
                    {
                        OnGameSceneResp(CGameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
            }
        }

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, Packet packet)
        {

            if (CUIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;

            //if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {
                case (byte)SubCmd.SUB_S_SEND_GAMERECORD:
                    {
                        Debug.Log("<color=red>>==============&游戏记录==============</color>");
                        GameRecord(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_GAME_FREE:
                    {
                        Debug.Log("<color=red>>==============&重开游戏空闲==============</color>");
                        //GameFree(packet);
                        StartCoroutine(GameFree(packet));
                        break;
                    }
                case (byte)SubCmd.SUB_S_GAME_START:
                    {
                        Debug.Log("<color=red>==============游戏开始>==============</color>");
                        GameStart(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_PLACE_JETTON:
                    {
                        Debug.Log("<color=red>>==============玩家下注>==============</color>");
                        update_JETTON(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_PLACE_JETTON_FAIL:
                    {
                        Debug.Log("<color=red>>==============下注失败>==============</color>");
                        update_JETTON_fail(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_GAME_END:
                    {
                        Debug.Log("<color=red>>==============游戏结束>==============</color>");
                        GameOver(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_CREATE_TRUN:
                    {
                        Debug.Log("<color=red>>==============生成转盘>==============</color>");
                        CreatTrun(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_PRIZE_COLOR:
                    {
                        Debug.Log("<color=red>>==============开奖颜色==============</color>");
                        PrizeColor(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_PLAYER_CONTINUE_BET:
                    {
                        Debug.Log("<color=red>>==============玩家续押==============</color>");
                        ContinueJetton(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_CLEAR_JETTON:
                    {
                        Debug.Log("<color=red>>==============清除下注==============</color>");
                        update_JETTON_clean(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_PLACE_CONTINUE_FAIL:
                    {
                        Debug.Log("<color=red>>==============续押失败==============</color>");
                        ContinueJettonFail(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_SEND_PRIZE_REWARD:
                    {
                        Debug.Log("<color=red>>==============中彩金==============</color>");
                        ShowPrizePool(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_SEND_PRIZE_DATA:
                    {
                        Debug.Log("<color=red>>==============更新彩金==============</color>");
                        UpdatePrizePool(packet);
                        break;
                    }
                case (byte)SubCmd.SUB_S_USEROFFLINE:
                    {
                        Debug.Log("<color=red>>==============断线重连==============</color>");
                        //UserOffLine(packet);
                        break;
                    }
            }
        }

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, Packet packet)
        {
            //Debuger.LogError(bGameStatus);
            switch (bGameStatus)
            {
                case (byte)SubCmd.GAME_STATUS_FREE:
                    {
                        Debug.Log("<color=red>>==============进入游戏场景_空闲>==============</color>");
                        GameStatus_GameFree(packet);
                        break;
                    }
                case (byte)SubCmd.GS_PLACE_JETTON:
                    {
                        Debug.Log("<color=red>>==============进入游戏场景_下注中>==============</color>");
                        GameStatus_GamePlay(packet);
                        break;
                    }
                case (byte)SubCmd.GS_GAME_END:
                    {
                        Debug.Log("<color=red>>==============进入游戏场景_结束.播放动画>==============</color>");
                        GameStatus_GameEnd(packet);
                        break;
                    }
            }
        }

        /// <summary>
        /// 游戏记录
        /// </summary>
        void GameRecord(Packet packet)
        {
            // 对接消息
            CMD_S_GameRecord _mdata = GameConvert.ByteToStruct<CMD_S_GameRecord>(packet.Data);
            for (int i = 0; i < _mdata.arrGamePrizeRecord.Length; i++)
            {
                RecordManager._instance.AddRecord(_mdata.arrGamePrizeRecord[i]);
            }
            RecordManager._instance.Right_Onclick();
        }

        /// <summary>
        /// 进入场景_空闲
        /// </summary>
        void GameStatus_GameFree(Packet packet)
        {
            // 打开黑屏等待画面
            WaitLabel.SetActive(false);
            // 对接消息
            CMD_S_StatusFree _mdata = GameConvert.ByteToStruct<CMD_S_StatusFree>(packet.Data);

            // 配置筹码
            CButtonOnClick._instance.m_cChipNum.chip01 = _mdata.iJetton1;
            CButtonOnClick._instance.m_cChipNum.chip02 = _mdata.iJetton2;
            CButtonOnClick._instance.m_cChipNum.chip03 = _mdata.iJetton3;
            // 显示筹码
            m_cChipNum.chip01.text = ChangeNum2String(_mdata.iJetton1);
            m_cChipNum.chip02.text = ChangeNum2String(_mdata.iJetton2);
            m_cChipNum.chip03.text = ChangeNum2String(_mdata.iJetton3);
//             m_cChipNum.chip01.text = _mdata.iJetton1.ToString();
//             m_cChipNum.chip02.text = _mdata.iJetton2.ToString();
//             m_cChipNum.chip03.text = _mdata.iJetton3.ToString(); 
            
            // 设置初始筹码
            GameEvent._instance.m_iCurChipNum = _mdata.iJetton1;

            // 设置金币
            GameEvent._instance.SetGoldNum(_mdata.iUserScore);
            // 客户端缓存金币数量
            m_iCurGoldNum = _mdata.iUserScore;
            m_iCurGoldNum_temp = _mdata.iUserScore;

            // 设置剩余时间
            //m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave,null);
            //m_cTimeNum.m_bIsOpen = true;

            // 初始化 倍率/押分 都为0
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum(0);

                _betPanelAnimals[animal].sa_red.SetBetNum(0);
                _betPanelAnimals[animal].sa_green.SetBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetNum(0);

                _betPanelAnimals[animal].sa_red.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_green.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetTotalBetNum(0);
                // 关闭动物按钮
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, false);
            }
            // 初始化庄闲和 倍率/押分 都为0
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetDoubelNum(0);
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum(0);
                enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum(0);
                // 关闭庄和闲按钮
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, false);
            }

            // 关闭取消/续押/自动按钮
            CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            CButtonOnClick._instance.SetButtonBt(ContinueButton, false);
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

            // 播放动物动画
            SceneAnimCtr._instance.AnimalAnim_Idle();

            // 清空按钮上的下注积分
            Array.Clear(m_iAllButtonBet, 0, m_iAllButtonBet.Length);
        }

        /// <summary>
        /// 进入场景_游戏下注中
        /// </summary>
        void GameStatus_GamePlay(Packet packet)
        {
            // 关闭黑屏等待画面
            WaitLabel.SetActive(false);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(1);
            // 播放动物动画
            SceneAnimCtr._instance.AnimalAnim_Idle();

            // 对接消息
            CMD_S_StatusJetton _mdata = GameConvert.ByteToStruct<CMD_S_StatusJetton>(packet.Data);

            // 配置筹码
            CButtonOnClick._instance.m_cChipNum.chip01 = _mdata.iJetton1;
            CButtonOnClick._instance.m_cChipNum.chip02 = _mdata.iJetton2;
            CButtonOnClick._instance.m_cChipNum.chip03 = _mdata.iJetton3;
            // 显示筹码
            m_cChipNum.chip01.text = ChangeNum2String(_mdata.iJetton1);
            m_cChipNum.chip02.text = ChangeNum2String(_mdata.iJetton2);
            m_cChipNum.chip03.text = ChangeNum2String(_mdata.iJetton3);
            // 设置初始筹码
            GameEvent._instance.m_iCurChipNum = _mdata.iJetton1;
                       
            // 设置剩余时间
            m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave, null);
            m_cTimeNum.m_bIsOpen = true;
            // 把时间面板移动出来
            StartCoroutine(GameEvent._instance.MoveTimerAnim((float)_mdata.cbTimeLeave));

            // 缓存玩家金币(未计算前)
			m_iCurGoldNum = _mdata.iUserScore;
            m_iCurGoldNum_temp = _mdata.iUserScore;

            // 设置倍率,下注,总注
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 2]);

                _betPanelAnimals[animal].sa_red.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 2]);

                _betPanelAnimals[animal].sa_red.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 2]);
                // 开启动物按钮
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, true);

                m_iAnimalRatio[animal, 0] = (int)_mdata.dwMul[(3 * animal) + 0];
                m_iAnimalRatio[animal, 1] = (int)_mdata.dwMul[(3 * animal) + 1];
                m_iAnimalRatio[animal, 2] = (int)_mdata.dwMul[(3 * animal) + 2];

                // 计算金币
                for (int color = 0; color < 3; color++)
                {
					m_iCurGoldNum -= _mdata.iJettonScore[(3 * animal) + color];
                }
            }

            // 庄闲和倍率/押分初始为0
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetDoubelNum(_mdata.dwMul[12+enjoyGame]);
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum((int)_mdata.iJettonScore[12 + enjoyGame]);
                enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum((int)_mdata.iAllUserJettonScore[12 + enjoyGame]);
                // 开启庄和闲按钮
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, true);

                m_iEnjoyGameRatio[enjoyGame] = (int)_mdata.dwMul[12 + enjoyGame];

				m_iCurGoldNum -= _mdata.iJettonScore[12 + enjoyGame];
            }
			
			// 设置金币
			GameEvent._instance.SetGoldNum(m_iCurGoldNum);
            CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            // 检测当前是否有下注,如果没下注,不开启取消按钮
            for (int i = 0; i < _mdata.iJettonScore.Length; i++)
            {
                if (_mdata.iJettonScore[i] > 0)
                {
                    CButtonOnClick._instance.SetButtonBt(CleanButton, true);
                    break;
                }
            }
            // 设置续押按钮
            if (_mdata.ContinueBetState == 1)
            {
                CButtonOnClick._instance.SetButtonBt(ContinueButton, true);
            }
            else
            {
                CButtonOnClick._instance.SetButtonBt(ContinueButton, false);
            }
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

            // 如果押分面板没打开,则自动打开
            if (!CButtonOnClick._instance.m_bBetPanelIsOpen)
            {
                CButtonOnClick._instance.BetButtonOnClick();
            }
        }

        /// <summary>
        /// 进入场景_游戏结束播放动画
        /// </summary>
        void GameStatus_GameEnd(Packet packet)
        {
            // 打开黑屏等待画面
            WaitLabel.SetActive(true);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(1);
            // 停止动物动画
            SceneAnimCtr._instance.AnimalAnim_StopIdle();

            // 对接消息
            CMD_S_StatusPlay _mdata = GameConvert.ByteToStruct<CMD_S_StatusPlay>(packet.Data);

            // 配置筹码
            CButtonOnClick._instance.m_cChipNum.chip01 = _mdata.iJetton1;
            CButtonOnClick._instance.m_cChipNum.chip02 = _mdata.iJetton2;
            CButtonOnClick._instance.m_cChipNum.chip03 = _mdata.iJetton3;
            // 显示筹码
            m_cChipNum.chip01.text = ChangeNum2String(_mdata.iJetton1);
            m_cChipNum.chip02.text = ChangeNum2String(_mdata.iJetton2);
            m_cChipNum.chip03.text = ChangeNum2String(_mdata.iJetton3);
            // 设置初始筹码
            GameEvent._instance.m_iCurChipNum = _mdata.iJetton1;

            // 设置金币
            GameEvent._instance.SetGoldNum(_mdata.iUserScore);
            // 设置剩余时间
            //m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave, null);
            //m_cTimeNum.m_bIsOpen = true;

            // 游戏状态
            byte gameStatus = _mdata.cbGameStatus;

            // 动物倍率/押分初始为0
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum(0);

                _betPanelAnimals[animal].sa_red.SetBetNum(0);
                _betPanelAnimals[animal].sa_green.SetBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetNum(0);

                _betPanelAnimals[animal].sa_red.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_green.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetTotalBetNum(0);
                // 关闭动物按钮
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, false);
            }
            // 庄闲和倍率/押分初始为0
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetDoubelNum(0);
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum(0);
                enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum(0);
                // 关闭庄和闲按钮
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, false);
            }
            // 关闭取消/续押/自动按钮
            CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            CButtonOnClick._instance.SetButtonBt(ContinueButton, false);
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

            // 清空按钮上的下注积分
            Array.Clear(m_iAllButtonBet, 0, m_iAllButtonBet.Length);
        }
               
        /// <summary>
        /// 游戏状态_游戏空闲
        /// </summary>
        IEnumerator GameFree(Packet packet)
        {
            // 是否播放游戏结束动画(重开游戏就设置为false)
            m_bPlayGameEndAnim = false;
            // 对接消息
            CMD_S_GameFree _mdata = GameConvert.ByteToStruct<CMD_S_GameFree>(packet.Data);

            // 设置金币
            GameEvent._instance.SetGoldNum(_mdata.iUserScore);
			// 客户端缓存金币数量
			m_iCurGoldNum = _mdata.iUserScore;
			m_iCurGoldNum_temp = _mdata.iUserScore;
            // 设置剩余时间
            //m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave, null);
            //m_cTimeNum.m_bIsOpen = true;

            // 关闭动物按钮
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum(0);

                _betPanelAnimals[animal].sa_red.SetBetNum(0);
                _betPanelAnimals[animal].sa_green.SetBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetBetNum(0);

                _betPanelAnimals[animal].sa_red.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_green.SetTotalBetNum(0);
                _betPanelAnimals[animal].sa_yellow.SetTotalBetNum(0);

                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, false);
            }
            // 关闭庄和闲按钮
            for (int enjoyGame = 0; enjoyGame < enjoyGames.Length; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum(0);
                enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum(0);
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, false);
            }
            // 关闭取消/续押/自动按钮
            //CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            //CButtonOnClick._instance.SetButtonBt(ContinueButton, false);
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

            // 以下接口暂未用到.
            //             //当前是游戏启动以来的第几局
            //             Int64 qwGameTimes = _mdata.qwGameTimes;
            //             public CMD_BANKER_INFO stBankerInfo;						//庄家信息
            //             public byte cbCanCancelBank;					//是否可以申请下庄（0： 不能下庄，1：能下庄）

            // 清空按钮上的下注积分
            Array.Clear(m_iAllButtonBet, 0, m_iAllButtonBet.Length);

			// 每局空闲,检测玩家金币是否不足,如果不足,显示tip并且自动退出游戏.
            if ((int)_mdata.iUserScore < 100) 
            {
                var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
                var tips = container.GetSurface<SurfaceTips>();
                tips.Show("您的金币不足,10秒后自动退出!", 10.0f, delegate()
                {
                    CGameEngine.Instance.Quit();
                });
            }

            yield return new WaitForSeconds(2f);

            // 重置游戏_特殊开奖模型(隐藏)
            SingleSpecialPrize._instance.ResetGame_SpecialPrize();
            // 播放动物动画
            SceneAnimCtr._instance.AnimalAnim_Idle();           
        }

        /// <summary>
        /// 游戏状态_开始游戏
        /// </summary>
        void GameStart(Packet packet)
        {
            // 游戏开始,下注时间打开
            m_bBetTime = true;
            // 关闭黑屏等待画面
            WaitLabel.SetActive(false);
            AudioBgCtr._instance.PlayBGM(0);

            // 对接消息
            CMD_S_GameStart _mdata = GameConvert.ByteToStruct<CMD_S_GameStart>(packet.Data);

//            // 设置金币
//            GameEvent._instance.SetGoldNum((int)_mdata.iUserScore);
//            // 客户端缓存金币数量
//            m_iCurGoldNum = (int)_mdata.iUserScore;
//            m_iCurGoldNum_temp = (int)_mdata.iUserScore;

            // 设置剩余时间
            m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave-1, null);
            m_cTimeNum.m_bIsOpen = true;

            // 设置动物倍率
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum((int)_mdata.dwMul[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum((int)_mdata.dwMul[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum((int)_mdata.dwMul[(3 * animal) + 2]);
                                
                // 缓存动物倍率
                m_iAnimalRatio[animal, 0] = (int)_mdata.dwMul[(3 * animal) + 0];
                m_iAnimalRatio[animal, 1] = (int)_mdata.dwMul[(3 * animal) + 1];
                m_iAnimalRatio[animal, 2] = (int)_mdata.dwMul[(3 * animal) + 2];

                // 设置动物按钮为开启状态
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, true);
            }
            // 设置庄闲和倍率
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetDoubelNum((int)_mdata.arrSTEnjoyGameAtt[enjoyGame].dwMul);
                
                m_iEnjoyGameRatio[enjoyGame] = (int)_mdata.arrSTEnjoyGameAtt[enjoyGame].dwMul;
                // 初始化庄和闲按钮,设置为开启状态
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, true);
            }
            // 开启取消/续押/自动按钮
            //CButtonOnClick._instance.SetButtonBt(CleanButton, true);
            if (_mdata.ContinueBetState == 1)
            {
                CButtonOnClick._instance.SetButtonBt(ContinueButton, true);
            }
            //CButtonOnClick._instance.SetButtonBt(AutoButton, true);

            // 开始游戏,如果押分面板没打开,则自动打开
            if (!CButtonOnClick._instance.m_bBetPanelIsOpen)
            {
                CButtonOnClick._instance.BetButtonOnClick();
            }
            // 把时间面板移动出来(暂不使用)
            StartCoroutine(GameEvent._instance.MoveTimerAnim((float)_mdata.cbTimeLeave));
        }

        /// <summary>
        /// 接服务器返回加注信息,显示信息
        /// </summary>
        void update_JETTON(Packet packet)
        {
            // 对接消息
            CMD_S_PlaceJetton _mdata = GameConvert.ByteToStruct<CMD_S_PlaceJetton>(packet.Data);

            // 检验是否自己加注.如果是:更新下注和总注.如果不是:更新总注.
            if (CGameEngine.Instance.MySelf.DeskStation == _mdata.wChairID)
            {
                // 开启取消押注按钮
                CButtonOnClick._instance.SetButtonBt(CleanButton, true);
                // 更新下注
                if (_mdata.eGamble == 0)
                {
                    // 设置押分面板_动物的分数
                    int animal = (int)_mdata.stAnimalInfo.eAnimal;
                    if (_mdata.stAnimalInfo.eColor == 0)
                    {
                        _betPanelAnimals[animal].sa_red.SetBetNum((int)_mdata.iPlaceJettonScore);
                    }
                    else if (_mdata.stAnimalInfo.eColor == 1)
                    {
						_betPanelAnimals[animal].sa_green.SetBetNum((int)_mdata.iPlaceJettonScore);
                    }
                    else if (_mdata.stAnimalInfo.eColor == 2)
                    {
						_betPanelAnimals[animal].sa_yellow.SetBetNum((int)_mdata.iPlaceJettonScore);
                    }
                }
                else if (_mdata.eGamble == 1)
                {
                    // 设置押分面板_庄闲和的分数
                    int enjoyGame = (int)_mdata.eEnjoyGameInfo;
					enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum((int)_mdata.iPlaceJettonScore);
                }
                m_iCurGoldNum = m_iCurGoldNum_temp;

                for (int i = 0; i < m_iAllButtonBet.Length; i++)
                {
                    m_iAllButtonBet[i] = _mdata.iUserAllJetton[i];
                    m_iCurGoldNum -= m_iAllButtonBet[i];
                }
                GameEvent._instance.SetGoldNum(m_iCurGoldNum);       
            }

            // 更新总注
            if (_mdata.eGamble == 0)
            {
                int animal = (int)_mdata.stAnimalInfo.eAnimal;
                if (_mdata.stAnimalInfo.eColor == 0)
                {
					_betPanelAnimals[animal]._red.GetComponent<SingleAnimal>().SetTotalBetNum((int)_mdata.iTotalPlayerJetton);
                }
                else if (_mdata.stAnimalInfo.eColor == 1)
                {
					_betPanelAnimals[animal]._green.GetComponent<SingleAnimal>().SetTotalBetNum((int)_mdata.iTotalPlayerJetton);
                }
                else if (_mdata.stAnimalInfo.eColor == 2)
                {
					_betPanelAnimals[animal]._yellow.GetComponent<SingleAnimal>().SetTotalBetNum((int)_mdata.iTotalPlayerJetton);
                }
            }
            else if (_mdata.eGamble == 1)
            {
                int enjoyGame = (int)_mdata.eEnjoyGameInfo;
				enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum((int)_mdata.iTotalPlayerJetton);
            }
        }

        /// <summary>
        /// 下注失败
        /// </summary>
        void update_JETTON_fail(Packet packet)
        {
            // 对接消息
            CMD_S_PlaceJettonFail _mdata = GameConvert.ByteToStruct<CMD_S_PlaceJettonFail>(packet.Data);

            string animal = eAnimalType_change(_mdata.stAnimalInfo.eAnimal);
            string color = eColorType_change(_mdata.stAnimalInfo.eColor);
            string banker = eEnjoyGameType_change(_mdata.eEnjoyGameInfo);

			if (_mdata.dwErrorCode == 0)
			{
				return;
			}
			else if (_mdata.dwErrorCode == 1)
			{
                m_gJettonFailTip.GetComponent<UILabel>().text = "您的金币不足,不能下注!";
			}
           else if (_mdata.dwErrorCode == 2)
            {
                if (_mdata.eGamble == 0)
                {
                    m_gJettonFailTip.GetComponent<UILabel>().text = "【" + animal + "】已经被您押爆!不能继续下注!";
                }
                else
                {
                    m_gJettonFailTip.GetComponent<UILabel>().text = "【" + banker + "】已经被您押爆!不能继续下注!";
                }
           }
           else if (_mdata.dwErrorCode == 3)
           {
               m_gJettonFailTip.GetComponent<UILabel>().text = "请选择筹码!";
           }
           else if (_mdata.dwErrorCode == 4)
           {
               m_gJettonFailTip.GetComponent<UILabel>().text = "所有动物已经被您押爆!不能继续下注!";
           }
           else if (_mdata.dwErrorCode == 5)
           {
               m_gJettonFailTip.GetComponent<UILabel>().text = "【庄闲和】已经被您押爆!不能继续下注!";
           }
           else
           {
               m_gJettonFailTip.GetComponent<UILabel>().text = "下注失败!";
           }
           if (!m_bBetTime) return;
           GameObject temp = (GameObject)Instantiate(m_gJettonFailTip, Vector3.zero, Quaternion.identity);
           temp.transform.parent = m_tTipsParent;
           temp.transform.localScale = Vector3.one;
           temp.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 接收服务器_清除下注
        /// </summary>
        void update_JETTON_clean(Packet packet)
        {
            // 对接消息
            CMD_S_ClearJetton _mdata = GameConvert.ByteToStruct<CMD_S_ClearJetton>(packet.Data);

            if (_mdata.dwErrorCode == 0)
            {
                for (int animal = 0; animal < 4; animal++)
                {
                    // 检验是否自己加注.如果是:更新下注和总注.如果不是:更新总注.
                    if (CGameEngine.Instance.MySelf.DeskStation == _mdata.wChairID)
                    {
                        _betPanelAnimals[animal].sa_red.SetBetNum(0);
                        _betPanelAnimals[animal].sa_green.SetBetNum(0);
                        _betPanelAnimals[animal].sa_yellow.SetBetNum(0);
                    }

					_betPanelAnimals[animal].sa_red.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 0]);
					_betPanelAnimals[animal].sa_green.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 1]);
					_betPanelAnimals[animal].sa_yellow.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 2]);
                }
                for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
                {
                    // 检验是否自己加注.如果是:更新下注和总注.如果不是:更新总注.
                    if (CGameEngine.Instance.MySelf.DeskStation == _mdata.wChairID)
                    {
                        enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum(0);
                    }
					enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[12 + enjoyGame]);
                }
            }
        }

        /// <summary>
        /// 游戏状态_游戏结束
        /// </summary>
        void GameOver(Packet packet)
        {
            // 是否播放游戏结束动画(在进入场景_下注时候,不允许播动画)
            if (m_bPlayGameEndAnim) { return; }
            // 游戏结束,下注时间关闭
            m_bBetTime = false;
            // 关闭倒计时面板
            GameEvent._instance.m_gTimerPanel.SetActive(false);
			// 开始变化彩金
			RandomPrizeNum._instance.m_bIsOpen = true;

            // 播放音效
            AudioBgCtr._instance.PlayBGM(1);
            // 停止动物动画
            SceneAnimCtr._instance.AnimalAnim_StopIdle();

            // 对接消息
            CMD_S_GameEnd _mdata = GameConvert.ByteToStruct<CMD_S_GameEnd>(packet.Data);

            // 设置剩余时间
            //m_cTimeNum.SetGameTimer((int)_mdata.dwTimeLeave, null);
            //m_cTimeNum.m_bIsOpen = true;
            // 剩余时间
            int time = (int)_mdata.dwTimeLeave;
            Debug.Log("<color=green>剩余时间:</color>" + time);

            // 开奖动物
            //int eAnimal = _mdata.stWinAnimal.stAnimalInfo.eAnimal;
            //int eColor = _mdata.stWinAnimal.stAnimalInfo.eColor;
            // 开奖动物模式
            int ePrizeMode = _mdata.stWinAnimal.ePrizeMode;
            // 开奖庄闲和
            int eEnjoyGameType = _mdata.stWinEnjoyGameType.ePrizeGameType;
            // 玩家成绩
            int iUserScore = (int)_mdata.iUserScore;

            /*
            当prizemode=eAnimalPrizeMode_SysPrize时，qwFlag表示开出来的系统彩金，
            当prizemode=eAnimalPrizeMode_RepeatTime时，qwFlag表示重复次数
            当prizemode=eAnimalPrizeMode_Flash时，qwFlag表示系统倍率
            */
            int qwFlag = (int)_mdata.stWinAnimal.qwFlag;

            float[] arrXisY = new float[2];
            int[] arrAnimal = new int[2];
            int[] arrColor = new int[2];

            // 获取到开奖结果的位置.
            arrXisY[0] = m_iPrizeIndex[0] * 15;
            arrAnimal[0] = _mdata.stWinAnimal.stAnimalInfo.eAnimal;
            arrColor[0] = _mdata.stWinAnimal.stAnimalInfo.eColor;

            if (ePrizeMode == 0)
            {
                ///////////////单颜色单动物///////////////
                // 场景动画
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex);

                // 正常模式:移动动物和播放Win动画(旋转动物包括移动相机)
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SimplePrizeAnim(AllAnimal, arrXisY[0], arrColor[0], arrAnimal[0], ePrizeMode));

                // 设置 && 显示结算面板数值(30s:跟服务器每局转动时间一致)            
                GameEvent._instance.SetAccountPanel_Simple(arrAnimal[0], arrColor[0], eEnjoyGameType, iUserScore);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_simple(30));
                // 更新开奖记录(延时31s:比服务器时间晚1s更新记录)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(31, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效(26s:动物停止时间22s+1s变大+1s上移+1s中移+1s延迟)
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(26, arrColor[0]));
            }
            else if (ePrizeMode == 1)
            {
                ///////////////单颜色///////////////
                // 特殊开奖动画
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SpcialPrize_SingleColor(arrColor[0]));

                // 单颜色:只闪烁水晶
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SingleColorAnim(arrColor[0]));

                // 场景动画
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex);
                // 设置 && 显示结算面板数值            
                GameEvent._instance.SetAccountPanel_SingleColor(arrColor[0], eEnjoyGameType, iUserScore);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_singleColor(30));
                // 更新开奖记录(延时24s)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(31, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(24, arrColor[0]));
            }
            else if (ePrizeMode == 2)
            {
                ///////////////单动物///////////////
                // 特殊开奖动画
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SpecialPrize_SingleAnimal(arrAnimal[0]));

                // 单动物:只闪烁水晶
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SingleAnimalAnim(arrColor[0], arrAnimal[0]));

                // 场景动画
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex);
                // 设置 && 显示结算面板数值            
                GameEvent._instance.SetAccountPanel_SingleAnimal(arrAnimal[0], eEnjoyGameType, iUserScore);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_singleAnimal(30));
                // 更新开奖记录(延时24s)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(31, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(24, arrColor[0]));
            }
            else if (ePrizeMode == 3)
            {
                ///////////////彩金///////////////
                ModelAnimation._instance.SpcialPrize_SysPrize();
                //Debug.Log("<color=green>彩金:</color>" + qwFlag);

//                     // 系统彩金:
//                     // 移动动物和播放Win动画(旋转动物包括移动相机)
//                     ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SysPrizeAnim(_animal, axisY, color, animal, prizeMode));

                // 旋转指针/动物/庄闲和 到正确的开奖结果
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex);
                // 设置 && 显示结算面板数值            
                GameEvent._instance.SetAccountPanel_SysPrize(arrAnimal[0], arrColor[0], eEnjoyGameType, iUserScore, qwFlag);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_sysPrize(23));
                // 更新开奖记录(延时24s)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(24, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(19, arrColor[0]));
            }
            else if (ePrizeMode == 4)
            {
                // 场景动画
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex);

                // 重复开奖的另外一只动物
                int _eAnimal = _mdata.stWinAnimal.arrstRepeatModePrize[0].eAnimal;
                int _eColor = _mdata.stWinAnimal.arrstRepeatModePrize[0].eColor;
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.RotModel_repeat(_eAnimal, m_iPrizeIndex[1]));
                //Debug.Log("<color=green>重复开奖:</color>" + _eAnimal + _eColor);
                // 重复开奖模式:移动动物和播放Win动画(旋转动物包括移动相机)
                for (int i = 0; i < 2; i++)
                {
                    arrXisY[i] = (float)(m_iPrizeIndex[i] * 15);                    
                }
                arrAnimal[1] = _eAnimal;
                arrColor[1] = _eColor;
                ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.RepeatPrizeAnim(AllAnimal, arrXisY, arrColor, arrAnimal));

                // 设置 && 显示结算面板数值            
                GameEvent._instance.SetAccountPanel_Repeat(arrAnimal, arrColor, eEnjoyGameType, iUserScore);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_repeat(52));
                // 更新开奖记录(延时30s)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(53, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效
                //GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(16, eColor));
            }
            else if (ePrizeMode == 5)
            {
                Debug.Log("<color=green>闪电</color>" + _mdata.dwTimeLeave);

                // 场景动画
                GameEvent._instance.ShowAllAnimation(arrAnimal[0], eEnjoyGameType, m_iPrizeIndex); 

                // 正常模式:移动动物和播放Win动画(旋转动物包括移动相机)
                //ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SimplePrizeAnim(AllAnimal, axisY, eColor, eAnimal, ePrizeMode));

                // 设置 && 显示结算面板数值            
                GameEvent._instance.SetAccountPanel_Simple(arrAnimal[0], arrColor[0], eEnjoyGameType, iUserScore);
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowAccountPanel_simple(23));
                // 更新开奖记录(延时24s)
                GameEvent._instance.StartCoroutine(GameEvent._instance.UpdatePrizeRecord(24, arrAnimal, arrColor, eEnjoyGameType, ePrizeMode));
                // 显示开奖粒子特效
                GameEvent._instance.StartCoroutine(GameEvent._instance.ShowEffect(19, arrColor[0]));
            }
            else
            {
                Debug.LogError("未知开奖模式(单颜色单动物,单颜色,单动物,彩金)");
            }
            
            // 关闭动物按钮
            for (int animal = 0; animal < 4; animal++)
            {
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, false);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, false);
            }
            // 关闭庄和闲按钮
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, false);
            }
            // 关闭取消/续押/自动按钮
            CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            CButtonOnClick._instance.SetButtonBt(ContinueButton, false);
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

            // 游戏结束(押分结束),如果押分面板没关闭,则自动关闭
            if (CButtonOnClick._instance.m_bBetPanelIsOpen)
            {
                CButtonOnClick._instance.BetButtonOnClick();
            }
        }

        /// <summary>
        /// 生成转盘
        /// </summary>
        void CreatTrun(Packet packet)
        {
            // 对接消息
            CMD_S_CreateTrun _mdata = GameConvert.ByteToStruct<CMD_S_CreateTrun>(packet.Data);

            // 根据服务器发送过来的转盘颜色配置转盘
            GameEvent._instance.CreateTrun(_mdata.TrunColor);
        }

        /// <summary>
        /// 开奖颜色
        /// </summary>
        void PrizeColor(Packet packet)
        {
            // 对接消息
            CMD_S_PrizeColor _mdata = GameConvert.ByteToStruct<CMD_S_PrizeColor>(packet.Data);

            // 服务器发送:开奖颜色对应的下标
            for (int i = 0; i < 3; i++)
            {
                m_iPrizeIndex[i] = _mdata.PrizeColorIndex[i];
            }
        }

        /// <summary>
        /// 续押事件
        /// </summary>
        void ContinueJetton(Packet packet)
        {
            // 对接消息
            CMD_S_ContinueJettons _mdata = GameConvert.ByteToStruct<CMD_S_ContinueJettons>(packet.Data);
                
            // 检验是否自己加注.如果是:更新下注和总注.如果不是:更新总注.
            if (CGameEngine.Instance.MySelf.DeskStation == _mdata.wChairID)
            {
                // 开启取消押注按钮
                CButtonOnClick._instance.SetButtonBt(CleanButton, true);
                // 每次续押之前,都把当局缓存的金币数用于减少
                m_iCurGoldNum = m_iCurGoldNum_temp;
                // 更新下注
                for (int animal = 0; animal < 4; animal++)
                {
					_betPanelAnimals[animal].sa_red.SetBetNum((int)_mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[0]);
					_betPanelAnimals[animal].sa_green.SetBetNum((int)_mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[1]);
					_betPanelAnimals[animal].sa_yellow.SetBetNum((int)_mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[2]);

                    // 金币减少
                    m_iCurGoldNum -= _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[0];
                    m_iCurGoldNum -= _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[1];
                    m_iCurGoldNum -= _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[2];
                    // 缓存续押总下注
                    m_iAllButtonBet[(3 * animal) + 0] = _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[0];
                    m_iAllButtonBet[(3 * animal) + 1] = _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[1];
                    m_iAllButtonBet[(3 * animal) + 2] = _mdata.iAnimalJettonNum[animal].iAnimalJettonNum_temp[2];
                }
                for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
                {
					enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum((int)_mdata.iEnjoyGameJettonNum[enjoyGame]);
                    // 金币减少
                    m_iCurGoldNum -= (int)_mdata.iEnjoyGameJettonNum[enjoyGame];
                    // 缓存续押总下注
                    m_iAllButtonBet[12 + enjoyGame] = (int)_mdata.iEnjoyGameJettonNum[enjoyGame];
                }
            }

            // 更新总注
            for (int animal = 0; animal < 4; animal++)
            {
				_betPanelAnimals[animal].sa_red.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 0]);
				_betPanelAnimals[animal].sa_green.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 1]);
				_betPanelAnimals[animal].sa_yellow.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[(3 * animal) + 2]);
            }
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
				enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum((int)_mdata.iTotalPlayerJetton[12 + enjoyGame]);
            }
            // 更新金币数量的显示
            GameEvent._instance.SetGoldNum(m_iCurGoldNum);
        }

        /// <summary>
        /// 续押失败
        /// </summary>
        void ContinueJettonFail(Packet packet)
        {
            // 对接消息
            CMD_S_ContinueJettonsFail _mdata = GameConvert.ByteToStruct<CMD_S_ContinueJettonsFail>(packet.Data);
            if (_mdata.ContinueJettonsFail == 0)
            {
                m_gJettonFailTip.GetComponent<UILabel>().text = "续押失败!";
            }
            if (!m_bBetTime) return;
            GameObject temp = (GameObject)Instantiate(m_gJettonFailTip, Vector3.zero, Quaternion.identity);
            temp.transform.parent = m_tTipsParent;
            temp.transform.localScale = Vector3.one;
            temp.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 更新彩金池的数值显示
        /// </summary>
        void UpdatePrizePool(Packet packet)
        {
            // 对接消息
            CMD_S_PRIZE_DATA _mdata = GameConvert.ByteToStruct<CMD_S_PRIZE_DATA>(packet.Data);
            m_cBounsNum.m_iNum = _mdata.lPrazePool;
			RandomPrizeNum._instance.randomPrizeNum(_mdata.lPrazePool);
        }

        /// <summary>
        /// 显示中彩金的效果
        /// </summary>
        void ShowPrizePool(Packet packet)
        {
             int mychirID = (int)CGameEngine.Instance.MySelf.DeskStation;
             // 对接消息
             CMD_S_SendPrizePoolReward _mdata = GameConvert.ByteToStruct<CMD_S_SendPrizePoolReward>(packet.Data);
//             if (mychirID == (int)_mdata.wChairID)
//             {
//                 m_gPrizePoolTip.GetComponent<UILabel>().text = "[ff0000]恭喜[-][ffff00]您[-][ff0000]获得[-][ffff00]" + _mdata.lRewardGold.ToString() + "[-][ff0000]彩金[-]";
//             }
//             else
//             {
//                 m_gPrizePoolTip.GetComponent<UILabel>().text = "[ff0000]恭喜[-][ffff00]" + (_mdata.wChairID + 1).ToString() + "[-][ff0000]号玩家获得[-][ffff00]" + _mdata.lRewardGold.ToString() + "[-][ff0000]彩金[-]";
//             }
// 
//             GameObject temp = (GameObject)Instantiate(m_gPrizePoolTip, Vector3.zero, Quaternion.identity);
//             temp.transform.parent = m_tTipsParent;
            //             temp.transform.localScale = Vector3.one;
            //             temp.transform.localPosition = Vector3.zero;   
             if (mychirID == (int)_mdata.wChairID)
             {
                 // 把五个结算面板的彩金提示都赋值(只有当前开奖模式的会被显示)
                 for (int i = 0; i < GameEvent._instance.m_arrobjPrizeTips.Length; i++)
                 {
                     GameObject temp = GameEvent._instance.m_arrobjPrizeTips[i];
                     temp.GetComponent<UILabel>().text = "[ff0000]恭喜[-][ffff00]您[-][ff0000]获得[-][ffff00]" + _mdata.lRewardGold.ToString() + "[-][ff0000]彩金[-]";
                     temp.SetActive(true);
                 }
             }
           
            // 接到消息则把值传入GameEvent中.在显示结算面板的时候,提前一秒钟在彩金面板显示效果.
            GameEvent._instance.m_lBounsNum = _mdata.lRewardGold;

            Debug.Log("<color=green>彩金:</color>" + _mdata.lRewardGold);
        }

        /// <summary>
        /// 断线重连
        /// </summary>
        void UserOffLine(Packet packet)
        {
             // 对接消息
            CMD_S_OnActionUserOffLine _mdata = GameConvert.ByteToStruct<CMD_S_OnActionUserOffLine>(packet.Data);
            // 如果不是下注阶段,返回不操作
//             if (_mdata.IsPlaceJetton != 0)
//             {
//                 return;
//             }            
            // 检验玩家ID是否匹配
            if (CGameEngine.Instance.MySelf.DeskStation != _mdata.wChairID)
            {
                return;
            }
            // 配置筹码
            CButtonOnClick._instance.m_cChipNum.chip01 = _mdata.iJetton1;
            CButtonOnClick._instance.m_cChipNum.chip02 = _mdata.iJetton2;
            CButtonOnClick._instance.m_cChipNum.chip03 = _mdata.iJetton3;
            // 显示筹码
            m_cChipNum.chip01.text = ChangeNum2String(_mdata.iJetton1);
            m_cChipNum.chip02.text = ChangeNum2String(_mdata.iJetton2);
            m_cChipNum.chip03.text = ChangeNum2String(_mdata.iJetton3);
            // 设置初始筹码
            GameEvent._instance.m_iCurChipNum = _mdata.iJetton1;

            // 设置剩余时间
            m_cTimeNum.SetGameTimer((int)_mdata.cbTimeLeave, null);
            m_cTimeNum.m_bIsOpen = true;
            // 把时间面板移动出来
            StartCoroutine(GameEvent._instance.MoveTimerAnim((float)_mdata.cbTimeLeave));

            // 缓存玩家金币(未计算前)
            m_iCurGoldNum = _mdata.iUserScore;
            m_iCurGoldNum_temp = _mdata.iUserScore;

            // 设置倍率,下注,总注
            for (int animal = 0; animal < 4; animal++)
            {
                _betPanelAnimals[animal].sa_red.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetBetDoubelNum(_mdata.dwMul[(3 * animal) + 2]);

                _betPanelAnimals[animal].sa_red.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetBetNum((int)_mdata.iJettonScore[(3 * animal) + 2]);

                _betPanelAnimals[animal].sa_red.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 0]);
                _betPanelAnimals[animal].sa_green.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 1]);
                _betPanelAnimals[animal].sa_yellow.SetTotalBetNum((int)_mdata.iAllUserJettonScore[(3 * animal) + 2]);
                // 开启动物按钮
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._red, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._green, true);
                CButtonOnClick._instance.SetButtonBt(_betPanelAnimals[animal]._yellow, true);

                // 计算金币
                for (int color = 0; color < 3; color++)
                {
					m_iCurGoldNum -= _mdata.iJettonScore[(3 * animal) + color];
                }
            }

            // 庄闲和倍率/押分初始为0
            for (int enjoyGame = 0; enjoyGame < 3; enjoyGame++)
            {
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetDoubelNum(_mdata.dwMul[12+enjoyGame]);
                enjoyGames[enjoyGame].seg_enjoyGame.SetBetNum((int)_mdata.iJettonScore[12 + enjoyGame]);
                enjoyGames[enjoyGame].seg_enjoyGame.SetTotalBetNum((int)_mdata.iAllUserJettonScore[12 + enjoyGame]);
                // 开启庄和闲按钮
                CButtonOnClick._instance.SetButtonBt(enjoyGames[enjoyGame]._enjoyGame, true);
                m_iCurGoldNum -= _mdata.iJettonScore[12 + enjoyGame];
            }

            // 设置金币
            GameEvent._instance.SetGoldNum(_mdata.iUserScore);

            // 关闭取消/续押/自动按钮
            CButtonOnClick._instance.SetButtonBt(CleanButton, true);
            CButtonOnClick._instance.SetButtonBt(ContinueButton, true);
            //CButtonOnClick._instance.SetButtonBt(AutoButton, false);

        }

        /// <summary>
        /// 数字转换成汉字
        /// </summary>
        string ChangeNum2String(long num)
        {
            string temp = "";
            if (100 <= num && 1000 > num)
            {
                temp = (num / 100).ToString() + "百";
            }
            else if (1000 <= num && 10000 > num)
            {
                temp = (num / 1000).ToString() + "千";
            }
            else if (10000 <= num && 1000000 > num)
            {
                temp = (num / 10000).ToString() + "万";
            }
            else if (1000000 <= num && 10000000 > num)
            {
                temp = (num / 1000000).ToString() + "百万";
            }
            else if (10000000 <= num && 100000000 > num)
            {
                temp = (num / 10000000).ToString() + "千万";
            }
            else if (100000000 <= num && 1000000000 > num)
            {
                temp = (num / 100000000).ToString() + "亿";
            }
            else
            {
                temp = "数字超过十亿,不予显示";
            }
            return temp;
        }

        /////////////////////////////////////////////////////发送消息
        /// <summary>
        /// 定义发送数据包
        /// </summary> 
        public void SendToGameSvr(uint wMainCmd, uint wSubCmdID, int wHandleCode, byte[] wByteBuffer)
        {
            CGameEngine.Instance.Send(wMainCmd, wSubCmdID, wHandleCode, wByteBuffer);
        }

        /// <summary>
        /// 发送动物押分数据包
        /// </summary>
        public virtual void AnimalButtonOnClick(int _animalType, int _colorType)
        {
            // 定义类型
            CMD_C_PlaceJetton info = new CMD_C_PlaceJetton();
            // 对应参数赋值
            info.eGamble = 0;
            info.stAnimalInfo.eAnimal = _animalType;
            info.stAnimalInfo.eColor = _colorType;
            info.iPlaceJettonScore = GameEvent._instance.m_iCurChipNum;
            // 打包数据
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            // 发送数据到服务器
            SendToGameSvr(MainCmd.MDM_GF_INFO, SubCmd.SUB_C_PLACE_JETTON, 0, dataBuffer);
        }

        /// <summary>
        /// 发送庄闲和押分数据包
        /// </summary>
        /// <param name="_enjoyGameType"></param>
        public virtual void enjoyGameButtonOnClick(int _enjoyGameType)
        {
            // 定义类型
            CMD_C_PlaceJetton info = new CMD_C_PlaceJetton();
            // 对应参数赋值
            info.eGamble = 1;
            info.eEnjoyGameInfo = _enjoyGameType;
            info.iPlaceJettonScore = GameEvent._instance.m_iCurChipNum;
            // 打包数据
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            // 发送数据到服务器
            SendToGameSvr(MainCmd.MDM_GF_INFO, SubCmd.SUB_C_PLACE_JETTON, 0, dataBuffer);
        }

        /// <summary>
        /// 清除下注
        /// </summary>
        public void ClearJetton()
        {
            // 关闭取消押注按钮
            CButtonOnClick._instance.SetButtonBt(CleanButton, false);
            // 定义类型
            CMD_C_PlaceJetton info = new CMD_C_PlaceJetton();
            // 打包数据
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            // 发送数据到服务器
            SendToGameSvr(MainCmd.MDM_GF_INFO, SubCmd.SUB_C_CLEAR_JETTON, 0, dataBuffer);

            // 更新金币数量的显示
            GameEvent._instance.SetGoldNum(m_iCurGoldNum_temp);
            // 每次清除下注,都把当局缓存的金币数用于减少
            m_iCurGoldNum = m_iCurGoldNum_temp;
            // 清空按钮上的下注积分
            Array.Clear(m_iAllButtonBet, 0, m_iAllButtonBet.Length);
        }

        /// <summary>
        /// 续押
        /// </summary>
        public void ContinueBet()
        {
            // 定义类型
            CMD_C_ContinueJetton info = new CMD_C_ContinueJetton();
            // 打包数据
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            // 发送数据到服务器
            SendToGameSvr(MainCmd.MDM_GF_INFO, SubCmd.SUB_C_PLAYER_CONTINUE_BET, 0, dataBuffer);
        }

        /// <summary>
        /// 管理员设置开奖结果
        /// </summary>
        public void AdministratorSetPrize()
        {
            // 定义类型
            CMD_C_Control info = new CMD_C_Control();
            // 对应参数赋值
            //info.eAnimalPrize = CUIGame._instance.eAnimalType_change(_administratorSetPrize.prizeAnimal);
            info.eAnimalPrize = (eAnimalPrizeMode)_administratorSetPrize.prizeGameMode;
            info.eAnimal = (eAnimalType)_administratorSetPrize.prizeAnimal;
            info.eColor = (eColorType)_administratorSetPrize.prizeColor;
            info.eEnjoyGame = (eEnjoyGameType)_administratorSetPrize.prizeEnjoyGame;
            // 打包数据
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            // 发送数据到服务器
            SendToGameSvr(MainCmd.MDM_GF_INFO, SubCmd.SUB_C_CONTROL_SET_PRIZE, 0, dataBuffer);
        }

		// 动物类型转换(接包得到Int转换成对应类型)
		public string eAnimalType_change(int temp_int)
		{
			string temp = "";
			switch (temp_int)
			{
			case -1: 
			{
				temp = "非动物";
				break;
			}
			case 0:
			{
				temp = "狮子";
				break;
			}
			case 1:
			{
				temp = "熊猫";
				break;
			}
			case 2:
			{
				temp = "猴子";
				break;
			}
			case 3:
			{
				temp = "兔子";
				break;
			}
			}
			return temp;
		}
		
		// 颜色类型转换(接包得到Int转换成对应类型)
		public string eColorType_change(int temp_int)
		{
			string temp = "";
			switch (temp_int)
			{
			case -1:
			{
				temp = "非颜色";
				break;
			}
			case 0:
			{
				temp = "红色";
				break;
			}
			case 1:
			{
				temp = "绿色";
				break;
			}
			case 2:
			{
				temp = "黄色";
				break;
			}
			}
			return temp;
		}
		
		// 庄和闲类型转换(接包得到Int转换成对应类型)
		public string eEnjoyGameType_change(int temp_int)
		{
			string temp = "";
			switch (temp_int)
			{
			case -1:
			{
				temp = "非庄闲和";
				break;
			}
			case 0:
			{
				temp = "庄";
				break;
			}
			case 1:
			{
				temp = "闲";
				break;
			}
			case 2:
			{
				temp = "和";
				break;
			}
			}
			return temp;
		}

    }
}
