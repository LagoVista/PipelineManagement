using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Web.Common.Attributes;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync()
        {
            var hostSummaries = await _dataStreamManager.GetDataStreamsForOrgAsync(OrgEntityHeader.Id, UserEntityHeader);
            var response = ListResponse<DataStreamSummary>.Create(hostSummaries);

            return response;
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
    }
}
