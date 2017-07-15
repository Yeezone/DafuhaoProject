using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.SHZ
{ 
	public class UIResult : MonoBehaviour
	{
		//彩线
		static byte[,]	m_lColorLines = 
		{
			{5,6,7,8,9},{0,1,2,3,4},{10,11,12,13,14},
			{0,6,12,8,4},{10,6,2,8,14},{0,1,7,3,4},
			{10,11,7,13,14},{5,11,12,13,9},{5,1,2,3,9}
		};	

		public static List<List<byte>> m_Result = new List<List<byte>>();

		public static List<int> tLines = new List<int>(); 

		public void OnDestroy()
		{
			tLines.Clear();
			m_Result.Clear();
			tLines = null;
			m_Result = null;
		}

		public static void CalcResultTimes(byte[] results, byte lines)
		{
			m_Result.Clear();
			tLines.Clear();
			byte[] res_count = new byte[9];
			bool isLine = false;

			do
			{
				for (byte j = 0; j < 15; ++j) {
					++res_count[results[j]];
				}

				//全部图标相同
				for(byte i=0; i<9; i++)
				{
					if(res_count[i]==15)
					{
						List<byte> tempList = new List<byte>();
						for(byte j=0; j<15; j++){
							tempList.Add(j);
						}
						m_Result.Add(tempList);
//						for(int k=0; k<9; k++) 
//							tLines.Add(k+1);
						return;
					}
				}

				for(int i=0; i<8; i++)
				{
					if(res_count[i]+res_count[8] >= 3 )
						isLine = true;
				}
				if(!isLine) break;

				int temp_count = 0;

				// 普通图标+水浒传
				for(byte i=0; i<8; i++)
				{
					for(byte j=0; j<9; j++)
					{
						if(j > lines-1) break;
						//必须靠边
						if( results[(m_lColorLines[j,0])]!= i && results[(m_lColorLines[j,4])]!= i)
						{
							if(results[(m_lColorLines[j,0])]!= 8 && results[(m_lColorLines[j,4])]!= 8)
								continue;
						}
						bool all_shz = true;
						temp_count = 0;

						for(byte k=0; k<5; k++)
						{
							if( results[(m_lColorLines[j,k])]!= i && results[(m_lColorLines[j,k])]!= 8)
								break;
							temp_count++;
							if(all_shz && results[(m_lColorLines[j,k])] != 8) all_shz = false;
						}

						//每条线上从左到右连续超过3个
						if(temp_count>=3 && !all_shz)
						{
							List<byte> tempList = new List<byte>();
							if(temp_count==5)
							{
								for(byte k=0; k<5; k++){
									tempList.Add(m_lColorLines[j,k]);
								}
							}
							else if(temp_count==4)
							{
								for(byte k=0; k<4; k++){
									tempList.Add(m_lColorLines[j,k]);
								}
							}
							else if(temp_count==3)
							{
								for(byte k=0; k<3; k++){
									tempList.Add(m_lColorLines[j,k]);
								}
							}
							tLines.Add(j+1);
							m_Result.Add(tempList);
						}
						else //begin else
						{
							//从右到左判断
							all_shz = true;
							temp_count = 0;
							for(byte k=4; k>=0; k--)
							{
								if( results[(m_lColorLines[j,k])]!= i && results[(m_lColorLines[j,k])]!= 8)
									break;
								temp_count++;
								if(all_shz && results[(m_lColorLines[j,k])] != 8) all_shz = false;
							}

							//连续超过3个
							if(temp_count>=3 && !all_shz)
							{
								List<byte> tempList = new List<byte>();
								if(temp_count==5)
								{
									for(byte k=0; k<5; k++){
										tempList.Add(m_lColorLines[j,k]);
									}
								}
								else if(temp_count==4)
								{
									for(byte k=1; k<5; k++){
										tempList.Add(m_lColorLines[j,k]);
									}
								}
								else if(temp_count==3)
								{
									for(byte k=2; k<5; k++){
										tempList.Add(m_lColorLines[j,k]);
									}
								}
								tLines.Add(j+1);
								m_Result.Add(tempList);						
							  }
						 } //end else

					}
				}

				
				if(res_count[3]+res_count[4]+res_count[5] == 15)
				{
					List<byte> tempList = new List<byte>();
					for(byte k=0; k<15; k++){
						tempList.Add(k);
					}
					m_Result.Add(tempList);	
				}
				else if(res_count[0]+res_count[1]+res_count[2] == 15)
				{
					List<byte> tempList = new List<byte>();
					for(byte k=0; k<15; k++){
						tempList.Add(k);
					}
					m_Result.Add(tempList);	
				}
			}while(false);
		}
	}
}