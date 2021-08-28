using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialPlatform.Groups.Shared;
using SocialPlatform.Groups.Shared.Messages;
using SocialPlatform.Groups.Shared.Messages.ClientToServer;
using SocialPlatform.Groups.Shared.Messages.ServerToClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.API
{
    public class WebSocketMiddleware
    {
        public WebSocketMiddleware(RequestDelegate next, IGroupRegistryService groupRegistryService, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _groupRegistryService = groupRegistryService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    /// <remarks>
                    /// In real world this would most likely be some token that was received from the authentication endpoint.
                    /// That we could verify against the authenticaiton endpoint to get the identity of the player.
                    /// </remarks>
                    var playerId = context.Request.Headers["playerId"][0];

                    // Create and run the message broker for routing messages between the client and different services.
                    var messageBroker = new MessageBroker(Guid.Parse(playerId), _groupRegistryService, socket, null);
                    await messageBroker.RunAsync(CancellationToken.None);
                }
                else
                {
                    _logger.LogInformation("Received invalid websocket request.");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Expected websocket request");
                }
            }
            else
            {
                await _next(context);
            }
        }

        private RequestDelegate _next;
        private IGroupRegistryService _groupRegistryService;
        private ILogger<WebSocketMiddleware> _logger;

    }
}
