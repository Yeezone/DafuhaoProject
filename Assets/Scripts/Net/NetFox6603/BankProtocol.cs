using com.QH.QPGame.Lobby;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.Services.NetFox
{
    internal class BankProtocol : Singleton<BankProtocol>
	{
        #region 协议解析

        public void ReceiveSubMoneyChange(Packet packet)
		{
			switch(packet.SubCmd)
			{
			case SubCommand.SUB_GP_USER_INSURE_SUCCESS:
			{
				AnalysisCheckSUC(packet);
				break;
			}
			case SubCommand.SUB_GP_USER_INSURE_FAILURE:
			{
				AnalysisCheckFailure(packet);
				break;
			}
			}
		}

		private bool AnalysisCheckSUC (Packet packet)
		{
			CMD_GP_UserInsureSuccess CheckMoney = GameConvert.ByteToStruct<CMD_GP_UserInsureSuccess>(packet.Data);
			if (CheckMoney.dwUserID == GameApp.GameData.UserInfo.UserID)
			{
				GameApp.GameData.UserInfo.CurMoney = CheckMoney.lUserScore;
				GameApp.GameData.UserInfo.CurBank = CheckMoney.lUserInsure;

                GameApp.Account.CallCheckMoneyEvent(true, CheckMoney.szDescribeString);
			}
			return true;
		}

		private bool AnalysisCheckFailure (Packet packet)
		{
			CMD_GP_UserInsureFailure CheckMoney = GameConvert.ByteToStruct<CMD_GP_UserInsureFailure>(packet.Data);
            GameApp.Account.CallCheckMoneyEvent(false, CheckMoney.szDescribeString);

			return true;
		}

		#endregion

    }
}

