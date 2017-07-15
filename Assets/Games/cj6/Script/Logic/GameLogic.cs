using System;
using Shared;

namespace com.QH.QPGame.BJL
{
	public class GameLogic
    {
        //
        public const ushort KIND_ID         = 1009;
        public const ushort GAME_PLAYER     = 100;
        public const string GAME_NAME       = "超级六";

		//当前游戏状态
		public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;				//等待开始
		public const ushort GS_WK_PLAY      = (ushort)GameState.GS_PLAYING;				//游戏进行
//		public const ushort GS_WK_BET     	= (ushort)GameState.GS_PLAYING;		 		//下注状态	
		public const ushort GS_WK_END       = (ushort)(GameState.GS_PLAYING+1);	 		//玩牌阶段	

		//游戏下注区域
		public const byte AREA_X = 0;		//地闲
		public const byte AREA_P = 1;		//平
		public const byte AREA_Z = 2;		//天庄
		public const byte AREA_CJ = 5;		//超级六
		public const byte AREA_DD = 6;		//地对
		public const byte AREA_TD = 7;		//天对

		public const ushort MAX_APPLY_BANKER  = 5;	//申请上庄人数上限

        public const byte   NULL_CHAIR      = 255;
        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码

		public const byte 	CHIP_ALL 		= 13;	//筹码类型数量

		public const int MAX_SCORE_HISTORY = 65;  	//最大游戏记录
		public const int MAX_AREA_NUM = 8;   		//下注区域数量

		//是否有效牌
		public static bool IsValidCard(byte cbCardData)
		{
			byte cbCardColor = GetCardColor(cbCardData);
			byte cbCardValue = GetCardValue(cbCardData);
			
			if ((cbCardData == 0x4E) || (cbCardData == 0x4F)) return true;
			if ((cbCardColor <= 0x30) && (cbCardValue >= 0x01) && (cbCardValue <= 0x0D)) return true;
			
			return false;
		}

		//获取牌值
		public static byte GetCardValue(byte cbCardData)
		{
			return (byte)(cbCardData & MASK_VALUE);
		}

		//获取花色
		public static byte GetCardColor(byte cbCardData)
		{
			return (byte)(cbCardData & MASK_COLOR);
		}

		//获取牌点
		public static byte GetCardPip(byte cbCardData)
		{
			return (byte)(cbCardData&0x0F);
		}

		//获取牌点
		public static byte GetCardListPip(byte[] cbCardData, byte cbCardCount)
		{
			//变量定义
			byte cbPipCount=0;
			
			//获取牌点
			for (byte i=0;i<cbCardCount;i++)
			{
				cbPipCount=(byte)((GetCardPip(cbCardData[i])+cbPipCount)%10);
			}
			
			return cbPipCount;
		}

    }
}
