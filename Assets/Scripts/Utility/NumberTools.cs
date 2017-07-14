namespace com.QH.QPGame.Utility
{
	public static class NumberTools 
	{

		public static string FormatToIndex(this int me)
		{
			if(me<10){
				return "0"+me;
			}else{
				return me.ToString();
			}
		}

        public static uint MakeLong(ushort lowPart, ushort highPart)
        {
            return (uint)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        public static ushort HiWord(uint dword)
        {
            return (ushort)(dword >> 16);
        }

        public static ushort LoWord(uint dword)
        {
            return (ushort)dword;
        }
	}
}