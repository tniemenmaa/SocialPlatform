using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SocialPlatform.Groups.Shared
{
    /// <summary>
    /// Base class for chat messages for groups.
    /// </summary>
    [KnownType(typeof(PlayerGroupMessage))]
    [DataContract]
    [Union(0, typeof(PlayerGroupMessage))]
    [MessagePackObject]
    public abstract class GroupMessage
    {
        /// <summary>
        /// Epoch format timestamp
        /// </summary>
        [DataMember]
        [Key(0)]
        public long Timestamp { get; set; }

        /// <summary>
        /// Ordinal number for the mssage starting from 0 for first message.
        /// </summary>
        [DataMember]
        [Key(1)]
        public int Ordinal { get; set; }

        // In real world we would have more sophisticated system for displaying messages.
        public abstract string ToConsoleString(Group group);
    }
}
