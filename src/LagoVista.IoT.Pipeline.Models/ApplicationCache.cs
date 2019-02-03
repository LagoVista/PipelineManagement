using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Models
{
    public enum CacheTypes
    {
        [EnumLabel(ApplicationCache.CacheType_LocalInMemory, PipelineAdminResources.Names.AppCache_CacheType_LocalInMemory, typeof(PipelineAdminResources))]
        LocalInMemory,

        [EnumLabel(ApplicationCache.CacheType_Redis, PipelineAdminResources.Names.AppCache_CacheType_Redis, typeof(PipelineAdminResources))]
        Redis,

        [EnumLabel(ApplicationCache.CacheType_NuvIot, PipelineAdminResources.Names.AppCache_CacheType_NuvIoT, typeof(PipelineAdminResources))]
        NuvIoT,
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.AppCache_Title, PipelineAdminResources.Names.AppCache_Help, PipelineAdminResources.Names.AppCache_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class ApplicationCache : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor
    {
        public ApplicationCache()
        {
            DefaultValues = new List<ApplicationCacheValue>();
        }

        public override string ModuleType => PipelineModuleConfiguration.PipelineModuleType_ApplicationCache;

        public const string CacheType_Redis = "redis";
        public const string CacheType_LocalInMemory = "localinmemory";
        public const string CacheType_NuvIot = "nuviot";


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_CacheType, EnumType: typeof(CacheTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.AppCache_SelectCacheType, IsRequired: true)]
        public EntityHeader<CacheTypes> CacheType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Uri, HelpResource: PipelineAdminResources.Names.AppCache_Uri_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Password, HelpResource: PipelineAdminResources.Names.AppCache_Password_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Password { get; set; }


        public string PasswordSecretId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_InitializationValues, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<ApplicationCacheValue> DefaultValues { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
            };
        }

        public new ApplicationCacheSummary CreateSummary()
        {
            return new ApplicationCacheSummary()
            {
                CacheTypeId = CacheType.Id,
                CacheType = CacheType.Text,
                Name = Name,
                Id = Id,
                Description = Description,
                IsPublic = IsPublic,
                Key = Key
            };
        }
    }

    public enum CacheValueDataTypes
    {
        [EnumLabel(ApplicationCacheValue.ValueType_String, PipelineAdminResources.Names.ApplicationCacheValue_Value_String, typeof(PipelineAdminResources))]
        String,

        [EnumLabel(ApplicationCacheValue.ValueType_Number, PipelineAdminResources.Names.ApplicationCacheValue_Value_Number, typeof(PipelineAdminResources))]
        Number,
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.ApplicationCacheValue_Title, PipelineAdminResources.Names.ApplicationCacheValue_Help, PipelineAdminResources.Names.ApplicationCacheValue_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class ApplicationCacheValue
    {
        public const string ValueType_String = "string";

        public const string ValueType_Number = "number";

        public string CreationDate { get; set; }

        public string LastUpdateDate { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Key, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Value, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Value { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Value_Type, EnumType: typeof(CacheTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.ApplicationCacheValue_Value_Type_Select, IsRequired: true)]
        public EntityHeader<CacheValueDataTypes> ValueType { get; set; }
    }

    public class ApplicationCacheSummary : SummaryData
    {
        public string CacheTypeId { get; set; }
        public string CacheType { get; set; }

    }
}
