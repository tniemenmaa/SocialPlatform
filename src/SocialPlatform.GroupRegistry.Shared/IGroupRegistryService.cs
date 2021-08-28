using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialPlatform.GroupRegistry.Shared
{
    public interface IGroupRegistryService : IService
    {
        Task<Group[]> GetGroups();
        Task AddOrUpdateGroup(Group group);
        Task RemoveGroup(Guid guid);
    }
}
