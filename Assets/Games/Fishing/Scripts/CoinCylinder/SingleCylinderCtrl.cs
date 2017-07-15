using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{

	// 金币柱的参数.
	[Serializable]
	public class CylinderParam
	{
		// 倍率.
		public int multi;
		// 分值.
		public int value;
		// 出现的位置.
		public Vector3 showupPos;
		// 出现的 rotation.
		public Quaternion showupRot;
	}


	public class SingleCylinderCtrl : MonoBehaviour 
	{
		private Transform curTrans;
		// 金币柱的创建队列.
		private List<CylinderParam> cylinder2CreateList = new List<CylinderParam>();
		// 金币柱的预设.
		public Transform cylinderPrefab;
		// 两个金币柱的距离.
		public float cylinderDistance = 30f;
		// 金币柱的时间间隔.
		public float cylinderCreateTimeGap = 1f;
		private float targetTime;

		// 最末尾的一个金币柱，可以根据这个金币柱获得下一个即将要创建的金币柱的创建位置.
		[HideInInspector] public Transform lastCreatedTrans;
		private Vector3 showUpPos;
		private Vector3 targetPos;

		// 金币柱的状态.
		private enum CylinderState
		{
			// 跳.
			jump,
			// 移动.
			move,
			// 移动一会，等待.
			moveWait,
			// 回收结束.
			finish
		}
		private CylinderState curCylinderState = CylinderState.finish;

		// 金币柱跳起来的时间长度.
		public float wait4CylinderMoveUpTimeLength = 0.5f;

		// 金币柱 lerp percent.
		private float lerpPercent;

		// 移动的速度.
		public float cylinderMoveSpeed = 2f;
		private Vector3 curStepPos;
		private Vector3 nextStepPos;

		// 移动一步等待的时间.
		public float moveOneStepWaitLength = 0.5f;

		// 已经创建的金币柱的缓存队列.
		private List<Transform> cylinderHaveCreatedList = new List<Transform>();

		//　此玩家在场景中的金币柱的数量.
		private int cylinderNumInScene = 0;

		// 金币柱的方向.
		private int dir;
		
		// 初始化金币柱的参数.
		public void Init(Vector3 _showUpPos, Vector3 _targetPos, int _dir)
		{
			showUpPos 	= _showUpPos;
			targetPos 	= _targetPos;
			dir = _dir;
		}

		void Awake () 
		{
			curTrans = this.transform;
		}

		void OnEnable()
		{
			cylinder2CreateList.Clear();
			cylinderHaveCreatedList.Clear();
		}

		void Update () 
		{
			// 超过五个就要等待.
			if(cylinderNumInScene<5 && cylinder2CreateList.Count>0)
			{
				if(Time.time>=targetTime)
				{
					CreateCylinder(cylinder2CreateList[0]);
					cylinder2CreateList.Remove(cylinder2CreateList[0]);
					targetTime = Time.time + cylinderCreateTimeGap;
				}
			}

			switch(curCylinderState)
			{			
				case CylinderState.jump:
				lerpPercent += Time.deltaTime;
				if(lerpPercent>wait4CylinderMoveUpTimeLength)
				{
					lerpPercent = 0f;
					curCylinderState = CylinderState.moveWait;
				}
				break;
				case CylinderState.move:
				lerpPercent += Time.deltaTime * cylinderMoveSpeed;
				curTrans.localPosition = Vector3.Lerp(curStepPos, nextStepPos, lerpPercent);
				if(lerpPercent>1f)
				{
					lerpPercent = 0f;
					curStepPos = curTrans.localPosition;
					switch(dir)
					{
					case 0:
						nextStepPos = curTrans.localPosition + new Vector3(cylinderDistance, 0f, 0f);
						break;
						
					case 1:
						nextStepPos = curTrans.localPosition + new Vector3(0f, cylinderDistance, 0f);
						break;
						
					case 2:
						nextStepPos = curTrans.localPosition - new Vector3(cylinderDistance, 0f, 0f);
						break;
						
					case 3:
						nextStepPos = curTrans.localPosition - new Vector3(0f, cylinderDistance, 0f);
						break;
					}
					curCylinderState = CylinderState.moveWait;
				}
				break;
				
				case CylinderState.moveWait:
				lerpPercent += Time.deltaTime;
				if(lerpPercent>=moveOneStepWaitLength)
				{
					lerpPercent = 0f;
					int _count = cylinderHaveCreatedList.Count;
					for(int i=0; i<_count; i++)
					{
						switch(dir)
						{
						case 0:
							if(cylinderHaveCreatedList[i].position.x>=targetPos.x)
							{
								cylinderHaveCreatedList[i].GetComponent<SingleCylinder>().Recycle();
								cylinderNumInScene--;
								cylinderHaveCreatedList.Remove(cylinderHaveCreatedList[i]);
								if(cylinderHaveCreatedList.Count==0)
								{
									curTrans.position = showUpPos;
									curCylinderState = CylinderState.finish;
								}
								return;
							}
							break;
						case 1:
							if(cylinderHaveCreatedList[i].position.y>=targetPos.y)
							{
								cylinderHaveCreatedList[i].GetComponent<SingleCylinder>().Recycle();
								cylinderNumInScene--;
								cylinderHaveCreatedList.Remove(cylinderHaveCreatedList[i]);
								if(cylinderHaveCreatedList.Count==0)
								{
									curTrans.position = showUpPos;
									curCylinderState = CylinderState.finish;
								}
								return;
							}
							break;
						case 2:
							if(cylinderHaveCreatedList[i].position.x<=targetPos.x)
							{
								cylinderHaveCreatedList[i].GetComponent<SingleCylinder>().Recycle();
								cylinderNumInScene--;
								cylinderHaveCreatedList.Remove(cylinderHaveCreatedList[i]);
								if(cylinderHaveCreatedList.Count==0)
								{
									curTrans.position = showUpPos;
									curCylinderState = CylinderState.finish;
								}
								return;
							}
							break;
						case 3:
							if(cylinderHaveCreatedList[i].position.y<=targetPos.y)
							{
								cylinderHaveCreatedList[i].GetComponent<SingleCylinder>().Recycle();
								cylinderNumInScene--;
								cylinderHaveCreatedList.Remove(cylinderHaveCreatedList[i]);
								if(cylinderHaveCreatedList.Count==0)
								{
									curTrans.position = showUpPos;
									curCylinderState = CylinderState.finish;
								}
								return;
							}
							break;
						}
					}
					curCylinderState = CylinderState.move;
				}
				break;

				default:
				break;
			}
		}
		
		// 添加一个要创建的金币柱.
		public void Add2Need2CreateList(int _multi, int _value, Vector3 _showupPos)
		{
			if(Time.time>targetTime)
			{
				targetTime = Time.time;
			}
			
			CylinderParam _b = new CylinderParam();
			_b.multi = _multi;
			_b.value = _value;
			_b.showupPos = _showupPos;
			_b.showupRot = Quaternion.Euler(new Vector3(0f, 0f, dir*90f));
			
			cylinder2CreateList.Add(_b);
		}

		//　创建一个金币柱.
		void CreateCylinder(CylinderParam _cp)
		{
			Transform _cylinder = Factory.Create(cylinderPrefab, Vector3.zero, Quaternion.identity);
			_cylinder.parent = transform;
			_cylinder.localScale = Vector3.one;
			cylinderNumInScene++;

			if(cylinderHaveCreatedList.Count>0)
			{
				_cylinder.localPosition = new Vector3(cylinderHaveCreatedList[cylinderHaveCreatedList.Count-1].localPosition.x, 0f, 0f)
					- new Vector3(cylinderDistance, 0f, 0f);
			}
			else 
			{
				_cylinder.position = _cp.showupPos;
			}

			// _cylinder.rotation = _cp.showupRot;
			_cylinder.localEulerAngles = Vector3.zero;

			_cylinder.GetComponent<SingleCylinder>().Init(_cp.multi, _cp.value, dir);
			cylinderHaveCreatedList.Add(_cylinder);
			
			if(curCylinderState==CylinderState.finish)
			{
				lerpPercent = 0f;
				curStepPos = curTrans.localPosition;
				switch(dir)
				{
				case 0:
					nextStepPos = curTrans.localPosition + new Vector3(cylinderDistance, 0f, 0f);
					break;
				case 1:
					nextStepPos = curTrans.localPosition + new Vector3(0f, cylinderDistance, 0f);
					break;
				case 2:
					nextStepPos = curTrans.localPosition - new Vector3(cylinderDistance, 0f, 0f);
					break;
				case 3:
					nextStepPos = curTrans.localPosition - new Vector3(0f, cylinderDistance, 0f);
					break;
				}
				curCylinderState = CylinderState.jump;
			}
		}

		/*void OnGUI()
		{
			if(!MessageHandler.Instance.showAllGui)
			{
				return;
			}
			GUILayout.Space(50f);
			GUILayout.Label("cylinder2CreateList = "+ cylinder2CreateList.Count);
			GUILayout.Label("cylinderHaveCreatedList = "+cylinderHaveCreatedList.Count);
			GUILayout.Label("curCylinderState = "+curCylinderState);
		}*/
	}
}
