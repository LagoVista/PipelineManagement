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
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Listener, Resources.PipelineAdminResources.Names.PipelineModuleType_Listener, typeof(Resources.PipelineAdminResources))]
        Listener,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Planner, Resources.PipelineAdminResources.Names.PipelineModuleType_Planner, typeof(Resources.PipelineAdminResources))]
        Planner,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Sentinel, Resources.PipelineAdminResources.Names.PipelineModuleType_Sentinel, typeof(Resources.PipelineAdminResources))]
        Sentinel,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_InputTranslator, Resources.PipelineAdminResources.Names.PipelineModuleType_InputTranslator, typeof(Resources.PipelineAdminResources))]
        InputTranslator,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_WorkFlow, Resources.PipelineAdminResources.Names.PipelineModuleType_Workflow, typeof(Resources.PipelineAdminResources))]
        Workflow,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_OutputTranslator, Resources.PipelineAdminResources.Names.PipelineModuleType_OutputTranslator, typeof(Resources.PipelineAdminResources))]
        OutputTranslator,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Transmitter, Resources.PipelineAdminResources.Names.PipelineModuleType_Transmitter, typeof(Resources.PipelineAdminResources))]
        Transmitter,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Custom, Resources.PipelineAdminResources.Names.PipelineModuleType_Custom, typeof(Resources.PipelineAdminResources))]
        Custom,
    }

    public abstract class PipelineModuleConfiguration : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IValidateable, IKeyedEntity, IPipelineModuleConfiguration, IOwnedEntity, INoSQLEntity
    {
        public const string PipelineModuleType_Listener = "listener";
        public const string PipelineModuleType_Planner = "planner";
        public const string PipelineModuleType_Sentinel = "sentinel";
        public const string PipelineModuleType_InputTranslator = "inputtranslator";
        public const string PipelineModuleType_WorkFlow = "workflow";
        public const string PipelineModuleType_OutputTranslator = "outputtranslator";
        public const string PipelineModuleType_Transmitter = "transmitter";
        public const string PipelineModuleType_Custom = "custom";

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Key, HelpResource: Resources.PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        public String DatabaseName { get; set; }

        public String EntityType { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_IsPublic,  FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public abstract string ModuleType { get; }

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
