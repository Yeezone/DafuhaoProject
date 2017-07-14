using UnityEngine;
using System.Collections;

public class CGameRoomItem : MonoBehaviour
{

    [System.Serializable]
    public class CGameRoomInfo
    {
        public uint roomId;				//房间ID
        public string roomName;			//房间名称
        public uint roomPeopleCnt;		//房间人数
        public uint roomPeopleUplimit;	//房间人数上限
        public uint roomDifen;			//房间底分
        public uint roomRuchang;		//房间入场金币
    }
    public CGameRoomInfo m_cGameInfo = new CGameRoomInfo();

	//房间样式图片的前缀名称
	public string m_strRoomSpriteName;
	//房间样式图片
	public UISprite m_sprRoomTypeSprite;
    //房间名称
    public UILabel m_lblRoomNameLabel;
    //房间人数
    public UILabel m_lblRoomNumLabel;
    //房间倍率
    public UILabel m_lblRoomRateLabel;
    //房间入场金币
    public UILabel m_lblRoomCellScoreLabel;

    /// <summary>
    /// 更新房间信息
    /// </summary>
    /// <param name="_RoomInfos">房间信息结构体</param>
    public void UpdateRoomItem(HallTransfer.RoomInfoS _RoomInfos)
    {
        m_cGameInfo.roomDifen = _RoomInfos.roomDifen;
        m_cGameInfo.roomId = _RoomInfos.roomId;
        m_cGameInfo.roomName = _RoomInfos.roomName;
        m_cGameInfo.roomPeopleCnt = _RoomInfos.roomPeopleCnt;
        m_cGameInfo.roomPeopleUplimit = _RoomInfos.roomPeopleUplimit;
        m_cGameInfo.roomRuchang = _RoomInfos.roomRuchang;

		if(m_lblRoomNameLabel!=null) m_lblRoomNameLabel.text = m_cGameInfo.roomName.ToString();
		if(m_lblRoomNumLabel!=null) m_lblRoomNumLabel.text = m_cGameInfo.roomPeopleCnt.ToString();
		if(m_lblRoomRateLabel!=null) m_lblRoomRateLabel.text = m_cGameInfo.roomDifen.ToString();
		if(m_lblRoomCellScoreLabel!=null) m_lblRoomCellScoreLabel.text = m_cGameInfo.roomRuchang.ToString();
        
    }

	// 独立更新房间图片(暂时只在休闲版使用,后期合并到上面更新房间信息方法中,适用所有版本)
	public void UpdateRoomSprite(int i)
	{
		m_sprRoomTypeSprite.spriteName = m_strRoomSpriteName + i.ToString();
	}
   
}
