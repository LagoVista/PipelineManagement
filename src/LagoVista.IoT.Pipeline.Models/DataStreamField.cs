﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.DataStreamField_Title, PipelineAdminResources.Names.DataStreamField_Help,
        PipelineAdminResources.Names.DataStreamField_Help, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        FactoryUrl: "/api/datastreamfield/factory")]
    public class DataStreamField : IValidateable, IFormDescriptor, IFormDescriptorCol2, IFormConditionalFields
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Description { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Notes, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Notes { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_FieldName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9_-]{1,64}$",
             RegExValidationMessageResource: PipelineAdminResources.Names.DataStreamField_FieldName_Invalid, HelpResource: PipelineAdminResources.Names.DataStreamField_FieldName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string FieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_DataType, EnumType: (typeof(ParameterTypes)), HelpResource: PipelineAdminResources.Names.DataStreamField_DataType_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.DataStreamField_DataType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public EntityHeader<ParameterTypes> FieldType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_UnitSet, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: PipelineAdminResources.Names.DataStreamField_UnitSet_Watermark,
           EntityHeaderPickerUrl: "/api/deviceadmin/unitsets", ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<UnitSet> UnitSet { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_StateSet, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: PipelineAdminResources.Names.DataStreamField_StateSet_Watermark,
            EntityHeaderPickerUrl: "/api/statemachine/statesets", ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<StateSet> StateSet { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_IsKey, HelpResource: PipelineAdminResources.Names.DataStreamField_IsKey_Description, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsKeyField { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_IsDatabaseGenerated, HelpResource: PipelineAdminResources.Names.DataStreamField_IsDatabaseGenerated_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsDatabaseGenerated { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_IsRequired, HelpResource: PipelineAdminResources.Names.DataStreamField_IsRequired_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsRequired { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_NumberDecimalPoints, HelpResource: PipelineAdminResources.Names.DataStreamField_NumberDecimalPoints_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int? NumberDecimalPoint { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_MinValue, HelpResource: PipelineAdminResources.Names.DataStreamField_MinValue_Help, FieldType: FieldTypes.Decimal, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public double? MinValue { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStreamField_MaxValue, HelpResource: PipelineAdminResources.Names.DataStreamField_MaxValue_Help, FieldType: FieldTypes.Decimal, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public double? MaxValue { get; set; }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(MinValue), nameof(MaxValue), nameof(NumberDecimalPoint), nameof(StateSet), nameof(UnitSet) },
                Conditionals = new List<FormConditional>()
                 {
                     new FormConditional()
                     {
                          Field = nameof(FieldType),
                          Value = nameof(TypeSystem.Decimal),
                          VisibleFields = new List<string>() {nameof(MinValue), nameof(MaxValue), nameof(NumberDecimalPoint)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(FieldType),
                          Value = nameof(TypeSystem.Integer),
                          VisibleFields = new List<string>() {nameof(MinValue), nameof(MaxValue)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(FieldType),
                          Value = nameof(TypeSystem.State),
                          VisibleFields = new List<string>() {nameof(StateSet)},
                          RequiredFields = new List<string>() {nameof(StateSet)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(FieldType),
                          Value = nameof(TypeSystem.ValueWithUnit),
                          VisibleFields = new List<string>() {nameof(UnitSet), nameof(NumberDecimalPoint), nameof(MinValue), nameof(MaxValue)},
                          RequiredFields = new List<string>() {nameof(UnitSet)}
                           
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
                nameof(FieldName),
                nameof(FieldType),
                nameof(StateSet),
                nameof(UnitSet),
                nameof(Description),
            };
        }

        public List<string> GetFormFieldsCol2()
        {
            return new List<string>()
            {
                nameof(IsKeyField),
                nameof(IsRequired),
                nameof(IsDatabaseGenerated),
                nameof(MinValue),
                nameof(MaxValue),
                nameof(NumberDecimalPoint),
                nameof(Notes),
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            /* If field type isn't specified, it won't be valid so can't check the rest */
            if (!EntityHeader.IsNullOrEmpty(FieldType))
            {
                switch (FieldType.Value)
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
