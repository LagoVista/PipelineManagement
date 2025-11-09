// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b6c4be266d23f0093b8c64218f9292e7d926ba72001dd415a52aee0517e6bc44
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
    public class TransmitterConfigurationRepo : DocumentDBRepoBase<TransmitterConfiguration>, ITransmitterConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public TransmitterConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) : 
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddTransmitterConfigurationAsync(TransmitterConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteTransmitterConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<TransmitterConfigurationSummary>> GetTransmitterConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync<TransmitterConfigurationSummary, TransmitterConfiguration>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration)
        {
            return UpsertDocumentAsync(transmitterConfiguration);
        }
    }
}
