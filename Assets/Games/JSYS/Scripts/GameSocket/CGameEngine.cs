using UnityEngine;
using System.Collections;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.JSYS
{
	[GameDesc(GameIDPrefix = 1016, Version="1.0.0")]
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
