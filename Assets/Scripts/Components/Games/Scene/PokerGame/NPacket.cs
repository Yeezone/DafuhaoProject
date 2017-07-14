using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.NetFox;
using com.QH.QPGame.Utility;

namespace Shared
//namespace com.QH.QPGame.GameCore
{
    public class SocketSetting
    {
        public const byte CHECK_CODE = 0;
        public const byte MESSAGE_VER = 0x66;
        public const int SOCKET_PACKAGE = 4096;
        public const ushort PACKAGE_DATA_SIZE = sizeof(ushort);
        public const ushort PACKAGE_HEAD_SIZE = 8;
        public const ushort PACKAGE_INFO_SIZE = 4;
        public const ushort PACKAGE_CMD_SIZE = 4;
        public const int SOCKET_BUFFER = 20 * SOCKET_PACKAGE;
        public const uint NET_BREAK_TIME = 30;

    }

    public class NPacket
    {

        byte[] 		 _buffer 	= new byte[SocketSetting.SOCKET_PACKAGE];
		int 		 _pnt 		= 0;
        PacketBuilder.CMD_Head_8 _head;
       
        public ushort SubCmd
        {
            get { return (ushort)_head.SubCmdID; }
            set { _head.SubCmdID = (ushort)value; }
        }
        public ushort PacketId
        {
            get { return (ushort)_head.MainCmdID; }
            set { _head.MainCmdID = (ushort)value; }
        }
       
        public byte[] Buff
        {
            get { return _buffer; }
        }
        public int Pnt
        {
            get { return _pnt; }
            set { _pnt = value; }
        }
        public PacketBuilder.CMD_Head_8 Head
		{
			get {return _head;}
		}
		public byte CheckCode
		{
			get {return _head.CheckCode;}
			set {_head.CheckCode = value;}
		}
		public byte MessageVer
		{
			get {return _head.MessageVer;}
			set {_head.MessageVer = value;}
		}
		public ushort DataSize
		{
			get {return _head.DataSize;}
			set {_head.DataSize = value;}
		}
        //----------------------------------------------------------
        public void AddLong(long value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 8);
            _pnt += 8;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void AddInt(int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 4);
            _pnt += 4;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void AddUInt(uint value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 4);
            _pnt += 4;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void AddFloat(float value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 4);
            _pnt += 4;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void AddShort(short value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 2);
            _pnt += 2;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
		public void AddUShort(ushort value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 2);
            _pnt += 2;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void Addbyte(byte value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _pnt, 1);
            _pnt += 1;
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
        }
        public void AddBool(bool value)
        {
            if (value == true)
                Addbyte((byte)1);
            else
                Addbyte((byte)0);
			
        }
		/*
        public void AddString(string value)
        {
			
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(value);
    

            Buffer.BlockCopy(data, 0, _buffer, _pnt, data.Length);
            _pnt += data.Length;
			
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 0, sizeof(ushort));
        }

		public void AddString2(string value)
        {
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(value);
    		short size = (short)data.Length;
			
			AddShort(size);

            Buffer.BlockCopy(data, 0, _buffer, _pnt, data.Length);
            _pnt += data.Length;
			
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 0, sizeof(ushort));
        }
        */
		public void AddString(string value,int nlen)
        {
			try
			{
				if(nlen==0) return;
				byte[] data = new byte[nlen];
				Array.Clear(data,0,nlen);
				
	            //byte[] ch = System.Text.Encoding.GetEncoding("gb2312").GetBytes(value);
				byte[] ch = Encoding.UTF8.GetBytes(value);
				
	    		Buffer.BlockCopy(ch,0,data,0,ch.Length);
				
	            Buffer.BlockCopy(data, 0, _buffer, _pnt, data.Length);
	            _pnt += nlen;
				
				_head.DataSize = (ushort)_pnt;
				Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
			}
			catch(Exception ex)
			{
				throw ex;
			}
        }

		public void AddBytes(byte[] value,int nlen)
		{
            Buffer.BlockCopy(value, 0, _buffer, _pnt, nlen);
            _pnt += nlen;
			
			_head.DataSize = (ushort)_pnt;
			Buffer.BlockCopy(BitConverter.GetBytes(_head.DataSize), 0, _buffer, 2, sizeof(ushort));
		}
        //----------------------------------------------------------
        public long GetLong()
        {
            long value = BitConverter.ToInt64(_buffer, _pnt);
            _pnt += 8;
            return value;
        }
        public int GetInt()
        {
            int value = BitConverter.ToInt32(_buffer, _pnt);
            _pnt += 4;
            return value;
        }
        public uint GetUInt()
        {
            uint value = BitConverter.ToUInt32(_buffer, _pnt);
            _pnt += 4;
            return value;
        }
        public short GetShort()
        {
            short value = BitConverter.ToInt16(_buffer, _pnt);
            _pnt += 2;
            return value;
        }
		public ushort GetUShort()
        {
            short value = BitConverter.ToInt16(_buffer, _pnt);
            _pnt += 2;
            return (ushort)value;
        }
        public byte GetByte()
        {
            byte value = _buffer[_pnt];
            _pnt += 1;
            return value;
        }
        public bool GetBool()
        {
            byte data = GetByte();

            if (data == 0)
                return false;

            return true;
        }
        public float GetFloat()
        {
            float value = BitConverter.ToSingle(_buffer, _pnt);
            _pnt += 4;
            return value;
        }
		/*
        public string GetString()
        {
			short n = 0;
			for(int i=_pnt;i<_head.DataSize-_pnt;i++)
			{
				if(_buffer[i]==0)
				{
					break;
				}
				else
				{
					n++;
				}
			}

            string str = System.Text.Encoding.GetEncoding("gb2312").GetString(_buffer, _pnt, n);
            _pnt += n;
			
            return str;
        }
		
		public string GetString2()
        {
            int value = (int)BitConverter.ToInt16(_buffer, _pnt);
            _pnt += 2;

            string str = System.Text.Encoding.GetEncoding("gb2312").GetString(_buffer, _pnt, value);
            _pnt += value;
			
            return str;
        }
        */
		
		public string GetString(int nlen)
        {
			try
			{
				int n = 0;
				if((_pnt+nlen)>DataSize)
				{
					n = DataSize - _pnt;
				}
				else
				{
                    n = nlen * Encoding.Unicode.GetByteCount(new char[1]);
                }
				
				//string str = System.Text.Encoding.GetEncoding("gb2312").GetString(_buffer, _pnt, n);
                string str = Encoding.Unicode.GetString(_buffer, _pnt, n);
					
	            _pnt += n;

                return StringHelper.CleanBlankString(str);
			}
			catch(Exception ex)
			{
				throw ex;
			}
        }
       
        public void GetBytes(ref byte[] data,int nlen)
        {
            Buffer.BlockCopy(_buffer, _pnt, data,0, nlen);
            _pnt += nlen;
        }
        public void CreateHead(ushort pid,ushort sid)
        {
            Pnt = 0;
			_head.DataSize  	= (ushort)Pnt;
			_head.CheckCode 	= SocketSetting.CHECK_CODE;
			_head.MessageVer	= SocketSetting.MESSAGE_VER;
            _head.MainCmdID     = (ushort)pid;
			_head.SubCmdID		= (ushort)sid;

            Addbyte(_head.MessageVer);
            Addbyte(_head.CheckCode);
            AddUShort(_head.DataSize);
            AddUShort(_head.MainCmdID);
			AddUShort(_head.SubCmdID);
        }
        public void BeginRead()
        {
            Pnt = 8;
        }
		public void BeginWrite()
        {
            Pnt = 8;
        }
        
		
		internal void SetBuff( byte[] buff,int len)
        {

            Buffer.BlockCopy(buff,0, _buffer,0,len);
            _head.CheckCode = _buffer[1];
            _head.MessageVer = _buffer[0];
			_head.DataSize  	= BitConverter.ToUInt16(_buffer,2);

			Pnt = len;

        }
       
        //----------------------------------------------------------
		public NPacket(ushort packetId, ushort subCmd)
        {
           
            Pnt = 0;
			_head.DataSize  	= (ushort)Pnt;
			_head.CheckCode 	= SocketSetting.CHECK_CODE;
			_head.MessageVer	= SocketSetting.MESSAGE_VER;
			_head.MainCmdID	= (ushort)packetId;
			_head.SubCmdID		= (ushort)subCmd;

            Addbyte(_head.MessageVer);
            Addbyte(_head.CheckCode);
            AddUShort(_head.DataSize);
            AddUShort(_head.MainCmdID);
			AddUShort(_head.SubCmdID);
			
        }
        public NPacket(ushort packetId): this(packetId, 0) { }

        public NPacket(): this((ushort)0, 0) { }

        
        public void Reset()
        {
            Pnt = 0;
			_head.DataSize  	= 0;
			_head.CheckCode 	= 0;
			_head.MessageVer	= 0;
            _head.MainCmdID = 0;
			_head.SubCmdID		= 0;
        }

    }


    public class NPacketPool
    {
        static int _MaxPoolSize = 400;
        static List<NPacket> enablePacket = new List<NPacket>();

        static NPacketPool()
        {

        }

        public static void CreatePool()
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                enablePacket.Clear();
                for (int i = 0; i < _MaxPoolSize; i++)
                {
                    enablePacket.Add(new NPacket());
                }
            }
        }
        public static NPacket GetEnablePacket()
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                if (enablePacket.Count > 0)
                {
                    var packet = enablePacket[0];
                    enablePacket.Remove(packet);
                    packet.Reset();
                    return packet;
                }
                else
                {
                    Logger.Net.Log("new NPacket.");
                    return new NPacket();
                }
            }
        }
        public static void DropPacket(NPacket packetOne)
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                if (enablePacket.Count < _MaxPoolSize)
                {
                    if (enablePacket.Contains(packetOne))
                    {
                        Logger.Net.LogError("this packet should not be dropped twice.");
                        return;
                    }
                    packetOne.Reset();
                    enablePacket.Add(packetOne);
                }
            }
        }
    }
}
