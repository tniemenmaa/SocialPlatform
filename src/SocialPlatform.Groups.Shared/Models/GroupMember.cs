using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Models
{
    [MessagePackObject]
    public class GroupMember
    {
        [Key(0)]
        public Guid PlayerId { get; set; }
        [Key(1)]
        public string Name { get; set; }

        public GroupMember() {}

        public GroupMember(Guid playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }
    }
}
