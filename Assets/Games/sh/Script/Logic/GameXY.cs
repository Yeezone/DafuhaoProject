using System;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.SH
{
	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
        public const ushort SUB_S_GAME_START        =       100;                                //游戏开始
        public const ushort SUB_S_ADD_SCORE         =       101;                                //加注结果
        public const ushort SUB_S_GIVE_UP           =       102;                                //放弃跟注
        public const ushort SUB_S_SEND_CARD         =       103;                                //发牌消息
        public const ushort SUB_S_GAME_END          =       104;                                //游戏结束


        //客户端命令结构
        public const ushort SUB_C_ADD_SCORE         =       1;                                  //用户加注
        public const ushort SUB_C_GIVE_UP           =       2;                                  //放弃消息
       
		
	};
	
	//
	class ExtraCmd : SubCommand
	{
		public const ushort SUB_GP_SET_PASS 		=       11;
		public const ushort SUB_GP_EXP_BUY_SCORE 	=       22;
		public const ushort SUB_GP_EXP_BUY_AWARD 	=       23;
        public const ushort SUB_GP_NOTICE           =       30;
	};


    //
    enum GameTimer
    {
        TIMER_SEND_CARD = 1000,

    };
}

