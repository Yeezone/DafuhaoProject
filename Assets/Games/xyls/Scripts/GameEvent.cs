using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.xyls
{

    [System.Serializable]
    public class AccountPanel_Simple
    {        
        // 结算面板
        public GameObject m_gAccountPanel;
        // 动物倍率数字
        public CLabelNum m_gAnimalNum;
        // 庄闲和倍率数字
        public CLabelNum m_gBankerNum;
        // 玩家得分
        public UILabel m_uPlayScore;
        // 开奖动物logo
        public UISprite m_tAnimalLogo;
        // 开奖庄闲和logo
        public UISprite m_tBankerLogo;
    }
    [System.Serializable]
    public class AccountPanel_SingleColor
    {
        // 结算面板
        public GameObject m_gAccountPanel;
        // 动物倍率数字
        public CLabelNum m_gAnimalNum1;
        public CLabelNum m_gAnimalNum2;
        public CLabelNum m_gAnimalNum3;
        public CLabelNum m_gAnimalNum4;
        // 庄闲和倍率数字
        public CLabelNum m_gBankerNum;
        // 玩家得分
        public UILabel m_uPlayScore;
        // 开奖动物logo
        public UISprite m_tAnimalLogo1;
        public UISprite m_tAnimalLogo2;
        public UISprite m_tAnimalLogo3;
        public UISprite m_tAnimalLogo4;
        // 开奖庄闲和logo
        public UISprite m_tBankerLogo;
    }
    [System.Serializable]
    public class AccountPanel_SingleAnimal
    {
        // 结算面板
        public GameObject m_gAccountPanel;
        // 动物倍率数字
        public CLabelNum m_gAnimalNum1;
        public CLabelNum m_gAnimalNum2;
        public CLabelNum m_gAnimalNum3;
        // 庄闲和倍率数字
        public CLabelNum m_gBankerNum;
        // 玩家得分
        public UILabel m_uPlayScore;
        // 开奖动物logo
        public UISprite m_tAnimalLogo1;
        public UISprite m_tAnimalLogo2;
        public UISprite m_tAnimalLogo3;
        // 开奖庄闲和logo
        public UISprite m_tBankerLogo;
    }
    [System.Serializable]
    public class AccountPanel_SysPrize
    {
        // 结算面板
        public GameObject m_gAccountPanel;
        // 动物倍率数字
        public CLabelNum m_gAnimalNum;
        // 庄闲和倍率数字
        public CLabelNum m_gBankerNum;
        // 玩家得分
        public UILabel m_uPlayScore;
        // 彩金
        public UILabel m_uSysPrize;
        // 开奖动物logo
        public UISprite m_tAnimalLogo;
        // 开奖庄闲和logo
        public UISprite m_tBankerLogo;
    }
    [System.Serializable]
    public class AccountPanel_Repeat
    {
        // 结算面板
        public GameObject m_gAccountPanel;
        // 动物倍率数字
        public CLabelNum m_gAnimalNum1;
        public CLabelNum m_gAnimalNum2;
        // 庄闲和倍率数字
        public CLabelNum m_gBankerNum;
        // 玩家得分
        public UILabel m_uPlayScore;
        // 开奖动物logo
        public UISprite m_tAnimalLogo1;
        public UISprite m_tAnimalLogo2;
        // 开奖庄闲和logo
        public UISprite m_tBankerLogo;
    }

	public class GameEvent : MonoBehaviour {

        public static GameEvent _instance;

        // 庄闲和挂载的脚本
        public ModelRot_Banker m_mBanker;

        // 金币面板
        public UILabel m_gGoldPanel;
        // 缓存彩金
        public long m_lBounsNum = 0;
        // 实际筹码值
        //[HideInInspector]
		public int m_iCurChipNum;
        // 时间面板
        public GameObject m_gTimerPanel;
        // 结算面板
        public AccountPanel_Simple m_Account_simple;
        public AccountPanel_SingleColor m_Account_singleColor;
        public AccountPanel_SingleAnimal m_Account_singleAnimal;
        public AccountPanel_SysPrize m_Account_sysPrize;
        public AccountPanel_Repeat m_Account_repeat;
        // 色板的渲染器
        public Renderer[] m_rSeban;
        // 色板的三种颜色材质
        public Material[] m_mSebanMaterials;
        // 开奖粒子效果
        public GameObject m_gEffect;
        public ParticleSystem m_pEffect01;

        // 彩金Tip提示
        public GameObject[] m_arrobjPrizeTips;

		void Awake () {
            _instance = this;
		}

        void Update()
        {
           
        }

        void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// 生成转盘
        /// </summary>
        public void CreateTrun(int[] trunColor)
        {
            for (int i = 0; i < trunColor.Length; i++)
            {
                if (trunColor[i]==0)
                {
                    //m_rSeban[i].material.color = Color.red;
                    m_rSeban[i].material = m_mSebanMaterials[0];
                }
                else if (trunColor[i] == 1)
                {
                    //m_rSeban[i].material.color = Color.green;
                    m_rSeban[i].material = m_mSebanMaterials[1];
                }
                else if (trunColor[i] == 2)
                {
                    //m_rSeban[i].material.color = Color.yellow;
                    m_rSeban[i].material = m_mSebanMaterials[2];
                }
                else
                {
                    Debug.LogError("未知颜色");
                }
            }
        }

        /// <summary>
        /// 旋转指针和动物指定到开奖结果
        /// </summary>
        public void ShowAllAnimation(int animal, int enjoyGameType, int[] prizeIndex)
        {
            // 获取到开奖结果的位置.
            //float axisY = prizeIndex[0] * 15;

            // 旋转动画
            ModelAnimation._instance.RotModel_zhizhen(animal, prizeIndex, enjoyGameType);

            // 庄闲和
//             m_mBanker.m_bIsOpen = true;
//             m_mBanker.m_fTimer = 0;
//             m_mBanker.m_iTarget = enjoyGameType; 

//             if (prizeMode==0)
//             {
//                 // 正常模式:
//                 // 移动动物和播放Win动画(旋转动物包括移动相机)
//                 ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SimplePrizeAnim(_animal, axisY, color, animal, prizeMode));
//             }
//             else if (prizeMode == 1)
//             {
//                 // 单颜色:只闪烁水晶
//                 ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SingleColorAnim(color));
//             }
//             else if (prizeMode == 2)
//             {
//                 // 单动物:只闪烁水晶
//                 ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SingleAnimalAnim(axisY, color, animal));
//             }
//             else if (prizeMode == 3)
//             {
//                 // 系统彩金:
//                 // 移动动物和播放Win动画(旋转动物包括移动相机)
//                 ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SysPrizeAnim(_animal, axisY, color, animal, prizeMode));
//             }
//             else if (prizeMode == 4)
//             {
//                 // 转动一次指针后,重复转动第二次
//                 //ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.RotModel_repeat(prizeIndex));
//                 // 正常模式:
//                 // 移动动物和播放Win动画(旋转动物包括移动相机)
//                 //ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SimplePrizeAnim(_animal, axisY, color, animal, prizeMode));
//             }
//             else if (prizeMode == 5)
//             {
//                 // 正常模式:
//                 // 移动动物和播放Win动画(旋转动物包括移动相机)
//                 ModelAnimation._instance.StartCoroutine(ModelAnimation._instance.SimplePrizeAnim(_animal, axisY, color, animal, prizeMode));
//             }
//             else
//             {
//                 Debug.LogError("场景动画:未知开奖结果");
//             }

            // 庄闲和
//             if (enjoyGameType==0)
//             {
//                 ModelAnimation._instance.target_enjoyGameType_temp = -0.03f;
//             }
//             else if (enjoyGameType == 1)
//             {
//                 ModelAnimation._instance.target_enjoyGameType_temp = -0.36f;
//             }
//             else if (enjoyGameType == 2)
//             {
//                 ModelAnimation._instance.target_enjoyGameType_temp = -0.69f;
//             }
//             ModelAnimation._instance.RotModel_enjoyGameType();
        }   

        /// <summary>
        /// 设置金币显示
        /// </summary>
        /// <param name="gold"></param> 
        public void SetGoldNum(long gold)
        {
            m_gGoldPanel.text = gold.ToString("N0");
        }

        /// <summary>
        /// 更新金币数量
        /// </summary> 
        public void UpdateGoldNum(int num)
        {
            CUIGame._instance.m_iCurGoldNum =CUIGame._instance.m_iCurGoldNum_temp - num;
            SetGoldNum(CUIGame._instance.m_iCurGoldNum);
        }
        
        /// <summary>
        /// 时间面板的移动动画
        /// </summary>
        public IEnumerator MoveTimerAnim(float waitTime)
        {
            m_gTimerPanel.SetActive(true);
            yield return new WaitForSeconds(waitTime-1);
            m_gTimerPanel.SetActive(false);
            //位移动画(暂时不使用)
//             m_gTimerPanel.GetComponent<TweenPosition>().from = new Vector3(765, 320, 0);
//             m_gTimerPanel.GetComponent<TweenPosition>().to = new Vector3(565, 320, 0);
//             m_gTimerPanel.GetComponent<TweenPosition>().enabled = true;
//             m_gTimerPanel.GetComponent<TweenPosition>().ResetToBeginning();
// 
//             yield return new WaitForSeconds(waitTime-1);
// 
//             m_gTimerPanel.GetComponent<TweenPosition>().from = new Vector3(565, 320, 0);
//             m_gTimerPanel.GetComponent<TweenPosition>().to = new Vector3(765, 320, 0);
//             m_gTimerPanel.GetComponent<TweenPosition>().enabled = true;
//             m_gTimerPanel.GetComponent<TweenPosition>().ResetToBeginning();
        }

        ///////////////////////////结算面板:正常开奖///////////////////////////////////////////
        /// <summary>
        /// 设置结算面板_正常模式
        /// </summary>
        public void SetAccountPanel_Simple(int animal, int color, int enjoyGame, int playerScore)
        {
            m_Account_simple.m_gAnimalNum.m_iNum = CUIGame._instance.m_iAnimalRatio[animal, color];
            m_Account_simple.m_gBankerNum.m_iNum = CUIGame._instance.m_iEnjoyGameRatio[enjoyGame];
            m_Account_simple.m_uPlayScore.text = playerScore.ToString();

            m_Account_simple.m_tAnimalLogo.spriteName = "animal_" + animal + "_" + color;
            m_Account_simple.m_tBankerLogo.spriteName = "banker_" + enjoyGame;
        }
        /// <summary>
        /// 显示结算面板_正常
        /// </summary>
        public IEnumerator ShowAccountPanel_simple(float waitTime)
        {
            yield return new WaitForSeconds(waitTime-1);
            // 显示彩金减少的效果
            RandomPrizeNum._instance.m_BounsNum.m_iNum -= m_lBounsNum;
            // 每次执行完,缓存的彩金都需要清空
            m_lBounsNum = 0;

            yield return new WaitForSeconds(1);
            TweenAlpha temp_ta = m_Account_simple.m_gAccountPanel.GetComponent<TweenAlpha>();
            temp_ta.PlayForward();
//             temp_ta.from = 0;
//             temp_ta.to = 1;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();

            yield return new WaitForSeconds(3f);

            temp_ta.PlayReverse();
//             temp_ta.from = 1;
//             temp_ta.to = 0;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();
        }
        ///////////////////////////结算面板:单颜色///////////////////////////////////////////
        /// <summary>
        /// 设置结算面板_单颜色
        /// </summary>
        public void SetAccountPanel_SingleColor(int color, int enjoyGame, int playerScore)
        {
            //倍率
            m_Account_singleColor.m_gAnimalNum1.m_iNum = CUIGame._instance.m_iAnimalRatio[0, color];
            m_Account_singleColor.m_gAnimalNum2.m_iNum = CUIGame._instance.m_iAnimalRatio[1, color];
            m_Account_singleColor.m_gAnimalNum3.m_iNum = CUIGame._instance.m_iAnimalRatio[2, color];
            m_Account_singleColor.m_gAnimalNum4.m_iNum = CUIGame._instance.m_iAnimalRatio[3, color];

            m_Account_singleColor.m_gBankerNum.m_iNum = CUIGame._instance.m_iEnjoyGameRatio[enjoyGame];
            m_Account_singleColor.m_uPlayScore.text = playerScore.ToString();
            //logo
            m_Account_singleColor.m_tAnimalLogo1.spriteName = "animal_0" + "_" + color;
            m_Account_singleColor.m_tAnimalLogo2.spriteName = "animal_1" + "_" + color;
            m_Account_singleColor.m_tAnimalLogo3.spriteName = "animal_2" + "_" + color;
            m_Account_singleColor.m_tAnimalLogo4.spriteName = "animal_3" + "_" + color;
            m_Account_singleColor.m_tBankerLogo.spriteName = "banker_" + enjoyGame;
        }
        /// <summary>
        /// 显示结算面板_单颜色
        /// </summary>
        public IEnumerator ShowAccountPanel_singleColor(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            TweenAlpha temp_ta = m_Account_singleColor.m_gAccountPanel.GetComponent<TweenAlpha>();
            temp_ta.PlayForward();
//             temp_ta.from = 0;
//             temp_ta.to = 1;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();

            yield return new WaitForSeconds(3f);

            temp_ta.PlayReverse();
//             temp_ta.from = 1;
//             temp_ta.to = 0;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();
        }
        ///////////////////////////结算面板:单动物///////////////////////////////////////////
        /// <summary>
        /// 设置结算面板_单颜色
        /// </summary>
        public void SetAccountPanel_SingleAnimal(int animal, int enjoyGame, int playerScore)
        {
            // 倍率
            m_Account_singleAnimal.m_gAnimalNum1.m_iNum = CUIGame._instance.m_iAnimalRatio[animal, 0];
            m_Account_singleAnimal.m_gAnimalNum2.m_iNum = CUIGame._instance.m_iAnimalRatio[animal, 1];
            m_Account_singleAnimal.m_gAnimalNum3.m_iNum = CUIGame._instance.m_iAnimalRatio[animal, 2];
            m_Account_singleAnimal.m_gBankerNum.m_iNum = CUIGame._instance.m_iEnjoyGameRatio[enjoyGame];
            m_Account_singleAnimal.m_uPlayScore.text = playerScore.ToString();
            //logo
            m_Account_singleAnimal.m_tAnimalLogo1.spriteName = "animal_" + animal + "_0";
            m_Account_singleAnimal.m_tAnimalLogo2.spriteName = "animal_" + animal + "_1";
            m_Account_singleAnimal.m_tAnimalLogo3.spriteName = "animal_" + animal + "_2";
            m_Account_singleAnimal.m_tBankerLogo.spriteName = "banker_" + enjoyGame;
        }
        /// <summary>
        /// 显示结算面板_单动物
        /// </summary>
        public IEnumerator ShowAccountPanel_singleAnimal(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            TweenAlpha temp_ta = m_Account_singleAnimal.m_gAccountPanel.GetComponent<TweenAlpha>();
            temp_ta.PlayForward();
//             temp_ta.from = 0;
//             temp_ta.to = 1;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();

            yield return new WaitForSeconds(3f);

            temp_ta.PlayReverse();
//             temp_ta.from = 1;
//             temp_ta.to = 0;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();
        }
        ///////////////////////////结算面板:彩金///////////////////////////////////////////
        /// <summary>
        /// 设置结算面板_彩金
        /// </summary>
        public void SetAccountPanel_SysPrize(int animal, int color, int enjoyGame, int playerScore, int sysPrizeScore)
        {
            m_Account_sysPrize.m_gAnimalNum.m_iNum = CUIGame._instance.m_iAnimalRatio[animal, color];
            m_Account_sysPrize.m_gBankerNum.m_iNum = CUIGame._instance.m_iEnjoyGameRatio[enjoyGame];
            m_Account_sysPrize.m_uPlayScore.text = playerScore.ToString();
            m_Account_sysPrize.m_uSysPrize.text = sysPrizeScore.ToString();

            m_Account_sysPrize.m_tAnimalLogo.spriteName = "animal_" + animal + "_" + color;
            m_Account_sysPrize.m_tBankerLogo.spriteName = "banker_" + enjoyGame;
        }
        /// <summary>
        /// 显示结算面板_彩金
        /// </summary>
        public IEnumerator ShowAccountPanel_sysPrize(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            TweenAlpha temp_ta = m_Account_sysPrize.m_gAccountPanel.GetComponent<TweenAlpha>();
            temp_ta.PlayForward();
//             temp_ta.from = 0;
//             temp_ta.to = 1;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();

            yield return new WaitForSeconds(3f);

            temp_ta.PlayReverse();
//             temp_ta.from = 1;
//             temp_ta.to = 0;
//             temp_ta.enabled = true;
//             temp_ta.ResetToBeginning();
        }
        ///////////////////////////结算面板:重复开奖///////////////////////////////////////////
        /// <summary>
        /// 设置结算面板_重复开奖
        /// </summary>
        public void SetAccountPanel_Repeat(int[] animal, int[] color, int enjoyGame, int playerScore)
        {
            // 倍率
            m_Account_repeat.m_gAnimalNum1.m_iNum = CUIGame._instance.m_iAnimalRatio[animal[0], color[0]];
            m_Account_repeat.m_gAnimalNum2.m_iNum = CUIGame._instance.m_iAnimalRatio[animal[1], color[1]];
            m_Account_repeat.m_gBankerNum.m_iNum = CUIGame._instance.m_iEnjoyGameRatio[enjoyGame];
            m_Account_repeat.m_uPlayScore.text = playerScore.ToString();
            //logo
            m_Account_repeat.m_tAnimalLogo1.spriteName = "animal_" + animal[0] + "_" + color[0];
            m_Account_repeat.m_tAnimalLogo2.spriteName = "animal_" + animal[1] + "_" + color[1];
            m_Account_repeat.m_tBankerLogo.spriteName = "banker_" + enjoyGame;
        }
        /// <summary>
        /// 显示结算面板_重复开奖
        /// </summary>
        public IEnumerator ShowAccountPanel_repeat(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            TweenAlpha temp_ta = m_Account_repeat.m_gAccountPanel.GetComponent<TweenAlpha>();
            temp_ta.PlayForward();
            //             temp_ta.from = 0;
            //             temp_ta.to = 1;
            //             temp_ta.enabled = true;
            //             temp_ta.ResetToBeginning();

            yield return new WaitForSeconds(3f);

            temp_ta.PlayReverse();
            //             temp_ta.from = 1;
            //             temp_ta.to = 0;
            //             temp_ta.enabled = true;
            //             temp_ta.ResetToBeginning();
        }
        ////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 更新开奖记录
        /// </summary>
        public IEnumerator UpdatePrizeRecord(float waitTime, int[] animal,int[] color,int enjoyGameType,int prizeType)
        {
            yield return new WaitForSeconds(waitTime);

            GamePrizeRecord prizeRecord;
            prizeRecord.animalIndex = animal;
            prizeRecord.colorIndex = color;
            prizeRecord.enjoyGameIndex = enjoyGameType;
            prizeRecord.gameType = prizeType;

            RecordManager._instance.AddRecord(prizeRecord);
            RecordManager._instance.Right_Onclick();
        }

        /// <summary>
        /// 显示开奖粒子特效
        /// </summary>
        public IEnumerator ShowEffect(float waitTime, int color)
        {
            yield return new WaitForSeconds(waitTime);
            if (color == 0)
            {
                m_pEffect01.startColor = Color.red;
            }
            else if (color == 1)
            {
                m_pEffect01.startColor = Color.green;
            }
            else if (color == 2)
            {
                m_pEffect01.startColor = Color.yellow;
            }
            else
            {
                Debug.LogError("粒子特效:未知颜色");
            }
            m_gEffect.SetActive(true);
            m_pEffect01.Play();
            yield return new WaitForSeconds(6f);
            m_gEffect.SetActive(false);
        }

        
	}
}
