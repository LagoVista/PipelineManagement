using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
        [DomainDescriptor]
        public class PipelineModuleDomain
    {
            //[DomainDescription(Name: "Device Admin", Description: "Models for working with and creating device configurations.  This includes things such as actions, attributes and state machines.")]
            public const string DeviceAdmin = "DeviceAdmin";

            //  [DomainDescription(Name: "State Machines", Description: "State Machines are used for a number of system level components.")]
            public const string StateMachines = "StateMachine";

            [DomainDescription(StateMachines)]
            public static DomainDescription StateMachineDomainDescription
            {
                get
                {
                    return new DomainDescription()
                    {
                        Description = "A general purpose set of classes to implement data driven inter communicating state machines.",
                        DomainType = DomainDescription.DomainTypes.BusinessObject,
                        Name = "State Machine",
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

            [DomainDescription(DeviceAdmin)]
            public static DomainDescription DeviceAdminDomainDescription
            {
                get
                {
                    return new DomainDescription()
                    {
                        Description = "A set of classes that contains meta data for managing IoT devices.",
                        DomainType = DomainDescription.DomainTypes.BusinessObject,
                        Name = "Device Administration",
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
