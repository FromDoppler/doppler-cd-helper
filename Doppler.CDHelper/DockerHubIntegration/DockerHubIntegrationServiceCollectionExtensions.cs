using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.DockerHubIntegration;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DockerHubIntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddDockerHubIntegration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DockerHubHookSettings>(configuration.GetSection("DockerHubHook"));
            services.AddSingleton<IDockerHubSecretValidator, DockerHubSecretValidator>();
            return services;
        }
    }
}
