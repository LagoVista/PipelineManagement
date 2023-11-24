using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class CustomPipelineModuleConfigurationRepo : DocumentDBRepoBase<CustomModuleConfiguration>, 
        ICustomPipelineConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public CustomPipelineModuleConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) : 
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration)
        {
            return CreateDocumentAsync(pipelineModuleConfiguration);
        }

        public Task DeleteCustomPipelineModuleConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<CustomModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<CustomModuleConfigurationSummary>> GetCustomPipelineModuleConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
            return ListResponse<CustomModuleConfigurationSummary>.Create(items.Model.Select(itm => itm.CreateSummary()), items);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration)
        {
            return UpsertDocumentAsync(pipelineModuleConfiguration);
        }
    }
}