

namespace com.QH.QPGame.Utility
{
	
	public class StringHelper
	{
		
		public const string EnCoding_GB1830="gb18030";

		public static byte[] String2DefaultByteArray(string s)
		{
			return System.Text.Encoding.Default.GetBytes(s);
		}

		public static string ByteArray2DefaultString (byte[] buffer)
		{
			return System.Text.Encoding.Default.GetString(buffer);
		}

		public static byte[] String2UnicodeByteArray (string s)
		{
			return System.Text.Encoding.Unicode.GetBytes(s);
		}

		public static byte[] String2UTF8ByteArray (string s)
		{
			return System.Text.Encoding.UTF8.GetBytes(s);
		}

		public static string ByteArray2UTF8tring (byte [] buffer)
		{
			return System.Text.Encoding.UTF8.GetString(buffer);
		}

        public static int GetStringLength(string str)
        {
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            return data.Length;
        }

        public static string CleanNumberString(string str)
        {
            string strRes = "";
            var array = str.ToCharArray();
            foreach (char c in array)
            {
                if (c >= '0' && c <= '9')
                {
                    strRes += c.ToString();
                }
            }
            return strRes;
        }

        public static string CleanBlankString(string str)
        {
            string strRes = "";
            var array = str.ToCharArray();
            foreach (char c in array)
            {
                if (c != 0)
                {
                    strRes += c.ToString();
                }
            }
            return strRes;
        }

	}
}

