using LagoVista.Core;
using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.UserAdmin.Interfaces.Managers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class DataStreamManager : ManagerBase, IDataStreamManager
    {
        IDataStreamRepo _dataStreamRepo;
        ISecureStorage _secureStorage;
        IDefaultInternalDataStreamConnectionSettings _defaultConnectionSettings;
        IOrganizationManager _orgManager;

        public DataStreamManager(IDataStreamRepo dataStreamRepo, IDefaultInternalDataStreamConnectionSettings defaultConnectionSettings, IOrganizationManager orgManager, IAdminLogger logger, ISecureStorage secureStorage, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _dataStreamRepo = dataStreamRepo;
            _secureStorage = secureStorage;
            _defaultConnectionSettings = defaultConnectionSettings;
            _orgManager = orgManager;
        }

        public async Task<InvokeResult> AddDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Create, user, org);
            if (stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                stream.AzureStorageAccountName = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccountId;
                stream.AzureAccessKey = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccessKey;
            }

            ValidationCheck(stream, Actions.Create);

            if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                if (!String.IsNullOrEmpty(stream.AzureAccessKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.AzureAccessKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    stream.AzureAccessKeySecureId = addSecretResult.Result;
                    stream.AzureAccessKey = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty AzureAccessKey, but it did not.");
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
                stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
            {
                if (!String.IsNullOrEmpty(stream.AwsSecretKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.AwsSecretKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    stream.AWSSecretKeySecureId = addSecretResult.Result;
                    stream.AwsSecretKey = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty AWSSecretKey, but it did not.");
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.SQLServer ||
                     stream.StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (!String.IsNullOrEmpty(stream.DbPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.DbPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    stream.DBPasswordSecureId = addSecretResult.Result;
                    stream.DbPassword = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty DbPassword, but it did not.");
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.Redis)
            {
                if (!String.IsNullOrEmpty(stream.RedisPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.RedisPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    stream.RedisPasswordSecureId = addSecretResult.Result;
                    stream.RedisPassword = null;
                }
            }
            else if(stream.StreamType.Value == DataStreamTypes.PointArrayStorage)
            {
                var orgDetails = await _orgManager.GetOrganizationAsync(org.Id, org, user);

                stream.DeviceIdFieldName = "deviceId";
                stream.TimestampFieldName = "timeStamp";
                stream.DbSchema = "public";
                stream.DbURL = _defaultConnectionSettings.PointArrayConnectionSettings.Uri;
                stream.CreateTableDDL = GetPointArrayDataStorageSQL_DDL();
                stream.DatabaseName = orgDetails.Namespace;
                stream.DbTableName = $"point_array_{stream.Key}";
                stream.DbUserName = orgDetails.Namespace;

                var dbPassword = Guid.NewGuid().ToId();

                var addSecretResult = await _secureStorage.AddSecretAsync(org, $"ps_db_uid_{org.Id}", dbPassword);
                if (!addSecretResult.Successful)
                {
                    return addSecretResult.ToInvokeResult();
                }

                stream.DBPasswordSecureId = addSecretResult.Result;
            }
            else
            {
                throw new Exception("New data stream Type was added, should likely add something here to store credentials.");
            }

            await _dataStreamRepo.AddDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        private async Task<InvokeResult> CreatePostgresUser(DataStream stream, String dbPassword)
        {
            var connString = $"Host={_defaultConnectionSettings.PointArrayConnectionSettings.Uri};Username={_defaultConnectionSettings.PointArrayConnectionSettings.UserName};Password={_defaultConnectionSettings.PointArrayConnectionSettings.Password};";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                cmd.Parameters.AddWithValue("@userName", stream.DbUserName);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    cmd.Parameters.Clear();
                    
                    cmd.CommandText = $"CREATE USER {stream.DbUserName} ;";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                    result = await cmd.ExecuteScalarAsync();
                }

                cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    cmd.Parameters.Clear();

                    cmd.CommandText = $"CREATE DATABASE {stream.DbName};";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                    result = await cmd.ExecuteScalarAsync();
                }


                conn.Close();
                return InvokeResult.Success;
            }
          
        }

        private string GetPointArrayDataStorageSQL_DDL()
        {
            return @"CREATE TABLE if not exists public.hvacdata(
                        id SERIAL,
                        deviceId text not null,
                        timeStamp timestamp not null,
                        sensorIndex smallint not null,
                        value float4 not null
                    );";
        }

        public async Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            try
            {
                var stream = await _dataStreamRepo.GetDataStreamAsync(id);

                if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                    stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                    stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                    stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
                {
                    if (String.IsNullOrEmpty(stream.AzureAccessKeySecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var azureSecretKeyResult = await _secureStorage.GetSecretAsync(org, stream.AzureAccessKeySecureId, user);
                    if (!azureSecretKeyResult.Successful)
                    {
                        return InvokeResult<DataStream>.FromInvokeResult(azureSecretKeyResult.ToInvokeResult());
                    }

                    stream.AzureAccessKey = azureSecretKeyResult.Result;
                }
                else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
                    stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
                {
                    if (String.IsNullOrEmpty(stream.AWSSecretKeySecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var awsSecretKeyResult = await _secureStorage.GetSecretAsync(org, stream.AWSSecretKeySecureId, user);
                    if (!awsSecretKeyResult.Successful)
                    {
                        return InvokeResult<DataStream>.FromInvokeResult(awsSecretKeyResult.ToInvokeResult());
                    }

                    stream.AwsSecretKey = awsSecretKeyResult.Result;
                }
                else if (stream.StreamType.Value == DataStreamTypes.SQLServer ||
                         stream.StreamType.Value == DataStreamTypes.Postgresql)
                {
                    if (String.IsNullOrEmpty(stream.DBPasswordSecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var dbSecretKeyResult = await _secureStorage.GetSecretAsync(org, stream.DBPasswordSecureId, user);
                    if (!dbSecretKeyResult.Successful)
                    {
                        return InvokeResult<DataStream>.FromInvokeResult(dbSecretKeyResult.ToInvokeResult());
                    }

                    stream.DbPassword = dbSecretKeyResult.Result;
                }
                else if (stream.StreamType.Value == DataStreamTypes.Redis)
                {
                    if (!String.IsNullOrEmpty(stream.RedisPasswordSecureId))
                    {
                        var getSecretResult = await _secureStorage.GetSecretAsync(org, stream.RedisPasswordSecureId, user);
                        if (!getSecretResult.Successful)
                        {
                            return InvokeResult<DataStream>.FromInvokeResult(getSecretResult.ToInvokeResult());
                        }

                        stream.RedisPassword = getSecretResult.Result;
                    }
                }
                else if (stream.StreamType.Value == DataStreamTypes.Postgresql)
                {
                    if (!String.IsNullOrEmpty(stream.DBPasswordSecureId))
                    {
                        var getSecretResult = await _secureStorage.GetSecretAsync(org, stream.DBPasswordSecureId, user);
                        if (!getSecretResult.Successful)
                        {
                            return InvokeResult<DataStream>.FromInvokeResult(getSecretResult.ToInvokeResult());
                        }

                        stream.DBPasswordSecureId = getSecretResult.Result;
                    }
                }

                return InvokeResult<DataStream>.Create(stream);
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<DataStream>.FromErrors(ErrorCodes.CouldNotLoadDataStreamModule.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<DependentObjectCheckResult> CheckDataStreamInUseAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(dataStream);
        }

        public async Task<InvokeResult> DeleteDatStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Delete, user, org);
            await CheckForDepenenciesAsync(dataStream);

            await _dataStreamRepo.DeleteDataStreamAsync(dataStreamId);

            return InvokeResult.Success;
        }

        public async Task<DataStream> GetDataStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Read, user, org);
            return dataStream;
        }

        public async Task<IEnumerable<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(DataStreamSummary));
            return await _dataStreamRepo.GetDataStreamsForOrgAsync(orgId);
        }

        public Task<bool> QueryKeyInUseAsync(string key, EntityHeader org)
        {
            return _dataStreamRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Update, user, org);
            ValidationCheck(stream, Actions.Update);

            if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                if (!String.IsNullOrEmpty(stream.AzureAccessKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.AzureAccessKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(stream.AzureAccessKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, stream.AzureAccessKeySecureId);
                    }

                    stream.AzureAccessKeySecureId = addSecretResult.Result;
                    stream.AzureAccessKey = null;
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
               stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
            {
                if (!String.IsNullOrEmpty(stream.AwsSecretKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.AwsSecretKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(stream.AWSSecretKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, stream.AWSSecretKeySecureId);
                    }

                    stream.AWSSecretKeySecureId = addSecretResult.Result;
                    stream.AwsSecretKey = null;
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.SQLServer ||
                    stream.StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (!String.IsNullOrEmpty(stream.DbPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.DbPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(stream.DBPasswordSecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, stream.DBPasswordSecureId);
                    }

                    stream.DBPasswordSecureId = addSecretResult.Result;
                    stream.DbPassword = null;
                }
            }
            else if(stream.StreamType.Value == DataStreamTypes.Redis)
            {
                if (!String.IsNullOrEmpty(stream.RedisPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.RedisPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(stream.RedisPasswordSecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, stream.RedisPasswordSecureId);
                    }

                    stream.RedisPasswordSecureId = addSecretResult.Result;
                    stream.RedisPassword = null;
                }
            }

            await _dataStreamRepo.UpdateDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        public async Task<ListResponse<DataStreamResult>> GetStreamDataAsync(DataStream stream, IDataStreamConnector connector, string deviceId, EntityHeader org, EntityHeader user, ListRequest request)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Read, user, org, "ReadDeviceData");

            await connector.InitAsync(stream);
            return await connector.GetItemsAsync(deviceId, request);
        }
    }
}
