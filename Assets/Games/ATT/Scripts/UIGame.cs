using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Services.Data;
using System.IO;
using Shared;
using System;

namespace com.QH.QPGame.ATT
{
    public class UIGame : MonoBehaviour
    {


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

        //框架事件入口
        private void OnTableUserEvent(TableEvents tevt, uint userid, object data)
        {

            if (UIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (tevt)
            {
                //用户进来
                case TableEvents.USER_COME:
                    {
                        PlayerInfo userdata = GameEngine.Instance.GetTableUserItem((byte)GameEngine.Instance.MySelf.DeskStation);
                        if (userid != userdata.ID)
                        {
                            int temp_iCahirID = GameEngine.Instance.UserIdToChairId(userid);
                            PlayerInfo userInfo = GameEngine.Instance.GetTableUserItem((ushort)temp_iCahirID);
                            if (userInfo != null)
                            {
                                int temp_iFaceID = (int)userInfo.HeadID;
                                CHeadManger._instance.ShowPlayerHead(temp_iCahirID, temp_iFaceID);
                            }
                        }
                        break;
                    }
                case TableEvents.USER_LEAVE:
                    {
                        for (int i = 0; i < UIManger.Instance.m_iGamePlayer; i++)
                        {
                            if (GameEngine.Instance.GetTableUserItem((ushort)i) == null)
                            {
                                CHeadManger._instance.PlayerExit(i);
                            }
                        }

                        break;
                    }
                case TableEvents.GAME_ENTER:
                    {
                        CHeadManger._instance.SetAllFalse();
                        for (int i = 0; i < UIManger.Instance.m_iGamePlayer; i++)
                        {
                            PlayerInfo playerdata = GameEngine.Instance.GetTableUserItem((ushort)i);
                            if (playerdata == null) continue;
                            CHeadManger._instance.ShowPlayerHead(i, (int)playerdata.HeadID);
                        }
                        break;
                    }
                case TableEvents.GAME_START:
                    {
                        CHeadManger._instance.SetAllFalse();
                        for (int i = 0; i < UIManger.Instance.m_iGamePlayer; i++)
                        {
                            PlayerInfo playerdata = GameEngine.Instance.GetTableUserItem((ushort)i);
                            if (playerdata == null) continue;
                            CHeadManger._instance.ShowPlayerHead(i, (int)playerdata.HeadID);
                        }
                        break;
                    }

            }
        }

        //框架消息入口
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {

            if (UIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (subcmd)
            {
                //游戏状态设置
                case SubCmd.SUB_GF_OPTION:
                    {
                        packet.BeginRead();
                        GameEngine.Instance.MySelf.GameStatus = packet.GetByte();
                        break;
                    }
                //游戏场景消息
                case SubCmd.SUB_GF_SCENE:
                    {
                        OnGameSceneResp(GameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
            }
        }

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {

            if (UIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;

            //if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {
                //开始下注(进入游戏初始化数据)
                case SubCmd.SUB_S_START_JETTON_JX:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        GameStartJettonJX(packet);
                        break;
                    }
                //开始发牌
                case SubCmd.SUB_S_START_DEAL_CARD_JX:
                    {
                        UIManger.Instance.m_bIsStartGame = true;
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        CPushAnimation._instance.gameObject.SetActive(true);
                        CPushAnimation._instance.SetPushAnimation("PUSHHELD");
                        
                        GameSendCard(packet);
                        break;
                    }
                //更新牌
                case SubCmd.SUB_S_UPDATE_CARD_JX:
                    {
                        UIManger.Instance.m_bIsStartGame = false;
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        UpdateCard(packet);

                        break;
                    }
                //比倍返回
                case SubCmd.SUB_S_COMPARE_RETURN_JG:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        ComPareRsualt(packet);
                        break;
                    }
                //游戏结束
                case SubCmd.SUB_S_GAME_END:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        GameEnd(packet);
                        break;
                    }
                //记录
                case SubCmd.SUB_S_SEND_RECORD:
                    {
                        GameRecord(packet);
                        break;
                    }
                //更新皮子分
                case SubCmd.SUB_S_UPDATE_PIZI:
                    {
                        packet.BeginRead();
                        UIManger.Instance.m_tReadFile.n5K = packet.GetInt();
                        UIManger.Instance.m_tReadFile.nRS = packet.GetInt();
                        UIManger.Instance.m_tReadFile.nSF = packet.GetInt();
                        UIManger.Instance.m_tReadFile.n4K = packet.GetInt();
                        break;
                    }
                case SubCmd.SUB_S_REWARD:
                    {
                        packet.BeginRead();
                        int iCardType = packet.GetInt();

                        Int16 iChaird = packet.GetShort();
                        int mychirID = (int)GameEngine.Instance.MySelf.DeskStation;
                        if ((int)iChaird == mychirID)
                        {
                            CEffectManger._instance.m_gMyEffect.SetActive(true);
                            CEffectManger._instance.m_gMyEffect.GetComponent<CEffectItem1>().SetEffect(iCardType);
                            if (CMusicManger._instance.m_bIsOpen)
                            {
                                CEffectManger._instance.m_gMyEffect.GetComponent<AudioSource>().Play();
                            }
                        }
                        else
                        {
                            CEffectManger._instance.m_gEffectList[iChaird].SetActive(true);
                            CEffectManger._instance.m_gEffectList[iChaird].GetComponent<CEffectItem1>().SetEffect(iCardType);
                            if (CMusicManger._instance.m_bIsOpen)
                            {
                                CEffectManger._instance.m_gEffectList[iChaird].GetComponent<AudioSource>().Play();
                            }

                        }
                        break;
                    }
                //金币不足返回
                case SubCmd.SUB_S_USER_NO_MONEY:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        CNomaney._instance.ShowWindow();
                        CBottomBTOnclick._instance.SetAllBTFalse();
                        long temp = CPlayerInfo._instance.m_iCreditNum ;
                        temp = temp - temp % 5;
                        if (temp > 80)
                        {
                            temp = 80;
                        }
                        CPlayerInfo._instance.m_gLabel80nums.GetComponent<CLabelNum>().m_iNum = temp;
                        break;
                    }
                //压分太小
                case SubCmd.SUB_S_USER_BET_MIN:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                      
                        CBottomBTOnclick._instance.SetAllBTFalse();
                        CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gStartBT, false);
                        CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gInputScoreBT, false);
                        if (CPlayerInfo._instance.m_iCreditNum < UIManger.Instance.m_cGameSceneFree.m_nSmallChip)
                        {
                            CNomaney._instance.ShowWindow();
                        }
                        break;
                    }
                //空闲请求比倍返回
                case SubCmd.SUB_S_COMPARE_F_REURN:
                    {
                        UIManger.Instance.m_bIsOpenTimer = false;
                        UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
                        CCompareManger._instance.SetCompareCard(CPokerPointsManger._instance.m_iBasePointBK);
                        CCompareManger._instance.SetCurrentCompare(0);
                        CCompareManger._instance.SetHistoryCard();
                        CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gBigBT, false);
                        CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gLittleBT, false);

                        // CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, true);
                        break;
                    }
                //彩金数据
                case SubCmd.SUB_S_SEND_PRIZE_DATA:
                    {
                        packet.BeginRead();
                        Int64 Prizepool = packet.GetLong();
                        if (Prizepool > 0)
                        {
                            CPrizeManger._instance.gameObject.SetActive(true);
                            CPrizeManger._instance.m_gPrizePool.GetComponent<CLabelNum>().m_iNum = Prizepool;
                            
                        }
                        else
                            CPrizeManger._instance.gameObject.SetActive(false);

                        break;
                    }
                case SubCmd.SUB_S_SEND_PRIZE_REWARD:
                    {
                        packet.BeginRead();

                        long lRewardPool = packet.GetLong();
                        Int16 wTableID = packet.GetShort();
                        Int16 wChairId = packet.GetShort();

                        if (lRewardPool > 0)
                        {
                            CPrizeManger._instance.gameObject.SetActive(true);
                            if (wTableID == GameEngine.Instance.MySelf.DeskNO && wChairId == GameEngine.Instance.MySelf.DeskStation)
                            {
                                UIManger.Instance.m_lGRPrizePool = lRewardPool;
                                CMusicManger._instance.PlaySound("Prompt");
                                CPrizeManger._instance.m_lMyReward.text = "恭喜您获得[ff0000]" + lRewardPool.ToString() + "[-]彩金奖励";
                                CPrizeManger._instance.OpenPrizePool(2);
                            }
                            else
                            {
                                int num = UIManger.Instance.m_iGamePlayer * wTableID + wChairId + 1;
                                CPrizeManger._instance.m_lReward.text = "恭喜[ff0000]" + num.ToString() + "号机[-]获得[ff0000]" + lRewardPool.ToString() + "[-]彩金奖励";
                                CPrizeManger._instance.OpenPrizePool(1);
                            }
                        }
                        break;
                    }

            }
        }

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {

            packet.BeginRead();
            byte _tempGameStaus = packet.GetByte();
            //Debuger.LogError(bGameStatus);
            switch (_tempGameStaus)
            {
                //空闲
                case (byte)GameLogic.GS_TK_FREE:
                    {
                        CPushAnimation._instance.gameObject.SetActive(true);
                        CPushAnimation._instance.SetPushAnimation("PUSHBET");
                        GameSceenGameFree(packet);
                        break;
                    }
                //下注
                case (byte)GameLogic.GS_TK_BET:
                    {
                        CPushAnimation._instance.gameObject.SetActive(true);
                        CPushAnimation._instance.SetPushAnimation("PUSHHELD");
                        GameSceenGameBET(packet);
                        break;
                    }
                //换牌
                case (byte)GameLogic.GS_TK_UPDATE_CARD:
                    {
                        CPushAnimation._instance.gameObject.SetActive(true);
                        CPushAnimation._instance.SetPushAnimation("TAKESCORE");
                        GameSceenGameUpdateCard(packet);
                        break;
                    }
                //比倍
                case (byte)GameLogic.GS_TK_COMPARE:
                    {
                        CPushAnimation._instance.gameObject.SetActive(true);
                        CPushAnimation._instance.SetPushAnimation("TAKESCORE");
                        GameSceenGameCompare(packet);
                        break;
                    }

            }
        }

        #region 发送消息给服务器
        public static void SendMsgToServer(NPacket packet)
        {
            //             if (_bEndGame)
            //             {
            //                 return;
            //             }
            GameEngine.Instance.Send(packet);
        }
        #endregion

        /// <summary>
        /// 游戏空闲状态
        /// </summary>
        /// <param name="packet"></param>
        public void GameSceenGameFree(NPacket packet)
        {
            CBottomBTOnclick._instance.m_bIsCompareFree = false;

            //读取消息
            packet.BeginRead();
            byte temp_gameStaus = packet.GetByte();
            UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_lExchangeGold = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_nSmallChip = packet.GetInt();
            UIManger.Instance.m_iGamePlayer = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_IsCompare = packet.GetBool();
            UIManger.Instance.m_iGameMaxPoint = packet.GetInt();
            UIManger.Instance.m_iGameStartPoint = packet.GetInt();
            UIManger.Instance.m_iGameTime = packet.GetInt();
            UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;
            int gamePrizeCellScore = packet.GetInt();
            CHelp._instance.m_text0.text = UIManger.Instance.m_cGameSceneFree.m_nSmallChip.ToString();
            CHelp._instance.m_text1.text = gamePrizeCellScore.ToString();

            CPokerPointsManger._instance.m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
            CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;
            CPokerPointsManger._instance.m_iBasePointBK = UIManger.Instance.m_iGameStartPoint;
            //隐藏头像
            if (UIManger.Instance.m_iGamePlayer < 6)
                CHeadManger._instance.SetHeadHide();
            PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((byte)GameEngine.Instance.MySelf.DeskStation);
            //设置玩家信息
            CPlayerInfo._instance.SetPlayerInfo(userdata.NickName, UIManger.Instance.m_cGameSceneFree.m_lExchangeGold, UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale, (int)userdata.Money, UIManger.Instance.m_cGameSceneFree.m_nSmallChip);
            CPrizeManger._instance.ShowHidePrize(0);
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerPointsManger._instance.ShowWindow();
            CBasePoint._instance.ShowWindow();
#else

#endif

        }
        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="packet"></param>
        public void GameSceenGameBET(NPacket packet)
        {
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerManger._instance.ShowWindow();
#else

#endif
            //读取消息
            packet.BeginRead();

            byte temp_gameStaus = packet.GetByte();
            UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_lExchangeGold = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_nSmallChip = packet.GetInt();
            UIManger.Instance.m_iGamePlayer = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_IsCompare = packet.GetBool();
            UIManger.Instance.m_iGameMaxPoint = packet.GetInt();
            UIManger.Instance.m_iGameStartPoint = packet.GetInt();
            UIManger.Instance.m_iGameTime = packet.GetInt();
            UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;

            int gamePrizeCellScore = packet.GetInt();
            CHelp._instance.m_text0.text = UIManger.Instance.m_cGameSceneFree.m_nSmallChip.ToString();
            CHelp._instance.m_text1.text = gamePrizeCellScore.ToString();

            CPokerPointsManger._instance.m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
            CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;
            CPokerPointsManger._instance.m_iBasePointBK = UIManger.Instance.m_iGameStartPoint;
            //隐藏头像
            if (UIManger.Instance.m_iGamePlayer < 6)
                CHeadManger._instance.SetHeadHide();
            PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((byte)GameEngine.Instance.MySelf.DeskStation);
            //设置玩家信息
            CPlayerInfo._instance.SetPlayerInfo(userdata.NickName, UIManger.Instance.m_cGameSceneFree.m_lExchangeGold, UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale, (int)userdata.Money, UIManger.Instance.m_cGameSceneFree.m_nSmallChip);

            UIManger.Instance.m_tReadFile.wID = packet.GetShort();
            UIManger.Instance.m_tReadFile.nHistoryJetton = packet.GetInt();
            UIManger.Instance.m_tReadFile.n5K = packet.GetInt();
            UIManger.Instance.m_tReadFile.nRS = packet.GetInt();
            UIManger.Instance.m_tReadFile.nSF = packet.GetInt();
            UIManger.Instance.m_tReadFile.n4K = packet.GetInt();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    UIManger.Instance.m_tReadFile.cbPointTimes[i, j] = packet.GetInt();
                    CPokerPointsManger._instance.m_lPointsList[9 - i].m_lTimesList[j] = UIManger.Instance.m_tReadFile.cbPointTimes[i, j];
                }
            }
            CCompareManger._instance.m_iTiems.Clear();
            for (int i = 0; i < 6; i++)
            {
                UIManger.Instance.m_tReadFile.cbCompareTimes[i] = packet.GetInt();
                CCompareManger._instance.m_iTiems.Add(UIManger.Instance.m_tReadFile.cbCompareTimes[i]);
            }
            //历史牌点
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);

            CBasePoint._instance.m_str5K = UIManger.Instance.m_tReadFile.n5K;
            CBasePoint._instance.m_strRS = UIManger.Instance.m_tReadFile.nRS;
            CBasePoint._instance.m_strSF = UIManger.Instance.m_tReadFile.nSF;
            CBasePoint._instance.m_str4K = UIManger.Instance.m_tReadFile.n4K;

            int temp_max = packet.GetInt();
            UIManger.Instance.m_iPeiLvType = packet.GetInt();
            CPokerPointsManger._instance.m_iBasePoints = packet.GetInt();
            UIManger.Instance.m_bIsUpdateCard = true;
            packet.GetBytes(ref UIManger.Instance.m_cUpdateCard.cCard.cbCard, 5);

            //设置扑克牌
            for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
            {
                UIManger.Instance.m_cUpdateCard.cCard.bBarter[i] = packet.GetBool();
                int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]) / 16 + 1;
                int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]);
                if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4F)
                {
                    color = 5;
                    value = 2;
                }

                CPokerManger._instance.SetCard(i, color, value);
            }
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gInputScoreBT, true);
            if (CPlayerInfo._instance.m_iCreditNum < UIManger.Instance.m_cGameSceneFree.m_nSmallChip)
            {
                //弹框告诉用户钱不够

            }
            else
            {
                CPokerManger._instance.OpenCard();
            }
            //设置倍率
            CPokerPointsManger._instance.SetLuckyTiemsColor(UIManger.Instance.m_iPeiLvType);
        }
        /// <summary>
        /// 换牌
        /// </summary>
        /// <param name="packet"></param>
        public void GameSceenGameUpdateCard(NPacket packet)
        {
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerManger._instance.ShowWindow();
#else

#endif

            //读取消息
            packet.BeginRead();

            byte temp_gameStaus = packet.GetByte();
            UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_lExchangeGold = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_nSmallChip = packet.GetInt();
            UIManger.Instance.m_iGamePlayer = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_IsCompare = packet.GetBool();
            UIManger.Instance.m_iGameMaxPoint = packet.GetInt();
            UIManger.Instance.m_iGameStartPoint = packet.GetInt();
            UIManger.Instance.m_iGameTime = packet.GetInt();
            UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;

            int gamePrizeCellScore = packet.GetInt();
            CHelp._instance.m_text0.text = UIManger.Instance.m_cGameSceneFree.m_nSmallChip.ToString();
            CHelp._instance.m_text1.text = gamePrizeCellScore.ToString();

            CPokerPointsManger._instance.m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
            CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;
            CPokerPointsManger._instance.m_iBasePointBK = UIManger.Instance.m_iGameStartPoint;
            //隐藏头像
            if (UIManger.Instance.m_iGamePlayer < 6)
                CHeadManger._instance.SetHeadHide();
            PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((byte)GameEngine.Instance.MySelf.DeskStation);
            //设置玩家信息
            CPlayerInfo._instance.SetPlayerInfo(userdata.NickName, UIManger.Instance.m_cGameSceneFree.m_lExchangeGold, UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale, (int)userdata.Money, UIManger.Instance.m_cGameSceneFree.m_nSmallChip);

            UIManger.Instance.m_tReadFile.wID = packet.GetShort();
            UIManger.Instance.m_tReadFile.nHistoryJetton = packet.GetInt();
            UIManger.Instance.m_tReadFile.n5K = packet.GetInt();
            UIManger.Instance.m_tReadFile.nRS = packet.GetInt();
            UIManger.Instance.m_tReadFile.nSF = packet.GetInt();
            UIManger.Instance.m_tReadFile.n4K = packet.GetInt();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    UIManger.Instance.m_tReadFile.cbPointTimes[i, j] = packet.GetInt();
                    CPokerPointsManger._instance.m_lPointsList[9 - i].m_lTimesList[j] = UIManger.Instance.m_tReadFile.cbPointTimes[i, j];
                }
            }
            CCompareManger._instance.m_iTiems.Clear();
            for (int i = 0; i < 6; i++)
            {
                UIManger.Instance.m_tReadFile.cbCompareTimes[i] = packet.GetInt();
                CCompareManger._instance.m_iTiems.Add(UIManger.Instance.m_tReadFile.cbCompareTimes[i]);
            }
            //历史牌点
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);

            CBasePoint._instance.m_str5K = UIManger.Instance.m_tReadFile.n5K;
            CBasePoint._instance.m_strRS = UIManger.Instance.m_tReadFile.nRS;
            CBasePoint._instance.m_strSF = UIManger.Instance.m_tReadFile.nSF;
            CBasePoint._instance.m_str4K = UIManger.Instance.m_tReadFile.n4K;

            UIManger.Instance.m_iPeiLvType = packet.GetInt();
            CPokerPointsManger._instance.m_iBasePoints = packet.GetInt();
            UIManger.Instance.m_bIsUpdateCard = false;
            packet.GetBytes(ref UIManger.Instance.m_cUpdateCard.cCard.cbCard, 5);

            //设置扑克牌
            for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
            {
                UIManger.Instance.m_cUpdateCard.cCard.bBarter[i] = packet.GetBool();
                int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]) / 16 + 1;
                int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]);
                if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4F)
                {
                    color = 5;
                    value = 2;
                }

                CPokerManger._instance.SetCard(i, color, value);
            }

            UIManger.Instance.m_cUpdateCard.bCardType = packet.GetByte();
            UIManger.Instance.m_cUpdateCard.bIsGameOver = packet.GetBool();
            UIManger.Instance.m_cUpdateCard.bIsCompare = UIManger.Instance.m_cGameSceneFree.m_IsCompare;

            //设置倍率
            CPokerPointsManger._instance.SetLuckyTiemsColor(UIManger.Instance.m_iPeiLvType);
            CPokerManger._instance.OpenCard();
            CBottomBTOnclick._instance.SetAllBTFalse();
            CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(true);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT1, false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, false);
            CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(false);
        }
        /// <summary>
        /// 比倍
        /// </summary>
        /// <param name="packet"></param>
        public void GameSceenGameCompare(NPacket packet)
        {
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();

#else

#endif
            CCompareManger._instance.ShowWindow();
            //读取消息
            packet.BeginRead();

            byte temp_gameStaus = packet.GetByte();
            UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_lExchangeGold = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_nSmallChip = packet.GetInt();
            UIManger.Instance.m_iGamePlayer = packet.GetInt();
            UIManger.Instance.m_cGameSceneFree.m_IsCompare = packet.GetBool();
            UIManger.Instance.m_iGameMaxPoint = packet.GetInt();
            UIManger.Instance.m_iGameStartPoint = packet.GetInt();
            UIManger.Instance.m_iGameTime = packet.GetInt();
            UIManger.Instance.m_fGameTime = (float)UIManger.Instance.m_iGameTime;

            int gamePrizeCellScore = packet.GetInt();
            CHelp._instance.m_text0.text = UIManger.Instance.m_cGameSceneFree.m_nSmallChip.ToString();
            CHelp._instance.m_text1.text = gamePrizeCellScore.ToString();

            CPokerPointsManger._instance.m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
            CPokerPointsManger._instance.m_iBasePointBK = UIManger.Instance.m_iGameStartPoint;
            CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;
            //隐藏头像
            if (UIManger.Instance.m_iGamePlayer < 6)
                CHeadManger._instance.SetHeadHide();
            PlayerInfo userdata = GameEngine.Instance.EnumTablePlayer((byte)GameEngine.Instance.MySelf.DeskStation);
            //设置玩家信息
            CPlayerInfo._instance.SetPlayerInfo(userdata.NickName, UIManger.Instance.m_cGameSceneFree.m_lExchangeGold, UIManger.Instance.m_cGameSceneFree.m_ilExchangeScale, (int)userdata.Money, UIManger.Instance.m_cGameSceneFree.m_nSmallChip);

            UIManger.Instance.m_tReadFile.wID = packet.GetShort();
            UIManger.Instance.m_tReadFile.nHistoryJetton = packet.GetInt();
            UIManger.Instance.m_tReadFile.n5K = packet.GetInt();
            UIManger.Instance.m_tReadFile.nRS = packet.GetInt();
            UIManger.Instance.m_tReadFile.nSF = packet.GetInt();
            UIManger.Instance.m_tReadFile.n4K = packet.GetInt();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    UIManger.Instance.m_tReadFile.cbPointTimes[i, j] = packet.GetInt();
                    CPokerPointsManger._instance.m_lPointsList[9 - i].m_lTimesList[j] = UIManger.Instance.m_tReadFile.cbPointTimes[i, j];
                }
            }
            CCompareManger._instance.m_iTiems.Clear();
            for (int i = 0; i < 6; i++)
            {
                UIManger.Instance.m_tReadFile.cbCompareTimes[i] = packet.GetInt();
                CCompareManger._instance.m_iTiems.Add(UIManger.Instance.m_tReadFile.cbCompareTimes[i]);
            }
            //历史牌点
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);

            CBasePoint._instance.m_str5K = UIManger.Instance.m_tReadFile.n5K;
            CBasePoint._instance.m_strRS = UIManger.Instance.m_tReadFile.nRS;
            CBasePoint._instance.m_strSF = UIManger.Instance.m_tReadFile.nSF;
            CBasePoint._instance.m_str4K = UIManger.Instance.m_tReadFile.n4K;

            UIManger.Instance.m_iPeiLvType = packet.GetInt();
            CPokerPointsManger._instance.m_iBasePoints = packet.GetInt();
            UIManger.Instance.m_bIsUpdateCard = false;
            packet.GetBytes(ref UIManger.Instance.m_cUpdateCard.cCard.cbCard, 5);

            //设置扑克牌
            for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
            {
                UIManger.Instance.m_cUpdateCard.cCard.bBarter[i] = packet.GetBool();
                int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]) / 16 + 1;
                int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]);
                if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4F)
                {
                    color = 5;
                    value = 2;
                }

                CPokerManger._instance.SetCard(i, color, value);
                CPokerManger._instance.m_lCPokerList[i].m_bIsOpenCard = true;
            }

            UIManger.Instance.m_cUpdateCard.bCardType = packet.GetByte();
            UIManger.Instance.m_cUpdateCard.bIsGameOver = packet.GetBool();
            UIManger.Instance.m_cUpdateCard.bIsCompare = UIManger.Instance.m_cGameSceneFree.m_IsCompare;


            UIManger.Instance.m_cCompareResualt.bIsWin = packet.GetBool();
            UIManger.Instance.m_cCompareResualt.cbCardData = packet.GetByte();
            UIManger.Instance.m_cCompareResualt.cbCompareID = packet.GetByte();
            int temp_compareBet = packet.GetInt();
            CBottomBTOnclick._instance.m_bIsCompareFree = packet.GetBool();
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);



            //设置倍率
            CPokerPointsManger._instance.SetLuckyTiemsColor(UIManger.Instance.m_iPeiLvType);

            int tempIndex = 10 - UIManger.Instance.m_cUpdateCard.bCardType;
            int tempType = UIManger.Instance.m_iPeiLvType;
            int tempbet = 0;
            if (CBottomBTOnclick._instance.m_bIsCompareFree)
            {
                tempbet = (int)temp_compareBet;
            }
            else
            {
                tempbet = CPokerPointsManger._instance.m_lPointsList[tempIndex].m_lPointList[tempType];
            }

            CCompareManger._instance.SetCompareCard(tempbet);
            CCompareManger._instance.SetOpenCard(UIManger.Instance.m_cCompareResualt.cbCardData, UIManger.Instance.m_cCompareResualt.bIsWin);
            CCompareManger._instance.SetHistoryCard();
            CBottomBTOnclick._instance.SetAllBTFalse();
            StartCoroutine(CCompareManger._instance.WaitTimeChangeState(1.5f));
            CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(true);
            CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT1, false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT2, false);
            CCompareManger._instance.SetCurrentCompare(UIManger.Instance.m_cCompareResualt.cbCompareID);

        }
        /// <summary>
        /// 开始下注
        /// </summary>
        /// <param name="packet"></param>
        public void GameStartJettonJX(NPacket packet)
        {

            //读取消息
            packet.BeginRead();
            UIManger.Instance.m_tReadFile.wID = packet.GetShort();
            UIManger.Instance.m_tReadFile.nHistoryJetton = packet.GetInt();
            UIManger.Instance.m_tReadFile.n5K = packet.GetInt();
            UIManger.Instance.m_tReadFile.nRS = packet.GetInt();
            UIManger.Instance.m_tReadFile.nSF = packet.GetInt();
            UIManger.Instance.m_tReadFile.n4K = packet.GetInt();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    UIManger.Instance.m_tReadFile.cbPointTimes[i, j] = packet.GetInt();
                    CPokerPointsManger._instance.m_lPointsList[9 - i].m_lTimesList[j] = UIManger.Instance.m_tReadFile.cbPointTimes[i, j];
                }
            }

            CCompareManger._instance.m_iTiems.Clear();
            for (int i = 0; i < 6; i++)
            {
                UIManger.Instance.m_tReadFile.cbCompareTimes[i] = packet.GetInt();
                CCompareManger._instance.m_iTiems.Add(UIManger.Instance.m_tReadFile.cbCompareTimes[i]);
            }
            //历史牌点
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);
            for (int i = 0; i < 6; i++)
            {
                Debug.Log(UIManger.Instance.m_tReadFile.cbHistoryCard[i].ToString());
            }
            CBasePoint._instance.m_str5K = UIManger.Instance.m_tReadFile.n5K;
            CBasePoint._instance.m_strRS = UIManger.Instance.m_tReadFile.nRS;
            CBasePoint._instance.m_strSF = UIManger.Instance.m_tReadFile.nSF;
            CBasePoint._instance.m_str4K = UIManger.Instance.m_tReadFile.n4K;

        }
        /// <summary>
        /// 第一次发牌
        /// </summary>
        /// <param name="packet"></param>
        public void GameSendCard(NPacket packet)
        {
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerManger._instance.ShowWindow();
#else

#endif
            packet.BeginRead();
            UIManger.Instance.m_bIsUpdateCard = true;
            int iMaxType = packet.GetInt();
            UIManger.Instance.m_iPeiLvType = packet.GetInt();

            packet.GetBytes(ref UIManger.Instance.m_cUpdateCard.cCard.cbCard, 5);

            //设置扑克牌
            for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
            {
                UIManger.Instance.m_cUpdateCard.cCard.bBarter[i] = packet.GetBool();

                int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]) / 16 + 1;
                int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]);
                if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4F)
                {
                    color = 5;
                    value = 2;
                }
                CPokerManger._instance.SetCard(i, color, value);
            }
            if (CBottomBTOnclick._instance.m_bIsInPutScore == false)
            {
                CPlayerInfo._instance.m_iGold -= CPlayerInfo._instance.m_iRoomTimes * CPokerPointsManger._instance.m_iBasePoints;
                CPlayerInfo._instance.m_iCreditNum = (int)(CPlayerInfo._instance.m_iGold / CPlayerInfo._instance.m_iRoomTimes);
            }
            CBottomBTOnclick._instance.m_bIsInPutScore = false;
            
            CPokerManger._instance.OpenCard();
            
            StartCoroutine(WaitSetStarBT(2.0f));
            //设置倍率
            CPokerPointsManger._instance.SetLuckyTiemsColor(UIManger.Instance.m_iPeiLvType);

            CBasePoint._instance.SetBasePoint();
        }
        public IEnumerator WaitSetStarBT(float _time)
        {
            yield return new WaitForSeconds(_time);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gStartBT, false);
        }
        /// <summary>
        /// 第二次发牌返回
        /// </summary>
        /// <param name="packet"></param>
        public void UpdateCard(NPacket packet)
        {
            //设置UI
            UIManger.Instance.m_bIsUpdateCard = false;
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gStartBT.gameObject, true);
            packet.BeginRead();
            Int64[] lScore = new long[10];
            UIManger.Instance.m_lAllScore = CPlayerInfo._instance.m_iGold;
            for (int i = 0; i < 10; i++)
            {
                lScore[i] = packet.GetLong();
                UIManger.Instance.m_lAllScore += lScore[i] * CPlayerInfo._instance.m_iRoomTimes;
            }
            Int64 lPZScore = packet.GetLong();
            UIManger.Instance.m_lPrizeScore = lPZScore;
            UIManger.Instance.m_lAllScore += lPZScore * CPlayerInfo._instance.m_iRoomTimes;
          
            packet.GetBytes(ref UIManger.Instance.m_cUpdateCard.cCard.cbCard, 5);
            for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
            {
                UIManger.Instance.m_cUpdateCard.cCard.bBarter[i] = packet.GetBool();

                if (!CPokerManger._instance.m_lCPokerList[i].m_bIsChecked)
                {
                    int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]) / 16 + 1;
                    int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_cUpdateCard.cCard.cbCard[i]);
                    if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4E)
                    {
                        color = 5;
                        value = 1;
                    }
                    else if (UIManger.Instance.m_cUpdateCard.cCard.cbCard[i] == 0x4F)
                    {
                        color = 5;
                        value = 2;
                    }
                    CPokerManger._instance.SetCard(i, color, value);
                }

            }

            UIManger.Instance.m_cUpdateCard.bCardType = packet.GetByte();
            UIManger.Instance.m_cUpdateCard.bIsGameOver = packet.GetBool();
            UIManger.Instance.m_cUpdateCard.bIsCompare = packet.GetBool();
            ///////////////////////////////////////////////////////////
            //文件记录
            int temp = packet.GetInt();
            temp = packet.GetInt();
            temp = packet.GetInt();
            temp = packet.GetInt();
            temp = packet.GetInt();
            temp = packet.GetInt();

            byte[] mm = new byte[6];
            //历史牌点
            packet.GetBytes(ref mm, 6);

            /////////////////////////////////////////////////////////

            UIManger.Instance.m_cUpdateCard.chip = packet.GetLong();
            UIManger.Instance.m_cUpdateCard.nDie = packet.GetInt();
            UIManger.Instance.m_cUpdateCard.chip = packet.GetInt();
            UIManger.Instance.m_cUpdateCard.nState = packet.GetInt();

            CPokerManger._instance.OpenCard();
            if (UIManger.Instance.m_cUpdateCard.bCardType > 0)
            {
                int cardType = UIManger.Instance.m_cUpdateCard.bCardType;
               // for()
               // CPokerPointsManger._instance.m_lPointsList[10 - cardType].m_lPointList[UIManger.Instance.m_iPeiLvType] = (int)lScore;
               // CPokerPointsManger._instance.m_iIndexColor = 10 - UIManger.Instance.m_cUpdateCard.bCardType;
                for (int i = 9; i >=0; i--)
                {
                    int index = 9 - i;
                    if (lScore[i] > 0)
                        CPokerPointsManger._instance.m_iIndexColor[index] = 1;
                        else
                        CPokerPointsManger._instance.m_iIndexColor[index] = 0;
                }
                    CPokerPointsManger._instance.m_bIsChangeColor = true;
            }
            //游戏结束
            if (UIManger.Instance.m_cUpdateCard.bIsGameOver)
            {
                CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gClearBT, true);

                //输赢判断
                if (UIManger.Instance.m_cUpdateCard.bCardType > 0)
                {
                    StartCoroutine(PlayWinMusic(1.2f));
                    //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
                    StartCoroutine(PhoneWaitTimeGetScore(3.0f));

#else
                    if (UIManger.Instance.m_cUpdateCard.bCardType >= 7)
                    {
                        StartCoroutine(PCWaitTimeGetScore(5.0f));
                    }
                    else
                    {
                        StartCoroutine(PCWaitTimeGetScore(3.0f));
                    }
                    
#endif

                }//输
                else
                {
                    CMusicManger._instance.PlaySound("NoCard");
                    StartCoroutine(UIManger.Instance.WaitTimeRectGame(3.0f));
                }
            }
            //比倍
            else if (UIManger.Instance.m_cUpdateCard.bIsCompare)
            {
                StartCoroutine(PlayWinMusic(1.2f));
               // StartCoroutine(PCWaitSetUI(1.2f));
#if !UNITY_STANDALONE_WIN
                StartCoroutine(PhoneWaitSetUI(5.0f));
#else
                StartCoroutine(PCWaitSetUI(1.2f));
#endif

            }

        }
        /// <summary>
        /// 手机无比倍
        /// 
        /// </summary>
        /// <param name="_waitTime"></param>
        /// <returns></returns>
        public IEnumerator PhoneWaitTimeGetScore(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            CPokerPointsManger._instance.SetScoreZero(UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType);
            CPokerPointsManger._instance.SetScoreAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType, 1);

            if (UIManger.Instance.m_cUpdateCard.bCardType >= 7 && UIManger.Instance.m_lPrizeScore>=200)
            {
                CBasePoint._instance.SetBasePointAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, 1);
            }
            CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT, false);
        }
        public IEnumerator PCWaitTimeGetScore(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            CPokerPointsManger._instance.SetScoreZero(UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType);
            CPokerPointsManger._instance.SetScoreAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType, 1);
            
            CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT, false);
            if (UIManger.Instance.m_cUpdateCard.bCardType >= 7 && UIManger.Instance.m_lPrizeScore >= 200)
            {
                CBasePoint._instance.SetBasePointAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, 1);
            }
        }
        public IEnumerator PlayWinMusic(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            CMusicManger._instance.PlaySound("HaveCard");
        }
        public IEnumerator PCWaitSetUI(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);

            #if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerPointsManger._instance.ShowWindow();
            CBasePoint._instance.ShowWindow();
#endif
            CPushAnimation._instance.gameObject.SetActive(true);
            CPushAnimation._instance.SetPushAnimation("TAKESCORE");
            CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(true);
            CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(true);
            CBottomBTOnclick._instance.m_gGetScoreBT.SetActive(false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT1, false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, false);
            CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(false);
            UIManger.Instance.m_bIsGetscore = true;
        }

        public IEnumerator PhoneWaitSetUI(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);

#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerPointsManger._instance.ShowWindow();
            CBasePoint._instance.ShowWindow();
#endif
            CPushAnimation._instance.gameObject.SetActive(true);
            CPushAnimation._instance.SetPushAnimation("TAKESCORE");
            CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(true);
            CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(true);
            CBottomBTOnclick._instance.m_gGetScoreBT.SetActive(false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT1, false);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, false);
            CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(false);
            UIManger.Instance.m_bIsGetscore = true;
        }
        /// <summary>
        /// 比倍结果返回
        /// </summary>
        /// <param name="packet"></param>
        public void ComPareRsualt(NPacket packet)
        {
            packet.BeginRead();
            UIManger.Instance.m_cCompareResualt.bIsWin = packet.GetBool();
            UIManger.Instance.m_cCompareResualt.cbCardData = packet.GetByte();
            UIManger.Instance.m_cCompareResualt.cbCompareID = packet.GetByte();
            packet.GetBytes(ref UIManger.Instance.m_tReadFile.cbHistoryCard, 6);

            CCompareManger._instance.SetOpenCard(UIManger.Instance.m_cCompareResualt.cbCardData, UIManger.Instance.m_cCompareResualt.bIsWin);
            CCompareManger._instance.SetHistoryCard();
            CBottomBTOnclick._instance.SetAllBTFalse();
            //比倍赢
            if (UIManger.Instance.m_cCompareResualt.bIsWin)
            {
                CPushAnimation._instance.gameObject.SetActive(true);
                CPushAnimation._instance.SetPushAnimation("win");

                CMusicManger._instance.PlaySound("CompareWin");
                CCompareManger._instance.m_gBig.SetActive(false);
                CCompareManger._instance.m_gSmall.SetActive(false);

                StartCoroutine(CCompareManger._instance.WaitTimeChangeState(1.5f));
                CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(true);
                CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(false);
                CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(true);
                CBottomBTOnclick._instance.m_gGetScoreBT.SetActive(false);
                CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT1, false);
                CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT2, false);
                CCompareManger._instance.SetCurrentCompare(UIManger.Instance.m_cCompareResualt.cbCompareID);
                UIManger.Instance.m_bIsGetscore = true;
            }
            else
            {
                if (GameLogic.GetCardValue(UIManger.Instance.m_cCompareResualt.cbCardData) > 7)
                {
                    CCompareManger._instance.m_gSmall.SetActive(true);
                    CCompareManger._instance.m_gSmall.GetComponent<CBigSmallAnimation>().RectAnimarion();
                    CCompareManger._instance.m_gBig.SetActive(false);
                }
                else
                {
                    CCompareManger._instance.m_gSmall.SetActive(false);
                    CCompareManger._instance.m_gBig.SetActive(true);
                    CCompareManger._instance.m_gSmall.GetComponent<CBigSmallAnimation>().RectAnimarion();
                }
                CCompareManger._instance.m_bIsWinPushDouble = false;
                CPushAnimation._instance.gameObject.SetActive(false);
                CMusicManger._instance.PlaySound("NoCard");
                CCompareManger._instance.m_lCompareList[CCompareManger._instance.m_iCurrentIndex].m_gLabel.GetComponent<CLabelNum>().m_iNum = 0;
                CCompareManger._instance.m_lCompareList[CCompareManger._instance.m_iCurrentIndex].m_gLabel.GetComponent<CLabelNum>().m_cColor = CCompareManger._instance.m_cGray;
                UIManger.Instance.m_lAllScore = CPlayerInfo._instance.m_iGold;
                StartCoroutine(UIManger.Instance.WaitTimeRectGame(3.0f));
            }

        }


        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="packet"></param>
        public void GameEnd(NPacket packet)
        {
            packet.BeginRead();
            int chip = packet.GetInt();
            bool bIswin = packet.GetBool();

            CBottomBTOnclick._instance.SetAllBTFalse();
            if (!CBottomBTOnclick._instance.m_bIsOpenComPare)
            {
                //CPokerPointsManger._instance.m_lPointsList[10 - UIManger.Instance.m_cUpdateCard.bCardType].m_lPointList[UIManger.Instance.m_iPeiLvType] = chip;
                CPokerPointsManger._instance.SetScoreAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType, 1);
                CPokerPointsManger._instance.SetScoreZero(UIManger.Instance.m_cUpdateCard.bCardType, UIManger.Instance.m_iPeiLvType);
                if (UIManger.Instance.m_cUpdateCard.bCardType >= 7 && UIManger.Instance.m_lPrizeScore >= 200)
                  CBasePoint._instance.SetBasePointAnimation(true, UIManger.Instance.m_cUpdateCard.bCardType, 1);
                //CBottomBTOnclick._instance.GetScoreBT_OnClick();
            }
            else
            {
                CCompareManger._instance.m_bIsOpen = true;
                UIManger.Instance.m_lAllScore = CPlayerInfo._instance.m_iGold + chip * CPlayerInfo._instance.m_iRoomTimes;
                //CPlayerInfo._instance.m_iGold += (chip * CPlayerInfo._instance.m_iRoomTimes);
            }

            CCompareManger._instance.m_gBig.SetActive(false);
            CCompareManger._instance.m_gSmall.SetActive(false);
            CCompareManger._instance.m_bIsWinPushDouble = false;
            CPushAnimation._instance.SetPushAnimation("TAKESCORE");

            CBottomBTOnclick._instance.m_gGetScoreBT1.SetActive(false);
            CBottomBTOnclick._instance.m_gGetScoreBT.SetActive(true);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gGetScoreBT, false);
        }

        /// <summary>
        /// 游戏记录
        /// </summary>
        /// <param name="packet"></param>
        public void GameRecord(NPacket packet)
        {
            Int64 temp_iRecord;

            packet.BeginRead();
            ////////////////////////////////(记录3数据)CRecordMangerThree////////////////////////////////////////
            //主游戏
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strM_Play_S = (int)temp_iRecord;
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strM_Play_WS = (int)temp_iRecord;
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strM_Play_N = (int)temp_iRecord;
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strM_Play_WN = (int)temp_iRecord;

            //比倍
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strC_Play_S = (int)temp_iRecord;
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strC_Play_WS = (int)temp_iRecord;
            
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.m_strC_Play_WN = (int)temp_iRecord;
            temp_iRecord = packet.GetLong(); 
            CRecordMangerThree._instance.m_strC_Play_LN = (int)temp_iRecord;
            

            //彩金
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.M_strC_Cai_S = (int)temp_iRecord;
            temp_iRecord = packet.GetLong();
            CRecordMangerThree._instance.M_strC_Cai_WN = (int)temp_iRecord;

            ////////////////////////////(记录1,2数据)CRecordMangerOne////////////////////////////////////////////////////////
            Int64 temp_iHeight = 0;
            Int64 temp_iMid = 0;
            Int64 temp_iLow = 0;
            Int64 temp_iAll = 0;
            //5K
            temp_iHeight = packet.GetLong();
            temp_iMid = packet.GetLong();
            temp_iLow = packet.GetLong();
            temp_iRecord = temp_iHeight + temp_iMid + temp_iLow;
            temp_iAll += temp_iRecord;
            CRecordMangerOne._instance.m_str5K_H = (int)temp_iHeight;
            CRecordMangerOne._instance.m_str5K_M = (int)temp_iMid;
            CRecordMangerOne._instance.m_str5K_L = (int)temp_iLow;
            CRecordMangerOne._instance.m_str5K_T = (int)temp_iRecord;

            CRecordMangerTwo._instance.m_str5K_H = (int)temp_iHeight;
            CRecordMangerTwo._instance.m_str5K_L = (int)temp_iLow;
            CRecordMangerTwo._instance.m_str5K_T = (int)temp_iRecord;

            //RS
            temp_iHeight = packet.GetLong();
            temp_iMid = packet.GetLong();
            temp_iLow = packet.GetLong();
            temp_iRecord = temp_iHeight + temp_iMid + temp_iLow;
            temp_iAll += temp_iRecord;
            CRecordMangerOne._instance.m_strRS_H = (int)temp_iHeight;
            CRecordMangerOne._instance.m_strRS_M = (int)temp_iMid;
            CRecordMangerOne._instance.m_strRS_L = (int)temp_iLow;
            CRecordMangerOne._instance.m_strRS_T = (int)temp_iRecord;

            CRecordMangerTwo._instance.m_strRS_H = (int)temp_iHeight;
            CRecordMangerTwo._instance.m_strRS_L = (int)temp_iLow;
            CRecordMangerTwo._instance.m_strRS_T = (int)temp_iRecord;
            //SF
            temp_iHeight = packet.GetLong();
            temp_iMid = packet.GetLong();
            temp_iLow = packet.GetLong();
            temp_iRecord = temp_iHeight + temp_iMid + temp_iLow;
            temp_iAll += temp_iRecord;
            CRecordMangerOne._instance.m_strSF_H = (int)temp_iHeight;
            CRecordMangerOne._instance.m_strSF_M = (int)temp_iMid;
            CRecordMangerOne._instance.m_strSF_L = (int)temp_iLow;
            CRecordMangerOne._instance.m_strSF_T = (int)temp_iRecord;

            CRecordMangerTwo._instance.m_strSF_H = (int)temp_iHeight;
            CRecordMangerTwo._instance.m_strSF_L = (int)temp_iLow;
            CRecordMangerTwo._instance.m_strSF_T = (int)temp_iRecord;
            //4K
            temp_iHeight = packet.GetLong();
            temp_iMid = packet.GetLong();
            temp_iLow = packet.GetLong();
            temp_iRecord = temp_iHeight + temp_iMid + temp_iLow;
            temp_iAll += temp_iRecord;
            CRecordMangerOne._instance.m_str4K_H = (int)temp_iHeight;
            CRecordMangerOne._instance.m_str4K_M = (int)temp_iMid;
            CRecordMangerOne._instance.m_str4K_L = (int)temp_iLow;
            CRecordMangerOne._instance.m_str4K_T = (int)temp_iRecord;

            CRecordMangerTwo._instance.m_str4K_H = (int)temp_iHeight;
            CRecordMangerTwo._instance.m_str4K_L = (int)temp_iLow;
            CRecordMangerTwo._instance.m_str4K_T = (int)temp_iRecord;

            //FH
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_strFH_T = (int)temp_iRecord;
            //FL
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_strFL_T = (int)temp_iRecord;
            //ST
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_strST_T = (int)temp_iRecord;
            //3K
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_str3K_T = (int)temp_iRecord;
            //2P
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_str2P_T = (int)temp_iRecord;
            //1P
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_str1P_T = (int)temp_iRecord;
            //NP
            temp_iRecord = packet.GetLong();
            temp_iAll += temp_iRecord;
            CRecordMangerTwo._instance.m_strNP_T = (int)temp_iRecord;
            //paly total
            CRecordMangerTwo._instance.m_strAll_T = (int)temp_iAll;

            Int64 temp_iAllInOut = 0;
            //in 
            temp_iRecord = packet.GetLong();
            temp_iAllInOut += temp_iRecord;
            CRecordMangerOne._instance.m_strA_In = (int)temp_iRecord;
            //out 
            temp_iRecord = packet.GetLong();
            temp_iAllInOut -= temp_iRecord;
            CRecordMangerOne._instance.m_strA_Out = (int)temp_iRecord;

            CRecordMangerOne._instance.m_strA_Balance = (int)temp_iAllInOut;

            temp_iAllInOut = 0;
            //curret in
            temp_iRecord = packet.GetLong();
            temp_iAllInOut += temp_iRecord;
            CRecordMangerOne._instance.m_strB_In = (int)temp_iRecord;

            //out 
            temp_iRecord = packet.GetLong();
            temp_iAllInOut -= temp_iRecord;
            CRecordMangerOne._instance.m_strB_Out = (int)temp_iRecord;

            CRecordMangerOne._instance.m_strB_Balance = (int)temp_iAllInOut;

        }

    }

}


