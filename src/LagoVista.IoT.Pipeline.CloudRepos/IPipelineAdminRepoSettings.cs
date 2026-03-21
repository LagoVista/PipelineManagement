using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.CloudRepos
{
    public interface IPipelineAdminRepoSettings
    {
        IConnectionSettings PipelineAdminDocDbStorage { get; }
        IConnectionSettings PipelineAdminTableStorage { get; }
    }
}
