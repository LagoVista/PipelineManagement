using LagoVista.Core.Attributes;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_Title, PipelineAdminResources.Names.Sentinel_Help, PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class SentinelConfiguration : PipelineModuleConfiguration
    {
        public SentinelConfiguration()
        {
            SecurityFields = new List<SecurityField>();
        }

        public override string ModuleType => PipelineModuleType_Sentinel;

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField, HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Help, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<SecurityField> SecurityFields { get; set; }
    }
}
