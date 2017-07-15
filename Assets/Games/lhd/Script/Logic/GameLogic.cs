using System;
using Shared;

namespace com.QH.QPGame.LHD
{
	public class GameLogic
    {
        //
        public const ushort KIND_ID         = 1007;
        public const ushort GAME_PLAYER     = 100;
        public const string GAME_NAME       = "百人龙虎斗";

		//当前游戏状态
        public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;			
		public const ushort GS_WK_CHIP      = (ushort)GameState.GS_PLAYING;			
		public const ushort GS_WK_OPEN     	= (ushort)GameState.GS_PLAYING+1;		 	//下注阶段	
		//public const ushort GS_WK_OPEN      = (ushort)(GameState.GS_PLAYING+2);	 	//玩牌阶段	

		//游戏下注区域
		public const byte AREA_LONG = 1;
		public const byte AREA_PING = 2;
		public const byte AREA_HU = 3;

		public const ushort MAX_APPLY_BANKER  = 5;	//申请上庄人数上限

		public const byte 	CHIP_ALL 		= 13;	//筹码类型数量
        public const byte   NULL_CHAIR      = 255;
        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码

		public const int MAX_SCORE_HISTORY = 65;  	//最大游戏记录
		public const int MAX_AREA_NUM = 12;   		//下注区域数量

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
