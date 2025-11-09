// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e46349b2fcc2a820bd8aa174ef4d6745344b4c4d2ba527420ce5c27c64e998dd
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.CloudStorage.DocumentDB;
using System.Linq;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class OutputTranslatorConfigurationRepo : DocumentDBRepoBase<OutputTranslatorConfiguration>, IOutputTranslatorConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public OutputTranslatorConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) :
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteOutputTranslatorConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<OutputTranslatorConfigurationSummary>> GetOutputTranslatorConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync< OutputTranslatorConfigurationSummary, OutputTranslatorConfiguration>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration listener)
        {
            return UpsertDocumentAsync(listener);
        }
    }
}
