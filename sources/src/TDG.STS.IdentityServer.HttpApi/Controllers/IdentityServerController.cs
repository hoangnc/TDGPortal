using TDG.STS.IdentityServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace TDG.STS.IdentityServer.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class IdentityServerController : AbpController
    {
        protected IdentityServerController()
        {
            LocalizationResource = typeof(IdentityServerResource);
        }
    }
}