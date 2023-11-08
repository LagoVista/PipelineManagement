using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface ISharedConnectionRepo
    {
        Task AddSharedConnectionAsync(SharedConnection connection);

        Task UpdateShareConnectionAsync(SharedConnection connection);

        Task<SharedConnection> GetSharedConnectionAsync(string id);

        Task<ListResponse<SharedConnectionSummary>> GetSharedConnectionsForOrgAsync(string orgId, ListRequest listRequest);

        Task DeleteSharedConnectionAsync(string id);

        Task<bool> QueryKeyInUseAsync(string key, string orgId);
    }
}
