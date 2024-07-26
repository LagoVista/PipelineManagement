using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_Title, PipelineAdminResources.Names.Sentinel_Help, 
        PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-ae-coding-badge", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/sentinels", GetUrl: "/api/pipeline/admin/sentinel/{id}", SaveUrl: "/api/pipeline/admin/sentinel",
        FactoryUrl: "/api/pipeline/admin/sentinel/factory", DeleteUrl: "/api/pipeline/admin/sentinel/{id}",
        ListUIUrl: "/iotstudio/make/sentinels", CreateUIUrl: "/iotstudio/make/sentinel/add", EditUIUrl: "/iotstudio/make/sentinel/{id}")]
    public class SentinelConfiguration : PipelineModuleConfiguration, IFormDescriptor, IIconEntity, ISummaryFactory
    {
        public SentinelConfiguration()
        {
            SecurityFields = new List<SecurityField>();
            Icon = "icon-ae-coding-badge";
        }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }


        public override string ModuleType => PipelineModuleType_Sentinel;

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField, 
            HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Help, FieldType: FieldTypes.ChildListInline, 
            ResourceType: typeof(PipelineAdminResources), FactoryUrl: "/api/pipeline/admin/sentinel/securityfield/factory")]
        public List<SecurityField> SecurityFields { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
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
                Icon = Icon,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinels_Title, PipelineAdminResources.Names.Sentinel_Help,
        PipelineAdminResources.Names.Sentinel_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-ae-coding-badge", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/sentinels", GetUrl: "/api/pipeline/admin/sentinel/{id}", SaveUrl: "/api/pipeline/admin/sentinel",
        FactoryUrl: "/api/pipeline/admin/sentinel/factory", DeleteUrl: "/api/pipeline/admin/sentinel/{id}")]
    public class SentinelConfigurationSummary : SummaryData
    {

    }
}
