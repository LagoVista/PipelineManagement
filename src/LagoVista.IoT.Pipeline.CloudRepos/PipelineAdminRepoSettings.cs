using LagoVista.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LagoVista.IoT.Pipeline.CloudRepos
{
    public class PipelineAdminRepoSettings : IPipelineAdminRepoSettings
    {
        public IConnectionSettings PipelineAdminDocDbStorage { get; }
        public IConnectionSettings PipelineAdminTableStorage { get; }

        public PipelineAdminRepoSettings(IConfiguration configuration)
        {
            PipelineAdminDocDbStorage = configuration.CreateDefaultDBStorageSettings();
            PipelineAdminTableStorage = configuration.CreateDefaultTableStorageSettings();
        }
    }
}
