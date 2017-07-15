using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.WXHH
{    
	public class PokerAnimCtr : MonoBehaviour {

	    public MeshMorpher m_Pai;
	    public MeshMorpher m_Bei;
		public MeshMorpher m_Pai_Bot;
	    public MeshMorpher m_Kuang;
	    private bool m_bIsOpen = false;

		public Material m_mSmallBall;
		public Texture[]  m_Texture;
		public GameObject cardView;
		public bool isShow = false;
		public bool isClose = false;

		public Mesh[] awakeMeshs;
		public Mesh[] LastMeshs;
		void Start()
	    {

	    }


	    void Update()
	    {
	        if (!m_bIsOpen)
	        {
	            m_Pai.m_AutomaticTime = 0;
	            m_Bei.m_AutomaticTime = 0;
//	            m_Kuang.m_AutomaticTime = 0;
//				m_Pai.m_AnimateAutomatically = false;
//				m_Bei.m_AnimateAutomatically = false;
//				m_Kuang.m_AnimateAutomatically = false;
	        }

			if (/*Input.GetKeyDown(KeyCode.P)||*/isShow)
			{
				isShow=false;
	            m_bIsOpen = true;
	            m_Pai.m_AnimateAutomatically = true;
	            m_Bei.m_AnimateAutomatically = true;
//	            m_Kuang.m_AnimateAutomatically = true;
	        }

			if (isClose)
			{
				isClose = false;
				m_Pai.m_AnimateAutomatically = false;
				m_Bei.m_AnimateAutomatically = false;
	            m_bIsOpen = false;
				m_Pai.GetComponent<MeshFilter>().mesh.vertices = awakeMeshs[0].vertices;
				m_Bei.GetComponent<MeshFilter>().mesh.vertices = awakeMeshs[1].vertices;
				m_Pai_Bot.gameObject.SetActive(false);
	        }

		}
	}
}