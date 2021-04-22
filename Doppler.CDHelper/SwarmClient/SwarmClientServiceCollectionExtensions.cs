using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwarmClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDummySwarmClient(
            this IServiceCollection services,
            IEnumerable<SwarmServiceDescription> clientResult = null)
        {
            clientResult ??= new[]
            {
                new SwarmServiceDescription()
                {
                    id = "penpnhep0bsciofxzd542v62n",
                    serviceName = "hello-int_hello-service",
                    repository = new SwarmServiceDescriptionRepository()
                    {
                        imageDigest = "sha256:c7a459f13dbf082fe9b6631783f897d54978a32cc91aa8dee5fcb5630fa18a0b",
                        name = "dopplerdock/hello-microservice",
                        tag = "INT"
                    }
                }
            };
            services.AddSingleton<DummySwarmClientResult>(new DummySwarmClientResult(clientResult));
            services.AddSingleton<ISwarmClient, DummySwarmClient>();
            return services;
        }

        public static IServiceCollection AddSwarmpitSwarmClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SwarmpitSwarmClientSettings>(configuration.GetSection("SwarmpitSwarmClient"));
            services.AddSingleton<ISwarmClient, SwarmpitSwarmClient>();
            return services;
        }
    }
}
