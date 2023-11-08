using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum CustomModuleTypes
    {
        [EnumLabel(CustomModuleConfiguration.CustomModuleType_Script, PipelineAdminResources.Names.CustomModule_CustomModuleType_Script, typeof(PipelineAdminResources))]
        Script,

        [EnumLabel(CustomModuleConfiguration.CustomModuleType_Container, PipelineAdminResources.Names.CustomModule_CustomModuleType_Container, typeof(PipelineAdminResources))]
        Container,

        [EnumLabel(CustomModuleConfiguration.CustomModuleType_WebFunction, PipelineAdminResources.Names.CustomModule_CustomModuleType_WebFunction, typeof(PipelineAdminResources))]
        WebFunction,

        [EnumLabel(CustomModuleConfiguration.CustomModuleType_DotNetAssembly, PipelineAdminResources.Names.CusotmModule_CustomModuleType_DotNetAssembly, typeof(PipelineAdminResources))]
        DotNetAssembly,
    }

    public enum UriAuthenticationTypes
    {
        [EnumLabel(CustomModuleConfiguration.AuthenticationType_Anonymous, PipelineAdminResources.Names.CustomModule_AuthenticationType_Anonymous, typeof(PipelineAdminResources))]
        Anonymous,

        [EnumLabel(CustomModuleConfiguration.AuthenticationType_BasicAuth, PipelineAdminResources.Names.CustomModule_AuthenticationType_BasicAuth, typeof(PipelineAdminResources))]
        BasicAuth,

        [EnumLabel(CustomModuleConfiguration.AuthenticationType_AuthenticationHeader, PipelineAdminResources.Names.CustomModule_AuthenticationType_AuthenticationHeader, typeof(PipelineAdminResources))]
        AuthenticationHeader,
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.CustomModule_Title, PipelineAdminResources.Names.CustomModule_Help, 
        PipelineAdminResources.Names.CustomModule_Description,EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/custommodules", SaveUrl: "/api/pipeline/admin/custommodule", GetUrl: "/api/pipeline/admin/custommodule/{id}",
        DeleteUrl: "/api/pipeline/admin/custommodule/{id}", FactoryUrl: "/api/pipeline/admin/custommodule/factory")]
    public class CustomModuleConfiguration : PipelineModuleConfiguration, IFormDescriptor
    {
        public const string CustomModuleType_Script = "script";
        public const string CustomModuleType_Container = "container";
        public const string CustomModuleType_WebFunction = "webfunction";
        public const string CustomModuleType_DotNetAssembly = "dotnetassembly";

        public const string AuthenticationType_Anonymous = "anonymous";
        public const string AuthenticationType_BasicAuth = "basicauth";
        public const string AuthenticationType_AuthenticationHeader = "authheader";

        
        public override string ModuleType => PipelineModuleType_Custom;

        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_CustomModuleType, EnumType:(typeof(CustomModuleTypes)), FieldType: FieldTypes.Picker, WaterMark:PipelineAdminResources.Names.CustomModule_CustomModuleType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public EntityHeader<CustomModuleTypes> CustomModuleType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_AuthenticationType, EnumType:(typeof(UriAuthenticationTypes)), FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.CustomModule_AuthenticationType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<UriAuthenticationTypes> AuthenticationType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public String Script { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Custom_Uri, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Uri { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Custom_AccountId, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AccountId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_AuthenticationHeader, HelpResource:PipelineAdminResources.Names.CustomModule_AuthenticationHeader_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AuthenticationHeader { get; set; }
        public string AuthenticationHeaderSecureId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Custom_AccountPassword, HelpResource: PipelineAdminResources.Names.Custom_AccountPassword_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AccountPassword { get; set; }
        public string AccountPasswordSecureId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_DotNetAssembly, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DotNetAssembly { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_DotNetClass, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DotNetClass { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_ContainerRepository, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark:PipelineAdminResources.Names.CustomModule_ContainerRepository_Select , IsRequired: false)]
        public EntityHeader ContainerRepository { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.CustomModule_ContainerTag, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.CustomModule_ContainerTag_Select, IsRequired: false)]
        public EntityHeader ContainerTag { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(CustomModuleType),
                nameof(Uri),
                nameof(Script),
                nameof(AuthenticationType),
                nameof(AccountId),
                nameof(AccountPassword),
                nameof(AuthenticationHeader),
                nameof(ContainerRepository),
                nameof(ContainerTag),
                nameof(DotNetAssembly),
                nameof(DotNetClass),
                nameof(Description)
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            if(EntityHeader.IsNullOrEmpty( CustomModuleType))
            {
                return;
            }

            switch(CustomModuleType.Value)
            {
                case CustomModuleTypes.Container:
                    if (EntityHeader.IsNullOrEmpty(ContainerRepository)) result.AddUserError("Container Repository is a required field.");
                    if (EntityHeader.IsNullOrEmpty(ContainerTag)) result.AddUserError("Container Tag is a required field.");
                    break;
                case CustomModuleTypes.DotNetAssembly:
                    if (String.IsNullOrEmpty(DotNetAssembly)) result.AddUserError("Name of the .NET Assembly is a required field.");
                    if (String.IsNullOrEmpty(DotNetClass)) result.AddUserError("Name of class within the .NET Assembly is a required field.");
                    break;
                case CustomModuleTypes.Script:
                    if (String.IsNullOrEmpty(Script)) result.AddUserError("Script is required.");
                    break;
                case CustomModuleTypes.WebFunction:
                    if (String.IsNullOrEmpty(Uri)) result.AddUserError("Uri is required.");

                    if (EntityHeader.IsNullOrEmpty(AuthenticationType))
                        result.AddUserError("Authentication type is a required field for Web Function type of custom modules.");
                    else
                    {
                        switch(AuthenticationType.Value)
                        {
                            case UriAuthenticationTypes.AuthenticationHeader:
                                if (action == Actions.Create)
                                {
                                    if (String.IsNullOrEmpty(AccountPassword)) result.AddUserError("Authorization header is required when adding.");
                                }
                                else if(action== Actions.Update)
                                {
                                    if (String.IsNullOrEmpty(AccountPassword)) result.AddUserError("Authorization Header or Authorization Header secure id is required when updating.");
                                }
                                break;
                            case UriAuthenticationTypes.BasicAuth:
                                if (String.IsNullOrEmpty(AccountId)) result.AddUserError("Account Id is required.");

                                if (action == Actions.Create)
                                {
                                    if (String.IsNullOrEmpty(AccountPassword)) result.AddUserError("Account password is required when adding.");
                                }
                                else if(action == Actions.Update)
                                {

                                    if (String.IsNullOrEmpty(AccountPassword) && String.IsNullOrEmpty(AccountPasswordSecureId)) result.AddUserError("Account Password or Secure Id is required when updating.");
                                }
                                break;
                        }
                    }

                    break;
            }
        }

        public CustomModuleConfigurationSummary CreateSummary()
        {
            return new CustomModuleConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.CustomModule_Title, PipelineAdminResources.Names.CustomModule_Help,
        PipelineAdminResources.Names.CustomModule_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources),
        GetListUrl: "/api/pipeline/admin/custommodules", SaveUrl: "/api/pipeline/admin/custommodule", GetUrl: "/api/pipeline/admin/custommodule/{id}",
        DeleteUrl: "/api/pipeline/admin/custommodule/{id}", FactoryUrl: "/api/pipeline/admin/custommodule/factory")]
    public class CustomModuleConfigurationSummary : SummaryData
    {

    }
}
