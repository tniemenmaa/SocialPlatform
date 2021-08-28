using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class CreateGroupResponse : INetworkMessage
    {
        [Key(0)]
        public bool Success { get; set; }

        [Key(1)]
        public Group Group { get; set; }

        [IgnoreMember]
        public byte MessageType => ServerToClientMessageTypes.CreateGroupResponse;

        public CreateGroupResponse(bool success, Group group)
        {
            Success = success;
            Group = group;
        }

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
