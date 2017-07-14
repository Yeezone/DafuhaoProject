using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace com.QH.QPGame.Utility
{
	
	public class MacAddress
	{
		public static string GetMacAddress()
		{
			try
			{
				string id = SystemInfo.deviceUniqueIdentifier;
				return id.Substring(0, 32);
			}
			catch(Exception ex)
			{
				Debug.LogError("GetMacAddress Faield:"+ex.Message);
			}

			return "";
		}
		
	}
}