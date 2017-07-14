using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.Services
{
    /// <summary>
    /// 封装异步tcp socket,提供发送接收缓存以及消息包分发等
    /// @Author: guofeng
    /// </summary>
    public class NetSocket
    {
        private Socket clientSocket = null;
        private List<ISocketListener> _listenerList = null;

        private byte[] _byteRecvBuffer = null;
        private int _recvBufferSize = 0;
        private int _recvPacketCount = 0;
        private uint _lastRecvTime = 0;
        private uint _lastSendTime = 0;
        private uint _lostCount = 0;

        private List<Packet> _packetlist = null;
        private List<Packet> _sendList = null;

        private IPacketBuilder _packetBuilder = null;

        private object lockObj = new object();

        public string ServerHost { get; private set; }
        public uint ServerPort { get; private set; }

        public int ID = -1;

        public NetSocket(int id, IPacketBuilder builder)
        {
            ID = id;
            _byteRecvBuffer = new byte[NetOptions.SOCKET_MAX_RECV_SIZE];
            _packetlist = new List<Packet>();

            _listenerList = new List<ISocketListener>();
            _sendList = new List<Packet>();

            _packetBuilder = builder;
        }

        public virtual bool StartConnect(string host, uint port)
        {
            if (clientSocket != null)
            {
                if (CheckSocketConnected())
                {
                    Logger.Net.LogWarning("socket connected already.");
                    //return true;
                }

                Close();
            }


            ServerPort = port;
            ServerHost = host;

            _recvBufferSize = 0;
            _recvPacketCount = 0;
            _lostCount = 0;

            _lastRecvTime = (uint) System.Environment.TickCount;
            _lastSendTime = (uint) System.Environment.TickCount;

            //Security.PrefetchSocketPolicy(ServerHost, 843, 5000);

            IPAddress addr = null;
            if (Array.Exists(NetOptions.DomainSuffixList, s => { return host.Contains(s);} ))
            {
                //addr = Dns.GetHostAddresses(host)[0];
                Dns.BeginGetHostAddresses(host, new AsyncCallback(getHostCallback), null);
                return true;
            }
            else
            {
                addr = IPAddress.Parse(host);
            }

            AsyncConnect(addr, port);

            return true;
        }

        public bool Reconnect()
        {
            return StartConnect(ServerHost, ServerPort);
        }

        public void Regiester(ISocketListener o)
        {
            _listenerList.Add(o);
        }

        public void UnRegiester(ISocketListener o)
        {
            _listenerList.Remove(o);
        }


        private void AsyncConnect(IPAddress host, uint port)
        {
            try
            {
				Logger.Net.Log("AsyncConnect:"+host+" port:"+port);

                lock(lockObj)
                {
                    if (clientSocket == null)
                    {
                        clientSocket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);
                        clientSocket.SetSocketOption(SocketOptionLevel.Socket,
                            SocketOptionName.Linger,
                            new LingerOption(false, 0));
                        clientSocket.SetSocketOption(SocketOptionLevel.Socket,
                            SocketOptionName.ReceiveBuffer,
                            NetOptions.SOCKET_MAX_RECV_SIZE);

                        clientSocket.Blocking = true;
                        clientSocket.NoDelay = true;
                        clientSocket.ReceiveTimeout = NetOptions.Timeout;
                        clientSocket.SendTimeout = NetOptions.Timeout;
                    }

                    clientSocket.BeginConnect(
                         new IPEndPoint(host, (int)port),
                         new AsyncCallback(connectCallback),
                         clientSocket
                         );
                }
            }
            catch (Exception exception)
            {
                Logger.Net.LogException(exception);
            }
        }

		private void getHostCallback(IAsyncResult asyncGetHost)
		{
			try
			{
				IPAddress[] addresses = Dns.EndGetHostAddresses(asyncGetHost);
                Logger.Net.Log("getHostCallback.  len:" + 
                    addresses.Length + " ip:" + 
                    (addresses.Length > 0 ? addresses[0].ToString() : ""));
                if (addresses.Length > 0)
				{
                    AsyncConnect(addresses[0], ServerPort);
				}
			}
			catch (Exception e)
			{
                Logger.Net.Log(e.Message);
                OnSocketDisconnect();
			}
		}

        private void connectCallback(IAsyncResult asyncConnect)
        {
            Logger.Net.Log("connectCallback");
            Socket socket = (Socket)asyncConnect.AsyncState;
            if (socket == null)
            {
                return;
            }

            try
            {
                socket.EndConnect(asyncConnect);
                if (asyncConnect.IsCompleted && socket.Connected)
                {
                    PushInternalMsg(Packet.enInternalMsgType.Connected);
                    BeginReceive(socket);
                }
            }
            catch (SocketException e)
            {
                Logger.Net.Log(e.Message);
                OnSocketDisconnect();
            }
            catch (Exception e)
            {
                Logger.Net.Log(e.Message);
            }
        }

        private void BeginReceive(Socket socket)
        {
            try
            {
                if (!socket.Connected)
                {
                    return;
                }

                socket.BeginReceive(
                    _byteRecvBuffer,
                    _recvBufferSize,
                    NetOptions.SOCKET_MAX_RECV_SIZE - _recvBufferSize,
                    SocketFlags.None,
                    new AsyncCallback(receiveCallback),
                    clientSocket
               );
            }
            catch (Exception e)
            {
                Logger.Net.Log(e.Message);
                OnSocketDisconnect();
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                var socket = (Socket)ar.AsyncState;
                if (!ar.IsCompleted || socket == null || socket.Connected == false)
                {
                    return;
                }
                
                int length = socket.EndReceive(ar);
                if (length == 0)
                {
                    OnSocketDisconnect();
                    return;
                }

                _recvBufferSize = _recvBufferSize + length;
                //
                _lastRecvTime = (uint)System.Environment.TickCount;
                //Split
                while (true)
                {
                    Packet packet = _packetBuilder.FromBytes(_byteRecvBuffer, _recvBufferSize,true);
                    if (packet == null)
                    {
                        BeginReceive(socket);
                        return;
                    }
                    else
                    {
                        _recvBufferSize = _recvBufferSize - (int)packet.Size;
                        Buffer.BlockCopy(_byteRecvBuffer, (int)packet.Size, _byteRecvBuffer, 0, _recvBufferSize);

                        _recvPacketCount++;

                        lock (((ICollection)_packetlist).SyncRoot)
                        {
                            _packetlist.Add(packet);
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Logger.Net.Log(e.Message);
                OnSocketDisconnect();
            }
            catch (Exception e)
            {
                Logger.Net.Log(e.Message);
            }
        }

        public void Update()
        {
            //CheckAlive();
            DoSend();
            DoDispatch();
        }

        public virtual void SendData(Packet packet)
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }

            _sendList.Add(packet);
        }

        public virtual void SendData(UInt32 mainCmd, UInt32 subCmd, int handleCode, byte[] dataBuffer)
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }

            var packet = NetPacketPool.PopPacket(Packet.enPacketType.Network);
            packet.MainCmd = mainCmd;
            packet.SubCmd = subCmd;
            packet.CheckCode = handleCode;
            packet.Data = dataBuffer;
            _sendList.Add(packet);
        }

        public void SendData(byte[] wByteBuffer, int len)
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }

            var packet = _packetBuilder.FromBytes(wByteBuffer, len,false);
            _sendList.Add(packet);
        }

        private void send(Packet packet)
        {
            try
            {
                var sendData = _packetBuilder.ToBytes(packet);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None,
                    new AsyncCallback(sendCallback), clientSocket);
            }
            catch (Exception e)
            {
                Logger.Net.Log(e.Message);
            }
        }

        private void syncSend(Packet packet)
        {
            lock(lockObj)
            {
                try
                {
                    var sendData = _packetBuilder.ToBytes(packet);
                    if (clientSocket.Send(sendData) != sendData.Length)
                    {
                        OnSocketDisconnect();
                    }
                }
                catch (Exception e)
                {
                    Logger.Net.Log(e.Message);
                }
            }
        }

        private void sendCallback(IAsyncResult asyncSend)
        {
            try
            {
                var socket = (Socket)asyncSend.AsyncState;
                socket.EndSend(asyncSend);
            }
            catch (SocketException e)
            {
                Logger.Net.Log(e.Message);
                OnSocketDisconnect();
            }
        }

        public bool CheckSocketConnected()
        {
            if (clientSocket != null)
            {
                try
                {
                    return !(clientSocket.Poll(1, SelectMode.SelectRead) && 
                        clientSocket.Available == 00);
                }
                catch (SocketException e)
                {
                    Logger.Net.Log(e.Message);
                    return false;
                }
            }

            return false;
        }

        private void OnSocketDisconnect()
        {
            Logger.Net.Log("OnSocketDisconnect");

            Shutdown();

            _sendList.Clear();
            _recvBufferSize = 0;

            _lastRecvTime = 0;
            _lastSendTime = 0;

            PushInternalMsg(Packet.enInternalMsgType.Disconnected);

            GC.Collect();
        }

        private void PushInternalMsg(Packet.enInternalMsgType type)
        {
            Logger.Net.Log("PushInternalMsg:" + type.ToString());

            var packet = NetPacketPool.PopPacket(Packet.enPacketType.Internal);
            packet.MainCmd = (uint)type;

            lock (((ICollection)_packetlist).SyncRoot)
            {
                _packetlist.Add(packet);
            }
        }

        public bool CheckAlive()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                int nRecvDelay = System.Environment.TickCount - (int)_lastRecvTime;
                int nSendDelay = System.Environment.TickCount - (int)_lastSendTime;

                //Heart check
                if (nRecvDelay >= NetOptions.Timeout * 5 /*&& nSendDelay >= timeout * 10*/)
                {
                    return false;
                    //OnSocketDisconnect();
                }
            }

            return true;
        }

        private void DispatchMsg(Packet packet)
        {
            if (_listenerList == null || _listenerList.Count == 0)
            {
                return;
            }

            try 
			{
            switch (packet.PacketType)
            {
                case Packet.enPacketType.Internal:
                    {
                        switch ((Packet.enInternalMsgType)packet.MainCmd)
                        {
                            case Packet.enInternalMsgType.Connected:
                                {
                                    _listenerList.ForEach(listener =>
                                    {
                                        listener.OnSocketConnected(this);

                                    });

                                    break;
                                }
                            case Packet.enInternalMsgType.Disconnected:
                                {
                                    _listenerList.ForEach(listener =>
                                    {
                                        listener.OnSocketDisconnected(this);

                                    });
                                    
                                    break;
                                }
                            case Packet.enInternalMsgType.Timeout:
                                {
                                    _listenerList.ForEach(listener =>
                                    {
                                        listener.OnSocketConnectTimout(this);

                                    });
                                  
                                    break;
                                }
                        }
                        break;
                    }
                case Packet.enPacketType.Network:
                    {
                        HandlePacket(packet);
                        break;
                    }
                default:
                    {
                        throw new Exception("incorrect packet:" + packet.PacketType);
                    }
            }
			}catch(Exception ex)
			{
                Logger.Net.LogException(ex);
			}
        }

        public virtual void HandlePacket(Packet packet)
        {
            /*_listenerList.ForEach(listener =>
                {
                    listener.OnMessage(this, packet);
                });*/
        }

        void DoSend()
        {
            lock (((ICollection)_sendList).SyncRoot)
            {
                for (int i = 0; i < _sendList.Count; i++)
                {
                    var packet = _sendList[i];
                    syncSend(packet);
                    NetPacketPool.DropPacket(packet);
                }

                _sendList.Clear();
            }
        }

        void DoDispatch()
        {
            lock (((ICollection)_packetlist).SyncRoot)
            {
                for (int i = 0; i < _packetlist.Count; i++)
                {
                    var packet = _packetlist[i];
                    DispatchMsg(packet);
                    NetPacketPool.DropPacket(packet);
                }

                _packetlist.Clear();
            }
        }

		private void Shutdown()
		{
            _packetBuilder.Reset();

		    lock(lockObj)
		    {
		        try
		        {
                    if (clientSocket != null)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        clientSocket = null;
                    }
                }
                catch (Exception ce)
                {
                    Logger.Net.Log(ce.Message);
                }
                finally
                {
                    clientSocket = null;
                }
		    }
		}

        public virtual void Close()
        {
            Logger.Net.Log("Close Socket. IP:"+ServerHost+" Port:"+ServerPort);

			DoSend();
			Shutdown();
        }

    }
}


