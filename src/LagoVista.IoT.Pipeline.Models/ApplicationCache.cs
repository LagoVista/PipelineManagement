using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline
{
    public enum CachceTypes
    {
        [EnumLabel(ApplicationCache.CacheType_Redis, PipelineAdminResources.Names.AppCache_CacheType_Redis, typeof(PipelineAdminResources))]
        Redis,

        [EnumLabel(ApplicationCache.CacheType_NuvIot, PipelineAdminResources.Names.AppCache_CacheType_NuvIoT, typeof(PipelineAdminResources))]
        NuvIoT,

    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.AppCache_Title, PipelineAdminResources.Names.AppCache_Help, PipelineAdminResources.Names.AppCache_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class ApplicationCache : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor
    {
        public override string ModuleType => PipelineModuleConfiguration.PipelineModuleType_ApplicationCache;

        public const string CacheType_Redis = "redis";
        public const string CacheType_NuvIot = "nuviot";


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_CacheType, EnumType: typeof(CachceTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.AppCache_SelectCacheType, IsRequired: true)]
        public EntityHeader<CachceTypes> CacheType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Uri, HelpResource: PipelineAdminResources.Names.AppCache_Uri_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.AppCache_Password, HelpResource: PipelineAdminResources.Names.AppCache_Password_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Password { get; set; }


        public string PasswordSecretId { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
            };

        }

        public ApplicationCacheSummary CreateSummary()
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

    public class ApplicationCacheSummary : SummaryData
    {
        public string CacheTypeId { get; set; }
        public string CacheType { get; set; }

    }
}
