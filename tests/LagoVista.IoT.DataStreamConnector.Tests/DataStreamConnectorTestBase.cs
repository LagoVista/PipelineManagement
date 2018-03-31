using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LagoVista.Core;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests
{
    public class DataStreamConnectorTestBase
    {
        protected void WriteResult(ListResponse<DataStreamResult> response)
        {
            var idx = 1;
            foreach (var item in response.Model)
            {
                Console.WriteLine($"Record {idx++}");

                foreach (var fld in item.Fields)
                {
                    Console.WriteLine($"\t{fld.Key} - {fld.Value}");
                }
                Console.WriteLine("----");
                Console.WriteLine();
            }
        }

        protected DataStreamRecord GetRecord(DataStream stream, string deviceId, string timeStamp, params KeyValuePair<string, object>[] items)
        {
            var record = new Pipeline.Admin.Models.DataStreamRecord()
            {
                DeviceId = deviceId,
                StreamId = stream.Id
                
            };

            if (!String.IsNullOrEmpty(timeStamp))
            {
                record.Timestamp = timeStamp;
            }

            foreach (var item in items)
            {
                record.Data.Add(item.Key, item.Value);
            }

            return record;
        }

        protected async Task<DataStreamRecord> AddObject(IDataStreamConnector connector, DataStream stream, string deviceId, string timeStamp, params KeyValuePair<string, object>[] items)
        {
            var record = GetRecord(stream, deviceId, timeStamp, items);
            var addResult = await connector.AddItemAsync(record);

            if (!addResult.Successful)
            {
                Console.WriteLine(addResult.Errors.First().Message);
            }

            Assert.IsTrue(addResult.Successful);
            return record;
        }

        protected async Task ValidatePaginatedRecordSet(string deviceId, IDataStreamConnector connector)
        {

            var getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 15
            });
            Assert.IsTrue(getResult.Successful);

            Assert.AreEqual("99", getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value.ToString());
            Assert.IsTrue(getResult.HasMoreRecords, "Should Have Records");
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { PageIndex = 1, PageSize = 15 });

            Assert.AreEqual("84", getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value.ToString());
            Assert.IsTrue(getResult.Successful);
            Assert.IsTrue(getResult.HasMoreRecords);
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { PageIndex = 6, PageSize = 15 });

            Assert.AreEqual("9", getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value.ToString());
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { PageIndex = 7, PageSize = 15 });

            Assert.AreEqual(0, getResult.PageSize);
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);
        }

        protected async Task ValidateDataFilterBefore(string deviceId, DataStream stream, IDataStreamConnector connector)
        {
            var getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                EndDate = DateTime.UtcNow.AddDays(-1).ToJSONString(),
            });

            Assert.IsTrue(getResult.Successful, getResult.Successful ? "Success" : $"Could not get result {getResult.Errors.First().Message}");

            WriteResult(getResult);

            Assert.IsFalse(getResult.HasMoreRecords);
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());
        }

        protected async Task ValidateDataFilterInRange(string deviceId, DataStream stream, IDataStreamConnector connector)
        {
            var getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                StartDate = DateTime.UtcNow.AddDays(-1).ToJSONString(),
                EndDate = DateTime.UtcNow.AddDays(1).ToJSONString(),
            });

            Assert.IsTrue(getResult.Successful, getResult.Successful ? "Success" : $"Could not get result {getResult.Errors.First().Message}");

            WriteResult(getResult);

            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());
        }

        protected enum QueryRangeType
        {
            Records_100,
            ForBeforeQuery,
            ForInRangeQuery,
            ForAfterQuery
        }


        protected async Task ValidateDataFilterAfter(string deviceId, DataStream stream, IDataStreamConnector connector)
        {
            var getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                StartDate = DateTime.UtcNow.AddDays(1).ToJSONString()
            });

            Assert.IsTrue(getResult.Successful, getResult.Successful ? "Success" : $"Could not get result {getResult.Errors.First().Message}");

            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);
        }

        protected List<DataStreamRecord> GetRecordsToInsert(DataStream stream, string deviceId, QueryRangeType rangeType)
        {
            var rnd = new Random();

            var records = new List<DataStreamRecord>();

            switch (rangeType)
            {
                case QueryRangeType.Records_100:
                    for (var idx = 0; idx < 100; ++idx)
                    {
                         records.Add(GetRecord(stream, "dev123", null, new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}")));
                    }

                    break;
                case QueryRangeType.ForBeforeQuery:
                    for (var idx = 0; idx < 10; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 10; idx < 20; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(0).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 20; idx < 30; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(-5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", true),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    break;
                case QueryRangeType.ForInRangeQuery:

                    for (var idx = 0; idx < 10; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 10; idx < 20; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(0).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", true),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 20; idx < 30; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(-5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }
                    break;
                case QueryRangeType.ForAfterQuery:
                    for (var idx = 0; idx < 10; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(-5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 10; idx < 20; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(0).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", false),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }

                    for (var idx = 20; idx < 30; ++idx)
                    {
                        records.Add((GetRecord(stream, deviceId, DateTime.Now.AddDays(5).ToJSONString(), 
                            new KeyValuePair<string, object>("pointIndex", idx),
                            new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("inrange", true),
                            new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                            new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"))));
                    }
                    break;
            }

            return records;
        }
    }
}
