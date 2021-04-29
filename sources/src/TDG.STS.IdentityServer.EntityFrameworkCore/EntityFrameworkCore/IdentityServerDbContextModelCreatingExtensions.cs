using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace TDG.STS.IdentityServer.EntityFrameworkCore
{
    public static class IdentityServerDbContextModelCreatingExtensions
    {
        public static void ConfigureTdgStsIdentityServer(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(IdentityServerConsts.DbTablePrefix + "YourEntities", IdentityServerConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}