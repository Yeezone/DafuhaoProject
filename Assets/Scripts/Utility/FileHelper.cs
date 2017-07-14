using System.IO;

namespace com.QH.QPGame.Utility
{
    class FileHelper
    {
        public static bool IsFileValid(string path, string md5)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            string oriMD5 = MD5Util.GetFileMD5(path);
            return string.Compare(md5, oriMD5, true) == 0;
        }
    }
}
