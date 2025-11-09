// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 4c761dd1134a9538d70cc61f67465731f561fb3530168271d11e9e23b7f47f29
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models.UIMetaData;
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
        Task<ListResponse<PlannerConfigurationSummary>> GetPlannerConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdatePlannerConfigurationAsync(PlannerConfiguration sentinel);
        Task DeletePlannerConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
