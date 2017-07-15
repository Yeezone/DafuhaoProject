using UnityEngine;
using System.Collections;
using com.QH.QPGame.Services.Data;
using System.IO;
using Shared;
using System;
using com.QH.QPGame.Services.Utility;
using System.Runtime.InteropServices;


namespace com.QH.QPGame.JSYS
{
    public class CUIGame : MonoBehaviour
    {

        public void Init()
        {
            CGameEngine.Instance.SetTableEventHandle(new TableEventHandle(OnTableUserEvent));
            CGameEngine.Instance.AddPacketHandle(MainCmd.MDM_GF_FRAME, new PacketHandle(OnFrameResp));
            CGameEngine.Instance.AddPacketHandle(MainCmd.MDM_GF_GAME, new PacketHandle(OnGameResp));

            if (!CGameEngine.Instance.AutoSit || CGameEngine.Instance.IsPlaying())
            {
                CGameEngine.Instance.SendUserSetting();
            }
            else
            {
                CGameEngine.Instance.SendUserSitdown();
            }
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

                        for (int i = 0; i < 8; i++)
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
                        for (int i = 0; i < 8; i++)
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
                        for (int i = 0; i < 8; i++)
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
        void OnFrameResp(ushort protocol, ushort subcmd, NPacket packet)
        {
            if (CUIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;
            //if (_bReqQuit == true) return;

            switch (subcmd)
            {
                //游戏状态设置
                case SubCmd.SUB_GF_OPTION:
                    {
                        packet.BeginRead();
                        CGameEngine.Instance.MySelf.GameStatus = packet.GetByte();
                        break;
                    }
                case SubCmd.SUB_GF_SCENE:
                    {
                        OnGameSceneResp(CGameEngine.Instance.MySelf.GameStatus, packet);
                        break;
                    }
            }
        }

        //游戏消息入口
        void OnGameResp(ushort protocol, ushort subcmd, NPacket packet)
        {

            if (CUIManger.Instance.SceneType != enSceneType.SCENE_GAME) return;

            //if (_bReqQuit == true) return;
            //游戏状态
            switch (subcmd)
            {
                //用户下注返回
                case SubCmd.SUB_S_PLAY_BET:
                    {
                        UserBetReturn(packet);
                        break;
                    }
                //游戏结果
                case SubCmd.SUB_S_GAME_END:
                    {
                        GameResualtReturn(packet);
                        break;
                    }
                //开始游戏
                case SubCmd.SUB_S_GAME_START:
                    {
                        GameStart(packet);
                        break;
                    }
                //清除下注
                case SubCmd.SUB_S_BET_CLEAR:
                    {
                        ClearBET(packet);
                        break;
                    }
                //续压失败
                case SubCmd.SUB_S_CONTINUE_BET_DEFEAT:
                    {
                        ContinueBETDefeat();
                        break;
                    }
                //彩金数据
                case SubCmd.SUB_S_SEND_PRIZE_DATA:
                    {
                        packet.BeginRead();
                        CBETManger._instance.m_cBetTop.m_lPrzeNum = packet.GetLong();
                        break;
                    }
                //彩金获奖
                case SubCmd.SUB_S_SEND_PRIZE_REWARD:
                    {
                        packet.BeginRead();
                        //去掉包头
                        byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
                        int len = Marshal.SizeOf(typeof(CMD_S_SendPrizePoolReward));
                        packet.GetBytes(ref _buffer, len);
                        CMD_S_SendPrizePoolReward _SendPrizePoolReward = GameConvert.ByteToStruct<CMD_S_SendPrizePoolReward>(_buffer);
                        CBETManger._instance.m_cBetTop.m_bIsPrize = true;
                        CBETManger._instance.m_cBetTop.m_lPrize = _SendPrizePoolReward.lRewardGold;
                        CBETManger._instance.m_cBetTop.m_iChairID = (ushort)_SendPrizePoolReward.wChairID;
                        CBETManger._instance.m_cBetTop.m_iMyChairID = CGameEngine.Instance.MySelf.DeskStation;
                        Debug.Log(_SendPrizePoolReward.wChairID.ToString());
                        CBETManger._instance.m_cBetTop.m_strNickName = CGameEngine.Instance.GetTableUserItem((ushort)_SendPrizePoolReward.wChairID).NickName;
                        break;
                    }
                case SubCmd.SUB_S_PLAY_BET_DEAFEAT:
                    {
                        CBETManger._instance.AddTiShi("下注失败");
                        break;
                    }
                case SubCmd.SUB_S_GAME_END_REVENUE:
                    {

                        packet.BeginRead();
                        Int64 temprevenue = packet.GetLong();
                        CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum -= temprevenue;
                        break;
                    }
                case SubCmd.SUB_S_GAME_END_IDI:
                    {
                        
                        break;
                    }
            }
        }

        //游戏场景消息处理函数
        void OnGameSceneResp(byte bGameStatus, NPacket packet)
        {
            //Debuger.LogError(bGameStatus);
            switch (bGameStatus)
            {
                case (byte)SubCmd.GAME_SCENE_FREE:
                    {
                        CMD_S_StatusFree _mdata = GameConvert.ByteToStruct<CMD_S_StatusFree>(packet.Buff);

                        
                        CBETManger._instance.m_cBetTop.m_cMulitly.m_iNum = _mdata.lCellScore;
                   
                        CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum = CGameEngine.Instance.MySelf.Money / _mdata.lCellScore;

                        for (int i = 0; i < CBETManger._instance.m_cChipManger.m_lChipList.Count; i++)
                        {
                            CBETManger._instance.m_cChipManger.m_lChipList[i] = _mdata.lBetNum[i];
                        }
                        CBETManger._instance.SetCurrentChip(0);

                        CBETManger._instance.RectBet();
                        //设置计时器
                        CBETManger._instance.m_cTimer.SetGameTimer(_mdata.cbTimeLeave, null);
                        //游戏记录
                        for (int i = 0; i < _mdata.nTurnTableRecord.Length; i++)
                            CRecordManger._instance.AddRecord(_mdata.nTurnTableRecord[i] + 1);

                        string str = "BETBg" + UnityEngine.Random.Range(1, 3).ToString();
                        CMusicManger_JSYS._instance.PlayBgSound(str);

                        CBETManger._instance.RectBet();
                        CBETManger._instance.m_bIsBetTime = false;
                        CUIManger.Instance.m_cGamePlayState.ShowWindow();
                        StartCoroutine(WaitSetm_bIsRandomPraize());
                        break;
                    }
                case (byte)SubCmd.GAME_SCENE_BET:
                    {
                        packet.BeginRead();

                        //去掉包头
                        byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
                        int len = Marshal.SizeOf(typeof(CMD_S_StatusPlay));
                        packet.GetBytes(ref _buffer, len);

                         CMD_S_StatusPlay _StatusPlay = GameConvert.ByteToStruct<CMD_S_StatusPlay>(_buffer);
                         CUIManger.Instance.m_cStatusPlay.cbTimeLeave = _StatusPlay.cbTimeLeave;

                         for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.lAllBet.Length;i++ )
                             CUIManger.Instance.m_cStatusPlay.lAllBet[i] = _StatusPlay.lAllBet[i];

                         CUIManger.Instance.m_cStatusPlay.lAreaLimitScore = _StatusPlay.lAreaLimitScore;

                         for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.lBetNum.Length; i++)
                         CUIManger.Instance.m_cStatusPlay.lBetNum[i] = _StatusPlay.lBetNum[i];

                         CUIManger.Instance.m_cStatusPlay.lCellScore = _StatusPlay.lCellScore;

                         for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.lPlayBet.Length; i++)
                         CUIManger.Instance.m_cStatusPlay.lPlayBet[i] = _StatusPlay.lPlayBet[i];

                         CUIManger.Instance.m_cStatusPlay.lPlayChip = _StatusPlay.lPlayChip;
                         CUIManger.Instance.m_cStatusPlay.lPlayLimitScore = _StatusPlay.lPlayLimitScore;
                         CUIManger.Instance.m_cStatusPlay.lPlayScore = _StatusPlay.lPlayScore;
                         CUIManger.Instance.m_cStatusPlay.lStorageStart = _StatusPlay.lStorageStart;

                         for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.nAnimalMultiple.Length; i++)
                         CUIManger.Instance.m_cStatusPlay.nAnimalMultiple[i] = _StatusPlay.nAnimalMultiple[i];

                         for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.nTurnTableRecord.Length; i++)
                         CUIManger.Instance.m_cStatusPlay.nTurnTableRecord[i] = _StatusPlay.nTurnTableRecord[i];

                        string str = "BETBg" + UnityEngine.Random.Range(1, 3).ToString();
                        CMusicManger_JSYS._instance.PlayBgSound(str);
                        CBETManger._instance.m_cBetTop.m_cMulitly.m_iNum = CUIManger.Instance.m_cStatusPlay.lCellScore;

                        CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum = CGameEngine.Instance.MySelf.Money / CUIManger.Instance.m_cStatusPlay.lCellScore;

                        for (int i = 0; i < CBETManger._instance.m_cChipManger.m_lChipList.Count; i++)
                        {
                            CBETManger._instance.m_cChipManger.m_lChipList[i] = CUIManger.Instance.m_cStatusPlay.lBetNum[i];
                        }
                        CBETManger._instance.SetCurrentChip(0);
                        
                        CBETManger._instance.RectBet();
                        for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
                        {
                            CBETManger._instance.m_listBetItem[i].m_lAllBet = CUIManger.Instance.m_cStatusPlay.lAllBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                            CBETManger._instance.m_listBetItem[i].m_lMyBet = CUIManger.Instance.m_cStatusPlay.lPlayBet[CBETManger._instance.m_listBetItem[i].m_iBetID];

                            if (CBETManger._instance.m_listBetItem[i].m_lMyBet >0)
                            {
                                 CBackGroundManger._instance.m_bIsGameStatus = true;
                            }
                        }
                        //设置计时器
                        CBETManger._instance.m_cTimer.SetGameTimer(CUIManger.Instance.m_cStatusPlay.cbTimeLeave, null);
                        for (int i = 0; i < CUIManger.Instance.m_cStatusPlay.nTurnTableRecord.Length; i++)
                            CRecordManger._instance.AddRecord(CUIManger.Instance.m_cStatusPlay.nTurnTableRecord[i] + 1);
                        //游戏倍数
                        for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
                        {
                            CBETManger._instance.m_listBetItem[i].m_cMulitTimes.m_iNum =
                            CUIManger.Instance.m_cStatusPlay.nAnimalMultiple[CBETManger._instance.m_listBetItem[i].m_iBetID];
                        }
                        CBETManger._instance.m_cMoveBet.PackUp_Onclick();
                        StartCoroutine(WaitSetm_bIsRandomPraize());
                        break;
                    }
                case (byte)SubCmd.GAME_SCENE_END:
                    {
                        packet.BeginRead();

                        //去掉包头
                        byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
                        int len = Marshal.SizeOf(typeof(CMD_S_StatusEnd));
                        packet.GetBytes(ref _buffer, len);

                        CMD_S_StatusEnd _StatusEnd = GameConvert.ByteToStruct<CMD_S_StatusEnd>(_buffer);

                        CBETManger._instance.m_cBetTop.m_cMulitly.m_iNum = _StatusEnd.lCellScore;

                        CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum = CGameEngine.Instance.MySelf.Money / _StatusEnd.lCellScore;
                        //设置筹码 
                        for (int i = 0; i < CBETManger._instance.m_cChipManger.m_lChipList.Count; i++)
                        {
                            CBETManger._instance.m_cChipManger.m_lChipList[i] = _StatusEnd.lBetNum[i];
                        }
                        CBETManger._instance.SetCurrentChip(0);
                        CBackGroundManger._instance.m_bIsGameStatus = false;
                        CBETManger._instance.RectBet();
                        //设置计时器
                        CBETManger._instance.m_cTimer.SetGameTimer(_StatusEnd.cbTimeLeave, null);
                        //游戏记录
                        for (int i = 0; i < _StatusEnd.nTurnTableRecord.Length; i++)
                            CRecordManger._instance.AddRecord(_StatusEnd.nTurnTableRecord[i] + 1);
                        for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
                        {
                            CBETManger._instance.m_listBetItem[i].m_lAllBet = _StatusEnd.lAllBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                            CBETManger._instance.m_listBetItem[i].m_lMyBet = _StatusEnd.lPlayBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                        }
                        //游戏倍数
                        for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
                        {
                            CBETManger._instance.m_listBetItem[i].m_cMulitTimes.m_iNum = _StatusEnd.nAnimalMultiple[CBETManger._instance.m_listBetItem[i].m_iBetID];
                        }
                        CBETManger._instance.m_bIsBetTime = false;
                        foreach(Transform child in CUIManger.Instance.m_cGamePlayState.transform )
                        {
                            if(child != null)
                            {
                                Debug.Log("child999:"+child.name.ToString()+"---pos:" +child.localPosition.ToString()+"---scale:"+child.lossyScale.ToString());
                            }
                        }
                       CUIManger.Instance.m_cGamePlayState.ShowWindow();
                       StartCoroutine(WaitSetm_bIsRandomPraize());
                        break;
                    }
            }
        }
        IEnumerator WaitSetm_bIsRandomPraize()
        {
            yield return new WaitForSeconds(1.0f);
            CBETManger._instance.PrizeGoldControl();
        }
        /// <summary>
        /// 下注返回
        /// </summary>
        /// <param name="packet"></param>
        public void UserBetReturn(NPacket packet)
        {
            packet.BeginRead();

            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_PlayBet));
            packet.GetBytes(ref _buffer, len);

            CMD_S_PlayBet _cPlayBet;
            _cPlayBet = GameConvert.ByteToStruct<CMD_S_PlayBet>(_buffer);

            if (_cPlayBet.wChairID == CGameEngine.Instance.MySelf.DeskStation)
            {
                CBETManger._instance.BetReturnSet(_cPlayBet.nAnimalIndex, _cPlayBet.lBetChip, _cPlayBet.lBetChip);
                CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum -= _cPlayBet.lBetChip;
                if (CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum < 0) CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum = 0;
                CBackGroundManger._instance.m_bIsGameStatus = true;
            }
            else
                CBETManger._instance.BetReturnSet(_cPlayBet.nAnimalIndex, _cPlayBet.lBetChip, 0);


        }
        /// <summary>
        /// 游戏结果返回
        /// </summary>
        /// <param name="packet"></param>
        public void GameResualtReturn(NPacket packet)
        {
            packet.BeginRead();

            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_GameEnd));
            packet.GetBytes(ref _buffer, len);

            for (int i = 0; i < CUIManger.Instance.m_cGameResualt.lPlayWin.Length; i++)
            {
                CUIManger.Instance.m_cGameResualt.lPlayWin[i] = 0;
            }
            CMD_S_GameEnd _sGameEnd = GameConvert.ByteToStruct<CMD_S_GameEnd>(_buffer);
            for (int i = 0; i < CUIManger.Instance.m_cGameResualt.bTurnTwoTime.Length; i++)
            CUIManger.Instance.m_cGameResualt.bTurnTwoTime[i] = _sGameEnd.bTurnTwoTime[i];

            CUIManger.Instance.m_cGameResualt.cbTimeLeave = _sGameEnd.cbTimeLeave;
            CUIManger.Instance.m_cGameResualt.lPlayPrizes = _sGameEnd.lPlayPrizes;
            CUIManger.Instance.m_cGameResualt.lPlayShowPrizes = _sGameEnd.lPlayShowPrizes;

            for (int i = 0; i < CUIManger.Instance.m_cGameResualt.lPlayWin.Length; i++)
            {
                CUIManger.Instance.m_cGameResualt.lPlayWin[i] = _sGameEnd.lPlayWin[i];
                
            }

            CUIManger.Instance.m_cGameResualt.nPrizesMultiple = _sGameEnd.nPrizesMultiple;

            for (int i = 0; i < CUIManger.Instance.m_cGameResualt.nTurnTableTarget.Length;i++ )
                CUIManger.Instance.m_cGameResualt.nTurnTableTarget[i] = _sGameEnd.nTurnTableTarget[i];

            CTurnManger._instance.OpenAniamation(CUIManger.Instance.m_cGameResualt.nTurnTableTarget[0]);
            CBETManger._instance.m_cMoveBet.PackDown_Onclick();
            CMusicManger_JSYS._instance.PlayBgSound("Turn");
            CTurnManger._instance.m_lWinScore = CUIManger.Instance.m_cGameResualt.lPlayWin[0];
          
            //转两圈
            if (CUIManger.Instance.m_cGameResualt.bTurnTwoTime[0] == 1)
            {
                StartCoroutine(OpenSharkWaitTimeSecond(25));
            }
           
            CBETManger._instance.m_cTimer.transform.parent.gameObject.SetActive(false);
            CBETManger._instance.m_bIsBetTime = false;
            CBETManger._instance.m_cBetTop.m_bIsRandomPraize = true;

        }
        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="packet"></param>
        public void GameStart(NPacket packet)
        {
            packet.BeginRead();
            CBETManger._instance.RectBet();

            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_GameStart));
            packet.GetBytes(ref _buffer, len);

            CMD_S_GameStart _cGameResualt;
            _cGameResualt = GameConvert.ByteToStruct<CMD_S_GameStart>(_buffer);

            CBETManger._instance.m_cMoveBet.PackUp_Onclick();
            CTurnManger._instance.m_CenterAnimal.HideWindow();
            CBETManger._instance.m_bIsBetTime = true;
            CBETManger._instance.m_cTimer.transform.parent.gameObject.SetActive(true);
            CBETManger._instance.m_cTimer.SetGameTimer(_cGameResualt.cbTimeLeave, null);
            CBackGroundManger._instance.SetBackGround(0);
            //CTurnManger._instance.m_lTurnList[CTurnManger._instance.m_iCurrentIndex].m_gAnimation.SetActive(false);
            CTurnManger._instance.m_lTurnList[CTurnManger._instance.m_iCurrentIndex].m_gTexture.SetActive(true);
            CTurnManger._instance.m_lTurnList[CTurnManger._instance.m_iCurrentIndex].m_gAnimationBK.GetComponent<CAnimation>().Stop();
            CUIManger.Instance.m_cGamePlayState.HideWindow();
            for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
            {
                CBETManger._instance.m_listBetItem[i].m_cMulitTimes.m_iNum = _cGameResualt.nAnimalMultiple[CBETManger._instance.m_listBetItem[i].m_iBetID];
            }

            string str = "BETBg" + UnityEngine.Random.Range(1, 3).ToString();
            CMusicManger_JSYS._instance.PlayBgSound(str);
            Debug.Log("999游戏开始：" + CBETManger._instance.m_cBetTop.m_cGetscore.m_iNum.ToString());
            CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum += CBETManger._instance.m_cBetTop.m_cGetscore.m_iNum;
            CBETManger._instance.m_cBetTop.m_cGetscore.m_iNum = 0;

        }
        /// <summary>
        /// 清除下注
        /// </summary>
        /// <param name="packet"></param>
        public void ClearBET(NPacket packet)
        {
            packet.BeginRead();

            //去掉包头
            byte[] _buffer = new byte[SocketSetting.SOCKET_PACKAGE];
            int len = Marshal.SizeOf(typeof(CMD_S_BetClear));
            packet.GetBytes(ref _buffer, len);

            CMD_S_BetClear _BetClear;
            _BetClear = GameConvert.ByteToStruct<CMD_S_BetClear>(_buffer);
            for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
            {
                CBETManger._instance.m_listBetItem[i].m_lAllBet -= _BetClear.lPlayBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                if (_BetClear.wChairID == CGameEngine.Instance.MySelf.DeskStation)
                {
                    CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum += _BetClear.lPlayBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                    CBETManger._instance.m_listBetItem[i].m_lMyBet -= _BetClear.lPlayBet[CBETManger._instance.m_listBetItem[i].m_iBetID];
                }
            }

        }
        /// <summary>
        /// 续压失败
        /// </summary>
        public void ContinueBETDefeat()
        {
            CBETManger._instance.AddTiShi("您的金币不足，不能进行续押！");
        }
        /// <summary>
        /// 启动第二圈
        /// </summary>
        /// <param name="_ftime"></param>
        /// <returns></returns>
        IEnumerator OpenSharkWaitTimeSecond(float _ftime)
        {
            yield return new WaitForSeconds(_ftime);
            CTurnManger._instance.OpenAniamation(CUIManger.Instance.m_cGameResualt.nTurnTableTarget[1]);
            CBETManger._instance.m_cMoveBet.PackDown_Onclick();
            CMusicManger_JSYS._instance.PlayBgSound("Turn");
            CTurnManger._instance.HideCenterAnimation();
            CTurnManger._instance.m_cFree.ShowWindow();
            CTurnManger._instance.m_lWinScore = CUIManger.Instance.m_cGameResualt.lPlayWin[1];
            for (int i = 0; i < CBETManger._instance.m_listBetItem.Count; i++)
            {
                CBETManger._instance.m_listBetItem[i].SetNormalColor();
            }
        }

        
    }
}
