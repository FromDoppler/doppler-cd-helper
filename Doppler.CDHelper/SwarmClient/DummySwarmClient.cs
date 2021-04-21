using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.SwarmClient
{
    public record DummySwarmClientResult(IEnumerable<SwarmServiceDescription> Value);

    public class DummySwarmClient : ISwarmClient
    {
        private readonly ILogger<DummySwarmClient> _logger;
        private readonly DummySwarmClientResult _dummyResult;

        public DummySwarmClient(
            ILogger<DummySwarmClient> logger,
            DummySwarmClientResult dummyResult)
        {
            _logger = logger;
            _dummyResult = dummyResult;
        }

        public Task<IEnumerable<SwarmServiceDescription>> GetServices()
        {
            _logger.LogInformation("GetServices()");
            return Task.FromResult(_dummyResult.Value);
        }

        public Task RedeployService(string serviceId)
        {
            _logger.LogInformation("RedeployService({serviceId})", serviceId);
            return Task.CompletedTask;
        }
    }
}
