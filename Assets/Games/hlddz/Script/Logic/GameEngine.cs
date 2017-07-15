namespace com.QH.QPGame.DDZ
{
	[GameDesc(GameIDPrefix = 1000, SceneName="level_1000", Version="1.0.1")]
	[GameDesc(GameIDPrefix = 1001, SceneName = "level_1000", Version="1.0.1")]
	[GameDesc(GameIDPrefix = 1003, SceneName = "level_1000", Version="1.0.1")]
    public class GameEngine : PokerGameAgent
    {
        public static GameEngine Instance
        {
            get
            {
				return instance as GameEngine;
            }
        }
    }
}
