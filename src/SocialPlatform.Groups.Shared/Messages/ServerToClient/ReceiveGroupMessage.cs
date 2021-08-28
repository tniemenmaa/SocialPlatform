using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class ReceiveGroupMessage : INetworkMessage
    {
        public ReceiveGroupMessage() { }

        public ReceiveGroupMessage(Guid groupId, GroupMessage message)
        {
            GroupId = groupId;
            Message = message;
        }

        [Key(0)]
        public Guid GroupId { get; set; }

        [Key(1)]
        public GroupMessage Message { get; set; }

        [IgnoreMember]
        public byte MessageType => ServerToClientMessageTypes.ReceiveGroupMessage;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
