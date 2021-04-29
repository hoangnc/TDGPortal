using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TDG.STS.IdentityServer.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class IdentityServerMigrationsDbContextFactory : IDesignTimeDbContextFactory<IdentityServerMigrationsDbContext>
    {
        public IdentityServerMigrationsDbContext CreateDbContext(string[] args)
        {
            IdentityServerEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<IdentityServerMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new IdentityServerMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TDG.STS.IdentityServer.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
