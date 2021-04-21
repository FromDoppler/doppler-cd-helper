using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Doppler.CDHelper.Controllers;
using Doppler.CDHelper.SwarmClient;
using Doppler.CDHelper.SwarmServiceSelection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Doppler.CDHelper
{
    public class DockerHubHandlerTest
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public DockerHubHandlerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task POST_dockerhub_handler_should_parse_and_log_callback_url()
        {
            // Arrange
            var secret = "my_secret";
            var callbackUrl = "https://registry.hub.docker.com/u/dopplerdock/doppler-cd-helper/hook/2jiedged52hbhfi1acgdggdi1ih123456/";

            var loggerMock = new Mock<ILogger<HooksController>>();

            using var customFactory = _factory.WithWebHostBuilder(c =>
            {
                c.ConfigureServices(s => s.AddSingleton(loggerMock.Object));
            });

            var client = customFactory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.PostAsync(
                $"/hooks/{secret}/",
                JsonContent.Create(new { callback_url = callbackUrl }));

            // Assert

            // It is a FormattedLogValues object
            // See: https://github.com/dotnet/runtime/blob/01b7e73cd378145264a7cb7a09365b41ed42b240/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/FormattedLogValues.cs#L16
            IReadOnlyList<KeyValuePair<string, object>> logParameters = null;

            loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().StartsWith("Hook event!") && AssertHelper.IsTypeAndGetValue(v, out logParameters)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            Assert.Contains($"secret: {secret};", logParameters.ToString());
            Assert.Contains(
                logParameters,
                x => x.Key == "@data" && x.Value is DockerHubHookData data && data.callback_url == callbackUrl);
        }

        [Fact]
        public async Task POST_dockerhub_handler_should_get_services_from_SwarmClient_and_filter_with_service_selector()
        {
            // Arrange
            var callbackUrl = "https://registry.hub.docker.com/u/dopplerdock/doppler-cd-helper/hook/2jiedged52hbhfi1acgdggdi1ih123456/";

            var currentServices = new[] { new SwarmServiceDescription() };

            var swarmClientMock = new Mock<ISwarmClient>();
            var swarmServiceSelectorMock = new Mock<ISwarmServiceSelector>();

            swarmClientMock.Setup(x => x.GetServices()).ReturnsAsync(currentServices);

            using var customFactory = _factory.WithWebHostBuilder(c =>
            {
                c.ConfigureServices(s => s
                    .AddSingleton(swarmClientMock.Object)
                    .AddSingleton(swarmServiceSelectorMock.Object));
            });

            var client = customFactory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.PostAsync(
                "/hooks/my_secret/",
                JsonContent.Create(new { callback_url = callbackUrl }));

            // Assert
            swarmClientMock.Verify(x => x.GetServices(), Times.Once);
            swarmServiceSelectorMock.Verify(
                x => x.GetServicesToRedeploy(
                    It.Is<DockerHubHookData>(v => v.callback_url == callbackUrl),
                    currentServices),
                Times.Once);
        }
    }
}
