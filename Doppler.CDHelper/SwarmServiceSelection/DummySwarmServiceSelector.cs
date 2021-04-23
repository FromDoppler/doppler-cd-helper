using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.DockerHubIntegration;
using Doppler.CDHelper.SwarmClient;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.SwarmServiceSelection
{
    public class DummySwarmServiceSelector : ISwarmServiceSelector
    {
        private readonly ILogger<DummySwarmServiceSelector> _logger;

        public DummySwarmServiceSelector(ILogger<DummySwarmServiceSelector> logger)
        {
            _logger = logger;
        }

        public IEnumerable<SwarmServiceDescription> GetServicesToRedeploy(DockerHubHookData hookData, IEnumerable<SwarmServiceDescription> allSwarmServices)
        {
            _logger.LogInformation("GetServicesToRedeploy({@hookData}, {@allSwarmServices})", hookData, allSwarmServices);
            return Enumerable.Empty<SwarmServiceDescription>();
        }
    }
}
