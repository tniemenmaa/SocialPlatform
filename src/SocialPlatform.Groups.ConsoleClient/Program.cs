using System.Threading.Tasks;

namespace SocialPlatform.Groups.ConsoleClient
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
