// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 6c7b52f34b1fd42c3a91832047cbd595ed9b34fb4e6f10f9d28609258f3e8882
// IndexVersion: 2
// --- END CODE INDEX META ---
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