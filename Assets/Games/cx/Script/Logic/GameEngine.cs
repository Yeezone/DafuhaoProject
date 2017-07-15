
using System;
using System.Collections.Generic;

using Shared;
using com.QH.QPGame.Services.Data;


namespace com.QH.QPGame.CX
{
	[GameDesc(GameIDPrefix = 1008, Version="1.0.0")]
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
