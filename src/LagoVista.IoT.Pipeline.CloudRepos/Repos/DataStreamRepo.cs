// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: de0d6c500fd0f02815676a48b2fbb40878ef18ca474a6b7824430cf948ca54c9
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.CloudStorage.DocumentDB;
using LagoVista.IoT.Logging.Loggers;
using System.Linq;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class DataStreamRepo : DocumentDBRepoBase<DataStream>, IDataStreamRepo
    {
        private bool _shouldConsolidateCollections;

        public DataStreamRepo(IPipelineAdminRepoSettings repoSettings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) : 
            base(repoSettings.PipelineAdminDocDbStorage.Uri, repoSettings.PipelineAdminDocDbStorage.AccessKey, repoSettings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;


        public Task AddDataStreamAsync(DataStream dataStream)
        {
            return CreateDocumentAsync(dataStream);
        }

        public Task DeleteDataStreamAsync(string dataStreamId)
        {
            return DeleteDocumentAsync(dataStreamId);
        }

        public Task<DataStream> GetDataStreamAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync<DataStreamSummary, DataStream>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateDataStreamAsync(DataStream dataStream)
        {
            return UpsertDocumentAsync(dataStream);
        }
    }
}
