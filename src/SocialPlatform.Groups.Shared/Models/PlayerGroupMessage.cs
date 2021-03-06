using MessagePack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SocialPlatform.Groups.Shared.Models
{
    /// <summary>
    /// Message type for plain text messages by players.
    /// </summary>
    /// <remarks>
    /// In real world we might have other types of messages as well.
    /// for example system messages or custom payload messages.
    /// </remarks>
    [MessagePackObject]
    public class PlayerGroupMessage : GroupMessage
    {
        public PlayerGroupMessage() { }

        public PlayerGroupMessage( Guid playerId, string text )
        {
            PlayerId = playerId;
            Text = text;
        }

        [Key(2)]
        public Guid PlayerId { get; set; }

        [Key(3)]
        public string Text { get; set; }

        public override string ToConsoleString(Group group)
        {
            string playerName = "Unknown";

            foreach (var member in group.Members)
            {
                if ( PlayerId == member.PlayerId )
                {
                    playerName = member.Name;
                    break;
                }
            }

            return $"[{DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}] {playerName}: {Text}";
        }
    }
}
