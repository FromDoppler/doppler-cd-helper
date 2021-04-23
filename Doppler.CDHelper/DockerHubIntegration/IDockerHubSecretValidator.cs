using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.CDHelper.DockerHubIntegration
{
    public interface IDockerHubSecretValidator
    {
        bool IsTheRightSecret(string secret);
    }
}
