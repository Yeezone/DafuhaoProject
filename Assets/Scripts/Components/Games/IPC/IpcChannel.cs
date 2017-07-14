using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.GameCore
{
    /// <summary>
    /// 游戏进程通讯,用于兼容老的c++框架游戏
    /// ipc创建窗口和读取消息只能处于同一线程,所以要开多线程则创建窗口要放到新线程执行或者不创建线程,由外部进行update调用
    /// @Author: guofeng
    /// </summary>
	public partial class IpcService
	{
        struct Send_Buffer
        {
            public Send_Buffer(DATA_BUFFER buf, int channel)
            {
                this.Data = buf;
                this.Channel = channel;
            }

            public DATA_BUFFER Data;
            public int Channel;
        }

		public void Register (IIpcListener listener)
		{
			if(!this.mListener.Contains(listener))
			{
				this.mListener.Add(listener);
			}
		}
		
		public void UnRegister (IIpcListener listener)
		{
			if(this.mListener.Contains(listener))
			{
				this.mListener.Remove(listener);
			}
		}
		
		public bool CreateServer ()
		{
			if (mReceiveThreadRuning) {
				return false;
			}
			
			Logger.Sys.Log("Start IPC Thread");

			mReceiveThread= Loom.StartSingleThread(ReceiveLoop,System.Threading.ThreadPriority.Normal,true);
			mReceiveThread.IsBackground=true;
			mReceiveThreadRuning = true;

			return mInitEvent.WaitOne(50000);

            /*
            if (mIPCBase != IntPtr.Zero)
            {
                return false;
            }
			
            if (!CreateIpcModuleEx(ref mIPCBase, ref mIPCModule))
            {
                return false;
            }
			
            return true;*/
        }
		
		public void DestoryServer ()
		{
			if (mIPCBase == IntPtr.Zero) {
				return;
			}
			
			mReceiveThreadRuning = false;
			
			mReceiveThread.Join ();

			mInitEvent.Reset();
			
			if (mIPCBase != IntPtr.Zero) {
				DestroyIpcModuleEx (mIPCBase);
				
				IpcWindowHandle = IntPtr.Zero;
				mIPCBase = IntPtr.Zero;
				mIPCModule = IntPtr.Zero;
			}
			
		}
		
		public void ReceiveLoop ()
		{
			if (mIPCBase != IntPtr.Zero) {
				return;
			}
			
			/*important!!!!
				win32窗口消息队列为线程单独所有,要开线程读取,则必须在该线程创建
			 */
			
			IpcWindowHandle = CreateIpcModuleEx (ref mIPCBase, ref mIPCModule);
			if (IpcWindowHandle == IntPtr.Zero) {
                Logger.Sys.LogError("call CreateIpcModuleEx failed");
				return;
			}


            Logger.Sys.Log("call CreateIpcModuleEx finished, handle:" + IpcWindowHandle.ToString("X"));

			mInitEvent.Set();
			
			int channel = -1;
			DATA_BUFFER ipcData = new DATA_BUFFER ();
			while (mReceiveThreadRuning) {
				
				//缓存发送,必须push到创建线程发送,这里先发送还是先接收或者一次性全发完再接收待优化
				if(mSendBufferQueue.Count != 0){

					IntPtr ptr = IntPtr.Zero;
					Send_Buffer sendData = mSendBufferQueue.Dequeue();
					
					try
					{
						ptr = Marshal.AllocHGlobal((int)sendData.Data.len);
						Marshal.Copy(sendData.Data.buf, 0, ptr, (int)sendData.Data.len);
						SendIpcData (mIPCBase, sendData.Channel, ptr, sendData.Data.len);
						//LogUtil.Log ("IPC send messge |chan:"+sendData.Channel+"|len:"+sendData.Data.len);
					}
					catch(Exception e)
					{
                        Logger.Sys.LogError("send data to ipc failed|msg:" + e.Message +
						                  "|source:"+e.Source+
						                  "|chan:"+ sendData.Channel+
						                  "|len:"+sendData.Data.len+
						                  "|stack:"+e.StackTrace
						                  );
					}
					finally
					{
						Marshal.FreeHGlobal(ptr);
					}
				}

			    try
			    {
			        if (PeekIPCMessage(mIPCBase, ref ipcData, ref channel) != true)
			        {
			            Thread.Sleep(10);
			            continue;
			        }

			        ByteBuffer buffer = ByteBufferPool.PopPacket(ipcData.buf, (int) ipcData.len);
			        IPC_Head_t head = GameConvert.ByteToStruct<IPC_Head_t>(buffer.PopByteArray(IPC_HEADER_LEN));
			        byte[] bytes = null;
			        if (head.wDataSize > IPC_HEADER_LEN)
			        {
			            bytes = buffer.PopByteArray((int) head.wDataSize - IPC_HEADER_LEN);
			        }
                    ByteBufferPool.DropPacket(buffer);

			        UInt32 mainID = head.wMainCmdID;
			        UInt32 subID = head.wSubCmdID;
			        UInt32 code = head.wHandleCode;

			        if (mainID == IPC_MAIN_IPC_KERNEL)
			        {
			            //IPC控制协议

			            switch (subID)
			            {
			                case IPC_SUB_IPC_CLIENT_CONNECT:
			                    {
			                        SendData(channel, IPC_MAIN_IPC_KERNEL, IPC_SUB_IPC_SERVER_ACCEPT, 0, null); //接受连接

			                        OnConnect(channel);

			                        break;
			                    }
			                case IPC_SUB_IPC_SERVER_ACCEPT:
			                    {
			                        OnConnect(channel);

			                        break;
			                    }
			                case IPC_SUB_IPC_CLIENT_CLOSE:
			                    {
			                        OnClose(channel);

			                        ResetChannel(channel);

			                        break;
			                    }
			                default:
			                    {
			                        //break到OnRead处理
			                        break;
			                    }
			            }
			        }

			        OnRead(channel, (int) mainID, (int) subID, (int) code, bytes);

			    }
			    catch (Exception e)
			    {
                    Logger.Sys.LogError("peek msg|msg:" + e.Message +
			                         "|source:" + e.Source +
			                         "|stack:" + e.StackTrace
			            );
			    }
			   
			}
			
			//貌似c#线程会被野蛮退出,走不到这里执行
			DestroyIpcModuleEx (mIPCBase);
			
			IpcWindowHandle = IntPtr.Zero;
			mIPCBase = IntPtr.Zero;
			mIPCModule = IntPtr.Zero;
		}
		
		private void OnConnect (int nChannelID)
		{
			foreach(var item in mListener)
			{
				Loom.DispatchToMainThread(
					()=>item.OnConnect (nChannelID),
					true);
			}
		}
		
		private void OnClose (int nChannelID)
		{
			foreach(var item in mListener)
			{
				Loom.DispatchToMainThread(
					()=>item.OnClose (nChannelID),
					true);
			}
		}
		
		private void OnRead (int nChannelID, int mainCmd, int subCmd, int handleCode, byte[] byteBuffer)
		{
			foreach(var item in mListener)
			{
				Loom.DispatchToMainThread(
					()=>item.OnRead (nChannelID, mainCmd, subCmd, handleCode, byteBuffer),
					true);
			}
		}
		
		
		public bool SendData (int nChannelID, int nMainID, int nSubID, int nHandleCode, byte[] bytes)
		{
			IPC_Head_t head = new IPC_Head_t ();
			head.wMainCmdID = (UInt32)nMainID;
			head.wSubCmdID = (UInt32)nSubID;
			head.wHandleCode = (UInt32)nHandleCode;
			head.wDataSize = IPC_HEADER_LEN;
			
			if (bytes != null) {
				UInt32 tempDataBufferSize = (UInt32)bytes.Length;
				head.wDataSize += tempDataBufferSize;
			}
			
			ByteBuffer buffer = ByteBufferPool.PopPacket ();
			byte[] headBuffer = GameConvert.StructToByteArray<IPC_Head_t> (head);
			buffer.Position = 0;
			buffer.PushByteArray (headBuffer);
			
			if (bytes != null) {
				buffer.PushByteArray (bytes);
			}
			
			DATA_BUFFER sendData = new DATA_BUFFER ();
			sendData.buf = buffer.ToByteArray ();
			sendData.len = (UInt32)buffer.Length;
			mSendBufferQueue.Enqueue(new Send_Buffer(sendData, nChannelID));

            ByteBufferPool.DropPacket(buffer);

			//LogUtil.Log ("IPC push messge |chan:"+nChannelID+"|main:"+nMainID+"|sub:"+nSubID+"|len:"+sendData.len);
			
			return true;
		}
		
		public void CloseIpcChannel (int nChannelID)
		{
			CloseIpcChannel (mIPCBase, nChannelID);
		}
		
		public void ResetChannel (int nChannelID)
		{
			if(mIPCBase == IntPtr.Zero) return;
			
			ResetChannel (mIPCBase, nChannelID);
		}
		
		public bool ConnectServer (IntPtr hServer, int nSeriNo)
		{
			return ConnectServer (mIPCBase, hServer, nSeriNo);
		}
		
		public IntPtr IpcWindowHandle	{ set; get; }
		
		private IntPtr mIPCBase = IntPtr.Zero;
		private IntPtr mIPCModule = IntPtr.Zero;
		private Thread mReceiveThread = null;
		private bool mReceiveThreadRuning = false;
		private List<IIpcListener> mListener = new List<IIpcListener>();
		private Queue<Send_Buffer> mSendBufferQueue = new Queue<Send_Buffer>();
		private AutoResetEvent mInitEvent = new AutoResetEvent(false);
	}
}
