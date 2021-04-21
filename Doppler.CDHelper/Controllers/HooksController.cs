using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;
using Doppler.CDHelper.SwarmServiceSelection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.Controllers
{
    [ApiController]
    public class HooksController
    {
        private readonly ILogger<HooksController> _logger;
        private readonly ISwarmClient _swarmClient;
        private readonly ISwarmServiceSelector _swarmServiceSelector;

        public HooksController(
            ILogger<HooksController> logger,
            ISwarmClient swarmClient,
            ISwarmServiceSelector swarmServiceSelector)
        {
            _logger = logger;
            _swarmClient = swarmClient;
            _swarmServiceSelector = swarmServiceSelector;
        }

        [HttpPost("/hooks/{secret}")]
        public async Task Post([FromRoute] string secret, [FromBody] DockerHubHookData data)
        {
            _logger.LogInformation("Hook event! secret: {secret}; data: {@data}", secret, data);

            var currentServices = await _swarmClient.GetServices();
            var servicesToRedeploy = _swarmServiceSelector.GetServicesToRedeploy(data, currentServices);
            // TODO: redeploy the identified services
        }
    }
}
