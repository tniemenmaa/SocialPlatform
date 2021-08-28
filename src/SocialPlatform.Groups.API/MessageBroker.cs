using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using SocialPlatform.Groups.Shared;
using SocialPlatform.Groups.Shared.Messages;
using SocialPlatform.Groups.Shared.Messages.ClientToServer;
using SocialPlatform.Groups.Shared.Messages.ServerToClient;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using SocialPlatform.Groups.Shared.Models;
using System.Collections.Generic;

namespace SocialPlatform.Groups.API
{
    /// <summary>
    /// Handles websocket connection and routing messages.
    /// </summary>
    public class MessageBroker : IGroupEvents
    {
        public MessageBroker(Guid playerId, IGroupRegistryService groupRegistryService, WebSocket socket, ILogger<MessageBroker> logger )
        {
            _playerId = playerId;
            _groupRegistryService = groupRegistryService;
            _socket = socket;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            _sendQueue = new ActionBlock<INetworkMessage>(async message => await SendMessageAsync(message));
            await Task.Run(() => ReceiveAsync(ct), ct);
        }

        public async Task HandleNetworkMessage( INetworkMessage message )
        { 
            INetworkMessage response;

            switch (message.MessageType)
            {
                case ClientToServerMessageTypes.GetGroupsRequest:
                    {
                        var groups = await _groupRegistryService.GetGroups();
                        response = new GetGroupsResponse(groups);
                        await SendMessageAsync(response);
                        break;
                    }
                case ClientToServerMessageTypes.CreateGroupRequest:
                    {
                        try
                        {
                            var cgr = (CreateGroupRequest)message;
                            var proxy = ActorProxy.Create<IGroupActor>(new ActorId(Guid.NewGuid()), Constants.GroupActorUri);
                            var group = await proxy.CreateGroup(cgr.Name, cgr.Creator);
                            response = new CreateGroupResponse(true, group);
                            await SubscribeToGroupEvents(group.Id);
                            await SendMessageAsync(response);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to handle {nameof(CreateGroupRequest)}");
                            response = new CreateGroupResponse(false, null);
                            await SendMessageAsync(response);
                        }
                        break;
                    }
                case ClientToServerMessageTypes.JoinGroupRequest:
                    {
                        var jgr = (JoinGroupRequest)message;
                        var proxy = ActorProxy.Create<IGroupActor>(new ActorId(jgr.GroupId), Constants.GroupActorUri);
                        var group = await proxy.JoinGroup(jgr.Player);
                        response = new JoinGroupResponse(group != null, group);
                        if (group != null)
                        {
                            await SubscribeToGroupEvents(group.Id);
                        }
                        await SendMessageAsync(response);
                        break;
                    }
                case ClientToServerMessageTypes.LeaveGroupRequest:
                    {
                        try
                        {
                            var lgr = (LeaveGroupRequest)message;
                            var proxy = ActorProxy.Create<IGroupActor>(new ActorId(lgr.GroupId), Constants.GroupActorUri);
                            await proxy.LeaveGroup(lgr.PlayerId);
                            response = new LeaveGroupResponse(true);
                            await UnsubscribeFromGroupEvents(lgr.GroupId);
                            await SendMessageAsync(response);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to handle {nameof(LeaveGroupRequest)}");
                            response = new LeaveGroupResponse(false);
                            await SendMessageAsync(response);
                        }
                        break;
                    }
                case ClientToServerMessageTypes.SendGroupMessage:
                    {
                        var sgm = (SendGroupMessage)message;
                        var proxy = ActorProxy.Create<IGroupActor>(new ActorId(sgm.GroupId), Constants.GroupActorUri);
                        await proxy.AddMessage(sgm.Message);
                    }
                    break;

                default:
                    _logger.LogError($"Unknown message type ({message.MessageType}) received.");
                    break;

            }
        }

        private async Task ReceiveAsync(CancellationToken ct)
        {
            try
            {
                var buffer = new byte[4096];
                WebSocketReceiveResult result;
                List<byte> messageBuffer = new List<byte>();

                // Keep listening to the socket until connection is closed
                do
                {
                    /// <remark> 
                    /// This is very naive implementation that does not handle
                    /// messages that are longer then 4096 bytes.
                    /// In real world application we should read until we encounter
                    /// EndOfMessage and possibly include the size of the message in the payload for optimization 
                    /// </remark>
                    result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);

                    if (result.EndOfMessage)
                    {
                        var message = NetworkMessageReader.Read(buffer, 0, result.Count);

                        await HandleNetworkMessage(message);
                    }

                }
                while (!result.CloseStatus.HasValue && !ct.IsCancellationRequested);

                await _socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Receive loop encountered exception.");
            }

        }

        private async Task SubscribeToGroupEvents( Guid groupId )
        {
            var proxy = ActorProxy.Create<IGroupActor>(new ActorId(groupId), Constants.GroupActorUri);
            await proxy.SubscribeAsync<IGroupEvents>(this);
        }

        private async Task UnsubscribeFromGroupEvents( Guid groupId ) 
        {
            var proxy = ActorProxy.Create<IGroupActor>(new ActorId(groupId), Constants.GroupActorUri);
            await proxy.UnsubscribeAsync<IGroupEvents>(this);
        }

        private async Task SendMessageAsync(INetworkMessage message)
        {
            try
            {
                var data = NetworkMessageWriter.Write(message);
                await _socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.Message($"Error sending message {e.Message}");
            }
        }

#region IGroupEvents

        public void GroupMessageReceived(Guid groupId, GroupMessage message)
        {
            ServiceEventSource.Current.Message($"Group message received by {_playerId}");
            _sendQueue.Post(new ReceiveGroupMessage(groupId, message));
        }

        public void GroupUpdated(Group group)
        {
            _sendQueue.Post(new GroupUpdatedMessage(group));
        }

#endregion
        
        private ActionBlock<INetworkMessage> _sendQueue;
        private WebSocket _socket;
        private Guid _playerId;
        private IGroupRegistryService _groupRegistryService;
        private ILogger<MessageBroker> _logger;
    }
}
