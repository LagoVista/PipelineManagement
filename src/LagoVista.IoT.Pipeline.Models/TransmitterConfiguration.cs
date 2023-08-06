using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using LagoVista.IoT.Pipeline.Models.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Transmitter_Title, PipelineAdminResources.Names.Transmitter_Help, PipelineAdminResources.Names.Transmitter_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class TransmitterConfiguration : PipelineModuleConfiguration, IFormDescriptor
    {
        public TransmitterConfiguration()
        {
            Headers = new List<Header>();
        }

        public enum TransmitterTypes
        {
            [EnumLabel("amqp", PipelineAdminResources.Names.Connection_Type_AMQP, typeof(PipelineAdminResources))]
            AMQP,
            [EnumLabel("azureserivcebus", PipelineAdminResources.Names.Connection_Type_AzureServiceBus, typeof(PipelineAdminResources))]
            AzureServiceBus,
            [EnumLabel("azureeventhub", PipelineAdminResources.Names.Connection_Type_AzureEventHub, typeof(PipelineAdminResources))]
            AzureEventHub,
            [EnumLabel("azureiothub", PipelineAdminResources.Names.Connection_Type_AzureIoTHub, typeof(PipelineAdminResources))]
            AzureIoTHub,
            [EnumLabel("kafka", PipelineAdminResources.Names.Connection_Type_Kafka, typeof(PipelineAdminResources))]
            Kafka,
            [EnumLabel("mqttclient", PipelineAdminResources.Names.Connection_Type_MQTT_Client, typeof(PipelineAdminResources))]
            MQTTClient,
            [EnumLabel("originallistener", PipelineAdminResources.Names.Transmitter_TransmitterType_OriginalListener, typeof(PipelineAdminResources))]
            OriginalListener,
            [EnumLabel("rabbitmq", PipelineAdminResources.Names.ConnectionType_RabbitMQ, typeof(PipelineAdminResources))]
            RabbitMQ,
            [EnumLabel("redis", PipelineAdminResources.Names.Connection_Type_Redis, typeof(PipelineAdminResources))]
            Redis,
            [EnumLabel("rest", PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
            Rest,
            [EnumLabel("rawtcp", PipelineAdminResources.Names.Connection_Type_TCP, typeof(PipelineAdminResources))]
            RawTCP,
            [EnumLabel("rawudp", PipelineAdminResources.Names.Connection_Type_UDP, typeof(PipelineAdminResources))]
            RawUDP,
            [EnumLabel("serialport", PipelineAdminResources.Names.ConnectionType_SerialPort, typeof(PipelineAdminResources))]
            SerialPort,
            [EnumLabel("hostedmqtt", PipelineAdminResources.Names.Connection_Type_HostedMQTT, typeof(PipelineAdminResources))]
            HostedMQTT,

            /*[EnumLabel("mqtt", PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
            MQTT,
            [EnumLabel("soap", PipelineAdminResources.Names.Connection_Type_Soap, typeof(PipelineAdminResources))]
            Soap,
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


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Anonymous, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool Anonymous { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_UserName, HelpResource: PipelineAdminResources.Names.Listener_UserName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string UserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Password, HelpResource: PipelineAdminResources.Names.Listener_Password_Help, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Password { get; set; }

        public string SecurePasswordId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HostName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HostName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKeyName, HelpResource: PipelineAdminResources.Names.Listener_AccessKeyName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKeyName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKey, HelpResource: PipelineAdminResources.Names.Listener_AccessKey_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKey { get; set; }

        public string SecureAccessKeyId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Transmitter_Headers, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public List<Header> Headers { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HubName, HelpResource: PipelineAdminResources.Names.Listener_HubName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HubName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Queue, HelpResource: PipelineAdminResources.Names.Listener_Queue_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Queue { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ExchangeName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ExchangeName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectToPort, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources))]
        public int? ConnectToPort { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SerialPort_BaudRate, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string BaudRate { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SerialPort_PortName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string PortName { get; set; }


        public override string ModuleType => PipelineModuleType_Transmitter;

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            if (EntityHeader.IsNullOrEmpty(TransmitterType))
            {
                result.AddUserError(PipelineAdminResources.Err_TransmitterTypeIsRequired);
                return;
            }

            switch (TransmitterType.Value)
            {
                case TransmitterTypes.AzureEventHub:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is a Required Field.");
                    if (string.IsNullOrEmpty(HubName)) result.AddUserError("Hub Name is a Required Field.");
                    if (string.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a Required Field.");
                    if (action == Actions.Create && string.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is Required for an Azure Event Hub.");
                    if (action == Actions.Update && string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for an Azure Event Hub.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");
                    break;
                case TransmitterTypes.AzureIoTHub:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is a Required Field.");
                    if (string.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a Required Field.");
                    if (action == Actions.Create && string.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is Required for Azure IoT Event Hub.");
                    if (action == Actions.Update && string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for Azure IoT Event Hub.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");
                    break;
                case TransmitterTypes.AzureServiceBus:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is a Required Field.");
                    if (string.IsNullOrEmpty(Queue)) result.AddUserError("Queue is a Required Field.");
                    if (string.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a Required Field.");
                    if (action == Actions.Create && string.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is Required for an Azure Service Bus Transmitter.");
                    if (action == Actions.Update && string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for an Azure Service Bus Transmitter.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");
                    break;
                case TransmitterTypes.MQTTClient:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is a Required Field.");
                    if (!ConnectToPort.HasValue) result.AddUserError("Port is a Required field, this is usually 1883 or 8883 for a secure connection.");
                    if (!Anonymous)
                    {
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required for non-Anonymous Connections.");
                        if (action == Actions.Create)
                        {
                            if (String.IsNullOrEmpty(Password)) result.AddUserError("Password is Required for non-Anonymous Connections");
                        }
                        else if (action == Actions.Update)
                        {
                            if (String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required for non-Anonymous Connections");
                        }
                    }
                    else
                    {
                        this.Password = null;
                        this.UserName = null;
                    }
                    break;
                case TransmitterTypes.OriginalListener:
                    break;
                case TransmitterTypes.Rest:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is a Required Field.");
                    if (String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password)) result.AddUserError("If User Name is Provided, Password must also be provided.");
                    if (!Anonymous)
                    {
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required for non-Anonymous Connections.");
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required for non-Anonymous Connections.");
                        if (action == Actions.Create)
                        {
                            if (String.IsNullOrEmpty(Password)) result.AddUserError("Password is Required for non-Anonymous Connections");
                        }
                        else if (action == Actions.Update)
                        {
                            if (String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required for non-Anonymous Connections");
                        }
                    }
                    else
                    {
                        this.Password = null;
                        this.UserName = null;
                    }

                    if (Headers == null) Headers = new List<Header>();
                    Headers.RemoveAll(hdr => String.IsNullOrEmpty(hdr.Name) && String.IsNullOrEmpty(hdr.Value));

                    foreach (var header in Headers)
                    {
                        if (string.IsNullOrEmpty(header.Name) || string.IsNullOrEmpty(header.Value)) result.AddUserError("Invalid Header Value, Name and Value are both Required.");
                    }

                    break;
            }
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(TransmitterType),
                nameof(HostName),
                nameof(SecureConnection),
                nameof(ExchangeName),
                nameof(Queue),
                nameof(HubName),
                nameof(AccessKeyName),
                nameof(AccessKey),
                nameof(Anonymous),
                nameof(PortName),
                nameof(BaudRate),
                nameof(UserName),
                nameof(Password),
                nameof(Description),
                nameof(Headers)
            };
        }
    }
}