using UnityEngine;
using System.Collections;


public class CGameItem : MonoBehaviour {
    /// <summary>
    /// 游戏图标
    /// </summary>
    public UISprite m_sprGameicoSprite;
    /// <summary>
    /// 游戏火热程度
    /// </summary>
    public UISprite m_sprHoticoSprite;
    /// <summary>
    /// 游戏名称
    /// </summary>
    public UILabel m_lblGameLabel;
    public UISprite m_sprGameName;

	public GameObject m_objGameCheckedItem;

    [System.Serializable]
    public class CGameInfo
    {
        public uint TypeID;
        public uint ID;
        public uint SortID;
        public bool Installed;			//是否安装
        public bool NeedUpdate;			//是否需要更新
        public string Name;
        public uint OnlineCnt;
        public bool IsHot;
        public bool IsOpen;
    }
    public CGameInfo m_cGameInfo = new CGameInfo();

    /// <summary>
    /// 更新游戏信息
    /// </summary>
    /// <param name="_gameInfo">游戏信息结构体</param>
   public void UpdateGameInfo(HallTransfer.GameInfoS _gameInfo)
    {
        if (m_cGameInfo == null)m_cGameInfo = new CGameItem.CGameInfo();

        m_cGameInfo.ID = _gameInfo.ID;
        m_cGameInfo.Installed = _gameInfo.Installed;
        m_cGameInfo.IsHot = _gameInfo.IsHot;
        m_cGameInfo.IsOpen = _gameInfo.IsOpen;
        m_cGameInfo.Name = _gameInfo.Name;
        m_cGameInfo.NeedUpdate = _gameInfo.NeedUpdate;
        m_cGameInfo.OnlineCnt = _gameInfo.OnlineCnt;
        m_cGameInfo.SortID = _gameInfo.SortID;
        m_cGameInfo.TypeID = _gameInfo.TypeID;

       //UI设置
		if(m_sprGameicoSprite!=null) m_sprGameicoSprite.spriteName = "gameico_" + m_cGameInfo.ID.ToString();
		if(m_sprHoticoSprite!=null) m_sprHoticoSprite.gameObject.SetActive(m_cGameInfo.IsHot);
		if(m_lblGameLabel!=null) m_lblGameLabel.text = m_cGameInfo.Name.ToString();
		if(m_sprGameName!=null) m_sprGameName.spriteName = "gamename_"+m_cGameInfo.ID.ToString();
    }
    //设置正常游戏按钮
    public void SetNormalGameItemUI()
   {
		if(m_objGameCheckedItem!=null) m_objGameCheckedItem.SetActive(false);
//       m_GameBTSprite.spriteName = "hall_game_btn";
//       m_GameBTSprite.GetComponent<UIButton>().normalSprite = "hall_game_btn";
   }
    //设置选中游戏按钮
   public void SetCheckedItemUI()
    {
		if(m_objGameCheckedItem!=null) m_objGameCheckedItem.SetActive(true);
//        m_GameBTSprite.spriteName = "hall_game_btn2";
//        m_GameBTSprite.GetComponent<UIButton>().normalSprite = "hall_game_btn2";
    }
}
