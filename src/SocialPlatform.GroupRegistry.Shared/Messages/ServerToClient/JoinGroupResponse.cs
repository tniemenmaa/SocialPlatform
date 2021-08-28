using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class JoinGroupResponse : INetworkMessage
    {
        public JoinGroupResponse() { }

        public JoinGroupResponse(bool success, Group group)
        {
            Success = success;
            Group = group;
        }

        [Key(0)]
        public bool Success { get; set; }

        [Key(1)]
        public Group Group { get; set; }

        [IgnoreMember]
        public byte MessageType => ServerToClientMessageTypes.JoinGroupResponse;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
