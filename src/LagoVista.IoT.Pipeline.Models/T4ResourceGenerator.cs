/*3/18/2023 5:56:33 AM*/
using System.Globalization;
using System.Reflection;

//Resources:PipelineAdminResources:AppCache_CacheType
namespace LagoVista.IoT.Pipeline.Models.Resources
{
	public class PipelineAdminResources
	{
        private static global::System.Resources.ResourceManager _resourceManager;
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static global::System.Resources.ResourceManager ResourceManager 
		{
            get 
			{
                if (object.ReferenceEquals(_resourceManager, null)) 
				{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LagoVista.IoT.Pipeline.Models.Resources.PipelineAdminResources", typeof(PipelineAdminResources).GetTypeInfo().Assembly);
                    _resourceManager = temp;
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static string GetResourceString(string key, params string[] tokens)
		{
			var culture = CultureInfo.CurrentCulture;;
            var str = ResourceManager.GetString(key, culture);

			for(int i = 0; i < tokens.Length; i += 2)
				str = str.Replace(tokens[i], tokens[i+1]);
										
            return str;
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
		/*
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static HtmlString GetResourceHtmlString(string key, params string[] tokens)
		{
			var str = GetResourceString(key, tokens);
							
			if(str.StartsWith("HTML:"))
				str = str.Substring(5);

			return new HtmlString(str);
        }*/
		
		public static string AppCache_CacheType { get { return GetResourceString("AppCache_CacheType"); } }
//Resources:PipelineAdminResources:AppCache_CacheType_LocalInMemory

		public static string AppCache_CacheType_LocalInMemory { get { return GetResourceString("AppCache_CacheType_LocalInMemory"); } }
//Resources:PipelineAdminResources:AppCache_CacheType_NuvIoT

		public static string AppCache_CacheType_NuvIoT { get { return GetResourceString("AppCache_CacheType_NuvIoT"); } }
//Resources:PipelineAdminResources:AppCache_CacheType_Redis

		public static string AppCache_CacheType_Redis { get { return GetResourceString("AppCache_CacheType_Redis"); } }
//Resources:PipelineAdminResources:AppCache_Description

		public static string AppCache_Description { get { return GetResourceString("AppCache_Description"); } }
//Resources:PipelineAdminResources:AppCache_Help

		public static string AppCache_Help { get { return GetResourceString("AppCache_Help"); } }
//Resources:PipelineAdminResources:AppCache_InitializationValues

		public static string AppCache_InitializationValues { get { return GetResourceString("AppCache_InitializationValues"); } }
//Resources:PipelineAdminResources:AppCache_Password

		public static string AppCache_Password { get { return GetResourceString("AppCache_Password"); } }
//Resources:PipelineAdminResources:AppCache_Password_Help

		public static string AppCache_Password_Help { get { return GetResourceString("AppCache_Password_Help"); } }
//Resources:PipelineAdminResources:AppCache_SelectCacheType

		public static string AppCache_SelectCacheType { get { return GetResourceString("AppCache_SelectCacheType"); } }
//Resources:PipelineAdminResources:AppCache_Title

		public static string AppCache_Title { get { return GetResourceString("AppCache_Title"); } }
//Resources:PipelineAdminResources:AppCache_Uri

		public static string AppCache_Uri { get { return GetResourceString("AppCache_Uri"); } }
//Resources:PipelineAdminResources:AppCache_Uri_Help

		public static string AppCache_Uri_Help { get { return GetResourceString("AppCache_Uri_Help"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Description

		public static string ApplicationCacheValue_Description { get { return GetResourceString("ApplicationCacheValue_Description"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Help

		public static string ApplicationCacheValue_Help { get { return GetResourceString("ApplicationCacheValue_Help"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Key

		public static string ApplicationCacheValue_Key { get { return GetResourceString("ApplicationCacheValue_Key"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Title

		public static string ApplicationCacheValue_Title { get { return GetResourceString("ApplicationCacheValue_Title"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Value

		public static string ApplicationCacheValue_Value { get { return GetResourceString("ApplicationCacheValue_Value"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Value_Number

		public static string ApplicationCacheValue_Value_Number { get { return GetResourceString("ApplicationCacheValue_Value_Number"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Value_String

		public static string ApplicationCacheValue_Value_String { get { return GetResourceString("ApplicationCacheValue_Value_String"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Value_Type

		public static string ApplicationCacheValue_Value_Type { get { return GetResourceString("ApplicationCacheValue_Value_Type"); } }
//Resources:PipelineAdminResources:ApplicationCacheValue_Value_Type_Select

		public static string ApplicationCacheValue_Value_Type_Select { get { return GetResourceString("ApplicationCacheValue_Value_Type_Select"); } }
//Resources:PipelineAdminResources:Common_Description

		public static string Common_Description { get { return GetResourceString("Common_Description"); } }
//Resources:PipelineAdminResources:Common_IsPublic

		public static string Common_IsPublic { get { return GetResourceString("Common_IsPublic"); } }
//Resources:PipelineAdminResources:Common_IsPublic_Help

		public static string Common_IsPublic_Help { get { return GetResourceString("Common_IsPublic_Help"); } }
//Resources:PipelineAdminResources:Common_Key

		public static string Common_Key { get { return GetResourceString("Common_Key"); } }
//Resources:PipelineAdminResources:Common_Key_Help

		public static string Common_Key_Help { get { return GetResourceString("Common_Key_Help"); } }
//Resources:PipelineAdminResources:Common_Key_Validation

		public static string Common_Key_Validation { get { return GetResourceString("Common_Key_Validation"); } }
//Resources:PipelineAdminResources:Common_Name

		public static string Common_Name { get { return GetResourceString("Common_Name"); } }
//Resources:PipelineAdminResources:Common_Notes

		public static string Common_Notes { get { return GetResourceString("Common_Notes"); } }
//Resources:PipelineAdminResources:Common_Script

		public static string Common_Script { get { return GetResourceString("Common_Script"); } }
//Resources:PipelineAdminResources:Common_UniqueId

		public static string Common_UniqueId { get { return GetResourceString("Common_UniqueId"); } }
//Resources:PipelineAdminResources:Connection_Select_Type

		public static string Connection_Select_Type { get { return GetResourceString("Connection_Select_Type"); } }
//Resources:PipelineAdminResources:Connection_Type_AMQP

		public static string Connection_Type_AMQP { get { return GetResourceString("Connection_Type_AMQP"); } }
//Resources:PipelineAdminResources:Connection_Type_AzureEventHub

		public static string Connection_Type_AzureEventHub { get { return GetResourceString("Connection_Type_AzureEventHub"); } }
//Resources:PipelineAdminResources:Connection_Type_AzureIoTHub

		public static string Connection_Type_AzureIoTHub { get { return GetResourceString("Connection_Type_AzureIoTHub"); } }
//Resources:PipelineAdminResources:Connection_Type_AzureServiceBus

		public static string Connection_Type_AzureServiceBus { get { return GetResourceString("Connection_Type_AzureServiceBus"); } }
//Resources:PipelineAdminResources:Connection_Type_Custom

		public static string Connection_Type_Custom { get { return GetResourceString("Connection_Type_Custom"); } }
//Resources:PipelineAdminResources:Connection_Type_FTP

		public static string Connection_Type_FTP { get { return GetResourceString("Connection_Type_FTP"); } }
//Resources:PipelineAdminResources:Connection_Type_Kafka

		public static string Connection_Type_Kafka { get { return GetResourceString("Connection_Type_Kafka"); } }
//Resources:PipelineAdminResources:Connection_Type_MQTT_Broker

		public static string Connection_Type_MQTT_Broker { get { return GetResourceString("Connection_Type_MQTT_Broker"); } }
//Resources:PipelineAdminResources:Connection_Type_MQTT_Client

		public static string Connection_Type_MQTT_Client { get { return GetResourceString("Connection_Type_MQTT_Client"); } }
//Resources:PipelineAdminResources:Connection_Type_POP3Server

		public static string Connection_Type_POP3Server { get { return GetResourceString("Connection_Type_POP3Server"); } }
//Resources:PipelineAdminResources:Connection_Type_Redis

		public static string Connection_Type_Redis { get { return GetResourceString("Connection_Type_Redis"); } }
//Resources:PipelineAdminResources:Connection_Type_Rest

		public static string Connection_Type_Rest { get { return GetResourceString("Connection_Type_Rest"); } }
//Resources:PipelineAdminResources:Connection_Type_SharedRest

		public static string Connection_Type_SharedRest { get { return GetResourceString("Connection_Type_SharedRest"); } }
//Resources:PipelineAdminResources:Connection_Type_Soap

		public static string Connection_Type_Soap { get { return GetResourceString("Connection_Type_Soap"); } }
//Resources:PipelineAdminResources:Connection_Type_TCP

		public static string Connection_Type_TCP { get { return GetResourceString("Connection_Type_TCP"); } }
//Resources:PipelineAdminResources:Connection_Type_UDP

		public static string Connection_Type_UDP { get { return GetResourceString("Connection_Type_UDP"); } }
//Resources:PipelineAdminResources:Connection_Type_WebSocket

		public static string Connection_Type_WebSocket { get { return GetResourceString("Connection_Type_WebSocket"); } }
//Resources:PipelineAdminResources:ConnectionType_MQTT_Listener

		public static string ConnectionType_MQTT_Listener { get { return GetResourceString("ConnectionType_MQTT_Listener"); } }
//Resources:PipelineAdminResources:ConnectionType_RabbitMQ

		public static string ConnectionType_RabbitMQ { get { return GetResourceString("ConnectionType_RabbitMQ"); } }
//Resources:PipelineAdminResources:ConnectionType_RabbitMQClient

		public static string ConnectionType_RabbitMQClient { get { return GetResourceString("ConnectionType_RabbitMQClient"); } }
//Resources:PipelineAdminResources:ConnectionType_SerialPort

		public static string ConnectionType_SerialPort { get { return GetResourceString("ConnectionType_SerialPort"); } }
//Resources:PipelineAdminResources:CusotmModule_CustomModuleType_DotNetAssembly

		public static string CusotmModule_CustomModuleType_DotNetAssembly { get { return GetResourceString("CusotmModule_CustomModuleType_DotNetAssembly"); } }
//Resources:PipelineAdminResources:Custom_AccountId

		public static string Custom_AccountId { get { return GetResourceString("Custom_AccountId"); } }
//Resources:PipelineAdminResources:Custom_AccountPassword

		public static string Custom_AccountPassword { get { return GetResourceString("Custom_AccountPassword"); } }
//Resources:PipelineAdminResources:Custom_AccountPassword_Help

		public static string Custom_AccountPassword_Help { get { return GetResourceString("Custom_AccountPassword_Help"); } }
//Resources:PipelineAdminResources:Custom_Uri

		public static string Custom_Uri { get { return GetResourceString("Custom_Uri"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationHeader

		public static string CustomModule_AuthenticationHeader { get { return GetResourceString("CustomModule_AuthenticationHeader"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationHeader_Help

		public static string CustomModule_AuthenticationHeader_Help { get { return GetResourceString("CustomModule_AuthenticationHeader_Help"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationType

		public static string CustomModule_AuthenticationType { get { return GetResourceString("CustomModule_AuthenticationType"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationType_Anonymous

		public static string CustomModule_AuthenticationType_Anonymous { get { return GetResourceString("CustomModule_AuthenticationType_Anonymous"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationType_AuthenticationHeader

		public static string CustomModule_AuthenticationType_AuthenticationHeader { get { return GetResourceString("CustomModule_AuthenticationType_AuthenticationHeader"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationType_BasicAuth

		public static string CustomModule_AuthenticationType_BasicAuth { get { return GetResourceString("CustomModule_AuthenticationType_BasicAuth"); } }
//Resources:PipelineAdminResources:CustomModule_AuthenticationType_Select

		public static string CustomModule_AuthenticationType_Select { get { return GetResourceString("CustomModule_AuthenticationType_Select"); } }
//Resources:PipelineAdminResources:CustomModule_ContainerRepository

		public static string CustomModule_ContainerRepository { get { return GetResourceString("CustomModule_ContainerRepository"); } }
//Resources:PipelineAdminResources:CustomModule_ContainerRepository_Select

		public static string CustomModule_ContainerRepository_Select { get { return GetResourceString("CustomModule_ContainerRepository_Select"); } }
//Resources:PipelineAdminResources:CustomModule_ContainerTag

		public static string CustomModule_ContainerTag { get { return GetResourceString("CustomModule_ContainerTag"); } }
//Resources:PipelineAdminResources:CustomModule_ContainerTag_Select

		public static string CustomModule_ContainerTag_Select { get { return GetResourceString("CustomModule_ContainerTag_Select"); } }
//Resources:PipelineAdminResources:CustomModule_CustomModuleType

		public static string CustomModule_CustomModuleType { get { return GetResourceString("CustomModule_CustomModuleType"); } }
//Resources:PipelineAdminResources:CustomModule_CustomModuleType_Container

		public static string CustomModule_CustomModuleType_Container { get { return GetResourceString("CustomModule_CustomModuleType_Container"); } }
//Resources:PipelineAdminResources:CustomModule_CustomModuleType_Script

		public static string CustomModule_CustomModuleType_Script { get { return GetResourceString("CustomModule_CustomModuleType_Script"); } }
//Resources:PipelineAdminResources:CustomModule_CustomModuleType_Select

		public static string CustomModule_CustomModuleType_Select { get { return GetResourceString("CustomModule_CustomModuleType_Select"); } }
//Resources:PipelineAdminResources:CustomModule_CustomModuleType_WebFunction

		public static string CustomModule_CustomModuleType_WebFunction { get { return GetResourceString("CustomModule_CustomModuleType_WebFunction"); } }
//Resources:PipelineAdminResources:CustomModule_Description

		public static string CustomModule_Description { get { return GetResourceString("CustomModule_Description"); } }
//Resources:PipelineAdminResources:CustomModule_DotNetAssembly

		public static string CustomModule_DotNetAssembly { get { return GetResourceString("CustomModule_DotNetAssembly"); } }
//Resources:PipelineAdminResources:CustomModule_DotNetClass

		public static string CustomModule_DotNetClass { get { return GetResourceString("CustomModule_DotNetClass"); } }
//Resources:PipelineAdminResources:CustomModule_Help

		public static string CustomModule_Help { get { return GetResourceString("CustomModule_Help"); } }
//Resources:PipelineAdminResources:CustomModule_Script

		public static string CustomModule_Script { get { return GetResourceString("CustomModule_Script"); } }
//Resources:PipelineAdminResources:CustomModule_Title

		public static string CustomModule_Title { get { return GetResourceString("CustomModule_Title"); } }
//Resources:PipelineAdminResources:CustomModule_Uri

		public static string CustomModule_Uri { get { return GetResourceString("CustomModule_Uri"); } }
//Resources:PipelineAdminResources:CustomModule_Uri_AccountId

		public static string CustomModule_Uri_AccountId { get { return GetResourceString("CustomModule_Uri_AccountId"); } }
//Resources:PipelineAdminResources:CustomModule_Uri_AccountPassword

		public static string CustomModule_Uri_AccountPassword { get { return GetResourceString("CustomModule_Uri_AccountPassword"); } }
//Resources:PipelineAdminResources:CustomModule_Uri_AccountPassword_Help

		public static string CustomModule_Uri_AccountPassword_Help { get { return GetResourceString("CustomModule_Uri_AccountPassword_Help"); } }
//Resources:PipelineAdminResources:DataaStream_SummaryLevel

		public static string DataaStream_SummaryLevel { get { return GetResourceString("DataaStream_SummaryLevel"); } }
//Resources:PipelineAdminResources:DataStream_AutoCreateTable

		public static string DataStream_AutoCreateTable { get { return GetResourceString("DataStream_AutoCreateTable"); } }
//Resources:PipelineAdminResources:DataStream_AutoCreateTable_Help

		public static string DataStream_AutoCreateTable_Help { get { return GetResourceString("DataStream_AutoCreateTable_Help"); } }
//Resources:PipelineAdminResources:DataStream_AWSAccessKey

		public static string DataStream_AWSAccessKey { get { return GetResourceString("DataStream_AWSAccessKey"); } }
//Resources:PipelineAdminResources:DataStream_AWSRegion

		public static string DataStream_AWSRegion { get { return GetResourceString("DataStream_AWSRegion"); } }
//Resources:PipelineAdminResources:DataStream_AWSSecretKey

		public static string DataStream_AWSSecretKey { get { return GetResourceString("DataStream_AWSSecretKey"); } }
//Resources:PipelineAdminResources:DataStream_AWSSecretKey_Help

		public static string DataStream_AWSSecretKey_Help { get { return GetResourceString("DataStream_AWSSecretKey_Help"); } }
//Resources:PipelineAdminResources:DataStream_AzureAccessKey

		public static string DataStream_AzureAccessKey { get { return GetResourceString("DataStream_AzureAccessKey"); } }
//Resources:PipelineAdminResources:DataStream_AzureAccessKeyHelp

		public static string DataStream_AzureAccessKeyHelp { get { return GetResourceString("DataStream_AzureAccessKeyHelp"); } }
//Resources:PipelineAdminResources:DataStream_AzureAccountId

		public static string DataStream_AzureAccountId { get { return GetResourceString("DataStream_AzureAccountId"); } }
//Resources:PipelineAdminResources:DataStream_AzureEventHubName

		public static string DataStream_AzureEventHubName { get { return GetResourceString("DataStream_AzureEventHubName"); } }
//Resources:PipelineAdminResources:DataStream_AzureEventHubPath

		public static string DataStream_AzureEventHubPath { get { return GetResourceString("DataStream_AzureEventHubPath"); } }
//Resources:PipelineAdminResources:DataStream_AzureStorageName

		public static string DataStream_AzureStorageName { get { return GetResourceString("DataStream_AzureStorageName"); } }
//Resources:PipelineAdminResources:DataStream_BlobStorage_InvalidName

		public static string DataStream_BlobStorage_InvalidName { get { return GetResourceString("DataStream_BlobStorage_InvalidName"); } }
//Resources:PipelineAdminResources:DataStream_BlobStoragePath

		public static string DataStream_BlobStoragePath { get { return GetResourceString("DataStream_BlobStoragePath"); } }
//Resources:PipelineAdminResources:DataStream_ConnectionString

		public static string DataStream_ConnectionString { get { return GetResourceString("DataStream_ConnectionString"); } }
//Resources:PipelineAdminResources:DataStream_ConnectionString_Help

		public static string DataStream_ConnectionString_Help { get { return GetResourceString("DataStream_ConnectionString_Help"); } }
//Resources:PipelineAdminResources:DataStream_CreateTableDDL

		public static string DataStream_CreateTableDDL { get { return GetResourceString("DataStream_CreateTableDDL"); } }
//Resources:PipelineAdminResources:DataStream_CreateTableDDL_Help

		public static string DataStream_CreateTableDDL_Help { get { return GetResourceString("DataStream_CreateTableDDL_Help"); } }
//Resources:PipelineAdminResources:DataStream_DateStorageFormat

		public static string DataStream_DateStorageFormat { get { return GetResourceString("DataStream_DateStorageFormat"); } }
//Resources:PipelineAdminResources:DataStream_DateStorageFormat_Help

		public static string DataStream_DateStorageFormat_Help { get { return GetResourceString("DataStream_DateStorageFormat_Help"); } }
//Resources:PipelineAdminResources:DataStream_DateStorageFormat_Select

		public static string DataStream_DateStorageFormat_Select { get { return GetResourceString("DataStream_DateStorageFormat_Select"); } }
//Resources:PipelineAdminResources:DataStream_DateStorageFormat_Type_Epoch

		public static string DataStream_DateStorageFormat_Type_Epoch { get { return GetResourceString("DataStream_DateStorageFormat_Type_Epoch"); } }
//Resources:PipelineAdminResources:DataStream_DateStorageFormat_Type_ISO8601

		public static string DataStream_DateStorageFormat_Type_ISO8601 { get { return GetResourceString("DataStream_DateStorageFormat_Type_ISO8601"); } }
//Resources:PipelineAdminResources:DataStream_DbName

		public static string DataStream_DbName { get { return GetResourceString("DataStream_DbName"); } }
//Resources:PipelineAdminResources:DataStream_DbPassword

		public static string DataStream_DbPassword { get { return GetResourceString("DataStream_DbPassword"); } }
//Resources:PipelineAdminResources:DataStream_DbPassword_Help

		public static string DataStream_DbPassword_Help { get { return GetResourceString("DataStream_DbPassword_Help"); } }
//Resources:PipelineAdminResources:DataStream_DbSchema

		public static string DataStream_DbSchema { get { return GetResourceString("DataStream_DbSchema"); } }
//Resources:PipelineAdminResources:DataStream_DbURL

		public static string DataStream_DbURL { get { return GetResourceString("DataStream_DbURL"); } }
//Resources:PipelineAdminResources:DataStream_DbUrl_InvalidUrl

		public static string DataStream_DbUrl_InvalidUrl { get { return GetResourceString("DataStream_DbUrl_InvalidUrl"); } }
//Resources:PipelineAdminResources:DataStream_DbUserName

		public static string DataStream_DbUserName { get { return GetResourceString("DataStream_DbUserName"); } }
//Resources:PipelineAdminResources:DataStream_DbValidateSchema

		public static string DataStream_DbValidateSchema { get { return GetResourceString("DataStream_DbValidateSchema"); } }
//Resources:PipelineAdminResources:DataStream_DbValidateSchema_Help

		public static string DataStream_DbValidateSchema_Help { get { return GetResourceString("DataStream_DbValidateSchema_Help"); } }
//Resources:PipelineAdminResources:DataStream_Description

		public static string DataStream_Description { get { return GetResourceString("DataStream_Description"); } }
//Resources:PipelineAdminResources:DataStream_DeviceId_InvalidFormat

		public static string DataStream_DeviceId_InvalidFormat { get { return GetResourceString("DataStream_DeviceId_InvalidFormat"); } }
//Resources:PipelineAdminResources:DataStream_DeviceIdFieldName

		public static string DataStream_DeviceIdFieldName { get { return GetResourceString("DataStream_DeviceIdFieldName"); } }
//Resources:PipelineAdminResources:DataStream_DeviceIdFieldName_Help

		public static string DataStream_DeviceIdFieldName_Help { get { return GetResourceString("DataStream_DeviceIdFieldName_Help"); } }
//Resources:PipelineAdminResources:DataStream_ESDomainName

		public static string DataStream_ESDomainName { get { return GetResourceString("DataStream_ESDomainName"); } }
//Resources:PipelineAdminResources:DataStream_ESDomainName_Invalid

		public static string DataStream_ESDomainName_Invalid { get { return GetResourceString("DataStream_ESDomainName_Invalid"); } }
//Resources:PipelineAdminResources:DataStream_ESIndexName

		public static string DataStream_ESIndexName { get { return GetResourceString("DataStream_ESIndexName"); } }
//Resources:PipelineAdminResources:DataStream_ESIndexName_Invalid

		public static string DataStream_ESIndexName_Invalid { get { return GetResourceString("DataStream_ESIndexName_Invalid"); } }
//Resources:PipelineAdminResources:DataStream_ESTypeName

		public static string DataStream_ESTypeName { get { return GetResourceString("DataStream_ESTypeName"); } }
//Resources:PipelineAdminResources:DataStream_ESTypeNameInvalid

		public static string DataStream_ESTypeNameInvalid { get { return GetResourceString("DataStream_ESTypeNameInvalid"); } }
//Resources:PipelineAdminResources:DataStream_Fields

		public static string DataStream_Fields { get { return GetResourceString("DataStream_Fields"); } }
//Resources:PipelineAdminResources:DataStream_Help

		public static string DataStream_Help { get { return GetResourceString("DataStream_Help"); } }
//Resources:PipelineAdminResources:DataStream_InvalidBucketName

		public static string DataStream_InvalidBucketName { get { return GetResourceString("DataStream_InvalidBucketName"); } }
//Resources:PipelineAdminResources:DataStream_InvalidEHName

		public static string DataStream_InvalidEHName { get { return GetResourceString("DataStream_InvalidEHName"); } }
//Resources:PipelineAdminResources:DataStream_InvalidEHPathName

		public static string DataStream_InvalidEHPathName { get { return GetResourceString("DataStream_InvalidEHPathName"); } }
//Resources:PipelineAdminResources:DataStream_InvalidTableName

		public static string DataStream_InvalidTableName { get { return GetResourceString("DataStream_InvalidTableName"); } }
//Resources:PipelineAdminResources:DataStream_RedisPassword

		public static string DataStream_RedisPassword { get { return GetResourceString("DataStream_RedisPassword"); } }
//Resources:PipelineAdminResources:DataStream_RedisPassword_Help

		public static string DataStream_RedisPassword_Help { get { return GetResourceString("DataStream_RedisPassword_Help"); } }
//Resources:PipelineAdminResources:DataStream_RedisServers

		public static string DataStream_RedisServers { get { return GetResourceString("DataStream_RedisServers"); } }
//Resources:PipelineAdminResources:DataStream_RedisServers_Help

		public static string DataStream_RedisServers_Help { get { return GetResourceString("DataStream_RedisServers_Help"); } }
//Resources:PipelineAdminResources:DataStream_S3_BucketName

		public static string DataStream_S3_BucketName { get { return GetResourceString("DataStream_S3_BucketName"); } }
//Resources:PipelineAdminResources:DataStream_SharedConnection

		public static string DataStream_SharedConnection { get { return GetResourceString("DataStream_SharedConnection"); } }
//Resources:PipelineAdminResources:DataStream_SharedConnection_Select

		public static string DataStream_SharedConnection_Select { get { return GetResourceString("DataStream_SharedConnection_Select"); } }
//Resources:PipelineAdminResources:DataStream_StreamType

		public static string DataStream_StreamType { get { return GetResourceString("DataStream_StreamType"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AWS_ElasticSearch

		public static string DataStream_StreamType_AWS_ElasticSearch { get { return GetResourceString("DataStream_StreamType_AWS_ElasticSearch"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AWS_S3

		public static string DataStream_StreamType_AWS_S3 { get { return GetResourceString("DataStream_StreamType_AWS_S3"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AzureBlob

		public static string DataStream_StreamType_AzureBlob { get { return GetResourceString("DataStream_StreamType_AzureBlob"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AzureBlob_Managed

		public static string DataStream_StreamType_AzureBlob_Managed { get { return GetResourceString("DataStream_StreamType_AzureBlob_Managed"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AzureEventHub

		public static string DataStream_StreamType_AzureEventHub { get { return GetResourceString("DataStream_StreamType_AzureEventHub"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_AzureEventHub_Managegd

		public static string DataStream_StreamType_AzureEventHub_Managegd { get { return GetResourceString("DataStream_StreamType_AzureEventHub_Managegd"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_DataLake

		public static string DataStream_StreamType_DataLake { get { return GetResourceString("DataStream_StreamType_DataLake"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_GeoSpatial

		public static string DataStream_StreamType_GeoSpatial { get { return GetResourceString("DataStream_StreamType_GeoSpatial"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_PointArrayStorage

		public static string DataStream_StreamType_PointArrayStorage { get { return GetResourceString("DataStream_StreamType_PointArrayStorage"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_PostgreSQL

		public static string DataStream_StreamType_PostgreSQL { get { return GetResourceString("DataStream_StreamType_PostgreSQL"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_Redis

		public static string DataStream_StreamType_Redis { get { return GetResourceString("DataStream_StreamType_Redis"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_Select

		public static string DataStream_StreamType_Select { get { return GetResourceString("DataStream_StreamType_Select"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_SQLServer

		public static string DataStream_StreamType_SQLServer { get { return GetResourceString("DataStream_StreamType_SQLServer"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_TableStorage

		public static string DataStream_StreamType_TableStorage { get { return GetResourceString("DataStream_StreamType_TableStorage"); } }
//Resources:PipelineAdminResources:DataStream_StreamType_TableStorage_Managed

		public static string DataStream_StreamType_TableStorage_Managed { get { return GetResourceString("DataStream_StreamType_TableStorage_Managed"); } }
//Resources:PipelineAdminResources:DataStream_SummaryData

		public static string DataStream_SummaryData { get { return GetResourceString("DataStream_SummaryData"); } }
//Resources:PipelineAdminResources:DataStream_SummaryData_Help

		public static string DataStream_SummaryData_Help { get { return GetResourceString("DataStream_SummaryData_Help"); } }
//Resources:PipelineAdminResources:DataStream_SummaryLevel_Help

		public static string DataStream_SummaryLevel_Help { get { return GetResourceString("DataStream_SummaryLevel_Help"); } }
//Resources:PipelineAdminResources:DataStream_TableName

		public static string DataStream_TableName { get { return GetResourceString("DataStream_TableName"); } }
//Resources:PipelineAdminResources:DataStream_TableStorage_InvalidName

		public static string DataStream_TableStorage_InvalidName { get { return GetResourceString("DataStream_TableStorage_InvalidName"); } }
//Resources:PipelineAdminResources:DataStream_TableStorageName

		public static string DataStream_TableStorageName { get { return GetResourceString("DataStream_TableStorageName"); } }
//Resources:PipelineAdminResources:DataStream_TimeStamp_InvalidFormat

		public static string DataStream_TimeStamp_InvalidFormat { get { return GetResourceString("DataStream_TimeStamp_InvalidFormat"); } }
//Resources:PipelineAdminResources:DataStream_TimeStampFieldName

		public static string DataStream_TimeStampFieldName { get { return GetResourceString("DataStream_TimeStampFieldName"); } }
//Resources:PipelineAdminResources:DataStream_TimeStampFieldName_Help

		public static string DataStream_TimeStampFieldName_Help { get { return GetResourceString("DataStream_TimeStampFieldName_Help"); } }
//Resources:PipelineAdminResources:DataStream_Title

		public static string DataStream_Title { get { return GetResourceString("DataStream_Title"); } }
//Resources:PipelineAdminResources:DataStreamField_DataType

		public static string DataStreamField_DataType { get { return GetResourceString("DataStreamField_DataType"); } }
//Resources:PipelineAdminResources:DataStreamField_DataType_Help

		public static string DataStreamField_DataType_Help { get { return GetResourceString("DataStreamField_DataType_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_DataType_Select

		public static string DataStreamField_DataType_Select { get { return GetResourceString("DataStreamField_DataType_Select"); } }
//Resources:PipelineAdminResources:DataStreamField_Description

		public static string DataStreamField_Description { get { return GetResourceString("DataStreamField_Description"); } }
//Resources:PipelineAdminResources:DataStreamField_FieldName

		public static string DataStreamField_FieldName { get { return GetResourceString("DataStreamField_FieldName"); } }
//Resources:PipelineAdminResources:DataStreamField_FieldName_Help

		public static string DataStreamField_FieldName_Help { get { return GetResourceString("DataStreamField_FieldName_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_FieldName_Invalid

		public static string DataStreamField_FieldName_Invalid { get { return GetResourceString("DataStreamField_FieldName_Invalid"); } }
//Resources:PipelineAdminResources:DataStreamField_Help

		public static string DataStreamField_Help { get { return GetResourceString("DataStreamField_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_IsDatabaseGenerated

		public static string DataStreamField_IsDatabaseGenerated { get { return GetResourceString("DataStreamField_IsDatabaseGenerated"); } }
//Resources:PipelineAdminResources:DataStreamField_IsDatabaseGenerated_Help

		public static string DataStreamField_IsDatabaseGenerated_Help { get { return GetResourceString("DataStreamField_IsDatabaseGenerated_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_IsKey

		public static string DataStreamField_IsKey { get { return GetResourceString("DataStreamField_IsKey"); } }
//Resources:PipelineAdminResources:DataStreamField_IsKey_Description

		public static string DataStreamField_IsKey_Description { get { return GetResourceString("DataStreamField_IsKey_Description"); } }
//Resources:PipelineAdminResources:DataStreamField_IsRequired

		public static string DataStreamField_IsRequired { get { return GetResourceString("DataStreamField_IsRequired"); } }
//Resources:PipelineAdminResources:DataStreamField_IsRequired_Help

		public static string DataStreamField_IsRequired_Help { get { return GetResourceString("DataStreamField_IsRequired_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_MaxValue

		public static string DataStreamField_MaxValue { get { return GetResourceString("DataStreamField_MaxValue"); } }
//Resources:PipelineAdminResources:DataStreamField_MaxValue_Help

		public static string DataStreamField_MaxValue_Help { get { return GetResourceString("DataStreamField_MaxValue_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_MinValue

		public static string DataStreamField_MinValue { get { return GetResourceString("DataStreamField_MinValue"); } }
//Resources:PipelineAdminResources:DataStreamField_MinValue_Help

		public static string DataStreamField_MinValue_Help { get { return GetResourceString("DataStreamField_MinValue_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_NumberDecimalPoints

		public static string DataStreamField_NumberDecimalPoints { get { return GetResourceString("DataStreamField_NumberDecimalPoints"); } }
//Resources:PipelineAdminResources:DataStreamField_NumberDecimalPoints_Help

		public static string DataStreamField_NumberDecimalPoints_Help { get { return GetResourceString("DataStreamField_NumberDecimalPoints_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_RegEx

		public static string DataStreamField_RegEx { get { return GetResourceString("DataStreamField_RegEx"); } }
//Resources:PipelineAdminResources:DataStreamField_RegEx_Help

		public static string DataStreamField_RegEx_Help { get { return GetResourceString("DataStreamField_RegEx_Help"); } }
//Resources:PipelineAdminResources:DataStreamField_StateSet

		public static string DataStreamField_StateSet { get { return GetResourceString("DataStreamField_StateSet"); } }
//Resources:PipelineAdminResources:DataStreamField_StateSet_Watermark

		public static string DataStreamField_StateSet_Watermark { get { return GetResourceString("DataStreamField_StateSet_Watermark"); } }
//Resources:PipelineAdminResources:DataStreamField_Title

		public static string DataStreamField_Title { get { return GetResourceString("DataStreamField_Title"); } }
//Resources:PipelineAdminResources:DataStreamField_UnitSet

		public static string DataStreamField_UnitSet { get { return GetResourceString("DataStreamField_UnitSet"); } }
//Resources:PipelineAdminResources:DataStreamField_UnitSet_Watermark

		public static string DataStreamField_UnitSet_Watermark { get { return GetResourceString("DataStreamField_UnitSet_Watermark"); } }
//Resources:PipelineAdminResources:Dictionary_Description

		public static string Dictionary_Description { get { return GetResourceString("Dictionary_Description"); } }
//Resources:PipelineAdminResources:Dictionary_Help

		public static string Dictionary_Help { get { return GetResourceString("Dictionary_Help"); } }
//Resources:PipelineAdminResources:Dictionary_Password

		public static string Dictionary_Password { get { return GetResourceString("Dictionary_Password"); } }
//Resources:PipelineAdminResources:Dictionary_Password_Help

		public static string Dictionary_Password_Help { get { return GetResourceString("Dictionary_Password_Help"); } }
//Resources:PipelineAdminResources:Dictionary_Title

		public static string Dictionary_Title { get { return GetResourceString("Dictionary_Title"); } }
//Resources:PipelineAdminResources:Dictionary_Type

		public static string Dictionary_Type { get { return GetResourceString("Dictionary_Type"); } }
//Resources:PipelineAdminResources:Dictionary_Type_NuvIoT

		public static string Dictionary_Type_NuvIoT { get { return GetResourceString("Dictionary_Type_NuvIoT"); } }
//Resources:PipelineAdminResources:Dictionary_Type_Redis

		public static string Dictionary_Type_Redis { get { return GetResourceString("Dictionary_Type_Redis"); } }
//Resources:PipelineAdminResources:Dictionary_Type_Select

		public static string Dictionary_Type_Select { get { return GetResourceString("Dictionary_Type_Select"); } }
//Resources:PipelineAdminResources:Dictionary_Uri

		public static string Dictionary_Uri { get { return GetResourceString("Dictionary_Uri"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadCustomModule

		public static string Err_CouldNotLoadCustomModule { get { return GetResourceString("Err_CouldNotLoadCustomModule"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadDataStream

		public static string Err_CouldNotLoadDataStream { get { return GetResourceString("Err_CouldNotLoadDataStream"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadInputTranslator

		public static string Err_CouldNotLoadInputTranslator { get { return GetResourceString("Err_CouldNotLoadInputTranslator"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadListener

		public static string Err_CouldNotLoadListener { get { return GetResourceString("Err_CouldNotLoadListener"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadOutputTranslator

		public static string Err_CouldNotLoadOutputTranslator { get { return GetResourceString("Err_CouldNotLoadOutputTranslator"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadPlanner

		public static string Err_CouldNotLoadPlanner { get { return GetResourceString("Err_CouldNotLoadPlanner"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadSentinel

		public static string Err_CouldNotLoadSentinel { get { return GetResourceString("Err_CouldNotLoadSentinel"); } }
//Resources:PipelineAdminResources:Err_CouldNotLoadTransmitter

		public static string Err_CouldNotLoadTransmitter { get { return GetResourceString("Err_CouldNotLoadTransmitter"); } }
//Resources:PipelineAdminResources:Err_TransmitterTypeIsRequired

		public static string Err_TransmitterTypeIsRequired { get { return GetResourceString("Err_TransmitterTypeIsRequired"); } }
//Resources:PipelineAdminResources:InputTranslator_DelimiterSquence_Help

		public static string InputTranslator_DelimiterSquence_Help { get { return GetResourceString("InputTranslator_DelimiterSquence_Help"); } }
//Resources:PipelineAdminResources:InputTranslator_DelimterSequence

		public static string InputTranslator_DelimterSequence { get { return GetResourceString("InputTranslator_DelimterSequence"); } }
//Resources:PipelineAdminResources:InputTranslator_Description

		public static string InputTranslator_Description { get { return GetResourceString("InputTranslator_Description"); } }
//Resources:PipelineAdminResources:InputTranslator_Help

		public static string InputTranslator_Help { get { return GetResourceString("InputTranslator_Help"); } }
//Resources:PipelineAdminResources:InputTranslator_Model

		public static string InputTranslator_Model { get { return GetResourceString("InputTranslator_Model"); } }
//Resources:PipelineAdminResources:InputTranslator_Model_Select

		public static string InputTranslator_Model_Select { get { return GetResourceString("InputTranslator_Model_Select"); } }
//Resources:PipelineAdminResources:InputTranslator_ModelRevision

		public static string InputTranslator_ModelRevision { get { return GetResourceString("InputTranslator_ModelRevision"); } }
//Resources:PipelineAdminResources:InputTranslator_ModelRevision_Select

		public static string InputTranslator_ModelRevision_Select { get { return GetResourceString("InputTranslator_ModelRevision_Select"); } }
//Resources:PipelineAdminResources:InputTranslator_SevenSegmentParser

		public static string InputTranslator_SevenSegmentParser { get { return GetResourceString("InputTranslator_SevenSegmentParser"); } }
//Resources:PipelineAdminResources:InputTranslator_SevenSegmentParser_Select

		public static string InputTranslator_SevenSegmentParser_Select { get { return GetResourceString("InputTranslator_SevenSegmentParser_Select"); } }
//Resources:PipelineAdminResources:InputTranslator_Title

		public static string InputTranslator_Title { get { return GetResourceString("InputTranslator_Title"); } }
//Resources:PipelineAdminResources:InputTranslator_TranslatorType

		public static string InputTranslator_TranslatorType { get { return GetResourceString("InputTranslator_TranslatorType"); } }
//Resources:PipelineAdminResources:InputTranslator_TranslatorType_Select

		public static string InputTranslator_TranslatorType_Select { get { return GetResourceString("InputTranslator_TranslatorType_Select"); } }
//Resources:PipelineAdminResources:Listener_AccessKey

		public static string Listener_AccessKey { get { return GetResourceString("Listener_AccessKey"); } }
//Resources:PipelineAdminResources:Listener_AccessKey_Help

		public static string Listener_AccessKey_Help { get { return GetResourceString("Listener_AccessKey_Help"); } }
//Resources:PipelineAdminResources:Listener_AccessKeyName

		public static string Listener_AccessKeyName { get { return GetResourceString("Listener_AccessKeyName"); } }
//Resources:PipelineAdminResources:Listener_AccessKeyName_Help

		public static string Listener_AccessKeyName_Help { get { return GetResourceString("Listener_AccessKeyName_Help"); } }
//Resources:PipelineAdminResources:Listener_Anonymous

		public static string Listener_Anonymous { get { return GetResourceString("Listener_Anonymous"); } }
//Resources:PipelineAdminResources:Listener_ConnectSSLTLS

		public static string Listener_ConnectSSLTLS { get { return GetResourceString("Listener_ConnectSSLTLS"); } }
//Resources:PipelineAdminResources:Listener_ConnectToPort

		public static string Listener_ConnectToPort { get { return GetResourceString("Listener_ConnectToPort"); } }
//Resources:PipelineAdminResources:Listener_ConsumerGroup_Help

		public static string Listener_ConsumerGroup_Help { get { return GetResourceString("Listener_ConsumerGroup_Help"); } }
//Resources:PipelineAdminResources:Listener_DefaultResponse

		public static string Listener_DefaultResponse { get { return GetResourceString("Listener_DefaultResponse"); } }
//Resources:PipelineAdminResources:Listener_DelimitedWithSOH_EOT

		public static string Listener_DelimitedWithSOH_EOT { get { return GetResourceString("Listener_DelimitedWithSOH_EOT"); } }
//Resources:PipelineAdminResources:Listener_DelimitedWithSOH_EOT_Help

		public static string Listener_DelimitedWithSOH_EOT_Help { get { return GetResourceString("Listener_DelimitedWithSOH_EOT_Help"); } }
//Resources:PipelineAdminResources:Listener_Description

		public static string Listener_Description { get { return GetResourceString("Listener_Description"); } }
//Resources:PipelineAdminResources:Listener_EndMessageSequence

		public static string Listener_EndMessageSequence { get { return GetResourceString("Listener_EndMessageSequence"); } }
//Resources:PipelineAdminResources:Listener_EndMessageSequence_Help

		public static string Listener_EndMessageSequence_Help { get { return GetResourceString("Listener_EndMessageSequence_Help"); } }
//Resources:PipelineAdminResources:Listener_Endpoint

		public static string Listener_Endpoint { get { return GetResourceString("Listener_Endpoint"); } }
//Resources:PipelineAdminResources:Listener_EventHub_ConsumerGroup

		public static string Listener_EventHub_ConsumerGroup { get { return GetResourceString("Listener_EventHub_ConsumerGroup"); } }
//Resources:PipelineAdminResources:Listener_ExchangeName

		public static string Listener_ExchangeName { get { return GetResourceString("Listener_ExchangeName"); } }
//Resources:PipelineAdminResources:Listener_FailedResponse

		public static string Listener_FailedResponse { get { return GetResourceString("Listener_FailedResponse"); } }
//Resources:PipelineAdminResources:Listener_Help

		public static string Listener_Help { get { return GetResourceString("Listener_Help"); } }
//Resources:PipelineAdminResources:Listener_HostName

		public static string Listener_HostName { get { return GetResourceString("Listener_HostName"); } }
//Resources:PipelineAdminResources:Listener_HubName

		public static string Listener_HubName { get { return GetResourceString("Listener_HubName"); } }
//Resources:PipelineAdminResources:Listener_HubName_Help

		public static string Listener_HubName_Help { get { return GetResourceString("Listener_HubName_Help"); } }
//Resources:PipelineAdminResources:Listener_KeepAliveToSendReply

		public static string Listener_KeepAliveToSendReply { get { return GetResourceString("Listener_KeepAliveToSendReply"); } }
//Resources:PipelineAdminResources:Listener_KeepAliveToSendReply_Timeout

		public static string Listener_KeepAliveToSendReply_Timeout { get { return GetResourceString("Listener_KeepAliveToSendReply_Timeout"); } }
//Resources:PipelineAdminResources:Listener_KeepAliveToSendReplyTimeout_Help

		public static string Listener_KeepAliveToSendReplyTimeout_Help { get { return GetResourceString("Listener_KeepAliveToSendReplyTimeout_Help"); } }
//Resources:PipelineAdminResources:Listener_Length_Endiness

		public static string Listener_Length_Endiness { get { return GetResourceString("Listener_Length_Endiness"); } }
//Resources:PipelineAdminResources:Listener_Length_Endiness_Help

		public static string Listener_Length_Endiness_Help { get { return GetResourceString("Listener_Length_Endiness_Help"); } }
//Resources:PipelineAdminResources:Listener_Length_Endiness_Select

		public static string Listener_Length_Endiness_Select { get { return GetResourceString("Listener_Length_Endiness_Select"); } }
//Resources:PipelineAdminResources:Listener_Length_Location

		public static string Listener_Length_Location { get { return GetResourceString("Listener_Length_Location"); } }
//Resources:PipelineAdminResources:Listener_Length_Location_Help

		public static string Listener_Length_Location_Help { get { return GetResourceString("Listener_Length_Location_Help"); } }
//Resources:PipelineAdminResources:Listener_Length_LocationByteLength

		public static string Listener_Length_LocationByteLength { get { return GetResourceString("Listener_Length_LocationByteLength"); } }
//Resources:PipelineAdminResources:Listener_Length_LocationByteLength_Help

		public static string Listener_Length_LocationByteLength_Help { get { return GetResourceString("Listener_Length_LocationByteLength_Help"); } }
//Resources:PipelineAdminResources:Listener_ListenerType

		public static string Listener_ListenerType { get { return GetResourceString("Listener_ListenerType"); } }
//Resources:PipelineAdminResources:Listener_ListenOnPort

		public static string Listener_ListenOnPort { get { return GetResourceString("Listener_ListenOnPort"); } }
//Resources:PipelineAdminResources:Listener_MaxMessageSize

		public static string Listener_MaxMessageSize { get { return GetResourceString("Listener_MaxMessageSize"); } }
//Resources:PipelineAdminResources:Listener_MaxMessageSize_Help

		public static string Listener_MaxMessageSize_Help { get { return GetResourceString("Listener_MaxMessageSize_Help"); } }
//Resources:PipelineAdminResources:Listener_MessageContainsLength

		public static string Listener_MessageContainsLength { get { return GetResourceString("Listener_MessageContainsLength"); } }
//Resources:PipelineAdminResources:Listener_MessageContainsLength_Help

		public static string Listener_MessageContainsLength_Help { get { return GetResourceString("Listener_MessageContainsLength_Help"); } }
//Resources:PipelineAdminResources:Listener_MessageLength_1_Byte

		public static string Listener_MessageLength_1_Byte { get { return GetResourceString("Listener_MessageLength_1_Byte"); } }
//Resources:PipelineAdminResources:Listener_MessageLength_2_Bytes

		public static string Listener_MessageLength_2_Bytes { get { return GetResourceString("Listener_MessageLength_2_Bytes"); } }
//Resources:PipelineAdminResources:Listener_MessageLength_4_Bytes

		public static string Listener_MessageLength_4_Bytes { get { return GetResourceString("Listener_MessageLength_4_Bytes"); } }
//Resources:PipelineAdminResources:Listener_MessageLength_Select

		public static string Listener_MessageLength_Select { get { return GetResourceString("Listener_MessageLength_Select"); } }
//Resources:PipelineAdminResources:Listener_MessageReceivedTimeout

		public static string Listener_MessageReceivedTimeout { get { return GetResourceString("Listener_MessageReceivedTimeout"); } }
//Resources:PipelineAdminResources:Listener_MessageReceivedTimeout_Help

		public static string Listener_MessageReceivedTimeout_Help { get { return GetResourceString("Listener_MessageReceivedTimeout_Help"); } }
//Resources:PipelineAdminResources:Listener_Origin

		public static string Listener_Origin { get { return GetResourceString("Listener_Origin"); } }
//Resources:PipelineAdminResources:Listener_Password

		public static string Listener_Password { get { return GetResourceString("Listener_Password"); } }
//Resources:PipelineAdminResources:Listener_Password_Help

		public static string Listener_Password_Help { get { return GetResourceString("Listener_Password_Help"); } }
//Resources:PipelineAdminResources:Listener_Path

		public static string Listener_Path { get { return GetResourceString("Listener_Path"); } }
//Resources:PipelineAdminResources:Listener_Port_Help

		public static string Listener_Port_Help { get { return GetResourceString("Listener_Port_Help"); } }
//Resources:PipelineAdminResources:Listener_Queue

		public static string Listener_Queue { get { return GetResourceString("Listener_Queue"); } }
//Resources:PipelineAdminResources:Listener_Queue_Help

		public static string Listener_Queue_Help { get { return GetResourceString("Listener_Queue_Help"); } }
//Resources:PipelineAdminResources:Listener_ResourceName

		public static string Listener_ResourceName { get { return GetResourceString("Listener_ResourceName"); } }
//Resources:PipelineAdminResources:Listener_RESTServerType

		public static string Listener_RESTServerType { get { return GetResourceString("Listener_RESTServerType"); } }
//Resources:PipelineAdminResources:Listener_RESTServerType_HTTP

		public static string Listener_RESTServerType_HTTP { get { return GetResourceString("Listener_RESTServerType_HTTP"); } }
//Resources:PipelineAdminResources:Listener_RESTServerType_HTTPorHTTPS

		public static string Listener_RESTServerType_HTTPorHTTPS { get { return GetResourceString("Listener_RESTServerType_HTTPorHTTPS"); } }
//Resources:PipelineAdminResources:Listener_RESTServerType_HTTPS

		public static string Listener_RESTServerType_HTTPS { get { return GetResourceString("Listener_RESTServerType_HTTPS"); } }
//Resources:PipelineAdminResources:Listener_RESTServerType_Select

		public static string Listener_RESTServerType_Select { get { return GetResourceString("Listener_RESTServerType_Select"); } }
//Resources:PipelineAdminResources:Listener_StartMessageSequence

		public static string Listener_StartMessageSequence { get { return GetResourceString("Listener_StartMessageSequence"); } }
//Resources:PipelineAdminResources:Listener_StartMessageSequence_Help

		public static string Listener_StartMessageSequence_Help { get { return GetResourceString("Listener_StartMessageSequence_Help"); } }
//Resources:PipelineAdminResources:Listener_Subscription

		public static string Listener_Subscription { get { return GetResourceString("Listener_Subscription"); } }
//Resources:PipelineAdminResources:Listener_Subscriptions

		public static string Listener_Subscriptions { get { return GetResourceString("Listener_Subscriptions"); } }
//Resources:PipelineAdminResources:Listener_SupportedProtocol

		public static string Listener_SupportedProtocol { get { return GetResourceString("Listener_SupportedProtocol"); } }
//Resources:PipelineAdminResources:Listener_Title

		public static string Listener_Title { get { return GetResourceString("Listener_Title"); } }
//Resources:PipelineAdminResources:Listener_Topic

		public static string Listener_Topic { get { return GetResourceString("Listener_Topic"); } }
//Resources:PipelineAdminResources:Listener_UserName

		public static string Listener_UserName { get { return GetResourceString("Listener_UserName"); } }
//Resources:PipelineAdminResources:Listener_UserName_Help

		public static string Listener_UserName_Help { get { return GetResourceString("Listener_UserName_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParseConfiguration_BinaryOffset_Help

		public static string MessageFieldParseConfiguration_BinaryOffset_Help { get { return GetResourceString("MessageFieldParseConfiguration_BinaryOffset_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryOffset

		public static string MessageFieldParserConfiguration_BinaryOffset { get { return GetResourceString("MessageFieldParserConfiguration_BinaryOffset"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType

		public static string MessageFieldParserConfiguration_BinaryType { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Boolean

		public static string MessageFieldParserConfiguration_BinaryType_Boolean { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Boolean"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Char

		public static string MessageFieldParserConfiguration_BinaryType_Char { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Char"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint

		public static string MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int16BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int16BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int16BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int16LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int16LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int16LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int32BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int32BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int32BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int32LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int32LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int32LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int64BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int64BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int64BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int64LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_Int64LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int64LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_Int8

		public static string MessageFieldParserConfiguration_BinaryType_Int8 { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_Int8"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_SelectDataType

		public static string MessageFieldParserConfiguration_BinaryType_SelectDataType { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_SelectDataType"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_SelectDataType_Help

		public static string MessageFieldParserConfiguration_BinaryType_SelectDataType_Help { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_SelectDataType_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint

		public static string MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_String

		public static string MessageFieldParserConfiguration_BinaryType_String { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_String"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt16BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt16BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt16BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt32BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt32BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt32BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt64BigEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt64BigEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt64BigEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian

		public static string MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_BinaryType_UInt8

		public static string MessageFieldParserConfiguration_BinaryType_UInt8 { get { return GetResourceString("MessageFieldParserConfiguration_BinaryType_UInt8"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_DelimitedColumnIndex

		public static string MessageFieldParserConfiguration_DelimitedColumnIndex { get { return GetResourceString("MessageFieldParserConfiguration_DelimitedColumnIndex"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_DelimitedColumnIndex_Help

		public static string MessageFieldParserConfiguration_DelimitedColumnIndex_Help { get { return GetResourceString("MessageFieldParserConfiguration_DelimitedColumnIndex_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Delimiter

		public static string MessageFieldParserConfiguration_Delimiter { get { return GetResourceString("MessageFieldParserConfiguration_Delimiter"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Delimitor_Help

		public static string MessageFieldParserConfiguration_Delimitor_Help { get { return GetResourceString("MessageFieldParserConfiguration_Delimitor_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Description

		public static string MessageFieldParserConfiguration_Description { get { return GetResourceString("MessageFieldParserConfiguration_Description"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Help

		public static string MessageFieldParserConfiguration_Help { get { return GetResourceString("MessageFieldParserConfiguration_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_KeyName

		public static string MessageFieldParserConfiguration_KeyName { get { return GetResourceString("MessageFieldParserConfiguration_KeyName"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_KeyName_Help

		public static string MessageFieldParserConfiguration_KeyName_Help { get { return GetResourceString("MessageFieldParserConfiguration_KeyName_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Length

		public static string MessageFieldParserConfiguration_Length { get { return GetResourceString("MessageFieldParserConfiguration_Length"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType

		public static string MessageFieldParserConfiguration_OutputType { get { return GetResourceString("MessageFieldParserConfiguration_OutputType"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_Boolean

		public static string MessageFieldParserConfiguration_OutputType_Boolean { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_Boolean"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_FloatingPoint

		public static string MessageFieldParserConfiguration_OutputType_FloatingPoint { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_FloatingPoint"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_Integer

		public static string MessageFieldParserConfiguration_OutputType_Integer { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_Integer"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_SelectDataType

		public static string MessageFieldParserConfiguration_OutputType_SelectDataType { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_SelectDataType"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_SelectDataType_Help

		public static string MessageFieldParserConfiguration_OutputType_SelectDataType_Help { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_SelectDataType_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_OutputType_String

		public static string MessageFieldParserConfiguration_OutputType_String { get { return GetResourceString("MessageFieldParserConfiguration_OutputType_String"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy

		public static string MessageFieldParserConfiguration_ParserStrategy { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Delimited

		public static string MessageFieldParserConfiguration_ParserStrategy_Delimited { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Delimited"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Header

		public static string MessageFieldParserConfiguration_ParserStrategy_Header { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Header"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Help

		public static string MessageFieldParserConfiguration_ParserStrategy_Help { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_JsonProperty

		public static string MessageFieldParserConfiguration_ParserStrategy_JsonProperty { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_JsonProperty"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_PathLocator

		public static string MessageFieldParserConfiguration_ParserStrategy_PathLocator { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_PathLocator"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help

		public static string MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help( string messageid) { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help", "{messageid}", messageid); }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Position

		public static string MessageFieldParserConfiguration_ParserStrategy_Position { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Position"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_QueryString

		public static string MessageFieldParserConfiguration_ParserStrategy_QueryString { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_QueryString"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_QueryStringField

		public static string MessageFieldParserConfiguration_ParserStrategy_QueryStringField { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_QueryStringField"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help

		public static string MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_RegEx

		public static string MessageFieldParserConfiguration_ParserStrategy_RegEx { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_RegEx"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_RegExHeader

		public static string MessageFieldParserConfiguration_ParserStrategy_RegExHeader { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_RegExHeader"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_RegExHeader_Help

		public static string MessageFieldParserConfiguration_ParserStrategy_RegExHeader_Help { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_RegExHeader_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Script

		public static string MessageFieldParserConfiguration_ParserStrategy_Script { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Script"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Select

		public static string MessageFieldParserConfiguration_ParserStrategy_Select { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Select"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_Substring

		public static string MessageFieldParserConfiguration_ParserStrategy_Substring { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_Substring"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_URIPath

		public static string MessageFieldParserConfiguration_ParserStrategy_URIPath { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_URIPath"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ParserStrategy_XMLProperty

		public static string MessageFieldParserConfiguration_ParserStrategy_XMLProperty { get { return GetResourceString("MessageFieldParserConfiguration_ParserStrategy_XMLProperty"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_QuotedText

		public static string MessageFieldParserConfiguration_QuotedText { get { return GetResourceString("MessageFieldParserConfiguration_QuotedText"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_QuotedText_Help

		public static string MessageFieldParserConfiguration_QuotedText_Help { get { return GetResourceString("MessageFieldParserConfiguration_QuotedText_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExGroupName

		public static string MessageFieldParserConfiguration_RegExGroupName { get { return GetResourceString("MessageFieldParserConfiguration_RegExGroupName"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExGroupName_Help

		public static string MessageFieldParserConfiguration_RegExGroupName_Help { get { return GetResourceString("MessageFieldParserConfiguration_RegExGroupName_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExLocator

		public static string MessageFieldParserConfiguration_RegExLocator { get { return GetResourceString("MessageFieldParserConfiguration_RegExLocator"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExLocator_Help

		public static string MessageFieldParserConfiguration_RegExLocator_Help { get { return GetResourceString("MessageFieldParserConfiguration_RegExLocator_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExValidation

		public static string MessageFieldParserConfiguration_RegExValidation { get { return GetResourceString("MessageFieldParserConfiguration_RegExValidation"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_RegExValidation_Help

		public static string MessageFieldParserConfiguration_RegExValidation_Help { get { return GetResourceString("MessageFieldParserConfiguration_RegExValidation_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StartIndex

		public static string MessageFieldParserConfiguration_StartIndex { get { return GetResourceString("MessageFieldParserConfiguration_StartIndex"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParser_NumberBytes

		public static string MessageFieldParserConfiguration_StringParser_NumberBytes { get { return GetResourceString("MessageFieldParserConfiguration_StringParser_NumberBytes"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParser_NumberBytes_Help

		public static string MessageFieldParserConfiguration_StringParser_NumberBytes_Help { get { return GetResourceString("MessageFieldParserConfiguration_StringParser_NumberBytes_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParserType

		public static string MessageFieldParserConfiguration_StringParserType { get { return GetResourceString("MessageFieldParserConfiguration_StringParserType"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParserType_Help

		public static string MessageFieldParserConfiguration_StringParserType_Help { get { return GetResourceString("MessageFieldParserConfiguration_StringParserType_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParserType_LeadingLength

		public static string MessageFieldParserConfiguration_StringParserType_LeadingLength { get { return GetResourceString("MessageFieldParserConfiguration_StringParserType_LeadingLength"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParserType_NullTerminated

		public static string MessageFieldParserConfiguration_StringParserType_NullTerminated { get { return GetResourceString("MessageFieldParserConfiguration_StringParserType_NullTerminated"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_StringParserType_Select

		public static string MessageFieldParserConfiguration_StringParserType_Select { get { return GetResourceString("MessageFieldParserConfiguration_StringParserType_Select"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_SubString_Help

		public static string MessageFieldParserConfiguration_SubString_Help { get { return GetResourceString("MessageFieldParserConfiguration_SubString_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_Title

		public static string MessageFieldParserConfiguration_Title { get { return GetResourceString("MessageFieldParserConfiguration_Title"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ValueName

		public static string MessageFieldParserConfiguration_ValueName { get { return GetResourceString("MessageFieldParserConfiguration_ValueName"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_ValueName_Help

		public static string MessageFieldParserConfiguration_ValueName_Help { get { return GetResourceString("MessageFieldParserConfiguration_ValueName_Help"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_XPath

		public static string MessageFieldParserConfiguration_XPath { get { return GetResourceString("MessageFieldParserConfiguration_XPath"); } }
//Resources:PipelineAdminResources:MessageFieldParserConfiguration_XPath_Help

		public static string MessageFieldParserConfiguration_XPath_Help { get { return GetResourceString("MessageFieldParserConfiguration_XPath_Help"); } }
//Resources:PipelineAdminResources:OutputTranslator_Description

		public static string OutputTranslator_Description { get { return GetResourceString("OutputTranslator_Description"); } }
//Resources:PipelineAdminResources:OutputTranslator_Help

		public static string OutputTranslator_Help { get { return GetResourceString("OutputTranslator_Help"); } }
//Resources:PipelineAdminResources:OutputTranslator_Title

		public static string OutputTranslator_Title { get { return GetResourceString("OutputTranslator_Title"); } }
//Resources:PipelineAdminResources:OutputTranslator_TranslatorType

		public static string OutputTranslator_TranslatorType { get { return GetResourceString("OutputTranslator_TranslatorType"); } }
//Resources:PipelineAdminResources:OutputTranslator_TranslatorType_Select

		public static string OutputTranslator_TranslatorType_Select { get { return GetResourceString("OutputTranslator_TranslatorType_Select"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Custom

		public static string PipelineModuleType_Custom { get { return GetResourceString("PipelineModuleType_Custom"); } }
//Resources:PipelineAdminResources:PipelineModuleType_DataStream

		public static string PipelineModuleType_DataStream { get { return GetResourceString("PipelineModuleType_DataStream"); } }
//Resources:PipelineAdminResources:PipelineModuleType_InputTranslator

		public static string PipelineModuleType_InputTranslator { get { return GetResourceString("PipelineModuleType_InputTranslator"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Listener

		public static string PipelineModuleType_Listener { get { return GetResourceString("PipelineModuleType_Listener"); } }
//Resources:PipelineAdminResources:PipelineModuleType_OutputTranslator

		public static string PipelineModuleType_OutputTranslator { get { return GetResourceString("PipelineModuleType_OutputTranslator"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Planner

		public static string PipelineModuleType_Planner { get { return GetResourceString("PipelineModuleType_Planner"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Sentinel

		public static string PipelineModuleType_Sentinel { get { return GetResourceString("PipelineModuleType_Sentinel"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Transmitter

		public static string PipelineModuleType_Transmitter { get { return GetResourceString("PipelineModuleType_Transmitter"); } }
//Resources:PipelineAdminResources:PipelineModuleType_Workflow

		public static string PipelineModuleType_Workflow { get { return GetResourceString("PipelineModuleType_Workflow"); } }
//Resources:PipelineAdminResources:Planner_Description

		public static string Planner_Description { get { return GetResourceString("Planner_Description"); } }
//Resources:PipelineAdminResources:Planner_DeviceIDParsers

		public static string Planner_DeviceIDParsers { get { return GetResourceString("Planner_DeviceIDParsers"); } }
//Resources:PipelineAdminResources:Planner_DeviceIDParsers_Help

		public static string Planner_DeviceIDParsers_Help { get { return GetResourceString("Planner_DeviceIDParsers_Help"); } }
//Resources:PipelineAdminResources:Planner_Help

		public static string Planner_Help { get { return GetResourceString("Planner_Help"); } }
//Resources:PipelineAdminResources:Planner_MessageTypeIDParsers

		public static string Planner_MessageTypeIDParsers { get { return GetResourceString("Planner_MessageTypeIDParsers"); } }
//Resources:PipelineAdminResources:Planner_MessageTypeIDParsers_Help

		public static string Planner_MessageTypeIDParsers_Help { get { return GetResourceString("Planner_MessageTypeIDParsers_Help"); } }
//Resources:PipelineAdminResources:Planner_PipelineModules

		public static string Planner_PipelineModules { get { return GetResourceString("Planner_PipelineModules"); } }
//Resources:PipelineAdminResources:Planner_Title

		public static string Planner_Title { get { return GetResourceString("Planner_Title"); } }
//Resources:PipelineAdminResources:Sentinel_Description

		public static string Sentinel_Description { get { return GetResourceString("Sentinel_Description"); } }
//Resources:PipelineAdminResources:Sentinel_Help

		public static string Sentinel_Help { get { return GetResourceString("Sentinel_Help"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField

		public static string Sentinel_SecurityField { get { return GetResourceString("Sentinel_SecurityField"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Description

		public static string Sentinel_SecurityField_Description { get { return GetResourceString("Sentinel_SecurityField_Description"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Help

		public static string Sentinel_SecurityField_Help { get { return GetResourceString("Sentinel_SecurityField_Help"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Locator

		public static string Sentinel_SecurityField_Locator { get { return GetResourceString("Sentinel_SecurityField_Locator"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Locator_Help

		public static string Sentinel_SecurityField_Locator_Help { get { return GetResourceString("Sentinel_SecurityField_Locator_Help"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Locator_Select

		public static string Sentinel_SecurityField_Locator_Select { get { return GetResourceString("Sentinel_SecurityField_Locator_Select"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Title

		public static string Sentinel_SecurityField_Title { get { return GetResourceString("Sentinel_SecurityField_Title"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Title_Help

		public static string Sentinel_SecurityField_Title_Help { get { return GetResourceString("Sentinel_SecurityField_Title_Help"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type

		public static string Sentinel_SecurityField_Type { get { return GetResourceString("Sentinel_SecurityField_Type"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_AccessKey

		public static string Sentinel_SecurityField_Type_AccessKey { get { return GetResourceString("Sentinel_SecurityField_Type_AccessKey"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_BasicAuth

		public static string Sentinel_SecurityField_Type_BasicAuth { get { return GetResourceString("Sentinel_SecurityField_Type_BasicAuth"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_Help

		public static string Sentinel_SecurityField_Type_Help { get { return GetResourceString("Sentinel_SecurityField_Type_Help"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_Script

		public static string Sentinel_SecurityField_Type_Script { get { return GetResourceString("Sentinel_SecurityField_Type_Script"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_Select

		public static string Sentinel_SecurityField_Type_Select { get { return GetResourceString("Sentinel_SecurityField_Type_Select"); } }
//Resources:PipelineAdminResources:Sentinel_SecurityField_Type_SharedSignature

		public static string Sentinel_SecurityField_Type_SharedSignature { get { return GetResourceString("Sentinel_SecurityField_Type_SharedSignature"); } }
//Resources:PipelineAdminResources:Sentinel_Title

		public static string Sentinel_Title { get { return GetResourceString("Sentinel_Title"); } }
//Resources:PipelineAdminResources:SerialPort_BaudRate

		public static string SerialPort_BaudRate { get { return GetResourceString("SerialPort_BaudRate"); } }
//Resources:PipelineAdminResources:SerialPort_PortName

		public static string SerialPort_PortName { get { return GetResourceString("SerialPort_PortName"); } }
//Resources:PipelineAdminResources:SharedConnection_AWS

		public static string SharedConnection_AWS { get { return GetResourceString("SharedConnection_AWS"); } }
//Resources:PipelineAdminResources:SharedConnection_Azure

		public static string SharedConnection_Azure { get { return GetResourceString("SharedConnection_Azure"); } }
//Resources:PipelineAdminResources:SharedConnection_ConnectionType

		public static string SharedConnection_ConnectionType { get { return GetResourceString("SharedConnection_ConnectionType"); } }
//Resources:PipelineAdminResources:SharedConnection_ConnectionType_Select

		public static string SharedConnection_ConnectionType_Select { get { return GetResourceString("SharedConnection_ConnectionType_Select"); } }
//Resources:PipelineAdminResources:SharedConnection_Database

		public static string SharedConnection_Database { get { return GetResourceString("SharedConnection_Database"); } }
//Resources:PipelineAdminResources:SharedConnection_Description

		public static string SharedConnection_Description { get { return GetResourceString("SharedConnection_Description"); } }
//Resources:PipelineAdminResources:SharedConnection_Help

		public static string SharedConnection_Help { get { return GetResourceString("SharedConnection_Help"); } }
//Resources:PipelineAdminResources:SharedConnection_Redis

		public static string SharedConnection_Redis { get { return GetResourceString("SharedConnection_Redis"); } }
//Resources:PipelineAdminResources:SharedConnection_Title

		public static string SharedConnection_Title { get { return GetResourceString("SharedConnection_Title"); } }
//Resources:PipelineAdminResources:Translator_Type_Binary

		public static string Translator_Type_Binary { get { return GetResourceString("Translator_Type_Binary"); } }
//Resources:PipelineAdminResources:Translator_Type_Custom

		public static string Translator_Type_Custom { get { return GetResourceString("Translator_Type_Custom"); } }
//Resources:PipelineAdminResources:Translator_Type_Delimited

		public static string Translator_Type_Delimited { get { return GetResourceString("Translator_Type_Delimited"); } }
//Resources:PipelineAdminResources:Translator_Type_JSON

		public static string Translator_Type_JSON { get { return GetResourceString("Translator_Type_JSON"); } }
//Resources:PipelineAdminResources:Translator_Type_Message

		public static string Translator_Type_Message { get { return GetResourceString("Translator_Type_Message"); } }
//Resources:PipelineAdminResources:Translator_Type_String

		public static string Translator_Type_String { get { return GetResourceString("Translator_Type_String"); } }
//Resources:PipelineAdminResources:Translator_Type_XML

		public static string Translator_Type_XML { get { return GetResourceString("Translator_Type_XML"); } }
//Resources:PipelineAdminResources:TranslatorType_AIModel

		public static string TranslatorType_AIModel { get { return GetResourceString("TranslatorType_AIModel"); } }
//Resources:PipelineAdminResources:TranslatorType_SevenSegmentParser

		public static string TranslatorType_SevenSegmentParser { get { return GetResourceString("TranslatorType_SevenSegmentParser"); } }
//Resources:PipelineAdminResources:Transmitter_Description

		public static string Transmitter_Description { get { return GetResourceString("Transmitter_Description"); } }
//Resources:PipelineAdminResources:Transmitter_Headers

		public static string Transmitter_Headers { get { return GetResourceString("Transmitter_Headers"); } }
//Resources:PipelineAdminResources:Transmitter_Help

		public static string Transmitter_Help { get { return GetResourceString("Transmitter_Help"); } }
//Resources:PipelineAdminResources:Transmitter_Title

		public static string Transmitter_Title { get { return GetResourceString("Transmitter_Title"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType

		public static string Transmitter_TransmitterType { get { return GetResourceString("Transmitter_TransmitterType"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType_OriginalListener

		public static string Transmitter_TransmitterType_OriginalListener { get { return GetResourceString("Transmitter_TransmitterType_OriginalListener"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType_Outbox

		public static string Transmitter_TransmitterType_Outbox { get { return GetResourceString("Transmitter_TransmitterType_Outbox"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType_SerialPort

		public static string Transmitter_TransmitterType_SerialPort { get { return GetResourceString("Transmitter_TransmitterType_SerialPort"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType_SMS

		public static string Transmitter_TransmitterType_SMS { get { return GetResourceString("Transmitter_TransmitterType_SMS"); } }
//Resources:PipelineAdminResources:Transmitter_TransmitterType_SMTP

		public static string Transmitter_TransmitterType_SMTP { get { return GetResourceString("Transmitter_TransmitterType_SMTP"); } }

		public static class Names
		{
			public const string AppCache_CacheType = "AppCache_CacheType";
			public const string AppCache_CacheType_LocalInMemory = "AppCache_CacheType_LocalInMemory";
			public const string AppCache_CacheType_NuvIoT = "AppCache_CacheType_NuvIoT";
			public const string AppCache_CacheType_Redis = "AppCache_CacheType_Redis";
			public const string AppCache_Description = "AppCache_Description";
			public const string AppCache_Help = "AppCache_Help";
			public const string AppCache_InitializationValues = "AppCache_InitializationValues";
			public const string AppCache_Password = "AppCache_Password";
			public const string AppCache_Password_Help = "AppCache_Password_Help";
			public const string AppCache_SelectCacheType = "AppCache_SelectCacheType";
			public const string AppCache_Title = "AppCache_Title";
			public const string AppCache_Uri = "AppCache_Uri";
			public const string AppCache_Uri_Help = "AppCache_Uri_Help";
			public const string ApplicationCacheValue_Description = "ApplicationCacheValue_Description";
			public const string ApplicationCacheValue_Help = "ApplicationCacheValue_Help";
			public const string ApplicationCacheValue_Key = "ApplicationCacheValue_Key";
			public const string ApplicationCacheValue_Title = "ApplicationCacheValue_Title";
			public const string ApplicationCacheValue_Value = "ApplicationCacheValue_Value";
			public const string ApplicationCacheValue_Value_Number = "ApplicationCacheValue_Value_Number";
			public const string ApplicationCacheValue_Value_String = "ApplicationCacheValue_Value_String";
			public const string ApplicationCacheValue_Value_Type = "ApplicationCacheValue_Value_Type";
			public const string ApplicationCacheValue_Value_Type_Select = "ApplicationCacheValue_Value_Type_Select";
			public const string Common_Description = "Common_Description";
			public const string Common_IsPublic = "Common_IsPublic";
			public const string Common_IsPublic_Help = "Common_IsPublic_Help";
			public const string Common_Key = "Common_Key";
			public const string Common_Key_Help = "Common_Key_Help";
			public const string Common_Key_Validation = "Common_Key_Validation";
			public const string Common_Name = "Common_Name";
			public const string Common_Notes = "Common_Notes";
			public const string Common_Script = "Common_Script";
			public const string Common_UniqueId = "Common_UniqueId";
			public const string Connection_Select_Type = "Connection_Select_Type";
			public const string Connection_Type_AMQP = "Connection_Type_AMQP";
			public const string Connection_Type_AzureEventHub = "Connection_Type_AzureEventHub";
			public const string Connection_Type_AzureIoTHub = "Connection_Type_AzureIoTHub";
			public const string Connection_Type_AzureServiceBus = "Connection_Type_AzureServiceBus";
			public const string Connection_Type_Custom = "Connection_Type_Custom";
			public const string Connection_Type_FTP = "Connection_Type_FTP";
			public const string Connection_Type_Kafka = "Connection_Type_Kafka";
			public const string Connection_Type_MQTT_Broker = "Connection_Type_MQTT_Broker";
			public const string Connection_Type_MQTT_Client = "Connection_Type_MQTT_Client";
			public const string Connection_Type_POP3Server = "Connection_Type_POP3Server";
			public const string Connection_Type_Redis = "Connection_Type_Redis";
			public const string Connection_Type_Rest = "Connection_Type_Rest";
			public const string Connection_Type_SharedRest = "Connection_Type_SharedRest";
			public const string Connection_Type_Soap = "Connection_Type_Soap";
			public const string Connection_Type_TCP = "Connection_Type_TCP";
			public const string Connection_Type_UDP = "Connection_Type_UDP";
			public const string Connection_Type_WebSocket = "Connection_Type_WebSocket";
			public const string ConnectionType_MQTT_Listener = "ConnectionType_MQTT_Listener";
			public const string ConnectionType_RabbitMQ = "ConnectionType_RabbitMQ";
			public const string ConnectionType_RabbitMQClient = "ConnectionType_RabbitMQClient";
			public const string ConnectionType_SerialPort = "ConnectionType_SerialPort";
			public const string CusotmModule_CustomModuleType_DotNetAssembly = "CusotmModule_CustomModuleType_DotNetAssembly";
			public const string Custom_AccountId = "Custom_AccountId";
			public const string Custom_AccountPassword = "Custom_AccountPassword";
			public const string Custom_AccountPassword_Help = "Custom_AccountPassword_Help";
			public const string Custom_Uri = "Custom_Uri";
			public const string CustomModule_AuthenticationHeader = "CustomModule_AuthenticationHeader";
			public const string CustomModule_AuthenticationHeader_Help = "CustomModule_AuthenticationHeader_Help";
			public const string CustomModule_AuthenticationType = "CustomModule_AuthenticationType";
			public const string CustomModule_AuthenticationType_Anonymous = "CustomModule_AuthenticationType_Anonymous";
			public const string CustomModule_AuthenticationType_AuthenticationHeader = "CustomModule_AuthenticationType_AuthenticationHeader";
			public const string CustomModule_AuthenticationType_BasicAuth = "CustomModule_AuthenticationType_BasicAuth";
			public const string CustomModule_AuthenticationType_Select = "CustomModule_AuthenticationType_Select";
			public const string CustomModule_ContainerRepository = "CustomModule_ContainerRepository";
			public const string CustomModule_ContainerRepository_Select = "CustomModule_ContainerRepository_Select";
			public const string CustomModule_ContainerTag = "CustomModule_ContainerTag";
			public const string CustomModule_ContainerTag_Select = "CustomModule_ContainerTag_Select";
			public const string CustomModule_CustomModuleType = "CustomModule_CustomModuleType";
			public const string CustomModule_CustomModuleType_Container = "CustomModule_CustomModuleType_Container";
			public const string CustomModule_CustomModuleType_Script = "CustomModule_CustomModuleType_Script";
			public const string CustomModule_CustomModuleType_Select = "CustomModule_CustomModuleType_Select";
			public const string CustomModule_CustomModuleType_WebFunction = "CustomModule_CustomModuleType_WebFunction";
			public const string CustomModule_Description = "CustomModule_Description";
			public const string CustomModule_DotNetAssembly = "CustomModule_DotNetAssembly";
			public const string CustomModule_DotNetClass = "CustomModule_DotNetClass";
			public const string CustomModule_Help = "CustomModule_Help";
			public const string CustomModule_Script = "CustomModule_Script";
			public const string CustomModule_Title = "CustomModule_Title";
			public const string CustomModule_Uri = "CustomModule_Uri";
			public const string CustomModule_Uri_AccountId = "CustomModule_Uri_AccountId";
			public const string CustomModule_Uri_AccountPassword = "CustomModule_Uri_AccountPassword";
			public const string CustomModule_Uri_AccountPassword_Help = "CustomModule_Uri_AccountPassword_Help";
			public const string DataaStream_SummaryLevel = "DataaStream_SummaryLevel";
			public const string DataStream_AutoCreateTable = "DataStream_AutoCreateTable";
			public const string DataStream_AutoCreateTable_Help = "DataStream_AutoCreateTable_Help";
			public const string DataStream_AWSAccessKey = "DataStream_AWSAccessKey";
			public const string DataStream_AWSRegion = "DataStream_AWSRegion";
			public const string DataStream_AWSSecretKey = "DataStream_AWSSecretKey";
			public const string DataStream_AWSSecretKey_Help = "DataStream_AWSSecretKey_Help";
			public const string DataStream_AzureAccessKey = "DataStream_AzureAccessKey";
			public const string DataStream_AzureAccessKeyHelp = "DataStream_AzureAccessKeyHelp";
			public const string DataStream_AzureAccountId = "DataStream_AzureAccountId";
			public const string DataStream_AzureEventHubName = "DataStream_AzureEventHubName";
			public const string DataStream_AzureEventHubPath = "DataStream_AzureEventHubPath";
			public const string DataStream_AzureStorageName = "DataStream_AzureStorageName";
			public const string DataStream_BlobStorage_InvalidName = "DataStream_BlobStorage_InvalidName";
			public const string DataStream_BlobStoragePath = "DataStream_BlobStoragePath";
			public const string DataStream_ConnectionString = "DataStream_ConnectionString";
			public const string DataStream_ConnectionString_Help = "DataStream_ConnectionString_Help";
			public const string DataStream_CreateTableDDL = "DataStream_CreateTableDDL";
			public const string DataStream_CreateTableDDL_Help = "DataStream_CreateTableDDL_Help";
			public const string DataStream_DateStorageFormat = "DataStream_DateStorageFormat";
			public const string DataStream_DateStorageFormat_Help = "DataStream_DateStorageFormat_Help";
			public const string DataStream_DateStorageFormat_Select = "DataStream_DateStorageFormat_Select";
			public const string DataStream_DateStorageFormat_Type_Epoch = "DataStream_DateStorageFormat_Type_Epoch";
			public const string DataStream_DateStorageFormat_Type_ISO8601 = "DataStream_DateStorageFormat_Type_ISO8601";
			public const string DataStream_DbName = "DataStream_DbName";
			public const string DataStream_DbPassword = "DataStream_DbPassword";
			public const string DataStream_DbPassword_Help = "DataStream_DbPassword_Help";
			public const string DataStream_DbSchema = "DataStream_DbSchema";
			public const string DataStream_DbURL = "DataStream_DbURL";
			public const string DataStream_DbUrl_InvalidUrl = "DataStream_DbUrl_InvalidUrl";
			public const string DataStream_DbUserName = "DataStream_DbUserName";
			public const string DataStream_DbValidateSchema = "DataStream_DbValidateSchema";
			public const string DataStream_DbValidateSchema_Help = "DataStream_DbValidateSchema_Help";
			public const string DataStream_Description = "DataStream_Description";
			public const string DataStream_DeviceId_InvalidFormat = "DataStream_DeviceId_InvalidFormat";
			public const string DataStream_DeviceIdFieldName = "DataStream_DeviceIdFieldName";
			public const string DataStream_DeviceIdFieldName_Help = "DataStream_DeviceIdFieldName_Help";
			public const string DataStream_ESDomainName = "DataStream_ESDomainName";
			public const string DataStream_ESDomainName_Invalid = "DataStream_ESDomainName_Invalid";
			public const string DataStream_ESIndexName = "DataStream_ESIndexName";
			public const string DataStream_ESIndexName_Invalid = "DataStream_ESIndexName_Invalid";
			public const string DataStream_ESTypeName = "DataStream_ESTypeName";
			public const string DataStream_ESTypeNameInvalid = "DataStream_ESTypeNameInvalid";
			public const string DataStream_Fields = "DataStream_Fields";
			public const string DataStream_Help = "DataStream_Help";
			public const string DataStream_InvalidBucketName = "DataStream_InvalidBucketName";
			public const string DataStream_InvalidEHName = "DataStream_InvalidEHName";
			public const string DataStream_InvalidEHPathName = "DataStream_InvalidEHPathName";
			public const string DataStream_InvalidTableName = "DataStream_InvalidTableName";
			public const string DataStream_RedisPassword = "DataStream_RedisPassword";
			public const string DataStream_RedisPassword_Help = "DataStream_RedisPassword_Help";
			public const string DataStream_RedisServers = "DataStream_RedisServers";
			public const string DataStream_RedisServers_Help = "DataStream_RedisServers_Help";
			public const string DataStream_S3_BucketName = "DataStream_S3_BucketName";
			public const string DataStream_SharedConnection = "DataStream_SharedConnection";
			public const string DataStream_SharedConnection_Select = "DataStream_SharedConnection_Select";
			public const string DataStream_StreamType = "DataStream_StreamType";
			public const string DataStream_StreamType_AWS_ElasticSearch = "DataStream_StreamType_AWS_ElasticSearch";
			public const string DataStream_StreamType_AWS_S3 = "DataStream_StreamType_AWS_S3";
			public const string DataStream_StreamType_AzureBlob = "DataStream_StreamType_AzureBlob";
			public const string DataStream_StreamType_AzureBlob_Managed = "DataStream_StreamType_AzureBlob_Managed";
			public const string DataStream_StreamType_AzureEventHub = "DataStream_StreamType_AzureEventHub";
			public const string DataStream_StreamType_AzureEventHub_Managegd = "DataStream_StreamType_AzureEventHub_Managegd";
			public const string DataStream_StreamType_DataLake = "DataStream_StreamType_DataLake";
			public const string DataStream_StreamType_GeoSpatial = "DataStream_StreamType_GeoSpatial";
			public const string DataStream_StreamType_PointArrayStorage = "DataStream_StreamType_PointArrayStorage";
			public const string DataStream_StreamType_PostgreSQL = "DataStream_StreamType_PostgreSQL";
			public const string DataStream_StreamType_Redis = "DataStream_StreamType_Redis";
			public const string DataStream_StreamType_Select = "DataStream_StreamType_Select";
			public const string DataStream_StreamType_SQLServer = "DataStream_StreamType_SQLServer";
			public const string DataStream_StreamType_TableStorage = "DataStream_StreamType_TableStorage";
			public const string DataStream_StreamType_TableStorage_Managed = "DataStream_StreamType_TableStorage_Managed";
			public const string DataStream_SummaryData = "DataStream_SummaryData";
			public const string DataStream_SummaryData_Help = "DataStream_SummaryData_Help";
			public const string DataStream_SummaryLevel_Help = "DataStream_SummaryLevel_Help";
			public const string DataStream_TableName = "DataStream_TableName";
			public const string DataStream_TableStorage_InvalidName = "DataStream_TableStorage_InvalidName";
			public const string DataStream_TableStorageName = "DataStream_TableStorageName";
			public const string DataStream_TimeStamp_InvalidFormat = "DataStream_TimeStamp_InvalidFormat";
			public const string DataStream_TimeStampFieldName = "DataStream_TimeStampFieldName";
			public const string DataStream_TimeStampFieldName_Help = "DataStream_TimeStampFieldName_Help";
			public const string DataStream_Title = "DataStream_Title";
			public const string DataStreamField_DataType = "DataStreamField_DataType";
			public const string DataStreamField_DataType_Help = "DataStreamField_DataType_Help";
			public const string DataStreamField_DataType_Select = "DataStreamField_DataType_Select";
			public const string DataStreamField_Description = "DataStreamField_Description";
			public const string DataStreamField_FieldName = "DataStreamField_FieldName";
			public const string DataStreamField_FieldName_Help = "DataStreamField_FieldName_Help";
			public const string DataStreamField_FieldName_Invalid = "DataStreamField_FieldName_Invalid";
			public const string DataStreamField_Help = "DataStreamField_Help";
			public const string DataStreamField_IsDatabaseGenerated = "DataStreamField_IsDatabaseGenerated";
			public const string DataStreamField_IsDatabaseGenerated_Help = "DataStreamField_IsDatabaseGenerated_Help";
			public const string DataStreamField_IsKey = "DataStreamField_IsKey";
			public const string DataStreamField_IsKey_Description = "DataStreamField_IsKey_Description";
			public const string DataStreamField_IsRequired = "DataStreamField_IsRequired";
			public const string DataStreamField_IsRequired_Help = "DataStreamField_IsRequired_Help";
			public const string DataStreamField_MaxValue = "DataStreamField_MaxValue";
			public const string DataStreamField_MaxValue_Help = "DataStreamField_MaxValue_Help";
			public const string DataStreamField_MinValue = "DataStreamField_MinValue";
			public const string DataStreamField_MinValue_Help = "DataStreamField_MinValue_Help";
			public const string DataStreamField_NumberDecimalPoints = "DataStreamField_NumberDecimalPoints";
			public const string DataStreamField_NumberDecimalPoints_Help = "DataStreamField_NumberDecimalPoints_Help";
			public const string DataStreamField_RegEx = "DataStreamField_RegEx";
			public const string DataStreamField_RegEx_Help = "DataStreamField_RegEx_Help";
			public const string DataStreamField_StateSet = "DataStreamField_StateSet";
			public const string DataStreamField_StateSet_Watermark = "DataStreamField_StateSet_Watermark";
			public const string DataStreamField_Title = "DataStreamField_Title";
			public const string DataStreamField_UnitSet = "DataStreamField_UnitSet";
			public const string DataStreamField_UnitSet_Watermark = "DataStreamField_UnitSet_Watermark";
			public const string Dictionary_Description = "Dictionary_Description";
			public const string Dictionary_Help = "Dictionary_Help";
			public const string Dictionary_Password = "Dictionary_Password";
			public const string Dictionary_Password_Help = "Dictionary_Password_Help";
			public const string Dictionary_Title = "Dictionary_Title";
			public const string Dictionary_Type = "Dictionary_Type";
			public const string Dictionary_Type_NuvIoT = "Dictionary_Type_NuvIoT";
			public const string Dictionary_Type_Redis = "Dictionary_Type_Redis";
			public const string Dictionary_Type_Select = "Dictionary_Type_Select";
			public const string Dictionary_Uri = "Dictionary_Uri";
			public const string Err_CouldNotLoadCustomModule = "Err_CouldNotLoadCustomModule";
			public const string Err_CouldNotLoadDataStream = "Err_CouldNotLoadDataStream";
			public const string Err_CouldNotLoadInputTranslator = "Err_CouldNotLoadInputTranslator";
			public const string Err_CouldNotLoadListener = "Err_CouldNotLoadListener";
			public const string Err_CouldNotLoadOutputTranslator = "Err_CouldNotLoadOutputTranslator";
			public const string Err_CouldNotLoadPlanner = "Err_CouldNotLoadPlanner";
			public const string Err_CouldNotLoadSentinel = "Err_CouldNotLoadSentinel";
			public const string Err_CouldNotLoadTransmitter = "Err_CouldNotLoadTransmitter";
			public const string Err_TransmitterTypeIsRequired = "Err_TransmitterTypeIsRequired";
			public const string InputTranslator_DelimiterSquence_Help = "InputTranslator_DelimiterSquence_Help";
			public const string InputTranslator_DelimterSequence = "InputTranslator_DelimterSequence";
			public const string InputTranslator_Description = "InputTranslator_Description";
			public const string InputTranslator_Help = "InputTranslator_Help";
			public const string InputTranslator_Model = "InputTranslator_Model";
			public const string InputTranslator_Model_Select = "InputTranslator_Model_Select";
			public const string InputTranslator_ModelRevision = "InputTranslator_ModelRevision";
			public const string InputTranslator_ModelRevision_Select = "InputTranslator_ModelRevision_Select";
			public const string InputTranslator_SevenSegmentParser = "InputTranslator_SevenSegmentParser";
			public const string InputTranslator_SevenSegmentParser_Select = "InputTranslator_SevenSegmentParser_Select";
			public const string InputTranslator_Title = "InputTranslator_Title";
			public const string InputTranslator_TranslatorType = "InputTranslator_TranslatorType";
			public const string InputTranslator_TranslatorType_Select = "InputTranslator_TranslatorType_Select";
			public const string Listener_AccessKey = "Listener_AccessKey";
			public const string Listener_AccessKey_Help = "Listener_AccessKey_Help";
			public const string Listener_AccessKeyName = "Listener_AccessKeyName";
			public const string Listener_AccessKeyName_Help = "Listener_AccessKeyName_Help";
			public const string Listener_Anonymous = "Listener_Anonymous";
			public const string Listener_ConnectSSLTLS = "Listener_ConnectSSLTLS";
			public const string Listener_ConnectToPort = "Listener_ConnectToPort";
			public const string Listener_ConsumerGroup_Help = "Listener_ConsumerGroup_Help";
			public const string Listener_DefaultResponse = "Listener_DefaultResponse";
			public const string Listener_DelimitedWithSOH_EOT = "Listener_DelimitedWithSOH_EOT";
			public const string Listener_DelimitedWithSOH_EOT_Help = "Listener_DelimitedWithSOH_EOT_Help";
			public const string Listener_Description = "Listener_Description";
			public const string Listener_EndMessageSequence = "Listener_EndMessageSequence";
			public const string Listener_EndMessageSequence_Help = "Listener_EndMessageSequence_Help";
			public const string Listener_Endpoint = "Listener_Endpoint";
			public const string Listener_EventHub_ConsumerGroup = "Listener_EventHub_ConsumerGroup";
			public const string Listener_ExchangeName = "Listener_ExchangeName";
			public const string Listener_FailedResponse = "Listener_FailedResponse";
			public const string Listener_Help = "Listener_Help";
			public const string Listener_HostName = "Listener_HostName";
			public const string Listener_HubName = "Listener_HubName";
			public const string Listener_HubName_Help = "Listener_HubName_Help";
			public const string Listener_KeepAliveToSendReply = "Listener_KeepAliveToSendReply";
			public const string Listener_KeepAliveToSendReply_Timeout = "Listener_KeepAliveToSendReply_Timeout";
			public const string Listener_KeepAliveToSendReplyTimeout_Help = "Listener_KeepAliveToSendReplyTimeout_Help";
			public const string Listener_Length_Endiness = "Listener_Length_Endiness";
			public const string Listener_Length_Endiness_Help = "Listener_Length_Endiness_Help";
			public const string Listener_Length_Endiness_Select = "Listener_Length_Endiness_Select";
			public const string Listener_Length_Location = "Listener_Length_Location";
			public const string Listener_Length_Location_Help = "Listener_Length_Location_Help";
			public const string Listener_Length_LocationByteLength = "Listener_Length_LocationByteLength";
			public const string Listener_Length_LocationByteLength_Help = "Listener_Length_LocationByteLength_Help";
			public const string Listener_ListenerType = "Listener_ListenerType";
			public const string Listener_ListenOnPort = "Listener_ListenOnPort";
			public const string Listener_MaxMessageSize = "Listener_MaxMessageSize";
			public const string Listener_MaxMessageSize_Help = "Listener_MaxMessageSize_Help";
			public const string Listener_MessageContainsLength = "Listener_MessageContainsLength";
			public const string Listener_MessageContainsLength_Help = "Listener_MessageContainsLength_Help";
			public const string Listener_MessageLength_1_Byte = "Listener_MessageLength_1_Byte";
			public const string Listener_MessageLength_2_Bytes = "Listener_MessageLength_2_Bytes";
			public const string Listener_MessageLength_4_Bytes = "Listener_MessageLength_4_Bytes";
			public const string Listener_MessageLength_Select = "Listener_MessageLength_Select";
			public const string Listener_MessageReceivedTimeout = "Listener_MessageReceivedTimeout";
			public const string Listener_MessageReceivedTimeout_Help = "Listener_MessageReceivedTimeout_Help";
			public const string Listener_Origin = "Listener_Origin";
			public const string Listener_Password = "Listener_Password";
			public const string Listener_Password_Help = "Listener_Password_Help";
			public const string Listener_Path = "Listener_Path";
			public const string Listener_Port_Help = "Listener_Port_Help";
			public const string Listener_Queue = "Listener_Queue";
			public const string Listener_Queue_Help = "Listener_Queue_Help";
			public const string Listener_ResourceName = "Listener_ResourceName";
			public const string Listener_RESTServerType = "Listener_RESTServerType";
			public const string Listener_RESTServerType_HTTP = "Listener_RESTServerType_HTTP";
			public const string Listener_RESTServerType_HTTPorHTTPS = "Listener_RESTServerType_HTTPorHTTPS";
			public const string Listener_RESTServerType_HTTPS = "Listener_RESTServerType_HTTPS";
			public const string Listener_RESTServerType_Select = "Listener_RESTServerType_Select";
			public const string Listener_StartMessageSequence = "Listener_StartMessageSequence";
			public const string Listener_StartMessageSequence_Help = "Listener_StartMessageSequence_Help";
			public const string Listener_Subscription = "Listener_Subscription";
			public const string Listener_Subscriptions = "Listener_Subscriptions";
			public const string Listener_SupportedProtocol = "Listener_SupportedProtocol";
			public const string Listener_Title = "Listener_Title";
			public const string Listener_Topic = "Listener_Topic";
			public const string Listener_UserName = "Listener_UserName";
			public const string Listener_UserName_Help = "Listener_UserName_Help";
			public const string MessageFieldParseConfiguration_BinaryOffset_Help = "MessageFieldParseConfiguration_BinaryOffset_Help";
			public const string MessageFieldParserConfiguration_BinaryOffset = "MessageFieldParserConfiguration_BinaryOffset";
			public const string MessageFieldParserConfiguration_BinaryType = "MessageFieldParserConfiguration_BinaryType";
			public const string MessageFieldParserConfiguration_BinaryType_Boolean = "MessageFieldParserConfiguration_BinaryType_Boolean";
			public const string MessageFieldParserConfiguration_BinaryType_Char = "MessageFieldParserConfiguration_BinaryType_Char";
			public const string MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint = "MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint";
			public const string MessageFieldParserConfiguration_BinaryType_Int16BigEndian = "MessageFieldParserConfiguration_BinaryType_Int16BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int16LittleEndian = "MessageFieldParserConfiguration_BinaryType_Int16LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int32BigEndian = "MessageFieldParserConfiguration_BinaryType_Int32BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int32LittleEndian = "MessageFieldParserConfiguration_BinaryType_Int32LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int64BigEndian = "MessageFieldParserConfiguration_BinaryType_Int64BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int64LittleEndian = "MessageFieldParserConfiguration_BinaryType_Int64LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_Int8 = "MessageFieldParserConfiguration_BinaryType_Int8";
			public const string MessageFieldParserConfiguration_BinaryType_SelectDataType = "MessageFieldParserConfiguration_BinaryType_SelectDataType";
			public const string MessageFieldParserConfiguration_BinaryType_SelectDataType_Help = "MessageFieldParserConfiguration_BinaryType_SelectDataType_Help";
			public const string MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint = "MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint";
			public const string MessageFieldParserConfiguration_BinaryType_String = "MessageFieldParserConfiguration_BinaryType_String";
			public const string MessageFieldParserConfiguration_BinaryType_UInt16BigEndian = "MessageFieldParserConfiguration_BinaryType_UInt16BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian = "MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt32BigEndian = "MessageFieldParserConfiguration_BinaryType_UInt32BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian = "MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt64BigEndian = "MessageFieldParserConfiguration_BinaryType_UInt64BigEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian = "MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian";
			public const string MessageFieldParserConfiguration_BinaryType_UInt8 = "MessageFieldParserConfiguration_BinaryType_UInt8";
			public const string MessageFieldParserConfiguration_DelimitedColumnIndex = "MessageFieldParserConfiguration_DelimitedColumnIndex";
			public const string MessageFieldParserConfiguration_DelimitedColumnIndex_Help = "MessageFieldParserConfiguration_DelimitedColumnIndex_Help";
			public const string MessageFieldParserConfiguration_Delimiter = "MessageFieldParserConfiguration_Delimiter";
			public const string MessageFieldParserConfiguration_Delimitor_Help = "MessageFieldParserConfiguration_Delimitor_Help";
			public const string MessageFieldParserConfiguration_Description = "MessageFieldParserConfiguration_Description";
			public const string MessageFieldParserConfiguration_Help = "MessageFieldParserConfiguration_Help";
			public const string MessageFieldParserConfiguration_KeyName = "MessageFieldParserConfiguration_KeyName";
			public const string MessageFieldParserConfiguration_KeyName_Help = "MessageFieldParserConfiguration_KeyName_Help";
			public const string MessageFieldParserConfiguration_Length = "MessageFieldParserConfiguration_Length";
			public const string MessageFieldParserConfiguration_OutputType = "MessageFieldParserConfiguration_OutputType";
			public const string MessageFieldParserConfiguration_OutputType_Boolean = "MessageFieldParserConfiguration_OutputType_Boolean";
			public const string MessageFieldParserConfiguration_OutputType_FloatingPoint = "MessageFieldParserConfiguration_OutputType_FloatingPoint";
			public const string MessageFieldParserConfiguration_OutputType_Integer = "MessageFieldParserConfiguration_OutputType_Integer";
			public const string MessageFieldParserConfiguration_OutputType_SelectDataType = "MessageFieldParserConfiguration_OutputType_SelectDataType";
			public const string MessageFieldParserConfiguration_OutputType_SelectDataType_Help = "MessageFieldParserConfiguration_OutputType_SelectDataType_Help";
			public const string MessageFieldParserConfiguration_OutputType_String = "MessageFieldParserConfiguration_OutputType_String";
			public const string MessageFieldParserConfiguration_ParserStrategy = "MessageFieldParserConfiguration_ParserStrategy";
			public const string MessageFieldParserConfiguration_ParserStrategy_Delimited = "MessageFieldParserConfiguration_ParserStrategy_Delimited";
			public const string MessageFieldParserConfiguration_ParserStrategy_Header = "MessageFieldParserConfiguration_ParserStrategy_Header";
			public const string MessageFieldParserConfiguration_ParserStrategy_Help = "MessageFieldParserConfiguration_ParserStrategy_Help";
			public const string MessageFieldParserConfiguration_ParserStrategy_JsonProperty = "MessageFieldParserConfiguration_ParserStrategy_JsonProperty";
			public const string MessageFieldParserConfiguration_ParserStrategy_PathLocator = "MessageFieldParserConfiguration_ParserStrategy_PathLocator";
			public const string MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help = "MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help";
			public const string MessageFieldParserConfiguration_ParserStrategy_Position = "MessageFieldParserConfiguration_ParserStrategy_Position";
			public const string MessageFieldParserConfiguration_ParserStrategy_QueryString = "MessageFieldParserConfiguration_ParserStrategy_QueryString";
			public const string MessageFieldParserConfiguration_ParserStrategy_QueryStringField = "MessageFieldParserConfiguration_ParserStrategy_QueryStringField";
			public const string MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help = "MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help";
			public const string MessageFieldParserConfiguration_ParserStrategy_RegEx = "MessageFieldParserConfiguration_ParserStrategy_RegEx";
			public const string MessageFieldParserConfiguration_ParserStrategy_RegExHeader = "MessageFieldParserConfiguration_ParserStrategy_RegExHeader";
			public const string MessageFieldParserConfiguration_ParserStrategy_RegExHeader_Help = "MessageFieldParserConfiguration_ParserStrategy_RegExHeader_Help";
			public const string MessageFieldParserConfiguration_ParserStrategy_Script = "MessageFieldParserConfiguration_ParserStrategy_Script";
			public const string MessageFieldParserConfiguration_ParserStrategy_Select = "MessageFieldParserConfiguration_ParserStrategy_Select";
			public const string MessageFieldParserConfiguration_ParserStrategy_Substring = "MessageFieldParserConfiguration_ParserStrategy_Substring";
			public const string MessageFieldParserConfiguration_ParserStrategy_URIPath = "MessageFieldParserConfiguration_ParserStrategy_URIPath";
			public const string MessageFieldParserConfiguration_ParserStrategy_XMLProperty = "MessageFieldParserConfiguration_ParserStrategy_XMLProperty";
			public const string MessageFieldParserConfiguration_QuotedText = "MessageFieldParserConfiguration_QuotedText";
			public const string MessageFieldParserConfiguration_QuotedText_Help = "MessageFieldParserConfiguration_QuotedText_Help";
			public const string MessageFieldParserConfiguration_RegExGroupName = "MessageFieldParserConfiguration_RegExGroupName";
			public const string MessageFieldParserConfiguration_RegExGroupName_Help = "MessageFieldParserConfiguration_RegExGroupName_Help";
			public const string MessageFieldParserConfiguration_RegExLocator = "MessageFieldParserConfiguration_RegExLocator";
			public const string MessageFieldParserConfiguration_RegExLocator_Help = "MessageFieldParserConfiguration_RegExLocator_Help";
			public const string MessageFieldParserConfiguration_RegExValidation = "MessageFieldParserConfiguration_RegExValidation";
			public const string MessageFieldParserConfiguration_RegExValidation_Help = "MessageFieldParserConfiguration_RegExValidation_Help";
			public const string MessageFieldParserConfiguration_StartIndex = "MessageFieldParserConfiguration_StartIndex";
			public const string MessageFieldParserConfiguration_StringParser_NumberBytes = "MessageFieldParserConfiguration_StringParser_NumberBytes";
			public const string MessageFieldParserConfiguration_StringParser_NumberBytes_Help = "MessageFieldParserConfiguration_StringParser_NumberBytes_Help";
			public const string MessageFieldParserConfiguration_StringParserType = "MessageFieldParserConfiguration_StringParserType";
			public const string MessageFieldParserConfiguration_StringParserType_Help = "MessageFieldParserConfiguration_StringParserType_Help";
			public const string MessageFieldParserConfiguration_StringParserType_LeadingLength = "MessageFieldParserConfiguration_StringParserType_LeadingLength";
			public const string MessageFieldParserConfiguration_StringParserType_NullTerminated = "MessageFieldParserConfiguration_StringParserType_NullTerminated";
			public const string MessageFieldParserConfiguration_StringParserType_Select = "MessageFieldParserConfiguration_StringParserType_Select";
			public const string MessageFieldParserConfiguration_SubString_Help = "MessageFieldParserConfiguration_SubString_Help";
			public const string MessageFieldParserConfiguration_Title = "MessageFieldParserConfiguration_Title";
			public const string MessageFieldParserConfiguration_ValueName = "MessageFieldParserConfiguration_ValueName";
			public const string MessageFieldParserConfiguration_ValueName_Help = "MessageFieldParserConfiguration_ValueName_Help";
			public const string MessageFieldParserConfiguration_XPath = "MessageFieldParserConfiguration_XPath";
			public const string MessageFieldParserConfiguration_XPath_Help = "MessageFieldParserConfiguration_XPath_Help";
			public const string OutputTranslator_Description = "OutputTranslator_Description";
			public const string OutputTranslator_Help = "OutputTranslator_Help";
			public const string OutputTranslator_Title = "OutputTranslator_Title";
			public const string OutputTranslator_TranslatorType = "OutputTranslator_TranslatorType";
			public const string OutputTranslator_TranslatorType_Select = "OutputTranslator_TranslatorType_Select";
			public const string PipelineModuleType_Custom = "PipelineModuleType_Custom";
			public const string PipelineModuleType_DataStream = "PipelineModuleType_DataStream";
			public const string PipelineModuleType_InputTranslator = "PipelineModuleType_InputTranslator";
			public const string PipelineModuleType_Listener = "PipelineModuleType_Listener";
			public const string PipelineModuleType_OutputTranslator = "PipelineModuleType_OutputTranslator";
			public const string PipelineModuleType_Planner = "PipelineModuleType_Planner";
			public const string PipelineModuleType_Sentinel = "PipelineModuleType_Sentinel";
			public const string PipelineModuleType_Transmitter = "PipelineModuleType_Transmitter";
			public const string PipelineModuleType_Workflow = "PipelineModuleType_Workflow";
			public const string Planner_Description = "Planner_Description";
			public const string Planner_DeviceIDParsers = "Planner_DeviceIDParsers";
			public const string Planner_DeviceIDParsers_Help = "Planner_DeviceIDParsers_Help";
			public const string Planner_Help = "Planner_Help";
			public const string Planner_MessageTypeIDParsers = "Planner_MessageTypeIDParsers";
			public const string Planner_MessageTypeIDParsers_Help = "Planner_MessageTypeIDParsers_Help";
			public const string Planner_PipelineModules = "Planner_PipelineModules";
			public const string Planner_Title = "Planner_Title";
			public const string Sentinel_Description = "Sentinel_Description";
			public const string Sentinel_Help = "Sentinel_Help";
			public const string Sentinel_SecurityField = "Sentinel_SecurityField";
			public const string Sentinel_SecurityField_Description = "Sentinel_SecurityField_Description";
			public const string Sentinel_SecurityField_Help = "Sentinel_SecurityField_Help";
			public const string Sentinel_SecurityField_Locator = "Sentinel_SecurityField_Locator";
			public const string Sentinel_SecurityField_Locator_Help = "Sentinel_SecurityField_Locator_Help";
			public const string Sentinel_SecurityField_Locator_Select = "Sentinel_SecurityField_Locator_Select";
			public const string Sentinel_SecurityField_Title = "Sentinel_SecurityField_Title";
			public const string Sentinel_SecurityField_Title_Help = "Sentinel_SecurityField_Title_Help";
			public const string Sentinel_SecurityField_Type = "Sentinel_SecurityField_Type";
			public const string Sentinel_SecurityField_Type_AccessKey = "Sentinel_SecurityField_Type_AccessKey";
			public const string Sentinel_SecurityField_Type_BasicAuth = "Sentinel_SecurityField_Type_BasicAuth";
			public const string Sentinel_SecurityField_Type_Help = "Sentinel_SecurityField_Type_Help";
			public const string Sentinel_SecurityField_Type_Script = "Sentinel_SecurityField_Type_Script";
			public const string Sentinel_SecurityField_Type_Select = "Sentinel_SecurityField_Type_Select";
			public const string Sentinel_SecurityField_Type_SharedSignature = "Sentinel_SecurityField_Type_SharedSignature";
			public const string Sentinel_Title = "Sentinel_Title";
			public const string SerialPort_BaudRate = "SerialPort_BaudRate";
			public const string SerialPort_PortName = "SerialPort_PortName";
			public const string SharedConnection_AWS = "SharedConnection_AWS";
			public const string SharedConnection_Azure = "SharedConnection_Azure";
			public const string SharedConnection_ConnectionType = "SharedConnection_ConnectionType";
			public const string SharedConnection_ConnectionType_Select = "SharedConnection_ConnectionType_Select";
			public const string SharedConnection_Database = "SharedConnection_Database";
			public const string SharedConnection_Description = "SharedConnection_Description";
			public const string SharedConnection_Help = "SharedConnection_Help";
			public const string SharedConnection_Redis = "SharedConnection_Redis";
			public const string SharedConnection_Title = "SharedConnection_Title";
			public const string Translator_Type_Binary = "Translator_Type_Binary";
			public const string Translator_Type_Custom = "Translator_Type_Custom";
			public const string Translator_Type_Delimited = "Translator_Type_Delimited";
			public const string Translator_Type_JSON = "Translator_Type_JSON";
			public const string Translator_Type_Message = "Translator_Type_Message";
			public const string Translator_Type_String = "Translator_Type_String";
			public const string Translator_Type_XML = "Translator_Type_XML";
			public const string TranslatorType_AIModel = "TranslatorType_AIModel";
			public const string TranslatorType_SevenSegmentParser = "TranslatorType_SevenSegmentParser";
			public const string Transmitter_Description = "Transmitter_Description";
			public const string Transmitter_Headers = "Transmitter_Headers";
			public const string Transmitter_Help = "Transmitter_Help";
			public const string Transmitter_Title = "Transmitter_Title";
			public const string Transmitter_TransmitterType = "Transmitter_TransmitterType";
			public const string Transmitter_TransmitterType_OriginalListener = "Transmitter_TransmitterType_OriginalListener";
			public const string Transmitter_TransmitterType_Outbox = "Transmitter_TransmitterType_Outbox";
			public const string Transmitter_TransmitterType_SerialPort = "Transmitter_TransmitterType_SerialPort";
			public const string Transmitter_TransmitterType_SMS = "Transmitter_TransmitterType_SMS";
			public const string Transmitter_TransmitterType_SMTP = "Transmitter_TransmitterType_SMTP";
		}
	}
}

