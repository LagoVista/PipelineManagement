﻿using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.DataStreamConnectors.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LagoVista.Core;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using Azure.Data.Tables;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    /* To Run Tests:
    * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
    *      AWSUSER
    *      AWSSECRET
    *      AWSACCESSKEY
    */
    [TestClass]
    public class TableStorageConnectorTests : DataStreamConnectorTestBase
    {
        DataStream _currentStream;

        private DataStream GetValidStream()
        {
            if (_currentStream != null)
            {
                return _currentStream;
            }

            _currentStream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                AzureStorageAccountName = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCOUNTID"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCESSKEY"),
                AzureTableStorageName = "unittesttable" + Guid.NewGuid().ToId(),
            };

            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.AzureAccessKey), "Access key must be provided as an Environment Variable in [AZUREACCESSKEY]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.AzureStorageAccountName), "Account Id must be provided as an Environment Variable in [AZUREACESSKEY]");

            return _currentStream;
        }

        private async Task<AzureTableStorageConnector> GetConnector(DataStream stream)
        {
            var connector = new AzureTableStorageConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            var result = await connector.InitAsync(stream);
            if (!result.Successful)
            {
                Assert.IsTrue(result.Successful, $"Did Not Create Connector {result.Errors.First().Message}");
            }
            return connector;
        }

        private async Task<TableClient> GetCloudTable(DataStream stream)
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={_currentStream.AzureStorageAccountName};AccountKey={_currentStream.AzureAccessKey}";
            var _cloudTable = new TableClient(connectionString, _currentStream.AzureTableStorageName);
            try
            {
                await _cloudTable.CreateIfNotExistsAsync();
                return _cloudTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestInitialize()]
        public async Task Init()
        {
            _currentStream = null;
            var stream = GetValidStream();
            var cloudTable = await GetCloudTable(stream);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            var stream = GetValidStream();
            stream.AzureStorageAccountName = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCOUNTID");
            stream.AzureAccessKey = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCESSKEY");

            var cloudTable = await GetCloudTable(stream);
            await cloudTable.DeleteAsync();
            _currentStream = null;
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_DataStream_CreateTable()
        {
            var stream = GetValidStream();
            await GetConnector(stream);

            var cloudTable = await GetCloudTable(stream);
            var query = cloudTable.QueryAsync<TableEntity>(tbl => tbl.PartitionKey == "ABC");
            var pages = query.AsPages().GetAsyncEnumerator();

            if (await pages.MoveNextAsync())
            {
                var pageOne = pages.Current.Values;

                foreach (var value in pageOne)
                {
                 
                }

                if (pages.Current.ContinuationToken != null)
                {
                   
                }
            }
        }

            [TestMethod]
        public async Task DataStream_Azure_TableStorage_CreateRecord()
        {
            var uniqueId = Guid.NewGuid().ToId();

            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var record = await AddObject(connector, stream, "abc123",
                null,
                new KeyValuePair<string, object>("pointOne", 37.5),
                new KeyValuePair<string, object>("pointTwo", 58.6),
                new KeyValuePair<string, object>("uniqueId", uniqueId),
                new KeyValuePair<string, object>("pointThree", "testing"));

            var cloudTable = GetCloudTable(stream);
/*            var recIdQuery = TableQuery.GenerateFilterCondition("uniqueId", QueryComparisons.Equal, uniqueId);

            var query = new TableQuery().Where(recIdQuery);

            var queryResult = (await cloudTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken()));


            Assert.AreEqual(uniqueId, queryResult.First().Properties["uniqueId"].PropertyAsObject);
            Assert.AreEqual("abc123", queryResult.First().Properties[stream.DeviceIdFieldName].PropertyAsObject);*/
        }
        /*

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_Validate_PaginatedItems()
        {
            var deviceId = "dev123";

            var stream = GetValidStream();
            var cloudTable = GetCloudTable(stream);
            var batchOper = new TableBatchOperation();

            var connector = await GetConnector(stream);

            for (var idx = 0; idx < 100; ++idx)
            {
                var record = GetRecord(stream, deviceId,
                    DateTime.UtcNow.AddMinutes(idx).ToJSONString(),
                    new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOne", 37.5),
                    new KeyValuePair<string, object>("pointTwo", 58.6),
                    new KeyValuePair<string, object>("uniqueId", Guid.NewGuid().ToId()),
                    new KeyValuePair<string, object>("pointThree", "testing"));

                batchOper.Add(TableOperation.Insert(DataStreamTSEntity.FromDeviceStreamRecord(stream, record)));
            }

            var results = await cloudTable.ExecuteBatchAsync(batchOper);
            Assert.AreEqual(100, results.Count, "Batch result size should match insert size");
            foreach (var result in results)
            {
                Assert.AreEqual(204, result.HttpStatusCode);
            }

            var getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { PageSize = 15 });

            Assert.IsTrue(getResult.Successful);
            WriteResult(getResult);

            Assert.AreEqual("99", getResult.Model.ToArray()[0].Where(fld => fld.Key == "pointIndex").First().Value.ToString());
            Assert.IsTrue(getResult.HasMoreRecords, "Should Have Records");
            WriteResult(getResult);

            for (var idx = 0; idx < 5; ++idx)
            {
                getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { NextPartitionKey = getResult.NextPartitionKey, NextRowKey = getResult.NextRowKey, PageSize = 15 });

                Assert.AreEqual((84 - (idx * 15)).ToString(), getResult.Model.ToArray()[0].Where(fld => fld.Key == "pointIndex").First().Value.ToString());
                Assert.IsTrue(getResult.Successful);
                Assert.IsTrue(getResult.HasMoreRecords);
                WriteResult(getResult);
            }

            getResult = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest() { NextPartitionKey = getResult.NextPartitionKey, NextRowKey = getResult.NextRowKey, PageSize = 15 });
            Assert.IsTrue(getResult.Successful);
            WriteResult(getResult);

            Assert.AreEqual("9", getResult.Model.ToArray()[0].Where(fld => fld.Key == "pointIndex").First().Value.ToString());
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.HasMoreRecords);

            WriteResult(getResult);
        }

        private async Task BulkInsert(IDataStreamConnector connector, DataStream stream, string deviceType, QueryRangeType rangeType)
        {
            var batchOper = new TableBatchOperation();
            var cloudTable = GetCloudTable(stream);
            var records = GetRecordsToInsert(stream, "dev123", rangeType);
            foreach (var record in records)
            {
                var tsRecord = DataStreamTSEntity.FromDeviceStreamRecord(stream, record);
                batchOper.Add(TableOperation.Insert(tsRecord));
                Console.WriteLine(tsRecord.RowKey);
            }

            var results = await cloudTable.ExecuteBatchAsync(batchOper);
            Assert.AreEqual(records.Count, results.Count, "Batch result size should match insert size");
            foreach (var result in results)
            {
                Assert.AreEqual(204, result.HttpStatusCode);
            }
            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);
        }


        [TestMethod]
        public async Task DataStream_Azure_TableStorage_DateFiltereBefore()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForBeforeQuery);

            await ValidateDataFilterBefore(deviceId, stream, connector);
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_DateFilteredInRange()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForInRangeQuery);

            await ValidateDataFilterInRange("dev123", stream, connector);
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_DateFilteredAfter()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForAfterQuery);

            await ValidateDataFilterAfter("dev123", stream, connector);
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_ValidateConnection_Valid()
        {
            var stream = GetValidStream();
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertSuccessful(validationResult);
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_ValidateConnection_BadCredentials_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureAccessKey = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "Server failed to authenticate the request. Make sure the value of Authorization header is formed correctly including the signature.");
        }

        [TestMethod]
        public async Task DataStream_Azure_TableStorage_ValidateConnection_InvalidAccountId_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureStorageAccountName = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "No such host is known. (isnottherightone.table.core.windows.net:443)");
        }*/
    }
}
