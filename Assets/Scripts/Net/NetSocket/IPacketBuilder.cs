using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services
{
    public interface IPacketBuilder
    {
        void Reset();
        Packet FromBytes(byte[] data, int len, bool encrypted);
        byte[] ToBytes(Packet packet);
        byte[] Decrypt(byte[] bytes);
        byte[] Encrypt(byte[] bytes);
    }
}
