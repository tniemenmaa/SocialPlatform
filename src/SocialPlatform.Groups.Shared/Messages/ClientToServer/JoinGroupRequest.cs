using MessagePack;
using SocialPlatform.Groups.Shared.Models;
using System;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
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

        /// <remarks>
        /// In real world this would be resolved on the server and not passed as a property.
        /// </remarks>
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
