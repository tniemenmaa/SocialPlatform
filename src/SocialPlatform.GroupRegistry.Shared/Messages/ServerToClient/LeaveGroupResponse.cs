using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class LeaveGroupResponse : INetworkMessage
    {        
        public LeaveGroupResponse() { }

        public LeaveGroupResponse(bool success)
        {
            Success = success;
        }

        [Key(0)]
        public bool Success { get; set; }

        [IgnoreMember]
        public byte MessageType => ServerToClientMessageTypes.LeaveGroupResponse;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
