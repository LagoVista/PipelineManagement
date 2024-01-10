using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Models
{

    public enum SecurityFieldType
    {
        [EnumLabel(SecurityField.FieldType_AccessKey, PipelineAdminResources.Names.Sentinel_SecurityField_Type_AccessKey, typeof(PipelineAdminResources))]
        AccessKey,
        [EnumLabel(SecurityField.FieldType_BasicAccess, PipelineAdminResources.Names.Sentinel_SecurityField_Type_BasicAuth, typeof(PipelineAdminResources))]
        BasicAccess,
        [EnumLabel(SecurityField.FieldType_Script, PipelineAdminResources.Names.Sentinel_SecurityField_Type_Script, typeof(PipelineAdminResources))]
        Script,
        [EnumLabel(SecurityField.FieldType_SharedSignature, PipelineAdminResources.Names.Sentinel_SecurityField_Type_SharedSignature, typeof(PipelineAdminResources))]
        SharedAccessSignature,
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_SecurityField_Title,
        PipelineAdminResources.Names.Sentinel_SecurityField_Help, PipelineAdminResources.Names.Sentinel_SecurityField_Description,
        EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources), FactoryUrl: "/api/pipeline/admin/sentinel/securityfield/factory")]
    public class SecurityField : IKeyedEntity, INamedEntity, IValidateable, IFormDescriptor, IFormConditionalFields
    {
        public const string FieldType_AccessKey = "accesskey";
        public const string FieldType_BasicAccess = "basicaccess";
        public const string FieldType_Script = "script";
        public const string FieldType_SharedSignature = "sharedsignature";

        [JsonProperty("id")]
        [FormField(LabelResource: PipelineAdminResources.Names.Common_UniqueId, IsUserEditable: false, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Id { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Name { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField_Locator, FactoryUrl: "/api/devicemessagetype/field/factory",
            HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Locator, FieldType: FieldTypes.EntityHeaderPicker, 
            WaterMark: PipelineAdminResources.Names.Sentinel_SecurityField_Locator_Select, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<DeviceMessageDefinitionField> Locator { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources))]
        public String Script { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField_Type, HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Type_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Sentinel_SecurityField_Type_Select, EnumType: typeof(SecurityFieldType), IsRequired: true, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<SecurityFieldType> FieldType { get; set; }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(Script) },
                Conditionals = new List<FormConditional>() {
                    new FormConditional()
                    {
                        Field = nameof(FieldType),
                        Value = FieldType_Script,
                        VisibleFields =  new List<string>() { nameof(Script) },
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
                nameof(Locator),
                nameof(Script),
                nameof(FieldType),
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {

        }
    }
}
