
using com.QH.QPGame.Services.Data;

using System;
using System.Collections.Generic;

namespace com.QH.QPGame.ZJH
{
	[GameDesc(GameIDPrefix = 1002, Version="1.0.0")]
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
