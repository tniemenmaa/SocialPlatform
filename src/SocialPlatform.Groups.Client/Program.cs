using Newtonsoft.Json;
using SocialPlatform.GroupRegistry.Shared;
using SocialPlatform.GroupRegistry.Shared.Messages;
using SocialPlatform.GroupRegistry.Shared.Messages.ClientToServer;
using SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DummyClient();
            Task t = client.RunAsync();
            t.Wait();
        }

        
    }
}
