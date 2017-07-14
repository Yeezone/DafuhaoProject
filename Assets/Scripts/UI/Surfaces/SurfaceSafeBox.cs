using System;
using UnityEngine;

namespace com.QH.QPGame.Lobby.Surfaces
{
	
	// 银行
	[System.Serializable]
	public class SaveAndTake
	{
		public UILabel money_label;
		public UILabel safeBoxMoney_label;
		public UIInput money_Input;
		public UILabel userMoney_label;
		public capitalMoney capitalMoney;
		public showWindow moneyAll_btn;

		public GameObject saveBtn;
		public GameObject takeBtn;

		public UILabel log0_label;
		public UILabel log1_label;
	}
	
	// 修改密码
	[System.Serializable]
	public class ChangePass
	{
		public UIButton change_btn;
		public UILabel log0_label;
		public UILabel log1_label;
		public UILabel log2_label;
//		public UIButton ridio0_btn;
//		public UIButton ridio1_btn;
//		public UIButton saveAndtake_btn;
	}

    public class SurfaceSafeBox : Surface
    {
		/// <summary>
		/// 银行
		/// </summary>
		public SaveAndTake saveAndTake;
		/// <summary>
		/// 修改密码
		/// </summary>
		public ChangePass changePass;

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
            GameApp.Account.SafetyBoxEvent += OnSendSafetyBoxResult;//发送解锁保险柜结果事件
            GameApp.Account.PassWDChangeEvent += OnSendChangePassWDResult;//修改密码返回事件
            GameApp.Account.CheckMoneyEvent += OnSendCheckInOrOUtMoneyResult;//发送取钱或存钱结果事件

            HallTransfer.Instance.ncSafetyBoxRequest += NcSafetyBoxRequest;//注册保险柜开启事件
            HallTransfer.Instance.ncSafetyBoxClick += NcSafetyBoxClick;//注册保险柜事件
            HallTransfer.Instance.ncCheckInMoney += NcCheckInMoney;//注册存钱事件
            HallTransfer.Instance.ncCheckOutMoney += NcCheckOutMoney;//注册取钱事件
            HallTransfer.Instance.ncChangePassWD += NcChangePassWD;//注册修改密码事件
        }

        //销毁注册事件
        private void UnRegisterEvent()
        {
                GameApp.Account.SafetyBoxEvent -= OnSendSafetyBoxResult;//发送解锁保险柜结果事件
                GameApp.Account.PassWDChangeEvent -= OnSendChangePassWDResult;//修改密码返回事件
                GameApp.Account.CheckMoneyEvent -= OnSendCheckInOrOUtMoneyResult;//发送取钱或存钱结果事件
            


            if (HallTransfer.Instance != null)
            {
                HallTransfer.Instance.ncSafetyBoxRequest -= NcSafetyBoxRequest;//注册保险柜开启事件
                HallTransfer.Instance.ncSafetyBoxClick -= NcSafetyBoxClick;//注册保险柜事件
                HallTransfer.Instance.ncCheckInMoney -= NcCheckInMoney;//注册存钱事件
                HallTransfer.Instance.ncCheckOutMoney -= NcCheckOutMoney;//注册取钱事件
                HallTransfer.Instance.ncChangePassWD -= NcChangePassWD;//注册修改密码事件
            }
        }

        public void OnSendUserInfo()//玩家数据改变发送
        {
            HallTransfer.MySelfInfo tempInfo = new HallTransfer.MySelfInfo();
            tempInfo.dwUserId = GameApp.GameData.UserInfo.UserID;
            tempInfo.dwNickName = GameApp.GameData.UserInfo.NickName;
            tempInfo.dwMoney = GameApp.GameData.UserInfo.CurMoney;
            tempInfo.dwLockMathine = GameApp.GameData.UserInfo.MoorMachine;
            tempInfo.dwVip = GameApp.GameData.UserInfo.Vip;
            tempInfo.dwRoomID = GameApp.GameData.EnterRoomID;
            tempInfo.dwDeskNo = GameApp.GameData.UserInfo.LastDeskNO;
            tempInfo.dwDeskStation = GameApp.GameData.UserInfo.LastDeskStation;
			tempInfo.dwInsureMoney = GameApp.GameData.UserInfo.CurBank;
            if (GameApp.GameData.UserInfo.HeadId == 0)
            {
                if (!GameApp.GameData.UserInfo.IsBoy)
                {
                    tempInfo.dwHeadID = 1;
                }
                else
                {
                    tempInfo.dwHeadID = 0;
                }
            }
            else
            {
                tempInfo.dwHeadID = GameApp.GameData.UserInfo.HeadId;
            }
            HallTransfer.Instance.cnSetUserInfo(tempInfo);//发送玩家数据
        }

        public void OnSendSafetyBoxResult(int dwCommanResult)
        {
            //发送解锁保险柜结果
            HallTransfer.Instance.cnSafetyBoxResult(dwCommanResult);
            if (dwCommanResult == 0)
            {
                OnSendBankMoney();
                OnSendUserInfo();
            }
        }

        public void OnSendChangePassWDResult(int wHandleCode)
        {
            //区别修改银行密码还是,登陆密码
            //HallTransfer.Instance.cnChangePassWDResult(wHandleCode);
			cnChangePassWDResult(wHandleCode);
        }

        public void OnSendCheckInOrOUtMoneyResult(bool wHandleCode, String Massege)
        {
            //发送取钱或存钱结果
            //HallTransfer.Instance.cnCheckInOrOUtMoneyResult(Massege);
			cnCheckInOrOUtMoneyResult(Massege);
            if (wHandleCode)
            {
                OnSendBankMoney();
                OnSendUserInfo();
            }
        }

        public void OnSendBankMoney()
        {
            Int64 tempMoney = GameApp.GameData.UserInfo.CurMoney;
            Int64 tempBank = GameApp.GameData.UserInfo.CurBank;
            //HallTransfer.Instance.cnUpdataBankMoney(tempMoney, tempBank);
			cnUpdataBankMoney(tempMoney, tempBank);
        }

        public void NcSafetyBoxRequest()
        {
            //点击保险柜判断是否在游戏中
            if (GameApp.GameMgr.IsInGame())
            {
                HallTransfer.Instance.cnSafetyBoxAnswer(0);
            }
            else
            {
                HallTransfer.Instance.cnSafetyBoxAnswer(1);
            }
        }

        public void NcSafetyBoxClick(string dwPassword)
        {
            //			//点击解锁保险柜按钮
            //            BankProtocol.Instance.SendOpenSafetyBoxRequest(dwPassword);
            //发送解锁保险柜结果
            HallTransfer.Instance.cnSafetyBoxResult(0);
            OnSendBankMoney();
            OnSendUserInfo();
        }

        public void NcChangePassWD(int dwChangeType, string dwOldPassword, string dwPassword)
        {
            //点击修改密码按钮
            GameApp.Account.SendChangePassWDRequest(dwChangeType, dwOldPassword, dwPassword);
        }

        public void NcCheckInMoney(Int64 dwMoney)
        {
            Debug.LogWarning(" NcCheckInMoney " + dwMoney);
            //点击存钱按钮
            GameApp.Account.SendCheckInMoneyRequest(dwMoney);
        }

        public void NcCheckOutMoney(Int64 dwMoney, string dwPassword)
        {
            Debug.LogWarning(" NcCheckOutMoney " + dwMoney + "dwPassword:" + dwPassword);
            //点击取钱按钮
            GameApp.Account.SendCheckOutMoneyRequest(dwMoney, dwPassword);
        }

//=================================================================================================================================
		/// <summary>
		/// 接收保险柜数据
		/// </summary>
		public void	cnUpdataBankMoney( Int64 money, Int64 bank)
		{
			Debug.LogWarning("cnUpdataBankMoney====================== " + money + " + " + bank);
			HallTransfer.Instance.uiConfig.bankMoney = bank;
			//存取界面 金钱赋值
			saveAndTake.money_label.text = money.ToString("N0");//HallTransfer.Instance.transMoney (money);
			//存取界面 银行赋值
			saveAndTake.safeBoxMoney_label.text = bank.ToString("N0");//HallTransfer.Instance.transMoney (bank);

			if(!HallTransfer.Instance.uiConfig.MobileEdition) saveAndTake.money_Input.isSelected = true;
			
			//GameObject.Find("userMoney_label").GetComponent<UILabel>().text = money.ToString();

			double maxCount;
			if(money >= bank)
			{
				maxCount = (double)money;
			}else{
				maxCount = (double)bank;
			}
			Debug.LogWarning("maxCount:" + maxCount);

			saveAndTake.capitalMoney.maxCount = maxCount;
			if (saveAndTake.moneyAll_btn!=null) saveAndTake.moneyAll_btn.input_txt[0] = maxCount.ToString();
		}

		/// <summary>
		/// 接收保险柜 取钱 存钱 返回消息
		/// </summary>
		public	void	cnCheckInOrOUtMoneyResult(/*int HandleCode,*/ string msg )
		{
			if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
			{
				HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(false);
			}
			
			Debug.LogWarning("cnCheckInOrOUtMoneyResult====================== "  + " + " + msg);

			saveAndTake.saveBtn.GetComponent<UIButton>().isEnabled = true;
			saveAndTake.takeBtn.GetComponent<UIButton>().isEnabled = true;

			GameObject tempToggle = saveAndTake.saveBtn.GetComponent<safeBox_saveTake_btnClick>().toggleBtn;
			if(tempToggle != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
			if(tempToggle != null) tempToggle.GetComponent<showWindow>().enabled = true;
			GameObject tempToggle1 = saveAndTake.saveBtn.GetComponent<safeBox_saveTake_btnClick>().toggleBtn1;
			if(tempToggle1 != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
			if(tempToggle1 != null) tempToggle.GetComponent<showWindow>().enabled = true;
			GameObject tempToggle2 = saveAndTake.saveBtn.GetComponent<safeBox_saveTake_btnClick>().toggleBtn2;
			if(tempToggle2 != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
			if(tempToggle2 != null) tempToggle.GetComponent<showWindow>().enabled = true;

			saveAndTake.log0_label.text = "";
			saveAndTake.log0_label.text = "";
			//SubCmdID:		2:取钱 3:存钱 9:游戏中取钱 11:游戏中存钱
			//HandleCode: 	1:取钱\存钱失败 2:取钱\存钱成功 3:游戏中存钱 4:密码错误 10:密码错误多次,账号锁定 11:游戏中取钱 12:钱不够
			if (msg != "") {
				HallTransfer.Instance.cnTipsBox (msg);
			}
			
			Invoke("cleanCheckInOrOUtMoneyResultLog",2.0f);
		}
		
		void cleanCheckInOrOUtMoneyResultLog()
		{
			saveAndTake.log0_label.text = "";
		}
		
//=================================================================================================================================
		/// <summary>
		/// 接收保险柜 修改密码 返回消息
		/// </summary>
		public	void	cnChangePassWDResult( int HandleCode )
		{
			if(HallTransfer.Instance.uiConfig.window_SafeBox_mask != null)
			{
				HallTransfer.Instance.uiConfig.window_SafeBox_mask.SetActive(false);
			}
			Debug.LogWarning("cnChangePassWDResult====================== "  + " + " + HandleCode);
			//Transform tempPageChange = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_changePass");
			if(changePass.change_btn!=null) changePass.change_btn.isEnabled = true;
			GameObject tempLog0 = changePass.log0_label.gameObject;
			GameObject tempLog1 = changePass.log1_label.gameObject;
			GameObject tempLog2 = changePass.log2_label.gameObject;
			GameObject change_btn = changePass.change_btn.gameObject;
//			GameObject ridio0_btn = changePass.ridio0_btn.gameObject;
//			GameObject ridio1_btn = changePass.ridio1_btn.gameObject;
//			GameObject toggleBtn = changePass.saveAndtake_btn.gameObject;
			
			change_btn.GetComponent<UIButton>().isEnabled = true;
//			ridio0_btn.GetComponent<UIButton>().isEnabled = true;
//			ridio1_btn.GetComponent<UIButton>().isEnabled = true;
//			ridio0_btn.GetComponent<showWindow>().enabled = true;
//			ridio1_btn.GetComponent<showWindow>().enabled = true;
//			toggleBtn.GetComponent<UIButton>().isEnabled = true;
//			toggleBtn.GetComponent<showWindow>().enabled = true;
			
			tempLog0.GetComponent<UILabel>().text = "";
			tempLog1.GetComponent<UILabel>().text = "";
			tempLog2.GetComponent<UILabel>().text = "";
			//HandleCode:	0:操作成功 1:操作失败 20:锁定或者解锁账号绑定
			if(HandleCode == 0)
			{
				tempLog0.GetComponent<UILabel>().color = Color.green;
				tempLog0.GetComponent<UILabel>().text = "修改成功";
				Invoke("cleanChangePassWDResultLog",2.0f);
				//			cnMsgBox("修 改 成 功 !");
			}else if(HandleCode == 1)
			{
				tempLog0.GetComponent<UILabel>().color = Color.red;
				tempLog0.GetComponent<UILabel>().text = "旧密码输入错误";
				Invoke("cleanChangePassWDResultLog",2.0f);
			}else if(HandleCode == 2)
			{
				tempLog0.GetComponent<UILabel>().color = Color.red;
				tempLog0.GetComponent<UILabel>().text = "账号有误";
				Invoke("cleanChangePassWDResultLog",2.0f);
			}
			else if(HandleCode == 3)
			{
				tempLog0.GetComponent<UILabel>().color = Color.red;
				tempLog0.GetComponent<UILabel>().text = "密码有误";
				Invoke("cleanChangePassWDResultLog",2.0f);
			}
			else if(HandleCode == 20)
			{
				tempLog0.GetComponent<UILabel>().color = Color.red;
				tempLog0.GetComponent<UILabel>().text = "账号已锁定";
				Invoke("cleanChangePassWDResultLog",2.0f);
			}
		}
		void cleanChangePassWDResultLog()
		{
			changePass.log0_label.text = "";
		}
		// 增加一百
		public void addMoney_100()
		{
			Int64 money = 0;
			if(saveAndTake.money_Input.value == "")
			{
				saveAndTake.money_Input.value = "0";
			}
			if (!Int64.TryParse(saveAndTake.money_Input.value, out money))
			{
				Debug.LogWarning("银行 要存储的金额转换失败");
				saveAndTake.log0_label.text = "金币只能为数字";
				Invoke("cleanLogLabel", 2.0f);
				return;
			}
			Int64 temp = money;
			temp += 100;
			saveAndTake.money_Input.value = temp.ToString();
		}
		// 增加一千
		public void addMoney_1000()
		{
			Int64 money = 0;
			if(saveAndTake.money_Input.value == "")
			{
				saveAndTake.money_Input.value = "0";
			}
			if (!Int64.TryParse(saveAndTake.money_Input.value, out money))
			{
				Debug.LogWarning("银行 要存储的金额转换失败");
				saveAndTake.log0_label.text = "金币只能为数字";
				Invoke("cleanLogLabel", 2.0f);
				return;
			}
			Int64 temp = money;
			temp += 1000;
			saveAndTake.money_Input.value = temp.ToString();
		}
		// 增加一万
		public void addMoney_10000()
		{
			Int64 money = 0;
			if(saveAndTake.money_Input.value == "")
			{
				saveAndTake.money_Input.value = "0";
			}
			if (!Int64.TryParse(saveAndTake.money_Input.value, out money))
			{
				Debug.LogWarning("银行 要存储的金额转换失败");
				saveAndTake.log0_label.text = "金币只能为数字";
				Invoke("cleanLogLabel", 2.0f);
				return;
			}
			Int64 temp = money;
			temp += 10000;
			saveAndTake.money_Input.value = temp.ToString();
		}
		// 增加十万
		public void addMoney_100000()
		{
			Int64 money = 0;
			if(saveAndTake.money_Input.value == "")
			{
				saveAndTake.money_Input.value = "0";
			}
			if (!Int64.TryParse(saveAndTake.money_Input.value, out money))
			{
				Debug.LogWarning("银行 要存储的金额转换失败");
				saveAndTake.log0_label.text = "金币只能为数字";
				Invoke("cleanLogLabel", 2.0f);
				return;
			}
			Int64 temp = money;
			temp += 100000;
			saveAndTake.money_Input.value = temp.ToString();
		}

		void cleanLogLabel()
		{
			saveAndTake.log0_label.text = "";
		}
    }

}

