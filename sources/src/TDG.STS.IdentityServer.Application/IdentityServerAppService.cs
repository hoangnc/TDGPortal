using System;
using System.Collections.Generic;
using System.Text;
using TDG.STS.IdentityServer.Localization;
using Volo.Abp.Application.Services;

namespace TDG.STS.IdentityServer
{
    /* Inherit your application services from this class.
     */
    public abstract class IdentityServerAppService : ApplicationService
    {
        protected IdentityServerAppService()
        {
            LocalizationResource = typeof(IdentityServerResource);
        }
    }
}
