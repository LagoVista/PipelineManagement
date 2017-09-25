using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.Core.Managers;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Interfaces;
using static LagoVista.Core.Models.AuthorizeResult;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Exceptions;
using LagoVista.IoT.Pipeline.Admin.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class PipelineModuleManager : ManagerBase, IPipelineModuleManager
    {
        IListenerConfigurationRepo _listenerConfigurationRepo;
        IPlannerConfigurationRepo _plannerConfigurationRepo;
        IInputTranslatorConfigurationRepo _inputTranslatorConfigurationRepo;
        ISentinelConfigurationRepo _sentinalConfigurationRepo;
        IOutputTranslatorConfigurationRepo _outputTranslatorConfigurationRepo;
        ITransmitterConfigurationRepo _transmitterConfigurationRepo;
        ICustomPipelineConfigurationRepo _customPipelineConfigurationRepo;

        public PipelineModuleManager(IListenerConfigurationRepo listenerConfigurationRep, IInputTranslatorConfigurationRepo inputConfigurationRepo, ISentinelConfigurationRepo sentinalConfigurationRepo, IPlannerConfigurationRepo plannerConfigurationRepo,
               IOutputTranslatorConfigurationRepo outputConfigurationRepo, ITransmitterConfigurationRepo translatorConfigurationRepo, ICustomPipelineConfigurationRepo pipelineConfigrationRepo, IAdminLogger logger, IAppConfig appConfig,
               IDependencyManager depManager, ISecurity security) : base(logger, appConfig, depManager, security)
        {
            _listenerConfigurationRepo = listenerConfigurationRep;
            _inputTranslatorConfigurationRepo = inputConfigurationRepo;
            _sentinalConfigurationRepo = sentinalConfigurationRepo;
            _outputTranslatorConfigurationRepo = outputConfigurationRepo;
            _transmitterConfigurationRepo = translatorConfigurationRepo;
            _customPipelineConfigurationRepo = pipelineConfigrationRepo;
            _plannerConfigurationRepo = plannerConfigurationRepo;
        }

        #region Add Methods
        public async Task<InvokeResult> AddListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(listenerConfiguration, Actions.Create);
            await _listenerConfigurationRepo.AddListenerConfigurationAsync(listenerConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddPlannerConfigurationAsync(PlannerConfiguration plannerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(plannerConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(plannerConfiguration, Actions.Create);
            await _plannerConfigurationRepo.AddPlannerConfigurationAsync(plannerConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(inputTranslatorConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(inputTranslatorConfiguration, Actions.Create);
            await _inputTranslatorConfigurationRepo.AddInputTranslatorConfigurationAsync(inputTranslatorConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddSentinelConfigurationAsync(SentinelConfiguration sentinelConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(sentinelConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(sentinelConfiguration, Actions.Create);
            await _sentinalConfigurationRepo.AddSentinelConfigurationAsync(sentinelConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(outputTranslatorConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(outputTranslatorConfiguration, Actions.Create);
            await _outputTranslatorConfigurationRepo.AddOutputTranslatorConfigurationAsync(outputTranslatorConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(transmitterConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(transmitterConfiguration, Actions.Create);
            await _transmitterConfigurationRepo.AddTransmitterConfigurationAsync(transmitterConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(pipelineModuleConfiguration, Actions.Create);
            await _customPipelineConfigurationRepo.AddCustomPipelineModuleConfigurationAsync(pipelineModuleConfiguration);
            return InvokeResult.Success;
        }
        #endregion

        #region Update Methods
        public async Task<InvokeResult> UpdateListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(listenerConfiguration, Actions.Update);
            await _listenerConfigurationRepo.UpdateListenerConfigurationAsync(listenerConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(inputTranslatorConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(inputTranslatorConfiguration, Actions.Update);
            await _inputTranslatorConfigurationRepo.UpdateInputTranslatorConfigurationAsync(inputTranslatorConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdatePlannerConfigurationAsync(PlannerConfiguration plannerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(plannerConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(plannerConfiguration, Actions.Update);
            await _plannerConfigurationRepo.UpdatePlannerConfigurationAsync(plannerConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateSentinelConfigurationAsync(SentinelConfiguration sentinalConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(sentinalConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(sentinalConfiguration, Actions.Update);
            await _sentinalConfigurationRepo.UpdateSentinelConfigurationAsync(sentinalConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(outputTranslatorConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(outputTranslatorConfiguration, Actions.Update);
            await _outputTranslatorConfigurationRepo.UpdateOutputTranslatorConfigurationAsync(outputTranslatorConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(transmitterConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(transmitterConfiguration, Actions.Update);
            await _transmitterConfigurationRepo.UpdateTransmitterConfigurationAsync(transmitterConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(pipelineModuleConfiguration, Actions.Update);
            await _customPipelineConfigurationRepo.UpdateCustomPipelineModuleConfigurationAsync(pipelineModuleConfiguration);
            return InvokeResult.Success;
        }
        #endregion

        #region Get Method
        public async Task<ListenerConfiguration> GetListenerConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var listenerConfiguration = await _listenerConfigurationRepo.GetListenerConfigurationAsync(id);
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Read, user, org);
            return listenerConfiguration;
        }

        public async Task<InputTranslatorConfiguration> GetInputTranslatorConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var inputTranslatorConfiguration = await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(inputTranslatorConfiguration, AuthorizeActions.Read, user, org);
            return inputTranslatorConfiguration;
        }

        public async Task<PlannerConfiguration> GetPlannerConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var plannerConfiguration = await _plannerConfigurationRepo.GetPlannerConfigurationAsync(id);
            await AuthorizeAsync(plannerConfiguration, AuthorizeActions.Read, user, org);
            return plannerConfiguration;
        }

        public async Task<SentinelConfiguration> GetSentinelConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var sentinalConfiguration = await _sentinalConfigurationRepo.GetSentinelConfigurationAsync(id);
            await AuthorizeAsync(sentinalConfiguration, AuthorizeActions.Read, user, org);
            return sentinalConfiguration;
        }

        public async Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var outputTranslator = await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(outputTranslator, AuthorizeActions.Read, user, org);
            return outputTranslator;
        }

        public async Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var transmitterConfiguration = await _transmitterConfigurationRepo.GetTransmitterConfigurationAsync(id);
            await AuthorizeAsync(transmitterConfiguration, AuthorizeActions.Read, user, org);
            return transmitterConfiguration;
        }

        public async Task<CustomPipelineModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            var pipelineModuleConfiguration = await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationAsync(id);
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Read, user, org);
            return pipelineModuleConfiguration;
        }
        #endregion

        #region Load Methods
        public async Task<InvokeResult<ListenerConfiguration>> LoadFullListenerConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<ListenerConfiguration>.Create(await _listenerConfigurationRepo.GetListenerConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<ListenerConfiguration>.FromErrors(ErrorCodes.CouldNotLoadListenerConfig.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<InputTranslatorConfiguration>> LoadFullInputTranslatorConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<InputTranslatorConfiguration>.Create(await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<InputTranslatorConfiguration>.FromErrors(ErrorCodes.CouldNotLoadInputTranslator.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<PlannerConfiguration>> LoadFullPlannerConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<PlannerConfiguration>.Create(await _plannerConfigurationRepo.GetPlannerConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<PlannerConfiguration>.FromErrors(ErrorCodes.CouldNotLoadPlanner.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<SentinelConfiguration>> LoadFullSentinelConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<SentinelConfiguration>.Create(await _sentinalConfigurationRepo.GetSentinelConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<SentinelConfiguration>.FromErrors(ErrorCodes.CouldNotLoadSentinel.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<OutputTranslatorConfiguration>> LoadFullOutputTranslatorConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<OutputTranslatorConfiguration>.Create(await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<OutputTranslatorConfiguration>.FromErrors(ErrorCodes.CouldNotLoadOutputTranslator.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<TransmitterConfiguration>> LoadFullTransmitterConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<TransmitterConfiguration>.Create(await _transmitterConfigurationRepo.GetTransmitterConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<TransmitterConfiguration>.FromErrors(ErrorCodes.CouldNotLoadTransmitter.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<InvokeResult<CustomPipelineModuleConfiguration>> LoadFullCustomPipelineModuleConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<CustomPipelineModuleConfiguration>.Create(await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<CustomPipelineModuleConfiguration>.FromErrors(ErrorCodes.CouldNotLoadCustomModule.ToErrorMessage($"ModuleId={id}"));
            }

        }
        #endregion

        #region Get For Org
        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetListenerConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(ListenerConfiguration));
            return await _listenerConfigurationRepo.GetListenerConfigurationsForOrgsAsync(orgId);
        }
        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetInputTranslatorConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(InputTranslatorConfiguration));
            return await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationsForOrgsAsync(orgId);
        }
        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetSentinelConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(SentinelConfiguration));
            return await _sentinalConfigurationRepo.GetSentinelConfigurationsForOrgsAsync(orgId);
        }

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetPlannerConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(SentinelConfiguration));
            return await _plannerConfigurationRepo.GetPlannerConfigurationsForOrgsAsync(orgId);
        }


        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetOutputTranslatorConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(OutputTranslatorConfiguration));
            return await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationsForOrgsAsync(orgId);
        }

        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetTransmitterConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(TransmitterConfiguration));
            return await _transmitterConfigurationRepo.GetTransmitterConfigurationsForOrgsAsync(orgId);
        }
        public async Task<IEnumerable<PipelineModuleConfigurationSummary>> GetCustomPipelineModuleConfiugrationsForOrgAsync(String orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(CustomPipelineModuleConfiguration));
            return await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationsForOrgsAsync(orgId);
        }
        #endregion

        #region Delete Methods
        public async Task<InvokeResult> DeleteListenerAsync(String id, EntityHeader org, EntityHeader user)
        {
            var listenerConfiguration = await _listenerConfigurationRepo.GetListenerConfigurationAsync(id);
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Delete, user, org);
            await _listenerConfigurationRepo.DeleteListenerConfigurationAsync(listenerConfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeletePlannerAsync(String id, EntityHeader org, EntityHeader user)
        {
            var plannerconfiguration = await _plannerConfigurationRepo.GetPlannerConfigurationAsync(id);
            await AuthorizeAsync(plannerconfiguration, AuthorizeActions.Delete, user, org);
            await _plannerConfigurationRepo.DeletePlannerConfigurationAsync(plannerconfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteInputTranslatorAsync(String id, EntityHeader org, EntityHeader user)
        {
            var inputTranslatorConfiguration = await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(inputTranslatorConfiguration, AuthorizeActions.Delete, user, org);
            await _inputTranslatorConfigurationRepo.DeleteInputTranslatorConfigurationAsync(inputTranslatorConfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteSentinelAsync(String id, EntityHeader org, EntityHeader user)
        {
            var sentinalConfiguration = await _sentinalConfigurationRepo.GetSentinelConfigurationAsync(id);
            await AuthorizeAsync(sentinalConfiguration, AuthorizeActions.Delete, user, org);
            await _sentinalConfigurationRepo.DeleteSentinelConfigurationAsync(sentinalConfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteOutputTranslatorAsync(String id, EntityHeader org, EntityHeader user)
        {
            var outputTranslatorConfiguration = await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(outputTranslatorConfiguration, AuthorizeActions.Delete, user, org);
            await _outputTranslatorConfigurationRepo.DeleteOutputTranslatorConfigurationAsync(outputTranslatorConfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteTransmitterAsync(String id, EntityHeader org, EntityHeader user)
        {
            var transmitterConfiguration = await _transmitterConfigurationRepo.GetTransmitterConfigurationAsync(id);
            await AuthorizeAsync(transmitterConfiguration, AuthorizeActions.Delete, user, org);
            await _transmitterConfigurationRepo.DeleteTransmitterConfigurationAsync(transmitterConfiguration.Id);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteCustomPipelineModuleAsync(String id, EntityHeader org, EntityHeader user)
        {
            var pipelineModuleConfiguration = await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationAsync(id);
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Delete, user, org);
            await _customPipelineConfigurationRepo.DeleteCustomPipelineModuleConfigurationAsync(pipelineModuleConfiguration.Id);
            return InvokeResult.Success;
        }
        #endregion

        #region Query Key in Use
        public Task<bool> QueryKeyInUseListenerConfigurationAsync(string key, EntityHeader org)
        {
            return _listenerConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public Task<bool> QueryKeyInUseInputTranslatorConfigurationAsync(string key, EntityHeader org)
        {
            return _inputTranslatorConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public Task<bool> QueryKeyInUsePlannerConfigurationAsync(string key, EntityHeader org)
        {
            return _plannerConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public Task<bool> QueryKeyInUseSentinelConfigurationAsync(string key, EntityHeader org)
        {
            return _sentinalConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public Task<bool> QueryKeyInUseOutputTranslatorConfigurationAsync(string key, EntityHeader org)
        {
            return _outputTranslatorConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public Task<bool> QueryKeyInUseTransmitterConfigurationAsync(string key, EntityHeader org)
        {
            return _transmitterConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }
        public Task<bool> QueryKeyInUseCustomPipelineModuleConfigurationAsync(string key, EntityHeader org)
        {
            return _customPipelineConfigurationRepo.QueryKeyInUseAsync(key, org.Id);
        }
        #endregion

        #region Check In Use
        public async Task<DependentObjectCheckResult> CheckInUseListener(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var listener = await _listenerConfigurationRepo.GetListenerConfigurationAsync(id);
            await AuthorizeAsync(listener, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(listener);
        }

        public async Task<DependentObjectCheckResult> CheckInUseInputTranslator(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }

        public async Task<DependentObjectCheckResult> CheckInUseSentinel(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _sentinalConfigurationRepo.GetSentinelConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }

        public async Task<DependentObjectCheckResult> CheckInUseOutputTranslator(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }

        public async Task<DependentObjectCheckResult> CheckInUseTransmitter(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _transmitterConfigurationRepo.GetTransmitterConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }

        public async Task<DependentObjectCheckResult> CheckInUsePlanner(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _plannerConfigurationRepo.GetPlannerConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }

        public async Task<DependentObjectCheckResult> CheckInUseCustom(string id, EntityHeader orgEntityHeader, EntityHeader userEntityHeader)
        {
            var module = await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationAsync(id);
            await AuthorizeAsync(module, AuthorizeActions.Read, userEntityHeader, orgEntityHeader);
            return await CheckForDepenenciesAsync(module);
        }
        #endregion
    }
}