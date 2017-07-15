using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ProtoBuf;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Fishing
{
	[Serializable]
	public class NavigParam 
	{
		public List<Vector3> path;
		public float timeLength;
	}
	public class NavigationInfo 
	{
		internal void LoadFromStream(Stream st)
		{
			/*
			if(Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.Android)
			{
				UnityEngine.Object obj = Resources.Load("game");
				TextAsset ta = (TextAsset)obj;
				st = new MemoryStream(ta.bytes);
			}
			else 
			{
				st = new FileStream(Application.streamingAssetsPath+"/"+"game.navigPak",FileMode.Open);
			}
			*/

			using(st)
			{
				ProtoReader pReader=new ProtoReader(st,null,null);

				navDict=new Dictionary<UInt32 ,NavigParam>();

				long fsSize=st.Length;

				while(fsSize>0){

					int readHeadSize=ProtoReader.DirectReadVarintInt32(st);

					fsSize-=4;

					byte[] tempHeadByte=ProtoReader.DirectReadBytes(st,(int)readHeadSize);

					fsSize-=readHeadSize;

					NavigHead nHead=ProtobufHelper.Deserialize<NavigHead>(tempHeadByte);

					int bodaySizeHead=ProtoReader.DirectReadVarintInt32(st);

					byte[] tempBodayByte=ProtoReader.DirectReadBytes(st,bodaySizeHead);

					fsSize-=bodaySizeHead;


					NavigationA nA=ProtobufHelper.Deserialize<NavigationA>(tempBodayByte);

					List<Vector3> v3List=new List<Vector3>();

					foreach(Pak.Vector3 tempV3 in nA.frame){

						Vector3 tempV=new Vector3();

						tempV.x=tempV3.x;
						tempV.y=tempV3.y;
						tempV.z=tempV3.z;

						v3List.Add(tempV);
					}
					NavigParam np = new NavigParam();
					np.path = v3List;
					np.timeLength = nHead.cycleTime;

					navDict.Add(nHead.idUInt,np);
			
//					Debug.LogError("****************************************");
//					Debug.LogError("HeadDataType====="+nHead.type);
//					Debug.LogError("HeadDataIdStr===="+nHead.idStr);
//					Debug.LogError("HeadDateUint====="+nHead.idUInt);
//					Debug.LogError("HeadDataCyTime=============================================="+nHead.cycleTime);
//					Debug.LogWarning("BodayListCount="+nA.frame.Count);
//				    Debug.LogError("****************************************");

				}
			}
		}

		private Dictionary<UInt32, NavigParam> navDict;
		public NavigParam GetActorNavigation(UInt32 wNavId)
		{
			return navDict[wNavId];
		}
	}
}

