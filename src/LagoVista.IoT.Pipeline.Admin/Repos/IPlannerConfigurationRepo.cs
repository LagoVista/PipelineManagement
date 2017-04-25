using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IPlannerConfigurationRepo
    {
        Task AddPlannerConfigurationAsync(PlannerConfiguration sentinel);
        Task<PlannerConfiguration> GetPlannerConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetPlannerConfigurationsForOrgsAsync(string orgId);
        Task UpdatePlannerConfigurationAsync(PlannerConfiguration sentinel);
        Task DeletePlannerConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
