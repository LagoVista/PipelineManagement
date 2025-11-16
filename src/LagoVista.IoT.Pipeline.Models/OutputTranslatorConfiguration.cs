// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: db17a6b96d4ee892db76f172fd1cdb4c7bbc8604d1617cd0a8cb903953982cb2
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.OutputTranslator_Title, PipelineAdminResources.Names.OutputTranslator_Help,
        PipelineAdminResources.Names.OutputTranslator_Description, EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-pz-translate-2", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/outputtranslators", GetUrl: "/api/pipeline/admin/outputtranslator/{id}", SaveUrl: "/api/pipeline/admin/outputtranslator",
        ListUIUrl: "/iotstudio/make/outputtranslators", EditUIUrl: "/iotstudio/make/outputtranslator/{0}", CreateUIUrl: "/iotstudio/make/outputtranslator/add",
        DeleteUrl: "/api/pipeline/admin/outputtranslator/{id}", FactoryUrl: "/api/pipeline/admin/outputtranslator/factory")]
    public class OutputTranslatorConfiguration : PipelineModuleConfiguration, IFormDescriptor, IFormConditionalFields, IIconEntity, ISummaryFactory
    {
        public enum OutputTranslatorTypes
        {
            [EnumLabel("message", PipelineAdminResources.Names.Translator_Type_Message, typeof(PipelineAdminResources))]
            MessageBased,
            /*[EnumLabel("binary", PipelineAdminResources.Names.Translator_Type_Binary, typeof(PipelineAdminResources))]
            Binary,
            [EnumLabel("string", PipelineAdminResources.Names.Translator_Type_String, typeof(PipelineAdminResources))]
            String,
            [EnumLabel("delimited", PipelineAdminResources.Names.Translator_Type_Delimited, typeof(PipelineAdminResources))]
            Delimited,            
            [EnumLabel("json", PipelineAdminResources.Names.Translator_Type_JSON, typeof(PipelineAdminResources))]
            JSON,            
            [EnumLabel("xml", PipelineAdminResources.Names.Translator_Type_XML, typeof(PipelineAdminResources))]
            XML,            */
            [EnumLabel("custom", PipelineAdminResources.Names.Translator_Type_Custom, typeof(PipelineAdminResources))]
            Custom
        }

        public OutputTranslatorConfiguration()
        {
            OutputTranslatorType = EntityHeader<OutputTranslatorTypes>.Create(OutputTranslatorTypes.MessageBased);
            Icon = "icon-pz-translate-2";
        }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }


        public override string ModuleType => PipelineModuleType_OutputTranslator;

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_TranslatorType, EnumType: (typeof(OutputTranslatorTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_TranslatorType_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<OutputTranslatorTypes> OutputTranslatorType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources))]
        public String Script { get; set; }


        public OutputTranslatorConfigurationSummary CreateSummary()
        {
            return new OutputTranslatorConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Icon = Icon,
                IsPublic = IsPublic,
                Description = Description,
                OutputTranslatorIypeId = OutputTranslatorType.Id,
                OutputTranslatorType = OutputTranslatorType.Text,
                Category = Category?.Text,
                CategoryId = Category?.Id,
                CategoryKey = Category?.Key,
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(Script) },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                         Field = nameof(OutputTranslatorType),
                         Value = "custom",
                         VisibleFields = new List<string>() {nameof(Script)}
                    }
                }
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
                nameof(OutputTranslatorType),
                nameof(Script)
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.OutputTranslators_Title, PipelineAdminResources.Names.OutputTranslator_Help,
        PipelineAdminResources.Names.OutputTranslator_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-pz-translate-2", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/outputtranslators", GetUrl: "/api/pipeline/admin/outputtranslator/{id}", SaveUrl: "/api/pipeline/admin/outputtranslator",
        DeleteUrl: "/api/pipeline/admin/outputtranslator/{id}", FactoryUrl: "/api/pipeline/admin/outputtranslator/factory")]
    public class OutputTranslatorConfigurationSummary : SummaryData
    {
        public string OutputTranslatorType { get; set; }
        public string OutputTranslatorIypeId { get; set; }
    }
}
