using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using SocialPlatform.Groups.Shared;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace SocialPlatform.Groups.Actors.Interfaces
{
    public interface IGroupActor : IActor, IActorEventPublisher<IGroupEvents>
    {
        Task<Group> CreateGroup(string name, GroupMember player);
        
        /// <summary>
        /// Attempts to join the group. Returns the group if successful.
        /// </summary>
        Task<Group> JoinGroup(GroupMember member);

        Task LeaveGroup(Guid playerId);

        Task<GroupMessage[]> GetMessages();
        Task AddMessage(GroupMessage message);
    }
}
