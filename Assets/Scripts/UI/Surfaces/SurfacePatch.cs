using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;
using System;


namespace com.QH.QPGame.Lobby.Surfaces
{
    public class SurfacePatch : Surface
    {
        private GameObject loading;
        private UILabel uiLabel;
        private float startTime;
		private bool patching = false;

        void OnEnable()
        {
            patching = false;
            startTime = 0.0f;

            GameApp.Network.NetworkStatusChangeEvent += HandleNetworkStatusChangeEvent; //注册网络出错事件
            GameApp.Account.VersionInfoEvent += Instance_HallVersionInfoEvent;
        }

        void OnDisable()
        {
            patching = false;
            startTime = 0.0f;

            GameApp.Network.NetworkStatusChangeEvent -= HandleNetworkStatusChangeEvent; //注册网络出错事件
            GameApp.Account.VersionInfoEvent -= Instance_HallVersionInfoEvent;

            HideStatus();
        }

        void Update()
        {
            if (patching && (Time.realtimeSinceStartup - startTime >= 20.0f))
            {
                patching = false;

                HideStatus();

                GameApp.PopupMgr.Confirm("更新出错", "检测超时", delegate(MessageBoxResult style)
                       {
                           GameApp.GetInstance().SwitchAccount();
                       });
            }
        }

        private void HandleNetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
			if (wError == NetworkManager.Status.Connected)
            {
                patching = true;
                startTime = Time.realtimeSinceStartup;

              //  ShowStatus("获取远程版本");

                GameApp.Account.ReqGetVersionInfo();
            }
            else
            {
                startTime = 0.0f;
                patching = false;
                HideStatus();
            }
        }

        private void Instance_HallVersionInfoEvent(int version)
        {
            string text = string.Format(
                "version info, local:{0} remote:{1}",
                GameApp.GameData.Version,
                GameVersion.Version2Str(version));
            Logger.UI.Log(text);

            GameApp.Updater.RemoteVersion = version;

            if (GameApp.Updater.IsForceUpdate())
            {
                text = string.Format(
                    "发现新的版本:{0}，准备开始更新。当前版本：{1}",
                    GameVersion.Version2Str(version),
                    GameApp.GameData.Version);
                GameApp.PopupMgr.Confirm("更新", text, delegate(MessageBoxResult style)
                        {
                            StartCoroutine(CheckUpdate());
                        },9999f);
            }
            else if(version != GameVersion.ProcessVersion(GameApp.GameData.Version))
            {
                StartCoroutine(CheckUpdate());
            }
            else
            {
                startTime = 0.0f;
                patching = false;
            }
        }

        private void ShowStatus(string text)
        {
            StartCoroutine(LoadAndShowStatus(text));
        }

        private void HideStatus()
        {
            UnLoadRes();
/*
            if (this.gameObject.activeSelf)
            {
                float delay = Math.Max(Time.realtimeSinceStartup - startLoading, 0.5f);
                Invoke("UnLoadRes", delay);
            }
            else
            {
                UnLoadRes();
            }*/
        }

        private IEnumerator LoadAndShowStatus(string text)
        {
            if (uiLabel == null)
            {
                yield return StartCoroutine(LoadRes());
            }

            if (uiLabel == null)
            {
                yield break;
            }

            uiLabel.text = text;
        }

        private IEnumerator LoadRes()
        {
            yield return StartCoroutine(
                   GameApp.ResMgr.LoadAssetAsyncInResources<GameObject>(
                   GlobalConst.UI.UI_PREFAB_LOADING_COMMON)
                   );
            var go = GameApp.ResMgr.GetLoadedAssetLoadFromResources<GameObject>(
                GlobalConst.UI.UI_PREFAB_LOADING_COMMON
                );
            if (go == null)
            {
                Logger.UI.LogError("load res failed, Prefab:" + GlobalConst.UI.UI_PREFAB_LOADING_COMMON);
                yield break;
            }

            loading = Instantiate(go);
            uiLabel = loading.GetComponentInChildren<UILabel>();
        }

        private void UnLoadRes()
        {
            if (loading != null)
            {
                Destroy(loading);
                loading = null;
                uiLabel = null;
            }
        }

        private IEnumerator CheckUpdate()
        {
            yield return StartCoroutine(LoadAndShowStatus("检查更新中"));

            GameApp.Updater.Run();

            while (!GameApp.Updater.Done)
            {
                if (uiLabel != null)
                {
                    uiLabel.text = GameApp.Updater.Status;
                }
                yield return null;
            }

            yield return null;

            patching = false;

            if (GameApp.Updater.Done && 
                GameApp.Updater.IsForceUpdate()) 
			{
                GameApp.Network.CloseAllSocket();
				GameApp.Updater.FullUpdate();
				yield break;
			}

            if (GameApp.Updater.Error)
            {
                GameApp.PopupMgr.Confirm("更新出错", GameApp.Updater.ErrorStr, delegate(MessageBoxResult style)
                    {
                        GameApp.GetInstance().SwitchAccount();
                    }, 999);
            }
            else
            {
                var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
                var login = container.GetSurface<SurfaceLogin>();
                login.SendLogonDataToUi();
            }

            gameObject.SetActive(false);

        }
    }
}


