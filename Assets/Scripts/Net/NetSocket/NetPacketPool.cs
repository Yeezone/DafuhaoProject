using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services
{

    public class NetPacketPool
    {
        private const int MaxPoolSize = 400;
        private static List<Packet> enablePacket = new List<Packet>();

        static NetPacketPool()
        {
        }

        public static void CreatePool()
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                enablePacket.Clear();
                for (int i = 0; i < MaxPoolSize; i++)
                {
                    enablePacket.Add(new Packet(Packet.enPacketType.Network));
                }
            }
        }

        public static Packet PopPacket(Packet.enPacketType type)
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                if (enablePacket.Count > 0)
                {
                    Packet usePacket = enablePacket[0];

                    enablePacket.Remove(usePacket);

                    usePacket.PacketType = type;
                    return usePacket;
                }
                else
                {
                    //Debug.Log("#################### new packet !!!!!!!!!!!!!!:" + System.Environment.TickCount.ToString());
                    return new Packet(type);
                }
            }
        }

        public static void DropPacket(Packet packet)
        {
            lock (((ICollection)enablePacket).SyncRoot)
            {
                if (enablePacket.Count < MaxPoolSize)
                {
                    packet.Reset();
                    enablePacket.Add(packet);
                }
            }
        }
    }
}
