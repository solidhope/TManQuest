using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwoDEngine.Network
{
    interface GameProtocolListener
    {
        void ManagerIntroRequest(Guid uuid, IPAddress addr, int socket);
    }
}
