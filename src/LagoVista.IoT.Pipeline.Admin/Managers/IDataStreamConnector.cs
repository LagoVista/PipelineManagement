using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IDataStreamConnector
    {
        Task<InvokeResult> InitAsync(DataStream stream);
        Task<InvokeResult> AddItemAsync(DataStreamRecord item, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddItemAsync(DataStreamRecord item);
        Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request);
        Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request);
        Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request);
        Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter);
        Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest);
        Task<InvokeResult> UpdateItem(Dictionary<string, object> items, Dictionary<string, object> recordFilter);
        Task<InvokeResult> ValidateConnectionAsync(DataStream stream);
    }
}
