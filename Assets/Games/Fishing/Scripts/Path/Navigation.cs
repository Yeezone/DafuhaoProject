using UnityEngine;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class Navigation
	{
		public NavPath _pathCfg;
		// move speed of the fish.
		public float _speed = 1f;
		public float _stupidMoveSpeed = 250f;
		Transform _objTrans;
        TweenRotation _objTween;
		Vector3 _startPos;
		Vector3 _lastPos;
		float _time;
		public float timeStamp;
		float _totalTimeFactor;
		Quaternion _pathRot = Quaternion.identity;
		bool _isPathEnd;
		// bool _isEnable = true;


	//	public void init(Transform objTrans, NavPath _path)
	//	{
	//		_objTrans = objTrans;
	//		ApplyPath(_path);
	//		start(_pathCfg, objTrans.eulerAngles);
	//	}

		public void init(Transform objTrans, NavPath _path)
		{
			_time = 0f;
			_objTrans = objTrans;
            _objTween = _objTrans.GetComponent<TweenRotation>();
			ApplyPath(_path);
			start(_pathCfg, objTrans.eulerAngles);
		}
		public void init(Transform objTrans, NavPath _path, float _dealyTimeLength)
		{
			_time = _dealyTimeLength;
			_objTrans = objTrans;
            _objTween = _objTrans.GetComponent<TweenRotation>();
			ApplyPath(_path);
			start(_pathCfg, objTrans.eulerAngles);
		}

		public void ApplyPath(NavPath _path)
		{
			_pathCfg = _path;
		}

		public void start(NavPath pathCfg, Vector3 rotAngle)
		{
			if (pathCfg != null)
			{
				_pathCfg = pathCfg;
			}

			if (rotAngle != Vector3.zero)
			{
				_pathRot = Quaternion.Euler(rotAngle);
			}
			
			if (_pathCfg == null)
			{
				return;
			}

			init();
			_startPos = _objTrans.localPosition;
			_lastPos  = _objTrans.localPosition;

			if (_pathCfg._time <= 0f)
			{
				_totalTimeFactor = 1f / 0.01f;
			}
			else
			{
				_totalTimeFactor = 1f / _pathCfg._time;
			}

			//设置初始朝向.
			Vector3 off = interpolate(0.05f);

			Vector3 targetPos = _startPos + off;

			// Vector3 dir = targetPos - _objTrans.localPosition;
			_objTrans.right = targetPos - _objTrans.localPosition;
			_isPathEnd = false;
		}

		public void forcePathEnd()
		{
			_isPathEnd = true;
		}

		public void update()
		{
			if(!_isPathEnd)
			{
				_time += _speed * Time.smoothDeltaTime;
				timeStamp = _time * _totalTimeFactor;

				if(timeStamp > 1f)
				{
					//_time -= _pathCfg._time;
					//_isPathEnd = true;
					StupidMove();
				}
				else 
				{
					pathMove(timeStamp);
				}
			}
		}

		public void SetCurStupidMoveSpeed(float _speed)
		{
			_stupidMoveSpeed = _speed;
		}
		void StupidMove()
		{
			Vector3 moveVec = _objTrans.right * _stupidMoveSpeed * Time.smoothDeltaTime;
			_objTrans.localPosition += moveVec;
		}

		void pathMove(float timeStamp)
		{
			//如果计算旋转.
			Vector3 off = interpolate(timeStamp);
			if (_pathRot != Quaternion.identity)
			{
				off = _pathRot * off;
			}
			Vector3 targetPos = _startPos + off;
			Vector3 disVec = targetPos - _lastPos;
			//to do ,这里需要再处理一下
            if (disVec.sqrMagnitude > 0.05f)
            {
                disVec.z = 0f;
                float resAngle = calAngle(disVec);
                Vector3 oriAngle = _objTrans.localEulerAngles;
                //oriAngle.z = resAngle;                

                if (disVec.x > 0)
                {
                    if (resAngle - oriAngle.z <= -180)
                    {
                        oriAngle.z = Mathf.Lerp(oriAngle.z, resAngle + 360, 1f);
                    }
                    else if (resAngle - oriAngle.z >= 180)
                    {
                        oriAngle.z = Mathf.Lerp(oriAngle.z, resAngle - 360, 1f);
                    }
                    else
                    {
                        oriAngle.z = Mathf.Lerp(oriAngle.z, resAngle, 1f);
                    }
                }
                else
                {
                    oriAngle.z = Mathf.Lerp(oriAngle.z, resAngle, 1f);
                }

                _objTrans.localEulerAngles = oriAngle;
            }
			_lastPos = _objTrans.localPosition;
			_objTrans.localPosition = targetPos;

            if (_objTween != null)
            {
                _objTween.duration = 0.02f;
                _objTween.from = _objTrans.localEulerAngles;
                _objTween.to = new Vector3(0, 0, calAngle(disVec));
            }       
		}

		public static float calAngle(Vector3 dir)
		{
			if (dir.y > 0f)
			{
				return Vector3.Angle(Vector3.right, dir);
			}
			else
			{
				return 360f - Vector3.Angle(Vector3.right, dir);
			}
		}


		/// <summary>
		/// 以下是抄过来的代码
		/// </summary>
		Vector3[] mTangents;
		Matrix4x4 mCoeffs;
		Matrix4x4 pt = new Matrix4x4();
		Vector4 powers = new Vector4();
		Vector4 ret = new Vector4();
		Vector4 finalRet = new Vector4();

		void init()
		{
			mCoeffs.m00 = 2f;
			mCoeffs.m01 = -2f;
			mCoeffs.m02 = 1f;
			mCoeffs.m03 = 1f;
			mCoeffs.m10 = -3f;
			mCoeffs.m11 = 3f;
			mCoeffs.m12 = -2f;
			mCoeffs.m13 = -1f;
			mCoeffs.m20 = 0f;
			mCoeffs.m21 = 0f;
			mCoeffs.m22 = 1f;
			mCoeffs.m23 = 0f;
			mCoeffs.m30 = 1f;
			mCoeffs.m31 = 0f;
			mCoeffs.m32 = 0f;
			mCoeffs.m33 = 0f;

			recalcTangents();
		}

		Vector3 interpolate(float t)
		{
			 // Currently assumes points are evenly spaced, will cause velocity
	        // change where this is not the case
	        // TODO: base on arclength?

	        // Work out which segment this is in
	        float fSeg = t * (_pathCfg._path.Length - 1);
	        uint segIdx = (uint)fSeg;
	        // Apportion t 
	        t = fSeg - (float)segIdx;
	        return interpolate(segIdx, t);
		}

		Vector3 interpolate(uint fromIndex, float t)
		{
			// Bounds check
	        if((fromIndex < _pathCfg._path.Length) == false)
			{
				return Vector3.zero;
			}

	        if ((fromIndex + 1) == _pathCfg._path.Length)
	        {
	            // Duff request, cannot blend to nothing
	            // Just return source
	            return _pathCfg._path[fromIndex];
	        }

	        // Fast special cases
	        if (t == 0.0f)
	        {
	            return _pathCfg._path[fromIndex];
	        }
	        else if(t == 1.0f)
	        {
	            return _pathCfg._path[fromIndex + 1];
	        }

	        // Real interpolation
	        // Form a vector of powers of t
	        float t2, t3;
	        t2 = t * t;
	        t3 = t2 * t;

			powers.x = t3;
			powers.y = t2;
			powers.z = t;
			powers.w = 1f;

	        // Algorithm is ret = powers * mCoeffs * Matrix4(point1, point2, tangent1, tangent2)
	        Vector3 point1 = _pathCfg._path[fromIndex];
	        Vector3 point2 = _pathCfg._path[fromIndex+1];
	        Vector3 tan1 = mTangents[fromIndex];
	        Vector3 tan2 = mTangents[fromIndex+1];

	        pt.m00 = point1.x;
	        pt.m01 = point1.y;
	        pt.m02 = point1.z;
	        pt.m03 = 1.0f;
	        pt.m10 = point2.x;
	        pt.m11 = point2.y;
	        pt.m12 = point2.z;
	        pt.m13 = 1.0f;
	        pt.m20 = tan1.x;
	        pt.m21 = tan1.y;
	        pt.m22 = tan1.z;
	        pt.m23 = 1.0f;
	        pt.m30 = tan2.x;
	        pt.m31 = tan2.y;
	        pt.m32 = tan2.z;
	        pt.m33 = 1.0f;

			ret.x = powers.x * mCoeffs.m00 + powers.y * mCoeffs.m10 + powers.z * mCoeffs.m20 + powers.w * mCoeffs.m30;
			ret.y = powers.x * mCoeffs.m01 + powers.y * mCoeffs.m11 + powers.z * mCoeffs.m21 + powers.w * mCoeffs.m31;
			ret.z = powers.x * mCoeffs.m02 + powers.y * mCoeffs.m12 + powers.z * mCoeffs.m22 + powers.w * mCoeffs.m32;
			ret.w = powers.x * mCoeffs.m03 + powers.y * mCoeffs.m13 + powers.z * mCoeffs.m23 + powers.w * mCoeffs.m33;

			finalRet.x = ret.x * pt.m00 + ret.y * pt.m10 + ret.z * pt.m20 + ret.w * pt.m30;
			finalRet.y = ret.x * pt.m01 + ret.y * pt.m11 + ret.z * pt.m21 + ret.w * pt.m31;
			finalRet.z = ret.x * pt.m02 + ret.y * pt.m12 + ret.z * pt.m22 + ret.w * pt.m32;
			finalRet.w = ret.x * pt.m03 + ret.y * pt.m13 + ret.z * pt.m23 + ret.w * pt.m33;

			return new Vector3(finalRet.x, finalRet.y, finalRet.z);
		}

		void recalcTangents()
		{
			// Catmull-Rom approach
			// 
			// tangent[i] = 0.5 * (point[i+1] - point[i-1])
			//
			// Assume endpoint tangents are parallel with line with neighbour

			int i, numPoints;
			bool isClosed = _pathCfg._isClosed;

			numPoints = _pathCfg._path.Length;
			if (numPoints < 2)
			{
				// Can't do anything yet
				return;
			}

			// Closed or open?
	// 		if (_pathCfg._path[0] == _pathCfg._path[numPoints - 1])
	// 		{
	// 			isClosed = true;
	// 		}
	// 		else
	// 		{
	// 			isClosed = false;
	// 		}

			mTangents = new Vector3[numPoints];

			for (i = 0; i < numPoints; ++i)
			{
				if (i == 0)
				{
					// Special case start
					if (isClosed)
					{
						// Use numPoints-2 since numPoints-1 is the last point and == [0]
						mTangents[i] = 0.5f * (_pathCfg._path[1] - _pathCfg._path[numPoints - 2]);
					}
					else
					{
						mTangents[i] = 0.5f * (_pathCfg._path[1] - _pathCfg._path[0]);
					}
				}
				else if (i == numPoints - 1)
				{
					// Special case end
					if (isClosed)
					{
						// Use same tangent as already calculated for [0]
						mTangents[i] = mTangents[0];
					}
					else
					{
						mTangents[i] = 0.5f * (_pathCfg._path[i] - _pathCfg._path[i - 1]);
					}
				}
				else
				{
					mTangents[i] = 0.5f * (_pathCfg._path[i + 1] - _pathCfg._path[i - 1]);
				}

			}
		}
	}
}