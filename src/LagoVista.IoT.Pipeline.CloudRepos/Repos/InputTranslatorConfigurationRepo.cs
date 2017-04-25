using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class InputTranslatorConfigurationRepo : DocumentDBRepoBase<InputTranslatorConfiguration>, IInputTranslatorConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public InputTranslatorConfigurationRepo(IPipelineAdminRepoSettings settings, ILogger logger) : base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger)
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

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetInputTranslatorConfigurationsForOrgsAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId);

            return from item in items
                   select item.CreateSummary();
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
