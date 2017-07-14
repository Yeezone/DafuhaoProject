using com.QH.QPGame.Services.Data;
using System.Collections;
using UnityEngine;

namespace com.QH.QPGame.Lobby.Surfaces
{
	public class SurfaceGame : Surface
    {
        private int retryTimes = 0;

		void Start ()
		{
            retryTimes = 0;
            RegisterEvent();

            Invoke("ShowEducateTip",2.0f);
        }
		
		void Update () 
        {
		    if (!GameApp.Network.IsConnectionActive(ConnectionID.Game) &&
                !GameApp.PopupMgr.IsTipsShown())
		    {
                GameApp.PopupMgr.ShowTips("网络较差，等待同步游戏数据", 10.0f, null);
		    }
        }

        void OnDestroy()
        {
            retryTimes = 0;
            UnRegisterEvent();
        }

        private void RegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent += HandleNetworkStatusChangeEvent;
            GameApp.GameSrv.SystemMessageEvent += Instance_SystemMessageEvent;
            GameApp.Account.SocketCloseNotifyEvent += OnSendSocketCloseNotify;//异地登陆事件
        }

        private void UnRegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent -= HandleNetworkStatusChangeEvent;
            GameApp.GameSrv.SystemMessageEvent -= Instance_SystemMessageEvent;
            GameApp.Account.SocketCloseNotifyEvent += OnSendSocketCloseNotify;//异地登陆事件
        }

        public void OnSendSocketCloseNotify(int wSubCmdID)
        {
            GameApp.PopupMgr.Confirm("提示", "账号在别处登录，请检查账户安全性!", 
                delegate(MessageBoxResult style)
            {
                GameApp.GetInstance().SwitchAccount();
            }, 5.0f);
        }

        public void NcMiniWindow()
        {
            //最小化窗口
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().ShowMinWindow();
#endif
        }
		
		public void NcMaxWindow()
		{
			//最大化窗口
			#if UNITY_STANDALONE_WIN
				Win32Api.GetInstance().SwitchMaxWindow();
			#endif
		}

	    public void SetupGame()
	    {
            //TODO 设置窗口标题，LOGO，加载跑马灯模块等
	    }

        public string GetRoomTitle()
        {
            var item = GameApp.GameListMgr.FindRoomItem((uint)GameApp.GameData.EnterRoomID);
            string roomName = item != null ? item.Name : "";
            return roomName;
        }

	    public string GetGameVersion()
	    {
            var item = GameApp.GameListMgr.FindRoomItem((uint)GameApp.GameData.EnterRoomID);
            var config = item != null ? GameApp.GameMgr.GetGameConfig((int)item.GameNameID) : null;
            string version = config != null ? config.Version : "";
	        return version;
	    }

	    private void HandleNetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
            if (socket == ConnectionID.Game)
            {
                GameApp.PopupMgr.HideTips();
                if (wError != NetworkManager.Status.Connected)
                {
                    if (retryTimes >= GlobalConst.Game.ReconnectTimes || 
                        !GameApp.GameMgr.IsInGame())
                    {
                        //TODO 提示无法连接
                        StartCoroutine(Quit(false));
                    }
                    else
                    {
                        //TODO 提示重连
                        retryTimes++;
                        StartCoroutine(Reconnect());
                    }
                }
            }
            else
            {
                StartCoroutine(Quit(true));
            }
        }

        private IEnumerator Quit(bool switchAccount)
        {
            float timePassed = 3.0f;
            while (timePassed > 0.0f)
            {
                timePassed -= Time.deltaTime;
                GameApp.PopupMgr.ShowTips("网络断开，无法连接服务器,"+((int)timePassed+1)+"秒后返回");
                yield return new WaitForEndOfFrame();
            }

            if (switchAccount)
            {
                GameApp.GetInstance().SwitchAccount();
            }
            else
            {
                GameApp.GameMgr.DestoryGame(true);
            }
        }

        private IEnumerator Reconnect()
        {
            float timePassed = retryTimes * 2 + 1;
            while (timePassed > 0.0f)
            {
                timePassed -= Time.deltaTime;

                GameApp.PopupMgr.ShowFormatTips("无法连接服务器,{0}秒后重试",
                    (int)timePassed + 1);

                yield return new WaitForEndOfFrame();
            }

            GameApp.PopupMgr.ShowFormatTips("尝试第({0}/{1})次连接服务器",
                retryTimes,
                GlobalConst.Game.ReconnectTimes);

            yield return new WaitForSeconds(1.0f);

            GameApp.GameMgr.ActiveGame();
        }

        void Instance_SystemMessageEvent(ushort wType, string msg)
        {
            if (((wType & (ushort)MsgType.MT_INFO) != 0) || 
                ((wType & (ushort)MsgType.MT_GLOBAL) != 0) ||
                ((wType & (ushort)MsgType.MT_EJECT) != 0))
            {
                if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
                (wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
                {
                    GameApp.PopupMgr.ShowTips(msg, 3.0f, delegate()
                    {
                        GameApp.GameMgr.DestoryGame(true);
                    });
                }
                else if ((wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
                {
                    GameApp.PopupMgr.ShowTips(msg, 3.0f, delegate()
                    {
                        GameApp.GameMgr.DestoryGame(false);
                    });
                }
                else
                {
                    GameApp.PopupMgr.ShowTips(msg, 3.0f, null);
                }
            }
        }

        void ShowEducateTip()
        {
            var item = GameApp.GameListMgr.FindRoomItem((uint)GameApp.GameData.EnterRoomID);
            if (item !=null)
            {
                if (item.IsEducate)
                {
                    GameApp.PopupMgr.ShowTips("您进入的是试玩场，输赢均不结算！", 8.0f, null);
                }
            }
        }
	}
}


