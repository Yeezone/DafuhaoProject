using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.BRPM
{
	public class ShowTweenControl : MonoBehaviour {

		//显示间隔
		public float showTime = 0;

		//显示物体下标
		public int showIndex = 0;

		//动画是否播放
		public bool IsTween = false;

		//计时
		public float time = 0;

		//显示的物体
		public List<GameObject> showObj = new List<GameObject>();

		void Update()
		{
			if(IsTween == true)
			{
				time += Time.deltaTime;
				if(time >= showTime)
				{
					time = 0;
					if(showIndex >= showObj.Count)
					{
						IsTween = false;
						showIndex = -1;
					}
					else
					{
						ShowObj();
					}
					showIndex++;
				}
			}
		}

		//生成物体
		void ShowObj()
		{
			GameObject obj = Instantiate(showObj[showIndex], Vector3.zero, Quaternion.identity) as GameObject;
			obj.transform.parent = this.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;

			obj.transform.GetComponent<DestroyControl>().destroyTime = showTime;
			PlayerTween(obj.transform);

			foreach(Transform child in obj.transform)
			{
				if(child != null)
				{
					PlayerTween(child);
				}
			}
		}

		//检查物体身上是否挂有动画组件
		void PlayerTween(Transform obj)
		{
			if(obj.GetComponent<TweenAlpha>() != null)
			{
				obj.GetComponent<TweenAlpha>().PlayForward();
			}
			if(obj.GetComponent<TweenPosition>() != null)
			{
				obj.GetComponent<TweenPosition>().PlayForward();
			}
			if(obj.GetComponent<TweenRotation>() != null)
			{
				obj.GetComponent<TweenRotation>().PlayForward();
			}
			if(obj.GetComponent<TweenScale>() != null)
			{
				obj.GetComponent<TweenScale>().PlayForward();
			}
			if(obj.GetComponent<TweenColor>() != null)
			{
				obj.GetComponent<TweenColor>().PlayForward();
			}

		}
	}
}


