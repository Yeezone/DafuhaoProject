using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Lobby.Surfaces;

namespace com.QH.QPGame.Lobby
{
    public class SceneManager : MonoBehaviour
    {
        public bool IsLoading
        {
            get { return ao != null; }
        }

        public float Progress
        {
            get
            {
                return ao != null ? ao.progress : 0f;
            }
        }

        public bool ActiveScene
        {
            set { if(ao != null) ao.allowSceneActivation = value; }
            get { return ao != null ? ao.allowSceneActivation : false; }
        }

        public bool Error;

        public string LastScene { get; set; }

        private AsyncOperation ao;

        public void Initialize()
        {
            LastScene = null;
            ao = null;

            if (string.Compare(Application.loadedLevelName, GlobalConst.UI.UI_SCENE_MAIN, true) != 0)
            {
                //GameApp.ResMgr.UnloadLevel(showSceneName);
                Logger.Sys.Log("Loading Scene:" + GlobalConst.UI.UI_SCENE_MAIN);
                Application.LoadLevel(GlobalConst.UI.UI_SCENE_MAIN);
            }
            else
            {
                Logger.Sys.Log("Loading Scene:" + GlobalConst.UI.UI_SCENE_LOGIN);
                Application.LoadLevel(GlobalConst.UI.UI_SCENE_LOGIN);
            }
	    }

        public void EnterScene(
            string sceneName, 
            bool showLoading = false, 
            bool autoSwitch = false,
            string loadingPrefab = "",
            string abName = ""
            )
		{
            Logger.Sys.Log("enter scene. Name:" + sceneName + " WithLoading:" + loadingPrefab);

            Error = false;

            if (showLoading)
            {
                var loadingObj = new GameObject("loading");
                var loading = loadingObj.AddComponent<SurfaceLoading>();
                loading.Show(sceneName, loadingPrefab, abName);
            }

            LastScene = Application.loadedLevelName;

            StartCoroutine(LoadScene(sceneName, showLoading, autoSwitch));
		}

        public void QuitScene()
        {
            EnterScene(LastScene, false, true);
        }
		
        private IEnumerator LoadScene(
            string sceneFile,
            bool showLoading,
            bool autoSwitch
            )
        {
            string sceneName = System.IO.Path.GetFileName(sceneFile);

#if !UNITY_EDITOR || TEST_AB

            if (!string.IsNullOrEmpty(LastScene) || ao != null)
            {
                GameApp.ResMgr.UnloadLevel(LastScene, true);
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                yield return StartCoroutine(GameApp.ResMgr.LoadLevel(sceneFile));
            }
#endif

            ao = Application.LoadLevelAsync(sceneName);
            if (ao == null)
            {
                Error = true;
                yield break;
            }

            ao.allowSceneActivation = !showLoading || autoSwitch;
            while (ao != null && ao.progress <= 0.9f) yield return new WaitForFixedUpdate();
            if (ao == null)
            {
                Error = true;
                yield break;
            }


            ao.allowSceneActivation = !showLoading || autoSwitch;
            yield return ao;
            ao = null;

#if !UNITY_EDITOR || TEST_AB
            GameApp.ResMgr.UnloadLevel(sceneFile, false);
#endif

            foreach (var item in UIRoot.list)
            {
                var container = item.gameObject.GetComponent<SurfaceContainer>();
                if (container != null)
                {
                    container.SwitchSurface(sceneName);
                }
            }
           
        }
	}
}

