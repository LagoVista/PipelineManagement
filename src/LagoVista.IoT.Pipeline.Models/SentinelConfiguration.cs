using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_Title, PipelineAdminResources.Names.Sentinel_Help, 
        PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/sentinels", GetUrl: "/api/pipeline/admin/sentinel/{id}", SaveUrl: "/api/pipeline/admin/sentinel",
        FactoryUrl: "/api/pipeline/admin/sentinel/factory", DeleteUrl: "/api/pipeline/admin/sentinel/{id}")]
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

        public SentinelConfigurationSummary CreateSummary()
        {
            return new SentinelConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_Title, PipelineAdminResources.Names.Sentinel_Help,
        PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/sentinels", GetUrl: "/api/pipeline/admin/sentinel/{id}", SaveUrl: "/api/pipeline/admin/sentinel",
        FactoryUrl: "/api/pipeline/admin/sentinel/factory", DeleteUrl: "/api/pipeline/admin/sentinel/{id}")]
    public class SentinelConfigurationSummary : SummaryData
    {

    }
}
