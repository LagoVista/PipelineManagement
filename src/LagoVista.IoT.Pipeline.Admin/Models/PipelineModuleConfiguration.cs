using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum PipelineModuleType
    {
        Listener,
        Planner,
        Sentinel,
        InputTranslator,
        Workflow,
        OutputTranslator,
        Transmitter,
        Custom,
    }

    public class PipelineModuleConfiguration : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IValidateable, IKeyedEntity, IPipelineModuleConfiguration, IOwnedEntity, INoSQLEntity
    {
        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Key, HelpResource: Resources.PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        public String DatabaseName { get; set; }

        public String EntityType { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_IsPublic,  FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public PipelineModuleConfigurationSummary CreateSummary()
        {
            return new PipelineModuleConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }

    public class PipelineModuleConfigurationSummary : LagoVista.Core.Models.SummaryData
    {

    }

}
