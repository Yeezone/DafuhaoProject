using System.Runtime.InteropServices;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.Services.NetFox
{
    internal static class ProtoHelper
    {
        #region 通用结构体初始化
        public static UserInfo InitUserInfo(CMD_GP_LogonSuccess logonInfo)
        {
            return new UserInfo()
                {
                    UserID = logonInfo.dwUserID,
                    HeadId = logonInfo.wFaceID,
                    Account = logonInfo.szAccounts,
                    NickName = logonInfo.szNickName,
                    CurMoney = logonInfo.lUserScore,
                    CurBank = logonInfo.lUserInsure,
                    Gender = logonInfo.cbGender,
                    DeskNO = CommonDefine.INVALID_TABLE,
                    DeskStation = CommonDefine.INVALID_CHAIR,
                    MoorMachine = logonInfo.cbMoorMachine,
                    CutRoomID = logonInfo.dwLockServerID
                };
        }

        public static PlayerInfo InitPlayerInfo(tagUserInfoHead userInfo)
        {
            return new PlayerInfo()
            {
                DeskNO = userInfo.wTableID,
                DeskStation = userInfo.wChairID,
                UserState = userInfo.cbUserStatus,

                Money = userInfo.lScore,
                BankMoney = userInfo.lInsure,

                HeadID = userInfo.wFaceID,

                ID = userInfo.dwUserID,
                 
                Gender = userInfo.cbGender,
                DrawCount = userInfo.dwDrawCount,
                LostCount = userInfo.dwLostCount,
                WinCount = userInfo.dwWinCount,
                VipLevel = userInfo.cbMemberOrder
            };
        }

        public static SGameTypeItem InitGameTypeItem(tagGameType data)
        {
			return new SGameTypeItem() { ID = data.TypeID, Name = data.TypeName};
        }

        public static SGameKindItem InitGameKindItem(tagGameKind data)
        {
            return new SGameKindItem()
            {
                ID = data.KindID,
                SortID = data.SortID,
                KindID = data.TypeID,
				JoinID = data.JoinID,
                Name = data.KindName
            };
        }

        public static SGameRoomItem InitGameRoomItem(tagGameServer data)
        {
            return new SGameRoomItem()
            {
                OnlineCnt = data.OnlineCount,
                SortID = data.SortID,
                ID = data.ServerID,
                ServiceIP = data.ServerAddr,
                ServicePort = data.ServerPort,
                Name = data.ServerName,
                GameNameID = data.KindID,
                BasePoint = (uint)data.lServerScore,
                LessMoney2Enter = (uint)data.lMinServerScore,
                HostType = enGameHostType.Scene,
				AutoSit =  false,
				FullCount = data.FullCount,
                NodeID = data.NodeID,
                IsEducate = false
            };
        }

		public static SGameNodeItem InitGameNodeItem(tagGameNode data)
		{
			return new SGameNodeItem()
			{
				KindID = data.KindID,
				JoinID = data.JoinID,
				SortID = data.SortID,
				NodeID = data.NodeID,
				Name = data.NodeName
			};
		}

        public static bool AppendDescDataString(ref ByteBuffer buffer, ushort type, string text)
        {
            byte[] textBytes = System.Text.Encoding.Unicode.GetBytes(text);

            tagDataDescribe desc = new tagDataDescribe();
            desc.wDataDescribe = type;
            desc.wDataSize = (ushort)textBytes.Length;
            byte[] descBytes = GameConvert.StructToByteArray<tagDataDescribe>(desc);
            buffer.PushByteArray(descBytes);
            buffer.PushByteArray(textBytes);

            return true;
        }

        public static ushort ReadDescDataString(ref ByteBuffer buffer, ref string text)
        {
            int dataDescLen = Marshal.SizeOf(typeof(tagDataDescribe));
            if ((buffer.Length - buffer.Position) <= dataDescLen)
            {
                return 0;
            }

            byte[] descData = buffer.PopByteArray(dataDescLen);
            tagDataDescribe desc = GameConvert.ByteToStruct<tagDataDescribe>(descData);
            if (desc.wDataDescribe == CommonDefine.DTP_NULL)
            {
                return 0;
            }

            byte[] data = buffer.PopByteArray(desc.wDataSize);
            text = System.Text.Encoding.Unicode.GetString(data);
            if (text.Length > 0)
            {
                text = text.Substring(0, text.IndexOf((char)0));
            }
            return desc.wDataDescribe;
        }

        #endregion
    }
}
