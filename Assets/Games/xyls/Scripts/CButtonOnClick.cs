using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    // 按钮上的筹码值
    [System.Serializable]
    public class chipNum
    {
        public int chip01;
        public int chip02;
        public int chip03;
    }

    // 菜单按钮的子级
    [System.Serializable]
    public class MenuBtn
    {
        public GameObject menuBtn;
        public TweenPosition helpBtn;
        public TweenPosition settingBtn;
        public TweenPosition exitBtn;
    }
    // 帮助/设置/退出面板
    [System.Serializable]
    public class HSE_Panel
    {
        public GameObject helpPanel;
        public GameObject settingPanel;
        public GameObject exitPanel;
    }


    public class CButtonOnClick : MonoBehaviour
    {
		public static CButtonOnClick _instance;
        // 押分面板
        public GameObject m_gBetPanel;
        // 收起按钮
        public GameObject m_gBetCloseBtn;
        // 押分面板是否开启
        [HideInInspector]
        public bool m_bBetPanelIsOpen = false;

        // 按钮上的筹码值
        public chipNum m_cChipNum;
        // 筹码按钮上的背景
        public GameObject[] m_gChipButtonBG = new GameObject[3];
        // 菜单按钮
        public MenuBtn m_mMenuBtn;
        // 菜单按钮的点击状态
        private bool m_bMenuBtnIsPress = false;
        // 帮助/设置/退出面板
        public HSE_Panel m_hHSE_Panel;

		void Start()
		{
			_instance = this;
		}

        void Update()
        {
            // Esc退出游戏
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ExitButtonOnClick();
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        // 设置筹码值01
        public void SetChipNum01()
        {
            GameEvent._instance.m_iCurChipNum = m_cChipNum.chip01;
            m_gChipButtonBG[0].SetActive(true);
            m_gChipButtonBG[1].SetActive(false);
            m_gChipButtonBG[2].SetActive(false);
        }

        // 设置筹码值02
        public void SetChipNum02()
        {
            GameEvent._instance.m_iCurChipNum = m_cChipNum.chip02;
            m_gChipButtonBG[0].SetActive(false);
            m_gChipButtonBG[1].SetActive(true);
            m_gChipButtonBG[2].SetActive(false);
        }

        // 设置筹码值03
        public void SetChipNum03()
        {
            GameEvent._instance.m_iCurChipNum = m_cChipNum.chip03;
            m_gChipButtonBG[0].SetActive(false);
            m_gChipButtonBG[1].SetActive(false);
            m_gChipButtonBG[2].SetActive(true);
        }

        /// <summary>
        /// 清除押注按钮
        /// </summary>
        public void ClearJettonOnClick()
        {
            CUIGame._instance.ClearJetton();
        }

        /// <summary>
        /// 续押按钮
        /// </summary>
        public void ContinueBetOnClick()
        {
            CUIGame._instance.ContinueBet();
        }

		/// <summary>
		/// 设置按钮激活状态
		/// </summary>
		public void SetButtonBt(GameObject _ButtonGameObject, bool _bIsDisable)
		{
			if (_bIsDisable)
			{				
				_ButtonGameObject.GetComponent<UIButton>().state = UIButton.State.Normal;
				_ButtonGameObject.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				_ButtonGameObject.GetComponent<UIButton>().state = UIButton.State.Disabled;
				_ButtonGameObject.GetComponent<BoxCollider>().enabled = false;
			}			
		}

		/// <summary>
		/// 退出游戏
		/// </summary>
		public void ExitGame()
		{
			CGameEngine.Instance.Quit();
		}

        /// <summary>
        /// 点击"押分"按钮,弹出押分面板
        /// </summary>
        public void BetButtonOnClick()
        {
			CloseHelpPanel();
			CloseSetPanel();
			CloseExitPanel();
			//如果打开菜单面板,则需要收回
			if (m_bMenuBtnIsPress)
			{
				MenuButtonOnClick();
			}

#if UNITY_STANDALONE_WIN
            if (m_bBetPanelIsOpen)
            {
                m_bBetPanelIsOpen = false;
                m_gBetCloseBtn.SetActive(false);
                TweenPosition temp = m_gBetPanel.GetComponent<TweenPosition>();
                temp.from = m_gBetPanel.transform.localPosition;
                temp.to = new Vector3(0, -650, 0);
                temp.enabled = true;
                temp.ResetToBeginning();
            }
            else
            {
                m_bBetPanelIsOpen = true;
                m_gBetCloseBtn.SetActive(true);
                TweenPosition temp = m_gBetPanel.GetComponent<TweenPosition>();
                temp.from = m_gBetPanel.transform.localPosition;
                temp.to = new Vector3(0, -50, 0);
                temp.enabled = true;
                temp.ResetToBeginning();
            }
#elif UNITY_EDITOR ||  UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
            if (m_bBetPanelIsOpen)
            {
                m_bBetPanelIsOpen = false;
                m_gBetCloseBtn.SetActive(false);
                TweenPosition temp = m_gBetPanel.GetComponent<TweenPosition>();
                temp.from = m_gBetPanel.transform.localPosition;
                temp.to = new Vector3(0, -610, 0);
                temp.enabled = true;
                temp.ResetToBeginning();
            }
            else
            {
                m_bBetPanelIsOpen = true;
                m_gBetCloseBtn.SetActive(true);
                TweenPosition temp = m_gBetPanel.GetComponent<TweenPosition>();
                temp.from = m_gBetPanel.transform.localPosition;
                temp.to = new Vector3(0, -10, 0);
                temp.enabled = true;
                temp.ResetToBeginning();
            }
#endif
        }

/////////////////////////////////////菜单及子级按钮事件//////////////////////////////////////
        /// <summary>
        /// 点击菜单按钮
        /// </summary>
        public void MenuButtonOnClick()
        {
			CloseHelpPanel();
			CloseSetPanel();
			CloseExitPanel();
			//如果打开者押分面板,则需要收回
			if (m_bBetPanelIsOpen)
			{
				BetButtonOnClick();
			}

            if (m_bMenuBtnIsPress)
            {
                m_bMenuBtnIsPress = false;

                m_mMenuBtn.helpBtn.PlayForward();

                m_mMenuBtn.settingBtn.PlayForward();

                m_mMenuBtn.exitBtn.PlayForward();
            }
            else
            {
                m_bMenuBtnIsPress = true;

                m_mMenuBtn.helpBtn.PlayReverse();

                m_mMenuBtn.settingBtn.PlayReverse();

                m_mMenuBtn.exitBtn.PlayReverse();
            }            
        }

        /// <summary>
        /// 帮助按钮
        /// </summary>
        public void HelpButtonOnClick()
        {
			CloseAllPanel();
            m_hHSE_Panel.helpPanel.SetActive(true);
        }

        /// <summary>
        /// 关闭帮助面板
        /// </summary>
        public void CloseHelpPanel()
        {
            m_hHSE_Panel.helpPanel.SetActive(false);
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        public void SetButtonOnClick()
        {
			CloseAllPanel();
            m_hHSE_Panel.settingPanel.SetActive(true);
        }

        /// <summary>
        /// 关闭设置面板
        /// </summary>
        public void CloseSetPanel()
        {
            m_hHSE_Panel.settingPanel.SetActive(false);
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        public void ExitButtonOnClick()
        {
			CloseAllPanel();
            m_hHSE_Panel.exitPanel.SetActive(true);
        }

        /// <summary>
        /// 关闭退出面板
        /// </summary>
        public void CloseExitPanel()
        {
            m_hHSE_Panel.exitPanel.SetActive(false);
        }

		/// <summary>
		/// 关闭所有面板
		/// </summary>
		public void CloseAllPanel()
		{
			CloseHelpPanel();
			CloseSetPanel();
			CloseExitPanel();
			//如果打开菜单或者押分面板,则需要收回
			if (m_bMenuBtnIsPress)
			{
				MenuButtonOnClick();
			}
			if (m_bBetPanelIsOpen)
			{
				BetButtonOnClick();
			}
		}

////////////////////////////////////////////////////////////////////////////////////////////

	}
}
