using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.DDZ.Help
{
    /// <summary>
    /// 花色
    /// </summary>
    public enum CardColor
    {
        //扑克花色
        UG_FANG_KUAI = 0x00, //方块	0000 0000
        UG_MEI_HUA = 0x10, //梅花	0001 0000
        UG_HONG_TAO = 0x20, //红桃	0010 0000
        UG_HEI_TAO = 0x30, //黑桃	0011 0000
        UG_NT_CARD = 0x40, //主牌	0100 0000
        UG_ERROR_HUA = 0xF0 //错误  1111 0000
    }

    public enum CardType
    {
        CT_ERROR = 0,
        CT_SINGLE = 1,
        CT_DOUBLE = 2,
        CT_THREE = 3,
        CT_THREE_TAKE_ONE = 4,
        CT_THREE_TAKE_DOUBLE = 5,
        CT_SINGLE_LINE = 6,
        CT_DOUBLE_LINE = 7,
        CT_THREE_LINE = 8,
        CT_THREE_LINE_TAKE_ONE = 9,
        CT_THREE_LINE_TAKE_TWO = 10,
        CT_FOUR_LINE_TAKE_ONE = 11,
        CT_FOUR_LINE_TAKE_TWO = 12,
        CT_BOMB_CARD = 13,
        CT_MISSILE_CARD = 14
    }
}
