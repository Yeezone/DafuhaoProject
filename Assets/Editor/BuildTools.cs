using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

using Game.Utili;

using System.Reflection;
using Ionic.Zip;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// 打包类
/// @Author:guofeng
/// </summary>
public partial class BuildTools
{
    //全局常量
    public static string PROJECTS_CONFIG_FILE = Application.dataPath + "/Projects/PlayerSettings";
    public static string RELEASE_PATH = Application.dataPath.Replace("Assets", "Release") + "/";
    public static string BUILD_ASSETS_PATH = Application.dataPath.Replace("Assets", "BuildAssets") + "/";
    public static string TEMP_PATH = Application.dataPath.Replace("Assets", "StreamingTemp");

    public static bool BuildPlatform(ProjectBuildData data, bool compress, bool saveAs, bool encrypt, bool buildAsSetup)
    {
        Debug.Log(">------------------ start build : " + data.Name + "   target:" + data.Target.ToString());

        EditorUserBuildSettings.SwitchActiveBuildTarget(data.Target);

        string symDefine = data.SymbolDefine;
        if (!encrypt)
        {
            if (!symDefine.Contains("DISABLE_ENCRYPT_FILES"))
            {
                symDefine += ";DISABLE_ENCRYPT_FILES";
            }
        }

        ApplyPlayerSettings(data);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            symDefine
            );

        AssetDatabase.Refresh();

        //获取SDK目录
        //string sdkDirPath = GetSDKDir();
        //设置放置apk文件目录

        BuildOptions options = BuildOptions.None;

        string dir = RELEASE_PATH + PlayerSettings.companyName + "/" + EditorUserBuildSettings.activeBuildTarget + "/";
        if (saveAs)
        {
            dir = EditorUtility.OpenFolderPanel("Please select the target directory", dir, "") + "/";
        }

        string fileName = "";
        string destFileName = "";
        string saveAsFileName = "";
        bool compressNeeds = compress;

        //string apkPath = SettingAPKDir();
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            fileName = CFile.GetUnusedFileName(dir, PlayerSettings.companyName, ".apk");
            fileName += "_V" + data.Version;
            if (!encrypt)
            {
                fileName += ".apk";
            }

            destFileName += fileName;
            saveAsFileName = destFileName;
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
        {
            if (compressNeeds || buildAsSetup)
            {
                saveAsFileName = PlayerSettings.companyName + "_V" + data.Version;
                fileName = CFile.GetUnusedFileName(dir, saveAsFileName, ".exe");
                saveAsFileName = fileName;
                destFileName = PlayerSettings.companyName + ".exe";

                dir += "__tmp/";
            }
            else
            {
                fileName = CFile.GetUnusedFileName(dir, PlayerSettings.companyName, ".exe");
                destFileName = fileName;
                destFileName += "_V" + data.Version + ".exe";
                saveAsFileName = destFileName;
            }
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            if (compressNeeds)
            {
                dir += "__tmp/";
            }

            destFileName += PlayerSettings.companyName;
            destFileName += "_" + data.Version;
            saveAsFileName = destFileName;
        }
        else
        {
            destFileName += PlayerSettings.companyName;
            saveAsFileName = destFileName;
        }

        Debug.Log("destFileName:" + destFileName);
        Debug.Log("saveAsFileName:" + saveAsFileName);

        /*//SDK转移
        if (!CopySDK(companyName, EditorUserBuildSettings.activeBuildTarget))
        {
            Debug.LogError("CopySDK Error.");
            return false;
        }*/

        //TODO 拷贝随包游戏
        /*if (!CopyAssetbundles(data))
        {
            Debug.LogError("Copy Assetbundles Error");
            return false;
        }*/


        //生成ab包
        /*if (!BuildAssetBundle(data))
        {
            return false;
        }*/

        //拷贝


        //标题画面更改
        if (!CopyLogo())
        {
            return false;
        }
		
		if (!CopyAssetbundles(data))
		{
			return false;
		}

		if (!CopyCustomAssets(data))
        {
            return false;
        }

		//保存配置文件
		if (!SaveConfig(data, encrypt))
        {
            return false;
        }


        if (encrypt)
        {
            options |= BuildOptions.AcceptExternalModificationsToPlayer;
        }

        //保存配置文件
        if (!BuildPlayer(dir, destFileName, options))
        {
            return false;
        }

        if (encrypt && !Encrypt(dir, destFileName))
        {
            return false;
        }

        //处理编译完成后问题
        if (compressNeeds && !DoCompress(dir, destFileName, saveAsFileName))
        {
            return false;
        }

        if (buildAsSetup && !MakeSetup(dir, destFileName, saveAsFileName))
        {
            return false;
        }

        //处理编译完成后问题
        if (!BuildEnd(dir))
        {
            return false;
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            data.SymbolDefine
            );

        AssetDatabase.Refresh();

        Debug.Log(">------------------ Build finished! Name: " + data.Name + "   Target: " + EditorUserBuildSettings.activeBuildTarget);

        return true;
    }

    private static bool AppendKeyStoreToFile(string dir, string fileName)
    {
        string path = dir + fileName + "/" + PlayerSettings.productName;
        using (var fs = File.OpenWrite(path + "/local.properties"))
        {
            string keystorePath = PlayerSettings.Android.keystoreName;
            if (!Path.IsPathRooted(PlayerSettings.Android.keystoreName))
            {
                keystorePath = BUILD_ASSETS_PATH + "/../" + keystorePath;
            }

            var strs = new[] {
					"key.store=" + keystorePath,
					"key.store.password="+ PlayerSettings.Android.keystorePass,
					"key.alias=" + PlayerSettings.Android.keyaliasName,
					"key.alias.password=" + PlayerSettings.Android.keyaliasPass
				};

            foreach (var item in strs)
            {
                var bytes = Encoding.Default.GetBytes(item + "\n");
                fs.Seek(0, SeekOrigin.End);
                fs.Write(bytes, 0, bytes.Length);
            }

            fs.Flush();
        }

        return true;
    }

    /*private static string FindVariableInPath(string keyword)
    {
        var pathVar = Environment.GetEnvironmentVariable("PATH");
        var vars = pathVar.Split(':');
        Console.WriteLine(pathVar);
        return Array.Find(vars, item => item.Contains(keyword));
    }*/

    private static bool UpdateAndroidProject(string dir, string fileName)
    {
        Debug.Log("update android project");

        string path = dir + fileName + "/" + PlayerSettings.productName;

        var sdkPath = Environment.GetEnvironmentVariable("ANDROID_SDK");
        if (string.IsNullOrEmpty(sdkPath))
        {
            Debug.Log("not found android-sdk in environment variable");
            return false;
        }

#if UNITY_EDITOR_WIN
        string androidPath = sdkPath + "/tools/android.bat";
#else
        string androidPath = sdkPath + "/tools/android";
#endif

        using (var process = new System.Diagnostics.Process())
        {
            process.StartInfo.FileName = androidPath;
            process.StartInfo.Arguments = "update project --path ./ --name " + fileName;
            process.StartInfo.WorkingDirectory = path;
            // 必须禁用操作系统外壳程序  
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
            process.Start();

            Debug.Log("exec:" + androidPath + " arguments:" + process.StartInfo.Arguments);

            while (!process.StandardOutput.EndOfStream)
            {
                var str = process.StandardOutput.ReadLine();
                Debug.Log(str);
            }

            process.WaitForExit();
            process.Close();
        }

        if (!AppendKeyStoreToFile(dir, fileName))
        {
            return false;
        }

        return true;
    }


    private static bool BuildAndroidProject(string dir, string fileName)
    {
        try
        {
            EditorUtility.DisplayProgressBar("building apk", "ant clean", 0.1f);

            string path = dir + fileName;

            var antPath = Environment.GetEnvironmentVariable("ANT_HOME");
            if (string.IsNullOrEmpty(antPath))
            {
                Debug.Log("not found ant path in environment variable");
                return false;
            }

#if UNITY_EDITOR_WIN
            string ant = antPath + "/bin/ant.bat";
#else
        string ant = antPath + "/bin/ant";
#endif

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = ant;
                process.StartInfo.Arguments = "clean";
                process.StartInfo.WorkingDirectory = path + "/" + PlayerSettings.productName;
                // 必须禁用操作系统外壳程序  
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                process.Start();

                Debug.Log("exec:" + ant + " arguments:" + process.StartInfo.Arguments);

                while (!process.StandardOutput.EndOfStream)
                {
                    var str = process.StandardOutput.ReadLine();
                    Debug.Log(str);
                }

                process.WaitForExit();
                process.Close();
            }

            EditorUtility.DisplayProgressBar("building apk", "exec ant", 0.5f);

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = ant;
                process.StartInfo.Arguments = EditorUserBuildSettings.development ? "debug" : "release";
                process.StartInfo.WorkingDirectory = path + "/" + PlayerSettings.productName;
                // 必须禁用操作系统外壳程序  
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                process.Start();

                Debug.Log("exec:" + ant + " arguments:" + process.StartInfo.Arguments);

                float progress = 0.6f;
                while (!process.StandardOutput.EndOfStream)
                {
                    var str = process.StandardOutput.ReadLine();
                    Debug.Log(str);
                    progress += 0.001f;
                    EditorUtility.DisplayProgressBar("building apk", str, progress);
                }

                process.WaitForExit();
                process.Close();
            }

            EditorUtility.DisplayProgressBar("building apk", "copy files", 0.8f);

            string apkFileName = fileName +
                                 (EditorUserBuildSettings.development ? "-debug" : "-release") + ".apk";
            string apkPath = path + "/" + PlayerSettings.productName + "/bin/" + apkFileName;
            if (!File.Exists(apkPath))
            {
                Debug.LogError("not found apk file:" + apkPath);
                return false;
            }

            if (File.Exists(dir + apkFileName))
            {
                File.Delete(dir + apkFileName);
            }

            Debug.Log("copy file from:" + apkPath + " to:" + dir + apkFileName);
            File.Copy(apkPath, dir + apkFileName);
            Directory.Delete(path, true);

            EditorUtility.DisplayProgressBar("building apk", "ant done", 1.0f);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public static byte[] Encrypt(byte[] bytes, byte[] keys)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)~bytes[i];
        }
        return bytes;
        /*using (var provider = new DESCryptoServiceProvider())
        {
            int len = bytes.Length/8;
            using (var ms = new MemoryStream(bytes, 0, len*8))
            {
                using (var cs = new CryptoStream(ms, provider.CreateEncryptor(keys, keys), CryptoStreamMode.Write))
                {
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }*/
    }

    private static byte[] DesKeysVector = { 0x34, 0xaf, 0xda, 0x08, 0x3a, 0x11, 0xcd, 0xff };

    private static bool Encrypt(string dir, string fileName)
    {
        try
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
            {
                //step1, encrypt the lib
                string name = dir + fileName.Replace(".exe", "_Data") + "/Managed/Assembly-CSharp.dll";
                Debug.Log("encrypt assymble file:" + name);
                var bytes = File.ReadAllBytes(name);
                ;
                bytes = Encrypt(bytes, DesKeysVector);
                File.WriteAllBytes(name, bytes);

                //step2, copy libmono into the lib dir
                string src = Application.dataPath + "/../BuildAssets/Mono/Windows/Mono.dll";
                string dest = dir + fileName.Replace(".exe", "_Data") + "/Mono/Mono.dll";
                File.Copy(src, dest, true);
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                //step1, encrypt the lib
                string name = dir + fileName + "/" + PlayerSettings.productName +
                              "/assets/bin/Data/Managed/Assembly-CSharp.dll";
                Debug.Log("encrypt assymble file:" + name);
                var bytes = File.ReadAllBytes(name);
                bytes = Encrypt(bytes, DesKeysVector);
                File.WriteAllBytes(name, bytes);


                //step2, copy libmono into the lib dir
                if ((PlayerSettings.Android.targetDevice & AndroidTargetDevice.ARMv7) != 0)
                {
                    string src = Application.dataPath + "/../BuildAssets/Mono/Android/armeabi-v7a";
                    string dest = dir + fileName + "/" + PlayerSettings.productName + "/libs/armeabi-v7a";
                    CFile.CopyDirectory(src, dest);
                }

                if ((PlayerSettings.Android.targetDevice & AndroidTargetDevice.x86) != 0)
                {
                    string src = Application.dataPath + "/../BuildAssets/Mono/Android/x86";
                    string dest = dir + fileName + "/" + PlayerSettings.productName + "/libs/x86";
                    CFile.CopyDirectory(src, dest);
                }

                //写key
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    if (!UpdateAndroidProject(dir, fileName))
                    {
                        Debug.LogError("update android project failed");
                        return false;
                    }
                }

                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    if (!BuildAndroidProject(dir, fileName))
                    {
                        Debug.LogError("build android project failed");
                        return false;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }


    public static void ApplyPlayerSettings(ProjectBuildData data)
    {
        Type type = typeof(PlayerSettings);


        var types = typeof(PlayerSettings).GetNestedTypes();
        foreach (var nested in types)
        {
            if (!data.Settings.ContainsKey(nested.Name))
            {
                continue;
            }

            string val = data.Settings[nested.Name];
            var reader = new LitJson.JsonReader(val);
            while (reader.Read())
            {
                switch (reader.Token)
                {
                    case LitJson.JsonToken.PropertyName:
                        {
                            string key = reader.Value.ToString();

                            reader.Read();

                            var info = nested.GetProperty(key);
                            if (info == null || !info.CanWrite)
                            {
                                Debug.LogWarning("ingore property:" + key);
                                continue;
                            }
                            if (info.PropertyType == typeof(string))
                            {
                                info.SetValue(null, reader.Value, null);
                            }
                            else if (info.PropertyType == typeof(bool))
                            {
                                info.SetValue(null, bool.Parse(reader.Value.ToString()), null);
                            }
                            else if (info.PropertyType == typeof(int))
                            {
                                info.SetValue(null, int.Parse(reader.Value.ToString()), null);
                            }
                            else if (info.PropertyType.IsEnum)
                            {
                                info.SetValue(null, Enum.Parse(info.PropertyType, reader.Value.ToString()), null);
                            }
                            else
                            {
                                Debug.LogWarning("unidentifiable property named:" + key + " type:" + info.PropertyType.Name);
                            }

                            break;
                        }
                }
            }
        }

        foreach (var col in data.Settings)
        {
            PropertyInfo info = type.GetProperty(col.Key);
            if (info == null || !info.CanWrite)
            {
                Debug.LogWarning("ignore property:" + col.Key);
                continue;
            }

            Debug.LogWarning("set property:" + col.Key);
            if (info.PropertyType == typeof(string))
            {
                info.SetValue(null, col.Value, null);
            }
            else if (info.PropertyType == typeof(bool))
            {
                info.SetValue(null, bool.Parse(col.Value), null);
            }
            else if (info.PropertyType == typeof(int))
            {
                info.SetValue(null, int.Parse(col.Value), null);
            }
            else if (info.PropertyType.IsEnum)
            {
                info.SetValue(null, Enum.Parse(info.PropertyType, col.Value), null);
            }
            else
            {
                Debug.LogWarning("unidentifiable field named:" + col.Key + " type:" + info.PropertyType.Name);
            }
        }

        if (data.Settings.ContainsKey("Icons"))
        {
            string icons = data.Settings["Icons"];
            var iconsList = icons.Split(',');
            var iconsTextureList = new List<Texture2D>();
            foreach (var str in iconsList)
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(str);
                iconsTextureList.Add(texture);    
            }

            var group = GetBuildGroupByTarget(data.Target);
            var iconSizes = PlayerSettings.GetIconSizesForTargetGroup(group);
            if (iconSizes.Length > iconsTextureList.Count)
            {
                int count = iconSizes.Length - iconsTextureList.Count;
                for (int i = 0; i < count; i++)
                {
                    iconsTextureList.Add(null);
                }
            }
            PlayerSettings.SetIconsForTargetGroup(group, iconsTextureList.ToArray());
        }

        ApplySelectedScene(data.Scenes);

        if (data.DebugBuild)
        {
            EditorUserBuildSettings.development = data.DebugBuild;
            EditorUserBuildSettings.connectProfiler = true;
            EditorUserBuildSettings.allowDebugging = true;
        }
        else
        {
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.connectProfiler = false;
            EditorUserBuildSettings.allowDebugging = false;
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, data.SymbolDefine);
        PlayerSettings.bundleVersion = data.Version;

        AssetDatabase.Refresh();
    }

    private static void ApplySelectedScene(List<string> scenesValue)
    {
        List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>();
        foreach (var scenePath in scenesValue)
        {
            var scene = new EditorBuildSettingsScene(scenePath, true);
            sceneList.Add(scene);
        }

        EditorBuildSettings.scenes = sceneList.ToArray();
    }

    /// <summary>
    /// 获取SDK目录
    /// </summary>
    /// <returns></returns>
    /*private static string GetSDKDir()
    {
        return EditorUtility.OpenFolderPanel("Please select the android sdk directory", SDK_PATH_DEFAULT, "");
    }*/

    /// <summary>
    /// 更改标题画面
    /// </summary>
    /// <param name="titleName"></param>
    private static bool CopyLogo()
    {
        try
        {
            string src = Application.dataPath + "/Projects/" + PlayerSettings.companyName + "/Logo";
            string desc = Application.dataPath + "/Logo";

            Debug.Log("copy file from:" + src + " to:" + desc);

            if (!Directory.Exists(src))
            {
                Debug.Log(src + " dir is not exists");
                return true;
            }

            if (Directory.Exists(desc))
            {
                CFile.DeleteDirectory(desc);
            }

            CFile.CopyDirectory(src, desc);
      
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }


    /// <summary>
    /// 拷贝自定义文件
    /// </summary>
    /// <returns></returns>
	private static bool CopyCustomAssets(ProjectBuildData data)
    {
        try
        {
            var srcDirs = new[] { "Streaming", "Audio", "Res" };
            var destDirs = new[] { "StreamingAssets", "Audio", "Resources" };
            for (int i = 0; i < srcDirs.Length; i++)
            {
                string srcDir = srcDirs[i];
                string destDir = destDirs[i];

                string src = Application.dataPath + "/Projects/" + PlayerSettings.companyName + "/" + srcDir;
                string defaultSrc = Application.dataPath + "/Projects/default/" + srcDir;
                string dest = Application.dataPath + "/" + destDir;

                Debug.Log("copy file from:" + src + " to:" + dest);

                if (!Directory.Exists(src))
                {
                    src = defaultSrc;
                    if (!Directory.Exists(src))
                    {
                        Debug.Log(src + " dir is not exists");
						continue;
                    }
                }

                CFile.CopyDirectory(src, dest);
            }

            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }

    private static bool SaveConfig(ProjectBuildData data, bool encrypt)
    {
        try
        {
            string path = Application.streamingAssetsPath + "/" + GameHelper.GetBuildTargetName(data.Target) + "/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + GlobalConst.Res.AppConfigFileName;
            string text = LitJson.JsonMapper.ToJson(data.Options);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            if (encrypt)
            {
                text = DESEncryption.Encrypt(text, GlobalConst.Res.EncryptPassword);
            }

            File.WriteAllText(fileName, text);


            fileName = path + GlobalConst.Res.GameConfigFileName;
            if (data.Games.Count > 0)
            {
                var ids = new List<int>();
                foreach (var game in data.Games)
                {
                    ids.Add(game.Key);
                }
                var games = GameWindow.ScanGames(data.Target, ids);
                foreach (var game in data.Games)
                {
                    var item = games.Find((config) => config.ID == game.Key);
                    item.Packed = game.Value;
                }
                text = LitJson.JsonMapper.ToJson(games);
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, text);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 拷贝SDK内容
    /// </summary>
    /// <param name="sdkName"></param>
    /// <returns></returns>
    private static bool CopySDK(string name, BuildTarget target)
    {
        try
        {
            string path = Application.dataPath + "/Projects/" + name + "/";
            string src = path + "Plugin/" + target.ToString();
            string desc = path + "Plugins/" + target.ToString();

            if (!Directory.Exists(src))
            {
                Debug.Log("sdk file is not exists");
                return true;
            }

            if (Directory.Exists(desc))
            {
                Directory.Delete(desc, true);
            }

            if (!Directory.Exists(path + "Plugins"))
            {
                Directory.CreateDirectory(path + "Plugins");
            }

            Directory.Move(src, desc);

            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }

        return true;
    }


    /*private static bool BuildAssetBundle(ProjectBuildData data)
    {
        try
        {
            string path = ABTools.outputPath + GetBuildTargetName(data.Target) + "/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var abList = new List<AssetBundleInfo>();
            string fileName = path + GlobalConst.Res.ResVersionFileName;
            if (File.Exists(fileName))
            {
                string text = File.ReadAllText(fileName);
                abList = LitJson.JsonMapper.ToObject<List<AssetBundleInfo>>(text);
            }

            if (!BuildAssetBundle(path, data.Target, data.Games, ref abList))
            {
                return false;
            }

            string configFileName = path + GlobalConst.Res.ResVersionFileName;
            string configText = LitJson.JsonMapper.ToJson(abList);
            if (File.Exists(configFileName))
            {
                File.Delete(configFileName);
            }
            File.WriteAllText(configFileName, configText);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }

        return true;
    }*/


    private static bool CopyAssetbundles(ProjectBuildData data)
    {
        try
        {
            string path = ABTools.outputPath + GameHelper.GetBuildTargetName(data.Target);
            string destPath = Application.streamingAssetsPath + "/" + GameHelper.GetBuildTargetName(data.Target);

            Debug.Log("copy ab from:" + path + "   to:" + destPath);

            if (Directory.Exists(TEMP_PATH))
            {
                Directory.Delete(TEMP_PATH, true);
            }

            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Debug.Log("move directory from:" + Application.streamingAssetsPath + " to:" + TEMP_PATH);
                Directory.Move(Application.streamingAssetsPath, TEMP_PATH);
            }

            if (!Directory.Exists(destPath))
            {
                Debug.Log("create dir:" + destPath);
                Directory.CreateDirectory(destPath);
            }

            if (!Directory.Exists(path))
            {
                Debug.LogError("src dir is not exists");
                return true;
            }

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                string fileName = new FileInfo(file).Name;
                string dest = destPath + "/" + fileName;
                Debug.Log("copy file:" + file + "   to:" + dest);
                File.Copy(file, dest, true);
            }

            string srcDir = path + "/games";
            if (!Directory.Exists(srcDir))
            {
                Directory.CreateDirectory(srcDir);
            }

            /*string destDir = destPath + "/games";
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }*/


            if (data.Games != null && data.Games.Count > 0)
            {
                var ids = new List<int>();
                foreach (var game in data.Games)
                {
                    ids.Add(game.Key);
                }
                var games = GameWindow.ScanGames(data.Target, ids);
                foreach (var item in games)
                {
                    if (data.Games[item.ID])
                    {
                        /* string srcGamesPath = srcDir + "/" + item.SceneName;
                         if (!Directory.Exists(srcGamesPath))
                         {
                             Debug.LogError("src game dir  is not exists! Dir:" + srcGamesPath);
                             return true;
                         }

                         string destGamesPath = destDir + "/" + item.SceneName;
                         if (!Directory.Exists(destGamesPath))
                         {
                             Directory.CreateDirectory(destGamesPath);
                         }*/

                        Debug.Log("ready copy pack game. Name:" + item.ID);

                        foreach (var assetBundle in item.AssetBundles)
                        {
                            string file = path + "/" + assetBundle;
                            if (!File.Exists(file))
                            {
                                Debug.LogError("game file is not exists:" + file);
                                return false;
                            }

                            string destFile = destPath + "/" + assetBundle;
                            Debug.Log(destFile);
                            string destFilePath = Path.GetDirectoryName(destFile);
                            if (!Directory.Exists(destFilePath))
                            {
                                Directory.CreateDirectory(destFilePath);
                            }

                            Debug.Log("copy game file:" + file + "   to:" + destFile);
                            File.Copy(file, destFile);
                        }
                    }
                }

            }

            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    /// <summary>
    /// 设置文件打包目录
    /// </summary>
    /// <returns></returns>
    /*private static string SettingAPKDir()
    {
        return EditorUtility.OpenFolderPanel("target directory", APK_PATH_DEFAULT, "");
    }*/

    /// <summary>
    /// 编译打包
    /// </summary>
    /// <param name="filePath"></param>
    private static bool BuildPlayer(string dir, string fileName, BuildOptions options)
    {
        try
        {
            string filePath = dir + fileName;

            Debug.Log("build player in path:" + filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //获取所有场景
            var sceneToBuild = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                sceneToBuild.Add(scene.path);
            }

            //打包开始
            string res = BuildPipeline.BuildPlayer(sceneToBuild.ToArray(), filePath, EditorUserBuildSettings.activeBuildTarget, options);
            if (res.Length > 0)
            {
                Debug.LogError("BuildPlayer failure: " + res);
                return false;
            }

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }

        return true;
    }


    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static bool DoCompress(string srcDir, string destFileName, string saveAs)
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            return true;
        }

        try
        {
            EditorUtility.DisplayProgressBar("give me a sec", "ready to compress", 0.0f);

            string zip = BUILD_ASSETS_PATH + "/Tools/7z.exe";
            string fileName = srcDir + "../" + saveAs;
            string dir = Path.GetDirectoryName(fileName);
            fileName = CFile.GetUnusedFileName(dir + "/", Path.GetFileNameWithoutExtension(fileName), ".zip") + ".zip";
            fileName = dir + "/" + fileName;
            string args = string.Format("-tzip a {0} {1}/*", fileName, srcDir);

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = zip;
                process.StartInfo.Arguments = args;
                //process.StartInfo.WorkingDirectory = BUILD_ASSETS_PATH + "Inno/";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                process.Start();

                Debug.Log("exec=>" + zip + " " + args);

                float progress = 0.0f;
                while (!process.StandardOutput.EndOfStream)
                {
                    var str = process.StandardOutput.ReadLine();
                    progress += 0.001f;
                    EditorUtility.DisplayProgressBar("compressing", str, progress);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return false;
                }
            }

            EditorUtility.ClearProgressBar();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }

    private static string GenericScript(string src, string fileName, string dir)
    {
        if (!File.Exists(src))
        {
            return null;
        }

        string text = File.ReadAllText(src);
        text = text.Replace("$ProductName$", PlayerSettings.productName);
        text = text.Replace("$CompanyName$", PlayerSettings.companyName);
        text = text.Replace("$DestFileName$", fileName);
        text = text.Replace("$Files$", dir);
        text = text.Replace("$SourceDir$", Path.GetDirectoryName(src));

        Encoding dest = Encoding.GetEncoding("gb2312");
        var bytes = Encoding.Convert(Encoding.Default, dest, Encoding.Default.GetBytes(text));
        string str = dest.GetString(bytes);

        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, str, dest);
        return tempFile;
    }

    private static bool MakeSetup(string srcDir, string destFileName, string saveAs)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows)
        {
            return true;
        }

        try
        {
            string builder = BUILD_ASSETS_PATH + "/Inno/bin/ISCC.exe";
            string scriptFile = BUILD_ASSETS_PATH + "Inno/Build.iss";
            string script = GenericScript(scriptFile, saveAs, srcDir);
            string args = string.Format("/O{0} {1}", srcDir + "/../", script);

            EditorUtility.DisplayProgressBar("building setup package", "take a while", 0.5f);

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = builder;
                process.StartInfo.Arguments = args;
                process.StartInfo.WorkingDirectory = BUILD_ASSETS_PATH + "Inno/";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                process.Start();

                Debug.Log("exec=>" + builder + " " + args);

                float progress = 0.0f;
                while (!process.StandardOutput.EndOfStream)
                {
                    var str = process.StandardOutput.ReadLine();
                    progress += 0.001f;
                    EditorUtility.DisplayProgressBar("processing", str, progress);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return false;
                }
            }

            EditorUtility.DisplayProgressBar("building setup package", "done", 0.5f);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }


    /// <summary>
    /// 编译打包善后处理
    /// </summary>
    /// <param name="tittle"></param>
    private static bool BuildEnd(string dir)
    {
        try
        {
            Debug.Log("restore directory from:" + TEMP_PATH + " to:" + Application.streamingAssetsPath + " dir:" + dir);
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath, true);
            }

            if (Directory.Exists(TEMP_PATH))
            {
                Directory.Move(TEMP_PATH, Application.streamingAssetsPath);
            }

            if (dir.EndsWith("/__tmp/"))
            {
                Directory.Delete(dir, true);
            }


            /*string src = Application.dataPath + "/Projects/" + name + "/Plugins/" + target.ToString();
			if(Directory.Exists(src))
			{
				Directory.Delete(src, true);
			}
            AssetDatabase.Refresh();*/
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }

        return true;
    }

    private static bool BuildResotre(string name, BuildTarget target)
    {
        try
        {
            /*string path = Application.dataPath + "/Projects/" + name+"/";
            string src = path + "Plugins/" + target.ToString();
            string desc = path + "Plugin/" + target.ToString();
			
            if (!Directory.Exists(src))
            {
                Debug.Log("sdk file is not exists");
                return true;
            }

            if(Directory.Exists(desc))
            {
                Directory.Delete(desc);
            }

            Debug.LogError("src:"+src+"       desc:"+desc);
            Directory.Move(src, desc);
			
            AssetDatabase.Refresh();*/
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }

        return true;
    }
}
