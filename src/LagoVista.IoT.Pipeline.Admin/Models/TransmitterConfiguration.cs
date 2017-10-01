using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Transmitter_Title, PipelineAdminResources.Names.Transmitter_Help, PipelineAdminResources.Names.Transmitter_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class TransmitterConfiguration : PipelineModuleConfiguration
    {
        public enum TransmitterTypes
        {
            [EnumLabel("azureserivcebus", PipelineAdminResources.Names.Connection_Type_AzureServiceBus, typeof(PipelineAdminResources))]
            AzureServiceBus,
            [EnumLabel("azureeventhub", PipelineAdminResources.Names.Connection_Type_AzureEventHub, typeof(PipelineAdminResources))]
            AzureEventHub,
            [EnumLabel("azureiothub", PipelineAdminResources.Names.Connection_Type_AzureIoTHub, typeof(PipelineAdminResources))]
            AzureIoTHub,
            [EnumLabel("rest", PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
            Rest,
            [EnumLabel("soap", PipelineAdminResources.Names.Connection_Type_Soap, typeof(PipelineAdminResources))]
            Soap,
            [EnumLabel("rawtcp", PipelineAdminResources.Names.Connection_Type_TCP, typeof(PipelineAdminResources))]
            RawTCP,
            [EnumLabel("rawudp", PipelineAdminResources.Names.Connection_Type_UDP, typeof(PipelineAdminResources))]
            RawUDP,
            [EnumLabel("amqp", PipelineAdminResources.Names.Connection_Type_AMQP, typeof(PipelineAdminResources))]
            AMQP,
            [EnumLabel("sms", PipelineAdminResources.Names.Transmitter_TransmitterType_SMS, typeof(PipelineAdminResources))]
            SMS,
            [EnumLabel("outbox", PipelineAdminResources.Names.Transmitter_TransmitterType_Outbox, typeof(PipelineAdminResources))]
            Outbox,
            [EnumLabel("smtp", PipelineAdminResources.Names.Transmitter_TransmitterType_SMTP, typeof(PipelineAdminResources))]
            SMTP,
            [EnumLabel("originallistener", PipelineAdminResources.Names.Transmitter_TransmitterType_OriginalListener, typeof(PipelineAdminResources))]
            OriginalListener,
            [EnumLabel("custom", PipelineAdminResources.Names.Connection_Type_Custom, typeof(PipelineAdminResources))]
            Custom
        }

        IConnectionSettings ConnectionSettings { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Transmitter_TransmitterType, EnumType: (typeof(TransmitterTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<TransmitterTypes> TransmitterType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Common_Script, HelpResource: PipelineAdminResources.Names.InputTranslator_DelimiterSquence_Help, FieldType: FieldTypes.NodeScript, ResourceType: typeof(PipelineAdminResources))]
        public String Script { get; set; }

        public override string ModuleType => PipelineModuleType_Transmitter;
    }
}