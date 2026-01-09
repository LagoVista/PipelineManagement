// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 10f30b349bb3b7005bdcd29184759133d8ced98b2a53d9a539777e6f1962f6dd
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.CloudStorage.DocumentDB;
using LagoVista.CloudStorage.Interfaces;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class ApplicationCacheRepo : DocumentDBRepoBase<ApplicationCache>, IApplicationCacheRepo
    {
        private bool _shouldConsolidateCollections;

        public ApplicationCacheRepo(IPipelineAdminRepoSettings repoSettings, IDocumentCloudCachedServices services) :
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, repoSettings.PipelineAdminDocDbStorage.ResourceName, services)
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

        public async Task<ListResponse<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync<ApplicationCacheSummary, ApplicationCache>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
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
