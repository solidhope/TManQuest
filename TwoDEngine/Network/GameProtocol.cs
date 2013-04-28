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
        enum OPCODE { INTRO };

        internal static Guid ReadGuid(BinaryReader rdr)
        {
            int uuidLength = rdr.ReadInt32();
            byte[] uuidBytes = rdr.ReadBytes(uuidLength);
            Guid uuid = new Guid(uuidBytes);
            return uuid;
        }

        internal static void WriteGuid(Guid uuid, BinaryWriter writer)
        {
            byte[] uuidBytes = uuid.ToByteArray();
            writer.Write(uuidBytes.Length);
            writer.Write(uuidBytes);
        }

        public static void Parse(GameProtocolListener listener, Guid? sender, Byte[] packet)
        {
            BinaryReader rdr = new BinaryReader(new MemoryStream(packet));
            OPCODE opcode = (OPCODE)rdr.ReadByte();
            switch (opcode)
            {
                case OPCODE.INTRO:
                    int uuidLength = rdr.ReadInt32();
                    byte[] uuidBytes = rdr.ReadBytes(uuidLength);
                    Guid uuid = new Guid(uuidBytes);
                    String name = rdr.ReadString();
                    String descr = rdr.ReadString();
                    IPAddress addr = IPAddress.Parse(rdr.ReadString());
                    int socketNum = rdr.ReadInt32();
                    listener.GameAnnouncement(uuid, name, descr,addr,socketNum);
                    break;
                ;
            }
        }

        internal static byte[] MakeGameAnnouncePacket(Guid uuid, String name, String description,IPAddress ip, int socketNum)
        {
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            writer.Write((byte)OPCODE.INTRO);
            byte[] uuidBytes = uuid.ToByteArray();
            writer.Write(uuidBytes.Length);
            writer.Write(uuidBytes);
            writer.Write(name);
            writer.Write(description);
            writer.Write(ip.ToString());
            writer.Write(socketNum);
            return mstream.ToArray();
        }



        internal static IList<ArraySegment<byte>> MakeJoinPacket(Guid uuid, IPAddress iPAddress, int p)
        {
            throw new NotImplementedException();
        }
    }
}
