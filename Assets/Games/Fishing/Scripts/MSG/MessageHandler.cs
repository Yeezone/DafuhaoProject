using System.IO;
using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class MessageHandler : MonoBehaviour 
	{
		public static		MessageHandler Instance;
		public TextAsset	DataRes;
		public TextAsset	GameRes;

        private IdMapInfo IdMap;
        private NavigationInfo NavigationMap;

        public void LoadNavigationMap(uint gameID)
        {
            //TODO 缓存
            if (IdMap == null)
            {
                IdMap = new IdMapInfo();

                using (Stream st = new MemoryStream(DataRes.bytes))
                {
                    IdMap.LoadFromStream(st);
                }
            }

            if (NavigationMap == null)
            {
                NavigationMap = new NavigationInfo();
                using (Stream st = new MemoryStream(GameRes.bytes))
                {
                    NavigationMap.LoadFromStream(st);
                }
            }
        }

		void Awake()
		{
			Instance = this;

            LoadNavigationMap(Fishing.Instance.GameID);

            //SendUserSettings()服务器框架设置玩家准备、更改玩家参数，SendUserReady()捕鱼游戏发场景函数
            MobileFishingProtocol.GetInstance().SendUserSettings();
            MobileFishingProtocol.GetInstance().SendUserReady();

			MobileFishingProtocol.GetInstance().onLoadGameStationSuccessEvent 	+= S_C_GameStation;
			MobileFishingProtocol.GetInstance().onUserJoinEvent 				+= S_C_UserJoin;
			MobileFishingProtocol.GetInstance().onReceiveSystemTimeEvent 		+= S_C_TimeSync;
			MobileFishingProtocol.GetInstance().onUserLeaveEvent 				+= S_C_UserLeave;
			MobileFishingProtocol.GetInstance().onBuyBulletEvent 				+= S_C_BuyBullet;
			MobileFishingProtocol.GetInstance().onGunFireEvent 					+= S_C_GunFire;
			MobileFishingProtocol.GetInstance().onGunPowerUpEvent 				+= S_C_GunPowerUp;

			//暂时还没使用.
			// MobileFishingProtocol.GetInstance().onGunMultiEvent 				+= S_C_GunPowerMulti;
			MobileFishingProtocol.GetInstance().onReceiveGeneratorNPCEvent 		+= S_C_NPC_Generator;
			MobileFishingProtocol.GetInstance().onNPCKillEvent			 		+= S_C_NPC_Kill;
			MobileFishingProtocol.GetInstance().onWinFromCacheEvent		 		+= S_C_WinFromCache;
			MobileFishingProtocol.GetInstance().onBalanceEvent			 		+= S_C_Balance;
			MobileFishingProtocol.GetInstance().onRobotGunFireNotifyEvent		+= S_C_RobotGunFireNotify;
			MobileFishingProtocol.GetInstance().onRobotGunFireEvent		 		+= S_C_RobotGunFire;

			Utility.SetResolutionRatio();
		}


		void OnDestroy()
		{

            // 记得清除factory里的缓存.
            Factory.Clear();

			// GameApp.SceneMgr.SceneLoaded -= MessageHandlerInit;
			MobileFishingProtocol.GetInstance().onLoadGameStationSuccessEvent 	-= S_C_GameStation;
			MobileFishingProtocol.GetInstance().onUserJoinEvent 				-= S_C_UserJoin;
			MobileFishingProtocol.GetInstance().onReceiveSystemTimeEvent 		-= S_C_TimeSync;
			MobileFishingProtocol.GetInstance().onUserLeaveEvent 				-= S_C_UserLeave;
			MobileFishingProtocol.GetInstance().onBuyBulletEvent 				-= S_C_BuyBullet;
			MobileFishingProtocol.GetInstance().onGunFireEvent 					-= S_C_GunFire;
			MobileFishingProtocol.GetInstance().onGunPowerUpEvent				-= S_C_GunPowerUp;

			//暂时还没使用.
			// MobileFishingProtocol.GetInstance().onGunMultiEvent 				-= S_C_GunPowerMulti;
			MobileFishingProtocol.GetInstance().onReceiveGeneratorNPCEvent 		-= S_C_NPC_Generator;
			MobileFishingProtocol.GetInstance().onNPCKillEvent			 		-= S_C_NPC_Kill;
			MobileFishingProtocol.GetInstance().onWinFromCacheEvent		 		-= S_C_WinFromCache;
			MobileFishingProtocol.GetInstance().onBalanceEvent			 		-= S_C_Balance;
			MobileFishingProtocol.GetInstance().onRobotGunFireNotifyEvent		-= S_C_RobotGunFireNotify;
			MobileFishingProtocol.GetInstance().onRobotGunFireEvent		 		-= S_C_RobotGunFire;

            Instance = null;
        }

		// 购买子弹.
		public void C_S_BuyBullet(uint _buyScore)
		{
			MobileFishingProtocol.GetInstance().UserBuyBullet(_buyScore);
		}
		
		// 加炮.
		public void C_S_GunPowerUp(uint _power)
		{
			MobileFishingProtocol.GetInstance().C_S_GunPowerUp(_power);
		}

		// 加炮倍率.
		public void C_S_GunPowerMulti(int _multi)
		{
			// MobileFishingProtocol.GetInstance().C_S_GunPowerMulti((uint)_multi);
		}

		// 发泡.
		public void C_S_GunFire(float _eulerZ, Vector2 _firePos, int _serverId, int _bulletCost, uint _serverTime)
		{
			MobileFishingProtocol.GetInstance().C_S_GunFire(_eulerZ, _firePos.x, _firePos.y, (uint)_serverId, (uint)_bulletCost, (uint)_serverTime);
		}

		// 子弹击中鱼.
		public void C_S_BulletAttack(int _serverID, int _bulletCost, int _bulletPower, int _multi)
		{
			MobileFishingProtocol.GetInstance().C_S_BulletAttack((uint)_serverID, (uint)_bulletCost, (uint)_bulletPower, (uint)_multi);
		}

		// 索要缓存.
		public void C_S_WinFromCache (uint _fishServerID, uint _power, uint _multi) 
		{
			MobileFishingProtocol.GetInstance().C_S_WinFromCache(_fishServerID, _power, _multi);
		}

		// 结算.
		public void C_S_Balance()
		{
			MobileFishingProtocol.GetInstance().C_S_Balance();
		}

		// 时间同步.
		public void C_S_TimeSync()
		{
			MobileFishingProtocol.GetInstance().C_S_TimeSync();
		}
		
		// 机器人发泡.
		public void C_S_RobotGunFire(int _robotChair, float _eulerZ, Vector2 _pos, int _serverId, int _bulletCost, uint _serverTime)
		{
			MobileFishingProtocol.GetInstance().C_S_RobotGunFire((uint)_robotChair, (uint)_serverId, _eulerZ, _pos.x, _pos.y, (uint)_bulletCost, _serverTime);
		}

		// 机器人子弹击中鱼.
		public void C_S_RobotBulletAttack(int _robotChair, int _serverID, int _bulletCost, int _bulletPower, int _multi)
		{
			MobileFishingProtocol.GetInstance().C_S_RobotBulletAttack((uint)_robotChair, (uint)_serverID, (uint)_bulletCost, (uint)_bulletPower, (uint)_multi);
		}



		// v ========== S - C ==========.
		public void S_C_GameStation(CMD_SC_GF_GAME_STATION _gameStateS)
		{
			CanonCtrl.Instance.Init((int)_gameStateS.gsJiTaiLeiXing, 
			                        (int)_gameStateS.gsToUBiRate, 
			                        (int)_gameStateS.gsGameRate, 
			                        (int)_gameStateS.gsPowerMin,			
			                        (int)_gameStateS.gsPowerMax,			
			                        (int)_gameStateS.gsPowerStep,		
			                        (int)_gameStateS.gsPowerMultiMax,	
			                        _gameStateS.gsPeak,			
			                        _gameStateS.gsServerTime,		
			                        (int)_gameStateS.gsMapID,			
			                        _gameStateS.gsMapTime,			
			                        (int)_gameStateS.gsSelfChair,		
			                        (int)_gameStateS.gsSelfUserId,		
			                        (int)_gameStateS.gsSelfGold,
			                        (int)_gameStateS.gsSelfVoucher		
			                        );
		}

		public void S_C_UserJoin(CMD_SC_GF_USER_JOIN _userJoin)
		{
			int _chair = (int)_userJoin.gsChair;
			CanonCtrl.Instance.S_C_UserJoin(_chair, (int)_userJoin.gsUserId, _userJoin.gsScore, (int)_userJoin.gsLevel, _userJoin.dwName, (int)_userJoin.gsPropIdShip, 
			                              (int)_userJoin.gsPropIdBullet, (int)_userJoin.gsPower, (int)_userJoin.gsPowerMulti);
		}

		public void S_C_UserLeave(CMD_SC_GF_USER_LEAVE _userLeave)
		{
			int _chair = (int)_userLeave.gsChair;
			CanonCtrl.Instance.UserLeave(_chair);
		}

		public void S_C_BuyBullet(CMD_SC_GF_USER_BUY_BULLET _buyBullet)
		{
			int _chair = (int)_buyBullet.gsChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].BuyBullet(_chair, _buyBullet.gsScore);
				
				// only shows the real player's gold value.
				if(_chair==CanonCtrl.Instance.realCanonID)
				{
					CanonCtrl.Instance.SetGoldValue((int)_buyBullet.gsGold);
				}
			}
		}

		public void S_C_GunFire(CMD_SC_GF_GUN_FIRE _gunFire)
		{
			int _chair = (int)_gunFire.gsChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].S_C_FakePly_GunFire(_chair, (float)_gunFire.gsRot, (float)_gunFire.gsZ, (float)_gunFire.gsY, (int)_gunFire.gsSsrverId, (int)_gunFire.gsCostVal, _gunFire.gsServerTime);
			}
		}

		public void S_C_GunPowerUp(CMD_SC_GF_GUN_POWER_UP _gunPowerUp)
		{
			int _chair = (int)_gunPowerUp.gsChair;
			if(_chair>=CanonCtrl.Instance.deskType || _chair<0)
			{
				return;
			}
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].GunPowerUp(_chair, (int)_gunPowerUp.gsPower);
			}
		}

		public void S_C_GunPowerMulti(CMD_SC_GF_GUN_POWER_MULTI _gunPowerMulti)
		{
			int _chair = (int)_gunPowerMulti.gsChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].S_C_GunPowerMulti(_chair, (int)_gunPowerMulti.gsMulti);
			}
		}

		public void S_C_NPC_Generator(CMD_SC_NPC_GENERATOR _npcGen)
		{
			// 通过名字获取路径.
	        string _npcName = IdMap.GetIdMapInfo(_npcGen.gsActorId);
            NavigParam _np = NavigationMap.GetActorNavigation(_npcGen.gsNavigId);
			Vector3 [] _pathNodes = _np.path.ToArray();

			// 把路径适配到本地设备.
			_pathNodes = Utility.TransformPosZ2X(_pathNodes);
			// 路径时间.
			float _timeLength = _np.timeLength;
			PathCtrl.Instance.S_C_NPC_Generator(_npcGen.gsServerTime, _npcGen.gsServerId, _npcName, _pathNodes, _timeLength, _npcGen.gsNavigRot, _npcGen.gsZ, _npcGen.gsY);
		}

		public void S_C_NPC_Kill(CMD_SC_GF_NPC_KILL _npcKill)
		{
			int _serverID 	= (int)_npcKill.gsNpcId;
			int _chair 		= (int)_npcKill.gsChairId;
			int _value 		= (int)_npcKill.gsRate*(int)_npcKill.gsPower;
			// 普通击杀.
			if(_npcKill.gsFlag==0)
			{
				if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
				{
					NPC_Kill_Class.NPC_Kill(_chair, (int)_npcKill.gsRate, (int)_npcKill.gsPower, _serverID, (int)_npcKill.gsNPCType);
				}
			}
			// 缓存击杀.
			else 
			{
				if(_chair==CanonCtrl.Instance.realCanonID)
				{
					if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
					{
						NPC_Kill_Class.NPC_Kill_AddCacheInList(_chair, (int)_npcKill.gsRate, (int)_npcKill.gsPower, _serverID, (int)_npcKill.gsNPCType);
					}
					C_S_WinFromCache(_npcKill.gsNpcId, _npcKill.gsPower, _npcKill.gsRate);
				}
			}
		}
		// 缓存击杀.
		public void S_C_WinFromCache (CMD_SC_GF_WIN_FROM_CACHE _winFromCache) 
		{
			// send to all real player.
			int _chair = (int)_winFromCache.gsChair;
			int _value = (int)_winFromCache.gsRate*(int)_winFromCache.gsPower;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				NPC_Kill_Class.NPC_Kill_GiveCacheFromList(_chair, (int)_winFromCache.gsRate, _value);
			}
		}

		//结算消息是广播的,(别人结算的信息也会收到)
		public void S_C_Balance (CMD_SC_BALANCE _balance) 
		{
			int _chair = (int)_balance.gsChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
//				CanonCtrl.Instance.S_C_AccountMsgHaveReceive = true;(真实玩家才改变)
				BuyScoreAndAccount.Instance.S_C_Account_NoPanel((int)_balance.gsGold, _chair);
			}
		}

		public void S_C_TimeSync(CMD_SC_GF_TIME_SYNC _timeSync)
		{
			TimeCtrl.Instance.serverTime = _timeSync.gsServerTime;
		}

		public void S_C_RobotGunFireNotify(CMD_SC_GF_ROBOT_GUN_FIRE_NOTIFY _notify)
		{
			int _npcID = (int)_notify.gsNpcId;
			int _chair = (int)_notify.gsRobotChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].S_C_RobotGunFireNotify(_npcID);
			}
		}

		public void S_C_RobotGunFire(CMD_SC_Robot_GUN_FIRE _robotGunFire)
		{
			int _chair = (int)_robotGunFire.gsRobotChair;
			if(CanonCtrl.Instance.singleCanonList[_chair]!=null)
			{
				CanonCtrl.Instance.singleCanonList[_chair].S_C_RobotGunFire(_robotGunFire.gsRot, _robotGunFire.gsZ, _robotGunFire.gsY, (int)_robotGunFire.gsSsrverId, 
				                                                            (int)_robotGunFire.gsCostVal, _robotGunFire.gsServerTime, _robotGunFire.gsChair);
			}
		}


		public bool showAllGui = false;
		/*void OnGUI()
		{
			if(!showAllGui)
			{
				return;
			}
			GUILayout.Space(0f);
			GUILayout.Label(" server2DeviceRatio = " + Utility.server_to_device_ratio);
			GUILayout.Label(" resolution = " + Screen.width+"/"+Screen.height);
			GUILayout.Label(" UIRoot = " + UIRoot.list[0].manualWidth+"/"+UIRoot.list[0].manualHeight);
			GUILayout.Label(" Input MousePos = " + Input.mousePosition);
		}*/
	}
}

