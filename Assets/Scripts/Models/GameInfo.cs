using System;
using System.Collections.Generic;

namespace com.QH.QPGame.Services.Data
{
    [Serializable]
    public class SGameTypeItem
    {
        public uint ID { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class SGameKindItem
    {
        public uint ID { get; set; }
        public uint KindID { get; set; }
        public uint JoinID { get; set; }
        public uint SortID { get; set; }
        public string Name { get; set; }
    }

	[Serializable]
	public class SGameNodeItem
	{
		public uint KindID { get; set; }
		public uint JoinID { get; set; }
		public uint SortID { get; set; }
		public uint NodeID { get; set; }
		public string Name { get; set; }
	} 

    public enum enGameHostType
    {
        Scene,
        Socket,
        IPC,
    }

    [Serializable]
    public class SGameRoomItem
    {
        public uint ID { get; set; }
        
        public uint SortID { get; set; }

        public uint Version { get; set; }

        public string ServiceIP { get; set; }

        public uint ServicePort { get; set; }

        public uint GameRule { get; set; }

        public uint DeskCount { get; set; }

        public uint LessPoint { get; set; }

        public uint BasePoint { get; set; }

        public uint Level { get; set; }

        public uint DeskPeople { get; set; }

        public string Name { get; set; }

        public uint GameNameID { get; set; }

        public uint VirtualUser { get; set; }
        public uint OnlineCnt { get; set; }
        public void UpdateOnlineCnt(uint dwCnt)
        {
            OnlineCnt = dwCnt + VirtualUser;
        }

        public uint LessMoney2Enter { get; set; }

        public enGameHostType HostType { get; set; }

        public bool AutoSit { get; set; }

		public uint FullCount { get; set;}

        public List<SRoomDeskItem> Desks = new List<SRoomDeskItem>();

        public uint NodeID { get; set; }

        public bool IsEducate { get; set; }

    }

    [Serializable]
    public class SRoomDeskItem
    {
        public UInt32 DeskID { get; set; }
        public byte   Status { get; set; }


    }

    [Serializable]
    public class GameList
    {
        public List<SGameTypeItem> TypeList = new List<SGameTypeItem>();
        public List<SGameKindItem> KindList = new List<SGameKindItem>();
        public List<SGameRoomItem> RoomList = new List<SGameRoomItem>();
		public List<SGameNodeItem> NodeList = new List<SGameNodeItem>();

        public void DumpToFile(string file)
        {
            string json = LitJson.JsonMapper.ToJson(this);
            System.IO.File.WriteAllText(file, json);
        }

    }
}
