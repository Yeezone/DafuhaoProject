using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public static class Utility
	{
		private static Camera UICam;
		public static Camera GetUICam()
		{
			if(UICam==null)
			{
				FindUICam();
			}
			return UICam;
		}
		private static void FindUICam()
		{
			UICam = GameObject.Find("UI_Root_").transform.FindChild("Camera").GetComponent<Camera>();
			if(UICam==null)
			{
				Debug.LogError("UICam can not be found !");
			}
		}

		// 获取屏幕四周的位置(鱼游游出屏幕检测，子弹碰撞到边界检测).
		private static Vector4 _xyzw;
//		private static bool haveGetViewportFramePos = false;
		public static Vector4 GetViewportFramePos()
		{
//			if(haveGetViewportFramePos)
//			{
//				return _xyzw; 
//			}
			if(UICam==null)
			{
				FindUICam();
			}

            Vector3 _leftBottom = UICam.ViewportToWorldPoint(Vector3.zero);
			Vector3 _rightTop   = UICam.ViewportToWorldPoint(new Vector3(1f,1f,0f));

			_xyzw = new Vector4(_leftBottom.x, _rightTop.y, _rightTop.x, _leftBottom.y); 
//			haveGetViewportFramePos = true;

			return _xyzw;
			Debug.Log ("永远只调用一次");
		}

		// 获取三维坐标在 viewport 坐标.
		public static Vector2 C_S_TransformFirePos(Vector3 _pos)
		{
			if(UICam==null)
			{
				FindUICam();
			}
			return UICam.WorldToViewportPoint(_pos);
		}

		// ngui 根节点.
		public static UIRoot uiRoot;
		// 服务器的分辨率.
		public static Vector2 serverResol = new Vector2(1600f, 900f);
		// 阿达说鱼应该按照这个比例来配.
#if UNITY_STANDALONE_WIN
		//PC端分辨率设置为 1280 750
		public static Vector2 resol_1136x640 = new Vector2(1280f, 750f);
#else
		//手机端分辨率设置为 1136 640
		public static Vector2 resol_1136x640 = new Vector2(1136f, 640f);
#endif

		// 服务器分辨率和设备分辨率的比例.
		public static Vector2 server_to_device_ratio = Vector2.one;
		// 设备分辨率和服务器（1600*900）的比例.
		public static Vector2 device_to_1600x900_ratio = Vector2.one;
		// 设备分辨率和 1136*640 的比例.
		public static Vector2 device_to_1136x640_ratio = Vector2.one;


		public static void SetResolutionRatio()
		{
			Vector2	_deviceResol = new Vector2(Screen.width, Screen.height);
			uiRoot 					= GameObject.Find("UI_Root_").GetComponent<UIRoot>();
			//uiRoot.scalingStyle 	= UIRoot.Scaling.ConstrainedOnMobiles;
			uiRoot.manualWidth  	= Screen.width;
			uiRoot.manualHeight 	= Screen.height;
			uiRoot.fitWidth 		= true;
			uiRoot.fitHeight 		= true;

			FindUICam();

//PC版强制设置屏幕
#if UNITY_STANDALONE_WIN
			//PC端设置分辨率
			serverResol = resol_1136x640;
#endif

			server_to_device_ratio = new Vector2(serverResol.x/_deviceResol.x, serverResol.y/_deviceResol.y);
			device_to_1600x900_ratio = new Vector2(_deviceResol.x/serverResol.x, _deviceResol.y/serverResol.y);
			device_to_1136x640_ratio = new Vector2(_deviceResol.x/resol_1136x640.x, _deviceResol.y/resol_1136x640.y);
			sameServerWidthDeviceHeight = serverResol.x/Screen.width * Screen.height;
		}

		// 把unity的角度转换为服务器所接受的角度.
		private static float sameServerWidthDeviceHeight = 0f;
		public static float C_S_TranformFireEulerZ(float _eulerZ)
		{
			if(_eulerZ>=180f)
			{
				_eulerZ = _eulerZ - 360f;
			}
			_eulerZ *= -1;
			int _kind = 0;
			if(_eulerZ>=90f)
			{
				_kind = 1;
				_eulerZ = _eulerZ - 90f;
			}
			else if(_eulerZ<=-90f)
			{
				_kind = -1;
				_eulerZ = 90f + _eulerZ;
			}

			float _radian = _eulerZ * Mathf.PI/180;
			float outcome = Mathf.Tan(_radian);
			float serverTargetHeight = outcome * 1600f * 900f / sameServerWidthDeviceHeight;
			_radian = Mathf.Atan(serverTargetHeight/1600);
			_eulerZ = _radian * 180/Mathf.PI;
			_eulerZ += _kind * 90f;

			return _eulerZ;
		}

		// 把从服务器获取的角度转换成本地设备的角度.
		public static float S_C_TranformFireEulerZ(float _eulerZ)
		{
			int _kind = 0;
			if(_eulerZ>=90f)
			{
				_kind = 1;
				_eulerZ = _eulerZ - 90f;
			}
			else if(_eulerZ<=-90f)
			{
				_kind = -1;
				_eulerZ = 90f + _eulerZ;
			}
			
			float _radian = _eulerZ * Mathf.PI/180;
			float outcome = Mathf.Tan(_radian);
			float deviceHeight = outcome * 1600 * sameServerWidthDeviceHeight / 900;
			_radian = Mathf.Atan(deviceHeight/1600);
			_eulerZ = _radian * 180/Mathf.PI;
			
			_eulerZ += _kind * 90f;
			
			return _eulerZ;
		}

		// 适配宽度.
		public static float TransformPixelX(float _x)
		{
			return _x/server_to_device_ratio.x;
		}
		// 适配高度.
		public static float TransformPixelY(float _y)
		{
			return _y/server_to_device_ratio.y;
		}
		// 把服务器发过来的坐标(z,y,x) 换成(x,y,z).
		public static Vector3 [] TransformPosZ2X(Vector3 [] _pos)
		{
			Vector3 [] temp = new Vector3[_pos.Length];
			int _length = _pos.Length;
			for(int i=0; i<_length; i++)
			{
				temp[i] = new Vector3(_pos[i].z, _pos[i].y, 0f);
			}
			return temp;
		}

		// 把 vector3 从 1600*900 转成手机设备适配的 vector3.
		public static Vector3 S_C_Transform_V3(Vector3 _v3)
		{
			Vector3 _temp = new Vector3(_v3.x/server_to_device_ratio.x, _v3.y/server_to_device_ratio.y, 0f);
			return _temp;
		}

		// 把路径的位置从 1600*900 转成手机设备适配的路径.
		public static Vector3 [] S_C_TransformPath(Vector3 [] _v3)
		{
			int _length = _v3.Length;
			Vector3 [] pos = new Vector3[_length];
			for(int i=0; i<_length; i++)
			{
				pos[i] = new Vector3(_v3[i].x/server_to_device_ratio.x, _v3[i].y/server_to_device_ratio.y, _v3[i].z);
			}
			return pos;
		}

		// 判断是否点击在ngui控件上.
		public static bool IsMouseOverUI()
		{
			Vector3 mousePosition = Input.mousePosition;
			GameObject hoverObj = UICamera.Raycast(mousePosition) ? UICamera.lastHit.collider.gameObject : null;
			if(hoverObj!=null)
			{
				return true;
			}
			else 
			{
				return false;
			}
		}

		// 判断一个三维位置是否在屏幕内.
		public static bool CheckIfPosInViewport(Vector3 _pos)
		{
			if(UICam==null)
			{
				FindUICam();
			}
			Vector3 _vp = UICam.WorldToViewportPoint(_pos);
			if(_vp.x<0f || _vp.x>1f || _vp.y<0f || _vp.y>1f)
			{
				return false;
			}
			return true;
		}

		// 把一个sprite物件在屏幕内按照一定大小进行缩放.
		public static void ResizeSprite2Screen(GameObject bg, Camera cam, float fit2ScreenWidth, float fit2ScreenHeight, float _viewportX, float _viewportY)
		{
			SpriteRenderer _bgSpriteRend = null;
			if(bg.GetComponent<SpriteRenderer>() != null) _bgSpriteRend = bg.GetComponent<SpriteRenderer>();
			if(_bgSpriteRend==null)
			{
				return;
			}
			bg.transform.localScale = Vector3.one;
			
			float width  = _bgSpriteRend.sprite.bounds.size.x;
			float height = _bgSpriteRend.sprite.bounds.size.y;
			
			float worldScreenHeight = cam.orthographicSize * 2;
			float worldScreenWidth 	= worldScreenHeight / Screen.height * Screen.width;
			
			float localScaleX = worldScreenWidth / width / fit2ScreenWidth;
			float localScaleY = worldScreenHeight / height / fit2ScreenHeight;
			
			Vector3 _tempPos = cam.ViewportToWorldPoint(new Vector3(_viewportX, _viewportY, 0f));
			bg.transform.position = new Vector3(_tempPos.x, _tempPos.y, bg.transform.position.z);
			bg.transform.localScale = new Vector3(localScaleX, localScaleY, bg.transform.localScale.z);
		}


		// char to int.
		public static int [] Char2Int(long _value)
		{
			char [] _cs = _value.ToString().ToCharArray();
			int _csLength = _cs.Length;
			int [] _out = new int[_csLength];
			for(int i=0; i<_csLength; i++)
			{
				_out[_csLength-1-i] = _cs[i] - '0'; 
			}
			return _out;
		}
		
		// char to int.
		public static int [] Char2Int(int _value)
		{
			char [] _cs = _value.ToString().ToCharArray();
			int _csLength = _cs.Length;
			int [] _out = new int[_csLength];
			for(int i=0; i<_csLength; i++)
			{
				_out[_csLength-1-i] = _cs[i] - '0'; 
			}
			return _out;
		}
	}
}