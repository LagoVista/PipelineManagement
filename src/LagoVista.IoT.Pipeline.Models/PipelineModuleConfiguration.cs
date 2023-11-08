using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum PipelineModuleType
    {
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Listener, PipelineAdminResources.Names.PipelineModuleType_Listener, typeof(PipelineAdminResources))]
        Listener,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Planner, PipelineAdminResources.Names.PipelineModuleType_Planner, typeof(PipelineAdminResources))]
        Planner,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Sentinel, PipelineAdminResources.Names.PipelineModuleType_Sentinel, typeof(PipelineAdminResources))]
        Sentinel,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_InputTranslator, PipelineAdminResources.Names.PipelineModuleType_InputTranslator, typeof(PipelineAdminResources))]
        InputTranslator,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_WorkFlow, PipelineAdminResources.Names.PipelineModuleType_Workflow, typeof(PipelineAdminResources))]
        Workflow,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_OutputTranslator, PipelineAdminResources.Names.PipelineModuleType_OutputTranslator, typeof(PipelineAdminResources))]
        OutputTranslator,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Transmitter, PipelineAdminResources.Names.PipelineModuleType_Transmitter, typeof(PipelineAdminResources))]
        Transmitter,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_DataStream, PipelineAdminResources.Names.PipelineModuleType_Transmitter, typeof(PipelineAdminResources))]
        DataStream,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_ApplicationCache, PipelineAdminResources.Names.PipelineModuleType_Transmitter, typeof(PipelineAdminResources))]
        ApplicationCache,
        [EnumLabel(PipelineModuleConfiguration.PipelineModuleType_Custom, PipelineAdminResources.Names.PipelineModuleType_DataStream, typeof(PipelineAdminResources))]
        Custom,
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help,
        PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/inputtranslators", GetUrl: "/api/pipeline/admin/inputtranslator/{id}", SaveUrl: "/api/pipeline/admin/inputtranslator",
        DeleteUrl: "/api/pipeline/admin/inputtranslator/{id}", FactoryUrl: "/api/pipeline/admin/inputtranslator/factory")]
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
        public const string PipelineModuleType_DataStream = "datastream";
        public const string PipelineModuleType_ApplicationCache = "applicationcachce";
        public const string PipelineModuleType_Dictionary = "dictionary";

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        public String DatabaseName { get; set; }

        public String EntityType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_IsPublic, HelpResource: PipelineAdminResources.Names.Common_IsPublic_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public abstract string ModuleType { get; }

        public bool DebugMode { get; set; }

    }
 
}
