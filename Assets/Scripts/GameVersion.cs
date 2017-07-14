using System;

namespace com.QH.QPGame.Lobby
{
    public static class GameVersion
    {
        public static int ProcessVersion(int product, int main, int sub, int build)
        {
            return (product << 24) + (main << 16) + (sub << 8) + build;
        }

        public static int ProcessVersion(string version)
        {
            string[] data = version.Split('.');

            int value = 0;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                value += int.Parse(data[i]) << ((data.Length - i - 1) * 8);
            }

            return value;
        }

        public static string Version2Str(int version)
        {
            byte[] bytes = BitConverter.GetBytes(version);
            return string.Format("{0}.{1}.{2}.{3}", bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        public static int GetMainVersion(string str)
        {
            string[] data = str.Split('.');
            int version = -1;
            int.TryParse(data[0], out version);
            return version;
        }

        public static int GetSubVersion(string str)
        {
            string[] data = str.Split('.');
            int version = -1;
            int.TryParse(data[1], out version);
            return version;
        }

        public static int GetProductVersion(string str)
        {
            string[] data = str.Split('.');
            int version = -1;
            int.TryParse(data[2], out version);
            return version;
        }

        public static int CompareMainVersion(string left, string right)
        {
            int v1 = GetMainVersion(left);
            int v2 = GetMainVersion(right);

            return v1 - v2;
        }


        /// <summary>
        /// 比较版本号,返回指定版本段比较结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="level">1代表主,2带表次,3代表</param>
        /// <returns></returns>
        public static int DiffVersion(string left, string right, int level)
        {
            string[] data = left.Split('.');
            string[] data2 = right.Split('.');

            int i = 0;
            for (; i < level; i++)
            {
                int value = int.Parse(data[i]) - int.Parse(data2[i]);
                if (value < 0)
                {
                    return i;
                }
                /*if (string.Compare(data[i], data2[i]) != 0)
                {
                    return i;
                }*/
            }

            return i == level ? level : 0;
        }

        public static int DiffVersion(int left, int right, int level)
        {
            int i = 0;
            for (; i < level; i++)
            {
                int value = (left >> ((3 - i) * 8)) - (right >> ((3 - i) * 8));
                if (value < 0)
                {
                    return i;
                }
                /*if (string.Compare(data[i], data2[i]) != 0)
                {
                    return i;
                }*/
            }

            return i == level ? level : 0;
        }

        /// <summary>
        /// 比较版本号,忽略编译号
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool CompareVersionButBuild(string left, string right)
        {
            string[] data = left.Split('.');
            string[] data2 = right.Split('.');

            return data[0] == data2[0] &&
                data[1] == data2[1] &&
                data[2] == data2[2];
        }


        public static bool IsUpdateNeeded(string current, string remote, int condition)
        {
            return DiffVersion(current, remote, condition) != condition;
        }

        public static bool IsUpdateNeeded(int current, int remote, int condition)
        {
            return DiffVersion(current, remote, condition) != condition;
        }


    }
}
