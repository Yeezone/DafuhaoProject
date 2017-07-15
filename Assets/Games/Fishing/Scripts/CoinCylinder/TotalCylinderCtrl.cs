using UnityEngine;

namespace com.QH.QPGame.Fishing
{

	public class TotalCylinderCtrl : MonoBehaviour 
	{
		public static TotalCylinderCtrl Instance;
		[HideInInspector] public SingleCylinderCtrl [] singleCylinderList = new SingleCylinderCtrl[8];

		//　是否要显示金币柱.
		public bool showCylider = true;

		// 金币柱的预设.
		public Transform cylinderCtrlPrefab;

		// 金币柱的父节点.
		public Transform cylinderCache;


		void Awake () 
		{
			Instance = this;
			if(cylinderCache==null)
			{
				cylinderCache = Utility.uiRoot.gameObject.transform.FindChild("Camera").FindChild("CylinderCache");
			}
		}

		// 按照 1600 * 900 适配金币柱的大小.
		void Start()
		{
			cylinderCache.localScale = new Vector3(cylinderCache.localScale.x * Utility.device_to_1600x900_ratio.x, cylinderCache.localScale.y * Utility.device_to_1600x900_ratio.y, 1f);
		}

		// 给一个玩家创建一个金币柱的控制器.
		public void CreateCylinderCtrl4OnePly(int _canonID, Vector3 _showUpPos, Vector3 _targetPos)
		{
			if(!showCylider)
			{
				return;
			}

			// 根据玩家的位置，计算出金币柱的 rotation.
			Quaternion _rot = Quaternion.identity;
			int _dir = CanonCtrl.Instance.dirArray[_canonID];
			_rot = Quaternion.Euler(new Vector3(0f, 0f, _dir*90f));

			// 创建一个金币柱控制器.
			Transform _trans = Factory.Create(cylinderCtrlPrefab, _showUpPos, _rot);
			_trans.parent = cylinderCache;
			_trans.localScale = Vector3.one;

			// 初始化该金币柱控制器.
			singleCylinderList[_canonID] = _trans.GetComponent<SingleCylinderCtrl>();
			singleCylinderList[_canonID].Init(_showUpPos, _targetPos, _dir);

			if(singleCylinderList[_canonID]==null)
			{
				Debug.Log("SingleCylinderCtrl can not be found!");
			}
		}

		// 玩家退出，回收这个玩家的金币柱控制器.
		public void ClearSingleCylinder(int _canonID)
		{
			if(!showCylider)
			{
				return;
			}
			if(singleCylinderList[_canonID]!=null)
			{
				Factory.Recycle(singleCylinderList[_canonID].transform);
			}
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}
