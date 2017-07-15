
namespace com.QH.QPGame.DDZ
{
    public class Level
    {
        public int lvl;
        public int curr_exp;
        public int next_exp;
        public string desc;
        public float award_rate;
        public float buy_rate;

    }

    public class GameConfig
    {
        private static GameConfig _instance;

        public static GameConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameConfig();

                return _instance;
            }
        }


        public int GetExpLevel(int n)
        {
            if (n < 100)
            {
                return 1;
            }
            else if (n < 500)
            {
                return 2;
            }
            else if (n < 1000)
            {
                return 3;
            }
            else if (n < 2000)
            {
                return 4;
            }
            else if (n < 5000)
            {
                return 5;
            }
            else if (n < 10000)
            {
                return 6;
            }
            else
            {
                return 7;
            }
        }

        public Level GetLevel(int n)
        {
            Level lv = new Level();

            if (n < 100)
            {
                lv.lvl = 1;
                lv.curr_exp = n;
                lv.next_exp = 100;
                lv.desc = "Lv.1";
                return lv;
            }
            else if (n < 1000)
            {
                lv.lvl = 2;
                lv.curr_exp = n;
                lv.next_exp = 1000;
                lv.desc = "Lv.2";
                return lv;
            }
            else if (n < 10000)
            {
                lv.lvl = 3;
                lv.curr_exp = n;
                lv.next_exp = 10000;
                lv.desc = "Lv.3";
                return lv;
            }
            else if (n < 100000)
            {
                lv.lvl = 4;
                lv.curr_exp = n;
                lv.next_exp = 100000;
                lv.desc = "Lv.4";
                return lv;
            }
            else if (n < 1000000)
            {
                lv.lvl = 5;
                lv.curr_exp = n;
                lv.next_exp = 1000000;
                lv.desc = "Lv.5";
                return lv;
            }
            else if (n < 10000000)
            {
                lv.lvl = 6;
                lv.curr_exp = n;
                lv.next_exp = 10000000;
                lv.desc = "Lv.6";
                return lv;
            }
            else if (n < 100000000)
            {
                lv.lvl = 7;
                lv.curr_exp = n;
                lv.next_exp = 100000000;
                lv.desc = "Lv.7";
                return lv;
            }
            else
            {
                lv.lvl = 8;
                lv.curr_exp = n;
                lv.next_exp = 1000000000;
                lv.desc = "Lv.8";
                return lv;
            }

        }

        public Level GetVipLevel(int n)
        {
            Level lv = new Level();
            if (n == 0)
            {
                lv.lvl = 0;
                lv.curr_exp = 0;
                lv.next_exp = 1;
                lv.desc = "无";
                lv.award_rate = 0f;
                lv.buy_rate = 0f;

                return lv;
            }
            else if (n < 600)
            {
                lv.lvl = 1;
                lv.curr_exp = n;
                lv.next_exp = 600;
                lv.desc = "Lv.1";
                lv.award_rate = 1.2f;
                lv.buy_rate = 0.01f;

                return lv;
            }
            else if (n < 1800)
            {
                lv.lvl = 2;
                lv.curr_exp = n;
                lv.next_exp = 1800;
                lv.desc = "Lv.2";
                lv.award_rate = 1.3f;
                lv.buy_rate = 0.02f;

                return lv;
            }
            else if (n < 3600)
            {
                lv.lvl = 3;
                lv.curr_exp = n;
                lv.next_exp = 3600;
                lv.desc = "Lv.3";
                lv.award_rate = 1.5f;
                lv.buy_rate = 0.03f;

                return lv;
            }
            else if (n < 6000)
            {
                lv.lvl = 4;
                lv.curr_exp = n;
                lv.next_exp = 6000;
                lv.desc = "Lv.4";
                lv.award_rate = 1.7f;
                lv.buy_rate = 0.04f;

                return lv;
            }
            else if (n < 10800)
            {
                lv.lvl = 5;
                lv.curr_exp = n;
                lv.next_exp = 10800;
                lv.desc = "Lv.5";
                lv.award_rate = 2.0f;
                lv.buy_rate = 0.05f;

                return lv;
            }
            else if (n < 32400)
            {
                lv.lvl = 6;
                lv.curr_exp = n;
                lv.next_exp = 32400;
                lv.desc = "Lv.6";
                lv.award_rate = 2.5f;
                lv.buy_rate = 0.06f;

                return lv;
            }
            else
            {
                lv.lvl = 7;
                lv.curr_exp = n;
                lv.next_exp = 1000000000;
                lv.desc = "Lv.7";
                lv.award_rate = 3.0f;
                lv.buy_rate = 0.07f;

                return lv;
            }

        }

        public string GetFaceName(int wFaceID)
        {
            wFaceID = (int) (wFaceID%16);
            return "face_" + wFaceID.ToString();

        }
    }
}