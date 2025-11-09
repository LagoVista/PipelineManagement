// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 4bff202f8a546324ba2fe0e1aee1143fcf0c8d326beaffcf15fbc43da4926639
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;

namespace LagoVista.IoT.Pipeline.Admin
{
    [DomainDescriptor]
    public class PipelineAdminDomain
    {
        public const string PipelineAdmin = "Pipeline Execution Modules";

        [DomainDescription(PipelineAdmin)]
        public static DomainDescription PipelineModules
        {
            get
            {
                return new DomainDescription()
                {
                    Description = "A set of classes that contains meta data for managing IoT Deployments.",
                    DomainType = DomainDescription.DomainTypes.BusinessObject,
                    Name = "Pipeline Management",
                    CurrentVersion = new Core.Models.VersionInfo()
                    {
                        Major = 0,
                        Minor = 8,
                        Build = 001,
                        DateStamp = new DateTime(2016, 12, 20),
                        Revision = 1,
                        ReleaseNotes = "Initial unstable preview"
                    }
                };
            }
        }

    }
}
