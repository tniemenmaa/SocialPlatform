using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using SocialPlatform.Groups.Shared;

namespace SocialPlatform.Groups.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();

            // Register group service for dependecy injection
            var proxyFactory = new ServiceProxyFactory(client => new FabricTransportServiceRemotingClientFactory());
            var groupService = proxyFactory.CreateServiceProxy<IGroupRegistryService>(Constants.GroupRegistryUri, new ServicePartitionKey(0));
            services.AddSingleton<IGroupRegistryService>(groupService);

            services.AddScoped<WebSocketConnection>();
    
            // In production this should be replaced with proper logging
            services.AddLogging(options =>
            {
                options.AddConsole();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseWebSockets();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });

            app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
