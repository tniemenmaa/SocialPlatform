using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages
{
    public interface INetworkMessage
    {
        byte MessageType { get; }

        byte[] Serialize();
    }
}
