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
