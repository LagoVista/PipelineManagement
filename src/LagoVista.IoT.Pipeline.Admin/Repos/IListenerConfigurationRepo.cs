using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IListenerConfigurationRepo
    {
        Task AddListenerConfigurationAsync(ListenerConfiguration listener);
        Task<ListenerConfiguration> GetListenerConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetListenerConfigurationsForOrgsAsync(string orgId);
        Task UpdateListenerConfigurationAsync(ListenerConfiguration listener);
        Task DeleteListenerConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
