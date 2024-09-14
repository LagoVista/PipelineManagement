using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.DeviceMessaging.Models.Resources;
using LagoVista.IoT.Pipeline.Models.Resources;
using System.Collections.Generic;
using System.Linq;
using static LagoVista.IoT.Pipeline.Admin.Models.TransmitterConfiguration;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
	public enum RESTListenerTypes
    {
        PipelineModule,
        InputCommandListener,
        AcmeListener
    }

    public enum ListenerTypes
    {
        [EnumLabel(ListenerConfiguration.ListenerTypes_AMQP, PipelineAdminResources.Names.Connection_Type_AMQP, typeof(PipelineAdminResources))]
        AMQP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureServiceBus, PipelineAdminResources.Names.Connection_Type_AzureServiceBus, typeof(PipelineAdminResources))]
        AzureServiceBus,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureEventHub, PipelineAdminResources.Names.Connection_Type_AzureEventHub, typeof(PipelineAdminResources))]
        AzureEventHub,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureIoTHub, PipelineAdminResources.Names.Connection_Type_AzureIoTHub, typeof(PipelineAdminResources))]
        AzureIoTHub,

        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_Listener, PipelineAdminResources.Names.ConnectionType_MQTT_Listener, typeof(PipelineAdminResources))]
        MQTTListener,

        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_Client, PipelineAdminResources.Names.Connection_Type_MQTT_Client, typeof(PipelineAdminResources))]
        MQTTClient,

        [EnumLabel(ListenerConfiguration.ListenerTypes_RawTCP, PipelineAdminResources.Names.Connection_Type_TCP, typeof(PipelineAdminResources))]
        RawTCP,

        [EnumLabel(ListenerConfiguration.ListenerTypes_RabbitMQClient, PipelineAdminResources.Names.ConnectionType_RabbitMQClient, typeof(PipelineAdminResources))]
        RabbitMQClient,

        [EnumLabel(ListenerConfiguration.ListenerTypes_REST, PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
        Rest,

        [EnumLabel(ListenerConfiguration.ListenerTypes_REDIS, PipelineAdminResources.Names.Connection_Type_Redis, typeof(PipelineAdminResources))]
        Redis,

        [EnumLabel(ListenerConfiguration.ListenerTypes_Kafka, PipelineAdminResources.Names.Connection_Type_Kafka, typeof(PipelineAdminResources))]
        Kafka,

        [EnumLabel(ListenerConfiguration.ListenerTypes_RawUdp, PipelineAdminResources.Names.Connection_Type_UDP, typeof(PipelineAdminResources))]
        RawUDP,

        [EnumLabel(ListenerConfiguration.ListenerTypes_SerialPort, PipelineAdminResources.Names.ConnectionType_SerialPort, typeof(PipelineAdminResources))]
        SerialPort,

        [EnumLabel(ListenerConfiguration.ListenerTypes_WebSocket, PipelineAdminResources.Names.Connection_Type_WebSocket, typeof(PipelineAdminResources))]
        WebSocket,

        [EnumLabel(ListenerConfiguration.ListenerTypes_FTP, PipelineAdminResources.Names.Connection_Type_FTP, typeof(PipelineAdminResources))]
        FTP,
       
        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_SharedBroker, PipelineAdminResources.Names.Connection_MQTT_SharedBroker, typeof(PipelineAdminResources))]
        SharedMqttListener,

        [EnumLabel(ListenerConfiguration.ListenerTypes_CoT, PipelineAdminResources.Names.Connection_CoT, typeof(PipelineAdminResources))]
        TakCursorOnTarget,

        /*
            Soap,
        
        [EnumLabel(ListenerConfiguration.ListenerTypes_SOAP, PipelineAdminResources.Names.Connection_Type_Soap, typeof(PipelineAdminResources))]
        
         * [EnumLabel(ListenerConfiguration.ListenerTypes_POP3Server, PipelineAdminResources.Names.Connection_Type_POP3Server, typeof(PipelineAdminResources))]
                POP3Server,
                [EnumLabel(ListenerConfiguration.ListenerTypes_Custom, PipelineAdminResources.Names.Connection_Type_Custom, typeof(PipelineAdminResources))]
                Custom*/
    }

    public enum MessageLengthSize
    {
        [EnumLabel(ListenerConfiguration.MessageLengthSize_One, PipelineAdminResources.Names.Listener_MessageLength_1_Byte, typeof(PipelineAdminResources))]
        OneByte,
        [EnumLabel(ListenerConfiguration.MessageLengthSize_Two, PipelineAdminResources.Names.Listener_MessageLength_2_Bytes, typeof(PipelineAdminResources))]
        TwoBytes,
        [EnumLabel(ListenerConfiguration.MessageLengthSize_Four, PipelineAdminResources.Names.Listener_MessageLength_4_Bytes, typeof(PipelineAdminResources))]
        FourBytes
    }

    public enum RESTServerTypes
    {
        [EnumLabel(ListenerConfiguration.RESTServerType_HTTP, PipelineAdminResources.Names.Listener_RESTServerType_HTTP, typeof(PipelineAdminResources))]
        HTTP,
        [EnumLabel(ListenerConfiguration.RESTServerType_HTTPS, PipelineAdminResources.Names.Listener_RESTServerType_HTTPS, typeof(PipelineAdminResources))]
        HTTPS,
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Listener_Title, PipelineAdminResources.Names.Listener_Help, PipelineAdminResources.Names.Listener_Description,
        EntityDescriptionAttribute.EntityTypes.CoreIoTModel, typeof(PipelineAdminResources), Icon: "icon-fo-listening", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/listeners", GetUrl: "/api/pipeline/admin/listener/{id}", SaveUrl: "/api/pipeline/admin/listener", DeleteUrl: "/api/pipeline/admin/listener/{id}",
        ListUIUrl: "/iotstudio/make/listeners", EditUIUrl: "/iotstudio/make/listener/{0}", CreateUIUrl: "/iotstudio/make/listener/add",
        FactoryUrl: "/api/pipeline/admin/listener/factory")]
    public class ListenerConfiguration : PipelineModuleConfiguration, IFormDescriptor, IFormConditionalFields, IIconEntity, ISummaryFactory
    {

        public const string MessageLengthSize_One = "one";
        public const string MessageLengthSize_Two = "two";
        public const string MessageLengthSize_Four = "four";

        public const string RESTServerType_HTTP = "http";
        public const string RESTServerType_HTTPS = "https";
        public const string RESTServerType_HTTPorHTTPS = "httporhttps";

        public const string ListenerTypes_AzureServiceBus = "azureserivcebus";
        public const string ListenerTypes_AzureEventHub = "azureeventhub";
        public const string ListenerTypes_RabbitMQ = "rabbitmq";
        public const string ListenerTypes_RabbitMQClient = "rabbitmqclient";
        public const string ListenerTypes_AzureIoTHub = "azureiothub";
        public const string ListenerTypes_REST = "rest";
        public const string ListenerTypes_SharedREST = "sharedrest";
        public const string ListenerTypes_SOAP = "soap";
        public const string ListenerTypes_RawTCP = "rawtcp";
        public const string ListenerTypes_RawUdp = "raw_udp";
        public const string ListenerTypes_AMQP = "amqp";
        public const string ListenerTypes_REDIS = "redis";
        public const string ListenerTypes_Kafka = "kafka";
        public const string ListenerTypes_MQTT_Broker = "mqttbroker";
        public const string ListenerTypes_MQTT_SharedBroker = "sharedmqttlistener";
        public const string ListenerTypes_MQTT_Listener = "mqttlistener";
        public const string ListenerTypes_MQTT_Client = "mqttclient";
        public const string ListenerTypes_POP3Server = "pop3server";
        public const string ListenerTypes_Custom = "custom";
        public const string ListenerTypes_SerialPort = "serialport";
        public const string ListenerTypes_WebSocket = "websocket";
        public const string ListenerTypes_FTP = "ftp";
        public const string ListenerTypes_CoT = "cottak";

        public override string ModuleType => PipelineModuleType_Listener;

        public ListenerConfiguration()
        {
            MqttSubscriptions = new List<MQTTSubscription>();
            AmqpSubscriptions = new List<string>();
            RESTListenerType = RESTListenerTypes.PipelineModule;
            Icon = "icon-fo-listening";
        }


        [FormField(LabelResource: PipelineAdminResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Icon { get; set; }

        public RESTListenerTypes RESTListenerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenerType, EnumType: (typeof(ListenerTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<ListenerTypes> ListenerType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT, HelpResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool? DelimitedWithSOHEOT { get; set; }

        [FormField(LabelResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType, HelpResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Help, FieldType: FieldTypes.Picker, WaterMark: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Select, EnumType: typeof(MessageContentTypes), ResourceType: typeof(DeviceMessagingAdminResources), IsRequired: true)]
        public EntityHeader<MessageContentTypes> ContentType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageContainsLength, HelpResource: PipelineAdminResources.Names.Listener_MessageContainsLength_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool? MessageLengthInMessage { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Location, HelpResource: PipelineAdminResources.Names.Listener_Length_Location_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageLengthLocation { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength, HelpResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_Length_Endiness_Select, EnumType: typeof(EndianTypes), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<MessageLengthSize> MessageLengthSize { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Endiness, HelpResource: PipelineAdminResources.Names.Listener_Length_Endiness_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_MessageLength_Select, EnumType: typeof(MessageLengthSize), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<EndianTypes> MessageLengthByteCountEndiness { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_RESTServerType, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_RESTServerType_Select, EnumType: typeof(RESTServerTypes), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<RESTServerTypes> RestServerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Anonymous, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool? Anonymous { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectSSLTLS, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool? SecureConnection { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Certificate,  FieldType: FieldTypes.SecureCertificate, SecureIdFieldName: nameof(CertificateSecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Certificate { get; set; }

        public string CertificateSecureId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Certificate, FieldType: FieldTypes.Password, SecureIdFieldName: nameof(CertificatePasswordSecureId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string CertificatePassword { get; set; }

        public string CertificatePasswordSecureId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_UserName, HelpResource: PipelineAdminResources.Names.Listener_UserName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string UserName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Password, HelpResource: PipelineAdminResources.Names.Listener_Password_Help, FieldType: FieldTypes.Password, SecureIdFieldName: nameof(SecurePasswordId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Password { get; set; }

        public string SecurePasswordId { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HostName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HostName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKeyName, HelpResource: PipelineAdminResources.Names.Listener_AccessKeyName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKeyName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKey, HelpResource: PipelineAdminResources.Names.Listener_AccessKey_Help, FieldType: FieldTypes.Password, SecureIdFieldName:nameof(SecureAccessKeyId), ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKey { get; set; }

        public string SecureAccessKeyId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Topic { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Queue, HelpResource: PipelineAdminResources.Names.Listener_Queue_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Queue { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Endpoint, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Endpoint { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ResourceName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ResourceName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_EventHub_ConsumerGroup, HelpResource: PipelineAdminResources.Names.Listener_ConsumerGroup_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ConsumerGroup { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HubName, HelpResource: PipelineAdminResources.Names.Listener_HubName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HubName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ExchangeName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ExchangeName { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenOnPort, HelpResource: PipelineAdminResources.Names.Listener_Port_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources))]
        public int? ListenOnPort { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectToPort, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources))]
        public int? ConnectToPort { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_SupportedProtocol, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources))]
        public string SupportedProtocol { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Path, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources))]
        public string Path { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Origin, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources))]
        public string Origin { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_DefaultResponse, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources))]
        public string DefaultResponse { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_FailedResponse, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources))]
        public string FailedResponse { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReply, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool KeepAliveToSendReply { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReply_Timeout, HelpResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReplyTimeout_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? KeepAliveToSendReplyTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_StartMessageSequence, HelpResource: PipelineAdminResources.Names.Listener_StartMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string StartMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_EndMessageSequence, HelpResource: PipelineAdminResources.Names.Listener_EndMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string EndMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout, HelpResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageReceiveTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MaxMessageSize, HelpResource: PipelineAdminResources.Names.Listener_MaxMessageSize_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MaxMessageSize { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Subscriptions, FactoryUrl: "/api/pipeline/admin/listener/subscription/factory", FieldType: FieldTypes.ChildListInline, ResourceType: typeof(PipelineAdminResources))]
        public List<MQTTSubscription> MqttSubscriptions { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SerialPort_BaudRate, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string BaudRate { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.SerialPort_PortName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string PortName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Subscriptions, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<string> AmqpSubscriptions { get; set; }


        public string CredentialsFileSecretId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_CredentialsFile, FieldType: FieldTypes.FileUpload,PrivateFileUpload:true, ResourceType: typeof(PipelineAdminResources))]
        public EntityHeader CredentialsFile { get; set; }


        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            if (EntityHeader.IsNullOrEmpty(ListenerType))
            {
                result.AddUserError("Listener Type is a Required Field.");
                return;
            }

            switch (ListenerType.Value)
            {
                case ListenerTypes.AMQP:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for Connecting to an AMQP Server.");
                    if (Anonymous.HasValue && !Anonymous.Value)
                    {
                        if (string.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required to connect to your AMQP server for non-anonymous connections.");
                        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required to connect to your AMQP server for non-anonymous connections.");
                    }
                    else
                    {
                        UserName = null;
                        Password = null;
                    }

                    if (!AmqpSubscriptions.Any()) result.AddUserError("Please ensure you provide at least one subscription (including wildcards * and #) that will be monitored for incoming messages.");
                    break;
                case ListenerTypes.AzureEventHub:
                    if (HostName != null && HostName.ToLower().StartsWith("sb://")) HostName = HostName.Substring(5);
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for Azure Event Hubs, this is the host name of your Event Hub without the sb:// protocol.");
                    if (string.IsNullOrEmpty(HubName)) result.AddUserError("Hub Name is Required for an Azure Event Hub Listener.");
                    if (string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for an Azure Event Hub listener.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");
                    break;
                case ListenerTypes.AzureIoTHub:
                    if (HostName != null && HostName.ToLower().StartsWith("sb://")) HostName = HostName.Substring(5);
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for Azure IoT Hub, this is found in the Event Hub-compatible endpoint field on the Azure Portal.");
                    if (string.IsNullOrEmpty(ResourceName)) result.AddUserError("Resource Name is require for Azure IoT Hub, this is found in the Event Hub-compatible name field on the Azure Portal.");
                    if (string.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a required field for an Azure Serice Bus Listener.");
                    if (string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for Azure IoT Event Hub.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");
                    break;
                case ListenerTypes.AzureServiceBus:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for an Azure Service Bus Listener, this is the host name of your Event Hub without the sb:// protocol.");
                    if (HostName != null && HostName.ToLower().StartsWith("sb://")) HostName = HostName.Substring(5);
                    if (string.IsNullOrEmpty(Queue)) result.AddUserError("Queue Name is required for an Azure Service Bus Listener.");
                    if (string.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a required field for an Azure Serice Bus Listener.");
                    if (string.IsNullOrEmpty(AccessKey) && string.IsNullOrEmpty(SecureAccessKeyId)) result.AddUserError("Access Key is Required for an Azure IoT Service Bus Listener.");
                    if (!string.IsNullOrEmpty(AccessKey) && !Utils.StringValidationHelper.IsBase64String(AccessKey)) result.AddUserError("Access Key does not appear to be a Base 64 string and is likely incorrect.");

                    break;
                case ListenerTypes.MQTTClient:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for Connecting to an MQTT Server.");
                    if (Anonymous.HasValue && !Anonymous.Value)
                    {
                        if (string.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required to connect to your MQTT Broker for non-anonymous connections.");
                        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required to connect to your MQTT Broker for non-anonymous connections.");
                    }
                    else
                    {
                        UserName = null;
                        Password = null;
                    }
                    if (!ConnectToPort.HasValue) result.AddUserError("Please provide a port that your MQTT Client will connect, usually 1883 or 8883 (SSL).");
                    MqttSubscriptions.RemoveAll(sub => string.IsNullOrEmpty(sub.Topic));
                    if (!MqttSubscriptions.Any()) result.AddUserError("Please ensure you provide at least one subscription (including wildcards + and #) that will be monitored for incoming messages.");

                    break;
                case ListenerTypes.WebSocket:
                    if (string.IsNullOrEmpty(HostName)) result.AddUserError("Host Name is required for an Azure Service Bus Listener, this is the host name of your Event Hub without the sb:// protocol.");
                    if (HostName != null && HostName.ToLower().StartsWith("wss://")) HostName = HostName.Substring(5);
                    if (HostName != null && HostName.ToLower().StartsWith("ws://")) HostName = HostName.Substring(4);
                    if (!string.IsNullOrEmpty(Path) && !Path.StartsWith("/")) Path = "/" + Path;

                    if (Anonymous.HasValue && !Anonymous.Value)
                    {
                        if (string.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required to be used to authenticate with your Web Socket Server.");
                        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required to be used to authenticate with your Web Socket Server..");
                    }
                    else
                    {
                        UserName = null;
                        Password = null;
                    }

                    break;
                case ListenerTypes.MQTTListener:
                    if (Anonymous.HasValue && !Anonymous.Value)
                    {
                        if (string.IsNullOrEmpty(UserName)) result.AddUserError("User Name is Required to be used to authenticate MQTT clients authenticating with your broker.");
                        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Password is Required to be used to authenticate MQTT clients authenticating with your broker.");
                    }
                    else
                    {
                        UserName = null;
                        Password = null;
                    }
                    if (!ListenOnPort.HasValue) result.AddUserError("Please provide a port that your MQTT listenr will listen for incoming messages, usually 1883.");
                    break;
                /*
            case ListenerTypes.MQTTBroker:
                break; 
            case ListenerTypes.RabbitMQ:
                break;
            case ListenerTypes.RabbitMQClient:
                break;*/
                case ListenerTypes.RawTCP:
                    if (!ListenOnPort.HasValue) result.AddUserError("Please provide a port that your TCP listenr will listen for incoming messages.");
                    break;
                case ListenerTypes.RawUDP:
                    if (!ListenOnPort.HasValue) result.AddUserError("Please provide a port that your UDP listenr will listen for incoming messages.");
                    break;
                case ListenerTypes.Rest:
                    if (!ListenOnPort.HasValue) result.AddUserError("Please provide a port that your REST listenr will listen for incoming messages, this is usually port 80 for HTTP and 443 for HTTPS.");
                    if (EntityHeader.IsNullOrEmpty(RestServerType)) result.AddUserError("Allowable Connection Type is required for an REST Listener.");
                    if (Anonymous.HasValue && !Anonymous.Value)
                    {
                        if (string.IsNullOrEmpty(UserName)) result.AddUserError("Anonymous connections are not enabled, you must provide a user name that will be used to connect to your REST endpoint.");
                        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(SecurePasswordId)) result.AddUserError("Anonymous connections are not enabled, you must provide a password that will be used to connect to your REST endpoint.");
                    }
                    else
                    {
                        UserName = null;
                        Password = null;
                    }
                    break;
                case ListenerTypes.TakCursorOnTarget:
                    if (EntityHeader.IsNullOrEmpty(CredentialsFile))
                        result.AddUserError("Must upload credentials file for TAK Server");
                    break;
            }

        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Category),
                nameof(ListenerType),
                nameof(ContentType),
                nameof(ListenOnPort),
                nameof(CredentialsFile),

                nameof(MessageReceiveTimeoutMS),

                nameof(StartMessageSequence),
                nameof(EndMessageSequence),

                nameof(KeepAliveToSendReply),
                nameof(KeepAliveToSendReplyTimeoutMS),

                nameof(MaxMessageSize),

                nameof(MessageLengthInMessage),
                nameof(MessageLengthLocation),
                nameof(MessageLengthSize),
                nameof(MessageLengthByteCountEndiness),
                nameof(SecureConnection),
                nameof(HostName),
                nameof(ResourceName),
                nameof(Endpoint),
                nameof(ConnectToPort),
                nameof(RestServerType),

                nameof(PortName),
                nameof(BaudRate),

                nameof(Queue),
                nameof(ExchangeName),
                nameof(HubName),

                nameof(Anonymous),
                nameof(UserName),
                nameof(Password),
                nameof(AccessKeyName),
                nameof(AccessKey),

                nameof(Path),
                nameof(SupportedProtocol),
                nameof(Origin),

                nameof(AmqpSubscriptions),
                nameof(MqttSubscriptions),
                nameof(ConsumerGroup),
                nameof(Topic),

                nameof(DelimitedWithSOHEOT),
                nameof(Description)
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = { nameof(MessageReceiveTimeoutMS), nameof(KeepAliveToSendReply), nameof(KeepAliveToSendReplyTimeoutMS), nameof(ListenOnPort), nameof(StartMessageSequence), nameof(EndMessageSequence),
                                      nameof(MaxMessageSize), nameof(MessageLengthInMessage), nameof(MessageLengthLocation), nameof(MessageLengthSize),nameof(MessageLengthByteCountEndiness),
                                      nameof(Anonymous), nameof(UserName), nameof(AccessKey), nameof(Queue), nameof(ExchangeName), nameof(HubName), nameof(AccessKey), nameof(AccessKeyName), nameof(Password),
                                      nameof(ConsumerGroup), nameof(ResourceName), nameof(SecureConnection), nameof(RestServerType), nameof(HostName), nameof(Endpoint), nameof(ConnectToPort), nameof(Path),
                                      nameof(Origin), nameof(SupportedProtocol), nameof(BaudRate), nameof(PortName), nameof(Topic), nameof(AmqpSubscriptions), nameof(MqttSubscriptions), nameof(DelimitedWithSOHEOT),
                                      nameof(CredentialsFile), nameof(Certificate), nameof(CertificatePassword),

                },
                 Conditionals = new List<FormConditional>()
                 {
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_AMQP,
                          VisibleFields = {nameof(HostName), nameof(Anonymous), nameof(Topic), nameof(AmqpSubscriptions)},
                          RequiredFields = {nameof(HostName)},
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_Kafka,
                          VisibleFields = {nameof(HostName), nameof(ConsumerGroup), nameof(MqttSubscriptions) },
                          RequiredFields = {nameof(HostName), nameof(ConsumerGroup), nameof(MqttSubscriptions) }

                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_SerialPort,
                          VisibleFields = {nameof(PortName), nameof(BaudRate)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_REDIS,
                          VisibleFields = {nameof(HostName), nameof(Anonymous),  nameof(MqttSubscriptions) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_AzureEventHub,
                          VisibleFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(ConsumerGroup), nameof(HostName), nameof(HubName) },
                          RequiredFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(ConsumerGroup), nameof(HostName), nameof(HubName) },
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_WebSocket,
                          VisibleFields = {nameof(Anonymous),  nameof(HostName), nameof(SupportedProtocol), nameof(Path), nameof(Origin) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_AzureIoTHub,
                          VisibleFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(ResourceName), nameof(ConsumerGroup), nameof(HostName) },
                          RequiredFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(ResourceName), nameof(ConsumerGroup), nameof(HostName) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_AzureServiceBus,
                          VisibleFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(Queue),  nameof(HostName) },
                          RequiredFields = {nameof(AccessKey), nameof(AccessKeyName), nameof(Queue),  nameof(HostName) },
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_MQTT_SharedBroker,
                          VisibleFields = {nameof(MqttSubscriptions) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_MQTT_Client,
                          VisibleFields = { nameof(Anonymous), nameof(SecureConnection), nameof(Certificate), nameof(CertificatePassword),  nameof(HostName), nameof(ConnectToPort), nameof(MqttSubscriptions) },
                          RequiredFields = {nameof(HostName), nameof(ConnectToPort)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_MQTT_Listener,
                          VisibleFields = { nameof(Anonymous),  nameof(ListenOnPort) },
                          RequiredFields = { nameof(ListenOnPort) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_RabbitMQ,
                          VisibleFields = { nameof(HostName), nameof(Anonymous), nameof(ListenOnPort), nameof(Queue), nameof(ExchangeName), nameof(MqttSubscriptions) },
                          RequiredFields = { nameof(HostName), nameof(ListenOnPort), nameof(Queue), nameof(ExchangeName) },

                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_MQTT_Broker,
                          VisibleFields = { nameof(Endpoint), nameof(Anonymous),  nameof(Queue), nameof(ExchangeName) },
                          RequiredFields = { nameof(Endpoint),  nameof(Queue), nameof(ExchangeName) }
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_RawTCP,
                          VisibleFields = { nameof(ListenOnPort)},
                          RequiredFields = {nameof(ListenOnPort)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_RawUdp,
                          VisibleFields = { nameof(ListenOnPort) },
                          RequiredFields = {nameof(ListenOnPort)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_FTP,
                          VisibleFields = { nameof(Anonymous), nameof(ListenOnPort) },
                          RequiredFields = {nameof(ListenOnPort)}
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_REST,
                          VisibleFields = { nameof(Anonymous), nameof(ListenOnPort), nameof(KeepAliveToSendReply), nameof(KeepAliveToSendReplyTimeoutMS), nameof(RestServerType)},
                          RequiredFields = { nameof(ListenOnPort), nameof(RestServerType)},

                     },
                     new FormConditional()
                     {
                          Field = nameof(Anonymous),
                          Value = "false",
                          VisibleFields = { nameof(UserName), nameof(Password) },
                          RequiredFields = { nameof(UserName), nameof(Password) },
                     },
                     new FormConditional()
                     {
                          Field = nameof(ListenerType),
                          Value = ListenerTypes_CoT,
                          VisibleFields = { nameof(CredentialsFile) },
                          RequiredFields = { nameof(CredentialsFile) },
                     },
                 }

            };
        }

        /// <summary>
        /// Populated at run time so any modules that need to create temporary storage for event hub checkpoint containers
        /// </summary>
        public string EventHubCheckPointContainerStorageAccountId { get; set; }

        /// <summary>
        /// Populated at run time so any modules that need to create temporary storage for event hub checkpoint containers
        /// </summary>
        public string EventHubCheckPointContainerStorageAccessKey { get; set; }

        public ListenerConfigurationSummary CreateSummary()
        {
            return new ListenerConfigurationSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Icon = Icon,
                ListenerType = ListenerType.Text,
                ListenerTypeId = ListenerType.Id,
                IsPublic = IsPublic,
                Description = Description,
                Category = Category
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary(); 
        }
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.Listeners_Title, PipelineAdminResources.Names.Listener_Help, PipelineAdminResources.Names.Listener_Description,
        EntityDescriptionAttribute.EntityTypes.Summary, typeof(PipelineAdminResources), Icon: "icon-fo-listening", Cloneable: true,
        GetListUrl: "/api/pipeline/admin/listeners", GetUrl: "/api/pipeline/admin/listener/{id}", SaveUrl: "/api/pipeline/admin/listener", DeleteUrl: "/api/pipeline/admin/listener/{id}",
        FactoryUrl: "/api/pipeline/admin/listener/factory")]
    public class ListenerConfigurationSummary : CategorizedSummaryData
    {
        public string ListenerType { get; set; }
        public string ListenerTypeId { get; set; }
    }
}