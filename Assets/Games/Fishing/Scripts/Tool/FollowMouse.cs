using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.Fishing
{
	public class FollowMouse : MonoBehaviour {
		// 
		public void Follow_over()
		{
			CanonCtrl.Instance.m_bFollowMouse = true;
		}
		public void Follow_out()
		{
			CanonCtrl.Instance.m_bFollowMouse = false;
		}
	}
}
