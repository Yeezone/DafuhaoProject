using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using com.QH.QPGame;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;

public class ABTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string assetPath = Application.dataPath + "/Resources/";//"/rawdata"----android
	// AssetBundle 的输入路径
    public static string outputPath = Application.dataPath + "/../Release/Assetbundle/";
	// oldAssetBundle 旧版本的资源路径
    public static string oldassetsbundlePath = Application.dataPath + "/../Release/OldAssetbundle/";
	// 修改过的文件目录
    public static string alterfilePath = Application.dataPath + "/../Release/AlterAssetbundle/";
    public static string newFilePath = Application.dataPath + "/../Release/NewAssetbundle/";
	// 生成表
	public static DataConfig m_new_dc = new DataConfig();
	public static DataConfig m_old_dc = new DataConfig();
	public static DataConfig m_alter_dc = new DataConfig();

    public static List<BuildTarget> GetAllBuildTargets()
    {
        var targets = new List<BuildTarget>();
        targets.Add(EditorUserBuildSettings.activeBuildTarget);

        if (!targets.Contains(BuildTarget.StandaloneWindows))
        {
            targets.Add(BuildTarget.StandaloneWindows);
        }

        if (!targets.Contains(BuildTarget.iOS))
        {
            targets.Add(BuildTarget.iOS);
        }

        if (!targets.Contains(BuildTarget.Android))
        {
            targets.Add(BuildTarget.Android);
        }

        return targets;
    }

    [MenuItem("BuildPacket/Assetbundle/BuildCurrent")]
    public static void BuildCurrentAssetbundle()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        var games = GameWindow.ScanGames(target);
        games.ForEach(item => { item.Packed = true; });
        BuildAssetbundle(target, games);
    }

    [MenuItem("BuildPacket/Assetbundle/BuildAll")]
    public static void BuildAllAssets()
    {
        var targets = GetAllBuildTargets();
        foreach (var buildTarget in targets)
        {
            var games = GameWindow.ScanGames(buildTarget);
            games.ForEach(item => { item.Packed = true; });
            BuildAssetbundle(buildTarget, games);
        }
    }

    [MenuItem("BuildPacket/Assetbundle/BuildWindows")]
    public static void BuildAssetsForWindows()
    {
        var games = GameWindow.ScanGames(BuildTarget.StandaloneWindows);
        games.ForEach(item => { item.Packed = true; });
        BuildAssetbundle(BuildTarget.StandaloneWindows, games);
    }

    [MenuItem("BuildPacket/Assetbundle/BuildiOS")]
    public static void BuildAssetsForiOS()
    {
        var games = GameWindow.ScanGames(BuildTarget.iOS);
        games.ForEach(item => { item.Packed = true; });
        BuildAssetbundle(BuildTarget.iOS, games);
    }

    [MenuItem("BuildPacket/Assetbundle/BuildAndroid")]
    public static void BuildAssetsForAndoird()
    {
        var games = GameWindow.ScanGames(BuildTarget.Android);
        games.ForEach(item => { item.Packed = true; });
        BuildAssetbundle(BuildTarget.Android, games);
    }

    [MenuItem("BuildPacket/Assetbundle/BuildChoice")]
    public static void BuildAssetsByChoice()
    {
        GameWindow window = (GameWindow)EditorWindow.GetWindow(typeof(GameWindow));
        window.title = "GameWindow";
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.InitData(EditorUserBuildSettings.activeBuildTarget, null, delegate(List<GameConfig> games)
        {
            if (games.Count == 0)
            {
                EditorUtility.DisplayDialog("build games", "nothing was select", "ok");
                return;
            }

            BuildAssetbundle(EditorUserBuildSettings.activeBuildTarget, games);
            EditorUtility.DisplayDialog("build games", "done", "ok");
        });
    }

    [MenuItem("BuildPacket/Assetbundle/BuildAllPlatformByChoice")]
    public static void BuildAssetsForAllPlatformByChoice()
    {
        GameWindow window = (GameWindow)EditorWindow.GetWindow(typeof(GameWindow));
        window.title = "GameWindow";
        window.autoRepaintOnSceneChange = true;
        window.Show();

        window.InitData(EditorUserBuildSettings.activeBuildTarget, null, delegate(List<GameConfig> games)
        {
            if (games.Count == 0)
            {
                EditorUtility.DisplayDialog("build games", "nothing was select", "ok");
                return;
            }

            var targets = GetAllBuildTargets();
            foreach (var buildTarget in targets)
            {
                var builds = new List<GameConfig>();
                var allGames = GameWindow.ScanGames(buildTarget);
                foreach (var gameConfig in games)
                {
                    var game = allGames.Find(item => item.ID == gameConfig.ID);
                    if (game != null)
                    {
                        game.Packed = gameConfig.Packed;
                        builds.Add(game);
                    }
                }
                BuildAssetbundle(buildTarget, builds);
            }

            EditorUtility.DisplayDialog("build games", "done", "ok");
        });
    }

    public static void BuildAssetbundle(BuildTarget target, List<GameConfig> games)
    {
        Debug.Log("build assetbundles, target:"+target);

        string path = outputPath + GameHelper.GetBuildTargetName(target) + "/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var oriList = new List<AssetBundleInfo>();
        string fileName = path + GlobalConst.Res.ResVersionFileName;
        if (File.Exists(fileName))
        {
            string text = File.ReadAllText(fileName);
            oriList = LitJson.JsonMapper.ToObject<List<AssetBundleInfo>>(text);
        }

        var newList = new List<AssetBundleInfo>();
        newList.AddRange(oriList);
        if (!BuildAssetBundle(path, target, games, ref newList))
        {
            Debug.LogError("build assetbundles failed.");
            return;
        }

        foreach (var gameConfig in games)
        {
            gameConfig.Packed = false;
        }

        string newText = LitJson.JsonMapper.ToJson(games);
        string gameConfFileName = path + "/" + GlobalConst.Res.GameConfigFileName;
        if (!File.Exists(gameConfFileName))
        {
            File.Delete(gameConfFileName);
        }
        File.WriteAllText(gameConfFileName, newText);


        List<AssetBundleInfo> diffList = CompareRes(oriList, newList);
        newText = LitJson.JsonMapper.ToJson(newList);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        File.WriteAllText(fileName, newText);

        CopyAlterRes(alterfilePath, target, diffList);
        CopyAlterRes(newFilePath, target, newList);

        EditorUtility.RevealInFinder(fileName);
    }


    public static bool BuildAssetBundle(string path, BuildTarget target, List<GameConfig> games, ref List<AssetBundleInfo> abList)
    {
        try
        {
            string gamesDir = path + "games/";
            if (!Directory.Exists(gamesDir))
            {
                Directory.CreateDirectory(gamesDir);
            }

            if (EditorUserBuildSettings.activeBuildTarget != target)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(target);
            }

            foreach (var game in games)
            {
                if (!game.Packed)
                {
                    continue;
                }

                var abPath = Array.Find(game.AssetBundles, s => s.Contains(GlobalConst.Res.SceneFileExt));
                if (string.IsNullOrEmpty(abPath))
                {
                    continue;
                }

                string name = Path.GetFileNameWithoutExtension(abPath);
                string destDirFileName = path + abPath;
                string destDirPath = Path.GetDirectoryName(destDirFileName);
                if (!Directory.Exists(destDirPath))
                {
                    Directory.CreateDirectory(destDirPath);
                }

                /*if (abList.Exists(item => string.Compare(item.Name, name, true) == 0))
                {
                    Debug.Log("the scene was already build. FileName:" + name);
                    continue;
                }*/

                Debug.Log("build scene from :" + game.SceneName + " dest:" + abPath);

                string res = BuildPipeline.BuildPlayer(
                   new string[] { game.SceneName },
                   destDirFileName,
                   target,
                   BuildOptions.BuildAdditionalStreamedScenes);

                if (res.Length > 0)
                {
                    Debug.LogError(res);
                    return false;
                }

                name = abPath.Replace(Path.GetExtension(abPath), "");

                var data_type = new AssetBundleInfo();
                //data_type.ID = "";
                data_type.Name = name;
                data_type.FileName = abPath;
                data_type.Hash = MD5Util.GetFileMD5(destDirFileName);
                data_type.Version = GameVersion.GetProductVersion(game.Version).ToString();

                var oldItem = abList.Find(item => { return string.Compare(item.Name, name, true) == 0; });
                if (oldItem != null)
                {
                    abList.Remove(oldItem);
                }

                Debug.Log("add builded scene:"+data_type+" name:"+name);
                abList.Add(data_type);
            }

            var manifest = BuildAssetBundle(path, target, games);
            if (manifest == null)
            {
                Debug.Log("null manifest");
                return true;
            }

            string[] assetBundles = manifest.GetAllAssetBundles();
            foreach (var ab in assetBundles)
            {
                string name = ab.Replace(Path.GetExtension(ab), "");

                var data_type = new AssetBundleInfo();
                //data_type.ID = "";
                data_type.Name = name;
                data_type.FileName = ab;
                data_type.Hash = MD5Util.GetFileMD5(path + ab);
                data_type.Version = "0";

                var oldItem = abList.Find(item => { return string.Compare(item.Name, name, true) == 0; });
                if (oldItem != null)
                {
                    abList.Remove(oldItem);

                    if (string.Compare(oldItem.Hash, data_type.Hash, true) != 0)
                    {
                        data_type.UpdateVersion();
                    }
                }

                Debug.Log("add builded ab:" + data_type + " name:" + name);
                abList.Add(data_type);
            }

            Debug.Log("assetbundle build finish");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }


    private static AssetBundleManifest BuildAssetBundle(string path, BuildTarget target, List<GameConfig> games)
    {
        var builds = new List<AssetBundleBuild>();
        var abs = AssetDatabase.GetAllAssetBundleNames();
        foreach (var ab in abs)
        {
            //1,筛选平台
            if (ab.EndsWith(".pc"))
            {
                if (target != BuildTarget.StandaloneWindows &&
                    target != BuildTarget.StandaloneWindows64 &&
                    target != BuildTarget.StandaloneOSXIntel &&
                    target != BuildTarget.StandaloneOSXIntel64 &&
                    target != BuildTarget.StandaloneOSXUniversal)
                {
                    continue;
                }
            }
            else if (ab.EndsWith(".mp"))
            {
                if (target != BuildTarget.iOS &&
                    target != BuildTarget.Android)
                {
                    continue;
                }
            }

            //2,筛选游戏
            if (ab.Contains("games/"))
            {
                string name = new FileInfo(ab).Directory.Name;
                if (games == null)
                {
                    continue;
                }

                var gameConfig = games.Find(config => name.Contains(config.ID.ToString()));
                if (gameConfig == null)
                {
                    continue;
                }

                if (!gameConfig.Packed)
                {
                    continue;
                }
            }

            //string dir = new FileInfo(ab).Directory.Name;
            var build = new AssetBundleBuild();
            build.assetBundleName = Path.GetDirectoryName(ab) + "/" + Path.GetFileNameWithoutExtension(ab);
            build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(ab);
            build.assetBundleVariant = Path.GetExtension(ab).Replace(".", "");
            builds.Add(build);

            Debug.Log("name:" + build.assetBundleName + " variant:" + build.assetBundleVariant + " ab:" + ab);
        }

        Debug.Log("Build scene for target:" + target + " by path:" + path);

        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(target);
        }

        return BuildPipeline.BuildAssetBundles(
               path,
               builds.ToArray(),
               BuildAssetBundleOptions.DeterministicAssetBundle/* | 
                BuildAssetBundleOptions.ForceRebuildAssetBundle*/,
               target
               );
    }

    private static List<AssetBundleInfo> CompareRes(List<AssetBundleInfo> oriRes, List<AssetBundleInfo> newRes)
    {
        var diffRes = new List<AssetBundleInfo>();
        foreach (var assetBundleInfo in newRes)
        {
            var oriItem = oriRes.Find(item => { return string.Compare(item.Name, assetBundleInfo.Name, true) == 0; });
            if (oriItem == null)
            {
                Debug.Log("add new ab:" + assetBundleInfo);
                diffRes.Add(assetBundleInfo);
            }
            else if (string.Compare(oriItem.Version, assetBundleInfo.Version, true) == 0)
            {
                Debug.Log("add alter ab:" + assetBundleInfo);
                diffRes.Add(assetBundleInfo);
            }
            else
            {
                Debug.Log("igonre ab");
            }
        }

        return diffRes;
    }

    private static void CopyAlterRes(string saveAs, BuildTarget target, List<AssetBundleInfo> res)
    {
        string srcPath = outputPath + GameHelper.GetBuildTargetName(target) + "/";
        string destPath = saveAs + GameHelper.GetBuildTargetName(target) + "/";

        /*if (Directory.Exists(destPath))
        {
            Directory.Delete(destPath, true);
        }*/
        Directory.CreateDirectory(destPath);


        foreach (var assetBundleInfo in res)
        {
            string srcFileName = srcPath + assetBundleInfo.FileName;
            string destFileName = destPath + assetBundleInfo.FileName;
            if (!File.Exists(srcFileName))
            {
                Debug.LogError("File is not exists. FileName:"+srcFileName);
                return;
            }

            string path = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.Copy(srcFileName, destFileName, true);
        }

        string configFile = srcPath + GlobalConst.Res.ResVersionFileName;
        string destConfigFile = destPath + GlobalConst.Res.ResVersionFileName;
        File.Copy(configFile, destConfigFile, true);
    }


    //判断文件是否被修改,升级版本
	public static void IsAlterFile(DataConfig new_config,DataConfig old_config)
	{
		string strKey;
		foreach(var item in new_config.DicAseetsData)
		{
			strKey = item.Key;
			if(old_config.DicAseetsData.ContainsKey(strKey))
			{
				if(!File.Exists(outputPath+strKey)||!File.Exists(oldassetsbundlePath+strKey))
					continue;
				string new_ab_path = outputPath+strKey;
				if(!isValidFileContent(new_ab_path,oldassetsbundlePath+strKey))
				{
					item.Value.Version = old_config.DicAseetsData[strKey].UpdateVersion();//升级版本号
                    new_config.DicDataTable[strKey]["version"] = item.Value.Version;
					if(!Directory.Exists(alterfilePath))
						Directory.CreateDirectory(alterfilePath);

					if (File.Exists(alterfilePath+strKey))
						File.Copy(new_ab_path, alterfilePath+strKey, true);
					else
						File.Copy(new_ab_path, alterfilePath+strKey);


					m_alter_dc.AddRow(new_config.DicAseetsData[strKey]);

                    Debug.Log("文件" + item.Value.FileName + "被修改，" + "version = " + item.Value.Version);
				}
			}
		}
		//保存修改文件的表
		m_alter_dc.Save(alterfilePath+"data.csv");
	}
	
	public static bool isValidFileContent(string filePath1, string filePath2) 
	{ 
		//创建一个哈希算法对象 
		using (HashAlgorithm hash = HashAlgorithm.Create()) 
		{ 
			using (FileStream file1 = new FileStream(filePath1, FileMode.Open),file2=new FileStream(filePath2,FileMode.Open)) 
			{ 
				byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
				byte[] hashByte2 = hash.ComputeHash(file2); 
				string str1 = System.BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
				string str2 = System.BitConverter.ToString(hashByte2); 
				return (str1==str2);//比较哈希码 
			} 
		} 
	}


    private static string[] ResFileExt = new string[]
            {
                "",
                ".pc",
                ".ab",
                ".mp",
                ".unity",
                ".scene",
                ".json"
            };

    [MenuItem("BuildPacket/Assetbundle/CopyNewAssetbundles")]
    public static void Copy()
    {
        EditorUtility.DisplayCancelableProgressBar("this may takes a while", "coping files", 0.0f);
        MycopyDirectory(outputPath, oldassetsbundlePath);
        MycopyDirectory(outputPath, Application.streamingAssetsPath);
        EditorUtility.ClearProgressBar();
    }

    #region 复制文件函数
	public static void MycopyDirectory(string sourceDirectory, string destDirectory)
	{
        Debug.Log("enter dir:"+sourceDirectory);

        //判断源目录和目标目录是否存在，如果不存在，则创建一个目录
		if (!Directory.Exists(sourceDirectory))
		{
			Directory.CreateDirectory(sourceDirectory);
		}
		if (!Directory.Exists(destDirectory))
		{
			Directory.CreateDirectory(destDirectory);
		}

        //拷贝文件
		copyFile(sourceDirectory, destDirectory);
		
		//拷贝子目录       
		//获取所有子目录名称
		string[] directionName = Directory.GetDirectories(sourceDirectory);
		
		foreach (string directionPath in directionName)
		{
			//根据每个子目录名称生成对应的目标子目录名称
			string directionPathTemp = destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);
			
			//递归下去
			MycopyDirectory(directionPath, directionPathTemp);
		}

    }

	public static void copyFile(string sourceDirectory, string destDirectory)
	{
		//获取所有文件名称
	    Debug.Log("src file:" + sourceDirectory);
        string[] fileName = Directory.GetFiles(sourceDirectory);
		foreach (string filePath in fileName)
		{
		    string ext = new FileInfo(filePath).Extension;
		    if (!ArrayUtility.Contains(ResFileExt, ext))
		    {
		        continue;
		    }

		    //根据每个文件名称生成对应的目标文件名称
			string filePathTemp = destDirectory + "\\" + filePath.Substring(sourceDirectory.Length);

            Debug.Log("found file:" + filePath + " dest:" + filePathTemp);

			//若不存在，直接复制文件；若存在，覆盖复制
			if (File.Exists(filePathTemp))
			{
				File.Copy(filePath, filePathTemp, true);
			}
			else
			{
				File.Copy(filePath, filePathTemp);
			}
		}
	}
	#endregion
}
