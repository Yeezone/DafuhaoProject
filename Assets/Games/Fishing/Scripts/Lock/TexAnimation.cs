using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	public class TexAnimation : MonoBehaviour 
	{
		public int time2PlayNextFrame = 20;
		private int curTime;
		public Texture [] texs;
		private int texNum;
		private Material mat;
		private int index;

		void Start () 
		{
			mat = GetComponent<LineRenderer>().sharedMaterial;
			texNum = texs.Length;
		}
		
		void Update () 
		{
			curTime += 20;
			if(curTime>time2PlayNextFrame)
			{
				curTime = 0;
				index ++;
				if(index>=texNum)
				{
					index = 0;
				}
				if(index>=texNum)
				{
					return;
				}
				mat.SetTexture("_MainTex", texs[index]);
			}
		}
	}
}
