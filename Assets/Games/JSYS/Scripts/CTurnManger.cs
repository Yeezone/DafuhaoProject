using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.JSYS
{
    public class CTurnManger : MonoBehaviour
    {

        public static CTurnManger _instance;
        /// <summary>
        /// 动物列表
        /// </summary>
        public List<CTurnItem> m_lTurnList = new List<CTurnItem>();

        /// <summary>
        /// 目标位置
        /// </summary>
        public int m_iTargetIndex = 0;
        /// <summary>
        /// 当前位置
        /// </summary>
        public int m_iCurrentIndex = 0;
        /// <summary>
        /// 加速度
        /// </summary>
        public float m_fAddSpeed = 0.01f;
        /// <summary>
        /// 控制速度
        /// </summary>
        public float m_fLimitSpeed = 1.0f;

        /// <summary>
        /// 最大速度
        /// </summary>
        public float m_fMaxSpeed = 0.02f;
        /// <summary>
        /// 启动转盘
        /// </summary>
        public bool m_bIsOpen = false;
        /// <summary>
        /// 速度（此值越大转盘越慢）
        /// </summary>
        public float m_fSpeed = 0.2f;
        /// <summary>
        /// 旋转时间
        /// </summary>
        private float m_fWorkTime = 0;
        /// <summary>
        /// 跟随设置
        /// </summary>
        public int m_iFollowNum = 0;

        /// <summary>
        /// 跟随加速度
        /// </summary>
        public int m_iAddForllowNum = 3;
        /// <summary>
        /// 最大跟随数
        /// </summary>
        public int m_iMaxForllowNum = 5;

        public int m_iIndexSpeed = 1;

        public float m_fMidSpeed = 0.2f;

        public int m_iTurnNum = 0;

        public int m_iFirstIndex = 0;

        private bool m_bIsAdd = true;

        public int m_iSpeedTurnNum = 0;
        public int m_iSpeedTurn = 3;

        public int m_inum = 0;

        public int m_iTurnCount = 0;
        public int m_iTurnTargetCount = 0;
        public int m_iReduceSpeedCount = 5;

        public int m_iLimitCount = 5;
        public float m_fReduceSpeed = 0.006f;

        /// <summary>
        /// 中间动物
        /// </summary>
        public CWindowShowHide_JSYS m_CenterAnimal;
        public CAnimation m_cStarAnimation;
        public GameObject m_cAnimalAnimation;
        public List<CLabelNum_JSYS> m_cTimeList = new List<CLabelNum_JSYS>();

        public int ss = 0;

        public List<UIAtlas> m_lAtlas = new List<UIAtlas>();

        public bool m_bIsClockWise = true;
        public long m_lWinScore = 0;

        public CWindowShowHide_JSYS m_cFree;

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
            SetAllNomal();
            SetCurrentIndex();
        }

        // Update is called once per frame
        void Update()
        {
            m_fWorkTime += Time.deltaTime;
            if (m_bIsOpen && m_fWorkTime >= m_fSpeed)
            {

                m_iIndexSpeed = 1;
                if (m_bIsClockWise)
                {
                    m_iCurrentIndex -= 1;
                }
                else
                {
                    m_iCurrentIndex += 1;
                }
                if (!m_bIsClockWise && m_iCurrentIndex >= m_lTurnList.Count)
                {
                    m_iCurrentIndex = 0;
                }
                else if (m_bIsClockWise && m_iCurrentIndex < 0)
                {
                    m_iCurrentIndex = m_lTurnList.Count - 1;
                }

             //   if (m_iCurrentIndex == m_iFirstIndex) m_iTurnCount += 1;

                if (m_iCurrentIndex == m_iTargetIndex)
                {
                    m_iTurnTargetCount += 1;
                    m_iTurnCount += 1;
                    if (m_iTurnTargetCount == m_iReduceSpeedCount)
                    {
                        m_inum = 0;
                    }
                }
                m_inum += 1;
                if (m_iTurnTargetCount == m_iReduceSpeedCount && m_inum >= 10) m_bIsAdd = false;

               
                m_iSpeedTurnNum += 1;
                if (m_bIsAdd)
                {
                    if (m_iSpeedTurn <= m_iSpeedTurnNum)
                    {
                        m_fSpeed -= m_fAddSpeed;
                        if (m_fSpeed <= m_fMaxSpeed) m_fSpeed = m_fMaxSpeed;
                        if (m_fSpeed <= m_fMidSpeed) m_iIndexSpeed = 1;
                        m_iSpeedTurnNum = 0;
                    }
                }
                else
                {
                    if (12<= m_iSpeedTurnNum)
                    {
                        m_fSpeed += 0.047f;
                        
                    }
                    if (m_fSpeed >= m_fMidSpeed) m_iIndexSpeed = 1;
                    if (m_iTurnCount >= m_iLimitCount && m_iCurrentIndex == m_iTargetIndex)
                    {
                        m_bIsOpen = false;

                    }
                }

                m_fWorkTime = 0;

                //跟随设置
                m_iTurnNum += 1;
                if (m_iTurnNum >= m_iAddForllowNum)
                {
                    if (m_bIsAdd)
                    {
                        m_iFollowNum += 1;
                        if (m_iFollowNum >= m_iMaxForllowNum) m_iFollowNum = m_iMaxForllowNum;
                    }
                    else
                    {
                        m_iFollowNum -= 1;
                        if (m_iFollowNum <= 0) m_iFollowNum = 0;
                    }
                    m_iTurnNum = 0;
                }
                SetCurrentIndex();
               // CMusicManger_JSYS._instance.PlayTimerSound();
                CMusicManger_JSYS._instance.PlaySound("5Second");
                if (!m_bIsOpen)
                {
                   // OpenReward();
                    StartCoroutine(WaitTimeOpenReward());
                }
            }


        }


        IEnumerator WaitTimeOpenReward()
        {
            yield return new WaitForSeconds(0.01f);
            OpenReward();
        }
        public void SetCurrentIndex()
        {
            SetAllNomal();
            Color _color = m_lTurnList[m_iCurrentIndex].m_gAnimationBK.GetComponent<UISprite>().color;
            _color.a = 1;
            m_lTurnList[m_iCurrentIndex].m_gAnimationBK.GetComponent<UISprite>().color = _color;
            for (int i = 0; i < m_iFollowNum; i++)
            {
                int _iIndex = m_iCurrentIndex;
                if (m_bIsClockWise)
                {
                    _iIndex = m_iCurrentIndex + i + 1;
                    if (_iIndex >= m_lTurnList.Count) _iIndex -= m_lTurnList.Count;
                }
                else
                {
                    _iIndex = m_iCurrentIndex - i - 1;
                    if (_iIndex < 0) _iIndex += m_lTurnList.Count;
                }

                _color = m_lTurnList[_iIndex].m_gAnimationBK.GetComponent<UISprite>().color;
                _color.a = 1.0f - (float)(i + 1) * 0.15f;
                m_lTurnList[_iIndex].m_gAnimationBK.GetComponent<UISprite>().color = _color;
            }


        }
        /// <summary>
        /// 把所有动物设置为常态
        /// </summary>
        public void SetAllNomal()
        {
            for (int i = 0; i < m_lTurnList.Count; i++)
            {
                //m_lTurnList[i].m_gAnimation.SetActive(false);
                m_lTurnList[i].m_gAnimationBK.SetActive(true);
                Color _color = m_lTurnList[i].m_gAnimationBK.GetComponent<UISprite>().color;
                _color.a = 0;
                m_lTurnList[i].m_gAnimationBK.GetComponent<UISprite>().color = _color;
                m_lTurnList[i].m_gTexture.SetActive(true);
            }
        }


        public void OpenAniamation(int _iTarget)
        {
            m_iTargetIndex = _iTarget;
            m_bIsOpen = true;
            m_fWorkTime = 0;
            m_iFirstIndex = m_iCurrentIndex;
            m_iTurnCount = 0;
            m_iTurnNum = 0;
            m_bIsAdd = true;
            m_iSpeedTurnNum = 0;
            m_fSpeed = 0.4f;
            m_iTurnTargetCount = 0;
            m_fReduceSpeed = 0.008f;

        }
        /// <summary>
        /// 开奖
        /// </summary>
        public void OpenReward()
        {
            CBackGroundManger._instance.m_bIsGameStatus = false;
            CBETManger._instance.m_cBetTop.m_bIsRandomPraize = false;
            CTurnManger._instance.m_cFree.HideWindow();
            CenterAnimation(m_lTurnList[m_iCurrentIndex].m_iAniamlID);
            //m_lTurnList[m_iCurrentIndex].m_gAnimation.SetActive(true);
            //m_lTurnList[m_iCurrentIndex].m_gTexture.SetActive(false);
            for (int i = 0; i < m_cTimeList.Count; i++)
            {
                m_cTimeList[i].m_iNum = CBETManger._instance.GetMulitTimes(m_lTurnList[m_iCurrentIndex].m_iAniamlID);
            }
            CBETManger._instance.SetDisable(m_lTurnList[m_iCurrentIndex].m_iAniamlID);
            CRecordManger._instance.AddRecord(m_lTurnList[m_iCurrentIndex].m_iAniamlID + 1);
            if (m_lTurnList[m_iCurrentIndex].m_iAniamlID < 4)
            {
                   CBackGroundManger._instance.SetBackGround(1);
            }
            else if (m_lTurnList[m_iCurrentIndex].m_iAniamlID >= 4 && m_lTurnList[m_iCurrentIndex].m_iAniamlID < 8)
            {
                CBackGroundManger._instance.SetBackGround(2);
            }
            else
            {
                CBackGroundManger._instance.SetBackGround(0);
            }
            StartCoroutine(WaitPickUpBet());
            CBETManger._instance.m_cBetTop.m_cGetscore.m_iNum = m_lWinScore;
            Debug.Log("<color=green>" + m_lWinScore.ToString() + "</color>");
            //CBETManger._instance.OpenGetScore(2.0f);

            string str = "Animal"+m_lTurnList[m_iCurrentIndex].m_iAniamlID.ToString();
            CMusicManger_JSYS._instance.PlaySound(str);

            StartCoroutine(WaitTimeExit());
            StartCoroutine(CBETManger._instance.WaitPlayerPrize());
           
        }
        IEnumerator WaitPickUpBet()
        {
            yield return new WaitForSeconds(5.0f);
            CBETManger._instance.m_cMoveBet.PackUp_Onclick();
        }
        /// <summary>
        /// 中间动画出现
        /// </summary>
        public void CenterAnimation(int _iAnimalIndex)
        {
            m_CenterAnimal.ShowWindow();

       
            m_cAnimalAnimation.GetComponent<UISprite>().spriteName = "che" + (_iAnimalIndex+1).ToString();
            ////走兽
            //if (_iAnimalIndex <= 3)
            //{
            //    m_cAnimalAnimation.GetComponent<UISprite>().atlas = m_lAtlas[0];
            //    m_cAnimalAnimation.GetComponent<UISprite>().name = "che" + _iAnimalIndex.ToString();
            //}
            ////飞禽
            //else if (_iAnimalIndex > 3 && _iAnimalIndex <= 7)
            //{
            //    m_cAnimalAnimation.GetComponent<UISprite>().atlas = m_lAtlas[1];
            //    m_cAnimalAnimation.m_strTextureName = "Win_Big_" + _iAnimalIndex.ToString() + "_";
            //}
            ////鲨鱼
            //else
            //{
            //    m_cAnimalAnimation.GetComponent<UISprite>().atlas = m_lAtlas[2];
            //    m_cAnimalAnimation.m_strTextureName = "Win_Big_" + _iAnimalIndex.ToString() + "_";
            //}
        }

        public void StartAnimation()
        {
            m_cStarAnimation.RecetAnimation();
            m_cStarAnimation.Play();
        }

        public void HideCenterAnimation()
        {
            TweenAlpha temp_Alpha = m_CenterAnimal.GetComponent<TweenAlpha>();
            temp_Alpha.from = 1.0f;
            temp_Alpha.to = 0;
            temp_Alpha.ResetToBeginning();
            temp_Alpha.enabled = true;
        }
        IEnumerator WaitTimeExit()
        {
            yield return new WaitForSeconds(3.0f);
            if (CBETManger._instance.m_cBetTop.m_cGameGold.m_iNum <= 0)
            {
                CGameEngine.Instance.Quit();
            }
        }

    }
}