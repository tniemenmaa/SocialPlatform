using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialPlatform.GroupRegistry.Shared
{
    public interface IGroupRegistry
    {
        Task AddOrUpdateGroup(Group group);
        Task<IEnumerable<Group>> GetGroups();
        Task RemoveGroup(Guid groupId);
    }
}
