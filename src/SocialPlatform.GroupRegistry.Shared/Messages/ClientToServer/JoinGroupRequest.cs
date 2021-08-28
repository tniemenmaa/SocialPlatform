using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ClientToServer
{
    [MessagePackObject]
    public class JoinGroupRequest : INetworkMessage
    {
        public JoinGroupRequest() { }

        public JoinGroupRequest(Guid groupId, GroupMember player)
        {
            GroupId = groupId;
            Player = player;
        }

        [Key(0)]
        public Guid GroupId { get; set; }

        // In real world this would be resolved on the server.
        [Key(1)]
        public GroupMember Player { get; set; }

        [IgnoreMember]
        public byte MessageType => ClientToServerMessageTypes.JoinGroupRequest;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
