using System;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.Services
{
    /// <summary>
    /// 封装网络消息
    /// </summary>
    /// @Author: guofeng
    /// 
    public class Packet
	{
	    public enum enPacketType
	    {
            None,
            Internal,
            System,
            Network
	    }

	    public enum enInternalMsgType
	    {
            Failed,
            Timeout,
            Connected,
            Disconnected
	    }

        public UInt32 MainCmd { get; set; }
        public UInt32 SubCmd { get; set; }
        public UInt32 Size { get; set; }
        public Int32 CheckCode { get; set; }
        public UInt32 Version { get; set; }
        public UInt32 HeadSize { get; set; }

        public byte[] Data { get; set; }
        public int DataSize 
        {
            get
            { 
                return Data == null ? 0 : Data.Length;
            }
        }

        public enPacketType PacketType { get; set; }

	    public Packet(enPacketType type)
	    {
            Reset();
            this.PacketType = type;
        }

	    public void Reset()
	    {
            this.PacketType = enPacketType.None;
            this.MainCmd = 0;
            this.SubCmd = 0;
            this.Size = 0;
            this.CheckCode = 0;
            this.Version = 0;
            this.Data = null;

            this.HeadSize = 0;
	    }

        public override string ToString()
        {
            return "Packet |main:" + MainCmd + "|sub:" + SubCmd + "|size:" + Size+" |Total size:"+(Size+HeadSize);
        }
       
	}

}

