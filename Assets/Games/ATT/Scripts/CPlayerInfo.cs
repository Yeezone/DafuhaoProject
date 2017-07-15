using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.ATT
{
    public class CPlayerInfo : MonoBehaviour
    {
        public static CPlayerInfo _instance;
        /// <summary>
        /// 积分
        /// </summary>
        public GameObject m_gCreditLabelNum;
  
        /// <summary>
        /// 积分与金币比例
        /// </summary>
        public GameObject m_gLabel_Points;
        /// <summary>
        /// 金币
        /// </summary>
        public GameObject m_gLabel_Gold;
        /// <summary>
        /// bet
        /// </summary>
        public GameObject m_gBETLabel_num;

        /// <summary>
        /// 左上角80nums
        /// </summary>
        public GameObject m_gLabel80nums;

        /// <summary>
        /// 左上角80nums最小下注
        /// </summary>
        public GameObject m_gLabel80numsMinBET;

        private long _iCreditNum;
        public long m_iCreditNum
        {
            get { return _iCreditNum; }
            set
            {
                _iCreditNum = value;
                m_gCreditLabelNum.GetComponent<CLabelNum>().m_iNum = _iCreditNum;
            }
        }

        private string _sPlayerAccount;
        public string m_sPlayerAccount
        {
            get { return _sPlayerAccount; }
            set
            {
                _sPlayerAccount = value;
                //m_gLabel_PlayerAccount.GetComponent<UILabel>().text = "玩家：" + _sPlayerAccount;
            }
        }

        private int _iRoomTiems;
        public int _iExscale;
        public int m_iRoomTimes
        {
            get { return _iRoomTiems; }
            set
            {
                _iRoomTiems = value;

                m_gLabel_Points.GetComponent<CLabelNum>().m_iNum = _iRoomTiems;
            }
        }

        private long _iGold;
        public long m_iGold
        {
            get { return _iGold; }
            set
            {
                _iGold = value;
                m_gLabel_Gold.GetComponent<CLabelNum>().m_iNum =_iGold;
            }
        }
        private int _iBet;
        public int m_iBet
        {
            get { return _iBet; }
            set
            {
                _iBet = value;
                m_gBETLabel_num.GetComponent<CLabelNum>().m_iNum = _iBet;
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
        }


        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="_strPlayerAccount">游戏账号</param>
        /// <param name="_iTiems">积分倍率</param>
        /// /// <param name="_iCreditExscale">房间倍率</param>
        /// <param name="_iGoldNum">金币数</param>
        /// /// <param name="_iMinBET">最小下注</param>
        public void SetPlayerInfo(string _strPlayerAccount, int _iTiems,int _iCreditExscale, int _iGoldNum,int _iMinBET)
        {
            m_sPlayerAccount = _strPlayerAccount;
            _iExscale = _iCreditExscale;
            m_iRoomTimes = _iTiems;
            m_iGold = _iGoldNum;
            m_iCreditNum = (int)(_iGoldNum / _iTiems);
            CPlayerInfo._instance.m_iBet = UIManger.Instance.m_iGameStartPoint;

            _iMinBET = 80;
            m_gLabel80numsMinBET.GetComponent<CLabelNum>().m_iNum = _iMinBET;
            if (m_iCreditNum < _iMinBET)
                m_gLabel80nums.GetComponent<CLabelNum>().m_iNum = 0;
            else
                m_gLabel80nums.GetComponent<CLabelNum>().m_iNum = 0;

            CBasePoint._instance.m_gLabelComputerNum.GetComponent<CLabelNum>().m_iNum = ((int)GameEngine.Instance.MySelf.DeskStation + 1) +( (int)GameEngine.Instance.MySelf.DeskNO  * UIManger.Instance.m_iGamePlayer);
        }

    }
}