using LagoVista.CloudStorage.DocumentDB;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class ApplicationCacheRepo : DocumentDBRepoBase<ApplicationCache>, IApplicationCacheRepo
    {
        private bool _shouldConsolidateCollections;

        public ApplicationCacheRepo(IPipelineAdminRepoSettings repoSettings, IAdminLogger logger) :
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, repoSettings.PipelineAdminDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;

        public Task AddApplicationCacheAsync(ApplicationCache cache)
        {
            return CreateDocumentAsync(cache);
        }

        public Task DeleteApplicationCacheAsync(string cacheId)
        {
            return DeleteDocumentAsync(cacheId);
        }

        public Task<ApplicationCache> GetApplicationCacheAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.OwnerOrganization.Id == orgId);

            return from item in items
                   select item.CreateSummary();
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateApplicationCacheAsync(ApplicationCache cache)
        {
            return UpsertDocumentAsync(cache);
        }
    }
}
