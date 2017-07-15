using UnityEngine;
using System.Collections;
using com.QH.QPGame.Services.Data;
using System;

namespace com.QH.QPGame.CX{

    public class UISettlement : MonoBehaviour 
    {

        Transform content;

        GameObject cancle;

        GameObject o_continue;

        UILabel getScore;

        UISprite resultsp = null;

        Action callbackAction = null;

        void Awake()
        {
            content = transform.Find("result/ScrollView/gameGrid");
            cancle = transform.Find("result/Cancle").gameObject;
            UIEventListener.Get(cancle).onClick = OnClick;

            o_continue = transform.Find("result/Sure").gameObject;
            UIEventListener.Get(o_continue).onClick = OnClickContinue;

            getScore = transform.Find("GetCoin").GetComponent<UILabel>();

            resultsp = transform.Find("title").GetComponent<UISprite>();
        }

        void OnEnable()
        {
            UIGame._bEndGame = true;
            SetUserClock(15);
        }

        void OnDisable()
        {
            UIGame._bEndGame = false;
        }

        void OnClick(GameObject obj)
        {
            GameEngine.Instance.Quit();
        }

        void OnClickContinue(GameObject obj)
        {
            if (callbackAction != null)
            {
                callbackAction();
            }
        }

	    // Use this for initialization
	    void Start () {
	
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void InitData(byte[] cbPlayStatus, byte[][] bcards, long[] lscore, byte cardsNum, PlayerInfo[] userdata, GameExitType endreasion = GameExitType.END_REASON_NORMAL, Action action = null)
        {
            
            for (byte i = 0; i < GameLogic.GAME_PLAYER; ++i)
            {
                Transform tr = content.GetChild(i);
                tr.GetChild(1).gameObject.SetActive(false);
                tr.GetChild(2).gameObject.SetActive(false);
                tr.GetChild(4).gameObject.SetActive(false);
                tr.GetChild(5).gameObject.SetActive(false);
                tr.GetChild(0).GetComponent<UILabel>().text = "";
                tr.GetChild(3).GetComponent<UILabel>().text = "";
                tr.Find("head").GetComponent<UISprite>().spriteName = "close";
                tr.Find("trial").GetComponent<UISprite>().spriteName = "close";
                tr.Find("peaceful").GetComponent<UISprite>().spriteName = "close";
            }  
            
            callbackAction = action;
            byte bindex = 0;
            for (byte i = 0; i < GameLogic.GAME_PLAYER;++i )
            {                 
                if (userdata[i] != null)
                {
                    Transform tr = content.GetChild(bindex);
                    tr.gameObject.SetActive(true);
                    tr.GetChild(0).GetComponent<UILabel>().text = userdata[i].NickName;
                    tr.GetChild(3).GetComponent<UILabel>().text = lscore[i].ToString();

                    if (endreasion == GameExitType.END_REASON_NORMAL || endreasion == GameExitType.END_REASON_PASS)
                    {
                        ++bindex;
                        if (cbPlayStatus[i] == 1 && cardsNum == 4 && UIGame._IsSplit == true)
                        {
                            byte[] headcard = new byte[2];
                            byte[] trialcard = new byte[2];
                            Buffer.BlockCopy(bcards[i], 0, headcard, 0, 2);
                            Buffer.BlockCopy(bcards[i], 2, trialcard, 0, 2);
                            bool isFir = GameLogic.CompareCardN(headcard, trialcard, 2);
                            string headname = GameLogic.GetHeadTailTypeStr(headcard, 2);
                            string trialname = GameLogic.GetHeadTailTypeStr(trialcard, 2);
                            //交换显示
                            if (!isFir)
                            {
                                string tmp = headname;
                                headname = trialname;
                                trialname = tmp;
                            }

                            if (GameLogic.GetPeaceful(bcards[i]) > 0)
                            {
                                byte cbcardtype = GameLogic.GetPeaceful(bcards[i]);
                                string cardname = "a_" + cbcardtype;
                                tr.Find("peaceful").GetComponent<UISprite>().spriteName = cardname;

                                tr.Find("head").GetComponent<UISprite>().gameObject.SetActive(false);
                                tr.Find("trial").GetComponent<UISprite>().gameObject.SetActive(false);
                                tr.Find("peaceful").GetComponent<UISprite>().gameObject.SetActive(true);
                            }
                            else
                            {
                                tr.Find("head").GetComponent<UISprite>().spriteName = headname;
                                tr.Find("trial").GetComponent<UISprite>().spriteName = trialname;

                                tr.Find("head").GetComponent<UISprite>().gameObject.SetActive(true);
                                tr.Find("trial").GetComponent<UISprite>().gameObject.SetActive(true);
                                tr.Find("peaceful").GetComponent<UISprite>().gameObject.SetActive(false);
                            }

                            tr.GetChild(1).gameObject.SetActive(false);
                            tr.GetChild(2).gameObject.SetActive(false);
                            tr.GetChild(4).gameObject.SetActive(true);
                            tr.GetChild(5).gameObject.SetActive(true);
                        }
                        else
                        {
                            tr.Find("peaceful").GetComponent<UISprite>().gameObject.SetActive(false);
                            tr.GetChild(1).gameObject.SetActive(true);
                            tr.GetChild(2).gameObject.SetActive(true);
                            tr.GetChild(4).gameObject.SetActive(true);
                            tr.GetChild(5).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        tr.Find("head").GetComponent<UISprite>().gameObject.SetActive(false);
                        tr.Find("trial").GetComponent<UISprite>().gameObject.SetActive(false);
                        tr.Find("peaceful").GetComponent<UISprite>().gameObject.SetActive(false);
                        tr.GetChild(1).gameObject.SetActive(false);
                        tr.GetChild(2).gameObject.SetActive(false);
                        tr.GetChild(4).gameObject.SetActive(false);
                        tr.GetChild(5).gameObject.SetActive(false);
                    }

                    if (GetSelfChair() == i)
                    {
                        if (lscore[i] > 0)
                        {
                            getScore.text = "+" + lscore[i];
                            resultsp.spriteName = "win";
                        }
                        else if (lscore[i] < 0)
                        {
                            getScore.text = "" + lscore[i];
                            resultsp.spriteName = "fail";
                        }
                        else
                        {
                            getScore.text = "+" + lscore[i];
                            resultsp.spriteName = "draw";
                        }
                    }
                }
            }
        }

        byte GetSelfChair()
        {
            return (byte)GameEngine.Instance.MySelf.DeskStation;
        }


        void OnTimerEnd()
        {
            GameEngine.Instance.Quit();
        }
        void SetUserClock(uint time)
        {
            gameObject.GetComponent<UIClock>().SetTimer(time * 1000);
        }


    }

}