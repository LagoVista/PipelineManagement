// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 69b5d622c36480b6648c430854acd01adb8890d57c05e8250c0ade452b959960
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
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
        Task<ListResponse<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<InvokeResult<SharedConnection>> LoadFullSharedConnectionAsync(String id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<string>> GetSharedConnectionSecretAsync(string id, EntityHeader org, EntityHeader user);
        Task<SharedConnection> GetSharedConnectionAsync(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckSharedConnectionInUseAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSharedConnectionAsync(string id, EntityHeader org, EntityHeader user);
        Task<bool> QueryKeyInUseAsync(string key, EntityHeader org);
    }
}
