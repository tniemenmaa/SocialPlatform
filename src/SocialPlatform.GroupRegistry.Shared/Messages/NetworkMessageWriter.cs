using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages
{
    public static class NetworkMessageWriter
    {
        public static byte[] Write( INetworkMessage message, int sequenceNumber )
        {
            using (MemoryStream ms = new MemoryStream()) {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(message.MessageType);
                    writer.Write(sequenceNumber);
                    writer.Write(message.Serialize());
                }
                return ms.ToArray();
            }
        }


    }
}
