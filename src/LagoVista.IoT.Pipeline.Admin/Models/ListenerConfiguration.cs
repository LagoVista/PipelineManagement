using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help, PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class ListenerConfiguration : PipelineModuleConfiguration
    {
        public enum ListenerTypes
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
            [EnumLabel("mqtt", PipelineAdminResources.Names.Connection_Type_MQTT, typeof(PipelineAdminResources))]
            MQTT,
            [EnumLabel("pop3server", PipelineAdminResources.Names.Connection_Type_POP3Server, typeof(PipelineAdminResources))]
            POP3Server,
            [EnumLabel("custom", PipelineAdminResources.Names.Connection_Type_Custom, typeof(PipelineAdminResources))]
            Custom
        }
        
        IConnectionSettings ConnectionSettings { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenerType, EnumType: (typeof(ListenerTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<ListenerTypes> ListenerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Port, HelpResource:PipelineAdminResources.Names.Listener_Port_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources),  IsRequired: false, IsUserEditable: true)]
        public int Port { get; set; }    
        
        public bool DelimitedWithSOHEOT { get; set; }

        public string Endpoint { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReply, HelpResource: PipelineAdminResources.Names.Listener_Port_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool KeepAliveToSendReply { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReplyTimeout,  FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int KeepAliveToSendReplyTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_StartMessageSequence, HelpResource:PipelineAdminResources.Names.Listener_StartMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string StartMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_EndMessageSequence, HelpResource: PipelineAdminResources.Names.Listener_EndMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string EndMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout, HelpResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageReceiveTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MaxMessageSize, HelpResource: PipelineAdminResources.Names.Listener_MaxMessageSize_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MaxMessageSize { get; set; }
    }
}