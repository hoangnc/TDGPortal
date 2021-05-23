using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDG.STS.IdentityServer.EntityFrameworkCore;
using TDG.STS.IdentityServer.Localization;
using TDG.STS.IdentityServer.MultiTenancy;
using TDG.STS.IdentityServer.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Abp.AspNetCore.Mvc.UI.Theme.AdminLTE;
using DocumentManagement.Web;
using MasterData.Web;
using Volo.Abp.Auditing;
using Volo.Abp.MailKit;
using MailKit.Security;
using MasterData;

namespace TDG.STS.IdentityServer.Web
{
    [DependsOn(
        typeof(AbpMailKitModule),
        typeof(IdentityServerHttpApiModule),
        typeof(IdentityServerApplicationModule),
        typeof(IdentityServerEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(MasterDataHttpApiClientModule),
        typeof(MasterDataWebModule),
        typeof(AbpAspNetCoreMvcUiAdminLTEThemeModule),
        //typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(DocumentManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    public class IdentityServerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(IdentityServerResource),
                    typeof(IdentityServerDomainModule).Assembly,
                    typeof(IdentityServerDomainSharedModule).Assembly,
                    typeof(IdentityServerApplicationModule).Assembly,
                    typeof(IdentityServerApplicationContractsModule).Assembly,
                    typeof(IdentityServerWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigureAuditLog();
            ConfigureEmailSender();
        }

        private void ConfigureEmailSender()
        {
            Configure<AbpMailKitOptions>(options =>
            {
                options.SecureSocketOption = SecureSocketOptions.StartTls;
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "IdentityServer";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityServerWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<IdentityServerDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TDG.STS.IdentityServer.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IdentityServerDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TDG.STS.IdentityServer.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IdentityServerApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TDG.STS.IdentityServer.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IdentityServerApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TDG.STS.IdentityServer.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IdentityServerWebModule>(hostingEnvironment.ContentRootPath);

                    options.FileSets.ReplaceEmbeddedByPhysical<DocumentManagementWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}document-management{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}DocumentManagement.Web"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiAdminLTEThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}themes{Path.DirectorySeparatorChar}Abp.AspNetCore.Mvc.UI.Theme.AdminLTE"));
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt", "famfamfam-flag-vn"));
                options.Languages.Add(new LanguageInfo("en", "en", "English", "famfamfam-flag-england"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文", "famfamfam-flag-zh"));
                /* options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "famfamfam-flag-ar"));
                 options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština", "famfamfam-flag-cs"));

                 options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar", "famfamfam-flag-hu"));
                 options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "famfamfam-flag-fr"));
                 options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português", "famfamfam-flag-pt"));
                 options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "famfamfam-flag-ru"));
                 options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "famfamfam-flag-tr"));
                 options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文", "famfamfam-flag-zh"));
                 options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文", "famfamfam-flag-zh"));*/
            });
        }

        private void ConfigureAuditLog()
        {
            Configure<AbpAuditingOptions>(options => {
                options.EntityHistorySelectors.AddAllEntities();
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new IdentityServerMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(IdentityServerApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TDG Portal API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TDG Portal API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
            
        }
    }
}
