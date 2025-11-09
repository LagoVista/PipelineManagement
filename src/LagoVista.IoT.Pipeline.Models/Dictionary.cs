// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 386c9bbb1c947abb2bc5b5d92987f2dda94514de80fff912a252a0f06da3e9cd
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Models
{
    public enum DictionaryTypes
    {
        [EnumLabel(Dictionary.DictionaryTypes_NuvIoT, PipelineAdminResources.Names.Dictionary_Type_NuvIoT, typeof(PipelineAdminResources))]
        NuvIoT,
        [EnumLabel(Dictionary.DictionaryTypes_Redis, PipelineAdminResources.Names.Dictionary_Type_Redis, typeof(PipelineAdminResources))]
        Redis,
    }


    // NOTE: Not currently fully implemented nor being used.
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Dictionary_Title, PipelineAdminResources.Names.Dictionary_Help, PipelineAdminResources.Names.Dictionary_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class Dictionary : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor
    {
        public const string DictionaryTypes_NuvIoT = "nuviot";
        public const string DictionaryTypes_Redis = "redis";

        public override string ModuleType => PipelineModuleType_Dictionary;

        [FormField(LabelResource: PipelineAdminResources.Names.Dictionary_Type, EnumType: typeof(DictionaryTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Dictionary_Type_Select, IsRequired: true)]
        public EntityHeader<DictionaryTypes> DictionaryType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Dictionary_Uri, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Dictionary_Password, HelpResource: PipelineAdminResources.Names.Dictionary_Password_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Password { get; set; }
        public string PasswordSecureId { get; set; }


        public List<string> GetFormFields()
        {
            return new List<string>()
            {

            };
        }
    }
}
