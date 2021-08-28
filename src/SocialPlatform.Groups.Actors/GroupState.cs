using SocialPlatform.Groups.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Actors
{
    public class GroupState
    {
        public string Name { get; set; }

        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

        public List<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
