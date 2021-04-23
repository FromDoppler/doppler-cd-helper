using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Doppler.CDHelper.DockerHubIntegration
{
    public class DockerHubSecretValidator : IDockerHubSecretValidator
    {
        private readonly DockerHubHookSettings _settings;

        public DockerHubSecretValidator(IOptions<DockerHubHookSettings> options)
            => _settings = options.Value;

        public bool IsTheRightSecret(string secret)
            => _settings.Secret == secret;
    }
}
