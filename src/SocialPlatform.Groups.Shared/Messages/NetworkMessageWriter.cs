using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages
{
    public static class NetworkMessageWriter
    {        
        /// <summary>
        /// Helper function for writing formmatted binary message from INetworkMessage.
        /// </summary>
        public static byte[] Write(INetworkMessage message)
        {
            using (MemoryStream ms = new MemoryStream()) {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(message.MessageType);
                    writer.Write(message.Serialize());
                }
                return ms.ToArray();
            }
        }


    }
}
