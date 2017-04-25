using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface ICustomPipelineConfigurationRepo
    {
        Task AddCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModule);
        Task<CustomPipelineModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetCustomPipelineModuleConfigurationsForOrgsAsync(string orgId);
        Task UpdateCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModule);
        Task DeleteCustomPipelineModuleConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}