﻿using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceAdmin.Interfaces.Repos;
using LagoVista.IoT.DeviceAdmin.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class OutputTranslatorConfigurationRepo : DocumentDBRepoBase<OutputTranslatorConfiguration>, IOutputTranslatorConfigurationRepo
    {
        private bool _shouldConsolidateCollections;

        public OutputTranslatorConfigurationRepo(IPipelineAdminRepoSettings settings, IAdminLogger logger) : base(settings.PipelineAdminDocDbStorage.Uri, settings.PipelineAdminDocDbStorage.AccessKey, settings.PipelineAdminDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = settings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections { get { return _shouldConsolidateCollections; } }

        public Task AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration listener)
        {
            return CreateDocumentAsync(listener);
        }

        public Task DeleteOutputTranslatorConfigurationAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetOutputTranslatorConfigurationsForOrgsAsync(string orgId)
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

        public Task UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration listener)
        {
            return UpsertDocumentAsync(listener);
        }
    }
}
