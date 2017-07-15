using System;
using com.QH.QPGame.Services.NetFox;


namespace com.QH.QPGame.XZMJ
{

	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
        public const ushort SUB_C_OUT_CARD          =   1;                                  //出牌命令
        public const ushort SUB_C_LISTEN_CARD       =   2;                                  //听牌命令
        public const ushort SUB_C_OPERATE_CARD      =   3;                                  //操作扑克
        public const ushort SUB_C_TRUSTEE           =   4;                                  //用户托管
        public const ushort SUB_C_LACK_CARD         =   5;                                  //选缺
        
        public const ushort SUB_S_GAME_START        =   100;                                //游戏开始
        public const ushort SUB_S_OUT_CARD          =   101;                                //出牌命令
        public const ushort SUB_S_SEND_CARD         =   102;                                //发送扑克
        public const ushort SUB_S_LISTEN_CARD       =   103;                                //听牌命令
        public const ushort SUB_S_OPERATE_NOTIFY    =   104;                                //操作提示
        public const ushort SUB_S_OPERATE_RESULT    =   105;                                //操作命令
        public const ushort SUB_S_GAME_END          =   106;                                //游戏结束
        public const ushort SUB_S_TRUSTEE           =   107;                                //用户托管
        public const ushort SUB_S_CHI_HU            =   108;                                //胡牌指令
        public const ushort SUB_S_GANG_SCORE        =   110;                                //刮风下雨
        public const ushort SUB_S_LACK_CARD         =   111;                                //选缺

	};
	
	//
	class ExtraCmd : SubCommand
	{
		public const ushort SUB_GP_SET_PASS 		=       11;
		public const ushort SUB_GP_EXP_BUY_SCORE 	=       22;
		public const ushort SUB_GP_EXP_BUY_AWARD 	=       23;
        public const ushort SUB_GP_NOTICE           =       30;
	};

}

