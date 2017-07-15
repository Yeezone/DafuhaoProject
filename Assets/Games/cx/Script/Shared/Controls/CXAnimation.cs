using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CXAnimationPlayStyle
{
    Loop,               //循环
    Once,               //一次
    PingPong            //来回

};
public enum CXAnimationPlayOrder
{
    Order,               //顺序播放
    ReversedOrder        //倒序播放
}
public class CXAnimation : MonoBehaviour
{

    public UISprite m_uSprite;
    /// <summary>
    /// 图集名
    /// </summary>
    public string m_strAtlsName;
    /// <summary>
    /// 图片公共名字
    /// </summary>
    public string m_strTextureName;
    /// <summary>
    /// 帧数
    /// </summary>
    public int m_iFPS;
    /// <summary>
    /// 设置每帧的播放时间
    /// </summary>
    public List<float> m_lfPerFrameTime = new List<float>();
    /// <summary>
    /// 播放时间
    /// </summary>
    private float m_fPlayTime = 0;
    /// <summary>
    /// 均匀动画每帧的时间
    /// </summary>
    public float m_fPerFrameTime = 0.034f;
    /// <summary>
    /// 当前帧下标
    /// </summary>
    public int m_iCurrentFrameIndex = 0;

    /// <summary>
    /// 初始下标
    /// </summary>
    public int m_iStartFrameIndex = 0;

    /// <summary>
    /// 播放模式
    /// </summary>
    public PMAnimationPlayStyle m_aAnimationPlayStyle = PMAnimationPlayStyle.Loop;

    /// <summary>
    /// 1表示往前，-1往后
    /// </summary>
    private int m_iPingPongState = 1;

    public PMAnimationPlayOrder m_aAnimationPlayOrder = PMAnimationPlayOrder.Order;
    /// <summary>
    /// 是否播放
    /// </summary>
    public bool m_bIsPlay = true;

    /// <summary>
    /// 是否自定义每帧的播放时间
    /// </summary>
    public bool m_bIsCustomPlayTime = false;

    /// <summary>
    /// 播放动画播放完之后是否隐藏图片
    /// </summary>
    public bool m_AnimationEndDis = false;

    /// <summary>
    /// 动画初始贴图下标
    /// </summary>
    public int startIndex = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_fPlayTime += Time.deltaTime;
        if (m_bIsPlay)
        {
            if (m_bIsCustomPlayTime && m_lfPerFrameTime.Count == m_iFPS)
            {
                if (m_fPlayTime >= m_lfPerFrameTime[m_iCurrentFrameIndex]) SwitchPlayStyle();
            }
            else
            {
                if (m_fPlayTime >= m_fPerFrameTime) SwitchPlayStyle();
            }
        }
    }
    /// <summary>
    /// 主动设置动画播放
    /// </summary>
    /// <param name="_iFrameIndex"></param>
    public void SetCurrentFrame(int _iFrameIndex)
    {
        m_iCurrentFrameIndex = _iFrameIndex;
        if (_iFrameIndex >= (m_iFPS - 1))
        {
            SwitchPlayStyle();
            return;
        }
        SetAnimationTexture();
    }

    /// <summary>
    /// 设置当前动画
    /// </summary>
    private void SwitchPlayStyle()
    {
        switch (m_aAnimationPlayStyle)
        {
            case PMAnimationPlayStyle.Loop:
                {
                    m_iPingPongState = 1;
                    if (m_aAnimationPlayOrder == PMAnimationPlayOrder.ReversedOrder) m_iPingPongState = -1;

                    m_iCurrentFrameIndex += m_iPingPongState;

                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= m_iFPS) m_iCurrentFrameIndex = m_iStartFrameIndex;
                    else if (m_iPingPongState == -1 && m_iCurrentFrameIndex < m_iStartFrameIndex)
                        m_iCurrentFrameIndex = m_iFPS - 1;
                    break;
                }
            case PMAnimationPlayStyle.Once:
                {
                    m_iPingPongState = 1;
                    if (m_aAnimationPlayOrder == PMAnimationPlayOrder.ReversedOrder) m_iPingPongState = -1;

                    m_iCurrentFrameIndex += m_iPingPongState;
                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= m_iFPS)
                    {
                        m_iCurrentFrameIndex = m_iFPS - 1;
                        //this.GetComponent<UISprite>().enabled = false;
                        Stop();
                    }
                    else if (m_iPingPongState == 1 && m_iCurrentFrameIndex < m_iStartFrameIndex)
                    {
                        m_iCurrentFrameIndex = m_iFPS - 1;
                        //this.GetComponent<UISprite>().enabled = false;
                        Stop();
                    }
                    break;
                }
            case PMAnimationPlayStyle.PingPong:
                {
                    m_iCurrentFrameIndex += m_iPingPongState;
                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= (m_iFPS - 1)) m_iPingPongState = -1;
                    else if (m_iPingPongState == -1 && m_iCurrentFrameIndex <= m_iStartFrameIndex) m_iPingPongState = 1;
                    break;
                }
        }

        SetAnimationTexture();
    }
    /// <summary>
    /// 设置动画贴图
    /// </summary>
    private void SetAnimationTexture()
    {
        m_uSprite.atlas.name = m_strAtlsName;
        m_uSprite.spriteName = m_strTextureName + m_iCurrentFrameIndex.ToString();
        m_fPlayTime = 0;
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    public void Play()
    {
        m_bIsPlay = true;
    }
    /// <summary>
    /// 初始化贴图
    /// </summary>
    public void RecetAnimation()
    {
        m_uSprite.transform.GetComponent<UISprite>().enabled = true;
        SetCurrentFrame(startIndex);
    }
    /// <summary>
    /// 停止动画
    /// </summary>
    public void Stop()
    {
        m_bIsPlay = false;
    }
}
