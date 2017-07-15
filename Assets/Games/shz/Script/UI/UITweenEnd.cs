using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.SHZ
{    
	public class UITweenEnd : MonoBehaviour {

		static public bool m_bIsShow;
		//循环次数
		public int  m_looperTimes;
		//当前次数
		public int  m_curTimes;
		public Vector3 m_vShowPos = new Vector3(0,0,0);
		public Vector3 m_vHidePos = new Vector3(-2000, 0, 0);
		
		public Vector3 m_vShowScale = new Vector3(1,1,1);
		public Vector3 m_vHideScale = new Vector3(0, 0, 0);
		
		public float m_fShowAlpha = 1.0f;
		public float m_fHideAlpha = 1.0f;
		
		[System.Serializable]
		public class CShowWindow
		{
			public bool m_bIsEnablePos = true;
			public bool m_bIsEnableScale = false;
			public bool m_bIsEnableAlpha = false;
		}
		public CShowWindow m_cShow = new CShowWindow();
		[System.Serializable]
		public class CHideWindow
		{
			public bool m_bIsEnablePos = true;
			public bool m_bIsEnableScale = false;
			public bool m_bIsEnableAlpha = false;
		}
		public CHideWindow m_cHide = new CHideWindow();

		public GameObject refImg;

		Vector3 vec = new Vector3(0,0,0);

		// Use this for initialization
		void Start () {
			m_curTimes = 1;
			m_bIsShow = true;
//			vec = this.transform.localPosition;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		/// <summary>
		/// 显示比倍窗口
		/// </summary>
		public void ShowWindow()
		{
			if(m_bIsShow)
			{
				Vector3 vec2 = refImg.transform.localPosition;
				vec = vec2 + new Vector3(0,1300,0);
				ResetPosition();
			
				if(m_cShow.m_bIsEnablePos) ShowPos();
	//			if (m_cShow.m_bIsEnableScale) ShowScale();
				if (m_cShow.m_bIsEnableAlpha)  ShowAlpha();
			}
			else
			{
				Vector3 vec2 = refImg.transform.localPosition;
			}
			
		}

		public void ResetPosition()
		{
			if(m_curTimes <= m_looperTimes)
			{
				this.transform.localPosition = vec;
			}

		}

		public void ShowPos()
		{
			TweenPosition tweenpos = this.GetComponent<TweenPosition>();
			tweenpos.ResetToBeginning();
			if (tweenpos != null && m_bIsShow)
			{
				tweenpos.from = this.transform.localPosition;
				tweenpos.to = m_vShowPos;
				tweenpos.ResetToBeginning();
				tweenpos.enabled = true;
				m_curTimes++;
			}
			if(m_curTimes >= m_looperTimes)
			{
//				this.gameObject.SetActive(false);
				m_curTimes = 0;	
			}
		}
		
		public void ShowScale()
		{
			TweenScale tweenscal = this.GetComponent<TweenScale>();
			if (tweenscal != null && m_bIsShow)
			{
				tweenscal.from = this.transform.localScale;
				tweenscal.to = m_vShowScale;
				tweenscal.ResetToBeginning();
				tweenscal.enabled = true;
			}
		}
		public void ShowAlpha()
		{
			TweenAlpha temp_Alpha = this.GetComponent<TweenAlpha>();
			if (temp_Alpha != null && m_bIsShow)
			{
				temp_Alpha.from = this.transform.GetComponent<UISprite>().alpha;
				temp_Alpha.to = m_fShowAlpha;
				temp_Alpha.ResetToBeginning();
				temp_Alpha.enabled = true;
			}
		}
		/// <summary>
		/// 隐藏比倍窗口
		/// </summary>
		public void HideWindow()
		{
			m_bIsShow = false;
			if (m_cHide.m_bIsEnablePos) HidePos();
			if (m_cHide.m_bIsEnableScale) HideScale();
			if (m_cHide.m_bIsEnableAlpha) HideAlpha();
			
		}
		
		public void HidePos()
		{
			TweenPosition tweenpos = this.GetComponent<TweenPosition>();
			if (tweenpos != null && !m_bIsShow)
			{
				tweenpos.from = this.transform.localPosition;
				tweenpos.to = m_vHidePos;
				tweenpos.ResetToBeginning();
				tweenpos.enabled = true;
			}
		}
		
		public void HideScale()
		{
			TweenScale tweenscal = this.GetComponent<TweenScale>();
			if (tweenscal != null && !m_bIsShow)
			{
				tweenscal.from = this.transform.localScale;
				tweenscal.to = m_vHideScale;
				tweenscal.ResetToBeginning();
				tweenscal.enabled = true;
			}
		}
		
		public void HideAlpha()
		{
			TweenAlpha temp_Alpha = this.GetComponent<TweenAlpha>();
			if (temp_Alpha != null && !m_bIsShow)
			{
				temp_Alpha.from = this.transform.GetComponent<UISprite>().alpha;
				temp_Alpha.to = m_fHideAlpha;
				temp_Alpha.ResetToBeginning();
				temp_Alpha.enabled = true;
			}
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		public void Play()
		{
			ResetPosition();
			m_bIsShow = true;
			ShowWindow();
		}

		public void Stop()
		{
			m_bIsShow = false;
		}
	}	
}
