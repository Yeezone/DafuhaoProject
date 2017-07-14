using System;
using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;


namespace com.QH.QPGame.Lobby.Surfaces
{

    public class SurfaceLogin : Surface
    {
        private int nConnectCount = 0;
        private bool bShowSocketTips = true;
        private bool bLoginAfterConnected = false;

        private string mServerAddr;
        private uint mServerPort;

        //public LoginTransfer login;

        void Start()
        {
            RegisterEvent();
            SendLogonDataToUi();
            //ConnectToServer();
            StartCoroutine(LoadServerList());
        }

        void OnDestroy()
        {
			UnRegisterEvent();
        }

        private void RegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent += HandleNetworkStatusChangeEvent;//注册网络出错事件
            #region 修改后
            GameApp.Account.LogonSuccessEvent += OnLogonSuccess;//登录成功
            GameApp.Account.LogonErrorEvent += SendToUiLogonError;//登陆出错事件
            GameApp.Account.GameListFinishEvent += OnGameListFinished;

            //没有用到
            //GameApp.Account.RegistFinishedEvent += SendToUiRegistError;//注册出错
            //GameApp.Account.RegistVerityEvent += SendToUiRegistVerityError;//注册账号验证结果
            #endregion
            LoginTransfer.Instance.ncLoginBtnClick += NcLoginBtnClick;//注册登陆事件
			LoginTransfer.Instance.ncGuestBtnClick += NcGuestBtnClick;//注册游客登陆事件
			//LoginTransfer.Instance.ncLoadComplete += OnLoadCompelte;//注册移动端注册账号验证事件
			LoginTransfer.Instance.ncUserRegistSubmit += NcGRegistBtnClick;//注册移动端注册账号事件
			LoginTransfer.Instance.ncUserVerify += NcUserNameVerity;//注册移动端注册账号验证事件
			LoginTransfer.Instance.ncConnectQuest += ConnectToServer;//注册断线重连事件
			LoginTransfer.Instance.ncQuitGame += NcQuitGame;
		}

        private void OnGameListFinished()
        {
            //StartCoroutine(Singleton<GameListLoader>.Instance.LoadAllGameRoom());
        }

		private void UnRegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent -= HandleNetworkStatusChangeEvent;
            #region 修改后
            GameApp.Account.LogonSuccessEvent -= OnLogonSuccess;//登录成功
            GameApp.Account.LogonErrorEvent -= SendToUiLogonError;//登陆出错事件
            GameApp.Account.GameListFinishEvent -= OnGameListFinished;
            //没有用到
            //GameApp.Account.RegistFinishedEvent -= SendToUiRegistError;//注册出错
            //GameApp.Account.RegistVerityEvent -= SendToUiRegistVerityError;//注册账号验证结果
            #endregion

            //LoginTransfer.Instance.ncLoadComplete -= OnLoadCompelte;//注册移动端注册账号验证事件
			LoginTransfer.Instance.ncLoginBtnClick -= NcLoginBtnClick;//注册登陆事件
			LoginTransfer.Instance.ncGuestBtnClick -= NcGuestBtnClick;//注册游客登陆事件
			LoginTransfer.Instance.ncUserRegistSubmit -= NcGRegistBtnClick;//注册移动端注册账号事件
			LoginTransfer.Instance.ncUserVerify -= NcUserNameVerity;//注册移动端注册账号验证事件
			LoginTransfer.Instance.ncConnectQuest -= ConnectToServer;//注册断线重连事件
			LoginTransfer.Instance.ncQuitGame -= NcQuitGame;
		}

        void HandleNetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
            Logger.UI.Log("network status changed. Socket:"+socket+" Status:"+wError);

            GameApp.PopupMgr.HideTips();

            if (!bShowSocketTips)
            {
                return;
            }

            switch (wError)
            {
                case NetworkManager.Status.Connected:
                    {
                        nConnectCount = 0;
                        LoginTransfer.Instance.cnNetworkError((int)wError);

                        if (bLoginAfterConnected)
                        {
                            bLoginAfterConnected = false;
                            NcLoginBtnClick(GameApp.GameData.Account, GameApp.GameData.Password, true);
                        }
                        else
                        {
                            Invoke("Disconnect", GlobalConst.Base.LoginTimeOut);
                        }

                        break;
                    }
                case NetworkManager.Status.Disconnected:
                case NetworkManager.Status.TimeOut:
                    {
                        if (nConnectCount >= GlobalConst.Game.ReconnectTimes)
                        {
                            nConnectCount = 0;
                            GameApp.PopupMgr.Confirm("提示", "与服务器连接失败,请检查网络后重试!",
                               delegate(MessageBoxResult result)
                               {
                                   LoginTransfer.Instance.cnNetworkError(3);
                               },
                           999f);
                        }
                        else
                        {
                            StartCoroutine(Reconnect());
                        }
                       
                        break;
                    }
            }
        }

        private IEnumerator Reconnect()
        {
            float timePassed = nConnectCount * 2 + 1;
            while (timePassed > 0.0f)
            {
                timePassed -= Time.deltaTime;
                GameApp.PopupMgr.ShowFormatTips("无法连接服务器,{0}秒后重试", 
                    (int)(timePassed) + 1);
                yield return new WaitForEndOfFrame();
            }

            GameApp.PopupMgr.ShowFormatTips("尝试第({0}/{1})次连接服务器",
                                            nConnectCount+1,
                                            GlobalConst.Game.ReconnectTimes);

            yield return new WaitForSeconds(1.0f);

            ConnectToServer();
        }

        private IEnumerator LoadServerList()
        {
            if (GameApp.Options.EnableServerList)
            {
                var url = GameHelper.ResolveDownloadUrl(GameApp.Options.ServerListUrl+"/"+GlobalConst.Res.ServerListFileName);
                GameApp.SvrListMgr.LoadServerList(url);
                while (!GameApp.SvrListMgr.Ready && !GameApp.SvrListMgr.Error)
                {
                    GameApp.PopupMgr.ShowTips(GameApp.SvrListMgr.StatusStr);
                    yield return new WaitForFixedUpdate();
                }
            }

            if (GameApp.Options.EnableServerList && !GameApp.SvrListMgr.Error)
            {
                mServerAddr = GameApp.SvrListMgr.PickALowestDelayServer();
                mServerPort = GameApp.GameData.Port;
            }
            else
            {
                mServerAddr = GameApp.Options.ServerHost;
                mServerPort = GameApp.GameData.Port;
            }

            GameApp.PopupMgr.ShowFormatTips("连接服务器{0}", mServerAddr);

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            nConnectCount++;
            bShowSocketTips = true;

            GameApp.PopupMgr.ShowTips("正在连接服务器");
            LoginTransfer.Instance.cnConnectingEvent();

            GameApp.Network.Connect(ConnectionID.Lobby, mServerAddr, mServerPort);
        }

        private void Disconnect()
        {
            if (GameApp.Network.IsConnectionVaild(ConnectionID.Lobby))
            {
                LoginTransfer.Instance.cnNetworkError(4);
                bShowSocketTips = false;
                GameApp.PopupMgr.Confirm("提示", "长时间没有操作，与服务器断开", null, 9999.0f);
            }
        }

        private void OnLogonSuccess()
        {
			//调用Ui函数判断注册界面是否存在
			LoginTransfer.Instance.cnLoginSuccessEvent();

            GameApp.Settings.Save();
            GameApp.SceneMgr.EnterScene(GlobalConst.UI.UI_SCENE_HALL, true);
        }

        public void SendLogonDataToUi()
        {
			bool save = GameApp.GameData.SavePassword;
            string accountStr = GameApp.GameData.Account;
            string passwordStr = GameApp.GameData.Password;
            string versionStr = GameApp.GameData.Version;
            bool showServerConfig = string.Compare("*.*.*.*", GameApp.Options.ServerHost, true) == 0;
            LoginTransfer.Instance.cnUserLoginMsg(accountStr, passwordStr, versionStr, save, showServerConfig);
        }

        public void SendToUiLogonError(int wHandleCode, string str)
        {
            LoginTransfer.Instance.cnLoginMsg(wHandleCode, str);
        }

        public void SendToUiRegistError(int wHandleCode)
        {
            LoginTransfer.Instance.cnRegistSubmitMsg(wHandleCode);
        }

        public void SendToUiRegistVerityError(int wHandleCode)
        {
            LoginTransfer.Instance.cnRegistVerityMsg(wHandleCode);
        }

		public void SetServerInfo(string server, int port, string version)
        {
            GameApp.Options.ServerHost = server;
            GameApp.Options.ServerPort = port;
            GameApp.GameData.Version = version;
            
            GameApp.Network.CloseAllSocket();
            ConnectToServer();
        }

        public void NcMiniWindow()
        {
            //最小化窗口
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().ShowMinWindow();
#endif
        }

        //登陆按钮
        public void NcLoginBtnClick(string userName, string passWord, bool save)
        {
            if (!GameApp.Network.IsConnectionVaild(ConnectionID.Lobby))
            {
                bLoginAfterConnected = true;
                ConnectToServer();
                return;
            }

            GameApp.GameData.SavePassword = save;
            GameApp.GameData.Account = userName;

			StringComparer comparer= StringComparer.OrdinalIgnoreCase;
			if( 0 != comparer.Compare( GameApp.GameData.Password, passWord))
			{
				GameApp.GameData.Password  =  MD5Util.GetMD5Hash( passWord );
			}

            GameApp.Account.SendLoginHallSvr(
                GameApp.GameData.Account,
                GameApp.GameData.Password,
                GameApp.GameData.MAC
                );

        }

        public void NcGuestBtnClick()
        {
            if (!string.IsNullOrEmpty(GameApp.GameData.Account) &&
                !string.IsNullOrEmpty(GameApp.GameData.Password))
            {
                GameApp.Account.SendLoginHallSvr(
                    GameApp.GameData.Account,
                    GameApp.GameData.Password,
                    GameApp.GameData.MAC
                    );
            }
            else
            {
                GameApp.Account.SendGuestRegistQuickMessage();
            }
        }

        public void NcGRegistBtnClick(LoginTransfer.UserRegistMsg userRegist)
        {
            if (!GameApp.Network.IsConnectionVaild(ConnectionID.Lobby))
            {
                LoginTransfer.Instance.cnRegistSubmitMsg(14);
                ConnectToServer();
                return;
            }

            GameApp.Account.SendUserRegist(userRegist.userID,
                userRegist.Gender,
                userRegist.userPassword,
                userRegist.Introducer
                );
        }

        public void NcUserNameVerity(string name)
        {
            GameApp.Account.SendRegistVerify(name);
        }

		public void  NcQuitGame()
		{
			GameApp.GetInstance().Quit();
		}
    }
}

