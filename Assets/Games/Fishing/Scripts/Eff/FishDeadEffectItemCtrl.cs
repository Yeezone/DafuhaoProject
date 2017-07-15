using UnityEngine;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class EffParam
	{
		/*
		[HideInInspector] public bool haveCreatedAudio = false;
		public AudioClip fishDeadAudioClip;
		public bool audioLoopUntilRecycle;
		public float fishDeadAudioTimeLength;
		*/

		public float time2CreateEff;
		[HideInInspector] public float targetTime2PlayEff;
		public Transform eff;
		// public AudioClip effAudioClip;
		public bool fixedOnScreen = true;
		[HideInInspector] public bool haveCreatedPar = false;
		public Vector3 pos;
		public Vector3 euler;
		public Vector3 scale = Vector3.one;

		public bool moveEff = false;
		public Vector3 effMove2Pos;
		public float effMoveTimeLength;
	}


	public class FishDeadEffectItemCtrl : MonoBehaviour 
	{
		private List<EffParam> deadEffList = new List<EffParam>();
		private bool startPlayPar = false;
		private Transform effCache;

		private int createdNum;
		private Transform fish;

		private int deadEffListCount;
		private Vector3 fishLocalPosition;		///记录鱼的当前位置


		public void StartCreateParticle(SingleFish _sf, List<EffParam> _deadEffList, Transform _effCache)
		{
			fish = _sf.transform;
			fishLocalPosition = fish.localPosition;
			deadEffList = _deadEffList;
			effCache = _effCache;

			createdNum = 0;
			startPlayPar = true;

			deadEffListCount = deadEffList.Count;

			for(int i=0; i<deadEffListCount; i++)
			{
				deadEffList[i].haveCreatedPar 	= false;
				deadEffList[i].targetTime2PlayEff 	= Time.time + deadEffList[i].time2CreateEff;

				/*
				deadEffList[i].haveCreatedAudio = false;
				if(deadEffList[i].audioLoopUntilRecycle)
				{
					deadEffList[i].fishDeadAudioTimeLength	= Time.time + _sf.deadTimeLength;
				}
				*/
			}
		}

		void Update () 
		{
			if(startPlayPar)
			{
				for(int i=0; i<deadEffListCount; i++)
				{
					/*
					if(!deadEffList[i].haveCreatedAudio)
					{
						if(deadEffList[i].fishDeadAudioClip!=null)
						{
							if(deadEffList[i].audioLoopUntilRecycle)
							{
								AudioCtrl.Instance.PlayClipUntilLoopFinish(deadEffList[i].fishDeadAudioClip, deadEffList[i].fishDeadAudioTimeLength);
							}
							else 
							{
								AudioSource.PlayClipAtPoint(deadEffList[i].fishDeadAudioClip, Vector3.zero);
							}							
						}
						deadEffList[i].haveCreatedAudio = true;
						createdNum++;
						if(createdNum>=deadEffListCount*2)
						{
							startPlayPar = false;
							Factory.Recycle(transform);
						}
					}
					*/

					if(!deadEffList[i].haveCreatedPar)
					{
						if(Time.time>=deadEffList[i].targetTime2PlayEff)
						{
							if(deadEffList[i].eff!=null)
							{
								Transform tempTrans			= Factory.Create(deadEffList[i].eff, Vector3.zero, Quaternion.identity);
								tempTrans.parent 			= effCache;
								tempTrans.localEulerAngles 	= deadEffList[i].euler;
								tempTrans.localScale 		= deadEffList[i].scale;

								if(deadEffList[i].fixedOnScreen)
								{
									Vector3 _transformPos 	= new Vector3(Utility.device_to_1136x640_ratio.x * deadEffList[i].pos.x, Utility.device_to_1136x640_ratio.y * deadEffList[i].pos.y, 0f);
									tempTrans.localPosition = _transformPos;
								}
								else 
								{
									Vector3 _basePos = deadEffList[i].pos + fishLocalPosition;
//									Vector3 _basePos = deadEffList[i].pos + fish.localPosition;
									Vector3 _transformPos 	= new Vector3(Utility.device_to_1136x640_ratio.x * _basePos.x, Utility.device_to_1136x640_ratio.y * _basePos.y, 0f);
									tempTrans.localPosition = _transformPos;
								}

								if(deadEffList[i].moveEff)
								{
									Vector3 _transformPos 	= new Vector3(Utility.device_to_1136x640_ratio.x * deadEffList[i].effMove2Pos.x, Utility.device_to_1136x640_ratio.y * deadEffList[i].effMove2Pos.y, 0f);
									EffectMove _qpe = tempTrans.GetComponent<EffectMove>(); 
									if(_qpe==null)
									{
										tempTrans.gameObject.AddComponent<EffectMove>().Move(deadEffList[i].effMoveTimeLength, _transformPos); 
									}
									else 
									{
										_qpe.Move(deadEffList[i].effMoveTimeLength, _transformPos); 
									}
								}

	//							if(deadEffList[i].effAudioClip!=null)
	//							{
	//								AudioSource.PlayClipAtPoint(deadEffList[i].effAudioClip, Vector3.zero);
	//							}
							}

							deadEffList[i].haveCreatedPar = true;
							createdNum++;
							if(createdNum>=deadEffListCount)
							{
								startPlayPar = false;
								Factory.Recycle(transform);
							}
						}
					}
				}
			}
		}
	}
}
