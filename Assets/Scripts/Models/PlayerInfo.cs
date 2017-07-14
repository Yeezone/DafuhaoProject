using System;

namespace com.QH.QPGame.Services.Data
{
    [Serializable]
    public class PlayerInfo 
	{
		public UInt32 ID{ get; set;}

		public string UserName{ get; set;}
		public string NickName{ get; set;}
		public string Signature{ get; set;}
		public string City{ get; set;}
		public string Province{ get; set;}

		public UInt32 HeadID { get; set;}

		public Int64 BankMoney {	get;set;	}
		public Int64 Money {	get;set;	}

		public ushort DeskNO {	get;set;	}
        public ushort DeskStation { get; set; }
        public byte UserState { get; set; }

        public Byte Gender { get; set; }
        public ushort Level { get; set; }
		public UInt32 DrawCount { get ; set;}
		public UInt32 LostCount { get ; set;}
		public UInt32 WinCount { get ; set;}
		public UInt32 VipLevel { get ; set; }
        public UInt32 Exp { get; set; }

        public byte GameStatus { get; set; }

        public void AddBankMoney(Int64 amount)
        {
            BankMoney += amount;
        }

        public void SubBankMoney(Int64 amout)
        {
            BankMoney -= amout;
        }


        public void AddMoney(Int64 amount)
        {
            Money += amount;
        }

        public void SubMoney(Int64 amount)
        {
            Money -= amount;
        }

	    public void SetState(ushort table, ushort chair, byte state)
	    {
	        this.DeskNO = table;
	        this.DeskStation = chair;
	        this.UserState = state;
	    }

	    public override string ToString()
        {
            return "ID:" + ID + "|Name:" + UserName + "|NickName:" + NickName + "|Money:" + Money + "|BankMoney:" +
                   BankMoney + "|Desk:" + this.DeskNO + "|DeskStation:"+this.DeskStation+"|State:"+this.UserState;
        }

	}

}


