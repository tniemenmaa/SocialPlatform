using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class GroupUpdatedMessage : INetworkMessage
    {
        public GroupUpdatedMessage() { }

        public GroupUpdatedMessage(Group group)
        {
            Group = group;
        }

        [Key(0)]
        public Group Group { get; set; }

        [IgnoreMember]
        public byte MessageType => ServerToClient.ServerToClientMessageTypes.GroupUpdatedMessage;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
