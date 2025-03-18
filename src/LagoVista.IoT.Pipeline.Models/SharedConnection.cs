using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Admin;
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
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_MQTT, PipelineAdminResources.Names.SharedConnection_MQTT, typeof(PipelineAdminResources))]
        Mqtt,
        [EnumLabel(SharedConnection.SHARED_CONNECTION_TYPE_SERVER, PipelineAdminResources.Names.SharedConnection_Server, typeof(PipelineAdminResources))]
        Server,
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.SharedConnection_Title, PipelineAdminResources.Names.SharedConnection_Help, PipelineAdminResources.Names.SharedConnection_Description,
        EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources),Icon: "icon-ae-connection-1",
        GetListUrl: "/api/sharedconnections", GetUrl: "/api/sharedconnection/{id}", SaveUrl: "/api/sharedconnection", FactoryUrl: "/api/sharedconnection/factory",
        ListUIUrl: "/iotstudio/settings/sharedconnections", CreateUIUrl: "/iotstudio/settings/sharedconnection/add", EditUIUrl: "/iotstudio/settings/sharedconnection/",
        DeleteUrl: "/api/sharedconnection/{id}")]
    public class SharedConnection : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IValidateable, IPipelineModuleConfiguration, IFormDescriptor, IFormConditionalFields, IIconEntity, ISummaryFactory, ICategorized
    {
        public SharedConnection()
        {
            Icon = "icon-ae-connection-1";
        }

        public const string SHARED_CONNECTION_TYPE_AWS = "aws";
        public const string SHARED_CONNECTION_TYPE_AZURE = "azure";
        public const string SHARED_CONNECTION_TYPE_REDIS = "redis";
        public const string SHARED_CONNECTION_TYPE_DATABASE = "database";
        public const string SHARED_CONNECTION_TYPE_SERVER = "server";
        public const string SHARED_CONNECTION_TYPE_MQTT = "mqtt";

        public string AWSSecretKeySecureId { get; set; }
        public string AzureAccessKeySecureId { get; set; }
        public string DBPasswordSecureId { get; set; }
        public string RedisPasswordSecureId { get; set; }
        public string ServerPasswordSecureId { get; set; }

        public string MqttPasswordSecureId { get; set; }



        [FormField(LabelResource: PipelineAdminResources.Names.Common_Category, FieldType: FieldTypes.Category, WaterMark: PipelineAdminResources.Names.Common_Category_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public EntityHeader Category { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_ConnectionType, EnumType: typeof(SharedConnectionTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources),
            WaterMark: PipelineAdminResources.Names.SharedConnection_ConnectionType_Select, IsRequired: true)]
        public EntityHeader<SharedConnectionTypes> ConnectionType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureStorageName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureStorageAccountName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureAccessKey, SecureIdFieldName: nameof(AzureAccessKeySecureId),
            HelpResource: PipelineAdminResources.Names.DataStream_AzureAccessKeyHelp, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureAccessKey { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSAccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsAccessKey { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSRegion, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsRegion { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSSecretKey, SecureIdFieldName: "awsSecretKeySecureId",
            HelpResource: PipelineAdminResources.Names.DataStream_AWSSecretKey_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsSecretKey { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbURL, ValidationRegEx: @"[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_DbUrl_InvalidUrl, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbURL { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbUserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbUserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbPassword, SecureIdFieldName: nameof(DBPasswordSecureId),
            HelpResource: PipelineAdminResources.Names.DataStream_DbPassword_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbSchema, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbSchema { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbName { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Server_Url, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ServerUrl { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Server_Port, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int ServerPort { get; set; } = 80;

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Server_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ServerUserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Server_Password, SecureIdFieldName: nameof(ServerPasswordSecureId), 
            FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ServerPassword { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Mqtt_Url, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string MqttUrl { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Mqtt_Port, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int MqttPort { get; set; } = 1883;

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Mqtt_Secure, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string MqttSecure { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Mqtt_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string MqttUsername { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SharedConnection_Mqtt_Password, SecureIdFieldName: nameof(MqttPasswordSecureId), FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string MqttPassword { get; set; }



        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_RedisPassword, SecureIdFieldName: nameof(RedisPasswordSecureId),
            HelpResource: PipelineAdminResources.Names.DataStream_RedisPassword_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
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
                Icon = Icon,
                Key = Key,
                Name = Name,
                Category = Category?.Text,
                CategoryId = Category?.Id,
                CategoryKey = Category?.Key,
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

            if (ConnectionType.Value == SharedConnectionTypes.Mqtt)
            {
                if (string.IsNullOrEmpty(MqttUrl)) result.Errors.Add(new ErrorMessage("URL of mqtt server is required for a mqtt server."));
                if (string.IsNullOrEmpty(MqttUsername)) result.Errors.Add(new ErrorMessage("User name of mqtt server is required for a mqtt server."));

                if (MqttPort == 0) result.Errors.Add(new ErrorMessage("MQTT Port should not be 0."));

                if (string.IsNullOrEmpty(MqttPassword) && string.IsNullOrEmpty(MqttPasswordSecureId))
                {
                    result.Errors.Add(new ErrorMessage("MQTT Password or SecretKeyId are required for a a mqtt server, if you are updating and replacing the password you should provide the new mqtt Password otherwise you should return the original secret key id."));
                }
            }


            if (ConnectionType.Value == SharedConnectionTypes.Server)
            {
                if (string.IsNullOrEmpty(ServerUrl)) result.Errors.Add(new ErrorMessage("URL of server is required for a server."));
                if (string.IsNullOrEmpty(ServerUserName)) result.Errors.Add(new ErrorMessage("Server User Name is required for a server."));
                if (ServerPort == 0) result.Errors.Add(new ErrorMessage("Server Port should not be 0."));

                if (string.IsNullOrEmpty(ServerPassword) && string.IsNullOrEmpty(ServerPasswordSecureId))
                {
                    result.Errors.Add(new ErrorMessage("Server Password or SecretKeyId are required for a server, if you are updating and replacing the key you should provide the new server password otherwise you should return the original secret key id."));
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
            if (EntityHeader.IsNullOrEmpty(ConnectionType))
            {
                return "Connection type not set.";
            }

            switch (ConnectionType.Value)
            {
                case SharedConnectionTypes.AWS: return $"{ConnectionType.Value} {AwsAccessKey}, {AwsRegion}";
                case SharedConnectionTypes.Azure: return $"{ConnectionType.Value} {AzureStorageAccountName}, {AzureAccessKeySecureId}";
                case SharedConnectionTypes.Database: return $"{ConnectionType.Value} {DbURL}, {DbName}, {DBPasswordSecureId}";
                case SharedConnectionTypes.Redis: return $"{ConnectionType.Value} {RedisServerUris} - {RedisPasswordSecureId}";
                case SharedConnectionTypes.Mqtt: return $"{ConnectionType.Value} {MqttUrl}:{MqttPort} {MqttUsername} - {RedisPasswordSecureId}";
                case SharedConnectionTypes.Server: return $"{ConnectionType.Value} {ServerUrl}:{ServerPort}, {ServerUserName} - {RedisPasswordSecureId}";
            }

            return base.ToString();
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
                nameof(ConnectionType),
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
                nameof(MqttUrl),
                nameof(MqttPort),
                nameof(MqttSecure),
                nameof(MqttUsername),
                nameof(MqttPassword),
                nameof(ServerUrl),
                nameof(ServerPort),
                nameof(ServerUserName),
                nameof(ServerPassword)
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(AwsAccessKey), nameof(AwsRegion), nameof(AwsSecretKey),
                    nameof(MqttUrl), nameof(MqttPort), nameof(MqttSecure), nameof(MqttUsername), nameof(MqttPassword), nameof(MqttPasswordSecureId),
                    nameof(ServerUrl), nameof(ServerPort), nameof(ServerUserName), nameof(ServerPassword),
                    nameof(DbName), nameof(DbPassword), nameof(DbSchema), nameof(DbURL), nameof(DbUserName), nameof(DbURL),
                    nameof(RedisPassword), nameof(RedisServerUris),
                    nameof(AzureAccessKey), nameof(AzureStorageAccountName) },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_AWS,
                        ForUpdate = true,
                        ForCreate = false,
                        VisibleFields = new List<string>() { nameof(AwsAccessKey), nameof(AwsRegion), nameof(AwsSecretKey)},
                        RequiredFields = new List<string>() { nameof(AwsAccessKey), nameof(AwsRegion)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_AWS,
                        VisibleFields = new List<string>() { nameof(AwsAccessKey), nameof(AwsRegion), nameof(AwsSecretKey)},
                        RequiredFields = new List<string>() { nameof(AwsAccessKey), nameof(AwsRegion), nameof(AwsSecretKey)},
                        ForCreate = true,
                        ForUpdate = false,
                    },

                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_AZURE,
                        ForCreate = false,
                        VisibleFields = new List<string>() { nameof(AzureAccessKey), nameof(AzureStorageAccountName)},
                        RequiredFields = new List<string>() {  nameof(AzureStorageAccountName)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_AZURE,
                        ForUpdate = false,
                        VisibleFields = new List<string>() { nameof(AzureAccessKey), nameof(AzureStorageAccountName)},
                        RequiredFields = new List<string>() {  nameof(AzureAccessKey), nameof(AzureStorageAccountName)},
                    },
               
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_REDIS,
                        ForCreate=false,
                        VisibleFields = new List<string>() { nameof(RedisPassword), nameof(RedisServerUris)},
                        RequiredFields = new List<string>() {  nameof(RedisServerUris)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_REDIS,
                        ForUpdate = false,
                        VisibleFields = new List<string>() { nameof(RedisPassword), nameof(RedisServerUris)},
                        RequiredFields = new List<string>() {  nameof(RedisPassword), nameof(RedisServerUris)},
                    },

                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_DATABASE,
                        ForCreate = false,
                        VisibleFields = new List<string>() { nameof(DbURL), nameof(DbName),nameof(DbSchema),nameof(DbUserName),nameof(DbPassword)},
                        RequiredFields = new List<string>() {  nameof(DbURL), nameof(DbName),nameof(DbSchema),nameof(DbUserName)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_DATABASE,
                        ForUpdate = false,
                        VisibleFields = new List<string>() { nameof(DbURL), nameof(DbName),nameof(DbSchema),nameof(DbUserName),nameof(DbPassword)},
                        RequiredFields = new List<string>() {  nameof(DbURL), nameof(DbName),nameof(DbSchema),nameof(DbUserName),nameof(DbPassword)},
                    },


                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_SERVER,
                        ForCreate = false,
                        VisibleFields = new List<string>() { nameof(ServerUrl), nameof(ServerPort),nameof(ServerUserName),nameof(ServerPassword)},
                        RequiredFields = new List<string>() { nameof(ServerUrl), nameof(ServerPort),nameof(ServerUserName)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_SERVER,
                        ForUpdate = false,
                        VisibleFields = new List<string>() { nameof(ServerUrl), nameof(ServerPort),nameof(ServerUserName),nameof(ServerPassword)},
                        RequiredFields = new List<string>() { nameof(ServerUrl), nameof(ServerPort),nameof(ServerUserName),nameof(ServerPassword)},
                    },

                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_MQTT,
                        ForCreate = false,
                        VisibleFields = new List<string>() { nameof(MqttUrl), nameof(MqttPort),nameof(MqttSecure),nameof(MqttUsername),nameof(MqttPassword)},
                        RequiredFields = new List<string>() {  nameof(MqttUrl), nameof(MqttPort),nameof(MqttUsername)},
                    },
                    new FormConditional()
                    {
                        Field = nameof(ConnectionType),
                        Value = SHARED_CONNECTION_TYPE_MQTT,
                        ForUpdate = false,
                        VisibleFields = new List<string>() { nameof(MqttUrl), nameof(MqttPort),nameof(MqttSecure),nameof(MqttUsername),nameof(MqttPassword)},
                        RequiredFields = new List<string>() {  nameof(MqttUrl), nameof(MqttPort),nameof(MqttUsername),nameof(MqttPassword)},
                    },
                }

            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.SharedConnections_Title, PipelineAdminResources.Names.SharedConnection_Help,
        PipelineAdminResources.Names.SharedConnection_Description,
        EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-ae-connection-1",
        GetListUrl: "/api/sharedconnections", GetUrl: "/api/sharedconnection/{id}", SaveUrl: "/api/sharedconnection", FactoryUrl: "/api/sharedconnection/factory",
        DeleteUrl: "/api/sharedconnection/{id}")]
    public class SharedConnectionSummary : SummaryData
    {
        public string ConnectionType { get; set; }
        public string ConnectionTypeId { get; set; }
    }
}
