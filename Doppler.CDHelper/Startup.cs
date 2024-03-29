using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Doppler.CDHelper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpLogging(c =>
            {
                c.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            });
            // Explicitly using Hellang because services.AddProblemDetails() generates an ambiguity
            // between Microsoft.AspNetCore.Http.Extensions.ProblemDetailsServiceCollectionExtensions
            // and Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions
            // TODO: consider replace Hellang by out of the box alternative (but it is not working fine right now)
            Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(services);
            services.AddDockerHubIntegration(Configuration);
            services.AddSwarmpitSwarmClient(Configuration);
            services.AddSwarmServiceSelector();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Doppler.CDHelper", Version = "v1" });

                var baseUrl = Configuration.GetValue<string>("BaseURL");
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    c.AddServer(new OpenApiServer() { Url = baseUrl });
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpLogging();

            Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.UseProblemDetails(app);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Doppler.CDHelper v1"));

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
