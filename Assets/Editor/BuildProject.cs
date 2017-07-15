using System.Text;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ProjectBuildData
{
    public string Name;
    public BuildTarget Target;
    public string Version;
    public List<string> Scenes;
    public string SymbolDefine;
    public bool DebugBuild;

    public GameOptions Options;
    public Dictionary<string, string> Settings;
    public Dictionary<int, bool> Games;

}


/// <summary>
/// 项目打包
/// @Author:guofeng
/// </summary>
public partial class BuildTools
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ProjectDescAttribute : Attribute
    {
        public string Name;
        //TODO 支持多个target
        public BuildTarget Target;

        public ProjectDescAttribute(string name, BuildTarget pf)
        {
            this.Name = name;
            this.Target = pf;
        }
    }


    public static BuildTargetGroup GetBuildGroupByTarget(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return BuildTargetGroup.Android;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
                return BuildTargetGroup.Standalone;
            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;
        }

        return BuildTargetGroup.Unknown;
    }

    public static DataTable LoadAndCreateProjects(string name)
    {
        DataTable dt = LoadFromDB(name);
        if (dt == null)
        {
            dt = new DataTable("PlayerSettings");
            dt.Columns.Add("ProjectName");
            dt.Columns.Add("Target");
            dt.Columns.Add("Version");
            dt.Columns.Add("Scenes");
            dt.Columns.Add("SymbolDefine");
            dt.Columns.Add("DebugBuild");
            dt.Columns.Add("Icons");
            dt.Columns.Add("Games");

            dt.PrimaryKey = new []{dt.Columns["ProjectName"]};

            FieldInfo[] optionFields = typeof (GameOptions).GetFields();
            foreach (var field in optionFields)
            {
                if (!field.IsPublic || field.IsStatic)
                {
                    continue;
                }

                if (!dt.Columns.Contains(field.Name))
                {
                    dt.Columns.Add(field.Name);
                }
            }

            var settingsProperties =
                typeof (PlayerSettings).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in settingsProperties)
            {
                if (property.CanWrite)
                {
                    dt.Columns.Add(property.Name);
                }
            }

            var types = typeof(PlayerSettings).GetNestedTypes();
            foreach (var type in types)
            {
                dt.Columns.Add(type.Name);

                /*var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
                foreach (var property in properties)
                {
                    Debug.Log("property:" + property.Name + " type:" + type.Name);
                }*/
            }
        }
        else
        {
            if (!dt.Columns.Contains("Games"))
            {
                dt.Columns.Add("Games");
            }

            if (!dt.Columns.Contains("Icons"))
            {
                dt.Columns.Add("Icons");
            }
        }

        return dt;
    }


    public static List<ProjectBuildData> LoadProjectList()
    {
       /* try
        {*/
            DataTable dt = LoadAndCreateProjects(PROJECTS_CONFIG_FILE);
            List<ProjectBuildData> projects = new List<ProjectBuildData>();

            PropertyInfo[] settingsFields = typeof(PlayerSettings).GetProperties(BindingFlags.Public | BindingFlags.Static);
            FieldInfo[] optionFields = typeof(GameOptions).GetFields();

            foreach (DataRow dr in dt.Rows)
            {
                ProjectBuildData data = new ProjectBuildData();
                data.Name = dr["ProjectName"].ToString();
                data.Target = (BuildTarget)Enum.Parse(typeof(BuildTarget), dr["Target"].ToString());
                data.Scenes = new List<string>(dr["Scenes"].ToString().Split(';'));
                data.Version = dr["Version"].ToString();
                data.SymbolDefine = dr["SymbolDefine"].ToString();
                data.DebugBuild = bool.Parse(dr["DebugBuild"].ToString());

                data.Options = new GameOptions();
                foreach (var field in optionFields)
                {
                    if (!field.IsPublic || field.IsStatic || !dt.Columns.Contains(field.Name))
                    {
                        continue;
                    }

                    if (field.FieldType == typeof(string))
                    {
                        field.SetValue(data.Options, dr[field.Name].ToString());
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        bool value = false;
                        if (bool.TryParse(dr[field.Name].ToString(), out value))
                        {
                            field.SetValue(data.Options, value);
                        }
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        field.SetValue(data.Options, int.Parse(dr[field.Name].ToString()));
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        field.SetValue(data.Options, Enum.Parse(field.FieldType, dr[field.Name].ToString()));
                    }
                    else if (field.FieldType.IsGenericType)
                    {
                        var json = dr[field.Name].ToString();
                        var type = typeof (LitJson.JsonMapper);
                        MethodInfo method = null;
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                        foreach (var methodInfo in methods)
                        {
                            var paramters = methodInfo.GetParameters();
                            if (methodInfo.Name == "ToObject" &&
                                methodInfo.IsGenericMethod &&
                                 paramters[0].ParameterType == typeof(string))
                            {
                                method = methodInfo;
                                break;
                            }
                        }

                        method = method.MakeGenericMethod(new Type[] {field.FieldType});
                        var obj = method.Invoke(null, new object[] {json});
                        field.SetValue(data.Options, obj);
                    }
                }

                data.Settings = new Dictionary<string, string>();
                foreach (var field in settingsFields)
                {
                    if (field.CanWrite)
                    {
                        data.Settings.Add(field.Name, dr[field.Name].ToString());
                    }
                }

                var types = typeof(PlayerSettings).GetNestedTypes();
                foreach (var type in types)
                {
                    if (!dr.Table.Columns.Contains(type.Name))
                    {
                        continue;
                    }

                    data.Settings[type.Name] = dr[type.Name].ToString();
                    /*var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
                    foreach (var property in properties)
                    {
                        Debug.Log("property:" + property.Name + " type:" + type.Name);

                    }*/
                }

                try
                {
                    string str = dr["Games"].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        var games = LitJson.JsonMapper.ToObject<Dictionary<string, bool>>(str);
						data.Games = new Dictionary<int, bool>();
						foreach(var g in games)
						{
							data.Games.Add(int.Parse(g.Key), g.Value);
						}
                    }

                    str = dr["Icons"].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        data.Settings["Icons"] = str;
                    }
                }
                catch (Exception ex)
                {
                    //Debug.LogException(ex);
                }

                projects.Add(data);
            }

            /* }
             catch (System.Exception ex)
             {
                 Debug.LogError(ex.Message);
                 return null;
             }*/

            return projects;
    }

    public static ProjectBuildData CreateProject()
    {
        ProjectBuildData data = new ProjectBuildData();
        data.Options = new GameOptions();
        data.Name = "";
        data.Target = EditorUserBuildSettings.activeBuildTarget;
        data.Scenes = new List<string>();

        data.Settings = new Dictionary<string, string>();
        data.Games = new Dictionary<int, bool>();
        return data;
    }

    public static void DeleteProject(ProjectBuildData data)
    {
        DataRow drProject = null;
        DataTable dt = LoadAndCreateProjects(PROJECTS_CONFIG_FILE);
        foreach (DataRow dr in dt.Rows)
        {
            string name = dr["ProjectName"].ToString();
            if (name == data.Name)
            {
                drProject = dr;
                break;
            }
        }

        if (drProject == null)
        {
            return;
        }

        dt.Rows.Remove(drProject);
        SaveToDB(dt, PROJECTS_CONFIG_FILE);
    }

    public static bool SaveToDB(DataTable dt, string fileName)
    {
        DBHelper.Save(dt, fileName);
        return true;
    }

    public static DataTable LoadFromDB(string fileName)
    {
        //return CSVFileHelper.OpenCSV(fileName);
        var dt = DBHelper.Load(fileName, "PlayerSettings");
        return dt;
    }

    public static bool SaveProject(bool add, ProjectBuildData data)
    {
        DataRow drProject = null;
        DataTable dt = LoadAndCreateProjects(PROJECTS_CONFIG_FILE);
        foreach (DataRow dr in dt.Rows)
        {
            string name = dr["ProjectName"].ToString();
            if (name == data.Name)
            {
                drProject = dr;
                break;
            }
        }

        if (add && drProject != null)
        {
            Debug.LogError("exist same project name already " + data.Name);
            return false;
        }
        else if (!add && drProject == null)
        {
            Debug.LogError("project not exist " + data.Name);
            return false;
        }
        else if (add)
        {
            drProject = dt.NewRow();
            dt.Rows.Add(drProject);
        }

        drProject["ProjectName"] = data.Name;
        drProject["Version"] = data.Version;

        List<string> sceneList = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            sceneList.Add(scene.path);
        }
        string scenes = string.Join(";", sceneList.ToArray());
        drProject["Scenes"] = scenes;

        drProject["Target"] = EditorUserBuildSettings.activeBuildTarget;
        drProject["SymbolDefine"] = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        drProject["DebugBuild"] = EditorUserBuildSettings.development.ToString();

        FieldInfo[] optionFields = typeof(GameOptions).GetFields();
        foreach (var field in optionFields)
        {
            if (!field.IsPublic || field.IsStatic)
            {
                continue;
            }

            if (!dt.Columns.Contains(field.Name))
            {
                dt.Columns.Add(field.Name);
            }

            var obj = field.GetValue(data.Options);
            if (obj != null)
            {
                if (field.FieldType == typeof(string) || 
                    field.FieldType == typeof(bool) ||
                    field.FieldType == typeof(int) || 
                    field.FieldType.IsEnum )
                {
                    drProject[field.Name] = obj.ToString();
                }
                else if(field.FieldType.IsGenericType)
                {
                    drProject[field.Name] = LitJson.JsonMapper.ToJson(obj);
                }
            }
        }

        PropertyInfo[] fields = typeof(PlayerSettings).GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (var field in fields)
        {
            if (!field.CanWrite)
            {
                continue;
            }

            var obj = field.GetValue(null, null);
            if (obj != null)
            {
                drProject[field.Name] = obj.ToString();
                if (field.PropertyType == typeof(Texture2D))
                {
                    var texture = obj as Texture2D;
                    drProject[field.Name] = texture.name;
                }
            }
        }

        var types = typeof(PlayerSettings).GetNestedTypes();
        foreach (var type in types)
        {
            var sb = new StringBuilder();
            var writer = new LitJson.JsonWriter(sb);

            writer.WriteObjectStart();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                writer.WritePropertyName(property.Name);
                var obj = property.GetValue(null, null);
                writer.Write(obj != null ? obj.ToString() : "");
            }

            writer.WriteObjectEnd();

            if (!drProject.Table.Columns.Contains(type.Name))
            {
                drProject.Table.Columns.Add(type.Name);
            }
            drProject[type.Name] = sb.ToString();
        }

        var iconList = new List<string>();
        var group = GetBuildGroupByTarget(data.Target);
        var icons = PlayerSettings.GetIconsForTargetGroup(group);

        foreach (var texture2D in icons)
        {
            if (texture2D != null)
            {
                var path = AssetDatabase.GetAssetPath(texture2D.GetInstanceID());
                iconList.Add(path);
            }
        }

        var iconsStr = string.Join(",", iconList.ToArray());
        drProject["Icons"] = iconsStr;

        if (data.Games != null)
        {
            var str = LitJson.JsonMapper.ToJson(data.Games);
            drProject["Games"] = str;
        }

        SaveToDB(dt, PROJECTS_CONFIG_FILE);

        //SaveConfig(data);

        return true;
    }


    public static DataRow LoadProject(string name)
    {
        DataTable dt = CSVFileHelper.OpenCSV(PROJECTS_CONFIG_FILE);
        if (dt == null)
        {
            return null;
        }

        foreach (DataRow dr in dt.Rows)
        {
            if (string.Compare(name, dr["ProjectName"].ToString(), true) == 0)
            {
                return dr;
            }
        }

        return null;
    }

}

