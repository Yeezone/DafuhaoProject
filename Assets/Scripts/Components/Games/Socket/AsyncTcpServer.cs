using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace com.QH.QPGame.GameCore
{
	/// <summary>
	/// 与客户端的连接已建立事件参数
	/// </summary>
	public class TcpClientConnectedEventArgs : EventArgs
	{
		/// <summary>
		/// 与客户端的连接已建立事件参数
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		public TcpClientConnectedEventArgs(TcpClient tcpClient)
		{
			if (tcpClient == null)
				throw new ArgumentNullException("tcpClient");
			
			this.TcpClient = tcpClient;
		}
		
		/// <summary>
		/// 客户端
		/// </summary>
		public TcpClient TcpClient { get; private set; }
	}


	/// <summary>
	/// 与客户端的连接已断开事件参数
	/// </summary>
	public class TcpClientDisconnectedEventArgs : EventArgs
	{
		/// <summary>
		/// 与客户端的连接已断开事件参数
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		public TcpClientDisconnectedEventArgs(TcpClient tcpClient)
		{
			if (tcpClient == null)
				throw new ArgumentNullException("tcpClient");
			
			this.TcpClient = tcpClient;
		}
		
		/// <summary>
		/// 客户端
		/// </summary>
		public TcpClient TcpClient { get; private set; }
	}

	/// <summary>
	/// Internal class to join the TCP client and buffer together
	/// for easy management in the server
	/// </summary>
	internal class TcpClientState
	{
		/// <summary>
		/// Constructor for a new Client
		/// </summary>
		/// <param name="tcpClient">The TCP client</param>
		/// <param name="buffer">The byte array buffer</param>
		public TcpClientState(TcpClient tcpClient, byte[] buffer)
		{
			if (tcpClient == null) throw new ArgumentNullException("tcpClient");
			if (buffer == null) throw new ArgumentNullException("buffer");
			
			this.TcpClient = tcpClient;
			this.Buffer = buffer;
		}
		
		/// <summary>
		/// Gets the TCP Client
		/// </summary>
		public TcpClient TcpClient { get; private set; }
		
		/// <summary>
		/// Gets the Buffer.
		/// </summary>
		public byte[] Buffer { get; private set; }
		
		/// <summary>
		/// Gets the network stream
		/// </summary>
		public NetworkStream NetworkStream 
		{ 
			get 
			{
				return TcpClient.GetStream();
			}
		}
	}


	/// <summary>
	/// 接收到数据报文事件参数
	/// </summary>
	/// <typeparam name="T">报文类型</typeparam>
	public class TcpDatagramReceivedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// 接收到数据报文事件参数
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		/// <param name="datagram">报文</param>
		public TcpDatagramReceivedEventArgs(TcpClient tcpClient, T datagram)
		{
			TcpClient = tcpClient;
			Datagram = datagram;
		}
		
		/// <summary>
		/// 客户端
		/// </summary>
		public TcpClient TcpClient { get; private set; }
		/// <summary>
		/// 报文
		/// </summary>
		public T Datagram { get; private set; }
	}

	/// <summary>
	/// 与服务器的连接已建立事件参数
	/// </summary>
	public class TcpServerConnectedEventArgs : EventArgs
	{
		/// <summary>
		/// 与服务器的连接已建立事件参数
		/// </summary>
		/// <param name="ipAddresses">服务器IP地址列表</param>
		/// <param name="port">服务器端口</param>
		public TcpServerConnectedEventArgs(IPAddress[] ipAddresses, int port)
		{
			if (ipAddresses == null)
				throw new ArgumentNullException("ipAddresses");
			
			this.Addresses = ipAddresses;
			this.Port = port;
		}
		
		/// <summary>
		/// 服务器IP地址列表
		/// </summary>
		public IPAddress[] Addresses { get; private set; }
		/// <summary>
		/// 服务器端口
		/// </summary>
		public int Port { get; private set; }
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string s = string.Empty;
			foreach (var item in Addresses)
			{
				s = s + item.ToString() + ',';
			}
			s = s.TrimEnd(',');
			s = s + ":" + Port.ToString();
			
			return s;
		}
	}


	/// <summary>
	/// 与服务器的连接已断开事件参数
	/// </summary>
	public class TcpServerDisconnectedEventArgs : EventArgs
	{
		/// <summary>
		/// 与服务器的连接已断开事件参数
		/// </summary>
		/// <param name="ipAddresses">服务器IP地址列表</param>
		/// <param name="port">服务器端口</param>
		public TcpServerDisconnectedEventArgs(IPAddress[] ipAddresses, int port)
		{
			if (ipAddresses == null)
				throw new ArgumentNullException("ipAddresses");
			
			this.Addresses = ipAddresses;
			this.Port = port;
		}
		
		/// <summary>
		/// 服务器IP地址列表
		/// </summary>
		public IPAddress[] Addresses { get; private set; }
		/// <summary>
		/// 服务器端口
		/// </summary>
		public int Port { get; private set; }
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string s = string.Empty;
			foreach (var item in Addresses)
			{
				s = s + item.ToString() + ',';
			}
			s = s.TrimEnd(',');
			s = s + ":" + Port.ToString();
			
			return s;
		}
	}

	/// <summary>
	/// 与服务器的连接发生异常事件参数
	/// </summary>
	public class TcpServerExceptionOccurredEventArgs : EventArgs
	{
		/// <summary>
		/// 与服务器的连接发生异常事件参数
		/// </summary>
		/// <param name="ipAddresses">服务器IP地址列表</param>
		/// <param name="port">服务器端口</param>
		/// <param name="innerException">内部异常</param>
		public TcpServerExceptionOccurredEventArgs(IPAddress[] ipAddresses, int port, Exception innerException)
		{
			if (ipAddresses == null)
				throw new ArgumentNullException("ipAddresses");
			
			this.Addresses = ipAddresses;
			this.Port = port;
			this.Exception = innerException;
		}
		
		/// <summary>
		/// 服务器IP地址列表
		/// </summary>
		public IPAddress[] Addresses { get; private set; }
		/// <summary>
		/// 服务器端口
		/// </summary>
		public int Port { get; private set; }
		/// <summary>
		/// 内部异常
		/// </summary>
		public Exception Exception { get; private set; }
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string s = string.Empty;
			foreach (var item in Addresses)
			{
				s = s + item.ToString() + ',';
			}
			s = s.TrimEnd(',');
			s = s + ":" + Port.ToString();
			
			return s;
		}
	}

	
	/// <summary>
	/// 异步TCP服务器
	/// </summary>
	public class AsyncTcpServer : IDisposable
	{
		#region Fields
		
		private TcpListener _listener;
		private Dictionary<string, TcpClientState> _clients;
		private bool _disposed = false;
		
		#endregion
		
		#region Ctors
		
		/// <summary>
		/// 异步TCP服务器
		/// </summary>
		/// <param name="listenPort">监听的端口</param>
		public AsyncTcpServer(int listenPort)
			: this(IPAddress.Any, listenPort)
		{
		}
		
		/// <summary>
		/// 异步TCP服务器
		/// </summary>
		/// <param name="localEP">监听的终结点</param>
		public AsyncTcpServer(IPEndPoint localEP)
			: this(localEP.Address, localEP.Port)
		{
		}
		
		/// <summary>
		/// 异步TCP服务器
		/// </summary>
		/// <param name="localIPAddress">监听的IP地址</param>
		/// <param name="listenPort">监听的端口</param>
		public AsyncTcpServer(IPAddress localIPAddress, int listenPort)
		{
			Address = localIPAddress;
			this.Encoding = Encoding.Default;
			
			_clients = new Dictionary<string, TcpClientState>();

            _listener = new TcpListener(Address, listenPort);
		}
		
		#endregion
		
		#region Properties 
		
		/// <summary>
		/// 服务器是否正在运行
		/// </summary>
		public bool IsRunning { get; private set; }
		/// <summary>
		/// 监听的IP地址
		/// </summary>
		public IPAddress Address { get; private set; }
		/// <summary>
		/// 监听的端口
		/// </summary>
		public int Port 
        {
            get { return ((IPEndPoint)_listener.LocalEndpoint).Port; }
		}
		/// <summary>
		/// 通信使用的编码
		/// </summary>
		public Encoding Encoding { get; set; }
		
		#endregion
		
		#region Server
		
		/// <summary>
		/// 启动服务器
		/// </summary>
		/// <returns>异步TCP服务器</returns>
		public AsyncTcpServer Start()
		{
			return Start(10);
		}
		
		/// <summary>
		/// 启动服务器
		/// </summary>
		/// <param name="backlog">服务器所允许的挂起连接序列的最大长度</param>
		/// <returns>异步TCP服务器</returns>
		public AsyncTcpServer Start(int backlog)
		{
			if (IsRunning) return this;
			
			IsRunning = true;
			
			_listener.Start(backlog);
			ContinueAcceptTcpClient(_listener);
			
			return this;
		}
		
		/// <summary>
		/// 停止服务器
		/// </summary>
		/// <returns>异步TCP服务器</returns>
		public AsyncTcpServer Stop()
		{
			if (!IsRunning) return this;
			
			try
			{
				_listener.Stop();
				
				foreach (var client in _clients.Values)
				{
					client.TcpClient.Client.Disconnect(false);
				}
				_clients.Clear();
			}
			catch (ObjectDisposedException ex)
			{
			}
			catch (SocketException ex)
			{
			}
			
			IsRunning = false;
			
			return this;
		}
		
		private void ContinueAcceptTcpClient(TcpListener tcpListener)
		{
			try
			{
				tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleTcpClientAccepted), tcpListener);
			}
			catch (ObjectDisposedException ex)
			{
			}
			catch (SocketException ex)
			{
			}
		}
		
		#endregion
		
		#region Receive
		
		private void HandleTcpClientAccepted(IAsyncResult ar)
		{
			if (!IsRunning) return;

			TcpListener tcpListener = (TcpListener)ar.AsyncState;

			TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
			if (!tcpClient.Connected) return;
			
			byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
			TcpClientState internalClient = new TcpClientState(tcpClient, buffer);
			
			// add client connection to cache
			string tcpClientKey = internalClient.TcpClient.Client.RemoteEndPoint.ToString();
			if(_clients.ContainsKey(tcpClientKey))
			{
				_clients[tcpClientKey] = internalClient;
			}
			else
			{
				_clients.Add(tcpClientKey, internalClient);
			}

			RaiseClientConnected(tcpClient);
			
			// begin to read data
			NetworkStream networkStream = internalClient.NetworkStream;
			ContinueReadBuffer(internalClient, networkStream);
			
			// keep listening to accept next connection
			ContinueAcceptTcpClient(tcpListener);
		}
		
		private void HandleDatagramReceived(IAsyncResult ar)
		{
			if (!IsRunning) return;
			
			try
			{
				TcpClientState internalClient = (TcpClientState)ar.AsyncState;
				//if (!internalClient.TcpClient.Connected) return;
				
				NetworkStream networkStream = internalClient.NetworkStream;
				
				int numberOfReadBytes = 0;
				try
				{
					// if the remote host has shutdown its connection, 
					// read will immediately return with zero bytes.
					numberOfReadBytes = networkStream.EndRead(ar);

					
				}
				catch (Exception ex)
				{
					numberOfReadBytes = 0;
				}

				Debug.Log("HandleDatagramReceived:"+numberOfReadBytes);
				
				if (numberOfReadBytes == 0)
				{
					// connection has been closed
					TcpClientState internalClientToBeThrowAway;
					string tcpClientKey = internalClient.TcpClient.Client.RemoteEndPoint.ToString();

					if(_clients.ContainsKey(tcpClientKey))
					{
						_clients.Remove(tcpClientKey);
					}

					RaiseClientDisconnected(internalClient.TcpClient);
					return;
				}
				
				// received byte and trigger event notification
				byte[] receivedBytes = new byte[numberOfReadBytes];
				Buffer.BlockCopy(internalClient.Buffer, 0, receivedBytes, 0, numberOfReadBytes);
				RaiseDatagramReceived(internalClient.TcpClient, receivedBytes);
				RaisePlaintextReceived(internalClient.TcpClient, receivedBytes);
				
				// continue listening for tcp datagram packets
				ContinueReadBuffer(internalClient, networkStream);
			}
			catch (InvalidOperationException ex)
			{
				Debug.Log("HandleDatagramReceived:"+ex.Message);
			}
		}
		
		private void ContinueReadBuffer(TcpClientState internalClient, NetworkStream networkStream)
		{
			try
			{
				networkStream.BeginRead(internalClient.Buffer, 0, internalClient.Buffer.Length, HandleDatagramReceived, internalClient);
			}
			catch (ObjectDisposedException ex)
			{
				Debug.Log("ContinueReadBuffer:"+ex.Message);
			}
		}
		
		#endregion
		
		#region Events
		
		/// <summary>
		/// 接收到数据报文事件
		/// </summary>
		public event EventHandler<TcpDatagramReceivedEventArgs<byte[]>> DatagramReceived;
		/// <summary>
		/// 接收到数据报文明文事件
		/// </summary>
		public event EventHandler<TcpDatagramReceivedEventArgs<string>> PlaintextReceived;
		
		private void RaiseDatagramReceived(TcpClient sender, byte[] datagram)
		{
			if (DatagramReceived != null)
			{
				DatagramReceived(this, new TcpDatagramReceivedEventArgs<byte[]>(sender, datagram));
			}
		}
		
		private void RaisePlaintextReceived(TcpClient sender, byte[] datagram)
		{
			if (PlaintextReceived != null)
			{
				PlaintextReceived(this, new TcpDatagramReceivedEventArgs<string>(sender, this.Encoding.GetString(datagram, 0, datagram.Length)));
			}
		}
		
		/// <summary>
		/// 与客户端的连接已建立事件
		/// </summary>
		public event EventHandler<TcpClientConnectedEventArgs> ClientConnected;
		/// <summary>
		/// 与客户端的连接已断开事件
		/// </summary>
		public event EventHandler<TcpClientDisconnectedEventArgs> ClientDisconnected;
		
		private void RaiseClientConnected(TcpClient tcpClient)
		{
			if (ClientConnected != null)
			{
				ClientConnected(this, new TcpClientConnectedEventArgs(tcpClient));
			}
		}
		
		private void RaiseClientDisconnected(TcpClient tcpClient)
		{
			if (ClientDisconnected != null)
			{
				ClientDisconnected(this, new TcpClientDisconnectedEventArgs(tcpClient));
			}
		}
		
		#endregion
		
		#region Send
		
		private void GuardRunning()
		{
			if (!IsRunning)
				throw new InvalidProgramException("This TCP server has not been started yet.");
		}
		
		/// <summary>
		/// 发送报文至指定的客户端
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		/// <param name="datagram">报文</param>
		public void Send(TcpClient tcpClient, byte[] datagram)
		{
			GuardRunning();
			
			if (tcpClient == null)
				throw new ArgumentNullException("tcpClient");
			
			if (datagram == null)
				throw new ArgumentNullException("datagram");
			
			try
			{
				NetworkStream stream = tcpClient.GetStream();
				if (stream.CanWrite)
				{
					stream.BeginWrite(datagram, 0, datagram.Length, HandleDatagramWritten, tcpClient);
				}
			}
			catch (ObjectDisposedException ex)
			{

			}
		}
		
		/// <summary>
		/// 发送报文至指定的客户端
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		/// <param name="datagram">报文</param>
		public void Send(TcpClient tcpClient, string datagram)
		{
			Send(tcpClient, this.Encoding.GetBytes(datagram));
		}
		
		/// <summary>
		/// 发送报文至所有客户端
		/// </summary>
		/// <param name="datagram">报文</param>
		public void SendToAll(byte[] datagram)
		{
			GuardRunning();
			
			foreach (var client in _clients.Values)
			{
				Send(client.TcpClient, datagram);
			}
		}
		
		/// <summary>
		/// 发送报文至所有客户端
		/// </summary>
		/// <param name="datagram">报文</param>
		public void SendToAll(string datagram)
		{
			GuardRunning();
			
			SendToAll(this.Encoding.GetBytes(datagram));
		}
		
		private void HandleDatagramWritten(IAsyncResult ar)
		{
			try
			{
				((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
			}
			catch (ObjectDisposedException ex)
			{
			}
			catch (InvalidOperationException ex)
			{
			}
			catch (IOException ex)
			{
			}
		}
		
		/// <summary>
		/// 发送报文至指定的客户端
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		/// <param name="datagram">报文</param>
		public void SyncSend(TcpClient tcpClient, byte[] datagram)
		{
			GuardRunning();
			
			if (tcpClient == null)
				throw new ArgumentNullException("tcpClient");
			
			if (datagram == null)
				throw new ArgumentNullException("datagram");
			
			try
			{
				NetworkStream stream = tcpClient.GetStream();
				if (stream.CanWrite)
				{
					stream.Write(datagram, 0, datagram.Length);
				}
			}
			catch (ObjectDisposedException ex)
			{
			}
		}
		
		/// <summary>
		/// 发送报文至指定的客户端
		/// </summary>
		/// <param name="tcpClient">客户端</param>
		/// <param name="datagram">报文</param>
		public void SyncSend(TcpClient tcpClient, string datagram)
		{
			SyncSend(tcpClient, this.Encoding.GetBytes(datagram));
		}
		
		/// <summary>
		/// 发送报文至所有客户端
		/// </summary>
		/// <param name="datagram">报文</param>
		public void SyncSendToAll(byte[] datagram)
		{
			GuardRunning();
			
			foreach (var client in _clients.Values)
			{
				SyncSend(client.TcpClient, datagram);
			}
		}
		
		/// <summary>
		/// 发送报文至所有客户端
		/// </summary>
		/// <param name="datagram">报文</param>
		public void SyncSendToAll(string datagram)
		{
			GuardRunning();
			
			SyncSendToAll(this.Encoding.GetBytes(datagram));
		}
		
		#endregion
		
		#region IDisposable Members
		
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; 
		/// <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					try
					{
						Stop();
						
						if (_listener != null)
						{
							_listener = null;
						}
					}
					catch (SocketException ex)
					{
					}
				}
				
				_disposed = true;
			}
		}
		
		#endregion
	}
}