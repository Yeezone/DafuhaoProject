using System;
using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using com.QH.QPGame.Lobby;
using com.QH.QPGame.Lobby.Surfaces;
using com.QH.QPGame.Services.Data;

namespace Assets.Editor
{
    internal class InspectTools
    {
        public static Dictionary<string, GameUIConfig[]> BuildGameConfig()
        {
            var configs = new Dictionary<string, GameUIConfig[]>();
            if (UIRoot.list.Count == 0)
            {
                return configs;
            }

            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            if (container == null)
            {
                return configs;
            }

            foreach (var surface in container.surfaces)
            {
                var uiConfigs = new List<GameUIConfig>();
                if (surface.ConfigUIList != null && surface.ConfigUIList.Count > 0)
                {
                    foreach (var gameObject in surface.ConfigUIList)
                    {
                        Debug.Log("obj:" + gameObject.name + " inst:" + gameObject.GetInstanceID());
                        var item = new GameUIConfig();
                        item.Activation = true;
                        item.Name = gameObject.name;
                        /*item.Position = gameObject.transform.localPosition;
                        item.Scale = gameObject.transform.localScale;*/
                        uiConfigs.Add(item);
                    }
                }

                configs.Add(surface.name, uiConfigs.ToArray());
            }

            return configs;
        }

        public static void SaveGameUIConfig(BuildTarget target, SceneStyle style)
        {
            var allConfigs = new Dictionary<string, Dictionary<string, GameUIConfig[]>>();

            var guids = AssetDatabase.FindAssets("t:Scene");
            var scenes = Array.ConvertAll<string, string>(guids, AssetDatabase.GUIDToAssetPath);
            foreach (var scene in scenes)
            {
                string lowerScene = scene.ToLower();

                if (!scene.Contains(style.ToString()))
                {
                    continue;
                }

                if (lowerScene.Contains("_pc"))
                {
                    if (target != BuildTarget.StandaloneWindows &&
                        target != BuildTarget.StandaloneWindows64)
                    {
                        continue;
                    }
                }
                else if (lowerScene.Contains("_phone"))
                {
                    if (target != BuildTarget.iOS &&
                        target != BuildTarget.Android)
                    {
                        continue;
                    }
                }

                if (!scene.StartsWith("Assets/Scenes/"))
                {
                    continue;
                }

                EditorApplication.OpenScene(scene);

                string sceneFileName = Path.GetFileNameWithoutExtension(scene);
                Debug.Log(sceneFileName + ":" + EditorApplication.currentScene);

                var configs = BuildGameConfig();
                allConfigs.Add(sceneFileName, configs);
            }
            
            string text = LitJson.JsonMapper.ToJson(allConfigs);

            string fileName = string.Format("{0}/{1}/{2}_{3}",
                Application.streamingAssetsPath,
                GameHelper.GetBuildTargetName(target),
                style.ToString(),
                GlobalConst.Res.UIConfigFileName);

            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Debug.Log("save to file :"+fileName+ " text:"+text);

            File.WriteAllText(fileName, text);
        }

        public enum SceneStyle
        {
            Blue,
            Gamble,
            Gold,
        }

        [MenuItem("InspectTools/SaveUIConfig")]
        public static void SaveUIConfig()
        {
            string oriScene = EditorApplication.currentScene;

            SaveGameUIConfig(BuildTarget.StandaloneWindows, SceneStyle.Blue);
            SaveGameUIConfig(BuildTarget.StandaloneWindows, SceneStyle.Gamble);
            SaveGameUIConfig(BuildTarget.Android, SceneStyle.Blue);
            SaveGameUIConfig(BuildTarget.Android, SceneStyle.Gamble);
            SaveGameUIConfig(BuildTarget.iOS, SceneStyle.Blue);
            SaveGameUIConfig(BuildTarget.iOS, SceneStyle.Gamble);

            if (EditorApplication.currentScene != oriScene)
            {
                EditorApplication.OpenScene(oriScene);
            }
        }

        [MenuItem("InspectTools/CheckMissing")]
        public static void CheckMissing()
        {

        }

        [MenuItem("InspectTools/DiffPrefab")]
        public static void DiffPrefab()
        {

        }

        [MenuItem("InspectTools/CheckNone")]
        public static void CheckNone()
        {

        }

    }
}
