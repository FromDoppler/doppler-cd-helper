using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doppler.CDHelper.SwarmClient
{
    public record SwarmpitSwarmClientSettings
    {
        public string AccessToken { get; init; }
        public string BaseUrl { get; init; }
    }
}
