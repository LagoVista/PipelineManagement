
using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.OutputTranslator_Title, PipelineAdminResources.Names.OutputTranslator_Help, PipelineAdminResources.Names.OutputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class OutputTranslatorConfiguration : PipelineModuleConfiguration
    {
        public enum OutputTranslatorTypes
        {
            [EnumLabel("binary", PipelineAdminResources.Names.Translator_Type_Binary, typeof(PipelineAdminResources))]
            Binary,
            [EnumLabel("string", PipelineAdminResources.Names.Translator_Type_String, typeof(PipelineAdminResources))]
            String,
            [EnumLabel("delimited", PipelineAdminResources.Names.Translator_Type_Delimited, typeof(PipelineAdminResources))]
            Delimited,            
            [EnumLabel("json", PipelineAdminResources.Names.Translator_Type_JSON, typeof(PipelineAdminResources))]
            JSON,            
            [EnumLabel("xml", PipelineAdminResources.Names.Translator_Type_XML, typeof(PipelineAdminResources))]
            XML,            
            [EnumLabel("custom", PipelineAdminResources.Names.Translator_Type_Custom, typeof(PipelineAdminResources))]
            Custom
        }

        public override string ModuleType => PipelineModuleType_OutputTranslator;

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_TranslatorType, EnumType: (typeof(OutputTranslatorTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_TranslatorType_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<OutputTranslatorTypes> OutputTranslatorType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, HelpResource: PipelineAdminResources.Names.InputTranslator_DelimiterSquence_Help, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources))]
        public String Script { get; set; }
    }
}
