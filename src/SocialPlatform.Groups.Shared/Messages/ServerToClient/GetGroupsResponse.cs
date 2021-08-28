using MessagePack;
using SocialPlatform.Groups.Shared.Models;

namespace SocialPlatform.Groups.Shared.Messages.ServerToClient
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
