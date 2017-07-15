using UnityEngine;
using System.Collections;
using System;
using Shared;

namespace com.QH.QPGame.XZMJ
{


 //类型子项
 public class tagKindItem
 {
     public byte cbWeaveKind;                        //组合类型
     public byte cbCenterCard;                       //中心扑克
     public byte[] cbCardIndex = new byte[4];        //扑克索引
 };
 //组合子项
 public class tagWeaveItem
 {
     public byte cbWeaveKind;                        //组合类型
     public byte cbCenterCard;                       //中心扑克
     public byte cbPublicCard;                       //公开标志
     public short wProvideUser;                      //供应用户
 };
 //胡牌结果
 public class tagChiHuResult
 {
     public long dwChiHuKind;                        //吃胡类型
     public short dwChiHuRight;                      //胡牌权位
     public short dwWinTimes;                        //番数数目
 };

 //杠牌结果
 public class tagGangCardResult
 {
     public byte cbCardCount;                        //扑克数目
     public byte[] cbCardData = new byte[4];         //扑克数据
     public byte cbGangType;                         //杠牌类型
 };

 //分析子项
 public class tagAnalyseItem
 {
     public byte cbCardEye;                          //牌眼扑克
     public byte[] cbWeaveKind =  new byte[4];       //组合类型
     public byte[] cbCenterCard = new byte[4];       //中心扑克
 };
 
 public class GameLogic
 {
     //
     public const ushort KIND_ID        =   301;
     public const ushort GAME_PLAYER    =   4;
     public const string GAME_NAME      =   "血战麻将";
     
     public const ushort GS_WK_FREE     =   (ushort)GameState.GS_FREE;
     public const ushort GS_WK_PLAYING  =   (ushort)GameState.GS_PLAYING + 1;
     
     //游戏状态
     public const ushort GS_MJ_FREE     =   (ushort)GameState.GS_FREE ;     //空闲状态
     public const ushort GS_MJ_PLAY     =   (ushort)GameState.GS_PLAYING + 1;//游戏状态
     
     //常量定义
     public const ushort MAX_WEAVE      =   4;    //最大组合
     public const ushort MAX_INDEX      =   34;   //最大索引
     public const ushort MAX_COUNT      =   14;   //最大数目
     public const ushort MAX_REPERTORY  =   108;  //最大库存
     
     public const byte   NULL_CHAIR     =   255;
     public const ushort HEAP_FULL_COUNT=   34;   //堆立全牌
     
     public const int CardCount         =   14;
     public const ushort MASK_COLOR     =   0xF0;  //花色掩码
     public const ushort MASK_VALUE     =   0x0F;  //数值掩码
     //动作标志
     public const byte WIK_NULL         =   0x00;  //没有类型
     public const byte WIK_LEFT         =   0x01;  //左吃类型
     public const byte WIK_CENTER       =   0x02;  //中吃类型
     public const byte WIK_RIGHT        =   0x04;  //右吃类型
     public const byte WIK_PENG         =   0x08;  //碰牌类型
     public const byte WIK_GANG         =   0x10;  //杠牌类型
     public const byte WIK_LISTEN       =   0x20;  //听牌类型
     public const byte WIK_CHI_HU       =   0x40;  //吃胡类型
     
     //胡牌定义

     //牌型掩码
     const long CHK_MASK_SMALL          = 0x0000FFFF;  //小胡掩码
     const long CHK_MASK_GREAT          = 0xFFFF0000;  //大胡掩码
     //小胡牌型

     const short CHK_JI_HU              = 0x00000001;  //鸡胡类型
     const short CHK_PING_HU            = 0x00000002;  //平胡类型
     //大胡牌型
     const long CHK_PENG_PENG           = 0x00010000;  //碰碰胡牌
     const long CHK_QI_XIAO_DUI         = 0x00020000;  //七小对牌
     const long CHK_SHI_SAN_YAO         = 0x00040000;  //十三幺牌



     //牌权掩码
     //const long  CHR_MASK_SMALL         =  0x000000FF;  //小胡掩码
     //const long CHR_MASK_GREAT          =  0xFFFFFF00;  //大胡掩码

     //大胡权位
     //const short CHR_DI                 = 0x00000100;   //地胡权位
     //const short CHR_TIAN               = 0x00000200;   //天胡权位
     //const short CHR_QING_YI_SE         = 0x00000400;   //清一色牌
     //const short CHR_QIANG_GANG         = 0x00000800;   //抢杆权位
     //const short CHK_QUAN_QIU_REN       = 0x00001000;   //全求权位

    public const short CHK_NULL               = 0x00000000;  //非胡类型
    public const int   CHR_QIANG_GANG        =  0x00000001; //抢杠
    public const int   CHR_GANG_SHANG_PAO    =  0x00000002; //杠上炮
    public const int   CHR_GANG_KAI          =  0x00000004; //杠上花
    public const int   CHR_TIAN_HU           =  0x00000008; //天胡
    public const int   CHR_DI_HU             =  0x00000010; //地胡
    public const int   CHR_DA_DUI_ZI         =  0x00000020; //对对胡
    public const int   CHR_QING_YI_SE        =  0x00000040; //清一色
    public const int   CHR_QI_XIAO_DUI       =  0x00000080; //暗七对
    public const int   CHR_DAI_YAO           =  0x00000100; //带幺九
    public const int   CHR_JIANG_DUI         =  0x00000200; //将对
    public const int   CHR_SHU_FAN           =  0x00000400; //素番
    public const int   CHR_QING_DUI          =  0x00000800; //清对
    public const int   CHR_LONG_QI_DUI       =  0x00001000; //龙七对
    public const int   CHR_QING_QI_DUI       =  0x00002000; //清七对
    public const int   CHR_QING_YAO_JIU      =  0x00004000; //清幺九
    public const int   CHR_QING_LONG_QI_DUI  =  0x00008000; //清龙七对




    public static int GetCardColor(byte bCard)
    {
        return (bCard&MASK_COLOR)>>4;
    }
    public static int GetCardValue(byte bCard)
    {
        return (bCard&MASK_VALUE);
    }

     //删除扑克
     public static bool RemoveCard(ref byte[] cbCardIndex, byte cbRemoveCard)
     {
         //删除扑克
         byte cbRemoveIndex = SwitchToCardIndex(cbRemoveCard);
         if (cbCardIndex[cbRemoveIndex] > 0)
         {
             cbCardIndex[cbRemoveIndex]--;
             return true;
         }

         return false;
     }
     
     //删除扑克
     public static bool RemoveCard(ref byte[] cbCardIndex, byte[] cbRemoveCard, byte cbRemoveCount)
     {
         //删除扑克
         for (byte i = 0;i < cbRemoveCount;i++)
         {
             //删除扑克
             byte cbRemoveIndex = SwitchToCardIndex(cbRemoveCard[i]);
             if (cbCardIndex[cbRemoveIndex]==0)
             {
                 //还原删除
                 for (byte j = 0; j < i; j++) 
                 {
                     cbCardIndex[SwitchToCardIndex(cbRemoveCard[j])]++;
                 }
     
                 return false;
             }
             else 
             {
                 //删除扑克
                 --cbCardIndex[cbRemoveIndex];
             }
         }
     
         return true;
     }
     
             //删除扑克
     public static bool RemoveCard1(ref byte[] cbCardData, byte cbCardCount, byte[] cbRemoveCard, byte cbRemoveCount)
     {
         //定义变量
         byte cbDeleteCount = 0;
         byte[] cbTempCardData = new byte[CardCount];
         if (cbCardCount > CardCount){
             return false;
         }
         
         Buffer.BlockCopy(cbCardData,0,cbTempCardData,0,cbCardCount);
         //置零扑克
         for (byte i = 0; i < cbRemoveCount;i++)
         {
             for (byte j = 0; j< cbCardCount; j++)
             {
                 if (cbRemoveCard[i]==cbTempCardData[j])
                 {
                     cbDeleteCount++;
                     cbTempCardData[j] = 0;
                     break;
                 }
             }
         }
     
         //成功判断
         if (cbDeleteCount != cbRemoveCount) 
         {
             return false;
         }
     
         //清理扑克
         byte cbCardPos = 0;
         for (byte i = 0; i < cbCardCount; i++)
         {
             if (cbTempCardData[i] != 0) 
                 cbCardData[cbCardPos++] = cbTempCardData[i];
         }
     
         return true;
     }
     
             //有效判断
     bool IsValidCard(byte cbCardData)
     {
         int cbValue = (cbCardData&MASK_VALUE);
         int cbColor = (cbCardData&MASK_COLOR)>>4;
         return (((cbValue >= 1) && (cbValue <= 9) && (cbColor <= 2)) || ((cbValue >= 1) && (cbValue <= 7) && (cbColor == 3)));
     }
     
             //扑克数目
     byte GetCardCount(byte[] cbCardIndex)
     {
         //数目统计
         byte cbCardCount=0;
         for (byte i = 0; i < MAX_INDEX; i++) 
             cbCardCount+= cbCardIndex[i];
     
         return cbCardCount;
     }
     
     //获取组合
     public static byte GetWeaveCard(byte cbWeaveKind, byte cbCenterCard, ref byte[,] cbCardBuffer, byte wOperateViewID,ref byte[] bCardIlieCount,ref byte[,] bCardHiddenIndex, byte cbPublicCard,bool bSelf)
     {
         //组合扑克
         switch (cbWeaveKind)
         {
         case WIK_LEFT:      //上牌操作
             {
                 //设置变量
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard + 1);
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard + 2);
                 return 3;
             }
         case WIK_RIGHT:     //上牌操作
             {
                 //设置变量
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard - 1);
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard - 2);
     
                 return 3;
             }
         case WIK_CENTER:    //上牌操作
             {
                 //设置变量
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard - 1);
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = (byte)(cbCenterCard + 1);
     
                 return 3;
             }
         case WIK_PENG:      //碰牌操作
             {
                 //设置变量
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                 bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                 cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
     
                 return 3;
             }
         case WIK_GANG:      //杠牌操作
             {
                 //设置变量
                 byte[,] cbCardlieIndex = new byte[4,MAX_INDEX];
                 byte[,] cbCardHiddenIndex = new byte[GAME_PLAYER,MAX_INDEX];
                 int count=0 , count1 = 0;
                 for(int i = 0; i< MAX_INDEX; i++)
                 {
                     cbCardlieIndex[wOperateViewID,i]   = cbCardBuffer[wOperateViewID,i];
                     cbCardHiddenIndex[wOperateViewID,i] =   bCardHiddenIndex[wOperateViewID,i];
                 }
             
                 for(int i = 0; i< MAX_INDEX; i++)
                 {
                     if(cbCardBuffer[wOperateViewID,i] == cbCenterCard)
                     {
                         count++;
                         count1 = i;
                     }
                 }
                 if(count == 3)
                 {
                     for(int i = count1; i< MAX_INDEX -1; i++)
                     {
                         cbCardBuffer[wOperateViewID,i + 1] = cbCardlieIndex[wOperateViewID,i];
                         bCardHiddenIndex[wOperateViewID,i + 1] = cbCardHiddenIndex[wOperateViewID,i];
                     }
                     bCardIlieCount[wOperateViewID]++;
         
                 }
                 else
                 {
                     if(bSelf)
                     {
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = 1;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                     }
                     else
                     {
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                         bCardHiddenIndex[wOperateViewID,bCardIlieCount[wOperateViewID]] = cbPublicCard;
                         cbCardBuffer[wOperateViewID,bCardIlieCount[wOperateViewID]++] = cbCenterCard;
                     }
                 }
                 return 4;
             }
         
         }
     
         return 0;
     }
     
     public static byte GetWeaveCard(byte cbWeaveKind, byte cbCenterCard,ref byte[] cbCardBuffer)
     {
         //组合扑克
         switch (cbWeaveKind)
         {
         case WIK_LEFT:      //上牌操作
             {
                 //设置变量
                 cbCardBuffer[0] = cbCenterCard;
                 cbCardBuffer[1] = (byte)(cbCenterCard+1);
                 cbCardBuffer[2] = (byte)(cbCenterCard+2);
                 return 3;
             }
         case WIK_RIGHT:     //上牌操作
             {
                 //设置变量
                 cbCardBuffer[0] = cbCenterCard;
                 cbCardBuffer[1] = (byte)(cbCenterCard-1);
                 cbCardBuffer[2] = (byte)(cbCenterCard-2);
     
                 return 3;
             }
         case WIK_CENTER:    //上牌操作
             {
                 //设置变量
                 cbCardBuffer[0] = cbCenterCard;
                 cbCardBuffer[1] = (byte)(cbCenterCard-1);
                 cbCardBuffer[2] = (byte)(cbCenterCard+1);
     
                 return 3;
             }
         case WIK_PENG:      //碰牌操作
             {
                 //设置变量
                 cbCardBuffer[0] = cbCenterCard;
                 cbCardBuffer[1] = (byte)(cbCenterCard);
                 cbCardBuffer[2] = (byte)(cbCenterCard);
     
                 return 3;
             }
         case WIK_GANG:      //杠牌操作
             {
                 //设置变量
                 cbCardBuffer[0] = cbCenterCard;
                 cbCardBuffer[1] = cbCenterCard;
                 cbCardBuffer[2] = cbCenterCard;
                 cbCardBuffer[3] = cbCenterCard;
     
                 return 4;
             }
         
         }
     
         return 0;
     }
     
     public static byte GetWeaveCard(byte cbCenterCard,ref byte[] cbCardBuffer,byte[] cbCardData, ref byte[] cbWeaveType)
     {
         
         int count =0,count1 = 0;
         byte index=0;
         for(int i=0; i < MAX_COUNT; i++){
             if(cbCardData[i] == (cbCenterCard + 1)){
                     count++;
             }else
             if(cbCardData[i] == (cbCenterCard + 2)){
                     count++;
             }
         }
         if(count >=2){
             cbCardBuffer[count1++] = cbCenterCard;
             cbCardBuffer[count1++] = (byte)(cbCenterCard+1);
             cbCardBuffer[count1++] = (byte)(cbCenterCard+2);
             cbWeaveType[index] = WIK_LEFT;
             index++;
         }
         count = 0;
         
         for(int i=0; i < MAX_COUNT; i++){
             if(cbCardData[i] == (cbCenterCard - 1)){
                     count++;
             }else
             if(cbCardData[i] == (cbCenterCard - 2)){
                     count++;
             }
         }
         if(count >=2){
             cbCardBuffer[count1++] = (byte)(cbCenterCard - 1);
             cbCardBuffer[count1++] = (byte)(cbCenterCard - 2);
             cbCardBuffer[count1++] = (byte)(cbCenterCard);
             cbWeaveType[index] = WIK_RIGHT;
             index++;
         }
         count = 0;
         
         for(int i=0; i < MAX_COUNT; i++){
             if(cbCardData[i] == (cbCenterCard - 1)){
                     count++;
             }else
             if(cbCardData[i] == (cbCenterCard + 1)){
                     count++;
             }
         }
         if(count >=2){
             cbCardBuffer[count1++] = (byte)(cbCenterCard - 1);
             cbCardBuffer[count1++] = (byte)(cbCenterCard);
             cbCardBuffer[count1++] = (byte)(cbCenterCard +1);
             cbWeaveType[index] = WIK_CENTER;
             index++;
         }
         count = 0;
         return index;
     }
     
     //动作等级
     byte GetUserActionRank(byte cbUserAction)
     {
         //胡牌等级
         if ((cbUserAction & WIK_CHI_HU) == 0 ? false : true) {
             return 4;
         }
     
         //杠牌等级
         if ((cbUserAction & WIK_GANG) == 0 ? false : true) { 
             return 3;
         }
     
         //碰牌等级
         if ((cbUserAction & WIK_PENG) == 0 ? false : true) { 
             return 2;
         }
     
         //上牌等级
         if ((cbUserAction & (WIK_RIGHT | WIK_CENTER | WIK_LEFT)) == 0 ? false : true) { 
             return 1; 
         }
     
         return 0;
     }
     
         
     //吃牌判断
     byte EstimateEatCard(byte[] cbCardIndex, byte cbCurrentCard)
     {
         //参数效验
         //ASSERT(IsValidCard(cbCurrentCard));
     
         //过滤判断
         //番子无连
         if (cbCurrentCard >= 0x31) 
             return WIK_NULL;
     
         //变量定义
         byte[] cbExcursion = {0,1,2};
         byte[] cbItemKind = {WIK_LEFT,WIK_CENTER,WIK_RIGHT};
     
         //吃牌判断
         byte cbEatKind = 0,cbFirstIndex = 0;
         byte cbCurrentIndex = SwitchToCardIndex(cbCurrentCard);
         for (byte i = 0; i< 3;i++)
         {
             byte cbValueIndex = (byte)(cbCurrentIndex % 9);
             if ((cbValueIndex >= cbExcursion[i]) && ((cbValueIndex - cbExcursion[i]) <= 6))
             {
                 //吃牌判断
                 cbFirstIndex = (byte)(cbCurrentIndex - cbExcursion[i]);
                 if ((cbCurrentIndex != cbFirstIndex) && (cbCardIndex[cbFirstIndex]==0))
                     continue;
                 if ((cbCurrentIndex != (cbFirstIndex + 1)) && (cbCardIndex[cbFirstIndex + 1]==0))
                     continue;
                 if ((cbCurrentIndex != (cbFirstIndex + 2)) && (cbCardIndex[cbFirstIndex + 2]==0))
                     continue;
     
                 //设置类型
                 cbEatKind |= cbItemKind[i];
             }
         }
     
         return cbEatKind;
     }
     
     //碰牌判断
     byte EstimatePengCard(byte[] cbCardIndex, byte cbCurrentCard)
     {
         //碰牌判断
         return (cbCardIndex[SwitchToCardIndex(cbCurrentCard)] >= 2) ? WIK_PENG:WIK_NULL;
     }
     
     //杠牌判断
     byte EstimateGangCard(byte[] cbCardIndex, byte cbCurrentCard)
     {
         //杠牌判断
         return (cbCardIndex[SwitchToCardIndex(cbCurrentCard)] == 3) ? WIK_GANG : WIK_NULL;
     }

     
     //杠牌分析
     public static byte AnalyseGangCard(byte[]cbCardIndex, tagWeaveItem[,] WeaveItem, byte cbWeaveCount, ref tagGangCardResult  GangCardResult, ushort wMeChairID)
     {
         //设置变量
         byte cbActionMask = WIK_NULL;
         GangCardResult.cbCardData = new byte[4];
         //手上杠牌
         for (byte i = 0; i < MAX_INDEX; i++)
         {
             if (cbCardIndex[i] == 4)
             {
                 cbActionMask |= WIK_GANG;
                 GangCardResult.cbCardData[GangCardResult.cbCardCount] = WIK_GANG;
                 GangCardResult.cbCardData[GangCardResult.cbCardCount++] = SwitchToCardData(i);
             }
         }
     
         //组合杠牌
         for (byte i = 0 ;i < cbWeaveCount; i++)
         {
             if (WeaveItem[wMeChairID,i].cbWeaveKind == WIK_PENG)
             {
                 if (cbCardIndex[SwitchToCardIndex(WeaveItem[wMeChairID,i].cbCenterCard)] == 1)
                 {
                     cbActionMask |= WIK_GANG;
                     GangCardResult.cbCardData[GangCardResult.cbCardCount] = WIK_GANG;
                     GangCardResult.cbCardData[GangCardResult.cbCardCount++] = WeaveItem[wMeChairID,i].cbCenterCard;
                 }
             }
         }
     
         return cbActionMask;
     }

     
     
     //扑克转换
     public static byte SwitchToCardData(byte cbCardIndex)
     {
         return (byte)(((cbCardIndex / 9) << 4) | (cbCardIndex % 9 + 1));
     }
     
     //扑克转换
     public static byte SwitchToCardIndex(byte cbCardData)
     {
         byte a = (byte)(((cbCardData & MASK_COLOR) >> 4) * 9 + (cbCardData & MASK_VALUE) - 1);
         return a;
     }
     
     //扑克转换
     public static byte SwitchToCardData(byte[] cbCardIndex, ref byte[] cbCardData)
     {
         //转换扑克
         byte cbPosition = 0;
         for (byte i = 0; i < MAX_INDEX; i++)
         {
             if (cbCardIndex[i] != 0)
             {
                 for (byte j = 0; j < cbCardIndex[i]; j++)
                 {
                     cbCardData[cbPosition++] = SwitchToCardData(i);
                 }
             }
         }
     
         return cbPosition;
     }
     
     //扑克转换
     public static byte SwitchToCardIndex(byte[] cbCardData, byte cbCardCount, ref byte[] cbCardIndex)
     {   
         //转换扑克
         for (byte i = 0; i < cbCardCount; i++)
         {
             cbCardIndex[SwitchToCardIndex(cbCardData[i])]++;
         }
     
         return cbCardCount;
     }
     
     //分析扑克
     public static bool AnalyseCard(byte[] cbCardIndex, tagWeaveItem[,] WeaveItem, byte cbWeaveCount,ref tagAnalyseItem[]  AnalyseItemArray ,ref int count, ushort wMeChairID)
     {
         //计算数目
         byte cbCardCount = 0;
         for (byte i = 0; i < MAX_INDEX; i++) {
             cbCardCount += cbCardIndex[i];
         }
         for(int i = 0; i< MAX_INDEX;i++){
             AnalyseItemArray[i] = new tagAnalyseItem();
             AnalyseItemArray[i].cbWeaveKind = new byte[4];
             AnalyseItemArray[i].cbCenterCard = new byte[4];
         }

         if ((cbCardCount < 2) || (cbCardCount > MAX_COUNT) || ((cbCardCount - 2) % 3 != 0)){
             return false;
         }
         //变量定义
         byte cbKindItemCount = 0;
         tagKindItem[] KindItem = new tagKindItem[MAX_COUNT - 2];
         for(int i = 0;i < MAX_COUNT - 2; i++){
             KindItem[i] = new tagKindItem();
         }
     
         //需求判断
         byte cbLessKindItem=(byte)((cbCardCount - 2) / 3);
         //单吊判断
         if (cbLessKindItem == 0)
         {
             //牌眼判断
             for (byte i = 0; i < MAX_INDEX; i++)
             {
                 if (cbCardIndex[i] == 2)
                 {                       
                     //设置结果
                     for (byte j = 0; j < cbWeaveCount; j++)
                     {
                         AnalyseItemArray[count].cbWeaveKind[j] = WeaveItem[wMeChairID,j].cbWeaveKind;
                         AnalyseItemArray[count].cbCenterCard[j] = WeaveItem[wMeChairID,j].cbCenterCard;
                     }
                     AnalyseItemArray[count].cbCardEye = SwitchToCardData(i);
                     count++;
                     return true;
                 }
             }
     
             return false;
         }

         //拆分分析
         if (cbCardCount >= 3)
         {
             
             for (byte i = 0; i < MAX_INDEX; i++)
             {
                 //同牌判断
                 if (cbCardIndex[i] >= 3)
                 {
                     
                     KindItem[cbKindItemCount].cbCardIndex[0] = i;
                     KindItem[cbKindItemCount].cbCardIndex[1] = i;
                     KindItem[cbKindItemCount].cbCardIndex[2] = i;
                     KindItem[cbKindItemCount].cbWeaveKind = WIK_PENG;
                     KindItem[cbKindItemCount++].cbCenterCard = SwitchToCardData(i);
                 }
                 //
                 //连牌判断
                 if ((i < (MAX_INDEX - 9)) && (cbCardIndex[i] > 0) && ((i % 9) < 7))
                 {
                     for (byte j = 1; j <= cbCardIndex[i]; j++)
                     {
                         if ((cbCardIndex[i + 1] >= j) && (cbCardIndex[i + 2] >= j))
                         {
                             
                             KindItem[cbKindItemCount].cbCardIndex[0] = i;
                             KindItem[cbKindItemCount].cbCardIndex[1] = (byte)(i + 1);
                             KindItem[cbKindItemCount].cbCardIndex[2] = (byte)(i + 2);
                             KindItem[cbKindItemCount].cbWeaveKind = WIK_LEFT;
                             KindItem[cbKindItemCount++].cbCenterCard = SwitchToCardData(i);
                         }
                     }
                 }
                 //
             }
         }
     
         //组合分析
         if (cbKindItemCount >= cbLessKindItem)
         {
             //变量定义
             byte[] cbCardIndexTemp = new byte[MAX_INDEX];
     
             //变量定义
             byte[] cbIndex={0,1,2,3};
             tagKindItem[] pKindItem = new tagKindItem[4];
             for(int i = 0;i < 4; i++){
                 pKindItem[i] = new tagKindItem();
             }
     
             //开始组合
             do
             {
                 //设置变量
                 Buffer.BlockCopy(cbCardIndex,0,cbCardIndexTemp,0,MAX_INDEX);
                 for (byte i = 0; i < cbLessKindItem; i++)
                     pKindItem[i] = KindItem[cbIndex[i]];
     
                 //数量判断
                 bool bEnoughCard = true;
                 for (byte i = 0; i < cbLessKindItem * 3; i++)
                 {
                     //存在判断
                     byte cbCardIndex1 = pKindItem[i / 3].cbCardIndex[i % 3]; 
                     if (cbCardIndexTemp[cbCardIndex1] == 0)
                     {
                         bEnoughCard = false;
                         break;
                     }
                     else 
                         cbCardIndexTemp[cbCardIndex1]--;
                 }
     
                 //胡牌判断
                 if (bEnoughCard == true)
                 {
                     //牌眼判断
                     byte cbCardEye = 0;
                     for (byte i = 0; i < MAX_INDEX; i++)
                     {
                         if (cbCardIndexTemp[i] == 2)
                         {
                             cbCardEye = SwitchToCardData(i);
                             break;
                         }
                     }

                     //组合类型
                     if (cbCardEye != 0)
                     {
                         //设置组合
                         for (byte i = 0; i < cbWeaveCount; i++)
                         {
                             AnalyseItemArray[count].cbWeaveKind[i] = WeaveItem[wMeChairID,i].cbWeaveKind;
                             AnalyseItemArray[count].cbCenterCard[i] = WeaveItem[wMeChairID,i].cbCenterCard;
                         }
     
                         //设置牌型
                         for (byte i = 0; i < cbLessKindItem; i++) 
                         {   
                             AnalyseItemArray[count].cbWeaveKind[i + cbWeaveCount] = WeaveItem[wMeChairID,i].cbWeaveKind;
                             AnalyseItemArray[count].cbCenterCard[i + cbWeaveCount] = WeaveItem[wMeChairID,i].cbCenterCard;
                         }
     
                         //设置牌眼
                         AnalyseItemArray[count].cbCardEye = cbCardEye;
     
                         //插入结果
                         count++;
                     }
                 }
     
                 //设置索引
                 if (cbIndex[cbLessKindItem - 1] == (cbKindItemCount - 1))
                 {
                     int i = cbLessKindItem - 1;
                     for (i = cbLessKindItem - 1; i > 0; i--)
                     {
                         if ((cbIndex[i - 1] + 1) != cbIndex[i])
                         {
                             byte cbNewIndex = cbIndex[i - 1];
                             for (int j = (i - 1); j < cbLessKindItem; j++) 
                                 cbIndex[j] = (byte)(cbNewIndex + j - i + 2);
                             break;
                         }
                     }
                     if (i == 0)
                         break;
                 }
                 else
                     cbIndex[cbLessKindItem - 1]++;
                 
             } while (true);
     
         }
         return (count > 0);
     }
 }
 
 
}
