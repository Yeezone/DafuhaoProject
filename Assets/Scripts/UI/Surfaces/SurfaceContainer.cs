using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;


namespace com.QH.QPGame.Lobby.Surfaces
{

    public class SurfaceContainer : MonoBehaviour
    {
        public Surface[] surfaces;

        private Dictionary<string, Surface> surfaceList;

        // Use this for initialization
        void Awake()
        {
            surfaceList = new Dictionary<string, Surface>();

            RegisterSurfaces();
            //StartCoroutine(LoadAllUIConfig());
        }

        // Update is called once per frame
//        void Update()
//        {
//
//        }

        void OnDestroy()
        {
        }

        private IEnumerator LoadAllUIConfig()
        {
            //string fileName = string.Format("{0}_{1}", "Blue", GlobalConst.Res.UIConfigFileName);
			string fileName = GlobalConst.Res.UIConfigFileName;
            yield return StartCoroutine(
                GameApp.ResMgr.LoadTextFileAsync(fileName));

            if (!GameApp.ResMgr.IsFileInCache(fileName))
            {
                yield break;
            }

            string text = GameApp.ResMgr.GetCachedTextFile(fileName);
            if (string.IsNullOrEmpty(text))
            {
                yield break;
            }

            Dictionary<string, GameUIConfig[]> allUIConfigs = null;
            var allSceneUIConfigs = LitJson.JsonMapper.ToObject<Dictionary<string, Dictionary<string, GameUIConfig[]>>>(text);
            if (allSceneUIConfigs.ContainsKey(Application.loadedLevelName))
            {
                allUIConfigs = allSceneUIConfigs[Application.loadedLevelName];
            }

            foreach (var surface in surfaces)
            {
                if (allUIConfigs != null &&
                    allUIConfigs.ContainsKey(surface.name))
                {
                    var configs = allUIConfigs[surface.name];
                    surface.ApplyConfig(configs);
                }
            }
        }

        public void SwitchSurface(string surName)
        {
            if (!surfaceList.ContainsKey(surName))
            {
                Debug.Log("no surface exists named:" + surName);
                return;
            }

            Surface surface = surfaceList[surName];
            if (surface != null)
            {
                surface.Show();
            }
        }

        public void SwitchSurface<T>() where T : Surface
        {
            foreach (var surface in surfaces)
            {
                T t = surface.GetComponent<T>();
                if (t != null)
                {
                    t.Show();
                }
            }
        }

        public void HideSurface(string surName)
        {
            Surface surface = surfaceList[surName];
            if (surface != null)
            {
                surface.Hide();
            }
        }

        public T GetSurface<T>() where T:Surface
        {
            foreach (var surface in surfaces)
            {
                var t = surface.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        public Surface GetSurfaceWithName(string wSurName)
        {
            Surface surface = surfaceList[wSurName];
            if (surface != null)
            {
                return surface;
            }
            return null;
        }

        private void RegisterSurfaces()
        {
            int surfaceCount = surfaces.Length;
            if (surfaceCount > 0)
            {
                for (int i = 0; i < surfaceCount; i++)
                {
                    Surface surface = surfaces[i];
                    if (surface == null)
                    {
                        Logger.UI.LogError("null surface. Scene:"+Application.loadedLevelName);
                        continue;
                    }
                    Register(surface);
                }
            }
        }

        private void Register(Surface sur)
        {
            if (!surfaceList.ContainsKey(sur.SurfaceName) ||
                !surfaceList.ContainsValue(sur))
            {
                surfaceList.Add(sur.SurfaceName, sur);
                return;
            }

            Debug.LogError("Can't Register The Surface Of " + sur.SurfaceName + "Twice");
        }

    }

}
