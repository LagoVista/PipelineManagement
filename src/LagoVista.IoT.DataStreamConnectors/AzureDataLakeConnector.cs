using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{

    /* For Future Expansion.... */
    public class AzureDataLakeConnector : IDataStreamConnector
    {
        DataStream _stream;

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;
            throw new NotImplementedException();
        }

        public Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            var result = new InvokeResult();

            return Task.FromResult(result);
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            throw new NotImplementedException();
        }

        public  Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException();
        }
    }
}
