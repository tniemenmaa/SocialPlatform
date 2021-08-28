using Microsoft.ServiceFabric.Services.Remoting;
using SocialPlatform.Groups.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.Shared
{
    public interface IGroupRegistryService : IService
    {
        /// <summary>
        /// Add or update a group definition for group listing.
        /// </summary>
        Task<Group[]> GetGroups();

        /// <summary>
        /// Returns all the groups. 
        /// </summary>
        /// <remarks>
        /// In real world this should support pagination and filtering.
        /// </remarks>
        Task AddOrUpdateGroup(Group group);

        /// <summary>
        /// Remove group so it will no longer show up in group listing.
        /// </summary>
        Task RemoveGroup(Guid guid);
    }
}
