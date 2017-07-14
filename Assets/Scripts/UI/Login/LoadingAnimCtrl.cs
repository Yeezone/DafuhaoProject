using UnityEngine;
using System.Collections;

public class LoadingAnimCtrl : MonoBehaviour {

	private float CurTime = 0;
	private float TimeInterval = 0.2f;
	private float CurRot = 0;
	
	// Update is called once per frame
	void Update () {
		CurTime += Time.deltaTime;
		if(CurTime >= TimeInterval)
		{
			CurTime = 0;
			CurRot = (CurRot-30f<=-360f?0:CurRot-30f);
			transform.localRotation = Quaternion.Euler(0,0,CurRot);
		}
	}
}
