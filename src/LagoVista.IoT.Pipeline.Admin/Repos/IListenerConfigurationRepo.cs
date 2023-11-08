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
