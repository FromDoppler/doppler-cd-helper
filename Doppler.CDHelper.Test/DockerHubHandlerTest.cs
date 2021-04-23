using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Doppler.CDHelper.DockerHubIntegration;
using Doppler.CDHelper.SwarmClient;
using Doppler.CDHelper.SwarmServiceSelection;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            var callback_url = "https://registry.hub.docker.com/u/dopplerdock/doppler-cd-helper/hook/2jiedged52hbhfi1acgdggdi1ih123456/";
            var tag = "tag";
            var repo_name = "repoName";

            var loggerMock = new Mock<ILogger<HooksController>>();

            using var customFactory = _factory.WithWebHostBuilder(c =>
            {
                c.ConfigureServices(s => s
                    .AddSingleton(loggerMock.Object)
                    .AddSingleton(Mock.Of<ISwarmClient>()));
            });

            var client = customFactory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.PostAsync(
                $"/hooks/{secret}/",
                JsonContent.Create(new
                {
                    callback_url,
                    push_data = new { tag },
                    repository = new { repo_name }
                }));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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
            Assert.Contains(logParameters, x =>
                x.Key == "@data"
                && x.Value is DockerHubHookData data
                && data.callback_url == callback_url
                && data.push_data.tag == tag
                && data.repository.repo_name == repo_name);
        }

        [Fact]
        public async Task POST_dockerhub_handler_should_get_services_from_SwarmClient_filter_with_service_selector_and_redeploy_selected_ones()
        {
            // Arrange
            var callback_url = "https://registry.hub.docker.com/u/dopplerdock/doppler-cd-helper/hook/2jiedged52hbhfi1acgdggdi1ih123456/";
            var tag = "tag";
            var repo_name = "repoName";

            var currentServices = new[] { new SwarmServiceDescription() };

            var selectedServiceId1 = "selectedServiceId1";
            var selectedServiceId2 = "selectedServiceId2";
            var selectedServices = new[]
            {
                new SwarmServiceDescription() { id = selectedServiceId1 },
                new SwarmServiceDescription() { id = selectedServiceId2 }
            };

            var swarmClientMock = new Mock<ISwarmClient>();
            var swarmServiceSelectorMock = new Mock<ISwarmServiceSelector>();

            swarmClientMock.Setup(x => x.GetServices()).ReturnsAsync(currentServices);

            swarmServiceSelectorMock
                .Setup(x => x.GetServicesToRedeploy(
                    It.Is<DockerHubHookData>(v =>
                        v.callback_url == callback_url
                        && v.push_data.tag == tag
                        && v.repository.repo_name == repo_name),
                    currentServices))
                .Returns(selectedServices);

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
                JsonContent.Create(new
                {
                    callback_url,
                    push_data = new { tag },
                    repository = new { repo_name }
                }));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            swarmClientMock.Verify(x => x.GetServices(), Times.Once);
            swarmClientMock.Verify(x => x.RedeployService(It.IsAny<string>()), Times.Exactly(selectedServices.Count()));
            swarmClientMock.Verify(x => x.RedeployService(selectedServiceId1), Times.Once);
            swarmClientMock.Verify(x => x.RedeployService(selectedServiceId2), Times.Once);
        }

        [Fact]
        public async Task POST_dockerhub_handler_should_get_services_from_swarmpit_filter_them_and_update_filtered_ones()
        {
            // Arrange
            const string baseUrl = "http://swarmpit/api/";
            const string accessToken = "abc123";
            const string helloRepositoryName = "dopplerdock/hello-microservice";
            const string cdHelperRepositoryName = "dopplerdock/doppler-cd-helper";
            const string intTag = "INT";
            const string qaTag = "QA";

            var hookData = new DockerHubHookData()
            {
                callback_url = "https://callback_url",
                push_data = new DockerHubHookDataPushData()
                {
                    tag = intTag
                },
                repository = new DockerHubHookDataRepository()
                {
                    repo_name = helloRepositoryName
                }
            };

            // should be redeployed
            var helloServiceInt = new SwarmServiceDescription()
            {
                id = "penpnhep0bsciofxzd542v62n",
                serviceName = "hello-int_hello-service",
                repository = new SwarmServiceDescriptionRepository()
                {
                    name = helloRepositoryName,
                    tag = intTag,
                    imageDigest = "sha256:c7a459f13dbf082fe9b6631783f897d54978a32cc91aa8dee5fcb5630fa18a0b"
                }
            };

            // should not be redeployed
            var cdHelperServiceInt = new SwarmServiceDescription()
            {
                id = "qn3jq891p77lyigwysj4fcww2",
                serviceName = "swarmpit_doppler-cd-helper",
                repository = new SwarmServiceDescriptionRepository()
                {
                    name = cdHelperRepositoryName,
                    tag = intTag,
                    imageDigest = "sha256:a247c00ad3ae505bbcbcbc48eb58a50a16e0628339803da65c9081be365b3b9c"
                }
            };

            // Exactly the same image with another configuration
            // should be redeployed
            var helloServiceIntWithAlternativeConfiguration = helloServiceInt with
            {
                id = "riq6gv8d0xr9rpagr9lg5xqrw",
                serviceName = "hello-int_hello-service_with_alternative_configuration",
            };

            // The same image name with a different tag with another configuration
            // should not be redeployed
            var helloServiceQA = helloServiceInt with
            {
                id = "riq6gv8d0xr9rpagr9lg5xqrw",
                serviceName = "hello-int_hello-service_with_alternative_configuration",
                repository = helloServiceInt.repository with
                {
                    tag = qaTag
                }
            };

            using var customFactory = _factory.WithWebHostBuilder(c =>
            {
                c.ConfigureServices(s => s.AddSingleton(Options.Create(new SwarmpitSwarmClientSettings()
                {
                    BaseUrl = baseUrl,
                    AccessToken = accessToken
                })));
            });

            customFactory.Server.PreserveExecutionContext = true;

            using var httpTest = new HttpTest();

            httpTest.ForCallsTo($"{baseUrl}services")
                .WithHeader("Authorization", $"Bearer {accessToken}")
                .WithVerb("GET")
                .RespondWithJson(new[] {
                    helloServiceInt,
                    cdHelperServiceInt,
                    helloServiceIntWithAlternativeConfiguration,
                    helloServiceQA
                });

            var client = customFactory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.PostAsync(
                "/hooks/my_secret/",
                JsonContent.Create(hookData));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, httpTest.CallLog.Count());
            httpTest.ShouldHaveCalled($"{baseUrl}services/{helloServiceInt.id}/redeploy")
                .WithOAuthBearerToken(accessToken)
                .WithVerb("POST");
            httpTest.ShouldHaveCalled($"{baseUrl}services/{helloServiceIntWithAlternativeConfiguration.id}/redeploy")
                .WithOAuthBearerToken(accessToken)
                .WithVerb("POST");
        }
    }
}
