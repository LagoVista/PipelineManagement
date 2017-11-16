using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Models
{

    public enum SecurityFieldType
    {
        SharedAccessSignature,
        AccessKey,
        BasicAccess
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Sentinel_SecurityField_Title, PipelineAdminResources.Names.Sentinel_SecurityField_Help, PipelineAdminResources.Names.Sentinel_SecurityField_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class SecurityField : IKeyedEntity, INamedEntity, IValidateable
    {
        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Name { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField_Locator, HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Locator, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<DeviceMessageDefinitionField> Locator { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Sentinel_SecurityField_Type, HelpResource: PipelineAdminResources.Names.Sentinel_SecurityField_Type_Help, FieldType: FieldTypes.Picker, WaterMark:PipelineAdminResources.Names.Sentinel_SecurityField_Type_Select, EnumType:typeof(SecurityFieldType), ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader<SecurityFieldType> FieldType { get; set; }
    }
}
