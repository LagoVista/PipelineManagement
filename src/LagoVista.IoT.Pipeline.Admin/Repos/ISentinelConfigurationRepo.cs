// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 9bd3d0905e4675ae288696390c4b7d0ed9a921b3119a3c49d5c08c0064c402ac
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
    public interface ISentinelConfigurationRepo
    {
        Task AddSentinelConfigurationAsync(SentinelConfiguration sentinel);
        Task<SentinelConfiguration> GetSentinelConfigurationAsync(string id);
        Task<ListResponse<SentinelConfigurationSummary>> GetSentinelConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateSentinelConfigurationAsync(SentinelConfiguration sentinel);
        Task DeleteSentinelConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
