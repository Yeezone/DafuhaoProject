using System;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Fishing
{
    //锁定策略
    public enum LockStrategy
    {
        DoubleClick
    }

    public enum FishingModule
    {
        //LockStrategy,
        AutoBuyScore
    }

    [GameDesc(GameIDPrefix = 4000, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4001, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4002, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4003, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4004, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4005, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4006, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4007, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4008, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 4009, Version = "1.0.4")]
    [GameDesc(GameIDPrefix = 415000)]
    public class Fishing : SceneGameAgent
    {
        public static Fishing Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
             
                return instance as Fishing;
            }
        }

        public LockStrategy LockStrategy
        {
            get 
            { 
                return LockStrategy.DoubleClick;
            }
        }
		
		public bool AutoBuyScore
		{
		    get
		    {
		        var index = (int) FishingModule.AutoBuyScore;
		        var status = GameApp.ModuleMgr.GetModuleStatus(ModuleKind.Game, (int) GameID, index);
		        return status == 0 ? false : true;
		    }
		}

        // 检测是否进入捕鱼游戏并且需要旋转屏幕
        public bool MarqueeReverse = true;


        protected override void RegisterEvent()
        {
            GameApp.GameSrv.GameCreatedEvent += Instance_GameCreatedEvent;
            GameApp.GameSrv.UserSitDownEvent += Instance_UserSitDownEvent;
            GameApp.GameSrv.GameMessageEvent += Instance_GameMessageEvent;
  
        }

        void Instance_GameCreatedEvent(uint wRoomId, bool playing)
        {
            //如果是自动坐桌子,需要发送坐下请求到服务器
            if (playing || roomItem.AutoSit)
            {
                EnterGameScene();
            }
        }

        void Instance_UserSitDownEvent(uint uid, ushort desk, ushort chairs)
        {
            //切换到游戏场景
            if (!roomItem.AutoSit && uid == GameApp.GameData.UserInfo.UserID)
            {
                EnterGameScene();
            }
        }

        void Instance_GameMessageEvent(Packet packet)
        {
            MobileFishingProtocol.Instance.HandlePacket(packet);
        }

        protected override void UnRegisterEvent()
        {
            GameApp.GameSrv.GameCreatedEvent -= Instance_GameCreatedEvent;
            GameApp.GameSrv.UserSitDownEvent -= Instance_UserSitDownEvent;
            GameApp.GameSrv.GameMessageEvent -= Instance_GameMessageEvent;
        }

        protected override void EnterGameScene()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().ResizeWindow(1280, 750);
            //切换英文键盘
            Win32Api.ActivateKeyboardLayout(new IntPtr(0x04090409), 0);
#endif
            base.EnterGameScene();
        }

        public void SendMessageToGameCenter(uint wMainCmdID, uint wSubCmdID, int wHandleCode, byte[] wBbyteBuffer)
        {
            GameApp.Network.SendToSvr(ConnectionID.Game, wMainCmdID, wSubCmdID, wHandleCode, wBbyteBuffer);
        }
    }

}