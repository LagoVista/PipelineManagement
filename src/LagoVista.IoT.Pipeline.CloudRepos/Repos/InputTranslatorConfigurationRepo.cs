// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e1f734b7cc5b1bbb9adefa441bb8f3bdb03365bca1de55414473293921f8114b
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
    public class InputTranslatorConfigurationRepo : DocumentDBRepoBase<InputTranslatorConfiguration>, IInputTranslatorConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public InputTranslatorConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) : 
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }
        public Task AddInputTranslatorConfigurationAsync(InputTranslatorConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteInputTranslatorConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<InputTranslatorConfiguration> GetInputTranslatorConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<InputTranslatorConfigurationSummary>> GetInputTranslatorConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync< InputTranslatorConfigurationSummary, InputTranslatorConfiguration>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry=>qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration listener)
        {
            return UpsertDocumentAsync(listener);
        }
    }
}
