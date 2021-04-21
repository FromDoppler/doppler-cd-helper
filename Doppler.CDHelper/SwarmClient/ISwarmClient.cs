using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.CDHelper.SwarmClient
{
    public interface ISwarmClient
    {
        Task<IEnumerable<SwarmServiceDescription>> GetServices();
        Task RedeployService(string serviceId);
    }
}
