// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: f352e1aae4065ea473eb51e1747dc556656c6aedc289484c32a8628e11b59d71
// IndexVersion: 2
// --- END CODE INDEX META ---
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
