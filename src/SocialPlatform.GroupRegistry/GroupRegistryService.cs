using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabricPlatform.GroupRegistry;
using SocialPlatform.GroupRegistry.Shared;

namespace SocialPlatform.GroupRegistry
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class GroupRegistryService : StatefulService, IGroupRegistryService
    {
        public GroupRegistryService(StatefulServiceContext context)
            : base(context)
        {
            _loggerFactory = new LoggerFactory();
            _logger = _loggerFactory.CreateLogger<GroupRegistryService>();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[] {
                new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _registry = new ReliableStateGroupRegistry(this.StateManager, _loggerFactory.CreateLogger<ReliableStateGroupRegistry>());
        }

        public async Task<Group[]> GetGroups()
        {
            return (await _registry.GetGroups()).ToArray();
        }

        public async Task AddOrUpdateGroup(Group group)
        {
            try
            {
                await _registry.AddOrUpdateGroup(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add group.");
                throw;
            }
        }

        public async Task RemoveGroup(Guid groupId)
        {
            await _registry.RemoveGroup(groupId);
        }

        private IGroupRegistry _registry;
        private ILogger<GroupRegistryService> _logger;
        private LoggerFactory _loggerFactory;
    }
}
