using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.ATT
{
    public class CCompareManger : MonoBehaviour
    {
        public static CCompareManger _instance;
        /// <summary>
        /// 比倍列表
        /// </summary>
        public List<CCompareSingle> m_lCompareList = new List<CCompareSingle>();
        /// <summary>
        /// 历史牌点
        /// </summary>
        public List<GameObject> m_gHistoryCard = new List<GameObject>();
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color m_cGray = Color.gray;
        public Color m_cRed = Color.red;
        /// <summary>
        /// 倍数
        /// </summary>
        public List<int> m_iTiems = new List<int>();
        /// <summary>
        /// 是否显示了窗口
        /// </summary>
        public bool m_bIsShow = false;

        /// <summary>
        /// 牌背景
        /// </summary>
        public GameObject m_gCardbg;
        /// <summary>
        /// 牌
        /// </summary>
        public GameObject m_gCard;
        /// <summary>
        /// 当前下标
        /// </summary>
        public int m_iCurrentIndex = 0;

        //输赢标识
        public GameObject m_gWinLose;

        public GameObject m_gBig;
        public GameObject m_gSmall;

        public float m_fSpeed = 1.0f;
        public float m_fWorkingTime = 0;

        public bool m_bIsWinPushDouble = false;


        public int m_iIndex = 0;


        /// <summary>
        /// 得分
        /// </summary>
        public float m_fPerNumTime = 0.2f;
        public float m_fWorkTime = 0;
        public bool m_bIsOpen = false; 


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
        }

        // Update is called once per frame
        void Update()
        {
            GetCompareScoreAnimation();

            m_fWorkingTime += Time.deltaTime;
            if (m_fWorkingTime >= m_fSpeed && m_bIsWinPushDouble)
            {
                m_fWorkingTime = 0;
                CPushAnimation._instance.gameObject.SetActive(true);
                if (m_iIndex == 0)
                {
                    m_iIndex = 1;
#if !UNITY_STANDALONE_WIN
            CPushAnimation._instance.SetPushAnimation("doubleup_mobile");
#else
                    CPushAnimation._instance.SetPushAnimation("doubleup_pc");
#endif
                }
                else
                {
                    m_iIndex = 0;
                    CPushAnimation._instance.gameObject.SetActive(true);
                    CPushAnimation._instance.SetPushAnimation("TAKESCORE");
                }
                
                
            }
        }
        /// <summary>
        /// 设置比倍分数 
        /// </summary>
        /// <param name="_iscore">基础分</param>
        public void SetCompareCard(int _iscore)
        {
            for (int i = 0; i < m_lCompareList.Count; i++)
            {
                m_lCompareList[i].m_iScore = _iscore * m_iTiems[i];
            }
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
            this.GetComponent<TweenPosition>().to = new Vector3(-2000, 0, 0);
            this.GetComponent<TweenPosition>().enabled = true;
        }

        /// <summary>
        /// 设置当前比倍下标
        /// </summary>
        /// <param name="_iCurrentIndex">当前下标</param>
        public void SetCurrentCompare(int _iCurrentIndex)
        {
            for (int i = 0; i < m_lCompareList.Count; i++)
            {
                m_lCompareList[i].m_gLabel.SetActive(true);
                m_lCompareList[i].m_gLabel.GetComponent<CLabelNum>().m_cColor = m_cGray;
                m_lCompareList[i].m_cAnimation.m_bIsOpen = false;
                m_lCompareList[i].m_cSprite.m_bIsOpen = false;
                if (m_iCurrentIndex > i) m_lCompareList[i].m_gBg.GetComponent<UISprite>().spriteName = "bibeikuang01";
                else
                m_lCompareList[i].m_gBg.GetComponent<UISprite>().spriteName = "bibeikuang02";

                m_lCompareList[i].m_bIsCurrentReward = false;
           
            }
            if (_iCurrentIndex >= m_lCompareList.Count) _iCurrentIndex = m_lCompareList.Count - 1;

            m_iCurrentIndex = _iCurrentIndex;
            m_lCompareList[m_iCurrentIndex].m_gLabel.GetComponent<CLabelNum>().m_cColor = m_cRed;
            m_lCompareList[m_iCurrentIndex].m_cAnimation.m_bIsOpen = true;
            
            m_lCompareList[m_iCurrentIndex].m_gBg.GetComponent<UISprite>().spriteName = "bibeikuang01";
            if (m_iCurrentIndex < 4)
            {
                m_lCompareList[m_iCurrentIndex + 1].m_gLabel.GetComponent<CLabelNum>().m_cColor = m_cRed;
                m_lCompareList[m_iCurrentIndex + 1].m_bIsCurrentReward = true;
            }
            if (m_iCurrentIndex < 5)
                m_lCompareList[m_iCurrentIndex].m_cSprite.m_bIsOpen = true;
        }

        /// <summary>
        /// 设置历史记录
        /// </summary>
        public void SetHistoryCard()
        {
            for (int i = 0; i <6; i++)
            {
                
               int color = (int)GameLogic.GetCardColor(UIManger.Instance.m_tReadFile.cbHistoryCard[i]) / 16 + 1;
                int value = (int)GameLogic.GetCardValue(UIManger.Instance.m_tReadFile.cbHistoryCard[i]);
                
                if (UIManger.Instance.m_tReadFile.cbHistoryCard[i] == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (UIManger.Instance.m_tReadFile.cbHistoryCard[i]== 0x4F)
                {
                    color = 5;
                    value = 2;
                }
               // Debug.Log("<color=red>" + color.ToString() + ",," + value + "</color>");
                m_gHistoryCard[5-i].GetComponent<UISprite>().spriteName = "Card_" + color.ToString() + "_" + value.ToString();
                
            }
        }

        /// <summary>
        /// 开牌
        /// </summary>
        /// <param name="_bCard">牌数据</param>
        /// <param name="_bIswin">是否是输赢</param>
        public void SetOpenCard(byte _bCard ,bool _bIswin)
        {
            SetOpenCloseCard(true);
            if (_bIswin)
            {
                m_gWinLose.GetComponent<UISprite>().spriteName = "win";
                m_iCurrentIndex += 1;
            }
            else
            {
                m_gWinLose.GetComponent<UISprite>().spriteName = "lose";
                m_iCurrentIndex += 1;
            }
            m_gWinLose.transform.localPosition = m_lCompareList[m_iCurrentIndex].transform.localPosition;

                int color = (int)GameLogic.GetCardColor(_bCard)/ 16 + 1;
                int value = (int)GameLogic.GetCardValue(_bCard);
                
                if (_bCard == 0x4E)
                {
                    color = 5;
                    value = 1;
                }
                else if (_bCard== 0x4F)
                {
                    color = 5;
                    value = 2;
                }
                

                m_gCard.GetComponent<UISprite>().spriteName = "Card_" + color.ToString() + "_" + value.ToString();
            
        }

        /// <summary>
        /// 关闭开牌
        /// </summary>
        /// <param name="_bIsOpen"></param>
        public void SetOpenCloseCard(bool _bIsOpen)
        {
            m_gCard.SetActive(_bIsOpen);
            m_gCardbg.SetActive(!_bIsOpen);
            m_gWinLose.SetActive(_bIsOpen);
            
        }

        public void RectCompare()
        {
            for (int i = 0; i < m_lCompareList.Count; i++)
            {
                m_lCompareList[i].m_gLabel.SetActive(true);
                m_lCompareList[i].m_gLabel.GetComponent<CLabelNum>().m_cColor = m_cGray;
                m_lCompareList[i].m_cAnimation.m_bIsOpen = false;
                m_lCompareList[i].m_cSprite.m_bIsOpen = false;
                m_lCompareList[i].m_gBg.GetComponent<UISprite>().spriteName = "bibeikuang02";
                m_lCompareList[i].m_bIsCurrentReward = false;
            }
            m_iCurrentIndex = 0;
            HideWindow();
            SetOpenCloseCard(false);
        }

        public IEnumerator WaitTimeChangeState(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            SetOpenCloseCard(false);
            m_gBig.SetActive(false);
            m_gSmall.SetActive(false);
             CPushAnimation._instance.gameObject.SetActive(true);
             if (UIManger.Instance.m_cCompareResualt.cbCompareID >= (m_lCompareList.Count-1))
             {
                 CPushAnimation._instance.SetPushAnimation("TAKESCORE");
                 CBottomBTOnclick._instance.SetButtonBt(CBottomBTOnclick._instance.m_gMoreThanBT2, true);
             }else
             {
                 m_bIsWinPushDouble = true;
#if !UNITY_STANDALONE_WIN
            CPushAnimation._instance.SetPushAnimation("doubleup_mobile");
#else
            CPushAnimation._instance.SetPushAnimation("doubleup_pc");
#endif
             }
        

        }

        /// <summary>
        /// 得分动画
        /// </summary>
        public void GetCompareScoreAnimation()
        {
            int point = 0;
            point = m_lCompareList[m_iCurrentIndex].m_iScore;

            if (m_fPerNumTime <= m_fWorkTime && point > 0 && m_bIsOpen)
            {
                point = m_lCompareList[m_iCurrentIndex].m_iScore -= 1;
                if (m_lCompareList[m_iCurrentIndex].m_iScore <= 0) m_lCompareList[m_iCurrentIndex].m_iScore = 0;

                m_lCompareList[m_iCurrentIndex].m_iScore = point;
                CPlayerInfo._instance.m_iGold +=  CPlayerInfo._instance.m_iRoomTimes;
                CPlayerInfo._instance.m_iCreditNum += 1;
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


    }
}