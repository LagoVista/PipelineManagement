using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface ITransmitterConfigurationRepo
    {
        Task AddTransmitterConfigurationAsync(TransmitterConfiguration deployment);
        Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetTransmitterConfigurationsForOrgsAsync(string orgId);
        Task UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitter);
        Task DeleteTransmitterConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
