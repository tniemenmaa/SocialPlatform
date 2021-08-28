using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient
{
    public static class ServerToClientMessageTypes
    {
        /// Server to client messages are within range [65-127]
        public const byte GetGroupsResponse = 65;
        public const byte CreateGroupResponse = 66;
        public const byte JoinGroupResponse = 67;
        public const byte LeaveGroupResponse = 68;
        public const byte ReceiveGroupMessage = 69;
        public const byte GroupUpdatedMessage = 70;
    }
}
