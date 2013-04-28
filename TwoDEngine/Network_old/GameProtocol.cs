using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace TwoDEngine.Network
{
    static class GameProtocol
    {
        enum OPCODE { REQUEST_INTRO, INTRO };

        public static void Parse(GameProtocolListener listener, Byte[] packet)
        {
            BinaryReader rdr = new BinaryReader(new MemoryStream(packet));
            OPCODE opcode = (OPCODE)rdr.ReadByte();
            switch (opcode)
            {
                case OPCODE.REQUEST_INTRO:
                    int uuidLength = rdr.ReadInt32();
                    byte[] uuidBytes = rdr.ReadBytes(uuidLength);
                    Guid uuid = new Guid(uuidBytes);
                    String ipStr = rdr.ReadString();
                    IPAddress ip = IPAddress.Parse(ipStr);
                    int socketNum = rdr.ReadInt32();
                    listener.ManagerIntroRequest(uuid, ip, socketNum);
                    break;
                case OPCODE.INTRO:

                    break;
            }
        }

        internal static Byte[] MakeRequestIntroPacket(Guid uuid, IPAddress ip, int socketNum)
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            writer.Write((byte)OPCODE.REQUEST_INTRO);
            byte[] uuidBytes =uuid.ToByteArray();
            writer.Write(uuidBytes.Length);
            writer.Write(uuidBytes);
            writer.Write(ip.ToString());
            writer.Write(socketNum);
            writer.Close();
            return mstream.ToArray();
        }

        internal static byte[] MakeIntroPacket(Guid uuid, List<LANGameImpl> hostedGameList)
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            writer.Write((byte)OPCODE.INTRO);
            byte[] uuidBytes = uuid.ToByteArray();
            writer.Write(uuidBytes.Length);
            writer.Write(uuidBytes);
            
            return mstream.ToArray();
        }
    }
}
