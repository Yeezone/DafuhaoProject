using System.Collections.Generic;
using UnityEngine;


namespace com.QH.QPGame.Fishing
{
	public class LockCtrl : MonoBehaviour 
	{
		public static LockCtrl Instance;
		// 每个玩家锁定的线的预设.
		public List<Transform> lines = new List<Transform>();
		// 每个玩家锁定的数字的预设.
        public List<Transform> linesNumber = new List<Transform>();

//		public Camera uiCam;
		// 锁定时使用3d方式来制作，所以需要另外一个camera(lockCam)来显示锁定的线.
//		public Camera lockCam; 

		// 锁定线的大小.
//		public float startWidth = 0.18f;
//		public float endWidth = 0.04f;

		// 每个玩家锁定线的控制器.
		[HideInInspector] public SingleLockCtrl [] lockCtrls = new SingleLockCtrl[10]; 
		
		[HideInInspector] public Transform _line = null;

		void Start()
		{
			Instance = this;
		}

		// 给一个玩家创建一个锁定线的控制器.
		public void CreateLockLine(int _canonID)
		{
			lockCtrls[_canonID] = null;

			if(lines[_canonID]==null)
			{
				Debug.LogError("lines["+_canonID+"] = null");
			}
			_line = Factory.Create(lines[_canonID], Vector3.zero, Quaternion.identity);
			_line.parent = this.transform;
            _line.localScale = Vector3.one;

			//	锁定线的坐标跟随炮管的坐标
			_line.position = CanonCtrl.Instance.singleCanonList[_canonID].transform.FindChild ("canonBarrelTrans").position;

			Transform _number = Factory.Create(linesNumber[_canonID], Vector3.zero, Quaternion.identity);
			_number.parent = _line;
			_number.localScale = Vector3.one;
			lockCtrls[_canonID] = _line.GetComponent<SingleLockCtrl>();
			lockCtrls[_canonID].Init(CanonCtrl.Instance.singleCanonList[_canonID].lockStartPos, _number);
		}

		// 把一条鱼锁定点在UI Camera的位置转换为 lock Camera 所在的位置.
//		public Vector3 uiCamPos_to_LockCamPos(Vector3 _fishPosInUICam)
//		{
//			Vector3 _fishViewportInUICam = uiCam.WorldToViewportPoint(_fishPosInUICam);
//			Vector3 _fishPosInLockCam = lockCam.ViewportToWorldPoint(new Vector3(_fishViewportInUICam.x, _fishViewportInUICam.y, -lockCam.transform.position.z));
//			return _fishPosInLockCam;
//		}

		// 玩家推出，清除这个玩家的锁定控制器.
		public void ClearSingleLock(int _canonID)
		{
			// Because the number is attached to the line, so when we destroy the line, the number will destroy automatically.
			if(lockCtrls[_canonID]!=null)
			{
				Factory.Recycle(lockCtrls[_canonID].transform);
			}
			lockCtrls[_canonID] = null;
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}