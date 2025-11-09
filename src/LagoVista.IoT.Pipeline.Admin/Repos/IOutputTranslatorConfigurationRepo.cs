// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: c8a97b060d4af951288004acca67e31034b3dd86a601f7c2029b00d4a616fbca
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IOutputTranslatorConfigurationRepo
    {
        Task AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslator);
        Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(string id);
        Task<ListResponse<OutputTranslatorConfigurationSummary>> GetOutputTranslatorConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslator);
        Task DeleteOutputTranslatorConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
