using UnityEngine;
using System.Collections;
using System;
using Shared;



namespace com.QH.QPGame.ATT
{
    public class tagProbability
    {
        public int[] nWinLose = new int[GameLogic.MAX_SIZE];//输赢概率
        public int[] pb5K = new int[GameLogic.MAX_SIZE];    //(五筒)
        public int[] pbRS = new int[GameLogic.MAX_SIZE];    //（同花大顺
        public int[] pbSF = new int[GameLogic.MAX_SIZE];    //（同花小顺
        public int[] pb4K = new int[GameLogic.MAX_SIZE];    //（四筒）
        public int[] pbFHv = new int[GameLogic.MAX_SIZE];    //福爷
        public int[] pbFL = new int[GameLogic.MAX_SIZE];    //同花
        public int[] pbST = new int[GameLogic.MAX_SIZE];    //顺子
        public int[] pb3K = new int[GameLogic.MAX_SIZE];    //三筒
        public int[] pb2P = new int[GameLogic.MAX_SIZE];    //两对
        public int[] pb1P = new int[GameLogic.MAX_SIZE];    //一对

    };

    public class tagReadFile
    {
        public Int16 wID;                //房间ID
        public int nHistoryJetton;      //历史筹码
        
        public int n5K;                //(五筒)
        public int nRS;                //（同花大顺）
        public int nSF;                //（同花小顺）
        public int n4K;                //（四筒）
        public byte[] cbHistoryCard = new byte[6];   //历史牌点
        public int[,] cbPointTimes = new int[10, 3];//压分倍率
        public int[] cbCompareTimes = new int[6];//比倍倍率
    };
    public class tagHistoryData
    {
        public Int64 lGamePlayFraction;          //主游戏玩的分数   
        public Int64 lGameWinFraction;           //主游戏赢的分数   
        public Int64 lGamePlay;                  //主游戏玩的次数   
        public Int64 lGameWin;                   //主游戏赢的赢数   
        public Int64 lPlayMultiplerFaction;      //比倍玩的分数     
        public Int64 lWinMultiplerFaction;       //比倍赢的分数     
        public Int64 lWinMultipler;              //比倍赢的次数     
        public Int64 lLoseMultipler;             //比倍输的次数     
        public Int64 lHandselFaction;            //彩金分数         
        public Int64 lWinHandselAmount;          //彩金中奖次数     


        public Int64 l5KAmountGao;                //(五筒)高     
        public Int64 l5KAmountZhong;              //(五筒)中       
        public Int64 l5KAmountDi;                 //(五筒)低

        public Int64 lRSAmountGao;             //(同花大顺)高
        public Int64 lRSAmountZhong;           //(同花大顺)中   
        public Int64 lRSAmountDi;              //(同花大顺低

        public Int64 lSFAmountGao;             //(同花小顺)高  
        public Int64 lSFAmountZhong;           //(同花小顺)中   
        public Int64 lSFAmountDi;              //(同花小顺)低

        public Int64 l4KAmountGao;             //(四筒)高      
        public Int64 l4KAmountZhong;           //(四筒)中       
        public Int64 l4KAmountDi;              //(四筒)低

        public Int64 lFHAmount;                  //福爷             
        public Int64 lFLAmount;                  //同花             
        public Int64 lSTAmount;                  //顺子             
        public Int64 l3KAmount;                  //三筒             
        public Int64 l2PAmount;                  //两对             
        public Int64 l1PAmount;                  //一对             
        public Int64 lLoseAmount;                //无牌次数         
        public Int64 lINRoomFactiont;            //进房间总分数     
        public Int64 lOUTRoomFaction;            //出房间总分数    
        public Int64 lFuHao;					  //压80分以上的次数（到达3次后归零）

        //机器人换牌配置信息
        public bool m_bIsCheat;						//是否作弊（换牌）
        public Int32 m_lRobotloseMax;				//机器人输的最大分数有概率换牌
        public Int32 m_lRobotLoseMaxChange;			//机器人输的最大分数百分百换牌
        public Byte m_btRobotLoseChangePercent;		//机器人输分，换牌百分比

        public Int32 m_lRealBankerRobotLoseMax;				//有概率换牌的起始输分分数
        public Int32 m_lRealBankerRobotLoseMaxChange;		//百分百换牌的起始输分分数
        public Byte m_btRealBankerRobotLoseChangePercent;	//起始输分，换牌百分比

        public Int32 m_lRobotProbabilityTanking;		//赢的金额数，机器人有概率放水
        public Int32 m_lRobotDefineTanking;			//赢的金额数，机器人百分百放水
        public Byte m_btRobotTankingPercent;		//机器人放水的百分比

        public Byte m_btUsuallyChangePercent;		//通常情况下，换牌百分比
        public Byte m_btRobotWinScorePercent;		//扣除机器人赢的分数的百分比
        public Int32 m_lRobotWinChiefScore;			//扣除的机器人赢的分数总和
        public Int32 m_lRobotResultScore;			//所有机器人输赢的总分

    };

    public class tagUserCard
    {
        public byte[] cbCard = new byte[5];
        public bool[] bBarter = new bool[5];
    };

    public class CMD_CardData
    {
        public tagUserCard CardData;
        public int nState;

    };
    public class GameLogic
    {
        //
        public const ushort KIND_ID = 1010;
        public const ushort GAME_PLAYER = 1;
        public const string GAME_NAME = "ATT2连环炮";

        //当前游戏状态
        public const ushort GS_TK_FREE = (ushort)GameState.GS_FREE;                                   //空闲状态
        public const ushort GS_TK_BET  = (ushort)GameState.GS_PLAYING+2 ;                             //下注请求第一次开牌
        public const ushort GS_TK_UPDATE_CARD = (ushort)GameState.GS_PLAYING + 3;                     //换牌
        public const ushort GS_TK_COMPARE = (ushort)GameState.GS_PLAYING + 4;                         //比牌阶段

        //下注位置定义；地、天、和、地1->地9、天1->天9，共21个下注位置
        public const byte INDEX_PLAYER = 0;								   //闲家索引
        public const byte INDEX_BANKER = 1;								   //庄家索引
        public const byte INDEX_DRAW = 2;							     	//和索引
        public const byte INDEX_PLAYER_MIN = 3;							 	//闲最小索引
        public const byte INDEX_PLAYER_MAX = 10;							//闲最大索引
        public const byte INDEX_BANKER_MIN = 10;							//庄最小索引
        public const byte INDEX_BANKER_MAX = 10;							//庄最大索引
        public const byte INDEX_MAX = 10;							        //最大索引

        public const byte HISTORY_COUNT = 30;    							//历史记录局数

        //数值掩码
        public const ushort LOGIC_MASK_COLOR = 0xF0;							       //花色掩码
        public const ushort LOGIC_MASK_VALUE = 0x0F;						           //数值掩码

        //public const Int64 MAX_SCORE = ((Int64)Mathf.Pow(2, 32) - 1);		        	//最大乐果

        public const ushort NO_WINNER = 0;								               //没有赢家时的取值
        // public const int[] LOSS_RATES = new int[] { 100, 800, 100, 1100, 1100, 54, 150, 200, 3200, 200 };

        public const ushort MAX_SIZE = 4;

        public const byte CARD_n5K = 10;          //(五筒)
        public const byte CARD_nRS = 9;           //（同花大顺）
        public const byte CARD_nSF = 8;           //（同花小顺）
        public const byte CARD_n4K = 7;           //（四筒）
        public const byte CARD_nFH = 6;           //福爷
        public const byte CARD_nFL = 5;          //同花
        public const byte CARD_nST = 4;          //顺子
        public const byte CARD_n3K = 3;           //三筒
        public const byte CARD_n2P = 2;          //两对
        public const byte CARD_n1P = 1;          //一对
        public const byte CARD_nLose = 0;         //输

        ///////////////////////////////变量定义////////////////////////

        private byte[] huan = new byte[5];
        private int huanZHH;
        private static byte[] m_cbCardListData = new byte[54] {
	0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//方块 1 - K
	0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//黑桃 1 - K 
	0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//红桃 1 - K 
	0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,	//梅花 1 - K 
	0x4E,0x4F
};				//扑克定义
        private static byte[] m_cbSmallBig = new byte[52]{
	    0x01,0x02,0x03,0x04,0x05,0x06,
		0x11,0x12,0x13,0x14,0x15,0x16,
		0x21,0x22,0x23,0x24,0x25,0x26,
		0x31,0x32,0x33,0x34,0x35,0x36,
		
		0x08,0x09,0x0A,0x0B,0x0C,0x0D,	
		0x18,0x19,0x1A,0x1B,0x1C,0x1D,	
		0x28,0x29,0x2A,0x2B,0x2C,0x2D,	
		0x38,0x39,0x3A,0x3B,0x3C,0x3D,	
		0x07,0x17,0x27,0x37
	
};				//
        private bool ifPing;
        private Byte[] m_RandCard = new Byte[10];
        private bool[] bBarter = new bool[5];
        private Int64 m_Data;


        private int m_SUIJI;


        public Int64 m_HuanGailV;
        public byte[] bCard = new byte[5];

        //构造函数
        public void Init()
        {

            m_HuanGailV = 500;
            m_SUIJI = 0;
            huanZHH = 0;
            for (int i = 0; i < 10; i++)
            {
                m_RandCard[i] = 0;
            }
            m_Data = 1000;
            ifPing = false;
            //AllocConsole();
            //freopen("CON","wt",stdout);
        }
        //设置随机范围
        public void SetSuiJiFangWei(int sStart)
        {
            m_SUIJI = UnityEngine.Random.Range(0, 50) + sStart;
        }

        //////////////////////////////////////////////////////////////////////////
        //获取牌值
        public static byte GetCardValue(byte cbCardData)
        {
            return (byte)(cbCardData & LOGIC_MASK_VALUE);
        }
        //获取花色
        public static byte GetCardColor(byte cbCardData)
        {
            return (byte)(cbCardData & LOGIC_MASK_COLOR);
        }
        ////////////////////////////////////////////////////////////////////////
//         public void ChaosCard(byte[] cCard, bool ifHuan)//混乱牌
//         {
// 
//             ArrayCard(cCard, m_cbCardListData, 54, 0);
// 
//             if (ifHuan)
//             {
//                 /*srand(time(NULL));*/
//                 int tidie = UnityEngine.Random.Range(0, 10) + 1;
//                 for (int t = 0; t < tidie; t++)
//                 {
//                     int die = 51;
//                     int tm1 = 0;
//                     int tm2 = 0;
//                     for (int i = 0; i < 52; i++)
//                     {
//                         tm1 = UnityEngine.Random.Range(0, (die + 1));//记录随机数
//                         tm2 = cCard[die];//数组末尾-die
//                         cCard[die] = cCard[tm1];
//                         cCard[tm1] = (byte)tm2;
//                         die--;
//                     }
//                 }
// 
//             }
//             else
//             {
//                 /*srand(time(NULL));*/
//                 int tidie = UnityEngine.Random.Range(0, 10) + 1;
//                 for (int t = 0; t < tidie; t++)
//                 {
//                     int die = 53;
//                     int tm1 = 0;
//                     int tm2 = 0;
//                     for (int i = 0; i < 54; i++)
//                     {
//                         tm1 = UnityEngine.Random.Range(0, (die + 1));//记录随机数
//                         tm2 = cCard[die];//数组末尾-die
//                         cCard[die] = cCard[tm1];
//                         cCard[tm1] = (byte)tm2;
//                         die--;
//                     }
//                 }
// 
//             }
//         }
// 
//         public void ArrayCard(byte[] destArray, byte[] srcArray, int size, int srcIncept) //destArray目标数组  srcArray源数组 size大小  srcIncept源数组起始位置
//         {
//             for (int i = 0; i < size; i++)
//             {
//                 destArray[i] = srcArray[i + srcIncept];
//             }
//         }
// //         public void RandomCardJX(byte[] cCard, byte bMaxCrad)//随机牌- cCard寄存牌的数组，bMaxCrad产生的最大牌
// // {
// // 	//cCard[0]=0x02;
// // 	//cCard[1]=0x03;
// // 	//cCard[2]=0x05;
// // 	//cCard[3]=0x06;
// // 	//cCard[4]=78;
// // 
// // 	//return;
// // /*	memset(bCard,0,sizeof(bCard));*/
// //     Array.Clear(bCard,0,bCard.Length);
// // /*	memset(huan,0,sizeof(huan));*/
// //      Array.Clear(huan,0,huan.Length);
// // startRand:
// //     srand(time(NULL));
// // 	
// // 	tagUserCard UserCard;
// // 	bool iInit=true;
// // 	while(iInit)
// // 	{
// // 		for(int i=0;i<10;i++)
// // 		{
// // 			m_RandCard[i]=0;
// // 		}
// // 
// // 		SeekCard(bMaxCrad);//产生牌
// // 		for (int i=0;i<5;i++)
// // 		{
// // 			cCard[i]=m_RandCard[i];
// // 			UserCard.cbCard[i]=cCard[i];
// // 			UserCard.bBarter[i]=true;
// // 		}
// // 		if (GetCardSort(UserCard.cbCard)<=bMaxCrad&&GetCardSort(UserCard.cbCard)>=bMaxCrad-2&&rand()%100<MAX_CARD)
// // 		{
// // 			iInit=false;
// // 		}
// // 		if (XiangTong(UserCard))
// // 		{
// // 			iInit=false;
// // 		}
// // 		
// // 	}
// // 
// // 	bool IsShun=true;
// //     int  amount=0;
// // 	while(IsShun)
// // 	{
// // 		amount++;
// // 		IsShun=true;
// // 		for(int i=0;i<4;i++)
// // 		{
// // 			if (UserCard.cbCard[i]%16>UserCard.cbCard[i+1]%16&&UserCard.cbCard[4]%16!=1)
// // 			{
// // 				IsShun=false;
// // 				break;
// // 			}
// // 		}
// // 
// // 		if (IsShun)
// // 		{
// // 			DisorganizeB(UserCard.cbCard,5);
// // 		}
// // 
// // 		for(int i=0;i<4;i++)
// // 		{
// // 			if (UserCard.cbCard[i]%16==UserCard.cbCard[i+1]%16)
// // 			{
// // 				IsShun=true;
// // 				DisorganizeB(UserCard.cbCard,5);
// // 				break;
// // 			}
// // 		}
// // 		if (amount>=MAX_HUAN)
// // 		{
// // 			if (XiangTong(UserCard))
// // 			{
// // 				IsShun=false;
// // 			}
// // 		}
// // 		
// //      }
// // 
// // 
// // 	if (XiangTong(UserCard)==false)
// // 	{
// // 		static int cci=0;
// // 		cci++;
// // 		if (cci>=5)
// // 		{
// // 			UserCard.cbCard[0]=0x02;
// // 			UserCard.bBarter[0]  =true;
// // 			UserCard.cbCard[1]  =0x14;
// // 			UserCard.bBarter[1]  =true;
// // 			UserCard.cbCard[2]  =0x0A;
// // 			UserCard.bBarter[2]  =true;
// // 			UserCard.cbCard[3]  =0x06;
// // 			UserCard.bBarter[3]  =true;
// // 			UserCard.cbCard[4]  =0x2C;
// // 			UserCard.bBarter[4]  =true;
// // 			ArrayCard(cCard,UserCard.cbCard,5);
// // 			IsShun=false;
// //             cci=0;
// // 			return;
// // 
// // 		}
// // 		else
// // 		{
// // 			goto startRand;
// // 		}
// // 	}
// // 	ArrayCard(cCard,UserCard.cbCard,5);
// // 	if (Arresa(cCard)==false)
// // 	{
// // 		goto startRand;
// // 	}
// // 
// // }
//         public void SeekCard(byte bMaxCrad)//随机牌- cCard寄存牌的数组，bMaxCrad产生的最大牌
//         {
// 
// 
//             switch (bMaxCrad)
//             {
//                 case CARD_n5K://(五筒)
//                     Max5K(bMaxCrad);
//                     return;
// 
//                 case CARD_nRS://（同花大顺）
//                     MaxRS(bMaxCrad);
//                     return;
// 
//                 case CARD_nSF://（同花小顺）
//                     MaxSF(bMaxCrad);
//                     return;
// 
//                 case CARD_n4K://（四筒）
//                     Max4K(bMaxCrad);
//                     return;
// 
//                 case CARD_nFH://福爷
//                     MaxFH(bMaxCrad);
//                     return;
// 
//                 case CARD_nFL://同花
//                     MaxFL(bMaxCrad);//FL最大牌
//                     return;
// 
//                 case CARD_nST://顺子
//                     MaxST(bMaxCrad);//ST最大牌
//                     return;
// 
//                 case CARD_n3K://三筒
//                     Max3K(bMaxCrad);//FL最大牌
//                     return;
// 
//                 case CARD_n2P://两对
//                     Max2P(bMaxCrad);//2P最大牌
//                     return;
// 
//                 case CARD_n1P://一对
//                    // Max1P(bMaxCrad);//1P最大牌
//                     return;
// 
//                 case CARD_nLose://输
//                     //MaxLose(bMaxCrad);
//                     return;
//             }
// 
//         }
// 
//         public void Max5K(byte bMaxCrad)//5K最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] data5K = new int[4] { 3, 0, 2, 1 };
//             int[] data5KID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
// 
//             DisorganizeI(data5K, 4);//打乱
//             DisorganizeI(data5KID, 5);//打乱
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 16;
//             m_RandCard[data5KID[0]] = (byte)(twR + data5K[0] * 16);
//             m_RandCard[data5KID[1]] = (byte)(twR + data5K[1] * 16);
//             m_RandCard[data5KID[2]] = (byte)(twR + data5K[2] * 16);
//             m_RandCard[data5KID[3]] = (byte)(twR + data5K[3] * 16);
//             m_RandCard[data5KID[4]] = (byte)(m_cbCardListData[rM % 2 == 0 ? 52 : 53]);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID])
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         public void MaxRS(byte bMaxCrad)//RS最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] dataRS = new int[4] { 3, 0, 2, 1 };
//             int[] dataRSID = new int[5] { 3, 0, 2, 1, 4 };
//             int[] dataRSCard = new int[7] { 10, 11, 12, 13, 1, 78, 79 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
//             DisorganizeI(dataRS, 4);//打乱
//             DisorganizeI(dataRSID, 5);//打乱
//             DisorganizeI(dataRSCard, 7);//打乱
// 
//             rM = rand() % 52;
//             twR = m_cbCardListData[rM] % 16;
//             m_RandCard[dataRSID[0]] = (byte)(dataRSCard[0] + (dataRSCard[0] > 13 ? 0 : dataRS[0] * 16));
//             m_RandCard[dataRSID[1]] = (byte)(dataRSCard[1] + (dataRSCard[1] > 13 ? 0 : dataRS[0] * 16));
//             m_RandCard[dataRSID[2]] = (byte)(dataRSCard[2] + (dataRSCard[2] > 13 ? 0 : dataRS[0] * 16));
//             m_RandCard[dataRSID[3]] = (byte)(dataRSCard[3] + (dataRSCard[3] > 13 ? 0 : dataRS[0] * 16));
//             m_RandCard[dataRSID[4]] = (byte)(dataRSCard[4] + (dataRSCard[4] > 13 ? 0 : dataRS[0] * 16));
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID])
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
// 
//         }
//         public void MaxSF(byte bMaxCrad)//SF最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] dataRS = new int[4] { 3, 0, 2, 1 };
//             int[] dataRSID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
//             DisorganizeI(dataRS, 4);//打乱
//             DisorganizeI(dataRSID, 5);//打乱
// 
// 
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 9 + 1;
//             m_RandCard[dataRSID[0]] = (byte)(rM < 5 ? 78 : twR + dataRS[0] * 16);
//             m_RandCard[dataRSID[1]] = (byte)(twR + dataRS[0] * 16 + 1);
//             m_RandCard[dataRSID[2]] = (byte)(rM == 7 ? 79 : twR + dataRS[0] * 16 + 2);
//             m_RandCard[dataRSID[3]] = (byte)(twR + dataRS[0] * 16 + 3);
//             m_RandCard[dataRSID[4]] = (byte)(twR + dataRS[0] * 16 + 4);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, 78, RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         public void Max4K(byte bMaxCrad)//4K最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] data5K = new int[4] { 3, 0, 2, 1 };
//             int[] data5KID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
// 
//             DisorganizeI(data5K, 4);//打乱
//             DisorganizeI(data5KID, 5);//打乱
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 16;
//             m_RandCard[data5KID[0]] = (byte)(twR + data5K[0] * 16);
//             m_RandCard[data5KID[1]] = (byte)(twR + data5K[1] * 16);
//             m_RandCard[data5KID[2]] = (byte)(rM == 3 ? m_cbCardListData[rM % 2 == 0 ? 52 : 53] : twR + data5K[2] * 16);
//             m_RandCard[data5KID[3]] = (byte)(twR + data5K[3] * 16);
//             rM = UnityEngine.Random.Range(0, 52);
//             int tmpP = m_cbCardListData[rM] % 16;
//             while (twR == tmpP)
//             {
//                 rM = UnityEngine.Random.Range(0, 52);
//                 tmpP = m_cbCardListData[rM] % 16;
//             }
//             m_RandCard[data5KID[4]] = (byte)(tmpP + data5K[3] * 16);
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, m_RandCard[data5KID[0]], RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         void MaxFH(byte bMaxCrad)//FH最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] data5K = new int[4] { 3, 0, 2, 1 };
//             int[] data5KID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
// 
//             DisorganizeI(data5K, 4);//打乱
//             DisorganizeI(data5KID, 5);//打乱
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 16;
//             m_RandCard[data5KID[0]] = (byte)(twR + data5K[0] * 16);
//             m_RandCard[data5KID[1]] = (byte)(twR + data5K[1] * 16);
//             m_RandCard[data5KID[2]] = (byte)(rM == 3 ? m_cbCardListData[rM % 2 == 0 ? 52 : 53] : twR + data5K[2] * 16);
// 
//             twR = twR <= 7 ? 7 + UnityEngine.Random.Range(0, 5) + 1 : 7 - UnityEngine.Random.Range(0, 5) + 1;
//             m_RandCard[data5KID[3]] = (byte)(twR + data5K[2] * 16);
//             m_RandCard[data5KID[4]] = (byte)(twR + data5K[3] * 16);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, m_RandCard[data5KID[0]], m_RandCard[data5KID[3]], RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         public void MaxFL(byte bMaxCrad)//FL最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] data5K = new int[4] { 3, 0, 2, 1 };
//             int[] data5KID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
// 
//             DisorganizeI(data5K, 4);//打乱
//             DisorganizeI(data5KID, 5);//打乱
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = UnityEngine.Random.Range(0, 3) + 1;
//             m_RandCard[data5KID[0]] = (byte)(twR + data5K[0] * 16);
//             twR = UnityEngine.Random.Range(0, 3) + 1 + 3;
//             m_RandCard[data5KID[1]] = (byte)(twR + data5K[0] * 16);
//             twR = UnityEngine.Random.Range(0, 3) + 1 + 6;
//             m_RandCard[data5KID[2]] = (byte)(twR + data5K[0] * 16);
//             twR = UnityEngine.Random.Range(0, 3) + 1 + 9;
//             m_RandCard[data5KID[3]] = (byte)(twR + data5K[0] * 16);
//             twR = UnityEngine.Random.Range(0, 3) + 1 + 9;
//             m_RandCard[data5KID[4]] = (byte)(rM == 0 ? m_cbCardListData[rM % 2 == 0 ? 52 : 53] : m_RandCard[data5KID[2]] + 1);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, 78, RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         public void MaxST(byte bMaxCrad)//ST最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] dataRS = new int[4] { 3, 0, 2, 1 };
//             int[] dataRSID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
//             DisorganizeI(dataRS, 4);//打乱
//             DisorganizeI(dataRSID, 5);//打乱
// 
// 
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 9 + 1;
//             m_RandCard[dataRSID[0]] = (byte)(rM < 5 ? 78 : twR + dataRS[0] * 16);
//             m_RandCard[dataRSID[1]] = (byte)(twR + dataRS[1] * 16 + 1);
//             m_RandCard[dataRSID[2]] = (byte)(rM == 7 ? 79 : twR + dataRS[2] * 16 + 2);
//             m_RandCard[dataRSID[3]] = (byte)(twR + dataRS[3] * 16 + 3);
//             m_RandCard[dataRSID[4]] = (byte)(twR + dataRS[UnityEngine.Random.Range(0, 4)] * 16 + 4);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, 78, RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
//         }
//         public void Max3K(byte bMaxCrad)//3K最大牌
// {
// 	int rM=0;
// 	int twR=0;
// 	int []data5K=new int[4]{3,0,2,1};
// 	int []data5KID=new int[5]{3,0,2,1,4};
// 	int die=5;
// 	int dID=0;
// 	byte [] RandCrad = new byte[54];
// 	
// 	DisorganizeI(data5K,4);//打乱
// 	DisorganizeI(data5KID,5);//打乱
// 	rM=UnityEngine.Random.Range(0,52);
// 	twR=m_cbCardListData[rM]%16;
// 	m_RandCard[data5KID[0]]=(byte)(twR+data5K[0]*16);
// 	m_RandCard[data5KID[1]]=(byte)(twR+data5K[1]*16);
// 	m_RandCard[data5KID[2]]=(byte)(rM==3?m_cbCardListData[rM%2==0?52:53]:twR+data5K[2]*16);
// 
// 	rM=UnityEngine.Random.Range(0,52);
// 	int tmpP=m_cbCardListData[rM]%16;
// 	while (twR==tmpP)
// 	{
// 		rM=UnityEngine.Random.Range(0,52);
// 		tmpP=m_cbCardListData[rM]%16;
// 	}
// 	m_RandCard[data5KID[3]]=(byte)(tmpP+data5K[3]*16);
// 	rM=UnityEngine.Random.Range(0,52);
// 	int tmpG=m_cbCardListData[rM]%16;
// 	while (tmpG==tmpP||tmpG==twR)
// 	{
// 		rM=UnityEngine.Random.Range(0,52);
// 		tmpG=m_cbCardListData[rM]%16;
// 	}
// 	m_RandCard[data5KID[4]]=(byte)(tmpG+data5K[0]*16);
// 
// 	ChaosCard(RandCrad,false); //混乱牌
// 	while(die<10)
// 	{
// 		if (m_RandCard[0]!=RandCrad[dID]&&
// 			m_RandCard[1]!=RandCrad[dID]&&
// 			m_RandCard[3]!=RandCrad[dID]&&
// 			m_RandCard[3]!=RandCrad[dID]&&
// 			m_RandCard[4]!=RandCrad[dID]&&
// 			m_RandCard[5]!=RandCrad[dID]&&
// 			m_RandCard[6]!=RandCrad[dID]&&
// 			m_RandCard[7]!=RandCrad[dID]&&
// 			m_RandCard[8]!=RandCrad[dID]&&
// 			m_RandCard[9]!=RandCrad[dID]&&
// 			Filtrate(bMaxCrad,m_RandCard[data5KID[0]],m_RandCard[data5KID[3]],m_RandCard[data5KID[4]],RandCrad[dID]) 
// 			)
// 		{
// 			m_RandCard[die]=RandCrad[dID];
// 			die++;
// 		}
// 		dID++;
// 	}
// 
// 	for (int i=0;i<5;i++)
// 	{
// 		bCard[i]=m_RandCard[i];
// 	}
// 	DisorganizeB(m_RandCard,10);//打乱
// }
//         public void Max2P(byte bMaxCrad)//2P最大牌
//         {
//             int rM = 0;
//             int twR = 0;
//             int[] data5K = new int[4] { 3, 0, 2, 1 };
//             int[] data5KID = new int[5] { 3, 0, 2, 1, 4 };
//             int die = 5;
//             int dID = 0;
//             byte[] RandCrad = new byte[54];
// 
// 
//             DisorganizeI(data5K, 4);//打乱
//             DisorganizeI(data5KID, 5);//打乱
//             rM = UnityEngine.Random.Range(0, 52);
//             twR = m_cbCardListData[rM] % 16;
//             m_RandCard[data5KID[0]] = (byte)(twR + data5K[0] * 16);
//             m_RandCard[data5KID[1]] = (byte)(twR + data5K[1] * 16);
//             rM = UnityEngine.Random.Range(0, 52);
//             int tmpP = m_cbCardListData[rM] % 16;
//             while (twR == tmpP)
//             {
//                 rM = UnityEngine.Random.Range(0, 52);
//                 tmpP = m_cbCardListData[rM] % 16;
//             }
//             m_RandCard[data5KID[2]] = (byte)(tmpP + data5K[2] * 16);
//             m_RandCard[data5KID[3]] = (byte)(tmpP + data5K[3] * 16);
//             rM = UnityEngine.Random.Range(0, 52);
//             int tmpG = m_cbCardListData[rM] % 16;
//             while (tmpG == tmpP || tmpG == twR)
//             {
//                 rM = UnityEngine.Random.Range(0, 52);
//                 tmpG = m_cbCardListData[rM] % 16;
//             }
//             m_RandCard[data5KID[4]] = (byte)(tmpG + data5K[0] * 16);
// 
//             ChaosCard(RandCrad, false); //混乱牌
//             while (die < 10)
//             {
//                 if (m_RandCard[0] != RandCrad[dID] &&
//                     m_RandCard[1] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[3] != RandCrad[dID] &&
//                     m_RandCard[4] != RandCrad[dID] &&
//                     m_RandCard[5] != RandCrad[dID] &&
//                     m_RandCard[6] != RandCrad[dID] &&
//                     m_RandCard[7] != RandCrad[dID] &&
//                     m_RandCard[8] != RandCrad[dID] &&
//                     m_RandCard[9] != RandCrad[dID] &&
//                     Filtrate(bMaxCrad, m_RandCard[data5KID[0]], m_RandCard[data5KID[3]], RandCrad[dID])
//                     )
//                 {
//                     m_RandCard[die] = RandCrad[dID];
//                     die++;
//                 }
//                 dID++;
//             }
// 
//             for (int i = 0; i < 5; i++)
//             {
//                 bCard[i] = m_RandCard[i];
//             }
//             DisorganizeB(m_RandCard, 10);//打乱
// 
//         }
//         // void Max1P(byte bMaxCrad)//1P最大牌
//         // {
//         // 
//         // 	byte []data1P = new byte[5];
//         // 
//         // 
//         // 	byte []RandCrad = new byte[54];
//         // 	if (UnityEngine.Random.Range(0,100)==1)
//         // 	{
//         // 		ChaosCard(RandCrad,false);
//         // 	}
//         // 	else
//         // 	{
//         // 		ChaosCard(RandCrad,true); //混乱牌
//         // 	}
//         // 
//         // 	bool ifStop=false;
//         // 	int die=0;
//         // 	while(!ifStop)
//         // 	{
//         // 		ArrayCard(data1P,RandCrad,5,die*5-1<=0?0:die*5-1);
//         // 		byte Type=GetCardSort(data1P);
//         // 		if(Type==1)
//         // 		{
//         // 			die = 0;
//         // 			ifStop=true;
//         // 			ArrayCard(m_RandCard,data1P,5);
//         // 		}
//         // 		if (die<10)
//         // 		{
//         // 			die++;
//         // 		}
//         // 		else
//         // 		{
//         // 			ChaosCard(RandCrad,true);
//         // 			die=0;
//         // 		}
//         // 	}
//         // 
//         // 	
//         // 
//         //     int dID=0;
//         // 	int dieCopy=5;
//         // 
//         // 	while(dieCopy<10)
//         // 	{
//         // 		if (m_RandCard[0]!=RandCrad[dID]&&
//         // 			m_RandCard[1]!=RandCrad[dID]&&
//         // 			m_RandCard[3]!=RandCrad[dID]&&
//         // 			m_RandCard[3]!=RandCrad[dID]&&
//         // 			m_RandCard[4]!=RandCrad[dID]&&
//         // 			m_RandCard[5]!=RandCrad[dID]&&
//         // 			m_RandCard[6]!=RandCrad[dID]&&
//         // 			m_RandCard[7]!=RandCrad[dID]&&
//         // 			m_RandCard[8]!=RandCrad[dID]&&
//         // 			m_RandCard[9]!=RandCrad[dID]/*&&*/
//         // 			//Filtrate(1,vi,RandCrad[dID]) 
//         // 			)
//         // 		{
//         // 			m_RandCard[dieCopy]=RandCrad[dID];
//         // 			dieCopy++;
//         // 		}
//         // 		dID++;
//         // 	}
//         // 
//         // 	for (int i=0;i<5;i++)
//         // 	{
//         // 		bCard[i]=m_RandCard[i];
//         // 	}
//         // 	DisorganizeB(m_RandCard,10);//打乱
//         // }
//         public void DisorganizeI(int[] data, int size)
//         {
// 
//             //srand(time(NULL));
//             int die = size;
// 
// 
//             for (int i = die - 1; i >= 0; i--)
//             {
//                 int rm = UnityEngine.Random.Range(0, (die));
//                 int tmp1 = data[rm];
//                 data[rm] = data[die - 1];
//                 data[die - 1] = tmp1;
//                 die--;
//             }
// 
// 
// 
//         }
//         public void DisorganizeB(byte[] data, int size)
//         {
// 
// 
//             int die = size;
// 
//             for (int i = die - 1; i >= 0; i--)
//             {
//                 int rm = UnityEngine.Random.Range(0, (die));
//                 byte tmp1 = data[rm];
//                 data[rm] = data[die - 1];
//                 data[die - 1] = tmp1;
//                 die--;
//             }
//         }
//         public bool Filtrate(byte bMaxCrad, byte Fcard, byte StartCard)
//         {
//             switch (bMaxCrad)
//             {
//                 case CARD_n5K://(五筒)
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
//                 case CARD_nRS://（同花大顺）
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
// 
//                 case CARD_nSF://（同花小顺）
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_n4K://（四筒）
//                     return Fcard % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 ? true : false;
// 
//                 case CARD_nFH://福爷
//                     return Fcard % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nFL://同花
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_nST://顺子
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_n3K://三筒
//                     return Fcard % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 ? true : false;
// 
//                 case CARD_n2P://两对
//                     return Fcard % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 ? true : false;
// 
//                 case CARD_n1P://一对
//                     return Fcard % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 ? true : false;
// 
//                 case CARD_nLose://输
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//             }
//             return true;
//         }
//         public bool Filtrate(byte bMaxCrad, byte Fcard1, byte Fcard2, byte StartCard)
//         {
//             switch (bMaxCrad)
//             {
//                 case CARD_n5K://(五筒)
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
//                 case CARD_nRS://（同花大顺）
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
// 
//                 case CARD_nSF://（同花小顺）
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
// 
//                 case CARD_n4K://（四筒）
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nFH://福爷
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nFL://同花
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_nST://顺子
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_n3K://三筒
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_n2P://两对
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_n1P://一对
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nLose://输
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//             }
//             return true;
//         }
//         public bool Filtrate(byte bMaxCrad, byte Fcard1, byte Fcard2, byte Fcard3, byte StartCard)
//         {
//             switch (bMaxCrad)
//             {
//                 case CARD_n5K://(五筒)
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
//                 case CARD_nRS://（同花大顺）
//                     return true/*Fcard%16!=StartCard%16?true:false*/;
// 
// 
//                 case CARD_nSF://（同花小顺）
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
// 
//                 case CARD_n4K://（四筒）
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nFH://福爷
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nFL://同花
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_nST://顺子
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//                 case CARD_n3K://三筒
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 && Fcard3 % 16 != StartCard ? true : false;
// 
//                 case CARD_n2P://两对
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_n1P://一对
//                     return Fcard1 % 16 != StartCard % 16 && StartCard != 78 && StartCard != 79 && Fcard2 % 16 != StartCard % 16 ? true : false;
// 
//                 case CARD_nLose://输
//                     return StartCard == 78 || StartCard == 79 ? false : true;
// 
//             }
//             return true;
//         }
    };

}
