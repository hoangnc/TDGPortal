using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace TDG.STS.IdentityServer.EntityFrameworkCore
{
    [DependsOn(
        typeof(IdentityServerEntityFrameworkCoreModule)
        )]
    public class IdentityServerEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityServerMigrationsDbContext>();
        }
    }
}
