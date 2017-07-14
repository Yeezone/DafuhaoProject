using System;

namespace com.QH.QPGame.Services.Data
{

    [Serializable]
    public class GameRecordItem
	{
        public UInt64 dwEndTime; 				//游戏结束时间
        public UInt32 dwGameKind;				//游戏类型
        public Int64 dwAmount;				//输赢金额
        public UInt32 dwAllCount; 			//总记录
	}

    [Serializable]
    public class LogonRecordItem
	{
        public UInt64 dwTmlogonTime;			//登陆时间               
        public UInt32 dwLogonIP; 				//登陆IP
	}
}

