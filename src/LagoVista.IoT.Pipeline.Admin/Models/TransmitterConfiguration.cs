using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Transmitter_Title, PipelineAdminResources.Names.Transmitter_Help, PipelineAdminResources.Names.Transmitter_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class TransmitterConfiguration : PipelineModuleConfiguration
    {
        public TransmitterConfiguration()
        {
            Headers = new ObservableCollection<Header>();
        }

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
            [EnumLabel("mqttclient", PipelineAdminResources.Names.Connection_Type_MQTT_Client, typeof(PipelineAdminResources))]
            MQTTClient,
            [EnumLabel("originallistener", PipelineAdminResources.Names.Transmitter_TransmitterType_OriginalListener, typeof(PipelineAdminResources))]
            OriginalListener,
            
            /*[EnumLabel("mqtt", PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
            MQTT,
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
            SMTP,*/

            /*[EnumLabel("custom", PipelineAdminResources.Names.Connection_Type_Custom, typeof(PipelineAdminResources))]
            Custom*/
        }

        IConnectionSettings ConnectionSettings { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Transmitter_TransmitterType, EnumType: (typeof(TransmitterTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<TransmitterTypes> TransmitterType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectSSLTLS, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool SecureConnection { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string UserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Password, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Password { get; set; }

        public string SecurePasswordId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HostName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HostName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKeyName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKeyName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKey { get; set; }

        public string SecureAccessKeyId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Transmitter_Headers, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public ObservableCollection<Header> Headers { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HubName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HubName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Queue, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Queue { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectToPort, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources))]
        public int? ConnectToPort { get; set; }


        public override string ModuleType => PipelineModuleType_Transmitter;

        [CustomValidator]
        public void Validate(ValidationResult result)
        {

        }
    }
}