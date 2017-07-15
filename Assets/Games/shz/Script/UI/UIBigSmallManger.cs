using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.SHZ
{

	// 买大小
	enum BuyType {
		BUY_SMALL = 0,
		BUY_TIE,
		BUY_BIG,
		BUY_NULL
	};

    public class UIBigSmallManger : MonoBehaviour
    {

        public static UIBigSmallManger _inatance;

		public GameObject[] gold = new GameObject[3];
		public AudioClip[] _DiceSound = new AudioClip[11];
		public AudioClip[] _Game2Sound = new AudioClip[8];

		//结果音效索引
		private byte m_index = 0;
		private int curBuyType = 0;
		private bool _bIsBuy= false;
		/// <summary>
        /// 得分
        /// </summary>
		public UILabelNum_SHZ m_labelWinScore;
        private long _lWinScore = 0;
        public long m_lWinScore
        {
            get { return _lWinScore; }
            set
            { 
                _lWinScore = value;
                m_labelWinScore.m_iNum = _lWinScore;
            }
        }

        /// <summary>
        /// 总下注
        /// </summary>
		public UILabelNum_SHZ m_labelAllBet;
        private long _lAllBet;
        public long m_lAllBet
        {
            get { return _lAllBet; }
            set
            {
                _lAllBet = value;
                m_labelAllBet.m_iNum = _lAllBet;
            }
        }
        /// <summary>
        /// 下注闪动
        /// </summary>
        public GameObject m_gDownBet;

        /// <summary>
        /// 结果坐标
        /// </summary>
        public List<Vector3> m_listVecPos = new List<Vector3>();
        public float m_fHeight = 252;
       
        public GameObject m_gResualt;
        /// <summary>
        /// 人物动画
        /// </summary>
        public List<UIAnimation> m_listPerAnimation0 = new List<UIAnimation>();
        public List<UIAnimation> m_listPerAnimation1 = new List<UIAnimation>();
        public List<UIAnimation> m_listPerAnimation2 = new List<UIAnimation>();

        /// <summary>
        /// 记录
        /// </summary>
        public UIBigSmallRecord m_Record;

        /// <summary>
        /// 中间结果
        /// </summary>
        public UISprite m_gCenterResualt;
        /// <summary>
        /// 结果
        /// </summary>
        public UISprite []m_gListResualt = new UISprite[2];

        public bool m_bIsWin = false;
        public int m_iPointNum = 0;

        public long m_lWinLossScore = 0;

        /// <summary>
        /// 输赢结果显示
        /// </summary>
		public UILabelNum_SHZ m_cWinLossNum;
        public UISprite m_WinLossTitle;
        public TweenAlpha m_WinLossResualt;
		public GameObject o_bet_tip;
		public GameObject o_btn_getSCore;

        void Awake()
        {
            _inatance = this;
        }

		void OnDestroy(){
			UIEventListener.Destroy(o_btn_getSCore);
			_inatance=null;
		}

        // Use this for initialization
        void Start()
        {
			AddButtonEvent();
        }

        // Update is called once per frame
        void Update()
        {

        }
		void AddButtonEvent()
		{
			UIEventListener.Get(o_btn_getSCore).onClick = EventClick;
		}
		
		void EventClick(GameObject obj)
		{
			if(obj.name.Equals("GetScoreBT"))
			{
				if(UIGame.Instance!=null)
				UIGame.Instance.OnGetResult();
			}
		}

		/// <summary>
		/// 播放音效
		/// </summary>
		void PlayAudio(int sound , AudioClip[] _Audio)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(_Audio[sound], fvol, 1);
		}

        /// <summary>
        /// 继续按钮
        /// </summary>
        public void ContinueBt_OnClick()
        {

        }

        /// <summary>
        /// 押小按钮
        /// </summary>
        public void LittleBT_OnClick()
        {
			#if UNITY_STANDALONE_WIN
						
			#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
			if(Input.touchCount > 1)	return;
			#endif
			if(!_bIsBuy)
			{
				_bIsBuy = true;
				UIGame.Instance.OnStartScene2((int)BuyType.BUY_SMALL,0);
				curBuyType = (int)BuyType.BUY_SMALL;
				Invoke("resumeBuyBtn",2.0f);
			}
        }
        /// <summary>
        /// 押和按钮
        /// </summary>
        public void DrawBT_OnClick()
        {
			#if UNITY_STANDALONE_WIN
			
			#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
			if(Input.touchCount > 1)	return;
			#endif
			if(!_bIsBuy)
			{
				_bIsBuy = true;
				UIGame.Instance.OnStartScene2((int)BuyType.BUY_TIE,0);
				curBuyType = (int)BuyType.BUY_TIE;
				Invoke("resumeBuyBtn",2.0f);
			}
        }
        /// <summary>
        /// 押大按钮
        /// </summary>
        public void BigBT_OnClick()
        {
			#if UNITY_STANDALONE_WIN
		
			#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
			if(Input.touchCount > 1)	return;
			#endif
			if(!_bIsBuy)
			{
				_bIsBuy = true;
				UIGame.Instance.OnStartScene2((int)BuyType.BUY_BIG,0);
				curBuyType = (int)BuyType.BUY_BIG;
				Invoke("resumeBuyBtn",2.0f);
			}
        }

		void resumeBuyBtn(){
			if(_bIsBuy) _bIsBuy = false;
		}

        public void SendBet(int _iIndex)
        {
            PlayDownAnimation();
        }

		void PlayVoice(int sound , AudioClip[] _Audio)
		{
			float fvol = NGUITools.soundVolume;

			NGUITools.PlaySound(_Audio[sound], fvol, 1);
		}

        /// <summary>
        /// 开奖
        /// </summary>
        /// <param name="_bIsWin">是否输赢</param>
        /// <param name="_PointNum">点数</param>
       /// <param name="_PointNum">输赢分数</param>
		public void OpenReward(bool _bIsWin,int _PointNum1, int _PointNum2,long _WinLossSocre)
        {
//			if(o_btn_getSCore.activeSelf) o_btn_getSCore.SetActive(false);
			o_btn_getSCore.GetComponent<UIButton>().isEnabled = false;
			if(o_bet_tip.activeSelf) o_bet_tip.SetActive(false);

			gold[curBuyType].SetActive(true);

            if (_lWinScore > 0)
                m_lAllBet = _lWinScore;

			_bIsBuy = false;

			int r0 = _PointNum1; 
			int r1 = _PointNum2;
//            if (_PointNum > 6)
//            {
//                r0 = Random.Range(_PointNum -6, 7);
//                r1 = _PointNum - r0;
//            }
//            else
//            {
//                r0 = Random.Range(1, _PointNum);
//                r1 = _PointNum - r0;
//            }
            m_gListResualt[0].spriteName = "BS_Dice" + r0.ToString();
            m_gListResualt[1].spriteName = "BS_Dice" + r1.ToString();
            if (r0 > r1)
            {
                int rr = r0;
                r0 = r1;
                r1 = rr;
            }
            m_gCenterResualt.gameObject.SetActive(true);
            m_gCenterResualt.spriteName = "BS_Dice" + r0.ToString() + r1.ToString();

			if(_PointNum1+_PointNum2 > 1)	m_index = (byte)(_PointNum1+_PointNum2-2);

            m_bIsWin = _bIsWin;
            PlayDownAnimation();
            StopDownBetAnimation();

			m_iPointNum = _PointNum1+_PointNum2;
			m_lWinLossScore = _WinLossSocre;
        }

		void voiceRecall()
		{
			PlayAudio(0,null);
		}

        /// <summary>
        /// 播放下注闪动
        /// </summary>
        public void PlayDownBetAnimation()
        {
            m_gDownBet.GetComponent<TweenAlpha>().enabled = true;
//			PlayAudio(3,_Game2Sound);
        }
        /// <summary>
        /// 停止下注闪动
        /// </summary>
        public void StopDownBetAnimation()
        {
            m_gDownBet.GetComponent<TweenAlpha>().enabled = false;
            m_gDownBet.GetComponent<UISprite>().alpha = 0;
        }
        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="_iIndex"></param>
        public void SetOpenResualt(int _iIndex)
        {
            m_gResualt.SetActive(true);
            m_gResualt.transform.localPosition = m_listVecPos[_iIndex];
            m_gResualt.GetComponent<TweenPosition>().from = m_gResualt.transform.localPosition;
            m_gResualt.GetComponent<TweenPosition>().to = new Vector3(m_gResualt.transform.localPosition.x, m_listVecPos[_iIndex].y - m_fHeight, m_listVecPos[_iIndex].z);
            m_gResualt.GetComponent<TweenPosition>().ResetToBeginning();
            m_gResualt.GetComponent<TweenPosition>().enabled = true;
            //记录
            m_Record.AddRecord(_iIndex);

            if (m_bIsWin)
            {
                m_WinLossTitle.spriteName = "BS_Win";
                m_cWinLossNum.m_strTextureName = "BS_Win";
				m_cWinLossNum.m_cSpecialWord.m_listStrName.Clear();
				m_cWinLossNum.m_cSpecialWord.m_listStrName.Add("BS_WinW");
                m_cWinLossNum.m_iNum = m_lWinLossScore;
            }
            else
            {
                m_WinLossTitle.spriteName = "BS_Loss";
                m_cWinLossNum.m_strTextureName = "BS_Loss";
				m_cWinLossNum.m_cSpecialWord.m_listStrName.Clear();
				m_cWinLossNum.m_cSpecialWord.m_listStrName.Add("BS_LossW");
                m_cWinLossNum.m_iNum = m_lWinLossScore;
            }
            m_WinLossResualt.ResetToBeginning();
            m_WinLossResualt.enabled = true;
        }
        /// <summary>
        /// 等待下注动画
        /// </summary>
        public void PlayWaitDownBet()
        {
			if(!o_bet_tip.activeSelf) o_bet_tip.SetActive(true);
			
			PlayAudio(4,_Game2Sound);
			o_btn_getSCore.GetComponent<UIButton>().isEnabled = true;

            for (int i = 0; i < m_listPerAnimation1.Count; i++)
            {
                m_listPerAnimation1[i].gameObject.SetActive(false);
            }
            m_listPerAnimation1[0].m_aAnimationPlayStyle = AnimationPlayStyle.Loop;
            m_listPerAnimation1[0].RecetAnimation();
            m_listPerAnimation1[0].gameObject.SetActive(true);
            m_listPerAnimation1[0].Play();

			if (_lWinScore > 0){
				m_lAllBet = _lWinScore;
			}else if(m_lWinLossScore<0)
			{
				m_lWinLossScore = 0;
				o_bet_tip.SetActive(false);
				o_btn_getSCore.GetComponent<UIButton>().isEnabled = false;
				Invoke("closeResult",2.0f);
			}
            PlayDownBetAnimation();
        }
		void closeResult()
		{
			UIGame.Instance.OnGetResult();
			o_btn_getSCore.GetComponent<UIButton>().isEnabled = true;
		}


        /// <summary>
        /// 播放下注动画
        /// </summary>
        public void PlayDownAnimation()
        {
            for (int i = 0; i < m_listPerAnimation0.Count; i++)
            {
                m_listPerAnimation0[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < m_listPerAnimation1.Count; i++)
            {
                m_listPerAnimation1[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < m_listPerAnimation2.Count; i++)
            {
                m_listPerAnimation2[i].gameObject.SetActive(false);
            }
            m_listPerAnimation0[1].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
            m_listPerAnimation0[1].RecetAnimation();
            m_listPerAnimation0[1].gameObject.SetActive(true);
            m_listPerAnimation0[1].Play();

            m_listPerAnimation1[1].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
            m_listPerAnimation1[1].RecetAnimation();
            m_listPerAnimation1[1].gameObject.SetActive(true);
            m_listPerAnimation1[1].Play();

            m_listPerAnimation2[1].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
            m_listPerAnimation2[1].RecetAnimation();
            m_listPerAnimation2[1].gameObject.SetActive(true);
            m_listPerAnimation2[1].Play();

        }
        /// <summary>
        /// 左边输赢动画
        /// </summary>
        public void PlayWinLossAnimation0()
        {
            for (int i = 0; i < m_listPerAnimation0.Count; i++)
                m_listPerAnimation0[i].gameObject.SetActive(false);

            if (m_bIsWin)
            {
                m_listPerAnimation0[3].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation0[3].RecetAnimation();
                m_listPerAnimation0[3].gameObject.SetActive(true);
                m_listPerAnimation0[3].Play();
            }
            else
            {
                m_listPerAnimation0[2].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation0[2].RecetAnimation();
                m_listPerAnimation0[2].gameObject.SetActive(true);
                m_listPerAnimation0[2].Play();
            }
        }
        /// <summary>
        /// 中输赢动画
        /// </summary>
        public void PlayWinLossAnimation1()
        {
            for (int i = 0; i < m_listPerAnimation1.Count; i++)
                m_listPerAnimation1[i].gameObject.SetActive(false);

            if (m_bIsWin)
            {
                m_listPerAnimation1[2].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation1[2].RecetAnimation();
                m_listPerAnimation1[2].gameObject.SetActive(true);
                m_listPerAnimation1[2].Play();
            }
            else
            {
                m_listPerAnimation1[3].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation1[3].RecetAnimation();
                m_listPerAnimation1[3].gameObject.SetActive(true);
                m_listPerAnimation1[3].Play();
            }

            //设置开大开小
            if (m_iPointNum < 7)
                SetOpenResualt(0);
            else if (m_iPointNum == 7)
                SetOpenResualt(1);
            else
                SetOpenResualt(2);
        }
        /// <summary>
        /// 右边输赢动画
        /// </summary>
        public void PlayWinLossAnimation2()
        {
            for (int i = 0; i < m_listPerAnimation2.Count; i++)
                m_listPerAnimation2[i].gameObject.SetActive(false);

            if (m_bIsWin)
            {
                m_listPerAnimation2[3].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation2[3].RecetAnimation();
                m_listPerAnimation2[3].gameObject.SetActive(true);
                m_listPerAnimation2[3].Play();
            }
            else
            {
                m_listPerAnimation2[2].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
                m_listPerAnimation2[2].RecetAnimation();
                m_listPerAnimation2[2].gameObject.SetActive(true);
                m_listPerAnimation2[2].Play();
            }
        }

        public void PlayFreeAnimation0()
        {
             for (int i = 0; i < m_listPerAnimation0.Count; i++)
                 m_listPerAnimation0[i].gameObject.SetActive(false);

             m_listPerAnimation0[0].m_aAnimationPlayStyle = AnimationPlayStyle.Loop;
             m_listPerAnimation0[0].RecetAnimation();
             m_listPerAnimation0[0].gameObject.SetActive(true);
             m_listPerAnimation0[0].Play();
			PlayAudio(0,_Game2Sound);
        }

        public void PlayFreeAnimation1()
        {
			for(int i= 0; i<3; i++)
			{
				gold[i].SetActive(false);
			}

            for (int i = 0; i < m_listPerAnimation1.Count; i++)
                m_listPerAnimation1[i].gameObject.SetActive(false);

            m_listPerAnimation1[4].m_aAnimationPlayStyle = AnimationPlayStyle.Once;
            m_listPerAnimation1[4].RecetAnimation();
            m_listPerAnimation1[4].gameObject.SetActive(true);
            m_listPerAnimation1[4].Play();

            ClearResualt();
        }

        public void PlayFreeAnimation2()
        {
            for (int i = 0; i < m_listPerAnimation2.Count; i++)
                m_listPerAnimation2[i].gameObject.SetActive(false);

            m_listPerAnimation2[0].m_aAnimationPlayStyle = AnimationPlayStyle.Loop;
            m_listPerAnimation2[0].RecetAnimation();
            m_listPerAnimation2[0].gameObject.SetActive(true);
            m_listPerAnimation2[0].Play();
			PlayAudio(3,_Game2Sound);
        }


        /// <summary>
        /// 清理准备下一把游戏
        /// </summary>
        public void ClearResualt()
        {
            m_WinLossResualt.enabled = false;
            m_WinLossResualt.GetComponent<UISprite>().alpha = 0;

            m_gResualt.SetActive(false);
            m_gCenterResualt.gameObject.SetActive(false);

            //输赢设置
            if (m_bIsWin)
                m_lWinScore = m_lWinLossScore;
            else
            {
				m_lAllBet=0;
				m_lWinScore = 0;
            }
        }

		public void OnDiceAudio()
		{
			if(m_index<11){
				PlayVoice(m_index,_DiceSound);
			}
		}

    }
}
