using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	// 背景图片和对应的水草.
	[Serializable]
	public class BGTexAndGrass
	{
		// 背景图片.
//		public Texture2D tex;
		public Texture tex;
		// 水草的父节点.
		public Transform grass;
		// 记录当前背景的水草.
		[HideInInspector] public List<Transform> grassItemList;
		// 当前背景下水草的数量.
		[HideInInspector] public int grassItemNum;
	}

	[RequireComponent(typeof(AudioSource))]
	public class WaveCtrl : MonoBehaviour 
	{
		public static WaveCtrl Instance;
		private Camera uiCam;

		// 背景初始位置. 震动背景图片后，图片就往这个位置拉回.
		private Vector3 bgOriPos;

		// 波浪移动的速度.
		public float moveSpeed;

		// 一张背景图的生命周期（毫秒）.
		public int mapTimeLength = 300000;
		private float mapTargetTime;
		// 提前5.5秒出现波浪.
		public int beforeTimeLength = 5500;

		// 背景图片和对应的水草.
		public List<BGTexAndGrass> TexAndGrass;

		// 波浪的状态.
		private enum MoveWaveState
		{
			none = 0,
			stopFire,
			moveInViewport,
			moveWithMask,
			moveOutViewport
		}
		private MoveWaveState curMoveWaveState  = MoveWaveState.none;

		// 背景图片占全屏的比例. 1是刚好全屏，值越小背景图就越大.
		public Vector2 bgScreenSize = new Vector2(1f, 1f);

		// 当前的背景图id.
		private int curTexIndex = 0;
		// 上一次背景图id.
		private int lastTexIndex = 0;

		// 哪一张图片当前在最前端.
		public bool upObjInUp;

		// 两张背景图obj.
		public GameObject upObj;
		public GameObject downObj;

		// 波浪.
		public Transform waveTrans;
		// 波浪的四个位置，viewpot为（1.1， 1， 0, -0.3）
		private Vector3 [] wavePos = new Vector3[8];

		// 波浪lerp percent.
		private float lerpPercent = 0f;

		private string maskUp = "maskUp";
		private string maskDown = "maskDown";

		// 两张背景图的移动方向
		private Vector2 upOffset = new Vector2(1500f, 0f);
		private Vector2 downOffset = new Vector2(0f, -1500f);		
		
		// 两张背景图片.
		public UITexture upBgTex;
		public UITexture downBgTex;
		
		// 两个背景对应的Panel
		public UIPanel upPanel;
		public UIPanel downPanel;

		// 背景接收到服务器的小心，开始计时.
		private bool startCounting = false;

		// 是否显示水纹.
		public bool showRipple = true;

		//2015-10-22   ADa禁用水波
//		public GameObject ripple0;
//		public GameObject ripple1;

		// 当前是否在切换场景.
		[HideInInspector] public bool changingWave;

		// 波浪声音.
		private AudioSource waveAudio;

		// 震动屏幕的幅度和时间.
		public Vector3 shakePower = new Vector3(0.1f, 0f, 0f);
		public float shakeTimeLength = 0.6f;

		// 切换场景时候显示的tip和显示的位置.
		public GameObject tipCache;
		public List<Transform> tips = new List<Transform>();
        // 鱼潮来袭Tip提示.
        public Transform YuChaoTip;
		public Vector3 tipShowUpViewport;

		void Awake()
		{
			Instance = this;
		
			waveAudio = GetComponent<AudioSource>();

//			waveTrans = GameObject.Find("Wave").transform;
			uiCam = Utility.GetUICam();

			// 获取背景的初始位置.
			bgOriPos = downObj.transform.position;

			// 检查图片是否为空.
//			CheckTexs();
		}

		void Start()
		{
			ReseizeBGStuff();
			InitMatOffset();
			SetWavePos();
		}

		// 初始化背景，水纹，水草大小(resize水草的父节点就可以了).
		void ReseizeBGStuff()
		{
			// resize bg.
			Utility.ResizeSprite2Screen(upObj,   uiCam, bgScreenSize.x, bgScreenSize.y, 0.5f, 0.5f);
			Utility.ResizeSprite2Screen(downObj, uiCam, bgScreenSize.x, bgScreenSize.y, 0.5f, 0.5f);

			// resize ripple.
			//2015-10-22   ADa禁用水波
//			if(showRipple)
//			{
//				Utility.ResizeSprite2Screen(ripple0, uiCam, 2f, 1f, 0.25f, 0.5f);
//				Utility.ResizeSprite2Screen(ripple1, uiCam, 2f, 1f, 0.75f, 0.5f);
//			}
//			else 
//			{
//				Destroy(ripple0);
//				Destroy(ripple1);
//			}

			// resize grass.
			for(int i=0; i<TexAndGrass.Count; i++)
			{
				TexAndGrass[0].grass.parent.localScale = upObj.transform.localScale;
			}
		}
        
		// 初始化背景材质中的背景图片，遮罩图片，shader的offset.
		void InitMatOffset()
		{
			// 
			upBgTex.mainTexture = TexAndGrass[curTexIndex].tex;
			downBgTex.mainTexture = TexAndGrass[curTexIndex].tex;
		}

		// 获取波浪移动的四个位置.
		void SetWavePos()
		{
			wavePos[0] = uiCam.ViewportToWorldPoint(new Vector3(1.3f, 0.5f, 0f));
			wavePos[1] = uiCam.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0f)); 
			wavePos[2] = uiCam.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0f));
			wavePos[3] = uiCam.ViewportToWorldPoint(new Vector3(-0.5f, 0.5f, 0f));

			wavePos[4] = uiCam.ViewportToWorldPoint(new Vector3(-0.3f, 0.5f, 0f));
			wavePos[5] = uiCam.ViewportToWorldPoint(new Vector3(-0.2f, 0.5f, 0f)); 
			wavePos[6] = uiCam.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 0f));
			wavePos[7] = uiCam.ViewportToWorldPoint(new Vector3(1.5f, 0.5f, 0f));
			waveTrans.position = wavePos[0];
			
			// 移动的速度.
			moveSpeed = 1.4f/(beforeTimeLength/1000f);
		}

		public void S_C_Init(uint serverTime, int _bgIndex, float _mapHaveRunTime)
		{
			// 计算当前和上一次使用的图片的id.
			curTexIndex 		= _bgIndex%TexAndGrass.Count;
			if(curTexIndex<=0)
			{
				lastTexIndex	= TexAndGrass.Count-1;
			}
			else 
			{
				lastTexIndex	= curTexIndex-1;
			}

			// 下一次切换场景的时间.
			uint _haveRunTime 	= (uint)(_mapHaveRunTime * 1000);
			mapTargetTime 		= serverTime + mapTimeLength - _haveRunTime - beforeTimeLength;
			startCounting 		= true;

			// 再次初始化 材质 的参数.
			InitMatOffset();

			// 显示 down obj 的.
			ShowTheUsingBGTex(false, true);

			// 隐藏波浪.
			waveTrans.gameObject.SetActive (false);

			// 获取水草.
			GetGrass();

			// 开启背景对应的水草.
			//InitGrass();
		}

		// 只显示当前使用的背景图片.
		void ShowTheUsingBGTex(bool _showUpObj, bool _showDownObj)
		{
			upObj.SetActive(_showUpObj);
			downObj.SetActive(_showDownObj);
		}

		//  ===== 水草 =====================================.
		// 获取全部水草.
		void GetGrass()
		{
			for(int i=0; i<TexAndGrass.Count; i++)
			{
				for(int j=0; j<TexAndGrass[i].grass.childCount; j++)
				{
					Transform _child = TexAndGrass[i].grass.GetChild(j);
					TexAndGrass[i].grassItemList.Add(_child);
					_child.gameObject.SetActive(false);
				}
				TexAndGrass[i].grassItemNum = TexAndGrass[i].grassItemList.Count;
			}
		}

		// 开启背景对应的水草.
		void InitGrass()
		{
			for(int i=0; i<TexAndGrass[curTexIndex].grassItemNum; i++)
			{
				TexAndGrass[curTexIndex].grassItemList[i].gameObject.SetActive(true);
// 				TexAndGrass[curTexIndex].grassItemList[i].GetComponent<SpriteRenderer>().enabled = true;
// 				TexAndGrass[curTexIndex].grassItemList[i].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
                TexAndGrass[curTexIndex].grassItemList[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
			}
		}

		// 更新水草.
		private bool haveActiveNextGrass = false;
		private bool haveFadeInNextGrass = true;
		private float haveFadeInNextGrassPercent = 0f;
		void UpdateGrass()
		{
			switch(curMoveWaveState)
			{
			case MoveWaveState.moveInViewport:
				// 先打开下一个场景里的水草. 并且把 alpha 设置为0. 不让其显示.
				OpenNextGrass();
				break;
			case MoveWaveState.moveWithMask:
				// 波浪移到水草的位置时候，就把上一个场景里的水草一个个地隐藏掉.
				for(int i=0; i<TexAndGrass[lastTexIndex].grassItemNum; i++)
				{
					if(TexAndGrass[lastTexIndex].grassItemList[i].gameObject.activeSelf && waveTrans.position.x<TexAndGrass[lastTexIndex].grassItemList[i].position.x)
					{
						TexAndGrass[lastTexIndex].grassItemList[i].gameObject.SetActive(false);
					}
				}
				break;
			case MoveWaveState.moveOutViewport:
				if(haveActiveNextGrass)
				{
					haveFadeInNextGrass = false;
					haveFadeInNextGrassPercent = 0f;
					haveActiveNextGrass = false;
				}
				break;
			case MoveWaveState.none:
				// 开始显示水草.
				FadeInNextGrass();
				break;
			}
		}

		//  打开下一个场景里的水草. 并且把 alpha 设置为0. 不让其显示.
		void OpenNextGrass()
		{
			if(haveActiveNextGrass)
			{
				return;
			}
			for(int i=0; i<TexAndGrass[curTexIndex].grassItemNum; i++)
			{
				TexAndGrass[curTexIndex].grassItemList[i].gameObject.SetActive(true);
// 				TexAndGrass[curTexIndex].grassItemList[i].GetComponent<SpriteRenderer>().enabled = true;
// 				TexAndGrass[curTexIndex].grassItemList[i].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
                TexAndGrass[curTexIndex].grassItemList[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0f);
			}
			haveActiveNextGrass = true;
		}

		// 开始显示水草.
		void FadeInNextGrass()
		{
			if(haveFadeInNextGrass)
			{
				return;
			}
			haveFadeInNextGrassPercent += Time.deltaTime*0.5f;
			for(int i=0; i<TexAndGrass[curTexIndex].grassItemNum; i++)
			{
//				TexAndGrass[curTexIndex].grassItemList[i].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,haveFadeInNextGrassPercent);
                TexAndGrass[curTexIndex].grassItemList[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, haveFadeInNextGrassPercent);
			}
			if(haveFadeInNextGrassPercent>=1)
			{
				haveFadeInNextGrass = true;
				haveFadeInNextGrassPercent = 0f;
			}
		}
		// ================================ Grass =====================================.


		
		// 震动背景.
		public void ShakeCamera()
		{
			iTween.Stop(downObj.gameObject);
			iTween.ShakePosition(downObj.gameObject, shakePower, shakeTimeLength);
		}
		// 震动屏幕后，将背景图片往北京的初始位置拽.
//		void AjustCam()
//		{
//			if(upObjInUp)
//			{
//				upObj.transform.localPosition = Vector3.Lerp(upObj.transform.position, bgOriPos, Time.deltaTime);
//			}
//			else 
//			{
//				downObj.transform.localPosition = Vector3.Lerp(downObj.transform.position, bgOriPos, Time.deltaTime);
//			}
//		}

		void FixedUpdate()
		{
			// 计时切换场景.
			if(startCounting)
			{
				if(TimeCtrl.Instance.serverTime>=mapTargetTime)
				{
					waveTrans.gameObject.SetActive (true);
					StartChangeMap();
                    CreatYuChaoTip();
					mapTargetTime = TimeCtrl.Instance.serverTime + mapTimeLength;
				}
			}
		}


		void Update () 
		{

//			AjustCam();
			UpdateGrass();


			switch(curMoveWaveState)
			{
				// 停止发泡等待3秒.
			case MoveWaveState.stopFire:
				lerpPercent += Time.deltaTime;
				if(lerpPercent>3f)
				{
					lerpPercent = 0f;
					ChangeMap();
					curMoveWaveState = MoveWaveState.moveInViewport;
				}
				break;

				// 波浪从 wavePos[0] 移动到 wavePos[1].
			case MoveWaveState.moveInViewport:
				// 1f/0.1f.
				lerpPercent += Time.deltaTime * moveSpeed * 10f;
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){					
					waveTrans.position = Vector3.Lerp(wavePos[4], wavePos[5], lerpPercent);	
				}else{					
					waveTrans.position = Vector3.Lerp(wavePos[0], wavePos[1], lerpPercent);	
				}
				
				if(lerpPercent>=1f)
				{
					lerpPercent = 0f;
					curMoveWaveState = MoveWaveState.moveWithMask;
				}
				break;

				// 波浪从 wavePos[1] 移动到 wavePos[2]. 与此同时两个背景图片开始做遮罩.
			case MoveWaveState.moveWithMask:
				lerpPercent += Time.deltaTime * moveSpeed;
				// mask.
				if(upObjInUp)
				{
					float _offset = Mathf.Lerp(upOffset.x, upOffset.y, lerpPercent);
					Vector2 temp = upPanel.clipOffset;
					temp.x = _offset;
					upPanel.clipOffset = temp;
					
					float _offset2 = Mathf.Lerp(downOffset.x, downOffset.y, lerpPercent);
					Vector2 temp2 = downPanel.clipOffset;
					temp2.x = _offset2;
					downPanel.clipOffset = temp2;
				}
				else 
				{
					float _offset = Mathf.Lerp(upOffset.x, upOffset.y, lerpPercent);
					Vector2 temp = downPanel.clipOffset;
					temp.x = _offset;
					downPanel.clipOffset = temp;
					
					float _offset2 = Mathf.Lerp(downOffset.x, downOffset.y, lerpPercent);
					Vector2 temp2 = upPanel.clipOffset;
					temp2.x = _offset2;
					upPanel.clipOffset = temp2;
				}

				// wave.
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
					waveTrans.position = Vector3.Lerp(wavePos[5], wavePos[6], lerpPercent);
				}else{
					waveTrans.position = Vector3.Lerp(wavePos[1], wavePos[2], lerpPercent);
				}

				if(lerpPercent>=1f)
				{
					curMoveWaveState = MoveWaveState.moveOutViewport;
					lerpPercent = 0f;
					
					// record last map fish.
					FishCtrl.Instance.RecordLastMapFish();
					ClearLastMapFish();

					waveAudio.Stop();
				}
				break;
			
				// 波浪从 wavePos[2] 移动到 wavePos[3].
			case MoveWaveState.moveOutViewport:
				// 1f/0.3f;
				lerpPercent += Time.deltaTime * moveSpeed * 3.33f;
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){					
					waveTrans.position = Vector3.Lerp(wavePos[6], wavePos[7], lerpPercent);
				}else{					
					waveTrans.position = Vector3.Lerp(wavePos[2], wavePos[3], lerpPercent);
				}

				if(lerpPercent>=1f)
				{
					curMoveWaveState = MoveWaveState.none;
					ResetSortingLayer();

					// 移动完了只显示当前使用的背景图.
					if(upObjInUp)
					{
						ShowTheUsingBGTex(true, false);
					}
					else 
					{
						ShowTheUsingBGTex(false, true);
					}
					
					// 播放下一首背景歌曲.
					AudioCtrl.Instance.NextBG();

					// 关闭海浪.
					waveTrans.gameObject.SetActive (false);

					// 恢复发泡和锁定状态.
					CanonCtrl.Instance.ResumeFireAndResumeLockState();
				}
				break;
			}
		}
		

		// 清除上个场景里的鱼.
		void ClearLastMapFish()
		{
			FishCtrl.Instance.ClearLastMapFish();
		}

		// 切换背景完毕后就重新设置背景的显示层.
		void ResetSortingLayer()
		{
//			ForCameraShakeInit();
			if(upObjInUp)
			{
				upObj.GetComponent<UIPanel>().depth = -5;
				downObj.GetComponent<UIPanel>().depth = 20;
			}
			else 
			{
				upObj.GetComponent<UIPanel>().depth = 20;
				downObj.GetComponent<UIPanel>().depth = -5;
			}
			changingWave = false;
		}

		// 停止发泡3秒，然后开始切换场景.
		void StartChangeMap()
		{
			if(curMoveWaveState != MoveWaveState.none)
			{
				return;
			}
			// 记录上一次发泡和锁定的状态.
			CanonCtrl.Instance.StopFireAndRecordLastLockState();

			lerpPercent = 0f;
			curMoveWaveState = MoveWaveState.stopFire;
			// 在第一阶段就禁止玩家发泡(3秒的禁炮才能实现)
			changingWave = true;
		}

		// 开始切换场景.
		void ChangeMap ()
		{
			// 下一次和上一次图片的id.
			curTexIndex += 1;
			if(curTexIndex>=TexAndGrass.Count)
			{
				curTexIndex = 0;
			}
			if(curTexIndex<=0)
			{
				lastTexIndex = TexAndGrass.Count-1;
			}
			else 
			{
				lastTexIndex = curTexIndex-1;
			}

			lerpPercent = 0f;
			curMoveWaveState = MoveWaveState.moveInViewport;

			// 播放海浪声音.
			waveAudio.Play();

			// 随机播放一个tip.
			Transform _tip = tips[UnityEngine.Random.Range(0, tips.Count)];
			if(_tip!=null)
			{
				Quaternion _rot;
				// 如果符合旋转条件,就对tip进行旋转生成.
				if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
					_rot = new Quaternion(0.0f,0.0f,1.0f,0.0f);
				}else{
					_rot = new Quaternion();
				}
				Transform _temp = Factory.Create(_tip, uiCam.ViewportToWorldPoint(tipShowUpViewport),_rot);
				_temp.parent = tipCache.transform;
                _temp.localScale = Vector3.one;
			}

			// 两张背景图打开.
			ShowTheUsingBGTex(true, true);

			// 通过两张背景图片层级关系决定这次切换背景使用哪一个作为背景.
			if(upObj.GetComponent<UIPanel>().depth > downObj.GetComponent<UIPanel>().depth)
			{
				upObjInUp = true;
				
				downObj.GetComponent<UIPanel>().depth = -5;
				downPanel.clipOffset = new Vector2(0f,0f);
				
				upObj.GetComponent<UIPanel>().depth = 20;
				upPanel.clipOffset = new Vector2(-1500f,0f);
				
				upBgTex.mainTexture = TexAndGrass[curTexIndex].tex;
			}
			else 
			{
				upObjInUp = false;

				downObj.GetComponent<UIPanel>().depth = 20;
				downPanel.clipOffset = new Vector2(-1500f,0f);
				
				upObj.GetComponent<UIPanel>().depth = -5;
				upPanel.clipOffset = new Vector2(0f,0f);
				
				downBgTex.mainTexture = TexAndGrass[curTexIndex].tex;
			}
		}
		
		void OnDestroy()
		{
			InitMatOffset();
			Instance = null;
		}

		void OnApplicationQuit()
		{
			InitMatOffset();
		}
		
		/*void OnGUI()
		{
			if(!MessageHandler.Instance.showAllGui)
			{
				return;
			}
			
			GUILayout.Space(300f);
			GUILayout.Label(" serverTime   = "+TimeCtrl.Instance.serverTime);
			GUILayout.Label("mapTargetTime = "+mapTargetTime);
			GUILayout.Space(20f);
			GUILayout.Label("upOffset = "+upOffset.ToString("N4"));
			GUILayout.Label("downOffset = "+downOffset.ToString("N4"));
//			GUILayout.Label("upMat.SetTextureOffset = "+upMat.GetTextureOffset("_Mask").ToString("N4"));
//			GUILayout.Label("downMat.SetTextureOffset = "+downMat.GetTextureOffset("_Mask").ToString("N4"));
		}	*/	
		
		void CheckTexs()
		{
			for(int i=0; i<TexAndGrass.Count; i++)
			{
				if(upObj==null || downObj==null)
				{
					Debug.LogError("upObj or downObj is null");
				}
				if(TexAndGrass[i]==null)
				{
					Debug.LogError("one of BG texture is null which attaching to the GameObject of WaveCtrl");
				}
			}
		}

        void CreatYuChaoTip()
        {
            // 创建一个鱼潮tip.
            if (YuChaoTip != null)
            {
                Quaternion _rot;
                // 如果符合旋转条件,就对tip进行旋转生成.
                if (CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of)
                {
                    _rot = new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
                }
                else
                {
                    _rot = new Quaternion();
                }
                Transform _temp = Factory.Create(YuChaoTip, uiCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), _rot);
                _temp.parent = tipCache.transform;
                _temp.localScale = Vector3.one;
            }
        }
	}
}
