using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages
{
    public static class NetworkMessageReader
    {
        public static (INetworkMessage, int) Read(byte[] data, int offset, int length)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    byte type = reader.ReadByte();
                    int sequence = reader.ReadInt32();
                    byte[] buffer = reader.ReadBytes(length - (int)ms.Position);
                    var message = NetworkMessageSerializer.Deserialize(type, buffer, 0, buffer.Length);
                    return (message, sequence);
                }
            }
        }
    }
}
