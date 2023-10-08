using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Models
{
    public enum SharedConnectionTypes
    {
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_AWS, PipelineAdminResources.Names.SharedConnection_AWS, typeof(PipelineAdminResources))]
        AWS,
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_AZURE, PipelineAdminResources.Names.SharedConnection_Azure, typeof(PipelineAdminResources))]
        Azure,
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_DATABASE, PipelineAdminResources.Names.SharedConnection_Database, typeof(PipelineAdminResources))]
        Database,
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_REDIS, PipelineAdminResources.Names.SharedConnection_Redis, typeof(PipelineAdminResources))]
        Redis,
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.SharedConnection_Title, PipelineAdminResources.Names.SharedConnection_Help, PipelineAdminResources.Names.SharedConnection_Description,
        EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class SharedConnection : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IValidateable, IKeyedEntity, IPipelineModuleConfiguration, IOwnedEntity, INoSQLEntity, IFormDescriptor
    {
        public const string SHARED_CONNECTION_TYPE_AWS = "aws";
        public const string SHARED_CONNECTION_TYPE_AZURE = "azure";
        public const string SHARED_CONNECTION_TYPE_REDIS = "redis";
        public const string SHARED_CONNECTION_TYPE_DATABASE = "database";

        public string AWSSecretKeySecureId { get; set; }
        public string AzureAccessKeySecureId { get; set; }
        public string DBPasswordSecureId { get; set; }
        public string RedisPasswordSecureId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Key, HelpResource: PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public String Key { get; set; }

        public String DatabaseName { get; set; }

        public String EntityType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_IsPublic, HelpResource: PipelineAdminResources.Names.Common_IsPublic_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_ConnectionType, EnumType: typeof(SharedConnectionTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources),
            WaterMark: PipelineAdminResources.Names.SharedConnection_ConnectionType_Select, IsRequired: true)]
        public EntityHeader<SharedConnectionTypes> ConnectionType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureStorageName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureStorageAccountName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureAccessKey, HelpResource: PipelineAdminResources.Names.DataStream_AzureAccessKeyHelp, FieldType: FieldTypes.Password, SecureIdFieldName: nameof(AzureAccessKeySecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureAccessKey { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSAccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsAccessKey { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSRegion, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsRegion { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSSecretKey, HelpResource: PipelineAdminResources.Names.DataStream_AWSSecretKey_Help, FieldType: FieldTypes.Password, SecureIdFieldName:nameof(AWSSecretKeySecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsSecretKey { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbURL, ValidationRegEx: @"[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_DbUrl_InvalidUrl, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbURL { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbUserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbUserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbPassword, HelpResource: PipelineAdminResources.Names.DataStream_DbPassword_Help, FieldType: FieldTypes.Password, SecureIdFieldName:nameof(DBPasswordSecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbSchema, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbSchema { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_RedisPassword, HelpResource: PipelineAdminResources.Names.DataStream_RedisPassword_Help, FieldType: FieldTypes.Password, SecureIdFieldName:nameof(RedisPasswordSecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RedisPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_RedisServers, HelpResource: PipelineAdminResources.Names.DataStream_RedisServers_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RedisServerUris { get; set; }


        public SharedConnectionSummary CreateSummary()
        {
            return new SharedConnectionSummary()
            {
                ConnectionType = ConnectionType.Text,
                ConnectionTypeId = ConnectionType.Id,
                Description = Description,
                Id = Id,
                IsPublic = false,
                Key = Key,
                Name = Name
            };
        }

        [PreValidation]
        public void PreValidate(Actions action)
        {
            if (!EntityHeader.IsNullOrEmpty(ConnectionType))
            {
                if (ConnectionType.Value != SharedConnectionTypes.Azure)
                {
                    AzureAccessKey = null;
                    AzureAccessKeySecureId = null;
                }

                if (ConnectionType.Value != SharedConnectionTypes.Redis)
                {
                    RedisPassword = null;
                    RedisPasswordSecureId = null;
                    RedisServerUris = null;
                }

                if (ConnectionType.Value != SharedConnectionTypes.AWS)
                {
                    AwsRegion = null;
                    AwsSecretKey = null;
                    AwsAccessKey = null;
                    AWSSecretKeySecureId = null;
                }

                if (ConnectionType.Value != SharedConnectionTypes.Database)
                {
                    DbName = null;
                    DbPassword = null;
                    DBPasswordSecureId = null;
                    DbURL = null;
                    DbUserName = null;
                }
            }
        }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            /* full validation will happen somewhere else, if not valid, just skip */
            if (result.Errors.Count > 0) return;

            #region AWS Types

            if (ConnectionType.Value == SharedConnectionTypes.AWS)
            {
                if (string.IsNullOrEmpty(AwsAccessKey)) result.Errors.Add(new ErrorMessage("AWS Acceess Key is required for AWS Data Streams."));

                if ((action == Actions.Create) && string.IsNullOrEmpty(AwsSecretKey))
                {
                    result.Errors.Add(new ErrorMessage("AWS Secret Key is required for AWS Data Streams (it will be encrypted at rest)."));
                }
                else if (string.IsNullOrEmpty(AwsSecretKey) && string.IsNullOrEmpty(AWSSecretKeySecureId))
                {
                    result.Errors.Add(new ErrorMessage("AWS Secret Key or SecretKeyId are required for AWS Data Streams, if you are updating and replacing the key you should provide the new AWSSecretKey otherwise you could return the original secret key id."));
                }


                if (string.IsNullOrEmpty(AwsRegion))
                {
                    result.Errors.Add(new ErrorMessage("AWS Region is a required field for AWS Data Streams."));
                }
                else
                {
                    if (!AWSUtils.AWSS3Regions.Contains(AwsRegion) &&
                        !AWSUtils.AWSESRegions.Contains(AwsRegion)) result.Errors.Add(new ErrorMessage($"Invalid AWS Region, Region [{AwsRegion}] could not be found."));
                }
            }
            #endregion

            if (ConnectionType.Value == SharedConnectionTypes.Redis)
            {
                if ((action == Actions.Create) && String.IsNullOrEmpty(RedisPassword))
                {
                    if (String.IsNullOrEmpty(RedisPassword)) result.Errors.Add(new ErrorMessage("Missing Passwrod."));
                }
                else if (String.IsNullOrEmpty(RedisPassword) && String.IsNullOrEmpty(RedisPasswordSecureId))
                {
                    result.Errors.Add(new ErrorMessage("RedisPassword Password or RedisPasswordSecureId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you should return the original secret key id."));
                }
            }

            if (ConnectionType.Value == SharedConnectionTypes.Database)
            {
                if (string.IsNullOrEmpty(DbURL)) result.Errors.Add(new ErrorMessage("URL of database server is required for a database data stream."));
                if (string.IsNullOrEmpty(DbUserName)) result.Errors.Add(new ErrorMessage("Database User Name is required for a database data stream."));
                if (string.IsNullOrEmpty(DbName)) result.Errors.Add(new ErrorMessage("Database Name is required for a database data stream."));

                if (string.IsNullOrEmpty(DbPassword) && string.IsNullOrEmpty(DBPasswordSecureId))
                {
                    result.Errors.Add(new ErrorMessage("Database Password or SecretKeyId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you should return the original secret key id."));
                }
            }

            #region Azure Type
            if (ConnectionType.Value == SharedConnectionTypes.Azure)
            {
                if ((action == Actions.Create) && string.IsNullOrEmpty(AzureAccessKey))
                {
                    result.Errors.Add(new ErrorMessage("Azure Access Key is required"));
                }
                else if (string.IsNullOrEmpty(AzureAccessKey) && string.IsNullOrEmpty(AzureAccessKeySecureId))
                {
                    result.Errors.Add(new ErrorMessage("Azure Access Key or SecretKeyId are required for azure resources, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id."));
                }
            }
            #endregion
        }

        public override string ToString()
        {
            if(EntityHeader.IsNullOrEmpty(ConnectionType))
            {
                return "Connection type not set.";
            }

            switch(ConnectionType.Value)
            {
                case SharedConnectionTypes.AWS: return $"{ConnectionType.Value} {AwsAccessKey}, {AwsRegion}";
                case SharedConnectionTypes.Azure: return $"{ConnectionType.Value} {AzureStorageAccountName}, {AzureAccessKeySecureId}";
                case SharedConnectionTypes.Database: return $"{ConnectionType.Value} {DbURL}, {DbName}, {DBPasswordSecureId}";
                case SharedConnectionTypes.Redis: return $"{ConnectionType.Value} {RedisServerUris} - {RedisPasswordSecureId}";
            }

            return base.ToString();
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(ConnectionType),
                nameof(Key),
                nameof(Description),
                nameof(RedisPassword),
                nameof(RedisServerUris),
                nameof(AzureStorageAccountName),
                nameof(AzureAccessKey),
                nameof(AwsAccessKey),
                nameof(AwsRegion),
                nameof(AwsSecretKey),
                nameof(DbURL),
                nameof(DbName),
                nameof(DbSchema),
                nameof(DbUserName),
                nameof(DbPassword),
            };
        }
    }

    public class SharedConnectionSummary : SummaryData
    {
        public string ConnectionType { get; set; }
        public string ConnectionTypeId { get; set; }
    }
}
