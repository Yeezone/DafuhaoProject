using UnityEngine;
using System.Collections;

public class singleGamePageLogo : MonoBehaviour 
{
	// 页数
	public int m_iGamePageNum = 0;
	//
	private int m_iCreatGamesNum;
	//
	private bool m_bIsCreatSixGames = true;
	// 预设场景中游戏按钮的位置
	private Vector3[] m_gameItemPoss;

	void Start()
	{
		m_iCreatGamesNum = CGameManager._instance.m_iCreatGamesNum;
		m_bIsCreatSixGames = CGameManager._instance.m_bIsCreatSixGames;
		m_gameItemPoss = CGameManager._instance.m_gameItemPoss;
	}

	// 点击页数Logo事件
	public void GamePageLogoOnClick()
	{
		if(m_iGamePageNum != CGameManager._instance.m_gameListCurPage)
		{
			for(int i = 0; i < m_iCreatGamesNum; i++)
			{
				// 隐藏下一页的GameLogo
				CGameManager._instance.m_lstGameItemList_Relax[((CGameManager._instance.m_gameListCurPage) * m_iCreatGamesNum) + i].transform.localScale = Vector3.zero;
				// 显示当前页的GameLogo
				GameObject temp = CGameManager._instance.m_lstGameItemList_Relax[(m_iGamePageNum * m_iCreatGamesNum) + i];
				temp.SetActive(true);
				temp.transform.localScale = Vector3.one;
				TweenPosition tempTP = temp.GetComponent<TweenPosition>();
                if (m_iGamePageNum > CGameManager._instance.m_gameListCurPage)
                {
					if(m_bIsCreatSixGames)
					{
						tempTP.from = new Vector3(m_gameItemPoss[i].x + 500, m_gameItemPoss[i].y, 0);
						tempTP.to = new Vector3(m_gameItemPoss[i].x, m_gameItemPoss[i].y, 0);
					}else{
						tempTP.from = new Vector3(m_gameItemPoss[i+6].x + 500, m_gameItemPoss[i+6].y, 0);
						tempTP.to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					}                 
                }
                else
                {
					if(m_bIsCreatSixGames)
					{
						tempTP.from = new Vector3(m_gameItemPoss[i].x - 500, m_gameItemPoss[i].y, 0);
						tempTP.to = new Vector3(m_gameItemPoss[i].x, m_gameItemPoss[i].y, 0);
					}else{
						tempTP.from = new Vector3(m_gameItemPoss[i+6].x - 500, m_gameItemPoss[i+6].y, 0);
						tempTP.to = new Vector3(m_gameItemPoss[i+6].x, m_gameItemPoss[i+6].y, 0);
					}                   
                }
				
				tempTP.enabled = true;
				tempTP.ResetToBeginning();
			}

            creatGamePageLogo._instance.UpdatePageLogo(CGameManager._instance.m_gameListCurPage, m_iGamePageNum);
			CGameManager._instance.m_gameListCurPage = m_iGamePageNum;
		}
	}

}
