﻿using LagoVista.CloudStorage.DocumentDB;
using System.Linq;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class PlannerConfigurationRepo : DocumentDBRepoBase<PlannerConfiguration>, IPlannerConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public PlannerConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger, ICacheProvider cacheProvider, IDependencyManager dependencyManager) : 
            base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger, cacheProvider, dependencyManager)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddPlannerConfigurationAsync(PlannerConfiguration sentinalConfiguration)
        {
            return CreateDocumentAsync(sentinalConfiguration);
        }

        public Task DeletePlannerConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<PlannerConfiguration> GetPlannerConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<ListResponse<PlannerConfigurationSummary>> GetPlannerConfigurationsForOrgsAsync(string orgId, ListRequest listRequest)
        {
            return await base.QuerySummaryAsync< PlannerConfigurationSummary, PlannerConfiguration>(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId, qry => qry.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdatePlannerConfigurationAsync(PlannerConfiguration sentinelConfiguration)
        {
            return UpsertDocumentAsync(sentinelConfiguration);
        }
    }
}
