using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Utility;
using System.IO;

namespace com.QH.QPGame.Fishing
{
	public class IdMapInfo
	{
		internal void LoadFromStream(Stream st)
		{
			mapDict=new Dictionary<UInt32 ,string>();

            byte[] readDataResult = new byte[st.Length];
            st.Read(readDataResult, 0, readDataResult.Length);

            IdPak pak = ProtobufHelper.Deserialize<IdPak>(readDataResult);

			foreach(IdPair tPair in pak.idPair)
            {
				mapDict.Add(tPair.idUInt,tPair.idStr);
			}
		}

		
		private Dictionary<UInt32 ,string> mapDict;


		public string GetIdMapInfo(UInt32 pMapId)
		{
			string actorStr=null;

			if(mapDict.ContainsKey(pMapId)){

				actorStr=mapDict[pMapId];
			}

			return actorStr;
		}


	}
}


