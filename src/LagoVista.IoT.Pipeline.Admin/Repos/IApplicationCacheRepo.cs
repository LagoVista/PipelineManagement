using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IApplicationCacheRepo
    {
        Task AddApplicationCacheAsync(ApplicationCache cache);

        Task UpdateApplicationCacheAsync(ApplicationCache cache);

        Task<ApplicationCache> GetApplicationCacheAsync(string id);

        Task<ListResponse<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId, ListRequest listRequest);

        Task DeleteApplicationCacheAsync(string cacheId);

        Task<bool> QueryKeyInUseAsync(string key, string orgId);
    }
}
