using Microsoft.ServiceFabric.Actors;
using SocialPlatform.GroupRegistry.Shared;
using System;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.Actors.Interfaces
{
    public interface IGroupEvents : IActorEvents
    {
        void GroupUpdated(Group group);
        void GroupMessageReceived(Guid groupId, GroupMessage message);
    }
}
