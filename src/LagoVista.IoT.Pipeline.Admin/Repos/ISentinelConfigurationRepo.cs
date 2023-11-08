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
