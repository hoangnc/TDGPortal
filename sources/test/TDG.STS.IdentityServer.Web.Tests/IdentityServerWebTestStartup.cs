using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace TDG.STS.IdentityServer
{
    public class IdentityServerWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<IdentityServerWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}