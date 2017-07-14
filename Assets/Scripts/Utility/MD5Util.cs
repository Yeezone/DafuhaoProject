using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace com.QH.QPGame.Utility
{
	
	public class MD5Util
	{
        public static string GetFileMD5(string fileName)
	    {
            string md5 = "";
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(
                    fileName,
                    FileMode.Open,
                    FileAccess.Read))
                {
                    md5 = MD5Util.GetMD5Hash(fs);
                }
            }
           
	        return md5;
	    }

	    public static string GetMD5Hash(Stream stream)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(stream);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

		public static string GetMD5Hash (string input)
		{
			MD5 md5Hash=MD5.Create();

			byte[] data=md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			StringBuilder sBuilder=new StringBuilder();

			for(int i=0;i<data.Length;i++){

				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();

		}


		public static bool VerfyMd5Hash(string input, string hash)
		{
			string hashOfInput=GetMD5Hash(input);

			StringComparer comparer= StringComparer.OrdinalIgnoreCase;

			if(0==comparer.Compare(hashOfInput,hash)){
				return true;
			}else{
				return false;
			}

		}



	}
}


