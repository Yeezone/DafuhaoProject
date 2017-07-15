using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class SingleSpecialPrize : MonoBehaviour
    {
        public static SingleSpecialPrize _instance;
        // 特殊开奖_列表_动物模型
        public GameObject[] m_gSPList;
		// 特殊开奖_列表_炸弹贴图
        public Texture[] m_tSmallBallTex;
		// 特殊开奖_列表_炸弹材质
        public Material m_mSmallBall;
        // 特殊开奖_炸弹模型
        public GameObject m_gBoom;

        private float timer;
        // 列表下模型的个数
        private int m_iModelIndex;
		private int m_iTexIndex;
        private int m_iTempIndex = 0;
        // 播放开关
        [HideInInspector]
        public bool m_bIsOpen = false;
        // 旋转组件
        private TweenRotation m_TweenRot;
        // 开奖下标
        [HideInInspector]
        public int m_iPrizeIndex;

        void Start()
        {
            _instance = this;
            m_iModelIndex = m_gSPList.Length;
			m_iTexIndex = m_tSmallBallTex.Length;
            m_TweenRot = GetComponent<TweenRotation>();

            ResetGame_SpecialPrize();
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f && m_bIsOpen)
            {                
                // 判断材质是否为空(检验是否单颜色)
                if (m_mSmallBall == null)
				{
					// 下标自增,并且对列表个数求余
					m_iTempIndex = (m_iTempIndex + 1) % m_iModelIndex;
                    if (m_iTempIndex == 0)
                    {
                        m_gSPList[m_iModelIndex - 1].SetActive(false);
                    }
                    else
                    {
                        m_gSPList[m_iTempIndex - 1].SetActive(false);
                    }
                    m_gSPList[m_iTempIndex].SetActive(true);
                }
                else
                {
					// 下标自增,并且对列表个数求余
					m_iTempIndex = (m_iTempIndex + 1) % m_iTexIndex;
                    m_gBoom.SetActive(true);
                    m_mSmallBall.mainTexture = m_tSmallBallTex[m_iTempIndex];
                }   
                timer = 0.0f;
            }

            // 如果旋转组件关闭,时间到,关闭所有动物,出现开奖动物/颜色(11s后自动关闭)
            if (m_TweenRot.enabled == false && m_bIsOpen)
            {
                 // 判断材质是否为空(检验是否单颜色)
                if (m_mSmallBall == null)
                {
                    for (int i = 0; i < m_gSPList.Length; i++)
                    {
                        m_gSPList[i].SetActive(false);
                    }
                }
               
                StartCoroutine(WaitTime2SetOffModel());
                m_bIsOpen = false;
            }
        }

        IEnumerator WaitTime2SetOffModel()
        {
            if (m_mSmallBall == null)
            {
                m_gSPList[m_iPrizeIndex].SetActive(true);
                if (m_gSPList[m_iPrizeIndex].GetComponentInChildren<Animator>() != null)
                {
                    m_gSPList[m_iPrizeIndex].GetComponentInChildren<Animator>().SetBool("Win", true);
                }

                yield return new WaitForSeconds(11f);

                m_gSPList[m_iPrizeIndex].SetActive(false);
                if (m_gSPList[m_iPrizeIndex].GetComponentInChildren<Animator>() != null)
                {
                    m_gSPList[m_iPrizeIndex].GetComponentInChildren<Animator>().SetBool("Win", false);
                }
            }
            else
            {
                m_mSmallBall.mainTexture = m_tSmallBallTex[m_iPrizeIndex];

                yield return new WaitForSeconds(11f);

                m_gBoom.SetActive(false);
            }            
        }

        /// <summary>
        /// 重置游戏_特殊开奖的动物和效果
        /// </summary>
        public void ResetGame_SpecialPrize()
        {
            if (m_gSPList != null)
            {
                for (int i = 0; i < m_gSPList.Length; i++)
                {
                    m_gSPList[i].SetActive(false);
                }
            }
            if (m_gBoom != null)
            {
                m_gBoom.SetActive(false);
            }
        }

    }
}
