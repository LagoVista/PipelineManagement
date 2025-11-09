// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 89d58a9a928e0e0d9125f9e39b76877d3affff51420f745c7d39d57e3934469c
// IndexVersion: 2
// --- END CODE INDEX META ---
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
