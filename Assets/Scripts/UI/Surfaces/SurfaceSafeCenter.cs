using UnityEngine;
using System;
using System.Collections.Generic;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.Lobby.Surfaces
{
    public class SurfaceSafeCenter : Surface
    {
        // Use this for initialization
        void Start()
        {
            RegisterEvent();
        }

        void OnDestroy()
        {
            UnRegisterEvent();
        }

        //注册事件
        private void RegisterEvent()
        {
            GameApp.Account.LogonRecordEvent += OnSendLogonRecord;//登陆日志事件
            GameApp.Account.GameRecordEvent += OnSendGameRecord;//游戏日志事件
            HallTransfer.Instance.ncSafetyCenter += NcSafetyCenter;//注册安全中心事件
            HallTransfer.Instance.ncGameRecord += NcGameRecord;//注册游戏记录事件
        }

        //销毁注册事件
        private void UnRegisterEvent()
        {
                GameApp.Account.LogonRecordEvent -= OnSendLogonRecord;//登陆日志事件
                GameApp.Account.GameRecordEvent -= OnSendGameRecord;//游戏日志事件
            if (HallTransfer.Instance != null)
            {
                HallTransfer.Instance.ncSafetyCenter -= NcSafetyCenter;//注册安全中心事件
                HallTransfer.Instance.ncGameRecord -= NcGameRecord;//注册游戏记录事件
            }
        }

        public void NcSafetyCenter()
        {
            //点击安全中心
            GameApp.Account.SendLogonRecordRequest();
        }


        public void NcGameRecord(HallTransfer.GameRecordRequest gameRecord)
        {
            //点击游戏记录
            GameApp.Account.SendGameRecordRequest(
                gameRecord.dwGameKind, 
                gameRecord.dwPage, 
                gameRecord.dwPageSize, 
                gameRecord.dwTime);
        }

        public void OnSendGameRecord(List<GameRecordItem> gameRecordList)
        {
            List<HallTransfer.GameRecord> tempGameRecordList = new List<HallTransfer.GameRecord>();
            foreach (var temp in gameRecordList)
            {
                HallTransfer.GameRecord tempGameRecord = new HallTransfer.GameRecord();
                tempGameRecord.dwEndTime = temp.dwEndTime;
                tempGameRecord.dwGameKind = temp.dwGameKind;
                tempGameRecord.dwAmount = (Int64)temp.dwAmount;
                tempGameRecord.dwAllCount = temp.dwAllCount;
                tempGameRecordList.Add(tempGameRecord);
            }
            //发送游戏日志
            HallTransfer.Instance.cnGameRecord(tempGameRecordList);
        }

        public void OnSendLogonRecord(List<LogonRecordItem> logonRecordList)
        {
            List<HallTransfer.LogonRecord> tempLogonRecordList = new List<HallTransfer.LogonRecord>();
            foreach (var temp in logonRecordList)
            {
                HallTransfer.LogonRecord tempLogonRecord = new HallTransfer.LogonRecord();
                tempLogonRecord.dwTmlogonTime = temp.dwTmlogonTime;
                tempLogonRecord.dwLogonIP = temp.dwLogonIP;

                tempLogonRecordList.Add(tempLogonRecord);
            }
            //发送登陆日志
            HallTransfer.Instance.cnLogonRecord(tempLogonRecordList);
        }
    }

}

