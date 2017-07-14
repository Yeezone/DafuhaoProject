using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.Lobby
{
    public enum ConnectionID
    {
        Lobby,
        Game
    }

    /// <summary>
    /// 网络连接管理器,负责接管所有网络连接,数据接收发送等
    /// @Author: guofeng
    /// </summary>
    public class NetworkManager : MonoBehaviour, ISocketListener
    {
        public enum Status
        {
            Connected,
            TimeOut,
            Disconnected
        }


        internal class Connection : NetSocket
        {
            public MutilMap<uint, Action<Packet>> Handlers;

            public bool Quiting = false;
            public bool Reconnecting = false;
            public int RetryTimes = 0;

            public Connection(int id, IPacketBuilder builder)
                : base(id, builder)
            {
                Handlers = new MutilMap<uint, Action<Packet>>();
            }

            public void RegisterHandler(uint cmd, Action<Packet> handler)
            {
                Handlers.Add(cmd, handler);
            }

            public void UnRegisterHandler(uint cmd, Action<Packet> handler)
            {
                List<Action<Packet>> list = Handlers[cmd];
                if (list != null && list.Contains(handler))
                {
                    Handlers[cmd].Remove(handler);
                }
            }

            public override void HandlePacket(Packet packet)
            {
                Logger.Net.Log("Handle :" + packet);

                List<Action<Packet>> list = Handlers[packet.MainCmd];
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        item(packet);
                    }
                }
            }

            public override void Close()
            {
                Quiting = true;
                base.Close();
            }

            public void Reset()
            {
                Reconnecting = false;
                RetryTimes = 0;
                Quiting = false;
            }
        }

        public delegate void NetworkStatusChangeHandler(ConnectionID socket, Status wError);
        public event NetworkStatusChangeHandler NetworkStatusChangeEvent;

        private Connection[] _connections;

        public bool Initialize()
        {
            NetworkStatusChangeEvent = null;

            NetPacketPool.CreatePool();
            ByteBufferPool.CreatePool();

            var len = Enum.GetValues(typeof(ConnectionID)).Length;
            _connections = new Connection[len];
            for (int i = 0; i < _connections.Length; i++)
            {
                _connections[i] = new Connection(i,
                       new PacketBuilder(GameApp.Options.EnableEncryption));
                _connections[i].Regiester(this);
            }

            return true;
        }

        public void Reset()
        {
            CloseAllSocket();
        }

        // Update is called once per frame
        void Update()
        {
            if (_connections != null)
            {
                for (int i = 0; i < _connections.Length; i++)
                {
                    if (_connections[i] != null)
                    {
                        _connections[i].Update();
                    }
                }
            }
        }

        void OnApplicationQuit()
        {
            Reset();
        }

        public void Connect(ConnectionID flag, string wConnectIp, uint wConnectPort)
        {
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return;
            }

            connect.Reset();
            connect.StartConnect(wConnectIp, wConnectPort);

            Logger.Net.Log("connect to server, ip:" + wConnectIp + " Port:" + wConnectPort);
        }


        public void SendToSvr(ConnectionID flag, uint wMainCmdID, uint wSubCmdID, int whandleCode, byte[] wBbyteBuffer)
        {
           // Logger.Net.Log("send data to hall server. MainCmd:" + wMainCmdID + " SubCmd:" + wSubCmdID);
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return;
            }
            connect.SendData(wMainCmdID, wSubCmdID, whandleCode, wBbyteBuffer);
        }

        public void SendToSvr(ConnectionID flag, byte[] wBbyteBuffer, int len)
        {
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return;
            }
            connect.SendData(wBbyteBuffer, len);
        }

        public bool IsConnectionVaild(ConnectionID flag)
        {
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return false;
            }
            return connect.CheckSocketConnected();
        }

        public bool IsConnectionActive(ConnectionID flag)
        {
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return false;
            }
            return connect.CheckAlive();
        }

        private Connection GetConnect(ConnectionID flag)
        {
            var index = (int)flag;
            if (index >= _connections.Length)
            {
                return null;
            }

            return _connections[index];
        }

        public void CloseConnect(ConnectionID flag)
        {
            var connect = GetConnect(flag);
            if (connect == null)
            {
                return;
            }
            connect.Close();
        }

        public void CloseAllSocket()
        {
            if (_connections != null)
            {
                for (int i = 0; i < _connections.Length; i++)
                {
                    if (_connections[i] != null)
                    {
                        _connections[i].Close();
                        _connections[i] = null;
                    }
                }
            }
            
            _connections = null;
        }

        public void RegisterHandler(ConnectionID flag, uint cmd, Action<Packet> handler)
        {
            var connect = GetConnect(flag);
            if (connect != null)
            {
                connect.RegisterHandler(cmd, handler);
            }
        }

        public void UnRegisterHandler(ConnectionID flag, uint cmd, Action<Packet> handler)
        {
            var connect = GetConnect(flag);
            if (connect != null)
            {
                connect.UnRegisterHandler(cmd, handler);
            }
        }

        public void OnSocketConnected(NetSocket socket)
        {
            OnSocketEvent(socket, Status.Connected);
        }

        public void OnSocketDisconnected(NetSocket socket)
        {
            OnSocketEvent(socket, Status.Disconnected);
        }

        public void OnSocketConnectTimout(NetSocket socket)
        {
            OnSocketEvent(socket, Status.TimeOut);
        }

        private void OnSocketEvent(NetSocket socket, Status status)
        {
            bool quiting = (socket as Connection).Quiting;
            var id = (ConnectionID)socket.ID;

            if (NetworkStatusChangeEvent != null && !quiting)
            {
                NetworkStatusChangeEvent(id, status);
            }
        }

        public bool OnMessage(NetSocket wSocket, Packet packet)
        {
            return true;
        }
    }
}


