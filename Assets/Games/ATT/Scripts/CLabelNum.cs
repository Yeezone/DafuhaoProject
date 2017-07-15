using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.ATT
{
    public enum AlignmentStyle
    {
        Left,
        Center,
        Right
    }

    public class CLabelNum : MonoBehaviour
    {


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
        public float m_fPerNumDistance = 10;
        /// <summary>
        /// 每个数字的宽度、高度
        /// </summary>
        private int _iPerNumWidth = 10;
        private int _iPerNumHeight = 10;
        public int m_iPerNumWidth = 10;
        public int m_iPerNumHeight = 10;

        private List<GameObject> m_lGameNumlist = new List<GameObject>();
        private List<GameObject> m_lGameNumlist2 = new List<GameObject>();

        public bool m_bIsMaxLengthLimit = false;
        public int m_iMaxLenght = 0;



        /// <summary>
        /// 第一个数字的位置
        /// </summary>
        private Vector3 m_vFistPos;

        void Start()
        {
        }
        void Update()
        {
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
            m_lGameNumlist2.Clear();

            for (int i = 0; i < m_lGameNumlist.Count; i++)
            {
                m_lGameNumlist2.Add(m_lGameNumlist[i]);
            }
            m_lGameNumlist.Clear();
            //             foreach (Transform child in this.transform)
            //             {
            //                 if (child != null) GameObject.Destroy(child.gameObject,0.0f);
            //             }
            long temp_num = _iNum;
            if (_iNum < 0) temp_num = (-1) * _iNum;
            int temp_count = GetNumLength();
            int _itempPerNumWidth = _iPerNumWidth;
            int _itempPerNumHeight = _iPerNumHeight;
            if (m_bIsMaxLengthLimit && temp_count * _iPerNumWidth > m_iMaxLenght && m_iMaxLenght>0)
            {
                _itempPerNumWidth = m_iMaxLenght / temp_count;
                _itempPerNumHeight = _iPerNumHeight * _itempPerNumWidth / _iPerNumWidth;
            }

            //绘画数字
            int temp_iLength = 0;
            if (temp_num == 0)
            {
                temp_iLength = 1;
                string strname = temp_num.ToString();

                UISprite temp_gobj;
                if (m_lGameNumlist2.Count >= 1) temp_gobj = m_lGameNumlist2[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _itempPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _itempPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _itempPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }
            while (temp_num >= 1)
            {
                long temp_SingleNum = temp_num % 10;
                temp_iLength += 1;
                string strname = temp_SingleNum.ToString();
                UISprite temp_gobj;
                if (m_lGameNumlist2.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlist2[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _itempPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _itempPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _itempPerNumWidth);
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
                if (m_lGameNumlist2.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlist2[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _itempPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _itempPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (GetNumLength() - 1) * (_fPerNumDistance + _itempPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }

            if (m_lGameNumlist2.Count > m_lGameNumlist.Count)
            {
                for (int i = m_lGameNumlist.Count; i < m_lGameNumlist2.Count; i++)
                {
                    DestroyObject(m_lGameNumlist2[i].gameObject, 0.0f);
                }
            }
            m_lGameNumlist2.Clear();
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
                        int _itempPerNumWidth = _iPerNumWidth;
                        int _itempPerNumHeight = _iPerNumHeight;
                        if (m_bIsMaxLengthLimit && temp_num * _iPerNumWidth > m_iMaxLenght && m_iMaxLenght > 0)
                        {
                            _itempPerNumWidth = m_iMaxLenght / temp_num;
                            _itempPerNumHeight = _iPerNumHeight * _itempPerNumWidth / _iPerNumWidth;
                        }
                        m_vFistPos.x += (temp_num * _itempPerNumWidth + (temp_num - 1) * _fPerNumDistance - _itempPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Center:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        int _itempPerNumWidth = _iPerNumWidth;
                        int _itempPerNumHeight = _iPerNumHeight;
                        if (m_bIsMaxLengthLimit && temp_num * _iPerNumWidth > m_iMaxLenght && m_iMaxLenght > 0)
                        {
                            _itempPerNumWidth = m_iMaxLenght / temp_num;
                            _itempPerNumHeight = _iPerNumHeight * _itempPerNumWidth / _iPerNumWidth;
                        }
                        m_vFistPos.x += ((temp_num * _itempPerNumWidth + (temp_num - 1) * _fPerNumDistance) * 0.5f - _itempPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Right:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        int _itempPerNumWidth = _iPerNumWidth;
                        int _itempPerNumHeight = _iPerNumHeight;
                        if (m_bIsMaxLengthLimit && temp_num * _iPerNumWidth > m_iMaxLenght && m_iMaxLenght > 0)
                        {
                            _itempPerNumWidth = m_iMaxLenght / temp_num;
                            _itempPerNumHeight = _iPerNumHeight * _itempPerNumWidth / _iPerNumWidth;
                        }
                        m_vFistPos.x -= (_itempPerNumWidth * 0.5f);
                        break;
                    }
            }

        }
    }
}