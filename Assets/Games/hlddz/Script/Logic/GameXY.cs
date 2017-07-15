using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.DDZ
{
	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//
		public const ushort SUB_S_SEND_CARD	 	= 100;
		public const ushort SUB_S_LAND_SCORE	= 101;
		public const ushort SUB_S_GAME_START 	= 102;
		public const ushort SUB_S_OUT_CARD	 	= 103;
		public const ushort SUB_S_PASS_CARD	 	= 104;
		public const ushort SUB_S_GAME_END	 	= 105;
		public const ushort SUB_S_USER_BRIGHT	= 106;
        public const ushort SUB_S_USER_DOUBLE   = 107;

		//
		public const ushort SUB_C_LAND_SCORE	= 1;
		public const ushort SUB_C_OUT_CARD	 	= 2;
		public const ushort SUB_C_PASS_CARD	 	= 3;
		public const ushort SUB_C_TRUSTEE 		= 4;
		public const ushort SUB_C_BRIGHT		= 5;
        public const ushort SUB_C_DOUBLE        = 6;



		
	};
    class SubCmdEx:SubCommand
    {
        //
        public const ushort SUB_S_SEND_CARD     = 100;
        public const ushort SUB_S_LAND_SCORE    = 101;
        public const ushort SUB_S_GAME_START    = 102;
        public const ushort SUB_S_OUT_CARD      = 103;
        public const ushort SUB_S_PASS_CARD     = 104;
        public const ushort SUB_S_GAME_END      = 105;
        public const ushort SUB_S_CHOICE_LOOK   = 106;


        //
        public const ushort SUB_C_LAND_SCORE    = 1;
        public const ushort SUB_C_OUT_CARD      = 2;
        public const ushort SUB_C_PASS_CARD     = 3;
        public const ushort SUB_C_TRUSTEE       = 4;

    };
	
	//
	class ExtraCmd : SubCommand
	{
		public const ushort SUB_GP_SET_PASS 		= 11;
		public const ushort SUB_GP_EXP_BUY_SCORE 	= 22;
		public const ushort SUB_GP_EXP_BUY_AWARD 	= 23;
        public const ushort SUB_GP_NOTICE           = 30;
	};

}

