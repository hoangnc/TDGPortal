using TDG.STS.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace TDG.STS.IdentityServer
{
    [DependsOn(
        typeof(IdentityServerEntityFrameworkCoreTestModule)
        )]
    public class IdentityServerDomainTestModule : AbpModule
    {

    }
}