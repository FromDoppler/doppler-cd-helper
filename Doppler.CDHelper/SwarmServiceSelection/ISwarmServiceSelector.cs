using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.CDHelper.SwarmClient;

namespace Doppler.CDHelper.SwarmServiceSelection
{
    public interface ISwarmServiceSelector
    {
        public IEnumerable<SwarmServiceDescription> GetServicesToRedeploy(DockerHubHookData hookData, IEnumerable<SwarmServiceDescription> allSwarmServices);
    }
}
