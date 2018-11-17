using LagoVista.Core.Attributes;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.CustomModule_Title, PipelineAdminResources.Names.CustomModule_Help, PipelineAdminResources.Names.CustomModule_Description,EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class CustomPipelineModuleConfiguration : PipelineModuleConfiguration
    {
        public String Script { get; set; }

        public override string ModuleType => PipelineModuleType_Custom;

        [FormField(LabelResource: PipelineAdminResources.Names.Custom_Uri, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Custom_AccountId, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AccountId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Custom_AccountPassword, HelpResource: PipelineAdminResources.Names.Custom_AccountPassword_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AccountPassword { get; set; }
        public string AccountPasswordSecureId { get; set; }
    }
}
