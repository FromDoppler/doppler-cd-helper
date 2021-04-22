using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;

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
                    id = "dummyId"
                }
            };
            services.AddSingleton<DummySwarmClientResult>(new DummySwarmClientResult(clientResult));
            services.AddSingleton<ISwarmClient, DummySwarmClient>();
            return services;
        }
    }
}
