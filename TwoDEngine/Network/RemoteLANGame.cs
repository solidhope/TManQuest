using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TwoDEngine.Network
{
    class RemoteLANGame: NetworkGame, GameProtocolListener
    {
        Guid uuid;
        String name;
        String description;
        Dictionary<Guid, Socket> playerSockets = new Dictionary<Guid, Socket>();
        IPAddress hostIp;
        int hostPort;
        Socket hostSocket;
        Socket incomingSockets;
        byte[] inbuff;


        public RemoteLANGame(Guid uuid, String name, String descr, IPAddress hostIp, int hostPort)
        {
            this.uuid = uuid;
            this.name = name;
            this.description = descr;
            this.hostIp = hostIp;
            this.hostPort = hostPort;
        }

        public void Join()
        {
            // init buffer
            inbuff = new byte[1024];
            // create a socket to listen for connections
            IPEndPoint listenerEndPoint = new IPEndPoint(IPAddress.Any, 0);
            incomingSockets = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp );
                
            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            incomingSockets.Bind(listenerEndPoint);
            incomingSockets.Listen(10);

             // Establish the remote endpoint for the outgoing host socket.
            // This example uses port 11000 on the local computer.
            IPEndPoint remoteEP = new IPEndPoint(hostIp,hostPort);

            // Create TCP/IP  socket.
            hostSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp );
            playerSockets.Add(uuid, hostSocket);
            hostSocket.Connect(remoteEP);
            hostSocket.Send(GameProtocol.MakeJoinPacket(uuid, listenerEndPoint.Address, listenerEndPoint.Port));
        }

        public void SendToPlayer(Guid uuid, byte[] packet)
        {
            playerSockets[uuid].Send(packet);
        }

        public void SendToAllPlayers(byte[] packet)
        {
            foreach (Socket s in playerSockets.Values)
            {
                s.Send(packet);
            }
        }

        public void Update()
        {
            // first handle anybody incoming
            while (incomingSockets.Poll(0, SelectMode.SelectRead))
            {
                Socket s = incomingSockets.Accept();
                s.Receive(inbuff);
                MemoryStream ms = new MemoryStream(inbuff);
                BinaryReader br = new BinaryReader(ms);
                Guid remoteUuid = GameProtocol.ReadGuid(br);
                playerSockets.Add(remoteUuid, s);
            }
            // now handle incoming packets on any of our connections
            foreach (KeyValuePair<Guid,Socket> kvp in playerSockets)
            {
                Guid uuid = kvp.Key;
                Socket s = kvp.Value;
                int avail = s.Available;
                while (avail > 0)
                {
                    s.Receive(inbuff);
                    GameProtocol.Parse(this, uuid, inbuff);
                }
            }
        }


        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public void Leave()
        {
            throw new NotImplementedException();
        }


        public void GameAnnouncement(Guid uuid, string name, string descr, IPAddress addr, int socknum)
        {
            // should never get one of these
            throw new Exception("Game announcement reception unsupported");
        }



    }
}
