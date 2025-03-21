﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Models.Resources;

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

    public abstract class PipelineModuleConfiguration : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IValidateable, IPipelineModuleConfiguration, ICategorized
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

        public abstract string ModuleType { get; }

        public bool DebugMode { get; set; }

    }

}
