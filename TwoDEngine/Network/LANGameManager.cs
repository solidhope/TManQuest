using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Microsoft.Xna.Framework;


namespace TwoDEngine.Network
{
    public class LANGameManager : GameComponent, NetworkGameManager, GameProtocolListener
    {
        String name;
        String description;
        Socket multicastSocket;
        IPAddress multicastIpAddr = IPAddress.Parse("224.5.6.7");
        IPEndPoint multicastEndPoint;
        float introFreq;
        float lastIntro;
        Socket p2pSocket;
        Byte[] incomingDataBuffer;
        Dictionary<Guid, NetworkGame> games = new Dictionary<Guid, NetworkGame>();


        public LANGameManager(Game game, String name, String description, int maxDataSize = 1024,
            int ttl = 1, float introductionFrequency = 2, IPAddress multicastIpAddress = null)
            : base(game)
        {
            this.name = name;
            this.description = description;
            this.introFreq = introductionFrequency;
            this.incomingDataBuffer = new Byte[maxDataSize];
            // register us
            game.Services.AddService(typeof(LANGameManager), this);
            game.Components.Add(this);
            Registry.Register(this);
            // set up our broadcast communication
            multicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
            if (multicastIpAddress != null)
            {
                multicastIpAddr = multicastIpAddress;
            }
            multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(multicastIpAddr));
            multicastSocket.SetSocketOption(SocketOptionLevel.IP,
                SocketOptionName.MulticastTimeToLive, ttl);
            multicastEndPoint = new IPEndPoint(multicastIpAddr, 4567);
            multicastSocket.Connect(multicastEndPoint);

        }
        // internal bookeepign methods



        // GameComponent Methods
        public override void Update(GameTime gameTime)
        {
            //process multicast packets
            while (multicastSocket.Available > 0)
            { // data on socket
                multicastSocket.Receive(incomingDataBuffer);
                GameProtocol.Parse(this, null, incomingDataBuffer); // uuid of sender not known so we pass null
            }
            foreach (NetworkGame game in games.Values)
            {
                game.Update();
            }

        }




        // Game Manager Methods

        public NetworkGame CreateGame(string name, string description)
        {
           
        }


        public NetworkGame[] ListGames()
        {
            return games.Values.ToArray();
        }


        // GameProtocolListener methods
        public void GameAnnouncement(Guid uuid, String name, String descr, IPAddress addr, int socknum)
        {
            if (!games.ContainsKey(uuid))
            {
                games.Add(uuid, new RemoteLANGame(uuid, name, descr, addr, socknum));
            }
        }

    }
}
