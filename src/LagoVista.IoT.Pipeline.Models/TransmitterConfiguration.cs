using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using System;
using System.Collections.Generic;
using LagoVista.IoT.Pipeline.Models.Resources;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Transmitter_Title, PipelineAdminResources.Names.Transmitter_Help,
        PipelineAdminResources.Names.Transmitter_Description, EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-pz-send-email",
        GetListUrl: "/api/pipeline/admin/transmitters", SaveUrl: "/api/pipeline/admin/transmitter", GetUrl: "/api/pipeline/admin/transmitter/{id}", FactoryUrl: "/api/pipeline/admin/transmitter/factory",
        ListUIUrl: "/iotstudio/make/transmitters", EditUIUrl: "/iotstudio/make/transmitter/{0}", CreateUIUrl: "/iotstudio/make/transmitter/add",
        DeleteUrl: "/api/pipeline/admin/transmitter/{id}")]
    public class TransmitterConfiguration : PipelineModuleConfiguration, IFormDescriptor, IIconEntity, IFormConditionalFields, ISummaryFactory
    {
        public TransmitterConfiguration()
        {
            Headers = new List<Header>();
            Icon = "icon-pz-send-email";
            Anonymous = true;
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


        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }

        IConnectionSettings ConnectionSettings { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Transmitter_TransmitterType, EnumType: (typeof(TransmitterTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<TransmitterTypes> TransmitterType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectSSLTLS, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool SecureConnection { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Anonymous, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool Anonymous { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_UserName, HelpResource: PipelineAdminResources.Names.Listener_UserName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string UserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Password, HelpResource: PipelineAdminResources.Names.Listener_Password_Help, FieldType: FieldTypes.Password, SecureIdFieldName: nameof(SecurePasswordId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Password { get; set; }

        public string SecurePasswordId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HostName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HostName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKeyName, HelpResource: PipelineAdminResources.Names.Listener_AccessKeyName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKeyName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKey, HelpResource: PipelineAdminResources.Names.Listener_AccessKey_Help, FieldType: FieldTypes.Password, SecureIdFieldName: nameof(SecureAccessKeyId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
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

        public TransmitterConfigurationSummary CreateSummary()
        {
            return new TransmitterConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Icon = Icon,
                IsPublic = IsPublic,
                Description = Description,
                TransmitterType = TransmitterType.Text,
                TransmitterTypeId = TransmitterType.Id,
                Category = Category
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
                nameof(TransmitterType),
                nameof(HostName),
                nameof(ConnectToPort),
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

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>()
                {
                    nameof(HostName), nameof(HubName), nameof(AccessKeyName), nameof(AccessKey), nameof(ConnectToPort), nameof(Anonymous), nameof(UserName), nameof(Password),
                    nameof(Headers), nameof(ExchangeName), nameof(Queue), nameof(HubName), nameof(PortName), nameof(BaudRate), nameof(SecureConnection)
                },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "amqp",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(UserName), nameof(Password), nameof(Queue)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(UserName), nameof(Password), nameof(Queue)},
                    },
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "azureserivcebus",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey), nameof(Queue)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey), nameof(Queue)},
                    },
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "azureeventhub",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey), nameof(HubName)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey), nameof(HubName)},
                    }, new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "azureiothub",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(AccessKeyName), nameof(AccessKey)},
                    }, new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "kafka",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ConnectToPort), nameof(UserName), nameof(Password)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(ConnectToPort)}
                    }, 
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "mqttclient",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ConnectToPort), nameof(Anonymous)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(ConnectToPort)}
                    },
                    new FormConditional()
                    {
                         Field = nameof(Anonymous),
                         Value = "false",
                         VisibleFields = new List<string>() {nameof(UserName), nameof(Password)},
                         RequiredFields = new List<string>() {nameof(UserName), nameof(Password)}
                    },
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "rabbitmq",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ExchangeName), nameof(Queue), nameof(Anonymous)},
                         RequiredFields = new List<string>() {nameof(HostName), nameof(ExchangeName), nameof(Queue)}
                    },new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "redis",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ConnectToPort), nameof(Password)},
                         RequiredFields = new List<string>() {nameof(HostName)}
                    },
                    new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "rest",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(Anonymous)},
                         RequiredFields = new List<string>() {nameof(HostName)}
                    },new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "rawtcp",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ConnectToPort)},
                         RequiredFields = new List<string>() { nameof(HostName), nameof(ConnectToPort) }
                    },new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "rawudp",
                         VisibleFields = new List<string>() {nameof(HostName), nameof(ConnectToPort)},
                         RequiredFields = new List<string>() { nameof(HostName), nameof(ConnectToPort) }
                    },new FormConditional()
                    {
                         Field = nameof(TransmitterType),
                         Value = "serialport",
                         VisibleFields = new List<string>() {nameof(PortName), nameof(BaudRate)},
                         RequiredFields = new List<string>() { nameof(PortName), nameof(BaudRate) }
                    }
                }
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Transmitters_Title, PipelineAdminResources.Names.Transmitter_Help,
        PipelineAdminResources.Names.Transmitter_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-pz-send-email",
        GetListUrl: "/api/pipeline/admin/transmitters", SaveUrl: "/api/pipeline/admin/transmitter", GetUrl: "/api/pipeline/admin/transmitter/{id}", FactoryUrl: "/api/pipeline/admin/transmitter/factory",
        DeleteUrl: "/api/pipeline/admin/transmitter/{id}")]
    public class TransmitterConfigurationSummary : CategorizedSummaryData
    {
        public string TransmitterType { get; set; }
        public string TransmitterTypeId { get; set; }
    }

}