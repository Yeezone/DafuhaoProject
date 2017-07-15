using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.Fishing
{
/// <summary>
/// 每一个小喇叭.
	/// </summary>
	public class TrumpetItem : MonoBehaviour 
	{
//		// 玩家id.
//		public NumItem plyID;
//		// 获得倍率.
//		public NumItem multi;
//		// 获得分值.
//		public NumItem value;

		// 玩家id.
		public UILabel plyID;
		// 获得倍率.
		public UILabel multi;
		// 获得分值.
		public UILabel value;

		// 一个喇叭的生命周期.
		public float lifeTimeLength = 3f;
		// 回收时间.
		private float targetTime2Recycle;
		// 是否已经回收.
		private bool haveRecycle = false;

		void Awake()
		{
			if(plyID==null)
			{
				plyID = transform.FindChild("plyID").GetComponent<UILabel>();
			}
			if(multi==null)
			{
				multi = transform.FindChild("multi").GetComponent<UILabel>();
			}
			if(value==null)
			{
				value = transform.FindChild("value").GetComponent<UILabel>();
			}

			StartCoroutine ("trumpetItem_Anm");
			gameObject.GetComponent<TweenScale>().enabled = true;
		}

		// 给喇叭赋值.
		public void ApplyParameters (int _plyID, int _multi, int _value) 
		{
			if(plyID!=null)
			{
				plyID.text = (_plyID+1).ToString ();
			}
			if(multi!=null)
			{
				multi.text = _multi.ToString ();
			}
			if(value!=null)
			{
				value.text = _value.ToString ();
			}
		}

		void OnEnable()
		{
			// 出现的时候设置生命周期.
			targetTime2Recycle = Time.time + lifeTimeLength;
			haveRecycle = false;
		}

		void Update()
		{
			if(haveRecycle)
			{
				return;
			}
			// 时间一到就回收.
			if(Time.time>=targetTime2Recycle)
			{
				Factory.Recycle(transform);
				haveRecycle = true;
			}
		}

		IEnumerator trumpetItem_Anm(){
			transform.localScale = new Vector3(10f,10f,1f);
			TweenScale.Begin(gameObject,0.2f,new Vector3(1f,1f,1f));
			yield return new WaitForSeconds(4.8f);
			TweenScale.Begin(gameObject,0.5f,new Vector3(0f,0f,1f));
			yield return new WaitForSeconds(0.5f);
			Destroy (gameObject);

// #if !UNITY_STANDALONE_WIN
// 			transform.localScale = new Vector3(10f,10f,1f);
// 			TweenScale.Begin(gameObject,0.2f,new Vector3(0.5f,0.5f,1f));
// 			yield return new WaitForSeconds(4.8f);
// 			TweenScale.Begin(gameObject,0.5f,new Vector3(0f,0f,1f));
// 			yield return new WaitForSeconds(0.5f);
// 			Destroy (gameObject);
// #endif
		}

	}
}