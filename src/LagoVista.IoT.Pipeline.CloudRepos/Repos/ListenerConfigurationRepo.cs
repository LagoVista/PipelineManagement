using LagoVista.CloudStorage.DocumentDB;
using System.Linq;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class ListenerConfigurationRepo : DocumentDBRepoBase<ListenerConfiguration>, IListenerConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public ListenerConfigurationRepo(IPipelineAdminRepoSettings settings,  IAdminLogger logger) : base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddListenerConfigurationAsync(ListenerConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteListenerConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<ListenerConfiguration> GetListenerConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<ListenerConfigurationSummary>> GetListenerConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
            return ListResponse<ListenerConfigurationSummary>.Create(items.Model.Select(itm => itm.CreateSummary()), items);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateListenerConfigurationAsync(ListenerConfiguration listener)
        {
            return UpsertDocumentAsync(listener);
        }
    }
}
