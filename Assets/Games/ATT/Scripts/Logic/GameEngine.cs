using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.ATT
{
	[GameDesc(GameIDPrefix = 1010, Version="1.0.0")]
    public class GameEngine : PokerGameAgent
    {
        public static GameEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                else
                {
                    return instance as GameEngine;
                }
            }
        }
    }
}