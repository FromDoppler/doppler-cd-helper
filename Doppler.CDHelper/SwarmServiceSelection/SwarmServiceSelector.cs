using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.SwarmServiceSelection
{
    public class SwarmServiceSelector : ISwarmServiceSelector
    {
        private readonly ILogger<SwarmServiceSelector> _logger;

        public SwarmServiceSelector(ILogger<SwarmServiceSelector> logger)
        {
            _logger = logger;
        }

        public IEnumerable<SwarmServiceDescription> GetServicesToRedeploy(
            DockerHubHookData hookData,
            IEnumerable<SwarmServiceDescription> allSwarmServices)
            => allSwarmServices.Where(x =>
                x.repository.name == hookData.repository.repo_name
                && x.repository.tag == hookData.push_data.tag);
    }
}
