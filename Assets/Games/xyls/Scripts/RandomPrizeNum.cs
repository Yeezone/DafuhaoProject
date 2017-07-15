using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
	public class RandomPrizeNum : MonoBehaviour {
		
		public static RandomPrizeNum _instance;
		
		// 彩金变化开关
		public bool m_bIsOpen = false;
		// 存储每局彩金变化的数组
		private long[] m_arrPrizeNum = new long[16];
		// 计时器
		private float m_fTimer = 0;
		private float m_fTmepTime = 0;
		// 彩金面板
		public CLabelNum m_BounsNum;
		// 持续时间
		public float m_fKeepTime = 10.0f;
		
		void Start () 
		{
			_instance = this;
		}
		
		void OnDestroy()
		{
			_instance = null;
		}
		
		void Update () 
		{
			if(m_bIsOpen)
			{
				m_fTmepTime += Time.deltaTime;
				if(m_fTmepTime >= m_fKeepTime)
				{
					m_bIsOpen = false;
					m_fTmepTime = 0;
				}
				// 每0.3s从列表取一个值显示到彩金面板
				if(Time.time - m_fTimer > 0.3f)
				{
					int temp = Random.Range(0,16);
					long prizeNum = m_arrPrizeNum[temp];
					m_BounsNum.m_iNum = prizeNum;
					m_fTimer = Time.time;
				}
			}
		}
		
		public void randomPrizeNum(long num)
		{
			// 每局开始,获取16个彩金切换值(从百分之50到百分之200)
			for(int i = 0; i < 16; i++)
			{
				m_arrPrizeNum[i] =(long)(0.1f * (i + 5) * num);
			}
		}
		
	}
}
