using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help, PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class InputTranslatorConfiguration : PipelineModuleConfiguration
    {        
        public enum InputTranslatorTypes
        {
            [EnumLabel("message", PipelineAdminResources.Names.Translator_Type_Message, typeof(PipelineAdminResources))]
            MessageBased,

            [EnumLabel("nuvaimodel", PipelineAdminResources.Names.TranslatorType_AIModel, typeof(PipelineAdminResources))]
            NuvAIModel,

            [EnumLabel("sevensegementparser", PipelineAdminResources.Names.TranslatorType_SevenSegmentParser, typeof(PipelineAdminResources))]
            SevenSegmentParser,

            /*[EnumLabel("binary", PipelineAdminResources.Names.Translator_Type_Binary, typeof(PipelineAdminResources))]
            Binary,
            [EnumLabel("string", PipelineAdminResources.Names.Translator_Type_String, typeof(PipelineAdminResources))]
            String,
            [EnumLabel("delimited", PipelineAdminResources.Names.Translator_Type_Delimited, typeof(PipelineAdminResources))]
            Delimited,
            [EnumLabel("json", PipelineAdminResources.Names.Translator_Type_JSON, typeof(PipelineAdminResources))]
            JSON,
            [EnumLabel("xml", PipelineAdminResources.Names.Translator_Type_XML, typeof(PipelineAdminResources))]
            XML,*/
            [EnumLabel("custom", PipelineAdminResources.Names.Translator_Type_Custom, typeof(PipelineAdminResources))]
            Custom
        }

        public InputTranslatorConfiguration()
        {
            InputTranslatorType = EntityHeader<InputTranslatorTypes>.Create(InputTranslatorTypes.MessageBased);
        }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_TranslatorType, EnumType: (typeof(InputTranslatorTypes)),  FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_TranslatorType_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<InputTranslatorTypes> InputTranslatorType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_DelimterSequence, HelpResource:PipelineAdminResources.Names.InputTranslator_DelimiterSquence_Help, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string DelimiterSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, HelpResource: PipelineAdminResources.Names.InputTranslator_DelimiterSquence_Help, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public String Script { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_Model, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_Model_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader Model { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_ModelRevision, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_ModelRevision_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader ModelRevision { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_SevenSegmentParser, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_SevenSegmentParser_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader SevenSegementParser { get; set; }


        public override string ModuleType => PipelineModuleType_InputTranslator;
    }
}