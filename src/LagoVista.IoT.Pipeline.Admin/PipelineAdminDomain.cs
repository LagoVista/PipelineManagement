using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;

namespace LagoVista.IoT.Pipeline.Admin
{
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
                    Name = "Deployment Admin",
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
