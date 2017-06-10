using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class CustomPipelineModuleConfigurationRepo : DocumentDBRepoBase<CustomPipelineModuleConfiguration>, ICustomPipelineConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public CustomPipelineModuleConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger) : 
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration)
        {
            return CreateDocumentAsync(pipelineModuleConfiguration);
        }

        public Task DeleteCustomPipelineModuleConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<CustomPipelineModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetCustomPipelineModuleConfigurationsForOrgsAsync(string orgId)
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

        public Task UpdateCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration)
        {
            return UpsertDocumentAsync(pipelineModuleConfiguration);
        }
    }
}