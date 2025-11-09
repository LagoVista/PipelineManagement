// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: bcb86bf3167b39afaf0308f8c26d0667f822192c885a87030a0ca47ed70b8f65
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Exceptions;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Rest.Controllers
{
    /// <summary>
    /// Client API Data Stream
    /// </summary>
    [Authorize(AuthenticationSchemes = "APIToken")]
    public class AppDataStreamController : LagoVistaBaseController
    {
        IDataStreamManager _dataStreamManager;
        IAdminLogger _adminlogger;

        public AppDataStreamController(IDataStreamManager dataStreamManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _dataStreamManager = dataStreamManager;
            _adminlogger = logger;
        }

        /// <summary>
        /// Data Stream - Get for Current Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/clientapi/datastreams")]
        public Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync()
        {
            return _dataStreamManager.GetDataStreamsForOrgAsync(OrgEntityHeader.Id, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Client API Data Stream - Get Data for Device
        /// </summary>
        /// <param name="datastreamid"></param>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        [HttpGet("/clientapi/datastream/{datastreamid}/data/{deviceid}")]
        public async Task<ListResponse<DataStreamResult>> GetDataAsync(string datastreamid, string deviceid)
        {
            var dataStream = await _dataStreamManager.LoadFullDataStreamConfigurationAsync(datastreamid, OrgEntityHeader, UserEntityHeader);

            var connectorResult = DataStreamServices.GetConnector(dataStream.Result.StreamType.Value, _adminlogger);

            return await _dataStreamManager.GetStreamDataAsync(dataStream.Result, connectorResult.Result, deviceid, OrgEntityHeader, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Client API Data Stream -
        /// </summary>
        [HttpPost("/clientapi/datastream/{datastreamid}/data/timeseriesquery")]
        public async Task<ListResponse<DataStreamResult>> GetDataAsync(string datastreamid, [FromBody] TimeSeriesAnalyticsRequest request)
        {
            var dataStream = await _dataStreamManager.LoadFullDataStreamConfigurationAsync(datastreamid, OrgEntityHeader, UserEntityHeader);
            var connectorResult = DataStreamServices.GetConnector(dataStream.Result.StreamType.Value, _adminlogger);
            var connector = connectorResult.Result;
            if(request.ListRequest == null)
            {
                request.ListRequest = new ListRequest()
                {
                    PageSize = 50,
                    PageIndex = 1
                };
            }

            return await connector.GetTimeSeriesAnalyticsAsync(request, request.ListRequest);
        }

        /// <summary>
        /// Client API Data Stream -
        /// </summary>
        [HttpPost("/clientapi/datastream/{datastreamid}/data/sqlquery")]
        public async Task<InvokeResult<List<DataStreamResult>>> SqlQuery(string datastreamid, [FromBody] SQLRequest request)
        {
            if(request == null)
            {
                return InvokeResult<List<DataStreamResult>>.FromError($"Empty SQL Request");
            }

            if (String.IsNullOrEmpty(request.Query))
            {
                return InvokeResult<List<DataStreamResult>>.FromError($"Empty Query SQL Request");
            }

            var dataStream = await _dataStreamManager.LoadFullDataStreamConfigurationAsync(datastreamid, OrgEntityHeader, UserEntityHeader);
            if(dataStream == null)
            {
                throw new RecordNotFoundException("DataStream", datastreamid);
            }

            var connectorResult = DataStreamServices.GetConnector(dataStream.Result.StreamType.Value, _adminlogger);
            if(connectorResult == null)
            {
                return InvokeResult<List<DataStreamResult>>.FromError($"Could not get data stream connector for: {dataStream.Result.StreamType.Value}");
            }

            var connector = connectorResult.Result;
            await connector.InitAsync(dataStream.Result);
            return await connector.ExecSQLAsync(request.Query, request.Parameters);
        }
    }
}
