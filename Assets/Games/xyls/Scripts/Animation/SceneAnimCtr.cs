using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class SceneAnimCtr : MonoBehaviour
    {
        public static SceneAnimCtr _instance;

        // 流光材质
        public Material liuGuang;
        // 流光材质偏移值
        private float liuGuang_m_offset;
        // 所有动物的动画控制器
        public Animator[] animalAnimCtr = new Animator[24];
        // 动画控制器的下标
        private int animCtrIndex = 0;
        // 计时器
        [HideInInspector]
        public float timer = 0;

        // 场景中水晶模型的材质
        public Material m_mCrystal;
        // 水晶贴图
        public Texture[] m_tCrystalTexture;
        // 水晶闪烁计数
        [HideInInspector]
        public int m_iCrystalChangeNum = 0;
        // 水晶闪烁多少次
        [HideInInspector]
        public int m_iCrystalFlashNum = 0;
        // 水晶闪烁开关
        [HideInInspector]
        public bool m_bCrystalChange = false;
        // 开奖颜色,用于闪烁水晶
        [HideInInspector]
        public int m_iPrizeColor;

        // 水晶闪烁模式(1:普通模式,2:多颜色闪烁模式)
        [HideInInspector]
        public int m_iCrystalFlashMode = 1;

        void Start()
        {
            _instance = this;
            ResetGame_Crystal();
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void Update()
        {

            timer += Time.deltaTime;

            //         if(timer>=0.02f){
            //             if (animCtrIndex < animalAnimCtr.Length)
            //             {
            //                 animalAnimCtr[animCtrIndex].enabled = true;
            //                 animCtrIndex++;
            //                 timer = 0;
            //             }
            //         }

            liuGuang_m_offset -= Time.deltaTime * 0.8f;
            liuGuang.mainTextureOffset = new Vector2(0, liuGuang_m_offset);

            ShowCrystalAnim(m_iPrizeColor);


        }

        /// <summary>
        /// 闪烁水晶
        /// </summary>
        public void ShowCrystalAnim(int j)
        {
            // i是默认蓝色贴图
            // j是变化后的颜色
            if (timer >= 0.5f && m_bCrystalChange)
            {
                if (m_iCrystalFlashMode == 1)
                {
                    if (m_iCrystalChangeNum % 2 == 0 && m_iCrystalChangeNum < m_iCrystalFlashNum)
                    {
                        m_mCrystal.mainTexture = m_tCrystalTexture[j];
                    }
                    else if (m_iCrystalChangeNum % 2 != 0 && m_iCrystalChangeNum < m_iCrystalFlashNum)
                    {
                        m_mCrystal.mainTexture = m_tCrystalTexture[3];
                    }
                    else if (m_iCrystalChangeNum >= m_iCrystalFlashNum)
                    {
                        m_bCrystalChange = false;
                    }

                    m_iCrystalChangeNum++;
                    timer = 0;
                }
                else if (m_iCrystalFlashMode == 2)
                {
                    if (m_iCrystalChangeNum < m_iCrystalFlashNum)
                    {
                        int temp = m_iCrystalChangeNum % 4;
                        m_mCrystal.mainTexture = m_tCrystalTexture[temp];
                    }
                    else if (m_iCrystalChangeNum >= m_iCrystalFlashNum)
                    {
                        m_bCrystalChange = false;
                    }
                    m_iCrystalChangeNum++;
                    timer = 0;
                }
            }
        }

        /// <summary>
        /// 开始播放动物动画(未使用)
        /// </summary>
        public void PlayAnimalAnim()
        {
            for (int i = 0; i < animalAnimCtr.Length; i++)
            {
                animalAnimCtr[i].Play("Idle");
            }
        }

        /// <summary>
        /// 暂停播放动物动画(未使用)
        /// </summary>
        public void StopAnimalAnim()
        {
            for (int i = 0; i < animalAnimCtr.Length; i++)
            {
                animalAnimCtr[i].StopPlayback();
            }
        }

        /// <summary>
        /// 动物动画_Idle
        /// </summary>
        public void AnimalAnim_Idle()
        {
            for (int index = 0; index < animalAnimCtr.Length; index++)
            {
                animalAnimCtr[index].SetBool("Idle", true);
            }
        }

        /// <summary>
        /// 动物动画_StopIdle
        /// </summary>
        public void AnimalAnim_StopIdle()
        {
            for (int index = 0; index < animalAnimCtr.Length; index++)
            {
                animalAnimCtr[index].SetBool("Idle", false);
            }
        }

        /// <summary>
        /// 动物动画_Win
        /// </summary>
        public void AnimalAnim_Win(GameObject prizeAnimal)
        {
            //prizeAnimal.GetComponent<Animator>().SetBool("Win", true);
            prizeAnimal.GetComponentInChildren<Animator>().SetBool("Win", true);
        }

        /// <summary>
        /// 动物动画_StopWin
        /// </summary>
        public void AnimalAnim_StopWin(GameObject prizeAnimal)
        {
            //prizeAnimal.GetComponent<Animator>().SetBool("Win", false);
            prizeAnimal.GetComponentInChildren<Animator>().SetBool("Win", false);
        }

        /// <summary>
        /// 重置游戏_水晶
        /// </summary>
        public void ResetGame_Crystal()
        {
            // 进入场景初始化为蓝色贴图
            m_mCrystal.mainTexture = m_tCrystalTexture[3];
        }

    }
}
