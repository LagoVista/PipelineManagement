using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_Title, PipelineAdminResources.Names.Sentinel_Help, PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class SentinelConfiguration : PipelineModuleConfiguration, IFormDescriptor
    {
        public SentinelConfiguration()
        {
            SecurityFields = new List<SecurityField>();
        }

        public override string ModuleType => PipelineModuleType_Sentinel;

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField, HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Help, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources))]
        public List<SecurityField> SecurityFields { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Description),
                nameof(SecurityFields)
            };
        }
    }
}
