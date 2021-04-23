using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Doppler.CDHelper.SwarmClient
{
    public class SwarmpitSwarmClient : ISwarmClient
    {
        private readonly SwarmpitSwarmClientSettings _settings;

        public SwarmpitSwarmClient(IOptions<SwarmpitSwarmClientSettings> options)
        {
            _settings = options.Value;
        }

        public Task<IEnumerable<SwarmServiceDescription>> GetServices()
            => $"{_settings.BaseUrl}services"
                .WithOAuthBearerToken(_settings.AccessToken)
                .GetJsonAsync<IEnumerable<SwarmServiceDescription>>();

        public Task RedeployService(string serviceId)
            => $"{_settings.BaseUrl}services/{serviceId}/redeploy"
                .WithOAuthBearerToken(_settings.AccessToken)
                .PostAsync();
    }
}
