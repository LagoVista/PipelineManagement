// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ed363d5373e891b1ee81501ba062f34835402aae3a7df3f9c5c1c95056dc6467
// IndexVersion: 2
// --- END CODE INDEX META ---
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
