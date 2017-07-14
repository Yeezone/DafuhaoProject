using UnityEngine;
using System.Collections;

public class ResetToggles : MonoBehaviour {

	// 所有Toggle列表的默认按钮
	public UIToggle[] m_gToggleBtnNormal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetTogglesList()
	{
		for(int i = 0; i < m_gToggleBtnNormal.Length; i++)
		{
			m_gToggleBtnNormal[i].Set(true);
		}
	}

}
