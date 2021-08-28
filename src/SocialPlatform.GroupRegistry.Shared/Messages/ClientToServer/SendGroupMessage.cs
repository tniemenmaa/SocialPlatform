using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ClientToServer
{
    [DataContract]
    [MessagePackObject]
    public class SendGroupMessage : INetworkMessage
    {
        public SendGroupMessage() { }

        public SendGroupMessage(Guid groupId, GroupMessage message)
        {
            GroupId = groupId;
            Message = message;
        }

        [DataMember]
        [Key(0)]
        public Guid GroupId { get; set; }

        [DataMember]
        [Key(1)]
        public GroupMessage Message { get; set; }

        [IgnoreMember]
        public byte MessageType => ClientToServerMessageTypes.SendGroupMessage;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
