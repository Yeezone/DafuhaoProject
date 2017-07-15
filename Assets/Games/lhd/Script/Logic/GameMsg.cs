using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.LHD
{

    public class GameMsg
    {
        //COMMON
        public const string MSG_CM_001 = "连接服务器失败!";
        public const string MSG_CM_002 = "您的网速不给力,请检查网络!";
        public const string MSG_CM_005 = "确定要逃跑吗?";
        public const string MSG_CM_006 = "确定要退出游戏吗?";
        public const string MSG_CM_007 = "确定要兑换吗?";
        public const string MSG_CM_008 = "您的网络已中断,请重新登陆!";
        public const string MSG_CM_009 = "您的网络超时,请重试!";
        //PLAZA
        public const string MSG_GP_001 = "请输入有效的帐号!";
        public const string MSG_GP_002 = "请输入有效的密码!";
        public const string MSG_GP_003 = "两次输入的密码不一致!";
        public const string MSG_GP_021 = "帐号只允许使用数字英文字母和下划线的组合!";
        public const string MSG_GP_022 = "密码只允许使用数字英文字母和下划线的组合!";


        public const string MSG_GP_004 = "兑换奖品成功,请等待管理员审核!";
        public const string MSG_GP_005 = "您的宝石数量不足,兑换失败!";

        public const string MSG_GP_006 = "游戏帐号无效!";
        public const string MSG_GP_007 = "帐号处于冻结状态!";
        public const string MSG_GP_008 = "游戏帐号无效!";
        public const string MSG_GP_009 = "密码修改成功请记住新密码!";

        public const string MSG_GP_010 = "金币兑换成功!";
        public const string MSG_GP_011 = "您的宝石数量不足,兑换失败!";

        public const string MSG_GP_012 = "游戏已经有新版本,现在要进行更新吗?";
        public const string MSG_GP_013 = "游戏已经有新版本,必须更新才能继续游戏!";
        public const string MSG_GP_014 = "";
        //ROOM
        public const string MSG_GR_001 = "您的游戏币不足500，不能进入本房间!";
        public const string MSG_GR_002 = "您的游戏币不足5000，不能进入本房间!";
        public const string MSG_GR_003 = "您的游戏币不足50000，不能进入本房间!";
        public const string MSG_GR_004 = "您的游戏币不足500000，不能进入本房间!";

        public const string MSG_GR_005 = "房间正在维护中,请稍后再试,或选择其他房间!";
        public const string MSG_GR_006 = "你的网速太不给力了,请重试!";
        public const string MSG_GR_007 = "此房间人员已满,请换个房间重试!";
        public const string MSG_GR_008 = "你的座位别人抢占了,请重试!";

        //GAME
        public const string MSG_GG_001 = "您的游戏币低于房间限制 , 不能继续游戏!";
        public const string MSG_GG_002 = "喂, 想什么呢, 快出牌呀!";
        public const string MSG_GG_003 = "喂, 快点出啊, 别磨磨蹭蹭的!";
        public const string MSG_GG_004 = "喂 有你这么出牌的么?";
        public const string MSG_GG_005 = "跟您合作太愉快了!";
        public const string MSG_GG_006 = "不好意思, 出错牌了...";

    }
}