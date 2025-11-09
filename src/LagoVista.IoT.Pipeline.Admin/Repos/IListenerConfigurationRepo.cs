// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 1f1bb20382853926531d4f9eff9da085199bf924930d4cda1e425b4fc7452bf5
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IListenerConfigurationRepo
    {
        Task AddListenerConfigurationAsync(ListenerConfiguration listener);
        Task<ListenerConfiguration> GetListenerConfigurationAsync(string id);
        Task<ListResponse<ListenerConfigurationSummary>> GetListenerConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateListenerConfigurationAsync(ListenerConfiguration listener);
        Task DeleteListenerConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
