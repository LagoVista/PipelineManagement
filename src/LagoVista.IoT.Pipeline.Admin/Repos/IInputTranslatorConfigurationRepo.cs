// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d172f3396e4d709c3d0303471a83d633edba0029c65fe1fadf58f3981e301a57
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
    public interface IInputTranslatorConfigurationRepo
    {
        Task AddInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslator);
        Task<InputTranslatorConfiguration> GetInputTranslatorConfigurationAsync(string id);
        Task<ListResponse<InputTranslatorConfigurationSummary>> GetInputTranslatorConfigurationsForOrgsAsync(string orgId, ListRequest listRequest);
        Task UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslator);
        Task DeleteInputTranslatorConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
