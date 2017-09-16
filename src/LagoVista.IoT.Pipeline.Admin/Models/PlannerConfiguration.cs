using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Models
{

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Planner_Title, PipelineAdminResources.Names.Planner_Help, PipelineAdminResources.Names.Planner_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class PlannerConfiguration : PipelineModuleConfiguration
    {
        public PlannerConfiguration()
        {
            DeviceIdParsers = new List<DeviceMessageDefinitionField>();
            MessageTypeIdParsers = new List<DeviceMessageDefinitionField>();
        }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_PipelineModules, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<PipelineModuleConfiguration> PipelineModules { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_DeviceIDParsers, HelpResource: PipelineAdminResources.Names.Planner_DeviceIDParsers_Help, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<DeviceMessageDefinitionField> DeviceIdParsers { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_MessageTypeIDParsers, HelpResource:PipelineAdminResources.Names.Planner_MessageTypeIDParsers_Help, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<DeviceMessageDefinitionField> MessageTypeIdParsers { get; set; }

        public override string ModuleType => PipelineModuleType_Planner;
    }
}
