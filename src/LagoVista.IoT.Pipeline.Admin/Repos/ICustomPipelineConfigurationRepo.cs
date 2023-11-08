using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface ICustomPipelineConfigurationRepo
    {
        Task AddCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModule);
        Task<CustomModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id);
        Task<ListResponse<CustomModuleConfigurationSummary>> GetCustomPipelineModuleConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModule);
        Task DeleteCustomPipelineModuleConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}