using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.ATT
{

    public enum enSceneType
    {
        SCENE_NONE = 0,
        SCENE_LOGIN,
        SCENE_REG,
        SCENE_QUICK,
        SCENE_SERVER,
        SCENE_GAME,
        SCENE_TASK,
        SCENE_SHOP,
        SCENE_HELP,
        SCENE_PERSON,
        SCENE_MODIFY,
        SCENE_SINGLE,
        SCENE_VIP
    };
    public class UIManger : MonoBehaviour
    {

        static UIManger instance = null;

        GameObject o_quick = null;
        GameObject o_login = null;
        GameObject o_reg = null;
        GameObject o_server = null;
        GameObject o_game = null;
        GameObject o_single = null;
        GameObject o_task = null;
        GameObject o_shop = null;

        GameObject o_help = null;

        GameObject o_person = null;
        GameObject o_modify = null;

        enSceneType _SceneType = enSceneType.SCENE_NONE;
        /// <summary>
        /// 玩家人数
        /// </summary>
        public int m_iGamePlayer = 6;
        public int m_iGameMaxPoint = 200;
        public int m_iGameStartPoint = 1;
        public int m_iGameTime = 120;
        public float m_fGameTime = 120;
        public bool m_bIsOpenTimer = false;
        public bool m_bIsGetscore = false;
        public bool m_bIsStartGame = false;
        public bool m_bIsPlayingGame = false;
        public static UIManger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIManger").AddComponent<UIManger>();
                }
                return instance;
            }
        }

        public enSceneType SceneType
        {
            get
            {
                return _SceneType;
            }
        }
        /// <summary>
        /// 游戏信息
        /// </summary>
        /// /// <summary>
        /// 游戏初始信息
        /// </summary>
        public CGameFree m_cGameSceneFree = new CGameFree();

        public CMD_S_Update_Card m_cUpdateCard = new CMD_S_Update_Card();
        //
        public tagReadFile m_tReadFile = new tagReadFile();

        /// <summary>
        /// 比倍结果
        /// </summary>
        public CMD_S_COMPARE_RESUALT m_cCompareResualt = new CMD_S_COMPARE_RESUALT();

        public bool m_bIsBigReward = false;
        public long m_lAllScore = 0;
        public long m_lPrizeScore = 0;
        private bool _bISUpdateCard;
        public long m_lGRPrizePool = 0;

        //是否更新牌
        public bool m_bIsUpdateCard
        {
            get { return _bISUpdateCard; }
            set
            {
                _bISUpdateCard = value;
                CPokerManger._instance.SetPockerBTDisable(_bISUpdateCard);
            }
        }

        //赔率类型
        public int m_iPeiLvType = 0;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            m_iGameTime = 120;
            m_fGameTime = (float)m_iGameTime;
        }

        void OnDestroy()
        {
            instance = null;
        }
        void Start()
        {
            //创建所有UI对象
            o_quick = GameObject.Find("scene_quick");
            o_reg = GameObject.Find("scene_reg");
            o_server = GameObject.Find("scene_server");

            o_game = GameObject.Find("GameParent");
            o_login = GameObject.Find("scene_login");

            o_task = GameObject.Find("scene_task");
            o_shop = GameObject.Find("scene_shop");

            o_help = GameObject.Find("scene_help");
            o_person = GameObject.Find("scene_person");
            o_modify = GameObject.Find("scene_modify");

            GoUI(enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
        }
        public void Update()
        {
           m_fGameTime -= Time.deltaTime;
           if (m_fGameTime <= 15 && !m_bIsOpenTimer)
            {
                m_bIsOpenTimer = true;
                CQuitUser._instance.ShowWindow();
                CQuitUser._instance.SetGameTime(m_fGameTime);
            }

            //踢出房间
           if (m_fGameTime <= 0)
           {
               if (m_bIsStartGame)
               {
                   m_fGameTime += 2;
                   m_bIsStartGame = false;
                   CBottomBTOnclick._instance.StartBT_OnClick();
                   StartCoroutine(WaitQuit());
               }
               else if (m_bIsGetscore)
               {
                   m_bIsGetscore = false;
                   CBottomBTOnclick._instance.CompareGetScore_Onclick();
                   GameEngine.Instance.Quit();
               }
               else
               {
                   GameEngine.Instance.Quit();
               }
               
            }
        }
        public void GoUI(enSceneType stFrom, enSceneType stTo)
        {
            // HideAllUI();

            _SceneType = enSceneType.SCENE_GAME;
            if (o_game != null)
            {
                o_game.SetActive(true);
                o_game.GetComponent<UIGame>().Init();
            }
        }
        public IEnumerator WaitTimeRectGame(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);

            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerPointsManger._instance.ShowWindow();
            CBasePoint._instance.ShowWindow();
#else

#endif
            CPokerPointsManger._instance.m_bIsChangeColor = false;

            RectGame();


        }
        IEnumerator WaitQuit()
        {
            yield return new WaitForSeconds(2.0f);
            m_bIsGetscore = false;
            CBottomBTOnclick._instance.CompareGetScore_Onclick();
            GameEngine.Instance.Quit();
        }
        public void RectGame()
        {
            //重置游戏界面
            
            CPokerManger._instance.RectCard();
            CBottomBTOnclick._instance.Reset();
            CCompareManger._instance.RectCompare();
            CCompareManger._instance.m_bIsWinPushDouble = false;
            CCompareManger._instance.m_bIsOpen = false;
            CPushAnimation._instance.gameObject.SetActive(true);
            CPushAnimation._instance.SetPushAnimation("PUSHBET");
            CCompareManger._instance.HideWindow();
            CBasePoint._instance.m_bIsOpen = false;
            CBasePoint._instance.SetBasePoint();
            m_lPrizeScore = 0;
            m_bIsPlayingGame = false;
            m_fGameTime = (float)m_iGameTime;
            m_bIsGetscore = false;
            m_bIsStartGame = false;
            if (CPlayerInfo._instance != null)
            {
                CPlayerInfo._instance.m_iGold = m_lAllScore + m_lGRPrizePool;
                CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;
                m_lGRPrizePool = 0;
                m_lAllScore = 0;
                if (CPlayerInfo._instance.m_iRoomTimes != 0)
                    CPlayerInfo._instance.m_iCreditNum = (CPlayerInfo._instance.m_iGold / CPlayerInfo._instance.m_iRoomTimes);
            }
            if (CPokerPointsManger._instance != null)
            {
                if (CPokerPointsManger._instance.m_iBasePoints > CPokerPointsManger._instance.m_iBasePointBK)
                {
                    if (CPlayerInfo._instance.m_iCreditNum > CPokerPointsManger._instance.m_iBasePoints)
                        CPokerPointsManger._instance.m_iBasePointBK = CPokerPointsManger._instance.m_iBasePoints;
                    else
                        CPokerPointsManger._instance.m_iBasePointBK = (int)CPlayerInfo._instance.m_iCreditNum;
                }
                else if (CPlayerInfo._instance.m_iCreditNum < CPokerPointsManger._instance.m_iBasePointBK)
                {
                    CPokerPointsManger._instance.m_iBasePointBK = (int)CPlayerInfo._instance.m_iCreditNum;
                }

                //if (CPokerPointsManger._instance.m_iBasePointBK == 0) CPokerPointsManger._instance.m_iBasePointBK = 1; 
                if (CPokerPointsManger._instance.m_iBasePointBK > 0 && m_cUpdateCard.bIsCompare)
                {
                    CBottomBTOnclick._instance.m_bIsCompareFree = true;
                    CBottomBTOnclick._instance.m_gMoreThanBT2.SetActive(false);
                    CBottomBTOnclick._instance.m_gMoreThanBT.SetActive(true);
                    CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, false);
                }
                
                CPokerPointsManger._instance.m_iBasePoints = CPlayerInfo._instance.m_iBet;
                CPokerPointsManger._instance.m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
            }
            CPokerPointsManger._instance.RectPoint();
            CBottomBTOnclick._instance.m_bIsOpenComPare = false;
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gRecordBT, false);

        }
        /// <summary>
        /// 隐藏所有窗口
        /// </summary>
        public void HideAllWindow()
        {
            CPokerPointsManger._instance.HideWindow();
            CBasePoint._instance.HideWindow();
            CPokerManger._instance.HideWindow();
            CCompareManger._instance.HideWindow();
            CRecordMangerOne._instance.HideWindow();
            CRecordMangerTwo._instance.HideWindow();
            CRecordMangerThree._instance.HideWindow();
        }
    }
}
