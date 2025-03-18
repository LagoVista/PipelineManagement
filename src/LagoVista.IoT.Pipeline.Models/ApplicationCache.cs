using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
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


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.AppCache_Title, PipelineAdminResources.Names.AppCache_Help, 
        PipelineAdminResources.Names.AppCache_Description, EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-ae-database-3",
        ListUIUrl: "/iotstudio/make/applicationcache/appcaches", EditUIUrl: "/iotstudio/make/applicationcache/{0}", CreateUIUrl: "/iotstudio/make/applicationcache/add",
        GetListUrl: "/api/appcaches", GetUrl: "/api/appcache/{id}", SaveUrl: "/api/appcache", DeleteUrl: "/api/appcache/{id}", FactoryUrl: "/api/appcache/factory")]
    public class ApplicationCache : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor, ISummaryFactory, IIconEntity,IFormConditionalFields
    {
        public ApplicationCache()
        {
            DefaultValues = new List<ApplicationCacheValue>();
            Icon = "icon-ae-database-3";
        }

        public override string ModuleType => PipelineModuleConfiguration.PipelineModuleType_ApplicationCache;

        public const string CacheType_Redis = "redis";
        public const string CacheType_LocalInMemory = "localinmemory";
        public const string CacheType_NuvIot = "nuviot";


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_CacheType, EnumType: typeof(CacheTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.AppCache_SelectCacheType, IsRequired: true)]
        public EntityHeader<CacheTypes> CacheType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Uri, HelpResource: PipelineAdminResources.Names.AppCache_Uri_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Password, HelpResource: PipelineAdminResources.Names.AppCache_Password_Help, 
            SecureIdFieldName:nameof(PasswordSecretId), FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Password { get; set; }

        public string PasswordSecretId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_InitializationValues, FieldType: FieldTypes.ChildListInline, FactoryUrl: "/api/appcache/value/factory", ResourceType: typeof(PipelineAdminResources))]
        public List<ApplicationCacheValue> DefaultValues { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
                nameof(CacheType),
                nameof(Uri),
                nameof(Password),
                nameof(DefaultValues),
                nameof(Description)
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult res, Actions action)
        {
            foreach (var value in DefaultValues)
            {
                value.Validate(res, action);
            }
        }


        public ApplicationCacheSummary CreateSummary()
        {
            return new ApplicationCacheSummary()
            {
                CacheTypeId = CacheType.Id,
                CacheType = CacheType.Text,
                Name = Name,
                Id = Id,
                Icon = Icon,
                Description = Description,
                IsPublic = IsPublic,
                Key = Key,
                Category = Category?.Text,
                CategoryId = Category?.Id,
                CategoryKey = Category?.Key,
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(Uri), nameof(Password) },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                       Field = nameof(CacheType),
                       Value = CacheType_Redis,
                       RequiredFields = new List<string>() { nameof(Uri)},
                       VisibleFields = new List<string>() {nameof(Uri), nameof(Password)}
                    }
                }
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

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.ApplicationCacheValue_Title, PipelineAdminResources.Names.ApplicationCacheValue_Help, 
        PipelineAdminResources.Names.ApplicationCacheValue_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources), 
        FactoryUrl: "/api/appcache/value/factory")]
    public class ApplicationCacheValue : IFormDescriptor
    {
        public const string ValueType_String = "string";

        public const string ValueType_Number = "number";

        public string CreationDate { get; set; }

        public string LastUpdateDate { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Key, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Value, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Value { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.ApplicationCacheValue_Value_Type, EnumType: typeof(CacheValueDataTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.ApplicationCacheValue_Value_Type_Select, IsRequired: true)]
        public EntityHeader<CacheValueDataTypes> ValueType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Description { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(ValueType),
                nameof(Value),
                nameof(Description)
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult res, Actions action)
        {            
            if (ValueType.Value == CacheValueDataTypes.Number)
            {
                if (!Double.TryParse(Value, out _))
                {
                    res.AddUserError($"Value provided for {Name} must be a number, it currently is {Value}.");
                }
            }
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.AppCache_Title, PipelineAdminResources.Names.AppCache_Help,
       PipelineAdminResources.Names.AppCache_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-ae-database-3",
       GetListUrl: "/api/appcaches", GetUrl: "/api/appcache/{id}", SaveUrl: "/api/appcache", DeleteUrl: "/api/appcache/{id}", FactoryUrl: "/api/appcache/factory")]
    public class ApplicationCacheSummary : SummaryData
    {
        public string CacheTypeId { get; set; }
        public string CacheType { get; set; }

    }
}
