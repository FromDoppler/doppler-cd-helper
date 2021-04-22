using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmServiceSelection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwarmServiceSelectionServiceCollectionExtensions
    {
        public static IServiceCollection AddDummySwarmServiceSelector(this IServiceCollection services)
        {
            services.AddSingleton<ISwarmServiceSelector, DummySwarmServiceSelector>();
            return services;
        }

        public static IServiceCollection AddSwarmServiceSelector(this IServiceCollection services)
        {
            services.AddSingleton<ISwarmServiceSelector, SwarmServiceSelector>();
            return services;
        }
    }
}
