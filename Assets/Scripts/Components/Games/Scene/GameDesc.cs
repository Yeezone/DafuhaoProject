using System;

namespace com.QH.QPGame
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    ///TODO  存放在文件中
    public class GameDescAttribute : Attribute
    {
        //定义游戏id,当超过6位时,忽略后面数字,比如捕鱼为415000001-41500005,则这里填写415000则匹配全部捕鱼
        //填写41500001则只匹配这一个捕鱼
        public UInt32 GameIDPrefix { get; set; }
        //定义当前版本,程序会优先匹配与服务器更接近的版本
        public string Version { get; set; }
        //游戏场景,如果为空,会根据Level_GAMEID来匹配场景
        public string SceneName { get; set; }
    }
}
