using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using com.QH.QPGame.GameUtils;
	

namespace com.QH.QPGame.Lobby.Surfaces
{
	public class SurfaceLoading : Surface 
    {
		public UISlider SliderWidget;
	    public UISprite Backgroud;
	    public UILabel Label;

		private float fakeSpeed=0.01f;
		private float curVal=0.0f;
		private bool  isComplete=false;

        private GameObject loading;
        private UISlider uiSlider;
        private UILabel uiLabel;

        internal class TipsData
        {
            public string Tips;
            public float Duration;
            public bool UseAnimation;
        }

        private Queue<TipsData> tipsQueue = new Queue<TipsData>();
        private TipsData currentTips = null;
        private string textAnimation = "";

	    private float beginTime = 0.0f;

	    private string abName = "";

	    private void OnDestroy()
	    {
            if (loading != null)
            {
                uiSlider = null;
                uiLabel = null;
                Destroy(loading);
                loading = null;
            }
	    }

	    public void Show(string scene, string prefab, string abName)
	    {
            Logger.UI.Log("begin show loading. Scene:"+scene+" Prefab:"+prefab+" Assetbundle:"+abName);

            beginTime = Time.realtimeSinceStartup;
	        StartCoroutine(Load(scene, prefab, abName));
	    }

	    private IEnumerator Load(string scene, string prefab, string abName)
	    {
	        if (string.IsNullOrEmpty(prefab))
	        {
	            prefab = GlobalConst.UI.UI_PREFAB_LOADING;
	        }

	        yield return StartCoroutine(
	            GameApp.ResMgr.LoadAssetAsyncInResources<GameObject>(prefab));
	        var go = GameApp.ResMgr.GetLoadedAssetLoadFromResources<GameObject>(prefab);
	        loading = Instantiate(go);
	        if (loading == null)
	        {
	            Logger.UI.LogError("no match loading prefabs loaded");
	            yield break;
	        }

	        loading.SetActive(true);
	        uiSlider = loading.GetComponentInChildren<UISlider>();
	        var items = loading.GetComponentsInChildren<UILabel>();
	        uiLabel = Array.Find(items, label => label.gameObject.name == "Lable_tips");

            SetTips("加载场景中", false, 999.0f);
            yield return null;

            
#if !UNITY_EDITOR || TEST_AB
            
	        if (GameApp.ResMgr.IsCachedLessThan(GlobalConst.Update.CachingSizeWarning))
	        {
	            SetTips("缓存空间快用尽了，可能无法释放游戏", false, 999.0f);
	            yield return null;
	        }

	        if (scene.StartsWith("games"))
            {
                if (!GameApp.ResMgr.IsSceneCached(scene))
	            {
	                SetTips("首次加载需要更多的时间", false, 999.0f);
	                yield return null;
	            }
            }
#endif

            while (GameApp.SceneMgr.Progress < 0.9f)
	        {
	            yield return new WaitForSeconds(0.1f);
	        }

	        if (GameApp.SceneMgr.Error)
	        {
	            SetTips("加载失败，可能是缓存空间不足", false, 999);
	            yield return new WaitForSeconds(3f);
	            GameApp.SceneMgr.QuitScene();
	        }
	        else
	        {
	            GameApp.SceneMgr.ActiveScene = true;
	        }

	    }

	    void Update()
	    {
            if (Time.realtimeSinceStartup - beginTime >
                GlobalConst.Base.LoadingTimeOut)
            {
                GameApp.SceneMgr.QuitScene();
                return;
            }

            UpdateTips(Time.deltaTime);
	        UpdateProgress(GameApp.SceneMgr.Progress);
	    }


	    public void AddTips(string str, bool useAnimation = false, float duration = 1.0f)
        {
            tipsQueue.Enqueue(new TipsData() { Tips = str, Duration = duration, UseAnimation = useAnimation });
        }

        public void SetTips(string str, bool useAnimation = false, float duration = 1.0f)
        {
            Debug.Log("set tips:"+str);

            currentTips = null;
            tipsQueue.Clear();
            currentTips = new TipsData() { Tips = str, Duration = duration, UseAnimation = useAnimation };
        }

        public void CleanAndHideTips()
        {
            tipsQueue.Clear();
            uiLabel.gameObject.SetActive(false);
        }

        public void UpdateTips(float time)
        {
            string text = "";
            if (currentTips != null && currentTips.Duration >= time)
            {
                currentTips.Duration -= time;
            }
            else
            {
                if (tipsQueue.Count != 0)
                {
                    currentTips = tipsQueue.Dequeue();
                    textAnimation = "";
                }
                else
                {
                    currentTips = null;
                    text = "";
                }
            }

            if (currentTips != null)
            {
                if (currentTips.UseAnimation)
                {
                    textAnimation += ".";
                    if (textAnimation.Length > 3)
                    {
                        textAnimation = ".";
                    }
                }

                text = currentTips.Tips + textAnimation;
            }

            if (uiLabel != null)
            {
                uiLabel.text = text;
            }

        }

        public void UpdateProgress(float progress)
        {
            if (uiSlider != null)
            {
                uiSlider.value = progress;
            }
        }

	   /* private void StartProgress ()
		{
			SliderWidget.value=0.0f;

			StartCoroutine(UpdateProgress());
		}

		IEnumerator UpdateProgress()
		{

			while (isComplete == false)
			{
				curVal += fakeSpeed * Time.deltaTime;

				if (curVal <0.8f)
				{
					SliderWidget.value = curVal;
				}
				else
				{
//					SliderWidget.value = 1f;
//					isComplete = true;
					break;
				}

			    yield break;
			}
		}


		public void CompleteLoading()
		{
//			fakeSpeed=1.0f;

			SliderWidget.value = 1f;
			isComplete = true;
		}
        */


	}
}


