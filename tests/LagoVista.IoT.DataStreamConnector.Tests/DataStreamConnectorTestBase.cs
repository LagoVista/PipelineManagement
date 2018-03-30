using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests
{
    public class DataStreamConnectorTestBase
    {
        protected async Task<DataStreamRecord> AddObject(IDataStreamConnector connector, string deviceId, params KeyValuePair<string, object>[] items)
        {
            var record = new Pipeline.Admin.Models.DataStreamRecord()
            {
                DeviceId = deviceId
            };

            foreach (var item in items)
            {
                record.Data.Add(item.Key, item.Value);
            }

            var addResult = await connector.AddItemAsync(record);
            Assert.IsTrue(addResult.Successful);

            return record;
        }
    }
}
