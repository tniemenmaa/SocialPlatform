using MessagePack;
using SocialPlatform.Groups.Shared.Models;

namespace SocialPlatform.Groups.Shared.Messages.ClientToServer
{
    [MessagePackObject]
    public class CreateGroupRequest : INetworkMessage
    {
        [Key(0)]
        public string Name { get; set; }

        // In real world this would be fetched from some sort of user service
        // based on clients access token / identity token / session user,
        // but for simplicity we allow client to supply this
        [Key(1)]
        public GroupMember Creator { get; set; }

        [IgnoreMember]
        public byte MessageType => ClientToServerMessageTypes.CreateGroupRequest;

        public CreateGroupRequest(string name, GroupMember creator)
        {
            Name = name;
            Creator = creator;
        }

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
