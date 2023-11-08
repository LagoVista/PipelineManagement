using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface ITransmitterConfigurationRepo
    {
        Task AddTransmitterConfigurationAsync(TransmitterConfiguration deployment);
        Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id);
        Task<ListResponse<TransmitterConfigurationSummary>> GetTransmitterConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitter);
        Task DeleteTransmitterConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
