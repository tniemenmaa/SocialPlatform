using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using SocialPlatform.Groups.Shared;
using SocialPlatform.Groups.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabricPlatform.GroupRegistry
{
    /// <remarks>
    /// Group registry build on top of Reliable Containers. 
    /// In real-world application this should be replaced with a database backed registry.
    /// </remarks>
    public class ReliableStateGroupRegistry : IGroupRegistry
    {
        public ReliableStateGroupRegistry(IReliableStateManager stateManager, ILogger<ReliableStateGroupRegistry> logger)
        {
            _stateManager = stateManager;
            _logger = logger;
        }


        public async Task AddOrUpdateGroup(Group group)
        {
            var groups = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Group>>(Constants.GroupCollectionName);

            using (ITransaction transaction = _stateManager.CreateTransaction())
            {
                await groups.AddOrUpdateAsync(transaction, group.Id, group, (id, value) => { return group; });
                await transaction.CommitAsync();
            }
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            IReliableDictionary<Guid, Group> groups = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Group>>(Constants.GroupCollectionName);

            var result = new List<Group>();

            using (ITransaction transaction = _stateManager.CreateTransaction())
            {
                var allGroups = await groups.CreateEnumerableAsync(transaction, EnumerationMode.Unordered);

                using (var enumerator = allGroups.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }


        public async Task RemoveGroup(Guid groupId)
        {
            IReliableDictionary<Guid, Group> groups = await _stateManager
              .GetOrAddAsync<IReliableDictionary<Guid, Group>>(Constants.GroupCollectionName);

            using (ITransaction transaction = _stateManager.CreateTransaction())
            {
                await groups.TryRemoveAsync(transaction, groupId);
                await transaction.CommitAsync();
            }
        }

        private readonly IReliableStateManager _stateManager;
        private readonly ILogger<ReliableStateGroupRegistry> _logger;
    }
}
