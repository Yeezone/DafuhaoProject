using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.BRPM
{
	public class DoorControl : MonoBehaviour {

		public GameObject[] door = new GameObject[GameXY.HORSES_ALL];

		//播放动画
		public void PlayAnimation()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				door[i].transform.GetComponent<PMAnimation>().Play();
			}
		}

		//停止播放
		public void StopAnimation()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				door[i].transform.GetComponent<PMAnimation>().Stop();
			}
		}

		//初始化贴图
		public void StartTextures()
		{
			for(int i=0; i<GameXY.HORSES_ALL; i++)
			{
				door[i].transform.GetComponent<PMAnimation>().RecetAnimation();
			}
		}
	}
}

