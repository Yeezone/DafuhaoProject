using UnityEngine;
using System.Collections;

public class CGetScreenWH : MonoBehaviour {

    /// <summary>
    /// 背景图
    /// </summary>
//    public GameObject m_gBack;

    /// <summary>
    /// UIRoot
    /// </summary>
    public GameObject m_gUIRoot;

    /// <summary>
    /// 游戏父节点
    /// </summary>
    public GameObject m_gParent;

	public int m_iBaseWidth = 1280;
	public int m_iBaseHeight = 750;
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

//#if !UNITY_STANDALONE_WIN
//        int screen_height = Screen.height;
//        int screen_width = Screen.width;
//
//        float back_width = m_gBack.GetComponent<UIWidget>().width;
//        float back_height = m_gBack.GetComponent<UIWidget>().height;
//		float temp_bh = back_height;
//        back_height = (float)screen_height * ((float)back_width / (float)screen_width);
////        m_gBack.GetComponent<UIWidget>().height = (int)back_height;
//
//        Vector3 scale = m_gParent.transform.localScale;
//		scale.y =  (float)back_height/(float)temp_bh  ;
//		//Debug.Log("<color=red>"+back_height+","+temp_bh+"</color>");
//
//        m_gParent.transform.localScale = scale;
//#endif


//#if UNITY_STANDALONE_WIN
		SetPcWindow();
//#endif
      
	}

	public void SetPcWindow()
	{
		int screen_height = Screen.height;
		int screen_width = Screen.width;
		
		
		Vector3 scale = m_gParent.transform.localScale;
		//scale.y = (float)back_height / (float)temp_bh;
		scale.y = (float)screen_height / m_iBaseHeight;
		scale.x = (float)screen_width / m_iBaseWidth;
		m_gParent.transform.localScale = scale;

	}


    
}
