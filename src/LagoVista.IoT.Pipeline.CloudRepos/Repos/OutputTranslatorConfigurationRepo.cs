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
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
            return ListResponse<OutputTranslatorConfigurationSummary>.Create(items.Model.Select(itm => itm.CreateSummary()), items);
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
