using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.ATT
{
    public class CPokerPointsManger : MonoBehaviour
    {
        public static CPokerPointsManger _instance;
        /// <summary>
        /// 分数列表
        /// </summary>
        public List<CPokerPoints> m_lPointsList = new List<CPokerPoints>();

        public Color m_cRed;
        public Color m_cYellow;

        /// <summary>
        /// 每个数字显示时间
        /// </summary>
        public float m_fPerNumTime;
        /// <summary>
        /// 计时器
        /// </summary>
        public float m_fWorkTime;

        public int m_iIndex = 0;
        public int m_iLuckIndex = 0;
        /// <summary>
        /// 速度
        /// </summary>
        public int m_iSpeed;

        public bool m_bIsOpen = false;

        /// <summary>
        /// 基础分
        /// </summary>
        private int _iBasePoints;
        public int m_iBasePoints
        {
            get { return _iBasePoints; }
            set
            {
                _iBasePoints = value;
                SetPockerPoints(_iBasePoints);
            }
        }

        public int m_iBasePointBK = 0;

        /// <summary>
        /// 是否显示了窗口
        /// </summary>
        public bool m_bIsShow = false;

        public Color m_ColorBlue = Color.blue;
        public Color m_cColorRed = Color.red;

        public float m_fWorkTimeColor = 0;
        public float m_fSpeedColor = 0.2f;
        public int m_inumcolor = 0;

        public int[] m_iIndexColor = new int[10];

        private bool _bIsChangeColor = false;
        public bool m_bIsChangeColor
        {
            get { return _bIsChangeColor; }
            set
            {
                _bIsChangeColor = value;
                m_fWorkTimeColor = 0;
                if (_bIsChangeColor == false)
                {
                    for (int i = 0; i < m_iIndexColor.Length; i++)
                    {
                        if (m_iIndexColor[i] == 1)
                        {
                           ColorControl(i, m_ColorBlue);
                        }
                    }
                    
                }
            }
        }


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
            SetPockerPoints(1);

        }

        // Update is called once per frame
        void Update()
        {
            GetScoreAnimation();

            if (_bIsChangeColor && m_fWorkTimeColor >= m_fSpeedColor)
            {
                m_fWorkTimeColor = 0;
                if (m_inumcolor == 0)
                {
                    for (int i = 0; i < m_iIndexColor.Length; i++)
                    {
                        if (m_iIndexColor[i] == 1)
                        {
                            ColorControl(i, m_ColorBlue);
                        }
                    }
                    m_inumcolor = 1;
                }
                else
                {
                    for (int i = 0; i < m_iIndexColor.Length; i++)
                    {
                        if (m_iIndexColor[i] == 1)
                        {
                            ColorControl(i, m_cColorRed);
                        }
                    }
                    m_inumcolor = 0;
                }

            }
            m_fWorkTimeColor += Time.deltaTime;
        }
        /// <summary>
        /// 设置扑克分数
        /// </summary>
        /// <param name="_IBasePoints"></param>
        public void SetPockerPoints(int _iPoints)
        {
            for (int i = 0; i < m_lPointsList.Count; i++)
            {
                for (int j = 0; j < m_lPointsList[i].m_lLabelList.Count; j++)
                {
                    m_lPointsList[i].SetPoint(j, _iPoints, m_cYellow);
                }

            }

            if (_iPoints < 80)
            {
                int times = (int)(_iPoints / 5);
                CPlayerInfo._instance.m_gLabel80nums.GetComponent<CLabelNum>().m_iNum = times * 5;
            }
            else
            {
                CPlayerInfo._instance.m_gLabel80nums.GetComponent<CLabelNum>().m_iNum = 80;
            }
        }

        /// <summary>
        /// 设置中奖数字颜色
        /// </summary>
        /// <param name="_iLuckyTimesIndex"></param>
        public void SetLuckyTiemsColor(int _iLuckyTimesIndex)
        {
            for (int i = 0; i < m_lPointsList.Count; i++)
            {
                m_lPointsList[i].m_lLabelList[_iLuckyTimesIndex].GetComponent<CLabelNum>().m_cColor = m_cRed;
            }
        }
        /// <summary>
        /// 得分动画
        /// </summary>
        public void GetScoreAnimation()
        {
           

            if (m_fPerNumTime <= m_fWorkTime && m_bIsOpen)
            { 
                int point = 0;
                for (int i = m_lPointsList.Count-1; i >= 0; i--)
                {
                    if (m_lPointsList[i].m_lPointList[m_iLuckIndex] > 0 && m_iIndexColor[i] ==1)
                    {
                        if (m_lPointsList[i].m_lPointList[m_iLuckIndex] <= m_iSpeed)
                        {
                            CPlayerInfo._instance.m_iCreditNum += m_lPointsList[i].m_lPointList[m_iLuckIndex];
                            CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_lPointsList[i].m_lPointList[m_iLuckIndex];
                            point = m_lPointsList[i].m_lPointList[m_iLuckIndex] = 0;
                        }
                        else
                        {
                             m_lPointsList[i].m_lPointList[m_iLuckIndex] -= m_iSpeed;
                             if (point < m_lPointsList[i].m_lPointList[m_iLuckIndex]) point = m_lPointsList[i].m_lPointList[m_iLuckIndex];
                             CPlayerInfo._instance.m_iCreditNum += m_iSpeed;
                             CPlayerInfo._instance.m_iGold += CPlayerInfo._instance.m_iRoomTimes * m_iSpeed;
                        }
                        
                        if (m_lPointsList[i].m_lPointList[m_iLuckIndex] <= 0) m_lPointsList[i].m_lPointList[m_iLuckIndex] = 0;
                        m_lPointsList[i].SetPointText(m_iLuckIndex, m_lPointsList[i].m_lPointList[m_iLuckIndex], m_cRed);
                        
                    }
                }
                CMusicManger._instance.PlaySound("di");
                if (point == 0)
                {
                    m_bIsOpen = false;

                    CPokerPointsManger._instance.m_bIsChangeColor = false;
                    UIManger.Instance.RectGame();
                }
                m_fWorkTime = 0;
            }

            m_fWorkTime += Time.deltaTime;
        }
        /// <summary>
        /// 设置得分动画
        /// </summary>
        /// <param name="_bIsOpen"></param>
        /// <param name="_iCardType"></param>
        /// <param name="_iPeiLvType"></param>
        /// <param name="_iSpeed"></param>
        public void SetScoreAnimation(bool _bIsOpen, int _iCardType, int _iPeiLvType, int _iSpeed)
        {
            //设置手机游戏显示界面
#if !UNITY_STANDALONE_WIN
            UIManger.Instance.HideAllWindow();
            CPokerPointsManger._instance.ShowWindow();
            CBasePoint._instance.ShowWindow();
#else

#endif
            m_iSpeed = _iSpeed;
            m_bIsOpen = _bIsOpen;
            m_iIndex = 10 - _iCardType;
            m_iLuckIndex = _iPeiLvType;
            m_fPerNumTime = 0.002f;
            m_fWorkTime = 0;

        }
        /// <summary>
        /// 设置分数
        /// </summary>
        /// <param name="_iCardType"></param>
        /// <param name="_iPeiLvType"></param>
        /// <returns></returns>
        public void SetScoreZero(int _iCardType, int _iPeiLvType)
        {
            for (int i = 0; i < m_lPointsList.Count; i++)
            {

                for (int j = 0; j < m_lPointsList[i].m_lLabelList.Count; j++)
                {
                    if (j == _iPeiLvType && m_iIndexColor[i] == 1)
                    {

                    }
                    else
                    {
                      m_lPointsList[i].m_lPointList[j] = 0;
                      m_lPointsList[i].m_lLabelList[j].GetComponent<CLabelNum>().m_iNum = 0;
                    }
                }

            }

        }
        /// <summary>
        /// 重置分数
        /// </summary>
        public void RectPoint()
        {
            CPokerPointsManger._instance.m_bIsChangeColor = false;
            m_bIsOpen = false;
            m_iBasePoints = UIManger.Instance.m_iGameStartPoint;
        }

        /// <summary>
        /// 显示比倍窗口
        /// </summary>
        public void ShowWindow()
        {
            m_bIsShow = true;
            this.GetComponent<TweenPosition>().from = this.transform.localPosition;
            this.GetComponent<TweenPosition>().to = new Vector3(0, 0, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }
        /// <summary>
        /// 隐藏比倍窗口
        /// </summary>
        public void HideWindow()
        {
            m_bIsShow = false;
            this.GetComponent<TweenPosition>().from = this.transform.localPosition;
            this.GetComponent<TweenPosition>().to = new Vector3(2000, 0, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }

        public void ColorControl(int _iIndex, Color _cColor)
        {
            if (_iIndex < m_lPointsList.Count)
            m_lPointsList[_iIndex].m_cColor = _cColor;
        }
    }
}
