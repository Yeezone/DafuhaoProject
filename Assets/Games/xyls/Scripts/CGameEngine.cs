using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    [GameDesc(GameIDPrefix = 1017, Version="1.1.2")]
    public class CGameEngine : PokerGameAgent
    {
        public static CGameEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                else
                {
                    return instance as CGameEngine;
                }
            }
        }
    }
}