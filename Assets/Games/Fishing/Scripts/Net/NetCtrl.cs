using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class BulletValue2NetType
	{
		public int bulletValue;
		public List<Transform> netPrefabs;
	}

	public class NetCtrl : MonoBehaviour 
	{
		public static NetCtrl Instance;
		private Transform curTrans;
		public List<BulletValue2NetType> netList = new List<BulletValue2NetType>();

		void Awake()
		{
			Instance = this;
			curTrans = this.transform;
		}

		public void CreateOneNet(int _canonIndex, int _bulletPower, Vector3 _pos, Quaternion _rot)
		{
			if(_canonIndex>=netList.Count)
			{
				Debug.LogError("Instantiate net from net list is error => _netIndex = "+_canonIndex );
				return;
			}
		
			int _netIndex = 0;
			for(int i=netList.Count-1; i<=0; i--)
			{
				if(_bulletPower>=netList[i].bulletValue)
				{
					_netIndex = i;
					break;
				}
			}

			Transform _net = Factory.Create(netList[_canonIndex].netPrefabs[_netIndex], _pos, _rot);
			_net.parent = curTrans;
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}