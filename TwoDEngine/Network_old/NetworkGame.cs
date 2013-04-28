using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoDEngine.Network
{
    public interface NetworkGame
    {
        public String GetName();
        public String GetDescription();
        public void Join();
        public void Leave();
        public void SendMessage(MemoryStream bytes);
        public void AddListener(NetworkGameListener listener);
    }
}
