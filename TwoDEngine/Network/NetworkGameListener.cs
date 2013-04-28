using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoDEngine.Network
{
    public interface NetworkGameListener
    {
        void PlayerJoined(Guid player);
        void PlayerLeft(Guid player);
        void PublicMessageArrived(MemoryStream message);
        void PrivateMessageArrived(Guid player, MemoryStream message);
    }
}
