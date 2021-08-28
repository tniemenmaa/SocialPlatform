using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient
{
    [MessagePackObject]
    public class GetGroupsResponse : INetworkMessage
    {
        public GetGroupsResponse(Group[] groups)
        {
            Groups = groups;
        }

        [Key(0)]
        public Group[] Groups;

        [IgnoreMember]
        public byte MessageType => ServerToClientMessageTypes.GetGroupsResponse;

        public byte[] Serialize()
        {
            return MessagePack.MessagePackSerializer.Serialize(this);
        }
    }
}
