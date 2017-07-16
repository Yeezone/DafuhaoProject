using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.NetFox;
using System;

namespace com.QH.QPGame.GDNN
{
	//主指令
	class MainCmd : MainCommand
	{
	};

	//子指令
	class SubCmd : SubCommand
	{

		//服务器命令结构
		public const ushort SUB_S_GAME_FREE = 99;
		//游戏空闲
		public const ushort SUB_S_GAME_START = 100;
		//游戏开始
		public const ushort SUB_S_PLACE_JETTON = 101;
		//用户下注
		public const ushort SUB_S_GAME_END = 102;
		//游戏结束
		public const ushort SUB_S_APPLY_BANKER = 103;
		//申请庄家
		public const ushort SUB_S_CHANGE_BANKER = 104;
		//切换庄家
		public const ushort SUB_S_CHANGE_USER_SCORE = 105;
		//更新积分
		public const ushort SUB_S_SEND_RECORD = 106;
		//游戏记录
		public const ushort SUB_S_PLACE_JETTON_FAIL = 107;
		//下注失败
		public const ushort SUB_S_CANCEL_BANKER = 108;
		//取消申请
		public const ushort SUB_S_AMDIN_COMMAND = 109;
		//系统控制
		public const ushort SUB_S_DISPATCH_CARD = 111;
		//先发两张牌



		//客户端命令结构
		public const ushort SUB_C_PLACE_JETTON = 1;
		//用户下注
		public const ushort SUB_C_APPLY_BANKER = 2;
		//申请庄家
		public const ushort SUB_C_CANCEL_BANKER = 3;
		//取消申请
		public const ushort SUB_C_AMDIN_COMMAND = 4;
		//系统控制

	};

	public class CGameXY : MonoBehaviour
	{

	}
}
