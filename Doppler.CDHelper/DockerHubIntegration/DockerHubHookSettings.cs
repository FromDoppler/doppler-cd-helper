using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doppler.CDHelper.DockerHubIntegration
{
    public record DockerHubHookSettings
    {
        public string Secret { get; init; }
    }
}
