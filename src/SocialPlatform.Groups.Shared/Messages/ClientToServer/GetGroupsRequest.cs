using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
{
    [MessagePackObject]
    public class GetGroupsRequest : INetworkMessage
    {
        [IgnoreMember]
        public byte MessageType => ClientToServerMessageTypes.GetGroupsRequest;

        public byte[] Serialize()
        {
            return MessagePack.MessagePackSerializer.Serialize(this);
        }
    }
}
