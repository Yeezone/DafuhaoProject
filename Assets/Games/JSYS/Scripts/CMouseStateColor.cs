using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMouseStateColor : MonoBehaviour {

    /// <summary>
    /// 按钮悬停颜色
    /// </summary>
    [System.Serializable]
    public class CButtonColor
    {
        public Color m_cNormalColor = Color.white;
        public Color m_cHoeverColor = Color.white;
        public Color m_cPressColor = Color.white;
        public Color m_cDisableColor = Color.white;
    }

    public CButtonColor m_ButtonColor = new CButtonColor();
    public CButtonColor m_ButtonColor2 = new CButtonColor();
    public List<GameObject> m_listColorGameObject = new List<GameObject>();
    public List<GameObject> m_listColorGameObject2 = new List<GameObject>();

    /// <summary>
    /// 弹起
    /// </summary>
    public void Mouse_Release()
    {
        SetButtonColor(m_ButtonColor.m_cHoeverColor);
        SetButtonColor2(m_ButtonColor2.m_cHoeverColor);
    }
    /// <summary>
    /// 鼠标移除
    /// </summary>
    public void Mouse_Exit()
    {
        SetButtonColor(m_ButtonColor.m_cNormalColor);
        SetButtonColor2(m_ButtonColor2.m_cNormalColor);
    }
    /// <summary>
    /// 鼠标下压
    /// </summary>
    public void Mouse_Press()
    {
        SetButtonColor(m_ButtonColor.m_cPressColor);
        SetButtonColor2(m_ButtonColor2.m_cPressColor);
    }
    /// <summary>
    /// 鼠标悬停
    /// </summary>
    public void Mouse_Hoever()
    {
        SetButtonColor(m_ButtonColor.m_cHoeverColor);
        SetButtonColor2(m_ButtonColor2.m_cHoeverColor);
    }

    /// <summary>
    /// 设置纯色
    /// </summary>
    /// <param name="_cColor"></param>
    public void SetButtonColor(Color _cColor)
    {
        for (int i = 0; i < m_listColorGameObject.Count; i++)
        {
            m_listColorGameObject[i].GetComponent<UISprite>().color = _cColor;
        }
    }
    /// <summary>
    /// 设置有色贴图
    /// </summary>
    /// <param name="_cColor"></param>
    public void SetButtonColor2(Color _cColor)
    {
        for (int i = 0; i < m_listColorGameObject2.Count; i++)
        {
            if (m_listColorGameObject2[i] != null)
            m_listColorGameObject2[i].GetComponent<UISprite>().color = _cColor;
        }
    }
}
