using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
{
    public static class ClientToServerMessageTypes
    {
        // Client to server messages are within range [1-64]
        public const byte GetGroupsRequest = 1;
        public const byte CreateGroupRequest = 2;
        public const byte JoinGroupRequest = 3;
        public const byte LeaveGroupRequest = 4;
        
        public const byte SendGroupMessage = 5;
    }
}
