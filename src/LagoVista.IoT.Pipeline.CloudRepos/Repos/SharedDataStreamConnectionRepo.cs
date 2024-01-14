﻿using LagoVista.CloudStorage.DocumentDB;
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

        public SharedDataStreamConnectionRepo(IPipelineAdminRepoSettings repoSettings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) :
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, 
                 repoSettings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
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
