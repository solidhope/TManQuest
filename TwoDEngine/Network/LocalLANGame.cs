using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine.Network
{
    class LocalLANGame : NetworkGame
    {
        Guid uuid;
        String name;
        String description;

        public LocalLANGame(Guid uuid, String name, String description)
        {
            this.uuid = uuid;
            this.name = name;
            this.description = description;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public void Join()
        {
            throw new NotImplementedException();
        }

        public void Leave()
        {
            throw new NotImplementedException();
        }

        public void SendToPlayer(Guid uuid, byte[] packet)
        {
            throw new NotImplementedException();
        }

        public void SendToAllPlayers(byte[] packet)
        {
            throw new NotImplementedException();
        }
    }
}
