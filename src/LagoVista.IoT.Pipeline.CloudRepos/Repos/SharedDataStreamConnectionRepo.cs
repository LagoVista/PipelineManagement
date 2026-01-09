// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3de70032b6cfe84c2d94bc6c58ad05d5ae867bdef9de258d4c7b58206f802fb0
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
    public class SharedDataStreamConnectionRepo : DocumentDBRepoBase<SharedConnection>, ISharedConnectionRepo
    {
        private bool _shouldConsolidateCollections;

        public SharedDataStreamConnectionRepo(IPipelineAdminRepoSettings repoSettings, IDocumentCloudCachedServices services) :
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, 
                 repoSettings.PipelineAdminDocDbStorage.ResourceName, services)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;
        
        public Task AddSharedConnectionAsync(SharedConnection connection)
        {
            return CreateDocumentAsync(connection);
        }

        public Task DeleteSharedConnectionAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<SharedConnection> GetSharedConnectionAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync<SharedConnectionSummary, SharedConnection>(qry=> qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateShareConnectionAsync(SharedConnection connection)
        {
            return UpsertDocumentAsync(connection);
        }
    }
}
