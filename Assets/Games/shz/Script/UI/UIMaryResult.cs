using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.SHZ
{ 
	public class UIMaryResult : MonoBehaviour 
	{
		// Use this for initialization
		void Start () {
		
		}

		void OnDestroy()
		{
			mList.Clear();
			mList = null;
		}

		public static List<int> mList = new List<int>(); 
		public static List<int> kList = new List<int>(); 

		public static void CalcResult(int[] results ,int r_result)
		{
			mList.Clear();
			int[] res_count = new int[9];

			for (int j = 0; j < 4; ++j) {

				++res_count[results[j]];
			}


			if(res_count[r_result]>0)
			{
				if(res_count[r_result] == 4)
				{
					for(int i=0; i<4; i++){
						mList.Add(i);
					}
				}
				else if(res_count[r_result] == 3)
				{
					//左边3个
					if(results[0] == r_result && results[3] != r_result)
					{
						for(int i=0; i<3; i++){
							mList.Add(i);
						}
					}else if(results[0] != r_result && results[3] == r_result)
					{
						for(int i=1; i<4; i++){
							mList.Add(i);
						}
					}
				}
				else
				{
					for(int i=0; i<4; i++)
					{
						if(results[i] == r_result){
							mList.Add(i);
						}
					}
				}

			}else
			{
				for (int i = 0; i < 9; ++i) 
				{
					if(res_count[i]==4)
					{
						for(int j=0; j<4; j++)
						{
							mList.Add(j);
						}
					}else if(res_count[i]==3)
					{
						for(int j=0; j<4; j++)
						{
							//左边3个
							if((results[0] == results[1])&& (results[0]== results[2]))
							{
								for(int k = 0; k<3; k++){
									mList.Add(k);
								}
							}else if((results[1] == results[2])&& (results[1]== results[3]))
							{
								for(int k=1; k<4; k++){
									mList.Add(k);
								}
							}
						}
					}
				}
			}
		}
	}
}