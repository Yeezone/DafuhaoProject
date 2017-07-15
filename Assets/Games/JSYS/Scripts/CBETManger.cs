using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;
using System.Runtime.InteropServices;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.JSYS
{
    public class CBETManger : MonoBehaviour
    {
        public static CBETManger _instance;
        /// <summary>
        /// 下注按钮列表
        /// </summary>
        public List<CBETItem> m_listBetItem = new List<CBETItem>();

        /// <summary>
        /// 计时器
        /// </summary>
        public CLabelNum_JSYS m_cTimer;

        /// <summary>
        /// 当前筹码
        /// </summary>
        public long m_lCurrentChip = 0;

        /// <summary>
        /// 帮助窗口
        /// </summary>
        public CWindowShowHide_JSYS m_cHelpWindow;

        /// <summary>
        /// 移动下注面板
        /// </summary>
        public CMoveBet m_cMoveBet;

        public GameObject m_gUpBT;
        public GameObject m_gDownBT;

        /// <summary>
        /// 是否是下注时间
        /// </summary>
        public bool m_bIsBetTime = true;

        [System.Serializable]
        public class CChip
        {
            /// <summary>
            /// 筹码列表
            /// </summary>
            public List<long> m_lChipList = new List<long>();
            /// <summary>
            /// 当前筹码下标
            /// </summary>
            public int m_iCurrentIndex = 0;

            public UISprite m_sSpriteTexture;
            public CLabelNum_JSYS m_cNum;
        }

        //提示
        [System.Serializable]
        public class CTiShi
        {
            public List<GameObject> m_listTiShi = new List<GameObject>();
            public int m_iMaxNum = 5;
            public int m_iCurrentNum = 0;
            public GameObject m_gTiShiPrefab;
            public GameObject m_gParent;

            public Vector3 m_vStarPos = new Vector3(0, 0, 0);
            public Vector3 m_vEndPos = new Vector3(0, 0, 0);
        }
        public CTiShi m_TiShiManger = new CTiShi();
        public CChip m_cChipManger = new CChip();


        /// <summary>
        /// 下注面板顶部信息
        /// </summary>
        [System.Serializable]
        public class CBetTop
        {
            /// <summary>
            /// 得分
            /// </summary>
            public CLabelNum_JSYS m_cGetscore;
            /// <summary>
            /// 彩金
            /// </summary>
            public CLabelNum_JSYS m_cCaiJin;
            public long m_lPrzeNum = 10000;
            public bool m_bIsRandomPraize = false;

            public bool m_bIsPrize = false;
            public long m_lPrize = 0;
            public ushort m_iChairID = 0;
            public ushort m_iMyChairID = 0;
            public string m_strNickName = "";
            /// <summary>
            /// 游戏币
            /// </summary>
            public CLabelNum_JSYS m_cGameGold;

            /// <summary>
            /// 倍率
            /// </summary>
            public CLabelNum_JSYS m_cMulitly;
            /// <summary>
            /// 彩金中奖
            /// </summary>
            public GameObject m_gPrizePool;
            public CWindowShowHide_JSYS m_cPrizePoolWindow;

            public float m_fWorkTime = 0;

        }
        /// <summary>
        /// 是否开始得分
        /// </summary>
        public bool m_bIsGetScore = false;
        public long m_lGetScoreSpeed = 1;
        public CBetTop m_cBetTop = new CBetTop();

        //
        /// <summary>
        /// 设置
        /// </summary>
        public CWindowShowHide_JSYS m_cSet;
        void Awake()
        {
            _instance = this;
        }

        void OnDestroy()
        {
            _instance = null;
        }
        // Use this for initialization
        void Start()
        {
#if !UNITY_STANDALONE_WIN
            m_gUpBT.SetActive(true);
            m_gDownBT.SetActive(false);
#endif
        }

        // Update is called once per frame
        void Update()
        {
            GetScoreAnimation();
            m_cBetTop.m_fWorkTime += Time.deltaTime;
            if (m_cBetTop.m_fWorkTime >= 0.3f && m_cBetTop.m_bIsRandomPraize)
            {
                m_cBetTop.m_fWorkTime = 0;
                PrizeGoldControl();
            }
        }
        /// <summary>
        /// 下注
        /// </summary>
        public void DownBET(CBETItem _BetItem)
        {
            if (CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum > m_lCurrentChip)
            {

                if (m_bIsBetTime)
                {
                    CMusicManger_JSYS._instance.PlaySound("InputScore");
                    //获取游戏记录
                    NPacket packet = NPacketPool.GetEnablePacket();
                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_PLAY_BET);
                    CMD_C_PlayBet playBet;
                    playBet.nAnimalIndex = _BetItem.m_iBetID;
                    playBet.lBetChip = m_lCurrentChip;
                    byte[] dataBuffer = GameConvert.StructToByteArray<CMD_C_PlayBet>(playBet);
                    int len = Marshal.SizeOf(typeof(CMD_C_PlayBet));
                    packet.AddBytes(dataBuffer, len);
                    CGameEngine.Instance.Send(packet);

                }
                else
                {
                    AddTiShi("非下注时间无法下注");
                }
            }
            else
            {
                AddTiShi("金币不足，请充值");
            }
        }
        /// <summary>
        /// 设置下注
        /// </summary>
        /// <param name="_iBetID">动物ID</param>
        /// <param name="_iChip">其他玩家下注</param>
        /// <param name="_iMyBet">自己下注</param>
        public void BetReturnSet(int _iBetID, long _iChip, long _iMyBet)
        {
            for (int i = 0; i < m_listBetItem.Count; i++)
            {
                if (m_listBetItem[i].m_iBetID == _iBetID)
                {
                    m_listBetItem[i].m_lAllBet += _iChip;
                    m_listBetItem[i].m_lMyBet += _iMyBet;
                }
            }
        }

        /// <summary>
        /// 获取倍数
        /// </summary>
        /// <param name="_iBetID"></param>
        /// <returns></returns>
        public long GetMulitTimes(int _iBetID)
        {
            for (int i = 0; i < m_listBetItem.Count; i++)
            {
                if (m_listBetItem[i].m_iBetID == _iBetID)
                {
                    return m_listBetItem[i].m_cMulitTimes.m_iNum;
                }
            }
            return 0;
        }
        /// <summary>
        /// 帮助页面加载
        /// </summary>
        public void HelpBT_Onclick()
        {
            m_cHelpWindow.ShowWindow();
        }
        
        public void ChipBT_Onclick()
        {
            CMusicManger_JSYS._instance.PlaySound("InputScore");
            SetCurrentChip(m_cChipManger.m_iCurrentIndex + 1);
        }
        /// <summary>
        /// 设置当前筹码
        /// </summary>
        /// <param name="_iIndex">当前筹码下标</param>
        public void SetCurrentChip(int _iIndex)
        {
            m_cChipManger.m_iCurrentIndex = _iIndex;

            if (m_cChipManger.m_iCurrentIndex >= m_cChipManger.m_lChipList.Count) m_cChipManger.m_iCurrentIndex = 0;

            m_lCurrentChip = m_cChipManger.m_lChipList[m_cChipManger.m_iCurrentIndex];
            m_cChipManger.m_sSpriteTexture.spriteName = "chipBL" + m_cChipManger.m_iCurrentIndex.ToString();
            m_cChipManger.m_sSpriteTexture.GetComponent<UIButton>().normalSprite = "chipBL" + m_cChipManger.m_iCurrentIndex.ToString();

            m_cChipManger.m_cNum.m_iNum = m_cChipManger.m_lChipList[m_cChipManger.m_iCurrentIndex];
        }
        /// <summary>
        /// 禁用下注按钮
        /// </summary>
        /// <param name="_iAnimalID"></param>
        public void SetDisable(int _iAnimalID)
        {
            for (int i = 0; i < m_listBetItem.Count; i++)
            {
                m_listBetItem[i].SetDisable(false);
                int temp_iAnimalID = m_listBetItem[i].m_iBetID;
                ////走兽
                //if (_iAnimalID < 4 && temp_iAnimalID == 11)
                //{
                //    m_listBetItem[i].SetRewardAnimal(true);
                //    continue;
                //}
                ////飞禽
                //else if (_iAnimalID <= 7 && _iAnimalID >= 4 && temp_iAnimalID == 10)
                //{
                //    m_listBetItem[i].SetRewardAnimal(true);
                //    continue;
                //}

                if (_iAnimalID == temp_iAnimalID)
                {
                    m_listBetItem[i].SetRewardAnimal(true);
                    continue;
                }
                m_listBetItem[i].SetRewardAnimal(false);
                m_listBetItem[i].SetDisableColor();

            }
        }
        /// <summary>
        /// 添加提示
        /// </summary>
        public void AddTiShi(string _str)
        {
            GameObject tempobj;
            if (m_TiShiManger.m_listTiShi.Count >= m_TiShiManger.m_iMaxNum)
            {
                tempobj = m_TiShiManger.m_listTiShi[0].gameObject;
                m_TiShiManger.m_listTiShi.Remove(m_TiShiManger.m_listTiShi[0]);
            }
            else
            {
                tempobj = (GameObject)Instantiate(m_TiShiManger.m_gTiShiPrefab, new Vector3(0, 0, 0), m_TiShiManger.m_gTiShiPrefab.transform.rotation);
            }
            tempobj.transform.parent = m_TiShiManger.m_gParent.transform;
            tempobj.transform.localScale = new Vector3(1, 1, 1);
            tempobj.transform.GetComponent<UILabel>().text = _str;
            tempobj.GetComponent<TweenPosition>().from = m_TiShiManger.m_vStarPos;
            tempobj.GetComponent<TweenPosition>().to = m_TiShiManger.m_vEndPos;
            tempobj.GetComponent<TweenPosition>().enabled = true;
            tempobj.GetComponent<TweenPosition>().ResetToBeginning();

            tempobj.GetComponent<TweenAlpha>().from = 1.0f;
            tempobj.GetComponent<TweenAlpha>().to = 0;
            tempobj.GetComponent<TweenAlpha>().enabled = true;
            tempobj.GetComponent<TweenAlpha>().ResetToBeginning();
            m_TiShiManger.m_listTiShi.Add(tempobj);
        }

        /// <summary>
        /// 得分动画
        /// </summary>
        public void GetScoreAnimation()
        {
            if (m_bIsGetScore)
            {
                if (m_cBetTop.m_cGetscore.m_iNum <= m_lGetScoreSpeed) m_lGetScoreSpeed = m_cBetTop.m_cGetscore.m_iNum;
                m_cBetTop.m_cGetscore.m_iNum -= m_lGetScoreSpeed;
                if (m_cBetTop.m_cGetscore.m_iNum <= 0)
                {
                    m_bIsGetScore = false;
                    m_cBetTop.m_cGetscore.m_iNum = 0;
                }
                m_cBetTop.m_cGameGold.m_iNum += m_lGetScoreSpeed;
            }
        }
        /// <summary>
        /// 启动得分动画
        /// </summary>
        /// <param name="_fAllTime">动画播放时间</param>
        public void OpenGetScore(float _fAllTime)
        {

            int temp_iSpeed = (int)(_fAllTime / Time.deltaTime);
            m_lGetScoreSpeed = m_cBetTop.m_cGetscore.m_iNum / temp_iSpeed;
            m_bIsGetScore = true;
        }

        /// <summary>
        /// 续压
        /// </summary>
        public void ContinueBET()
        {
            if (CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum > 0)
            {
                if (m_bIsBetTime)
                {
                    CMusicManger_JSYS._instance.PlaySound("InputScore");
                    //发送续压
                    NPacket packet = NPacketPool.GetEnablePacket();
                    packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_CONTINUE_BET);
                    CGameEngine.Instance.Send(packet);

                }
                else
                {
                    AddTiShi("非下注时间无法下注");
                }
            }
            else
            {
                AddTiShi("金币不足，请充值");
            }
        }

        //清除
        public void ClearBT_OnClick()
        {
            if (m_bIsBetTime)
            {
                //获取游戏记录
                NPacket packet = NPacketPool.GetEnablePacket();
                packet.CreateHead(MainCmd.MDM_GF_GAME, SubCmd.SUB_C_BET_CLEAR);
                packet.AddInt(0);
                CGameEngine.Instance.Send(packet);
            }
            else
            {
                AddTiShi("非下注时间无法下注");
            }
        }
        /// <summary>
        /// 重置下注
        /// </summary>
        public void RectBet()
        {
            for (int i = 0; i < m_listBetItem.Count; i++)
            {
                m_listBetItem[i].SetNormalColor();
                m_listBetItem[i].SetDisable(true);
                m_listBetItem[i].m_lMyBet = 0;
                m_listBetItem[i].m_lAllBet = 0;
                m_listBetItem[i].SetRewardAnimal(false);
            }
        }

        //显示设置
        public void SetBt_OnClick()
        {
            m_cSet.ShowWindow();
        }
        //关闭设置
        public void CloseSet_OnClick()
        {
            m_cSet.HideWindow();
        }
        //彩金随机变化
        public void PrizeGoldControl()
        {
            int rand = Random.Range(10, 12);
            int randnum = Random.Range(5, 21);

            m_cBetTop.m_cCaiJin.m_iNum = m_cBetTop.m_lPrzeNum * rand * randnum / 100;
        }
        //等待播放彩金中奖效果
        public IEnumerator WaitPlayerPrize()
        {
            yield return new WaitForSeconds(2.0f);
 
            if (m_cBetTop.m_bIsPrize)
            {
                string strNickName = m_cBetTop.m_strNickName;
                if (m_cBetTop.m_iChairID == m_cBetTop.m_iMyChairID && m_cBetTop.m_lPrize > 0)
                {
                    CBETManger._instance.m_cBetTop.m_gPrizePool.GetComponent<UILabel>().text =
                        "[00FF00]恭喜[-][FF0000]您[-]获得[FF0000]" + m_cBetTop.m_lPrize.ToString() + "[-][00FF00]彩金[-]";
                    CBETManger._instance.m_cBetTop.m_cPrizePoolWindow.ShowWindow();
                    if (m_cBetTop.m_cMulitly.m_iNum != 0)
                    m_cBetTop.m_cGameGold.m_iNum += m_cBetTop.m_lPrize / m_cBetTop.m_cMulitly.m_iNum;
                    StartCoroutine(WaitTimePrizePoolHide(3.0f));
                }
                else if (strNickName != null && m_cBetTop.m_lPrize > 0)
                {
                    CBETManger._instance.m_cBetTop.m_gPrizePool.GetComponent<UILabel>().text =
                        "[00FF00]恭喜[-][FF0000]" + strNickName + "[-]获得[FF0000]" + m_cBetTop.m_lPrize.ToString() + "[-][00FF00]彩金[-]";
                    CBETManger._instance.m_cBetTop.m_cPrizePoolWindow.ShowWindow();

                    StartCoroutine(WaitTimePrizePoolHide(3.0f));
                }
                CBETManger._instance.m_cBetTop.m_cCaiJin.m_iNum -= m_cBetTop.m_lPrize;
            }
        }
        //等待隐藏彩金中奖效果
        IEnumerator WaitTimePrizePoolHide(float _ftime)
        {
            yield return new WaitForSeconds(_ftime);
            CBETManger._instance.m_cBetTop.m_cPrizePoolWindow.HideWindow();
            m_cBetTop.m_lPrize = 0;
        }
    }

    public class TiShiItem
    {
        public GameObject m_gTishi;
        public bool m_bIsMove;
    }
}
