using System;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using com.QH.QPGame.Lobby;
using System.Reflection;
using System.IO;
using com.QH.QPGame.Services.Data;

public class SpecificProjectsWindow : EditorWindow
{
    private List<ProjectBuildData> projectDatas;
    private Dictionary<string, List<string>> projectNames;

    private Dictionary<string, bool> selected = new Dictionary<string, bool>();
    private Action<List<string>> cb = null;


    public void Init(List<ProjectBuildData> projects, Action<List<string>> cb)
    {
        this.cb = cb;

        selected.Clear();
        projectDatas = projects;
        projectNames = new Dictionary<string, List<string>>();

        foreach (var project in projectDatas)
        {
            string name = project.Settings["companyName"];
            if (!projectNames.ContainsKey(name))
            {
                projectNames[name] = new List<string>();
            }

            selected[name] = false;
            selected[project.Name] = false;
            projectNames[name].Add(project.Name);
        }
    }

    private Vector2 pos = Vector2.zero;

    private void OnGUI()
    {
        if (projectNames == null) return;

        pos = EditorGUILayout.BeginScrollView(pos, false, false);

        foreach (var data in projectNames)
        {
            EditorGUILayout.BeginHorizontal();
            selected[data.Key] = EditorGUILayout.BeginToggleGroup(data.Key, selected[data.Key]);
            foreach (var projectData in data.Value)
            {
                selected[projectData] = EditorGUILayout.ToggleLeft(projectData, selected[data.Key] && selected[projectData]);
            }
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Select All"))
        {
            var list = new List<string>();
            list.AddRange(selected.Keys);
            foreach (var item in list)
            {
                selected[item] = true;
            }
        }

        if (GUILayout.Button("UnSelect"))
        {
            var list = new List<string>();
            list.AddRange(selected.Keys);
            foreach (var item in list)
            {
                selected[item] = !selected[item];
            }
        }

        if (GUILayout.Button("No Select"))
        {
            var list = new List<string>();
            list.AddRange(selected.Keys);
            foreach (var item in list)
            {
                selected[item] = false;
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Selected"))
        {
            var projectsToBuild = new List<string>();

            foreach (var sec in selected)
            {
                if (!sec.Value)
                {
                    continue;
                }
                var item = projectDatas.Find(prj => string.Compare(prj.Name, sec.Key, true) == 0);
                if (item == null)
                {
                    continue;
                }

                projectsToBuild.Add(item.Name);
                Debug.Log(item.Name);
            }

            if (projectsToBuild.Count == 0)
            {
                EditorUtility.DisplayDialog("error", "nothing to build", "ok");
            }
            else
            {
                cb(projectsToBuild);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

}

/// <summary>
/// 项目配置
/// @Author:guofeng
/// </summary>
public class ProjectWindow : EditorWindow
{
    [MenuItem("BuildPacket/BuildConfig")]
    static void Init()
    {
        ProjectWindow window = (ProjectWindow)EditorWindow.GetWindow(typeof(ProjectWindow));
        window.title = "BuildConfig";
        window.autoRepaintOnSceneChange = true;
        window.InitProjects();
        window.Show();
    }

    public string gamesAssetbundlePath = 
		Application.dataPath.Replace("Assets", "assetbundle") + "/" + 
        EditorUserBuildSettings.activeBuildTarget.ToString()+"/games";

    List<ProjectBuildData> projects = null;
    ProjectBuildData project = null;
    int selected = -1;
    bool add = false;
    private List<GameConfig> games = null;
    private Vector2 pos = Vector2.zero;
    private bool reveal = true;
    private bool shutdown = false;
    private bool compress = false;
    private bool encrypt = true;
    private bool buildAsSetup = false;

    private GameWindow gameWindow;

    void OnGUI()
    {
        pos = EditorGUILayout.BeginScrollView(pos, false, false);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (projects != null)
		{
            string[] names = new string[projects.Count];
            for (int i = 0; i < projects.Count; i++)
            {
                names[i] = projects[i].Name;
            }

            int index = EditorGUILayout.Popup("Projects", selected, names);
			if (index != -1 && index != selected || project == null)
			{
				SelectChanged(index);
			}
		}
        
       
        if(!add)
        {
            if (GUILayout.Button("New"))
            {
                NewProject();
            }
        }
        else
        {
            if (GUILayout.Button("Cancel"))
            {
                add = false;
                selected = projects.Count - 1;
            }
        }


        if (selected != -1 && GUILayout.Button("Delete"))
        {
            if (EditorUtility.DisplayDialog("are you sure?", "this action cannot be restore", "yes", "no"))
            {
                DeleteProject();
            }
        }
        EditorGUILayout.EndHorizontal();


        if (project == null)
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            return;
        }

        if (add)
        {
            project.Name = EditorGUILayout.TextField("ProjectName", project.Name);
        }
        else
        {
            EditorGUILayout.LabelField("ProjectName", project.Name);
        }

        EditorGUILayout.LabelField("TargetPlatform", project.Target.ToString());

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        ApplyOptionsData(project.Target);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Packed Games", EditorStyles.boldLabel);
        if (GUILayout.Button("Configuration"))
        {
            gameWindow = (GameWindow)EditorWindow.GetWindow(typeof(GameWindow));
            gameWindow.title = "Games";
            gameWindow.ShowCustomData = true;
            gameWindow.InitData(project.Target, project.Games, delegate(List<GameConfig> list)
                {
                    project.Games = new Dictionary<int, bool>();
                    foreach (var gameConfig in list)
                    {
                        project.Games.Add(gameConfig.ID, gameConfig.Packed);
                    }
                });
            //gameWindow.InitData(project.Target);
            gameWindow.BeginWindows();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginToggleGroup("Games", games != null);
        foreach (var game in games)
        {
            EditorGUILayout.BeginHorizontal();
            project.Games[game.ID] = EditorGUILayout.ToggleLeft(game.ID.ToString(),
                project.Games.ContainsKey(game.ID) && project.Games[game.ID]);
            EditorGUILayout.LabelField("Version: " + game.Version);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Separator();

        //EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndScrollView();




        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        shutdown = GUILayout.Toggle(shutdown, "shutdown after tasks all done(2 min)");
        reveal = GUILayout.Toggle(reveal, "reveal in finder after tasks all done");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        compress = GUILayout.Toggle(compress, "compress the builds(disable in Android)");
        encrypt = GUILayout.Toggle(encrypt, "encrypt the builds(disable in iOS)");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        buildAsSetup = GUILayout.Toggle(buildAsSetup, "make a setup package(Windows only)");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        if (!add && GUILayout.Button("Load"))
        {
            BuildTools.ApplyPlayerSettings(project);
        }

        if (GUILayout.Button("Save"))
        {
            if (string.IsNullOrEmpty(project.Name))
            {
                EditorUtility.DisplayDialog("error", "project name could not be empty", "ok");
            }
            else
            {
                if (project.Target != EditorUserBuildSettings.activeBuildTarget)
                {
                    if (EditorUtility.DisplayDialog("are you sure?", "different platform settings saving!", "yes", "no"))
                    {
                        SaveProject();
                    }
                }
                else
                {
                    SaveProject();
                }
            }
        }

        EditorGUILayout.EndHorizontal();




        EditorGUILayout.BeginHorizontal();
        if (!add)
        {

            if (GUILayout.Button("Build"))
            {
                var projectsToBuild = new List<string>();
                projectsToBuild.Add(project.Name);
                BuildProjects(projectsToBuild);

                return;
            }


            if (GUILayout.Button("BuildAndSaveAs"))
            {
                var projectsToBuild = new List<string>();
                projectsToBuild.Add(project.Name);
                BuildProjects(projectsToBuild, true);
                return;
            }

            if (GUILayout.Button("BuildPlatform"))
            {
                var projectsToBuild = new List<string>();
                foreach (var item in projects)
                {
                    if(item.Target == EditorUserBuildSettings.activeBuildTarget)
                    {
                        projectsToBuild.Add(item.Name);
                    }
                }

                BuildProjects(projectsToBuild);
                return;
            }

            if (GUILayout.Button("BuildAll"))
            {
                var projectsToBuild = new List<string>();
                foreach (var item in projects)
                {
                    projectsToBuild.Add(item.Name);
                }
                BuildProjects(projectsToBuild);
                return;
            }

            if (GUILayout.Button("BuildSpecific"))
            {
                SpecificProjectsWindow window = (SpecificProjectsWindow)EditorWindow.GetWindow(typeof(SpecificProjectsWindow));
                window.title = "projects";
                /*window.autoRepaintOnSceneChange = true;
                var rect = this.position;
                rect.y += 300;
                rect.height = 400;
                rect.width += 30;
                window.position = rect;*/
                window.Init(projects, BuildProjects);
                window.BeginWindows();

                return;
            }

        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();

    }

    void OnFocus()
    {
        if (projects == null)
        {
            InitProjects();
        }
    }

    void OnProjectChange()
    {
    }

    private void InitProjects()
    {
        projects = BuildTools.LoadProjectList();
        if (projects == null)
        {
            return;
        }

		projects.Sort(delegate(ProjectBuildData x, ProjectBuildData y) {
			return string.Compare(x.Name, y.Name);
		});
    }

    void NewProject()
    {
        selected = -1;
        add = true;
        project = BuildTools.CreateProject();
    }

    void DeleteProject()
    {
        BuildTools.DeleteProject(project);
        InitProjects();
        add = false;
        project = null;
        selected = projects.Count - 1;
    }

    void SaveProject()
    {
        if (BuildTools.SaveProject(add, project))
        {
            InitProjects();
            if (add)
            {
                add = false;

				for(int i=0; i<projects.Count; i++)
				{
					if(string.Compare(projects[i].Name, project.Name) == 0)
					{
						selected = i;
						break;
					}
				}

            }
        }
        else
        {
            EditorUtility.DisplayDialog("error", "failed", "ok");
        }
    }

    private void SelectChanged(int index)
    {
        add = false;
        selected = index;
        if (projects != null && index != -1 && projects.Count > index)
        {
            project = projects[index];
            if (project.Games == null)
            {
                project.Games = new Dictionary<int, bool>();
            }
            games = GameWindow.ScanGames(project.Target);
            games.Sort((x, y) => { return x.ID - y.ID; });
        }

        if (gameWindow != null)
        {
            gameWindow.EndWindows();
        }
    }

    private bool IsSamePlatform(RuntimePlatform platform, BuildTarget target)
    {
        if (platform == RuntimePlatform.Android)
            return target == BuildTarget.Android;

        if (platform == RuntimePlatform.WindowsPlayer)
            return target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64;

        if (platform == RuntimePlatform.IPhonePlayer)
            return target == BuildTarget.iOS;

        return false;
    }

    void ApplyOptionsData(BuildTarget target)
    {
        FieldInfo[] fields = typeof(GameOptions).GetFields();
        foreach (var field in fields)
        {
            if (!field.IsPublic || field.IsStatic)
            {
                continue;
            }

            string text = field.Name;
            string tooltip = "";
            bool matched = true;
            System.Object[] objs = field.GetCustomAttributes(typeof(PlatformDesc), false);
            foreach(var obj in objs)
            {
                PlatformDesc desc = (obj as PlatformDesc);
                if (!desc.AllPlatform && !IsSamePlatform(desc.Platform, target))
                {
                    matched = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(desc.Comments))
                    {
                        tooltip = desc.Comments;
                    }

                    matched = true;
                    break;
                }
            }

            if(!matched)
            {
                continue;
            }

            if (field.FieldType == typeof(string))
            {
                field.SetValue(
                    project.Options, EditorGUILayout.TextField(new GUIContent(text, tooltip),
                        field.GetValue(project.Options).ToString())
                    );
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(
                    project.Options, EditorGUILayout.Toggle(new GUIContent(text, tooltip),
                        bool.Parse(field.GetValue(project.Options).ToString()))
                    );
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(project.Options,
                    EditorGUILayout.IntField(new GUIContent(text, tooltip),
                        int.Parse(field.GetValue(project.Options).ToString()))
                        );
            }
            else if (field.FieldType.IsEnum)
            {
                var value = (Enum) Enum.Parse(field.FieldType, field.GetValue(project.Options).ToString());
                field.SetValue(project.Options,
                    EditorGUILayout.EnumPopup(new GUIContent(text, tooltip), value));
            }
            else if(field.FieldType.IsGenericType)
            {
                var value = field.GetValue(project.Options);
                if (value != null)
                {
                    var dict = (Dictionary<string, object>)value;
                    var keys = new List<string>(dict.Keys);

                    var count = EditorGUILayout.IntField(text, dict.Count);

                    if (count != dict.Count)
                    {
                        int remain = count - dict.Count;
                        if (remain > 0)
                        {
                            for (int i = 0; i < remain; i++)
                            {
                                int index = i;
                                do
                                {
                                    string newKey = "key" + index;
                                    if (!dict.ContainsKey(newKey))
                                    {
                                        dict.Add(newKey, null);
                                        break;
                                    }
                                    index++;
                                } while (true);
                            }
                        }
                        else if(remain < 0)
                        {
                            remain = Math.Abs(remain);
                            for (int i = 0; i < remain; i++)
                            {
                                if (i < keys.Count)
                                {
                                    dict.Remove(keys[i]);
                                }
                            }
                        }
                    }

                    keys = new List<string>(dict.Keys);
                    foreach (var key in keys)
                    {
                        EditorGUILayout.BeginHorizontal();
                        var newKey = EditorGUILayout.TextField("Key", key);
                        if (newKey != key && !keys.Contains(newKey))
                        {
                            var tmpValue = dict[key];
                            dict.Remove(key);
                            dict.Add(newKey, tmpValue);
                        }
                        else
                        {
                            newKey = key;
                        }
                        dict[newKey] = EditorGUILayout.TextField("Value", dict[newKey] == null ? "" : dict[newKey].ToString());
                        EditorGUILayout.EndHorizontal();
                    }

                    field.SetValue(project.Options, dict);
                }

                EditorGUILayout.Separator();
            }
        }
    }

    public void BuildProjects(List<string> pjrs)
    {
        BuildProjects(pjrs, false);
    }

    public void BuildProjects(List<string> pjrs, bool saveAs)
    {
        if (pjrs.Count == 0)
        {
            EditorUtility.DisplayDialog("error", "nothing to build", "ok");
            return;            
        }

        var prjsToBuild = new List<ProjectBuildData>();
        for (int i = 0; i < pjrs.Count; i++)
        {
            var name = pjrs[i];
            var item = projects.Find(it => string.Compare(it.Name, name, true) == 0);
            if (item != null)
            {
                if (item.Target == EditorUserBuildSettings.activeBuildTarget || prjsToBuild.Count == 0)
                {
                    prjsToBuild.Insert(0, item);
                }
                else
                {
                    var index = prjsToBuild.FindIndex(0, data => data.Target == item.Target);
                    if (index == -1)
                    {
                        prjsToBuild.Add(item);
                    }
                    else
                    {
                        prjsToBuild.Insert(index, item);
                    }
                }
            }
        }


        double startTime = EditorApplication.timeSinceStartup;
        bool interrupt = false;
        EditorUtility.DisplayCancelableProgressBar("pack tools", "ready to go", 0.0f);
        for (int i = 0; i < prjsToBuild.Count; i++)
        {
            var item = prjsToBuild[i];

            EditorUtility.DisplayCancelableProgressBar("pack tools", "do packing, run task:" + item.Name + ", " + i + "of" + prjsToBuild.Count, (float)i / prjsToBuild.Count);
            
            if (!BuildTools.BuildPlatform(item, compress, saveAs, encrypt, buildAsSetup))
            {
                interrupt = true;
                break;
            }
        }

        int timeUsed = (int)(EditorApplication.timeSinceStartup - startTime);

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("pack tools", interrupt ? "interrupted" : "all done in " + timeUsed + " sec", "ok");

        if (reveal && !interrupt && Directory.Exists(BuildTools.RELEASE_PATH))
        {
            EditorUtility.RevealInFinder(BuildTools.RELEASE_PATH);
        }

        if (!interrupt && shutdown)
        {
#if UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start("shutdown.exe", "-s -t 120");
#endif
        }
    }

}

