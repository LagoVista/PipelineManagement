using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Web.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.UserAdmin.Models.Account;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Pipeline.Admin.Rest.Controllers
{

    [Authorize]
    public class PipelineModuleController : LagoVistaBaseController
    {

        IPipelineModuleManager _pipelineModuleManager;
        public PipelineModuleController(IPipelineModuleManager pipelineModuleManager, UserManager<AppUser> userManager, ILogger logger) : base(userManager, logger)
        {
            _pipelineModuleManager = pipelineModuleManager;
        }

        #region Listener CRUD and Company...
        /// <summary>
        /// Listener - Add
        /// </summary>
        /// <param name="listenerConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/listener")]
        public Task<InvokeResult> AddTransmitterConfigurationAsync([FromBody] ListenerConfiguration listenerConfiguration)
        {
            return _pipelineModuleManager.AddListenerConfigurationAsync(listenerConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Listener - Update
        /// </summary>
        /// <param name="listenerConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/listener")]
        public Task<InvokeResult> UpdateTransmitterConfigurationAsync([FromBody] ListenerConfiguration listenerConfiguration)
        {
            SetUpdatedProperties(listenerConfiguration);
            return _pipelineModuleManager.UpdateListenerConfigurationAsync(listenerConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Listener - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/listener/{id}")]
        public async Task<DetailResponse<ListenerConfiguration>> GetListenerConfigurationAsync(String id)
        {
            var listenerConfig = await _pipelineModuleManager.GetListenerConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<ListenerConfiguration>.Create(listenerConfig);
        }

        /// <summary>
        /// Listener - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/listeners")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetListenerConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetListenerConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Listener - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/listener/{id}")]
        public Task<InvokeResult> DeleteListenerConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteListenerAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Listener - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/listener/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseListenerConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseListenerConfigurationAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Listener - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/listener/{id}/inuse")]
        public Task<DependentObjectCheckResult> ListenerInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseListener(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Listener - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/listener/factory")]
        public DetailResponse<ListenerConfiguration> CreateListener()
        {
            var listener =  DetailResponse<ListenerConfiguration>.Create();
            listener.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(listener.Model);
            SetAuditProperties(listener.Model);

            return listener;
        }
        #endregion

        #region Input Translator CRUD and Company...
        /// <summary>
        /// Input Translator - Add
        /// </summary>
        /// <param name="inputTranslatorConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/inputtranslator")]
        public Task<InvokeResult> AddInputTranslatorConfigurationAsync([FromBody] InputTranslatorConfiguration inputTranslatorConfiguration)
        {
            return _pipelineModuleManager.AddInputTranslatorConfigurationAsync(inputTranslatorConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Input Translator - Update
        /// </summary>
        /// <param name="inputTranslatorConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/inputtranslator")]
        public Task<InvokeResult> UpdateInputTranslatorConfigurationAsync([FromBody] InputTranslatorConfiguration inputTranslatorConfiguration)
        {
            return _pipelineModuleManager.UpdatedInputTranslatorConfigurationAsync(inputTranslatorConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Input Translator - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/inputtranslator/{id}")]
        public async Task<DetailResponse<InputTranslatorConfiguration>> GetInputTranslatorConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetInputTranslatorConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<InputTranslatorConfiguration>.Create(config);
        }

        /// <summary>
        /// Input Translator - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/inputtranslators")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetInputTranslatorConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetInputTranslatorConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Input Translator - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/inputtranslator/{id}")]
        public Task<InvokeResult> DeleteInputTranslatorConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteInputTranslatorAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Input Translator - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/inputtranslator/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseInputTranslatorConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseInputTranslatorConfigurationAsync(key, OrgEntityHeader);
        }


        /// <summary>
        /// Input Translator - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/inputtranslator/{id}/inuse")]
        public Task<DependentObjectCheckResult> InputTranslatorInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseInputTranslator(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Input Translator - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/inputtranslator/factory")]
        public DetailResponse<InputTranslatorConfiguration> CreateInputTranslator()
        {
            var inputTranslatorConfiguration = DetailResponse<InputTranslatorConfiguration>.Create();
            inputTranslatorConfiguration.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(inputTranslatorConfiguration.Model);
            SetAuditProperties(inputTranslatorConfiguration.Model);

            return inputTranslatorConfiguration;
        }
        #endregion

        #region Sentinel CRUD and Company...
        /// <summary>
        /// Sentinel - Add
        /// </summary>
        /// <param name="sentinelConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/sentinel")]
        public Task<InvokeResult> AddSentinelConfigurationAsync([FromBody] SentinelConfiguration sentinelConfiguration)
        {
            return _pipelineModuleManager.AddSentinelConfigurationAsync(sentinelConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Sentinel - Update
        /// </summary>
        /// <param name="sentinelConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/sentinel")]
        public Task<InvokeResult> UpdateSentinelConfigurationAsync([FromBody] SentinelConfiguration sentinelConfiguration)
        {
            return _pipelineModuleManager.UpdatedSentinelConfigurationAsync(sentinelConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Sentinel - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/sentinel/{id}")]
        public async Task<DetailResponse<SentinelConfiguration>> GetSentinelConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetSentinelConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<SentinelConfiguration>.Create(config);
        }

        /// <summary>
        /// Sentinel - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/sentinels")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetSentinelConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetSentinelConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Sentinel - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/sentinel/{id}")]
        public Task<InvokeResult> DeleteSentinelConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteSentinelAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Sentinel - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/sentinel/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseSentinelConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseSentinelConfigurationAsync(key, OrgEntityHeader);
        }


        /// <summary>
        /// Sentinel - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/sentinel/{id}/inuse")]
        public Task<DependentObjectCheckResult> SentinelInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseSentinel(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Sentinel - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/sentinel/factory")]
        public DetailResponse<SentinelConfiguration> CreateSentinel()
        {
            var sentinelConfiguration = DetailResponse<SentinelConfiguration>.Create();
            sentinelConfiguration.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(sentinelConfiguration.Model);
            SetAuditProperties(sentinelConfiguration.Model);

            return sentinelConfiguration;
        }
        #endregion

        #region Output Translator CRUD and Company...
        /// <summary>
        /// Output Translator - Add
        /// </summary>
        /// <param name="outputTranslatorConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/outputtranslator")]
        public Task<InvokeResult> AddOutputTranslatorConfigurationAsync([FromBody] OutputTranslatorConfiguration outputTranslatorConfiguration)
        {
            return _pipelineModuleManager.AddOutputTranslatorConfigurationAsync(outputTranslatorConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Output Translator - Update
        /// </summary>
        /// <param name="outputTranslatorConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/outputtranslator")]
        public Task<InvokeResult> UpdateOutputTranslatorConfigurationAsync([FromBody] OutputTranslatorConfiguration outputTranslatorConfiguration)
        {
            return _pipelineModuleManager.UpdateOutputTranslatorConfigurationAsync(outputTranslatorConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Output Translator - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/outputtranslator/{id}")]
        public async Task<DetailResponse<OutputTranslatorConfiguration>> GetOutputTranslatorConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetOutputTranslatorConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<OutputTranslatorConfiguration>.Create(config);
        }

        /// <summary>
        /// Output Translator - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/outputtranslators")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetOutputTranslatorConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetOutputTranslatorConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Output Translator - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/outputtranslator/{id}")]
        public Task<InvokeResult> DeleteOutputTranslatorConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteOutputTranslatorAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Output Translator - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/outputtranslator/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseOutputTranslatorConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseOutputTranslatorConfigurationAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Output Translator - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/outputtranslator/{id}/inuse")]
        public Task<DependentObjectCheckResult> OutputTranslatorInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseOutputTranslator(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Output Translator - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/outputtranslator/factory")]
        public DetailResponse<OutputTranslatorConfiguration> CreateOutputTranslator()
        {
            var outputTranslatorConfiguration = DetailResponse<OutputTranslatorConfiguration>.Create();
            outputTranslatorConfiguration.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(outputTranslatorConfiguration.Model);
            SetAuditProperties(outputTranslatorConfiguration.Model);

            return outputTranslatorConfiguration;
        }
        #endregion

        #region Transmitter CRUD and Company...
        /// <summary>
        /// Transmitter - Add
        /// </summary>
        /// <param name="transmitterConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/transmitter")]
        public Task<InvokeResult> AddTransmitterConfigurationAsync([FromBody] TransmitterConfiguration transmitterConfiguration)
        {
            return _pipelineModuleManager.AddTransmitterConfigurationAsync(transmitterConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Transmitter - Update
        /// </summary>
        /// <param name="transmitterConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/transmitter")]
        public Task<InvokeResult> UpdateTransmitterConfigurationAsync([FromBody] TransmitterConfiguration transmitterConfiguration)
        {
            return _pipelineModuleManager.UpdatedTransmitterConfigurationAsync(transmitterConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Transmitter - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/transmitter/{id}")]
        public async Task<DetailResponse<TransmitterConfiguration>> GetTransmitterConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetTransmitterConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);

            return DetailResponse<TransmitterConfiguration>.Create(config);
        }

        /// <summary>
        /// Transmitter - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/transmitters")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetTransmitterConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetTransmitterConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Transmitter - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/transmitter/{id}")]
        public Task<InvokeResult> DeleteTransmitterConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteTransmitterAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Transmitter - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/transmitter/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseTransmitterConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseTransmitterConfigurationAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Transmitter - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/transmitter/{id}/inuse")]
        public Task<DependentObjectCheckResult> TransmitterInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseTransmitter(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Transmitter - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/transmitter/factory")]
        public DetailResponse<TransmitterConfiguration> CreateTransmitter()
        {
            var transmitter = DetailResponse<TransmitterConfiguration>.Create();
            transmitter.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(transmitter.Model);
            SetAuditProperties(transmitter.Model);

            return transmitter;
        }
        #endregion

        #region Planner CRUD and Company...
        /// <summary>
        /// Planner - Add
        /// </summary>
        /// <param name="plannerConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/planner")]
        public Task<InvokeResult> AddPlannerConfigurationAsync([FromBody] PlannerConfiguration plannerConfiguration)
        {
            return _pipelineModuleManager.AddPlannerConfigurationAsync(plannerConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Planner - Update
        /// </summary>
        /// <param name="transmitterConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/planner")]
        public Task<InvokeResult> UpdatePlannerConfigurationAsync([FromBody] PlannerConfiguration transmitterConfiguration)
        {
            return _pipelineModuleManager.UpdatePlannerConfigurationAsync(transmitterConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Planner - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/planner/{id}")]
        public async Task<DetailResponse<PlannerConfiguration>> GetPlannerConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetPlannerConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);

            return DetailResponse<PlannerConfiguration>.Create(config);
        }

        /// <summary>
        /// Planner - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/pipeline/admin/planners")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetPlannerConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetPlannerConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Planner - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/planner/{id}")]
        public Task<InvokeResult> DeletePlannerConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeletePlannerAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Planner - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/planner/{key}/keyinuse")]
        public Task<bool> QueryKeyInUsePlannerConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUsePlannerConfigurationAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Planner - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/planner/{id}/inuse")]
        public Task<DependentObjectCheckResult> PlannerInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUsePlanner(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Planner - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/planner/factory")]
        public DetailResponse<PlannerConfiguration> CreatePlanner()
        {
            var planner = DetailResponse<PlannerConfiguration>.Create();
            planner.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(planner.Model);
            SetAuditProperties(planner.Model);

            return planner;
        }

        /// <summary>
        /// Field Parser - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/fieldparser/factory")]
        public DetailResponse<MessageFieldParserConfiguration> CreateFieldParser()
        {
            var fieldParser = DetailResponse<MessageFieldParserConfiguration>.Create();
            fieldParser.Model.Id = Guid.NewGuid().ToId();

            return fieldParser;
        }
        #endregion

        #region Custom Module Configuration CRUD and Company...
        /// <summary>
        /// Custom - Add
        /// </summary>
        /// <param name="customPipelineModuleConfiguration"></param>
        /// <returns></returns>
        [HttpPost("/api/pipeline/admin/custommodule")]
        public Task<InvokeResult> AddCustomPipelineModuleConfigurationAsync([FromBody] CustomPipelineModuleConfiguration customPipelineModuleConfiguration)
        {
            return _pipelineModuleManager.AddCustomPipelineModuleConfigurationAsync(customPipelineModuleConfiguration, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Custom - Update
        /// </summary>
        /// <param name="customPipelineModuleConfiguration"></param>
        /// <returns></returns>
        [HttpPut("/api/pipeline/admin/custommodule")]
        public Task<InvokeResult> UpdateCustomPipelineModuleConfigurationAsync([FromBody] CustomPipelineModuleConfiguration customPipelineModuleConfiguration)
        {
            return _pipelineModuleManager.UpdateCustomPipelineModuleConfigurationAsync(customPipelineModuleConfiguration, UserEntityHeader);
        }

        /// <summary>
        /// Custom - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/custommodule/{id}")]
        public async Task<DetailResponse<CustomPipelineModuleConfiguration>> GetCustomPipelineModuleConfigurationAsync(String id)
        {
            var config = await _pipelineModuleManager.GetCustomPipelineModuleConfigurationAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<CustomPipelineModuleConfiguration>.Create(config);
        }

        /// <summary>
        /// Custom - Get For Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{ordi}/pipeline/admin/custommodules")]
        public async Task<ListResponse<PipelineModuleConfigurationSummary>> GetCustomPipelineModuleConfigurationsForOrgAsync(String orgid)
        {
            var configs = await _pipelineModuleManager.GetCustomPipelineModuleConfiugrationsForOrgAsync(orgid, UserEntityHeader);
            return ListResponse<PipelineModuleConfigurationSummary>.Create(configs);
        }

        /// <summary>
        /// Custom - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/pipeline/admin/custommodule/{id}")]
        public Task<InvokeResult> DeleteCustomPipelineModuleConfigurationAsync(String id)
        {
            return _pipelineModuleManager.DeleteCustomPipelineModuleAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Custom - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/pipeline/admin/custommodule/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseCustomPipelineModuleConfigurationAsync(String key)
        {
            return _pipelineModuleManager.QueryKeyInUseCustomPipelineModuleConfigurationAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Planner - In Use Check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/planner/{id}/inuse")]
        public Task<DependentObjectCheckResult> CustomModelInUseCheck(string id)
        {
            return _pipelineModuleManager.CheckInUseCustom(id, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Custom - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/pipeline/admin/custommodule/factory")]
        public DetailResponse<CustomPipelineModuleConfiguration> CreateCustomModule()
        {
            var customModuleConfiguration = DetailResponse<CustomPipelineModuleConfiguration>.Create();
            customModuleConfiguration.Model.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(customModuleConfiguration.Model);
            SetAuditProperties(customModuleConfiguration.Model);

            return customModuleConfiguration;
        }
        #endregion
    }
}
