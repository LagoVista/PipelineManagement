using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.DataStreamField_Title, PipelineAdminResources.Names.DataStreamField_Help, PipelineAdminResources.Names.DataStreamField_Help, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class DataStreamField : IValidateable
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Name { get; set;}

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Description { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Notes, FieldType: FieldTypes.MultiLineText,  ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Notes { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_FieldName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9]{2,64}$",
             RegExValidationMessageResource: PipelineAdminResources.Names.DataStreamField_FieldName_Invalid, HelpResource: PipelineAdminResources.Names.DataStreamField_FieldName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string FieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_DataType, EnumType: (typeof(ParameterTypes)), HelpResource: PipelineAdminResources.Names.DataStreamField_DataType_Help, FieldType: FieldTypes.Picker, WaterMark:PipelineAdminResources.Names.DataStreamField_DataType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public EntityHeader<ParameterTypes> FieldType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_UnitSet, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: PipelineAdminResources.Names.DataStreamField_UnitSet_Watermark,  ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<UnitSet> UnitSet { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_StateSet, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: PipelineAdminResources.Names.DataStreamField_StateSet_Watermark, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<StateSet> StateSet { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_IsRequired, HelpResource: PipelineAdminResources.Names.DataStreamField_IsRequired_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsRequired { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_NumberDecimalPoints, HelpResource: PipelineAdminResources.Names.DataStreamField_NumberDecimalPoints_Help, FieldType: FieldTypes.Integer,ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int? NumberDecimalPoint { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_MinValue, HelpResource: PipelineAdminResources.Names.DataStreamField_MinValue_Help, FieldType: FieldTypes.Decimal,  ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public double? MinValue { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_MaxValue, HelpResource: PipelineAdminResources.Names.DataStreamField_MaxValue_Help, FieldType: FieldTypes.Decimal, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public double? MaxValue { get; set; }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            /* If field type isn't specified, it won't be valid so can't check the rest */
            if(!EntityHeader.IsNullOrEmpty(FieldType))
            {
               switch(FieldType.Value)
                {
                    case ParameterTypes.ValueWithUnit:
                        if (EntityHeader.IsNullOrEmpty(UnitSet)) result.Errors.Add(new ErrorMessage($"Unit Set is required on field {Name}"));
                        break;
                    case ParameterTypes.State:
                        if (EntityHeader.IsNullOrEmpty(StateSet)) result.Errors.Add(new ErrorMessage($"State Set is required on field {Name}"));
                        break;
                }
            }
        }

    }
}
