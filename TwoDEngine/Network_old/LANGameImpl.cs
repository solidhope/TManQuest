using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine.Network
{
    class LANGameImpl : NetworkGame
    {
        Guid uud;
        String name;
        String description;

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

        public void SendMessage(System.IO.MemoryStream bytes)
        {
            throw new NotImplementedException();
        }

        public void AddListener(NetworkGameListener listener)
        {
            throw new NotImplementedException();
        }
    }
}
