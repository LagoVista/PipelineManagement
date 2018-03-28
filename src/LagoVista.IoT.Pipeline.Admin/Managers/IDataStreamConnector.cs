using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.DeviceManagement.Core;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IDeviceStreamConnector
    {
        Task AddArchiveAsync(string instanceId, DataStreamRecord archiveEntry, EntityHeader org, EntityHeader user);
        Task<ListResponse<List<object>>> GetForDateRangeAsync(string streamId, string deviceId, ListRequest request, EntityHeader org, EntityHeader user);
    }
}
