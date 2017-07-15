using UnityEngine;
using System;

namespace com.QH.QPGame.Fishing
{
	public class PathCtrl : MonoBehaviour 
	{
		public static PathCtrl Instance;

		void Awake()
		{
			Instance = this;
		}

		// 从服务器接收到创建一条鱼的消息.
		public void S_C_NPC_Generator(UInt32 _serverTime, UInt32 _serverID, string _actorName, Vector3 [] _path, float _timeLength, float _rotX, float _x, float _y)
		{
			Vector3 [] tranPos = Utility.S_C_TransformPath(_path);
			FishParam _fp = new FishParam();
			NavPath _np = new NavPath();
			_fp.serverTime = _serverTime;
			_fp.serverID = (int)_serverID;
			_np._path = tranPos;
			_fp.pathTimeLength = _timeLength;
			_fp.name = _actorName;
			// 从 server 接收到的角度和 unity 的角度是相反的. 
			_fp.rotX = -_rotX; 
			_fp.pos = Utility.S_C_Transform_V3(new Vector3(_x, _y, 0f));
			_fp.navPath = ShiftPath(_np, _fp.pos, _fp.rotX);

			FishCtrl.Instance.AddFishInCreatList(_fp);
		}

		// 转换路径.
		private NavPath ShiftPath(NavPath _np, Vector3 _basePos, float _rotX)
		{
			int _num = _np._path.Length; 
			for(int i=0; i<_num; i++)
			{
				// 1, rotate.
				_np._path[i] = Quaternion.Euler(0f,0f,_rotX)*_np._path[i];
				// 2, group local position.
				_np._path[i] += _basePos;
			}
			return _np; 
		}

		// shadow的路径.
		public NavPath GetShadowPath(NavPath _np, Vector3 _shadowShift)
		{
			// we need to create a new one NavPath and return it.
			NavPath np = new NavPath();
			int _num = _np._path.Length;
			np._path = new Vector3[_num];
			for(int i=0; i<_num; i++)
			{
				// 1, rotate.
				np._path[i] = _np._path[i];
				np._path[i] += _shadowShift;
			}
			np._time = _np._time;
			return np; 
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}
