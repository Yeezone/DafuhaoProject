using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.ATT
{
    public class CPokerPoints : MonoBehaviour
    {
        /// <summary>
        /// 倍数
        /// </summary>

        public List<int> m_lTimesList = new List<int>();

        /// <summary>
        /// label 对象
        /// </summary>
        public List<GameObject> m_lLabelList = new List<GameObject>();

        /// <summary>
        /// 牌型
        /// </summary>
        public int m_iCardType;

        public List<int> m_lPointList = new List<int>();

        public GameObject m_gTitle;

        private Color _cColor = Color.red;
        public Color m_cColor
        {
            get { return _cColor; }
            set 
            {
                _cColor = value;
                m_gTitle.GetComponent<UISprite>().color = _cColor;
            }
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
        /// 设置压分界面
        /// </summary>
        /// <param name="_iIndex"></param>
        /// <param name="_iPoint"></param>
        public void SetPoint(int _iIndex, int _iPoint, Color _cColor)
        {
            m_lPointList[_iIndex] = _iPoint * m_lTimesList[_iIndex];
            m_lLabelList[_iIndex].GetComponent<CLabelNum>().m_cColor = _cColor;
            m_lLabelList[_iIndex].GetComponent<CLabelNum>().m_iNum = m_lPointList[_iIndex];
        }

        public void SetPointText(int _iIndex, int _iPoint, Color _cColor)
        {
            m_lLabelList[_iIndex].GetComponent<CLabelNum>().m_cColor = _cColor;
            m_lLabelList[_iIndex].GetComponent<CLabelNum>().m_iNum = _iPoint;
        }
    }
}