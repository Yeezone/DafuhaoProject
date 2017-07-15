using System;

using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using com.QH.QPGame;
using com.QH.QPGame.Lobby;
using System.Reflection;
using System.IO;
using com.QH.QPGame.Services.Data;

/// <summary>
/// 游戏配置
/// @Author:guofeng
/// </summary>
public class GameWindow : EditorWindow
{
    private Vector2 pos = Vector2.zero;
    private BuildTarget buildTarget;
    private List<GameConfig> games;
    public bool ShowCustomData = false;

    private Action<List<GameConfig>> OnFinished;

    [MenuItem("BuildPacket/SaveGameConfig")]
    static void Init()
    {
        GameWindow window = (GameWindow)EditorWindow.GetWindow(typeof(GameWindow));
        window.title = "GameWindow";
        window.autoRepaintOnSceneChange = true;
        window.ShowCustomData = true;
        window.Show();
        window.InitData(EditorUserBuildSettings.activeBuildTarget, null, delegate(List<GameConfig> list)
            {
                if (list.Count == 0)
                {
                    EditorUtility.DisplayDialog("save game config", "nothing is selected", "ok");
                    return;
                }
                string path = Application.streamingAssetsPath + "/" + GameHelper.GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget) + "/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = path + GlobalConst.Res.GameConfigFileName; ;
                string text = LitJson.JsonMapper.ToJson(list);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.WriteAllText(fileName, text);
                EditorUtility.DisplayDialog("save game config", "done", "ok");
            });
    }


    public static List<GameConfig> ScanGames(BuildTarget target, List<int> ids = null )
    {
        var games = new List<GameConfig>();
        var assets = AssetDatabase.GetAllAssetBundleNames();
        Type[] typeList = typeof(GameConfig).Assembly.GetTypes();
        foreach (var type in typeList)
        {
            if (type.IsSubclassOf(typeof(SceneGameAgent)))
            {
                Attribute[] abt = Attribute.GetCustomAttributes(type, typeof(GameDescAttribute), true);
                foreach (var attribute in abt)
                {
                    var gda = attribute as GameDescAttribute;
                    if (ids != null && !ids.Contains((int)gda.GameIDPrefix))
                    {
                        continue;
                    }

                    var config = new GameConfig();
                    config.ID = (int)gda.GameIDPrefix;
                    config.Version = string.IsNullOrEmpty(gda.Version) ? "1.0.0" : gda.Version;
                    config.Assembly = type.FullName;
                    config.Packed = false;
                    config.SceneName = gda.SceneName ?? "level_" + config.ID;
                    string keyword = config.SceneName + " t:Scene";
                    List<string> list = new List<string>();
                    var guids = AssetDatabase.FindAssets(keyword);
                    var scenes = Array.ConvertAll<string, string>(guids, AssetDatabase.GUIDToAssetPath);
                    foreach (var scene in scenes)
                    {
                        if (scene.Contains("StreamingAssets"))
                        {
                            continue;
                        }
                        if (scene.ToLower().Contains("_pc"))
                        {
                            if (target != BuildTarget.StandaloneWindows &&
                                target != BuildTarget.StandaloneWindows64)
                            {
                                continue;
                            }
                        }
                        else if (scene.ToLower().Contains("_phone"))
                        {
                            if (target != BuildTarget.iOS &&
                                target != BuildTarget.Android)
                            {
                                continue;
                            }
                        }

                        config.SceneName = scene;
                        string sceneFileName = Path.GetFileName(scene).Replace(".unity", GlobalConst.Res.SceneFileExt);
                        string assetScenePath = "games/lv_" + config.ID + "/" + sceneFileName;
                        list.Add(assetScenePath);
                    }


                    foreach (var asset in assets)
                    {
                        if (asset.Contains("pc"))
                        {
                            if (target != BuildTarget.StandaloneWindows &&
                                target != BuildTarget.StandaloneWindows64)
                            {
                                continue;
                            }
                        }
                        else if (asset.Contains("mp"))
                        {
                            if (target != BuildTarget.iOS &&
                                target != BuildTarget.Android)
                            {
                                continue;
                            }
                        }

                        if (asset.Contains("games/lv_" + config.ID))
                        {
                            list.Add(asset);
                        }
                    }

                    config.AssetBundles = list.ToArray();

                    /*var customData = new Dictionary<string, object>();
                    if (attribute.GetType().IsSubclassOf(typeof(GameDescAttribute)))
                    {
                        var properties = attribute.GetType().GetProperties(
                            BindingFlags.Instance | BindingFlags.Public |BindingFlags.DeclaredOnly);
                        foreach (var property in properties)
                        {
                            var obj = property.GetValue(attribute, null);
                            customData.Add(property.Name, obj);
                        }
                    }

                    config.CustomData = customData;*/
                    games.Add(config);
                }
            }
        }

        games.Sort((x, y) => x.ID - y.ID);

        return games;
    }

    public void InitData(BuildTarget target, Dictionary<int, bool> selected, Action<List<GameConfig>> finished)
    {
        buildTarget = target;
        this.OnFinished = finished;

        this.games = ScanGames(target);

        if (selected != null)
        {
            foreach (var select in selected)
            {
                selectGames.Add(select.Key, true);
            }
        }
        else
        {
            foreach (var gameConfig in games)
            {
                selectGames.Add(gameConfig.ID, true);
            }
        }
        
    }

    public static void ApplyRawData(BuildTarget target, ref List<GameConfig> games)
    {
        var rawGames = ScanGames(target);
        var fields = typeof(GameConfig).GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var gameConfig in games)
        {
            var rawGameConfig = rawGames.Find(item => item.ID == gameConfig.ID);
            if (rawGameConfig == null)
            {
                continue;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field.IsDefined(typeof(NoUpdateNeededAttribute), false))
                {
                    continue;
                }

                var value = field.GetValue(rawGameConfig);
                field.SetValue(gameConfig, value);
            }
        }
    }

    private Dictionary<int,bool> selectGames = new Dictionary<int, bool>(); 

    void OnGUI()
    {
        if (games == null) return;

        pos = EditorGUILayout.BeginScrollView(pos);

        foreach (var gameConfig in games)
        {
            /*bool val = project.Games.ContainsKey(id) && project.Games[id] != 0;
               bool val2 = project.Games.ContainsKey(id) && project.Games[id] == 2;

               val = EditorGUILayout.BeginToggleGroup(id, val);
               val2 = EditorGUILayout.ToggleLeft("pack", val2);
               EditorGUILayout.EndToggleGroup();
               project.Games[id] = val && val2 ? 2 : val ? 1 : 0;*/

            bool selected = selectGames.ContainsKey(gameConfig.ID) ? selectGames[gameConfig.ID] : false;
            //EditorGUILayout.BeginHorizontal();
            selectGames[gameConfig.ID] = EditorGUILayout.BeginToggleGroup("ID:" + gameConfig.ID, selected);
            /*if (GUILayout.Button("CustomConfig"))
            {

            }*/
            //EditorGUILayout.EndHorizontal();

            gameConfig.Packed = EditorGUILayout.Toggle("Pack", gameConfig.Packed);

          
            EditorGUILayout.LabelField("Version:", gameConfig.Version);
            EditorGUILayout.LabelField("Assembly:", gameConfig.Assembly);
            EditorGUILayout.LabelField("Scene:", gameConfig.SceneName);

            EditorGUILayout.PrefixLabel("AssetBundles:");
            if (gameConfig.AssetBundles != null)
            {
                foreach (var assetBundle in gameConfig.AssetBundles)
                {
                    GUILayout.Box(assetBundle);
                }
            }

            /*if (ShowCustomData)
            {
                EditorGUILayout.PrefixLabel("CustomData:");
                if (gameConfig.CustomData != null)
                {
                    var list = new List<string>();
                    list.AddRange(gameConfig.CustomData.Keys);
                    foreach (var item in list)
                    {
                        var value = gameConfig.CustomData[item];
                        if (value == null)
                        {
                            continue;
                        }
                        var valueType = value.GetType();
                        //gameConfig.CustomData[item] = EditorGUILayout.TextField(item, gameConfig.CustomData[item].ToString());
                        if (valueType == typeof(string))
                        {
                            gameConfig.CustomData[item] = EditorGUILayout.TextField(new GUIContent(item),
                                    value.ToString());
                        }
                        else if (valueType == typeof(bool))
                        {
                            gameConfig.CustomData[item] = EditorGUILayout.Toggle(new GUIContent(item), (bool)value);
                        }
                        else if (valueType == typeof(int))
                        {
                            gameConfig.CustomData[item] = EditorGUILayout.IntField(new GUIContent(item), (int)value);
                        }
                        else if (valueType.IsEnum)
                        {
                            var enumValue = (Enum)Enum.Parse(valueType, value.ToString());
                            gameConfig.CustomData[item] = EditorGUILayout.EnumPopup(item, enumValue);
                           
                        }
                    }
                }
            }*/

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndToggleGroup();
        }
        EditorGUILayout.EndScrollView();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Select All"))
        {
            var list = new List<int>();
            list.AddRange(selectGames.Keys);
            foreach (var item in list)
            {
                selectGames[item] = true;
            }
        }

        if (GUILayout.Button("UnSelect"))
        {
            var list = new List<int>();
            list.AddRange(selectGames.Keys);
            foreach (var item in list)
            {
                selectGames[item] = !selectGames[item];
            }
        }

        if (GUILayout.Button("No Select"))
        {
            var list = new List<int>();
            list.AddRange(selectGames.Keys);
            foreach (var item in list)
            {
                selectGames[item] = false;
            }
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("cancel"))
        {
            this.Close();
        }

        if (GUILayout.Button("ok"))
        {
            var selected = new List<GameConfig>();
            foreach (var gameConfig in games)
            {
                if (selectGames[gameConfig.ID])
                {
                    selected.Add(gameConfig);
                }
            }

            OnFinished(selected);
            this.Close();
        }
        EditorGUILayout.EndHorizontal();


    }

    void OnFocus()
    {
        
    }

    void OnProjectChange()
    {

    }
}

