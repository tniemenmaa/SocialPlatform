using SocialPlatform.Groups.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.Shared
{
    public interface IGroupRegistry
    {

        /// <summary>
        /// Add or update a group definition for group listing.
        /// </summary>
        Task AddOrUpdateGroup(Group group);

        /// <summary>
        /// Returns all the groups. 
        /// </summary>
        /// <remarks>
        /// In real world this should support pagination and filtering.
        /// </remarks>
        Task<IEnumerable<Group>> GetGroups();

        /// <summary>
        /// Remove group so it will no longer show up in group listing.
        /// </summary>
        Task RemoveGroup(Guid groupId);
    }
}
