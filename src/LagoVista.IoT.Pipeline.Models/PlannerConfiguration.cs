using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Planner_Title, PipelineAdminResources.Names.Planner_Help, 
        PipelineAdminResources.Names.Planner_Description, EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-pz-planner", Cloneable: true,
        ListUIUrl: "/iotstudio/make/planners", EditUIUrl: "/iotstudio/make/planner/{0}", CreateUIUrl: "/iotstudio/make/planner/add",
        GetListUrl: "/api/pipeline/admin/planners", GetUrl: "/api/pipeline/admin/planner/{id}", SaveUrl: "/api/pipeline/admin/planner", DeleteUrl: "/api/pipeline/admin/planner",
        FactoryUrl: "/api/pipeline/admin/planner/factory")]
    public class PlannerConfiguration : PipelineModuleConfiguration, IFormDescriptor, IIconEntity, ISummaryFactory
    {
        public PlannerConfiguration()
        {
            DeviceIdParsers = new List<MessageAttributeParser>();
            MessageTypeIdParsers = new List<MessageAttributeParser>();
            Icon = "icon-pz-planner";
        }


        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_PipelineModules, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<PipelineModuleConfiguration> PipelineModules { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_DeviceIDParsers, HelpResource: PipelineAdminResources.Names.Planner_DeviceIDParsers_Help, 
            FactoryUrl: "/api/messageattributeparser/factory", FieldType: FieldTypes.ChildListInline, InPlaceEditing:false, ResourceType: typeof(PipelineAdminResources))]
        public List<MessageAttributeParser> DeviceIdParsers { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Planner_MessageTypeIDParsers, HelpResource: PipelineAdminResources.Names.Planner_MessageTypeIDParsers_Help,
            FactoryUrl: "/api/messageattributeparser/factory", FieldType: FieldTypes.ChildListInline, InPlaceEditing: false, ResourceType: typeof(PipelineAdminResources))]
        public List<MessageAttributeParser> MessageTypeIdParsers { get; set; }

        public override string ModuleType => PipelineModuleType_Planner;

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
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

        public PlannerConfigurationSummary CreateSummary()
        {
            return new PlannerConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Icon = Icon,
                IsPublic = IsPublic,
                Description = Description,
                Category = Category?.Text,
                CategoryId = Category?.Id,
                CategoryKey = Category?.Key,
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Planners_Title, PipelineAdminResources.Names.Planner_Help,
        PipelineAdminResources.Names.Planner_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-pz-planner", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/planners", GetUrl: "/api/pipeline/admin/planner/{id}", SaveUrl: "/api/pipeline/admin/planner", DeleteUrl: "/api/pipeline/admin/planner",
        FactoryUrl: "/api/pipeline/admin/planner/factory")]
    public class PlannerConfigurationSummary : SummaryData
    {
    }
}
