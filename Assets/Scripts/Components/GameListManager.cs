using System.Collections.Generic;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 游戏列表管理器
    /// @Author: guofeng
    /// </summary>
    public class GameListManager
    {
        private GameList _gameList = null;

        public GameList GameList
        {
            get { return _gameList; }
            set { _gameList = value; }
        }

        public GameListManager()
        {
            _gameList = new GameList();
        }

        public void Clear()
        {
            _gameList.KindList.Clear();
            _gameList.NodeList.Clear();
            _gameList.RoomList.Clear();
            _gameList.TypeList.Clear();
        }

        public void AppendTypeItem(SGameTypeItem item)
        {
            foreach (var kind in _gameList.TypeList)
            {
                if (kind.ID == item.ID)
                {
                    _gameList.TypeList.Remove(kind);
                    break;
                }
            }
            _gameList.TypeList.Add(item);
        }

        public void AppendKindItem(SGameKindItem item)
        {
            foreach (var name in _gameList.KindList)
            {
                if (name.ID == item.ID)
                {
                    _gameList.KindList.Remove(name);
                    break;
                }
            }

            _gameList.KindList.Add(item);
        }

        public void AppendRoomItem(SGameRoomItem item)
        {
            foreach (var room in _gameList.RoomList)
            {
                if (room.ID == item.ID)
                {
                    _gameList.RoomList.Remove(room);
                    break;
                }
            }

            _gameList.RoomList.Add(item);
        }

        public void AppendNodeItem(SGameNodeItem item)
        {
            foreach (var node in _gameList.NodeList)
            {
                if (node.NodeID == item.NodeID)
                {
                    _gameList.NodeList.Remove(node);
                    break;
                }
            }
            _gameList.NodeList.Add(item);
        }

        public void UpdateOnlineCnt(uint dwRoomID, uint cnt)
        {
            var item = FindRoomItem(dwRoomID);
            if (item != null)
            {
                item.UpdateOnlineCnt(cnt);
            }
        }

        public SGameRoomItem FindRoomItem(uint roomID)
        {
            foreach (var room in _gameList.RoomList)
            {
                if (room.ID == roomID)
                {
                    return room;
                }
            }

            return null;
        }

        public bool IsRoomExists(uint roomID)
        {
            return FindRoomItem(roomID) != null;
        }

        public uint RoomID2NameID(uint roomID)
        {
            return 0;
        }

        public SGameKindItem FindKindItem(uint id)
        {
            foreach (var item in _gameList.KindList)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public List<SGameKindItem> FindKindList()
        {
            var list = _gameList.KindList;
            list.Sort(delegate(SGameKindItem left, SGameKindItem right)
                {
                    return (int) (left.SortID - right.SortID);
                });

            return list;
        }

        public List<SGameRoomItem> FindRoomList(uint nameID)
        {
            var list = new List<SGameRoomItem>();
            foreach (var item in _gameList.RoomList)
            {
                if (item.GameNameID == nameID)
                {
                    list.Add(item);
                }
            }

            list.Sort(delegate(SGameRoomItem left, SGameRoomItem right)
                {
                    return (int) (left.SortID - right.SortID);
                });

            return list;
        }

        public List<SGameRoomItem> FindRoomListByNodeID(uint NodeID)
        {
            var list = new List<SGameRoomItem>();
            foreach (var item in _gameList.RoomList)
            {
                if (item.NodeID == NodeID)
                {
                    list.Add(item);
                }
            }

            list.Sort(delegate(SGameRoomItem left, SGameRoomItem right)
                {
                    return (int) (left.SortID - right.SortID);
                });
            return list;
        }

        public SGameNodeItem FindNodeItem(uint nodeID)
        {
            return _gameList.NodeList.Find(item => item.NodeID == nodeID);
        }

        public List<SGameNodeItem> FindNodeList()
        {
            var list = _gameList.NodeList;
            list.Sort(delegate(SGameNodeItem left, SGameNodeItem right)
                {
                    return (int) (left.SortID - right.SortID);
                });

            return list;
        }

    }
}

