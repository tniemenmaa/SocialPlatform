using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using SocialPlatform.Groups.Actors.Interfaces;
using SocialPlatform.GroupRegistry.Shared;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace SocialPlatform.Groups.Actors
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class GroupActor : Actor, IGroupActor
    {
        public const int MaxMemberCount = 20;

        public GroupActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
            var proxyFactory = new ServiceProxyFactory(client => new FabricTransportServiceRemotingClientFactory());
            _groupRegistry = proxyFactory.CreateServiceProxy<IGroupRegistryService>(Constants.GroupRegistryUri, new ServicePartitionKey(0));
        }

        protected async override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            
            var state = await StateManager.TryGetStateAsync<GroupState>(this.Id.ToString());
            
            if (state.HasValue)
            {
                _state = state.Value;
            }

            await base.OnActivateAsync();
        }

        protected override async Task OnDeactivateAsync()
        {
            if (_state != null)
            {
                await StateManager.SetStateAsync(this.Id.ToString(), _state);
                await StateManager.SaveStateAsync();
            }

            await base.OnDeactivateAsync();
        }

        public async Task<Group> CreateGroup(string name, GroupMember player)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (_state != null )
            {
                throw new InvalidOperationException("Group is already created.");
            }

            _state = new GroupState();
            _state.Name = name;
            _state.Members.Add(player);

            await StateManager.SetStateAsync(this.Id.ToString(), _state);
            await StateManager.SaveStateAsync();


            // Add entry to registry as well
            var group = new Group
            {
                Id = this.Id.GetGuidId(),
                Name = _state.Name,
                Members = _state.Members.ToArray()
            };

            await _groupRegistry.AddOrUpdateGroup(group);

            return group;
        }

        public async Task AddMessage(GroupMessage message)
        {

            message.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _state.Messages.Add(message);

            await StateManager.SetStateAsync(this.Id.ToString(), _state);

            // Push message to all players who are online
            var ev = GetEvent<IGroupEvents>();
            ev.GroupMessageReceived(this.Id.GetGuidId(), message);
        }

        public async Task<Group> JoinGroup(GroupMember member)
        {
            if (_state.Members.Count >= GroupActor.MaxMemberCount)
            {
                return null;
            }

            _state.Members.Add(member);
            await StateManager.SetStateAsync(this.Id.ToString(), _state);
            await StateManager.SaveStateAsync();

            var group =  new Group
            {
                Id = this.Id.GetGuidId(),
                Name = _state.Name,
                Members = _state.Members.ToArray(),
                Messages = _state.Messages.ToArray()
            };

            // Update registry
            await _groupRegistry.AddOrUpdateGroup(group);

            // Notify other players
            var ev = GetEvent<IGroupEvents>();
            ev.GroupUpdated(group);

            return group;
        }

        public async Task LeaveGroup(Guid playerId)
        {
            if ( _state == null )
            {
                throw new InvalidOperationException("Group does not exist.");
            }

            var member = _state.Members.Find(x => x.PlayerId == playerId);

            if ( member != null )
            {
                _state.Members.Remove(member);
                await StateManager.SetStateAsync(this.Id.ToString(), _state);
                await StateManager.SaveStateAsync();
            }

            if ( _state.Members.Count == 0 )
            {
                // Remove state and remove group from registry
                _state = null;
                await _groupRegistry.RemoveGroup(this.Id.GetGuidId());
                await StateManager.RemoveStateAsync(this.Id.ToString());
            }
            else
            {
                // Notify other players
                var ev = GetEvent<IGroupEvents>();
                var group = new Group
                {
                    Id = this.Id.GetGuidId(),
                    Name = _state.Name,
                    Members = _state.Members.ToArray()
                };
                ev.GroupUpdated(group);
            }
        }

        public Task<GroupMessage[]> GetMessages()
        {
            return Task.FromResult(_state.Messages.ToArray());
        }

        private GroupState _state;
        private IGroupRegistryService _groupRegistry;
    }
}
