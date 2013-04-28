using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoDEngine.Network
{
    public interface NetworkGameListener
    {
        void PlayerJoined(NetworkGamePlayer player);
        void PlayerLeft(NetworkGamePlayer player);
        void PublicMessageArrived(MemoryStream message);
        void PrivateMessageArrived(NetworkGamePlayer player, MemoryStream message);
    }
}
