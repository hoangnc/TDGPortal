using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace TDG.STS.IdentityServer.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<IdentityServerWebModule>( options =>
            {
                //options.PlugInSources.AddFolder(@"D:\Projects\TDG.STS.IdentityServer\sources\modules\DocumentManagement\src\DocumentManagement.Web\bin\Debug\net5.0");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
