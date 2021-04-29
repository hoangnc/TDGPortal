using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace TDG.STS.IdentityServer.Web
{
    [Dependency(ReplaceServices = true)]
    public class IdentityServerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "TDG Portal";
    }
}
