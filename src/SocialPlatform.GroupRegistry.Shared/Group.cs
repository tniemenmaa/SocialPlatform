using MessagePack;
using System;
using System.Runtime.Serialization;

namespace SocialPlatform.GroupRegistry.Shared
{
    [DataContract]
    [MessagePackObject]
    public class Group
    {
        [Key(0)]
        [DataMember]
        public Guid Id { get; set; }
        
        [Key(1)]
        [DataMember]
        public string Name { get; set; }
        
        [Key(2)]
        [DataMember]
        public GroupMember[] Members { get; set; }

        [Key(3)]
        [DataMember]
        public GroupMessage[] Messages { get; set; }
    }
}
