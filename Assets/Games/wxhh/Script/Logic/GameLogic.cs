using System;
using Shared;

namespace com.QH.QPGame.WXHH
{
	public class GameLogic
    {
        //公共定义
        public const ushort KIND_ID         = 1011;
        public const ushort GAME_PLAYER     = 8;
        public const string GAME_NAME       = "五星鸿辉";

		//当前游戏状态
        public const ushort GS_WK_FREE      = (ushort)GameState.GS_FREE;			
		public const ushort GS_WK_CHIP      = (ushort)GameState.GS_PLAYING;			
		public const ushort GS_WK_END     	= (ushort)GameState.GS_PLAYING+1;		 		

		// 五星区域索引
		public const byte enAreaBlack   =  0;		//黑桃
		public const byte enAreaRed 	=  1;		//红桃
		public const byte enAreaFlower  =  2;		//梅花
		public const byte enAreaSquare  =  3;		//方块
		public const byte enAreaKing    =  4;		//皇冠


        public const byte   NULL_CHAIR      = 255;
        public const ushort MASK_COLOR      = 0xF0; //花色掩码
        public const ushort MASK_VALUE      = 0x0F; //数值掩码

		public const int MAX_SCORE_HISTORY = 100;  	//每轮最大游戏局数
		public const int MAX_AREA_NUM = 5;   		//下注区域数量

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
			return (byte)((cbCardData & MASK_COLOR) >> 4);
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
