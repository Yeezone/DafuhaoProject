using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	//[ExecuteInEditMode]
	public class NavPath
	{
		public string _desc;
		public Vector3[] _path;
		// time length of the path.
		public float _time = 10f;
		public bool _isClosed;

		/*
		// Use this for initialization
		void Start()
		{
			Transform anchor_ori = Global.getAnchorOri();
			Transform selfTrans = transform;
			selfTrans.parent = anchor_ori;
			selfTrans.localScale = Vector3.one;

			iTweenPath pathTool = GetComponent<iTweenPath>();

			for (int i = 0; i < pathTool.nodes.Count; ++i)
			{
				pathTool.nodes[i] = selfTrans.rotation * pathTool.nodes[i];
			}

			Vector3 pathScale = selfTrans.lossyScale;
			Vector3[] editPath = pathTool.nodes.ToArray();
			for (int i = 0; i < editPath.Length; ++i )
			{
				editPath[i].x /= pathScale.x;
				editPath[i].y /= pathScale.y;
				editPath[i].z /= pathScale.z;
			}

			if (editPath.Length > 0)
			{
				int pathNum = editPath.Length;
				if (_isClosed)
				{
					++pathNum;
				}

				_path = new Vector3[pathNum];
				for (int i = 0; i < editPath.Length; ++i)
				{
					_path[i] = selfTrans.rotation * editPath[i];
				}

				if (_isClosed)
				{
					_path[pathNum - 1] = Vector3.zero;
				}
			}
		}

		void OnEnable()
		{
			Start();
		}
		*/
	}
}