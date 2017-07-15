using UnityEngine;
using System.Collections;
using System;

namespace com.QH.QPGame.BRPM
{
	public class HorseControl : MonoBehaviour {
        private GameObject o_Result = null;
        private int[] areaMultiple = new int[GameXY.AREA_ALL];                      //区域倍数
        private long[] m_lAreaInAllScore = new long[15];		                    //每个区域下注总分(取值)
        private long m_UpdateScore = 0;                                             //更新玩家得分
        private long m_LastUpdateScore = 0;                                         //更新玩家得分                               
        private long updatescore = 0;
        private long m_chipLow = 1;

        public GameObject o_UpdateScore; 
        //相机效果预制
        public GameObject cameraEffect;
        //赛马结果预制
        public GameObject o_GameResultData;
        //控制的马匹
        public GameObject[] horse = new GameObject[GameXY.HORSES_ALL];
        //马匹动画速度
        public float[] tweenSpeed = new float[5];
        //跟随相机
        public GameObject horseCamera = null;

		//显示马头像
		public GameObject horseHead1 = null;
		public GameObject horseHead2 = null;
		public GameObject[] mapHorse = new GameObject[GameXY.HORSES_ALL];

		//中奖马匹
		public int first = 0;
		public int secound = 0;
        public int multipleResult = 0;
        //记录马匹位置
        private float[] horseDistance = new float[GameXY.HORSES_ALL];
        private int[] horseSotr = new int[GameXY.HORSES_ALL];

		//马在每秒中的加速度加速度
		public int[,] addSpeed = new int[GameXY.HORSES_ALL,GameXY.HORSES_TIME]; 
		//动画的暂停与播放
		public bool IsPlay = false;
		//跑马是否开始
		public bool IsChageSpeed = false;
		//动画变换加速度的时间间隔
		private int speedTime = 2;
		//变换加速度的时间下标
		private int indexTime = 0;
		//马移动的平均单位
		private float moveUnit = 560;
		//变换移动范围
		private float addMoveValue = 0.12f;

        //马匹初始位置
        public float startHorsePos = -450.0f;
        //起点到终点的距离(需要是6的倍数)
        public float allDistance = 0;
		//马多跑出的时间（相对于服务器）
		private float surpassTime = 0;
        //初始速度(服务器取值范围)
        private float speed = 200.0f;
        //计时
        private float time = 0;

		//相机移动控制
        public float camerMoveCondition = -130; //马匹跑出该点时相机跟随
		private float totalTime = 0;
        public float cameraFinishDistance = 300;

        //游戏结束是否马移动
        public bool horseMoveEnd = false;
        //相机是否移动
        private bool IsMove = true;

        private int tweeenSpeedIndex = 0;
        public int[,] tweenSpeedIndexChange = new int[GameXY.HORSES_ALL, GameXY.HORSES_TIME];

        void Start()
		{
            o_Result = transform.parent.parent.Find("ResultPanel").gameObject;
			StartData();

			if(IsPlay == false)
			{
				StopAnimation();
			}
			else
			{
				PlayAnimation();
			}

            for (int i = 0; i < GameXY.HORSES_ALL; i++)
            {
                horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime = tweenSpeed[1];	
            }
        }

        float changeTime = 0;
		void FixedUpdate()
		{
//             if (updatescore != m_UpdateScore)
//             {
//                 if (updatescore < m_UpdateScore)
//                 {
//                     updatescore += m_chipLow;
//                     o_UpdateScore.GetComponent<label_number>().m_iNum = updatescore;
//                 }
//                 else
//                 {
//                     updatescore -= m_chipLow;
//                     o_UpdateScore.GetComponent<label_number>().m_iNum = updatescore;
//                 }
//             }


			if(IsChageSpeed)
			{
				totalTime += Time.deltaTime;
				time += Time.deltaTime;
                changeTime += Time.deltaTime;

				HorseMove();
				SortHorse();
                if (horse[0].transform.localPosition.x >= camerMoveCondition && IsMove == true)
				{
					CameraControl();
				}
				if(time >= speedTime)
				{
					time = 0;  
					indexTime++;
				}
                if (changeTime >= 0.7f)
                {
                    HorseTween();
                    changeTime = 0;
                }
			}
		}
		//初始化数据
		public void StartData()
		{
			indexTime = 0;
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime = 0.034f;
			}
		}

		//播放动画
		public void PlayAnimation()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				horse[i].transform.GetComponent<PMAnimation>().Play();
			}
		}
		
		//停止播放
		public void StopAnimation()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				horse[i].transform.GetComponent<PMAnimation>().Stop();
			}
		}
        //写入下注数据
        public void SetBetScore(int[] areamultiple, long[] lAreaInAllScore, long chipLow)
        {
            m_chipLow = chipLow;
            for (int i = 0; i < GameXY.AREA_ALL; i++)
            {
                areaMultiple[i] = areamultiple[i];
                m_lAreaInAllScore[i] = lAreaInAllScore[i];
            }
        }


        //马匹动画速度变换
        void HorseTween()
        {
            for (int i = 0; i < GameXY.HORSES_ALL; i++)
            {
                if (indexTime > GameXY.HORSES_TIME - 1)
                {
                    indexTime = GameXY.HORSES_TIME - 1;
                }

                int count = 0;
                if (addSpeed[i, indexTime] >= 150) count = 0;
                if (addSpeed[i, indexTime] < 150 && addSpeed[i, indexTime] >= 0) count = 1;
                if (addSpeed[i, indexTime] < 0 && addSpeed[i, indexTime] >= -150) count = 2;
                if (addSpeed[i, indexTime] < -150) count = 3;

                if (horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime > tweenSpeed[count])
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime == tweenSpeed[k])
                        {
                            horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime = tweenSpeed[--k];
                        }
                    }
                }
                else if (horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime < tweenSpeed[count])
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime == tweenSpeed[k])
                        {
                            horse[i].transform.GetComponent<PMAnimation>().m_fPerFrameTime = tweenSpeed[++k];
                        }
                    }
                }

            }
        }

		//马匹移动速度
		void HorseMove()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				if(indexTime > GameXY.HORSES_TIME - 1)
                {
					indexTime = GameXY.HORSES_TIME - 1;
				}

                horse[i].transform.localPosition += new Vector3(Time.deltaTime * (moveUnit + (addSpeed[i, indexTime] * addMoveValue * moveUnit) / speed), 0, 0);

                if (horseMoveEnd == false)
                {
               		mapHorse[i].transform.localPosition
                    += new Vector3(Time.deltaTime * (moveUnit / ((allDistance - startHorsePos) / 400) + (addSpeed[i, indexTime] * addMoveValue * moveUnit) / (speed * ((allDistance - startHorsePos) / 400))), 0, 0);
                }

                if (horse[secound].transform.localPosition.x >= allDistance && horseMoveEnd == false)
				{
                    
                    AudioSoundManger_PM._instance.StopHorseSound();
                    StartCoroutine(SetResultData());
                    StartCoroutine(ShowResult());
					StopAnimation();
					IsChageSpeed = false;
                    GameObject obj = (GameObject)Instantiate(cameraEffect, Vector3.zero, Quaternion.identity);
                    obj.transform.parent = transform.Find("CameraParent");
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;

                    horseHead1.transform.GetComponent<UISprite>().spriteName = "fj_" + (first + 1).ToString();
                    horseHead2.transform.GetComponent<UISprite>().spriteName = "fj_" + (secound + 1).ToString();
					return;
				}
			}

		}

		//相机移动
		void CameraControl()
		{
            if (horseCamera.transform.localPosition.x < allDistance - cameraFinishDistance)
			{
				horseCamera.transform.localPosition += new Vector3(Time.deltaTime*moveUnit, 0, 0);
			}
			else
			{
				IsMove = false;
			}
		}

		//初始化贴图动画
		public void StartTextures()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				horse[i].transform.GetComponent<PMAnimation>().RecetAnimation();
			}
		}

		//排序名次
		void SortHorse()
		{
            if (horseMoveEnd != false) return;

			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				horseDistance[i] = horse[i].transform.localPosition.x;
			}

			int nFirst = GameXY.HORSES_ALL;
			int nSecond = GameXY.HORSES_ALL;

			//第一名
			float nTemp = 0;
			for(int nHorses = 0; nHorses < GameXY.HORSES_ALL; ++nHorses)
			{
				if ( nTemp == 0 || nTemp < horseDistance[nHorses] )
				{
					nTemp = horseDistance[nHorses];
					nFirst = nHorses;
				}
			}
			
			//第二名
			nTemp = 0;
			for(int nHorses = 0; nHorses < GameXY.HORSES_ALL; ++nHorses)
			{
				if ( nHorses != nFirst && ( nTemp == 0 || nTemp < horseDistance[nHorses] ))
				{
					nTemp = horseDistance[nHorses];
					nSecond = nHorses;
				}
			}

			horseHead1.transform.GetComponent<UISprite>().spriteName = "fj_" + (nFirst+1).ToString();
			horseHead2.transform.GetComponent<UISprite>().spriteName = "fj_" + (nSecond+1).ToString();
            ShowResultScore(nFirst, nSecond);
		}

        IEnumerator ShowResult()
        {
            yield return new WaitForSeconds(3.0f);
            o_Result.SetActive(true);
            o_Result.transform.GetComponent<TweenScale>().enabled = true;
            o_Result.transform.GetComponent<TweenScale>().PlayForward();
        }

        //显示玩家得分
        void ShowResultScore(int nFirst, int nSecond)
        {
            int areaid = 0;
            for (int i = 0; i < GameXY.HORSES_ALL - 1; i++)
            {
                for (int k = 0; k < GameXY.HORSES_ALL - i - 1; k++)
                {
                    if ((nFirst == i  && nSecond == GameXY.HORSES_ALL - k - 1)
                        || (nSecond == i && nFirst == GameXY.HORSES_ALL - k - 1))
                    {                        
                        m_UpdateScore = areaMultiple[areaid] * m_lAreaInAllScore[areaid];
                        o_UpdateScore.GetComponent<label_number>().m_iNum = m_UpdateScore;
                        //m_chipLow = (m_UpdateScore - m_LastUpdateScore) / 100;
                    }
                    areaid++;
                }
           }
        }


        //生成结果
        IEnumerator SetResultData()
        {
            yield return new WaitForSeconds(1.0f);
            GameObject obj = (GameObject)Instantiate(o_GameResultData, Vector3.zero, Quaternion.identity);
            obj.transform.parent = transform.Find("CameraParent");
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            obj.transform.Find("resultdata1").GetComponent<label_number>().m_iNum = first + 1;
            obj.transform.Find("resultdata2").GetComponent<label_number>().m_iNum = secound + 1;
            obj.transform.Find("resultdata_multiple").GetComponent<label_number>().m_iNum = multipleResult;
        }

	}
}


