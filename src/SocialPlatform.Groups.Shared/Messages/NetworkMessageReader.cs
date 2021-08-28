using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages
{
    public static class NetworkMessageReader
    {
        /// <summary>
        /// Helper function for reading formmatted binary message and returning INetworkMessage.
        /// </summary>
        public static INetworkMessage Read(byte[] data, int offset, int length)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    byte type = reader.ReadByte();
                    byte[] buffer = reader.ReadBytes(length - (int)ms.Position);
                    var message = NetworkMessageSerializer.Deserialize(type, buffer, 0, buffer.Length);
                    return message;
                }
            }
        }
    }
}
