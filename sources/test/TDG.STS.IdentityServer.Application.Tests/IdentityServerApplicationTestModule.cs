using Volo.Abp.Modularity;

namespace TDG.STS.IdentityServer
{
    [DependsOn(
        typeof(IdentityServerApplicationModule),
        typeof(IdentityServerDomainTestModule)
        )]
    public class IdentityServerApplicationTestModule : AbpModule
    {

    }
}