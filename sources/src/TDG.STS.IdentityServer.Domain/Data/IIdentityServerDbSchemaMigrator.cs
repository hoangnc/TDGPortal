using System.Threading.Tasks;

namespace TDG.STS.IdentityServer.Data
{
    public interface IIdentityServerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
