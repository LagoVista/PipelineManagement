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
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetInputTranslatorConfigurationsForOrgsAsync(string orgId);
        Task UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslator);
        Task DeleteInputTranslatorConfigurationAsync(string id);
        Task<bool> QueryKeyInUseAsync(String key, String orgId);
    }
}
