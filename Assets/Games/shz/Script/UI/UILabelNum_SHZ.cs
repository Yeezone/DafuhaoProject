using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace com.QH.QPGame.SHZ
{
    public enum AlignmentStyle
    {
        Left,
        Center,
        Right
    }

	public class UILabelNum_SHZ : MonoBehaviour
    {

        /// <summary>
        /// 万字，亿字等显示
        /// </summary>
        [System.Serializable]
        public class CSpecialWord
        {
            /// <summary>
            /// 限制数
            /// </summary>
            public List<long> m_listNum = new List<long>();
            /// <summary>
            /// 限制单位
            /// </summary>
            public List<long> m_listDWNum = new List<long>();
            public int m_iLenght = 0;
            public List<string> m_listStrName = new List<string>();
        }
        public CSpecialWord m_cSpecialWord = new CSpecialWord();
        /// <summary>
        /// 图集
        /// </summary>
        public UIAtlas m_Atls;
        /// <summary>
        /// 公共名字
        /// </summary>
        public string m_strTextureName;
        /// <summary>
        /// 数字
        /// </summary>
        private long _iNum = -9999999;
        public long m_iNum;

        /// <summary>
        /// 颜色
        /// </summary>
        private Color _cColor;
        public Color m_cColor = Color.white;

        /// <summary>
        /// 对齐方式
        /// </summary>
        private AlignmentStyle _AlignmentStyle;
        public AlignmentStyle m_AlignmentStyle = AlignmentStyle.Left;
        /// <summary>
        /// 是否有正负号
        /// </summary>
        public bool m_bIsSign = false;

        /// <summary>
        /// 层级
        /// </summary>
        public int m_iDepth = 0;

        /// <summary>
        /// 每个字的间隔
        /// </summary>
        private float _fPerNumDistance = 10;
        public  float m_fPerNumDistance = 10;
        /// <summary>
        /// 每个数字的宽度、高度
        /// </summary>
        private int _iPerNumWidth = 10;
        private int _iPerNumHeight = 10;
        public  int m_iPerNumWidth = 10;
        public  int m_iPerNumHeight = 10;

        private List<GameObject> m_lGameNumlist = new List<GameObject>();
        private List<GameObject> m_lGameNumlistBK = new List<GameObject>();

        /// <summary>
        /// 第一个数字的位置
        /// </summary>
        private Vector3 m_vFistPos;

        /// <summary>
        /// 是否是定时器
        /// </summary>
        public bool m_bIsTimer = false;
        private float m_fWorkTime = 0;
        private float m_fSpeed = 1.0f;
        public bool m_bIsOpen = false;
        public delegate void TimerOnChangge();
        public TimerOnChangge m_OnChange = null;
        public long m_iLimitTime = 5;
        void Start()
        {
        }
        void Update()
        {
            m_fWorkTime += Time.deltaTime;
            //定时器设置
            if (m_bIsTimer && m_bIsOpen && m_fWorkTime >= m_fSpeed && m_iNum >= 0)
            {
                m_iNum -= 1;
                if (m_iLimitTime >= m_iNum)
                {
                   // CMusicManger_JSYS._instance.PlayTimerSound();
                }
                if (m_iNum == 0)
                {
                    m_bIsOpen = false;
                    if (m_OnChange != null) m_OnChange();
                }
                m_fWorkTime = 0;
            }
            //修改颜色
            if (_cColor != m_cColor)
            {
                _cColor = m_cColor;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UISprite>().color = _cColor;
                    }
                }
            }
            //修改对齐方式
            if (_AlignmentStyle != m_AlignmentStyle || _fPerNumDistance != m_fPerNumDistance)
            {
                _fPerNumDistance = m_fPerNumDistance;
                _AlignmentStyle = m_AlignmentStyle;
                SwitchFistPos();
                for (int i = 0; i < m_lGameNumlist.Count; i++)
                {
                    float temp_X = m_vFistPos.x - i * (m_fPerNumDistance + _iPerNumWidth);
                    m_lGameNumlist[i].transform.localPosition = new Vector3(temp_X, 0, 0);
                }
            }
            //修改宽高
            if (_iPerNumWidth != m_iPerNumWidth)
            {
                _iPerNumWidth = m_iPerNumWidth;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UIWidget>().width = _iPerNumWidth;
                    }
                }
            }
            if (_iPerNumHeight != m_iPerNumHeight)
            {
                _iPerNumHeight = m_iPerNumHeight;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UIWidget>().height = _iPerNumHeight;
                    }
                }
            }
            //修改数值
            if (_iNum != m_iNum)
            {
                _iNum = m_iNum;
                SwitchFistPos();
                SetNum();
            }

        }
        /// <summary>
        /// 设置数字
        /// </summary>
        private void SetNum()
        {
            m_lGameNumlistBK.Clear();
            for (int i = 0; i < m_lGameNumlist.Count; i++)
            {
                m_lGameNumlistBK.Add(m_lGameNumlist[i]);
            }
            m_lGameNumlist.Clear();

            long temp_num = _iNum;
            if (_iNum < 0) temp_num = (-1) * m_iNum;

            //绘画数字
            int temp_iLength = 0;
            if (temp_num == 0)
            {
                temp_iLength += 1;
                string strname = temp_num.ToString();

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }

            for (int i = 0; i < m_cSpecialWord.m_iLenght; i++)
            {
                if (temp_num >= m_cSpecialWord.m_listNum[i])
                {
                    temp_iLength += 1;

                    UISprite temp_gobj;
                    if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                    else
                        temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + m_cSpecialWord.m_listStrName[i]);

                    temp_gobj.transform.name = m_cSpecialWord.m_listStrName[i];
                    temp_gobj.GetComponent<UISprite>().spriteName = m_cSpecialWord.m_listStrName[i];
                    temp_gobj.GetComponent<UISprite>().color = _cColor;
                    temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                    temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                    temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                    float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                    temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                    m_lGameNumlist.Add(temp_gobj.gameObject);

                    temp_num = temp_num / m_cSpecialWord.m_listDWNum[i];

                    break;
                }
            }
            while (temp_num >= 1)
            {
                long temp_SingleNum = temp_num % 10;
                temp_iLength += 1;
                string strname = temp_SingleNum.ToString();

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);

                temp_num = temp_num / 10;

            }

            //设置正负号
            if (m_bIsSign)
            {
                string strname = "";
                if (_iNum >= 0) strname = "+";
                if (_iNum < 0) strname = "-";

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (GetNumLength() - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }

            if (m_lGameNumlistBK.Count > m_lGameNumlist.Count)
            {
                for (int i = m_lGameNumlist.Count; i < m_lGameNumlistBK.Count; i++)
                {
                    DestroyObject(m_lGameNumlistBK[i].gameObject, 0.0f);
                }
            }
            m_lGameNumlistBK.Clear();
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <returns></returns>
        private int GetNumLength()
        {
            int temp_iLength = 0;
            long temp_num = _iNum;
            if (_iNum < 0) temp_num = (-1) * _iNum;
            if (m_bIsSign)
            {
                temp_iLength += 1;
            }
            if (temp_num == 0) temp_iLength += 1;
            for (int i = 0; i < m_cSpecialWord.m_iLenght; i++)
            {
                if (temp_num >= m_cSpecialWord.m_listNum[i])
                {
                    temp_num = temp_num / m_cSpecialWord.m_listDWNum[i];
                    temp_iLength += 1;
                    break;
                }
            }
            while (temp_num >= 1)
            {
                temp_iLength += 1;
                temp_num = temp_num / 10;
            }
            return temp_iLength;

        }

        /// <summary>
        /// 计算第一个字符的坐标
        /// </summary>
        private void SwitchFistPos()
        {
            switch (_AlignmentStyle)
            {
                case AlignmentStyle.Left:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        m_vFistPos.x += (temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance - _iPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Center:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        m_vFistPos.x += ((temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance) * 0.5f - _iPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Right:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        m_vFistPos.x -= (m_iPerNumWidth * 0.5f);
                        break;
                    }
            }

        }

        /// <summary>
        /// 设置计时器
        /// </summary>
        /// <param name="_iTime">倒计时时间</param>
        /// <param name="_TimerOnchange">倒计时响应</param>
        public void SetGameTimer(int _iTime, TimerOnChangge _TimerOnchange)
        {
            m_iNum = _iTime;
            m_bIsTimer = true;
            m_OnChange = _TimerOnchange;
            PlayTimer();
        }
        /// <summary>
        /// 停止计时器
        /// </summary>
        public void StopTimer()
        {
            m_bIsOpen = false;
        }
        /// <summary>
        /// 计时器播放
        /// </summary>
        public void PlayTimer()
        {
            m_bIsOpen = true;
        }

    }
}