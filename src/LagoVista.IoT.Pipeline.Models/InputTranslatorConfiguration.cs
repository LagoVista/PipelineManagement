using LagoVista.AI.Models;
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help, 
        PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/inputtranslators", GetUrl: "/api/pipeline/admin/inputtranslator/{id}", SaveUrl: "/api/pipeline/admin/inputtranslator",
        DeleteUrl: "/api/pipeline/admin/inputtranslator/{id}",  FactoryUrl: "/api/pipeline/admin/inputtranslator/factory")]
    public class InputTranslatorConfiguration : PipelineModuleConfiguration, IFormDescriptor, IFormConditionalFields
    {
        public enum InputTranslatorTypes
        {
            [EnumLabel("message", PipelineAdminResources.Names.Translator_Type_Message, typeof(PipelineAdminResources))]
            MessageBased,

            [EnumLabel("nuvaimodel", PipelineAdminResources.Names.TranslatorType_AIModel, typeof(PipelineAdminResources))]
            NuvAIModel,

            [EnumLabel("custom", PipelineAdminResources.Names.Translator_Type_Custom, typeof(PipelineAdminResources))]
            Custom,

            [EnumLabel("script", PipelineAdminResources.Names.Translator_Type_Script, typeof(PipelineAdminResources))]
            Script,
        }

        public InputTranslatorConfiguration()
        {
            InputTranslatorType = EntityHeader<InputTranslatorTypes>.Create(InputTranslatorTypes.MessageBased);
        }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_TranslatorType, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_TranslatorType_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<InputTranslatorTypes> InputTranslatorType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_DelimterSequence, HelpResource: PipelineAdminResources.Names.InputTranslator_DelimiterSquence_Help, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string DelimiterSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, ScriptTemplateName:"input_translator_script", WaterMark:PipelineAdminResources.Names.Common_EditScript, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public String Script { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_Model, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_Model_Select, IsRequired: false, IsUserEditable: true)]
        public EntityHeader<Model> Model { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.InputTranslator_ModelRevision, EnumType: (typeof(InputTranslatorTypes)), FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.InputTranslator_ModelRevision_Select, IsRequired: false, IsUserEditable: true)]
        public EntityHeader<ModelRevision> ModelRevision { get; set; }


        public override string ModuleType => PipelineModuleType_InputTranslator;

        public List<string> GetFormFields()
        {

            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(InputTranslatorType),
                nameof(Script),
                nameof(Model),
                nameof(ModelRevision),
                nameof(Description),
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(DelimiterSequence), nameof(Script), nameof(Model), nameof(ModelRevision) },
                Conditionals = new List<FormConditional>()
                 {
                      new FormConditional()
                      {
                           Field = nameof(InputTranslatorType),
                           Value = "nuvaimodel",
                           VisibleFields = new List<string>() { nameof(Model), nameof(ModelRevision)},
                           RequiredFields = new List<string> { nameof(Model), nameof(ModelRevision) },
                      },
                      new FormConditional()
                      {
                           Field = nameof(InputTranslatorType),
                           Value = "script",
                           VisibleFields = new List<string>() { nameof(Script)},
                           RequiredFields = new List<string> { nameof(Script) },
                      }
                 }
            };
        }

        public InputTranslatorConfigurationSummary CreateSummary()
        {
            return new InputTranslatorConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help,
        PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/inputtranslators", GetUrl: "/api/pipeline/admin/inputtranslator/{id}", SaveUrl: "/api/pipeline/admin/inputtranslator",
        DeleteUrl: "/api/pipeline/admin/inputtranslator/{id}", FactoryUrl: "/api/pipeline/admin/inputtranslator/factory")]
    public class InputTranslatorConfigurationSummary : SummaryData
    {

    }
}