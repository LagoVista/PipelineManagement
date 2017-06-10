using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.CloudRepos;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class TransmitterConfigurationRepo : DocumentDBRepoBase<TransmitterConfiguration>, ITransmitterConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public TransmitterConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger) : base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddTransmitterConfigurationAsync(TransmitterConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteTransmitterConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetTransmitterConfigurationsForOrgsAsync(string orgId)
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

        public Task UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration)
        {
            return UpsertDocumentAsync(transmitterConfiguration);
        }
    }
}
