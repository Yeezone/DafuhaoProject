using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;

namespace LOAssetFramework
{
    public class LOAssetManager : MonoBehaviour
	{
		public string URI{ set; get;}
		public string ManifestName{ set; get;}
		public Action<bool> InitBlock;
        public bool BlockInited;

		public AssetBundleManifest manifest{ set; get;}

        private LOAssetCache cache = new LOAssetCache();

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The asset.</returns>
		public T GetAsset<T>(string assetbundlename,string assetname) where T:UnityEngine.Object
		{
			var lab = cache.GetBundleCache(assetbundlename);
			if (lab == null) 
            {
                Debug.LogError("not found which ab named:"+assetbundlename);
				return null;
			}

            var obj = lab.GetAsset(assetname);
            if (obj != null)
            {
                return obj as T;
            }

            var t = lab.Bundle.LoadAsset<T>(assetname);
            //同步加载之后清除loading并设置缓存
            if (t != null)
            {
                lab.RemoveLoading(assetname);
                lab.SetAssetCache(assetname, t);
            }

            return t;
		}

        public IEnumerator LoadAssetAsync<T>(string assetbundlename, string assetname) where T : UnityEngine.Object
        {
            var lab = cache.GetBundleCache(assetbundlename);
            if (lab == null)
            {
                yield break;
            }

            var obj = lab.GetAsset(assetname);
            if (obj != null)
            {
                yield break;
            }

            var req = lab.Bundle.LoadAssetAsync<T>(assetname);

            lab.AddLoading(assetname, req);

            while (!req.isDone) yield return null;

            lab.RemoveLoading(assetname);
            lab.SetAssetCache(assetname, req.asset);
        }

        public void UnloadAssetBundle(string abName, bool remoteAll)
        {
            var lab = cache.GetBundleCache(abName);
            if (lab != null)
            {
                Logger.Res.Log("unload ab. Name:" + abName);
                if(lab.Release(remoteAll))
                {
                    cache.FreeBundle(abName);
                }

                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            else
            {
                Logger.Res.Log("try to unload an unloaded ab!!! Name:" + abName);
            }
        }

        public AssetBundle GetAssetBundle(string abName)
        {
            var lab = cache.GetBundleCache(abName);
            if (lab == null)
            {
                return null;
            }

            return lab.Bundle;
        }

        public bool IsLoadingOrLoaded(string abName)
        {
            if (cache.InCache(abName))
            {
                return true;
            }

            if (cache.InWWWCache(abName))
            {
                return true;
            }

            return false;
        }

        IEnumerator LoadManifestBundle()
		{
            if (!cache.InCache(ManifestName)) 
            {
                // 通过网络下载AssetBundle
                WWW www = IsLoadAssetBundleAtInternal(ManifestName);
                yield return www;
			}

			this.manifest = this.GetAsset<AssetBundleManifest>(ManifestName,"AssetBundleManifest");
			InitBlock (this.manifest != null);
            BlockInited = true;
		}

		public void Initialize()
		{
			//StartCoroutine (LoadManifestBundle());
		}


		#region 加载包裹系列函数

		/// <summary>
		/// 检查是否已经从网络下载
		/// </summary>
		protected WWW IsLoadAssetBundleAtInternal (string assetBundleName, int version = 0)
		{
			//已经存在了
			var bundle = cache.GetBundleCache(assetBundleName);

			if (bundle != null)
			{
				//保留一次
				bundle.Retain ();
				return null;
			}

			//如果WWW缓存策略中包含有对应的关键字,则返回null
			if (cache.InWWWCache (assetBundleName)) {
				return null;
			}

			//创建下载链接
		    string url = URI + assetBundleName;
            Logger.Res.Log("load ab from url. Url:" + url);

            /*Debug.Log("Version cached. Value:" + Caching.IsVersionCached(url, 0));
            Debug.Log("Caching maximumAvailableDiskSpace:" + Caching.maximumAvailableDiskSpace);
            Debug.Log("Caching spaceFree:" + Caching.spaceFree);
            Debug.Log("Caching spaceOccupied:" + Caching.spaceOccupied);
            */

		    WWW www = null;
		    if (Caching.enabled)
		    {
                www = WWW.LoadFromCacheOrDownload(url, version);
		    }
		    else
		    {
		        www = new WWW(url);
		    }

			//加入缓存策略
			cache.SetWWWCache(assetBundleName, www);
			return www;
		}


		IEnumerator LoadDependencies(string assetBundleName)
		{
			if (this.manifest == null) {
				yield break;
			}

			// 获取依赖包裹
			string[] dependencies = this.manifest.GetAllDependencies(assetBundleName);
			if (dependencies.Length == 0)
			{
				yield break;
			}

            Logger.Res.Log("LoadDependencies:" + string.Join(";", dependencies));

			// 记录并且加载所有的依赖包裹
			cache.SetDependCache(assetBundleName, dependencies);

			for (int i = 0; i < dependencies.Length; i++) 
			{
				yield return IsLoadAssetBundleAtInternal (dependencies [i]);
			}
		}

		/// <summary>
		/// 加载资源包
		/// </summary>
		IEnumerator LoadAssetBundle(string assetBundleName, int version)
		{
			if (cache.InCache(assetBundleName)) {
				yield break;
			}
			// 通过网络下载AssetBundle
            WWW www = IsLoadAssetBundleAtInternal(assetBundleName, version);
		    yield return www;

		    // 通过网络加载失败，下载依赖包裹
		    //yield return StartCoroutine(LoadDependencies(assetBundleName));
		}

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public IEnumerator LoadAssetBundleAsync(string assetBundleName, int version)
        {
            //开始加载包裹
            yield return StartCoroutine(LoadAssetBundle(assetBundleName, version));
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        public IEnumerator LoadLevelAsync(string assetBundleName, int version)
        {
            //加载资源包
            yield return StartCoroutine(LoadAssetBundle(assetBundleName, version));
        }

        public void Reset()
        {
            cache.FreeAll();
        }

        #endregion


        #region Update

        void OnDestroy()
        {
            cache.FreeAll();
        }

        void Update()
        {
            // Collect all the finished WWWs.
            var remoteList = new List<string>();
            var keys = cache.WwwCache.Keys;
            foreach (var key in keys)
            {
                WWW download = cache.WwwCache[key];
                // 下载失败
                if (download.error != null)
                {
                    Logger.Res.LogError(key + " load failed!!!! Error:" + download.error + " URI:" + download.url);

                    cache.ErrorCache.Add(key, download.error);
                    remoteList.Add(key);
                }
                // 下载成功
                else if (download.isDone)
                {
                    Logger.Res.Log(key + " loaded:" + download.assetBundle.name + " URI:" + download.url);

                    LOAssetBundle ab = new LOAssetBundle(download.assetBundle, key);
                    cache.SetBundleCache(key, ab);
                    //download.assetBundle.Unload(false);
                    remoteList.Add(key);
                }
            }

            foreach (var ab in remoteList)
            {
                cache.WwwCache[ab].Dispose();
                cache.WwwCache.Remove(ab);
            }
        }

        #endregion
	}
}

