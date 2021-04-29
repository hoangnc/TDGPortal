using TDG.STS.IdentityServer.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TDG.STS.IdentityServer.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class IdentityServerPageModel : AbpPageModel
    {
        protected IdentityServerPageModel()
        {
            LocalizationResourceType = typeof(IdentityServerResource);
        }
    }
}