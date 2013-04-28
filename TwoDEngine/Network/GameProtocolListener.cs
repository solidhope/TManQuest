using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwoDEngine.Network
{
    interface GameProtocolListener
    {
        void GameAnnouncement(Guid uuid, String name, String descr, IPAddress addr,int socknum);
    }
}
