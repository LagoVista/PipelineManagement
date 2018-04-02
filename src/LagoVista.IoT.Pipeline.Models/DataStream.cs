using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Pipeline.Models.Resources;
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
        [EnumLabel(DataStream.StreamType_AzureEventHub, PipelineAdminResources.Names.DataStream_StreamType_AzureEventHub, typeof(PipelineAdminResources))]
        AzureEventHub,
        [EnumLabel(DataStream.StreamType_AzureTableStorage, PipelineAdminResources.Names.DataStream_StreamType_TableStorage, typeof(PipelineAdminResources))]
        AzureTableStorage,
        [EnumLabel(DataStream.StreamType_AzureTableStorage_Managed, PipelineAdminResources.Names.DataStream_StreamType_TableStorage_Managed, typeof(PipelineAdminResources))]
        AzureTableStorage_Managed,
        //[EnumLabel(DataStream.StreamType_DataLake, PipelineAdminResources.Names.DataStream_StreamType_DataLake, typeof(PipelineAdminResources))]
        //AzureDataLake,
        [EnumLabel(DataStream.StreamType_SQLServer, PipelineAdminResources.Names.DataStream_StreamType_SQLServer, typeof(PipelineAdminResources))]
        SQLServer


    }

    public enum DateStorageFormats
    {
        [EnumLabel(DataStream.StreamType_Epcoh, PipelineAdminResources.Names.DataStream_DateStorageFormat_Type_ISO8601, typeof(PipelineAdminResources))]
        Epoch,
        [EnumLabel(DataStream.StreamType_ISO8601, PipelineAdminResources.Names.DataStream_DateStorageFormat_Type_Epoch, typeof(PipelineAdminResources))]
        ISO8601
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.DataStream_Title, PipelineAdminResources.Names.DataStream_Help, PipelineAdminResources.Names.DataStream_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources))]
    public class DataStream : PipelineModuleConfiguration, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IFormDescriptor
    {
        public const string StreamType_AWS_S3 = "awss3";
        public const string StreamType_AWS_ElasticSearch = "awselasticsearch";
        public const string StreamType_AzureBlob = "azureblob";
        public const string StreamType_AzureBlob_Managed = "azureblobmanaged";
        public const string StreamType_AzureTableStorage = "azuretablestorage";
        public const string StreamType_AzureEventHub = "azureeventhub";
        public const string StreamType_AzureTableStorage_Managed = "azuretablestoragemanaged";
        public const string StreamType_SQLServer = "sqlserver";

        //public const string StreamType_DataLake = "azuredatalake";


        public const string StreamType_Epcoh = "expoch";
        public const string StreamType_ISO8601 = "iso8601";

        public DataStream()
        {
            Fields = new List<DataStreamField>();
            TimeStampFieldName = "timeStamp";
            DeviceIdFieldName = "deviceId";
            DateStorageFormat = EntityHeader<DateStorageFormats>.Create(DateStorageFormats.ISO8601);
        }

        public string AWSSecretKeySecureId { get; set; }
        public string AzureAccessKeySecureId { get; set; }
        public string DBPasswordSecureId { get; set; }



        public override string ModuleType => PipelineModuleType_DataStream;


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_StreamType, EnumType: typeof(DataStreamTypes), FieldType: FieldTypes.Picker, RegExValidationMessageResource: PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.DataStream_StreamType_Select, IsRequired: true)]
        public EntityHeader<DataStreamTypes> StreamType { get; set; }


        #region Data Formatting Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_TimeStampFieldName, ValidationRegEx: @"^[\p{L}_][\p{L}\p{N}_]{3,32}$", HelpResource: PipelineAdminResources.Names.DataStream_TimeStampFieldName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string TimeStampFieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DeviceIdFieldName, ValidationRegEx: @"^[\p{L}_][\p{L}\p{N}_]{3,32}$", HelpResource: PipelineAdminResources.Names.DataStream_DeviceIdFieldName_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public string DeviceIdFieldName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DateStorageFormat, EnumType: (typeof(DateStorageFormats)), HelpResource: PipelineAdminResources.Names.DataStream_DateStorageFormat,
            FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.DataStreamField_DataType_Select, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public EntityHeader<DateStorageFormats> DateStorageFormat { get; set; }
        #endregion


        #region Amazon Properties
        #region AWS S3 Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_S3_BucketName, ValidationRegEx: @"^([a-z]|(\d(?!\d{0,2}\.\d{1,3}\.\d{1,3}\.\d{1,3})))([a-z\d]|(\.(?!(\.|-)))|(-(?!\.))){1,61}[a-z\d]$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidBucketName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string S3BucketName { get; set; }
        #endregion

        #region AWS Elastic Search Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESDomainName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z-]{2,100}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_ESDomainName_Invalid, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ESDomainName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESIndexName, ValidationRegEx: @"^[a-zA-Z][a-zA-Z@$#_-]{2,100}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_ESIndexName_Invalid, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ESIndexName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_ESTypeName, ValidationRegEx: @"^[a-zA-Z0-9_-]{3,100}$",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_ESTypeNameInvalid, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ESTypeName { get; set; }
        #endregion

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSAccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AWSAccessKey { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSRegion, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AWSRegion { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AWSSecretKey, HelpResource: PipelineAdminResources.Names.DataStream_AWSSecretKey_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AWSSecretKey { get; set; }
        #endregion

        #region Azure Properties
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureAccountId, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string AzureAccountId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AzureAccessKey, HelpResource: PipelineAdminResources.Names.DataStream_AzureAccessKeyHelp, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
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

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbPassword, HelpResource: PipelineAdminResources.Names.DataStream_DbPassword_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbPassword { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbValidateSchema, HelpResource: PipelineAdminResources.Names.DataStream_DbValidateSchema_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public bool DbValidateSchema { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_DbURL, ValidationRegEx: @"[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_DbUrl_InvalidUrl, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string DbURL { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_TableName, ValidationRegEx: @"^[\p{L}_][\p{L}\p{N}@$#_]{0,127}$", FieldType: FieldTypes.Text,
            RegExValidationMessageResource: PipelineAdminResources.Names.DataStream_InvalidTableName, ResourceType: typeof(PipelineAdminResources))]
        public string DbTableName { get; set; }

        /* Currently not implemented */
        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_AutoCreateTable, HelpResource: PipelineAdminResources.Names.DataStream_AutoCreateTable_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool AutoCreateSQLTable { get; set; }
        #endregion


        [FormField(LabelResource: PipelineAdminResources.Names.DataStream_Fields, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<DataStreamField> Fields { get; set; }


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
                StreamTypeKey = StreamType.Id
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(DataStream.Name),
                nameof(DataStream.Key),
                nameof(DataStream.Description),
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            /* full validation will happen somewhere else, if not valid, just skip */
            if (result.Errors.Count > 0) return;

            //TODO: Need to add localized error messages
            if (Fields.Select(fld => fld.Key).Distinct().Count() != Fields.Count) result.Errors.Add(new ErrorMessage("Keys on fields must be unique."));
            if (Fields.Select(fld => fld.FieldName).Distinct().Count() != Fields.Count) result.Errors.Add(new ErrorMessage("Field Names on fields must be unique."));

            if (StreamType.Value != DataStreamTypes.AzureBlob && 
                StreamType.Value != DataStreamTypes.AzureTableStorage &&
                StreamType.Value != DataStreamTypes.AzureEventHub &&
                StreamType.Value != DataStreamTypes.AzureTableStorage_Managed)
            {
                AzureAccessKey = null;
                AzureAccessKeySecureId = null;
            }

            if (StreamType.Value != DataStreamTypes.AWSS3 && StreamType.Value != DataStreamTypes.AWSElasticSearch)
            {
                AWSRegion = null;
                AWSSecretKey = null;
                AWSAccessKey = null;
                AWSSecretKeySecureId = null;
            }

            if (StreamType.Value != DataStreamTypes.SQLServer)
            {
                DbName = null;
                DbPassword = null;
                DBPasswordSecureId = null;
                DbTableName = null;
                DbURL = null;
                DbUserName = null;
                DbValidateSchema = false;
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

            #region AWS Types
            if (StreamType.Value == DataStreamTypes.AWSElasticSearch ||
                StreamType.Value == DataStreamTypes.AWSS3)
            {
                if (string.IsNullOrEmpty(AWSAccessKey)) result.Errors.Add(new ErrorMessage("AWS Acceess Key is required for AWS Data Streams."));
                if ((action == Actions.Update) && string.IsNullOrEmpty(AWSSecretKey) && string.IsNullOrEmpty(AWSSecretKeySecureId)) result.Errors.Add(new ErrorMessage("AWS Secret Key or SecretKeyId are required for AWS Data Streams, if you are updating and replacing the key you should provide the new AWSSecretKey otherwise you could return the original secret key id."));
                if ((action == Actions.Create) && string.IsNullOrEmpty(AWSSecretKey)) result.Errors.Add(new ErrorMessage("AWS Secret Key is required for AWS Data Streams (it will be encrypted at rest)."));

                if (StreamType.Value == DataStreamTypes.AWSS3 && string.IsNullOrEmpty(S3BucketName)) result.Errors.Add(new ErrorMessage("Please Provide an S3 Bucket Name."));

                if (StreamType.Value == DataStreamTypes.AWSElasticSearch)
                {
                    if (string.IsNullOrEmpty(ESDomainName)) result.Errors.Add(new ErrorMessage("Elastic Search Domain Name is required."));
                    if (string.IsNullOrEmpty(ESIndexName)) result.Errors.Add(new ErrorMessage("Elastic Search Index Name is required."));
                    if (string.IsNullOrEmpty(ESTypeName)) result.Errors.Add(new ErrorMessage("Elastic Search Type Name is required."));
                }

                if (string.IsNullOrEmpty(AWSRegion))
                {
                    result.Errors.Add(new ErrorMessage("AWS Region is a required field for AWS Data Streams."));
                }
                else
                {
                    if (StreamType.Value == DataStreamTypes.AWSS3)
                    {
                        if (!AWSUtils.AWSS3Regions.Contains(AWSRegion)) result.Errors.Add(new ErrorMessage($"Invalid AWS Region, Region [{AWSRegion}] could not be found."));
                    }
                    else if (StreamType.Value == DataStreamTypes.AWSElasticSearch)
                    {
                        if (!AWSUtils.AWSS3Regions.Contains(AWSRegion)) result.Errors.Add(new ErrorMessage($"Invalid AWS  Region, Region [{AWSRegion}] could not be found."));
                    }
                }
            }
            #endregion

            if (StreamType.Value == DataStreamTypes.SQLServer)
            {
                if (string.IsNullOrEmpty(DbURL)) result.Errors.Add(new ErrorMessage("URL of database server is required for a database data stream."));
                if (string.IsNullOrEmpty(DbUserName)) result.Errors.Add(new ErrorMessage("Database User Name is required for a database data stream."));
                if (string.IsNullOrEmpty(DbName)) result.Errors.Add(new ErrorMessage("Database Name is required for a database data stream."));
                if (string.IsNullOrEmpty(DbTableName)) result.Errors.Add(new ErrorMessage("Database Table Name is required for a database data stream."));

                if ((action == Actions.Create) && string.IsNullOrEmpty(DbPassword)) result.Errors.Add(new ErrorMessage("Database Password is required for a database data streams"));
                if ((action == Actions.Update) && string.IsNullOrEmpty(DbPassword) && string.IsNullOrEmpty(DBPasswordSecureId)) result.Errors.Add(new ErrorMessage("Database Password or SecretKeyId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id."));
            }


            #region Azure Type
            if (StreamType.Value == DataStreamTypes.AzureTableStorage)                
            {
                if (string.IsNullOrEmpty(AzureTableStorageName)) result.Errors.Add(new ErrorMessage("Table Name for Table Storage Account is a Required Field"));
            }

            if (StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                AzureTableStorageName = $"DataStream{OwnerOrganization.Id}{Key}";
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

            if (StreamType.Value == DataStreamTypes.AzureEventHub || StreamType.Value == DataStreamTypes.AzureBlob || StreamType.Value == DataStreamTypes.AzureTableStorage)
            {
                if ((action == Actions.Create) && string.IsNullOrEmpty(AzureAccessKey)) result.Errors.Add(new ErrorMessage("Azure Access Key is required"));
                if ((action == Actions.Update) && string.IsNullOrEmpty(AzureAccessKey) && string.IsNullOrEmpty(AzureAccessKeySecureId)) result.Errors.Add(new ErrorMessage("Azure Access Key or SecretKeyId are required for azure resources, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id."));
            }
            #endregion

        }
    }

    public class DataStreamSummary : SummaryData
    {
        public string StreamType { get; set; }
        public string StreamTypeKey { get; set; }
    }
}
