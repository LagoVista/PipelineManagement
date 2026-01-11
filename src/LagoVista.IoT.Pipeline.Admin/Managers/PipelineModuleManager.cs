// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: eae4cf5e267e83932e0cd056210383c1f826afb0f6778ea3e54c6a34bc85823a
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.Core.Managers;
using LagoVista.Core.Interfaces;
using static LagoVista.Core.Models.AuthorizeResult;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Exceptions;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.AI;
using System.Linq;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.AI.Interfaces.Repos;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class PipelineModuleManager : ManagerBase, IPipelineModuleManager
    {
        readonly IListenerConfigurationRepo _listenerConfigurationRepo;
        readonly IPlannerConfigurationRepo _plannerConfigurationRepo;
        readonly IInputTranslatorConfigurationRepo _inputTranslatorConfigurationRepo;
        readonly ISentinelConfigurationRepo _sentinalConfigurationRepo;
        readonly IOutputTranslatorConfigurationRepo _outputTranslatorConfigurationRepo;
        readonly ITransmitterConfigurationRepo _transmitterConfigurationRepo;
        readonly ICustomPipelineConfigurationRepo _customPipelineConfigurationRepo;
        readonly ISecureStorage _secureStorage;
        readonly IModelRepo _modelRepo;


        public PipelineModuleManager(IListenerConfigurationRepo listenerConfigurationRep, ISecureStorage secureStorage, IInputTranslatorConfigurationRepo inputConfigurationRepo, ISentinelConfigurationRepo sentinalConfigurationRepo, IPlannerConfigurationRepo plannerConfigurationRepo,
               IModelRepo modelRepo, IOutputTranslatorConfigurationRepo outputConfigurationRepo, ITransmitterConfigurationRepo translatorConfigurationRepo, ICustomPipelineConfigurationRepo pipelineConfigrationRepo, IAdminLogger logger, IAppConfig appConfig,
               IDependencyManager depManager, ISecurity security) : base(logger, appConfig, depManager, security)
        {
            _listenerConfigurationRepo = listenerConfigurationRep;
            _inputTranslatorConfigurationRepo = inputConfigurationRepo;
            _sentinalConfigurationRepo = sentinalConfigurationRepo;
            _outputTranslatorConfigurationRepo = outputConfigurationRepo;
            _transmitterConfigurationRepo = translatorConfigurationRepo;
            _customPipelineConfigurationRepo = pipelineConfigrationRepo;
            _plannerConfigurationRepo = plannerConfigurationRepo;
            _secureStorage = secureStorage;
            _modelRepo = modelRepo;
        }

        #region Add Methods
        public async Task<InvokeResult> AddListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(listenerConfiguration, Actions.Create);

            if (!String.IsNullOrEmpty(listenerConfiguration.AccessKey))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, listenerConfiguration.AccessKey);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                listenerConfiguration.SecureAccessKeyId = addSecretResult.Result;
                listenerConfiguration.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.Password))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, listenerConfiguration.Password);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                listenerConfiguration.SecurePasswordId = addSecretResult.Result;
                listenerConfiguration.Password = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.Certificate))
            {
                var result = await _secureStorage.AddSecretAsync(org, listenerConfiguration.Certificate);
                if (!result.Successful) return result.ToInvokeResult();

                listenerConfiguration.CertificateSecureId = result.Result;
                listenerConfiguration.Certificate = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.CertificatePassword))
            {
                var result = await _secureStorage.AddSecretAsync(org, listenerConfiguration.CertificatePassword);
                if (!result.Successful) return result.ToInvokeResult();

                listenerConfiguration.CertificatePasswordSecureId = result.Result;
                listenerConfiguration.CertificatePassword = null;
            }

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

            if (!String.IsNullOrEmpty(transmitterConfiguration.AccessKey))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, transmitterConfiguration.AccessKey);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                transmitterConfiguration.SecureAccessKeyId = addSecretResult.Result;
                transmitterConfiguration.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(transmitterConfiguration.Password))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, transmitterConfiguration.Password);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                transmitterConfiguration.SecurePasswordId = addSecretResult.Result;
                transmitterConfiguration.Password = null;
            }

            await _transmitterConfigurationRepo.AddTransmitterConfigurationAsync(transmitterConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Create, user, org);
            ValidationCheck(pipelineModuleConfiguration, Actions.Create);

            if (!String.IsNullOrEmpty(pipelineModuleConfiguration.AuthenticationHeader))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, pipelineModuleConfiguration.AuthenticationHeader);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                pipelineModuleConfiguration.AuthenticationHeaderSecureId = addSecretResult.Result;
                pipelineModuleConfiguration.AuthenticationHeader = null;
            }

            if (!String.IsNullOrEmpty(pipelineModuleConfiguration.AccountPassword))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, pipelineModuleConfiguration.AccountPassword);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                pipelineModuleConfiguration.AccountPasswordSecureId = addSecretResult.Result;
                pipelineModuleConfiguration.AccountPassword = null;
            }

            await _customPipelineConfigurationRepo.AddCustomPipelineModuleConfigurationAsync(pipelineModuleConfiguration);

            return InvokeResult.Success;
        }
        #endregion

        #region Update Methods
        public async Task<InvokeResult> UpdateListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(listenerConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(listenerConfiguration, Actions.Update);

            if (!String.IsNullOrEmpty(listenerConfiguration.AccessKey))
            {
                if (!String.IsNullOrEmpty(listenerConfiguration.SecurePasswordId))
                {
                    await _secureStorage.RemoveSecretAsync(org, listenerConfiguration.SecureAccessKeyId);
                }

                var addSecretResult = await _secureStorage.AddSecretAsync(org, listenerConfiguration.AccessKey);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                listenerConfiguration.SecureAccessKeyId = addSecretResult.Result;
                listenerConfiguration.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.Password))
            {
                if (!String.IsNullOrEmpty(listenerConfiguration.SecurePasswordId))
                {
                    await _secureStorage.RemoveSecretAsync(org, listenerConfiguration.SecurePasswordId);
                }

                var addSecretResult = await _secureStorage.AddSecretAsync(org, listenerConfiguration.Password);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                listenerConfiguration.SecurePasswordId = addSecretResult.Result;
                listenerConfiguration.Password = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.Certificate))
            {
                if (!String.IsNullOrEmpty(listenerConfiguration.CertificateSecureId))
                {
                    await _secureStorage.RemoveSecretAsync(org, listenerConfiguration.CertificateSecureId);
                }

                var result = await _secureStorage.AddSecretAsync(org, listenerConfiguration.Certificate);
                if (!result.Successful) return result.ToInvokeResult();

                listenerConfiguration.CertificateSecureId = result.Result;
                listenerConfiguration.Certificate = null;
            }

            if (!String.IsNullOrEmpty(listenerConfiguration.CertificatePassword))
            {
                if (!String.IsNullOrEmpty(listenerConfiguration.CertificatePasswordSecureId))
                {
                    await _secureStorage.RemoveSecretAsync(org, listenerConfiguration.CertificatePasswordSecureId);
                }

                var result = await _secureStorage.AddSecretAsync(org, listenerConfiguration.CertificatePassword);
                if (!result.Successful) return result.ToInvokeResult();

                listenerConfiguration.CertificatePasswordSecureId = result.Result;
                listenerConfiguration.CertificatePassword = null;
            }

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

            if (!String.IsNullOrEmpty(transmitterConfiguration.AccessKey))
            {
                await _secureStorage.RemoveSecretAsync(org, transmitterConfiguration.SecureAccessKeyId);

                var addSecretResult = await _secureStorage.AddSecretAsync(org, transmitterConfiguration.AccessKey);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                transmitterConfiguration.SecureAccessKeyId = addSecretResult.Result;
                transmitterConfiguration.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(transmitterConfiguration.Password))
            {
                await _secureStorage.RemoveSecretAsync(org, transmitterConfiguration.SecurePasswordId);

                var addSecretResult = await _secureStorage.AddSecretAsync(org, transmitterConfiguration.Password);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                transmitterConfiguration.SecurePasswordId = addSecretResult.Result;
                transmitterConfiguration.Password = null;
            }

            await _transmitterConfigurationRepo.UpdateTransmitterConfigurationAsync(transmitterConfiguration);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> UpdateCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(pipelineModuleConfiguration, AuthorizeActions.Update, user, org);
            ValidationCheck(pipelineModuleConfiguration, Actions.Update);

            if (!String.IsNullOrEmpty(pipelineModuleConfiguration.AuthenticationHeader))
            {
                await _secureStorage.RemoveSecretAsync(org, pipelineModuleConfiguration.AuthenticationHeaderSecureId);

                var addSecretResult = await _secureStorage.AddSecretAsync(org, pipelineModuleConfiguration.AuthenticationHeader);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                pipelineModuleConfiguration.AuthenticationHeaderSecureId = addSecretResult.Result;
                pipelineModuleConfiguration.AuthenticationHeader = null;
            }

            if (!String.IsNullOrEmpty(pipelineModuleConfiguration.AccountPassword))
            {
                await _secureStorage.RemoveSecretAsync(org, pipelineModuleConfiguration.AccountPasswordSecureId);

                var addSecretResult = await _secureStorage.AddSecretAsync(org, pipelineModuleConfiguration.AccountPassword);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                pipelineModuleConfiguration.AccountPasswordSecureId = addSecretResult.Result;
                pipelineModuleConfiguration.AccountPassword = null;
            }

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

        public async Task<CustomModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(String id, EntityHeader org, EntityHeader user)
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

                var inputTranslator = await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationAsync(id);

                switch(inputTranslator.InputTranslatorType.Value)
                {
                    case InputTranslatorConfiguration.InputTranslatorTypes.NuvAIModel:
                        if(EntityHeader.IsNullOrEmpty(inputTranslator.Model))
                        {
                            throw new InvalidDataException("Input Translator set to be NuvAI, but does not have a model object assigned.");
                        }

                        inputTranslator.Model.Value = await _modelRepo.GetModelAsync(inputTranslator.Model.Id);

                        if(inputTranslator.Model.Value == null)
                        {
                            throw new RecordNotFoundException("Model", inputTranslator.Model.Id);
                        }

                        if (EntityHeader.IsNullOrEmpty(inputTranslator.ModelRevision) &&
                            EntityHeader.IsNullOrEmpty(inputTranslator.Model.Value.PreferredRevision))
                        {
                            throw new InvalidDataException("Input Translator does not have a model revision, model does not have a preferred revision.");
                        }

                        if (EntityHeader.IsNullOrEmpty(inputTranslator.ModelRevision))
                        {
                            inputTranslator.ModelRevision.Value = inputTranslator.Model.Value.Revisions.Where(rev => rev.Id == inputTranslator.Model.Value.PreferredRevision.Id).FirstOrDefault();
                            if (inputTranslator.ModelRevision.Value == null)
                            {
                                throw new InvalidDataException("Could not find preferred model revision.");
                            }
                        }
                        else
                        {
                            inputTranslator.ModelRevision.Value = inputTranslator.Model.Value.Revisions.Where(rev => rev.Id == inputTranslator.ModelRevision.Id).FirstOrDefault();
                            if (inputTranslator.ModelRevision.Value == null)
                            {
                                throw new InvalidDataException("Could not find specified model revision.");
                            }
                        }

                            break;
                }
                    
                    
                return InvokeResult<InputTranslatorConfiguration>.Create(inputTranslator);
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

        public async Task<InvokeResult<CustomModuleConfiguration>> LoadFullCustomPipelineModuleConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<CustomModuleConfiguration>.Create(await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationAsync(id));
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<CustomModuleConfiguration>.FromErrors(ErrorCodes.CouldNotLoadCustomModule.ToErrorMessage($"ModuleId={id}"));
            }

        }
        #endregion

        #region Get For Org
        public async Task<ListResponse<ListenerConfigurationSummary>> GetListenerConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(ListenerConfiguration));
            return await _listenerConfigurationRepo.GetListenerConfigurationsForOrgsAsync(orgId, listRequest);
        }
        public async Task<ListResponse<InputTranslatorConfigurationSummary>> GetInputTranslatorConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(InputTranslatorConfiguration));
            return await _inputTranslatorConfigurationRepo.GetInputTranslatorConfigurationsForOrgsAsync(orgId, listRequest);
        }
        public async Task<ListResponse<SentinelConfigurationSummary>> GetSentinelConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(SentinelConfiguration));
            return await _sentinalConfigurationRepo.GetSentinelConfigurationsForOrgsAsync(orgId, listRequest);
        }

        public async Task<ListResponse<PlannerConfigurationSummary>> GetPlannerConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(SentinelConfiguration));
            return await _plannerConfigurationRepo.GetPlannerConfigurationsForOrgsAsync(orgId, listRequest);
        }


        public async Task<ListResponse<OutputTranslatorConfigurationSummary>> GetOutputTranslatorConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(OutputTranslatorConfiguration));
            return await _outputTranslatorConfigurationRepo.GetOutputTranslatorConfigurationsForOrgsAsync(orgId, listRequest);
        }

        public async Task<ListResponse<TransmitterConfigurationSummary>> GetTransmitterConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(TransmitterConfiguration));
            return await _transmitterConfigurationRepo.GetTransmitterConfigurationsForOrgsAsync(orgId, listRequest);
        }
        public async Task<ListResponse<CustomModuleConfigurationSummary>> GetCustomPipelineModuleConfiugrationsForOrgAsync(String orgId, EntityHeader user, ListRequest listRequest)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(CustomModuleConfiguration));
            return await _customPipelineConfigurationRepo.GetCustomPipelineModuleConfigurationsForOrgsAsync(orgId, listRequest);
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