using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IApplicationCacheManager
    {
        Task<InvokeResult> AddApplicationCacheAsync(ApplicationCache stream,  EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateApplicationCacheAsync(ApplicationCache stream, EntityHeader org, EntityHeader user);
        Task<InvokeResult<ApplicationCache>> LoadFullApplicationCacheConfigurationAsync(String id, EntityHeader org, EntityHeader user);
        Task<IEnumerable<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId, EntityHeader user);

        Task<ApplicationCache> GetApplicationCacheAsync(string applicationCacheId, EntityHeader org, EntityHeader user);

        Task<DependentObjectCheckResult> CheckApplicationCacheInUseAsync(string applicationCacheId, EntityHeader org, EntityHeader user);

        Task<InvokeResult> DeleteDatStreamAsync(string applicationCacheId, EntityHeader org, EntityHeader user);

        Task<bool> QueryKeyInUseAsync(string key, EntityHeader org);

    }
}
