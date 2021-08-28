using MessagePack;
using SocialPlatform.Groups.Shared.Models;
using System;
using System.Runtime.Serialization;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
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
