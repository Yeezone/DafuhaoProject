using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services
{
    public class NetOptions
    {
        // 长度定义
        public static int SOCKET_TCP_BUFFER = 32768;   // 网络缓冲 32K
        public static int SOCKET_MAX_RECV_SIZE = 4096 * 3;
        public static int SOCKET_MAX_SEND_SIZE = 4096 * 3;

        public static bool EncryptEnable;
        public static int Timeout = 6000;

        public static string[] DomainSuffixList = 
        { 
            ".com", ".net", ".org", ".cn", ".cx", ".cc", ".co" 
        };

    }
}
