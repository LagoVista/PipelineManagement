using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Rest.Controllers
{
    public class SharedConnectorController : LagoVistaBaseController
    {
        ISharedConnectionManager _connectionManager;

        public SharedConnectorController(ISharedConnectionManager connectionManager, UserManager<AppUser> userManager, IAdminLogger logger) :
            base(userManager, logger)
        {
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// Shared Connection - Add
        /// </summary>
        /// <param name="sharedConnection"></param>
        /// <returns></returns>
        [HttpPost("/api/sharedconnection")]
        public Task<InvokeResult> AddSharedConnectionAsync([FromBody] SharedConnection sharedConnection)
        {
            return _connectionManager.AddSharedConnectionAsync(sharedConnection, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Shared Connection - Get
        /// </summary>
        /// <returns>A Shared Connectoin</returns>
        [HttpGet("/api/sharedconnection/{id}")]
        public async Task<DetailResponse<SharedConnection>> GetSharedConnectionAsync(string id)
        {
            var sharedConnection = await _connectionManager.GetSharedConnectionAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<SharedConnection>.Create(sharedConnection);
        }

        /// <summary>
        /// Shared Connections - Update
        /// </summary>
        /// <param name="sharedConnection"></param>
        /// <returns></returns>
        [HttpPut("/api/sharedconnection")]
        public Task<InvokeResult> UpdateSharedConnectionAsync([FromBody] SharedConnection sharedConnection)
        {
            SetUpdatedProperties(sharedConnection);
            return _connectionManager.UpdateSharedConnectionAsync(sharedConnection, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Shared Connections - Get for Current Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/sharedconnections")]
        public Task<ListResponse<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync()
        {
            return _connectionManager.GetSharedConnectionsForOrgAsync(OrgEntityHeader.Id, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Shared Connection - Can Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/sharedconnection/{id}/inuse")]
        public Task<DependentObjectCheckResult> InUseCheck(String id)
        {
            return _connectionManager.CheckSharedConnectionInUseAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Shared Connection - Key In Use
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/sharedconnection/{key}/keyinuse")]
        public Task<bool> HostKeyInUse(String key)
        {
            return _connectionManager.QueryKeyInUseAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Shared Connection - Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/api/sharedconnection/{id}")]
        public Task<InvokeResult> DeleteSharedConnectionAsync(string id)
        {
            return _connectionManager.DeleteSharedConnectionAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Shared Connection - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/sharedconnection/factory")]
        public DetailResponse<SharedConnection> CreateSharedConnection()
        {

            var response = DetailResponse<SharedConnection>.Create();
            response.Model.Id = Guid.NewGuid().ToId();
            SetAuditProperties(response.Model);
            SetOwnedProperties(response.Model);
            return response;
        }

        /// <summary>
        /// Shared Data Stream Retreive Secret
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/sharedconnection/{id}/secret")]
        public Task<InvokeResult<string>> GetSecrets(String id)
        {
            return _connectionManager.GetSharedConnectionSecretAsync(id, OrgEntityHeader, UserEntityHeader);
        }
    }
}
