using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            // TODO: implement me!
            => Task.FromResult(Enumerable.Empty<SwarmServiceDescription>());

        public Task RedeployService(string serviceId)
            // TODO: implement me!
            => Task.CompletedTask;
    }
}
