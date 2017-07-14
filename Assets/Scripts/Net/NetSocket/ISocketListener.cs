using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services
{
    public interface ISocketListener
    {
        void OnSocketConnected(NetSocket socket);

        void OnSocketDisconnected(NetSocket socket);

        void OnSocketConnectTimout(NetSocket socket);

        bool OnMessage(NetSocket socket, Packet packet);
    }
}
