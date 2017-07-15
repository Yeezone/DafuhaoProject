using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.SHZ
{
	public class UICloseLoading : MonoBehaviour {

		public GameObject o_loading;
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnClick()
		{
			if(o_loading!=null)
			{
				o_loading.SetActive(false);
				Destroy(o_loading);
			}
		}
	}
}
