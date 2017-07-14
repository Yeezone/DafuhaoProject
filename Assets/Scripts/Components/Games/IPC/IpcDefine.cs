using System;
using System.Runtime.InteropServices;

namespace com.QH.QPGame.GameCore
{
	public interface IIpcListener
	{
		void OnConnect (int nChannelID);
		
		void OnClose (int nChannelID);
		
		void OnRead (int nChannelID, int mainCmd, int subCmd, int handleCode, byte[] byteBuffer);
	}

	public partial class IpcService
	{
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern bool CreateIpcModule (ref IntPtr ipcbase, ref IntPtr ipcmodule, IntPtr handle);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern void DestroyIpcModule (ref IntPtr ipcbase);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern IntPtr CreateIpcModuleEx (ref IntPtr ipcbase, ref IntPtr handle);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern void DestroyIpcModuleEx (IntPtr handle);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern bool PeekIPCMessage (IntPtr ipcbase, ref DATA_BUFFER buffer, ref int channel);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern bool SendIpcData (IntPtr ipcbase, int nChannelID, IntPtr data, UInt32 len);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern void CloseIpcChannel (IntPtr ipcbase, int nChannelID);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern void ResetChannel (IntPtr ipcbase, int nChannelID);
		
		#if UNITY_EDITOR
		[DllImport("BaseIpcDX64")]
		#else
		[DllImport("BaseIpc")]
		#endif
		static extern bool ConnectServer (IntPtr ipcbase, IntPtr hServer, int nSeriNo);
		
		///内核主命令码
		const int IPC_MAIN_IPC_KERNEL = 0;						//内核命令
		const int IPC_SUB_IPC_CLIENT_CONNECT = 1;				//连接通知，后带参数为IPC服务器序号，长度为1字节
		const int IPC_SUB_IPC_SERVER_ACCEPT = 2;				//应答通知
		const int IPC_SUB_IPC_CLIENT_CLOSE = 3;					//关闭通知
		
		const int IPC_HEADER_LEN = 20;
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct IPC_Head_t
		{
			public UInt32 wDataSize;							//数据大小
			public UInt32 wMainCmdID;							//主命令码
			public UInt32 wSubCmdID;							//子命令码
			public UInt32 wHandleCode;                         	//处理命令
			public UInt32 wVersion;								//IPC 版本
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		struct DATA_BUFFER
		{
			public UInt32 len;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
			public byte[] buf;
		};

	}
}

