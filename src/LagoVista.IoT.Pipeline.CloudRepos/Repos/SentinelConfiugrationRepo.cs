// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: f4474d53b7650df4baac4c25a38bbec23412d0bddb3d2510b7be17ed779f2a77
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.CloudStorage.DocumentDB;
using System.Linq;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class SentinelConfigurationRepo : DocumentDBRepoBase<SentinelConfiguration>, ISentinelConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public SentinelConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) :
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddSentinelConfigurationAsync(SentinelConfiguration sentinalConfiguration)
        {
            return CreateDocumentAsync(sentinalConfiguration);
        }

        public Task DeleteSentinelConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<SentinelConfiguration> GetSentinelConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<SentinelConfigurationSummary>> GetSentinelConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync<SentinelConfigurationSummary, SentinelConfiguration>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateSentinelConfigurationAsync(SentinelConfiguration sentinelConfiguration)
        {
            return UpsertDocumentAsync(sentinelConfiguration);
        }
    }
}
