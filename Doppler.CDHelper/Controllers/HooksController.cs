using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Doppler.CDHelper.Controllers
{
    [ApiController]
    public class HooksController
    {
        private readonly ILogger<HooksController> _logger;

        public HooksController(ILogger<HooksController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/hooks/{secret}")]
        public async Task Post([FromRoute] string secret, [FromBody] DockerHubHookData data)
        {
            _logger.LogInformation("Hook event! secret: {secret}; data: {@data}", secret, data);
            await Task.CompletedTask;
        }
    }
}
