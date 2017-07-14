using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class creatGamePageLogo : MonoBehaviour {

	public static creatGamePageLogo _instance;

	// 页面Logo父节点
	public Transform m_posPageLogoParent;
	// 页面Logo(小圆圈标志)
	public GameObject m_objPageLogo;
	// 页面Logos
	public List<GameObject> m_lstPageLogos = new List<GameObject>();

	void Awake()
	{
		_instance = this;
	}
	void OnDestroy()
	{
		_instance = null;
	}

	void Start ()
	{
		m_lstPageLogos.Clear();
	}


	void Update () 
	{
		
	}

	// 创建当前页面Logo
	public void CreatPageLogo(int creatNum)
	{
		ResetPageLogo();
		for(int i = 0; i < creatNum; i++)
		{
			GameObject temp =(GameObject) Instantiate(m_objPageLogo,Vector3.zero,Quaternion.identity);
			temp.transform.parent = m_posPageLogoParent;
			temp.transform.localScale = Vector3.one;
			
			temp.transform.localPosition = new Vector3(((i*40) -(creatNum*15)),0,0);
			m_lstPageLogos.Add(temp);

			temp.GetComponent<singleGamePageLogo>().m_iGamePageNum = i;

			if(i == 0)
			{
				temp.GetComponent<TweenScale>().PlayReverse();
			}
		}
	}
	
	// 更新当前页面Logo
	public void UpdatePageLogo(int lastPageNum, int curPageNum)
	{
		if(curPageNum > m_lstPageLogos.Count)
		{
			Debug.LogError("CurPageNum exceed the maximum limit!!");
			return;
		}
		// 关闭上一个Logo
		TweenScale temp_scale0 = m_lstPageLogos[lastPageNum].GetComponent<TweenScale>();
		temp_scale0.PlayForward();
		TweenColor temp_color0 = m_lstPageLogos[lastPageNum].GetComponent<TweenColor>();
		temp_color0.enabled = true;
		temp_color0.PlayForward();
		// 开启当前Logo
		TweenScale temp_scale1 = m_lstPageLogos[curPageNum].GetComponent<TweenScale>();
		temp_scale1.PlayReverse();
		TweenColor temp_color1 = m_lstPageLogos[curPageNum].GetComponent<TweenColor>();
		temp_color1.enabled = true;
		temp_scale1.PlayReverse();
	}

	// 重置页面Logo
	void ResetPageLogo()
	{
		if(m_lstPageLogos != null)
		{
			foreach(GameObject var in m_lstPageLogos)
			{
				Destroy(var);
			}
		}
		m_lstPageLogos.Clear();
	}

}
