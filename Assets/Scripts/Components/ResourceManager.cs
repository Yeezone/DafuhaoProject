using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;

using UnityEngine;
using LOAssetFramework;

namespace com.QH.QPGame.Lobby
{
    // <summary>
    // 资源管理器，实现资源包读取，屏蔽开发模式以及随包、下载目录等路径细节
    // Version2 实现按队列加载，实现精细化管理
    // @Author: guofeng
    // </summary>
    public class ResourceManager : MonoBehaviour
    {
        public enum BundleState
        {
            NotFound,
            OutOfDate,
            Invalid
        }


        private LOAssetManager[] abMgr = null;

        private List<string> downloadedABs = new List<string>();
        private Hashtable cachedFiles = new Hashtable();
        private Dictionary<string, AssetBundleInfo> localResConfig = new Dictionary<string, AssetBundleInfo>();
        private Dictionary<string, AssetBundleInfo> remoteResConfig = new Dictionary<string, AssetBundleInfo>();

        public bool ResReady { get; private set; }

        private string downloadPath = GameHelper.GetDownloadPath(false);
        private string downloadLoadPath = GameHelper.GetDownloadPath();

        public bool Initialize()
        {
            Logger.Res.Log("Res System Init");
            
            Reset();

#if !UNITY_EDITOR || TEST_AB
            if(abMgr == null)
            {
                abMgr = new LOAssetManager[2];
                for (int i = 0; i < 2; i++)
                {
                    abMgr[i] = gameObject.AddComponent<LOAssetManager>();
                    abMgr[i].URI = i == 0 ? GameHelper.GetInternalPath(true) : GameHelper.GetDownloadPath(true);
                }
            }

#endif
            StartCoroutine(InternalInit());

            return true;
        }

        private IEnumerator LoadConfig()
        {
            yield return StartCoroutine(
                LoadTextFileAsync(
                    GlobalConst.Res.AppConfigFileName)
                );

            yield return StartCoroutine(
                LoadTextFileAsync(
                    GlobalConst.Res.ResVersionFileName)
                );

            yield return StartCoroutine(
                LoadTextFileAsync(
                    GlobalConst.Res.GameConfigFileName)
                );

            GameApp.Options = LoadGameOptions();
            GameApp.GameData.Initialize();
            GameApp.Settings.Load();

            LoadResConfig();
        }

        private IEnumerator InternalInit()
        {
            ScanDownloadedFiles();

            yield return StartCoroutine(LoadConfig());

#if !UNITY_EDITOR || TEST_AB
            if (GameApp.Options.EnableCacheAB && 
                GameApp.Options.AutoCleanCache &&
                Caching.enabled )
            {
                while (!Caching.ready)
                {
                    yield return null;
                }

                if ( string.Compare(GameApp.Options.Version, 
                    GameApp.GameData.InstallVersion, true) != 0)
                {
                    Logger.Res.Log("clean all cache and download files.PreVer:"+GameApp.GameData.InstallVersion);

                    CleanCache();
                    DeleteAllDownloadedFiles(true);
                
                    yield return StartCoroutine(LoadConfig());
                }
            }
#endif

            ResReady = true;
        }

        public void Reset()
        {
            ResReady = false;
            cachedFiles.Clear();
            downloadedABs.Clear();
            localResConfig.Clear();
            remoteResConfig.Clear();
        }

        public void ScanDownloadedFiles()
        {
            Logger.Res.Log("scan files in path:" + downloadPath);

            try
            {
                downloadedABs.Clear();
                if (!Directory.Exists(downloadPath))
                {
                    Logger.Res.Log("download path is not exists Path:" + downloadPath);
                    return;
                }

                var files = Directory.GetFiles(downloadPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    //TODO 校验文件md5
                    string fileName = file.Replace(downloadPath, "").Replace("\\", "/");
                    //Logger.Res.Log("add file:" + fileName);
                    downloadedABs.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
        }

        public void DeleteAllDownloadedFiles(bool cleanCache)
        {
            try
            {
                if (Directory.Exists(downloadPath))
                {
                    Directory.Delete(downloadPath, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
        }

        public void CleanCache()
        {
            if (Caching.enabled && Caching.ready)
            {
                Caching.CleanCache();
            }
        }


        public void LoadRemoteResConfigFromString(string text)
        {
            try
            {
                remoteResConfig.Clear();
                var remoteResVer = LitJson.JsonMapper.ToObject<List<AssetBundleInfo>>(text);
                foreach (var assetBundleInfo in remoteResVer)
                {
                    if (remoteResConfig.ContainsKey(assetBundleInfo.FileName))
                    {
                        Logger.Res.LogError(assetBundleInfo.FileName + " 已经存在");
                        return;
                    }
                    remoteResConfig.Add(assetBundleInfo.FileName, assetBundleInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
        }

        public GameOptions LoadGameOptions()
        {
            try
            {
                string text = GetCachedTextFile(GlobalConst.Res.AppConfigFileName);
                if (string.IsNullOrEmpty(text))
                {
                    Logger.Res.LogError("load app config file failed.");
                    return null;
                }

//#if !DISABLE_ENCRYPT_FILES && !UNITY_EDITOR
//            text = DESEncryption.Decrypt(text, GlobalConst.Res.EncryptPassword);
//#endif

                return LitJson.JsonMapper.ToObject<GameOptions>(text);
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
            return null;
        }

        public void SaveGameOptions(GameOptions options)
        {
            string text = LitJson.JsonMapper.ToJson(options);

//#if !DISABLE_ENCRYPT_FILES && !UNITY_EDITOR
//            text = DESEncryption.Encrypt(text, GlobalConst.Res.EncryptPassword);
//#endif

            SaveTextFile(GlobalConst.Res.AppConfigFileName, text);
        }

        /// <summary>
        /// 加载本地资源配置文件
        /// </summary>
        public void LoadResConfig()
        {
            try
            {
                localResConfig.Clear();
                string text = GetCachedTextFile(GlobalConst.Res.ResVersionFileName);
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                var configData = LitJson.JsonMapper.ToObject<List<AssetBundleInfo>>(text);
                if (configData == null)
                {
                    Logger.Sys.LogError("load res version file failed. Text:" + text);
                    return;
                }

                localResConfig = new Dictionary<string, AssetBundleInfo>();
                foreach (var config in configData)
                {
                    // Logger.Sys.Log("add res config :" + config.FileName);
                    localResConfig.Add(config.FileName, config);
                }

            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
        }

        /// <summary>
        /// 保存资源配置文件
        /// </summary>
        public void SaveResConfig()
        {
            try
            {
                var data = new AssetBundleInfo[localResConfig.Count];
                localResConfig.Values.CopyTo(data, 0);

                string text = LitJson.JsonMapper.ToJson(data);
                GameApp.ResMgr.SaveTextFile(GlobalConst.Res.ResVersionFileName, text);
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
        }

        public void UpdateResConfig(string key, AssetBundleInfo ab)
        {
            localResConfig[key] = ab;
        }

        /// <summary>
        /// 返回本地是否包含某个资源
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public bool IsResDownloaded(string ab)
        {
            /*if (!remoteResConfig.ContainsKey(ab))
            {
                return false;
            }*/

            if (localResConfig.ContainsKey(ab))
            {
                string path = downloadPath + ab;
                //var item = localResConfig[ab];
                //return GameHelper.IsFileValid(path, item.Hash);
                try
                {
                    bool exists = File.Exists(path);
                    return exists;
                }
                catch (Exception ex)
                {
                    Logger.Res.LogException(ex);
                }
            }

            Logger.UI.Log("no exists in local res config. File:" + ab);
            return false;
        }

        /// <summary>
        /// 检查是否需要更新某个资源
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public bool IsResNeedsUpdate(string ab)
        {
            if (!localResConfig.ContainsKey(ab))
            {
                Logger.UI.Log("no exists in local res config. File:" + ab);
                return false;
            }

            if (!remoteResConfig.ContainsKey(ab))
            {
                Logger.UI.Log("no exists in remote res config. File:" + ab);
                return false;
            }

            var local = localResConfig[ab];
            var remote = remoteResConfig[ab];
            return !IsValidABFile(local, remote, false);
        }

        public List<string> FindUpdatableNames(bool includeGames)
        {
            var names = new List<string>();
            foreach (var ab in remoteResConfig)
            {
                if (!includeGames)
                {
                    if (ab.Key.StartsWith("games/"))
                    {
                        continue;
                    }
                }
                names.Add(ab.Key);
            }

            return names;
        }

        /// <summary>
        /// 返回需要更新或下载的ab
        /// </summary>
        public IList<AssetBundleInfo> FindUpdatableRes(string[] abs)
        {
            var lst = new List<AssetBundleInfo>();
            foreach (var ab in abs)
            {
                AssetBundleInfo remoteItem = null;
                AssetBundleInfo localItem = null;

                if (remoteResConfig.ContainsKey(ab))
                {
                    remoteItem = remoteResConfig[ab];
                }

                if (localResConfig.ContainsKey(ab))
                {
                    localItem = localResConfig[ab];
                }

                //本地远端都没有，忽略文件
                if (localItem == null && remoteItem == null)
                {
                    Logger.UI.Log("not found neither local nor remote");
                    continue;
                }
                
                //本地远端都有，比较版本
                if (localItem != null && remoteItem != null)
                {
                    if (IsValidABFile(localItem, remoteItem, true))
                    {
                        Logger.UI.Log("invalid ab version. local:" + localItem + " remote:" + remoteItem);
                        continue;
                    }

                    lst.Add(remoteItem);
                }
                else
                {
                    lst.Add(remoteItem);
                }
            }
            return lst;
        }

        public bool IsValidABFile(AssetBundleInfo local, AssetBundleInfo remote, bool checkFile)
        {
            try
            {
                if (!remote.Equals(local))
                {
                    Logger.UI.Log("different ab name or version. local:" + local + " remote:" + remote);
                    return false;
                }

                if (!checkFile)
                {
                    return true;
                }

                //TODO 随包资源不支持IO，需要调用平台接口
                //校验md5
                string fileName = downloadPath + local.FileName;
                if (GameApp.Options.EnableVerifyAB)
                {
                    if (remote.Hash.CompareTo(local.Hash) != 0)
                    {
                        Logger.UI.Log("different ab hash");
                        return false;
                    }

                    if (FileHelper.IsFileValid(fileName, local.Hash))
                    {
                        Logger.UI.Log("different ab file hash");
                        return true;
                    }
                    else
                    {
                        Logger.Sys.LogWarning("same ab file:" + local + " with different md5 value");
                    }
                }
                else
                {
                    return File.Exists(fileName);
                }
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }
            
            return false;
        }


        public IEnumerator LoadZipFileAsync(string zip, string file)
        {
            yield break;
        }

        public IEnumerator LoadAudioFileAsync(string file)
        {
            yield break;
        }

        public IEnumerator LoadMovieFileAsync(string file)
        {
            yield break;
        }

        public IEnumerator LoadTextureFileAsync(string file)
        {
            yield break;
        }

        public IEnumerator LoadBinaryFileAsync(string file)
        {
            yield break;
        }

        /// <summary>
        /// 从文件系统加载文本文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerator LoadTextFileAsync(string file)
        {
            string url = null;
            WWW www = null;

            Logger.Res.Log("load text:" + file);

#if !UNITY_EDITOR || TEST_AB
            if (downloadedABs.Contains(file))
            {
                url = GameHelper.GetDownloadPath(true) + file;
                www = new WWW(url);
                yield return www;
            }
#endif
            if (www == null || !string.IsNullOrEmpty(www.error))
            {
                url = GameHelper.GetInternalPath(true) + file;
                www = new WWW(url);
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    Logger.Res.Log("get file text error. Error:" + www.error + " File:" + file);
                    yield break;
                }
            }

            cachedFiles[file] = www.text;
        }

        public bool IsFileInCache(string file)
        {
            return cachedFiles.ContainsKey(file);
        }

        /// <summary>
        /// 获取异步加载的文本文件内容
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetCachedTextFile(string file)
        {
            if (cachedFiles.ContainsKey(file))
            {
                return cachedFiles[file] as string;
            }

            return null;
        }

        /// <summary>
        /// 保存文本内容到文件系统
        /// </summary>
        /// <param name="file"></param>
        /// <param name="text"></param>
        /// <param name="cache"></param>
        public void SaveTextFile(string file, string text, bool cache = true)
        {
            try
            {
                if (cache)
                {
                    cachedFiles[file] = text;
                }

                string path = downloadPath + file;
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(path, text);
            }
            catch (Exception ex)
            {
                Logger.Res.LogException(ex);
            }

        }

        /// <summary>
        /// 从已经加载的ab包中同步读取asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string abName, string asset) where T : UnityEngine.Object
        {
            T t = default(T);

#if UNITY_EDITOR && !TEST_AB
            //编辑器状态下从文件系统中读取
            abName += GameHelper.GetABExt(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
            string[] paths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, asset);
            foreach (var path in paths)
            {
                Logger.Res.Log("load asset from path! AB:" + abName + "  Asset:" + asset+" Path:"+path);
                t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (t != default(T))
                {
                    break;
                }
            }

            if (t == default(T))
            {
                t = Resources.Load<T>(asset);
            }

#else
            abName += GameHelper.GetABExt(Application.platform);
            t = GetLoader(abName).GetAsset<T>(abName, asset);
            if (t != default(T))
            {
                return t;
            }

#endif

            if (t == default(T))
            {
                Logger.Sys.LogError("load asset from ab failed! AB:" + abName + "  Asset:" + asset);
            }

            return t;
        }

        public IEnumerator LoadAssetAsync<T>(string ab, string asset) where T : UnityEngine.Object
        {
            string abName = ab + GameHelper.GetABExt(Application.platform);
            yield return StartCoroutine(GetLoader(abName).LoadAssetAsync<T>(abName, asset));
        }

        public T GetLoadedAsset<T>(string ab, string asset) where T : UnityEngine.Object
        {
            string abName = ab+GameHelper.GetABExt(Application.platform);
            var obj = GetLoader(abName).GetAsset<T>(abName, asset);
            if (obj != default(T))
            {
                return obj;
                //return Instantiate<T>(obj);
            }
            return default(T);
        }


        /// <summary>
        /// 同步从内置resources读取GO
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAssetInResources(string prefab)
        {
            return Resources.Load(prefab);
            //return Instantiate(obj) as GameObject;
        }

		public string LoadTextInResources(string file)
		{
			var obj = Resources.Load(file);
			if (obj == null) 
			{
				return null;
			}

			var textObj = obj as TextAsset;
			return textObj.text;
		}

       /// <summary>
       /// 获取异步从内置resources加载的asset
       /// </summary>
       /// <param name="prefab"></param>
       /// <returns></returns>
        public UnityEngine.Object GetLoadedAssetLoadFromResources(string prefab)
        {
            if (!cachedFiles.ContainsKey(prefab))
            {
                return null;
            }

            var obj =  cachedFiles[prefab] as UnityEngine.Object;
            if (obj != null)
            {
                return obj;
                //return Instantiate(obj);
            }

            return null;
        }

        public T GetLoadedAssetLoadFromResources<T>(string prefab) where T : UnityEngine.Object
        {
            if (!cachedFiles.ContainsKey(prefab))
            {
                return null;
            }

            var obj = cachedFiles[prefab];
            return obj as T;
            //var t = obj as T;
            //return Instantiate<T>(t);
        }

        /// <summary>
        /// 从内置resources包异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetAsyncInResources<T>(string prefab) where T : UnityEngine.Object
        {
            if (cachedFiles.ContainsKey(prefab))
            {
                yield break;
            }

            var req =  Resources.LoadAsync<T>(prefab);
            yield return req;

            while (!req.isDone)
            {
                yield return null;
            }

            Logger.Res.Log("asset loaded from resources. Prefab:"+prefab);
            cachedFiles[prefab] = req.asset;
        }



        /// <summary>
        /// 从ab包里读取文件
        /// </summary>
        /// <param name="ab"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string LoadText(string ab, string file)
	    {
            var asset = LoadAsset<TextAsset>(ab, file);
	        return asset.text;
	    }

        public byte[] LoadBytes(string ab, string file)
        {
            var asset = LoadAsset<TextAsset>(ab, file);
            return asset.bytes;            
        }


        private LOAssetManager GetLoader(string abName)
        {
            if (downloadedABs.Contains(abName))
            {
                return abMgr[1];
            }
            return abMgr[0];
        }


        /// <summary>
        /// 从文件系统中异步加载ab
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundleAync(string abName)
        {
            Logger.Res.Log("try to load bundle. Name:" + abName);

            float beginReadTime = Time.realtimeSinceStartup;

            abName += GameHelper.GetABExt(Application.platform);
            if (!localResConfig.ContainsKey(abName))
            {
                Logger.Res.LogError("no match config for bundle which named:" + abName);
                yield break;
            }

            int version = int.Parse(localResConfig[abName].Version);
            yield return StartCoroutine(GetLoader(abName).LoadAssetBundleAsync(abName, version));

            float useTime = Time.realtimeSinceStartup - beginReadTime;
            Logger.Res.Log("ab loaded. Name:"+abName+" UseTime:"+useTime);
        }

        /// <summary>
        /// 从文件系统中异步加载场景
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public IEnumerator LoadLevel(string scene)
        {
            float beginReadTime = Time.realtimeSinceStartup;
            
            scene += GlobalConst.Res.SceneFileExt;
            if (!localResConfig.ContainsKey(scene))
            {
                Logger.Res.Log("no match config for scene which named:"+scene);
                yield break;
            }

            int version = int.Parse(localResConfig[scene].Version);
            yield return StartCoroutine(GetLoader(scene).LoadLevelAsync(scene, version));

            float useTime = Time.realtimeSinceStartup - beginReadTime;
            Logger.Res.Log("level loaded. Name:" + scene + " UseTime:" + useTime);
        }

        public bool IsSceneCached(string scene)
        {
            Logger.Res.Log("check cache, ready:" + Caching.ready + " enabled:" + Caching.enabled);
            if (!Caching.ready || !Caching.enabled)
            {
                return false;
            }

            scene += GlobalConst.Res.SceneFileExt;
            if (!localResConfig.ContainsKey(scene))
            {
                return true;
            }

            int version = int.Parse(localResConfig[scene].Version);
            string url = GetLoader(scene).URI + scene;

            Logger.Res.Log("check cache, url:"+url+" ver:"+version);
            return Caching.IsVersionCached(url, version);
        }

        public bool IsCachedLessThan(long size)
        {
            if (!Caching.ready || !Caching.enabled)
            {
                return false;
            }

            Logger.Res.Log("check cache, free:"+Caching.spaceFree+
                " spaceOccupied:"+Caching.spaceOccupied+
                " maximumAvailableDiskSpace:"+Caching.maximumAvailableDiskSpace);

            return Caching.spaceFree < size;
        }

        /// <summary>
        /// 返回是否加载了某个ab包
        /// </summary>
        public bool IsAssetBundleLoaded(string abName)
        {
            abName += GameHelper.GetABExt(Application.platform);
            return GetLoader(abName).GetAssetBundle(abName) != null;
        }

        public bool IsAssetBundleCached(string abName)
        {
            if (!Caching.ready || !Caching.enabled)
            {
                return false;
            }
            abName += GameHelper.GetABExt(Application.platform);
            if (!localResConfig.ContainsKey(abName))
            {
                Logger.Res.Log("no match config for bundle which named:" + abName);
                return false;
            }

            int version = int.Parse(localResConfig[abName].Version);
            string url = GetLoader(abName).URI + abName;
            return Caching.IsVersionCached(url, version);
        }

        /// <summary>
        /// 卸载已经加载的ab
        /// </summary>
        public void UnloadAssetBundle(string abName, bool remoteAll)
        {
            Logger.Res.Log("unload ab:" + abName);

#if !UNITY_EDITOR || TEST_AB
            abName += GameHelper.GetABExt(Application.platform);
            GetLoader(abName).UnloadAssetBundle(abName, remoteAll);
#endif
        }

        /// <summary>
        /// 卸载已经加载的场景
        /// </summary>
        public void UnloadLevel(string scene, bool remoteAll)
        {
            Logger.Res.Log("unload level:" + scene);

#if !UNITY_EDITOR || TEST_AB
            scene += GlobalConst.Res.SceneFileExt;
            GetLoader(scene).UnloadAssetBundle(scene, remoteAll);
#endif
        }


    }
}


