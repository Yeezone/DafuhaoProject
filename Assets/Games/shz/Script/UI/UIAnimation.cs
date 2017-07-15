using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


namespace com.QH.QPGame.SHZ
{    
	public enum AnimationPlayStyle
	{
	    Loop,               //循环
	    Once,               //一次
		Repeat,				//多次
	    PingPong            //来回

	};
	public enum AnimationPlayOrder
	{
	    Order,               //顺序播放
	    ReversedOrder        //倒序播放
	}
	public class UIAnimation : MonoBehaviour {

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
	    public float m_fPerFrameTime = 0.1f;
		/// <summary>
		/// 重复次数
		/// </summary>
		public int m_bRepeatTimes;
		///当前重复次数
		private int m_bCurTimes; 
	    /// <summary>
	    /// 当前帧下标
	    /// </summary>
	    public int m_iCurrentFrameIndex = 0;
	    /// <summary>
	    /// 播放模式
	    /// </summary>
	    public AnimationPlayStyle m_aAnimationPlayStyle = AnimationPlayStyle.Loop;

	    /// <summary>
	    /// 1表示往前，-1往后
	    /// </summary>
	    private int m_iPingPongState = 1;

	    public AnimationPlayOrder m_aAnimationPlayOrder = AnimationPlayOrder.Order;
	    /// <summary>
	    /// 是否播放
	    /// </summary>
	    public bool m_bIsPlay = false;

	    /// <summary>
	    /// 是否自定义每帧的播放时间
	    /// </summary>
	    public bool m_bIsCustomPlayTime = false;

		/// <summary>
		/// 动画播放完是否隐藏对象
		/// </summary>
		public bool m_bIsDisableSprite;
		// Use this for initialization

        public UnityEvent m_OnChange = new UnityEvent();


		void Start () 
	    {
			m_bCurTimes = 1;
		}
		
		// Update is called once per frame
		void Update () 
	    {
	        if (m_bIsPlay)
	        {
	            if (m_bIsCustomPlayTime && m_lfPerFrameTime.Count == m_iFPS)
	            {
					if (m_fPlayTime >= m_lfPerFrameTime[m_iCurrentFrameIndex])  
					{
						SwitchPlayStyle();
					}
	            }
	            else
	            {
	                if (m_fPlayTime >= m_fPerFrameTime)  SwitchPlayStyle();
	            }
	        }
	        m_fPlayTime += Time.deltaTime;
		}
	    /// <summary>
	    /// 主动设置动画播放
	    /// </summary>
	    /// <param name="_iFrameIndex"></param>
	    public void SetCurrentFrame(int _iFrameIndex)
	    {
	        m_iCurrentFrameIndex = _iFrameIndex;
	        if (_iFrameIndex >= (m_iFPS -1))
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
	            case AnimationPlayStyle.Loop:
	                {
	                    m_iPingPongState = 1;
	                    if (m_aAnimationPlayOrder == AnimationPlayOrder.ReversedOrder) m_iPingPongState = -1;

	                    m_iCurrentFrameIndex += m_iPingPongState;

	                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= m_iFPS) m_iCurrentFrameIndex = 0;
	                    else if (m_iPingPongState == -1 && m_iCurrentFrameIndex <0 )
	                        m_iCurrentFrameIndex = m_iFPS - 1;

	                    break;
	                }
	            case AnimationPlayStyle.Once:
	                {
	                    m_iPingPongState = 1;
	                    if (m_aAnimationPlayOrder == AnimationPlayOrder.ReversedOrder) m_iPingPongState = -1;

	                    m_iCurrentFrameIndex += m_iPingPongState;
	                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= m_iFPS)
	                    {
	                        m_iCurrentFrameIndex = 0;

							if(m_bIsDisableSprite){
//								this.GetComponent<UISprite>().enabled = false;
								this.GetComponent<UISprite>().gameObject.SetActive(false);
							}
                            if (m_OnChange != null) m_OnChange.Invoke();
	                        Stop();
	                    }
	                    else if (m_iPingPongState == 1 && m_iCurrentFrameIndex <0)
	                    {
	                        m_iCurrentFrameIndex = m_iFPS - 1;
							if(m_bIsDisableSprite){
//								this.GetComponent<UISprite>().enabled = false;
								this.GetComponent<UISprite>().gameObject.SetActive(false);
							}
                            if (m_OnChange != null) m_OnChange.Invoke();
	                        Stop();
	                    }
	                    break;
	                }
				case AnimationPlayStyle.Repeat:
				{
					m_iPingPongState = 1;
					if (m_aAnimationPlayOrder == AnimationPlayOrder.ReversedOrder) m_iPingPongState = -1;
					
					m_iCurrentFrameIndex += m_iPingPongState;
					if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= m_iFPS)
					{
						m_iCurrentFrameIndex = 0;
						
						if(m_bCurTimes >= m_bRepeatTimes)
						{
							if(m_bIsDisableSprite){
//								this.GetComponent<UISprite>().enabled = false;
								this.GetComponent<UISprite>().gameObject.SetActive(false);
								Stop();
							}
                            if (m_OnChange != null) m_OnChange.Invoke();
						}
						m_bCurTimes++;
					}
					else if (m_iPingPongState == 1 && m_iCurrentFrameIndex <0)
					{
						m_iCurrentFrameIndex = m_iFPS - 1;

						if(m_bCurTimes >= m_bRepeatTimes)
						{
							if(m_bIsDisableSprite){
//								this.GetComponent<UISprite>().enabled = false;
								this.GetComponent<UISprite>().gameObject.SetActive(false);
							}
                            if (m_OnChange != null) m_OnChange.Invoke();
							Stop();
						}
						m_bCurTimes++;
					}
					break;
				}
	            case AnimationPlayStyle.PingPong:
	                {
	                    m_iCurrentFrameIndex += m_iPingPongState;
	                    if (m_iPingPongState == 1 && m_iCurrentFrameIndex >= (m_iFPS - 1)) m_iPingPongState = -1;
	                    else if (m_iPingPongState == -1 && m_iCurrentFrameIndex <= 0) m_iPingPongState = 1;
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
	       // m_uSprite.atlas.name = m_strAtlsName;
	        m_uSprite.spriteName = m_strTextureName + m_iCurrentFrameIndex.ToString();
	        m_fPlayTime = 0;
	    }
	    /// <summary>
	    /// 播放动画
	    /// </summary>
	    public void Play()
	    {
			m_iCurrentFrameIndex = 0;
			m_bIsPlay = true;
	    }
	    public void RecetAnimation()
	    {
	        this.GetComponent<UISprite>().enabled = true;
	        SetCurrentFrame(0);
	    }
	    /// <summary>
	    /// 停止动画
	    /// </summary>
	    public void Stop()
	    {
	        m_bIsPlay = false;
			m_bCurTimes = 1;
			m_iCurrentFrameIndex = 0;  
	    }
	}
}
