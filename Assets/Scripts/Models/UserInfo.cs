using System;

namespace com.QH.QPGame.Services.Data
{
    [Serializable]
    public class UserInfo
	{
		public UInt32 UserID { get; set;}    
		public UInt32 Phone { get; set;}
		public UInt32 HeadId { get; set; }

        public byte Gender { get; set; }

	    public bool IsBoy
	    {
	        get { return Gender == 0; }
	    }

	    public string Account { get; set;}
        public string NickName { get; set; }
        public string Password { get; set; }

        public Int64 CurMoney { get; set; }
        public Int64 CurBank { get; set; }
        public uint Exp { get; set; }

        public void AddMoney(Int64 amount)
		{
			CurMoney += amount;
		}

        public void SubMoney(Int64 amount)
		{
			CurMoney -= amount;
		}


        public void AddBankMoney(Int64 amount)
		{
			CurBank += amount;
		}

        public void SubBankMoney(Int64 amout)
		{
			CurBank -= amout;
		}

        public void SetSitInfo(ushort table, ushort chair)
        {
            this.LastDeskNO = this.DeskNO;
            this.LastDeskStation = this.DeskStation;

            this.DeskNO = table;
            this.DeskStation = chair;
        }

		public UInt32 MoorMachine { get; set; }

		public UInt32 Vip { get; set; }

        public Int32 CutRoomID { set; get; }

        public ushort DeskNO { get; set; }
		public ushort DeskStation {	get;set;	}

        public ushort LastDeskNO { get; set; }
        public ushort LastDeskStation { get; set; }

        public string UnderWrite { get; set; }

	}
}

