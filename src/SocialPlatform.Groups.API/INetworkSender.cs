using SocialPlatform.GroupRegistry.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.API
{
    public interface INetworkSender
    {
        Task SendMessageAsync(INetworkMessage message);
    }
}
