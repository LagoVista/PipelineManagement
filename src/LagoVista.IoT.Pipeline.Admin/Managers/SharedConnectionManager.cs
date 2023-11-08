using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class SharedConnectionManager : ManagerBase, ISharedConnectionManager
    {
        ISharedConnectionRepo _sharedConnectionRepo;
        ISecureStorage _secureStorage;

        public SharedConnectionManager(ISharedConnectionRepo sharedConnectionRepo, IAdminLogger logger, ISecureStorage secureStorage,
            IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) : base(logger, appConfig, depmanager, security)
        {
            _sharedConnectionRepo = sharedConnectionRepo;
            _secureStorage = secureStorage;
        }

        private async Task<InvokeResult> StoreSecretsAsync(SharedConnection connection, EntityHeader org)
        {
            if (connection.ConnectionType.Value == SharedConnectionTypes.Azure)
            {
                if (!String.IsNullOrEmpty(connection.AzureAccessKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, connection.AzureAccessKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(connection.AzureAccessKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, connection.AzureAccessKeySecureId);
                    }

                    connection.AzureAccessKeySecureId = addSecretResult.Result;
                    connection.AzureAccessKey = null;
                }
            }
            else if (connection.ConnectionType.Value == SharedConnectionTypes.AWS)
            {
                if (!String.IsNullOrEmpty(connection.AwsSecretKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, connection.AwsSecretKey);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(connection.AWSSecretKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, connection.AWSSecretKeySecureId);
                    }

                    connection.AWSSecretKeySecureId = addSecretResult.Result;
                    connection.AwsSecretKey = null;
                }
            }
            else if (connection.ConnectionType.Value == SharedConnectionTypes.Database)
            {
                if (!String.IsNullOrEmpty(connection.DbPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, connection.DbPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(connection.DBPasswordSecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, connection.DBPasswordSecureId);
                    }

                    connection.DBPasswordSecureId = addSecretResult.Result;
                    connection.DbPassword = null;
                }
            }
            else if (connection.ConnectionType.Value == SharedConnectionTypes.Redis)
            {
                if (!String.IsNullOrEmpty(connection.RedisPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, connection.RedisPassword);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(connection.RedisPasswordSecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, connection.RedisPasswordSecureId);
                    }

                    connection.RedisPasswordSecureId = addSecretResult.Result;
                    connection.RedisPassword = null;
                }
            }

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddSharedConnectionAsync(SharedConnection connection, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(connection, AuthorizeResult.AuthorizeActions.Create, user, org);

            ValidationCheck(connection, Actions.Create);

            var result = await StoreSecretsAsync(connection, org);
            if (!result.Successful) return result;

            await _sharedConnectionRepo.AddSharedConnectionAsync(connection);

            return InvokeResult.Success;
        }

        public async Task<DependentObjectCheckResult> CheckSharedConnectionInUseAsync(string id, EntityHeader org, EntityHeader user)
        {
            var sharedConnection = await _sharedConnectionRepo.GetSharedConnectionAsync(id);
            await AuthorizeAsync(sharedConnection, AuthorizeResult.AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(sharedConnection);
        }

        public async Task<InvokeResult> DeleteSharedConnectionAsync(string id, EntityHeader org, EntityHeader user)
        {
            var cache = await _sharedConnectionRepo.GetSharedConnectionAsync(id);
            await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Delete, user, org);
            await CheckForDepenenciesAsync(cache);

            await _sharedConnectionRepo.DeleteSharedConnectionAsync(id);

            return InvokeResult.Success;
        }

        public async Task<SharedConnection> GetSharedConnectionAsync(string id, EntityHeader org, EntityHeader user)
        {
            var cahce = await _sharedConnectionRepo.GetSharedConnectionAsync(id);
            await AuthorizeAsync(cahce, AuthorizeResult.AuthorizeActions.Read, user, org);
            return cahce;
        }

        public async Task<ListResponse<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(SharedConnection));
            return await _sharedConnectionRepo.GetSharedConnectionsForOrgAsync(orgId,listRequest);
        }

        public Task<bool> QueryKeyInUseAsync(string key, EntityHeader org)
        {
            return _sharedConnectionRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateSharedConnectionAsync(SharedConnection connection, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(connection, AuthorizeResult.AuthorizeActions.Update, user, org);
            ValidationCheck(connection, Actions.Update);

            var result = await StoreSecretsAsync(connection, org);
            if (!result.Successful) return result;

            await _sharedConnectionRepo.UpdateShareConnectionAsync(connection);

            return InvokeResult.Success;
        }

        public async Task<InvokeResult<string>> GetSharedConnectionSecretAsync(string id, EntityHeader org, EntityHeader user)
        {
            var sharedConnection = await _sharedConnectionRepo.GetSharedConnectionAsync(id);

            //Auth should take care of this, but this is very, very important.
            if (sharedConnection.OwnerOrganization.Id != org.Id)
            {
                throw new NotAuthorizedException("Invalid get secret call.");
            }

            await AuthorizeAsync(sharedConnection, AuthorizeResult.AuthorizeActions.Read, user, org, "GetDataStreamSecret");

            switch (sharedConnection.ConnectionType.Value)
            {
                case SharedConnectionTypes.AWS:
                    return await _secureStorage.GetSecretAsync(org, sharedConnection.AWSSecretKeySecureId, user);

                case SharedConnectionTypes.Azure:
                    return await _secureStorage.GetSecretAsync(org, sharedConnection.AzureAccessKeySecureId, user);

                case SharedConnectionTypes.Redis:
                    return await _secureStorage.GetSecretAsync(org, sharedConnection.RedisPasswordSecureId, user);

                case SharedConnectionTypes.Database:
                    return await _secureStorage.GetSecretAsync(org, sharedConnection.DBPasswordSecureId, user);

            }

            throw new InvalidOperationException($"Secret of type {sharedConnection.ConnectionType.Value} not supported.");
        }

        public async Task<InvokeResult<SharedConnection>> LoadFullSharedConnectionAsync(string id, EntityHeader org, EntityHeader user)
        {
            var connection = await _sharedConnectionRepo.GetSharedConnectionAsync(id);
            await AuthorizeAsync(connection, AuthorizeResult.AuthorizeActions.Read, user, org, "WithSecrets");

            switch (connection.ConnectionType.Value)
            {
                case SharedConnectionTypes.AWS:
                    {
                        var result = await _secureStorage.GetSecretAsync(org, connection.AWSSecretKeySecureId, user);
                        if (!result.Successful) return InvokeResult<SharedConnection>.FromInvokeResult(result.ToInvokeResult());
                        connection.AwsSecretKey = result.Result;
                    }
                    break;
                case SharedConnectionTypes.Azure:
                    {
                        var result = await _secureStorage.GetSecretAsync(org, connection.AzureAccessKeySecureId, user);
                        if (!result.Successful) return InvokeResult<SharedConnection>.FromInvokeResult(result.ToInvokeResult());
                        connection.AzureAccessKey = result.Result;
                    }
                    break;
                case SharedConnectionTypes.Redis:
                    {
                        var result = await _secureStorage.GetSecretAsync(org, connection.RedisPasswordSecureId, user);
                        if (!result.Successful) return InvokeResult<SharedConnection>.FromInvokeResult(result.ToInvokeResult());
                        connection.RedisPassword = result.Result;
                    }
                    break;

                case SharedConnectionTypes.Database:
                    {
                        var result = await _secureStorage.GetSecretAsync(org, connection.DBPasswordSecureId, user);
                        if (!result.Successful) return InvokeResult<SharedConnection>.FromInvokeResult(result.ToInvokeResult());
                        connection.DbPassword = result.Result;
                    }
                    break;
            }

            return InvokeResult<SharedConnection>.Create(connection);
        }
    }
}
