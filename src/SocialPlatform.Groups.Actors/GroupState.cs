using SocialPlatform.Groups.Shared.Models;
using System.Collections.Generic;

namespace SocialPlatform.Groups.Actors
{
    public class GroupState
    {
        public string Name { get; set; }

        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

        public List<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
