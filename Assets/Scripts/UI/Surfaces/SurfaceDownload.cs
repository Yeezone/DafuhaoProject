using System;
using UnityEngine;
using com.QH.QPGame.GameUtils;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.Lobby.Surfaces
{
    public class SurfaceDownload : Surface
    {
        public GameObject ObjUpdate;
        public GameObject ObjTitle;
        public GameObject ObjText;
        public GameObject ObjFileText;
        public GameObject ObjSlider;
        public GameObject ObjCancelBtn;
        public GameObject ObjRetryBtn;

        public Action<int> OnDownloaded;

        private Stack<int> tasks = new Stack<int>();
        private int currentGameID = -1;

        void OnDestroy()
        {
            if (currentGameID != -1)
            {
                currentGameID = -1;
                GameApp.Updater.CancelDownload(currentGameID);
            }
        }

        public void AddTasks(List<int> gameIDs, Action<int> callback)
        {
            foreach (var gameID in gameIDs)
            {
                if (!tasks.Contains(gameID))
                {
                    Logger.UI.Log("add download task. GameID:" + gameID);
                    tasks.Push(gameID);
                }
            }

            OnDownloaded = callback;
            currentGameID = -1;
            StartCoroutine(DownloadGame());
        }

        public void AddTask(int gameID, Action<int> callback)
        {
            tasks.Push(gameID);
            OnDownloaded = callback;
            currentGameID = -1;
            StartCoroutine(DownloadGame());
        }

        public void CancelTask()
        {
            //TODO 移除
            GameApp.PopupMgr.MsgBox("确认", "是否取消下载？", ButtonStyle.Cancel | ButtonStyle.OK,
                delegate(MessageBoxResult result)
                {
                    if (result == MessageBoxResult.OK)
                    {
                        ObjUpdate.SetActive(false);
                        if (currentGameID != -1)
                        {
                            currentGameID = -1;
                            GameApp.Updater.CancelDownload(currentGameID);
                        }
                    }
                });
        }

        public void RetryTask()
        {
            if (currentGameID != -1)
            {
                tasks.Push(currentGameID);
                currentGameID = -1;
                StartCoroutine(DownloadGame());
            }
        }

        private IEnumerator DownloadConfig()
        {
            if (GameApp.Updater.RemoteConfigLoaded)
            {
                yield break;
            }

            var slider = ObjSlider.GetComponent<UISlider>();
            var label = ObjText.GetComponent<UILabel>();
            var title = ObjTitle.GetComponent<UILabel>();
            var file = ObjFileText.GetComponent<UILabel>();

            title.text = "读取配置";

            GameApp.Updater.Run();

            while (!GameApp.Updater.Error && !GameApp.Updater.RemoteConfigLoaded)
            {
                file.text = GameApp.Updater.DownloadedCount + "/" + GameApp.Updater.TotalFileCount;
                slider.value = GameApp.Updater.Progress;
                label.text = GameApp.Updater.Status + " " + GameApp.Updater.ProgressStr;
                yield return null;
            }

            if (GameApp.Updater.Error)
            {
                file.text = "";
                title.text = "出错了";
                label.text = GameApp.Updater.ErrorStr;
                ObjRetryBtn.SetActive(true);
                ObjCancelBtn.SetActive(false);
            }
        }

        private IEnumerator DownloadGame()
        {
            if (currentGameID != -1)
            {
                yield break;
            }

            ObjUpdate.SetActive(true);
            ObjRetryBtn.SetActive(false);
            ObjCancelBtn.SetActive(true);

            var slider = ObjSlider.GetComponent<UISlider>();
            var label = ObjText.GetComponent<UILabel>();
            var title = ObjTitle.GetComponent<UILabel>();
            var file = ObjFileText.GetComponent<UILabel>();

            yield return StartCoroutine(DownloadConfig());

            title.text = "游戏下载";
            
            while (tasks.Count > 0)
            {
                currentGameID = tasks.Pop();

                GameApp.Updater.DownloadGame(currentGameID);

                while (!GameApp.Updater.Done)
                {
                    file.text = GameApp.Updater.DownloadedCount + "/" + GameApp.Updater.TotalFileCount;
                    slider.value = GameApp.Updater.Progress;
                    label.text = GameApp.Updater.Status + " " + GameApp.Updater.ProgressStr;
                    yield return null;
                }

                if (GameApp.Updater.Error)
                {
                    file.text = "";
                    title.text = "出错了";
                    label.text = GameApp.Updater.ErrorStr;
                    ObjRetryBtn.SetActive(true);
                    ObjCancelBtn.SetActive(false);
                    yield break;
                }
            }

            if (!GameApp.Updater.Error && GameApp.Updater.Done)
            {
                if (OnDownloaded != null)
                {
                    OnDownloaded(currentGameID);
                }
            }

            ObjUpdate.SetActive(false);
            currentGameID = -1;
        }

    }
}


