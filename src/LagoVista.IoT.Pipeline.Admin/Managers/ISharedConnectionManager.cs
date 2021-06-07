using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface ISharedConnectionManager
    {
        Task<InvokeResult> AddSharedConnectionAsync(SharedConnection connection,  EntityHeader org, EntityHeader user);
    
        Task<InvokeResult> UpdateSharedConnectionAsync(SharedConnection connection, EntityHeader org, EntityHeader user);
        Task<IEnumerable<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId, EntityHeader user);
        Task<InvokeResult<SharedConnection>> LoadFullSharedConnectionAsync(String id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<string>> GetSharedConnectionSecretAsync(string id, EntityHeader org, EntityHeader user);
        Task<SharedConnection> GetSharedConnectionAsync(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckSharedConnectionInUseAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSharedConnectionAsync(string id, EntityHeader org, EntityHeader user);
        Task<bool> QueryKeyInUseAsync(string key, EntityHeader org);
    }
}
