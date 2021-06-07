using LagoVista.CloudStorage.DocumentDB;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class SharedDataStreamConnectionRepo : DocumentDBRepoBase<SharedConnection>, ISharedConnectionRepo
    {
        private bool _shouldConsolidateCollections;

        public SharedDataStreamConnectionRepo(IPipelineAdminRepoSettings repoSettings, IAdminLogger logger) :
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, 
                 repoSettings.PipelineAdminDocDbStorage.ResourceName, logger)
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

        public async Task<IEnumerable<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.OwnerOrganization.Id == orgId);

            return from item in items
                   select item.CreateSummary();
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
