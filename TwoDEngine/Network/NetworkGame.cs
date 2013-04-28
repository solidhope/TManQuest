using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoDEngine.Network
{
    public interface NetworkGame
    {
        String GetName();
        String GetDescription();
        void Join();
        void Leave();
        void SendToPlayer(Guid uuid, byte[] packet);
        void SendToAllPlayers(byte[] packet);
    }
}
