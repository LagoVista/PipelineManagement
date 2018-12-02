using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IApplicationCacheRepo
    {
        Task AddApplicationCacheAsync(ApplicationCache cache);

        Task UpdateApplicationCacheAsync(ApplicationCache cache);

        Task<ApplicationCache> GetApplicationCacheAsync(string id);

        Task<IEnumerable<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId);

        Task DeleteApplicationCacheAsync(string cacheId);

        Task<bool> QueryKeyInUseAsync(string key, string orgId);
    }
}
