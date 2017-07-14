using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services.Data
{
    [Serializable]
    public class AssetBundleInfo
    {
        public string Name;
        public string FileName;
        public string Hash;
        public string Version;

        //public bool Updatable = true;
        //public bool Packed;

        public override bool Equals(object obj)
        {
            AssetBundleInfo ab = obj as AssetBundleInfo;
            return ab != null &&
                 string.Compare(this.Name, ab.Name, true) == 0 /*&&
                string.Compare(this.Hash, ab.Hash, true) == 0*/ &&
                string.Compare(this.Version, ab.Version, true) == 0;
        }

        public string UpdateVersion()
        {
            if (string.IsNullOrEmpty(Version)) return Version;

            int verion = int.Parse(Version);
            Version = (verion++).ToString();
            return Version;
            /*string[] versions = Version.Split('.');
            for (int i = versions.Length-1; i >=0; i--)
            {
                int value = int.Parse(versions[i]);
                if (value < 255)
                {
                    versions[i] = (++value).ToString();
                    break;
                }
            }*/

            /*int a = 0, b = 0, c = 0;
            a = int.Parse(Version.Substring(Version.Length - 1, 1));
            a = ++a > 9 ? 0 : a;
            b = int.Parse(Version.Substring(Version.Length - 3, 1));
            b = a == 0 ? (++b > 9 ? 0 : b) : b;
            c = int.Parse(Version.Substring(Version.Length - 5, 1));
            c = (b == 0 && a == 0) ? ++c : c;

            if (c > 9)
            {
                UnityEngine.Debug.LogError("版本号已经用完！");
            }
            else
            {
                Version = string.Format("{0}.{1}.{2}", c, b, a);
            }
            return Version;*/
        }

        public override string ToString()
        {
            return this.Name + "  " + this.Hash + "  " + this.Version;
        }
    }

    public class ResVersionDesc
    {
        public List<AssetBundleInfo> AssetBundles;
    }


    [Serializable]
    public class AppVersionDesc
    {
        [Serializable]
        public class AppVersionConfig
        {
            public string Version;
            public string DownloadUrl;
        }

        public List<AppVersionConfig> VersionList;

        public AppVersionConfig FindConfig(string[] versions)
        {
            foreach (var version in versions)
            {
                var config = VersionList.Find(
                    item => string.Compare(item.Version, version, true) == 0
                    );

                if (config != null)
                {
                    return config;
                }
            }

            return null;
        }
    }
}
