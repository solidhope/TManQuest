using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine.Network
{
    public interface NetworkGamePlayer
    {
        void SendPlayerMessage(MemoryStream bytes);
        string GetPlayerName();
        
    }
}
