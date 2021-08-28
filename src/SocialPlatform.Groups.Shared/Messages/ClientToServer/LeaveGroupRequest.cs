using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
{
    [MessagePackObject]
    public class LeaveGroupRequest : INetworkMessage
    {
        public LeaveGroupRequest() { }

        public LeaveGroupRequest(Guid groupId, Guid playerId)
        {
            GroupId = groupId;
            PlayerId = playerId;
        }

        [Key(0)]
        public Guid GroupId { get; set; }

        // In real world this would not be passed as property
        [Key(1)]
        public Guid PlayerId { get; set; }

        [IgnoreMember]
        public byte MessageType => ClientToServerMessageTypes.LeaveGroupRequest;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
