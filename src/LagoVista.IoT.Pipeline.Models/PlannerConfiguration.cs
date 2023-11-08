﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Planner_Title, PipelineAdminResources.Names.Planner_Help, 
        PipelineAdminResources.Names.Planner_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/planners", GetUrl: "/api/pipeline/admin/planner/{id}", SaveUrl: "/api/pipeline/admin/planner", DeleteUrl: "/api/pipeline/admin/planner",
        FactoryUrl: "/api/pipeline/admin/listener/factory")]
    public class PlannerConfiguration : PipelineModuleConfiguration, IFormDescriptor
    {
        public PlannerConfiguration()
        {
            DeviceIdParsers = new List<DeviceMessageDefinitionField>();
            MessageTypeIdParsers = new List<DeviceMessageDefinitionField>();
        }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_PipelineModules, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<PipelineModuleConfiguration> PipelineModules { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_DeviceIDParsers, HelpResource: PipelineAdminResources.Names.Planner_DeviceIDParsers_Help, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources))]
        public List<DeviceMessageDefinitionField> DeviceIdParsers { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_MessageTypeIDParsers, HelpResource: PipelineAdminResources.Names.Planner_MessageTypeIDParsers_Help, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources))]
        public List<DeviceMessageDefinitionField> MessageTypeIdParsers { get; set; }

        public override string ModuleType => PipelineModuleType_Planner;

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Description),
                nameof(DeviceIdParsers),
                nameof(MessageTypeIdParsers),
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            foreach (var fld in DeviceIdParsers)
            {
                result.Concat(fld.Validate());
            }

            foreach (var fld in MessageTypeIdParsers)
            {
                result.Concat(fld.Validate());
            }
        }

        public new PlannerConfigurationSummary CreateSummary()
        {
            return new PlannerConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Planner_Title, PipelineAdminResources.Names.Planner_Help,
        PipelineAdminResources.Names.Planner_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/planners", GetUrl: "/api/pipeline/admin/planner/{id}", SaveUrl: "/api/pipeline/admin/planner", DeleteUrl: "/api/pipeline/admin/planner",
        FactoryUrl: "/api/pipeline/admin/listener/factory")]
    public class PlannerConfigurationSummary : SummaryData
    {
    }
}
