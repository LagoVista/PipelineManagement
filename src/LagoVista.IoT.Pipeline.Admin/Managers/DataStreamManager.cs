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
using LagoVista.UserAdmin;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class DataStreamManager : ManagerBase, IDataStreamManager
    {
        private readonly IDataStreamRepo _dataStreamRepo;
        private readonly ISecureStorage _secureStorage;
        private readonly IDefaultInternalDataStreamConnectionSettings _defaultConnectionSettings;
        private readonly IOrgUtils _orgUtils;
        private readonly ISharedConnectionManager _sharedDataStreamConnectionManager;
        public DataStreamManager(IDataStreamRepo dataStreamRepo, ISharedConnectionManager sharedDataStreamConnectionManager, IDefaultInternalDataStreamConnectionSettings defaultConnectionSettings, IOrgUtils orgUtils, IAdminLogger logger, ISecureStorage secureStorage, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _dataStreamRepo = dataStreamRepo ?? throw new ArgumentNullException(nameof(IDataStreamRepo));
            _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(ISecureStorage));
            _defaultConnectionSettings = defaultConnectionSettings ?? throw new ArgumentNullException(nameof(IDefaultInternalDataStreamConnectionSettings));
            _sharedDataStreamConnectionManager = sharedDataStreamConnectionManager ?? throw new ArgumentNullException(nameof(SharedConnectionManager));
            _orgUtils = orgUtils ?? throw new ArgumentNullException(nameof(IOrgUtils));
        }

        private async Task<InvokeResult> CreatePointArrayStorageAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            var orgNamespace = (await _orgUtils.GetOrgNamespaceAsync(org.Id)).Result;

            stream.DeviceIdFieldName = "device_id";
            stream.TimestampFieldName = "time_stamp";
            stream.DbSchema = "public";
            stream.DbURL = _defaultConnectionSettings.PointArrayConnectionSettings.Uri;
            stream.DbTableName = $"point_array_{stream.Key}";

            stream.CreateTableDDL = GetPointArrayDataStorageSQL_DDL(stream.DbTableName);

            stream.DatabaseName = orgNamespace;
            stream.DbName = orgNamespace;
            stream.DbUserName = orgNamespace;

            stream.AutoCreateSQLTable = false;

            // we will create it here as part of our setup.
            stream.DBPasswordSecureId = $"ps_db_uid_{org.Id}";

            var existingPassword = await _secureStorage.GetSecretAsync(org, stream.DBPasswordSecureId, user);
            if (existingPassword.Successful)
            {
                stream.DbPassword = existingPassword.Result;
            }
            else
            {
                stream.DbPassword = Guid.NewGuid().ToId();

                var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.DBPasswordSecureId, stream.DbPassword);
                if (!addSecretResult.Successful)
                {
                    return addSecretResult.ToInvokeResult();
                }
            }

            stream.Fields.Clear();

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Start Time Stamp",
                Key = "starttimestamp",
                FieldName = "starttimestamp",
                Description = "Time in seconds from UTC epoch (1/1/1970).",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                IsRequired = true,
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Sensor Index",
                Key = "sensorindex",
                FieldName = "sensorindex",
                Description = "Sensor Index on Board for this point array.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                IsRequired = true,
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Point Count",
                Key = "pointcount",
                FieldName = "pointcount",
                Description = "Number of points that make up this point array.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                IsRequired = true
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Interval",
                Key = "interval",
                FieldName = "interval",
                Description = "Interval between sample collection points.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal),
                IsRequired = true,
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Point Array",
                Key = "pointarray",
                FieldName = "pointarray",
                Description = "Collection of points that make up the data collected from the device.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DecimalArray),
                IsRequired = true,
            });

            this.ValidationCheck(stream, Actions.Create);

            await CreatePostgresStorage(stream, stream.DbPassword);

            stream.DbPassword = null;

            return InvokeResult.Success;
        }


        public async Task<InvokeResult> CreateGeoSpatialStorageAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            var orgNamespace = (await _orgUtils.GetOrgNamespaceAsync(org.Id)).Result;

            stream.DeviceIdFieldName = "device_id";
            stream.TimestampFieldName = "time_stamp";
            stream.DbSchema = "public";
            stream.DbURL = _defaultConnectionSettings.GeoSpatialConnectionSettings.Uri;
            stream.DbTableName = $"geospatial_{stream.Key}";

            stream.CreateTableDDL = GetGeoSpatialStorageSQL_DDL(stream.DbTableName);

            // we will create it here as part of our setup.
            stream.DBPasswordSecureId = $"ps_db_uid_{org.Id}";

            stream.DatabaseName = orgNamespace;
            stream.DbName = orgNamespace;
            stream.DbUserName = orgNamespace;

            stream.AutoCreateSQLTable = false;

            var existingPassword = await _secureStorage.GetSecretAsync(org, stream.DBPasswordSecureId, user);
            if (existingPassword.Successful)
            {
                stream.DbPassword = existingPassword.Result;
            }
            else
            {
                stream.DbPassword = Guid.NewGuid().ToId();

                var addSecretResult = await _secureStorage.AddSecretAsync(org, stream.DBPasswordSecureId, stream.DbPassword);
                if (!addSecretResult.Successful)
                {
                    return addSecretResult.ToInvokeResult();
                }
            }

            stream.Fields.Clear();

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Start Time Stamp",
                Key = "starttimestamp",
                FieldName = "starttimestamp",
                Description = "Time in seconds from UTC epoch (1/1/1970).",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                IsRequired = true,
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Point Count",
                Key = "pointcount",
                FieldName = "pointcount",
                Description = "Number of points that make up this point array.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                IsRequired = true
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Interval",
                Key = "interval",
                FieldName = "interval",
                Description = "Interval between sample collection points.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal),
                IsRequired = true,
            });

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Point Array",
                Key = "pointarray",
                FieldName = "pointarray",
                Description = "Collection of points that make up the data collected from the device.",
                FieldType = EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DecimalArray),
                IsRequired = true,
            });


            this.ValidationCheck(stream, Actions.Create);

            await CreatePostgresStorage(stream, stream.DbPassword);

            stream.DbPassword = null;

            return InvokeResult.Success;

        }

        public async Task<InvokeResult> AddDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Create, user, org);
            if (stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                stream.AzureStorageAccountName = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccountId;
                stream.AzureAccessKey = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccessKey;
            }

            this.ValidationCheck(stream, Actions.Create);

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
                    throw new Exception("Validation should have not null or empty AzureAccessKey, but it did not.");
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
                    throw new Exception("Validation should have not null or empty AWSSecretKey, but it did not.");
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
                    throw new Exception("Validation should have not null or empty DbPassword, but it did not.");
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
            else if (stream.StreamType.Value == DataStreamTypes.PointArrayStorage)
            {
                await CreatePointArrayStorageAsync(stream, org, user);
            }
            else if(stream.StreamType.Value == DataStreamTypes.GeoSpatial)
            {
                await CreateGeoSpatialStorageAsync(stream, org, user);
            }
            else
            {
                throw new Exception("New data stream Type was added, should likely add something here to store credentials.");
            }


            await _dataStreamRepo.AddDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        private async Task<InvokeResult> CreatePostgresStorage(DataStream stream, String dbPassword)
        {
            var connString = $"Host={_defaultConnectionSettings.PointArrayConnectionSettings.Uri};Username={_defaultConnectionSettings.PointArrayConnectionSettings.UserName};Password={_defaultConnectionSettings.PointArrayConnectionSettings.Password};";
            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                conn.Open();
                Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Check User {stream.DbUserName}");
                // Create the user.
                cmd.Connection = conn;
                cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                cmd.Parameters.AddWithValue("@userName", stream.DbUserName);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Does Not Exist - creating {stream.DbUserName}");
                    cmd.Parameters.Clear();

                    cmd.CommandText = $"CREATE USER {stream.DbUserName} with LOGIN PASSWORD '{dbPassword}';";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                    cmd.Parameters.AddWithValue("@userName", stream.DbUserName);
                    result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return InvokeResult.FromError("Could not create local user.");
                    }
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Does Not Exist - created {stream.DbUserName}");
                }
                else
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Exists {stream.DbUserName}");

                // Create teh database.
                cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                cmd.Parameters.AddWithValue("@dbname", stream.DatabaseName);
                result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    cmd.Parameters.Clear();

                    cmd.CommandText = $"CREATE DATABASE {stream.DatabaseName};";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DatabaseName);
                    result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return InvokeResult.FromError("Could not create local database.");
                    }
                }

                conn.Close();
            }

            // now login to postgres with the user for this org.
            connString = $"Host={stream.DbURL};Username={stream.DbUserName};Password={dbPassword};Database={stream.DbName}";
            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                // Create storage.
                conn.Open();
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                cmd.CommandText = stream.CreateTableDDL;
                cmd.ExecuteNonQuery();
                conn.Close();
                return InvokeResult.Success;
            }
        }

        private string GetPointArrayDataStorageSQL_DDL(String tableName)
        {
            return $@"CREATE TABLE if not exists {tableName}(
                        id SERIAL,
                        device_id text not null,
                        time_stamp timestamp not null,
                        sensor_index smallint not null,
                        value float4 not null
                    );";
        }

        private string GetGeoSpatialStorageSQL_DDL(String tableName)
        {
            return $@"CREATE TABLE if not exists {tableName}(
                        id SERIAL,
                        device_id text not null,
                        time_stamp timestamp not null,
                        value geometry not null
                    );";
        }


        private async Task<InvokeResult<DataStream>> PopulateCredentialsAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
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
                     stream.StreamType.Value == DataStreamTypes.Postgresql ||
                     stream.StreamType.Value == DataStreamTypes.GeoSpatial ||
                     stream.StreamType.Value == DataStreamTypes.PointArrayStorage)
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

            return InvokeResult<DataStream>.Create(stream);
        }

        public async Task<InvokeResult<DataStream>> PopulateSharedConnectionAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            var sharedConnection = await _sharedDataStreamConnectionManager.LoadFullSharedConnectionAsync(stream.SharedConnection.Id, org, user);

            switch (stream.StreamType.Value)
            {
                case DataStreamTypes.AWSElasticSearch:
                case DataStreamTypes.AWSS3:
                    if(sharedConnection.Result.ConnectionType.Value != Pipeline.Models.SharedConnectionTypes.AWS)
                    {
                        return InvokeResult<DataStream>.FromError($"Mismatch in stream types, shared connection type {sharedConnection.Result.ConnectionType} - {stream.StreamType.Value}");
                    }

                    stream.AwsAccessKey = sharedConnection.Result.AwsAccessKey;
                    stream.AwsRegion = sharedConnection.Result.AwsRegion;
                    stream.AwsSecretKey = sharedConnection.Result.AwsSecretKey;
                    stream.AWSSecretKeySecureId = sharedConnection.Result.AWSSecretKeySecureId;
                    break;

                case DataStreamTypes.AzureBlob:
                case DataStreamTypes.AzureEventHub:
                case DataStreamTypes.AzureTableStorage:
                case DataStreamTypes.AzureTableStorage_Managed:
                    if (sharedConnection.Result.ConnectionType.Value != Pipeline.Models.SharedConnectionTypes.Azure)
                    {
                        return InvokeResult<DataStream>.FromError($"Mismatch in stream types, shared connection type {sharedConnection.Result.ConnectionType} - {stream.StreamType.Value}");
                    }
                    
                    stream.AzureAccessKey = sharedConnection.Result.AzureAccessKey;
                    stream.AzureAccessKeySecureId = sharedConnection.Result.AzureAccessKeySecureId;
                    stream.AzureStorageAccountName = sharedConnection.Result.AzureStorageAccountName;
                    break;

                case DataStreamTypes.Postgresql:
                case DataStreamTypes.PointArrayStorage:
                case DataStreamTypes.GeoSpatial:
                case DataStreamTypes.SQLServer:
                    if (sharedConnection.Result.ConnectionType.Value != Pipeline.Models.SharedConnectionTypes.Database)
                    {
                        return InvokeResult<DataStream>.FromError($"Mismatch in stream types, shared connection type {sharedConnection.Result.ConnectionType} - {stream.StreamType.Value}");
                    }
                   
                    stream.DbName = sharedConnection.Result.DbName;
                    stream.DbUserName = sharedConnection.Result.DbUserName;
                    stream.DbPassword = sharedConnection.Result.DbPassword;
                    stream.DBPasswordSecureId = sharedConnection.Result.DBPasswordSecureId;
                    stream.DbURL = sharedConnection.Result.DbURL;
                    stream.DbSchema = sharedConnection.Result.DbSchema;

                    break;
            
                case DataStreamTypes.Redis:
                    if (sharedConnection.Result.ConnectionType.Value != Pipeline.Models.SharedConnectionTypes.Redis)
                    {
                        return InvokeResult<DataStream>.FromError($"Mismatch in stream types, shared connection type {sharedConnection.Result.ConnectionType} - {stream.StreamType.Value}");
                    }
                  
                    stream.RedisPassword = sharedConnection.Result.RedisPassword;
                    stream.RedisServerUris = sharedConnection.Result.RedisServerUris;
                    stream.RedisPasswordSecureId = sharedConnection.Result.RedisPasswordSecureId;
                    break;
            }

            return InvokeResult<DataStream>.Create(stream);
        }

        public async Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            try
            {
                var stream = await _dataStreamRepo.GetDataStreamAsync(id);
                await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Read, user, org, "LoadWithSecrets");
                if (!EntityHeader.IsNullOrEmpty(stream.SharedConnection))
                {
                    return await PopulateSharedConnectionAsync(stream, org, user);
                }
                else
                {
                    return await PopulateCredentialsAsync(stream, org, user);
                }
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
            else if (stream.StreamType.Value == DataStreamTypes.Redis)
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

        public async Task<InvokeResult<string>> GetDataStreamSecretAsync(string id, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(id);

            //Auth should take care of this, but this is very, very important.
            if(dataStream.OwnerOrganization.Id != org.Id)
            {
                throw new NotAuthorizedException("Invalid get secret call."); 
            }

            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Read, user, org, "GetDataStreamSecret" );

            switch (dataStream.StreamType.Value)
            {
                case DataStreamTypes.AWSElasticSearch:
                case DataStreamTypes.AWSS3:
                    return await _secureStorage.GetSecretAsync(org, dataStream.AWSSecretKeySecureId, user);

                case DataStreamTypes.AzureBlob:
                case DataStreamTypes.AzureEventHub:
                case DataStreamTypes.AzureTableStorage:
                    return await _secureStorage.GetSecretAsync(org, dataStream.AzureAccessKeySecureId, user);
                
                case DataStreamTypes.Redis:
                    return await _secureStorage.GetSecretAsync(org, dataStream.RedisPasswordSecureId, user);

                case DataStreamTypes.Postgresql:
                case DataStreamTypes.SQLServer:
                    return await _secureStorage.GetSecretAsync(org, dataStream.DBPasswordSecureId, user);
                
                case DataStreamTypes.AzureTableStorage_Managed:
                case DataStreamTypes.PointArrayStorage:
                    return InvokeResult<string>.FromError("No secret available.");
            }
           
            throw new InvalidOperationException($"Secret of type {dataStream.StreamType.Value} not supported.");
        }
    }
}
