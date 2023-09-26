using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum DataStreamTypes
    {
        [EnumLabel(DataStream.StreamType_AWS_ElasticSearch, PipelineAdminResources.Names.DataStream_StreamType_AWS_ElasticSearch, typeof(PipelineAdminResources))]
        AWSElasticSearch,
        [EnumLabel(DataStream.StreamType_AWS_S3, PipelineAdminResources.Names.DataStream_StreamType_AWS_S3, typeof(PipelineAdminResources))]
        AWSS3,
        [EnumLabel(DataStream.StreamType_AzureBlob, PipelineAdminResources.Names.DataStream_StreamType_AzureBlob, typeof(PipelineAdminResources))]
        AzureBlob,
        //[EnumLabel(DataStream.StreamType_AzureBlob_Managed, PipelineAdminResources.Names.DataStream_StreamType_AzureBlob_Managed, typeof(PipelineAdminResources))]
        //AzureBlob_Managed,
        [EnumLabel(DataStream.StreamType_AzureEventHub,  PipelineAdminResources.Names.DataStream_StreamType_AzureEventHub, typeof(PipelineAdminResources))]
        AzureEventHub,
        [EnumLabel(DataStream.StreamType_AzureTableStorage, PipelineAdminResources.Names.DataStream_StreamType_TableStorage, typeof(PipelineAdminResources))]
        AzureTableStorage,
        [EnumLabel(DataStream.StreamType_AzureTableStorage_Managed, PipelineAdminResources.Names.DataStream_StreamType_TableStorage_Managed, typeof(PipelineAdminResources))]
        AzureTableStorage_Managed,
        [EnumLabel(DataStream.StreamType_PostgreSQL, PipelineAdminResources.Names.DataStream_StreamType_PostgreSQL, typeof(PipelineAdminResources))]
        Postgresql,
        [EnumLabel(DataStream.StreamType_PointArrayStorage, PipelineAdminResources.Names.DataStream_StreamType_PointArrayStorage, typeof(PipelineAdminResources))]
        PointArrayStorage,
        [EnumLabel(DataStream.StreamType_Redis, PipelineAdminResources.Names.DataStream_StreamType_Redis, typeof(PipelineAdminResources))]
        Redis,
        //[EnumLabel(DataStream.StreamType_DataLake, PipelineAdminResources.Names.DataStream_StreamType_DataLake, typeof(PipelineAdminResources))]
        //AzureDataLake,
        [EnumLabel(DataStream.StreamType_SQLServer, PipelineAdminResources.Names.DataStream_StreamType_SQLServer, typeof(PipelineAdminResources))]
        SQLServer,
        [EnumLabel(DataStream.StreamType_GeoSpatial, PipelineAdminResources.Names.DataStream_StreamType_GeoSpatial, typeof(PipelineAdminResources))]
        GeoSpatial

    }

    public enum DateStorageFormats
    {
        [EnumLabel(DataStream.StreamType_Epcoh, PipelineAdminResources.Names.DataStream_DateStorageFormat_Type_ISO8601, typeof(PipelineAdminResources))]
        Epoch,
        [EnumLabel(DataStream.StreamType_ISO8601, PipelineAdminResources.Names.DataStream_DateStorageFormat_Type_Epoch, typeof(PipelineAdminResources))]
        ISO8601
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.DataStream_Title, PipelineAdminResources.Names.DataStream_Help, 
        PipelineAdminResources.Names.DataStream_Description, 
        EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources),
        GetUrl: "/api/datastream/{id}", SaveUrl: "/api/datastream", GetListUrl: "/api/datastreams", FactoryUrl: "/api/datastream/factory", DeleteUrl: "/api/datastream/{id}")]
    public class DataStream : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor, IFormConditionalFields
    {
        public const string StreamType_AWS_S3 = "awss3";
        public const string StreamType_AWS_ElasticSearch = "awselasticsearch";
        public const string StreamType_AzureBlob = "azureblob";
        public const string StreamType_AzureBlob_Managed = "azureblobmanaged";
        public const string StreamType_AzureTableStorage = "azuretablestorage";
        public const string StreamType_AzureEventHub = "azureeventhub";
        public const string StreamType_AzureTableStorage_Managed = "azuretablestoragemanaged";
        public const string StreamType_SQLServer = "sqlserver";
        public const string StreamType_PostgreSQL = "postgresql";
        public const string StreamType_PointArrayStorage = "pointarray";
        public const string StreamType_Redis = "redis";
        public const string StreamType_GeoSpatial = "geospatial";

        //public const string StreamType_DataLake = "azuredatalake";


        public const string StreamType_Epcoh = "expoch";
        public const string StreamType_ISO8601 = "iso8601";

        public DataStream()
        {
            Fields = new List<DataStreamField>();
            TimestampFieldName = "timeStamp";
            DeviceIdFieldName = "deviceId";
            DateStorageFormat = EntityHeader<DateStorageFormats>.Create(DateStorageFormats.ISO8601);
        }

        public string AWSSecretKeySecureId { get; set; }
        public string AzureAccessKeySecureId { get; set; }
        public string DBPasswordSecureId { get; set; }
        public string RedisPasswordSecureId { get; set; }

        public override string ModuleType => PipelineModuleType_DataStream;


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_StreamType, EnumType: typeof(DataStreamTypes), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.DataStream_StreamType_Select, IsRequired: true)]
        public EntityHeader<DataStreamTypes> StreamType { get; set; }


        #region Data Formatting Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_TimeStampFieldName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9_]{2,64}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_TimeStamp_InvalidFormat,
            HelpResource: PipelineAdminResources.Names.DataStream_TimeStampFieldName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string TimestampFieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DeviceIdFieldName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9_]{2,64}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_DeviceId_InvalidFormat,
            HelpResource: PipelineAdminResources.Names.DataStream_DeviceIdFieldName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string DeviceIdFieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DateStorageFormat, EnumType: (typeof(DateStorageFormats)), HelpResource: PipelineAdminResources.Names.DataStream_DateStorageFormat,
            FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.DataStreamField_DataType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public EntityHeader<DateStorageFormats> DateStorageFormat { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataaStream_SummaryLevel, HelpResource: PipelineAdminResources.Names.DataStream_SummaryLevel_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool SummaryLevelData { get; set; }

        #endregion

        #region Amazon Properties
        #region AWS S3 Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_S3_BucketName, ValidationRegEx: @"^([a-z]|(\d(?!\d{0,2}\.\d{1,3}\.\d{1,3}\.\d{1,3})))([a-z\d]|(\.(?!(\.|-)))|(-(?!\.))){1,61}[a-z\d]$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidBucketName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string S3BucketName { get; set; }
        #endregion

        #region AWS Elastic Search Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESDomainName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ElasticSearchDomainName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESIndexName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9]{2,64}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_ESIndexName_Invalid, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ElasticSearchIndexName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESTypeName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z0-9]{2,64}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_ESTypeNameInvalid, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ElasticSearchTypeName { get; set; }
        #endregion

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSAccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsAccessKey { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSRegion, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsRegion { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSSecretKey, HelpResource: PipelineAdminResources.Names.DataStream_AWSSecretKey_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AwsSecretKey { get; set; }
        #endregion

        #region Azure Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureStorageName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureStorageAccountName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureAccessKey, HelpResource: PipelineAdminResources.Names.DataStream_AzureAccessKeyHelp, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureAccessKey { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_TableStorageName, ValidationRegEx: @"^[a-zA-Z0-9]{3,63}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_TableStorage_InvalidName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureTableStorageName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_BlobStoragePath, ValidationRegEx: @"^[a-z0-9]+(-[a-z0-9]+)*$", FieldType: FieldTypes.Text,
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_BlobStorage_InvalidName, ResourceType: typeof(PipelineAdminResources))]
        public string AzureBlobStorageContainerName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureEventHubName, FieldType: FieldTypes.Text, ValidationRegEx: @"^[a-zA-Z0-9][a-zA-Z0-9.\-_]{5,49}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidEHName, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureEventHubName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureEventHubPath, FieldType: FieldTypes.Text, ValidationRegEx: @"^[a-zA-Z0-9][a-zA-Z0-9.\-_]{0,127}$",
             RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidEHPathName, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureEventHubEntityPath { get; set; }
        #endregion

        #region RDBMSProperties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbUserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbUserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbPassword, HelpResource: PipelineAdminResources.Names.DataStream_DbPassword_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbSchema, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbSchema { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbValidateSchema, HelpResource: PipelineAdminResources.Names.DataStream_DbValidateSchema_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public bool DbValidateSchema { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbURL, ValidationRegEx: @"[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_DbUrl_InvalidUrl, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbURL { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_TableName, ValidationRegEx: @"^[a-zA-Z0-9_]{0,127}$", FieldType: FieldTypes.Text,
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidTableName, ResourceType: typeof(PipelineAdminResources))]
        public string DbTableName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AutoCreateTable, HelpResource: PipelineAdminResources.Names.DataStream_AutoCreateTable_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool AutoCreateSQLTable { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_CreateTableDDL, HelpResource: PipelineAdminResources.Names.DataStream_CreateTableDDL_Help, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string CreateTableDDL { get; set; }

        #endregion

        #region Redis Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_RedisPassword, HelpResource: PipelineAdminResources.Names.DataStream_RedisPassword_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RedisPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_RedisServers, HelpResource: PipelineAdminResources.Names.DataStream_RedisServers_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RedisServerUris { get; set; }

        #endregion

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_SummaryData, HelpResource: PipelineAdminResources.Names.DataStream_SummaryData_Help,  FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public bool IsSummaryLevelData { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_Fields, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources))]
        public List<DataStreamField> Fields { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_SharedConnection, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.DataStream_SharedConnection_Select)]
        public EntityHeader<SharedConnection> SharedConnection { get; set; }


        public new DataStreamSummary CreateSummary()
        {
            return new DataStreamSummary()
            {
                Description = Description,
                Id = Id,
                Name = Name,
                IsPublic = IsPublic,
                Key = Key,
                StreamType = StreamType.Text,
                StreamTypeKey = StreamType.Id,
                DeviceIdFieldName = DeviceIdFieldName,
                TimestampFieldName = TimestampFieldName
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = { nameof(ElasticSearchDomainName), nameof(ElasticSearchIndexName), nameof(ElasticSearchTypeName),
                    nameof(AwsAccessKey), nameof(AwsSecretKey), nameof(RedisPassword), nameof(RedisServerUris), nameof(DbName),
                    nameof(DbPassword), nameof(DbUserName), nameof(DbTableName), nameof(DbURL), nameof(DbSchema), nameof(CreateTableDDL),
                    nameof(DbValidateSchema), nameof(S3BucketName), nameof(AzureAccessKey), nameof(AzureStorageAccountName), nameof(DateStorageFormat),
                    nameof(AzureBlobStorageContainerName), nameof(AzureEventHubEntityPath), nameof(AzureEventHubName), nameof(AzureTableStorageName),
                    nameof(AutoCreateSQLTable), nameof(DeviceIdFieldName), nameof(TimestampFieldName) },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                        Field = nameof(StreamType),
                        Value = StreamType_AWS_ElasticSearch,
                        VisibleFields =  {nameof(ElasticSearchDomainName), nameof(ElasticSearchIndexName), nameof(ElasticSearchTypeName), nameof(AwsAccessKey), nameof(AwsSecretKey)}
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
                nameof(StreamType),
                nameof(SharedConnection),
                nameof(IsSummaryLevelData),
                nameof(TimestampFieldName),
                nameof(DeviceIdFieldName),
                nameof(DateStorageFormat),

                nameof(AwsAccessKey),
                nameof(AwsSecretKey),

                nameof(AzureStorageAccountName),
                nameof(AzureAccessKey),

                nameof(AzureBlobStorageContainerName),

                nameof(AzureEventHubName),
                nameof(AzureEventHubEntityPath),

                nameof(AzureTableStorageName),

                nameof(DbURL),
                nameof(DbName),
                nameof(DbUserName),
                nameof(DbPassword),
                nameof(DbSchema),
                nameof(DbValidateSchema),
                nameof(DbTableName),
                nameof(AutoCreateSQLTable),
                nameof(CreateTableDDL),

                nameof(RedisServerUris),
                nameof(RedisPassword),

                nameof(ElasticSearchDomainName),
                nameof(ElasticSearchIndexName),
                nameof(ElasticSearchTypeName),

                nameof(S3BucketName),
                nameof(Description)
            };
        }

        [PreValidation]
        public void PreValidate(Actions action)
        {
            if (!EntityHeader.IsNullOrEmpty(StreamType))
            {
                if (!EntityHeader.IsNullOrEmpty(SharedConnection))
                {
                    AzureAccessKey = null;
                    AzureAccessKeySecureId = null;
                    AzureStorageAccountName = null;
                    DbName = null;
                    DbPassword = null;
                    DBPasswordSecureId = null;
                    DbURL = null;
                    DbUserName = null;
                    AwsRegion = null;
                    AwsSecretKey = null;
                    AwsAccessKey = null;
                    AWSSecretKeySecureId = null;
                }

                if (StreamType.Value == DataStreamTypes.AzureTableStorage_Managed && action == Actions.Create)
                {
                    AzureTableStorageName = $"DataStream{OwnerOrganization.Id}{Key}";
                }

                if (StreamType.Value != DataStreamTypes.AzureBlob &&
                    StreamType.Value != DataStreamTypes.AzureTableStorage &&
                    StreamType.Value != DataStreamTypes.AzureEventHub &&
                    StreamType.Value != DataStreamTypes.AzureTableStorage_Managed)
                {
                    AzureStorageAccountName = null;
                    AzureAccessKey = null;
                    AzureAccessKeySecureId = null;
                }

                if (StreamType.Value != DataStreamTypes.AWSS3 && StreamType.Value != DataStreamTypes.AWSElasticSearch)
                {
                    AwsRegion = null;
                    AwsSecretKey = null;
                    AwsAccessKey = null;
                    AWSSecretKeySecureId = null;
                }

                if (StreamType.Value != DataStreamTypes.SQLServer &&
                    StreamType.Value != DataStreamTypes.Postgresql &&
                    StreamType.Value != DataStreamTypes.GeoSpatial &&
                    StreamType.Value != DataStreamTypes.PointArrayStorage)
                {
                    DbName = null;
                    DbPassword = null;
                    DBPasswordSecureId = null;
                    DbSchema = null;
                    DbTableName = null;
                    DbURL = null;
                    DbUserName = null;
                    DbValidateSchema = false;
                }

                if(StreamType.Value != DataStreamTypes.Redis)
                {
                    RedisPassword = null;
                    RedisPasswordSecureId = null;
                    RedisServerUris = null;
                }

                if (StreamType.Value != DataStreamTypes.AzureEventHub)
                {
                    AzureEventHubEntityPath = null;
                    AzureEventHubName = null;
                }

                if (StreamType.Value != DataStreamTypes.AzureTableStorage &&
                    StreamType.Value != DataStreamTypes.AzureTableStorage_Managed)
                {
                    AzureTableStorageName = null;
                }

                if (StreamType.Value != DataStreamTypes.AzureBlob)
                {
                    AzureBlobStorageContainerName = null;
                }
            }
        }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            /* full validation will happen somewhere else, if not valid, just skip */
            if (result.Errors.Count > 0) return;

            if (Fields.Select(fld => fld.Key).Distinct().Count() != Fields.Count) result.Errors.Add(new ErrorMessage("Keys on fields must be unique."));
            if (Fields.Select(fld => fld.FieldName).Distinct().Count() != Fields.Count) result.Errors.Add(new ErrorMessage("Field Names on fields must be unique."));

            if(IsSummaryLevelData && !Fields.Select(fld=>fld.FieldName.ToLower() == "period" && fld.FieldType.Value == DeviceAdmin.Models.ParameterTypes.String).Any())
            {
                result.Errors.Add(new ErrorMessage("If the data stream is summary level data, it must contain a string field with the name period."));
            }

            #region AWS Types
            if (StreamType.Value == DataStreamTypes.AWSElasticSearch ||
                StreamType.Value == DataStreamTypes.AWSS3)
            {
                if (EntityHeader.IsNullOrEmpty(SharedConnection))
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
                        if (StreamType.Value == DataStreamTypes.AWSS3)
                        {
                            if (!AWSUtils.AWSS3Regions.Contains(AwsRegion)) result.Errors.Add(new ErrorMessage($"Invalid AWS Region, Region [{AwsRegion}] could not be found."));
                        }
                        else if (StreamType.Value == DataStreamTypes.AWSElasticSearch)
                        {
                            if (!AWSUtils.AWSESRegions.Contains(AwsRegion)) result.Errors.Add(new ErrorMessage($"Invalid AWS Region, Region [{AwsRegion}] could not be found."));
                        }
                    }
                }

                if (StreamType.Value == DataStreamTypes.AWSS3 && string.IsNullOrEmpty(S3BucketName)) result.Errors.Add(new ErrorMessage("Please Provide an S3 Bucket Name."));

                if (StreamType.Value == DataStreamTypes.AWSElasticSearch)
                {
                    if (string.IsNullOrEmpty(ElasticSearchDomainName))
                    {
                        result.Errors.Add(new ErrorMessage("Elastic Search Domain Name is required."));
                    }
                    else
                    {
                        if (!Uri.IsWellFormedUriString(ElasticSearchDomainName, UriKind.Absolute)) result.Errors.Add(new ErrorMessage(PipelineAdminResources.DataStream_ESDomainName_Invalid));
                    }
                    if (string.IsNullOrEmpty(ElasticSearchIndexName)) result.Errors.Add(new ErrorMessage("Elastic Search Index Name is required."));
                    if (string.IsNullOrEmpty(ElasticSearchTypeName)) result.Errors.Add(new ErrorMessage("Elastic Search Type Name is required."));
                }
            }
            #endregion

            if (StreamType.Value == DataStreamTypes.Redis && EntityHeader.IsNullOrEmpty(SharedConnection))
            {
                if(String.IsNullOrEmpty(RedisServerUris))
                {
                    result.Errors.Add(new ErrorMessage("REDIS Urls are required for a redis type of connection."));
                }

                if ((action == Actions.Create) && String.IsNullOrEmpty(RedisPassword))
                {
                    if (String.IsNullOrEmpty(RedisPassword)) result.Errors.Add(new ErrorMessage("Missing Passwrod for REDIS connection."));
                }
                else if (String.IsNullOrEmpty(RedisPassword) && String.IsNullOrEmpty(RedisPasswordSecureId))
                {
                    result.Errors.Add(new ErrorMessage("RedisPassword Password or RedisPasswordSecureId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you should return the original secret key id."));
                }
            }

            if (StreamType.Value == DataStreamTypes.SQLServer ||
                StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (EntityHeader.IsNullOrEmpty(SharedConnection))
                {
                    if (string.IsNullOrEmpty(DbURL)) result.Errors.Add(new ErrorMessage("URL of database server is required for a database data stream."));
                    if (string.IsNullOrEmpty(DbUserName)) result.Errors.Add(new ErrorMessage("Database User Name is required for a database data stream."));
                    if (string.IsNullOrEmpty(DbName)) result.Errors.Add(new ErrorMessage("Database Name is required for a database data stream."));

                    if (string.IsNullOrEmpty(DbPassword) && string.IsNullOrEmpty(DBPasswordSecureId))
                    {
                        result.Errors.Add(new ErrorMessage("Database Password or SecretKeyId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you should return the original secret key id."));
                    }
                }

                if (string.IsNullOrEmpty(DbTableName)) result.Errors.Add(new ErrorMessage("Database Table Name is required for a database data stream."));
            }

            if (StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (string.IsNullOrEmpty(DbSchema)) result.Errors.Add(new ErrorMessage("Database Schema is required for a Postgres database."));
            }

            #region Azure Type
            if (StreamType.Value == DataStreamTypes.AzureTableStorage)
            {
                if (string.IsNullOrEmpty(AzureTableStorageName)) result.Errors.Add(new ErrorMessage("Table Name for Table Storage Account is a Required Field"));
            }

            if (StreamType.Value == DataStreamTypes.AzureBlob)
            {
                if (string.IsNullOrEmpty(AzureBlobStorageContainerName)) result.Errors.Add(new ErrorMessage("Name of Azure Blob Container is required."));
            }

            if (StreamType.Value == DataStreamTypes.AzureEventHub)
            {
                if (string.IsNullOrEmpty(AzureEventHubName)) result.Errors.Add(new ErrorMessage("Name of event hub is a required field."));
                if (string.IsNullOrEmpty(AzureEventHubEntityPath)) result.Errors.Add(new ErrorMessage("Entity path on event hub is a required field."));
            }

            if ((StreamType.Value == DataStreamTypes.AzureBlob || StreamType.Value == DataStreamTypes.AzureTableStorage) && EntityHeader.IsNullOrEmpty(SharedConnection))
            {
                if (string.IsNullOrEmpty(AzureStorageAccountName)) result.Errors.Add(new ErrorMessage("Name of Azure Storage Account is Required."));
            }

            if ((StreamType.Value == DataStreamTypes.AzureEventHub || StreamType.Value == DataStreamTypes.AzureBlob || StreamType.Value == DataStreamTypes.AzureTableStorage) && 
                EntityHeader.IsNullOrEmpty(SharedConnection))
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
    }

    public class DataStreamSummary : SummaryData
    {
        public string StreamType { get; set; }
        public string StreamTypeKey { get; set; }

        public String TimestampFieldName { get; set; }
        public String DeviceIdFieldName { get; set; }
    }
}
