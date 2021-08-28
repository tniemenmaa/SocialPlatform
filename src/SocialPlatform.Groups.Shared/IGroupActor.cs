using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using SocialPlatform.Groups.Shared.Models;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace SocialPlatform.Groups.Shared
{
    /// <summary>
    /// Interface for group that offers chat functionality.
    /// </summary>
    public interface IGroupActor : IActor, IActorEventPublisher<IGroupEvents>
    {
        /// <summary>
        /// Attempt to initialize the group.
        /// </summary>
        Task<Group> CreateGroup(string name, GroupMember player);
        
        /// <summary>
        /// Attempts to join the group. Returns the group if successful.
        /// </summary>
        Task<Group> JoinGroup(GroupMember member);

        /// <summary>
        /// Remove player from the group.
        /// </summary>
        Task LeaveGroup(Guid playerId);

        /// <summary>
        /// Return messages from the group. In real world this should support pagination
        /// as the number of messages will grow indefinitely and returning all at once 
        /// makes no sense.
        /// </summary>
        Task<GroupMessage[]> GetMessages();
        
        /// <summary>
        ///  Add message to group and push it to all group members that are online.
        /// </summary>
        Task AddMessage(GroupMessage message);
    }
}
