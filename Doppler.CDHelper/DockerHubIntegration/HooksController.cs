using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;
using Doppler.CDHelper.SwarmServiceSelection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.DockerHubIntegration
{
    [ApiController]
    public class HooksController
    {
        private readonly ILogger<HooksController> _logger;
        private readonly IDockerHubSecretValidator _dockerHubSecretValidator;
        private readonly ISwarmClient _swarmClient;
        private readonly ISwarmServiceSelector _swarmServiceSelector;

        public HooksController(
            ILogger<HooksController> logger,
            IDockerHubSecretValidator dockerHubSecretValidator,
            ISwarmClient swarmClient,
            ISwarmServiceSelector swarmServiceSelector)
        {
            _logger = logger;
            _dockerHubSecretValidator = dockerHubSecretValidator;
            _swarmClient = swarmClient;
            _swarmServiceSelector = swarmServiceSelector;
        }

        [HttpPost("/hooks/{secret}")]
        public async Task<IActionResult> Post([FromRoute] string secret, [FromBody] DockerHubHookData data)
        {
            if (!_dockerHubSecretValidator.IsTheRightSecret(secret))
            {
                _logger.LogWarning("Hook event with a wrong secret! secret: {secret}; data: {@data}", secret, data);
                return new UnauthorizedResult();
            }

            _logger.LogDebug("Hook event. Data: {@data}", data);

            var currentServices = await _swarmClient.GetServices();
            var servicesToRedeploy = _swarmServiceSelector.GetServicesToRedeploy(data, currentServices);

            _logger.LogDebug("Services to redeploy: {@servicesToRedeploy}", servicesToRedeploy);

            foreach (var service in servicesToRedeploy)
            {
                // TODO: Consider compare local service digest with docker hub one.
                // Since hook data does not contain digest information, we cannot confirm if
                // the service is not already updated.
                _logger.LogDebug("Redeploying {@service}", service);
                await _swarmClient.RedeployService(service.id);
                _logger.LogDebug("Done {@service}", service);
            }

            return new OkResult();
        }
    }
}
