using Microsoft.ServiceFabric.Actors;
using SocialPlatform.Groups.Shared.Models;
using System;

namespace SocialPlatform.Groups.Shared
{
    /// <summary>
    /// Interface for subscribing to events sent by IGroupActor.
    /// </summary>
    public interface IGroupEvents : IActorEvents
    {
        /// <summary>
        /// Notification for when group composition has changed.
        /// </summary>
        void GroupUpdated(Group group);

        /// <summary>
        /// Notification for when a new message is sent to the group.
        /// </summary>
        void GroupMessageReceived(Guid groupId, GroupMessage message);
    }
}
