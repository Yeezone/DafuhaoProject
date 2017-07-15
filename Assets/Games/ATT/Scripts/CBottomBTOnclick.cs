using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace com.QH.QPGame.ATT
{
    public class CBottomBTOnclick : MonoBehaviour
    {

        public static CBottomBTOnclick _instance;

        public GameObject m_gClearBT;
        public GameObject m_gBigBT;
        public GameObject m_gAutoOpenBT;
        public GameObject m_gAutoCloseBT;
        public GameObject m_gRecordBT;
        public GameObject m_gGetScoreBT;
        /// <summary>
        /// 比倍才有
        /// </summary>
        public GameObject m_gGetScoreBT1;
        public GameObject m_gMoreThanBT;
        public GameObject m_gLittleBT;
        public GameObject m_gInputScoreBT;
        public GameObject m_gStartBT;
        public GameObject m_gMusicOpenBT;
        public GameObject m_gMusicCloseBT;
        public GameObject m_gSafeBT;
        public GameObject m_gMoreThanBT2;

        public bool m_bIsFirstInput = true;

        public GameObject m_gCompare;

        public bool m_bIsInPutScore = false;

        public float m_fWorkTime = 0;

        //记录管理
        public List<GameObject> m_lRecordManger = new List<GameObject>();
        public int m_iCurrentIndex = -1;

        public bool m_bIsGetRecord = true;

        /// <summary>
        /// 空闲比倍
        /// </summary>
        public bool m_bIsCompareFree;

        public bool m_bIsOpenComPare = false;


        void Awake()
        {
            _instance = this;

            //SetAuto(true);
            SetMusic(true);
            Reset();

        }
        void OnDestroy()
        {
            _instance = null;
        }
        // Use this for initialization
        void Start()
        {
            m_gInputScoreBT.GetComponent<COnDownBeing>().m_OnChange = DownBeingInputScore;
        }

        // Update is called once per frame
        void Update()
        {
            m_fWorkTime += Time.deltaTime;
            if (m_bIsGetRecord == false && m_fWorkTime >= 20.0f)
            {
                m_bIsGetRecord = true;
            }
        }
        /// <summary>
        /// 启动按钮响应
        /// </summary>
        public void StartBT_OnClick()
        {
            //第一次请求发牌
            if (!UIManger.Instance.m_bIsUpdateCard)
            {
                if (CPokerPointsManger._instance.m_iBasePointBK == 0 && CPokerPointsManger._instance.m_iBasePoints == 0)
                {
                    return;
                }
                if (CPokerPointsManger._instance.m_iBasePoints == UIManger.Instance.m_iGameStartPoint)
                    CPokerPointsManger._instance.m_iBasePoints = CPokerPointsManger._instance.m_iBasePointBK;

                CPlayerInfo._instance.m_iBet = CPokerPointsManger._instance.m_iBasePoints;
                CPokerPointsManger._instance.m_iBasePointBK = CPokerPointsManger._instance.m_iBasePoints;
                m_bIsFirstInput = true;
                //请求发牌
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_START_DEAL_JX);
               
                //压分
                packet.AddLong(CPokerPointsManger._instance.m_iBasePoints);
                //此数据无效
                packet.AddInt(0);

                GameEngine.Instance.Send(packet);
                SetButtonBt(m_gStartBT, true);
                SetButtonBt(m_gInputScoreBT, true);
            }
            //第二次请求发牌
            else
            {

                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_UPDATE_CARD_JX);
                packet.AddBytes(UIManger.Instance.m_cUpdateCard.cCard.cbCard, UIManger.Instance.m_cUpdateCard.cCard.cbCard.Length);
                for (int i = 0; i < UIManger.Instance.m_cUpdateCard.cCard.bBarter.Length; i++)
                {
                    //牌的值
                    packet.AddBool(UIManger.Instance.m_cUpdateCard.cCard.bBarter[i]);

                }
                GameEngine.Instance.Send(packet);
                SetButtonBt(m_gStartBT, true);

            }
            SetButtonBt(m_gRecordBT,true);
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, true);
            CBottomBTOnclick._instance.m_bIsCompareFree = false;
            UIManger.Instance.m_bIsPlayingGame = true;

        }
        /// <summary>
        /// 压分按钮响应
        /// </summary>
        public void InputScoreBt_OnClick()
        {
            if (CPokerPointsManger._instance.m_iBasePoints < 1 && CPlayerInfo._instance.m_iCreditNum <= 1) 
                CPokerPointsManger._instance.m_iBasePoints = (int)CPlayerInfo._instance.m_iCreditNum;

            else if (CPokerPointsManger._instance.m_iBasePoints < UIManger.Instance.m_iGameMaxPoint
                &&  CPlayerInfo._instance.m_iCreditNum >0)
            {
                m_bIsInPutScore = true; 
                CPokerPointsManger._instance.m_iBasePoints += 1;

                if (m_bIsFirstInput && CPlayerInfo._instance.m_iCreditNum > UIManger.Instance.m_iGameStartPoint)
                {
                    m_bIsFirstInput = false;
                    CPlayerInfo._instance.m_iCreditNum -= UIManger.Instance.m_iGameStartPoint;
                    CPlayerInfo._instance.m_iGold -= (CPlayerInfo._instance.m_iRoomTimes * UIManger.Instance.m_iGameStartPoint);
                }
                if (CPlayerInfo._instance.m_iCreditNum >= 1)
                {
                    CPlayerInfo._instance.m_iCreditNum -= 1;
                }
                if (CPlayerInfo._instance.m_iGold >= CPlayerInfo._instance.m_iRoomTimes)
                {
                    CPlayerInfo._instance.m_iGold -= CPlayerInfo._instance.m_iRoomTimes;
                }
                if (CPokerPointsManger._instance.m_iBasePoints >=UIManger.Instance.m_iGameMaxPoint
                 || CPlayerInfo._instance.m_iCreditNum <= 0)
                {
                    m_gInputScoreBT.GetComponent<COnDownBeing>().SetDownBeingFalse();

                    SetButtonBt(m_gInputScoreBT, true);
                    StartBT_OnClick();

                }
            }
           
            
            CBottomBTOnclick._instance.m_bIsCompareFree = false;
            CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT, true);
            CPlayerInfo._instance.m_iBet = CPokerPointsManger._instance.m_iBasePoints;
            CMusicManger._instance.PlaySound("Bet");
            
           
        }
        /// <summary>
        /// 按住压分
        /// </summary>
        public void DownBeingInputScore()
        {
            InputScoreBt_OnClick();
        }
        /// <summary>
        /// 清楚按钮响应
        /// </summary>
        public void ClearBt_OnClick()
        {
            for (int i = 0; i < CPokerManger._instance.m_lCPokerList.Count; i++)
            {
                CPokerManger._instance.m_lCPokerList[i].m_bIsChecked = false;
            }
            SetButtonBt(m_gClearBT, true);

        }
        /// <summary>
        /// 自动开
        /// </summary>
        public void AutoOpenBT_OnClick()
        {
            SetAuto(true);
        }
        /// <summary>
        /// 自动关
        /// </summary>
        public void AutoCloseBT_OnClick()
        {
            SetAuto(false);
        }
        /// <summary>
        /// 音乐开
        /// </summary>
        public void MusicOpenBT_OnClick()
        {
            SetMusic(true);
        }
        /// <summary>
        /// 音乐关
        /// </summary>
        public void MusicCloseBT_OnClick()
        {
            SetMusic(false);
        }
        /// <summary>
        /// 得分按钮
        /// </summary>
        public void GetScoreBT_OnClick()
        {
            UIManger.Instance.RectGame();
            SetButtonBt(m_gGetScoreBT, true);
        }
        /// <summary>
        /// 比倍得分
        /// </summary>
        public void CompareGetScore_Onclick()
        {
            //圧大
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_GS);
            packet.AddInt(0);
            GameEngine.Instance.Send(packet);
        }
        /// <summary>
        /// 启动比倍
        /// </summary>
        public void Compare_Onclick()
        {
            CMusicManger._instance.PlaySound("CompareBT");
            m_gCompare.SetActive(true);
            m_bIsOpenComPare = true;
            CCompareManger._instance.ShowWindow();
            UIManger.Instance.m_bIsGetscore = false;
            if (!m_bIsCompareFree)
            {
                int tempIndex = 10 - UIManger.Instance.m_cUpdateCard.bCardType;
                int tempType = UIManger.Instance.m_iPeiLvType;
                if (CPlayerInfo._instance.m_iRoomTimes == 0) CPlayerInfo._instance.m_iRoomTimes = 1;
                int tempscore = (int)((UIManger.Instance.m_lAllScore - CPlayerInfo._instance.m_iGold) /(long) CPlayerInfo._instance.m_iRoomTimes);
                CCompareManger._instance.SetCompareCard(tempscore);
                CCompareManger._instance.SetCurrentCompare(0);
                CCompareManger._instance.SetHistoryCard();
                SetAllBTFalse();
                SetButtonBt(m_gBigBT, false);
                SetButtonBt(m_gLittleBT, false);
                SetButtonBt(m_gMoreThanBT, true);
            }
            else
            {
                
                if (CPlayerInfo._instance.m_iCreditNum >= CPokerPointsManger._instance.m_iBasePointBK && CPlayerInfo._instance.m_iCreditNum > 0)
                {
                    //空闲状态请求比倍
                    NPacket packet = NPacketPool.GetEnablePacket();
                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_FREE);
                    packet.AddInt(CPokerPointsManger._instance.m_iBasePointBK);
                    GameEngine.Instance.Send(packet);

                    SetAllBTFalse();
                    CPlayerInfo._instance.m_iGold -= CPokerPointsManger._instance.m_iBasePointBK * CPlayerInfo._instance.m_iRoomTimes;
                    CPlayerInfo._instance.m_iCreditNum = (int)(CPlayerInfo._instance.m_iGold / CPlayerInfo._instance.m_iRoomTimes);
                    
                }
                else
                {
                    CNomaney._instance.ShowWindow();
                }
            }
            CCompareManger._instance.m_gBig.SetActive(true);
            CCompareManger._instance.m_gBig.GetComponent<CBigSmallAnimation>().RectAnimarion();
            CCompareManger._instance.m_gSmall.SetActive(true);
            CCompareManger._instance.m_gSmall.GetComponent<CBigSmallAnimation>().RectAnimarion();
            CPushAnimation._instance.gameObject.SetActive(false);
            CCompareManger._instance.m_bIsWinPushDouble = false;
            UIManger.Instance.m_bIsPlayingGame = true;
        }
        /// <summary>
        /// 继续比倍
        /// </summary>
        public void Compare2_Onclick()
        {
            CMusicManger._instance.PlaySound("CompareBT");
            SetAllBTFalse();
            SetButtonBt(m_gBigBT, false);
            SetButtonBt(m_gLittleBT, false);
            SetButtonBt(m_gMoreThanBT2, true);
            CCompareManger._instance.m_bIsWinPushDouble = false;
            CCompareManger._instance.m_gBig.SetActive(true);
            CCompareManger._instance.m_gBig.GetComponent<CBigSmallAnimation>().RectAnimarion();
            CCompareManger._instance.m_gSmall.SetActive(true);
            CCompareManger._instance.m_gSmall.GetComponent<CBigSmallAnimation>().RectAnimarion();
            CPushAnimation._instance.gameObject.SetActive(false);
            UIManger.Instance.m_bIsPlayingGame = true;

        }
        public void BigBT_Onclick()
        {
            
            //圧大
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_XZ);
            packet.AddInt(2);

            GameEngine.Instance.Send(packet);
            SetButtonBt(m_gBigBT, true);
            SetButtonBt(m_gLittleBT, true);
        }
        public void LittleBT_Onclick()
        {
            //圧大
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_COMPARE_XZ);
            //压分
            packet.AddInt(1);
            GameEngine.Instance.Send(packet);

            SetButtonBt(m_gBigBT, true);
            SetButtonBt(m_gLittleBT, true);
        }
        /// <summary>
        /// 游戏记录
        /// </summary>
        public void RecordBT_Onclick()
        {
            if (m_bIsGetRecord)
            {
                //获取游戏记录
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_GET_RECORD);
                GameEngine.Instance.Send(packet);
                m_fWorkTime = 0;
                m_bIsGetRecord = false;

            }
            SetRecordShowHide(m_iCurrentIndex + 1);
        }
        public void CloseWindow()
        {
            if (UIManger.Instance.m_bIsPlayingGame )
            {
                CPlayingGame._instance.ShowWindow();
            }
            else
            {
                GameEngine.Instance.Quit();
            }
            
        }
        public void LittleWindow()
        {

        }
        /// <summary>
        /// 设置音乐
        /// </summary>
        /// <param name="_bIsMusic"></param>
        public void SetMusic(bool _bIsMusic)
        {
            if (CMusicManger._instance != null) CMusicManger._instance.m_bIsOpen = _bIsMusic;
            m_gMusicCloseBT.SetActive(_bIsMusic);
            m_gMusicOpenBT.SetActive(!_bIsMusic);
        }
        /// <summary>
        /// 设置自动开关
        /// </summary>
        /// <param name="_bIsAuto"></param>
        public void SetAuto(bool _bIsAuto)
        {
            m_gAutoCloseBT.SetActive(_bIsAuto);
            m_gAutoOpenBT.SetActive(!_bIsAuto);
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            SetButtonBt(m_gClearBT, true);
            SetButtonBt(m_gBigBT, true);
            SetButtonBt(m_gGetScoreBT, true);
            SetButtonBt(m_gLittleBT, true);
            SetButtonBt(m_gMoreThanBT, true);
            SetButtonBt(m_gInputScoreBT, false);
            SetButtonBt(m_gStartBT, false);
            SetButtonBt(m_gGetScoreBT1, true);
            SetButtonBt(m_gMoreThanBT2, true);

        }

        public void SetAllBTFalse()
        {
            SetButtonBt(m_gClearBT, true);
            SetButtonBt(m_gBigBT, true);
            SetButtonBt(m_gGetScoreBT, true);
            SetButtonBt(m_gLittleBT, true);
            SetButtonBt(m_gMoreThanBT, true);
            SetButtonBt(m_gInputScoreBT, true);
            SetButtonBt(m_gStartBT, true);
            SetButtonBt(m_gGetScoreBT1, true);
            SetButtonBt(m_gMoreThanBT2, true);
        }
        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="_bIsDisable">是否禁用</param>
        public void SetButtonBt(GameObject _ButtonGameObject, bool _bIsDisable)
        {
            if (_bIsDisable)
            {
                _ButtonGameObject.GetComponent<UIButton>().isEnabled = false;

            }
            else
            {
                _ButtonGameObject.GetComponent<UIButton>().isEnabled = true;
            }

        }

        public void SetRecordShowHide(int _iIndex)
        {
            m_iCurrentIndex = _iIndex;

            if (m_iCurrentIndex > m_lRecordManger.Count) m_iCurrentIndex = 0;

#if !UNITY_STANDALONE_WIN
           
            CRecordMangerOne._instance.HideWindow();
            CRecordMangerTwo._instance.HideWindow();
            CRecordMangerThree._instance.HideWindow();
#else
            CRecordMangerOne._instance.HideWindow();
            CRecordMangerTwo._instance.HideWindow();
            CRecordMangerThree._instance.HideWindow();
#endif

            if (m_iCurrentIndex < m_lRecordManger.Count)
            {
                if (m_lRecordManger[m_iCurrentIndex].gameObject == CRecordMangerOne._instance.gameObject)
                {
                    CRecordMangerOne._instance.ShowWindow();
                }
                else if (m_lRecordManger[m_iCurrentIndex].gameObject == CRecordMangerTwo._instance.gameObject)
                {
                    CRecordMangerTwo._instance.ShowWindow();
                }
                else if (m_lRecordManger[m_iCurrentIndex].gameObject == CRecordMangerThree._instance.gameObject)
                {
                    CRecordMangerThree._instance.ShowWindow();
                }
            }
        }
    }
}