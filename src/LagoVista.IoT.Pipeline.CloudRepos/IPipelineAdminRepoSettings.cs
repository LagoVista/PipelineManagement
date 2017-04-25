using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.CloudRepos
{
    public interface IPipelineAdminRepoSettings
    {
        IConnectionSettings PipelineAdminDocDbStorage { get; set; }
        IConnectionSettings PipelineAdminTableStorage { get; set; }

        bool ShouldConsolidateCollections { get; }
    }
}
