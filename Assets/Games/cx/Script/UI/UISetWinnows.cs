using UnityEngine;
using System.Collections;
using Shared;
using System;

namespace com.QH.QPGame.CX
{
    public class UISetWinnows : MonoBehaviour
    {
        //获取UIgame
        public GameObject scenegame;
        //筹码框显示筹码
        public GameObject[] o_ChipFrame = new GameObject[3];
        //筹码个数
        private byte chipCount = 0;
        //当前筹码数值
        private int[] chipValue = new int[3];
        //簸簸数显示与记录
        private GameObject o_BoBoScore;

        //筹码按钮
        private GameObject btnChip1;
        private GameObject btnChip2;
        private GameObject btnChip3;

        public long MaxBoboNum = GameLogic.GAME_MAX_INVERT;
        public long MinBoboNum = GameLogic.GAME_MIN_INVERT;
        public int BoboChangeNum = GameLogic.GAME_CHANGE_NUM;

        private int ReadyTime = 15;

        private GameObject btnSetMax;
        private GameObject btnSetMin;
        private GameObject btnConfirm;
        private GameObject clockValue;

        private int lCellScore = 0;

        //筹码数值
        private int value1 = 0;
        private int value2 = 1;
        private int value3 = 2;

        void Start()
        {
            lCellScore = scenegame.GetComponent<UIGame>()._lCellScore;

            value1 = lCellScore;
            value2 = lCellScore * 10;
            value3 = lCellScore * 100;

            btnChip1.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = value1;
            btnChip2.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = value2;
            btnChip3.transform.Find("sp_txt").GetComponent<cx_number>().m_iNum = value3;
        }

        void Awake()
        {
            o_BoBoScore = transform.Find("boboScore").gameObject;

            btnChip1 = transform.Find("btn_bet1").gameObject;
            btnChip2 = transform.Find("btn_bet2").gameObject;
            btnChip3 = transform.Find("btn_bet3").gameObject;

            btnSetMax = transform.Find("MaxBobo/BtnSetmax").gameObject;
            btnSetMin = transform.Find("MinBobo/BtnSetmin").gameObject;

            btnConfirm = transform.Find("Confirm").gameObject;

            clockValue = transform.Find("clock/lbl_num").gameObject;

            UIEventListener.Get(btnSetMax).onClick = OnClick;
            UIEventListener.Get(btnSetMin).onClick = OnClick;
            UIEventListener.Get(btnConfirm).onClick = OnClick;
            UIEventListener.Get(btnChip1).onClick = OnClick;
            UIEventListener.Get(btnChip2).onClick = OnClick;
            UIEventListener.Get(btnChip3).onClick = OnClick;
        }

        public void InitData(long score, bool repeat, long winScore)
        {
            MaxBoboNum = GameLogic.GAME_MAX_INVERT;
            MinBoboNum = GameLogic.GAME_MIN_INVERT;
            BoboChangeNum = GameLogic.GAME_CHANGE_NUM;

            long currentScore = scenegame.transform.GetComponent<UIGame>().currentBoboshu;
            scenegame.transform.GetComponent<UIGame>().currentBoboshu = MinBoboNum;
            o_BoBoScore.GetComponent<cx_number>().m_iNum = MinBoboNum;
            //GameLogic.GAME_MIN_INVERT = o_BoBoScore.GetComponent<cx_number>().m_iNum;

            if (score < GameLogic.GAME_MAX_INVERT)
            {
                MaxBoboNum = (int)score;
            }
            else
            {
                MaxBoboNum = GameLogic.GAME_MAX_INVERT;
            }

            if (repeat == true) RepeatBet();
        }

        void FixedUpdate()
        {
            if (clockValue.GetComponent<cx_number>().m_iNum <= 1)
            {
                RepeatBet();
            }
        }

        void OnDisEnable()
        {
            MaxBoboNum = GameLogic.GAME_MAX_INVERT;
            MinBoboNum = GameLogic.GAME_MIN_INVERT;
        }

        void OnEnable()
        {
            ReadyTime = 15;
            SetUserClock(14);
        }

        //重复下注
        void RepeatBet()
        {            
            ReadyGo();
            StartFrameChip();
        }

        void OnClick(GameObject obj)
        {
            //操作按钮
            if (obj.name.Equals("BtnSetmax"))
            {
                o_BoBoScore.GetComponent<cx_number>().m_iNum = MaxBoboNum;
            }
            else if (obj.name.Equals("BtnSetmin"))
            {
                o_BoBoScore.GetComponent<cx_number>().m_iNum = MinBoboNum;
            }
            else if (obj.name.Equals("Confirm"))
            {
                ReadyGo();
                StartFrameChip();
            }
            else if (obj.name.Equals("btn_bet1"))
            {
                int value = (int)o_BoBoScore.GetComponent<cx_number>().m_iNum;
                o_BoBoScore.GetComponent<cx_number>().m_iNum = GetBoboNumber(value, lCellScore);
            }
            else if (obj.name.Equals("btn_bet2"))
            {
                int value = (int)o_BoBoScore.GetComponent<cx_number>().m_iNum;
                o_BoBoScore.GetComponent<cx_number>().m_iNum = GetBoboNumber(value, lCellScore * 10);
            }
            else if (obj.name.Equals("btn_bet3"))
            {
                int value = (int)o_BoBoScore.GetComponent<cx_number>().m_iNum;
                o_BoBoScore.GetComponent<cx_number>().m_iNum = GetBoboNumber(value, lCellScore * 100);
            }
        }

        private void ReadyGo()
        {
            long boboScore = o_BoBoScore.GetComponent<cx_number>().m_iNum;

            scenegame.transform.GetComponent<UIGame>().currentBoboshu = boboScore;
            scenegame.transform.Find("repeat_boboshu").gameObject.SetActive(true);
            scenegame.transform.Find("repeat_boboshu").transform.Find("boboScore").GetComponent<cx_number>().m_iNum = boboScore;
            //合法性判断
            if (boboScore > MaxBoboNum)
            {
                boboScore = MaxBoboNum;
            }
            if (boboScore < MinBoboNum)
            {
                boboScore = MinBoboNum;
            }

            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_USER_INVEST);
            packet.AddLong(boboScore);
            UIGame.SendMsgToServer(packet);
            UIGame.o_player_allinvert.SetActive(true);
            gameObject.SetActive(false);
        }

        public void CloseWindows()
        {
            long boboScore = o_BoBoScore.GetComponent<cx_number>().m_iNum;
            //合法性判断
            if (boboScore > MaxBoboNum)
            {
                boboScore = MaxBoboNum;
            }
            if (boboScore < MinBoboNum)
            {
                boboScore = MinBoboNum;
            }

            StartFrameChip();
            UIGame.o_player_allinvert.SetActive(true);
            gameObject.SetActive(false);            
        }

        /// <summary>
        /// 设置簸簸数显示
        /// </summary>
        /// <param name="num"></param>
        /// <param name="isadd"></param>
        /// <returns></returns>
        private long GetBoboNumber(int num, int addnum)
        {
            long outnum;
            if (IsInt(num))
            {
                outnum = num + addnum;

                if (chipCount < 3)
                {
                    int chipcount = 0;

                    if (chipCount < 1)
                    {
                        chipcount = 0;
                    }
                    else
                    {
                        chipcount = chipCount - 1;
                    }
                    if (chipValue[chipCount] != addnum && chipValue[chipCount] == 0 && chipValue[chipcount] != addnum && chipValue[0] != addnum)
                    {
                        chipValue[chipCount] = addnum;
                        ShowFrameChip(chipCount, addnum);
                        chipCount++;
                    }
                }
            }
            else
            {
                outnum = MinBoboNum;
            }
            if (outnum < MinBoboNum)
            {
                outnum = MinBoboNum;
            }
            else if (outnum > MaxBoboNum)
            {
                outnum = MaxBoboNum;
            }
            return outnum;
        }

        public static bool IsInt(int str)
        {
            int result = -1;
            try
            {
                result = str;
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool Isint(string str)
        {
            int result = -1;
            try
            {
                result = int.Parse(str);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }


        void ShowFrameChip(byte chipIndex, int chipScore)
        {
            int nameindex = 0;

            if (value1 == chipScore) nameindex = 0;
            if (value2 == chipScore) nameindex = 1;
            if (value3 == chipScore) nameindex = 2;

            o_ChipFrame[chipIndex].GetComponent<UISprite>().spriteName = "frameChip_" + nameindex.ToString();
            o_ChipFrame[chipIndex].transform.Find("value").GetComponent<cx_number>().m_iNum = chipScore;
            o_ChipFrame[chipIndex].SetActive(true);
        }
        void StartFrameChip()
        {
            for (int i = 0; i < 3; i++)
            {
                chipCount = 0;
                chipValue[i] = 0;
                o_ChipFrame[i].transform.Find("value").GetComponent<cx_number>().m_iNum = 0;
                o_ChipFrame[i].SetActive(false);
            }
        }

        void OnTimerEnd()
        {
            ReadyGo();
        }
        void SetUserClock(uint time)
        {
            try
            {
                gameObject.GetComponent<UIClock>().SetTimer(time * 1000);

            }
            catch (Exception ex)
            {
                //UIMsgBox.Instance.Show(true, ex.Message);
            }
        }

    }
}
