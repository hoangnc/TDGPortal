using TDG.STS.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace TDG.STS.IdentityServer.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(IdentityServerEntityFrameworkCoreDbMigrationsModule),
        typeof(IdentityServerApplicationContractsModule)
        )]
    public class IdentityServerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
