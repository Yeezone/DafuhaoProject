using System;
using UnityEngine;
using com.QH.QPGame.Services;
using com.QH.QPGame.Services.Utility;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Fishing
{
    public class MobileFishingProtocol : Singleton<MobileFishingProtocol>
	{
		public delegate void OnLoadDataSuccessHandler();
		public event OnLoadDataSuccessHandler LoadDataSuccess;

		public delegate void OnLoadGameDataSuccessHandler();
		public event OnLoadGameDataSuccessHandler onLoadGameDataSuccessEvent;

		public delegate void OnLoadGameStationSuccessHandler(CMD_SC_GF_GAME_STATION wStation);
		public event OnLoadGameStationSuccessHandler onLoadGameStationSuccessEvent;
		
		public delegate void OnUserJoinHandler(CMD_SC_GF_USER_JOIN userJoin);
		public event OnUserJoinHandler onUserJoinEvent;

		public delegate void OnUserLeaveHandler(CMD_SC_GF_USER_LEAVE userLeave);
		public event OnUserLeaveHandler onUserLeaveEvent;

		public delegate void OnReceiveGeneratorNPCHandler(CMD_SC_NPC_GENERATOR npcGenerator);
		public event OnReceiveGeneratorNPCHandler onReceiveGeneratorNPCEvent;
		
		public delegate void OnBuyBulletHandler(CMD_SC_GF_USER_BUY_BULLET buyBullet);
		public event OnBuyBulletHandler onBuyBulletEvent;
	
		public delegate void OnGunFireHandler(CMD_SC_GF_GUN_FIRE gunFire);
		public event OnGunFireHandler onGunFireEvent;
		
		public delegate void OnGunPowerUpHandler(CMD_SC_GF_GUN_POWER_UP gunPower);
		public event OnGunPowerUpHandler onGunPowerUpEvent;

		public delegate void OnNPCKillHandler(CMD_SC_GF_NPC_KILL npcKill);
		public event OnNPCKillHandler onNPCKillEvent;

		public delegate void OnWinFromCacheHandler(CMD_SC_GF_WIN_FROM_CACHE winFromCache);
		public event OnWinFromCacheHandler onWinFromCacheEvent;
		
		public delegate void OnAccountHandler(CMD_SC_BALANCE balance);
		public event OnAccountHandler onBalanceEvent;
		
		public delegate void OnReceiveSystemTimeHandler(CMD_SC_GF_TIME_SYNC timeSync);
		public event OnReceiveSystemTimeHandler onReceiveSystemTimeEvent;
		
		public delegate void OnRobotGunFireNotifyHandler(CMD_SC_GF_ROBOT_GUN_FIRE_NOTIFY notify);
		public event OnRobotGunFireNotifyHandler onRobotGunFireNotifyEvent;
		
		public delegate void OnRobotGunFireHandler(CMD_SC_Robot_GUN_FIRE robotGunFire);
		public event OnRobotGunFireHandler onRobotGunFireEvent;

        public void HandlePacket(Packet packet)
        {
            switch (packet.MainCmd)
            {
                case CMD_Fishing.MDM_GF_INFO:
                    {
                        this.DidReceiveGameInfo((int)packet.SubCmd, (int)packet.CheckCode, packet.Data, packet.DataSize);
                        break;
                    }
            }
        }

		private void DidReceiveGameInfo(int wSubCmdID,int wHandleCode, byte[] wByteBuffer, int wDataSize)
		{
			switch(wSubCmdID)
			{
			case CMD_Fishing.SUB_SC_GF_GAME_STATION:
			{
				DidReceiveGameStation(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_TIME_SYNC:
			{
				DidReceiveSystemTime(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_USER_JOIN:
			{
				DidUserJoin(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_USER_LEAVE:
			{
				DidUserLeave(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_NPC_GENERATOR:
			{
				DidReceiveGeneratorNPC(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_BUY_BULLET:
			{
				DidBuyBullet(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_GUN_FIRE:
			{
				DidGunFire(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_GUN_POWER_UP:
			{
				DidGunPowerUp(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_NPC_KILL:
			{
				DidNPCKill(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_WIN_FROM_CACHE:
			{
				DidWinFromCache(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_BALANCE:
			{
				DidBalance(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_ROBOT_NOTIFY_GUN_FIRE:
			{
				DidRobotGunFireNotify(wHandleCode,wByteBuffer,wDataSize);
				break;
			}
			case CMD_Fishing.SUB_SC_GF_ROBOT_GUN_FIRE:
			{
				DidRobotGunFire(wHandleCode,wByteBuffer,wDataSize);
				break;
			}

			}

		}


		// ============================ S - C ==============================.
		private bool DidReceiveGameStation(int wHandleCode, byte[] wByteBuffer, int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("Game Station Data Error !");
				return false;
			}

			CMD_SC_GF_GAME_STATION gameStationS = GameConvert.ByteToStruct<CMD_SC_GF_GAME_STATION>(wByteBuffer, wByteBuffer.Length);


			if(onLoadGameStationSuccessEvent!=null)
			{
				onLoadGameStationSuccessEvent(gameStationS);
			}



//			Debug.Log("****************************************");
//			Debug.Log("GAME_STATION_JI_TAI==========="+gameStationS.gsJiTaiLeiXing);
//			Debug.Log("GAME_STATION_GAME_RATE========"+gameStationS.gsGameRate);
//			Debug.Log("GAME_STATION_POWER_MIN========"+gameStationS.gsPowerMin);
//			Debug.Log("GAME_STATION_PEAK============="+gameStationS.gsPeak);
//			Debug.Log("GAME_STATION_SERVER_TIME======"+gameStationS.gsServerTime);
//			Debug.Log("GAME_STATION_MAP_ID==========="+gameStationS.gsMapID);
//			Debug.Log("GAME_STATION_SELF_VOUCHER====="+gameStationS.gsSelfVoucher);
//			Debug.Log("****************************************");

			return true;
		}

		private bool DidReceiveSystemTime(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("System Time data Error!!");
				return false;
			}

			CMD_SC_GF_TIME_SYNC systemTime = GameConvert.ByteToStruct<CMD_SC_GF_TIME_SYNC>(wByteBuffer, wByteBuffer.Length);

			if(onReceiveSystemTimeEvent!=null){
				onReceiveSystemTimeEvent(systemTime);
			}


			return true;
		}
		
		private bool DidUserJoin(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_USER_JOIN userJoin = GameConvert.ByteToStruct<CMD_SC_GF_USER_JOIN>(wByteBuffer, wByteBuffer.Length);

			if(onUserJoinEvent!=null){
				
				onUserJoinEvent(userJoin);
			}

			
			return true;
		}

		private bool DidUserLeave(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}

//			public  UInt32              gsServerTime;                   ///时间
//			public  UInt32              gsServerId;	                    ///服务器  ID
//			public  UInt32              gsActorId;                   	///NPC ID
//			public  UInt32              gsNavigId;                      ///导航
//			public  float               gsNavigRot;	                    ///导航角度
//			public  float               gsZ;	                        ///出生坐标
//			public  float               gsY;                            ///出生坐标


//			Debug.Log("****************************************");
//			Debug.Log("NPC_GENERATOR_ServerTime==========="+npcGenerator.gsServerTime);
//			Debug.Log("NPC_GENERATOR_ServerId============="+npcGenerator.gsServerId);
//			Debug.Log("NPC_GENERATOR_ActorId=============="+npcGenerator.gsActorId);
//			Debug.Log("NPC_GENERATOR_NavigId=============="+npcGenerator.gsNavigId);
//			Debug.Log("NPC_GENERATOR_NavigRot============="+npcGenerator.gsNavigRot);
//			Debug.Log("NPC_GENERATOR_Z===================="+npcGenerator.gsZ);
//			Debug.Log("NPC_GENERATOR_Y===================="+npcGenerator.gsY);
//			Debug.Log("****************************************");

			
			CMD_SC_GF_USER_LEAVE userLeave = GameConvert.ByteToStruct<CMD_SC_GF_USER_LEAVE>(wByteBuffer, wByteBuffer.Length);

			if(onUserLeaveEvent!=null){
				
				onUserLeaveEvent(userLeave);
			}


			return true;
		}
		
		private bool DidReceiveGeneratorNPC(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
            int dataLen = wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("Generator NPC data Error!!");
				return false;
			}
			CMD_SC_NPC_GENERATOR npcGenerator = GameConvert.ByteToStruct<CMD_SC_NPC_GENERATOR>(wByteBuffer, wByteBuffer.Length);
			
			//			Debug.Log("****************************************");
			//			Debug.Log("NPC_GENERATOR_ServerTime==========="+npcGenerator.gsServerTime);
			//			Debug.Log("NPC_GENERATOR_ServerId============="+npcGenerator.gsServerId);
			//			Debug.Log("NPC_GENERATOR_ActorId=============="+npcGenerator.gsActorId);
			//			Debug.Log("NPC_GENERATOR_NavigId=============="+npcGenerator.gsNavigId);
			//			Debug.Log("NPC_GENERATOR_NavigRot============="+npcGenerator.gsNavigRot);
			//			Debug.Log("NPC_GENERATOR_Z===================="+npcGenerator.gsZ);
			//			Debug.Log("NPC_GENERATOR_Y===================="+npcGenerator.gsY);
			//			Debug.Log("****************************************");

			if(onReceiveGeneratorNPCEvent!=null){

				onReceiveGeneratorNPCEvent(npcGenerator);
			}

			return true;
		}
		
		private bool DidBuyBullet(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_USER_BUY_BULLET buyBullet = GameConvert.ByteToStruct<CMD_SC_GF_USER_BUY_BULLET>(wByteBuffer, wByteBuffer.Length);

			if(onBuyBulletEvent!=null){
				
				onBuyBulletEvent(buyBullet);
			}

			// Debug.LogWarning(" ==================================================================== 0 S - C DidBuyBullet = "+buyBullet.gsScore);
			
			return true;
		}

		private bool DidGunFire(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_GUN_FIRE gunFire = GameConvert.ByteToStruct<CMD_SC_GF_GUN_FIRE>(wByteBuffer, wByteBuffer.Length);
			
			if(onGunFireEvent!=null){
				
				onGunFireEvent(gunFire);
			}
			
			return true;
		}
		
		private bool DidGunPowerUp(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen = wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_GUN_POWER_UP gunPowerUp = GameConvert.ByteToStruct<CMD_SC_GF_GUN_POWER_UP>(wByteBuffer, wByteBuffer.Length);
			
			if(onGunPowerUpEvent!=null){
				
				onGunPowerUpEvent(gunPowerUp);
			}
			return true;
		}

		private bool DidGunPowerMulti(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			// onGunPowerMultiEvent(xxx);
			return true;
		}
		
		private bool DidNPCKill(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_NPC_KILL npcKill = GameConvert.ByteToStruct<CMD_SC_GF_NPC_KILL>(wByteBuffer, wByteBuffer.Length);

			if(onNPCKillEvent!=null){
				
				onNPCKillEvent(npcKill);
			}

			
			return true;
		}
		
		private bool DidWinFromCache(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_WIN_FROM_CACHE winFromCache = GameConvert.ByteToStruct<CMD_SC_GF_WIN_FROM_CACHE>(wByteBuffer, wByteBuffer.Length);

			if(onWinFromCacheEvent!=null){
				onWinFromCacheEvent(winFromCache);
			}

			
			return true;
		}
		
		private bool DidBalance(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_BALANCE balance = GameConvert.ByteToStruct<CMD_SC_BALANCE>(wByteBuffer, wByteBuffer.Length);

			if(onBalanceEvent!=null){
				
				onBalanceEvent (balance);
			}

			
			return true;
		}
						
		private bool DidRobotGunFireNotify(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_GF_ROBOT_GUN_FIRE_NOTIFY robotGunFireNotify = GameConvert.ByteToStruct<CMD_SC_GF_ROBOT_GUN_FIRE_NOTIFY>(wByteBuffer, wByteBuffer.Length);
			
			if(onRobotGunFireNotifyEvent!=null){
				
				onRobotGunFireNotifyEvent (robotGunFireNotify);
			}
			return true;
		}

		private bool DidRobotGunFire(int wHandleCode,byte[] wByteBuffer,int wDataSize)
		{
			int dataLen=wByteBuffer.Length;
			if(dataLen<wDataSize)
			{
				Debug.LogError("User join in data Error!!");
				return false;
			}
			
			CMD_SC_Robot_GUN_FIRE robotGunFire = GameConvert.ByteToStruct<CMD_SC_Robot_GUN_FIRE>(wByteBuffer, wByteBuffer.Length);
			
			if(onRobotGunFireEvent!=null){
				
				onRobotGunFireEvent (robotGunFire);
			}
			return true;
		}


		


		// ============================ C - S ==============================.
		public void SendUserReady()
		{
			Debug.Log("C_S_SendUserReady");
            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_USER_READY, 0, null);
		}

        public void SendUserSettings()
        {
            CMD_GF_GameOption option = new CMD_GF_GameOption();
            byte[] dataBuffer = GameConvert.StructToByteArray(option);
            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_GAME_FRAME, CMD_Fishing.SUB_GF_GAME_OPTION, 0, dataBuffer);
        }

        public void UserBuyBullet(UInt32 wScore)
		{
			CMD_CS_GF_BUY_BULLET buyScore = new CMD_CS_GF_BUY_BULLET();

			buyScore.gsScore = wScore;

			byte[] dataBuffer = GameConvert.StructToByteArray (buyScore);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_USER_BUY_BULLET, 0, dataBuffer);
		}

		public void C_S_GunPowerUp(UInt32 power)
		{
			CMD_CS_GF_GUN_POWER_UP gunPowerUp = new CMD_CS_GF_GUN_POWER_UP();
			
			gunPowerUp.gsUpScore = power;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (gunPowerUp);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_USER_GUN_POWER_UP, 0, dataBuffer);
		}

		public void C_S_GunPowerMulti(UInt32 power)
		{
		}

		public void C_S_GunFire(float gsRot, float gsZ, float gsY, uint gsServerID, uint gsCostVal, uint gsServerTime)
		{
			CMD_CS_GF_USER_FIRE userFire = new CMD_CS_GF_USER_FIRE();
			
			userFire.gsRot 			= gsRot;
			userFire.gsZ  			= gsZ;
			userFire.gsY			= gsY;
			userFire.gsServerId 	= gsServerID;
			userFire.gsCostVal 		= gsCostVal;
			userFire.gsServerTime 	= gsServerTime;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (userFire);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_USER_GUN_FIRE, 0, dataBuffer);

			//Debug.LogWarning(" =================================================================================  CMD_CS_GF_USER_FIRE ");
		}

		public void C_S_BulletAttack(uint gsServerId, uint gsCost, uint gsPower, uint gsRate)
		{
			CMD_CS_GF_BULLET_ATTACK bulletAttack = new CMD_CS_GF_BULLET_ATTACK();
			
			bulletAttack.gsServerId 	= gsServerId;
			bulletAttack.gsCost 		= gsCost;
			bulletAttack.gsPower		= gsPower;
			bulletAttack.gsRate			= gsRate;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (bulletAttack);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_BULLET_ATTACK, 0, dataBuffer);
		}

		public void C_S_WinFromCache(uint _serverID, uint _bulletPower, uint _multi)
		{
			CMD_CS_GF_GET_SCORE_CACHE cahce = new CMD_CS_GF_GET_SCORE_CACHE();
			
			cahce.gsNPCId 	= _serverID;
			cahce.gsPower	= _bulletPower;
			cahce.gsRate	= _multi;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (cahce);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_WIN_FROM_CACHE, 0, dataBuffer);
		}

		public void C_S_Balance()
		{
            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_BALANCE, 0, null);
		}

		public void C_S_TimeSync()
		{
            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_TIME_SYNC, 0, null);
		}

		public void C_S_RobotGunFire(uint _chair, uint _serverID, float _rot, float _X, float _Y, uint _costValue, uint _serverTime)
		{
			CMD_CS_GF_ROBOT_GUN_FIRE robotGunFire = new CMD_CS_GF_ROBOT_GUN_FIRE();
			robotGunFire.gsRoboChair	= _chair;
			robotGunFire.gsSsrverId		= _serverID;
			robotGunFire.gsRot			= _rot;
			robotGunFire.gsZ			= _X;
			robotGunFire.gsY			= _Y;
			robotGunFire.gsCostVal		= _costValue;
			robotGunFire.gsServerTime	= _serverTime;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (robotGunFire);

            Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO, CMD_Fishing.SUB_CS_GF_ROBOT_GUN_FIRE, 0, dataBuffer);
		}

		public void C_S_RobotBulletAttack(uint _robotChair, uint _serverID, uint _cost, uint _power, uint _multi)
		{
			CMD_CS_GF_ROBOT_BULLET_ATTACK robotBulletAttack = new CMD_CS_GF_ROBOT_BULLET_ATTACK();
			robotBulletAttack.gsRobotChair	= _robotChair;
			robotBulletAttack.gsNpcID		= _serverID;
			robotBulletAttack.gsCost		= _cost;
			robotBulletAttack.gsPower		= _power;
			robotBulletAttack.gsRate		= _multi;
			
			byte[] dataBuffer = GameConvert.StructToByteArray (robotBulletAttack);

			if(Fishing.Instance != null)
			{
				Fishing.Instance.SendMessageToGameCenter(CMD_Fishing.MDM_GF_INFO ,CMD_Fishing.SUB_CS_GF_ROBOT_BULLET_ATTACK, 0, dataBuffer);
			}
		}

    }
}


