using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Resources;
using LagoVista.IoT.Pipeline.Admin.Resources;
using System.Collections.Generic;
using System.Linq;

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
        /*[EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_Broker, PipelineAdminResources.Names.Connection_Type_MQTT_Broker, typeof(PipelineAdminResources))]
        MQTTBroker,*/
        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_Client, PipelineAdminResources.Names.Connection_Type_MQTT_Client, typeof(PipelineAdminResources))]
        MQTTClient,
        [EnumLabel(ListenerConfiguration.ListenerTypes_RawTCP, PipelineAdminResources.Names.Connection_Type_TCP, typeof(PipelineAdminResources))]
        RawTCP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_RabbitMQClient, PipelineAdminResources.Names.ConnectionType_RabbitMQClient, typeof(PipelineAdminResources))]
        RabbitMQClient,
        [EnumLabel(ListenerConfiguration.ListenerTypes_REST, PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
        Rest,

        [EnumLabel(ListenerConfiguration.ListenerTypes_SharedREST, PipelineAdminResources.Names.Connection_Type_SharedRest, typeof(PipelineAdminResources))]
        SharedRest,
        /*[EnumLabel(ListenerConfiguration.ListenerTypes_RabbitMQ, PipelineAdminResources.Names.ConnectionType_RabbitMQ, typeof(PipelineAdminResources))]
        RabbitMQ,*/
        [EnumLabel(ListenerConfiguration.ListenerTypes_RawUdp, PipelineAdminResources.Names.Connection_Type_UDP, typeof(PipelineAdminResources))]
        RawUDP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_WebSocket, PipelineAdminResources.Names.Connection_Type_WebSocket, typeof(PipelineAdminResources))]
        WebSocket,
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

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help, PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class ListenerConfiguration : PipelineModuleConfiguration
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
        public const string ListenerTypes_MQTT_Broker = "mqttbroker";
        public const string ListenerTypes_MQTT_Listener = "mqttlistener";
        public const string ListenerTypes_MQTT_Client = "mqttclient";
        public const string ListenerTypes_POP3Server = "pop3server";
        public const string ListenerTypes_Custom = "custom";
        public const string ListenerTypes_WebSocket = "websocket";

        public override string ModuleType => PipelineModuleType_Listener;

        public ListenerConfiguration()
        {
            MqttSubscriptions = new List<MQTTSubscription>();
            AmqpSubscriptions = new List<string>();
            RESTListenerType = RESTListenerTypes.PipelineModule;
        }

        public RESTListenerTypes RESTListenerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenerType, EnumType: (typeof(ListenerTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<ListenerTypes> ListenerType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT, HelpResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool DelimitedWithSOHEOT { get; set; }

        [FormField(LabelResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType, HelpResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Help, FieldType: FieldTypes.Picker, WaterMark: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Select, EnumType: typeof(MessageContentTypes), ResourceType: typeof(DeviceMessagingAdminResources), IsRequired: true)]
        public EntityHeader<MessageContentTypes> ContentType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageContainsLength, HelpResource: PipelineAdminResources.Names.Listener_MessageContainsLength_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool MessageLengthInMessage { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Location, HelpResource: PipelineAdminResources.Names.Listener_Length_Location_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageLengthLocation { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength, HelpResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_Length_Endiness_Select, EnumType: typeof(EndianTypes), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<MessageLengthSize> MessageLengthSize { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Endiness, HelpResource: PipelineAdminResources.Names.Listener_Length_Endiness_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_MessageLength_Select, EnumType: typeof(MessageLengthSize), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<EndianTypes> MessageLengthByteCountEndiness { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_RESTServerType, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_RESTServerType_Select, EnumType: typeof(RESTServerTypes), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<RESTServerTypes> RestServerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Anonymous, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool Anonymous { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectSSLTLS, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources))]
        public bool SecureConnection { get; set; }

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

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Subscriptions, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<MQTTSubscription> MqttSubscriptions { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Subscriptions, FieldType: FieldTypes.ChildList, ResourceType: typeof(PipelineAdminResources))]
        public List<string> AmqpSubscriptions { get; set; }

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
                    if (!Anonymous)
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
                    if (!Anonymous)
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

                    if (!Anonymous)
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
                    if (!Anonymous)
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
                    if (!Anonymous)
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
            }
            
        }

        /// <summary>
        /// Populated at run time so any modules that need to create temporary storage for event hub checkpoint containers
        /// </summary>
        public string EventHubCheckPointContainerStorageAccountId { get; set; }

        /// <summary>
        /// Populated at run time so any modules that need to create temporary storage for event hub checkpoint containers
        /// </summary>
        public string EventHubCheckPointContainerStorageAccessKey { get; set; }
    }
}