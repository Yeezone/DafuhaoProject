using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class SingleBullet : MonoBehaviour 
	{
		private Transform curTrans;
		public float speed = 6f;
		// left, top, right, down.
		// 左，上，右，下的边界.
		private Vector4 viewportRect;

		// 当前移动的方向.
		public enum dir
		{
			none=0,
			left,
			up,
			right,
			down
		}
		private dir curDir = dir.none;

		// 子弹所属的玩家id.
		private int canonID;
		private int bulletPower;
		private int bulletCost;

		// 如果是robot 发射的子弹，那么这个子弹有这个玩家来检测.
		private int robotBulletMonitorCanonID;

		// 自当所锁定的鱼.
		private SingleFish lockSingleFish;


		void Awake () 
		{
			curTrans 	 = transform;
			viewportRect = Utility.GetViewportFramePos();
		}

		void OnEnable()
		{
			lockSingleFish  = null;
			robotBulletMonitorCanonID = -1;
			curDir = dir.none;
		}

		// 子弹初始化.
		public void Init(int _canonID, int _bulletCost, int _bulletPower, Transform _lockedFish, int _robotBulletMonitorCanonID)
		{
			canonID = _canonID;
			if(_lockedFish!=null)
			{
				lockSingleFish = _lockedFish.GetComponent<SingleFish>();
			}

			bulletPower = _bulletPower;
			bulletCost 	= _bulletCost;
			robotBulletMonitorCanonID = _robotBulletMonitorCanonID; 
		}

		void Update () 
		{
			// 跟新子弹反弹.
			if(curDir!=dir.left)
			{
				if(curTrans.position.x<viewportRect.x)
				{
					curDir 		= dir.left;
					curTrans.up = Vector3.Reflect(curTrans.up, Vector3.right);
				}
			}
			if(curDir!=dir.up)
			{
				if(curTrans.position.y>viewportRect.y)
				{
					curDir 		= dir.up;
					curTrans.up = Vector3.Reflect(curTrans.up, Vector3.down);
				}
			}
			if(curDir!=dir.right)
			{
				if(curTrans.position.x>viewportRect.z)
				{
					curDir 		= dir.right;
					curTrans.up = Vector3.Reflect(curTrans.up, Vector3.left);
				}
			}
			if(curDir!=dir.down)
			{
				if(curTrans.position.y<viewportRect.w)
				{
					curDir 		= dir.down;
					curTrans.up = Vector3.Reflect(curTrans.up, Vector3.up);
				}
			}

			// 如果有锁定的鱼.
			if(lockSingleFish!=null)
			{
				// 锁定的这条鱼还在锁定状态.
				if(lockSingleFish.locking)
				{
					// 自当跟随.
					Vector3 _dir = lockSingleFish.transform.position - curTrans.position;
					curTrans.up = _dir;
					curTrans.Translate(Vector3.up * Time.deltaTime * speed);
				}
				// 自当朝着前方打.
				else 
				{
					curTrans.Translate(Vector3.up * Time.deltaTime * speed);
				}
			}
			// 自当朝着前方打.
			else 
			{
				curTrans.Translate(Vector3.up * Time.deltaTime * speed);
			}
			
		}

		// unity 自带的碰撞检测方法.
		void OnTriggerEnter2D(Collider2D _collider)
		{
			// 如果有锁定的鱼 &&　锁定的这条鱼还在锁定状态.         have a locking fish, the locking fish is not dead or move out viewport yet.
			if(lockSingleFish!=null && lockSingleFish.locking)
			{
				// 如果碰到的鱼不是锁定的鱼.
				if(lockSingleFish!=_collider.GetComponent<SingleFish>())
				{
					return;
				}
			}
			// 避免子弹互相碰撞
			if(_collider.GetComponent<SingleFish>() == null){
				return;
			}

			// 创建网格.
			if(CanonCtrl.Instance.singleCanonList[canonID]!=null)
			{
				CanonCtrl.Instance.singleCanonList[canonID].CreateOneNet(curTrans.position, curTrans.rotation);
			}

			SingleFish _sf = _collider.GetComponent<SingleFish>();

			_sf.ChangeColor();

			// 1, no real canon bullet(fake canon, robot canon)
			// 2, not robot Bullet(fake canon, real canon)
			// so it is a fake canon.

			// 1, 如果不是真实的玩家 && 也不是机器人.（其他玩家）.
			if(canonID!=CanonCtrl.Instance.realCanonID && robotBulletMonitorCanonID<0)
			{
				// 那么仅仅只是回收子弹就可以了.
				Factory.Recycle(curTrans);
				return;
			}
			else 
			{
				// 鱼还没死.
				if(!_sf.dead)
				{
					// 2, 如果是机器人发出的子弹.
					if(robotBulletMonitorCanonID>=0)
					{
						MessageHandler.Instance.C_S_RobotBulletAttack(canonID, _sf.fishServerID, bulletCost, bulletPower, _sf.multi);
					}
					// 3, 如果是真实玩家发出的子弹.
					else 
					{
						MessageHandler.Instance.C_S_BulletAttack(_sf.fishServerID, bulletCost, bulletPower, _sf.multi);
						// 玩家在场景中的子弹数目减少一个.
						CanonCtrl.Instance.singleCanonList[canonID].RealGunAddBulletNumInScene(-1);
					}

					// 告诉这条鱼，打中它的子弹的bulletcost为多少.
					_sf.bulletCost = bulletCost;
				}

				// 回收子弹.
				Factory.Recycle(curTrans);
			}
		}
	}
}