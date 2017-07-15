using UnityEngine;
using System.Collections;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Utility;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.xyls
{
    public class SingleAnimal : MonoBehaviour
    {

        // 每个小面板下--倍率
        public UILabel doubelNum;
        // 每个小面板下--我的押分
        public UILabel mBetNum;
        // 每个小面板下--总
        public UILabel totalBetNum;

        //[HideInInspector]
        public int animalType;
        //[HideInInspector]
        public int colorType;


        void Awake()
        {
            
        }

        void Start()
        {

        }

        /// <summary>
        /// 显示动物倍率
        /// </summary>
        /// <param name="_doubelNum"></param>
        public void SetBetDoubelNum(int _doubelNum)
        {
            doubelNum.text = _doubelNum.ToString();
        }

        /// <summary>
        /// 显示押分数值
        /// </summary>
        /// <param name="_mBetNum"></param>
        public void SetBetNum(int _mBetNum)
        {
            mBetNum.text = _mBetNum.ToString("N0");
        }

		/// <summary>
		/// 显示总押分数值
		/// </summary>
		/// <param name="_totalBetNum"></param>
		public void SetTotalBetNum(int _totalBetNum)
		{
            totalBetNum.text = _totalBetNum.ToString("N0");
		}

		/// <summary>
		/// 玩家点击动物按钮押分,发送数据包
		/// </summary>
        public void AnimalButtonOnClick()
        {
			CUIGame._instance.AnimalButtonOnClick(animalType,colorType);
        }

    }
}
